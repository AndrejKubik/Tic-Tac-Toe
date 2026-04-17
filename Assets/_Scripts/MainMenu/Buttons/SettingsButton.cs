using Snek.GameUIPlus;
using Snek.SingletonManager;
using Snek.Utilities;
using SnekEditor.SettingsMenu;

[UseSnekInspector]
public class SettingsButton : SnekUIButtonWithSFX
{
    private SnekSettingsMenu _settingsMenu;

    protected override void Initialize()
    {
        base.Initialize();

        _settingsMenu = SnekSingletonManager.GetSingleton<SnekSettingsMenu>();
    }

    protected override void OnButtonClick()
    {
        base.OnButtonClick();

        _settingsMenu.ShowMenu(true);
    }
}
