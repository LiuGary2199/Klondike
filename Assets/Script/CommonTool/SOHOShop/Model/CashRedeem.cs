using System;
using System.Collections.Generic;

/// <summary>
/// 绿币提现记录
/// </summary>
public class CashRedeem : Redeem
{
    public long startTime;          // 本次提现开始时间
    public long endTime;            // 本次提现结束时间
    public List<CheckoutCondition> preConditions;    // 提现条件
    public bool canCheckout;        // 满足提现条件

    public CashRedeem()
    {
        state = RedeemState.Init;
        userAccount = SOHOShopDataManager.instance.currentUserAccount;
        if (SOHOShopDataManager.instance.shopJson.cash_checkout_conditions != null && SOHOShopDataManager.instance.shopJson.cash_checkout_conditions.Length > 0)
        {
            preConditions = new List<CheckoutCondition>();
            foreach (CheckoutCondition condition in SOHOShopDataManager.instance.shopJson.cash_checkout_conditions)
            {
                preConditions.Add((CheckoutCondition)condition.Clone());
            }
        }
        if (SOHOShopDataManager.instance.shopJson.cash_checkout_tasks != null && SOHOShopDataManager.instance.shopJson.cash_checkout_tasks.Length > 0)
        {
            checkoutTasks = new List<CheckoutTask>();
            foreach (CheckoutTask task in SOHOShopDataManager.instance.shopJson.cash_checkout_tasks)
            {
                CheckoutTask newTask = (CheckoutTask)task.Clone();
                if (newTask.is_static == 1)
                {
                    newTask.value = SOHOShopDataManager.instance.GetStaticCashoutTaskValue(newTask.type);
                }
                checkoutTasks.Add(newTask);
            }
            currentTaskIndex = 0;
            currentTask = checkoutTasks[0];
        }
    }

    public void Init()
    {
        if (checkoutTasks != null)
        {
            foreach(CheckoutTask task in checkoutTasks)
            {
                if (task.is_static == 1)
                {
                    task.value = SOHOShopDataManager.instance.GetStaticCashoutTaskValue(task.type);
                }
            }

            if (currentTask.is_static == 1)
            {
                currentTask.value = SOHOShopDataManager.instance.GetStaticCashoutTaskValue(currentTask.type);
            }
        }
    }

    /// <summary>
    /// 是否满足提现条件
    /// </summary>
    /// <returns></returns>
    public bool CheckCanCashout()
    {
        if (state != RedeemState.Init && state != RedeemState.Unchecked)
        {
            return false;
        }
        else if (state == RedeemState.Unchecked)
        {
            return true;
        }
        else
        {
            if (preConditions == null || preConditions.Count == 0)
            {
                double cashBalance = SaveDataManager.GetDouble(SOHOShopConst.sv_CashWithdrawBalance);
                if (DateUtil.Current() >= endTime && cashBalance > 0)
                {
                    UpdateState();
                    return true;
                }
            }
            else
            {
                foreach (CheckoutCondition condition in preConditions)
                {
                    if (condition.conditionType == CheckoutConditionType.Time)
                    {
                        if (DateUtil.Current() >= endTime)
                        {
                            UpdateState();
                            return true;
                        }
                    }
                    else if (condition.conditionType == CheckoutConditionType.Balance)
                    {
                        double cashBalance = SaveDataManager.GetDouble(SOHOShopConst.sv_CashWithdrawBalance);
                        if (cashBalance >= condition.condition_num)
                        {
                            UpdateState();
                            return true;
                        }
                    }
                    else if (condition.conditionType == CheckoutConditionType.Custom)
                    {
                        if (condition.value >= condition.condition_num)
                        {
                            UpdateState();
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    /// <summary>
    /// 修改自定义提现条件值
    /// </summary>
    /// <param name="name"></param>
    /// <param name="addValue"></param>
    public void AddPreConditionValue(string name, double addValue = 1)
    {
        if (state != RedeemState.Init)
        {
            return;
        }
        if (preConditions == null || preConditions.Count == 0)
        {
            return;
        }
        for (int i = 0; i < preConditions.Count; i++)
        {
            if (preConditions[i].conditionType == CheckoutConditionType.Custom && name.Equals(preConditions[i].name))
            {
                preConditions[i].value += addValue;
                if (preConditions[i].value >= preConditions[i].condition_num)
                {
                    UpdateState();
                }
            }
        }
    }

    private void UpdateState()
    {
        if (state != RedeemState.Init)
        {
            return;
        }

        double cashBalance = SaveDataManager.GetDouble(SOHOShopConst.sv_CashWithdrawBalance);
        if (cashBalance > 0)
        {
            canCheckout = true;
        }
        else
        {
            canCheckout = false;
        }
    }
}

public class CheckoutCondition
{
    public string type;  // 条件类型
    public CheckoutConditionType conditionType;
    public string name;         // 条件名称（用于自定义条件）
    public double condition_num;    // 条件数量
    public double value;           // 已完成数量
    public string desc;

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}

/// <summary>
/// 提现条件
/// </summary>
public enum CheckoutConditionType
{
    Time,       // 时间
    Balance,    // 提现金额
    Custom      // 用户自定义任务
}
