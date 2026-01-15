using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldAmazonRedeemItem : MonoBehaviour
{
    public Image PayImage;
    public GameObject State_1;
    public GameObject GoldImage;
    public GameObject AmazonImage;
    public GameObject CashImage;
    public Image ProgressImage;
    public Text CashoutText;
    public Button CashOutBtn;
    
    public GameObject State_Task;
    public Text TaskText;

    public GameObject State_Time;
    public Text TimeText;

    private double goldOrAmazon;
    private GoldRedeem goldRedeemItem;

    private long init_countdown;    // Time类型的任务，倒计时
    private Coroutine countdownCoroutine;

    private void Start()
    {
        CashOutBtn.onClick.AddListener(() =>
        {
            if (goldOrAmazon >= goldRedeemItem.num)
            {
                SOHOPanelManager.instance.InitSourcePanel(SOHOPanelManager.SourcePanel.GoldAndAmazonPanel, SOHOPanelManager.SourceButton.Cashout, null, goldRedeemItem);
                if (SOHOShopDataManager.instance.currentUserAccount == null)
                {
                    UIManager.GetInstance().ShowUIForms(SOHOShopUtil.PanelName("WithdrawPanel"));
                }
                else
                {
                    UIManager.GetInstance().ShowUIForms(SOHOShopUtil.PanelName("ConfirmEmailPanel"));
                }
            }
            else
            {
                string msg = "Gold is not enough";
                if (goldRedeemItem.type == "gold")
                {
                    msg = "Gold is not enough";
                } 
                else if (goldRedeemItem.type == "amazon")
                {
                    msg = "Amazon is not enough";
                }
                else if (goldRedeemItem.type == "cash")
                {
                    msg = "Cash is not enough";
                }
                ToastManager.GetInstance().ShowToast(msg);
            }
            
        });
    }

    public void InitItem(GoldRedeem item, double gold, double amazon, double cash, int index)
    {
        goldRedeemItem = item;

        UserAccount currentUserAccount = SOHOShopDataManager.instance.currentUserAccount;
        if (currentUserAccount == null)
        {
            PayImage.sprite = Resources.Load<Sprite>("SOHOShop/UI_Redeem/UI_Pay/" + UserAccount.WithdrawMethod.PayPal.ToString());
        }
        else
        {
            PayImage.sprite = Resources.Load<Sprite>("SOHOShop/UI_Redeem/UI_Pay/" + currentUserAccount.method.ToString());
        }

        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
        }
        
        if (goldRedeemItem.state == Redeem.RedeemState.Init)
        {
            State_1.SetActive(true);
            State_Task.SetActive(false);
            State_Time.SetActive(false);
            GameObject activeObj = GoldImage;
            goldOrAmazon = gold;
            if (item.type == "gold")
            {
                activeObj = GoldImage;
                goldOrAmazon = gold;
            }
            else if (item.type == "amazon")
            {
                activeObj = AmazonImage;
                goldOrAmazon = amazon;
            }
            else if (item.type == "cash")
            {
                activeObj = CashImage;
                goldOrAmazon = cash;
            }
            activeObj.SetActive(true);
            activeObj.transform.Find("Text").GetComponent<Text>().text = NumberUtil.DoubleToStr(goldOrAmazon) + "/" + item.num;
            ProgressImage.fillAmount = (float)(goldOrAmazon / item.num);
            CashoutText.text = "$" + item.cashout;

            CashOutBtn.gameObject.SetActive(item.state == Redeem.RedeemState.Init);

        } else if (goldRedeemItem.state == Redeem.RedeemState.Checked)
        {
            CashoutText.text = "$" + item.cashout;
            State_1.SetActive(false);
            State_Task.SetActive(true);
            TaskText.text = goldRedeemItem.currentTask.desc
                    .Replace("${value}", goldRedeemItem.currentTask.value.ToString())
                    .Replace("${task_num}", goldRedeemItem.currentTask.task_num.ToString());
            if (goldRedeemItem.currentTask.type.Equals("Time"))
            {
                State_Time.SetActive(true);
                init_countdown = (long)goldRedeemItem.currentTask.task_num + (long)goldRedeemItem.currentTask.value - DateUtil.Current();
                countdownCoroutine = StartCoroutine(StartCountDown());
            }
            else
            {
                State_Time.SetActive(false);
            }
        }
    }

    private IEnumerator StartCountDown()
    {
        while (init_countdown > 0)
        {
            init_countdown--;
            TimeText.text = DateUtil.SecondsFormat(init_countdown);
            yield return new WaitForSeconds(1f);

            if (init_countdown <= 0)
            {
                GoldRedeemManager.instance.FinishTimeTask(goldRedeemItem);
            }
        }
    }
}
