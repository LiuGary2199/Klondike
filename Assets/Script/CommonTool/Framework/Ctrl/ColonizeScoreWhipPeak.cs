using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 每日签到
/// </summary>
namespace zeta_framework
{
    public class ColonizeScoreWhipPeak: Colonize
    {
        public static ColonizeScoreWhipPeak Instance;

        private List<ActivityDailyGiftDB> dailyStorm;

        public ColonizeScoreWhipPeak()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        public override void CudGradual(JsonData setting)
        {
            if (setting != null)
            {
                dailyStorm = JsonMapper.ToObject<List<ActivityDailyGiftDB>>(setting.ToJson());
            }
            else
            {
                dailyStorm = new();
            }
        }


        /// <summary>
        /// 获取当前应该是第几天签到（从0开始）
        /// </summary>
        /// <returns></returns>
        public int FarVoyagerDodge()
        {
            return LordlyBrook % dailyStorm.Count;
        }

        /// <summary>
        /// 获取每日签到所有配置
        /// </summary>
        /// <returns></returns>
        public List<ActivityDailyGiftDB> FarRotGradual()
        {
            return dailyStorm;
        }

        /// <summary>
        /// 获取第n天的奖励
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public List<AfarPerry> FarStarveSoDodge(int index)
        {
            List<AfarPerry> Dynamic= new();
            ActivityDailyGiftDB dailyGift = dailyStorm[index];
            if (!string.IsNullOrEmpty(dailyGift.Infantile_To))
            {
                Dynamic.AddRange(OverheadPeak.Instance.FarAfarPerrySoAt(dailyGift.Infantile_To));
            }
            if (!string.IsNullOrEmpty(dailyGift.Seal_To) && dailyGift.Seal_Toy > 0)
            {
                AfarPerry SealPerry= new(dailyGift.Seal_To, dailyGift.Seal_Toy);
                Dynamic.Add(SealPerry);
            }

            return Dynamic;
        }

        /// <summary>
        /// 领取签到奖励
        /// </summary>
        public void Dynamic()
        {
            int index = FarVoyagerDodge();
            List<AfarPerry> Dynamic= FarStarveSoDodge(index);
            OverheadPeak.Instance.PegAfarPerry(Dynamic);
            // 活动设置finish状态
            Fertilizer();
        }
    }
}

