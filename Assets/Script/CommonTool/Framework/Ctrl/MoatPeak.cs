using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zeta_framework
{
    public class MoatPeak : IPeak
    {
        public static MoatPeak Instance;

        private List<Moat> Brave;
        private Dictionary<string, Moat> DashMate;     // 所有皮肤，key:皮肤id
        private Dictionary<string, List<Moat>> DashTypes;  // 所有皮肤分类，key：皮肤分类
        private Dictionary<string, Moat> CavernMoat;    // 当前使用的皮肤, key:皮肤分类

        /// <summary>
        /// 构造函数，初始化Excel中设置的值
        /// </summary>
        /// <param name="setting"></param>
        public MoatPeak(JsonData setting)
        {
            if (Instance == null)
            {
                Instance = this;
            }
            Brave = new();
            DashMate = new();
            DashTypes = new();
            CavernMoat = new();
            if (setting != null)
            {
                Brave = JsonMapper.ToObject<List<Moat>>(setting.ToJson());
                Brave.ForEach(skin =>
                {
                    DashMate.Add(skin.Seal_To, skin);
                    // 皮肤分类
                    if (!DashTypes.ContainsKey(skin.Dash_Mode))
                    {
                        DashTypes.Add(skin.Dash_Mode, new());
                    }
                    DashTypes[skin.Dash_Mode].Add(skin);
                    // 当前正在使用的皮肤，默认使用第一个
                    if (!CavernMoat.ContainsKey(skin.Dash_Mode))
                    {
                        CavernMoat.Add(skin.Dash_Mode, skin);
                    }
                });
            }

            // 向资源管理器中注册经验变更回调事件
            AfricanExtendHorse.FarBefriend().Iroquois(CAdjoin.We_AfarUranus_ + OverheadPeak.Instance.Use.id, (md) =>
            {
                LordlySoSet();
            });
        }

        /// <summary>
        /// 初始化存档数据
        /// </summary>
        /// <param name="data"></param>
        public void Deaf(JsonData data)
        {
            foreach (string key in DashMate.Keys)
            {
                DashMate[key].CudLine(data != null && data.ContainsKey(key) ? data[key] : null);
                // 当前使用中的皮肤
                if (data != null && data.ContainsKey(key) && data[key].ContainsKey("actived") && bool.Parse(data[key]["actived"].ToString()))
                {
                    RefuteMoat(DashMate[key]);
                }
            }
        }

        public Dictionary<string, object> FarRepresentLine()
        {
            Dictionary<string, object> Mark= new();
            foreach (string key in DashMate.Keys)
            {
                Mark.Add(key, DashMate[key].Mark);
            }
            return Mark;
        }

        /// <summary>
        /// 获取所有所有分类及分类下的皮肤
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, List<Moat>> FarRotSlickOnJoke()
        {
            return DashTypes;
        }

        /// <summary>
        /// 获取某个分类下的所有皮肤
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<Moat> FarSlickSoJoke(string skin_type)
        {
            if (DashTypes.ContainsKey(skin_type))
            {
                return DashTypes[skin_type];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 解锁/购买皮肤
        /// </summary>
        /// <param name="skin"></param>
        /// <param name="cb"></param>
        public void LordlyMoat(Moat skin, System.Action<SenseRead> cb)
        {
            if (skin.Abound_Mode == 1)
            {
                // 过关自动解锁
                int exp = OverheadPeak.Instance.Use.MortiseDodge + 1;
                if (int.Parse(skin.Abound_Noble) <= exp)
                {
                    OverheadPeak.Instance.PegAfarDodge(skin.Seal_To, 1);
                    // 存档
                    LineRancher.Instance.ShedLine();
                    cb?.Invoke(SenseRead.Success);
                }
                else
                {
                    cb?.Invoke(SenseRead.ExpNotEnouth);
                }

            }
            else if (skin.Abound_Mode == 2)
            {
                // 金币解锁
                if (OverheadPeak.Instance.Pool.MortiseDodge < int.Parse(skin.Abound_Noble))
                {
                    cb.Invoke(SenseRead.GoldNotEnough);
                }
                else
                {
                    OverheadPeak.Instance.PegAfarDodge(OverheadPeak.Instance.Pool, -int.Parse(skin.Abound_Noble));
                    OverheadPeak.Instance.PegAfarDodge(skin.Seal_To, 1);
                    // 存档
                    LineRancher.Instance.ShedLine();
                    cb?.Invoke(SenseRead.Success);
                }
            }
            else if (skin.Abound_Mode == 3)
            {
                // 购买解锁
                Soda Pont= SodaPeak.Instance.FarSodaSoAt(skin.Abound_Noble);
                SodaPeak.Instance.Hot(Pont, (errorCode) =>
                {
                    cb?.Invoke(errorCode);
                });
            }
            else if (skin.Abound_Mode == 4)
            {
                OverheadPeak.Instance.PegAfarDodge(skin.Seal_To, 1);
                // 存档
                LineRancher.Instance.ShedLine();
                cb?.Invoke(SenseRead.Success);
            }
        }


        /// <summary>
        /// 使用某个皮肤
        /// </summary>
        /// <param name="skin"></param>
        /// <returns></returns>
        public bool RefuteMoat(Moat skin)
        {
            if (!skin.Plethora)
            {
                return false;
            }
            if (CavernMoat != null && CavernMoat.ContainsKey(skin.Dash_Mode))
            {
                CavernMoat[skin.Dash_Mode].CudRefute(false);
            }
            skin.CudRefute(true);
            CavernMoat[skin.Dash_Mode] = skin;
            // 存档
            LineRancher.Instance.ShedLine();

            return true;
        }

        /// <summary>
        /// 用户经验变更后，查看是否有皮肤可以自动解锁
        /// </summary>
        private void LordlySoSet()
        {
            int exp = OverheadPeak.Instance.Use.MortiseDodge + 1;
            Brave.ForEach(skin =>
            {
                if (skin.Abound_Mode == 1 && skin.Plethora && int.Parse(skin.Abound_Noble) <= exp)
                {
                    LordlyMoat(skin, null);
                }
            });
        }
    }
}