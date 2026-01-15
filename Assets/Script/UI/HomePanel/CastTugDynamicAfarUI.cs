using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zeta_framework;

/// <summary>
/// 金箔活动动画脚本
/// </summary>

public class CastTugDynamicAfarUI : DynamicAfarUI
{
[UnityEngine.Serialization.FormerlySerializedAs("ProgressBar")]    public ManeuverUI ManeuverMan;
[UnityEngine.Serialization.FormerlySerializedAs("RewardIcon")]    public RectTransform StarvePart;

    private int Bachelor;
    private int Weave;

    private SetTug Leg;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        Leg = SetTugPeak.Instance.FarTugLineSoAt("GoldBox");

        Bachelor = Leg.MortiseManeuver;
        Weave = Leg.MortiseMy;
        DeafManeuver();
    }

    public override void DynamicSteamshipCb()
    {
        ExpBoxDB LegDodgeLine= Leg.FarTugDodgeLineSoMy(Weave);
        if (LegDodgeLine == null)
        {
            gameObject.SetActive(false);
            DynamicRancher.Instance.Open();
            return;
        }

        // 进度条动画
        if (Weave == Leg.MortiseMy)
        {
            // 当前进度没有完成，不需要发放奖励，仅展示进度条滚动动画
            ManeuverMan.BalconyManeuver(Leg.MortiseManeuver, LegDodgeLine.Use_Noble, true, () => {
                Bachelor = Leg.MortiseManeuver;
                DynamicRancher.Instance.Open();
            });
        }
        else
        {
            // 等级不同，说明当前级别进度已经完成
            // 进度条滚动到最后，然后弹出奖励
            // 当前等级奖励发放后，需要再次执行该回调函数继续执行下一级进度条动画
            ManeuverMan.BalconyManeuver(LegDodgeLine.Use_Noble, LegDodgeLine.Use_Noble, true, () => {
                // 优于奖励动画插入接口将动画插入list最前面，所以先插入下一次进度条动画，再插入奖励动画
                // 下一次进度条滚动插入到奖励动画队列中
                CollectItemData data1 = new(Seal_To, 0, BadgePoint, RidPerry, DynamicSteamshipCb);
                List<CollectItemData> list1 = new()
                {
                    data1
                };
                DynamicRancher.Instance.AttainInstinct(list1);

                // 将奖励插入到奖励动画队列中
                List<CollectItemData> list2 = new();
                if (!string.IsNullOrEmpty(LegDodgeLine.Seal_To))
                {
                    CollectItemData data2 = new(LegDodgeLine.Seal_To, LegDodgeLine.Seal_Noble, StarvePart, null, null);
                    list2.Add(data2);
                }
                if (!string.IsNullOrEmpty(LegDodgeLine.Infantile_To))
                {
                    foreach(AfarPerry itemgroup in OverheadPeak.Instance.FarAfarPerrySoAt(LegDodgeLine.Infantile_To))
                    {
                        CollectItemData Mark= new(itemgroup.Seal_To, itemgroup.Seal_Toy, StarvePart, null, null);
                        list2.Add(Mark);
                    }
                }
                DynamicRancher.Instance.AttainInstinct(list2);

                Weave++;
                Bachelor = 0;
                DeafManeuver();

                DynamicRancher.Instance.Open();
            });
        }
    }

    /// <summary>
    /// 初始化进度条
    /// </summary>
    private void DeafManeuver()
    {
        ExpBoxDB LegDodgeLine= Leg.FarTugDodgeLineSoMy(Weave);
        if (LegDodgeLine != null)
        {
            ManeuverMan.BalconyManeuver(Bachelor, LegDodgeLine.Use_Noble, false);
            if (!string.IsNullOrEmpty(LegDodgeLine.Infantile_To))
            {
                StarvePart.GetComponent<Image>().sprite = Resources.Load<Sprite>("Art/Tex/UI/Afar/itemgroup");
            }
            else if (!string.IsNullOrEmpty(LegDodgeLine.Seal_To))
            {
                StarvePart.GetComponent<Image>().sprite = OverheadPeak.Instance.FarAfarSoAt(LegDodgeLine.Seal_To).Part;
            }
        }
    }
}