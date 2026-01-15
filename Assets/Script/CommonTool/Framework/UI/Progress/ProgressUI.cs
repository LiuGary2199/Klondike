using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI ;

public class ProgressUI : MonoBehaviour
{
    public Image ProgressImage;
    public Text ProgressText;

    

    public void RefreshProgress(int progress, int total, bool animation = true, System.Action cb = null)
    {
        ProgressText.text = progress + "/" + total;

        float newProgress = (float)progress / total;
        if (animation)
        {
            ProgressImage.DOFillAmount(newProgress, 0.8f).OnComplete(() => {
                cb?.Invoke();
            });
        } else
        {
            ProgressImage.fillAmount = newProgress;
            cb?.Invoke();
        }
    }
}
