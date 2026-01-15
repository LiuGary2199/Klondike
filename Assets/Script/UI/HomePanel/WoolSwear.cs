using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zeta_framework;

public class WoolSwear : SnowUIPlace
{
    public static WoolSwear Instance;
[UnityEngine.Serialization.FormerlySerializedAs("GoldBar")]
    public GameObject CastMan;
[UnityEngine.Serialization.FormerlySerializedAs("DiamondBar")]    public GameObject MagentaMan;
[UnityEngine.Serialization.FormerlySerializedAs("HealthBar")]    public GameObject WanderMan;
[UnityEngine.Serialization.FormerlySerializedAs("PlayButton")]    public Button HaulGibbon;


    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        HaulGibbon.onClick.AddListener(() => {
            SenseRead errorCode = DodgePeak.Instance.BadgeDodge();
            if (errorCode != SenseRead.Success)
            {
                GroomRancher.FarBefriend().BindGroom(ErrorCodeMessage.FarAfrican(errorCode));
            }
        });
    }

    /// <summary>
    /// 展示资源收集动画
    /// </summary>
    /// <param name="items"></param>
    public void BindDynamicSteamship(Action cb = null)
    {
        if (!gameObject.activeInHierarchy)
        {
            // 如果是用UIManager的ShowUIForms打开的HomePanel，请修改下面的打开方式
            //KickSwear.Instance.OpenTab("Home");
        }

        DynamicRancher.Instance.HaulDynamicSteamship(cb);
    }

    public void BindDynamicSteamship(List<AfarPerry> list, Action cb = null) {
        if (!gameObject.activeInHierarchy)
        {
            // 如果是用UIManager的ShowUIForms打开的HomePanel，请修改下面的打开方式
            //KickSwear.Instance.OpenTab("Home");
        }

        DynamicRancher.Instance.HaulDynamicSteamship(list, cb);
    }

}
