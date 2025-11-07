namespace DivineSkies.Modules.Game
{
    public enum GameEndReason { Successful, AltSuccessful, Consensual, Unwillingly, Cancelled}

    public interface IGameController<TCard> where TCard : CardBase
    {
        public GameVisualization<TCard> Visualization { get; }
        public int StaminaPoints { get; set; }
        public void NextTurn();
        public void DrawCard(int amount);
        public void End(GameEndReason reason);
    }

    public abstract class GameController<TCard, TModule> : ModuleBase<TModule>, IGameController<TCard> where TCard : CardBase where TModule : Core.ModuleBase
    {
        public GameVisualization<TCard> Visualization
        {
            get
            {
                _visualization ??= FindAnyObjectByType<GameVisualization<TCard>>();
                return _visualization;
            }
        }
        private GameVisualization<TCard> _visualization;

        protected int _currentStaminaPoints;
        public virtual int StaminaPoints { get => _currentStaminaPoints; set { _currentStaminaPoints = value; Visualization.UpdateStamina(_currentStaminaPoints); } }

        protected CardDeck<TCard> _drawDeck;
        protected CardDeck<TCard> _discardDeck;
        protected CardDeck<TCard> _handDeck;

        protected CardUseStrategyBase<TCard> _cardUseStrategy;

        public override void Initialize()
        {
            _discardDeck = new CardDeck<TCard>(Visualization.DiscardDeck); //needs to be created first, so we can reference it as backup deck
            _handDeck = new CardDeck<TCard>(Visualization.HandDeck);
            _drawDeck = new CardDeck<TCard>(GetDeckCards(), _discardDeck, Visualization.DrawDeck);
        }

        protected abstract TCard[] GetDeckCards();

        public abstract void NextTurn();

        public virtual void DrawCard(int amount)
        {
            TCard[] result = new TCard[amount];
            for (int i = 0; i < amount; i++)
            {
                result[i] = _drawDeck.DrawTopCard();
                _handDeck.AddCard(result[i]);
            }

            for (int i = 0; i < result.Length; i++)
                Visualization.HandCards.VisualizeNewHandCard(result[i]);
            Visualization.HandCards.RearrangeHandCards();
            Visualization.HandCards.RefreshCards(this);
        }

        public void TryPlayCard(TCard card, bool fromOpponent)
        {
            if (_cardUseStrategy.CheckStaminaCost && !fromOpponent)
            {
                if (StaminaPoints < card.Cost)
                {
                    this.PrintMessage("You have not enough stamina to play this card, wait for the next turn");
                    return;
                }
                StaminaPoints -= card.Cost;
                _discardDeck.AddCard(card);
                _handDeck.RemoveCard(card);
                Visualization.HandCards.RemoveHandCard(card);
            }

            if (_cardUseStrategy.PrintDescription)
            {
                Visualization.PrintDescription(card.PlayText, true);
            }

            _cardUseStrategy.UseCard(card);
        }

        public abstract void End(GameEndReason reason);
    }
}