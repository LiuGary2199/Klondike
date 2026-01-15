using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace zeta_framework
{
    public class ResourceCtrl : ResourceDB, ICtrl
    {
        public static ResourceCtrl Instance;

        public ResourceCtrl()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        /// <summary>
        /// 初始化存档数据
        /// </summary>
        /// <param name="data"></param>
        public void Init(JsonData data)
        {
            foreach (PropertyInfo propertyInfo in GetType().GetProperties())
            {
                object item = propertyInfo.GetValue(this);
                MethodInfo methodInfo = item.GetType().GetMethod("SetData");
                string key = propertyInfo.Name;
                methodInfo.Invoke(item, new object[] { data != null && data.ContainsKey(key) ? data[key] : null });
            }
        }

        /// <summary>
        /// 需要存档的数据
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> GetSerializeData()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            foreach (PropertyInfo property in GetType().GetProperties())
            {
                Item item = (Item)property.GetValue(this);
                data.Add(property.Name, item.data);
            }
            return data;
        }

        /// <summary>
        /// 修改资源数值
        /// </summary>
        /// <param name="item">要修改的资源实例</param>
        /// <param name="_value">变化值</param>
        /// <param name="checkMax">是否检查最大值</param>
        /// <returns></returns>
        public bool AddItemValue(Item item, int _value, bool checkMax = false)
        {
            bool addSuccess;
            if (item.id == ItemType.unlimit_health.ToString())
            {
                // 如果是增加无限体力，走体力管理的接口
                HealthCtrl.Instance.AddUnlimitedTime(_value);
                addSuccess = true;
            }
            else
            {
                addSuccess = item.AddValue(_value, checkMax);
            }
            if (addSuccess)
            {
                MessageCenterLogic.GetInstance().Send(CConfig.mg_ItemChange_ + item.id, new MessageData(_value));
            }
            return addSuccess;
        }
        public bool AddItemValue(string item_id, int _value, bool checkMax = false)
        {
            return AddItemValue(GetItemById(item_id), _value, checkMax);
        }

        /// <summary>
        /// 发放奖励
        /// </summary>
        /// <param name="itemGroups"></param>
        public void AddItemGroup(List<ItemGroup> itemGroups)
        {
            if (itemGroups != null)
            {
                foreach (ItemGroup itemGroup in itemGroups)
                {
                    AddItemValue(itemGroup.Item, itemGroup.item_num);
                }
            }
        }
        public void AddItemGroup(string itemgroup_id)
        {
            AddItemGroup(GetItemGroupById(itemgroup_id));
        }

        /// <summary>
        /// 直接设置属性值
        /// </summary>
        /// <param name="item"></param>
        /// <param name="newValue"></param>
        /// <param name="checkValue"></param>
        public bool SetItemValue(Item item, int newValue, bool checkValue = false)
        {
            int oldValue = item.currentValue;
            bool success = item.SetValue(newValue, checkValue);
            if (success)
            {
                MessageCenterLogic.GetInstance().Send(CConfig.mg_ItemChange_ + item.id, new MessageData(newValue - oldValue));
            }
            return success;
        }
        public bool SetItemValue(string item_id, int newValue, bool checkValue = false)
        {
            Item item = GetItemById(item_id);
            return SetItemValue(item, newValue, checkValue);
        }

        /// <summary>
        /// 根据item_id获取资源对象
        /// </summary>
        /// <param name="item_id"></param>
        /// <returns></returns>
        public Item GetItemById(string item_id)
        {
            return (Item)GetType().GetProperty(item_id).GetValue(this);
        }

        /// <summary>
        /// 根据itemgroup_id获取资源组
        /// </summary>
        /// <param name="itemgroup_id"></param>
        /// <returns></returns>
        public List<ItemGroup> GetItemGroupById(string itemgroup_id)
        {
            if (string.IsNullOrEmpty(itemgroup_id) || !ItemGroupCtrl.Instance.itemGroups.ContainsKey(itemgroup_id))
            {
                return null;
            }
            else
            {
                return ItemGroupCtrl.Instance.itemGroups[itemgroup_id];
            }
        }

        public List<ItemGroup> GetItemGroupByIds(string shop_id, string itemgroup_id, string item_id, int item_num)
        {
            if (!string.IsNullOrEmpty(shop_id))
            {
                Shop shop = ShopCtrl.Instance.GetShopById(shop_id);
                return shop.itemGroup;
            }
            else if (!string.IsNullOrEmpty(itemgroup_id))
            {
                return GetItemGroupById(itemgroup_id);
            }
            else
            {
                return new List<ItemGroup>() { new(item_id, item_num) };
            }
        }
    }

    public enum ItemType
    {
        gold,
        diamond,
        health,
        unlimit_health,
        exp
    }
}
