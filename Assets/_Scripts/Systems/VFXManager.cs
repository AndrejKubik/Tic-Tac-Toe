using System;
using Snek.SingletonManager;
using Snek.Utilities;
using UnityEngine;

[UseSnekInspector]
public class VFXManager : SnekMonoSingleton
{
    private GameRoundManager _roundManager;

    [SerializeField] private PlayerWinEffect _playerWinEffect;

    protected override void Initialize()
    {
        _roundManager = SnekSingletonManager.GetSingleton<GameRoundManager>();
    }

    protected override void Validate()
    {
        if (!_roundManager)
            FailValidation("Cannot find Game Round Manager singleton.");

        if (!_playerWinEffect)
            FailValidation("Player win effect not assigned.");
    }

    protected override void OnInitializationSuccess()
    {
        _roundManager.OnNewRoundStarted += OnNewRoundStart; 
    }

    private void OnDestroy()
    {
        _roundManager.OnNewRoundStarted -= OnNewRoundStart;
    }

    private void OnNewRoundStart()
    {
        _playerWinEffect.Stop();
    }

    public void PlayPlayerWinEffect(Vector3 startPosition, Vector3 endPosition, Action onFinishCallback = null)
    {
        _playerWinEffect.Play(startPosition, endPosition, onFinishCallback);
    }
}
