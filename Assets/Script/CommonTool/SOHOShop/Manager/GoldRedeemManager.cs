using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldRedeemManager : MonoBehaviour
{
    public static GoldRedeemManager instance;

    public GoldRedeem[] goldRedeemGroup;
    public List<GoldRedeem> waitingGoldRedeems;

    private void Awake()
    {
        instance = this;
    }

    public void initGoldAmazonRedeemList()
    {

        // 进入排名的提现记录
        string[] withdrawRecordList = SaveDataManager.GetStringArray(SOHOShopConst.sv_GoldAmazonWithdrawRecord);
        waitingGoldRedeems = new List<GoldRedeem>();
        if (withdrawRecordList.Length > 0)
        {
            for (int i = 0; i < withdrawRecordList.Length; i++)
            {
                GoldRedeem item = JsonMapper.ToObject<GoldRedeem>(withdrawRecordList[i]);
                waitingGoldRedeems.Add(item);
            }
        }

        string[] withdrawList = SaveDataManager.GetStringArray(SOHOShopConst.sv_GoldAmazonWithdraw);
        if (withdrawList.Length == 0)
        {
            goldRedeemGroup = SOHOShopDataManager.instance.shopJson.withdraw_group;
            for (int i = 0; i < goldRedeemGroup.Length; i++)
            {
                goldRedeemGroup[i].state = Redeem.RedeemState.Init;
                goldRedeemGroup[i].id = i + 1;
            }
        }
        else
        {
            goldRedeemGroup = new GoldRedeem[withdrawList.Length];
            for (int i = 0; i < withdrawList.Length; i++)
            {
                GoldRedeem item = JsonMapper.ToObject<GoldRedeem>(withdrawList[i]);
                if (item.id == 0)
                {
                    item.id = i + 1;
                }
                goldRedeemGroup[i] = item;
            }

            AddTaskValue("Time");
        }

        updateWaitingRank();
    }

    // 金币、Amazon提现
    public void CashOutGoldRedeem(int id)
    {
        int index = GetIndexById(id);
        goldRedeemGroup[index].Cashout();
        if (goldRedeemGroup[index].state == Redeem.RedeemState.Waiting)
        {
            FinishRedeem(index);
        }

        saveGoldRedeemList();
        MessageCenterLogic.GetInstance().Send(SOHOShopConst.mg_RefreshGoldAmazonWithdrawList);

        // 检查任务是否完成
        if (goldRedeemGroup[index].currentTask != null)
        {
            AddTaskValue(goldRedeemGroup[index].currentTask.type, 0);
        }
    }

    private int GetIndexById(int id)
    {
        int index = -1;
        for (int i = 0; i < goldRedeemGroup.Length; i++)
        {
            GoldRedeem item = goldRedeemGroup[i];
            if (item.id == id)
            {
                index = i;
                break;
            }
        }
        return index;
    }
    private void FinishRedeem(int index)
    {
        GoldRedeem item = goldRedeemGroup[index];
        item.state = Redeem.RedeemState.Init;
        if (item.checkoutTasks != null)
        {
            foreach (CheckoutTask task in item.checkoutTasks)
            {
                if (task.is_static == 1)
                {
                    task.value = SOHOShopDataManager.instance.GetStaticCashoutTaskValue(task.type);
                }
                else
                {
                    task.value = 0;
                }
            }
        }
        // 创建新记录
        Dictionary<string, long> maxRankRes = RedeemUtil.getMaxRank(waitingGoldRedeems.ToArray());
        int maxRank = (int)maxRankRes["maxRank"];
        long lastUpdateTime = maxRankRes["lastUpdateTime"];
        long now = DateUtil.Current();
        GoldRedeem newRedeemItem = (GoldRedeem)item.Clone();
        newRedeemItem.state = Redeem.RedeemState.Waiting;
        maxRank = maxRank == 0 ? Random.Range(2000, 4000) : maxRank + RedeemUtil.randomRank(now - lastUpdateTime);
        newRedeemItem.rank = maxRank;
        newRedeemItem.lastUpdateRankTime = now;
        waitingGoldRedeems.Add(newRedeemItem);
        saveWaitingRedeemList();
    }

    /// <summary>
    /// 完成一个提现任务
    /// </summary>
    /// <param name="type"></param>
    public void AddTaskValue(string type = "AD", double addValue = 1)
    {
        for (int i = 0; i < goldRedeemGroup.Length; i++)
        {
            GoldRedeem item = goldRedeemGroup[i];
            if (item.state == Redeem.RedeemState.Checked)
            {
                bool finish = item.AddTaskValue(type, addValue);
                // 如果该提现记录完成了所有任务，进入排队阶段
                if (finish)
                {
                    FinishRedeem(i);
                }
            }
        }
        
        saveGoldRedeemList();
        MessageCenterLogic.GetInstance().Send(SOHOShopConst.mg_RefreshGoldAmazonWithdrawList);
    }

    /// <summary>
    /// 完成时间任务
    /// </summary>
    /// <param name="redeem"></param>
    public void FinishTimeTask(GoldRedeem redeem, int index = -1)
    {
        bool finish = redeem.AddTaskValue("Time", 1);
        // 如果该提现记录完成了所有任务，进入排队阶段
        if (finish)
        {
            if (index == -1)
            {
                FinishRedeem(GetIndexById(redeem.id));
            }
            else
            {
                FinishRedeem(index);
            }
        }
        saveGoldRedeemList();
        MessageCenterLogic.GetInstance().Send(SOHOShopConst.mg_RefreshGoldAmazonWithdrawList);
    }
    

    private void saveGoldRedeemList()
    {
        List<string> strings = new List<string>();
        foreach (GoldRedeem item in goldRedeemGroup)
        {
            strings.Add(JsonMapper.ToJson(item));
        }
        SaveDataManager.SetStringArray(SOHOShopConst.sv_GoldAmazonWithdraw, strings.ToArray());
    }

    private void saveWaitingRedeemList()
    {
        List<string> strings = new List<string>();
        foreach (GoldRedeem item in waitingGoldRedeems)
        {
            strings.Add(JsonMapper.ToJson(item));
        }
        SaveDataManager.SetStringArray(SOHOShopConst.sv_GoldAmazonWithdrawRecord, strings.ToArray());
    }

    // 修改提现排名
    public void updateWaitingRank()
    {
        RedeemUtil.updateWaitingRank(waitingGoldRedeems.ToArray());
        saveGoldRedeemList();
    }

    // 修改用户提现账户
    public void ChangeUserAccount()
    {
        foreach (GoldRedeem item in goldRedeemGroup)
        {
            if (item.state == Redeem.RedeemState.Init)
            {
                item.userAccount = SOHOShopDataManager.instance.currentUserAccount;
            }
        }
        saveGoldRedeemList();
        MessageCenterLogic.GetInstance().Send(SOHOShopConst.mg_RefreshGoldAmazonWithdrawList);
    }
}
