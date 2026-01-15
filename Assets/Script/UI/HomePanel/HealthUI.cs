using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zeta_framework;

public class HealthUI : MonoBehaviour
{
    public Text HealthNumText;
    public Text HealthCountdownText;

    private int countdown;

    // Start is called before the first frame update
    void Start()
    {
        MessageCenterLogic.GetInstance().Register(CConfig.mg_ItemChange_ + ResourceCtrl.Instance.health.id, (md) => {
            RenderData();
        });

        MessageCenterLogic.GetInstance().Register(CConfig.mg_ItemChange_ + ResourceCtrl.Instance.unlimit_health.id, (md) => {
            RenderData();
        });
    }

    private void OnEnable()
    {
        RenderData();
    }

    private void OnApplicationPause(bool pause)
    {
        if (!pause)
        {
            // 切回前台
            RenderData();
        }
    }

    private void RenderData()
    {
        if (HealthCtrl.Instance.IsUnlimitedState())
        {
            // 无限体力
            HealthNumText.text = "∞";
            // 无限体力倒计时
            countdown = HealthCtrl.Instance.UnlimitedCountdown();
            HealthCountdownText.text = DateUtil.SecondsFormat(countdown);
        }
        else
        {
            HealthCtrl.Instance.GetCurrentHealth(out int health, out countdown);
            HealthNumText.text = health.ToString();
            HealthCountdownText.text = HealthCtrl.Instance.IsFull() ? "Full" : DateUtil.SecondsFormat(countdown);
        }

        
        StopCoroutine(nameof(StartCountdown));
        if (!HealthCtrl.Instance.IsFull() || HealthCtrl.Instance.IsUnlimitedState())
        {
            StartCoroutine(nameof(StartCountdown));
        }
    }

    private IEnumerator StartCountdown()
    {
        while(true)
        {
            yield return new WaitForSeconds(1);
            countdown--;
            if (countdown <= 0)
            {
                RenderData();
            }
            else
            {
                HealthCountdownText.text = DateUtil.SecondsFormat(countdown);
            }
        }
    }
}
