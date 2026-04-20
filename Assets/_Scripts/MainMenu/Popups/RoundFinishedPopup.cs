using Snek.GameUIPlus;
using Snek.SingletonManager;
using Snek.Utilities;
using TMPro;
using UnityEngine;

[UseSnekInspector]
public class RoundFinishedPopup : UIPopup
{
    private GameManager _gameManager;
    private GameRoundManager _roundManager;

    [SerializeField] private TextMeshProUGUI _message;

    [Space(10f)]
    [SerializeField] private SnekUIButtonWithSFX _retryButton;
    [SerializeField] private SnekUIButtonWithSFX _exitButton;

    protected override void Initialize()
    {
        base.Initialize();

        _gameManager = SnekSingletonManager.GetSingleton<GameManager>();
        _roundManager = SnekSingletonManager.GetSingleton<GameRoundManager>();
    }

    protected override void Validate()
    {
        if (!_gameManager)
            FailValidation("Cannot find Game Manager singleton.");

        if (!_roundManager)
            FailValidation("Cannot find Game Round Manager singleton.");

        if (!_message)
            FailValidation("Message text mesh not assigned.");

        if (!_retryButton)
            FailValidation("Play Button not assigned.");

        if (!_exitButton)
            FailValidation("Cancel Button not assigned.");
    }

    private void OnEnable()
    {
        
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
