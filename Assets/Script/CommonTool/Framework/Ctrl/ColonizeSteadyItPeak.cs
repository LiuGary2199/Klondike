using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 去广告活动
/// </summary>

namespace zeta_framework
{
    public class ColonizeSteadyItPeak: Colonize
    {
        public static ColonizeSteadyItPeak Instance;

        public const string SodaAt= "s_remove_ad"; // 去广告在商店中的配置(Soda)

        public ColonizeSteadyItPeak()
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
        public bool UpGuinea()
        {
            return OverheadPeak.Instance.remove_It.MortiseDodge > 0;
        }

        /// <summary>
        /// 购买去广告
        /// </summary>
        /// <param name="cb"></param>
        public void Hot(System.Action<SenseRead> cb)
        {
            if (UpGuinea())
            {
                cb?.Invoke(SenseRead.Success);
            }

            Soda shopItem = SodaPeak.Instance.FarSodaSoAt(SodaAt);
            SodaPeak.Instance.Hot(shopItem, (errorCode) => { 
                if (errorCode == SenseRead.Success)
                {
                    // 购买成功，给奖励
                    OverheadPeak.Instance.PegAfarPerry(shopItem.gp_Bid);
                    // 活动状态改为Finish
                    Fertilizer();
                    cb?.Invoke(SenseRead.Success);
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
        public List<AfarPerry> FarBritain() {
            Soda shopItem = SodaPeak.Instance.FarSodaSoAt(SodaAt);
            return shopItem.SealPerry;
        }
    }
}

