using DivineSkies.Tools.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace DivineSkies.Modules.Game
{
    public interface ICardDeck
    {
        public int Count { get; }
    }

    public class CardDeck<TCard> : ICardDeck where TCard : CardBase
    {
        public int Count => _containingCards.Count;
        private List<TCard> _containingCards;
        private CardDeck<TCard> _backUpDeck;
        private CardDeckVisualization _visualization;

        public CardDeck(CardDeckVisualization visualization)
        {
            _containingCards = new List<TCard>();
            _visualization = visualization;
        }
        public CardDeck(TCard[] startCards, CardDeckVisualization visualization)
        {
            _containingCards = new List<TCard>(startCards);
            _visualization = visualization;
            RefreshVisualization();
        }
        public CardDeck(TCard[] startCards, CardDeck<TCard> backUpDeckReference, CardDeckVisualization visualization)
        {
            _containingCards = new List<TCard>(startCards);
            _backUpDeck = backUpDeckReference;
            _visualization = visualization;
            RefreshVisualization();
        }

        private void RefreshVisualization()
        {
            _visualization?.Refresh(_containingCards.Cast<CardBase>().ToArray());

        }

        public TCard[] GetCards()
        {
            return _containingCards.ToArray();
        }

        public void ClearCards()
        {
            _containingCards.Clear();
            RefreshVisualization();
        }

        public virtual void AddCard(TCard cardToAdd)
        {
            _containingCards.Add(cardToAdd);
            RefreshVisualization();
        }

        public virtual void AddCards(TCard[] cardsToAdd)
        {
            _containingCards.AddRange(cardsToAdd);
            RefreshVisualization();
        }

        public virtual TCard DrawTopCard()
        {
            if (_containingCards.Count == 0)
            {
                if (_backUpDeck != null)
                {
                    _backUpDeck.Shuffle();
                    AddCards(_backUpDeck.GetCards());
                    _backUpDeck.ClearCards();
                }
                else
                {
                    this.PrintError("You tried to draw from an empty deck.");
                    return default;
                }
            }

            TCard result = _containingCards[0];
            _containingCards.RemoveAt(0);

            RefreshVisualization();

            return result;
        }

        public virtual TCard[] DrawTopCards(int amount)
        {
            TCard[] result = new TCard[amount];

            for (int i = 0; i < amount; i++)
                result[i] = DrawTopCard();

            return result;
        }

        public void Shuffle() => _containingCards.Shuffle();

        public bool RemoveCard(TCard card)
        {
            var removed = _containingCards.Remove(card);
            RefreshVisualization();

            return removed;
        }
    }
}
