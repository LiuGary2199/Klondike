using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedeemItem : MonoBehaviour
{
    public Button CashOutBtn;
    public GameObject State_1;
    public GameObject State_2;
    public GameObject State_3;
    public GameObject State_4;
    public Text State_1_CountDownText;  // 新建状态的提现记录的倒计时
    public Text State_1_ConditionText;
    public GameObject State_2_Text1;
    public GameObject State_2_Text2;
    public Text State_2_CountDownText;
    public GameObject State_3_TimeTask;
    public Text State_3_TimeText;
    public Image State_3_FillAmount;
    public Text State_3_ProcessText;
    public Text State_3_DescText;
    public Text State_4_RankText;
    public Button State_4_ItemLong;
    public Text State_4_ItemLongText1;
    public Text State_4_ItemLongText2;
    public Button State_4_DetailBtn;

    private CashRedeem cashRedeem;
    private long init_countdown;         // 距离可提现倒计时
    private Coroutine countdownCoroutinue;
    private long task_time_countdown;   // Time类型任务倒计时
    private Coroutine taskTimeCountdownCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        // 提现按钮点击
        CashOutBtn.onClick.AddListener(() => {
            SOHOPanelManager.instance.InitSourcePanel(SOHOPanelManager.SourcePanel.RedeemPanel, SOHOPanelManager.SourceButton.Cashout, cashRedeem, null);
            if (SOHOShopDataManager.instance.currentUserAccount == null)
            {
                UIManager.GetInstance().ShowUIForms(SOHOShopUtil.PanelName("WithdrawPanel"));
            }
            else
            {
                UIManager.GetInstance().ShowUIForms(SOHOShopUtil.PanelName("ConfirmEmailPanel"));
            }
        });

        State_4_DetailBtn.onClick.AddListener(() =>
        {
            State_4_DetailBtn.gameObject.SetActive(false);
            State_4_ItemLong.gameObject.SetActive(true);
            GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x, 590f);
        });
        State_4_ItemLong.onClick.AddListener(() =>
        {
            State_4_DetailBtn.gameObject.SetActive(true);
            State_4_ItemLong.gameObject.SetActive(false);
            GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x, 430);
        });
    }

    public void InitRedeemItem(CashRedeem cashItem)
    {
        cashRedeem = cashItem;

        if (taskTimeCountdownCoroutine != null)
        {
            StopCoroutine(taskTimeCountdownCoroutine);
        }

        RefreshItem();
    }

    private void RefreshItem()
    {
        GameObject SelectedState = null;
        Redeem.RedeemState state = cashRedeem.state;
        if (state == Redeem.RedeemState.Init && !cashRedeem.canCheckout)
        {
            SelectedState = State_1;
            init_countdown = cashRedeem.endTime - DateUtil.Current();
            if (init_countdown <= 0)
            {
                CashRedeemManager.instance.FinishInitCountDown(cashRedeem);
                RefreshItem();
                return;
            }
            else if (gameObject.activeInHierarchy)
            {
                if (countdownCoroutinue != null)
                {
                    StopCoroutine(countdownCoroutinue);
                }
                countdownCoroutinue = StartCoroutine(StartCountDown());
            }
            if (cashRedeem.preConditions != null && cashRedeem.preConditions.Count > 0)
            {
                State_1_ConditionText.text = cashRedeem.preConditions[0].desc.Replace("${value}", cashRedeem.preConditions[0].condition_num.ToString());
                State_1_ConditionText.gameObject.SetActive(true);
            }
            else
            {
                State_1_ConditionText.gameObject.SetActive(false);
            }
        }
        else if (state == Redeem.RedeemState.Unchecked || state == Redeem.RedeemState.Init && cashRedeem.canCheckout)
        {
            SelectedState = State_2;
            State_2_Text1.SetActive(state == Redeem.RedeemState.Init);
            State_2_Text2.SetActive(state == Redeem.RedeemState.Init);
            if (state == Redeem.RedeemState.Init)
            {
                State_2_Text1.SetActive(true);
                State_2_Text2.SetActive(true);
                init_countdown = cashRedeem.endTime - DateUtil.Current();
                if (init_countdown <= 0)
                {
                    CashRedeemManager.instance.FinishInitCountDown(cashRedeem);
                }
                else if (gameObject.activeInHierarchy)
                {
                    if (countdownCoroutinue != null)
                    {
                        StopCoroutine(countdownCoroutinue);
                    }
                    countdownCoroutinue = StartCoroutine(StartCountDown());
                }
            }
            else
            {
                State_2_Text1.SetActive(false);
                State_2_Text2.SetActive(false);
            }
        }
        else if (state == Redeem.RedeemState.Checked)
        {
            SelectedState = State_3;
            double value = cashRedeem.currentTask.value;
            double task_num = cashRedeem.currentTask.task_num;
            State_3_DescText.text = cashRedeem.currentTask.desc.Replace("${value}", value.ToString()).Replace("${task_num}", task_num.ToString());
            if (cashRedeem.currentTask.type.Equals("Time"))
            {
                State_3_TimeTask.SetActive(true);
                State_3_FillAmount.transform.parent.gameObject.SetActive(false);
                task_time_countdown = (long)cashRedeem.currentTask.value + (long)cashRedeem.currentTask.task_num - DateUtil.Current();
                if (task_time_countdown <= 0)
                {
                    CashRedeemManager.instance.FinishTimeTask(cashRedeem);
                    RefreshItem();
                    return;
                }
                else
                {
                    if (gameObject.activeInHierarchy)
                    {
                        taskTimeCountdownCoroutine = StartCoroutine(StartTaskTimeCountdown());
                    }
                }
            }
            else
            {
                State_3_FillAmount.transform.parent.gameObject.SetActive(true);
                State_3_TimeTask.SetActive(false);
                State_3_FillAmount.fillAmount = (float)(value / task_num);
                string processText = value + "/" + task_num;
                State_3_ProcessText.text = processText;
            }
        }
        else if (state == Redeem.RedeemState.Waiting)
        {
            SelectedState = State_4;
            State_4_RankText.text = "Your current queue position is : " + cashRedeem.rank;
            State_4_ItemLong.gameObject.SetActive(false);
            State_4_DetailBtn.gameObject.SetActive(true);
            State_4_ItemLongText1.text = "You have collected $" + NumberUtil.DoubleToStr(cashRedeem.cashout) + " Within " + DateUtil.SecondsFormat(SOHOShopDataManager.instance.shopJson.cash_withdraw_time);
            State_4_ItemLongText2.text = "There are " + (cashRedeem.rank - 1) + " players in front. Your position will be refreshed every minute.Thank you for your patience";
        }

        State_1.SetActive(false);
        State_2.SetActive(false);
        State_3.SetActive(false);
        State_4.SetActive(false);

        if (SelectedState != null)
        {
            SelectedState.SetActive(true);
            SelectedState.transform.Find("BalanceNumber").GetComponent<Text>().text = "$ " + NumberUtil.DoubleToStr(cashRedeem.cashout);
            UserAccount userAccount = cashRedeem.userAccount;

            if (userAccount != null)
            {
                SelectedState.transform.Find("PayPalImage").gameObject.SetActive(true);
                SelectedState.transform.Find("PayPalImage").GetComponent<Image>().sprite = Resources.Load<Sprite>("SOHOShop/UI_Redeem/UI_Pay/" + userAccount.method.ToString());
            }
            else
            {
                SelectedState.transform.Find("PayPalImage").gameObject.SetActive(false);
            }
        }
    }

    // 未到可以提现时间，则开启倒计时
    private IEnumerator StartCountDown()
    {
        while (init_countdown > 0)
        {
            init_countdown--;
            State_1_CountDownText.text = DateUtil.SecondsFormat(init_countdown);
            State_2_CountDownText.text = DateUtil.SecondsFormat(init_countdown);
            yield return new WaitForSeconds(1f);

            if (init_countdown <= 0)
            {
                CashRedeemManager.instance.FinishInitCountDown(cashRedeem);
                RefreshItem();
            }
        }
    }

    private IEnumerator StartTaskTimeCountdown()
    {
        while (task_time_countdown > 0)
        {
            task_time_countdown--;
            State_3_TimeText.text = DateUtil.SecondsFormat(task_time_countdown);
            yield return new WaitForSeconds(1f);
            if (task_time_countdown <= 0)
            {
                CashRedeemManager.instance.FinishTimeTask(cashRedeem);
                RefreshItem();
            }
        }
    }
}
