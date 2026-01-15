using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zeta_framework;

public class BaseActivityPrefab : MonoBehaviour
{
    public string activity_id;
    public Text CountdownText;

    private Activity activity;

    // Start is called before the first frame update
    void Start()
    {
        activity = ActivityCtrl.Instance.GetActivityById<Activity>(activity_id);
        ChangeShowState();
        
        if (!string.IsNullOrEmpty(activity.panel) && GetComponent<Button>() != null)
        {
            GetComponent<Button>().onClick.AddListener(() => {
                UIManager.GetInstance().ShowUIForms(ActivityCtrl.Instance.GetActivityById<Activity>(activity_id).panel);
            });
        }

        // 监听活动状态变化，显示/隐藏图标
        MessageCenterLogic.GetInstance().Register(CConfig.mg_ActivityStateChange_ + activity_id, (md) => {
            ChangeShowState();
        });
    }

    /// <summary>
    /// 根据活动状态，确定是否显示当前活动prefab
    /// </summary>
    private void ChangeShowState()
    {
        gameObject.SetActive(activity.NeedShow());
        if (transform.parent.GetComponent<AutoLayoutVertical>() != null)
        {
            transform.parent.GetComponent<AutoLayoutVertical>().RefreshLayout();
        }

        if (CountdownText != null)
        {
            if(activity.State == ActivityState.Attending)
            {
                CountdownText.transform.parent.gameObject.SetActive(true);
                CountdownText.text = DateUtil.SecondsFormat2(activity.EndTime - DateUtil.Current());
            } 
            else if(activity.State == ActivityState.NeedSettlement)
            {
                CountdownText.transform.parent.gameObject.SetActive(true);
                CountdownText.text = "Claim";
            }
            else
            {
                CountdownText.transform.parent.gameObject.SetActive(false);
            }
        }
    }
}
