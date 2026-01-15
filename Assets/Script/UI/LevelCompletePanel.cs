using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompletePanel : BaseUIForms
{
    [Header("按钮")]
    public Button ADButton;
    public Button NextLevelButton;
    public GameObject ADText;
    [Header("转盘组")]
    public SlotGroup SlotBG;

    public GameObject RewardCashImage;
    public GameObject RewardGoldImage;
    public Text RewardText;

    private double rewardValue;
    private bool hasClickedAdBtn;

    // Start is called before the first frame update
    void Start()
    {
        ADButton.onClick.AddListener(() => {
            if (isNewUser())
            {
                playSlot();
            }
            else
            {
                ADManager.Instance.playRewardVideo((success) =>
                {
                    if (success)
                    {
                        playSlot();
                    }
                }, "101");
            }
        });

        NextLevelButton.onClick.AddListener(() =>
        {
            NextLevelButton.enabled = false;
            if (CommonUtil.IsApple())
            {
                GameDataManager.GetInstance().AddGold(rewardValue, transform);
            }
            else
            {
                GameDataManager.GetInstance().AddCash(rewardValue, transform);
            }
            CloseUIForm(GetType().Name);
            if (!hasClickedAdBtn)
            {
                ADManager.Instance.NoThanksAddCount();
            }
        });

    }

    public override void Display(object uiFormParams)
    {
        base.Display(uiFormParams);

        if (isNewUser())
        {
            ADText.SetActive(false);
            NextLevelButton.gameObject.SetActive(false);
        }
        else
        {
            ADText.SetActive(true);
            NextLevelButton.gameObject.SetActive(true);
        }
        NextLevelButton.enabled = true;

        ADButton.gameObject.SetActive(true);
        hasClickedAdBtn = false;

        // 根据实际项目计算奖励
        //rewardValue = CommonUtil.IsApple() ? NetInfoMgr.instance.InitData.box_gold_price * GameUtil.GetGoldMulti() : NetInfoMgr.instance.InitData.passlevel_cash_price * GameUtil.GetCashMulti();
        rewardValue = 1 * GameUtil.GetCashMulti();
        RewardCashImage.SetActive(!CommonUtil.IsApple());
        RewardGoldImage.SetActive(CommonUtil.IsApple());
        RewardText.text = "+" + NumberUtil.DoubleToStr(rewardValue);

        SlotBG.initMulti();
    }

    private bool isNewUser()
    {
        return !PlayerPrefs.HasKey(CConfig.sv_FirstSlot + "Bool") || SaveDataManager.GetBool(CConfig.sv_FirstSlot);
    }
    // 计算本次slot应该获得的奖励
    private int getSlotMultiIndex()
    {
        // 新用户，第一次固定翻5倍
        if (isNewUser())
        {
            int index = 0;
            foreach (SlotItem wg in NetInfoMgr.instance.InitData.slot_group)
            {
                if (wg.multi == 5)
                {
                    return index;
                }
                index++;
            }
        }
        else
        {
            int sumWeight = 0;
            foreach (SlotItem wg in NetInfoMgr.instance.InitData.slot_group)
            {
                sumWeight += wg.weight;
            }
            int r = Random.Range(0, sumWeight);
            int nowWeight = 0;
            int index = 0;
            foreach (SlotItem wg in NetInfoMgr.instance.InitData.slot_group)
            {
                nowWeight += wg.weight;
                if (nowWeight > r)
                {
                    return index;
                }
                index++;
            }

        }
        return 0;
    }


    private void playSlot()
    {
        NextLevelButton.gameObject.SetActive(false);
        ADButton.gameObject.SetActive(false);
        int index = getSlotMultiIndex();
        SlotBG.slot(index, (multi) => {
            // slot结束后的回调
            
            AnimationController.ChangeNumber(rewardValue, rewardValue * multi, 0, RewardText, "+", () =>
            {
                rewardValue = rewardValue * multi;
                RewardText.text = "+" + NumberUtil.DoubleToStr(rewardValue);
                hasClickedAdBtn = true;
                NextLevelButton.gameObject.SetActive(true);
            });
        });

        SaveDataManager.SetBool(CConfig.sv_FirstSlot, false);
    }
}
