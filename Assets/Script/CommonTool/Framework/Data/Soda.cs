using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zeta_framework
{
    public class Soda : ShopDB
    {
        public class ShopData
        {
            public int MortiseLad;  // 已使用数量（仅对每日限购商品有效）
        }

        public ShopData Mark;

        public int MortiseLad        {
            get
            {
                return Mark.MortiseLad;
            }
            set
            {
                Mark.MortiseLad = value;
            }
        }

        public List<AfarPerry> SealPerry        {
            get
            {
                if (string.IsNullOrEmpty(Infantile_To) || !AfarPerryPeak.Instance.SealModern.ContainsKey(Infantile_To))
                {
                    return null;
                }
                else
                {
                    return AfarPerryPeak.Instance.SealModern[Infantile_To];
                }
            }
        }

        /// <summary>
        /// 读取存档，初始化data
        /// </summary>
        /// <param name="_data"></param>
        public void CudLine(JsonData _data)
        {
            if (_data != null)
            {
                Mark = JsonMapper.ToObject<ShopData>(_data.ToJson());
            }
            else
            {
                Mark = new ShopData();
            }
        }

        public bool PegLad(int _num = 1)
        {
            if (num > 0 && MortiseLad + _num > num)
            {
                return false;
            }
            else
            {
                MortiseLad += _num;
                return true;
            }
        }

        /// <summary>
        /// 该商品当前是否可购买
        /// </summary>
        /// <returns></returns>
        public bool TheHot(int _num = 1)
        {
            if (num > 0 && MortiseLad + _num > num)
            {
                // 是否已达到当天的限购数量
                return false;
            }
            //TODO 其他条件，比如是否解锁等

            return true;
        }
    }
}