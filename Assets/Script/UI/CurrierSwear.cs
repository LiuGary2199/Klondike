using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrierSwear : MonoBehaviour
{
    [UnityEngine.Serialization.FormerlySerializedAs("sliderImage")] public Image WisdomFifth;
    [UnityEngine.Serialization.FormerlySerializedAs("progressText")] public Text BachelorMoss;
    // Start is called before the first frame update
    void Start()
    {
        WisdomFifth.fillAmount = 0;
        BachelorMoss.text = "0%";
    }

    void Update()
    {
        if (WisdomFifth.fillAmount <= 0.8f || (ToeBoldLeg.instance.Amino && CashOutManager.FarBefriend().Ready))
        {
            WisdomFifth.fillAmount += Time.deltaTime * .2f;
            BachelorMoss.text = (int)(WisdomFifth.fillAmount * 100) + "%";
            if (WisdomFifth.fillAmount >= 1)
            {
                // 安卓平台特殊屏蔽规则 被屏蔽玩家显示提示 阻止进入
                if (LimyRancher.instance.AndroidBlockCheck())
                    return;

                UnfoldGate.UpMound();

                Destroy(transform.parent.gameObject);
                LimyRancher.instance.AbutDeaf();
            }
        }
    }
}
