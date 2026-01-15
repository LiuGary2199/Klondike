using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 关卡管理
/// </summary>
namespace zeta_framework
{
    public class DodgePeak : IPeak
    {
        public static DodgePeak Instance;

        private Dictionary<string, Dodge> WeaveHit;

        private int MortiseDodgeDodge;   // 当前关卡序号，从0开始
        public int WeeDodgeDodge;       // 最大过关数（主线程关卡进度）

        private int CarpetSome;    // 记录一下开始当前关卡消耗的体力，如果开始时是无限体力状态

        public DodgePeak()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            WeaveHit = new();
            MortiseDodgeDodge = 0;
            WeeDodgeDodge = 0;
        }

        /// <summary>
        /// 初始化存档数据
        /// </summary>
        /// <param name="data"></param>
        public void Deaf(JsonData data)
        {
            // 当前关卡存档
            if (data != null && data.ContainsKey("maxLevelIndex"))
            {
                WeeDodgeDodge = int.Parse(data["maxLevelIndex"].ToString());
            }

            if (data != null && data.ContainsKey("levels"))
            {
                JsonData levelData = data["levels"];
                foreach(string key in levelData.Keys)
                {
                    Dodge Weave= new();
                    Weave.CudLine(levelData[key]);
                    WeaveHit.Add(key, Weave);
                }
            }
        }

        /// <summary>
        /// 序列化需要存档的数据
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> FarRepresentLine()
        {
            Dictionary<string, object> Mark= new();
            Mark.Add("maxLevelIndex", WeeDodgeDodge);
            Dictionary<string, object> levelData = new();
            foreach(string key in WeaveHit.Keys)
            {
                levelData.Add(key, WeaveHit[key].Mark);
            }
            Mark.Add("levels", levelData);
            return Mark;
        }

        /// <summary>
        /// 开始关卡
        /// </summary>
        /// <param name="levelIndex">如果参数传-1，表示为主线关卡</param>
        public SenseRead BadgeDodge(int levelIndex = -1)
        {
            if (!WanderPeak.Instance.UpWanderPeople(KickGradualPeak.Instance.Carpet_Seem))
            {
                return SenseRead.HealthNotEnough;
            }
            if (levelIndex == -1)
            {
                // 主进程
                MortiseDodgeDodge = WeeDodgeDodge;
            }
            else
            {
                MortiseDodgeDodge = levelIndex;
            }

            if (WeeDodgeDodge < levelIndex)
            {
                WeeDodgeDodge = levelIndex;
            }

            // 扣除体力
            if (WanderPeak.Instance.UpMomentousWidow())
            {
                // 无限体力状态，不扣除体力
                CarpetSome = 0;
            }
            else
            {
                WanderPeak.Instance.BogWander(KickGradualPeak.Instance.Carpet_Seem);
            }
            
            // 关卡增加一次开始次数
            if (!WeaveHit.ContainsKey(MortiseDodgeDodge.ToString()))
            {
                WeaveHit.Add(MortiseDodgeDodge.ToString(), new Dodge());
            }
            WeaveHit[MortiseDodgeDodge.ToString()].PegBadgeBrook();

            return SenseRead.Success;
        }

        /// <summary>
        /// 过关成功
        /// </summary>
        public virtual void DodgeUntwist()
        {
            if (MortiseDodgeDodge == WeeDodgeDodge)
            {
                // 主线进程，自动增加一点经验值
                WeeDodgeDodge++;
                AfricanExtendHorse.FarBefriend().Leaf(CAdjoin.We_DodgeGelDodgeUranus);
                OverheadPeak.Instance.PegAfarDodge(OverheadPeak.Instance.Use, 1);
                // 增加连胜值
                OverheadPeak.Instance.PegAfarDodge(OverheadPeak.Instance.Anthocyanin_Limb, 1);
            }
            // 恢复体力
            WanderPeak.Instance.PegWander(CarpetSome);
            // 关卡增加一次过关成功次数
            WeaveHit[MortiseDodgeDodge.ToString()].PegUntwistBrook();
            // 存档
            LineRancher.Instance.ShedLine();
        }

        /// <summary>
        /// 过关失败
        /// </summary>
        public virtual void DodgeGobi()
        {
            // 连胜数值清零
            OverheadPeak.Instance.CudAfarDodge(OverheadPeak.Instance.Anthocyanin_Limb, 0);
        }
    }
}
