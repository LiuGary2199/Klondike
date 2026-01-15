using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI ;

public class ManeuverUI : MonoBehaviour
{
[UnityEngine.Serialization.FormerlySerializedAs("ProgressImage")]    public Image ManeuverFifth;
[UnityEngine.Serialization.FormerlySerializedAs("ProgressText")]    public Text ManeuverMoss;

    

    public void BalconyManeuver(int progress, int total, bool animation = true, System.Action cb = null)
    {
        ManeuverMoss.text = progress + "/" + total;

        float newProgress = (float)progress / total;
        if (animation)
        {
            ManeuverFifth.DOFillAmount(newProgress, 0.8f).OnComplete(() => {
                cb?.Invoke();
            });
        } else
        {
            ManeuverFifth.fillAmount = newProgress;
            cb?.Invoke();
        }
    }
}
