using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using static zeta_framework.Colonize;

/// <summary>
/// 活动管理
/// </summary>
namespace zeta_framework
{
    public class ColonizePeak : ActivityCtrlDB, IPeak
    {
        public static ColonizePeak Instance;
        
        public ColonizePeak()
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
        public void DugoutSewColonize(JsonData setting)
        {
            foreach (PropertyInfo propertyInfo in GetType().GetProperties())
            {
                object Northern= propertyInfo.GetValue(this);
                MethodInfo methodInfo = Northern.GetType().GetMethod("SetSetting");
                string key = "Colonize" + propertyInfo.Name;
                methodInfo.Invoke(Northern, new object[] { setting != null && setting.ContainsKey(key) ? setting[key] : null });
            }
        }

        /// <summary>
        /// 读取每个活动的存档
        /// </summary>
        /// <param name="data"></param>
        public void Deaf(JsonData data)
        {
            foreach (PropertyInfo propertyInfo in GetType().GetProperties())
            {
                object Northern= propertyInfo.GetValue(this);
                MethodInfo methodInfo = Northern.GetType().GetMethod("SetData");
                string key = propertyInfo.Name;
                methodInfo.Invoke(Northern, new object[] { data != null && data.ContainsKey(key) ? data[key] : null });
            }
        }

        public Dictionary<string, object> FarRepresentLine()
        {
            Dictionary<string, object> Mark= new();
            foreach (PropertyInfo property in GetType().GetProperties())
            {
                Colonize Northern= (Colonize)property.GetValue(this);
                Mark.Add(property.Name, Northern.FarLine());
            }
            return Mark;
        }

        /// <summary>
        /// 根据活动id获取活动实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="activity_id"></param>
        /// <returns></returns>
        public T FarColonizeSoAt<T>(string activity_id)
        {
            return (T)GetType().GetProperty(activity_id).GetValue(this);
        }

        public List<Colonize> FarNewsworthy()
        {
            List<Colonize> list = new List<Colonize>();
            foreach (PropertyInfo propertyInfo in GetType().GetProperties())
            {
                list.Add((Colonize)propertyInfo.GetValue(this));
            }
            return list;
        }

        public void SolderColonizeWidow()
        {
            foreach (PropertyInfo propertyInfo in GetType().GetProperties())
            {
                object Northern= propertyInfo.GetValue(this);
                MethodInfo methodInfo = Northern.GetType().GetMethod("CalculateState");
                methodInfo.Invoke(Northern, null);
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
