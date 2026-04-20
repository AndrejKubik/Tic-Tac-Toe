using System;
using Snek.PlayModeManager;
using Snek.SingletonManager;
using Snek.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[UseSnekInspector]
public class GameManager : SnekMonoSingleton
{
    private GameRoundManager _roundManager;

    [SerializeField] private SceneAsset _gameScene;
    [SerializeField] private SceneAsset _mainMenuScene;

    protected override void Initialize()
    {
        _roundManager = SnekSingletonManager.GetSingleton<GameRoundManager>();
    }

    protected override void Validate()
    {
        if (!_roundManager)
            FailValidation("Cannot find Game Round Manager singleton.");

        if (!_gameScene)
            FailValidation("Game scene not assigned.");

        if (!_mainMenuScene)
            FailValidation("Main menu scene not assigned.");
    }

    public void StartGame()
    {
        SceneManager.sceneLoaded += OnGameplaySceneLoaded;

        SceneManager.LoadScene(_gameScene.name);
    }

    private void OnGameplaySceneLoaded(Scene scene, LoadSceneMode loadMode)
    {
        SceneManager.sceneLoaded -= OnGameplaySceneLoaded;

        _roundManager.StartRound();
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(_mainMenuScene.name);
    }

    public void ExitGame()
    {
        if (Application.isEditor)
            SnekPlayModeManagerRuntime.RequestPlayModeExit();
        else
            Application.Quit();
    }
}
