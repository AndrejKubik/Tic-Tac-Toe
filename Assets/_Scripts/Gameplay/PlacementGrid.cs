using Snek.SingletonManager;
using Snek.Utilities;
using UnityEngine;

[UseSnekInspector]
public class PlacementGrid : SnekMonoBehaviour
{
    private const int Columns = 3;
    private const int Rows = 3;

    public const int TotalCells = Rows * Columns;

    private GameRoundManager _roundManager;
    private GameThemeManager _themeManager;

    private PlacementGridButton[] _buttons;

    protected override void Initialize()
    {
        _roundManager = SnekSingletonManager.GetSingleton<GameRoundManager>();
        _themeManager = SnekSingletonManager.GetSingleton<GameThemeManager>();

        _buttons = GetComponentsInChildren<PlacementGridButton>(true);
    }

    protected override void Validate()
    {
        if (!_roundManager)
            FailValidation("Cannot find Game Round Manager singleton.");

        if (!_themeManager)
            FailValidation("Cannot find Game Theme Manager singleton.");

        if (_buttons == null || _buttons.Length != TotalCells)
            FailValidation($"Number of found placement grid buttons is invalid, must be [{TotalCells}].");
    }

    protected override void OnInitializationSuccess()
    {
        _roundManager.OnNewRoundStarted += ResetAllCells;

        for (int i = 0; i < _buttons.Length; i++)
        {
            PlacementGridButton button = _buttons[i];

            button.CellIndex = i;
            button.SetExternalCallback(OnGridButtonClick, button);
        }
    }

    private void OnDestroy()
    {
        _roundManager.OnNewRoundStarted -= ResetAllCells;
    }

    public void ResetAllCells()
    {
        foreach (PlacementGridButton button in _buttons)
        {
            button.State = PlacementGridButtonState.None;
            button.SetSymbolSprite(null);
            button.EnableInteraction(true);
        }
    }

    private void OnGridButtonClick(PlacementGridButton button)
    {
        if (button.State != PlacementGridButtonState.None)
            return;

        if (_roundManager.GetCurrentTurnSymbol() == PlacementGridButtonState.X)
        {
            button.State = PlacementGridButtonState.X;
            button.SetSymbolSprite(_themeManager.GetSymbolX());
        }
        else if (_roundManager.GetCurrentTurnSymbol() == PlacementGridButtonState.O)
        {
            button.State = PlacementGridButtonState.O;
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

        _roundManager.EndTurn(button.CellIndex, button.State);
    }
}

