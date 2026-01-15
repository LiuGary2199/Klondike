using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class RateUsManager : MonoBehaviour
{
    public static RateUsManager instance;

    public string appid;

    private void Awake()
    {
        instance = this;
    }

    //获取IOS函数声明
#if UNITY_IOS
    [DllImport("__Internal")]
    internal extern static void openRateUsUrl(string appId);
#endif

    public void OpenAPPinMarket()
    {
#if UNITY_ANDROID || UNITY_EDITOR
        Application.OpenURL("market://details?id=" + appid);
#elif UNITY_IOS
        openRateUsUrl(appid);
#endif
    }
}
