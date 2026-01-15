using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DodgeSkeletalSwear : SnowUIPlace
{
    [Header("按钮")]
[UnityEngine.Serialization.FormerlySerializedAs("ADButton")]    public Button ADGibbon;
[UnityEngine.Serialization.FormerlySerializedAs("NextLevelButton")]    public Button OpenLevelGibbon;
[UnityEngine.Serialization.FormerlySerializedAs("ADText")]    public GameObject ADMoss;
    [Header("转盘组")]
[UnityEngine.Serialization.FormerlySerializedAs("SlotBG")]    public DishPerry DishBG;
[UnityEngine.Serialization.FormerlySerializedAs("RewardCashImage")]
    public GameObject StarveNoteFifth;
[UnityEngine.Serialization.FormerlySerializedAs("RewardGoldImage")]    public GameObject StarveCastFifth;
[UnityEngine.Serialization.FormerlySerializedAs("RewardText")]    public Text StarveMoss;

    private double ValleyDodge;
    private bool AgeLifewayItGem;

    // Start is called before the first frame update
    void Start()
    {
        ADGibbon.onClick.AddListener(() => {
            if (ByGunPork())
            {
                LionDish();
            }
            else
            {
                ADRancher.Befriend.LionStarveAlder((success) =>
                {
                    if (success)
                    {
                        LionDish();
                    }
                }, "101");
            }
        });

        OpenLevelGibbon.onClick.AddListener(() =>
        {
            OpenLevelGibbon.enabled = false;
            if (UnfoldGate.UpMound())
            {
                KickLineRancher.FarBefriend().PegCast(ValleyDodge, transform);
            }
            else
            {
                KickLineRancher.FarBefriend().PegNote(ValleyDodge, transform);
            }
            FirstUIMode(GetType().Name);
            if (!AgeLifewayItGem)
            {
                ADRancher.Befriend.IfCoronaPegWaist();
            }
        });

    }

    public override void Display(object uiFormParams)
    {
        base.Display(uiFormParams);

        if (ByGunPork())
        {
            ADMoss.SetActive(false);
            OpenLevelGibbon.gameObject.SetActive(false);
        }
        else
        {
            ADMoss.SetActive(true);
            OpenLevelGibbon.gameObject.SetActive(true);
        }
        OpenLevelGibbon.enabled = true;

        ADGibbon.gameObject.SetActive(true);
        AgeLifewayItGem = false;

        // 根据实际项目计算奖励
        //rewardValue = UnfoldGate.IsApple() ? ToeBoldLeg.instance.InitData.box_gold_price * GameUtil.GetGoldMulti() : ToeBoldLeg.instance.InitData.passlevel_cash_price * GameUtil.GetCashMulti();
        ValleyDodge = 1 * GameUtil.GetCashMulti();
        StarveNoteFifth.SetActive(!UnfoldGate.UpMound());
        StarveCastFifth.SetActive(UnfoldGate.UpMound());
        StarveMoss.text = "+" + FormalGate.EmbryoBeSow(ValleyDodge);

        DishBG.RateMedal();
    }

    private bool ByGunPork()
    {
        return !PlayerPrefs.HasKey(CAdjoin.Or_BefitDish + "Bool") || ShedLineRancher.FarGray(CAdjoin.Or_BefitDish);
    }
    // 计算本次slot应该获得的奖励
    private int HueDishMedalDodge()
    {
        // 新用户，第一次固定翻5倍
        if (ByGunPork())
        {
            int index = 0;
            foreach (SlotItem wg in ToeBoldLeg.instance.DeafLine.slot_group)
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
            foreach (SlotItem wg in ToeBoldLeg.instance.DeafLine.slot_group)
            {
                sumWeight += wg.weight;
            }
            int r = Random.Range(0, sumWeight);
            int nowWeight = 0;
            int index = 0;
            foreach (SlotItem wg in ToeBoldLeg.instance.DeafLine.slot_group)
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


    private void LionDish()
    {
        OpenLevelGibbon.gameObject.SetActive(false);
        ADGibbon.gameObject.SetActive(false);
        int index = HueDishMedalDodge();
        DishBG.None(index, (multi) => {
            // slot结束后的回调
            
            SteamshipRevolution.UranusFormal(ValleyDodge, ValleyDodge * multi, 0, StarveMoss, "+", () =>
            {
                ValleyDodge = ValleyDodge * multi;
                StarveMoss.text = "+" + FormalGate.EmbryoBeSow(ValleyDodge);
                AgeLifewayItGem = true;
                OpenLevelGibbon.gameObject.SetActive(true);
            });
        });

        ShedLineRancher.CudGray(CAdjoin.Or_BefitDish, false);
    }
}
