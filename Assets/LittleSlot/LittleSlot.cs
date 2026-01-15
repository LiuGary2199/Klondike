using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

/// <summary>  </summary>
public class LittleSlot : MonoBehaviour
{
    public GameObject SlotMachine;
    public Sprite[] SlotSprites;
    public Transform RealItemParent;
    private List<List<Transform>> RealItems;
    int[] RealLoopCount = new int[3];
    public Transform FakeItemsParent;
    List<List<Transform>> FakeItems;
    int[] FakeLoopCount = new int[3];
    int FakeLoopMax = 35;
    int ItemHigh = 200;
    int Top = 400;
    int Bottom = -400;
    bool IsSloting;
    bool IsSlotWin;
    //int WinIndex;
    RewardData Reward;
    public Button AllGetBtn;
    public Button GetBtn;
    public SkeletonGraphic BgSpine;
    public List<GameObject> ShowList;


    #region 老虎机
    void Init()
    {
        RealItems = new List<List<Transform>>();
        for (int i = 0; i < 3; i++)
        {
            RealItems.Add(new List<Transform>());
            for (int j = 0; j < 1; j++)
            {
                Transform Item = RealItemParent.GetChild(i * 1 + j);
                Item.gameObject.SetActive(false);
                Item.name = $"第{i + 1}列  第{j + 1}行";
                RealItems[i].Add(Item);
            }
        }
        FakeItems = new List<List<Transform>>();
        for (int i = 0; i < 3; i++)
        {
            FakeItems.Add(new List<Transform>());
            for (int j = 0; j < 5; j++)
            {
                Transform Item = FakeItemsParent.GetChild(i * 5 + j);
                int Index = UnityEngine.Random.Range(0, SlotSprites.Length);
                SetSlotIcon(Item, Index);
                Item.name = $"第{i + 1}列  第{j + 1}行";
                FakeItems[i].Add(Item);
            }
        }

        GetBtn.onClick.AddListener(() =>
        {
            Reward.num *= .1f;
            if (Reward.type == RewardType.Gold)
            {
                KickLineRancher.FarBefriend().PegCast(Reward.num);
                KickSwear.Instance.SolderCast();
            }
            else if (Reward.type == RewardType.Cash)
            {
                KickLineRancher.FarBefriend().PegNote(Reward.num);
                KickSwear.Instance.SolderNote();
            }
            gameObject.SetActive(false);
            MoldRancher.FarBefriend().Award(1, () => { Freezing_Rancher.Instance.CigarKickRid(true); });
            WhimInuitRemove.FarBefriend().LeafInuit("1015", "2", Reward.num.ToString());
            ADRancher.Befriend.IfCoronaPegWaist();
        });
        AllGetBtn.onClick.AddListener(() =>
        {
            ADRancher.Befriend.LionStarveAlder((ok) =>
            {
                if (ok)
                {
                    if (Reward.type == RewardType.Gold)
                    {
                        KickLineRancher.FarBefriend().PegCast(Reward.num);
                        KickSwear.Instance.SolderCast();
                    }
                    else if (Reward.type == RewardType.Cash)
                    {
                        KickLineRancher.FarBefriend().PegNote(Reward.num);
                        KickSwear.Instance.SolderNote();
                    }
                    gameObject.SetActive(false);
                    MoldRancher.FarBefriend().Award(1, () => { Freezing_Rancher.Instance.CigarKickRid(true); });
                    WhimInuitRemove.FarBefriend().LeafInuit("1015", "1", Reward.num.ToString());
                }
            }, "6");
        });
    }

    public void Show()
    {
        GetBtn.gameObject.SetActive(false);
        AllGetBtn.gameObject.SetActive(false);

        BgSpine.Skeleton.SetToSetupPose();
        BgSpine.AnimationState.ClearTracks();
        BgSpine.Update(0);
        BgSpine.AnimationState.SetAnimation(0, "animation", false);
        SteamshipRevolution.UrnSwearBind(ShowList);
    }

