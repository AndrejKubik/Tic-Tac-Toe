using Snek.SingletonManager;
using SnekEditor.SettingsMenu;

public class SettingsMenu : SnekSettingsMenu
{
    private ScreenLayoutManager _screenLayoutManager;

    protected override void Initialize()
    {
        _screenLayoutManager = SnekSingletonManager.GetSingleton<ScreenLayoutManager>();
    }

    protected override void Validate()
    {
        base.Validate();

        if (!_screenLayoutManager)
            FailValidation("Cannot find Screen Layout Manager singleton.");
    }
}
