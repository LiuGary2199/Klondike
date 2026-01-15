using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zeta_framework;

public class LoveFordSwear : SnowUIPlace
{
[UnityEngine.Serialization.FormerlySerializedAs("RankItemPrefab")]    public GameObject LoveAfarQuiver;
[UnityEngine.Serialization.FormerlySerializedAs("RankItemContainer")]    public Transform LoveAfarAttention;
[UnityEngine.Serialization.FormerlySerializedAs("CloseButton")]    public Button FirstGibbon;
[UnityEngine.Serialization.FormerlySerializedAs("Bottom")]    public GameObject Grotto;
[UnityEngine.Serialization.FormerlySerializedAs("ClaimButton")]    public Button WrongGibbon;
[UnityEngine.Serialization.FormerlySerializedAs("MyRankItem")]    public GameObject MyLoveAfar;
[UnityEngine.Serialization.FormerlySerializedAs("Main")]    public Transform Limy;
[UnityEngine.Serialization.FormerlySerializedAs("Time")]    public GameObject Mold;
[UnityEngine.Serialization.FormerlySerializedAs("RankId")]    public string LoveAt;
[UnityEngine.Serialization.FormerlySerializedAs("RankItemHeight")]
    public float LoveAfarMilieu= 130;
    private List<LoveAfarUI> HailRural;
    private int FeedLoveDodge= 20;   // “我的”排名
    private Love rank;

    // Start is called before the first frame update
    void Start()
    {
        FirstGibbon.onClick.AddListener(() => {
            FirstUIMode(GetType().Name);
        });

        WrongGibbon.onClick.AddListener(() =>
        {
            List<AfarPerry> Dynamic= rank.Fertilizer();
            if (Dynamic != null && Dynamic.Count > 0)
            {
                
            }
            FirstUIMode(GetType().Name);
        });

        // 监听活动状态修改
        AfricanExtendHorse.FarBefriend().Iroquois(CAdjoin.We_ColonizeWidowUranus_ + LovePeak.Instance.FarLoveSoAt(LoveAt).Northern_To, (md) => {
            SolderColonizeWidow();
        });
    }

    public override void Display(object uiFormParams)
    {
        base.Display(uiFormParams);

        rank ??= LovePeak.Instance.FarLoveSoAt(LoveAt);
        rank.BasicallyLove();

        DeafLoveTrip();

        SolderColonizeWidow();
    }

    private void DeafLoveTrip()
    {
        MyLoveAfar.transform.SetParent(Limy);
        MyLoveAfar.transform.SetAsLastSibling();
        MyLoveAfar.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0);
        MyLoveAfar.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0);
        MyLoveAfar.transform.localScale = new Vector3(1, 1, 1);


        for (int i = LoveAfarAttention.childCount - 1; i >= 0; i--)
        {
            Destroy(LoveAfarAttention.GetChild(i).gameObject);
        }

        HailRural = new List<LoveAfarUI>();

        Sprite Gram= OverheadPeak.Instance.FarAfarSoAt(rank.Seal_To).Part;
        for (int i = 0; i < rank.Mark.Climb.Count; i++)
        {
            RankUser user = rank.Mark.Climb[i];
            GameObject rankItem = Instantiate(LoveAfarQuiver, LoveAfarAttention);
            rankItem.GetComponent<LoveAfarUI>().Deaf(Gram, user, false, rank);
            HailRural.Add(rankItem.GetComponent<LoveAfarUI>());
        }

        // “我的”排名
        RankUser myLove= rank.myLove;
        MyLoveAfar.GetComponent<LoveAfarUI>().Deaf(Gram, myLove, true, rank);
        StartCoroutine(LoveSlumSteamship(myLove));
    }

    private IEnumerator LoveSlumSteamship(RankUser myRank)
    {
        yield return new WaitForEndOfFrame();
        if (myRank.Undergo < rank.Wee_Undergo)
        {
            if (FeedLoveDodge == -1 || myRank.Undergo >= FeedLoveDodge)
            {
                // 如果比上次排名下降，直接定位到当前排名位置
                MyLoveAfar.transform.SetParent(LoveAfarAttention);
                MyLoveAfar.transform.SetSiblingIndex(myRank.Undergo);
                MyLoveAfar.transform.localScale = new Vector3(1, 1, 1);
                yield return new WaitForEndOfFrame();
                LoveAfarAttention.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, Mathf.Max(0, (myRank.Undergo - 2) * LoveAfarMilieu));
            }
            else
            {
                MyLoveAfar.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, Limy.GetComponent<RectTransform>().sizeDelta.y / 2);    // 将MyRankItem放到中间
                LoveAfarAttention.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, Mathf.Max(0, FeedLoveDodge * LoveAfarMilieu));    // ScrollView移动到上次排名位置
                // ScrollView滚动动画
                LoveAfarAttention.GetComponent<RectTransform>().DOAnchorPosY(Mathf.Max(0, (myRank.Undergo - 2) * LoveAfarMilieu), 1f).SetDelay(0.5f).OnComplete(() =>
                {
                    MyLoveAfar.transform.SetParent(LoveAfarAttention);
                    MyLoveAfar.transform.SetSiblingIndex(myRank.Undergo);
                    MyLoveAfar.transform.localScale = new Vector3(1, 1, 1);
                });
            }
        }

        FeedLoveDodge = myRank.Undergo;
    }

    private void SolderColonizeWidow()
    {
        Colonize Northern= rank.Northern;
        Mold.GetComponent<MoldUI>().DeafRidMold(Northern.RidMold);
        ActivityState state = Northern.Widow;
        if (state == ActivityState.NeedSettlement)
        {
            // 待结算
            FirstGibbon.gameObject.SetActive(false);
            Grotto.SetActive(true);
            if (rank.myLove.ValleyDodge != -1)
            {
                WrongGibbon.transform.Find("Text").GetComponent<Text>().text = rank.myLove.ValleyDodge == -1 ? "Close" : "Claim";
            }
        }
        else
        {
            FirstGibbon.gameObject.SetActive(true);
            Grotto.SetActive(false);
        }
    }

    private void OnMouseDown()
    {
        // 点击任意位置，关闭奖励提示框
        if (HailRural != null)
        {
            foreach (LoveAfarUI rankItem in HailRural)
            {
                rankItem.SendStarveGrotto();
            }
        }
        if (MyLoveAfar != null)
        {
            MyLoveAfar.GetComponent<LoveAfarUI>().SendStarveGrotto();
        }
    }
}
