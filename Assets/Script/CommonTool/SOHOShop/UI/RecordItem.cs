using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordItem : MonoBehaviour
{
    public Image UserCountIcon;
    public Text CashText;
    public Text EmailText;
    public Text DateTimeText;
    public GameObject GoldRedeemRank;
    public Text RankText;

    

    public void InitData(GoldRedeem item)
    {
        UserCountIcon.sprite = Resources.Load<Sprite>("SOHOShop/UI_Redeem/UI_Pay/" + item.userAccount.method.ToString());
        CashText.text = "$" + NumberUtil.DoubleToStr(item.cashout);
        EmailText.text = item.userAccount.email;
        DateTimeText.text = "withdraw in " + DateUtil.DateTimeFormat(DateUtil.SecondsToDateTime(item.lastUpdateRankTime), "g");
        GoldRedeemRank.SetActive(true);
        RankText.text = item.rank.ToString();
    }

    public void InitData(CashRedeem item)
    {
        UserCountIcon.sprite = Resources.Load<Sprite>("SOHOShop/UI_Redeem/UI_Pay/" + item.userAccount.method.ToString());
        CashText.text = "$" + NumberUtil.DoubleToStr(item.cashout);
        EmailText.text = item.userAccount.email;
        DateTimeText.text = "withdraw in " + DateUtil.DateTimeFormat(DateUtil.SecondsToDateTime(item.lastUpdateRankTime), "g");
        GoldRedeemRank.SetActive(false);
    }
}
