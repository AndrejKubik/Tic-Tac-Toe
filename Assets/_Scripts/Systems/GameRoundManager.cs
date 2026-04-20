using Snek.Utilities;
using UnityEngine;

[UseSnekInspector]
public class GameRoundManager : SnekMonoBehaviour
{
    private PlayerTurn _firstTurn = PlayerTurn.Player1;

    public PlayerTurn CurrentTurn { get; private set; }
    private RoundData _currentRoundData;

    private bool _isRoundInProgress = false;

    private static readonly int[][] _cellWinCombos =
    {
        // Horizontal Combinations
        new[] {0, 1, 2},
        new[] {3, 4, 5},
        new[] {6, 7, 8},

        // Vertical Combinations
        new[] {0, 3, 6},
        new[] {1, 4, 7},
        new[] {2, 5, 8},

        // Diagonal Combinations
        new[] {0, 4, 8},
        new[] {2, 4, 6}
    };

    public PlacementGridButtonState[] _playingBoard = new PlacementGridButtonState[PlacementGrid.TotalCells];

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

        CurrentTurn = _firstTurn;

        _isRoundInProgress = true;
    }

    public void EndTurn(int playedCellIndex, PlacementGridButtonState playedCellState)
    {
        _playingBoard[playedCellIndex] = playedCellState;

        _currentRoundData.TotalMoves++;

        if (IsAnyWinComboAchieved(playedCellState))
            FinishRound(false);
        else if (_currentRoundData.TotalMoves == 9)
            FinishRound(true);
        else
            CurrentTurn = GetNextPlayerTurn(CurrentTurn);
    }

    private bool IsAnyWinComboAchieved(PlacementGridButtonState playedCellState)
    {
        foreach (int[] winCombo in _cellWinCombos)
            if (IsWinComboAchieved(winCombo, playedCellState))
                return true;

        return false;
    }

    private bool IsWinComboAchieved(int[] winCombo, PlacementGridButtonState playedCellState)
    {
        return _playingBoard[winCombo[0]] == playedCellState
            && _playingBoard[winCombo[1]] == playedCellState
            && _playingBoard[winCombo[2]] == playedCellState;
    }

    private PlayerTurn GetNextPlayerTurn(PlayerTurn playerTurn)
    {
        return playerTurn == PlayerTurn.Player1 ? PlayerTurn.Player2 : PlayerTurn.Player1;
    }

    public void FinishRound(bool isDraw)
    {
        _isRoundInProgress = false;

        _firstTurn = GetNextPlayerTurn(_firstTurn);

        if (isDraw)
            Debug.Log("Draw!");
        else
            Debug.Log($"{CurrentTurn} wins!");
    }
}
