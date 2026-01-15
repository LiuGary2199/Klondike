using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zeta_framework
{
    public class ExpBox
    {
        private List<ExpBoxDB> boxLevelData;     // 宝箱等级配置

        public ExpBox()
        {
            boxLevelData = new();
        }

        public void SetSettingData(List<ExpBoxDB> _boxLevelData)
        {
            boxLevelData = _boxLevelData;
        }

        public class ExpBoxData
        {
            public int currentValue;   // 升级所需资源当前值
            public int currentLv;   // 当前等级， 从0开始
            public int currentProgress;     // 当前等级进度
        }

        public ExpBoxData data { get; private set; }

        public int currentLv
        {
            get
            {
                return data.currentLv;
            }
        }

        public int currentProgress
        {
            get
            {
                return data.currentProgress;
            }
        }

        /// <summary>
        /// 宝箱最大等级
        /// </summary>
        public int maxLevel
        {
            get
            {
                return boxLevelData.Count;
            }
        }

        /// <summary>
        /// 读取存档，初始化data
        /// </summary>
        /// <param name="_data"></param>
        public void SetData(JsonData _data)
        {
            if (_data != null)
            {
                data = JsonMapper.ToObject<ExpBoxData>(_data.ToJson());
            }
            else
            {
                data = new ExpBoxData();
            }

            // 注册经验变更回调事件
            MessageCenterLogic.GetInstance().Register(CConfig.mg_ItemChange_ + boxLevelData[0].exp_key, (md) => {
                AddCurrentValue(md.valueInt);
            });
        }

        /// <summary>
        /// 获取某个等级的配置，如果超过配置的最大等级，根据配置“通关后奖励策略”取值，如果没有配置，取null
        /// </summary>
        /// <param name="lv"></param>
        /// <returns></returns>
        public ExpBoxDB GetBoxLevelDataByLv(int lv)
        {
            if (lv < boxLevelData.Count)
            {
                return boxLevelData[lv];
            }
            else
            {
                // 通关后奖励策略
                int last_lv_strategy = GameSettingCtrl.Instance.GetSettingById<int>("last_lv_strategy_" + boxLevelData[0].box_id);
                if (last_lv_strategy == 0)
                {
                    // 通关后不给奖励
                    return null;
                }
                else if (last_lv_strategy == 1)
                {
                    // 通关后按最后一级给奖励
                    return boxLevelData[boxLevelData.Count - 1];
                }
                else if (last_lv_strategy == 2)
                {
                    // 通关后重新从第一级循环给奖励
                    return boxLevelData[lv / boxLevelData.Count];
                }
                return null;
            }
        }

        /// <summary>
        /// 某个等级，到当前等级的所有配置
        /// </summary>
        /// <param name="lv"></param>
        /// <returns></returns>
        public List<ExpBoxDB> GetBoxLevelDataFromLv(int lv)
        {
            List<ExpBoxDB> dataList = new();

            if (lv < data.currentLv)
            {
                for(int i = lv; i <= data.currentLv; i++)
                {
                    ExpBoxDB setting = boxLevelData[i];
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
        public void AddCurrentValue(int _value)
        {
            data.currentValue += _value;

            // 计算等级
            CalculateLv(out int cl, out int cp);
            if (data.currentLv < cl)
            {
                for (int i = data.currentLv; i < cl; i++)
                {
                    // 发放奖励
                    ExpBoxDB db = GetBoxLevelDataByLv(i);
                    if (db != null && !string.IsNullOrEmpty(db.itemgroup_id))
                    {
                        ResourceCtrl.Instance.AddItemGroup(db.itemgroup_id);
                    }
                    else if (db != null && !string.IsNullOrEmpty(db.item_id))
                    {
                        ResourceCtrl.Instance.AddItemValue(db.item_id, db.item_value);
                    }
                    data.currentLv++;
                }
            }
            data.currentProgress = cp;
            DataManager.Instance.SaveData();
        }

        /// <summary>
        /// 计算等级和进度
        /// </summary>
        /// <param name="cl"></param>
        /// <param name="ce"></param>
        private void CalculateLv(out int cl, out int ce)
        {
            cl = 0;
            ce = data.currentValue;
            for (int i = 0; i < boxLevelData.Count; i++)
            {
                if (ce >= boxLevelData[i].exp_value)
                {
                    cl++;
                    ce -= boxLevelData[i].exp_value;
                }
            }
            // 如果已达到最后一级，按照最后一级配置继续增加等级
            if (cl == boxLevelData.Count)
            {
                int lastLvExpValue = boxLevelData[boxLevelData.Count - 1].exp_value;
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

