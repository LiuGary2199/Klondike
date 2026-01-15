using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zeta_framework
{
    public class SkinCtrl : ICtrl
    {
        public static SkinCtrl Instance;

        private List<Skin> skins;
        private Dictionary<string, Skin> skinDict;     // 所有皮肤，key:皮肤id
        private Dictionary<string, List<Skin>> skinTypes;  // 所有皮肤分类，key：皮肤分类
        private Dictionary<string, Skin> activeSkin;    // 当前使用的皮肤, key:皮肤分类

        /// <summary>
        /// 构造函数，初始化Excel中设置的值
        /// </summary>
        /// <param name="setting"></param>
        public SkinCtrl(JsonData setting)
        {
            if (Instance == null)
            {
                Instance = this;
            }
            skins = new();
            skinDict = new();
            skinTypes = new();
            activeSkin = new();
            if (setting != null)
            {
                skins = JsonMapper.ToObject<List<Skin>>(setting.ToJson());
                skins.ForEach(skin =>
                {
                    skinDict.Add(skin.item_id, skin);
                    // 皮肤分类
                    if (!skinTypes.ContainsKey(skin.skin_type))
                    {
                        skinTypes.Add(skin.skin_type, new());
                    }
                    skinTypes[skin.skin_type].Add(skin);
                    // 当前正在使用的皮肤，默认使用第一个
                    if (!activeSkin.ContainsKey(skin.skin_type))
                    {
                        activeSkin.Add(skin.skin_type, skin);
                    }
                });
            }

            // 向资源管理器中注册经验变更回调事件
            MessageCenterLogic.GetInstance().Register(CConfig.mg_ItemChange_ + ResourceCtrl.Instance.exp.id, (md) =>
            {
                UnlockByExp();
            });
        }

        /// <summary>
        /// 初始化存档数据
        /// </summary>
        /// <param name="data"></param>
        public void Init(JsonData data)
        {
            foreach (string key in skinDict.Keys)
            {
                skinDict[key].SetData(data != null && data.ContainsKey(key) ? data[key] : null);
                // 当前使用中的皮肤
                if (data != null && data.ContainsKey(key) && data[key].ContainsKey("actived") && bool.Parse(data[key]["actived"].ToString()))
                {
                    ActiveSkin(skinDict[key]);
                }
            }
        }

        public Dictionary<string, object> GetSerializeData()
        {
            Dictionary<string, object> data = new();
            foreach (string key in skinDict.Keys)
            {
                data.Add(key, skinDict[key].data);
            }
            return data;
        }

        /// <summary>
        /// 获取所有所有分类及分类下的皮肤
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, List<Skin>> GetAllSkinsOfType()
        {
            return skinTypes;
        }

        /// <summary>
        /// 获取某个分类下的所有皮肤
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<Skin> GetSkinsByType(string skin_type)
        {
            if (skinTypes.ContainsKey(skin_type))
            {
                return skinTypes[skin_type];
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
        public void UnlockSkin(Skin skin, System.Action<ErrorCode> cb)
        {
            if (skin.unlock_type == 1)
            {
                // 过关自动解锁
                int exp = ResourceCtrl.Instance.exp.currentValue + 1;
                if (int.Parse(skin.unlock_value) <= exp)
                {
                    ResourceCtrl.Instance.AddItemValue(skin.item_id, 1);
                    // 存档
                    DataManager.Instance.SaveData();
                    cb?.Invoke(ErrorCode.Success);
                }
                else
                {
                    cb?.Invoke(ErrorCode.ExpNotEnouth);
                }

            }
            else if (skin.unlock_type == 2)
            {
                // 金币解锁
                if (ResourceCtrl.Instance.gold.currentValue < int.Parse(skin.unlock_value))
                {
                    cb.Invoke(ErrorCode.GoldNotEnough);
                }
                else
                {
                    ResourceCtrl.Instance.AddItemValue(ResourceCtrl.Instance.gold, -int.Parse(skin.unlock_value));
                    ResourceCtrl.Instance.AddItemValue(skin.item_id, 1);
                    // 存档
                    DataManager.Instance.SaveData();
                    cb?.Invoke(ErrorCode.Success);
                }
            }
            else if (skin.unlock_type == 3)
            {
                // 购买解锁
                Shop shop = ShopCtrl.Instance.GetShopById(skin.unlock_value);
                ShopCtrl.Instance.Buy(shop, (errorCode) =>
                {
                    cb?.Invoke(errorCode);
                });
            }
            else if (skin.unlock_type == 4)
            {
                ResourceCtrl.Instance.AddItemValue(skin.item_id, 1);
                // 存档
                DataManager.Instance.SaveData();
                cb?.Invoke(ErrorCode.Success);
            }
        }


        /// <summary>
        /// 使用某个皮肤
        /// </summary>
        /// <param name="skin"></param>
        /// <returns></returns>
        public bool ActiveSkin(Skin skin)
        {
            if (!skin.unlocked)
            {
                return false;
            }
            if (activeSkin != null && activeSkin.ContainsKey(skin.skin_type))
            {
                activeSkin[skin.skin_type].SetActive(false);
            }
            skin.SetActive(true);
            activeSkin[skin.skin_type] = skin;
            // 存档
            DataManager.Instance.SaveData();

            return true;
        }

        /// <summary>
        /// 用户经验变更后，查看是否有皮肤可以自动解锁
        /// </summary>
        private void UnlockByExp()
        {
            int exp = ResourceCtrl.Instance.exp.currentValue + 1;
            skins.ForEach(skin =>
            {
                if (skin.unlock_type == 1 && skin.unlocked && int.Parse(skin.unlock_value) <= exp)
                {
                    UnlockSkin(skin, null);
                }
            });
        }
    }
}