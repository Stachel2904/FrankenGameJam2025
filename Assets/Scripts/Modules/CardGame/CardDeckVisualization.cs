using DivineSkies.Tools.Extensions;
using TMPro;
using UnityEngine;

namespace DivineSkies.Modules.Game
{
    public class CardDeckVisualization : MonoBehaviour
    {
        [SerializeField] protected TextMeshProUGUI _amountTxt;

        public virtual void Refresh(CardBase[] containingCards)
        {
            int amount = containingCards.Length;
            _amountTxt.text = amount.ToColoredString(amount > 0);
        }
    }
}