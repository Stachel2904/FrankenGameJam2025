using System.Collections;
using UnityEngine;
using DivineSkies.Modules.Popups;
using System;

namespace DivineSkies.Modules.Game.Combat
{
    public class CardGameController : GameController<AnimalCard, CardGameController>
    {
        private int _turnCount;

        public override IEnumerator InitializeAsync()
        {
            yield return base.InitializeAsync();
            base.Initialize();

            _turnCount = 0;

            _drawDeck.Shuffle();
            DrawCard(5);

            Visualization.Setup(this);
        }

        public override void NextTurn()
        {
            _turnCount++;

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
            //ToDo: Add Animal Cards
            return Array.Empty<AnimalCard>();
        }
    }
}