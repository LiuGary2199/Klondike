using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zeta_framework
{
    public class SetTug
    {
        private List<ExpBoxDB> LegDodgeLine;     // 宝箱等级配置

        public SetTug()
        {
            LegDodgeLine = new();
        }

        public void CudGradualLine(List<ExpBoxDB> _boxLevelData)
        {
            LegDodgeLine = _boxLevelData;
        }

        public class ExpBoxData
        {
            public int MortiseDodge;   // 升级所需资源当前值
            public int MortiseMy;   // 当前等级， 从0开始
            public int MortiseManeuver;     // 当前等级进度
        }

        public ExpBoxData Mark{ get; private set; }

        public int MortiseMy        {
            get
            {
                return Mark.MortiseMy;
            }
        }

        public int MortiseManeuver        {
            get
            {
                return Mark.MortiseManeuver;
            }
        }

        /// <summary>
        /// 宝箱最大等级
        /// </summary>
        public int WeeDodge        {
            get
            {
                return LegDodgeLine.Count;
            }
        }

        /// <summary>
        /// 读取存档，初始化data
        /// </summary>
        /// <param name="_data"></param>
        public void CudLine(JsonData _data)
        {
            if (_data != null)
            {
                Mark = JsonMapper.ToObject<ExpBoxData>(_data.ToJson());
            }
            else
            {
                Mark = new ExpBoxData();
            }

            // 注册经验变更回调事件
            AfricanExtendHorse.FarBefriend().Iroquois(CAdjoin.We_AfarUranus_ + LegDodgeLine[0].Use_Fix, (md) => {
                PegVoyagerDodge(md.NobleWit);
            });
        }

        /// <summary>
        /// 获取某个等级的配置，如果超过配置的最大等级，根据配置“通关后奖励策略”取值，如果没有配置，取null
        /// </summary>
        /// <param name="lv"></param>
        /// <returns></returns>
        public ExpBoxDB FarTugDodgeLineSoMy(int lv)
        {
            if (lv < LegDodgeLine.Count)
            {
                return LegDodgeLine[lv];
            }
            else
            {
                // 通关后奖励策略
                int last_lv_strategy = KickGradualPeak.Instance.FarGradualSoAt<int>("last_lv_strategy_" + LegDodgeLine[0].Leg_To);
                if (last_lv_strategy == 0)
                {
                    // 通关后不给奖励
                    return null;
                }
                else if (last_lv_strategy == 1)
                {
                    // 通关后按最后一级给奖励
                    return LegDodgeLine[LegDodgeLine.Count - 1];
                }
                else if (last_lv_strategy == 2)
                {
                    // 通关后重新从第一级循环给奖励
                    return LegDodgeLine[lv / LegDodgeLine.Count];
                }
                return null;
            }
        }

        /// <summary>
        /// 某个等级，到当前等级的所有配置
        /// </summary>
        /// <param name="lv"></param>
        /// <returns></returns>
        public List<ExpBoxDB> FarTugDodgeLineDukeMy(int lv)
        {
            List<ExpBoxDB> dataList = new();

            if (lv < Mark.MortiseMy)
            {
                for(int i = lv; i <= Mark.MortiseMy; i++)
                {
                    ExpBoxDB setting = LegDodgeLine[i];
                    if (setting != null)
                    {
                        dataList.Add(setting);
                    }
                }
            }
            return dataList;
        }

        /// <summary>
        /// 增加宝箱进度
        /// </summary>
        /// <param name="_value"></param>
        public void PegVoyagerDodge(int _value)
        {
            Mark.MortiseDodge += _value;

            // 计算等级
            BasicallyMy(out int cl, out int cp);
            if (Mark.MortiseMy < cl)
            {
                for (int i = Mark.MortiseMy; i < cl; i++)
                {
                    // 发放奖励
                    ExpBoxDB db = FarTugDodgeLineSoMy(i);
                    if (db != null && !string.IsNullOrEmpty(db.Infantile_To))
                    {
                        OverheadPeak.Instance.PegAfarPerry(db.Infantile_To);
                    }
                    else if (db != null && !string.IsNullOrEmpty(db.Seal_To))
                    {
                        OverheadPeak.Instance.PegAfarDodge(db.Seal_To, db.Seal_Noble);
                    }
                    Mark.MortiseMy++;
                }
            }
            Mark.MortiseManeuver = cp;
            LineRancher.Instance.ShedLine();
        }

        /// <summary>
        /// 计算等级和进度
        /// </summary>
        /// <param name="cl"></param>
        /// <param name="ce"></param>
        private void BasicallyMy(out int cl, out int ce)
        {
            cl = 0;
            ce = Mark.MortiseDodge;
            for (int i = 0; i < LegDodgeLine.Count; i++)
            {
                if (ce >= LegDodgeLine[i].Use_Noble)
                {
                    cl++;
                    ce -= LegDodgeLine[i].Use_Noble;
                }
            }
            // 如果已达到最后一级，按照最后一级配置继续增加等级
            if (cl == LegDodgeLine.Count)
            {
                int lastLvExpValue = LegDodgeLine[LegDodgeLine.Count - 1].Use_Noble;
                if (lastLvExpValue > 0)
                {
                    while (ce >= lastLvExpValue)
                    {
                        cl++;
                        ce -= lastLvExpValue;
                    }
                }
            }
        }
    }
}

