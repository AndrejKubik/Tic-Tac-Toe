using System;
using Snek.SingletonManager;
using Snek.Utilities;
using UnityEngine;

[UseSnekInspector]
public class GameRoundManager : SnekMonoSingleton
{
    private UIPopupManager _popupManager;

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

    private PlacementGridButtonState[] _playingBoard;

    public event Action OnNewRoundStarted;

    protected override void Initialize()
    {
        _popupManager = SnekSingletonManager.GetSingleton<UIPopupManager>();
    }

    protected override void Validate()
    {
        if (!_popupManager)
            FailValidation("Cannot find UI Popup Manager singleton.");
    }

    private void Update()
    {
        if (_isRoundInProgress)
            _currentRoundData.ElapsedTime += Time.deltaTime;
    }

    public void StartRound(bool isNewGame)
    {
        PlacementGridButtonState player1Symbol;
        PlacementGridButtonState player2Symbol;

        if (isNewGame)
        {
            player1Symbol = PlacementGridButtonState.X;
            player2Symbol = PlacementGridButtonState.O;
        }
        else
        {
            player1Symbol = SwitchSymbol(GetCurrentRoundPlayer1Symbol());
            player2Symbol = SwitchSymbol(GetCurrentRoundPlayer2Symbol());
        }

        _currentRoundData = new RoundData()
        {
            ElapsedTime = 0f,
            TotalMoves = 0,
            Player1Symbol = player1Symbol,
            Player2Symbol = player2Symbol
        };

        CurrentTurn = _firstTurn;
        _playingBoard = new PlacementGridButtonState[PlacementGrid.TotalCells];

        _isRoundInProgress = true;

        OnNewRoundStarted?.Invoke();
    }

    private PlacementGridButtonState SwitchSymbol(PlacementGridButtonState currentSymbol)
    {
        if (currentSymbol == PlacementGridButtonState.X)
            return PlacementGridButtonState.O;
        else if (currentSymbol == PlacementGridButtonState.O)
            return PlacementGridButtonState.X;

        Debug.LogError("Trying to switch a symbol from NONE state.");

        return PlacementGridButtonState.None;
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

        ResolveRound(isDraw);

        _popupManager.ShowPopup<RoundFinishedPopup>(true);
    }

    private void ResolveRound(bool isDraw)
    {
        if (isDraw)
            SetCurrentRoundResult(RoundResult.Draw);
        else if (CurrentTurn == PlayerTurn.Player1)
            SetCurrentRoundResult(RoundResult.Player1Win);
        else if (CurrentTurn == PlayerTurn.Player2)
            SetCurrentRoundResult(RoundResult.Player2Win);
    }

    private void SetCurrentRoundResult(RoundResult result)
    {
        _currentRoundData.Result = result;
    }

    public RoundData GetCurrentRoundData()
    {
        return _currentRoundData;
    }

    public PlacementGridButtonState GetCurrentRoundPlayer1Symbol()
    {
        return _currentRoundData.Player1Symbol;
    }

    public PlacementGridButtonState GetCurrentRoundPlayer2Symbol()
    {
        return _currentRoundData.Player2Symbol;
    }

    public PlacementGridButtonState GetCurrentTurnSymbol()
    {
        return CurrentTurn == PlayerTurn.Player1 ?
            GetCurrentRoundPlayer1Symbol() : GetCurrentRoundPlayer2Symbol();
    }
}
