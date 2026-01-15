using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zeta_framework;

public class LoveAfarUI : MonoBehaviour
{
[UnityEngine.Serialization.FormerlySerializedAs("ActiveBgColor")]    public Color RefuteHeEager;
[UnityEngine.Serialization.FormerlySerializedAs("DefaultBgColor")]    public Color FloristHeEager;
[UnityEngine.Serialization.FormerlySerializedAs("ActiveTextColor")]    public Color RefuteMossEager;
[UnityEngine.Serialization.FormerlySerializedAs("DefaultTextColor")]    public Color FloristMossEager;
[UnityEngine.Serialization.FormerlySerializedAs("Bg")]    public Image He;
[UnityEngine.Serialization.FormerlySerializedAs("Ranking")]    public Image Livable;
[UnityEngine.Serialization.FormerlySerializedAs("RankingText")]    public Text LivableMoss;
[UnityEngine.Serialization.FormerlySerializedAs("Avatar")]    public Image Purify;
[UnityEngine.Serialization.FormerlySerializedAs("UserName")]    public Text PorkStew;
[UnityEngine.Serialization.FormerlySerializedAs("ResourceBar")]    public Image OverheadMan;
[UnityEngine.Serialization.FormerlySerializedAs("ResourceImage")]    public Image OverheadFifth;
[UnityEngine.Serialization.FormerlySerializedAs("ResourceText")]    public Text OverheadMoss;
[UnityEngine.Serialization.FormerlySerializedAs("RewardButton")]    public Button StarveGibbon;
[UnityEngine.Serialization.FormerlySerializedAs("RankSprites")]    public Sprite[] LoveOutflow;
[UnityEngine.Serialization.FormerlySerializedAs("RewardSprites")]    public Sprite[] StarveOutflow;
[UnityEngine.Serialization.FormerlySerializedAs("RewardCanvas")]    public GameObject StarveCanvas;
[UnityEngine.Serialization.FormerlySerializedAs("RewardContainer")]    public Transform StarveAttention;
[UnityEngine.Serialization.FormerlySerializedAs("RankRewardPrefab")]    public GameObject LoveStarveQuiver;

    // Start is called before the first frame update
    void Start()
    {
        StarveGibbon.onClick.AddListener(() => {
            StarveCanvas.SetActive(!StarveCanvas.activeSelf);
        });

    }

    public void Deaf(Sprite itemIcon, RankUser userInfo, bool active, Love rank)
    {
        He.color = active ? RefuteHeEager : FloristHeEager;
        if (userInfo.Undergo < LoveOutflow.Length)
        {
            Livable.sprite = LoveOutflow[userInfo.Undergo];
            Livable.enabled = true;
        }
        else
        {
            Livable.enabled = false;
        }
        LivableMoss.text = userInfo.Undergo > rank.Wee_Undergo ? rank.Wee_Undergo + "+" : (userInfo.Undergo + 1).ToString();
        if (!string.IsNullOrEmpty(userInfo.Source))
        {
            // TODO 用户头像
        }
        PorkStew.color = active ? RefuteMossEager : FloristMossEager;
        PorkStew.text = userInfo.TheyStew;
        OverheadMan.color = active ? RefuteHeEager : FloristHeEager;
        OverheadFifth.sprite = itemIcon;
        OverheadMoss.color = active ? RefuteMossEager : FloristMossEager;
        OverheadMoss.text = userInfo.SealLad.ToString();
        if (userInfo.ValleyDodge == -1)
        {
            StarveGibbon.gameObject.SetActive(false);
        }
        else
        {
            StarveGibbon.gameObject.SetActive(true);
            StarveGibbon.GetComponent<Image>().sprite = StarveOutflow[userInfo.ValleyDodge];
            DeafBritain(rank.FarStarveSoLivable(userInfo.Undergo));
        }
    }

    private void DeafBritain(List<AfarPerry> rewards)
    {
        for (int i = StarveAttention.childCount - 1; i >= 0; i--)
        {
            Destroy(StarveAttention.GetChild(i).gameObject);
        }
        if (rewards != null)
        {
            if (rewards != null)
            {
                foreach (AfarPerry item in rewards)
                {
                    GameObject itemUI = Instantiate(LoveStarveQuiver, StarveAttention);
                    itemUI.transform.Find("Icon").GetComponent<Image>().sprite = item.Afar.Part;
                    itemUI.transform.Find("Text").GetComponent<Text>().text = "+" + item.AfarLadSow;
                }
            }
        }
        
    }

    public void SendStarveGrotto()
    {
        StarveCanvas.SetActive(false);
    }
}
