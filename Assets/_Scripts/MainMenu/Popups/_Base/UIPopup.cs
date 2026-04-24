using Snek.SingletonManager;
using Snek.Utilities;

public abstract class UIPopup : SnekMonoBehaviour
{
    protected UIPopupManager _popupManager;

    protected override void Initialize()
    {
        _popupManager = SnekSingletonManager.GetSingleton<UIPopupManager>();
    }

    protected void ClosePopup()
    {
        _popupManager.ShowPopup(this, false);
    }
}
