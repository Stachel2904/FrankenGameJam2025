using DivineSkies.Modules.Game.TurnBased.Card;
using System.Collections.Generic;

namespace DivineSkies.Modules.Game.Card
{
    public class AnimalCard : CardBase
    {
        public AnimalsSpecies Animal;
        public override string PlayText => Description;

        public AnimalCard(AnimalsSpecies animal)
        {
            Animal = animal;
            Name = Animal.ToString();
            DrawEffects = new List<ICardEffect>();
            PlayEffects = new List<ICardEffect>
            {
                new AnimalCardPlayEffect(animal)
            };
            DiscardEffects = new List<ICardEffect>();
        }

        public override string GetCardText()
        {
            return Animal switch
            {
                AnimalsSpecies.Beaver => "Gains 4 points near water",
                AnimalsSpecies.Bunny => "Gains 1 point for each adjacent bunny",
                AnimalsSpecies.Fox => "Gains 1 point for each adjacent bunny or mouse",
                AnimalsSpecies.Mouse => "Gains 3 points",
                AnimalsSpecies.Snake => "Gains 2 points for each adjacent mouse and -1 point for each adjacent other animal",
                AnimalsSpecies.Eagle => "Gains 1 point for each mouse or snake in line of sight until another animal",
                _ => Animal.ToString() + "'s Effect",
            };
        }
    }

    public class AnimalCardPlayEffect : ICardEffect
    {
        private readonly AnimalsSpecies _animal;

        public AnimalCardPlayEffect(AnimalsSpecies animal)
        {
            _animal = animal;
        }

        public bool Evaluate()
        {
            return true;
        }

        public void Execute()
        {
            AnimalCardGameController.Main.Processor.OnAnimalSelected(_animal);
        }
    }
}