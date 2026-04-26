using System;
using Snek.SingletonManager;
using Snek.Utilities;
using UnityEngine;

[UseSnekInspector]
public class VFXManager : SnekMonoSingleton
{
    private GameManager _gameManager;
    private GameRoundManager _roundManager;
    private GameThemeManager _themeManager;

    [SerializeField] private PlayerWinEffect _playerWinEffect;

    protected override void Initialize()
    {
        _gameManager = SnekSingletonManager.GetSingleton<GameManager>();
        _roundManager = SnekSingletonManager.GetSingleton<GameRoundManager>();
        _themeManager = SnekSingletonManager.GetSingleton<GameThemeManager>();
    }

    protected override void Validate()
    {
        if (!_gameManager)
            FailValidation("Cannot find Game Manager singleton.");

        if (!_roundManager)
            FailValidation("Cannot find Game Round Manager singleton.");

        if (!_themeManager)
            FailValidation("Cannot find Game Theme Manager singleton.");

        if (!_playerWinEffect)
            FailValidation("Player win effect not assigned.");
    }

    protected override void OnInitializationSuccess()
    {
        _gameManager.OnReturnedToMainMenu += HidePlayerWinEffect; 
        _roundManager.OnRoundStarted += HidePlayerWinEffect; 
    }

    private void OnDestroy()
    {
        _gameManager.OnReturnedToMainMenu -= HidePlayerWinEffect;
        _roundManager.OnRoundStarted -= HidePlayerWinEffect;
    }

    public void PlayPlayerWinEffect(
        Vector3 startPosition,
        Vector3 endPosition,
        PlacementGridCellSymbol winSymbol,
        Action onFinishCallback = null)
    {
        if(winSymbol == PlacementGridCellSymbol.None)
        {
            Debug.LogError("Cannot play player win effect for NONE symbol(empty cell).");

            return;
        }

        //_playerWinEffect.SetColor(GetPlayerWinEffectColor(winSymbol));
        _playerWinEffect.SetColor(Color.orangeRed);
        _playerWinEffect.Play(startPosition, endPosition, onFinishCallback);
    }

    private void HidePlayerWinEffect()
    {
        _playerWinEffect.Stop();
    }

    private Color GetPlayerWinEffectColor(PlacementGridCellSymbol winSymbol)
    {
        return winSymbol switch
        {
            PlacementGridCellSymbol.X => _themeManager.GetSymbolXColor(),
            PlacementGridCellSymbol.O => _themeManager.GetSymbolOColor(),
            _ => Color.white,
        };
    }
}
