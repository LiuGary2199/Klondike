using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 事件渗透
/// </summary>
public class PopulateInuitOperation : MonoBehaviour, ICanvasRaycastFilter
{
    private Image PotatoFifth;
    public void CudLondonFifth(Image target)
    {
        PotatoFifth = target;
    }
    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        if (PotatoFifth == null)
        {
            return true;
        }
        return !RectTransformUtility.RectangleContainsScreenPoint(PotatoFifth.rectTransform, sp, eventCamera);
    }
}