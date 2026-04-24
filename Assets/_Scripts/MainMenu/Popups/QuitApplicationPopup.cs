using Snek.GameUIPlus;
using Snek.SingletonManager;
using Snek.Utilities;
using UnityEngine;

[UseSnekInspector]
public class QuitApplicationPopup : UIPopup
{
    private GameManager _gameManager;

    [Space(10f)]
    [SerializeField] private SnekUIButtonWithSFX _quitButton;
    [SerializeField] private SnekUIButtonWithSFX _cancelButton;

    protected override void Initialize()
    {
        base.Initialize();

        _gameManager = SnekSingletonManager.GetSingleton<GameManager>();
    }

    protected override void Validate()
    {
        if (!_gameManager)
            FailValidation("Cannot find Game Manager singleton.");

        if (!_quitButton)
            FailValidation("Play Button not assigned.");

        if (!_cancelButton)
            FailValidation("Cancel Button not assigned.");
    }

    protected override void OnInitializationSuccess()
    {
        _quitButton.SetExternalCallback(OnQuitButtonClick);
        _cancelButton.SetExternalCallback(ClosePopup);
    }

    private void OnQuitButtonClick()
    {
        _gameManager.QuitGame();
    }
}
