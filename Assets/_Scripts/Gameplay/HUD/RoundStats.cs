using Snek.SingletonManager;
using Snek.Utilities;
using TMPro;
using UnityEngine;

[UseSnekInspector]
public class RoundStats : SnekMonoBehaviour
{
    private GameRoundManager _roundManager;

    [SerializeField] private TextMeshProUGUI _roundDuration;
    [SerializeField] private TextMeshProUGUI _player1Moves;
    [SerializeField] private TextMeshProUGUI _player2Moves;

    protected override void Initialize()
    {
        _roundManager = SnekSingletonManager.GetSingleton<GameRoundManager>();
    }

    protected override void Validate()
    {
        if (!_roundManager)
            FailValidation("Cannot find Game Round Manager singleton.");

        if (!_roundDuration)
            FailValidation("Round duration Text Mesh not assigned.");

        if (!_player1Moves)
            FailValidation("Player 1 moves Text Mesh not assigned.");

        if (!_player2Moves)
            FailValidation("Player 2 moves Text Mesh not assigned.");
    }

    protected override void OnInitializationSuccess()
    {
        _roundManager.OnRoundStarted += OnNewRoundStart;
        _roundManager.OnElapsedTimeUpdated += OnRoundElapsedTimeUpdate;
        _roundManager.OnPlayerMovesUpdated += OnPlayerMovesUpdate;
    }

    private void OnDestroy()
    {
        _roundManager.OnRoundStarted -= OnNewRoundStart;
        _roundManager.OnElapsedTimeUpdated -= OnRoundElapsedTimeUpdate;
        _roundManager.OnPlayerMovesUpdated -= OnPlayerMovesUpdate;
    }

    private void OnNewRoundStart()
    {
        _player1Moves.text = 0.ToString();
        _player2Moves.text = 0.ToString();
    }

    private void OnRoundElapsedTimeUpdate(float newElapsedTime)
    {
        _roundDuration.text = newElapsedTime.ToString("F2");
    }

    private void OnPlayerMovesUpdate(PlayerInstance player, int newMovesCount)
    {
        TextMeshProUGUI movesTextMesh = player == PlayerInstance.Player1 ?
            _player1Moves : _player2Moves;

        movesTextMesh.text = newMovesCount.ToString();
    }
}
