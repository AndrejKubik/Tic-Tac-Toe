using Snek.GameUIPlus;
using Snek.SingletonManager;
using Snek.Utilities;

[UseSnekInspector]
public class PlayButton : SnekUIButtonWithSFX
{
    private UIPopupManager _popupManager;

    protected override void Initialize()
    {
        base.Initialize();

        _popupManager = SnekSingletonManager.GetSingleton<UIPopupManager>();
    }

    protected override void Validate()
    {
        base.Validate();

        if (!_popupManager)
            FailValidation("Cannot find UI Popup Manager singleton.");
    }

    protected override void OnButtonClick()
    {
        base.OnButtonClick();

        _popupManager.ShowPopup<GameThemeSelectionPopup>(true);
    }
}
