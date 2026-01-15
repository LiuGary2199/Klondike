using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zeta_framework
{
    /// <summary>
    /// 无尽宝藏
    /// </summary>
    public class ActivityEndlessTreasureCtrl : Activity
    {
        public static ActivityEndlessTreasureCtrl Instance;

        public List<ActivityEndlessTreasureDB> endlessTreasures;

        private int currentIndex;   // 当前待领取的奖励序号（从0开始）
        private int currentAttendTimes; // 存档中参加的活动期数

        public int CurrentIndex
        {
            get
            {
                return currentIndex;
            }
        }

        public ActivityEndlessTreasureCtrl() { 
            Instance ??= this;
        }

        /// <summary>
        /// 初始化无尽宝藏的设置
        /// </summary>
        /// <param name="setting"></param>
        public override void SetSetting(JsonData setting)
        {
            if (setting != null)
            {
                endlessTreasures = JsonMapper.ToObject<List<ActivityEndlessTreasureDB>>(setting.ToJson());
            }
            else
            {
                endlessTreasures = new();
            }
        }

        /// <summary>
        /// 初始化存档
        /// </summary>
        /// <param name="_data"></param>
        public override void SetData(JsonData _data)
        {
            base.SetData(_data != null && _data.ContainsKey("data") ? _data["data"] : null);
            // 判断是否为新一期活动，是否需要重置待领取的奖励序号
            currentAttendTimes = _data != null && _data.ContainsKey("attendTime") ? int.Parse(_data["attendTime"].ToString()) : 0;
            currentIndex = currentAttendTimes == AttendTimes && _data != null && _data.ContainsKey("currentIndex") ? int.Parse(_data["currentIndex"].ToString()) : 0;
        }

        public override object GetData()
        {
            Dictionary<string, object> data = new()
            {
                { "data", base.GetData() },
                { "attendTime", currentAttendTimes },
                { "currentIndex", currentIndex }
            };
            return data;
        }

        // 领取奖励
        public void ClaimReward()
        {
            ActivityEndlessTreasureDB itemDB = endlessTreasures[currentAttendTimes];
            // 在商店中配置的奖励，购买时已发放奖励，所以此处只需要给shop_id为空的配置发放奖励
            if (string.IsNullOrEmpty(itemDB.shop_id))
            {
                if (!string.IsNullOrEmpty(itemDB.itemgroup_id))
                {
                    ResourceCtrl.Instance.AddItemGroup(itemDB.itemgroup_id);
                }
                else if (!string.IsNullOrEmpty(itemDB.item_id))
                {
                    ResourceCtrl.Instance.AddItemValue(itemDB.item_id, itemDB.item_num);
                }
            }

            currentIndex++;
            DataManager.Instance.SaveData();
        }
    }
}

