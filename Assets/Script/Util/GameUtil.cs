using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUtil
{
    /// <summary>
    /// 获取multi系数
    /// </summary>
    /// <returns></returns>
    private static double GetMulti(RewardType type, double cumulative, MultiGroup[] multiGroup, bool IsRandom = false)
    {
        foreach (MultiGroup item in multiGroup)
        {
            if (item.max > cumulative)
            {
                if (type == RewardType.Cash)
                {
                    if (IsRandom)
                    {
                        float random = Random.Range((float)ToeBoldLeg.instance.DeafLine.cash_random[0], (float)ToeBoldLeg.instance.DeafLine.cash_random[1]);
                        return item.multi * (1 + random);
                    }
                    else
                    {
                        return item.multi;
                    }
                }
                else
                {
                    return item.multi;
                }
            }
        }
        return 1;
    }

    public static double GetGoldMulti(bool IsRandom = false)
    {
        return GetMulti(RewardType.Gold, ShedLineRancher.FarEmbryo(CAdjoin.Or_GenerationCastSlit), ToeBoldLeg.instance.DeafLine.gold_group, IsRandom);
    }

    public static double GetCashMulti(bool IsRandom = false)
    {
        return GetMulti(RewardType.Cash, ShedLineRancher.FarEmbryo(CAdjoin.Or_GenerationGoods), ToeBoldLeg.instance.DeafLine.cash_group, IsRandom);
    }
    public static double GetAmazonMulti()
    {
        return GetMulti(RewardType.Amazon, ShedLineRancher.FarEmbryo(CAdjoin.Or_GenerationEntity), ToeBoldLeg.instance.DeafLine.amazon_group);
    }
}


/// <summary>
/// 奖励类型
/// </summary>
public enum RewardType { Gold, Cash, Tip, Undo, Amazon }
