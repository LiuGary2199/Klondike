using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using zeta_framework;

/// <summary>
/// 活动自动打开弹窗管理
/// </summary>
public class ColonizeTautPlusRancher : MonoBehaviour
{
    public static ColonizeTautPlusRancher Instance;

    private List<string> UrgeSwearTrip; // 自动打开弹窗列表
    private Action Or;

    // Start is called before the first frame update
    public void Start()
    {
        Instance = this;
    }

    public void PlusSwear(int auto_open_time, Action cb = null)
    {
        UrgeSwearTrip = new();
        
        // 遍历所有活动，查看当前时机需要自动打开的弹窗
        List<Colonize> activities = ColonizePeak.Instance.FarNewsworthy();
        activities.Sort((a, b) => a.Seep_Urge_Fanciful.CompareTo(b.Seep_Urge_Fanciful));
        foreach (Colonize activity in activities)
        {
            if (activity == null || string.IsNullOrEmpty(activity.Hilly) || string.IsNullOrEmpty(activity.Seep_Urge_Copy)) 
            {  
                continue;
            }

            if (activity.Seep_Urge_Copy.Contains(auto_open_time.ToString())) {
                if (activity.Widow == ActivityState.NotAttend || activity.Widow == ActivityState.Attending || activity.Widow == ActivityState.NeedSettlement)
                {
                    // 仅活动状态为未参加、进行中、未结算时，可以打开弹窗
                    UrgeSwearTrip.Add(activity.Hilly);
                }
            }
        }
        this.Or = cb;

        PlusOpen();
    }

    public void PlusOpen()
    {
        if (UrgeSwearTrip.Count > 0)
        {
            string panelName = UrgeSwearTrip[0];
            StartCoroutine(PlusSwear(panelName));
            UrgeSwearTrip.RemoveAt(0);
        }
        else
        {
            Or?.Invoke();
        }
    }

    private IEnumerator PlusSwear(string panelName)
    {
        yield return new WaitForEndOfFrame();
        UIRancher.FarBefriend().BindUIPlace(panelName);
    }
}
