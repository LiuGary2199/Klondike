using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldRedeem : Redeem
{
    public string type;    // gold/amazon
    public int num;        // 提现所需金币

    public void Cashout()
    {
        userAccount = SOHOShopDataManager.instance.currentUserAccount;
        if (type == "gold")
        {
            SOHOShopManager.instance.subGoldAction(num);
        }
        else if (type == "amazon")
        {
            SOHOShopManager.instance.subAmazonAction(num);
        }
        else if (type == "cash")
        {
            SOHOShopManager.instance.subCashAction(num);
        }
        if (SOHOShopDataManager.instance.shopJson.gold_checkout_tasks == null || SOHOShopDataManager.instance.shopJson.gold_checkout_tasks.Length == 0)
        {
            FinishTasks();
        }
        else
        {
            state = RedeemState.Checked;
            checkoutTasks = new List<CheckoutTask>();
            foreach (CheckoutTask task in SOHOShopDataManager.instance.shopJson.gold_checkout_tasks)
            {
                checkoutTasks.Add((CheckoutTask)task.Clone());
            }
            currentTaskIndex = 0;
            currentTask = checkoutTasks[0];
        }
    }

    public void FinishTasks()
    {
        Dictionary<string, long> maxRankRes = RedeemUtil.getMaxRank(GoldRedeemManager.instance.goldRedeemGroup);
        int maxRank = (int)maxRankRes["maxRank"];
        long lastUpdateTime = maxRankRes["lastUpdateTime"];
        long now = DateUtil.Current();
        maxRank = maxRank == 0 ? Random.Range(2000, 4000) : maxRank + RedeemUtil.randomRank(now - lastUpdateTime);
        state = RedeemState.Waiting;
        rank = maxRank;
        lastUpdateRankTime = now;
    }
}
