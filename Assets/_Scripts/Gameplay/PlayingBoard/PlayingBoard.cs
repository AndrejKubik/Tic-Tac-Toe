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

    private GameRoundManager _roundManager;
    private GameThemeManager _themeManager;

    private PlacementGridCellButton[] _buttons;
    
    [SerializeField] private Image _background;

    protected override void Initialize()
    {
        _roundManager = SnekSingletonManager.GetSingleton<GameRoundManager>();
        _themeManager = SnekSingletonManager.GetSingleton<GameThemeManager>();

        _buttons = GetComponentsInChildren<PlacementGridCellButton>(true);
    }

    protected override void Validate()
    {
        if (!_roundManager)
            FailValidation("Cannot find Game Round Manager singleton.");

        if (!_themeManager)
            FailValidation("Cannot find Game Theme Manager singleton.");

        if (_buttons == null || _buttons.Length != TotalCells)
            FailValidation($"Number of found placement grid buttons is invalid, must be [{TotalCells}].");

        if (!_background)
            FailValidation("Background image not assigned.");
    }

    protected override void OnInitializationSuccess()
    {
        _roundManager.OnNewRoundStarted += OnNewRoundStart;

        for (int i = 0; i < _buttons.Length; i++)
        {
            PlacementGridCellButton button = _buttons[i];

            button.CellIndex = i;
            button.SetExternalCallback(OnGridButtonClick, button);
        }
    }

    private void OnDestroy()
    {
        _roundManager.OnNewRoundStarted -= OnNewRoundStart;
    }

    private void OnNewRoundStart()
    {
        ResetAllCells();

        _background.color = _themeManager.GetBackgroundColor();
    }

    public void ResetAllCells()
    {
        foreach (PlacementGridCellButton button in _buttons)
        {
            button.CellState = PlacementGridCellState.None;
            button.SetSymbolSprite(null);
            button.EnableInteraction(true);
        }
    }

    private void OnGridButtonClick(PlacementGridCellButton button)
    {
        if (button.CellState != PlacementGridCellState.None)
            return;

        if (_roundManager.GetCurrentTurnSymbol() == PlacementGridCellState.X)
        {
            button.CellState = PlacementGridCellState.X;
            button.SetSymbolSprite(_themeManager.GetSymbolX());
        }
        else if (_roundManager.GetCurrentTurnSymbol() == PlacementGridCellState.O)
        {
            button.CellState = PlacementGridCellState.O;
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

        _roundManager.EndTurn(button.CellIndex, button.CellState);
    }
}

