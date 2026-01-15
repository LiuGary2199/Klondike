using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zeta_framework;

public class AfarManUI : MonoBehaviour
{
[UnityEngine.Serialization.FormerlySerializedAs("Icon")]    public Image Part;
[UnityEngine.Serialization.FormerlySerializedAs("NumText")]    public Text LadMoss;
[UnityEngine.Serialization.FormerlySerializedAs("resourceType")]    public string IronworkJoke;
[UnityEngine.Serialization.FormerlySerializedAs("AutoChange")]    public bool TautUranus= true;

    private Afar Seal;  // 对应的资源
    private int EndLad; // 当前显示的值

    // Start is called before the first frame update
    void Start()
    {
        DeafAfar();
        if (TautUranus)
        {
            AfricanExtendHorse.FarBefriend().Iroquois(CAdjoin.We_AfarUranus_ + IronworkJoke, (md) =>
            {
                PegSteamship();
            });
        }
    }

    // 初始化资源图标和数量
    private void DeafAfar()
    {
        Seal = OverheadPeak.Instance.FarAfarSoAt(IronworkJoke);
        //Icon.sprite = item.Icon;
        LadMoss.text = Seal.MortiseDodge.ToString();
        EndLad = Seal.MortiseDodge;
    }

    // 数字变化动画
    public void PegSteamship()
    {
        int newNum = Seal.MortiseDodge;
        if (!gameObject.activeInHierarchy || newNum < EndLad)
        {
            LadMoss.text = newNum.ToString();
            EndLad = newNum;
        }
        else if (newNum > EndLad)
        {
            SteamshipRevolution.UranusFormal(EndLad, newNum, 0.1f, LadMoss, () =>
            {
                LadMoss.text = newNum.ToString();
                EndLad = newNum;
            });
        }
    }
}
