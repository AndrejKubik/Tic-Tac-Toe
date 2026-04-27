using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class PlacementGrid : GridLayoutGroup
{
    public Rect GetRect()
    {
        return rectTransform.rect;
    }
}
