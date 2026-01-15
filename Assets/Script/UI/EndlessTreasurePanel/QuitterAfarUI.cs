using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zeta_framework;

public class QuitterAfarUI : MonoBehaviour
{
[UnityEngine.Serialization.FormerlySerializedAs("CollectItemPrefab")]    public GameObject DynamicAfarQuiver;
[UnityEngine.Serialization.FormerlySerializedAs("RewardsContainer")]    public Transform BritainAttention;
[UnityEngine.Serialization.FormerlySerializedAs("GetButton")]    public Button FarGibbon;
[UnityEngine.Serialization.FormerlySerializedAs("LockIcon")]    public GameObject BentPart;
[UnityEngine.Serialization.FormerlySerializedAs("PriceText")]    public Text BreadMoss;
[UnityEngine.Serialization.FormerlySerializedAs("ItemBG")]    public Image AfarBG;

    private ActivityEndlessTreasureDB SealDB;
    private bool BySample= false;

    // Start is called before the first frame update
    void Start()
    {
        FarGibbon.onClick.AddListener(() => { 
            if (BySample)
            {
                GroomRancher.FarBefriend().BindGroom("Claim previous offer to unlock");
            }
            else
            {
                if (string.IsNullOrEmpty(SealDB.Pont_To))
                {
                    // 免费
                    WrongStarve();
                }
                else
                {
                    // 购买
                    SodaPeak.Instance.Hot(SodaPeak.Instance.FarSodaSoAt(SealDB.Pont_To), (errorCode) => {
                        WrongStarve();
                    });
                }
            }
        });
    }

    public void PearlyAfar(ActivityEndlessTreasureDB item, bool isLock)
    {
        this.SealDB = item;
        // 背景色
        if (!string.IsNullOrEmpty(item.color) && ColorUtility.TryParseHtmlString(item.color, out Color color))
        {
            AfarBG.color = color;
        }
        else
        {
            AfarBG.color = Color.white;
        }
        // 奖励
        for (int i = BritainAttention.childCount - 1; i >= 0; i--)
        {
            Destroy(BritainAttention.GetChild(i).gameObject);
        }
        List<AfarPerry> Dynamic= OverheadPeak.Instance.FarAfarPerrySoEar(item.Pont_To, item.Infantile_To, item.Seal_To, item.Seal_Toy);
        foreach(AfarPerry reward in Dynamic)
        {
            GameObject rewardUI = Instantiate(DynamicAfarQuiver, BritainAttention);
            rewardUI.GetComponent<StarveAfarUI>().Pearly(reward.Afar.Part, reward.Seal_Toy);
        }

        // 价格
        if (string.IsNullOrEmpty(item.Pont_To) || SodaPeak.Instance.FarSodaSoAt(item.Pont_To).Guest == 0)
        {
            BreadMoss.text = "Free";
        }
        else
        {
            BreadMoss.text = "$" + SodaPeak.Instance.FarSodaSoAt(item.Pont_To).Guest;
        }

        BySample = isLock;
        BentPart.SetActive(isLock);
    }

    // 解锁
    public void UnlockAfar()
    {
        if (BySample)
        {
            // TODO 解锁动画
        }

        BySample = false;
        BentPart.SetActive(false);
    }

    // 发奖
    private void WrongStarve()
    {
        List<AfarPerry> Dynamic= OverheadPeak.Instance.FarAfarPerrySoEar(SealDB.Pont_To, SealDB.Infantile_To, SealDB.Seal_To, SealDB.Seal_Toy);
        ColonizeQuitterClearingPeak.Instance.WrongStarve();
        WoolSwear.Instance.BindDynamicSteamship(Dynamic);
        QuitterClearingSwear.Instance.SlumSteamship();
    }
}
