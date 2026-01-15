using System.Collections;
using System.Collections.Generic;
using Coffee.UIExtensions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

/// <summary> 纸牌控制类 </summary>
public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [HideInInspector] public CardData Data;
    public Image SuitImage;
    public Image LittleSuitImage;
    public Text ValueText;
    public GameObject BackImage;

    private Vector3 originalPosition;
    private Transform originalParent;
    private List<Card> draggingCards = new List<Card>();
    private bool isDragging = false; // 添加拖拽状态标记
    public UIParticle SpecialEffect;
    public GameObject Special_ScratchCard;
    public GameObject Special_LuckyWheel;
    public GameObject Special_Cash;


    public void Init(CardData data)
    {
        Data = data;
        SuitImage.sprite = Klondike_Manager.Instance.GetSuitSprite(data);
        LittleSuitImage.sprite = Klondike_Manager.Instance.GetSuitSprite(data, true);
        ValueText.text = Klondike_Manager.Instance.ValueTexts[(int)data.Value];
        ValueText.color = data.IsRed() ? Color.red : Color.black;
        BackImage.SetActive(!data.IsFaceUp);
        transform.name = Klondike_Manager.Instance.GetCardDisplayName(data);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // 如果正在赢牌收集阶段 不执行点击操作
        if (Klondike_Manager.Instance.IsWinCollecting)
            return;

        if (!Data.IsFaceUp)
            return;

        // 隐藏提示
        GamePanel.Instance.HideAllTips();

        isDragging = true; // 开始拖拽
        draggingCards = Klondike_Manager.Instance.GetDraggingCards(this);
        foreach (var card in draggingCards)
        {
            card.originalPosition = card.transform.position;
            card.originalParent = card.transform.parent;
            card.transform.SetParent(GamePanel.Instance.CardParent_Drag); // 拖动时
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 如果正在赢牌收集阶段 不执行点击操作
        if (Klondike_Manager.Instance.IsWinCollecting)
            return;

        if (!Data.IsFaceUp)
            return;
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        mousePos.z = 0;
        for (int i = 0; i < draggingCards.Count; i++)
        {
            draggingCards[i].transform.position = mousePos + new Vector3(0, -i * Klondike_Manager.Instance.Gap_Y);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 如果正在赢牌收集阶段 不执行点击操作
        if (Klondike_Manager.Instance.IsWinCollecting)
            return;

        if (!Data.IsFaceUp)
            return;
        // 检测拖拽目标
        Card targetCard = GetTargetCardUnderMouse();
        bool moved = false;

        // 优先检查上部接龙区
        int upAreaIdx = Klondike_Manager.Instance.GetUpAreaIndexUnderMouse(draggingCards);
        if (upAreaIdx != -1 && Klondike_Manager.Instance.CanMoveToUpArea(this, upAreaIdx))
        {
            moved = Klondike_Manager.Instance.MoveCardToUpArea(this, upAreaIdx);
            //上部接龙区只能放单张牌
            draggingCards[0].transform.SetParent(GamePanel.Instance.CardParent_UpArea);
        }
        // 然后检查下部接龙区叠放
        else if (targetCard != null && Klondike_Manager.Instance.CanStackOn(this, targetCard))
        {
            moved = Klondike_Manager.Instance.MoveCardStack(this, targetCard);
            // 移动到下部接龙区
            foreach (var card in draggingCards)
                card.transform.SetParent(GamePanel.Instance.CardParent_DownArea);
        }
        // 最后检查空下部接龙区
        else
        {
            int emptyDownIdx = Klondike_Manager.Instance.GetEmptyDownAreaIndexUnderMouse(draggingCards);
            if (emptyDownIdx != -1 && Klondike_Manager.Instance.CanMoveToEmptyDownArea(this))
            {
                moved = Klondike_Manager.Instance.MoveCardStackToEmptyDownArea(this, emptyDownIdx);
                foreach (var card in draggingCards)
                    card.transform.SetParent(GamePanel.Instance.CardParent_DownArea);
            }
        }

        if (!moved)
        {
            // 回到原位
            foreach (var card in draggingCards)
            {
                card.transform.position = card.originalPosition;
                card.transform.SetParent(card.originalParent);
            }
        }
        draggingCards.Clear();

        // 延迟重置拖拽状态，避免立即触发点击
        StartCoroutine(ResetDraggingState());
    }

    // 延迟重置拖拽状态
    private IEnumerator ResetDraggingState()
    {
        yield return new WaitForEndOfFrame();
        isDragging = false;
    }

    // 点击操作
    public void OnPointerClick(PointerEventData eventData)
    {
        // 如果正在赢牌收集阶段 不执行点击操作
        if (Klondike_Manager.Instance.IsWinCollecting)
            return;

        // 如果正在拖拽 不执行点击操作
        if (isDragging)
            return;

        // 隐藏提示
        GamePanel.Instance.HideAllTips();

        // 正面朝上的牌，尝试自动移动
        if (Data.IsFaceUp)
        {
            // 检查是否是一组牌（下部接龙区的连续正面朝上牌）
            var draggingCards = Klondike_Manager.Instance.GetDraggingCards(this);
            if (draggingCards.Count > 1)
            {
                // 是一组牌，尝试自动移动到下部接龙区尾部
                Klondike_Manager.Instance.AutoMoveCardStack(this);
            }
            else
            {
                // 单张牌，尝试自动移动到最佳位置
                Klondike_Manager.Instance.AutoMoveCard(this);
            }
        }
    }

    // 鼠标下的Card
    private Card GetTargetCardUnderMouse()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);
        foreach (var result in results)
        {
            Card card = result.gameObject.GetComponent<Card>();
            if (card != null && card != this)
            {
                // 排除正在拖动的牌
                bool isDragging = false;
                foreach (var draggingCard in draggingCards)
                {
                    if (card == draggingCard)
                    {
                        isDragging = true;
                        break;
                    }
                }
                if (!isDragging)
                    return card;
            }
        }
        return null;
    }

    public void ShowSpecial(string SpecialType)
    {
        SpecialEffect.Play();
        if (SpecialType == "ScratchCard")
        {
            //Special_ScratchCard.SetActive(true);
            ShowSpecialAni(Special_ScratchCard);
        }
        else if (SpecialType == "LuckyWheel")
        {
            //Special_LuckyWheel.SetActive(true);
            ShowWheelAni(Special_LuckyWheel);
        }
        else if (SpecialType == "Money")
        {
            //Special_Cash.SetActive(true);
            ShowSpecialAni(Special_Cash);
        }

        TimeManager.GetInstance().Delay(1, () =>
        {
            Special_ScratchCard.SetActive(false);
            Special_LuckyWheel.SetActive(false);
            Special_Cash.SetActive(false);
        });
    }

    public void ShowSpecialAni(GameObject Item)
    {
        GameObject AniItem = Instantiate(Item, UIManager.GetInstance()._Top.transform);
        AniItem.SetActive(true);
        AniItem.transform.position = Item.transform.position;
        AniItem.transform.localScale = new Vector3(0, 0, 0);
        Vector3 EndPos = new Vector3(0, 0, 0);
        Vector3 midPos = new Vector3(Item.transform.position.x, EndPos.y, 0);
        Vector3[] Specialpath = BezierUtils.GetBeizerList(Item.transform.position, midPos, EndPos, 6);
        AniItem.transform.DOScale(2.5f, 0.7f).SetEase(Ease.OutBack);
        AniItem.transform.DOPath(Specialpath, 0.7f, PathType.CatmullRom).OnComplete(() =>
        {
            AniItem.transform.DOScale(0, 0.2f).SetEase(Ease.InBack).SetDelay(0.3f).OnComplete(() =>
            {

                Destroy(AniItem);
            });
        });
    }

    public void ShowWheelAni(GameObject Item)
    {
        GameObject AniItem = Instantiate(Item, UIManager.GetInstance()._Top.transform);
        AniItem.SetActive(true);
        AniItem.transform.position = Item.transform.position;
        AniItem.transform.localScale = new Vector3(0, 0, 0);
        Vector3 EndPos = new Vector3(0, 0, 0);
        Vector3 midPos = new Vector3(Item.transform.position.x, EndPos.y, 0);
        Vector3[] Specialpath = BezierUtils.GetBeizerList(Item.transform.position, midPos, EndPos, 6);
        AniItem.transform.DOScale(2.5f, 0.7f).SetEase(Ease.OutBack);
        AniItem.transform.DOPath(Specialpath, 0.7f, PathType.CatmullRom).OnComplete(() =>
        {
            AniItem.transform.DOScale(1, 0.5f).SetDelay(0.3f).SetEase(Ease.InBack);
            AniItem.transform.DOMove(GamePanel.Instance.LuckyWheel_Fill.transform.position, 0.5f).SetDelay(0.3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                GameObject ParticleMagic = Instantiate(GamePanel.Instance.StarsEffect, GamePanel.Instance.transform);
                ParticleMagic.transform.position = GamePanel.Instance.LuckyWheel_Fill.transform.position;
                ParticleMagic.SetActive(true);
                Destroy(AniItem);

                GamePanel.Instance.SetLuckyWheelFill();
            });
        });
    }
}