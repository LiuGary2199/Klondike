using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zeta_framework
{
    public class Afar : ItemDB
    {
        public class ItemData
        {
            public int MortiseDodge;
        }

        public ItemData Mark;

        // 资源当前值
        public int MortiseDodge        {
            get
            {
                return Mark.MortiseDodge;
            }
            private set
            {
                Mark.MortiseDodge = value;
            }
        }

        // 资源图标
        private Sprite _Part;
        public Sprite Part        {
            get
            {
                if (_Part == null)
                {
                    _Part = Resources.Load<Sprite>(Gram);
                }
                return _Part;
            }
        }

        /// <summary>
        /// 读取存档，初始化data
        /// ResourceCtrl中通过反射调用，不要删除
        /// </summary>
        /// <param name="_data"></param>
        public void CudLine(JsonData _data)
        {
            if (_data != null)
            {
                Mark = JsonMapper.ToObject<ItemData>(_data.ToJson());
            }
            else
            {
                Mark = new ItemData();
                Mark.MortiseDodge = ContendDodge;
            }
        }

        public bool PegDodge(int _value, bool checkMax)
        {
            int newValue = MortiseDodge + _value;
            if (LidDodge != -1 && newValue < LidDodge)
            {
                return false;
            }
            if (WeeDodge != -1 && newValue > WeeDodge && checkMax)
            {
                newValue = Math.Max(WeeDodge, MortiseDodge);
            }
            MortiseDodge = newValue;

            LineRancher.Instance.ShedLine();
            return true;
        }

        public bool CudDodge(int newValue, bool checkValue)
        {
            if (checkValue)
            {
                if (LidDodge != -1 && newValue < LidDodge)
                {
                    return false;
                }
                if (WeeDodge != -1 && newValue > WeeDodge)
                {
                    return false;
                }
            }
            MortiseDodge = newValue;
            LineRancher.Instance.ShedLine();
            return true;
        }
    }
}


