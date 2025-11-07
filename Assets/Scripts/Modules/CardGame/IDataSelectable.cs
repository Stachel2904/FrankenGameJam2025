using System;

namespace DivineSkies.Modules.Game
{
    public interface IDataSelectable<TData>
    {
        public void SetSelectionState(bool isSelected);
        public void Setup(TData data, Action<IDataSelectable<TData>, TData> onSelect);
    }
}