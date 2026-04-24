using Snek.GameUIPlus;
using Snek.SingletonManager;
using Snek.Utilities;
using UnityEngine;

[UseSnekInspector]
public class GameThemeSelectionPopup : UIPopup
{
    private GameManager _gameManager;

    [SerializeField] private SnekUIButtonWithSFX _playButton;
    [SerializeField] private SnekUIButtonWithSFX _cancelButton;

    protected override void Initialize()
    {
        base.Initialize();

        _gameManager = SnekSingletonManager.GetSingleton<GameManager>();
    }

    protected override void Validate()
    {
        if (!_playButton)
            FailValidation("Play Button not assigned.");

        if (!_cancelButton)
            FailValidation("Cancel Button not assigned.");
    }

    protected override void OnInitializationSuccess()
    {
        _playButton.SetExternalCallback(OnPlayButtonClick);
        _cancelButton.SetExternalCallback(ClosePopup);
    }

    private void OnPlayButtonClick()
    {
        _gameManager.StartGame();
        
        ClosePopup();
    }
}
