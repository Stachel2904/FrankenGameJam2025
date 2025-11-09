using DivineSkies.Modules.Popups;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DivineSkies.Modules.Game.Card
{
    public class CardGameController : GameController<AnimalCard, CardGameController>
    {
        private const int MAX_TURNS = 30;
        private AnimalsSpecies _currentSelectedAnimal;
        private int _turnCount;
        private int _points;
        private GridManager _gridManager;

        public override IEnumerator InitializeAsync()
        {
            yield return base.InitializeAsync();
            base.Initialize();

            _gridManager = new GridManager(10);

            _turnCount = 0;

            _drawDeck.Shuffle();
            DrawCard(5);

            Visualization.Setup(this);
            (Visualization as CardGameVisualization).RefreshPointDisplay(0, 0);
            (Visualization as CardGameVisualization).RefreshLeftTurns(MAX_TURNS);
        }

        public override void OnSceneFullyLoaded()
        {
            base.OnSceneFullyLoaded();

            GridField[] fields = FindObjectsByType<GridField>(FindObjectsSortMode.None);
            foreach (GridField field in fields)
            {
                _gridManager.AddField(field);
            }

            _gridManager.EnableSelection(false);
        }

        public override void NextTurn()
        {
            _turnCount++;

            bool wasRemoved = false;
            foreach (AnimalCard card in _handDeck.GetCards())
            {
                if(!wasRemoved && _currentSelectedAnimal == card.Animal)
                {
                    DiscardHandCard(card, false);
                    wasRemoved = true;
                    continue;
                }

                DiscardHandCard(card);
            }

            int lastPoints = _points;
            _points = CalculatePoints().Values.Sum();

            (Visualization as CardGameVisualization).RefreshPointDisplay(_points, _points -  lastPoints);
            (Visualization as CardGameVisualization).RefreshLeftTurns(MAX_TURNS - _turnCount);

            if(_turnCount >= MAX_TURNS)
            {
                End(GameEndReason.Successful);
                return;
            }

            DrawCard(5);
            _currentSelectedAnimal = AnimalsSpecies.None;
        }

        public void DiscardHandCard(AnimalCard card, bool addToDiscard = true)
        {
            if (addToDiscard)
            {
                _discardDeck.AddCard(card);
            }

            _handDeck.RemoveCard(card);
            Visualization.HandCards.RemoveHandCard(card);
        }

        public override void End(GameEndReason result)
        {
            Dictionary<AnimalsSpecies, int> points = CalculatePoints();

            string pointOutput = "";
            foreach (KeyValuePair<AnimalsSpecies, int> kvp in points)
            {
                pointOutput += kvp.Key.ToString() + ": " + kvp.Value + " points \n";
            }
            pointOutput += "\nSum: " + points.Values.Sum();

            Popup.Create<NotificationPopup>().Init("Score", pointOutput, CloseCombat);
        }

        private void CloseCombat()
        {
            ModuleController.LoadScene(SceneNames.MainMenuScene);
        }

        protected override AnimalCard[] GetDeckCards()
        {
            List<AnimalCard> cards = new List<AnimalCard>();
            cards.AddRange(CreateMultiple(AnimalsSpecies.Mouse, 35));
            cards.AddRange(CreateMultiple(AnimalsSpecies.Bunny, 35));
            cards.AddRange(CreateMultiple(AnimalsSpecies.Snake, 10));
            cards.AddRange(CreateMultiple(AnimalsSpecies.Beaver, 15));
            cards.AddRange(CreateMultiple(AnimalsSpecies.Eagle, 10));
            cards.AddRange(CreateMultiple(AnimalsSpecies.Fox, 25));
            return cards.ToArray();
        }

        private List<AnimalCard> CreateMultiple(AnimalsSpecies species, int amount)
        {
            List<AnimalCard> cards = new List<AnimalCard>();
            for (int i = 0; i < amount; i++)
            {
                cards.Add(new AnimalCard(species));
            }
            return cards;
        }

        public void OnAnimalSelected(AnimalsSpecies species)
        {
            _currentSelectedAnimal = species;

            _gridManager.EnableSelection(true);
        }

        public void OnFieldSelected(GridField field)
        {
            field.SetAnimal(_currentSelectedAnimal);

            _gridManager.EnableSelection(false);

            NextTurn();
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
                if(iField.Animal is not AnimalsSpecies.None)
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