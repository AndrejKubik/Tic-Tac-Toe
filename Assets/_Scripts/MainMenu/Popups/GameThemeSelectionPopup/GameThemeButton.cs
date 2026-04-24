using System;
using Snek.GameUIPlus;
using Snek.Utilities;
using UnityEngine;
using UnityEngine.UI;

[UseSnekInspector]
public class GameThemeButton : SnekUIButtonWithSFX
{
    [SerializeField] private GameTheme _theme;

    [Space(10f)]
    [SerializeField] private Image _symbolX;
    [SerializeField] private Image _symbolO;

    [Space(10f)]
    [SerializeField] private GameObject _selectionIndicator;

    private Action<GameThemeButton> _customExternalCallback;

    protected override void Validate()
    {
        base.Validate();

        if (!_symbolX)
            FailValidation("Symbol X image not assigned.");

        if (!_symbolO)
            FailValidation("Symbol O image not assigned.");

        if (!_selectionIndicator)
            FailValidation("Selection Indicator not assigned.");

        if (!_theme.IsValid())
            FailValidation("Invalid Game Theme data assigned.");
    }

    public void SetExternalCallback(Action<GameThemeButton> callback)
    {
        _customExternalCallback = callback;

        SetExternalCallback(OnButtonClick);
    }

    new private void OnButtonClick()
    {
        _customExternalCallback.Invoke(this);
    }

    public GameTheme GetGameTheme()
    {
        return _theme;
    }

    public void ShowSelectionIndicator(bool newState)
    {
        _selectionIndicator.SetActive(newState);
    }
}