    public void Slot()
    {
        if (RealItems == null || RealItems.Count == 0)
            Init();

        GetBtn.gameObject.SetActive(false);
        AllGetBtn.gameObject.SetActive(false);
        IsSloting = true;
        IsSlotWin = true;//Random.value < .7f; //中奖概率
        int[] RealItemIndex = new int[3];
        if (IsSlotWin)
        {
            //WinIndex = GetWinIndexByWeight();
            Reward = KickLineRancher.FarBefriend().FarStarveLineSoRevertBusDelay(ToeBoldLeg.instance._KickLine.slots_data_list);
            if (Reward.num > 9.99f)
                Reward.num = (float)System.Math.Round(Random.Range(7f, 9.99f), 2);
            print("老虎机中奖：" + Reward.type + "  数值： " + Reward.num);
            int[] Indexs = new int[3];
            // 将小数转换为整数（乘以100）
            int totalValue = Mathf.RoundToInt(Reward.num * 100);
            // 拆分三个数字
            Indexs[0] = totalValue / 100;         // 十位
            Indexs[1] = (totalValue / 10) % 10;   // 个位
            Indexs[2] = totalValue % 10;          // 十分位
            for (int i = 0; i < 3; i++)
                RealItemIndex[i] = Indexs[i];
        }
        else
        {
            int[] temp = new int[SlotSprites.Length];
            for (int i = 0; i < SlotSprites.Length; i++)
                temp[i] = i;
            for (int i = 0; i < 3; i++)
            {
                int tempIndex = UnityEngine.Random.Range(0, SlotSprites.Length - i);
                int tempValue = temp[tempIndex];
                temp[tempIndex] = temp[SlotSprites.Length - i];
                temp[SlotSprites.Length - i] = tempValue;
            }
            for (int i = 0; i < 3; i++)
                RealItemIndex[i] = temp[i];
        }
        for (int i = 0; i < 3; i++)
        {
            Transform Item = RealItems[i][0];
            SetSlotIcon(Item, RealItemIndex[i]);
            Item.gameObject.SetActive(false);
            Item.transform.localPosition = new Vector2(Item.transform.localPosition.x, Item.transform.localPosition.y + ItemHigh);
        }

        for (int i = 0; i < 3; i++)
        {
            RealLoopCount[i] = 0;
            FakeLoopCount[i] = 0;
            for (int j = 0; j < 5; j++)
                FakeItems[i][j].Find("Icon").gameObject.SetActive(true);
            int Index = i;
            MoldRancher.FarBefriend().Award(Index * .4f, () =>
            {
                FakeColScrollAnim(Index, "开始");
                //LeafyLeg.GetInstance().PlayEffect(LeafyJoke.UIMusic.Sound_SlotsRolling);
            });
        }
    }
    int GetWinIndexByWeight()
    {
        // 0:+10钻 1：+20钻 2：+30钻 3：+50钻
        int[] WinIndex = new int[] { 0, 1, 2, 3 };
        int[] Weights = new int[] { 10, 10, 10, 10 };
        int Sum = 0;
        for (int i = 0; i < WinIndex.Length; i++)
            Sum += Weights[i];
        int RandomNum = UnityEngine.Random.Range(0, Sum);
        for (int i = 0; i < WinIndex.Length; i++)
        {
            RandomNum -= Weights[i];
            if (RandomNum < 0)
                return WinIndex[i];
        }
        return 0;
    }

    void SetSlotIcon(Transform Item, int Index)
    {
        Item.Find("Icon").GetComponent<Image>().sprite = SlotSprites[Index];
        Item.Find("Icon").gameObject.SetActive(true);
    }

