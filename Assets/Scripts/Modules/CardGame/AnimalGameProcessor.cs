using DivineSkies.Modules.Game.TurnBased;
using DivineSkies.Modules.Popups;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DivineSkies.Modules.Game.Card
{
    public class AnimalGameProcessor : ITurnBaseGameProcessor
    {
        private const int MAX_TURNS = 30;
        private const int HAND_CARD_COUNT = 5;

        public AnimalsSpecies CurrentSelectedAnimal { get; set; }

        private AnimalCardGameController _controller;
        private GridManager _gridManager;

        private int _turnCount;
        private int _points;

        public void SetController(IGameController controller)
        {
            _controller = controller as AnimalCardGameController;
            _turnCount = 0;
            _gridManager = new GridManager(10);
        }

        public void OnStart()
        {
            GridField[] fields = UnityEngine.Object.FindObjectsByType<GridField>(FindObjectsSortMode.None);
            foreach (GridField field in fields)
            {
                _gridManager.AddField(field);
            }

            _gridManager.EnableSelection(false);

            _controller.DrawCards(HAND_CARD_COUNT);
            _controller.Visualization.RefreshPointDisplay(0, 0);
            _controller.Visualization.RefreshLeftTurns(MAX_TURNS);
        }

        public void OnAnimalSelected(AnimalsSpecies species)
        {
            CurrentSelectedAnimal = species;

            _gridManager.EnableSelection(true);
        }

        public void OnFieldSelected(GridField field)
        {
            field.SetAnimal(CurrentSelectedAnimal);

            _gridManager.EnableSelection(false);

            _controller.NextTurn();
        }

        public void OnNextTurn()
        {
            _turnCount++;

            _controller.ClearHandCards();

            int lastPoints = _points;
            _points = CalculatePoints().Values.Sum();

            _controller.Visualization.RefreshPointDisplay(_points, _points - lastPoints);
            _controller.Visualization.RefreshLeftTurns(MAX_TURNS - _turnCount);

            if (_turnCount >= MAX_TURNS)
            {
                _controller.End(GameEndReason.Successful);
                return;
            }

            _controller.DrawCards(HAND_CARD_COUNT);
            CurrentSelectedAnimal = AnimalsSpecies.None;
        }

        public void OnEnd(GameEndReason reason)
        {
            Dictionary<AnimalsSpecies, int> points = CalculatePoints();

            string pointOutput = "";
            foreach (KeyValuePair<AnimalsSpecies, int> kvp in points)
            {
                pointOutput += kvp.Key.ToString() + ": " + kvp.Value + " points \n";
            }
            pointOutput += "\nSum: " + points.Values.Sum();

            Popup.Create<NotificationPopup>().Init("Score", pointOutput, EndGame);

            string newScore = "" + points.Values.Sum() + "," + DateTime.Now;
            if (PlayerPrefs.HasKey("highscores"))
            {
                string oldHighscores = PlayerPrefs.GetString("highscores");
                newScore = oldHighscores + "|" + newScore;
            }

            PlayerPrefs.SetString("highscores", newScore);
        }

        private void EndGame()
        {
            ModuleController.LoadScene(SceneNames.MainMenuScene);
        }

        private Dictionary<AnimalsSpecies, int> CalculatePoints()
        {
            Dictionary<AnimalsSpecies, int> scores = new Dictionary<AnimalsSpecies, int>
            {
                { AnimalsSpecies.Fox, 0 },
                { AnimalsSpecies.Bunny, 0 },
                { AnimalsSpecies.Eagle, 0 },
                { AnimalsSpecies.Snake, 0 },
                { AnimalsSpecies.Mouse, 0 },
                { AnimalsSpecies.Beaver, 0 }
            };

            foreach (List<GridField> row in _gridManager)
            {
                foreach (GridField field in row)
                {
                    int fieldScore = 0;
                    switch (field.Animal)
                    {
                        case AnimalsSpecies.Mouse:
                            fieldScore = 3;
                            break;
                        case AnimalsSpecies.Beaver:
                            fieldScore = field.WaterField ? 4 : 0;
                            break;
                        case AnimalsSpecies.Bunny:
                            fieldScore = _gridManager.GetNeighbours(field).Count(f => f.Animal is AnimalsSpecies.Bunny) * 1;
                            break;
                        case AnimalsSpecies.Fox:
                            fieldScore = _gridManager.GetNeighbours(field).Count(f => f.Animal is AnimalsSpecies.Bunny or AnimalsSpecies.Mouse) * 1;
                            break;
                        case AnimalsSpecies.Snake:
                            fieldScore = _gridManager.GetNeighbours(field).Count(f => f.Animal is AnimalsSpecies.Mouse) * 2;
                            fieldScore -= _gridManager.GetNeighbours(field).Count(f => f.Animal is not AnimalsSpecies.Mouse and not AnimalsSpecies.None) * 1;
                            break;
                        case AnimalsSpecies.Eagle:
                            int x = Mathf.RoundToInt(field.transform.position.x);
                            int z = Mathf.RoundToInt(field.transform.position.z);
                            fieldScore = DoEagleCalculation(x, z, 1, true);
                            fieldScore += DoEagleCalculation(x, z, -1, true);
                            fieldScore += DoEagleCalculation(x, z, 1, false);
                            fieldScore += DoEagleCalculation(x, z, -1, false);
                            break;
                        default:
                            continue;
                    }
                    scores[field.Animal] += fieldScore;
                }
            }

            return scores;
        }

        private int DoEagleCalculation(int x, int z, int direction, bool isHorizontal)
        {
            int result = 0;

            if (isHorizontal)
            {
                x += direction;
            }
            else
            {
                z += direction;
            }

            while (_gridManager.TryGet(x, z, out GridField iField) && iField.Animal is AnimalsSpecies.Mouse or AnimalsSpecies.Snake or AnimalsSpecies.None)
            {
                if (iField.Animal is not AnimalsSpecies.None)
                {
                    result += 1;
                }

                if (isHorizontal)
                {
                    x += direction;
                }
                else
                {
                    z += direction;
                }
            }

            return result;
        }

    }
}