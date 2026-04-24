using System;
using Snek.SingletonManager;
using Snek.Utilities;
using UnityEngine;

[UseSnekInspector]
public class GameRoundManager : SnekMonoSingleton
{
    private UIPopupManager _popupManager;
    private StatsManager _statsManager;

    private PlayerInstance _firstTurn = PlayerInstance.Player1;

    private PlayerInstance _currentTurn;
    private RoundData _currentRoundData;

    public bool IsRoundInProgress { get; private set; }

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

    private PlacementGridCellState[] _playingBoardCells;

    public event Action OnNewRoundStarted;
    public event Action<float> OnElapsedTimeUpdated;
    public event Action<PlayerInstance, int> OnPlayerMovesUpdated;
    public event Action<RoundData> OnRoundFinished;

    protected override void Initialize()
    {
        _popupManager = SnekSingletonManager.GetSingleton<UIPopupManager>();
        _statsManager = SnekSingletonManager.GetSingleton<StatsManager>();
    }

    protected override void Validate()
    {
        if (!_popupManager)
            FailValidation("Cannot find UI Popup Manager singleton.");

        if (!_statsManager)
            FailValidation("Cannot find Stats Manager singleton.");
    }

    private void Update()
    {
        if (IsRoundInProgress)
            UpdateElapsedTime();
    }

    private void UpdateElapsedTime()
    {
        _currentRoundData.ElapsedTime += Time.deltaTime;

        OnElapsedTimeUpdated?.Invoke(_currentRoundData.ElapsedTime);
    }

    public void StartRound(bool isNewGame)
    {
        PlacementGridCellState player1Symbol;
        PlacementGridCellState player2Symbol;

        if (isNewGame)
        {
            player1Symbol = PlacementGridCellState.X;
            player2Symbol = PlacementGridCellState.O;
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

        _currentTurn = _firstTurn;
        _playingBoardCells = new PlacementGridCellState[PlayingBoard.TotalCells];

        IsRoundInProgress = true;

        OnNewRoundStarted?.Invoke();
    }

    private PlacementGridCellState SwitchSymbol(PlacementGridCellState currentSymbol)
    {
        if (currentSymbol == PlacementGridCellState.X)
            return PlacementGridCellState.O;
        else if (currentSymbol == PlacementGridCellState.O)
            return PlacementGridCellState.X;

        Debug.LogError("Trying to switch a symbol from NONE state.");

        return PlacementGridCellState.None;
    }

    public void EndTurn(int playedCellIndex, PlacementGridCellState playedCellState)
    {
        _playingBoardCells[playedCellIndex] = playedCellState;

        UpdatePlayerMovesCount();

        if (IsAnyWinComboAchieved(playedCellState))
            FinishRound(false);
        else if (_currentRoundData.TotalMoves == 9)
            FinishRound(true);
        else
            _currentTurn = GetNextPlayerTurn(_currentTurn);
    }

    private void UpdatePlayerMovesCount()
    {
        _currentRoundData.TotalMoves++;

        if (_currentTurn == PlayerInstance.Player1)
        {
            _currentRoundData.Player1Moves++;

            OnPlayerMovesUpdated?.Invoke(_currentTurn, _currentRoundData.Player1Moves);
        }
        else
        {
            _currentRoundData.Player2Moves++;

            OnPlayerMovesUpdated?.Invoke(_currentTurn, _currentRoundData.Player2Moves);
        }
    }

    private bool IsAnyWinComboAchieved(PlacementGridCellState playedCellState)
    {
        foreach (int[] winCombo in _cellWinCombos)
            if (IsWinComboAchieved(winCombo, playedCellState))
                return true;

        return false;
    }

    private bool IsWinComboAchieved(int[] winCombo, PlacementGridCellState playedCellState)
    {
        return _playingBoardCells[winCombo[0]] == playedCellState
            && _playingBoardCells[winCombo[1]] == playedCellState
            && _playingBoardCells[winCombo[2]] == playedCellState;
    }

    private PlayerInstance GetNextPlayerTurn(PlayerInstance playerTurn)
    {
        return playerTurn == PlayerInstance.Player1 ? PlayerInstance.Player2 : PlayerInstance.Player1;
    }

    public void FinishRound(bool isDraw)
    {
        IsRoundInProgress = false;

        _firstTurn = GetNextPlayerTurn(_firstTurn);

        ResolveRound(isDraw);

        _statsManager.StoreRoundData(_currentRoundData);
        _popupManager.ShowPopup<RoundFinishedPopup>(true);
    }

    private void ResolveRound(bool isDraw)
    {
        if (isDraw)
            SetCurrentRoundResult(RoundResult.Draw);
        else if (_currentTurn == PlayerInstance.Player1)
            SetCurrentRoundResult(RoundResult.Player1Win);
        else if (_currentTurn == PlayerInstance.Player2)
            SetCurrentRoundResult(RoundResult.Player2Win);
    }

    private void SetCurrentRoundResult(RoundResult result)
    {
        _currentRoundData.Result = result;
    }

    public RoundResult GetCurrentRoundResult()
    {
        return _currentRoundData.Result;
    }

    public PlacementGridCellState GetCurrentRoundPlayer1Symbol()
    {
        return _currentRoundData.Player1Symbol;
    }

    public PlacementGridCellState GetCurrentRoundPlayer2Symbol()
    {
        return _currentRoundData.Player2Symbol;
    }

    public PlacementGridCellState GetCurrentTurnSymbol()
    {
        return _currentTurn == PlayerInstance.Player1 ?
            GetCurrentRoundPlayer1Symbol() : GetCurrentRoundPlayer2Symbol();
    }
}
