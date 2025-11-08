using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DivineSkies.Modules.UI;
using DivineSkies.Modules.Game.Card;

namespace DivineSkies.Modules.Game
{
    public class VisualPlayingCard : UiItemBase, IDataSelectable<CardBase>
    {
        [SerializeField] private TextMeshProUGUI _txtHeadline, _txtEffect;
        [SerializeField] private Button _btnSelect;
        [SerializeField] private Image _icon;
        [SerializeField] private Sprite[] _animalIcons;

        public CardBase CardReference => _cardReference;

        private CardBase _cardReference;
        private Action<IDataSelectable<CardBase>, CardBase> _onSelect;


        private void Start()
        {
            _btnSelect.onClick.RemoveAllListeners();
            _btnSelect.onClick.AddListener(() => _onSelect?.Invoke(this, _cardReference));
        }

        public void Setup(CardBase card, Action<IDataSelectable<CardBase>, CardBase> onSelectCallback)
        {
            _onSelect = onSelectCallback;
            _cardReference = card;

            _txtHeadline.text = _cardReference.Name;
            _txtEffect.text = _cardReference.GetCardText();
            SetSelectionState(false);

            SetIcon((card as AnimalCard).Animal);
        }

        private void SetIcon(AnimalsSpecies species)
        {
            _icon.sprite = _animalIcons[(int)species];
        }

        public void Refresh() => Setup(_cardReference, _onSelect);

        public void SetSelectionState(bool isSelected)
        {
            if (isSelected)
            {
                _onSelect?.Invoke(this, _cardReference);
            }
        }
    }
}
