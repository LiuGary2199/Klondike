using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;


/// <summary> 引导面板 </summary>
public class InferSwear : SnowUIPlace
{
    Image Ocher;
[UnityEngine.Serialization.FormerlySerializedAs("Center")]    public RectTransform Extend;
[UnityEngine.Serialization.FormerlySerializedAs("Hand")]    public Transform Hope;
[UnityEngine.Serialization.FormerlySerializedAs("InfoBg")]    public Transform BoldBg;
[UnityEngine.Serialization.FormerlySerializedAs("Info")]    public Text Bold;
    [HideInInspector] [UnityEngine.Serialization.FormerlySerializedAs("GuideIndex")]public int InferIndex;
[UnityEngine.Serialization.FormerlySerializedAs("NextBtn")]    public Button OpenGem;
    Coroutine BoldSlumIE;


    public override void Display(object uiFormParams)
    {
        base.Display(uiFormParams);
        Ocher = GetComponent<Image>();
        Leaf(true);
    }

    public void BindVerb(Transform Target)
    {
        RectTransform TargetRect = Target.GetComponent<RectTransform>();
        BindVerb(TargetRect.position, TargetRect.sizeDelta * Target.localScale);
    }
    public void BindVerb(Vector2 Pos, Vector2 Size)
    {
        Ocher.raycastTarget = true;
        Extend.DOKill();
        Extend.DOMove(Pos, .5f);
        Extend.DOSizeDelta(Size, .7f).OnComplete(() =>
        {
            Ocher.raycastTarget = false;
        });
    }

    public void BindHope(Vector2[] Poss)
    {
        Hope.DOKill();
        Hope.gameObject.SetActive(true);
        Hope.position = Poss[0];
        HopeTire(Poss, 0);
    }
    void HopeTire(Vector2[] Poss, int Index)
    {
        Hope.DOMove(Poss[Index], .4f).SetEase(Ease.Linear).OnComplete(() =>
        {
            if (Hope.gameObject.activeSelf)
            {
                if (Index < Poss.Length - 1)
                    HopeTire(Poss, Index + 1);
                else
                    HopeTire(Poss, 0);
            }
        });
    }

    public void BindBold(string Text, int OffsetY = -300)
    {
        Bold.DOKill();
        BoldBg.DOKill();
        Bold.text = "";
        BoldBg.gameObject.SetActive(true);
        BoldBg.localScale = new Vector2(0, 1);
        BoldBg.DOScale(Vector2.one, .3f);
        Bold.DOText(Text, .3f).SetDelay(.3f);

        if (BoldSlumIE != null)
            StopCoroutine(BoldSlumIE);
        if (Extend.localPosition.y - BoldBg.localPosition.y != OffsetY)
            BoldSlumIE = StartCoroutine(nameof(BoldSlum), OffsetY);
    }
    IEnumerator BoldSlum(int PosY)
    {
        Vector2 UpdatePos = BoldBg.localPosition;
        Vector2 TargetPos = new Vector2(0, Extend.localPosition.y + PosY);
        while (true)
        {
            yield return null;
            TargetPos.y = Extend.localPosition.y + PosY;
            UpdatePos.y = Mathf.Lerp(UpdatePos.y, TargetPos.y, Time.deltaTime * 10);
            BoldBg.localPosition = UpdatePos;
            if (Mathf.Abs(UpdatePos.y - TargetPos.y) < 1)
                break;
        }
    }

    public void BindOpenGem(Action OnBtnClick)
    {
        OpenGem.onClick.RemoveAllListeners();
        OpenGem.onClick.AddListener(() =>
        {
            OpenGem.onClick.RemoveAllListeners();
            OnBtnClick();
        });
        OpenGem.gameObject.SetActive(true);
    }

    public void Leaf(bool IsBlcok)
    {
        Hope.DOKill();
        Bold.DOKill();
        BoldBg.DOKill();
        Extend.DOKill();
        Hope.gameObject.SetActive(false);
        Bold.text = "";
        BoldBg.gameObject.SetActive(false);
        OpenGem.gameObject.SetActive(false);
        Extend.localPosition = Vector2.zero;
        Extend.sizeDelta = Vector2.one * 3000;
        Ocher.raycastTarget = IsBlcok;
    }

}
