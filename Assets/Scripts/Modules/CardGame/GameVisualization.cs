using UnityEngine;

namespace DivineSkies.Modules.Game
{
    public enum GameAnimationType { Attack, Seduce, Heal, Displeasure, Fail, Success, Stun }

    public abstract class GameVisualization<TCard> : MonoBehaviour where TCard : CardBase
    {
        public HandCardVisualization HandCards;
        public CardDeckVisualization DrawDeck;
        public CardDeckVisualization HandDeck;
        public CardDeckVisualization DiscardDeck;

        private IGameController<TCard> _controller;

        public void Setup(IGameController<TCard> gameController)
        {
            _controller = gameController;

            Init();
        }

        protected virtual void Init() { }
    }
}