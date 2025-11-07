using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DivineSkies.Modules.ToolTip;
using DivineSkies.Modules.UI;

namespace DivineSkies.Modules.Game
{
    public class VisualPlayingCard : UiItemBase, IDataSelectable<CardBase>
    {
        [SerializeField] private TextMeshProUGUI _txtStaminaCost, _txtHeadline, _txtEffect;
        [SerializeField] private GameObject _negativeState;
        [SerializeField] private Button _btnSelect;
        [SerializeField] private Sprite _cardBGSpriteCombat, _cardBGSpriteDate, _cardBGSpriteSex;
        [SerializeField] private Image _cardBG;
        [SerializeField] private ToolTipProvider _toolTipTarget;

        private readonly Color _validStamina = new(0.2f, 0.15f, 0.5f, 1f), _missingStamina = new(0.8f, 0f, 0f, 1f);

        public CardBase CardReference => _cardReference;

        public bool SetLastSiblingOnHightlight = true;
        public bool AnimateHighlight = true;

        private CardBase _cardReference;
        private Vector3 preHighlightLocation;
        private Quaternion preHighlightRotation;
        private int preHighlightSiblingIndex;
        private Action<IDataSelectable<CardBase>, CardBase> _onSelect;


        private void Start()
        {
            EventTrigger.Entry highlightTrigger = new EventTrigger.Entry();
            highlightTrigger.eventID = EventTriggerType.PointerEnter;
            highlightTrigger.callback.AddListener((eventData) => { Highlight(); });
            _btnSelect.GetComponent<EventTrigger>().triggers.Add(highlightTrigger);

            EventTrigger.Entry dehighlightTrigger = new EventTrigger.Entry();
            dehighlightTrigger.eventID = EventTriggerType.PointerExit;
            dehighlightTrigger.callback.AddListener((eventData) => { Dehighlight(); });
            _btnSelect.GetComponent<EventTrigger>().triggers.Add(dehighlightTrigger);

            _btnSelect.onClick.RemoveAllListeners();
            _btnSelect.onClick.AddListener(() => _onSelect?.Invoke(this, _cardReference));
        }

        public void Setup(CardBase card, Action<IDataSelectable<CardBase>, CardBase> onSelectCallback)
        {
            _onSelect = onSelectCallback;
            _cardReference = card;

            _txtStaminaCost.text = _cardReference.Cost.ToString();
            _txtHeadline.text = _cardReference.Name;
            _txtEffect.text = _cardReference.GetCardText();
            SetSelectionState(false);

            SetBG(card.GetType());
        }

        private void SetBG(Type cardType)
        {
        }

        public void Refresh() => Setup(_cardReference, _onSelect);

        public void RefreshStaminaColor(int currentStamina)
        {
            _txtStaminaCost.color = currentStamina >= _cardReference.Cost ? _validStamina : _missingStamina;
        }

        public void SetSelectionState(bool isNegative) => _negativeState.SetActive(isNegative);

        private void Highlight()
        {
            if (!AnimateHighlight)
            {
                return;
            }

            preHighlightLocation = transform.localPosition;
            preHighlightRotation = transform.rotation;
            preHighlightSiblingIndex = transform.GetSiblingIndex();

            transform.localPosition = new Vector3(preHighlightLocation.x, 250, 1);
            transform.rotation = Quaternion.identity;
            transform.localScale = Vector3.one * 1.5f;
            if(SetLastSiblingOnHightlight)
                transform.SetAsLastSibling();
        }

        private void Dehighlight()
        {
            if (!AnimateHighlight)
            {
                return;
            }

            transform.localPosition = preHighlightLocation;
            transform.rotation = preHighlightRotation;
            transform.localScale = Vector3.one;
            if (SetLastSiblingOnHightlight)
                transform.SetSiblingIndex(preHighlightSiblingIndex);
        }
    }
}
