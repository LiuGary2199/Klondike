using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Tab按钮样式脚本
/// </summary>

public class ZooAfarRevolution : MonoBehaviour
{
[UnityEngine.Serialization.FormerlySerializedAs("Icon")]    public Image Part;
[UnityEngine.Serialization.FormerlySerializedAs("Title")]    public Text Delft;

    

    public void CudRefuteUI(bool active, ZooRevolution controller, TabItem tabItem)
    {
        if (Delft != null && controller.RefuteEager != null)
        {
            Delft.color = active ? controller.RefuteEager : controller.InventorEager;
        }
        if (gameObject.GetComponent<Image>() != null && controller.RefuteBG != null)
        {
            gameObject.GetComponent<Image>().sprite = active ? controller.RefuteBG : controller.InventorBG;
        }
        if (Part != null && tabItem.RefutePart != null)
        {
            Part.sprite = active ? tabItem.RefutePart : tabItem.InventorPart;
        }
    }
}
