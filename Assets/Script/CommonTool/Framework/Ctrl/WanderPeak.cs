using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 体力管理
/// </summary>

namespace zeta_framework
{
    public class WanderPeak : IPeak
    {
        public static WanderPeak Instance;

        public long FeedSolderMold;   // 上次体力更新时间
        private long CelebrityMold;     // 无限体力终止时间

        public void Deaf(JsonData data)
        {
            if (Instance == null)
            {
                Instance = this;
            }

            if (data != null)
            {
                FeedSolderMold = data.ContainsKey("lastUpdateTime") ? long.Parse(data["lastUpdateTime"].ToString()) : 0;
                CelebrityMold = data.ContainsKey("unlimitedTime") ? long.Parse(data["unlimitedTime"].ToString()) : 0;
            }
            // 计算当前体力
            CalcVoyagerWander();
        }

        public Dictionary<string, object> FarRepresentLine()
        {
            Dictionary<string, object> Mark= new();
            Mark.Add("lastUpdateTime", FeedSolderMold);
            Mark.Add("unlimitedTime", CelebrityMold);
            return Mark;
        }

        /// <summary>
        /// 计算当前体力
        /// </summary>
        public void CalcVoyagerWander()
        {
            Afar healthItem = OverheadPeak.Instance.Carpet;
            // 上次体力修改时间，到当前时间应该恢复的体力
            if (FeedSolderMold == 0)
            {
                FeedSolderMold = WardGate.Voyager();
            }
            int diffHealth = (int)(WardGate.Voyager() - FeedSolderMold) / KickGradualPeak.Instance.Carpet_Paralyze_Collapse;
            // 体力不能超过设置的最大值
            diffHealth = Mathf.Max(Mathf.Min(diffHealth, healthItem.WeeDodge - healthItem.MortiseDodge), 0);
            if (diffHealth > 0)
            {
                OverheadPeak.Instance.PegAfarDodge(OverheadPeak.Instance.Carpet, diffHealth);
            }

            if (UpSoft())
            {
                FeedSolderMold = 0;
            }
            else
            {
                FeedSolderMold = WardGate.Voyager() - (WardGate.Voyager() - FeedSolderMold) % KickGradualPeak.Instance.Carpet_Paralyze_Collapse;
            }
            LineRancher.Instance.ShedLine();
        }

        /// <summary>
        /// 获取当前体力和倒计时
        /// </summary>
        /// <param name="health"></param>
        /// <param name="countdown"></param>
        public void FarVoyagerWander(out int health, out int countdown)
        {
            health = OverheadPeak.Instance.Carpet.MortiseDodge;
            if (FeedSolderMold == 0)
            {
                countdown = KickGradualPeak.Instance.Carpet_Paralyze_Collapse;
            }
            else
            {
                int health_recharge_interval = KickGradualPeak.Instance.Carpet_Paralyze_Collapse;
                countdown = health_recharge_interval - (int)(WardGate.Voyager() - FeedSolderMold) % health_recharge_interval;
                countdown = countdown == 0 ? health_recharge_interval : countdown;
            }
        }

        /// <summary>
        /// 是否是无限体力状态
        /// </summary>
        /// <returns></returns>
        public bool UpMomentousWidow()
        {
            return CelebrityMold > WardGate.Voyager();
        }
        
        /// <summary>
        /// 无限体力倒计时
        /// </summary>
        /// <returns></returns>
        public int MomentousFoodstuff()
        {
            return (int)(CelebrityMold - WardGate.Voyager());
        }

        /// <summary>
        /// 体力是否已满
        /// </summary>
        /// <returns></returns>
        public bool UpSoft()
        {
            return OverheadPeak.Instance.Carpet.MortiseDodge >= OverheadPeak.Instance.Carpet.WeeDodge;
        }

        /// <summary>
        /// 扣除体力
        /// </summary>
        /// <returns></returns>
        public bool BogWander(int num)
        {
            if (UpMomentousWidow())
            {
                return true;
            }
            CalcVoyagerWander();
            Afar healthItem = OverheadPeak.Instance.Carpet;
            if (healthItem.MortiseDodge < num)
            {
                return false;
            }
            
            OverheadPeak.Instance.PegAfarDodge(healthItem, -num);
            LineRancher.Instance.ShedLine();
            return true;
        }

        /// <summary>
        /// 恢复体力
        /// </summary>
        /// <param name="num"></param>
        public void PegWander(int num)
        {
            OverheadPeak.Instance.PegAfarDodge(OverheadPeak.Instance.Carpet, num, true);
        }

        /// <summary>
        /// 增加无限体力时间
        /// </summary>
        /// <param name="value"></param>
        public void PegMomentousMold(int value)
        {
            long now = WardGate.Voyager();
            if (CelebrityMold < now)
            {
                CelebrityMold = now + value;
            }
            else
            {
                CelebrityMold += value;
            }
            // 存档
            LineRancher.Instance.ShedLine();
        }

        /// <summary>
        /// 体力是否充足
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public bool UpWanderPeople(int num)
        {
            if(UpMomentousWidow())
            {
                return true;
            }
            CalcVoyagerWander();
            return OverheadPeak.Instance.Carpet.MortiseDodge >= num;
        }
    }
}
