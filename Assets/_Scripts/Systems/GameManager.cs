using Snek.PlayModeManager;
using Snek.SingletonManager;
using Snek.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[UseSnekInspector]
public class GameManager : SnekMonoSingleton
{
    [SerializeField] private SceneAsset _gameScene;
    [SerializeField] private SceneAsset _mainMenuScene;

    protected override void Validate()
    {
        if (!_gameScene)
            FailValidation("Game scene not assigned.");

        if (!_mainMenuScene)
            FailValidation("Main menu scene not assigned.");
    }

    public void StartGame()
    {
        SceneManager.LoadScene(_gameScene.name);
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
