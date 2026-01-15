using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zeta_framework;

public class RankStarPanel : BaseUIForms
{
    public GameObject RankItemPrefab;
    public Transform RankItemContainer;
    public Button CloseButton;
    public GameObject Bottom;
    public Button ClaimButton;
    public GameObject MyRankItem;
    public Transform Main;
    public GameObject Time;
    public string RankId;

    public float RankItemHeight = 130;
    private List<RankItemUI> rankItems;
    private int lastRankIndex = 20;   // “我的”排名
    private Rank rank;

    // Start is called before the first frame update
    void Start()
    {
        CloseButton.onClick.AddListener(() => {
            CloseUIForm(GetType().Name);
        });

        ClaimButton.onClick.AddListener(() =>
        {
            List<ItemGroup> rewards = rank.Settlement();
            if (rewards != null && rewards.Count > 0)
            {
                
            }
            CloseUIForm(GetType().Name);
        });

        // 监听活动状态修改
        MessageCenterLogic.GetInstance().Register(CConfig.mg_ActivityStateChange_ + RankCtrl.Instance.GetRankById(RankId).activity_id, (md) => {
            UpdateActivityState();
        });
    }

    public override void Display(object uiFormParams)
    {
        base.Display(uiFormParams);

        rank ??= RankCtrl.Instance.GetRankById(RankId);
        rank.CalculateRank();

        InitRankList();

        UpdateActivityState();
    }

    private void InitRankList()
    {
        MyRankItem.transform.SetParent(Main);
        MyRankItem.transform.SetAsLastSibling();
        MyRankItem.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0);
        MyRankItem.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0);
        MyRankItem.transform.localScale = new Vector3(1, 1, 1);


        for (int i = RankItemContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(RankItemContainer.GetChild(i).gameObject);
        }

        rankItems = new List<RankItemUI>();

        Sprite icon = ResourceCtrl.Instance.GetItemById(rank.item_id).Icon;
        for (int i = 0; i < rank.data.users.Count; i++)
        {
            RankUser user = rank.data.users[i];
            GameObject rankItem = Instantiate(RankItemPrefab, RankItemContainer);
            rankItem.GetComponent<RankItemUI>().Init(icon, user, false, rank);
            rankItems.Add(rankItem.GetComponent<RankItemUI>());
        }

        // “我的”排名
        RankUser myRank = rank.myRank;
        MyRankItem.GetComponent<RankItemUI>().Init(icon, myRank, true, rank);
        StartCoroutine(RankMoveAnimation(myRank));
    }

    private IEnumerator RankMoveAnimation(RankUser myRank)
    {
        yield return new WaitForEndOfFrame();
        if (myRank.ranking < rank.max_ranking)
        {
            if (lastRankIndex == -1 || myRank.ranking >= lastRankIndex)
            {
                // 如果比上次排名下降，直接定位到当前排名位置
                MyRankItem.transform.SetParent(RankItemContainer);
                MyRankItem.transform.SetSiblingIndex(myRank.ranking);
                MyRankItem.transform.localScale = new Vector3(1, 1, 1);
                yield return new WaitForEndOfFrame();
                RankItemContainer.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, Mathf.Max(0, (myRank.ranking - 2) * RankItemHeight));
            }
            else
            {
                MyRankItem.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, Main.GetComponent<RectTransform>().sizeDelta.y / 2);    // 将MyRankItem放到中间
                RankItemContainer.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, Mathf.Max(0, lastRankIndex * RankItemHeight));    // ScrollView移动到上次排名位置
                // ScrollView滚动动画
                RankItemContainer.GetComponent<RectTransform>().DOAnchorPosY(Mathf.Max(0, (myRank.ranking - 2) * RankItemHeight), 1f).SetDelay(0.5f).OnComplete(() =>
                {
                    MyRankItem.transform.SetParent(RankItemContainer);
                    MyRankItem.transform.SetSiblingIndex(myRank.ranking);
                    MyRankItem.transform.localScale = new Vector3(1, 1, 1);
                });
            }
        }

        lastRankIndex = myRank.ranking;
    }

    private void UpdateActivityState()
    {
        Activity activity = rank.activity;
        Time.GetComponent<TimeUI>().InitEndTime(activity.EndTime);
        ActivityState state = activity.State;
        if (state == ActivityState.NeedSettlement)
        {
            // 待结算
            CloseButton.gameObject.SetActive(false);
            Bottom.SetActive(true);
            if (rank.myRank.rewardIndex != -1)
            {
                ClaimButton.transform.Find("Text").GetComponent<Text>().text = rank.myRank.rewardIndex == -1 ? "Close" : "Claim";
            }
        }
        else
        {
            CloseButton.gameObject.SetActive(true);
            Bottom.SetActive(false);
        }
    }

    private void OnMouseDown()
    {
        // 点击任意位置，关闭奖励提示框
        if (rankItems != null)
        {
            foreach (RankItemUI rankItem in rankItems)
            {
                rankItem.HideRewardCanvas();
            }
        }
        if (MyRankItem != null)
        {
            MyRankItem.GetComponent<RankItemUI>().HideRewardCanvas();
        }
    }
}
