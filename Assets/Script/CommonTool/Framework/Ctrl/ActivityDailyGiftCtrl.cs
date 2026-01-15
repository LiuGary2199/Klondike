using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 每日签到
/// </summary>
namespace zeta_framework
{
    public class ActivityDailyGiftCtrl: Activity
    {
        public static ActivityDailyGiftCtrl Instance;

        private List<ActivityDailyGiftDB> dailyGifts;

        public ActivityDailyGiftCtrl()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        public override void SetSetting(JsonData setting)
        {
            if (setting != null)
            {
                dailyGifts = JsonMapper.ToObject<List<ActivityDailyGiftDB>>(setting.ToJson());
            }
            else
            {
                dailyGifts = new();
            }
        }


        /// <summary>
        /// 获取当前应该是第几天签到（从0开始）
        /// </summary>
        /// <returns></returns>
        public int GetCurrentIndex()
        {
            return AttendTimes % dailyGifts.Count;
        }

        /// <summary>
        /// 获取每日签到所有配置
        /// </summary>
        /// <returns></returns>
        public List<ActivityDailyGiftDB> GetAllSetting()
        {
            return dailyGifts;
        }

        /// <summary>
        /// 获取第n天的奖励
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public List<ItemGroup> GetRewardByIndex(int index)
        {
            List<ItemGroup> rewards = new();
            ActivityDailyGiftDB dailyGift = dailyGifts[index];
            if (!string.IsNullOrEmpty(dailyGift.itemgroup_id))
            {
                rewards.AddRange(ResourceCtrl.Instance.GetItemGroupById(dailyGift.itemgroup_id));
            }
            if (!string.IsNullOrEmpty(dailyGift.item_id) && dailyGift.item_num > 0)
            {
                ItemGroup itemGroup = new(dailyGift.item_id, dailyGift.item_num);
                rewards.Add(itemGroup);
            }

            return rewards;
        }

        /// <summary>
        /// 领取签到奖励
        /// </summary>
        public void Collect()
        {
            int index = GetCurrentIndex();
            List<ItemGroup> rewards = GetRewardByIndex(index);
            ResourceCtrl.Instance.AddItemGroup(rewards);
            // 活动设置finish状态
            Settlement();
        }
    }
}

