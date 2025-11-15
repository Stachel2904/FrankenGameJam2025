using UnityEngine;
using UnityEngine.UI;
using DivineSkies.Modules.Game.Card;
using DivineSkies.Modules.Game.TurnBased.Card;

namespace DivineSkies.Modules.Game
{
    public class AnimalVisualPlayingCard : VisualPlayingCard
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Sprite[] _animalIcons;

        public override void Setup(CardBase card)
        {
            base.Setup(card);

            SetIcon((card as AnimalCard).Animal);
        }

        protected override void OnPlay()
        {
            AnimalCardGameController.Main.PlayCard(CardReference as AnimalCard);
        }

        private void SetIcon(AnimalsSpecies species)
        {
            _icon.sprite = _animalIcons[(int)species];
        }
    }
}
