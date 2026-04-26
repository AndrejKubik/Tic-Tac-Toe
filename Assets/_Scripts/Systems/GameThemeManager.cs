using Snek.SingletonManager;
using Snek.Utilities;
using UnityEngine;

[UseSnekInspector]
public class GameThemeManager : SnekMonoSingleton
{
    [SerializeField] private GameTheme _selectedTheme;

    public void SelectTheme(GameTheme theme)
    {
        _selectedTheme = theme;
    }

    public Color GetBackgroundColor()
    {
        return _selectedTheme.BackgroundColor;
    }

    public Sprite GetSymbolX()
    {
        return _selectedTheme.SymbolX;
    }

    public Sprite GetSymbolO()
    {
        return _selectedTheme.SymbolO;
    }

    public Color GetGridColor()
    {
        return _selectedTheme.GridColor;
    }

    public Color GetSymbolXColor()
    {
        return _selectedTheme.SymbolXColor;
    }

    public Color GetSymbolOColor()
    {
        return _selectedTheme.SymbolOColor;
    }
}
