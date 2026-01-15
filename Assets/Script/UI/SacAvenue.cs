using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary> 飞行气泡 </summary>
public class SacAvenue : MonoBehaviour
{
[UnityEngine.Serialization.FormerlySerializedAs("FlyX")]    public int SacX= 1100;
[UnityEngine.Serialization.FormerlySerializedAs("TopY")]    public int CupY= 300;
[UnityEngine.Serialization.FormerlySerializedAs("BottomY")]    public int GrottoY= -220;
[UnityEngine.Serialization.FormerlySerializedAs("OffectY")]    public int EnamelY= 200;
[UnityEngine.Serialization.FormerlySerializedAs("AnimTime")]    public int TireMold= 10;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            //LeafyLeg.GetInstance().PlayEffect(LeafyJoke.UIMusic.Sound_Bubble);
            OnBubbleOnClick();
        });
    }

    public void Sac()
    {
        gameObject.SetActive(true);
        //左上→右上
        transform.localPosition = new Vector3(-SacX, CupY, 0);
        transform.DOLocalMoveY(CupY - EnamelY, 1).SetLoops(TireMold, LoopType.Yoyo).SetEase(Ease.Linear);
        transform.DOLocalMoveX(SacX, TireMold).SetEase(Ease.Linear).OnComplete(() =>
        {
            transform.DOKill();
            //右下→左下
            transform.localPosition = new Vector3(SacX, GrottoY, 0);
            transform.DOLocalMoveY(GrottoY + EnamelY, 1).SetLoops(TireMold, LoopType.Yoyo).SetEase(Ease.Linear);
            transform.DOLocalMoveX(-SacX, TireMold).SetEase(Ease.Linear).OnComplete(() =>
            {
                transform.DOKill();
                //左上→右上
                transform.localPosition = new Vector3(-SacX, CupY, 0);
                transform.DOLocalMoveY(CupY - EnamelY, 1).SetLoops(TireMold, LoopType.Yoyo).SetEase(Ease.Linear);
                transform.DOLocalMoveX(SacX, TireMold).SetEase(Ease.Linear).OnComplete(() =>
                {
                    transform.DOKill();
                    gameObject.SetActive(false);
                });
            });
        });
    }

    void OnBubbleOnClick()
    {
        UIRancher.FarBefriend().BindUIPlace(nameof(SacAvenueStarveSwear));
        transform.DOKill();
        gameObject.SetActive(false);
        LeafyLeg.FarBefriend().HaulGuinea(LeafyJoke.UIMusic.SFX_FlyBubble);
        WhimInuitRemove.FarBefriend().LeafInuit("1016");
    }
}
