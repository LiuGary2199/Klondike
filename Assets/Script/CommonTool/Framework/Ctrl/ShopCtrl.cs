using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zeta_framework
{
    public class ShopCtrl : ICtrl
    {
        public static ShopCtrl Instance;

        private List<Shop> shops;
        private Dictionary<string, Shop> shopDict;   // key:shop.id, value: shop

        /// <summary>
        /// 构造函数，初始化Excel中设置的值
        /// </summary>
        /// <param name="setting"></param>
        public ShopCtrl(JsonData setting)
        {
            if (Instance == null)
            {
                Instance = this;
            }
            shops = new List<Shop>();
            shopDict = new Dictionary<string, Shop>();
            if (setting != null)
            {
                shops = JsonMapper.ToObject<List<Shop>>(setting.ToJson());
                shops.ForEach(shop =>
                {
                    shopDict.Add(shop.id, shop);
                });
            }
#if IAP
            // 初始化内购组件
            new IAPManager();
#endif
        }

        /// <summary>
        /// 初始化存档数据
        /// </summary>
        /// <param name="data"></param>
        public void Init(JsonData data)
        {
            foreach (string key in shopDict.Keys)
            {
                shopDict[key].SetData(data != null && data.ContainsKey(key) ? data[key] : null);
            }
        }

        public Dictionary<string, object> GetSerializeData()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            foreach (string key in shopDict.Keys)
            {
                data.Add(key, shopDict[key].data);
            }
            return data;
        }

        /// <summary>
        /// 查询所有商品
        /// </summary>
        /// <param name="only_show">是否仅包含商店中的商品</param>
        /// <returns></returns>
        public List<Shop> GetShopList(bool only_show)
        {
            if (only_show)
            {
                return shops.FindAll(shop => { return shop.is_show == true; });
            }
            else
            {
                return shops;
            }
        }

        public Shop GetShopById(string shop_id)
        {
            if (shopDict != null && shopDict.ContainsKey(shop_id))
            {
                return shopDict[shop_id];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 购买商品
        /// </summary>
        /// <param name="shop"></param>
        public void Buy(Shop shop, System.Action<ErrorCode> cb)
        {
            if (!shop.CanBuy())
            {
                cb?.Invoke(ErrorCode.OutOfStock);
            }

            if (shop.purchase_type == 1)
            {
                // 内购
#if IAP
                IAPManager.Instance.StartPurchase(shop, (success) =>
                {
                    if (success)
                    {
                        // 购买成功
                        cb?.Invoke(ErrorCode.Success);
                    }
                    else
                    {
                        cb?.Invoke(ErrorCode.PurchaseFailed);
                    }
                });
#endif
            }
            else if (shop.purchase_type == 2 || shop.purchase_type == 3)
            {
                // 金币 / 钻石
                Item item = shop.purchase_type == 2 ? ResourceCtrl.Instance.gold : ResourceCtrl.Instance.diamond;
                if (item.currentValue < shop.price)
                {
                    cb?.Invoke(shop.purchase_type == 2 ? ErrorCode.GoldNotEnough : ErrorCode.DiamondNotEnouth);
                    return;
                }
                else
                {
                    ResourceCtrl.Instance.AddItemValue(item, -(int)shop.price);
                }
                // 发放奖励
                DistributeRewards(shop);
                cb?.Invoke(ErrorCode.Success);
            }
        }

        // 发放奖励
        public void DistributeRewards(Shop shop)
        {
            foreach (ItemGroup reward in shop.itemGroup)
            {
                ResourceCtrl.Instance.AddItemValue(reward.Item, reward.item_num);
            }

            shop.AddNum(1);

            DataManager.Instance.SaveData();
        }
    }
}