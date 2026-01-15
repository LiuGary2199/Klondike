using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 由于UI节点上直接使用LayoutGroup组件，会导致无法正确获取子元素的世界坐标
/// 所以自己写一个脚本，实现自动排列
/// </summary>
public class AutoLayoutVertical : MonoBehaviour
{
    public float spacing = 0;

    // Start is called before the first frame update
    void Start()
    {
        RefreshLayout();
    }

    public void RefreshLayout()
    {
        float y = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf)
            {
                RectTransform rect = transform.GetChild(i).GetComponent<RectTransform>();
                rect.anchorMin = new Vector2(0.5f, 1);
                rect.anchorMax = new Vector2(0.5f, 1);
                rect.anchoredPosition = new Vector2(rect.position.x, y - rect.sizeDelta.y / 2 - spacing * i);
                y -= rect.sizeDelta.y;
            }
        }
    }
}
