using System;
using Snek.PlayModeManager;
using Snek.SingletonManager;
using Snek.Utilities;
using UnityEngine.SceneManagement;
using UnityEngine;

[UseSnekInspector]
public class GameManager : SnekMonoSingleton
{
    private GameRoundManager _roundManager;

    public event Action OnGameStarted;
    public event Action OnReturnedToMainMenu;

    public string MainMenuSceneName;
    public string GameSceneName;

    protected override void Initialize()
    {
        _roundManager = SnekSingletonManager.GetSingleton<GameRoundManager>();
    }

    protected override void Validate()
    {
        if (!_roundManager)
            FailValidation("Cannot find Game Round Manager singleton.");

        if (string.IsNullOrEmpty(MainMenuSceneName))
            FailValidation("Game scene not assigned.");

        if (string.IsNullOrEmpty(GameSceneName))
            FailValidation("Main menu scene not assigned.");
    }

    protected override void OnInitializationSuccess()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(GameSceneName);
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode loadMode)
    {
        if (scene.name == MainMenuSceneName)
            OnReturnedToMainMenu?.Invoke();
        else if (scene.name == GameSceneName)
            OnGameStarted?.Invoke();
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(MainMenuSceneName);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        SnekPlayModeManagerRuntime.RequestPlayModeExit();
#else
        Application.Quit();
#endif
    }
}
