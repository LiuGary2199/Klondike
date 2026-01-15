using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Redeem
{
    // 提现状态 初始化、未提现、任务、排队、完成
    public enum RedeemState { Init, Unchecked, Checked, Waiting, Complete };

    public int id;
    public double cashout;          // 提现金额
    public RedeemState state;     // 状态
    public int rank;                // 排队顺序
    public long lastUpdateRankTime; // 上次修改排队时间
    public UserAccount userAccount; // 用户账户
    public List<CheckoutTask> checkoutTasks;
    public int currentTaskIndex;
    public CheckoutTask currentTask;

    public object Clone()
    {
        return this.MemberwiseClone();
    }


    /// <summary>
    /// 增加任务值
    /// </summary>
    /// <param name="type"></param>
    /// <param name="addValue"></param>
    /// <returns>完成所有任务</returns>
    public bool AddTaskValue(string type, double addValue)
    {
        // 没有任务，直接返回
        if (checkoutTasks == null || checkoutTasks.Count == 0)
        {
            return true;
        }

        if (currentTaskIndex == -1)
        {
            currentTaskIndex = 0;
            currentTask = checkoutTasks[currentTaskIndex];
        }

        if (currentTask.type.Equals(type))
        {
            bool finishCurrentTask = false; // 是否完成了当前任务
            if(type.Equals("Time"))
            {
                if (currentTask.value + currentTask.task_num <= DateUtil.Current())
                {
                    finishCurrentTask = true;
                }
            }
            else
            {
                if (currentTask.is_static == 1)
                {
                    currentTask.value = SOHOShopDataManager.instance.GetStaticCashoutTaskValue(type);
                }
                else
                {
                    currentTask.value += addValue;
                }
                // 完成当前任务
                if (currentTask.value >= currentTask.task_num)
                {
                    finishCurrentTask = true;
                }
            }
            if (finishCurrentTask)
            {
                if (currentTaskIndex == checkoutTasks.Count - 1)
                {
                    // 如果该提现记录完成了所有任务，进入排队阶段
                    return true;
                }
                else
                {
                    currentTaskIndex++;
                    currentTask = checkoutTasks[currentTaskIndex];
                    // 如果是时间类型任务，value记录任务开始时间（当前时间）
                    if (currentTask.type.Equals("Time"))
                    {
                        currentTask.value = DateUtil.Current();
                    }
                    else if (currentTask.is_static == 1)
                    {
                        // 如果是静态任务，可能任务已经完成，所以重新判断是否所有任务都已完成
                        return AddTaskValue(currentTask.type, 0);
                    }
                }
            }
        }
        return false;
    }
}

/// <summary>
/// 提现任务
/// </summary>
public class CheckoutTask
{
    public string type;
    public double task_num;
    public double value;
    public string desc;
    public int is_static;   // 是否为静态任务，静态任务是指所有提现记录中的该任务值统一使用游戏中的某个值（例如，游戏关卡数），静态任务修改值使用SOHOShopManager.SetTaskValue

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}

