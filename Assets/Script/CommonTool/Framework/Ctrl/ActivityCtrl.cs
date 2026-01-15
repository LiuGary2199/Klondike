using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using static zeta_framework.Activity;

/// <summary>
/// 活动管理
/// </summary>
namespace zeta_framework
{
    public class ActivityCtrl : ActivityCtrlDB, ICtrl
    {
        public static ActivityCtrl Instance;
        
        public ActivityCtrl()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        /// <summary>
        /// 每个活动初始化自己的配置
        /// </summary>
        /// <param name="setting"></param>
        public void CreateSubActivity(JsonData setting)
        {
            foreach (PropertyInfo propertyInfo in GetType().GetProperties())
            {
                object activity = propertyInfo.GetValue(this);
                MethodInfo methodInfo = activity.GetType().GetMethod("SetSetting");
                string key = "Activity" + propertyInfo.Name;
                methodInfo.Invoke(activity, new object[] { setting != null && setting.ContainsKey(key) ? setting[key] : null });
            }
        }

        /// <summary>
        /// 读取每个活动的存档
        /// </summary>
        /// <param name="data"></param>
        public void Init(JsonData data)
        {
            foreach (PropertyInfo propertyInfo in GetType().GetProperties())
            {
                object activity = propertyInfo.GetValue(this);
                MethodInfo methodInfo = activity.GetType().GetMethod("SetData");
                string key = propertyInfo.Name;
                methodInfo.Invoke(activity, new object[] { data != null && data.ContainsKey(key) ? data[key] : null });
            }
        }

        public Dictionary<string, object> GetSerializeData()
        {
            Dictionary<string, object> data = new();
            foreach (PropertyInfo property in GetType().GetProperties())
            {
                Activity activity = (Activity)property.GetValue(this);
                data.Add(property.Name, activity.GetData());
            }
            return data;
        }

        /// <summary>
        /// 根据活动id获取活动实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="activity_id"></param>
        /// <returns></returns>
        public T GetActivityById<T>(string activity_id)
        {
            return (T)GetType().GetProperty(activity_id).GetValue(this);
        }

        public List<Activity> GetActivities()
        {
            List<Activity> list = new List<Activity>();
            foreach (PropertyInfo propertyInfo in GetType().GetProperties())
            {
                list.Add((Activity)propertyInfo.GetValue(this));
            }
            return list;
        }

        public void UpdateActivityState()
        {
            foreach (PropertyInfo propertyInfo in GetType().GetProperties())
            {
                object activity = propertyInfo.GetValue(this);
                MethodInfo methodInfo = activity.GetType().GetMethod("CalculateState");
                methodInfo.Invoke(activity, null);
            }
        }
    }

    /// <summary>
    /// 活动状态
    /// </summary>
    public enum ActivityState
    {
        None,
        NotOpen, // 活动未开启
        NotUnlock, // 活动未解锁
        NotAttend, // 本期还未参与
        Attending, // 参赛中
        NeedSettlement, // 已结束未结算
        Finished,   // 已结束已结算
    }
}
