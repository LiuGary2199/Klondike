using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CashOutButton : MonoBehaviour
{
    public Text RedeemCountdownText;

    private long cashWithdrawCountdown;
    private Coroutine countdownCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        showCashWithdrawShopCountdown();

        // 刷新现金提现倒计时
        MessageCenterLogic.GetInstance().Register(SOHOShopConst.mg_RefreshCountdown, (messageData) => {
            showCashWithdrawShopCountdown();
        });

        MessageCenterLogic.GetInstance().Register(SOHOShopConst.mg_ShowCashShopHand, (messageData) => {
            showHand();
        });
    }

    private void OnApplicationPause(bool pause)
    {
        if (!pause)
        {
            // 切回前台时，重新获取倒计时
            showCashWithdrawShopCountdown();
        }
    }

    public void showHand()
    {
        bool showHand = SOHOShopManager.instance.HasUnchckedCashWithdraw();
        if (showHand)
        {
            transform.Find("Hand").gameObject.SetActive(true);
        }
        else
        {
            transform.Find("Hand").gameObject.SetActive(false);
        }
    }


    // 显示倒计时
    public void showCashWithdrawShopCountdown()
    {
        if (CommonUtil.IsApple())
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
            cashWithdrawCountdown = SOHOShopManager.instance.GetCashWithdrawCountDown();
            if (cashWithdrawCountdown > 0)
            {
                RedeemCountdownText.transform.parent.gameObject.SetActive(true);
                if (countdownCoroutine != null)
                {
                    StopCoroutine(countdownCoroutine);
                }
                countdownCoroutine = StartCoroutine(cashCountdown());
            }
            else
            {
                RedeemCountdownText.transform.parent.gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator cashCountdown()
    {
        while (cashWithdrawCountdown >= 0)
        {
            cashWithdrawCountdown--;
            RedeemCountdownText.text = DateUtil.SecondsFormat(cashWithdrawCountdown);
            yield return new WaitForSeconds(1);
        }
        // 重新获取倒计时
        SOHOShopManager.instance.CashWithdrawFinishCountdown();
        yield return new WaitForSeconds(1);
        showCashWithdrawShopCountdown();
    }
}
