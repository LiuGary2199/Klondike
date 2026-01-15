using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zeta_framework;

public class ItemBarUI : MonoBehaviour
{
    public Image Icon;
    public Text NumText;
    public string resourceType;
    public bool AutoChange = true;

    private Item item;  // 对应的资源
    private int oldNum; // 当前显示的值

    // Start is called before the first frame update
    void Start()
    {
        InitItem();
        if (AutoChange)
        {
            MessageCenterLogic.GetInstance().Register(CConfig.mg_ItemChange_ + resourceType, (md) =>
            {
                AddAnimation();
            });
        }
    }

    // 初始化资源图标和数量
    private void InitItem()
    {
        item = ResourceCtrl.Instance.GetItemById(resourceType);
        //Icon.sprite = item.Icon;
        NumText.text = item.currentValue.ToString();
        oldNum = item.currentValue;
    }

    // 数字变化动画
    public void AddAnimation()
    {
        int newNum = item.currentValue;
        if (!gameObject.activeInHierarchy || newNum < oldNum)
        {
            NumText.text = newNum.ToString();
            oldNum = newNum;
        }
        else if (newNum > oldNum)
        {
            AnimationController.ChangeNumber(oldNum, newNum, 0.1f, NumText, () =>
            {
                NumText.text = newNum.ToString();
                oldNum = newNum;
            });
        }
    }
}
