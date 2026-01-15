using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using zeta_framework;

/// <summary>
/// 活动自动打开弹窗管理
/// </summary>
public class ActivityAutoOpenManager : MonoBehaviour
{
    public static ActivityAutoOpenManager Instance;

    private List<string> openPanelList; // 自动打开弹窗列表
    private Action cb;

    // Start is called before the first frame update
    public void Start()
    {
        Instance = this;
    }

    public void OpenPanel(int auto_open_time, Action cb = null)
    {
        openPanelList = new();
        
        // 遍历所有活动，查看当前时机需要自动打开的弹窗
        List<Activity> activities = ActivityCtrl.Instance.GetActivities();
        activities.Sort((a, b) => a.auto_open_priority.CompareTo(b.auto_open_priority));
        foreach (Activity activity in activities)
        {
            if (activity == null || string.IsNullOrEmpty(activity.panel) || string.IsNullOrEmpty(activity.auto_open_time)) 
            {  
                continue;
            }

            if (activity.auto_open_time.Contains(auto_open_time.ToString())) {
                if (activity.State == ActivityState.NotAttend || activity.State == ActivityState.Attending || activity.State == ActivityState.NeedSettlement)
                {
                    // 仅活动状态为未参加、进行中、未结算时，可以打开弹窗
                    openPanelList.Add(activity.panel);
                }
            }
        }
        this.cb = cb;

        OpenNext();
    }

    public void OpenNext()
    {
        if (openPanelList.Count > 0)
        {
            string panelName = openPanelList[0];
            StartCoroutine(OpenPanel(panelName));
            openPanelList.RemoveAt(0);
        }
        else
        {
            cb?.Invoke();
        }
    }

    private IEnumerator OpenPanel(string panelName)
    {
        yield return new WaitForEndOfFrame();
        UIManager.GetInstance().ShowUIForms(panelName);
    }
}
