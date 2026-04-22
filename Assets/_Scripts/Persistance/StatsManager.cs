using System.Collections.Generic;
using Snek.SingletonManager;
using Snek.Utilities;
using UnityEngine;

[UseSnekInspector]
public class StatsManager : SnekMonoSingleton
{
    private PersistenceManager _persistenceManager;

    [SerializeField] private List<RoundData> _playedRounds;

    protected override void Initialize()
    {
        _persistenceManager = SnekSingletonManager.GetSingleton<PersistenceManager>();
    }

    protected override void Validate()
    {
        if (!_persistenceManager)
            FailValidation("Cannot find Persistence Manager singleton.");
    }

    protected override void OnInitializationSuccess()
    {
        _playedRounds = _persistenceManager.LoadPlayedRounds();
    }

    public void StoreRoundData(RoundData round)
    {
        _playedRounds.Add(round);

        _persistenceManager.SavePlayedRounds(_playedRounds);
    }

    public int GetGamesPlayedCount()
    {
        return _playedRounds.Count;
    }

    public int GetGamesWithResult(RoundResult roundResult)
    {
        List<RoundData> filteredGames = _playedRounds.FindAll(game => game.Result == roundResult);

        return filteredGames.Count;
    }

    public float GetAverageRoundDuration()
    {
        if (_playedRounds == null || GetGamesPlayedCount() == 0)
            return 0f;

        float sum = 0f;

        foreach (RoundData round in _playedRounds)
            sum += round.ElapsedTime;

        return sum / GetGamesPlayedCount();
    }
}
