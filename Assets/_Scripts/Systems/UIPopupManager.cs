using System.Collections.Generic;
using Snek.SingletonManager;
using Snek.Utilities;
using UnityEngine;

[UseSnekInspector]
public class UIPopupManager : SnekMonoSingleton
{
    [SerializeField] private List<UIPopup> _popupPrefabs = new List<UIPopup>();
    [SerializeField] private Transform _popupSpawner;

    private readonly List<UIPopup> _popups = new List<UIPopup>();

    protected override void Validate()
    {
        if (!_popupSpawner)
            FailValidation("Popup Spawner transform not assigned.");

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
            popup.gameObject.SetActive(newState);
        else
            Debug.LogError(
                $"Close popup failed.\n" +
                $"Cannot find requested popup.");
    }
}
