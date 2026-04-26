using System;
using UnityEngine;

[Serializable]
public struct GameTheme
{
    public Color BackgroundColor;
    public Color GridColor;

    public Sprite SymbolX;
    public Sprite SymbolO;

    public Color SymbolXColor;
    public Color SymbolOColor;

    public readonly bool IsValid()
    {
        return SymbolX != null && SymbolO != null;
    }
}
