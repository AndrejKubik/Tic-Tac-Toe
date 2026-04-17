using System.Collections.Generic;
using Snek.Utilities;
using UnityEngine;

[UseSnekInspector]
public class UIPopupManager : SnekMonoBehaviour
{
    [SerializeField] private List<UIPopup> _popupPrefabs = new List<UIPopup>();

    private readonly List<UIPopup> _popups = new List<UIPopup>();

    protected override void Initialize()
    {
        foreach (UIPopup popup in _popupPrefabs)
        {
            UIPopup popupInstance = Instantiate(popup, transform);

            _popups.Add(popupInstance);
        }
    }

    protected override void Validate()
    {
        if (_popups.Count != _popupPrefabs.Count)
            FailValidation("Some UI Popup instances were not initialized successfully.");
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
