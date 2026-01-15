using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonUtil
{
    [HideInInspector] public static string Adjust_TrackerName; //归因渠道名称 由NetInfoMgr的CheckAdjustNetwork方法赋值
    static string Save_AP; //ApplePie的本地存档 存储第一次进入状态 未来不再受ApplePie开关影响
    static string NormalModeName = "pie"; //正常模式名称
    static string Distances; //距离黑名单位置的距离 打点用
    static string Reason; //进审理由 打点用
    [HideInInspector] public static string StepLog = ""; //判断流程 打点用

    public static bool IsApple()
    {
        //测试
        //return true;

        if (PlayerPrefs.HasKey("Save_AP"))  //优先使用本地存档
            Save_AP = PlayerPrefs.GetString("Save_AP");
        if (string.IsNullOrEmpty(Save_AP)) //无本地存档 读取网络数据
            CheckOnlineData();

        if (Save_AP != "P")
            return true;
        else
            return false;
    }
    static void CheckOnlineData() //读取网络数据 判断进入哪种游戏模式
    {
        string OtherChance = "NO"; //进审之后 是否还有可能变正常
        Save_AP = "P";
        if (NetInfoMgr.instance.ConfigData.apple_pie != NormalModeName) //审模式 
        {
            OtherChance = "YES";
            Save_AP = "A";
            if (string.IsNullOrEmpty(Reason))
                Reason = "ApplePie";
        }
        StepLog = "0:" + Save_AP;
        //判断地理位置
        if (!string.IsNullOrEmpty(NetInfoMgr.instance.ConfigData.LocationList) && NetInfoMgr.instance.UserData != null)
        {
            //判断运营商信息
            if (NetInfoMgr.instance.UserData.IsHaveApple)
            {
                Save_AP = "A";
                if (string.IsNullOrEmpty(Reason))
                    Reason = "HaveApple";
            }
            StepLog += "  1:" + Save_AP;
            //判断经纬度
            LocationData[] LocationDatas = LitJson.JsonMapper.ToObject<LocationData[]>(NetInfoMgr.instance.ConfigData.LocationList);
            if (LocationDatas != null && LocationDatas.Length > 0 && NetInfoMgr.instance.UserData.lat != 0 && NetInfoMgr.instance.UserData.lon != 0)
            {
                for (int i = 0; i < LocationDatas.Length; i++)
                {
                    float Distance = GetDistance((float)LocationDatas[i].X, (float)LocationDatas[i].Y,
                    (float)NetInfoMgr.instance.UserData.lat, (float)NetInfoMgr.instance.UserData.lon);
                    Distances += Distance.ToString() + ",";
                    if (Distance <= LocationDatas[i].Radius)
                    {
                        Save_AP = "A";
                        if (string.IsNullOrEmpty(Reason))
                            Reason = "Location";
                        break;
                    }
                }
            }
            StepLog += "  2:" + Save_AP;
            //判断城市
            string[] HeiCityList = LitJson.JsonMapper.ToObject<string[]>(NetInfoMgr.instance.ConfigData.HeiCity);
            if (!string.IsNullOrEmpty(NetInfoMgr.instance.UserData.regionName) && HeiCityList != null && HeiCityList.Length > 0)
            {
                for (int i = 0; i < HeiCityList.Length; i++)
                {
                    if (HeiCityList[i] == NetInfoMgr.instance.UserData.regionName
                    || HeiCityList[i] == NetInfoMgr.instance.UserData.city)
                    {
                        Save_AP = "A";
                        if (string.IsNullOrEmpty(Reason))
                            Reason = "City";
                        break;
                    }
                }
            }
            StepLog += "  3:" + Save_AP;
            //判断黑名单
            string[] HeiIPs = LitJson.JsonMapper.ToObject<string[]>(NetInfoMgr.instance.ConfigData.HeiNameList);
            if (HeiIPs != null && HeiIPs.Length > 0 && !string.IsNullOrEmpty(NetInfoMgr.instance.UserData.query))
            {
                string[] IpNums = NetInfoMgr.instance.UserData.query.Split('.');
                for (int i = 0; i < HeiIPs.Length; i++)
                {
                    string[] HeiIpNums = HeiIPs[i].Split('.');
                    bool isMatch = true;
                    for (int j = 0; j < HeiIpNums.Length; j++) //黑名单IP格式可能是任意位数 根据位数逐个比对
                    {
                        if (HeiIpNums[j] != IpNums[j])
                            isMatch = false;
                    }
                    if (isMatch)
                    {
                        Save_AP = "A";
                        if (string.IsNullOrEmpty(Reason))
                            Reason = "IP";
                        break;
                    }
                }
            }
            StepLog += "  4:" + Save_AP;
        }
        StepLog += "  5:" + Save_AP;
        //判断自然量
        if (!string.IsNullOrEmpty(NetInfoMgr.instance.ConfigData.fall_down))
        {
            if (NetInfoMgr.instance.ConfigData.fall_down == "bottom") //仅判断Organic
            {
                if (Adjust_TrackerName == "Organic") //打开自然量 且 归因渠道是Organic 审模式
                {
                    Save_AP = "A";
                    if (string.IsNullOrEmpty(Reason))
                        Reason = "FallDown";
                }
            }
            else if (NetInfoMgr.instance.ConfigData.fall_down == "down") //判断Organic + NoUserConsent
            {
                if (Adjust_TrackerName == "Organic" || Adjust_TrackerName == "No User Consent") //打开自然量 且 归因渠道是Organic或NoUserConsent 审模式
                {
                    Save_AP = "A";
                    if (string.IsNullOrEmpty(Reason))
                        Reason = "FallDown";
                }
            }
        }
        StepLog += "  6:" + Save_AP;

        PlayerPrefs.SetString("Save_AP", Save_AP);
        PlayerPrefs.SetString("OtherChance", OtherChance);
        //打点
        if (!string.IsNullOrEmpty(SaveDataManager.GetString(CConfig.sv_LocalServerId)))
            SendEvent();


        //本地log
        Debug.Log("++++++判断流程： " + StepLog);
        if (NetInfoMgr.instance.UserData != null)
        {
            string Info = "游戏模式：" + (Save_AP == "A" ? "审" : "正常")
                       + "\n进审理由：" + Reason
                       + "\n开关： " + NetInfoMgr.instance.ConfigData.apple_pie
                       + "\n是否包含苹果： " + NetInfoMgr.instance.UserData.IsHaveApple
                       + "\n位置黑名单： " + NetInfoMgr.instance.ConfigData.LocationList?.Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace(" ", "").Replace("[", "").Replace("]", "")
                       + "\n用户位置： " + NetInfoMgr.instance.UserData.lat + "," + NetInfoMgr.instance.UserData.lon
                       + "\n黑名单城市： " + NetInfoMgr.instance.ConfigData.HeiCity?.Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace(" ", "").Replace("[", "").Replace("]", "")
                       + "\n用户城市: " + NetInfoMgr.instance.UserData.regionName
                       + "\n与黑名单地点距离： " + Distances
                       + "\nIP黑名单： " + NetInfoMgr.instance.ConfigData.HeiNameList?.Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace(" ", "").Replace("[", "").Replace("]", "")
                       + "\n用户IP： " + NetInfoMgr.instance.UserData.query
                       + "\n自然量开关： " + NetInfoMgr.instance.ConfigData.fall_down
                       + "\n归因渠道： " + Adjust_TrackerName
                       + "\n接口返回信息： " + NetInfoMgr.instance.UserDataStr;
            Debug.Log("++++++" + Info);
        }
    }
    static float GetDistance(float lat1, float lon1, float lat2, float lon2)
    {
        const float R = 6371f; // 地球半径，单位：公里
        float latDistance = Mathf.Deg2Rad * (lat2 - lat1);
        float lonDistance = Mathf.Deg2Rad * (lon2 - lon1);
        float a = Mathf.Sin(latDistance / 2) * Mathf.Sin(latDistance / 2)
               + Mathf.Cos(Mathf.Deg2Rad * lat1) * Mathf.Cos(Mathf.Deg2Rad * lat2)
               * Mathf.Sin(lonDistance / 2) * Mathf.Sin(lonDistance / 2);
        float c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
        return R * c * 1000; // 距离，单位：米
    }

    public static void SendEvent()
    {
        //打点
        if (NetInfoMgr.instance.UserData != null)
        {
            string Info1 = "[" + (Save_AP == "A" ? "审" : "正常")
                                       + "] [" + Reason + "]";
            string Info2 = "[" + NetInfoMgr.instance.UserData.lat + "," + NetInfoMgr.instance.UserData.lon
                           + "] [" + NetInfoMgr.instance.UserData.regionName
                           + "] [" + Distances + "]";
            string Info3 = "[" + NetInfoMgr.instance.UserData.query
                           + "] [" + Adjust_TrackerName + "]";
            PostEventScript.GetInstance().SendEvent("3000", Info1, Info2, Info3);
        }
        else
            PostEventScript.GetInstance().SendEvent("3000", "No UserData");
        PostEventScript.GetInstance().SendEvent("3001", (Save_AP == "A" ? "审" : "正常"), StepLog, NetInfoMgr.instance.DataFrom);
        PlayerPrefs.SetInt("SendedEvent", 1);
    }



    public static bool IsEditor()
    {
#if UNITY_EDITOR
        return true;
#else
        return false;
#endif
    }

    /// <summary>
    /// 是否为竖屏
    /// </summary>
    /// <returns></returns>
    public static bool IsPortrait()
    {
        return Screen.height > Screen.width;
    }

    /// <summary>
    /// UI的本地坐标转为屏幕坐标
    /// </summary>
    /// <param name="tf"></param>
    /// <returns></returns>
    public static Vector2 LocalPoint2ScreenPoint(RectTransform tf)
    {
        if (tf == null)
        {
            return Vector2.zero;
        }

        Vector2 fromPivotDerivedOffset = new Vector2(tf.rect.width * 0.5f + tf.rect.xMin, tf.rect.height * 0.5f + tf.rect.yMin);
        Vector2 screenP = RectTransformUtility.WorldToScreenPoint(null, tf.position);
        screenP += fromPivotDerivedOffset;
        return screenP;
    }


    /// <summary>
    /// UI的屏幕坐标，转为本地坐标
    /// </summary>
    /// <param name="tf"></param>
    /// <param name="startPos"></param>
    /// <returns></returns>
    public static Vector2 ScreenPoint2LocalPoint(RectTransform tf, Vector2 startPos)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(tf, startPos, null, out localPoint);
        Vector2 pivotDerivedOffset = new Vector2(tf.rect.width * 0.5f + tf.rect.xMin, tf.rect.height * 0.5f + tf.rect.yMin);
        return tf.anchoredPosition + localPoint - pivotDerivedOffset;
    }

    public static Vector2 GetWorldPositionOfRectTransform(RectTransform rectTransform)
    {
        // 从RectTransform开始，逐级向上遍历父级
        Vector2 worldPosition = rectTransform.anchoredPosition;
        for (RectTransform rt = rectTransform; rt != null; rt = rt.parent as RectTransform)
        {
            worldPosition += new Vector2(rt.localPosition.x, rt.localPosition.y);
            worldPosition += rt.pivot * rt.sizeDelta;

            // 考虑到UI元素的缩放
            worldPosition *= rt.localScale;

            // 如果父级不是Canvas，则停止遍历
            if (rt.parent != null && rt.parent.GetComponent<Canvas>() == null)
                break;
        }

        // 将结果从本地坐标系转换为世界坐标系
        return rectTransform.root.TransformPoint(worldPosition);
    }
}
