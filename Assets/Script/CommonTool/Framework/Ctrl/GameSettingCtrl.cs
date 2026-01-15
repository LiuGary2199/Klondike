using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using UnityEngine;

/// <summary>
/// 游戏配置
/// </summary>
namespace zeta_framework
{
    public class GameSettingCtrl : SettingDB
    {
        public static GameSettingCtrl Instance;

        public GameSettingCtrl(JsonData setting)
        {
            if (Instance == null)
            {
                Instance = this;
            }

            if (setting != null)
            {
                List<GameSettingDB> list = JsonMapper.ToObject<List<GameSettingDB>>(setting.ToJson());
                list.ForEach(db => {
                    // 反射为属性赋值
                    PropertyInfo propertyInfo = GetType().GetProperty(db.id);
                    if (propertyInfo != null)
                    {
                        object _value = Convert.ChangeType(db.value, propertyInfo.PropertyType, CultureInfo.InvariantCulture);
                        propertyInfo.SetValue(this, _value);
                    }
                });
            }
        }

        /// <summary>
        /// 根据名称获取某个配置的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetSettingById<T>(string id)
        {
            PropertyInfo property = GetType().GetProperty(id);
            if (property != null)
            {
                return (T)property.GetValue(this);
            }
            else
            {
                return default;
            }
        }
        
    }
}
