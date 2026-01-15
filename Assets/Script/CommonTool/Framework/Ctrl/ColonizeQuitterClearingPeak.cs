using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zeta_framework
{
    /// <summary>
    /// 无尽宝藏
    /// </summary>
    public class ColonizeQuitterClearingPeak : Colonize
    {
        public static ColonizeQuitterClearingPeak Instance;

        public List<ActivityEndlessTreasureDB> AirportStatuette;

        private int MortiseDodge;   // 当前待领取的奖励序号（从0开始）
        private int MortiseLordlyBrook; // 存档中参加的活动期数

        public int VoyagerDodge        {
            get
            {
                return MortiseDodge;
            }
        }

        public ColonizeQuitterClearingPeak() { 
            Instance ??= this;
        }

        /// <summary>
        /// 初始化无尽宝藏的设置
        /// </summary>
        /// <param name="setting"></param>
        public override void CudGradual(JsonData setting)
        {
            if (setting != null)
            {
                AirportStatuette = JsonMapper.ToObject<List<ActivityEndlessTreasureDB>>(setting.ToJson());
            }
            else
            {
                AirportStatuette = new();
            }
        }

        /// <summary>
        /// 初始化存档
        /// </summary>
        /// <param name="_data"></param>
        public override void CudLine(JsonData _data)
        {
            base.CudLine(_data != null && _data.ContainsKey("data") ? _data["data"] : null);
            // 判断是否为新一期活动，是否需要重置待领取的奖励序号
            MortiseLordlyBrook = _data != null && _data.ContainsKey("attendTime") ? int.Parse(_data["attendTime"].ToString()) : 0;
            MortiseDodge = MortiseLordlyBrook == LordlyBrook && _data != null && _data.ContainsKey("currentIndex") ? int.Parse(_data["currentIndex"].ToString()) : 0;
        }

        public override object FarLine()
        {
            Dictionary<string, object> Mark= new()
            {
                { "data", base.FarLine() },
                { "attendTime", MortiseLordlyBrook },
                { "currentIndex", MortiseDodge }
            };
            return Mark;
        }

        // 领取奖励
        public void WrongStarve()
        {
            ActivityEndlessTreasureDB SealDB= AirportStatuette[MortiseLordlyBrook];
            // 在商店中配置的奖励，购买时已发放奖励，所以此处只需要给shop_id为空的配置发放奖励
            if (string.IsNullOrEmpty(SealDB.Pont_To))
            {
                if (!string.IsNullOrEmpty(SealDB.Infantile_To))
                {
                    OverheadPeak.Instance.PegAfarPerry(SealDB.Infantile_To);
                }
                else if (!string.IsNullOrEmpty(SealDB.Seal_To))
                {
                    OverheadPeak.Instance.PegAfarDodge(SealDB.Seal_To, SealDB.Seal_Toy);
                }
            }

            MortiseDodge++;
            LineRancher.Instance.ShedLine();
        }
    }
}

