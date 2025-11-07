using UnityEngine;

namespace DivineSkies.Modules.Game
{
    public abstract class CardBase
    {
        private static long idCounter = long.MinValue;
        public static long CurrentID { private get => ++idCounter; set => idCounter = (long)Mathf.Max(idCounter, value); }
        public abstract string PlayText { get; }

        public long Id;
        public bool IsInDeck;
        public string Name;
        public int Cost;
        public string Description;

        protected static T Create<T>(T template) where T : CardBase, new()
        {
            T created = new();

            created.Id = CurrentID;

            created.Name = template.Name;
            created.Cost = template.Cost;
            created.Description = template.Description;

            return created;
        }

        public abstract string GetCardText();

        public abstract void Play();
    }
}
