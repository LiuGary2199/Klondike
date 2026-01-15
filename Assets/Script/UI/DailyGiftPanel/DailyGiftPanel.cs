using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zeta_framework;

public class DailyGiftPanel : BaseUIForms
{
    public static DailyGiftPanel Instance;

    public Button CloseButton;
    public List<GiftItemUI> GiftItems;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        CloseButton.onClick.AddListener(() => {
            CloseUIForm(GetType().Name);
            //ActivityAutoOpenManager.Instance.OpenNext();
        });
    }

    public override void Display(object uiFormParams)
    {
        base.Display(uiFormParams);

        InitData();
    }

    private void InitData()
    {
        ActivityState activityState = ActivityDailyGiftCtrl.Instance.State;    // 活动状态
        int collectedIndex = ActivityDailyGiftCtrl.Instance.GetCurrentIndex();    // 当前是第几天
        // 如果今天已经领取，比如Day1，collectedIndex == 1，activityState = Finish
        // 如果今天未领取，比如Day1，当前collectedIndex == 0， activityState = Attending
        for (int i = 0; i < GiftItems.Count; i++)
        {
            GiftItemUI item = GiftItems[i];
            int state;  // 第i天奖励状态：1：未领取；2：已领取；3：待领取
            if (i > collectedIndex)
            {
                state = 1;
            }
            else if (i < collectedIndex)
            {
                state = 2;
            }
            else
            {
                state = activityState == ActivityState.Attending ? 3 : 1;
            }
            item.Init(i, state);
        }
    }

    public void ClaimRewards(List<ItemGroup> rewards)
    {
        // 显示领奖效果
        HomePanel.Instance.ShowCollectAnimation(rewards, () =>
        {
            //ActivityAutoOpenManager.Instance.OpenNext();
        });
        // 关闭当前窗口
        CloseUIForm(GetType().Name);
    }
    
}
