using System.Collections.Generic;
using DivineSkies.Modules.Game.TurnBased.Card;

namespace DivineSkies.Modules.Game.Card
{
    public class AnimalCardGameController : CardGameController<AnimalCardGameController, AnimalGameProcessor, AnimalGameVisualization, AnimalCard>
    {
        public void ClearHandCards()
        {
            bool wasRemoved = false;
            foreach (AnimalCard card in _handCards.ToArray())
            {
                if (!wasRemoved && Processor.CurrentSelectedAnimal == card.Animal)
                {
                    DiscardHandCard(card, false);
                    wasRemoved = true;
                    continue;
                }

                DiscardHandCard(card);
            }
        }

        public void DiscardHandCard(AnimalCard card, bool addToDiscard)
        {
            if (addToDiscard)
            {
                DiscardHandCard(card);
            }
            else
            {
                _handCards.Remove(card);
                Visualization.HandCards.Refresh();
            }
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

        public override AnimalGameProcessor CreateProcessor()
        {
            return new AnimalGameProcessor();
        }

        public override AnimalGameVisualization CreateVisualization()
        {
            return FindAnyObjectByType<AnimalGameVisualization>();
        }
    }
}