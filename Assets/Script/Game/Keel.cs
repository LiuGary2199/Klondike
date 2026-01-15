using System.Collections;
using System.Collections.Generic;
using Coffee.UIExtensions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

/// <summary> 纸牌控制类 </summary>
public class Keel : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [HideInInspector][UnityEngine.Serialization.FormerlySerializedAs("Data")] public CardData Line;
    [UnityEngine.Serialization.FormerlySerializedAs("SuitImage")] public Image LeapFifth;
    [UnityEngine.Serialization.FormerlySerializedAs("LittleSuitImage")] public Image HiccupLeapFifth;
    [UnityEngine.Serialization.FormerlySerializedAs("ValueText")] public Text DodgeMoss;
    [UnityEngine.Serialization.FormerlySerializedAs("BackImage")] public GameObject SortFifth;

    private Vector3 SexuallyCitation;
    private Transform SexuallyQuasar;
    private List<Keel> OvercastBrass = new List<Keel>();
    private bool ByDecision = false; // 添加拖拽状态标记
    [UnityEngine.Serialization.FormerlySerializedAs("SpecialEffect")] public UIParticle BrieflyGuinea;
    [UnityEngine.Serialization.FormerlySerializedAs("Special_ScratchCard")] public GameObject Briefly_ThriftyKeel;
    [UnityEngine.Serialization.FormerlySerializedAs("Special_LuckyWheel")] public GameObject Briefly_AwaitFaith;
    [UnityEngine.Serialization.FormerlySerializedAs("Special_Cash")] public GameObject Briefly_Note;

    public void Deaf(CardData data)
    {
        Line = data;
        LeapFifth.sprite = Freezing_Rancher.Instance.FarLeapReview(data);
        HiccupLeapFifth.sprite = Freezing_Rancher.Instance.FarLeapReview(data, true);
        DodgeMoss.text = Freezing_Rancher.Instance.DodgeVogue[(int)data.Value];
        DodgeMoss.color = data.UpCod() ? Color.red : Color.black;
        SortFifth.SetActive(!data.UpHighOf);
        transform.name = Freezing_Rancher.Instance.FarKeelPartnerStew(data);
    }

    // 点击操作
    public void OnPointerClick(PointerEventData eventData)
    {
        // 如果正在赢牌收集阶段 不执行点击操作
        if (Freezing_Rancher.Instance.UpUrnExorbitant)
            return;

        // 如果正在拖拽 不执行点击操作
        if (ByDecision)
            return;

        // 隐藏提示
        KickSwear.Instance.SendRotVole();

        // 正面朝上的牌，尝试自动移动
        if (Line.UpHighOf)
        {
            // 检查是否是一组牌（下部接龙区的连续正面朝上牌）
            var OvercastBrass = Freezing_Rancher.Instance.FarDecisionBrass(this);
            if (OvercastBrass.Count > 1)
            {
                // 是一组牌，尝试自动移动到下部接龙区尾部
                Freezing_Rancher.Instance.TautSlumKeelRobin(this);
            }
            else
            {
                // 单张牌，尝试自动移动到最佳位置
                Freezing_Rancher.Instance.TautSlumKeel(this);
            }
        }

        LeafyLeg.FarBefriend().HaulBalance(Lofelt.NiceVibrations.HapticPatterns.PresetType.LightImpact);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // 如果正在赢牌收集阶段 不执行点击操作
        if (Freezing_Rancher.Instance.UpUrnExorbitant)
            return;

        if (!Line.UpHighOf)
            return;

        // 隐藏提示
        KickSwear.Instance.SendRotVole();

        ByDecision = true; // 开始拖拽
        OvercastBrass = Freezing_Rancher.Instance.FarDecisionBrass(this);
        foreach (var card in OvercastBrass)
        {
            card.SexuallyCitation = card.transform.position;
            card.SexuallyQuasar = card.transform.parent;
            card.transform.SetParent(KickSwear.Instance.KeelQuasar_Waxy); // 拖动时
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 如果正在赢牌收集阶段 不执行点击操作
        if (Freezing_Rancher.Instance.UpUrnExorbitant)
            return;
        Debug.Log("AdSceneOnDrag" + eventData);
        if (!Line.UpHighOf)
            return;
        Debug.Log("AdSceneOnDrag" + eventData);
        //Vector3 mousePos = Input.mousePosition;
        Vector3 mousePos = AdSceneManager.Instance.GetMousePos();

        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        mousePos.z = 0;
        for (int i = 0; i < OvercastBrass.Count; i++)
        {
            OvercastBrass[i].transform.position = mousePos + new Vector3(0, -i * Freezing_Rancher.Instance.Inn_Y);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 如果正在赢牌收集阶段 不执行点击操作
        if (Freezing_Rancher.Instance.UpUrnExorbitant)
            return;

        if (!Line.UpHighOf)
            return;
        // 检测拖拽目标
        Keel targetCard = FarLondonKeelChinaMouse();
        bool moved = false;

        // 优先检查上部接龙区
        int upAreaIdx = Freezing_Rancher.Instance.FarOfPeckDodgeChinaBefit(OvercastBrass);
        if (upAreaIdx != -1 && Freezing_Rancher.Instance.TheSlumBeOfPeck(this, upAreaIdx))
        {
            moved = Freezing_Rancher.Instance.SlumKeelBeOfPeck(this, upAreaIdx);
            //上部接龙区只能放单张牌
            OvercastBrass[0].transform.SetParent(KickSwear.Instance.KeelQuasar_OfPeck);
        }
        // 然后检查下部接龙区叠放
        else if (targetCard != null && Freezing_Rancher.Instance.TheRobinDy(this, targetCard))
        {
            moved = Freezing_Rancher.Instance.SlumKeelRobin(this, targetCard);
            // 移动到下部接龙区
            foreach (var card in OvercastBrass)
                card.transform.SetParent(KickSwear.Instance.KeelQuasar_PinkPeck);
        }
        // 最后检查空下部接龙区
        else
        {
            int emptyDownIdx = Freezing_Rancher.Instance.FarStashPinkPeckDodgeChinaBefit(OvercastBrass);
            if (emptyDownIdx != -1 && Freezing_Rancher.Instance.TheSlumBeStashPinkPeck(this))
            {
                moved = Freezing_Rancher.Instance.SlumKeelRobinBeStashPinkPeck(this, emptyDownIdx);
                foreach (var card in OvercastBrass)
                    card.transform.SetParent(KickSwear.Instance.KeelQuasar_PinkPeck);
            }
        }

        if (!moved)
        {
            // 回到原位
            foreach (var card in OvercastBrass)
            {
                card.transform.position = card.SexuallyCitation;
                card.transform.SetParent(card.SexuallyQuasar);
            }
        }
        OvercastBrass.Clear();

        // 延迟重置拖拽状态，避免立即触发点击
        StartCoroutine(BurstDecisionWidow());
    }

    // 延迟重置拖拽状态
    private IEnumerator BurstDecisionWidow()
    {
        yield return new WaitForEndOfFrame();
        ByDecision = false;
    }



    // 鼠标下的Card
    private Keel FarLondonKeelChinaMouse()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = AdSceneManager.Instance.GetMousePos()
        };
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);
        foreach (var result in results)
        {
            Keel card = result.gameObject.GetComponent<Keel>();
            if (card != null && card != this)
            {
                // 排除正在拖动的牌
                bool ByDecision = false;
                foreach (var draggingCard in OvercastBrass)
                {
                    if (card == draggingCard)
                    {
                        ByDecision = true;
                        break;
                    }
                }
                if (!ByDecision)
                    return card;
            }
        }
        return null;
    }

    public void BindBriefly(string SpecialType)
    {
        BrieflyGuinea.Play();
        if (SpecialType == "ScratchCard")
        {
            //Special_ScratchCard.SetActive(true);
            BindBrieflyLug(Briefly_ThriftyKeel);
        }
        else if (SpecialType == "LuckyWheel")
        {
            //Special_LuckyWheel.SetActive(true);
            BindFaithLug(Briefly_AwaitFaith);
        }
        else if (SpecialType == "Money")
        {
            //Special_Cash.SetActive(true);
            BindBrieflyLug(Briefly_Note);
        }

        MoldRancher.FarBefriend().Award(1, () =>
        {
            Briefly_ThriftyKeel.SetActive(false);
            Briefly_AwaitFaith.SetActive(false);
            Briefly_Note.SetActive(false);
        });
    }

    public void BindBrieflyLug(GameObject Afar)
    {
        GameObject AniItem = Instantiate(Afar, UIRancher.FarBefriend()._Cup.transform);
        AniItem.SetActive(true);
        AniItem.transform.position = Afar.transform.position;
        AniItem.transform.localScale = new Vector3(0, 0, 0);
        Vector3 EndPos = new Vector3(0, 0, 0);
        Vector3 midPos = new Vector3(Afar.transform.position.x, EndPos.y, 0);
        Vector3[] Specialpath = BezierUtils.GetBeizerList(Afar.transform.position, midPos, EndPos, 6);
        AniItem.transform.DOScale(2.5f, 0.7f).SetEase(Ease.OutBack);
        AniItem.transform.DOPath(Specialpath, 0.7f, PathType.CatmullRom).OnComplete(() =>
        {
            AniItem.transform.DOScale(0, 0.2f).SetEase(Ease.InBack).SetDelay(0.3f).OnComplete(() =>
            {

                Destroy(AniItem);
            });
        });
    }

    public void BindFaithLug(GameObject Afar)
    {
        GameObject AniItem = Instantiate(Afar, UIRancher.FarBefriend()._Cup.transform);
        AniItem.SetActive(true);
        AniItem.transform.position = Afar.transform.position;
        AniItem.transform.localScale = new Vector3(0, 0, 0);
        Vector3 EndPos = new Vector3(0, 0, 0);
        Vector3 midPos = new Vector3(Afar.transform.position.x, EndPos.y, 0);
        Vector3[] Specialpath = BezierUtils.GetBeizerList(Afar.transform.position, midPos, EndPos, 6);
        AniItem.transform.DOScale(2.5f, 0.7f).SetEase(Ease.OutBack);
        AniItem.transform.DOPath(Specialpath, 0.7f, PathType.CatmullRom).OnComplete(() =>
        {
            AniItem.transform.DOScale(1, 0.5f).SetDelay(0.3f).SetEase(Ease.InBack);
            AniItem.transform.DOMove(KickSwear.Instance.AwaitFaith_Gene.transform.position, 0.5f).SetDelay(0.3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                GameObject ParticleMagic = Instantiate(KickSwear.Instance.TaintGuinea, KickSwear.Instance.transform);
                ParticleMagic.transform.position = KickSwear.Instance.AwaitFaith_Gene.transform.position;
                ParticleMagic.SetActive(true);
                Destroy(AniItem);

                KickSwear.Instance.CudAwaitFaithGene();
            });
        });
    }
}