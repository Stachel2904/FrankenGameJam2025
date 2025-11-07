using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DivineSkies.Modules.UI;
using System.Collections;
using DivineSkies.UiAnimations;

namespace DivineSkies.Modules.Game
{
    public enum GameAnimationType { Attack, Seduce, Heal, Displeasure, Fail, Success, Stun }

    public abstract class GameVisualization<TCard> : MonoBehaviour where TCard : CardBase
    {
        public HandCardVisualization HandCards;
        public CardDeckVisualization DrawDeck;
        public CardDeckVisualization HandDeck;
        public CardDeckVisualization DiscardDeck;

        [SerializeField] private Button _btnNextTurn;
        [SerializeField] private Transform _staminaSourceIcon;
        [SerializeField] private WritingTextMeshPro _txtDescription;
        [SerializeField] private TextMeshProUGUI _levelText, _nextLevelText;

        private readonly List<string> _descriptionQueue = new List<string>();

        private bool _queueIsRunning;
        private IGameController<TCard> _controller;
        private int _maxStaminaPoints;
        private Color _aciveStaminaColor, _inactiveStaminaColor, _overflowStaminaColor;

        public void Setup(IGameController<TCard> gameController)
        {
            _controller = gameController;
            _btnNextTurn?.onClick.AddListener(_controller.NextTurn);
            _maxStaminaPoints = _controller.StaminaPoints;

            for (int i = _staminaSourceIcon.parent.childCount; i < _maxStaminaPoints; i++)
            {
                Instantiate(_staminaSourceIcon, _staminaSourceIcon.parent);
            }

            _aciveStaminaColor = _staminaSourceIcon.GetComponent<Image>().color;
            _inactiveStaminaColor = Color.gray;
            _overflowStaminaColor = new Color(0f, 0.85f, 1f);

            Init();

            UpdateStamina(_controller.StaminaPoints);
        }

        protected virtual void Init() { }

        public void UpdateStamina(int value)
        {
            //Set Color of standard stamina
            for (int i = 0; i < Mathf.Min(_staminaSourceIcon.parent.childCount, _maxStaminaPoints); i++)
                _staminaSourceIcon.parent.GetChild(i).GetComponent<Image>().color = i < value ? _aciveStaminaColor : _inactiveStaminaColor;

            //Create overflow
            for (int i = _staminaSourceIcon.parent.childCount; i < value; i++)
                Instantiate(_staminaSourceIcon, _staminaSourceIcon.parent).GetComponent<Image>().color = _overflowStaminaColor;

            //Destroy overflow
            for (int i = _staminaSourceIcon.parent.childCount - 1; i >= Mathf.Max(_maxStaminaPoints, value); i--)
                Destroy(_staminaSourceIcon.parent.GetChild(i).gameObject);

            HandCards.RefreshCards(_controller);
        }

        public void PrintDescription(string text, bool overrideQueue = false)
        {
            if(overrideQueue)
                _descriptionQueue.Clear();

            if(!_queueIsRunning) //if queue is empty, restart writing loop anew
                _txtDescription.StartWriting(text, writingFinishCallback: () => StartCoroutine(PrintNextDescription()));

            _descriptionQueue.Add(text);
        }

        private IEnumerator PrintNextDescription()
        {
            _descriptionQueue.RemoveAt(0);
            _queueIsRunning = true;

            yield return new WaitForSeconds(1f);

            if (_descriptionQueue.Count > 0)
                _txtDescription.StartWriting(_descriptionQueue[0], writingFinishCallback: () => StartCoroutine(PrintNextDescription()));

            _queueIsRunning = false;
        }
    }
}