using Snek.GameUIPlus;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class PlacementGridButton : SnekUIButtonWithSFX
{
    private Image _image;

    private PlacementGridButtonState _state = PlacementGridButtonState.None;

    protected override void Initialize()
    {
        base.Initialize();

        _image = GetComponent<Image>();
    }

    protected override void Validate()
    {
        base.Validate();

        if (!_image)
            FailValidation("Image component not found.");
    }

    protected override void OnButtonClick()
    {
        base.OnButtonClick();

        
    }
}
