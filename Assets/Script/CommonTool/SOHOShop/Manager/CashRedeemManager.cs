using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashRedeemManager : MonoBehaviour
{
    public static CashRedeemManager instance;
    
    public List<CashRedeem> CashRedeemList; // 提现记录

    private void Awake()
    {
        instance = this;
    }

    public void InitCashWithdraw()
    {
        // 提现列表
        if (CashRedeemList == null || CashRedeemList.Count == 0)
        {
            CashRedeemList = new List<CashRedeem>();
            string[] stringList = SaveDataManager.GetStringArray(SOHOShopConst.sv_CashWithdrawList);
            foreach (string item in stringList)
            {
                CashRedeem redeem = JsonMapper.ToObject<CashRedeem>(item);
                redeem.Init();
                FinishTimeTask(redeem);
                CashRedeemList.Add(redeem);
            }
        }

        updateCashWithdraw();
        updateWaitingRank();

    }

    // 检查更新绿币提现记录
    private void updateCashWithdraw()
    {
        long current = DateUtil.Current();
        long[] timePoints = SOHODateUtil.StartAndEndPointTimeOfNow();
        long startTime = CashRedeemList.Count == 0 ? current : timePoints[0];
        long endTime = startTime + SOHOShopDataManager.instance.shopJson.cash_withdraw_time;

        double cashBalance = SaveDataManager.GetDouble(SOHOShopConst.sv_CashWithdrawBalance);
        cashBalance = NumberUtil.Round(cashBalance);
        // 判断当前时间点是否已有记录
        bool flag = false;
        List<CashRedeem> removeList = new List<CashRedeem>();
        foreach(CashRedeem item in CashRedeemList)
        {
            if (item.state == Redeem.RedeemState.Init)
            {
                // 如果当前时间已经有记录，则跳过；否则修改已有记录状态
                if (item.startTime <= current && item.endTime > current)
                {
                    flag = true;
                    item.cashout = cashBalance;
                }
                else
                {
                    if (cashBalance == 0)
                    {
                        // 如果当前的记录余额为0，则重新开始倒计时
                        flag = true;
                        item.startTime = startTime;
                        item.endTime = endTime;
                    }
                    else
                    {
                        item.state = Redeem.RedeemState.Unchecked;
                        item.cashout = cashBalance;
                        cashBalance = 0;
                        SaveDataManager.SetString(SOHOShopConst.sv_CashWithdrawBalance, cashBalance.ToString());

                        // 打点
                        PostEventScript.GetInstance().SendEvent("1304", NumberUtil.DoubleToStr(item.cashout));
                    }
                }
                item.canCheckout = item.CheckCanCashout();
            }
            else if (item.cashout == 0)
            {
                removeList.Add(item);
            }
        }
        for (int i = 0; i < removeList.Count; i++)
        {
            CashRedeemList.Remove(removeList[i]);
        }
        if (!flag)
        {
            // 创建新记录
            CashRedeem newCashWighdraw = new CashRedeem();
            newCashWighdraw.id = CashRedeemList.Count;
            newCashWighdraw.cashout = cashBalance;
            newCashWighdraw.startTime = startTime;
            newCashWighdraw.endTime = endTime;
            CashRedeemList.Add(newCashWighdraw);
        }
        // 保存提现记录
        saveCashWitrdrawsList();

        MessageCenterLogic.GetInstance().Send(SOHOShopConst.mg_RefreshCountdown);
        MessageCenterLogic.GetInstance().Send(SOHOShopConst.mg_ShowCashShopHand, new MessageData(false));
    }

    /// <summary>
    /// 保存提现记录
    /// </summary>
    private void saveCashWitrdrawsList()
    {
        List<string> strings = new List<string>();
        for (int i = 0; i < CashRedeemList.Count; i++)
        {
            strings.Add(JsonMapper.ToJson(CashRedeemList[i]));
        }
        SaveDataManager.SetStringArray(SOHOShopConst.sv_CashWithdrawList, strings.ToArray());
        // 刷新列表
        MessageCenterLogic.GetInstance().Send(SOHOShopConst.mg_RefreshCashWithdrawList);
    }

    /// <summary>
    /// 增加现金余额
    /// </summary>
    /// <param name="num"></param>
    public void AddCashBalance()
    {
        double balance = SOHOShopManager.instance.getCashAction();
        foreach (CashRedeem item in CashRedeemList)
        {
            if (item.state == Redeem.RedeemState.Unchecked)
            {
                balance = Math.Max(balance, 0);
                if (item.cashout > balance)
                {
                    item.cashout = balance;
                }
                balance -= item.cashout;
            }
        }
        SaveDataManager.SetDouble(SOHOShopConst.sv_CashWithdrawBalance, balance);
        updateCashWithdraw();
    }

    /// <summary>
    /// 是否有未提现的记录
    /// </summary>
    /// <returns></returns>
    public bool HasUncheckedStateCashWithdraw()
    {
        foreach(CashRedeem item in CashRedeemList)
        {
            if (item.state == Redeem.RedeemState.Unchecked)
            {
                return true;
            }
        }
        return false;
    }

    public void AddPreCondition(string name, double addValue)
    {
        foreach (CashRedeem item in CashRedeemList)
        {
            if (item.state == Redeem.RedeemState.Init && !item.canCheckout)
            {
                item.AddPreConditionValue(name, addValue);
            }
        }
    }

    /// <summary>
    /// 完成一个提现任务
    /// </summary>
    /// <param name="type"></param>
    public void AddTaskValue(string type = "AD", double addValue = 1)
    {
        Dictionary<string, long> maxRankRes = RedeemUtil.getMaxRank(CashRedeemList.ToArray());
        int maxRank = (int)maxRankRes["maxRank"];
        long lastUpdateTime = maxRankRes["lastUpdateTime"];

        long now = DateUtil.Current();
        foreach (CashRedeem item in CashRedeemList)
        {
            if (item.state == Redeem.RedeemState.Checked)
            {
                bool finish = item.AddTaskValue(type, addValue);

                // 如果该提现记录完成了所有任务，进入排队阶段
                if (finish)
                {
                    item.state = Redeem.RedeemState.Waiting;
                    maxRank = maxRank == 0 ? UnityEngine.Random.Range(2000, 4000) : maxRank + RedeemUtil.randomRank(now - lastUpdateTime);
                    lastUpdateTime = now;
                    item.rank = maxRank;
                    item.lastUpdateRankTime = now;

                    // 打点
                    PostEventScript.GetInstance().SendEvent("1306");
                }
            }
        }
        saveCashWitrdrawsList();
    }

    /// <summary>
    /// 完成时间任务
    /// </summary>
    /// <param name="redeem"></param>
    public void FinishTimeTask(CashRedeem redeem)
    {
        bool finish = redeem.AddTaskValue("Time", 1);
        // 如果该提现记录完成了所有任务，进入排队阶段
        if (finish)
        {
            redeem.state = Redeem.RedeemState.Waiting;
            Dictionary<string, long> maxRankRes = RedeemUtil.getMaxRank(CashRedeemList.ToArray());
            int maxRank = (int)maxRankRes["maxRank"];
            long lastUpdateTime = maxRankRes["lastUpdateTime"];
            long now = DateUtil.Current();
            maxRank = maxRank == 0 ? UnityEngine.Random.Range(2000, 4000) : maxRank + RedeemUtil.randomRank(now - lastUpdateTime);
            redeem.rank = maxRank;
            redeem.lastUpdateRankTime = now;

            // 打点
            PostEventScript.GetInstance().SendEvent("1306");
        }
        saveCashWitrdrawsList();
    }

    /// <summary>
    /// 提现
    /// </summary>
    /// <param name="id"></param>
    public void CheckCashWithdraw(int id)
    {
        if (SOHOShopDataManager.instance.currentUserAccount == null)
        {
            UIManager.GetInstance().ShowUIForms(SOHOShopUtil.PanelName("WithdrawPanel"));
            return;
        }

        CashRedeem selectedCashWithdraw = null;
        foreach(CashRedeem item in CashRedeemList)
        {
            if (item.id == id)
            {
                selectedCashWithdraw = item;
                break;
            }
        }

        if (selectedCashWithdraw == null || !(selectedCashWithdraw.state == Redeem.RedeemState.Unchecked || selectedCashWithdraw.state == Redeem.RedeemState.Init && selectedCashWithdraw.canCheckout))
        {
            return;
        }

        if (selectedCashWithdraw.state == Redeem.RedeemState.Init)
        {
            SaveDataManager.SetDouble(SOHOShopConst.sv_CashWithdrawBalance, 0);
            long nowTicks = DateUtil.Current();  // 当前时间
            long startTicks = DateUtil.GetTimestamp(DateTime.Today); // 当日零点时间
            int initSeconds = (int)(nowTicks - startTicks);
            SaveDataManager.SetInt(SOHOShopConst.sv_InitSeconds, initSeconds);
        }
        // 修改记录状态
        selectedCashWithdraw.state = Redeem.RedeemState.Checked;
        selectedCashWithdraw.endTime = DateUtil.Current() > selectedCashWithdraw.endTime ? selectedCashWithdraw.endTime : DateUtil.Current();
        // 扣除游戏现金余额
        SOHOShopManager.instance.subCashAction(selectedCashWithdraw.cashout);
        saveCashWitrdrawsList();
        MessageCenterLogic.GetInstance().Send(SOHOShopConst.mg_ShowCashShopHand, new MessageData(false));

        // 检查更新提现记录
        updateCashWithdraw();
        
        // 打点
        PostEventScript.GetInstance().SendEvent("1305");

        // 检查任务是否完成
        if (selectedCashWithdraw.currentTask != null)
        {
            AddTaskValue(selectedCashWithdraw.currentTask.type, 0);
        }
    }

    // 到达可提现时间
    public void FinishInitCountDown(CashRedeem obj)
    {
        updateCashWithdraw();
    }

    // 每隔一段时间，减少提现排队当前排名
    public void updateWaitingRank()
    {
        RedeemUtil.updateWaitingRank(CashRedeemList.ToArray());
        saveCashWitrdrawsList();
    }


    /// <summary>
    /// 修改用户账户
    /// </summary>
    public void ChangeUserAccount()
    {
        foreach (CashRedeem item in CashRedeemList)
        {
            if ((item.userAccount == null || item.state == Redeem.RedeemState.Init || item.state == Redeem.RedeemState.Unchecked) 
                && SOHOShopDataManager.instance.currentUserAccount != null)
            {
                item.userAccount = SOHOShopDataManager.instance.currentUserAccount;
            }
        }
        // 保存提现记录
        saveCashWitrdrawsList();
        // 刷新页面
        MessageCenterLogic.GetInstance().Send(SOHOShopConst.mg_RefreshCashWithdrawList);
        MessageCenterLogic.GetInstance().Send(SOHOShopConst.mg_RefreshCashWithdrawUserAccount);
    }

    // 计算init状态的记录倒计时
    public long getCurrentCountDown() 
    {
        long countdown = 0;
        foreach(CashRedeem item in CashRedeemList)
        {
            if (item.state == Redeem.RedeemState.Init)
            {
                countdown = item.endTime - DateUtil.Current();
                break;
            }
        }

        return countdown;
    }

    // 是否有未点击提现的记录
    public bool hasUncheckedItem()
    {
        foreach(CashRedeem item in CashRedeemList)
        {
            if(item.state == Redeem.RedeemState.Unchecked || item.state == Redeem.RedeemState.Init && item.canCheckout)
            {
                return true;
            }
        }
        return false;
    }
}
