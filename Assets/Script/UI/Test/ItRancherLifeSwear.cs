using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItRancherLifeSwear : SnowUIPlace
{
[UnityEngine.Serialization.FormerlySerializedAs("LastPlayTimeCounterText")]    public Text BulkHaulMoldShudderMoss;
[UnityEngine.Serialization.FormerlySerializedAs("Counter101Text")]    public Text Shudder101Moss;
[UnityEngine.Serialization.FormerlySerializedAs("Counter102Text")]    public Text Shudder102Moss;
[UnityEngine.Serialization.FormerlySerializedAs("Counter103Text")]    public Text Shudder103Moss;
[UnityEngine.Serialization.FormerlySerializedAs("TrialNumText")]    public Text RallyLadMoss;
[UnityEngine.Serialization.FormerlySerializedAs("PlayRewardedAdButton")]    public Button HaulComputerItGibbon;
[UnityEngine.Serialization.FormerlySerializedAs("PlayInterstitialAdButton")]    public Button HaulExpansionistItGibbon;
[UnityEngine.Serialization.FormerlySerializedAs("NoThanksButton")]    public Button NoCoronaGibbon;
[UnityEngine.Serialization.FormerlySerializedAs("TrialNumButton")]    public Button RallyLadGibbon;
[UnityEngine.Serialization.FormerlySerializedAs("CloseButton")]    public Button FirstGibbon;
[UnityEngine.Serialization.FormerlySerializedAs("TimeInterstitialText")]    public Text MoldExpansionistMoss;
[UnityEngine.Serialization.FormerlySerializedAs("PauseTimeInterstitialButton")]    public Button CaputMoldExpansionistGibbon;
[UnityEngine.Serialization.FormerlySerializedAs("ResumeTimeInterstitialButton")]    public Button ReputeMoldExpansionistGibbon;

    private void Start()
    {
        InvokeRepeating(nameof(BindShudderMoss), 0, 0.5f);

        FirstGibbon.onClick.AddListener(() => {
            FirstUIMode(GetType().Name);
        });

        HaulComputerItGibbon.onClick.AddListener(() => {
            ADRancher.Befriend.LionStarveAlder((success) => { }, "10");
        });

        HaulExpansionistItGibbon.onClick.AddListener(() => {
            ADRancher.Befriend.LionExpansionistIt(1);
        });

        NoCoronaGibbon.onClick.AddListener(() => {
            ADRancher.Befriend.IfCoronaPegWaist();
        });

        RallyLadGibbon.onClick.AddListener(() => {
            ADRancher.Befriend.SolderRallyLad(ShedLineRancher.FarWit(CAdjoin.Or_It_First_Toy) + 1);
            RallyLadMoss.text = ShedLineRancher.FarWit(CAdjoin.Or_It_First_Toy).ToString();
        });

        CaputMoldExpansionistGibbon.onClick.AddListener(() => {
            ADRancher.Befriend.CaputMoldExpansionist();
            BindCaputMoldExpansionist();
        });

        ReputeMoldExpansionistGibbon.onClick.AddListener(() => {
            ADRancher.Befriend.ReputeMoldExpansionist();
            BindCaputMoldExpansionist();
        });

    }

    public override void Display(object uiFormParams)
    {
        base.Display(uiFormParams);
        RallyLadMoss.text = ShedLineRancher.FarWit(CAdjoin.Or_It_First_Toy).ToString();
        BindCaputMoldExpansionist();
    }

    private void BindShudderMoss()
    {
        BulkHaulMoldShudderMoss.text = ADRancher.Befriend.FeedHaulMoldShudder.ToString();
        Shudder101Moss.text = ADRancher.Befriend.Hominid101.ToString();
        Shudder102Moss.text = ADRancher.Befriend.Hominid102.ToString();
        Shudder103Moss.text = ADRancher.Befriend.Hominid103.ToString();
    }

    private void BindCaputMoldExpansionist()
    {
        MoldExpansionistMoss.text = ADRancher.Befriend.VagueMoldExpansionist ? "已暂停" : "未暂停";
    }
}
