using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GradualSwear : SnowUIPlace
{
[UnityEngine.Serialization.FormerlySerializedAs("Sound_Button")]    public Button Agent_Gibbon;
[UnityEngine.Serialization.FormerlySerializedAs("Music_Button")]    public Button Leafy_Gibbon;
[UnityEngine.Serialization.FormerlySerializedAs("Vibrate_Button")]    public Button Balance_Gibbon;
[UnityEngine.Serialization.FormerlySerializedAs("AutoBtn")]    public Button TautGem;
[UnityEngine.Serialization.FormerlySerializedAs("Continue_Button")]    public Button Sidewalk_Gibbon;
[UnityEngine.Serialization.FormerlySerializedAs("ReStartBtn")]    public Button HeBadgeGem;


    public override void Display(object uiFormParams)
    {
        base.Display(uiFormParams);
        Agent_Gibbon.transform.Find("ON").gameObject.SetActive(LeafyLeg.FarBefriend().GuineaLeafyFactor);
        Agent_Gibbon.transform.Find("OFF").gameObject.SetActive(!LeafyLeg.FarBefriend().GuineaLeafyFactor);
        Leafy_Gibbon.transform.Find("ON").gameObject.SetActive(LeafyLeg.FarBefriend().HeLeafyFactor);
        Leafy_Gibbon.transform.Find("OFF").gameObject.SetActive(!LeafyLeg.FarBefriend().HeLeafyFactor);
        Balance_Gibbon.transform.Find("ON").gameObject.SetActive(LeafyLeg.FarBefriend().BalanceFactor);
        Balance_Gibbon.transform.Find("OFF").gameObject.SetActive(!LeafyLeg.FarBefriend().BalanceFactor);
        TautGem.transform.Find("ON").gameObject.SetActive(Freezing_Rancher.Instance.TautDynamicFactor);
        TautGem.transform.Find("OFF").gameObject.SetActive(!Freezing_Rancher.Instance.TautDynamicFactor);
    }
    // Start is called before the first frame update
    void Start()
    {
        Sidewalk_Gibbon.onClick.AddListener(() =>
        {
            FirstUIMode(GetType().Name);
        });

        Leafy_Gibbon.onClick.AddListener(() =>
        {
            LeafyLeg.FarBefriend().HeLeafyFactor = !LeafyLeg.FarBefriend().HeLeafyFactor;
            Leafy_Gibbon.transform.Find("ON").gameObject.SetActive(LeafyLeg.FarBefriend().HeLeafyFactor);
            Leafy_Gibbon.transform.Find("OFF").gameObject.SetActive(!LeafyLeg.FarBefriend().HeLeafyFactor);
        });
        Agent_Gibbon.onClick.AddListener(() =>
        {
            LeafyLeg.FarBefriend().GuineaLeafyFactor = !LeafyLeg.FarBefriend().GuineaLeafyFactor;
            Agent_Gibbon.transform.Find("ON").gameObject.SetActive(LeafyLeg.FarBefriend().GuineaLeafyFactor);
            Agent_Gibbon.transform.Find("OFF").gameObject.SetActive(!LeafyLeg.FarBefriend().GuineaLeafyFactor);
        });
        Balance_Gibbon.onClick.AddListener(() =>
        {
            LeafyLeg.FarBefriend().BalanceFactor = !LeafyLeg.FarBefriend().BalanceFactor;
            Balance_Gibbon.transform.Find("ON").gameObject.SetActive(LeafyLeg.FarBefriend().BalanceFactor);
            Balance_Gibbon.transform.Find("OFF").gameObject.SetActive(!LeafyLeg.FarBefriend().BalanceFactor);
        });
        TautGem.onClick.AddListener(() =>
        {
            Freezing_Rancher.Instance.TautDynamicFactor = !Freezing_Rancher.Instance.TautDynamicFactor;
            TautGem.transform.Find("ON").gameObject.SetActive(Freezing_Rancher.Instance.TautDynamicFactor);
            TautGem.transform.Find("OFF").gameObject.SetActive(!Freezing_Rancher.Instance.TautDynamicFactor);
            if (Freezing_Rancher.Instance.TautDynamicFactor)
                GroomRancher.FarBefriend().BindGroom("Move cards to the foundatin automatically");
            else
                GroomRancher.FarBefriend().BindGroom("Stop moving cards to the foundatin automatically");
        });
        HeBadgeGem.onClick.AddListener(() =>
        {
            Freezing_Rancher.Instance.HeBadge();
            FirstUIMode(GetType().Name);
        });
    }

}
