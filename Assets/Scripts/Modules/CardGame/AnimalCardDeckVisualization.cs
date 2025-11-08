using System;
using System.Linq;
using DivineSkies.Modules.Game.Card;

namespace DivineSkies.Modules.Game
{
    public class AnimalCardDeckVisualization : CardDeckVisualization
    {
        public override void Refresh(CardBase[] containingCards)
        {
            string output = "";
            foreach (AnimalsSpecies value in Enum.GetValues(typeof(AnimalsSpecies)).Cast<AnimalsSpecies>())
            {
                if(value == AnimalsSpecies.None)
                {
                    continue;
                }
                output += value + ": " + containingCards.Count(c => (c as AnimalCard).Animal == value) + "\n";
            }
            _amountTxt.text = output;
        }
    }
}