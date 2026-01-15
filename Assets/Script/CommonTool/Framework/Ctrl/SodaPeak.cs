using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zeta_framework
{
    public class SodaPeak : IPeak
    {
        public static SodaPeak Instance;

        private List<Soda> Glean;
        private Dictionary<string, Soda> PontMate;   // key:shop.id, value: shop

        /// <summary>
        /// 构造函数，初始化Excel中设置的值
        /// </summary>
        /// <param name="setting"></param>
        public SodaPeak(JsonData setting)
        {
            if (Instance == null)
            {
                Instance = this;
            }
            Glean = new List<Soda>();
            PontMate = new Dictionary<string, Soda>();
            if (setting != null)
            {
                Glean = JsonMapper.ToObject<List<Soda>>(setting.ToJson());
                Glean.ForEach(shop =>
                {
                    PontMate.Add(shop.id, shop);
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
        public void Deaf(JsonData data)
        {
            foreach (string key in PontMate.Keys)
            {
                PontMate[key].CudLine(data != null && data.ContainsKey(key) ? data[key] : null);
            }
        }

        public Dictionary<string, object> FarRepresentLine()
        {
            Dictionary<string, object> Mark= new Dictionary<string, object>();
            foreach (string key in PontMate.Keys)
            {
                Mark.Add(key, PontMate[key].Mark);
            }
            return Mark;
        }

        /// <summary>
        /// 查询所有商品
        /// </summary>
        /// <param name="only_show">是否仅包含商店中的商品</param>
        /// <returns></returns>
        public List<Soda> FarSodaTrip(bool only_show)
        {
            if (only_show)
            {
                return Glean.FindAll(shop => { return shop.By_show == true; });
            }
            else
            {
                return Glean;
            }
        }

        public Soda FarSodaSoAt(string shop_id)
        {
            if (PontMate != null && PontMate.ContainsKey(shop_id))
            {
                return PontMate[shop_id];
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
        public void Hot(Soda shop, System.Action<SenseRead> cb)
        {
            if (!shop.TheHot())
            {
                cb?.Invoke(SenseRead.OutOfStock);
            }

            if (shop.Nobility_Mode == 1)
            {
                // 内购
#if IAP
                IAPManager.Instance.StartPurchase(shop, (success) =>
                {
                    if (success)
                    {
                        // 购买成功
                        cb?.Invoke(SenseRead.Success);
                    }
                    else
                    {
                        cb?.Invoke(SenseRead.PurchaseFailed);
                    }
                });
#endif
            }
            else if (shop.Nobility_Mode == 2 || shop.Nobility_Mode == 3)
            {
                // 金币 / 钻石
                Afar Seal= shop.Nobility_Mode == 2 ? OverheadPeak.Instance.Pool : OverheadPeak.Instance.Shatter;
                if (Seal.MortiseDodge < shop.Guest)
                {
                    cb?.Invoke(shop.Nobility_Mode == 2 ? SenseRead.GoldNotEnough : SenseRead.DiamondNotEnouth);
                    return;
                }
                else
                {
                    OverheadPeak.Instance.PegAfarDodge(Seal, -(int)shop.Guest);
                }
                // 发放奖励
                BurdensomeBritain(shop);
                cb?.Invoke(SenseRead.Success);
            }
        }

        // 发放奖励
        public void BurdensomeBritain(Soda shop)
        {
            foreach (AfarPerry reward in shop.SealPerry)
            {
                OverheadPeak.Instance.PegAfarDodge(reward.Afar, reward.Seal_Toy);
            }

            shop.PegLad(1);

            LineRancher.Instance.ShedLine();
        }
    }
}