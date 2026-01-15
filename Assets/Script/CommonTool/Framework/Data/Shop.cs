using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zeta_framework
{
    public class Shop : ShopDB
    {
        public class ShopData
        {
            public int currentNum;  // 已使用数量（仅对每日限购商品有效）
        }

        public ShopData data;

        public int currentNum
        {
            get
            {
                return data.currentNum;
            }
            set
            {
                data.currentNum = value;
            }
        }

        public List<ItemGroup> itemGroup
        {
            get
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
        }

        /// <summary>
        /// 读取存档，初始化data
        /// </summary>
        /// <param name="_data"></param>
        public void SetData(JsonData _data)
        {
            if (_data != null)
            {
                data = JsonMapper.ToObject<ShopData>(_data.ToJson());
            }
            else
            {
                data = new ShopData();
            }
        }

        public bool AddNum(int _num = 1)
        {
            if (num > 0 && currentNum + _num > num)
            {
                return false;
            }
            else
            {
                currentNum += _num;
                return true;
            }
        }

        /// <summary>
        /// 该商品当前是否可购买
        /// </summary>
        /// <returns></returns>
        public bool CanBuy(int _num = 1)
        {
            if (num > 0 && currentNum + _num > num)
            {
                // 是否已达到当天的限购数量
                return false;
            }
            //TODO 其他条件，比如是否解锁等

            return true;
        }
    }
}