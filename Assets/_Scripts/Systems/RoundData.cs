using System;
using UnityEngine;

[Serializable]
public struct RoundData
{
    public PlacementGridButtonState Player1Symbol;
    public PlacementGridButtonState Player2Symbol;

    [Space(10f)]
    public RoundResult Result;
    public int TotalMoves;
    public float ElapsedTime;
}
