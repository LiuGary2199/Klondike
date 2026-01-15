using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;

/// <summary> 飞行气泡奖励 </summary>
public class FlyBubbleRewardPanel : BaseUIForms
{
    public GameObject Gold;
    public Text GoldText;
    float GoldReward;
    public GameObject Cash;
    public Text CashText;
    float CashReward;
    public Button GiveupBtn;
    public Button GetBtn;
    public GameObject RewardSpine;
    public List<GameObject> RewardShowList;


    private void Start()
    {
        GetBtn.onClick.AddListener(() =>
        {
            ADManager.Instance.playRewardVideo((ok) =>
            {
                if (ok)
                {
                    if (GoldReward > 0)
                    {
                        GameDataManager.GetInstance().AddGold(GoldReward);
                        GamePanel.Instance.UpdateGold();
                        PostEventScript.GetInstance().SendEvent("1017", "1", GoldReward.ToString());
                    }
                    if (CashReward > 0)
                    {
                        GameDataManager.GetInstance().AddCash(CashReward);
                        GamePanel.Instance.UpdateCash();
                        PostEventScript.GetInstance().SendEvent("1017", "1", CashReward.ToString());
                    }
                    CloseUIForm(nameof(FlyBubbleRewardPanel));
                }
            }, "");
        });
        GiveupBtn.onClick.AddListener(() =>
        {
            CloseUIForm(nameof(FlyBubbleRewardPanel));
        });
    }

    public override void Display(object uiFormParams)
    {
        base.Display(uiFormParams);
        GoldReward = 0;
        CashReward = 0;
        Gold.SetActive(false);
        Cash.SetActive(false);
        RewardData Data = GameDataManager.GetInstance().GetRewardDataByWeightAndRange(NetInfoMgr.instance._GameData.flybubble_data_list);
        if (Data.type == RewardType.Gold)
        {
            GoldReward = Data.num;
            GoldText.text = GoldReward.ToString();
            Gold.SetActive(true);
        }
        else if (Data.type == RewardType.Cash)
        {
            CashReward = Data.num;
            CashText.text = CashReward.ToString();
            Cash.SetActive(true);
        }
    }

    void InitRewardAni()
    {
        // 重置骨骼到初始姿态
        SkeletonGraphic ItemSpine = RewardSpine.GetComponent<SkeletonGraphic>();
        ItemSpine.Skeleton.SetToSetupPose();
        ItemSpine.AnimationState.ClearTracks();
        // 强制立即更新状态（重要！）
        ItemSpine.Update(0);
        ItemSpine.AnimationState.SetAnimation(0, "Reward", false);
        AnimationController.WinPanelShow(RewardShowList);
    }
}
