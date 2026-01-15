using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zeta_framework;

public class ScoreWhipSwear : SnowUIPlace
{
    public static ScoreWhipSwear Instance;
[UnityEngine.Serialization.FormerlySerializedAs("CloseButton")]
    public Button FirstGibbon;
[UnityEngine.Serialization.FormerlySerializedAs("GiftItems")]    public List<WhipAfarUI> WhipRural;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        FirstGibbon.onClick.AddListener(() => {
            FirstUIMode(GetType().Name);
            //ColonizeTautPlusRancher.Instance.OpenNext();
        });
    }

    public override void Display(object uiFormParams)
    {
        base.Display(uiFormParams);

        DeafLine();
    }

    private void DeafLine()
    {
        ActivityState activityState = ColonizeScoreWhipPeak.Instance.Widow;    // 活动状态
        int collectedIndex = ColonizeScoreWhipPeak.Instance.FarVoyagerDodge();    // 当前是第几天
        // 如果今天已经领取，比如Day1，collectedIndex == 1，activityState = Finish
        // 如果今天未领取，比如Day1，当前collectedIndex == 0， activityState = Attending
        for (int i = 0; i < WhipRural.Count; i++)
        {
            WhipAfarUI Seal= WhipRural[i];
            int state;  // 第i天奖励状态：1：未领取；2：已领取；3：待领取
            if (i > collectedIndex)
            {
                state = 1;
            }
            else if (i < collectedIndex)
            {
                state = 2;
            }
            else
            {
                state = activityState == ActivityState.Attending ? 3 : 1;
            }
            Seal.Deaf(i, state);
        }
    }

    public void WrongBritain(List<AfarPerry> rewards)
    {
        // 显示领奖效果
        WoolSwear.Instance.BindDynamicSteamship(rewards, () =>
        {
            //ColonizeTautPlusRancher.Instance.OpenNext();
        });
        // 关闭当前窗口
        FirstUIMode(GetType().Name);
    }
    
}
