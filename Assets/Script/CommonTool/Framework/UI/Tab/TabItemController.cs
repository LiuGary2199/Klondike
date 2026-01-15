using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Tab按钮样式脚本
/// </summary>

public class TabItemController : MonoBehaviour
{
    public Image Icon;
    public Text Title;

    

    public void SetActiveUI(bool active, TabController controller, TabItem tabItem)
    {
        if (Title != null && controller.ActiveColor != null)
        {
            Title.color = active ? controller.ActiveColor : controller.InactiveColor;
        }
        if (gameObject.GetComponent<Image>() != null && controller.ActiveBG != null)
        {
            gameObject.GetComponent<Image>().sprite = active ? controller.ActiveBG : controller.InactiveBG;
        }
        if (Icon != null && tabItem.ActiveIcon != null)
        {
            Icon.sprite = active ? tabItem.ActiveIcon : tabItem.InactiveIcon;
        }
    }
}
