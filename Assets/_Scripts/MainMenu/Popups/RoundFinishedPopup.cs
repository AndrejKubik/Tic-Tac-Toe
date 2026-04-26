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
        RoundResult roundResult = _roundManager.GetCurrentRoundResult();

        _message.text = roundResult switch
        {
            RoundResult.Draw => "It's a draw!",
            RoundResult.Player1Win => "Player 1 wins!",
            RoundResult.Player2Win => "Player 2 wins!",
            _ => string.Empty
        };
    }

    protected override void OnInitializationSuccess()
    {
        _retryButton.SetExternalCallback(OnRetryButtonClick);
        _exitButton.SetExternalCallback(OnExitButtonClick);
    }

    private void OnRetryButtonClick()
    {
        _roundManager.StartNextRound();

        ClosePopup();
    }

    private void OnExitButtonClick()
    {
        _gameManager.ReturnToMainMenu();

        ClosePopup();
    }
}
