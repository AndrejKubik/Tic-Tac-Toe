using System;
using UnityEngine;

[Serializable]
public struct GameTheme
{
    public Color BackgroundColor;

    public Sprite SymbolX;
    public Sprite SymbolO;

    public readonly bool IsValid()
    {
        return SymbolX != null && SymbolO != null;
    }
}
