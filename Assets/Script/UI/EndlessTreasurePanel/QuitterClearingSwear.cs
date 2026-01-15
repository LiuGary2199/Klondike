using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using zeta_framework;

public class QuitterClearingSwear : SnowUIPlace
{
    public static QuitterClearingSwear Instance;
[UnityEngine.Serialization.FormerlySerializedAs("CloseBtn")]
    public Button FirstGem;
[UnityEngine.Serialization.FormerlySerializedAs("ClockText")]    public Text DiverMoss;
[UnityEngine.Serialization.FormerlySerializedAs("Container")]    public Transform Attention;
[UnityEngine.Serialization.FormerlySerializedAs("EndlessItemPrefab")]    public GameObject QuitterAfarQuiver;

    private List<GameObject> AirportAfarTrip= new();
    private int MeDodge;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        FirstGem.onClick.AddListener(() => {
            FirstUIMode(GetType().Name);
        });
    }

    public override void Display(object uiFormParams)
    {
        base.Display(uiFormParams);

        MeDodge = ColonizeQuitterClearingPeak.Instance.VoyagerDodge;

        if (AirportAfarTrip.Count == 0 )
        {
            DeafTrip();
        }
        else
        {
            BalconyTrip();
        }

        DiverMoss.text = WardGate.CarvingExceed2(ColonizeQuitterClearingPeak.Instance.RidMold - WardGate.Voyager());
    }

    private void DeafTrip()
    {
        for(int i = Attention.childCount - 1; i >= 0; i--)
        {
            Destroy(Attention.GetChild(i).gameObject);
        }

        int MortiseDodge= ColonizeQuitterClearingPeak.Instance.VoyagerDodge;
        for (int i = 0; i < ColonizeQuitterClearingPeak.Instance.AirportStatuette.Count; i++)
        {
            ActivityEndlessTreasureDB Seal= ColonizeQuitterClearingPeak.Instance.AirportStatuette[i];
            GameObject itemUI = Instantiate(QuitterAfarQuiver, Attention);
            itemUI.GetComponent<QuitterAfarUI>().PearlyAfar(Seal, i > MortiseDodge);
            AirportAfarTrip.Add(itemUI);
        }

        BalconyTrip();
    }

    private void BalconyTrip()
    {
        int MortiseDodge= ColonizeQuitterClearingPeak.Instance.VoyagerDodge;
        for (int i = 0; i < AirportAfarTrip.Count; i++)
        {
            AirportAfarTrip[i].SetActive(i >= MortiseDodge);
        }
        AirportAfarTrip[MortiseDodge].GetComponent<QuitterAfarUI>().UnlockAfar();
    }

    public void SlumSteamship() {
        int MortiseDodge= ColonizeQuitterClearingPeak.Instance.VoyagerDodge;
        if (MeDodge < MortiseDodge)
        {
            // 领奖后移动动画
            AirportAfarTrip[MeDodge].GetComponent<CanvasGroup>().DOFade(0, 0.5f).OnComplete(() =>
            {
                AirportAfarTrip[MeDodge].SetActive(false);
                AirportAfarTrip[MortiseDodge].GetComponent<QuitterAfarUI>().UnlockAfar();
                MeDodge = MortiseDodge;
            });
        }
    }
}
