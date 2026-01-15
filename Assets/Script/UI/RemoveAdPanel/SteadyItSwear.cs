using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zeta_framework;

/// <summary>
/// 去广告弹窗
/// </summary>
public class SteadyItSwear : SnowUIPlace
{
[UnityEngine.Serialization.FormerlySerializedAs("CloseButton")]    public Button FirstGibbon;
[UnityEngine.Serialization.FormerlySerializedAs("BuyButton")]    public Button HotGibbon;
    
    // Start is called before the first frame update
    void Start()
    {
        FirstGibbon.onClick.AddListener(() => {
            FirstUIMode(GetType().Name);
        });

        HotGibbon.onClick.AddListener(() => {
            ColonizeSteadyItPeak.Instance.Hot((errorCode) => {
                if (errorCode == SenseRead.Success)
                {
                    WoolSwear.Instance.BindDynamicSteamship(ColonizeScoreWhipPeak.Instance.FarStarveSoDodge(0));
                    FirstUIMode(GetType().Name);
                }
                else
                {
                    GroomRancher.FarBefriend().BindGroom(ErrorCodeMessage.FarAfrican(errorCode));
                }
            });
        });
    }

    public override void Hidding()
    {
        base.Hidding();
        //ColonizeTautPlusRancher.Instance.OpenNext();
    }
}
