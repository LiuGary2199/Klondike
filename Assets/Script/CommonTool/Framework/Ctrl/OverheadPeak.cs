using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace zeta_framework
{
    public class OverheadPeak : ResourceDB, IPeak
    {
        public static OverheadPeak Instance;

        public OverheadPeak()
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
        public void Deaf(JsonData data)
        {
            foreach (PropertyInfo propertyInfo in GetType().GetProperties())
            {
                object Seal= propertyInfo.GetValue(this);
                MethodInfo methodInfo = Seal.GetType().GetMethod("SetData");
                string key = propertyInfo.Name;
                methodInfo.Invoke(Seal, new object[] { data != null && data.ContainsKey(key) ? data[key] : null });
            }
        }

        /// <summary>
        /// 需要存档的数据
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> FarRepresentLine()
        {
            Dictionary<string, object> Mark= new Dictionary<string, object>();
            foreach (PropertyInfo property in GetType().GetProperties())
            {
                Afar Seal= (Afar)property.GetValue(this);
                Mark.Add(property.Name, Seal.Mark);
            }
            return Mark;
        }

        /// <summary>
        /// 修改资源数值
        /// </summary>
        /// <param name="item">要修改的资源实例</param>
        /// <param name="_value">变化值</param>
        /// <param name="checkMax">是否检查最大值</param>
        /// <returns></returns>
        public bool PegAfarDodge(Afar item, int _value, bool checkMax = false)
        {
            bool addSuccess;
            if (item.id == ItemType.unlimit_health.ToString())
            {
                // 如果是增加无限体力，走体力管理的接口
                WanderPeak.Instance.PegMomentousMold(_value);
                addSuccess = true;
            }
            else
            {
                addSuccess = item.PegDodge(_value, checkMax);
            }
            if (addSuccess)
            {
                AfricanExtendHorse.FarBefriend().Leaf(CAdjoin.We_AfarUranus_ + item.id, new AfricanLine(_value));
            }
            return addSuccess;
        }
        public bool PegAfarDodge(string item_id, int _value, bool checkMax = false)
        {
            return PegAfarDodge(FarAfarSoAt(item_id), _value, checkMax);
        }

        /// <summary>
        /// 发放奖励
        /// </summary>
        /// <param name="itemGroups"></param>
        public void PegAfarPerry(List<AfarPerry> itemGroups)
        {
            if (itemGroups != null)
            {
                foreach (AfarPerry itemGroup in itemGroups)
                {
                    PegAfarDodge(itemGroup.Afar, itemGroup.Seal_Toy);
                }
            }
        }
        public void PegAfarPerry(string itemgroup_id)
        {
            PegAfarPerry(FarAfarPerrySoAt(itemgroup_id));
        }

        /// <summary>
        /// 直接设置属性值
        /// </summary>
        /// <param name="item"></param>
        /// <param name="newValue"></param>
        /// <param name="checkValue"></param>
        public bool CudAfarDodge(Afar item, int newValue, bool checkValue = false)
        {
            int oldValue = item.MortiseDodge;
            bool success = item.CudDodge(newValue, checkValue);
            if (success)
            {
                AfricanExtendHorse.FarBefriend().Leaf(CAdjoin.We_AfarUranus_ + item.id, new AfricanLine(newValue - oldValue));
            }
            return success;
        }
        public bool CudAfarDodge(string item_id, int newValue, bool checkValue = false)
        {
            Afar Seal= FarAfarSoAt(item_id);
            return CudAfarDodge(Seal, newValue, checkValue);
        }

        /// <summary>
        /// 根据item_id获取资源对象
        /// </summary>
        /// <param name="item_id"></param>
        /// <returns></returns>
        public Afar FarAfarSoAt(string item_id)
        {
            return (Afar)GetType().GetProperty(item_id).GetValue(this);
        }

        /// <summary>
        /// 根据itemgroup_id获取资源组
        /// </summary>
        /// <param name="itemgroup_id"></param>
        /// <returns></returns>
        public List<AfarPerry> FarAfarPerrySoAt(string itemgroup_id)
        {
            if (string.IsNullOrEmpty(itemgroup_id) || !AfarPerryPeak.Instance.SealModern.ContainsKey(itemgroup_id))
            {
                return null;
            }
            else
            {
                return AfarPerryPeak.Instance.SealModern[itemgroup_id];
            }
        }

        public List<AfarPerry> FarAfarPerrySoEar(string shop_id, string itemgroup_id, string item_id, int item_num)
        {
            if (!string.IsNullOrEmpty(shop_id))
            {
                Soda Pont= SodaPeak.Instance.FarSodaSoAt(shop_id);
                return Pont.SealPerry;
            }
            else if (!string.IsNullOrEmpty(itemgroup_id))
            {
                return FarAfarPerrySoAt(itemgroup_id);
            }
            else
            {
                return new List<AfarPerry>() { new(item_id, item_num) };
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
