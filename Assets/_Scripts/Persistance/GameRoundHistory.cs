using System.Collections.Generic;
using Snek.Utilities;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(GameRoundHistory), fileName = nameof(GameRoundHistory))]
[UseSnekInspector]
public class GameRoundHistory : SnekScriptableObject
{
    [SerializeField] private List<RoundData> _playedRounds = new List<RoundData>();

    public void StoreRoundData(RoundData round)
    {
        _playedRounds.Add(round);
    }
}
