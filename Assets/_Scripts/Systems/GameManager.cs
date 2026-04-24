using Snek.PlayModeManager;
using Snek.SingletonManager;
using Snek.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

[UseSnekInspector]
public class GameManager : SnekMonoSingleton
{
    private GameRoundManager _roundManager;

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

    public void StartGame()
    {
        SceneManager.sceneLoaded += OnGameplaySceneLoaded;

        SceneManager.LoadScene(GameSceneName);
    }

    private void OnGameplaySceneLoaded(Scene scene, LoadSceneMode loadMode)
    {
        SceneManager.sceneLoaded -= OnGameplaySceneLoaded;

        _roundManager.StartRound(true);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(MainMenuSceneName);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        SnekPlayModeManagerRuntime.RequestPlayModeExit();
#elif UNITY_WEBGL
        Application.OpenURL(Application.absoluteURL);
#else
        Application.Quit();
#endif
    }
}
