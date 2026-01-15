using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 体力管理
/// </summary>

namespace zeta_framework
{
    public class HealthCtrl : ICtrl
    {
        public static HealthCtrl Instance;

        public long lastUpdateTime;   // 上次体力更新时间
        private long unlimitedTime;     // 无限体力终止时间

        public void Init(JsonData data)
        {
            if (Instance == null)
            {
                Instance = this;
            }

            if (data != null)
            {
                lastUpdateTime = data.ContainsKey("lastUpdateTime") ? long.Parse(data["lastUpdateTime"].ToString()) : 0;
                unlimitedTime = data.ContainsKey("unlimitedTime") ? long.Parse(data["unlimitedTime"].ToString()) : 0;
            }
            // 计算当前体力
            CalcCurrentHealth();
        }

        public Dictionary<string, object> GetSerializeData()
        {
            Dictionary<string, object> data = new();
            data.Add("lastUpdateTime", lastUpdateTime);
            data.Add("unlimitedTime", unlimitedTime);
            return data;
        }

        /// <summary>
        /// 计算当前体力
        /// </summary>
        public void CalcCurrentHealth()
        {
            Item healthItem = ResourceCtrl.Instance.health;
            // 上次体力修改时间，到当前时间应该恢复的体力
            if (lastUpdateTime == 0)
            {
                lastUpdateTime = DateUtil.Current();
            }
            int diffHealth = (int)(DateUtil.Current() - lastUpdateTime) / GameSettingCtrl.Instance.health_recharge_interval;
            // 体力不能超过设置的最大值
            diffHealth = Mathf.Max(Mathf.Min(diffHealth, healthItem.maxValue - healthItem.currentValue), 0);
            if (diffHealth > 0)
            {
                ResourceCtrl.Instance.AddItemValue(ResourceCtrl.Instance.health, diffHealth);
            }

            if (IsFull())
            {
                lastUpdateTime = 0;
            }
            else
            {
                lastUpdateTime = DateUtil.Current() - (DateUtil.Current() - lastUpdateTime) % GameSettingCtrl.Instance.health_recharge_interval;
            }
            DataManager.Instance.SaveData();
        }

        /// <summary>
        /// 获取当前体力和倒计时
        /// </summary>
        /// <param name="health"></param>
        /// <param name="countdown"></param>
        public void GetCurrentHealth(out int health, out int countdown)
        {
            health = ResourceCtrl.Instance.health.currentValue;
            if (lastUpdateTime == 0)
            {
                countdown = GameSettingCtrl.Instance.health_recharge_interval;
            }
            else
            {
                int health_recharge_interval = GameSettingCtrl.Instance.health_recharge_interval;
                countdown = health_recharge_interval - (int)(DateUtil.Current() - lastUpdateTime) % health_recharge_interval;
                countdown = countdown == 0 ? health_recharge_interval : countdown;
            }
        }

        /// <summary>
        /// 是否是无限体力状态
        /// </summary>
        /// <returns></returns>
        public bool IsUnlimitedState()
        {
            return unlimitedTime > DateUtil.Current();
        }
        
        /// <summary>
        /// 无限体力倒计时
        /// </summary>
        /// <returns></returns>
        public int UnlimitedCountdown()
        {
            return (int)(unlimitedTime - DateUtil.Current());
        }

        /// <summary>
        /// 体力是否已满
        /// </summary>
        /// <returns></returns>
        public bool IsFull()
        {
            return ResourceCtrl.Instance.health.currentValue >= ResourceCtrl.Instance.health.maxValue;
        }

        /// <summary>
        /// 扣除体力
        /// </summary>
        /// <returns></returns>
        public bool UseHealth(int num)
        {
            if (IsUnlimitedState())
            {
                return true;
            }
            CalcCurrentHealth();
            Item healthItem = ResourceCtrl.Instance.health;
            if (healthItem.currentValue < num)
            {
                return false;
            }
            
            ResourceCtrl.Instance.AddItemValue(healthItem, -num);
            DataManager.Instance.SaveData();
            return true;
        }

        /// <summary>
        /// 恢复体力
        /// </summary>
        /// <param name="num"></param>
        public void AddHealth(int num)
        {
            ResourceCtrl.Instance.AddItemValue(ResourceCtrl.Instance.health, num, true);
        }

        /// <summary>
        /// 增加无限体力时间
        /// </summary>
        /// <param name="value"></param>
        public void AddUnlimitedTime(int value)
        {
            long now = DateUtil.Current();
            if (unlimitedTime < now)
            {
                unlimitedTime = now + value;
            }
            else
            {
                unlimitedTime += value;
            }
            // 存档
            DataManager.Instance.SaveData();
        }

        /// <summary>
        /// 体力是否充足
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public bool IsHealthEnough(int num)
        {
            if(IsUnlimitedState())
            {
                return true;
            }
            CalcCurrentHealth();
            return ResourceCtrl.Instance.health.currentValue >= num;
        }
    }
}
