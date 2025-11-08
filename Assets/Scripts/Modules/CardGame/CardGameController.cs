using System.Collections;
using UnityEngine;
using DivineSkies.Modules.Popups;
using System.Collections.Generic;

namespace DivineSkies.Modules.Game.Card
{
    public class CardGameController : GameController<AnimalCard, CardGameController>
    {
        private AnimalsSpecies _currentSelectedAnimal;
        private int _turnCount;
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

            foreach (AnimalCard card in _handDeck.GetCards())
            {
                DiscardHandCard(card);
            }

            DrawCard(5);
        }

        public void DiscardHandCard(AnimalCard card)
        {
            _discardDeck.AddCard(card);
            _handDeck.RemoveCard(card);
            Visualization.HandCards.RemoveHandCard(card);
        }

        public override void End(GameEndReason result)
        {
            Popup.Create<NotificationPopup>().Init("Game Over", "The Game is done.", CloseCombat);
        }

        private void CloseCombat()
        {
            Debug.Log("Closed Combat");
        }

        protected override AnimalCard[] GetDeckCards()
        {
            List<AnimalCard> cards = new List<AnimalCard>();
            cards.AddRange(CreateMultiple(AnimalsSpecies.Mouse, 30));
            cards.AddRange(CreateMultiple(AnimalsSpecies.Bunny, 30));
            cards.AddRange(CreateMultiple(AnimalsSpecies.Snake, 15));
            cards.AddRange(CreateMultiple(AnimalsSpecies.Beaver, 10));
            cards.AddRange(CreateMultiple(AnimalsSpecies.Eagle, 10));
            cards.AddRange(CreateMultiple(AnimalsSpecies.Fox, 15));
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
    }
}