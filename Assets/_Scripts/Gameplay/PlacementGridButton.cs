using Snek.GameUIPlus;
using Snek.Utilities;
using UnityEngine;
using UnityEngine.UI;

[UseSnekInspector]
[RequireComponent(typeof(Image))]
public class PlacementGridButton : SnekUIButtonWithSFX
{
    [SerializeField] private Image _symbol;

    public int CellIndex;
    public PlacementGridButtonState State = PlacementGridButtonState.None;

    protected override void Validate()
    {
        base.Validate();

        if (!_symbol)
            FailValidation("Symbol Image component not assigned.");
    }

    public void SetSymbolSprite(Sprite sprite)
    {
        _symbol.sprite = sprite;
    }
}
