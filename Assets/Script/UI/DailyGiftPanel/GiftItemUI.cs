using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zeta_framework;

public class GiftItemUI : MonoBehaviour
{
    public GameObject UnGet;    // 未领取
    public GameObject Claimed;  // 已领取
    public GameObject Current;  // 进行中
    public Image ClaimedIcon;   // 已领取的奖励Icon
    public Text ClaimedNum;     // 已领取的奖励数量
    public Button CollectButton;    // 领奖励按钮
    private int DayIndex;    // 第几天

    // Start is called before the first frame update
    void Start()
    {
        CollectButton.onClick.AddListener(() => {
            int currentIndex = ActivityDailyGiftCtrl.Instance.GetCurrentIndex();
            ActivityDailyGiftCtrl.Instance.Collect();
            DailyGiftPanel.Instance.ClaimRewards(ActivityDailyGiftCtrl.Instance.GetRewardByIndex(currentIndex));
        });
    }

    /// <summary>
    /// 初始化某天奖励
    /// </summary>
    /// <param name="state"></param>
    public void Init(int index, int state)
    {
        DayIndex = index;

        UnGet.SetActive(false);
        Claimed.SetActive(false);
        Current.SetActive(false);

        if (state == 1)
        {
            // 未领取
            UnGet.SetActive(true);
        }
        else if (state == 2)
        {
            // 已领取，显示具体奖励
            List<ItemGroup> rewards = ActivityDailyGiftCtrl.Instance.GetRewardByIndex(DayIndex);
            if (rewards.Count > 0)
            {
                ClaimedIcon.sprite = rewards[0].Item.Icon;
                ClaimedNum.text = "+" + rewards[0].item_num;
            }
            Claimed.SetActive(true);
        }
        else if (state == 3)
        {
            // 待领取
            Current.SetActive(true);
        }
    }
}