    void RealColScrollAnim(int Index)
    {
        float AnimTime = 0.2f;
        Ease AnimEase = Ease.OutBack;
        for (int i = 0; i < 1; i++)
        {
            Transform Item = RealItems[Index][i];
            Item.gameObject.SetActive(true);
            Item.transform.DOLocalMoveY(Item.transform.localPosition.y - ItemHigh, AnimTime).SetEase(AnimEase).OnUpdate(() =>
            {
                for (int j = 0; j < 5; j++)
                {
                    Transform FakeItem = FakeItems[Index][j];
                    if (Mathf.Abs(Item.transform.position.y - FakeItem.transform.position.y) < .5f)
                        FakeItem.Find("Icon").gameObject.SetActive(false);
                }
            });
        }
        MoldRancher.FarBefriend().Award(AnimTime, () =>
        {
            // if (Index == 0)
            //     LeafyLeg.GetInstance().PlayEffect(LeafyJoke.UIMusic.Sound_High_Rate_3Slot);
            // else if (Index == 1)
            //     LeafyLeg.GetInstance().PlayEffect(LeafyJoke.UIMusic.Sound_High_Rate_4Slot);
            // else if (Index == 2)
            //     LeafyLeg.GetInstance().PlayEffect(LeafyJoke.UIMusic.Sound_High_Rate_5Slot);
            LeafyLeg.FarBefriend().HaulGuinea(LeafyJoke.UIMusic.SFX_SlotsGetReward);
            if (Index == 2)
                SlotFinish();
        });
    }
    void FakeColScrollAnim(int Index, string AnimType)
    {
        float AnimTime = 0;
        Ease AnimEase = Ease.Linear;
        if (AnimType == "开始")
        {
            AnimTime = 0.2f;
            AnimEase = Ease.InSine;
            LeafyLeg.FarBefriend().HaulGuinea(LeafyJoke.UIMusic.SFX_SlotsRotate);
        }
        else if (AnimType == "持续")
        {
            AnimTime = 0.05f;
            AnimEase = Ease.Linear;
        }
        else if (AnimType == "结束")
        {
            AnimTime = 0.2f;
            AnimEase = Ease.OutBack;
        }
        for (int i = 0; i < 5; i++)
        {
            Transform Item = FakeItems[Index][i];
            Item.transform.DOLocalMoveY(Item.transform.localPosition.y - ItemHigh, AnimTime).SetEase(AnimEase);
        }
        MoldRancher.FarBefriend().Award(AnimTime, () =>
        {
            for (int i = 0; i < 5; i++)
            {
                Transform Item = FakeItems[Index][i];
                if (Item.transform.localPosition.y < Bottom)
                {
                    Item.transform.localPosition = new Vector2(Item.transform.localPosition.x, Top);
                    SetSlotIcon(Item, UnityEngine.Random.Range(0, SlotSprites.Length));
                    LeafyLeg.FarBefriend().HaulBalance(Lofelt.NiceVibrations.HapticPatterns.PresetType.SoftImpact);
                    break;
                }
            }

            if (AnimType == "开始")
                FakeColScrollAnim(Index, "持续");
            else if (AnimType == "持续")
            {
                FakeLoopCount[Index]++;
                if (FakeLoopCount[Index] < FakeLoopMax)
                    FakeColScrollAnim(Index, "持续");
                else
                    FakeColScrollAnim(Index, "结束");

                if (FakeLoopCount[Index] == FakeLoopMax)
                    RealColScrollAnim(Index);
            }
        });
    }

    void SlotFinish()
    {
        IsSloting = false;
        //GetBtn.gameObject.SetActive(true);
        AllGetBtn.gameObject.SetActive(true);
        AllGetBtn.transform.localPosition = new Vector2(0, -398);
        MoldRancher.FarBefriend().Award(2.5f, () =>
        {
            GetBtn.gameObject.SetActive(true);
            GetBtn.transform.localPosition = new Vector2(155, -398);
            AllGetBtn.transform.localPosition = new Vector2(-155, -398);
        });
    }
    #endregion

}
