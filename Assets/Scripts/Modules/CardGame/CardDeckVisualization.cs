using UnityEngine;
using TMPro;
using DivineSkies.Tools.Extensions;

namespace DivineSkies.Modules.Game
{
    public class CardDeckVisualization : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _amountTxt;

        public void Refresh(int amount)
        {
            _amountTxt.text = amount.ToColoredString(amount > 0);
        }
    }
}