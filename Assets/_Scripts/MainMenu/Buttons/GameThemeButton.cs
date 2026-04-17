using System;
using Snek.GameUIPlus;
using Snek.Utilities;
using UnityEngine;
using UnityEngine.UI;

[UseSnekInspector]
public class GameThemeButton : SnekUIButtonWithSFX
{
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

    public Sprite GetSpriteX()
    {
        return _symbolX.sprite;
    }

    public Sprite GetSpriteO()
    {
        return _symbolO.sprite;
    }

    public void ShowSelectionIndicator(bool newState)
    {
        _selectionIndicator.SetActive(newState);
    }
}
