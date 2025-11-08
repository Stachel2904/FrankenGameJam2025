using System.Collections.Generic;
using UnityEngine;
using DivineSkies.Tools.Extensions;
using DivineSkies.Modules.ResourceManagement;

namespace DivineSkies.Modules.Game
{
    public class HandCardVisualization : MonoBehaviour
    {
        private readonly List<VisualPlayingCard> _visualHandCards = new List<VisualPlayingCard>();

        public void RearrangeHandCards()
        {
            int amount = _visualHandCards.Count;
            int cardDistance = 120;
            float locationValue = -8 * amount + 10;
            float rotationValue = -3 * amount;
            for (int i = 0; i < amount; i++)
            {
                float centeredIndex = (amount == 1) ? 0 : i / (float)(amount - 1) * 2 - 1;
                _visualHandCards[i].transform.localPosition = new Vector3(centeredIndex * cardDistance * amount / 2, locationValue * Mathf.Pow(centeredIndex, 2), 0);
                _visualHandCards[i].transform.rotation = Quaternion.Euler(0, 0, rotationValue * centeredIndex);
                _visualHandCards[i].transform.localScale = Vector3.one;
            }
        }

        public void RefreshCards<TCard>(IGameController<TCard> controller) where TCard : CardBase
        {
        }

        /// <summary>
        /// Call "Rearrange Cards" afterwards.
        /// </summary>
        public void VisualizeNewHandCard(CardBase card)
        {
            VisualPlayingCard createdCard = ResourceController.Main.LoadAndInstatiatePrefab<VisualPlayingCard>(transform);
            createdCard.Setup(card, (i, c) => c.Play());
            _visualHandCards.Add(createdCard);
        }

        /// <summary>
        /// Call "Rearrange Cards" afterwards.
        /// </summary>
        public void RemoveHandCard(CardBase card, bool automaticRearrange = true)
        {
            if (!_visualHandCards.TryFind((c) => c.CardReference == card, out VisualPlayingCard cardToDestroy))
            {
                Debug.LogWarning("You tried to Destroy a handcard, that wasn't found.");
                return;
            }

            //Destroy Visual Card
            _visualHandCards.Remove(cardToDestroy);
            Destroy(cardToDestroy.gameObject);

            if (automaticRearrange)
                RearrangeHandCards();
        }

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

        public void Focus()
        {
            (transform as RectTransform).anchoredPosition = Vector2.up * 100;
        }

        public void Unfocus()
        {
            (transform as RectTransform).anchoredPosition = Vector2.zero;
        }
    }
}