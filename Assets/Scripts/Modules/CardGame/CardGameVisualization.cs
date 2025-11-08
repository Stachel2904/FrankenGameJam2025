using DivineSkies.Modules.Game;
using DivineSkies.Modules.Game.Card;
using DivineSkies.Tools.Extensions;
using System;
using TMPro;
using UnityEngine;

public class CardGameVisualization : GameVisualization<AnimalCard>
{
    [SerializeField] private TextMeshProUGUI _pointsOutput, _deltaOutput, _leftTurns;

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