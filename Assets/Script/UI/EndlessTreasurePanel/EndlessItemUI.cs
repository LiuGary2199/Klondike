using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zeta_framework;
using static UnityEditor.Progress;

public class EndlessItemUI : MonoBehaviour
{
    public GameObject CollectItemPrefab;
    public Transform RewardsContainer;
    public Button GetButton;
    public GameObject LockIcon;
    public Text PriceText;
    public Image ItemBG;

    private ActivityEndlessTreasureDB itemDB;
    private bool isLocked = false;

    // Start is called before the first frame update
    void Start()
    {
        GetButton.onClick.AddListener(() => { 
            if (isLocked)
            {
                ToastManager.GetInstance().ShowToast("Claim previous offer to unlock");
            }
            else
            {
                if (string.IsNullOrEmpty(itemDB.shop_id))
                {
                    // 免费
                    ClaimReward();
                }
                else
                {
                    // 购买
                    ShopCtrl.Instance.Buy(ShopCtrl.Instance.GetShopById(itemDB.shop_id), (errorCode) => {
                        ClaimReward();
                    });
                }
            }
        });
    }

    public void RenderItem(ActivityEndlessTreasureDB item, bool isLock)
    {
        this.itemDB = item;
        // 背景色
        if (!string.IsNullOrEmpty(item.color) && ColorUtility.TryParseHtmlString(item.color, out Color color))
        {
            ItemBG.color = color;
        }
        else
        {
            ItemBG.color = Color.white;
        }
        // 奖励
        for (int i = RewardsContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(RewardsContainer.GetChild(i).gameObject);
        }
        List<ItemGroup> rewards = ResourceCtrl.Instance.GetItemGroupByIds(item.shop_id, item.itemgroup_id, item.item_id, item.item_num);
        foreach(ItemGroup reward in rewards)
        {
            GameObject rewardUI = Instantiate(CollectItemPrefab, RewardsContainer);
            rewardUI.GetComponent<RewardItemUI>().Render(reward.Item.Icon, reward.item_num);
        }

        // 价格
        if (string.IsNullOrEmpty(item.shop_id) || ShopCtrl.Instance.GetShopById(item.shop_id).price == 0)
        {
            PriceText.text = "Free";
        }
        else
        {
            PriceText.text = "$" + ShopCtrl.Instance.GetShopById(item.shop_id).price;
        }

        isLocked = isLock;
        LockIcon.SetActive(isLock);
    }

    // 解锁
    public void UnlockItem()
    {
        if (isLocked)
        {
            // TODO 解锁动画
        }

        isLocked = false;
        LockIcon.SetActive(false);
    }

    // 发奖
    private void ClaimReward()
    {
        List<ItemGroup> rewards = ResourceCtrl.Instance.GetItemGroupByIds(itemDB.shop_id, itemDB.itemgroup_id, itemDB.item_id, itemDB.item_num);
        ActivityEndlessTreasureCtrl.Instance.ClaimReward();
        HomePanel.Instance.ShowCollectAnimation(rewards);
        EndlessTreasurePanel.Instance.MoveAnimation();
    }
}
