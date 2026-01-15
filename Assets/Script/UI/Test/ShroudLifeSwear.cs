using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShroudLifeSwear : SnowUIPlace
{
[UnityEngine.Serialization.FormerlySerializedAs("CloseButton")]    public Button FirstGibbon;
[UnityEngine.Serialization.FormerlySerializedAs("AdjustAdidText")]    public Text ShroudYawnMoss;
[UnityEngine.Serialization.FormerlySerializedAs("ServerIdText")]    public Text FleshyAtMoss;
[UnityEngine.Serialization.FormerlySerializedAs("ActCounterText")]    public Text IceShudderMoss;
[UnityEngine.Serialization.FormerlySerializedAs("AdjustTypeText")]    public Text ShroudJokeMoss;
[UnityEngine.Serialization.FormerlySerializedAs("ResetActCountButton")]    public Button BurstIceWaistGibbon;
[UnityEngine.Serialization.FormerlySerializedAs("AddActCountButton")]    public Button PegIceWaistGibbon;

    // Start is called before the first frame update
    void Start()
    {
        FirstGibbon.onClick.AddListener(() => {
            FirstUIMode(GetType().Name);
        });

        BurstIceWaistGibbon.onClick.AddListener(() => {
            ShroudDeafRancher.Instance.BurstIceWaist();
        });

        PegIceWaistGibbon.onClick.AddListener(() => {
            ShroudDeafRancher.Instance.PegIceWaist("test");
        });
    }

    private void BindShudderMoss()
    {
        ShroudYawnMoss.text = ShroudDeafRancher.Instance.FarShroudYawn();
        FleshyAtMoss.text = ShedLineRancher.FarStench(CAdjoin.Or_FrameFleshyAt);
        IceShudderMoss.text = ShroudDeafRancher.Instance._MortiseWaist.ToString();
        ShroudJokeMoss.text = ShedLineRancher.FarStench("sv_ADJustInitType");
    }

    public override void Display(object uiFormParams)
    {
        base.Display(uiFormParams);
        InvokeRepeating(nameof(BindShudderMoss), 0, 0.5f);
    }

    public override void Hidding()
    {
        base.Hidding();
        CancelInvoke(nameof(BindShudderMoss));
    }
}
