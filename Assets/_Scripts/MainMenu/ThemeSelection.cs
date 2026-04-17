using Snek.Utilities;
using UnityEngine;

[UseSnekInspector]
public class ThemeSelection : SnekMonoBehaviour
{
    private GameThemeButton[] _themeButtons;

    private GameThemeButton _selectedButton;

    protected override void Initialize()
    {
        _themeButtons = GetComponentsInChildren<GameThemeButton>();
    }

    protected override void Validate()
    {
        if (_themeButtons == null || _themeButtons.Length < 1)
            FailValidation("Cannot find any game theme button.");
    }

    protected override void OnInitializationSuccess()
    {
        foreach (GameThemeButton button in _themeButtons)
            button.SetExternalCallback(SelectTheme);

        SelectTheme(_themeButtons[0]);
    }

    private void SelectTheme(GameThemeButton button)
    {
        _selectedButton = button;

        foreach (GameThemeButton theme in _themeButtons)
            theme.ShowSelectionIndicator(theme == _selectedButton);
    }
}
