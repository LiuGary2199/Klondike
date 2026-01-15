using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zeta_framework;

public class ShopItemUI : MonoBehaviour
{
    public Text Title;
    public Image RewardIcon;
    public Text RewardCount;
    public Button BuyButton;
    public Image BuyIcon;
    public Text BuyButtonText;
    public Transform RewardItemContent;
    public GameObject TipTag;
    public GameObject DiscountTag;
    public Text DiscountText;

    public Shop shop;
    private ShopPanel shopPanel;
    private bool singleMode;    // 单个奖励

    private void Start()
    {
        BuyButton.onClick.AddListener(() =>
        {
            List<ItemGroup> rewards = shop.itemGroup;
            ShopCtrl.Instance.Buy(shop, (errorCode) => {
                if (errorCode == ErrorCode.Success)
                {
                    HomePanel.Instance.ShowCollectAnimation(rewards);
                    //shopPanel.RefreshList();
                }
                else
                {
                    ToastManager.GetInstance().ShowToast(ErrorCodeMessage.GetMessage(errorCode));
                }
            });
        });
    }

    public void Init(Shop shop, ShopPanel shopPanel)
    {
        this.shop = shop;
        this.shopPanel = shopPanel;
        singleMode = RewardItemContent == null;
        if (Title != null)
        {
            Title.text = !string.IsNullOrEmpty(shop.title) ? shop.title : "TAURUS SPECIAL PACK";
        }
        // 大奖Icon显示excel中配置的shop_icon
        RewardIcon.sprite = Resources.Load<Sprite>(shop.shop_icon);
        // 大奖数量显示奖品列表中的第一个
        RewardCount.text = singleMode ? "x" + shop.itemGroup[0].item_num.ToString() : shop.itemGroup[0].item_num.ToString();
        if (shop.price == 0)
        {
            // 免费
            BuyIcon.gameObject.SetActive(false);
            BuyButtonText.text = "Free";
        }
        else if (shop.purchase_type == 1)
        {
            // 现金
            BuyIcon.gameObject.SetActive(false);
            BuyButtonText.text = "US$" + NumberUtil.DoubleToStr(shop.price, 2);
        }
        else
        {
            // 其他资源
            BuyIcon.gameObject.SetActive(true);
            BuyIcon.sprite = shop.purchase_type == 2 ? ResourceCtrl.Instance.gold.Icon : ResourceCtrl.Instance.diamond.Icon;
            BuyButtonText.text = NumberUtil.DoubleToStr(shop.price, 0);
        }

        if (shop.itemGroup != null && shop.itemGroup.Count > 0 && !singleMode)
        {
            GameObject rewardItemPrefab = Resources.Load<GameObject>("UIPanel/Shop/ShopRewardItem");
            for(int i = 1; i < shop.itemGroup.Count; i++)
            {
                ItemGroup itemGroup = shop.itemGroup[i];
                GameObject ItemGroupUI = Instantiate(rewardItemPrefab, RewardItemContent);
                ItemGroupUI.GetComponent<ShopRewardItemUI>().Init(itemGroup);
            }
        }
    }

}
