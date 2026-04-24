using System;
using UnityEngine;

[Serializable]
public struct RoundData
{
    public PlacementGridCellState Player1Symbol;
    public PlacementGridCellState Player2Symbol;

    [Space(10f)]
    public RoundResult Result;

    [Space(10f)]
    public int TotalMoves;
    public int Player1Moves;
    public int Player2Moves;

    [Space(10f)]
    public float ElapsedTime;
}
