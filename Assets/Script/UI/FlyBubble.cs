using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary> 飞行气泡 </summary>
public class FlyBubble : MonoBehaviour
{
    public int FlyX = 1100;
    public int TopY = 300;
    public int BottomY = -220;
    public int OffectY = 200;
    public int AnimTime = 10;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            //MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Sound_Bubble);
            OnBubbleOnClick();
        });
    }

    public void Fly()
    {
        gameObject.SetActive(true);
        //左上→右上
        transform.localPosition = new Vector3(-FlyX, TopY, 0);
        transform.DOLocalMoveY(TopY - OffectY, 1).SetLoops(AnimTime, LoopType.Yoyo).SetEase(Ease.Linear);
        transform.DOLocalMoveX(FlyX, AnimTime).SetEase(Ease.Linear).OnComplete(() =>
        {
            transform.DOKill();
            //右下→左下
            transform.localPosition = new Vector3(FlyX, BottomY, 0);
            transform.DOLocalMoveY(BottomY + OffectY, 1).SetLoops(AnimTime, LoopType.Yoyo).SetEase(Ease.Linear);
            transform.DOLocalMoveX(-FlyX, AnimTime).SetEase(Ease.Linear).OnComplete(() =>
            {
                transform.DOKill();
                //左上→右上
                transform.localPosition = new Vector3(-FlyX, TopY, 0);
                transform.DOLocalMoveY(TopY - OffectY, 1).SetLoops(AnimTime, LoopType.Yoyo).SetEase(Ease.Linear);
                transform.DOLocalMoveX(FlyX, AnimTime).SetEase(Ease.Linear).OnComplete(() =>
                {
                    transform.DOKill();
                    gameObject.SetActive(false);
                });
            });
        });
    }

    void OnBubbleOnClick()
    {
        UIManager.GetInstance().ShowUIForms(nameof(FlyBubbleRewardPanel));
        transform.DOKill();
        gameObject.SetActive(false);
        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.SFX_FlyBubble);
        PostEventScript.GetInstance().SendEvent("1016");
    }
}
