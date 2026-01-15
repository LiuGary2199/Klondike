using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 由于UI节点上直接使用LayoutGroup组件，会导致无法正确获取子元素的世界坐标
/// 所以自己写一个脚本，实现自动排列
/// </summary>
public class TautPotatoDuctless : MonoBehaviour
{
[UnityEngine.Serialization.FormerlySerializedAs("spacing")]    public float Faction= 0;

    // Start is called before the first frame update
    void Start()
    {
        BalconyPotato();
    }

    public void BalconyPotato()
    {
        float y = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf)
            {
                RectTransform Cash= transform.GetChild(i).GetComponent<RectTransform>();
                Cash.anchorMin = new Vector2(0.5f, 1);
                Cash.anchorMax = new Vector2(0.5f, 1);
                Cash.anchoredPosition = new Vector2(Cash.position.x, y - Cash.sizeDelta.y / 2 - Faction * i);
                y -= Cash.sizeDelta.y;
            }
        }
    }
}
