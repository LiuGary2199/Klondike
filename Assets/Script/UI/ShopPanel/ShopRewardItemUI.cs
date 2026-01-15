using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zeta_framework;

public class ShopRewardItemUI : MonoBehaviour
{
    public Image ItemIcon;
    public Text ItemNum;

    public void Init(ItemGroup itemGroup)
    {
        ItemIcon.sprite = itemGroup.Item.Icon;
        ItemNum.text = "X" + itemGroup.item_num.ToString();
    }
}
