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
        for (int i = 0; i < _buttons.Length; i++)
        {
            PlacementGridButton button = _buttons[i];

            button.CellIndex = i;
            button.SetExternalCallback(OnGridButtonClick, button);
        }
    }

    private void OnGridButtonClick(PlacementGridButton button)
    {
        if (button.State != PlacementGridButtonState.None)
            return;

        if(_roundManager.CurrentTurn == PlayerTurn.Player1)
        {
            button.State = PlacementGridButtonState.X;
            button.SetSymbolSprite(_themeManager.GetSymbolX());
        }
        else
        {
            button.State = PlacementGridButtonState.O;
            button.SetSymbolSprite(_themeManager.GetSymbolO());
        }

        button.EnableInteraction(false);

        _roundManager.EndTurn(button.CellIndex, button.State);
    }
}

