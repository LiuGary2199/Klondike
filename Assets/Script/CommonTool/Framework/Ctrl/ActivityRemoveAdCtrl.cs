using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 去广告活动
/// </summary>

namespace zeta_framework
{
    public class ActivityRemoveAdCtrl: Activity
    {
        public static ActivityRemoveAdCtrl Instance;

        public const string ShopId = "s_remove_ad"; // 去广告在商店中的配置(Shop)

        public ActivityRemoveAdCtrl()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }


        /// <summary>
        /// 去广告功能是否已经生效
        /// </summary>
        /// <returns></returns>
        public bool IsEffect()
        {
            return ResourceCtrl.Instance.remove_ad.currentValue > 0;
        }

        /// <summary>
        /// 购买去广告
        /// </summary>
        /// <param name="cb"></param>
        public void Buy(System.Action<ErrorCode> cb)
        {
            if (IsEffect())
            {
                cb?.Invoke(ErrorCode.Success);
            }

            Shop shopItem = ShopCtrl.Instance.GetShopById(ShopId);
            ShopCtrl.Instance.Buy(shopItem, (errorCode) => { 
                if (errorCode == ErrorCode.Success)
                {
                    // 购买成功，给奖励
                    ResourceCtrl.Instance.AddItemGroup(shopItem.gp_pid);
                    // 活动状态改为Finish
                    Settlement();
                    cb?.Invoke(ErrorCode.Success);
                }
                else
                {
                    // 购买失败，直接返回
                    cb?.Invoke(errorCode);
                }
            });
        }

        /// <summary>
        /// 去广告活动的所有奖励
        /// </summary>
        /// <returns></returns>
        public List<ItemGroup> GetRewards() {
            Shop shopItem = ShopCtrl.Instance.GetShopById(ShopId);
            return shopItem.itemGroup;
        }
    }
}

