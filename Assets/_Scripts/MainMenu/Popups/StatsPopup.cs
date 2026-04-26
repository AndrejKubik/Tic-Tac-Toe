using Snek.GameUIPlus;
using Snek.SingletonManager;
using Snek.Utilities;
using TMPro;
using UnityEngine;

[UseSnekInspector]
public class StatsPopup : UIPopupWithSFX
{
    private StatsManager _statsManager;

    [Space(10f)]
    [SerializeField] private SnekUIButtonWithSFX _closeButton;

    [Space(10f)]
    [SerializeField] private TextMeshProUGUI _gamesPlayedCount;
    [SerializeField] private TextMeshProUGUI _player1WinsCount;
    [SerializeField] private TextMeshProUGUI _player2WinsCount;
    [SerializeField] private TextMeshProUGUI _drawCount;
    [SerializeField] private TextMeshProUGUI _averateGameTime;

    protected override void Initialize()
    {
        base.Initialize();

        _statsManager = SnekSingletonManager.GetSingleton<StatsManager>();
    }

    protected override void Validate()
    {
        base.Validate();

        if (!_closeButton)
            FailValidation("Cancel Button not assigned.");

        if (!_statsManager)
            FailValidation("Cannot find Stats Manager singleton.");
    }

    protected override void OnInitializationSuccess()
    {
        _closeButton.SetExternalCallback(ClosePopup);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        _gamesPlayedCount.text = _statsManager.GetGamesPlayedCount().ToString();
        _player1WinsCount.text = _statsManager.GetGamesWithResult(RoundResult.Player1Win).ToString();
        _player2WinsCount.text = _statsManager.GetGamesWithResult(RoundResult.Player2Win).ToString();
        _drawCount.text = _statsManager.GetGamesWithResult(RoundResult.Draw).ToString();
        _averateGameTime.text = _statsManager.GetAverageRoundDuration().ToString("F2");
    }
}
