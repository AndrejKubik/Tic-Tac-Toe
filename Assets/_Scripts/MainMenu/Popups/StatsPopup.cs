using Snek.GameUIPlus;
using Snek.Utilities;
using UnityEngine;

[UseSnekInspector]
public class StatsPopup : UIPopup
{
    [SerializeField] private SnekUIButtonWithSFX _closeButton;

    protected override void Validate()
    {
        if (!_closeButton)
            FailValidation("Cancel Button not assigned.");
    }

    protected override void OnInitializationSuccess()
    {
        _closeButton.SetExternalCallback(ClosePopup);
    }
}
