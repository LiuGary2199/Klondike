using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zeta_framework;

public class SodaSwear : SnowUIPlace
{
[UnityEngine.Serialization.FormerlySerializedAs("Content")]    public GameObject Totally;

    private Dictionary<string, GameObject> SodaRural;


    


    public override void Display(object uiFormParams)
    {
        base.Display(uiFormParams);

        if (SodaRural == null)
        {
            PearlyTrip();
        }
        else
        {
            BalconyTrip();
        }
    }

    public void PearlyTrip()
    {
        // 清除当前数据
        for (int i = Totally.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(Totally.transform.GetChild(i).gameObject);
        }

        // 加载商店节点
        SodaRural = new();
        List<Soda> shopList = SodaPeak.Instance.FarSodaTrip(true);
        foreach (Soda shop in shopList)
        {
            if (shop.By_show)
            {
                GameObject shopItemUI;
                if (shop.SealPerry.Count > 1)
                {
                    shopItemUI = Instantiate(Resources.Load<GameObject>("UIPanel/Soda/ShopItem"), Totally.transform);
                } else
                {
                    shopItemUI = Instantiate(Resources.Load<GameObject>("UIPanel/Soda/ShopItemSingle"), Totally.transform);
                }
                shopItemUI.GetComponent<SodaAfarUI>().Deaf(shop, this);
                shopItemUI.SetActive(shop.num <= 0 || shop.MortiseLad < shop.num);

                SodaRural.Add(shop.id, shopItemUI);
            }
        }
    }

    public void BalconyTrip()
    {
        foreach(string shopId in  SodaRural.Keys) {
            Soda Pont= SodaPeak.Instance.FarSodaSoAt(shopId);
            SodaRural[shopId].SetActive(Pont.num <= 0 || Pont.MortiseLad < Pont.num);
        }

    }

}
