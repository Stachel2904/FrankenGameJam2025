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
        }

        public override string GetCardText()
        {
            return "Place a " + Animal.ToString() + " into your world.";
        }

        public override void Play()
        {
            CardGameController.Main.OnAnimalSelected(Animal);
        }
    }
}