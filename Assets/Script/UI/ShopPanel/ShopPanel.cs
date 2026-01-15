using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zeta_framework;

public class ShopPanel : BaseUIForms
{
    public GameObject Content;

    private Dictionary<string, GameObject> ShopItems;


    


    public override void Display(object uiFormParams)
    {
        base.Display(uiFormParams);

        if (ShopItems == null)
        {
            RenderList();
        }
        else
        {
            RefreshList();
        }
    }

    public void RenderList()
    {
        // 清除当前数据
        for (int i = Content.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(Content.transform.GetChild(i).gameObject);
        }

        // 加载商店节点
        ShopItems = new();
        List<Shop> shopList = ShopCtrl.Instance.GetShopList(true);
        foreach (Shop shop in shopList)
        {
            if (shop.is_show)
            {
                GameObject shopItemUI;
                if (shop.itemGroup.Count > 1)
                {
                    shopItemUI = Instantiate(Resources.Load<GameObject>("UIPanel/Shop/ShopItem"), Content.transform);
                } else
                {
                    shopItemUI = Instantiate(Resources.Load<GameObject>("UIPanel/Shop/ShopItemSingle"), Content.transform);
                }
                shopItemUI.GetComponent<ShopItemUI>().Init(shop, this);
                shopItemUI.SetActive(shop.num <= 0 || shop.currentNum < shop.num);

                ShopItems.Add(shop.id, shopItemUI);
            }
        }
    }

    public void RefreshList()
    {
        foreach(string shopId in  ShopItems.Keys) {
            Shop shop = ShopCtrl.Instance.GetShopById(shopId);
            ShopItems[shopId].SetActive(shop.num <= 0 || shop.currentNum < shop.num);
        }

    }

}
