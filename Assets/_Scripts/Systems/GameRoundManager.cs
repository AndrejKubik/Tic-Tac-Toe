using System;
using Snek.SingletonManager;
using Snek.Utilities;
using UnityEngine;

[UseSnekInspector]
public class GameRoundManager : SnekMonoSingleton
{
    private GameManager _gameManager;
    private StatsManager _statsManager;

    private PlayerInstance _firstTurn = PlayerInstance.Player1;

    private PlayerInstance _currentTurn;
    private RoundData _currentRoundData;
    private int[] _currentWinCombo = null;
    private int _lastPlayedCellIndex = -1;

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

    private PlacementGridCellSymbol[] _playingBoardCells;

    public event Action OnRoundStarted;
    public event Action<float> OnElapsedTimeUpdated;
    public event Action<PlayerInstance, int> OnPlayerMovesUpdated;
    public event Action<int[], int> OnRoundFinished;

    protected override void Initialize()
    {
        _gameManager = SnekSingletonManager.GetSingleton<GameManager>();
        _statsManager = SnekSingletonManager.GetSingleton<StatsManager>();
    }

    protected override void Validate()
    {
        if (!_gameManager)
            FailValidation("Cannot find Game Manager singleton.");

        if (!_statsManager)
            FailValidation("Cannot find Stats Manager singleton.");
    }

    protected override void OnInitializationSuccess()
    {
        _gameManager.OnGameStarted += StartNewGame;
    }

    private void OnDestroy()
    {
        _gameManager.OnGameStarted -= StartNewGame;
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

    private void StartNewGame()
    {
        _currentRoundData = new RoundData()
        {
            Player1Symbol = PlacementGridCellSymbol.X,
            Player2Symbol = PlacementGridCellSymbol.O
        };

        _firstTurn = PlayerInstance.Player1;

        StartRound();
    }

    public void StartNewRound()
    {
        _currentRoundData = new RoundData()
        {
            Player1Symbol = SwitchSymbol(GetCurrentRoundPlayer1Symbol()),
            Player2Symbol = SwitchSymbol(GetCurrentRoundPlayer2Symbol())
        };

        StartRound();
    }

    private void StartRound()
    {
        _currentRoundData.ElapsedTime = 0f;
        _currentRoundData.TotalMoves = 0;

        _currentTurn = _firstTurn;
        _playingBoardCells = new PlacementGridCellSymbol[PlayingBoard.TotalCells];
        _currentWinCombo = null;

        IsRoundInProgress = true;

        OnRoundStarted?.Invoke();
    }

    private PlacementGridCellSymbol SwitchSymbol(PlacementGridCellSymbol currentSymbol)
    {
        if (currentSymbol == PlacementGridCellSymbol.X)
            return PlacementGridCellSymbol.O;
        else if (currentSymbol == PlacementGridCellSymbol.O)
            return PlacementGridCellSymbol.X;

        Debug.LogError("Trying to switch a symbol from NONE state.");

        return PlacementGridCellSymbol.None;
    }

    public void EndTurn(int playedCellIndex, PlacementGridCellSymbol playedCellState)
    {
        _playingBoardCells[playedCellIndex] = playedCellState;
        _lastPlayedCellIndex = playedCellIndex;

        UpdatePlayerMovesCount();

        if (IsAnyWinComboAchieved(playedCellState))
            FinishRound(false);
        else if (_currentRoundData.TotalMoves == PlayingBoard.TotalCells)
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

    private bool IsAnyWinComboAchieved(PlacementGridCellSymbol playedCellState)
    {
        foreach (int[] winCombo in _cellWinCombos)
            if (IsWinComboAchieved(winCombo, playedCellState))
            {
                _currentWinCombo = winCombo;

                return true;
            }

        return false;
    }

    private bool IsWinComboAchieved(int[] winCombo, PlacementGridCellSymbol playedCellState)
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

        if (!isDraw)
            OnRoundFinished?.Invoke(_currentWinCombo, _lastPlayedCellIndex);

        _statsManager.StoreRoundData(_currentRoundData);
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

    public PlacementGridCellSymbol GetCurrentRoundPlayer1Symbol()
    {
        return _currentRoundData.Player1Symbol;
    }

    public PlacementGridCellSymbol GetCurrentRoundPlayer2Symbol()
    {
        return _currentRoundData.Player2Symbol;
    }

    public PlacementGridCellSymbol GetCurrentTurnSymbol()
    {
        return _currentTurn == PlayerInstance.Player1 ?
            GetCurrentRoundPlayer1Symbol() : GetCurrentRoundPlayer2Symbol();
    }
}
