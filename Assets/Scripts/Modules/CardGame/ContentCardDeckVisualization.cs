using System;
using System.Linq;
using DivineSkies.Modules.Game.Card;
using DivineSkies.Modules.Game.TurnBased.Card;

namespace DivineSkies.Modules.Game
{
    public class ContentCardDeckVisualization : CardDeckVisualization
    {
        public override void Refresh()
        {
            string output = "";
            foreach (AnimalsSpecies value in Enum.GetValues(typeof(AnimalsSpecies)).Cast<AnimalsSpecies>())
            {
                if(value == AnimalsSpecies.None)
                {
                    continue;
                }
                output += value + ": " + _displayingDeck.Count(c => ((AnimalCard)c).Animal == value) + "\n";
            }
            _amountTxt.text = output;
        }
    }
}