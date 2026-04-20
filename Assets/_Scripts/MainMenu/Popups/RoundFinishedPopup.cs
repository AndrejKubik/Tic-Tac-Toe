using Snek.GameUIPlus;
using Snek.SingletonManager;
using Snek.Utilities;
using UnityEngine;

[UseSnekInspector]
public class RoundFinishedPopup : UIPopup
{
    private GameManager _gameManager;

    [SerializeField] private SnekUIButtonWithSFX _retryButton;
    [SerializeField] private SnekUIButtonWithSFX _exitButton;

    protected override void Initialize()
    {
        base.Initialize();

        _gameManager = SnekSingletonManager.GetSingleton<GameManager>();
    }

    protected override void Validate()
    {
        if (!_retryButton)
            FailValidation("Play Button not assigned.");

        if (!_exitButton)
            FailValidation("Cancel Button not assigned.");
    }

    protected override void OnInitializationSuccess()
    {
        _retryButton.SetExternalCallback(OnPlayButtonClick);
        _exitButton.SetExternalCallback(OnExitButtonClick);
    }

    private void OnPlayButtonClick()
    {
        _gameManager.StartGame();

        ClosePopup();
    }

    private void OnExitButtonClick()
    {
        _gameManager.ReturnToMainMenu();

        ClosePopup();
    }
}
