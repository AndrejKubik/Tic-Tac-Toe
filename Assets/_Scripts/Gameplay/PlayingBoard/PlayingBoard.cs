using System;
using Snek.SingletonManager;
using Snek.Utilities;
using UnityEngine;
using UnityEngine.UI;

[UseSnekInspector]
public class PlayingBoard : SnekMonoBehaviour
{
    private const int Columns = 3;
    private const int Rows = 3;

    public const int TotalCells = Rows * Columns;

    private GameManager _gameManager;
    private GameRoundManager _roundManager;
    private GameThemeManager _themeManager;
    private VFXManager _vfxManager;
    private UIPopupManager _popupManager;

    private PlacementGridCellButton[] _cellButtons;

    [SerializeField] private Image _background;

    protected override void Initialize()
    {
        _gameManager = SnekSingletonManager.GetSingleton<GameManager>();
        _roundManager = SnekSingletonManager.GetSingleton<GameRoundManager>();
        _themeManager = SnekSingletonManager.GetSingleton<GameThemeManager>();
        _vfxManager = SnekSingletonManager.GetSingleton<VFXManager>();
        _popupManager = SnekSingletonManager.GetSingleton<UIPopupManager>();

        _cellButtons = GetComponentsInChildren<PlacementGridCellButton>(true);
    }

    protected override void Validate()
    {
        if (!_gameManager)
            FailValidation("Cannot find Game Manager singleton.");

        if (!_roundManager)
            FailValidation("Cannot find Game Round Manager singleton.");

        if (!_themeManager)
            FailValidation("Cannot find Game Theme Manager singleton.");

        if (!_vfxManager)
            FailValidation("Cannot find VFX Manager singleton.");

        if (!_popupManager)
            FailValidation("Cannot find UI Popup Manager singleton.");

        if (_cellButtons == null || _cellButtons.Length != TotalCells)
            FailValidation($"Number of found placement grid buttons is invalid, must be [{TotalCells}].");

        if (!_background)
            FailValidation("Background image not assigned.");
    }

    protected override void OnInitializationSuccess()
    {
        _gameManager.OnGameStarted += OnNewGameStart;
        _roundManager.OnRoundStarted += OnNewRoundStart;
        _roundManager.OnRoundFinished += OnRoundFinish;

        for (int i = 0; i < _cellButtons.Length; i++)
        {
            PlacementGridCellButton button = _cellButtons[i];

            button.CellIndex = i;
            button.SetExternalCallback(OnGridButtonClick, button);
        }
    }

    private void OnDestroy()
    {
        _gameManager.OnGameStarted -= OnNewGameStart;
        _roundManager.OnRoundStarted -= OnNewRoundStart;
        _roundManager.OnRoundFinished -= OnRoundFinish;
    }

    private void OnNewGameStart()
    {
        foreach (PlacementGridCellButton button in _cellButtons)
            button.SetBorderColor(_themeManager.GetGridColor());
    }

    private void OnNewRoundStart()
    {
        ResetAllCells();

        _background.color = _themeManager.GetBackgroundColor();
    }

    private void OnRoundFinish(int[] winCombo, int lastPlayedCellIndex)
    {
        foreach (PlacementGridCellButton button in _cellButtons)
            button.EnableInteraction(false);

        int end1CellIndex = winCombo[0];
        int end2CellIndex = winCombo[2];

        PlacementGridCellButton startCell = lastPlayedCellIndex == end2CellIndex ?
            _cellButtons[end1CellIndex] : _cellButtons[end2CellIndex];

        PlacementGridCellButton endCell = lastPlayedCellIndex == end2CellIndex ?
            _cellButtons[end2CellIndex] : _cellButtons[end1CellIndex];

        _vfxManager.PlayPlayerWinEffect(
            startCell.GetWorldPosition(),
            endCell.GetWorldPosition(),
            _cellButtons[lastPlayedCellIndex].CellSymbol,
            OnPlayerWinEffectFinish);
    }

    public void ResetAllCells()
    {
        foreach (PlacementGridCellButton button in _cellButtons)
        {
            button.CellSymbol = PlacementGridCellSymbol.None;
            button.SetSymbolSprite(null);
            button.EnableInteraction(true);
        }
    }

    private void OnGridButtonClick(PlacementGridCellButton button)
    {
        if (button.CellSymbol != PlacementGridCellSymbol.None)
            return;

        if (_roundManager.GetCurrentTurnSymbol() == PlacementGridCellSymbol.X)
        {
            button.CellSymbol = PlacementGridCellSymbol.X;
            button.SetSymbolSprite(_themeManager.GetSymbolX());
        }
        else if (_roundManager.GetCurrentTurnSymbol() == PlacementGridCellSymbol.O)
        {
            button.CellSymbol = PlacementGridCellSymbol.O;
            button.SetSymbolSprite(_themeManager.GetSymbolO());
        }
        else
        {
            Debug.LogError(
                $"Trying to place NONE on a cell outside of {nameof(ResetAllCells)}() method.\n" +
                $"Aborting...");

            return;
        }

        button.EnableInteraction(false);

        _roundManager.EndTurn(button.CellIndex, button.CellSymbol);
    }

    private void OnPlayerWinEffectFinish()
    {
        _popupManager.ShowPopup<RoundFinishedPopup>(true);
    }
}

