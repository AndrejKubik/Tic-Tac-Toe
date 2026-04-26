using Snek.SingletonManager;
using Snek.Utilities;

public abstract class UIPopup : SnekMonoBehaviour
{
    protected UIPopupManager _popupManager;

    protected override void Initialize()
    {
        _popupManager = SnekSingletonManager.GetSingleton<UIPopupManager>();
    }

    protected override void Validate()
    {
        if (!_popupManager)
            FailValidation("Cannot find UI Popup Manager singleton.");
    }

    protected void ClosePopup()
    {
        _popupManager.ShowPopup(this, false);
    }
}
