using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zeta_framework
{
    public class Item : ItemDB
    {
        public class ItemData
        {
            public int currentValue;
        }

        public ItemData data;

        // 资源当前值
        public int currentValue
        {
            get
            {
                return data.currentValue;
            }
            private set
            {
                data.currentValue = value;
            }
        }

        // 资源图标
        private Sprite _Icon;
        public Sprite Icon
        {
            get
            {
                if (_Icon == null)
                {
                    _Icon = Resources.Load<Sprite>(icon);
                }
                return _Icon;
            }
        }

        /// <summary>
        /// 读取存档，初始化data
        /// ResourceCtrl中通过反射调用，不要删除
        /// </summary>
        /// <param name="_data"></param>
        public void SetData(JsonData _data)
        {
            if (_data != null)
            {
                data = JsonMapper.ToObject<ItemData>(_data.ToJson());
            }
            else
            {
                data = new ItemData();
                data.currentValue = defaultValue;
            }
        }

        public bool AddValue(int _value, bool checkMax)
        {
            int newValue = currentValue + _value;
            if (minValue != -1 && newValue < minValue)
            {
                return false;
            }
            if (maxValue != -1 && newValue > maxValue && checkMax)
            {
                newValue = Math.Max(maxValue, currentValue);
            }
            currentValue = newValue;

            DataManager.Instance.SaveData();
            return true;
        }

        public bool SetValue(int newValue, bool checkValue)
        {
            if (checkValue)
            {
                if (minValue != -1 && newValue < minValue)
                {
                    return false;
                }
                if (maxValue != -1 && newValue > maxValue)
                {
                    return false;
                }
            }
            currentValue = newValue;
            DataManager.Instance.SaveData();
            return true;
        }
    }
}


