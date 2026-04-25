using Snek.GameUIPlus;
using Snek.Utilities;
using UnityEngine;
using UnityEngine.UI;

[UseSnekInspector]
[RequireComponent(typeof(Image))]
public class PlacementGridCellButton : SnekUIButtonWithSFX
{
    private RectTransform _rectTransform;

    [SerializeField] private Image _symbol;

    public int CellIndex;
    public PlacementGridCellState CellState = PlacementGridCellState.None;

    protected override void Initialize()
    {
        base.Initialize();

        _rectTransform = GetComponent<RectTransform>();
    }

    protected override void Validate()
    {
        base.Validate();

        if (!_rectTransform)
            FailValidation("Cannot find Rect Transform component.");

        if (!_symbol)
            FailValidation("Symbol Image component not assigned.");
    }

    public void SetSymbolSprite(Sprite sprite)
    {
        _symbol.sprite = sprite;

        _symbol.color = sprite == null ?
            Color.clear : Color.white;
    }

    public Vector3 GetWorldPosition()
    {
        return _rectTransform.position;
    }
}
