namespace DivineSkies.Modules.Game
{
    public abstract class CardUseStrategyBase<TCard> where TCard : CardBase
    {
        public virtual bool CheckStaminaCost => true;
        public virtual bool PrintDescription => true;

        protected IGameController<TCard> _controller;

        public CardUseStrategyBase(IGameController<TCard> controller)
        {
            _controller = controller;
        }

        public abstract void UseCard(TCard card);
    }
}