using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zeta_framework;

public class SodaAfarUI : MonoBehaviour
{
[UnityEngine.Serialization.FormerlySerializedAs("Title")]    public Text Delft;
[UnityEngine.Serialization.FormerlySerializedAs("RewardIcon")]    public Image StarvePart;
[UnityEngine.Serialization.FormerlySerializedAs("RewardCount")]    public Text StarveWaist;
[UnityEngine.Serialization.FormerlySerializedAs("BuyButton")]    public Button HotGibbon;
[UnityEngine.Serialization.FormerlySerializedAs("BuyIcon")]    public Image HotPart;
[UnityEngine.Serialization.FormerlySerializedAs("BuyButtonText")]    public Text HotGibbonMoss;
[UnityEngine.Serialization.FormerlySerializedAs("RewardItemContent")]    public Transform StarveAfarTotally;
[UnityEngine.Serialization.FormerlySerializedAs("TipTag")]    public GameObject MayBut;
[UnityEngine.Serialization.FormerlySerializedAs("DiscountTag")]    public GameObject ReappearBut;
[UnityEngine.Serialization.FormerlySerializedAs("DiscountText")]    public Text ReappearMoss;
[UnityEngine.Serialization.FormerlySerializedAs("shop")]
    public Soda Pont;
    private SodaSwear PontSwear;
    private bool DollarBelt;    // 单个奖励

    private void Start()
    {
        HotGibbon.onClick.AddListener(() =>
        {
            List<AfarPerry> Dynamic= Pont.SealPerry;
            SodaPeak.Instance.Hot(Pont, (errorCode) => {
                if (errorCode == SenseRead.Success)
                {
                    WoolSwear.Instance.BindDynamicSteamship(Dynamic);
                    //shopPanel.RefreshList();
                }
                else
                {
                    GroomRancher.FarBefriend().BindGroom(ErrorCodeMessage.FarAfrican(errorCode));
                }
            });
        });
    }

    public void Deaf(Soda shop, SodaSwear shopPanel)
    {
        this.Pont = shop;
        this.PontSwear = shopPanel;
        DollarBelt = StarveAfarTotally == null;
        if (Delft != null)
        {
            Delft.text = !string.IsNullOrEmpty(shop.Price) ? shop.Price : "TAURUS SPECIAL PACK";
        }
        // 大奖Icon显示excel中配置的shop_icon
        StarvePart.sprite = Resources.Load<Sprite>(shop.Pont_Gram);
        // 大奖数量显示奖品列表中的第一个
        StarveWaist.text = DollarBelt ? "x" + shop.SealPerry[0].Seal_Toy.ToString() : shop.SealPerry[0].Seal_Toy.ToString();
        if (shop.Guest == 0)
        {
            // 免费
            HotPart.gameObject.SetActive(false);
            HotGibbonMoss.text = "Free";
        }
        else if (shop.Nobility_Mode == 1)
        {
            // 现金
            HotPart.gameObject.SetActive(false);
            HotGibbonMoss.text = "US$" + FormalGate.EmbryoBeSow(shop.Guest, 2);
        }
        else
        {
            // 其他资源
            HotPart.gameObject.SetActive(true);
            HotPart.sprite = shop.Nobility_Mode == 2 ? OverheadPeak.Instance.Pool.Part : OverheadPeak.Instance.Shatter.Part;
            HotGibbonMoss.text = FormalGate.EmbryoBeSow(shop.Guest, 0);
        }

        if (shop.SealPerry != null && shop.SealPerry.Count > 0 && !DollarBelt)
        {
            GameObject rewardItemPrefab = Resources.Load<GameObject>("UIPanel/Soda/ShopRewardItem");
            for(int i = 1; i < shop.SealPerry.Count; i++)
            {
                AfarPerry SealPerry= shop.SealPerry[i];
                GameObject ItemGroupUI = Instantiate(rewardItemPrefab, StarveAfarTotally);
                ItemGroupUI.GetComponent<SodaStarveAfarUI>().Deaf(SealPerry);
            }
        }
    }

}
