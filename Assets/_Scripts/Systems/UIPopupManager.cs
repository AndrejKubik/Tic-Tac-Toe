using System;
using System.Collections.Generic;
using Snek.SingletonManager;
using Snek.Utilities;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;

[UseSnekInspector]
public class UIPopupManager : SnekMonoSingleton
{
    private GameManager _gameManager;

    [SerializeField] private List<UIPopup> _popupPrefabs = new List<UIPopup>();
    [SerializeField] private Transform _popupSpawner;
    [SerializeField] private GameObject _backgroundDim;

    private readonly List<UIPopup> _popups = new List<UIPopup>();

    protected override void Initialize()
    {
        _gameManager = SnekSingletonManager.GetSingleton<GameManager>();
    }

    protected override void Validate()
    {
        if (!_gameManager)
            FailValidation("Cannot find Game Manager singleton.");

        if (!_popupSpawner)
            FailValidation("Popup Spawner transform not assigned.");

        if (!_backgroundDim)
            FailValidation("Background dim game object not assigned.");

        if (IsAnyPrefabReferenceMissing())
            FailValidation("Found missing references in popup prefabs list.");
    }

    protected override void OnInitializationSuccess()
    {
        foreach (UIPopup prefab in _popupPrefabs)
        {
            UIPopup popup = Instantiate(prefab, _popupSpawner);

            popup.name = prefab.name;
            popup.gameObject.SetActive(false);
            popup.transform.SetParent(transform, true);

            _popups.Add(popup);
        }

        Destroy(_popupSpawner.gameObject);

        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }

    private void OnSceneLoad(Scene arg0, LoadSceneMode arg1)
    {
        _backgroundDim.SetActive(false);
    }

    private bool IsAnyPrefabReferenceMissing()
    {
        foreach (UIPopup prefab in _popupPrefabs)
            if (prefab == null)
                return true;

        return false;
    }

    /// <summary>
    /// Show or hide popup by type
    /// </summary>
    public void ShowPopup<T>(bool newState) where T : UIPopup
    {
        foreach (UIPopup popup in _popups)
            if (popup is T targetPopup)
            {
                targetPopup.gameObject.SetActive(newState);

                _backgroundDim.SetActive(newState);

                return;
            }

        Debug.LogError(
            $"Cannot find requested popup.\n" +
            $"Type: {typeof(T).Name}");
    }

    /// <summary>
    /// Show or hide popup by direct reference
    /// </summary>
    public void ShowPopup(UIPopup popup, bool newState)
    {
        if(popup)
        {
            popup.gameObject.SetActive(newState);

            _backgroundDim.SetActive(newState);
        }
        else
            Debug.LogError(
                $"Close popup failed.\n" +
                $"Cannot find requested popup.");
    }
}
