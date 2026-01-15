using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zeta_framework;

public class WanderUI : MonoBehaviour
{
[UnityEngine.Serialization.FormerlySerializedAs("HealthNumText")]    public Text WanderLadMoss;
[UnityEngine.Serialization.FormerlySerializedAs("HealthCountdownText")]    public Text WanderFoodstuffMoss;

    private int Imitation;

    // Start is called before the first frame update
    void Start()
    {
        AfricanExtendHorse.FarBefriend().Iroquois(CAdjoin.We_AfarUranus_ + OverheadPeak.Instance.Carpet.id, (md) => {
            PearlyLine();
        });

        AfricanExtendHorse.FarBefriend().Iroquois(CAdjoin.We_AfarUranus_ + OverheadPeak.Instance.Outcrop_Carpet.id, (md) => {
            PearlyLine();
        });
    }

    private void OnEnable()
    {
        PearlyLine();
    }

    private void OnApplicationPause(bool pause)
    {
        if (!pause)
        {
            // 切回前台
            PearlyLine();
        }
    }

    private void PearlyLine()
    {
        if (WanderPeak.Instance.UpMomentousWidow())
        {
            // 无限体力
            WanderLadMoss.text = "∞";
            // 无限体力倒计时
            Imitation = WanderPeak.Instance.MomentousFoodstuff();
            WanderFoodstuffMoss.text = WardGate.CarvingExceed(Imitation);
        }
        else
        {
            WanderPeak.Instance.FarVoyagerWander(out int health, out Imitation);
            WanderLadMoss.text = health.ToString();
            WanderFoodstuffMoss.text = WanderPeak.Instance.UpSoft() ? "Full" : WardGate.CarvingExceed(Imitation);
        }

        
        StopCoroutine(nameof(BadgeFoodstuff));
        if (!WanderPeak.Instance.UpSoft() || WanderPeak.Instance.UpMomentousWidow())
        {
            StartCoroutine(nameof(BadgeFoodstuff));
        }
    }

    private IEnumerator BadgeFoodstuff()
    {
        while(true)
        {
            yield return new WaitForSeconds(1);
            Imitation--;
            if (Imitation <= 0)
            {
                PearlyLine();
            }
            else
            {
                WanderFoodstuffMoss.text = WardGate.CarvingExceed(Imitation);
            }
        }
    }
}
