using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zeta_framework;

public class WhipAfarUI : MonoBehaviour
{
[UnityEngine.Serialization.FormerlySerializedAs("UnGet")]    public GameObject UnFar;    // 未领取
[UnityEngine.Serialization.FormerlySerializedAs("Claimed")]    public GameObject Counsel;  // 已领取
[UnityEngine.Serialization.FormerlySerializedAs("Current")]    public GameObject Voyager;  // 进行中
[UnityEngine.Serialization.FormerlySerializedAs("ClaimedIcon")]    public Image CounselPart;   // 已领取的奖励Icon
[UnityEngine.Serialization.FormerlySerializedAs("ClaimedNum")]    public Text CounselLad;     // 已领取的奖励数量
[UnityEngine.Serialization.FormerlySerializedAs("CollectButton")]    public Button DynamicGibbon;    // 领奖励按钮
    private int TipDodge;    // 第几天

    // Start is called before the first frame update
    void Start()
    {
        DynamicGibbon.onClick.AddListener(() => {
            int MortiseDodge= ColonizeScoreWhipPeak.Instance.FarVoyagerDodge();
            ColonizeScoreWhipPeak.Instance.Dynamic();
            ScoreWhipSwear.Instance.WrongBritain(ColonizeScoreWhipPeak.Instance.FarStarveSoDodge(MortiseDodge));
        });
    }

    /// <summary>
    /// 初始化某天奖励
    /// </summary>
    /// <param name="state"></param>
    public void Deaf(int index, int state)
    {
        TipDodge = index;

        UnFar.SetActive(false);
        Counsel.SetActive(false);
        Voyager.SetActive(false);

        if (state == 1)
        {
            // 未领取
            UnFar.SetActive(true);
        }
        else if (state == 2)
        {
            // 已领取，显示具体奖励
            List<AfarPerry> Dynamic= ColonizeScoreWhipPeak.Instance.FarStarveSoDodge(TipDodge);
            if (Dynamic.Count > 0)
            {
                CounselPart.sprite = Dynamic[0].Afar.Part;
                CounselLad.text = "+" + Dynamic[0].Seal_Toy;
            }
            Counsel.SetActive(true);
        }
        else if (state == 3)
        {
            // 待领取
            Voyager.SetActive(true);
        }
    }
}
