using System;
using DivineSkies.Tools.Extensions;

namespace DivineSkies.Modules.Game.Combat
{
    public class AnimalCard : CardBase
    {
        public AnimalsSpecies Animal;
        public override string PlayText => Description;

        public override string GetCardText()
        {
            return Animal.ToString();
        }

        public override void Play()
        {
            //ToDo: Place Animal
        }
    }
}