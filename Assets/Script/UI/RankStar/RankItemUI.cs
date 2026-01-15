using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zeta_framework;

public class RankItemUI : MonoBehaviour
{
    public Color ActiveBgColor;
    public Color DefaultBgColor;
    public Color ActiveTextColor;
    public Color DefaultTextColor;
    public Image Bg;
    public Image Ranking;
    public Text RankingText;
    public Image Avatar;
    public Text UserName;
    public Image ResourceBar;
    public Image ResourceImage;
    public Text ResourceText;
    public Button RewardButton;
    public Sprite[] RankSprites;
    public Sprite[] RewardSprites;
    public GameObject RewardCanvas;
    public Transform RewardContainer;
    public GameObject RankRewardPrefab;

    // Start is called before the first frame update
    void Start()
    {
        RewardButton.onClick.AddListener(() => {
            RewardCanvas.SetActive(!RewardCanvas.activeSelf);
        });

    }

    public void Init(Sprite itemIcon, RankUser userInfo, bool active, Rank rank)
    {
        Bg.color = active ? ActiveBgColor : DefaultBgColor;
        if (userInfo.ranking < RankSprites.Length)
        {
            Ranking.sprite = RankSprites[userInfo.ranking];
            Ranking.enabled = true;
        }
        else
        {
            Ranking.enabled = false;
        }
        RankingText.text = userInfo.ranking > rank.max_ranking ? rank.max_ranking + "+" : (userInfo.ranking + 1).ToString();
        if (!string.IsNullOrEmpty(userInfo.avatar))
        {
            // TODO 用户头像
        }
        UserName.color = active ? ActiveTextColor : DefaultTextColor;
        UserName.text = userInfo.userName;
        ResourceBar.color = active ? ActiveBgColor : DefaultBgColor;
        ResourceImage.sprite = itemIcon;
        ResourceText.color = active ? ActiveTextColor : DefaultTextColor;
        ResourceText.text = userInfo.itemNum.ToString();
        if (userInfo.rewardIndex == -1)
        {
            RewardButton.gameObject.SetActive(false);
        }
        else
        {
            RewardButton.gameObject.SetActive(true);
            RewardButton.GetComponent<Image>().sprite = RewardSprites[userInfo.rewardIndex];
            InitRewards(rank.GetRewardByRanking(userInfo.ranking));
        }
    }

    private void InitRewards(List<ItemGroup> rewards)
    {
        for (int i = RewardContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(RewardContainer.GetChild(i).gameObject);
        }
        if (rewards != null)
        {
            if (rewards != null)
            {
                foreach (ItemGroup item in rewards)
                {
                    GameObject itemUI = Instantiate(RankRewardPrefab, RewardContainer);
                    itemUI.transform.Find("Icon").GetComponent<Image>().sprite = item.Item.Icon;
                    itemUI.transform.Find("Text").GetComponent<Text>().text = "+" + item.ItemNumStr;
                }
            }
        }
        
    }

    public void HideRewardCanvas()
    {
        RewardCanvas.SetActive(false);
    }
}
