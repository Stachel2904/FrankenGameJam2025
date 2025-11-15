using UnityEngine;
using DivineSkies.Modules.Game.TurnBased.Card;

namespace DivineSkies.Modules.Game
{
    public class FocusingHandCardVisualization : HandCardVisualization
    {
        bool _isFocused = false;
        private void Update()
        {
            if(Input.mousePosition.y < 50 && !_isFocused)
            {
                _isFocused = true;
                Focus();
            }
            else if(Input.mousePosition.y > 200 && _isFocused)
            {
                _isFocused = false;
                Unfocus();
            }
        }

        private void Focus()
        {
            (transform as RectTransform).anchoredPosition = Vector2.up * 120;
        }

        private void Unfocus()
        {
            (transform as RectTransform).anchoredPosition = Vector2.zero;
        }
    }
}