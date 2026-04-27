using System;
using Snek.SingletonManager;
using Snek.Utilities;
using UnityEngine;

[UseSnekInspector]
public class ScreenLayoutManager : SnekMonoSingleton
{
    private int _currentWidth;
    private int _currentHeight;

    public event Action<ScreenOrientation> OnScreenChanged;

    protected override void Initialize()
    {
        CacheCurrentResolution();
    }

    private void Update()
    {
        if(IsScreenChanged())
        {
            CacheCurrentResolution();

            OnScreenChanged?.Invoke(GetCurrentOrientation());
        }
    }

    private bool IsScreenChanged()
    {
        return Screen.width != _currentWidth
            || Screen.height != _currentHeight;
    }

    private void CacheCurrentResolution()
    {
        _currentWidth = Screen.width;
        _currentHeight = Screen.height;
    }

    public ScreenOrientation GetCurrentOrientation()
    {
        return Screen.width > Screen.height ?
            ScreenOrientation.Landscape : ScreenOrientation.Portrait;
    }
}

