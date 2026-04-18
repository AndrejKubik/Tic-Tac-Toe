using Snek.Utilities;
using UnityEngine;

[UseSnekInspector]
public class GameRoundManager : SnekMonoBehaviour
{
    private PlayerTurn _firstTurn = PlayerTurn.Player1;
    private PlayerTurn _currentTurn = PlayerTurn.Player1;

    private RoundData _currentRoundData;

    private bool _isRoundInProgress = false;

    private void Update()
    {
        if (_isRoundInProgress)
            _currentRoundData.ElapsedTime += Time.deltaTime;
    }

    public void StartRound()
    {
        _currentRoundData = new RoundData()
        {
            ElapsedTime = 0f,
            TotalMoves = 0,
        };

        SwitchPlayer(ref _firstTurn);

        _currentTurn = _firstTurn;

        _isRoundInProgress = true;
    }

    public void EndTurn()
    {
        SwitchPlayer(ref _currentTurn);
    }

    private void SwitchPlayer(ref PlayerTurn turn)
    {
        if (turn == PlayerTurn.Player1)
            turn = PlayerTurn.Player2;
        else
            turn = PlayerTurn.Player1;
    }

    public void FinishRound()
    {
        _isRoundInProgress = false;
    }
}
