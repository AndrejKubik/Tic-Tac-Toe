using System;
using UnityEngine;

[Serializable]
public struct RoundData
{
    public PlacementGridCellState Player1Symbol;
    public PlacementGridCellState Player2Symbol;

    [Space(10f)]
    public RoundResult Result;
    public int TotalMoves;
    public float ElapsedTime;
}
