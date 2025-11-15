using DivineSkies.Modules.Game.TurnBased.Card;
using DivineSkies.Tools.Extensions;
using TMPro;
using UnityEngine;

namespace DivineSkies.Modules.Game.Card
{
    public class AnimalGameVisualization : MonoBehaviour, ICardGameVisualization
    {
        [SerializeField] private TextMeshProUGUI _pointsOutput, _deltaOutput, _leftTurns;
        [SerializeField] private FocusingHandCardVisualization _handCards;
        public HandCardVisualization HandCards => _handCards;

        [SerializeField] private ContentCardDeckVisualization _drawDeck;
        public CardDeckVisualization DrawDeck => _drawDeck;

        [SerializeField] private ContentCardDeckVisualization _discardDeck;
        public CardDeckVisualization DiscardDeck => _discardDeck;

        public void SetController(IGameController controller)
        {
        }

        public void RefreshPointDisplay(int newPoints, int deltaPoints)
        {
            _pointsOutput.text = "Score: " + newPoints.ToString();
            _deltaOutput.text = deltaPoints == 0 ? "" : deltaPoints.ToSignedStringWithColorTag();
        }

        public void RefreshLeftTurns(int leftTurns)
        {
            _leftTurns.text = leftTurns + " turn" + (leftTurns == 1 ? "" : "s") + " left";
        }
    }
}