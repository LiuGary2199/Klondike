using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnfoldGate
{
    [HideInInspector] public static string Shroud_DistendStew; //归因渠道名称 由ToeBoldLeg的CheckAdjustNetwork方法赋值
    static string Shed_AP; //ApplePie的本地存档 存储第一次进入状态 未来不再受ApplePie开关影响
    static string RecessBeltStew = "pie"; //正常模式名称
    static string Trumpeter; //距离黑名单位置的距离 打点用
    static string Tethys; //进审理由 打点用
    [HideInInspector] public static string BoatWax = ""; //判断流程 打点用

    public static bool IsApple()
    {
        return ToeBoldLeg.instance.AdjoinLine.apple_pie != RecessBeltStew;
    }

    public static bool UpMound()
    {
        //测试
        //return false;

        if (PlayerPrefs.HasKey("Shed_AP"))  //优先使用本地存档
            Shed_AP = PlayerPrefs.GetString("Shed_AP");
        if (string.IsNullOrEmpty(Shed_AP)) //无本地存档 读取网络数据
            CigarLagoonLine();

        if (Shed_AP != "P")
            return true;
        else
            return false;
    }
    public static void CigarLagoonLine() //读取网络数据 判断进入哪种游戏模式
    {
        string OtherChance = "NO"; //进审之后 是否还有可能变正常
        Shed_AP = "P";
        if (ToeBoldLeg.instance.AdjoinLine.apple_pie != RecessBeltStew) //审模式 
        {
            OtherChance = "YES";
            Shed_AP = "A";
            if (string.IsNullOrEmpty(Tethys))
                Tethys = "ApplePie";
        }
        BoatWax = "0:" + Shed_AP;
        //判断运营商信息
        if (ToeBoldLeg.instance.UserData != null && ToeBoldLeg.instance.UserData.IsHaveApple)
        {
            Shed_AP = "A";
            if (string.IsNullOrEmpty(Tethys))
                Tethys = "HaveApple";
            BoatWax += "1:" + Shed_AP;
        }
        if (ToeBoldLeg.instance.BlockRule != null)
        {
            //判断经纬度
            LocationData[] LocationDatas = ToeBoldLeg.instance.BlockRule.LocationList;
            if (LocationDatas != null && LocationDatas.Length > 0 && ToeBoldLeg.instance.UserData != null && ToeBoldLeg.instance.UserData.lat != 0 && ToeBoldLeg.instance.UserData.lon != 0)
            {
                for (int i = 0; i < LocationDatas.Length; i++)
                {
                    float Distance = GetDistance((float)LocationDatas[i].X, (float)LocationDatas[i].Y,
                    (float)ToeBoldLeg.instance.UserData.lat, (float)ToeBoldLeg.instance.UserData.lon);
                    Trumpeter += Distance.ToString() + ",";
                    if (Distance <= LocationDatas[i].Radius)
                    {
                        Shed_AP = "A";
                        if (string.IsNullOrEmpty(Tethys))
                            Tethys = "Location";
                        break;
                    }
                }
            }
            BoatWax += "2:" + Shed_AP;
            //判断城市
            string[] HeiCityList = ToeBoldLeg.instance.BlockRule.CityList;
            if (!string.IsNullOrEmpty(ToeBoldLeg.instance.UserData.regionName) && HeiCityList != null && HeiCityList.Length > 0)
            {
                for (int i = 0; i < HeiCityList.Length; i++)
                {
                    if (HeiCityList[i] == ToeBoldLeg.instance.UserData.regionName
                    || HeiCityList[i] == ToeBoldLeg.instance.UserData.city)
                    {
                        Shed_AP = "A";
                        if (string.IsNullOrEmpty(Tethys))
                            Tethys = "City";
                        break;
                    }
                }
            }
            BoatWax += "3:" + Shed_AP;
            //判断黑名单
            string[] HeiIPs = ToeBoldLeg.instance.BlockRule.IPList;
            if (HeiIPs != null && HeiIPs.Length > 0 && !string.IsNullOrEmpty(ToeBoldLeg.instance.UserData.query))
            {
                string[] IpNums = ToeBoldLeg.instance.UserData.query.Split('.');
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
                        Shed_AP = "A";
                        if (string.IsNullOrEmpty(Tethys))
                            Tethys = "IP";
                        break;
                    }
                }
            }
            BoatWax += "4:" + Shed_AP;
        }
        //判断自然量
        if (!string.IsNullOrEmpty(ToeBoldLeg.instance.BlockRule.fall_down))
        {
            // if (ToeBoldLeg.instance.BlockRule.fall_down == "bottom") //仅判断Organic
            // {
            //     if (Adjust_TrackerName == "Organic") //打开自然量 且 归因渠道是Organic 审模式
            //     {
            //         Shed_AP = "A";
            //         if (string.IsNullOrEmpty(Tethys))
            //             Tethys = "FallDown";
            //     }
            // }
            // else if (ToeBoldLeg.instance.BlockRule.fall_down == "down") //判断Organic + NoUserConsent
            // {
            //     if (Adjust_TrackerName == "Organic" || Adjust_TrackerName == "No User Consent") //打开自然量 且 归因渠道是Organic或NoUserConsent 审模式
            //     {
            //         Shed_AP = "A";
            //         if (string.IsNullOrEmpty(Tethys))
            //             Tethys = "FallDown";
            //     }
            // }
        }
        BoatWax += "5:" + Shed_AP;

        //安卓平台特殊屏蔽策略
        if (Application.platform == RuntimePlatform.Android && ToeBoldLeg.instance.BlockRule != null)
        {
            AndroidJavaClass aj = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject p = aj.GetStatic<AndroidJavaObject>("currentActivity");

            //判断是否使用VPN
            if (ToeBoldLeg.instance.BlockRule.BlockVPN)
            {
                bool isVpnConnected = p.CallStatic<bool>("isVpn");
                if (isVpnConnected)
                {
                    Shed_AP = "A";
                    if (string.IsNullOrEmpty(Tethys))
                        Tethys = "VPN";
                }
            }
            BoatWax += "6:" + Shed_AP;

            //是否使用模拟器
            if (ToeBoldLeg.instance.BlockRule.BlockSimulator)
            {
                bool isSimulator = p.CallStatic<bool>("isSimulator");
                if (isSimulator)
                {
                    Shed_AP = "A";
                    if (string.IsNullOrEmpty(Tethys))
                        Tethys = "Simulator";
                }
            }
            BoatWax += "7:" + Shed_AP;
            //是否root
            if (ToeBoldLeg.instance.BlockRule.BlockRoot)
            {
                bool isRoot = p.CallStatic<bool>("isRoot");
                if (isRoot)
                {
                    Shed_AP = "A";
                    if (string.IsNullOrEmpty(Tethys))
                        Tethys = "Root";
                }
            }
            BoatWax += "8:" + Shed_AP;
            //是否使用开发者模式
            if (ToeBoldLeg.instance.BlockRule.BlockDeveloper)
            {
                bool isDeveloper = p.CallStatic<bool>("isDeveloper");
                if (isDeveloper)
                {
                    Shed_AP = "A";
                    if (string.IsNullOrEmpty(Tethys))
                        Tethys = "Developer";
                }
            }
            BoatWax += "9:" + Shed_AP;

            //是否使用USB调试
            if (ToeBoldLeg.instance.BlockRule.BlockUsb)
            {
                bool isUsb = p.CallStatic<bool>("isUsb");
                if (isUsb)
                {
                    Shed_AP = "A";
                    if (string.IsNullOrEmpty(Tethys))
                        Tethys = "UsbDebug";
                }
            }
            BoatWax += "10:" + Shed_AP;

            //是否使用sim卡
            if (ToeBoldLeg.instance.BlockRule.BlockSimCard)
            {
                bool isSimCard = p.CallStatic<bool>("isSimcard");
                if (!isSimCard)
                {
                    Shed_AP = "A";
                    if (string.IsNullOrEmpty(Tethys))
                        Tethys = "SimCard";
                }
            }
            BoatWax += "11:" + Shed_AP;
        }
        PlayerPrefs.SetString("Shed_AP", Shed_AP);
        PlayerPrefs.SetString("OtherChance", OtherChance);

        //打点
        if (!string.IsNullOrEmpty(ShedLineRancher.FarStench(CAdjoin.Or_FrameFleshyAt)))
            LeafInuit();
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

    public static void LeafInuit()
    {
        //打点
        if (ToeBoldLeg.instance.UserData != null)
        {
            string Info1 = "[" + (Shed_AP == "A" ? "审" : "正常") + "] [" + Tethys + "]";
            string Info2 = "[" + ToeBoldLeg.instance.UserData.lat + "," + ToeBoldLeg.instance.UserData.lon + "] [" + ToeBoldLeg.instance.UserData.regionName + "] [" + Trumpeter + "]";
            string Info3 = "[" + ToeBoldLeg.instance.UserData.query + "] [Null]";  // [" + Adjust_TrackerName + "]";
            WhimInuitRemove.FarBefriend().LeafInuit("3000", Info1, Info2, Info3);
        }
        else
            WhimInuitRemove.FarBefriend().LeafInuit("3000", "No UserData");
        WhimInuitRemove.FarBefriend().LeafInuit("3001", (Shed_AP == "A" ? "审" : "正常"), BoatWax, ToeBoldLeg.instance.LineDuke);
        PlayerPrefs.SetInt("SendedEvent", 1);
    }

    public static string AndroidBlockCheck()
    {
        if (ToeBoldLeg.instance.BlockRule != null)
        {
            AndroidJavaClass aj = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject p = aj.GetStatic<AndroidJavaObject>("currentActivity");
            if (ToeBoldLeg.instance.BlockRule.BlockVPN)
            {
                bool isVpnConnected = p.CallStatic<bool>("isVpn");
                if (isVpnConnected)
                    return "Vpn";
            }
            if (ToeBoldLeg.instance.BlockRule.BlockDeveloper)
            {
                bool isDeveloper = p.CallStatic<bool>("isDeveloper");
                if (isDeveloper)
                    return "Developer";
            }
            if (ToeBoldLeg.instance.BlockRule.BlockUsb)
            {
                bool isUsb = p.CallStatic<bool>("isUsb");
                if (isUsb)
                    return "Usb";
            }
            if (ToeBoldLeg.instance.BlockRule.BlockSimCard)
            {
                bool isSimCard = p.CallStatic<bool>("isSimcard");
                if (!isSimCard)
                    return "SimCard";
            }
        }
        return "";
    }



    public static bool UpSchist()
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
    public static bool UpCivility()
    {
        return Screen.height > Screen.width;
    }

    /// <summary>
    /// UI的本地坐标转为屏幕坐标
    /// </summary>
    /// <param name="tf"></param>
    /// <returns></returns>
    public static Vector2 FramePerry2PlightPerry(RectTransform tf)
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
    public static Vector2 PlightPerry2FramePerry(RectTransform tf, Vector2 startPos)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(tf, startPos, null, out localPoint);
        Vector2 pivotDerivedOffset = new Vector2(tf.rect.width * 0.5f + tf.rect.xMin, tf.rect.height * 0.5f + tf.rect.yMin);
        return tf.anchoredPosition + localPoint - pivotDerivedOffset;
    }

    public static Vector2 FarNicheCitationOfSoulSkeptical(RectTransform rectTransform)
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
