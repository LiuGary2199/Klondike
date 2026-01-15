using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedeemPanel : BaseUIForms
{
    public Button HomeButton;
    public Button ReviseButton;
    public Button RecordButton;
    public Button InstructionsButton;
    public Transform Container;
    public Image UserAccountMathodImg;
    public Text UserAccountEmailText;
    public HorizontalLayoutGroup layout;

    private Dictionary<string, GameObject> redeemCashoutItems;

    private void Start()
    {
        HomeButton.onClick.AddListener(() =>
        {
            MessageCenterLogic.GetInstance().Send(CConfig.mg_GameSuspend);
            CloseUIForm(SOHOShopUtil.PanelName(GetType().Name));
        });

        // 修改账户按钮
        ReviseButton.onClick.AddListener(() => {
            SOHOPanelManager.instance.InitSourcePanel(SOHOPanelManager.SourcePanel.RedeemPanel, SOHOPanelManager.SourceButton.Revise, null, null);
            UIManager.GetInstance().ShowUIForms(SOHOShopUtil.PanelName("WithdrawPanel"));
        });

        // 提现历史按钮
        RecordButton.onClick.AddListener(() => {
            openRecordPanel();
        });

        InstructionsButton.onClick.AddListener(() => {
            UIManager.GetInstance().ShowUIForms(SOHOShopUtil.PanelName("InstructionsPanel"));
        });

        MessageCenterLogic.GetInstance().Register(SOHOShopConst.mg_RefreshCashWithdrawList, (md) => {
            if (gameObject.activeInHierarchy)
            {
                refreshList();
            }
        });

        MessageCenterLogic.GetInstance().Register(SOHOShopConst.mg_RefreshCashWithdrawUserAccount, (md) => {
            refreshUserAccount();
        });

    }

    public override void Display(object uiFormParams)
    {
        base.Display(uiFormParams);

        refreshList();
        refreshUserAccount();

        // 打点
        PostEventScript.GetInstance().SendEvent("1301", NumberUtil.DoubleToStr(SOHOShopManager.instance.getCashAction()));

    }

    private void OnApplicationPause(bool pause)
    {
        if (!pause && gameObject.activeInHierarchy)
        {
            // 切回前台时，重新获取倒计时
            refreshList();
        }
    }

    private void refreshUserAccount()
    {
        UserAccount currentUserAccount = SOHOShopDataManager.instance.currentUserAccount;
        if (currentUserAccount == null)
        {
            UserAccountMathodImg.sprite = Resources.Load<Sprite>("SOHOShop/UI_Redeem/UI_Pay/" + UserAccount.WithdrawMethod.PayPal.ToString());
            UserAccountEmailText.text = "Please enter your withdrawal account";
        }
        else
        {
            UserAccountMathodImg.sprite = Resources.Load<Sprite>("SOHOShop/UI_Redeem/UI_Pay/" + currentUserAccount.method.ToString());
            UserAccountEmailText.text = currentUserAccount.email;
        }
    }

    private void refreshList()
    {
        if (redeemCashoutItems == null)
        {
            redeemCashoutItems = new Dictionary<string, GameObject>();
        }

        for (int i = CashRedeemManager.instance.CashRedeemList.Count; i > 0; i--)
        {
            CashRedeem item = CashRedeemManager.instance.CashRedeemList[i - 1];
            
            if (item.state != Redeem.RedeemState.Complete)
            {
                if (!redeemCashoutItems.ContainsKey(item.id.ToString()))
                {
                    GameObject redeemItem = Instantiate(Resources.Load<GameObject>("SOHOShop/UIPanel/" + (CommonUtil.IsPortrait() ? "Portrait" : "Landscape") + "/RedeemItem"), Container);
                    redeemItem.transform.SetSiblingIndex(CashRedeemManager.instance.CashRedeemList.Count - i);
                    redeemCashoutItems.Add(item.id.ToString(), redeemItem);
                }
                redeemCashoutItems[item.id.ToString()].GetComponent<RedeemItem>().InitRedeemItem(item);
            } 
            else if (redeemCashoutItems.ContainsKey(item.id.ToString()))
            {
                Destroy(redeemCashoutItems[item.id.ToString()]);
                redeemCashoutItems.Remove(item.id.ToString());
            }
        }

        if (!CommonUtil.IsPortrait())
        {
            if (CashRedeemManager.instance.CashRedeemList.Count == 1)
            {
                layout.childAlignment = TextAnchor.MiddleCenter;
                layout.GetComponent<RectTransform>().SetAnchor(AnchorPresets.MiddleCenter);
                layout.GetComponent<RectTransform>().SetPivot(PivotPresets.MiddleCenter);
                layout.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            }
            else
            {
                layout.childAlignment = TextAnchor.MiddleLeft;
                layout.GetComponent<RectTransform>().SetAnchor(AnchorPresets.MiddleLeft);
                layout.GetComponent<RectTransform>().SetPivot(PivotPresets.MiddleLeft);
                layout.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            }
        }

        Container.GetComponent<RectTransform>().sizeDelta = new Vector2(Container.GetComponent<RectTransform>().sizeDelta.x, 430 * CashRedeemManager.instance.CashRedeemList.Count);

    }


    private void openRecordPanel()
    {
        GameObject panel = UIManager.GetInstance().ShowUIForms(SOHOShopUtil.PanelName("RecordPanel"));
        panel.GetComponent<RecordPanel>().InitData("cash");
    }

    public override void Hidding()
    {
        base.Hidding();
    }
}
