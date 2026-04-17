using Snek.GameUIPlus;
using Snek.SingletonManager;
using Snek.Utilities;

[UseSnekInspector]
public class ExitButton : SnekUIButtonWithSFX
{
    private GameManager _gameManager;

    protected override void Initialize()
    {
        base.Initialize();

        _gameManager = SnekSingletonManager.GetSingleton<GameManager>();
    }

    protected override void OnButtonClick()
    {
        base.OnButtonClick();

        _gameManager.ExitGame();
    }
}
