using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using System;

/// <summary> 克朗代克纸牌玩法管理 </summary>
public class Freezing_Rancher : MonoBehaviour
{
    public static Freezing_Rancher Instance;
    [UnityEngine.Serialization.FormerlySerializedAs("CardPrefab")]
    public Keel KeelQuiver;
    [UnityEngine.Serialization.FormerlySerializedAs("SuitSprites")] public Sprite[] LeapOutflow; //7个花色 黑桃 红桃 梅花 方片 J Q K 
    [HideInInspector][UnityEngine.Serialization.FormerlySerializedAs("ValueTexts")] public string[] DodgeVogue = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
    [UnityEngine.Serialization.FormerlySerializedAs("Gap_Y")] public float Inn_Y = .4f; //接龙时上下两张牌的间隔
    [HideInInspector][UnityEngine.Serialization.FormerlySerializedAs("UpDeck_FaceUp")] public List<CardData> OfPram_HighOf = new List<CardData>(); // 上部牌堆 正面朝上
    [HideInInspector][UnityEngine.Serialization.FormerlySerializedAs("UpDeck_FaceDown")] public List<CardData> OfPram_HighPink = new List<CardData>(); // 上部牌堆 背面朝上
    [HideInInspector][UnityEngine.Serialization.FormerlySerializedAs("UpArea")] public List<List<CardData>> OfPeck = new List<List<CardData>>(); // 上部接龙区 4组
    [HideInInspector][UnityEngine.Serialization.FormerlySerializedAs("DownArea")] public List<List<CardData>> PinkPeck = new List<List<CardData>>(); // 下部接龙区 7组
    float WaxyEvidenceCigar = .5f; //判断两张牌之间的距离 用以在拖动操作时检测是否可以叠放
    Dictionary<CardData, Keel> PoseLineBeKeelAgo = new Dictionary<CardData, Keel>(); // 性能优化：CardData到Card的映射
    private const string ShedCan = "klondike_save"; // 存档key
    private Stack<KlondikeGameState> FileRobin = new Stack<KlondikeGameState>(); // 撤销栈
    [Range(0f, 1f)][UnityEngine.Serialization.FormerlySerializedAs("SmartFlipProbability")] float CedarBarbImmigration = 1f; // 智能换牌概率（0~1），如0.8表示80%概率执行智能换牌
    [HideInInspector][UnityEngine.Serialization.FormerlySerializedAs("IsWinCollecting")] public bool UpUrnExorbitant = false; // 是否在赢牌收集阶段
    private float CopyShudder = 0f; // 时间计数器
    private const int MoldEssenceDemolish = 15; // 时间惩罚间隔
    private const int MoldEssenceDense = -2; // 时间惩罚分数
    private int FeedDenseEssenceMold = 0; // 上次时间惩罚时间
    private int DealWaist = 0; // 操作次数统计
    private float AbutBadgeMold = 0f; // 游戏开始时间
    List<string> OfPram_StarveKeelProse = new List<string>(); //上部牌堆奖励牌
    bool UpDepositBriefly_ThriftyKeel = false; //是否触发小游戏 刮刮卡
    bool UpDepositBriefly_AwaitFaith = false; //是否触发小游戏 幸运轮盘
    bool UpDepositBriefly_Sonic = false; //是否触发小游戏 现金奖励
    bool UpDepositBriefly_Dish = false; //是否触发小游戏 老虎机
    bool UpDepositBriefly;
    Coroutine MimeBrieflyKickIE;
    float BrieflyKickMoldShudder;
    [HideInInspector][UnityEngine.Serialization.FormerlySerializedAs("LuckyWheelCount")] public int AwaitFaithWaist;
    bool UpPlayedBrieflyDish = false; //是否已经玩过老虎机 每局游戏只出一次
    Vector3 TautSlumKeelPegDenseGuineaSum;


    void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;
    }

    public void Deaf()
    {
        AwaitFaithWaist = ShedLineRancher.FarWit("LuckyWheelCount");
        KickSwear.Instance.CudAwaitFaithGene();
        UpPlayedBrieflyDish = ShedLineRancher.FarGray("IsPlayedSpecialSlot");
        CedarBarbImmigration = (float)ToeBoldLeg.instance._KickLine.smartflip_probability;

        // bool IsGuide = ShedLineRancher.GetBool("IsGuide");
        // if (!IsGuide)
        // {
        //     StartNewGame_Guide();
        //     return;
        // }

        if (!RockKick()) //加载存档
            BadgeGunKick(); // 没有存档则新开一局
    }

    private void OnApplicationQuit()
    {
        ShedKick();
    }
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
            ShedKick();
    }

    void Update()
    {
        if (KickSwear.Instance != null)
        {
            // 更新游戏时间显示
            KickSwear.Instance.SolderKickMold(FarKickMold());

            // 每15秒减2分
            CopyShudder += Time.deltaTime;
            int penaltyTimes = (int)(CopyShudder / MoldEssenceDemolish);
            if (penaltyTimes > FeedDenseEssenceMold)
            {
                KickSwear.Instance.PegDense(MoldEssenceDense);
                FeedDenseEssenceMold = penaltyTimes;
            }

            //计算小游戏触发间隔
            BrieflyKickMoldShudder += Time.deltaTime;
        }

        //测试
        if (Input.GetKeyDown(KeyCode.R))
        {
            KickSwear.Instance.Jobber(true);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //UIRancher.GetInstance().ShowUIForms(nameof(ThriftySwear));
            KickSwear.Instance.HaulHiccupDish();
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            SteamshipRevolution.CarveNeon(KickSwear.Instance.CarveNeon, new Vector3(0, 0, 0), new Vector3(1, 1, 1));
        }
    }

    // 增加操作次数
    private void PegSlumWaist()
    {
        DealWaist++;
        if (KickSwear.Instance != null)
            KickSwear.Instance.SolderSlumWaist(DealWaist);
    }

    // 获取当前游戏时间（秒）
    public float FarKickMold()
    {
        return Time.time - AbutBadgeMold;
    }

    public Sprite FarLeapReview(CardData card, bool IsLittle = false) //获取花色图片
    {
        if (IsLittle)
            return LeapOutflow[(int)card.Leap];
        else if (card.Value == CardValue.J)
            return LeapOutflow[4];
        else if (card.Value == CardValue.Q)
            return LeapOutflow[5];
        else if (card.Value == CardValue.K)
            return LeapOutflow[6];
        else
            return LeapOutflow[(int)card.Leap];
    }

    public string FarKeelPartnerStew(CardData card) //获取牌的显示名称
    {
        string suitName = "";
        switch (card.Leap)
        {
            case CardSuit.BlackPeach: suitName = "♠"; break;
            case CardSuit.RedPeach: suitName = "♥"; break;
            case CardSuit.BlackFlower: suitName = "♣"; break;
            case CardSuit.Cube: suitName = "♦"; break;
        }
        return suitName + DodgeVogue[(int)card.Value];
    }

    public string FarPinkPeckHighPinkKeelLad_HutInuit() //获取下部接龙区背面朝上的牌数 打点用
    {
        int Lad = 0;
        for (int i = 0; i < PinkPeck.Count; i++)
        {
            for (int j = 0; j < PinkPeck[i].Count; j++)
            {
                if (!PinkPeck[i][j].UpHighOf)
                    Lad++;
            }
        }
        return Lad.ToString();
    }

    public void FarOfPeckStarve(int index) //获取上部接龙区奖励
    {
        RewardData Line = KickLineRancher.FarBefriend().FarStarveLineSoRevertBusDelay(ToeBoldLeg.instance._KickLine.uparea_collectcard_data_list);
        if (Line.type == RewardType.Gold)
        {
            KickLineRancher.FarBefriend().PegCast(Line.num);
            KickSwear.Instance.SolderCast();
            WhimInuitRemove.FarBefriend().LeafInuit("1008", "1", Line.num.ToString());
        }
        else if (Line.type == RewardType.Cash)
        {
            KickLineRancher.FarBefriend().PegNote(Line.num);
            KickSwear.Instance.SolderNote();

            WhimInuitRemove.FarBefriend().LeafInuit("1008", "2", Line.num.ToString());
        }
        KickSwear.Instance.BindOfPeckStarveGuinea(index, Line.type);
    }

    public void FarOfPramStarve() //获取上部牌堆奖励
    {
        RewardData Line = KickLineRancher.FarBefriend().FarStarveLineSoRevertBusDelay(ToeBoldLeg.instance._KickLine.uparea_filp_data_list);
        if (Line.type == RewardType.Gold)
        {
            KickLineRancher.FarBefriend().PegCast(Line.num);
            KickSwear.Instance.SolderCast();
            WhimInuitRemove.FarBefriend().LeafInuit("1010", "1", Line.num.ToString());
        }
        else if (Line.type == RewardType.Cash)
        {
            KickLineRancher.FarBefriend().PegNote(Line.num);
            KickSwear.Instance.SolderNote();
            WhimInuitRemove.FarBefriend().LeafInuit("1010", "2", Line.num.ToString());
        }
        KickSwear.Instance.BindOfPramStarveGuinea(Line.type);
    }
    #region  初始化游戏
    void BadgeGunKick() // 初始化游戏 根据规则控制难度
    {
        KickSwear.Instance.HeavyDense();
        // 重置操作次数和时间
        UpPlayedBrieflyDish = false;
        ShedLineRancher.CudGray("IsPlayedSpecialSlot", false);
        DealWaist = 0;
        AbutBadgeMold = Time.time;
        if (KickSwear.Instance != null)
        {
            KickSwear.Instance.SolderSlumWaist(DealWaist);
            KickSwear.Instance.SolderKickMold(0);
        }
        //初始化上部接龙区
        OfPeck.Clear();
        for (int i = 0; i < 4; i++)
            OfPeck.Add(new List<CardData>());
        // 初始布置
        // a、随机下部接龙区域 7列每列分别有1~7张牌
        // b、剩余牌收起，放置上部翻牌区
        // c、3张A和K从来不沉底（沉底的牌是一组2~13(红黑交叉的13张)+2~13中可重复的9张）
        List<CardData> CardPools_UnallocatedPool = new List<CardData>(); //未分配的牌52张 等所有牌生成结束后再分入沉底或不沉底牌池
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 13; j++)
            {
                CardData Mark = new CardData((CardSuit)i, (CardValue)j, false, false);
                CardPools_UnallocatedPool.Add(Mark);
            }
        }
        CardPools_UnallocatedPool = CardPools_UnallocatedPool.OrderBy(x => UnityEngine.Random.value).ToList();// 未分配的牌随机打乱顺序
        List<CardData> CardPools_Sink = new List<CardData>(); // 沉底牌池 在下部接龙区 被盖住的牌
        List<CardData> CardPools_NoSink = new List<CardData>(); // 不沉底的牌 在上部牌堆 或者 下部接龙区最底下被翻开的牌
        //随机选一个初始颜色 这组牌沉底  （此时沉底共12张 不沉底0张）
        bool SinkColorRed = UnityEngine.Random.value < .5f;
        for (int i = 1; i < 13; i++)
        {
            CardData SinkData = CardPools_UnallocatedPool.Select(x => x).Where(x => x.UpCod() == SinkColorRed && (int)x.Value == i).First();
            SinkColorRed = !SinkColorRed;
            CardPools_Sink.Add(SinkData);
            CardPools_UnallocatedPool.Remove(SinkData);
        }
        //剩余牌中 A和K不沉底 （此时沉底共12张 不沉底6张）
        for (int i = 0; i < CardPools_UnallocatedPool.Count; i++)
        {
            if (CardPools_UnallocatedPool[i].Value == CardValue.One || CardPools_UnallocatedPool[i].Value == CardValue.K)
            {
                CardPools_NoSink.Add(CardPools_UnallocatedPool[i]);
                CardPools_UnallocatedPool.RemoveAt(i);
                i--;
            }
        }
        //剩余牌中 9张牌沉底 （此时沉底共21张 不沉底6张）
        for (int i = 0; i < 9; i++)
        {
            CardPools_Sink.Add(CardPools_UnallocatedPool[0]);
            CardPools_UnallocatedPool.RemoveAt(0);
        }
        //剩余牌不沉底 （此时沉底共21张 不沉底31张）
        for (int i = 0; i < CardPools_UnallocatedPool.Count; i++)
            CardPools_NoSink.Add(CardPools_UnallocatedPool[i]);
        //打乱顺序
        CardPools_Sink = CardPools_Sink.OrderBy(x => UnityEngine.Random.value).ToList();
        CardPools_NoSink = CardPools_NoSink.OrderBy(x => UnityEngine.Random.value).ToList();
        // 生成下部接龙区 沉底的牌
        int usedSinkCards = 0;
        for (int i = 0; i < 7; i++)
        {
            List<CardData> cards = new List<CardData>();
            for (int j = 0; j < i; j++)
                cards.Add(CardPools_Sink[usedSinkCards + j]);
            PinkPeck.Add(cards);
            usedSinkCards += i;
        }
        // 删除已使用的沉底牌
        CardPools_Sink.RemoveRange(0, usedSinkCards);
        // 生成下部接龙区 不沉底的牌
        for (int i = 0; i < 7; i++)
        {
            CardPools_NoSink[0].UpHighOf = true;
            PinkPeck[i].Add(CardPools_NoSink[0]);
            CardPools_NoSink.RemoveAt(0);
        }
        Vector2[] Sum_PinkPeck = KickSwear.Instance.Sum_PinkPeck.Select(x => (Vector2)x.position).ToArray();
        for (int i = 0; i < PinkPeck.Count && i < Sum_PinkPeck.Length; i++)
        {
            for (int j = 0; j < PinkPeck[i].Count; j++)
            {
                Keel card = DugoutKeel(PinkPeck[i][j]);
                card.transform.DOMove(Sum_PinkPeck[i] + new Vector2(0, -j * Inn_Y), 0.5f).SetDelay(j * 0.05f).OnStart(() =>
                {
                    if (i % 2 == 0)
                        LeafyLeg.FarBefriend().HaulGuinea(LeafyJoke.UIMusic.SFX_CardSet);
                });
            }
        }

        // 生成上部牌堆 背面朝上
        for (int i = 0; i < 24; i++)
        {
            OfPram_HighPink.Add(CardPools_NoSink[0]);
            CardPools_NoSink.RemoveAt(0);
        }
        Vector2 Sum_OfPram_HighPink = KickSwear.Instance.Sum_OfPram_HighPink.position;
        for (int i = 0; i < OfPram_HighPink.Count; i++)
        {
            Keel card = DugoutKeel(OfPram_HighPink[i]);
            card.transform.DOMove(Sum_OfPram_HighPink, 0.5f).SetDelay(i * 0.03f).OnStart(() =>
                {
                    if (i % 2 == 0)
                        LeafyLeg.FarBefriend().HaulGuinea(LeafyJoke.UIMusic.SFX_CardSet);
                });
        }

        //上部牌堆区 随机些牌记录为奖励牌
        int UpDeck_RewardCard_Count = 0;
        for (int i = 0; i < OfPram_HighPink.Count; i++)
        {
            //根据概率确定奖励牌数量
            if (UnityEngine.Random.value < ToeBoldLeg.instance._KickLine.uparea_filp_reward_rate)
                UpDeck_RewardCard_Count++;
        }
        List<string> TempCardName = new List<string>();
        for (int i = 0; i < OfPram_HighPink.Count; i++)
            TempCardName.Add(FarKeelPartnerStew(OfPram_HighPink[i]));
        TempCardName = TempCardName.OrderBy(x => UnityEngine.Random.value).ToList();
        for (int i = 0; i < UpDeck_RewardCard_Count; i++)
            OfPram_StarveKeelProse.Add(TempCardName[i]);
        string UpDeck_RewardCard_Save = string.Join(",", OfPram_StarveKeelProse);
        ShedLineRancher.CudStench("UpDeck_RewardCardNames", UpDeck_RewardCard_Save);
        MoldRancher.FarBefriend().Award(1, () => { KickSwear.Instance.SendVerb(); });

        //第二局游戏开始时 弹好评
        if (ShedLineRancher.FarGray("WaitRateUs"))
        {
            UIRancher.FarBefriend().BindUIPlace(nameof(MaskUsSwear));
            ShedLineRancher.CudGray("WaitRateUs", false);
        }
    }

    //生成牌
    Keel DugoutKeel(CardData data)
    {
        Keel card = Instantiate(KeelQuiver);
        // 判断初始归属区域
        if (PinkPeck.Any(area => area.Contains(data)))
            card.transform.SetParent(KickSwear.Instance.KeelQuasar_PinkPeck);
        else if (OfPram_HighPink.Contains(data) || OfPram_HighOf.Contains(data))
            card.transform.SetParent(KickSwear.Instance.KeelQuasar_OfPram);
        else if (OfPeck.Any(area => area.Contains(data)))
            card.transform.SetParent(KickSwear.Instance.KeelQuasar_OfPeck);
        else
            card.transform.SetParent(KickSwear.Instance.KeelQuasar_OfPram);
        card.transform.localScale = Vector3.one;
        card.transform.position = KickSwear.Instance.Sum_KeelAvid.position;
        card.Deaf(data);

        // 建立映射关系
        PoseLineBeKeelAgo[data] = card;
        return card;
    }
    #endregion

    #region  翻牌接龙等逻辑
    //上部牌堆翻牌
    public void OfPram_Back()
    {
        ShedWidowHutNote(); // 撤销快照
        PegSlumWaist(); // 增加操作次数
        // 隐藏提示
        KickSwear.Instance.SendRotVole();
        if (OfPram_HighPink.Count > 0)
        {
            var cardData = OfPram_HighPink.Last();
            OfPram_HighPink.RemoveAt(OfPram_HighPink.Count - 1);
            cardData.UpHighOf = true;
            OfPram_HighOf.Add(cardData);
            // UI
            var cardObj = FarKeelBur(cardData);
            if (cardObj != null)
            {
                cardObj.transform.SetParent(KickSwear.Instance.KeelQuasar_OfPram);
                cardObj.transform.SetAsLastSibling();
                // 添加翻牌动画
                StartCoroutine(BarbPramKeelSteamship(cardObj, cardData));

                if (OfPram_StarveKeelProse.Contains(cardObj.transform.name))
                {
                    FarOfPramStarve();
                    OfPram_StarveKeelProse.Remove(cardObj.transform.name);
                    string UpDeck_RewardCard_Save = string.Join(",", OfPram_StarveKeelProse);
                    ShedLineRancher.CudStench("UpDeck_RewardCardNames", UpDeck_RewardCard_Save);
                }
            }
        }
        else if (OfPram_HighPink.Count == 0 && OfPram_HighOf.Count > 0)
        {
            // 全部回收
            StartCoroutine(ConcertRotHighOfBrass());
        }
        ShedKick();
    }

    // 翻牌动画
    private IEnumerator BarbPramKeelSteamship(Keel cardObj, CardData cardData)
    {
        LeafyLeg.FarBefriend().HaulGuinea(LeafyJoke.UIMusic.SFX_CardSwitch);
        KickSwear.Instance.BindVerb();
        // 第一阶段：旋转到90度
        cardObj.transform.DOMove(KickSwear.Instance.Sum_OfPram_HighOf.position, 0.2f).SetEase(Ease.OutQuad);
        cardObj.transform.DORotate(new Vector3(0, 90, 0), 0.15f).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(0.15f);
        // 在90度时切换牌面显示
        cardObj.Deaf(cardData);
        // 第二阶段：旋转回0度
        cardObj.transform.DORotate(Vector3.zero, 0.15f).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(0.15f);
        KickSwear.Instance.SendVerb();
        CigarKickRid();
    }

    // 回收所有正面朝上的牌
    private IEnumerator ConcertRotHighOfBrass()
    {
        KickSwear.Instance.BindVerb();
        for (int i = OfPram_HighOf.Count - 1; i >= 0; i--)
        {
            var cardData = OfPram_HighOf[i];
            cardData.UpHighOf = false;
            OfPram_HighPink.Add(cardData);

            // UI
            var cardObj = FarKeelBur(cardData);
            if (cardObj != null)
            {
                cardObj.transform.SetParent(KickSwear.Instance.KeelQuasar_OfPram);
                cardObj.transform.SetAsLastSibling();

                // 添加回收动画
                cardObj.transform.DOMove(KickSwear.Instance.Sum_OfPram_HighPink.position, 0.2f).SetEase(Ease.InQuad);
                cardObj.transform.DORotate(new Vector3(0, 180, 0), 0.2f).SetEase(Ease.InOutQuad)
                    .OnComplete(() =>
                    {
                        cardObj.Deaf(cardData);
                        cardObj.transform.DORotate(Vector3.zero, 0.1f).SetEase(Ease.OutQuad);
                    });
                LeafyLeg.FarBefriend().HaulGuinea(LeafyJoke.UIMusic.SFX_CardSwitch);
                yield return new WaitForSeconds(0.05f); // 添加延迟，让回收过程更流畅
            }
        }
        OfPram_HighOf.Clear();
        KickSwear.Instance.SendVerb();
        ShedKick();
        CigarKickRid();
    }

    // 获取拖动的牌（包括下方所有牌）
    public List<Keel> FarDecisionBrass(Keel topCard)
    {
        // 下部接龙区连续正面朝上的牌
        for (int i = 0; i < PinkPeck.Count; i++)
        {
            var area = PinkPeck[i];
            for (int j = 0; j < area.Count; j++)
            {
                if (FarKeelBur(area[j]) == topCard)
                {
                    List<Keel> result = new List<Keel>();
                    // 从当前牌开始，收集所有连续正面朝上的牌
                    for (int k = j; k < area.Count; k++)
                    {
                        if (area[k].UpHighOf)
                        {
                            Keel c = FarKeelBur(area[k]);
                            if (c != null)
                                result.Add(c);
                        }
                        else
                        {
                            break; // 遇到背面朝上的牌就停止
                        }
                    }
                    return result;
                }
            }
        }
        // 上部接龙区 只允许单张
        for (int i = 0; i < OfPeck.Count; i++)
        {
            var area = OfPeck[i];
            if (area.Count > 0 && FarKeelBur(area.Last()) == topCard)
                return new List<Keel> { topCard };
        }
        // 支持上部牌堆正面区 只允许单张
        if (OfPram_HighOf.Count > 0 && FarKeelBur(OfPram_HighOf.Last()) == topCard)
            return new List<Keel> { topCard };
        return new List<Keel> { topCard };
    }

    // 判断能否叠放到目标牌
    public bool TheRobinDy(Keel movingCard, Keel targetCard)
    {
        // 下部区域：红黑交错，递减
        for (int i = 0; i < PinkPeck.Count; i++)
        {
            var area = PinkPeck[i];
            if (area.Count > 0 && FarKeelBur(area.Last()) == targetCard)
            {
                bool colorDiff = movingCard.Line.UpCod() != targetCard.Line.UpCod();
                bool valueDiff = (int)movingCard.Line.Value == (int)targetCard.Line.Value - 1;
                //print("颜色是否不同： " + colorDiff);
                //print("牌值是否递减： " + valueDiff + " 拖动牌：" + movingCard.Data.Value + " 目标牌：" + targetCard.Data.Value);
                return colorDiff && valueDiff;
            }
        }
        // 上部接龙区：同花色，递增
        for (int i = 0; i < OfPeck.Count; i++)
        {
            var area = OfPeck[i];
            if (area.Count > 0 && FarKeelBur(area.Last()) == targetCard)
            {
                bool sameSuit = movingCard.Line.Leap == targetCard.Line.Leap;
                bool valueInc = (int)movingCard.Line.Value == (int)targetCard.Line.Value + 1;
                //print("颜色是否相同： " + sameSuit);
                //print("牌值是否递增： " + valueInc + " 拖动牌：" + movingCard.Data.Value + " 目标牌：" + targetCard.Data.Value);
                return sameSuit && valueInc;
            }
        }
        return false;
    }

    // 移动一叠牌到目标牌下方
    public bool SlumKeelRobin(Keel movingCard, Keel targetCard)
    {
        ShedWidowHutNote(); // 撤销快照
        PegSlumWaist(); // 增加操作次数
        bool result = false;
        bool fromUpDeckFaceUp = false;
        bool fromUpArea = false;
        if (PinkPeck.Any(area => area.Contains(targetCard.Line)))
        {
            var source = FindKeelIodine(movingCard);
            if (source.fromCol == -1) return false;
            List<CardData> movingList;
            if (source.fromUpDeckFaceUp)
            {
                movingList = new List<CardData> { OfPram_HighOf[source.fromIdx] };
                OfPram_HighOf.RemoveAt(source.fromIdx);
                fromUpDeckFaceUp = true;
            }
            else if (source.fromUpArea)
            {
                movingList = new List<CardData> { OfPeck[source.fromUpAreaIdx].Last() };
                OfPeck[source.fromUpAreaIdx].RemoveAt(OfPeck[source.fromUpAreaIdx].Count - 1);
                fromUpArea = true;
            }
            else
            {
                movingList = PinkPeck[source.fromCol].GetRange(source.fromIdx, PinkPeck[source.fromCol].Count - source.fromIdx);
                PinkPeck[source.fromCol].RemoveRange(source.fromIdx, PinkPeck[source.fromCol].Count - source.fromIdx);
            }
            PinkPeck[PinkPeck.FindIndex(area => area.Contains(targetCard.Line))].AddRange(movingList);
            // UI移动
            Vector3 AddScoreEffectPos = Vector3.zero;
            for (int k = 0; k < movingList.Count; k++)
            {
                Keel cardObj = FarKeelBur(movingList[k]);
                cardObj.transform.SetParent(KickSwear.Instance.KeelQuasar_PinkPeck);
                cardObj.transform.SetAsLastSibling();
                Vector3 targetPos = KickSwear.Instance.Sum_PinkPeck[PinkPeck.FindIndex(area => area.Contains(targetCard.Line))].position + new Vector3(0, -(PinkPeck[PinkPeck.FindIndex(area => area.Contains(targetCard.Line))].Count - movingList.Count + k) * Inn_Y);
                cardObj.transform.DOMove(targetPos, 0.3f).SetEase(Ease.OutQuad);
                LeafyLeg.FarBefriend().HaulGuinea(LeafyJoke.UIMusic.SFX_CardLink);
                if (k == 0)
                {
                    AddScoreEffectPos = targetPos;
                    MoldRancher.FarBefriend().Award(0.3f, () =>
                    {
                        LeafyLeg.FarBefriend().HaulBalance(Lofelt.NiceVibrations.HapticPatterns.PresetType.MediumImpact);
                    });
                }
            }
            if (!source.fromUpDeckFaceUp && !source.fromUpArea)
                CigarBarbTulipSlum(source.fromCol);
            CigarKickRid();
            result = true;
            if (fromUpDeckFaceUp)
                KickSwear.Instance.PegDense(5, AddScoreEffectPos);
            if (fromUpArea)
                KickSwear.Instance.PegDense(-10, AddScoreEffectPos);
        }
        ShedKick();
        return result;
    }

    // 获取空下部区域索引（通过距离检测）
    public int FarStashPinkPeckDodgeChinaBefit(List<Keel> draggingCards)
    {
        for (int i = 0; i < KickSwear.Instance.Sum_PinkPeck.Count; i++)
        {
            if (PinkPeck[i].Count == 0)
            {
                Vector3 targetPos = KickSwear.Instance.Sum_PinkPeck[i].position;
                // 检查拖动牌列表中是否有牌距离目标位置足够近
                foreach (var card in draggingCards)
                {
                    float distance = Vector3.Distance(card.transform.position, targetPos);
                    if (distance <= WaxyEvidenceCigar)
                        return i;
                }
            }
        }
        return -1;
    }

    // 能否移动到空下部区域（只能K）
    public bool TheSlumBeStashPinkPeck(Keel movingCard)
    {
        return movingCard.Line.Value == CardValue.K;
    }
    // 移动到空下部区域
    public bool SlumKeelRobinBeStashPinkPeck(Keel movingCard, int areaIdx)
    {
        ShedWidowHutNote(); // 撤销快照
        PegSlumWaist(); // 增加操作次数
        bool result = false;
        var source = FindKeelIodine(movingCard);
        if (source.fromCol == -1) return false;
        List<CardData> movingList;
        bool fromUpArea = false;
        if (source.fromUpDeckFaceUp)
        {
            movingList = new List<CardData> { OfPram_HighOf[source.fromIdx] };
            OfPram_HighOf.RemoveAt(source.fromIdx);
        }
        else if (source.fromUpArea)
        {
            movingList = new List<CardData> { OfPeck[source.fromUpAreaIdx].Last() };
            OfPeck[source.fromUpAreaIdx].RemoveAt(OfPeck[source.fromUpAreaIdx].Count - 1);
            fromUpArea = true;
        }
        else
        {
            movingList = PinkPeck[source.fromCol].GetRange(source.fromIdx, PinkPeck[source.fromCol].Count - source.fromIdx);
            PinkPeck[source.fromCol].RemoveRange(source.fromIdx, PinkPeck[source.fromCol].Count - source.fromIdx);
        }
        PinkPeck[areaIdx].AddRange(movingList);
        Vector3 AddScoreEffectPos = Vector3.zero;
        for (int k = 0; k < movingList.Count; k++)
        {
            Keel cardObj = FarKeelBur(movingList[k]);
            cardObj.transform.SetParent(KickSwear.Instance.KeelQuasar_PinkPeck);
            cardObj.transform.SetAsLastSibling();
            Vector3 targetPos = KickSwear.Instance.Sum_PinkPeck[areaIdx].position + new Vector3(0, -(PinkPeck[areaIdx].Count - movingList.Count + k) * Inn_Y);
            cardObj.transform.DOMove(targetPos, 0.3f).SetEase(Ease.OutQuad);
            LeafyLeg.FarBefriend().HaulGuinea(LeafyJoke.UIMusic.SFX_CardLink);
            if (k == 0)
                AddScoreEffectPos = targetPos;
        }
        if (!source.fromUpDeckFaceUp && !source.fromUpArea)
            CigarBarbTulipSlum(source.fromCol);
        CigarKickRid();
        result = true;
        ShedKick();
        if (fromUpArea)
            KickSwear.Instance.PegDense(-10, AddScoreEffectPos);
        return result;
    }

    // 获取上部接龙区索引（通过距离检测）
    public int FarOfPeckDodgeChinaBefit(List<Keel> draggingCards)
    {
        for (int i = 0; i < KickSwear.Instance.Sum_OfPeck.Count; i++)
        {
            Vector3 targetPos = KickSwear.Instance.Sum_OfPeck[i].position;
            // 检查拖动牌列表中是否有牌距离目标位置足够近
            foreach (var card in draggingCards)
            {
                float distance = Vector3.Distance(card.transform.position, targetPos);
                if (distance <= WaxyEvidenceCigar)
                    return i;
            }
        }
        return -1;
    }
    // 能否移动到上部接龙区
    public bool TheSlumBeOfPeck(Keel movingCard, int upAreaIdx)
    {
        // 上部接龙区只能放置单张牌
        var OvercastBrass = FarDecisionBrass(movingCard);
        if (OvercastBrass.Count > 1)
            return false;

        var area = OfPeck[upAreaIdx];
        if (area.Count == 0)
            return movingCard.Line.Value == CardValue.One; // 只能A
        var last = area.Last();
        return movingCard.Line.Leap == last.Leap && (int)movingCard.Line.Value == (int)last.Value + 1;
    }
    // 复活专用：能否移动到上部接龙区（不检查拖动牌数量）
    public bool TheSlumBeOfPeckHutGraham(Keel movingCard, int upAreaIdx)
    {
        var area = OfPeck[upAreaIdx];
        if (area.Count == 0)
            return movingCard.Line.Value == CardValue.One; // 只能A
        var last = area.Last();
        return movingCard.Line.Leap == last.Leap && (int)movingCard.Line.Value == (int)last.Value + 1;
    }
    // 移动到上部接龙区
    public bool SlumKeelBeOfPeck(Keel movingCard, int upAreaIdx)
    {
        ShedWidowHutNote(); // 撤销快照
        PegSlumWaist(); // 增加操作次数
        bool result = false;
        var source = FindKeelIodine(movingCard);
        if (source.fromCol == -1) return false;
        CardData cardData;
        int addScore = 0;
        if (source.fromUpDeckFaceUp)
        {
            cardData = OfPram_HighOf[source.fromIdx];
            OfPram_HighOf.RemoveAt(source.fromIdx);
            addScore = 15;
        }
        else if (source.fromUpArea)
        {
            cardData = OfPeck[source.fromUpAreaIdx].Last();
            OfPeck[source.fromUpAreaIdx].RemoveAt(OfPeck[source.fromUpAreaIdx].Count - 1);
            // 拖回下部接龙区时减分
            addScore = 0;
        }
        else
        {
            cardData = PinkPeck[source.fromCol][source.fromIdx];
            PinkPeck[source.fromCol].RemoveAt(source.fromIdx);
            addScore = 10;
        }
        OfPeck[upAreaIdx].Add(cardData);
        var cardObj2 = FarKeelBur(cardData);
        cardObj2.transform.SetParent(KickSwear.Instance.KeelQuasar_OfPeck);
        cardObj2.transform.SetAsLastSibling();
        Vector3 targetPos = KickSwear.Instance.Sum_OfPeck[upAreaIdx].position;
        cardObj2.transform.DOMove(targetPos, 0.3f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            LeafyLeg.FarBefriend().HaulBalance(Lofelt.NiceVibrations.HapticPatterns.PresetType.MediumImpact);
        });
        LeafyLeg.FarBefriend().HaulGuinea(LeafyJoke.UIMusic.SFX_CardCollect);

        // 检查是否是第一次收入上部接龙区  检测是否触发slot
        CigarOfPeckComputerBusCigarDish(cardData, upAreaIdx);

        if (!source.fromUpDeckFaceUp && !source.fromUpArea)
            CigarBarbTulipSlum(source.fromCol);
        CigarKickRid();
        // 触发自动收牌功能
        if (TautDynamicFactor)
            StartCoroutine(TautDynamicBrassViability());
        result = true;
        ShedKick();
        if (addScore > 0)
            KickSwear.Instance.PegDense(addScore, targetPos);
        return result;
    }

    // 移动后自动翻牌
    private void CigarBarbTulipSlum(int fromCol)
    {
        var area = PinkPeck[fromCol];
        if (area.Count > 0 && !area.Last().UpHighOf)
        {
            // 智能换牌逻辑：优先把缺的牌翻出来
            TryCedarBarbKeel(fromCol);
            area.Last().UpHighOf = true;
            var cardObj = FarKeelBur(area.Last());
            if (cardObj != null)
            {
                // 添加翻牌旋转动画
                StartCoroutine(BarbKeelSteamship(cardObj, area.Last()));

                //翻牌后判断是否触发小游戏 胜利不触发
                if (!UpUrnExorbitant)
                {
                    string SpecialType = "Null";
                    if (BrieflyKickMoldShudder > ToeBoldLeg.instance._KickLine.downarea_specialgame_time)
                    {
                        SpecialType = KickLineRancher.FarBefriend().FarBrieflyKickJokeSoRevert(ToeBoldLeg.instance._KickLine.downarea_timespecialgame_weight_group);
                        MoldRancher.FarBefriend().Award(0.15f, () =>
                        {
                            cardObj.BindBriefly(SpecialType);
                            if (SpecialType == "ScratchCard" && !UpDepositBriefly_ThriftyKeel)
                            {
                                UpDepositBriefly = true;
                                UpDepositBriefly_ThriftyKeel = true;
                                WhimInuitRemove.FarBefriend().LeafInuit("1009", "1");
                            }
                            else if (SpecialType == "Money" && !UpDepositBriefly_Sonic)
                            {
                                UpDepositBriefly = true;
                                UpDepositBriefly_Sonic = true;
                                WhimInuitRemove.FarBefriend().LeafInuit("1009", "3");
                            }
                            BrieflyKickMoldShudder = 0;
                        });
                        //print("触发特殊游戏：时间触发： 当前时间： " + BrieflyKickMoldShudder + " 触发时间： " + ToeBoldLeg.instance._KickLine.downarea_specialgame_time + " 类型： " + SpecialType);
                    }
                    else
                    {
                        SpecialType = KickLineRancher.FarBefriend().FarBrieflyKickJokeSoRevert(ToeBoldLeg.instance._KickLine.downarea_specialgame_weight_group);
                        if (SpecialType != "Null")
                        {
                            MoldRancher.FarBefriend().Award(0.15f, () =>
                            {
                                cardObj.BindBriefly(SpecialType);
                                if (SpecialType == "LuckyWheel")
                                {
                                    AwaitFaithWaist++;
                                    ShedLineRancher.CudWit("LuckyWheelCount", AwaitFaithWaist);
                                    if (AwaitFaithWaist >= 4 && !UpDepositBriefly_AwaitFaith)
                                    {
                                        UpDepositBriefly = true;
                                        UpDepositBriefly_AwaitFaith = true;
                                    }
                                    LeafyLeg.FarBefriend().HaulGuinea(LeafyJoke.UIMusic.SFX_LuckyWheelCollection);
                                    WhimInuitRemove.FarBefriend().LeafInuit("1009", "2");
                                }
                            });
                        }
                        //print("触发特殊游戏： 概率触发： 类型： " + SpecialType);
                    }
                }
                else
                {
                    UpDepositBriefly_ThriftyKeel = false;
                    UpDepositBriefly_AwaitFaith = false;
                    UpDepositBriefly_Sonic = false;
                    UpDepositBriefly_Dish = false;
                }
            }
        }
    }

    // 翻牌旋转动画
    private IEnumerator BarbKeelSteamship(Keel cardObj, CardData cardData)
    {
        LeafyLeg.FarBefriend().HaulGuinea(LeafyJoke.UIMusic.SFX_CardOpen);
        KickSwear.Instance.BindVerb();
        // 第一阶段：旋转到90度
        cardObj.transform.DORotate(new Vector3(0, 90, 0), 0.15f).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(0.15f);
        // 在90度时切换牌面显示
        cardObj.Deaf(cardData);
        // 第二阶段：旋转回0度
        cardObj.transform.DORotate(Vector3.zero, 0.15f).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(0.15f);
        KickSwear.Instance.SendVerb();
        CigarKickRid();
    }

    //触发小游戏
    void DepositBrieflyKick()
    {
        if (MimeBrieflyKickIE != null)
            MoldRancher.FarBefriend().FloeAward(MimeBrieflyKickIE);

        if (KickSwear.Instance.FinishSwear.activeSelf
        || KickSwear.Instance._HiccupDish.gameObject.activeSelf
        || (UIRancher.FarBefriend().FarSwearSoStew(nameof(ThriftySwear)) != null && UIRancher.FarBefriend().FarSwearSoStew(nameof(ThriftySwear)).gameObject.activeSelf)
        || (UIRancher.FarBefriend().FarSwearSoStew(nameof(AwaitFaithSwear)) != null && UIRancher.FarBefriend().FarSwearSoStew(nameof(AwaitFaithSwear)).gameObject.activeSelf)
        || (UIRancher.FarBefriend().FarSwearSoStew(nameof(StarveSwear)) != null && UIRancher.FarBefriend().FarSwearSoStew(nameof(StarveSwear)).gameObject.activeSelf))
            return;

        if (UpDepositBriefly_Dish)
        {
            KickSwear.Instance.BindVerb();
            UpPlayedBrieflyDish = true;
            ShedLineRancher.CudGray("IsPlayedSpecialSlot", true);
            MimeBrieflyKickIE = MoldRancher.FarBefriend().Award(1, () =>
            {
                UpDepositBriefly = false;
                UpDepositBriefly_Dish = false;
                KickSwear.Instance.SendVerb();
                KickSwear.Instance.HaulHiccupDish();
                MimeBrieflyKickIE = null;
            });
        }
        else if (UpDepositBriefly_ThriftyKeel)
        {
            KickSwear.Instance.BindVerb();
            MimeBrieflyKickIE = MoldRancher.FarBefriend().Award(1, () =>
            {
                UpDepositBriefly = false;
                UpDepositBriefly_ThriftyKeel = false;
                KickSwear.Instance.SendVerb();
                UIRancher.FarBefriend().BindUIPlace(nameof(ThriftySwear));
                MimeBrieflyKickIE = null;
            });
        }
        else if (UpDepositBriefly_AwaitFaith)
        {
            KickSwear.Instance.BindVerb();
            MimeBrieflyKickIE = MoldRancher.FarBefriend().Award(1.5f, () =>
            {
                UpDepositBriefly = false;
                AwaitFaithWaist = 0;
                ShedLineRancher.CudWit("LuckyWheelCount", AwaitFaithWaist);
                KickSwear.Instance.CudAwaitFaithGene();
                UpDepositBriefly_AwaitFaith = false;
                KickSwear.Instance.SendVerb();
                UIRancher.FarBefriend().BindUIPlace(nameof(AwaitFaithSwear));
                MimeBrieflyKickIE = null;
            });
        }
        else if (UpDepositBriefly_Sonic)
        {
            KickSwear.Instance.BindVerb();
            MimeBrieflyKickIE = MoldRancher.FarBefriend().Award(1, () =>
            {
                UpDepositBriefly = false;
                UpDepositBriefly_Sonic = false;
                KickSwear.Instance.SendVerb();
                RewardData Date = KickLineRancher.FarBefriend().FarStarveLineSoRevertBusDelay(ToeBoldLeg.instance._KickLine.downarea_filp_money_data_list);
                if (Date.type == RewardType.Gold)
                    UIRancher.FarBefriend().BindUIPlace(nameof(StarveSwear)).GetComponent<StarveSwear>().Deaf(Date, null, null, "1011");
                else if (Date.type == RewardType.Cash)
                    UIRancher.FarBefriend().BindUIPlace(nameof(StarveSwear)).GetComponent<StarveSwear>().Deaf(null, Date, null, "1011");
                MimeBrieflyKickIE = null;
            });
        }
        else
        {
            CigarKickRid();
        }
    }

    // 获取CardData对应的Card对象
    private Keel FarKeelBur(CardData data)
    {
        PoseLineBeKeelAgo.TryGetValue(data, out Keel card);
        return card;
    }
    // 提取公共的查找逻辑
    private (int fromCol, int fromIdx, bool fromUpDeckFaceUp, bool fromUpArea, int fromUpAreaIdx) FindKeelIodine(Keel movingCard)
    {
        int fromCol = -1, fromIdx = -1;
        bool fromUpDeckFaceUp = false;
        bool fromUpArea = false;
        int fromUpAreaIdx = -1;

        // 从下部接龙区查找
        for (int c = 0; c < PinkPeck.Count; c++)
        {
            int f = PinkPeck[c].FindIndex(x => FarKeelBur(x) == movingCard);
            if (f != -1) { fromCol = c; fromIdx = f; break; }
        }
        // 从上部牌堆正面区查找
        if (fromCol == -1)
        {
            int f = OfPram_HighOf.FindIndex(x => FarKeelBur(x) == movingCard);
            if (f != -1) { fromCol = -2; fromIdx = f; fromUpDeckFaceUp = true; }
        }
        // 从上部接龙区查找
        if (fromCol == -1)
        {
            for (int u = 0; u < OfPeck.Count; u++)
            {
                if (OfPeck[u].Count > 0 && FarKeelBur(OfPeck[u].Last()) == movingCard)
                {
                    fromCol = -3; fromIdx = 0; fromUpArea = true; fromUpAreaIdx = u; break;
                }
            }
        }

        return (fromCol, fromIdx, fromUpDeckFaceUp, fromUpArea, fromUpAreaIdx);
    }
    #endregion

    #region  胜利失败判定
    // 在各种操作结束后调用此方法进行判定
    public void CigarKickRid(bool AutoCollect = false)
    {
        if (KickSwear.Instance.FinishSwear.activeSelf
        || KickSwear.Instance._HiccupDish.gameObject.activeSelf
        || (UIRancher.FarBefriend().FarSwearSoStew(nameof(ThriftySwear)) != null && UIRancher.FarBefriend().FarSwearSoStew(nameof(ThriftySwear)).gameObject.activeSelf)
        || (UIRancher.FarBefriend().FarSwearSoStew(nameof(AwaitFaithSwear)) != null && UIRancher.FarBefriend().FarSwearSoStew(nameof(AwaitFaithSwear)).gameObject.activeSelf)
        || (UIRancher.FarBefriend().FarSwearSoStew(nameof(StarveSwear)) != null && UIRancher.FarBefriend().FarSwearSoStew(nameof(StarveSwear)).gameObject.activeSelf))
            return;

        // 检查胜利条件
        if (CigarUrn())
        {
            UpDepositBriefly_ThriftyKeel = false;
            UpDepositBriefly_AwaitFaith = false;
            UpDepositBriefly_Sonic = false;
            UpDepositBriefly_Dish = false;
            return;
        }

        // 检查小游戏待触发
        if (UpDepositBriefly_ThriftyKeel || UpDepositBriefly_AwaitFaith || UpDepositBriefly_Sonic || UpDepositBriefly_Dish)
        {
            DepositBrieflyKick();
            return;
        }

        //检查是否所有下部接龙区的牌都已翻开
        bool allCardsFaceUp = true;
        for (int i = 0; i < PinkPeck.Count; i++)
        {
            for (int j = 0; j < PinkPeck[i].Count; j++)
            {
                if (!PinkPeck[i][j].UpHighOf)
                {
                    allCardsFaceUp = false;
                    break;
                }
            }
            if (!allCardsFaceUp) break;
        }

        // 如果所有牌都已翻开，触发自动收牌
        if (allCardsFaceUp)
        {
            StartCoroutine(Urn_TautDynamic());
            return;
        }

        // 检查游戏是否卡死
        if (UpKickGreat())
        {
            KickSwear.Instance.Jobber(false);
            return;
        }

        // 触发自动收牌功能
        if (AutoCollect && TautDynamicFactor)
            StartCoroutine(TautDynamicBrassViability());
    }

    // 胜利判定
    bool CigarUrn()
    {
        int count = 0;
        for (int i = 0; i < OfPeck.Count; i++)
        {
            if (OfPeck[i].Count == 13)
                count++;
        }
        if (count == 4)
        {
            Debug.Log("胜利！");
            KickSwear.Instance.Jobber(true);
            return true;
        }
        return false;
    }

    // 检查游戏是否卡死
    bool UpKickGreat()
    {
        // 检查是否还有任何可能的移动
        // 1. 检查上部接龙区移动
        if (CigarMayBoatA().AgeMay) return false;

        // 2. 检查下部接龙区内部连接
        if (CigarMayBoatB().AgeMay) return false;

        // 3. 检查上部翻牌区移动
        if (CigarMayBoatC().AgeMay) return false;

        // 4. 检查是否可以翻牌
        if (CigarMayBoatD().AgeMay) return false;

        // 如果所有检查都失败，游戏确实卡住了
        return true;
    }

    // 胜利后自动收牌（如果所有牌都已翻开）
    private IEnumerator Urn_TautDynamic()
    {
        if (UpUrnExorbitant) yield break;
        UpUrnExorbitant = true;
        KickSwear.Instance.BindVerb();
        Debug.Log("检测到所有牌都已翻开，开始自动收牌...");

        // 先把上部翻牌区背面朝上的牌全部移到正面区（视觉上不需要动画）
        for (int i = OfPram_HighPink.Count - 1; i >= 0; i--)
        {
            CardData cardData = OfPram_HighPink[i];
            Keel cardObj = FarKeelBur(cardData);
            if (cardObj != null)
                cardObj.SortFifth.SetActive(false);
            OfPram_HighOf.Add(cardData);
        }
        OfPram_HighPink.Clear();

        bool hasCollected;
        do
        {
            hasCollected = false;
            // 1. 检查下部接龙区
            for (int i = 0; i < PinkPeck.Count; i++)
            {
                if (PinkPeck[i].Count > 0)
                {
                    CardData cardData = PinkPeck[i].Last();
                    Keel cardObj = FarKeelBur(cardData);
                    for (int k = 0; k < OfPeck.Count; k++)
                    {
                        if (TheSlumBeOfPeck(cardObj, k))
                        {
                            PinkPeck[i].RemoveAt(PinkPeck[i].Count - 1);
                            OfPeck[k].Add(cardData);

                            // 检查是否是第一次收入上部接龙区  检测是否触发slot
                            CigarOfPeckComputerBusCigarDish(cardData, k);

                            // UI动画
                            if (cardObj != null)
                            {
                                cardObj.transform.SetParent(KickSwear.Instance.KeelQuasar_OfPeck);
                                cardObj.transform.SetAsLastSibling();
                                Vector3 targetPos = KickSwear.Instance.Sum_OfPeck[k].position;
                                cardObj.transform.DOMove(targetPos, 0.2f).SetEase(Ease.OutBack);
                                cardObj.transform.DOScale(Vector3.one * 1.1f, 0.1f).SetEase(Ease.OutQuad)
                                    .OnComplete(() => { cardObj.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.InQuad); });
                            }
                            hasCollected = true;
                            yield return new WaitForSeconds(0.15f);
                            break;
                        }
                    }
                    if (hasCollected) break;
                }
            }
            // 2. 检查上部翻牌区
            if (!hasCollected && OfPram_HighOf.Count > 0)
            {
                // 遍历上部翻牌区的所有牌，从最上面开始检查
                for (int i = OfPram_HighOf.Count - 1; i >= 0; i--)
                {
                    CardData cardData = OfPram_HighOf[i];
                    Keel cardObj = FarKeelBur(cardData);
                    for (int k = 0; k < OfPeck.Count; k++)
                    {
                        if (TheSlumBeOfPeck(cardObj, k))
                        {
                            OfPram_HighOf.RemoveAt(i);
                            OfPeck[k].Add(cardData);

                            // 检查是否是第一次收入上部接龙区  检测是否触发slot
                            CigarOfPeckComputerBusCigarDish(cardData, k);

                            if (cardObj != null)
                            {
                                cardObj.transform.SetParent(KickSwear.Instance.KeelQuasar_OfPeck);
                                cardObj.transform.SetAsLastSibling();
                                Vector3 targetPos = KickSwear.Instance.Sum_OfPeck[k].position;
                                cardObj.transform.DOMove(targetPos, 0.2f).SetEase(Ease.OutBack);
                                cardObj.transform.DOScale(Vector3.one * 1.1f, 0.1f).SetEase(Ease.OutQuad)
                                    .OnComplete(() => { cardObj.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.InQuad); });
                            }
                            hasCollected = true;
                            yield return new WaitForSeconds(0.15f);
                            break;
                        }
                    }
                    if (hasCollected) break;
                }
            }
        } while (hasCollected && !UpDepositBriefly);

        UpUrnExorbitant = false;
        //KickSwear.Instance.HideMask();

        // 等待所有动画完成后，统一触发小游戏和结算
        yield return new WaitForSeconds(0.2f);
        CigarKickRid();
    }
    #endregion

    #region  相应点击牌自动移动
    // 自动移动单张牌（优先上部接龙区，其次下部接龙区）
    public bool TautSlumKeel(Keel card)
    {
        // 优先尝试移动到上部接龙区
        for (int i = 0; i < OfPeck.Count; i++)
        {
            if (TheSlumBeOfPeck(card, i))
            {
                return SlumKeelBeOfPeck(card, i);
            }
        }
        // 尝试移动到下部接龙区
        for (int i = 0; i < PinkPeck.Count; i++)
        {
            if (PinkPeck[i].Count > 0)
            {
                Keel targetCard = FarKeelBur(PinkPeck[i].Last());
                if (TheRobinDy(card, targetCard))
                {
                    return SlumKeelRobin(card, targetCard);
                }
            }
        }
        // 尝试移动到空的下部接龙区（如果是K）
        for (int i = 0; i < PinkPeck.Count; i++)
        {
            if (PinkPeck[i].Count == 0 && TheSlumBeStashPinkPeck(card))
            {
                return SlumKeelRobinBeStashPinkPeck(card, i);
            }
        }
        return false;
    }

    // 自动移动一组牌到下部接龙区尾部
    public bool TautSlumKeelRobin(Keel topCard)
    {
        var OvercastBrass = FarDecisionBrass(topCard);
        if (OvercastBrass.Count <= 1) return false; // 只有单张牌不处理
        // 查找最佳目标位置
        int bestTargetCol = -1;
        Keel bestTargetCard = null;
        // 优先查找能接上的牌
        for (int i = 0; i < PinkPeck.Count; i++)
        {
            if (PinkPeck[i].Count > 0)
            {
                Keel targetCard = FarKeelBur(PinkPeck[i].Last());
                if (TheRobinDy(topCard, targetCard))
                {
                    bestTargetCol = i;
                    bestTargetCard = targetCard;
                    break;
                }
            }
        }
        // 如果找到目标，执行移动
        if (bestTargetCard != null)
        {
            return SlumKeelRobin(topCard, bestTargetCard);
        }
        // 尝试移动到空的下部接龙区（如果是K）
        for (int i = 0; i < PinkPeck.Count; i++)
        {
            if (PinkPeck[i].Count == 0 && TheSlumBeStashPinkPeck(topCard))
            {
                return SlumKeelRobinBeStashPinkPeck(topCard, i);
            }
        }
        return false;
    }
    #endregion

    #region  提示

    // 步骤a：检查下部接龙区和上部翻牌区的龙尾牌是否可以移动到上部接龙区
    public TipResult CigarMayBoatA()
    {
        // 检查下部接龙区的龙尾牌
        for (int i = 0; i < PinkPeck.Count; i++)
        {
            if (PinkPeck[i].Count > 0)
            {
                CardData tailCard = PinkPeck[i].Last();
                if (tailCard.UpHighOf)
                {
                    Keel cardObj = FarKeelBur(tailCard);
                    for (int j = 0; j < OfPeck.Count; j++)
                    {
                        if (TheSlumBeOfPeck(cardObj, j))
                        {
                            Vector3 targetPos = KickSwear.Instance.Sum_OfPeck[j].position;
                            return new TipResult(true, cardObj, targetPos, $"可以将{FarKeelPartnerStew(tailCard)}移动到上部接龙区");
                        }
                    }
                }
            }
        }

        // 检查上部翻牌区的牌
        if (OfPram_HighOf.Count > 0)
        {
            CardData faceUpCard = OfPram_HighOf.Last();
            Keel cardObj = FarKeelBur(faceUpCard);
            for (int j = 0; j < OfPeck.Count; j++)
            {
                if (TheSlumBeOfPeck(cardObj, j))
                {
                    Vector3 targetPos = KickSwear.Instance.Sum_OfPeck[j].position;
                    return new TipResult(true, cardObj, targetPos, $"可以将{FarKeelPartnerStew(faceUpCard)}移动到上部接龙区");
                }
            }
        }

        return new TipResult(false, null, Vector3.zero);
    }

    // 步骤b：检查下部接龙区内部连接
    public TipResult CigarMayBoatB()
    {
        // 遍历所有下部接龙区，查找可以连接的牌
        for (int i = 0; i < PinkPeck.Count; i++)
        {
            if (PinkPeck[i].Count == 0) continue;

            // 查找当前区域的龙尾牌
            CardData tailCard = PinkPeck[i].Last();
            if (!tailCard.UpHighOf) continue;

            Keel tailCardObj = FarKeelBur(tailCard);

            // 查找其他区域的龙头牌（最上面的牌）
            for (int j = 0; j < PinkPeck.Count; j++)
            {
                if (i == j || PinkPeck[j].Count == 0) continue;

                // 查找j区域中连续正面朝上的牌组
                for (int k = 0; k < PinkPeck[j].Count; k++)
                {
                    if (PinkPeck[j][k].UpHighOf)
                    {
                        Keel headCardObj = FarKeelBur(PinkPeck[j][k]);
                        if (TheRobinDy(headCardObj, tailCardObj))
                        {
                            Vector3 targetPos = tailCardObj.transform.position;
                            return new TipResult(true, headCardObj, targetPos, $"可以将{FarKeelPartnerStew(PinkPeck[j][k])}连接到{FarKeelPartnerStew(tailCard)}");
                        }
                        break; // 只检查第一个正面朝上的牌
                    }
                }
            }
        }

        // 检查K牌是否可以移动到空的下部接龙区
        int bestMoveScore = 0;
        TipResult bestKMove = new TipResult(false, null, Vector3.zero);

        for (int i = 0; i < PinkPeck.Count; i++)
        {
            if (PinkPeck[i].Count == 0) continue; // 跳过空区域

            // 查找当前区域中不在龙头的K牌
            for (int j = 0; j < PinkPeck[i].Count - 1; j++) // 不检查最后一张牌（龙头）
            {
                if (PinkPeck[i][j].UpHighOf && PinkPeck[i][j].Value == CardValue.K)
                {
                    // 检查从K牌开始到区域末尾是否都是连续正面朝上的牌
                    bool canMove = true;
                    int DealWaist = 0;
                    for (int checkIdx = j; checkIdx < PinkPeck[i].Count; checkIdx++)
                    {
                        if (!PinkPeck[i][checkIdx].UpHighOf)
                        {
                            canMove = false;
                            break;
                        }
                        DealWaist++;
                    }

                    if (canMove && DealWaist > bestMoveScore)
                    {
                        // 检查是否有空的下部接龙区可以移动
                        for (int k = 0; k < PinkPeck.Count; k++)
                        {
                            if (PinkPeck[k].Count == 0)
                            {
                                // 只有当K牌不是龙头时才提示移动（避免K牌在空位时的错误提示）
                                if (j > 0)
                                {
                                    Keel kCardObj = FarKeelBur(PinkPeck[i][j]);
                                    Vector3 targetPos = KickSwear.Instance.Sum_PinkPeck[k].position;
                                    bestKMove = new TipResult(true, kCardObj, targetPos, $"可以将K牌及其下方{DealWaist}张牌移动到空的下部接龙区");
                                    bestMoveScore = DealWaist;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        if (bestKMove.AgeMay)
        {
            return bestKMove;
        }

        return new TipResult(false, null, Vector3.zero);
    }

    // 步骤c：检查上部翻牌区未翻开的牌是否可以移动
    public TipResult CigarMayBoatC()
    {
        // 检查上部翻牌区是否有牌可以移动
        if (OfPram_HighOf.Count > 0)
        {
            CardData faceUpCard = OfPram_HighOf.Last();
            Keel cardObj = FarKeelBur(faceUpCard);

            // 检查是否可以移动到下部接龙区
            for (int i = 0; i < PinkPeck.Count; i++)
            {
                if (PinkPeck[i].Count > 0)
                {
                    Keel targetCard = FarKeelBur(PinkPeck[i].Last());
                    if (TheRobinDy(cardObj, targetCard))
                    {
                        Vector3 targetPos = targetCard.transform.position;
                        return new TipResult(true, cardObj, targetPos, $"可以将{FarKeelPartnerStew(faceUpCard)}移动到下部接龙区");
                    }
                }
                else if (TheSlumBeStashPinkPeck(cardObj))
                {
                    Vector3 targetPos = KickSwear.Instance.Sum_PinkPeck[i].position;
                    return new TipResult(true, cardObj, targetPos, $"可以将{FarKeelPartnerStew(faceUpCard)}移动到空的下部接龙区");
                }
            }
        }

        return new TipResult(false, null, Vector3.zero);
    }

    // 步骤d：提示翻牌
    public TipResult CigarMayBoatD()
    {
        // 检查是否需要翻牌
        if (OfPram_HighPink.Count > 0)
        {
            // 还有牌可以翻
            Vector3 targetPos = KickSwear.Instance.Sum_OfPram_HighOf.position;
            return new TipResult(true, null, targetPos, "点击翻牌按钮翻开新牌");
        }
        else if (OfPram_HighOf.Count > 0)
        {
            // 优化：遍历所有正面朝上的牌，判断是否有能移动的
            bool hasMovable = false;
            CardData movableCard = null;
            Vector3 movableTarget = Vector3.zero;
            string moveMsg = "";
            foreach (var cardData in OfPram_HighOf)
            {
                Keel cardObj = FarKeelBur(cardData);
                // 检查能否移动到下部接龙区
                for (int i = 0; i < PinkPeck.Count; i++)
                {
                    if (PinkPeck[i].Count > 0)
                    {
                        Keel targetCard = FarKeelBur(PinkPeck[i].Last());
                        if (TheRobinDy(cardObj, targetCard))
                        {
                            hasMovable = true;
                            movableCard = cardData;
                            movableTarget = targetCard.transform.position;
                            moveMsg = $"可以将{FarKeelPartnerStew(cardData)}移动到下部接龙区";
                            break;
                        }
                    }
                    else if (TheSlumBeStashPinkPeck(cardObj))
                    {
                        hasMovable = true;
                        movableCard = cardData;
                        movableTarget = KickSwear.Instance.Sum_PinkPeck[i].position;
                        moveMsg = $"可以将{FarKeelPartnerStew(cardData)}移动到空的下部接龙区";
                        break;
                    }
                }
                if (hasMovable) break;
                // 检查能否移动到上部接龙区
                for (int j = 0; j < OfPeck.Count; j++)
                {
                    if (TheSlumBeOfPeck(cardObj, j))
                    {
                        hasMovable = true;
                        movableCard = cardData;
                        movableTarget = KickSwear.Instance.Sum_OfPeck[j].position;
                        moveMsg = $"可以将{FarKeelPartnerStew(cardData)}移动到上部接龙区";
                        break;
                    }
                }
                if (hasMovable) break;
            }
            if (hasMovable)
            {
                // 有可移动的牌，提示玩家继续操作
                return new TipResult(true, FarKeelBur(movableCard), movableTarget, moveMsg);
            }
            else
            {
                // 所有正面朝上的牌都无法移动，才提示死局
                Vector3 stuckPos = KickSwear.Instance.Sum_OfPram_HighPink.position;
                return new TipResult(false, null, stuckPos, "游戏无法继续，建议重新开始");
            }
        }
        else
        {
            // 没有牌了，检查是否胜利
            if (CigarUrn())
                return new TipResult(true, null, Vector3.zero, "恭喜！游戏胜利！");
            else
                return new TipResult(false, null, Vector3.zero, "游戏结束");
        }
    }
    #endregion

    #region 自动收牌功能
    public bool TautDynamicFactor
    {
        get { return ShedLineRancher.FarGray("AutoCollectSwitch"); }
        set { ShedLineRancher.CudGray("AutoCollectSwitch", value); }
    }

    private IEnumerator TautDynamicBrassViability()
    {
        KickSwear.Instance.BindVerb();
        yield return new WaitForSeconds(0.1f);

        bool hasCollected;
        int maxIterations = 100;
        int currentIteration = 0;

        do
        {
            hasCollected = false;
            currentIteration++;

            // 1. 检查下部接龙区的龙尾牌
            for (int i = 0; i < PinkPeck.Count; i++)
            {
                if (PinkPeck[i].Count > 0)
                {
                    CardData tailCard = PinkPeck[i].Last();
                    if (tailCard.UpHighOf)
                    {
                        Keel cardObj = FarKeelBur(tailCard);
                        for (int j = 0; j < OfPeck.Count; j++)
                        {
                            if (TheSlumBeOfPeck(cardObj, j))
                            {
                                SlumKeelBeOfPeckDisagree(cardObj, j);
                                KickSwear.Instance.PegDense(10, TautSlumKeelPegDenseGuineaSum);
                                hasCollected = true;
                                yield return new WaitForSeconds(0.2f);
                                break;
                            }
                        }
                        if (hasCollected) break;
                    }
                }
            }

            // 2. 检查上部翻牌区的牌
            if (!hasCollected && OfPram_HighOf.Count > 0)
            {
                CardData faceUpCard = OfPram_HighOf.Last();
                Keel cardObj = FarKeelBur(faceUpCard);
                for (int j = 0; j < OfPeck.Count; j++)
                {
                    if (TheSlumBeOfPeck(cardObj, j))
                    {
                        SlumKeelBeOfPeckDisagree(cardObj, j);
                        KickSwear.Instance.PegDense(15, TautSlumKeelPegDenseGuineaSum);
                        hasCollected = true;
                        yield return new WaitForSeconds(0.2f);
                        break;
                    }
                }
            }

            // 3. 检查是否所有下部接龙区都没有盖着的牌
            if (!hasCollected)
            {
                bool allFaceUp = true;
                for (int i = 0; i < PinkPeck.Count; i++)
                {
                    for (int j = 0; j < PinkPeck[i].Count; j++)
                    {
                        if (!PinkPeck[i][j].UpHighOf)
                        {
                            allFaceUp = false;
                            break;
                        }
                    }
                    if (!allFaceUp) break;
                }

                // 如果所有下部接龙区都已翻开，自动收上部翻牌区所有能收的牌
                if (allFaceUp)
                {
                    bool collectedAny = false;
                    // 尝试收上部翻牌区最上面的牌（玩家可见的牌）
                    while (OfPram_HighOf.Count > 0)
                    {
                        CardData faceUpCard = OfPram_HighOf.Last();
                        Keel cardObj = FarKeelBur(faceUpCard);
                        bool moved = false;
                        for (int j = 0; j < OfPeck.Count; j++)
                        {
                            if (TheSlumBeOfPeck(cardObj, j))
                            {
                                SlumKeelBeOfPeckDisagree(cardObj, j);
                                KickSwear.Instance.PegDense(15, TautSlumKeelPegDenseGuineaSum);
                                collectedAny = true;
                                moved = true;
                                yield return new WaitForSeconds(0.2f);
                                break;
                            }
                        }
                        if (!moved) break; // 如果当前牌不能收，跳出
                    }
                    if (collectedAny)
                    {
                        hasCollected = true;
                    }
                }
            }

            // 防止死循环
            if (currentIteration > maxIterations)
            {
                Debug.LogWarning("自动收牌迭代次数过多，停止自动收牌");
                break;
            }

        } while (hasCollected && !UpDepositBriefly);

        // 检查游戏结束条件
        KickSwear.Instance.SendVerb();
        yield return new WaitForSeconds(0.2f);
        CigarKickRid();
    }

    // 内部移动方法，不触发自动收牌（避免递归）
    private bool SlumKeelBeOfPeckDisagree(Keel movingCard, int upAreaIdx)
    {
        var source = FindKeelIodine(movingCard);
        if (source.fromCol == -1) return false;

        CardData cardData;
        if (source.fromUpDeckFaceUp)
        {
            cardData = OfPram_HighOf[source.fromIdx];
            OfPram_HighOf.RemoveAt(source.fromIdx);
        }
        else if (source.fromUpArea)
        {
            cardData = OfPeck[source.fromUpAreaIdx].Last();
            OfPeck[source.fromUpAreaIdx].RemoveAt(OfPeck[source.fromUpAreaIdx].Count - 1);
        }
        else
        {
            cardData = PinkPeck[source.fromCol][source.fromIdx];
            PinkPeck[source.fromCol].RemoveAt(source.fromIdx);
        }
        OfPeck[upAreaIdx].Add(cardData);
        var cardObj2 = FarKeelBur(cardData);
        cardObj2.transform.SetParent(KickSwear.Instance.KeelQuasar_OfPeck);
        cardObj2.transform.SetAsLastSibling();

        // 检查是否是第一次收入上部接龙区  检测是否触发slot
        CigarOfPeckComputerBusCigarDish(cardData, upAreaIdx);

        // 添加收牌动画
        Vector3 targetPos = KickSwear.Instance.Sum_OfPeck[upAreaIdx].position;
        TautSlumKeelPegDenseGuineaSum = targetPos;
        cardObj2.transform.DOMove(targetPos, 0.3f).SetEase(Ease.OutQuad);
        LeafyLeg.FarBefriend().HaulGuinea(LeafyJoke.UIMusic.SFX_CardCollect);

        if (!source.fromUpDeckFaceUp && !source.fromUpArea)
            CigarBarbTulipSlum(source.fromCol);

        return true;
    }
    #endregion

    #region 撤销功能
    private void ShedWidowHutNote()
    {
        KlondikeGameState state = new KlondikeGameState();
        state.OfPram_HighOf = HostFadeKeelLineTrip(OfPram_HighOf);
        state.OfPram_HighPink = HostFadeKeelLineTrip(OfPram_HighPink);
        state.OfPeck = HostFadeKeelLineTripTrip(OfPeck);
        state.PinkPeck = HostFadeKeelLineTripTrip(PinkPeck);
        // 保存所有牌的UI状态
        state.KeelUIJobber = new List<CardUIState>();
        foreach (var card in PoseLineBeKeelAgo.Values)
        {
            if (card != null)
                state.KeelUIJobber.Add(new CardUIState(card));
        }
        // 保存操作次数和游戏时间
        state.SlumWaist = DealWaist;
        state.KickMold = FarKickMold();
        FileRobin.Push(state);
    }
    private List<CardData> HostFadeKeelLineTrip(List<CardData> src)
    {
        List<CardData> copy = new List<CardData>();
        foreach (var c in src)
            copy.Add(new CardData(c.Leap, c.Value, c.UpHighOf, c.UpOfPeckComputer));
        return copy;
    }
    private List<List<CardData>> HostFadeKeelLineTripTrip(List<List<CardData>> src)
    {
        List<List<CardData>> copy = new List<List<CardData>>();
        foreach (var l in src)
            copy.Add(HostFadeKeelLineTrip(l));
        return copy;
    }

    public void Note()
    {
        if (!TheNote())
            return;
        PegSlumWaist(); // 增加操作次数
        var state = FileRobin.Pop();
        // 先恢复数据（不刷新UI）
        OfPram_HighOf = HostFadeKeelLineTrip(state.OfPram_HighOf);
        OfPram_HighPink = HostFadeKeelLineTrip(state.OfPram_HighPink);
        OfPeck = HostFadeKeelLineTripTrip(state.OfPeck);
        PinkPeck = HostFadeKeelLineTripTrip(state.PinkPeck);
        // 恢复操作次数和游戏时间
        DealWaist = state.SlumWaist;
        float savedGameTime = state.KickMold;
        AbutBadgeMold = Time.time - savedGameTime;

        // 更新UI显示
        if (KickSwear.Instance != null)
        {
            KickSwear.Instance.SolderSlumWaist(DealWaist);
            KickSwear.Instance.SolderKickMold(savedGameTime);
        }

        // 播放撤销动画
        StartCoroutine(HaulNoteSteamship(state.KeelUIJobber));
        ShedKick();
    }
    // 撤销动画协程
    private IEnumerator HaulNoteSteamship(List<CardUIState> targetStates)
    {
        KickSwear.Instance.BindVerb();
        int animCount = 0;
        foreach (var target in targetStates)
        {
            // 找到当前对应的Card对象
            Keel card = null;
            foreach (var c in PoseLineBeKeelAgo.Values)
            {
                if (c != null && c.Line.Leap == target.Leap && c.Line.Value == target.Value)
                {
                    card = c;
                    break;
                }
            }
            if (card == null) continue;

            // 位置不同，移动动画
            if ((card.transform.position - target.Citation).sqrMagnitude > 0.01f)
            {
                animCount++;
                card.transform.DOMove(target.Citation, 0.3f).SetEase(Ease.OutQuad).OnComplete(() => { animCount--; });
            }

            // 父物体不同，动画后SetParent
            if (card.transform.parent == null || card.transform.parent.name != target.QuasarStew)
            {
                animCount++;
                card.transform.DOMove(target.Citation, 0.3f).SetEase(Ease.OutQuad).OnComplete(() =>
                {
                    // 检查card是否已被销毁
                    if (card != null)
                    {
                        // 查找目标父物体
                        Transform newParent = SealQuasarSoStew(target.QuasarStew);
                        if (newParent != null) card.transform.SetParent(newParent);
                    }
                    animCount--;
                });
            }

            // 正反面不同，翻转动画
            if (card.Line.UpHighOf != target.UpHighOf)
            {
                animCount++;
                StartCoroutine(NoteBarbKeelTire(card, target.UpHighOf, () => { animCount--; }));
            }
        }

        // 等待所有动画完成
        float waitTime = 0f;
        while (animCount > 0 && waitTime < 2f)
        {
            yield return null;
            waitTime += Time.deltaTime;
        }

        // 动画完成后，彻底刷新所有牌UI和父物体，确保一致
        BalconyRotBrass();
        KickSwear.Instance.SendVerb();
        CigarKickRid();
    }
    // 查找父物体
    private Transform SealQuasarSoStew(string name)
    {
        if (KickSwear.Instance.KeelQuasar_PinkPeck.name == name) return KickSwear.Instance.KeelQuasar_PinkPeck;
        if (KickSwear.Instance.KeelQuasar_OfPeck.name == name) return KickSwear.Instance.KeelQuasar_OfPeck;
        if (KickSwear.Instance.KeelQuasar_OfPram.name == name) return KickSwear.Instance.KeelQuasar_OfPram;
        if (KickSwear.Instance.KeelQuasar_Waxy.name == name) return KickSwear.Instance.KeelQuasar_Waxy;
        return null;
    }
    // 撤销翻牌动画
    private IEnumerator NoteBarbKeelTire(Keel card, bool toFaceUp, System.Action onEnd)
    {
        // 检查card是否已被销毁
        if (card == null)
        {
            onEnd?.Invoke();
            yield break;
        }

        // 先旋转到90度
        card.transform.DORotate(new Vector3(0, 90, 0), 0.15f).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(0.15f);

        // 再次检查card是否已被销毁
        if (card == null)
        {
            onEnd?.Invoke();
            yield break;
        }

        // 切换牌面
        card.Line.UpHighOf = toFaceUp;
        card.Deaf(card.Line);

        // 旋转回0度
        card.transform.DORotate(Vector3.zero, 0.15f).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(0.15f);

        onEnd?.Invoke();
    }
    // 刷新所有牌并带动画
    private void BalconyRotBrass()
    {
        // 清理所有Card对象
        foreach (var card in PoseLineBeKeelAgo.Values)
        {
            if (card != null)
                Destroy(card.gameObject);
        }
        PoseLineBeKeelAgo.Clear();
        // 重新初始化UpArea和DownArea，防止为空
        if (OfPeck == null || OfPeck.Count != 4)
        {
            OfPeck = new List<List<CardData>>();
            for (int i = 0; i < 4; i++) OfPeck.Add(new List<CardData>());
        }
        if (PinkPeck == null || PinkPeck.Count != 7)
        {
            PinkPeck = new List<List<CardData>>();
            for (int i = 0; i < 7; i++) PinkPeck.Add(new List<CardData>());
        }
        List<Keel> allCards = new List<Keel>();
        // 下部接龙区
        for (int i = 0; i < PinkPeck.Count; i++)
        {
            for (int j = 0; j < PinkPeck[i].Count; j++)
            {
                Keel card = DugoutKeel(PinkPeck[i][j]);
                card.transform.SetParent(KickSwear.Instance.KeelQuasar_PinkPeck);
                card.transform.localScale = Vector3.one;
                card.transform.position = KickSwear.Instance.Sum_PinkPeck[i].position + new Vector3(0, -j * Inn_Y);
                card.Deaf(PinkPeck[i][j]);
                allCards.Add(card);
            }
        }
        // 上部接龙区
        for (int i = 0; i < OfPeck.Count; i++)
        {
            for (int j = 0; j < OfPeck[i].Count; j++)
            {
                Keel card = DugoutKeel(OfPeck[i][j]);
                card.transform.SetParent(KickSwear.Instance.KeelQuasar_OfPeck);
                card.transform.localScale = Vector3.one;
                card.transform.position = KickSwear.Instance.Sum_OfPeck[i].position;
                card.Deaf(OfPeck[i][j]);
                allCards.Add(card);
            }
        }
        // 上部牌堆
        foreach (var c in OfPram_HighPink)
        {
            Keel card = DugoutKeel(c);
            card.transform.SetParent(KickSwear.Instance.KeelQuasar_OfPram);
            card.transform.localScale = Vector3.one;
            card.transform.position = KickSwear.Instance.Sum_OfPram_HighPink.position;
            card.Deaf(c);
            allCards.Add(card);
        }
        foreach (var c in OfPram_HighOf)
        {
            Keel card = DugoutKeel(c);
            card.transform.SetParent(KickSwear.Instance.KeelQuasar_OfPram);
            card.transform.localScale = Vector3.one;
            card.transform.position = KickSwear.Instance.Sum_OfPram_HighOf.position;
            card.Deaf(c);
            allCards.Add(card);
        }
        StartCoroutine(AwardSendVerbTulipTire(0.2f));
    }
    private IEnumerator AwardSendVerbTulipTire(float delay)
    {
        yield return new WaitForSeconds(delay);
        KickSwear.Instance.SendVerb();
    }
    //是否可以撤销
    public bool TheNote()
    {
        return FileRobin.Count > 0;
    }
    #endregion

    #region 存档方法
    public void ShedKick()
    {
        if (OfPeck == null)
            OfPeck = new List<List<CardData>>();
        if (PinkPeck == null)
            PinkPeck = new List<List<CardData>>();
        KlondikeGameState state = new KlondikeGameState();
        state.OfPram_HighOf = HostFadeKeelLineTrip(OfPram_HighOf);
        state.OfPram_HighPink = HostFadeKeelLineTrip(OfPram_HighPink);
        state.OfPeck = HostFadeKeelLineTripTrip(OfPeck);
        state.PinkPeck = HostFadeKeelLineTripTrip(PinkPeck);
        state.KeelUIJobber = new List<CardUIState>();
        // 保存操作次数和游戏时间
        state.SlumWaist = DealWaist;
        state.KickMold = FarKickMold();
        // 按照实际牌堆顺序生成CardUIStates
        // 1. 下部接龙区（从下到上，从左到右）
        for (int i = 0; i < PinkPeck.Count; i++)
        {
            for (int j = 0; j < PinkPeck[i].Count; j++)
            {
                Keel card = FarKeelBur(PinkPeck[i][j]);
                if (card != null)
                    state.KeelUIJobber.Add(new CardUIState(card));
            }
        }
        // 2. 上部接龙区（从左到右）
        for (int i = 0; i < OfPeck.Count; i++)
        {
            for (int j = 0; j < OfPeck[i].Count; j++)
            {
                Keel card = FarKeelBur(OfPeck[i][j]);
                if (card != null)
                    state.KeelUIJobber.Add(new CardUIState(card));
            }
        }
        // 3. 上部牌堆背面区（从下到上）
        for (int i = 0; i < OfPram_HighPink.Count; i++)
        {
            Keel card = FarKeelBur(OfPram_HighPink[i]);
            if (card != null)
                state.KeelUIJobber.Add(new CardUIState(card));
        }
        // 4. 上部牌堆正面区（从下到上）
        for (int i = 0; i < OfPram_HighOf.Count; i++)
        {
            Keel card = FarKeelBur(OfPram_HighOf[i]);
            if (card != null)
                state.KeelUIJobber.Add(new CardUIState(card));
        }
        int CardCount = state.KeelUIJobber.Count;
        if (CardCount != 52) //存档数据错误
            return;
        string json = JsonUtility.ToJson(state);
        ShedLineRancher.CudStench(ShedCan, json);
    }
    // 读档方法
    public bool RockKick()
    {
        if (!PlayerPrefs.HasKey(ShedCan))
            return false;
        string json = ShedLineRancher.FarStench(ShedCan);
        //print("读取存档： " + json);
        if (string.IsNullOrEmpty(json))
            return false;
        KlondikeGameState state = null;
        try
        {
            state = JsonUtility.FromJson<KlondikeGameState>(json);
        }
        catch (Exception e)
        {
            Debug.LogError($"存档数据解析失败: {e}");
            return false;
        }
        if (state == null) return false;
        OfPram_HighOf = HostFadeKeelLineTrip(state.OfPram_HighOf);
        OfPram_HighPink = HostFadeKeelLineTrip(state.OfPram_HighPink);
        // 兼容旧存档：如果UpArea/DownArea为空，则用CardUIStates重建
        if ((state.OfPeck == null || state.OfPeck.Count == 0) && (state.PinkPeck == null || state.PinkPeck.Count == 0) && state.KeelUIJobber != null)
        {
            ControlPeckDukeKeelUIJobber(state.KeelUIJobber);
        }
        else
        {
            OfPeck = HostFadeKeelLineTripTrip(state.OfPeck);
            PinkPeck = HostFadeKeelLineTripTrip(state.PinkPeck);
        }
        if (OfPram_HighOf == null) OfPram_HighOf = new List<CardData>();
        if (OfPram_HighPink == null) OfPram_HighPink = new List<CardData>();
        if (OfPeck == null) OfPeck = new List<List<CardData>>();
        if (PinkPeck == null) PinkPeck = new List<List<CardData>>();

        //读取上部牌堆奖励牌
        string UpDeck_RewardCard_Save = ShedLineRancher.FarStench("UpDeck_RewardCardNames");
        if (!string.IsNullOrEmpty(UpDeck_RewardCard_Save))
            OfPram_StarveKeelProse = UpDeck_RewardCard_Save.Split(',').ToList();

        // 读取操作次数和游戏时间
        DealWaist = state.SlumWaist;
        float savedGameTime = state.KickMold;
        AbutBadgeMold = Time.time - savedGameTime; // 调整开始时间以保持游戏时间

        // 更新UI显示
        if (KickSwear.Instance != null)
        {
            KickSwear.Instance.SolderSlumWaist(DealWaist);
            KickSwear.Instance.SolderKickMold(savedGameTime);
        }

        // 使用动画生成牌，类似StartNewGame的效果
        StartCoroutine(RockKickDenySteamship(state.KeelUIJobber));
        return true;
    }
    // 读档时使用动画生成牌
    private IEnumerator RockKickDenySteamship(List<CardUIState> cardUIStates)
    {
        KickSwear.Instance.BindVerb();

        // 清理所有Card对象
        foreach (var card in PoseLineBeKeelAgo.Values)
        {
            if (card != null)
                Destroy(card.gameObject);
        }
        PoseLineBeKeelAgo.Clear();

        // 重新初始化UpArea和DownArea，防止为空
        if (OfPeck == null || OfPeck.Count != 4)
        {
            OfPeck = new List<List<CardData>>();
            for (int i = 0; i < 4; i++) OfPeck.Add(new List<CardData>());
        }
        if (PinkPeck == null || PinkPeck.Count != 7)
        {
            PinkPeck = new List<List<CardData>>();
            for (int i = 0; i < 7; i++) PinkPeck.Add(new List<CardData>());
        }

        int cardIndex = 0;

        // 1. 下部接龙区（从下到上，从左到右）
        for (int i = 0; i < PinkPeck.Count; i++)
        {
            for (int j = 0; j < PinkPeck[i].Count; j++)
            {
                Keel card = DugoutKeel(PinkPeck[i][j]);
                card.transform.SetParent(KickSwear.Instance.KeelQuasar_PinkPeck);
                card.transform.localScale = Vector3.one;
                card.transform.position = KickSwear.Instance.Sum_KeelAvid.position;
                card.Deaf(PinkPeck[i][j]);

                Vector3 targetPos = KickSwear.Instance.Sum_PinkPeck[i].position + new Vector3(0, -j * Inn_Y);
                card.transform.DOMove(targetPos, 0.5f).SetDelay(cardIndex * 0.05f).OnStart(() =>
                {
                    if (i % 2 == 0)
                        LeafyLeg.FarBefriend().HaulGuinea(LeafyJoke.UIMusic.SFX_CardSet);
                });
                cardIndex++;
            }
        }

        // 2. 上部接龙区（从左到右）
        for (int i = 0; i < OfPeck.Count; i++)
        {
            for (int j = 0; j < OfPeck[i].Count; j++)
            {
                Keel card = DugoutKeel(OfPeck[i][j]);
                card.transform.SetParent(KickSwear.Instance.KeelQuasar_OfPeck);
                card.transform.localScale = Vector3.one;
                card.transform.position = KickSwear.Instance.Sum_KeelAvid.position;
                card.Deaf(OfPeck[i][j]);

                Vector3 targetPos = KickSwear.Instance.Sum_OfPeck[i].position;
                card.transform.DOMove(targetPos, 0.5f).SetDelay(cardIndex * 0.05f).OnStart(() =>
                {
                    if (i % 2 == 0)
                        LeafyLeg.FarBefriend().HaulGuinea(LeafyJoke.UIMusic.SFX_CardSet);
                });
                cardIndex++;
            }
        }

        // 3. 上部牌堆背面区（从下到上）
        for (int i = 0; i < OfPram_HighPink.Count; i++)
        {
            Keel card = DugoutKeel(OfPram_HighPink[i]);
            card.transform.SetParent(KickSwear.Instance.KeelQuasar_OfPram);
            card.transform.localScale = Vector3.one;
            card.transform.position = KickSwear.Instance.Sum_KeelAvid.position;
            card.Deaf(OfPram_HighPink[i]);

            Vector3 targetPos = KickSwear.Instance.Sum_OfPram_HighPink.position;
            card.transform.DOMove(targetPos, 0.5f).SetDelay(cardIndex * 0.03f).OnStart(() =>
                {
                    if (i % 2 == 0)
                        LeafyLeg.FarBefriend().HaulGuinea(LeafyJoke.UIMusic.SFX_CardSet);
                });
            cardIndex++;
        }

        // 4. 上部牌堆正面区（从下到上）
        for (int i = 0; i < OfPram_HighOf.Count; i++)
        {
            Keel card = DugoutKeel(OfPram_HighOf[i]);
            card.transform.SetParent(KickSwear.Instance.KeelQuasar_OfPram);
            card.transform.localScale = Vector3.one;
            card.transform.position = KickSwear.Instance.Sum_KeelAvid.position;
            card.Deaf(OfPram_HighOf[i]);

            Vector3 targetPos = KickSwear.Instance.Sum_OfPram_HighOf.position;
            card.transform.DOMove(targetPos, 0.5f).SetDelay(cardIndex * 0.03f).OnStart(() =>
                {
                    if (i % 2 == 0)
                        LeafyLeg.FarBefriend().HaulGuinea(LeafyJoke.UIMusic.SFX_CardSet);
                });
            cardIndex++;
        }

        // 等待所有动画完成
        float totalDelay = cardIndex * 0.05f + 0.5f;
        yield return new WaitForSeconds(totalDelay);

        KickSwear.Instance.SendVerb();
    }
    // 新增：根据CardUIStates自动推断并重建UpArea/DownArea
    private void ControlPeckDukeKeelUIJobber(List<CardUIState> cardUIStates)
    {
        // 先清空
        OfPeck = new List<List<CardData>>();
        PinkPeck = new List<List<CardData>>();
        for (int i = 0; i < 4; i++) OfPeck.Add(new List<CardData>());
        for (int i = 0; i < 7; i++) PinkPeck.Add(new List<CardData>());
        // 遍历所有CardUIState
        foreach (var state in cardUIStates)
        {
            CardData Mark = new CardData(state.Leap, state.Value, state.UpHighOf, state.UpOfPeckComputer);
            if (state.QuasarStew.Contains("上部接龙区"))
            {
                // 归入UpArea，按位置y排序
                int idx = 0;
                float minDist = float.MaxValue;
                for (int i = 0; i < 4; i++)
                {
                    float dist = Mathf.Abs(state.Citation.x - KickSwear.Instance.Sum_OfPeck[i].position.x);
                    if (dist < minDist) { minDist = dist; idx = i; }
                }
                OfPeck[idx].Add(Mark);
            }
            else if (state.QuasarStew.Contains("下部接龙区"))
            {
                int idx = 0;
                float minDist = float.MaxValue;
                for (int i = 0; i < 7; i++)
                {
                    float dist = Mathf.Abs(state.Citation.x - KickSwear.Instance.Sum_PinkPeck[i].position.x);
                    if (dist < minDist) { minDist = dist; idx = i; }
                }
                PinkPeck[idx].Add(Mark);
            }
            // 其他区域（如上部牌堆）不处理
        }
    }
    #endregion

    #region 智能换牌
    // 智能换牌：下部接龙区未翻开的牌在翻开前，优先把场上缺的牌翻出来
    private void TryCedarBarbKeel(int col)
    {
        // 概率控制
        if (UnityEngine.Random.value >= CedarBarbImmigration) return;
        var area = PinkPeck[col];
        if (area.Count == 0) return;
        if (area.Last().UpHighOf) return;

        // 1. 收集所有下部接龙区未翻开的牌
        List<(int col, int idx, CardData card)> unflipped = new List<(int, int, CardData)>();
        for (int c = 0; c < PinkPeck.Count; c++)
            for (int i = 0; i < PinkPeck[c].Count; i++)
                if (!PinkPeck[c][i].UpHighOf)
                    unflipped.Add((c, i, PinkPeck[c][i]));

        // 2. 优先级1：能接在下部接龙区的牌
        foreach (var entry in unflipped)
        {
            if (TheRobinDyEraPinkPeck(entry.card, entry.col))
            {
                if (entry.col != col || entry.idx != PinkPeck[col].Count - 1)
                    VeryRiverbankKeel(col, PinkPeck[col].Count - 1, entry.col, entry.idx);
                return;
            }
        }
        // 3. 优先级2：能接在上部接龙区的牌
        foreach (var entry in unflipped)
        {
            if (TheRobinDyEraOfPeck(entry.card))
            {
                if (entry.col != col || entry.idx != PinkPeck[col].Count - 1)
                    VeryRiverbankKeel(col, PinkPeck[col].Count - 1, entry.col, entry.idx);
                return;
            }
        }
        // 4. 否则不换
    }
    // 交换两张未翻开的牌的位置
    private void VeryRiverbankKeel(int colA, int idxA, int colB, int idxB)
    {
        // 交换数据
        var tmp = PinkPeck[colA][idxA];
        PinkPeck[colA][idxA] = PinkPeck[colB][idxB];
        PinkPeck[colB][idxB] = tmp;

        // 交换UI位置和父物体（无动画，玩家无感知）
        var cardA = FarKeelBur(PinkPeck[colA][idxA]);
        var cardB = FarKeelBur(PinkPeck[colB][idxB]);
        if (cardA != null && cardB != null)
        {
            // 交换父物体
            var parentA = cardA.transform.parent;
            var parentB = cardB.transform.parent;
            cardA.transform.SetParent(parentB);
            cardB.transform.SetParent(parentA);

            // 交换位置
            Vector3 posA = cardA.transform.position;
            cardA.transform.position = cardB.transform.position;
            cardB.transform.position = posA;

            // 交换层级（保持渲染顺序）
            int siblingA = cardA.transform.GetSiblingIndex();
            int siblingB = cardB.transform.GetSiblingIndex();
            cardA.transform.SetSiblingIndex(siblingB);
            cardB.transform.SetSiblingIndex(siblingA);
        }
    }

    // 能否接在下部接龙区（排除本列）
    private bool TheRobinDyEraPinkPeck(CardData card, int excludeCol)
    {
        for (int i = 0; i < PinkPeck.Count; i++)
        {
            if (i == excludeCol) continue;
            var area = PinkPeck[i];
            if (area.Count == 0) continue;
            var top = area.Last();
            var cardObj = FarKeelBur(card);
            var topObj = FarKeelBur(top);
            if (cardObj != null && topObj != null && TheRobinDy(cardObj, topObj)) return true;
        }
        return false;
    }

    // 能否接在上部接龙区
    private bool TheRobinDyEraOfPeck(CardData card)
    {
        var cardObj = FarKeelBur(card);
        if (cardObj == null) return false;
        for (int i = 0; i < OfPeck.Count; i++)
        {
            if (TheSlumBeOfPeck(cardObj, i)) return true;
        }
        return false;
    }
    #endregion

    #region  结算
    // 检查是否是第一次收入上部接龙区  检测是否触发slot
    void CigarOfPeckComputerBusCigarDish(CardData cardData, int upIdx)
    {
        if (!cardData.UpOfPeckComputer)
        {
            cardData.UpOfPeckComputer = true;
            FarOfPeckStarve(upIdx);

            //判断是否触发小游戏 slot
            if (!UpPlayedBrieflyDish)
            {
                int SevenCount = 0;
                for (int l = 0; l < OfPeck.Count; l++)
                {
                    for (int j = 0; j < OfPeck[l].Count; j++)
                    {
                        if (OfPeck[l][j].Value == CardValue.Seven)
                            SevenCount++;
                    }
                }
                if (SevenCount >= 4 && !UpDepositBriefly_Dish)
                {
                    UpDepositBriefly = true;
                    UpDepositBriefly_Dish = true;
                }
            }

            //真提现任务统计
            CashOutManager.FarBefriend().AddTaskValue("UpCollectCount", 1);
        }
    }

    public void HeBadge() //重新开始
    {
        HeavyKick();
        BadgeGunKick();
    }
    public void HeavyKick() //清空场景 清空存档 销毁场景中物体 
    {
        // 清理所有Card对象
        foreach (var card in PoseLineBeKeelAgo.Values)
        {
            if (card != null)
                Destroy(card.gameObject);
        }
        PoseLineBeKeelAgo.Clear();

        // 清空所有数据
        OfPram_HighOf.Clear();
        OfPram_HighPink.Clear();
        OfPeck.Clear();
        PinkPeck.Clear();

        // 清空撤销栈和其他状态
        FileRobin.Clear();
        CopyShudder = 0f;
        FeedDenseEssenceMold = 0;
        DealWaist = 0;
        AbutBadgeMold = Time.time;

        // 清理存档
        if (PlayerPrefs.HasKey(ShedCan))
        {
            PlayerPrefs.DeleteKey(ShedCan);
        }
    }

    public void Graham() //复活
    {
        PegSlumWaist(); // 增加操作次数
        StartCoroutine(GrahamViability());
        LeafyLeg.FarBefriend().HaulGuinea(LeafyJoke.UIMusic.SFX_Wand);
    }
    // private IEnumerator GrahamViability()
    // {
    //     KickSwear.Instance.BindVerb();
    //     bool hasRevived = false;

    //     // i. 遍历所有下部接龙区盖着的牌，检查能否收入上部接龙区
    //     for (int col = 0; col < PinkPeck.Count; col++)
    //     {
    //         for (int idx = 0; idx < PinkPeck[col].Count; idx++)
    //         {
    //             var cardData = PinkPeck[col][idx];
    //             if (!cardData.UpHighOf)
    //             {
    //                 cardData.UpHighOf = true;
    //                 Keel cardObj = FarKeelBur(cardData);
    //                 for (int upIdx = 0; upIdx < OfPeck.Count; upIdx++)
    //                 {
    //                     if (TheSlumBeOfPeckHutGraham(cardObj, upIdx))
    //                     {
    //                         // 复活：收入上部接龙区
    //                         PinkPeck[col].RemoveAt(idx);
    //                         OfPeck[upIdx].Add(cardData);
    //                         // 检查是否是第一次收入上部接龙区  检测是否触发slot
    //                         CigarOfPeckComputerBusCigarDish(cardData, upIdx);

    //                         if (cardObj != null)
    //                         {
    //                             Vector3 targetPos = KickSwear.Instance.Sum_OfPeck[upIdx].position;
    //                             cardObj.transform.DOMove(targetPos, .3f).SetEase(Ease.Linear).SetDelay(1.2f).OnStart(() =>
    //                             {
    //                                 cardObj.transform.SetParent(KickSwear.Instance.KeelQuasar_OfPeck);
    //                                 cardObj.transform.SetAsLastSibling();
    //                                 cardObj.Deaf(cardData);
    //                             });
    //                             SteamshipRevolution.CarveNeon(KickSwear.Instance.CarveNeon, cardObj.transform.position, targetPos);
    //                         }
    //                         print($"复活成功 - 收入上部接龙区：{FarKeelPartnerStew(cardData)} 移动到上部接龙区：{upIdx}");
    //                         hasRevived = true;
    //                         yield return new WaitForSeconds(1.35f);
    //                         break;
    //                     }
    //                 }
    //                 if (hasRevived) break;
    //                 cardData.UpHighOf = false; // 恢复原状态
    //             }
    //         }
    //         if (hasRevived) break;
    //     }

    //     // ii. 如果没有收入上部接龙区，检查能否在下部接龙区接龙
    //     if (!hasRevived)
    //     {
    //         for (int col = 0; col < PinkPeck.Count; col++)
    //         {
    //             for (int idx = 0; idx < PinkPeck[col].Count; idx++)
    //             {
    //                 var cardData = PinkPeck[col][idx];
    //                 if (!cardData.UpHighOf)
    //                 {
    //                     cardData.UpHighOf = true;
    //                     Keel cardObj = FarKeelBur(cardData);

    //                     // 检查能否接龙到其他下部接龙区
    //                     for (int targetCol = 0; targetCol < PinkPeck.Count; targetCol++)
    //                     {
    //                         if (targetCol == col) continue; // 跳过自己

    //                         if (PinkPeck[targetCol].Count > 0)
    //                         {
    //                             Keel targetCard = FarKeelBur(PinkPeck[targetCol].Last());
    //                             if (TheRobinDy(cardObj, targetCard))
    //                             {
    //                                 // 复活：接龙到下部接龙区
    //                                 PinkPeck[col].RemoveAt(idx);
    //                                 PinkPeck[targetCol].Add(cardData);
    //                                 if (cardObj != null)
    //                                 {
    //                                     Vector3 targetPos = KickSwear.Instance.Sum_PinkPeck[targetCol].position + new Vector3(0, -(PinkPeck[targetCol].Count - 1) * Inn_Y);
    //                                     cardObj.transform.DOMove(targetPos, .3f).SetEase(Ease.Linear).SetDelay(1.2f).OnStart(() =>
    //                                     {
    //                                         cardObj.transform.SetParent(KickSwear.Instance.KeelQuasar_PinkPeck);
    //                                         cardObj.transform.SetAsLastSibling();
    //                                         cardObj.Deaf(cardData);
    //                                     });
    //                                     SteamshipRevolution.CarveNeon(KickSwear.Instance.CarveNeon, cardObj.transform.position, targetPos);
    //                                 }
    //                                 print($"复活成功 - 下部接龙：{FarKeelPartnerStew(cardData)} 移动到下部接龙区：{targetCol}");
    //                                 hasRevived = true;
    //                                 yield return new WaitForSeconds(1.35f);
    //                                 break;
    //                             }
    //                         }
    //                         else if (cardData.Value == CardValue.K)
    //                         {
    //                             // 如果是K牌，可以移动到空的下部接龙区
    //                             PinkPeck[col].RemoveAt(idx);
    //                             PinkPeck[targetCol].Add(cardData);
    //                             if (cardObj != null)
    //                             {
    //                                 Vector3 targetPos = KickSwear.Instance.Sum_PinkPeck[targetCol].position;
    //                                 cardObj.transform.DOMove(targetPos, .3f).SetEase(Ease.Linear).SetDelay(1.2f).OnStart(() =>
    //                                 {
    //                                     cardObj.transform.SetParent(KickSwear.Instance.KeelQuasar_PinkPeck);
    //                                     cardObj.transform.SetAsLastSibling();
    //                                     cardObj.Deaf(cardData);
    //                                 });
    //                                 SteamshipRevolution.CarveNeon(KickSwear.Instance.CarveNeon, cardObj.transform.position, targetPos);
    //                             }
    //                             print($"复活成功 - K牌移动：{FarKeelPartnerStew(cardData)} 移动到空的下部接龙区：{targetCol}");
    //                             hasRevived = true;
    //                             yield return new WaitForSeconds(1.35f);
    //                             break;
    //                         }
    //                     }
    //                     if (hasRevived) break;
    //                     cardData.UpHighOf = false; // 恢复原状态
    //                 }
    //             }
    //             if (hasRevived) break;
    //         }
    //     }

    //     if (!hasRevived)
    //     {
    //         print("复活失败：没有找到可移动的牌");
    //     }

    //     yield return new WaitForSeconds(1f);
    //     ShedKick();
    //     BalconyRotBrass();
    //     KickSwear.Instance.SendVerb();
    //     CigarKickRid();
    // }

    enum MoveType { UpArea, DownArea }
    private IEnumerator GrahamViability()
    {
        KickSwear.Instance.BindVerb();
        bool hasRevived = false;

        // 创建一个列表来存储所有符合条件的牌及其移动信息
        List<(CardData cardData, int sourceCol, int targetIndex, MoveType moveType)> validMoves = new List<(CardData, int, int, MoveType)>();

        // i. 遍历所有下部接龙区盖着的牌，检查能否收入上部接龙区
        for (int col = 0; col < PinkPeck.Count; col++)
        {
            for (int idx = 0; idx < PinkPeck[col].Count; idx++)
            {
                var cardData = PinkPeck[col][idx];
                if (!cardData.UpHighOf)
                {
                    cardData.UpHighOf = true;
                    Keel cardObj = FarKeelBur(cardData);

                    // 检查能否收入上部接龙区
                    for (int upIdx = 0; upIdx < OfPeck.Count; upIdx++)
                    {
                        if (TheSlumBeOfPeckHutGraham(cardObj, upIdx))
                        {
                            validMoves.Add((cardData, col, upIdx, MoveType.UpArea));
                        }
                    }

                    cardData.UpHighOf = false; // 恢复原状态
                }
            }
        }

        // ii. 如果没有找到上部移动，检查能否在下部接龙区接龙
        if (validMoves.Count == 0)
        {
            for (int col = 0; col < PinkPeck.Count; col++)
            {
                for (int idx = 0; idx < PinkPeck[col].Count; idx++)
                {
                    var cardData = PinkPeck[col][idx];
                    if (!cardData.UpHighOf)
                    {
                        cardData.UpHighOf = true;
                        Keel cardObj = FarKeelBur(cardData);

                        // 检查能否接龙到其他下部接龙区
                        for (int targetCol = 0; targetCol < PinkPeck.Count; targetCol++)
                        {
                            if (targetCol == col) continue; // 跳过自己

                            if (PinkPeck[targetCol].Count > 0)
                            {
                                Keel targetCard = FarKeelBur(PinkPeck[targetCol].Last());
                                if (TheRobinDy(cardObj, targetCard))
                                {
                                    validMoves.Add((cardData, col, targetCol, MoveType.DownArea));
                                }
                            }
                            else if (cardData.Value == CardValue.K)
                            {
                                // K牌可以移动到空的下部接龙区
                                validMoves.Add((cardData, col, targetCol, MoveType.DownArea));
                            }
                        }

                        cardData.UpHighOf = false; // 恢复原状态
                    }
                }
            }
        }

        // 如果找到有效移动，选择点数最小的牌
        if (validMoves.Count > 0)
        {
            // 按点数排序 (A最小，K最大)
            validMoves = validMoves.OrderBy(move => (int)move.cardData.Value).ToList();

            // 获取点数最小的牌
            var selectedMove = validMoves[0];
            var cardData = selectedMove.cardData;
            cardData.UpHighOf = true; // 确认翻牌

            if (selectedMove.moveType == MoveType.UpArea)
            {
                // 执行上部移动
                int upIdx = selectedMove.targetIndex;
                PinkPeck[selectedMove.sourceCol].Remove(cardData);
                OfPeck[upIdx].Add(cardData);
                CigarOfPeckComputerBusCigarDish(cardData, upIdx);

                Keel cardObj = FarKeelBur(cardData);
                if (cardObj != null)
                {
                    Vector3 targetPos = KickSwear.Instance.Sum_OfPeck[upIdx].position;
                    cardObj.transform.DOMove(targetPos, .3f).SetEase(Ease.Linear).SetDelay(1.2f).OnStart(() =>
                    {
                        cardObj.transform.SetParent(KickSwear.Instance.KeelQuasar_OfPeck);
                        cardObj.transform.SetAsLastSibling();
                        cardObj.Deaf(cardData);
                    });
                    SteamshipRevolution.CarveNeon(KickSwear.Instance.CarveNeon, cardObj.transform.position, targetPos);
                }
                print($"复活成功 - 收入上部接龙区：{FarKeelPartnerStew(cardData)} 移动到上部接龙区：{upIdx}");
            }
            else
            {
                // 执行下部移动
                int targetCol = selectedMove.targetIndex;
                PinkPeck[selectedMove.sourceCol].Remove(cardData);
                PinkPeck[targetCol].Add(cardData);

                Keel cardObj = FarKeelBur(cardData);
                if (cardObj != null)
                {
                    Vector3 targetPos;
                    if (PinkPeck[targetCol].Count > 1)
                    {
                        targetPos = KickSwear.Instance.Sum_PinkPeck[targetCol].position + new Vector3(0, -(PinkPeck[targetCol].Count - 1) * Inn_Y);
                    }
                    else
                    {
                        targetPos = KickSwear.Instance.Sum_PinkPeck[targetCol].position;
                    }

                    cardObj.transform.DOMove(targetPos, .3f).SetEase(Ease.Linear).SetDelay(1.2f).OnStart(() =>
                    {
                        cardObj.transform.SetParent(KickSwear.Instance.KeelQuasar_PinkPeck);
                        cardObj.transform.SetAsLastSibling();
                        cardObj.Deaf(cardData);
                    });
                    SteamshipRevolution.CarveNeon(KickSwear.Instance.CarveNeon, cardObj.transform.position, targetPos);
                }
                print($"复活成功 - 下部接龙：{FarKeelPartnerStew(cardData)} 移动到下部接龙区：{targetCol}");
            }

            hasRevived = true;
            yield return new WaitForSeconds(1.35f);
        }

        if (!hasRevived)
        {
            print("复活失败：没有找到可移动的牌");
        }

        yield return new WaitForSeconds(1f);
        ShedKick();
        BalconyRotBrass();
        KickSwear.Instance.SendVerb();
        CigarKickRid();
    }
    #endregion

    #region 新手引导
    void BadgeGunKick_Infer() // 新手引导专用初始化方法
    {
        KickSwear.Instance.HeavyDense();
        // 重置操作次数和时间
        UpPlayedBrieflyDish = false;
        ShedLineRancher.CudGray("IsPlayedSpecialSlot", false);
        DealWaist = 0;
        AbutBadgeMold = Time.time;
        if (KickSwear.Instance != null)
        {
            KickSwear.Instance.SolderSlumWaist(DealWaist);
            KickSwear.Instance.SolderKickMold(0);
        }
        //初始化上部接龙区
        OfPeck.Clear();
        for (int i = 0; i < 4; i++)
            OfPeck.Add(new List<CardData>());
        // 初始布置
        List<CardData> CardPools_UnallocatedPool = new List<CardData>();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 13; j++)
            {
                CardData Mark = new CardData((CardSuit)i, (CardValue)j, false, false);
                CardPools_UnallocatedPool.Add(Mark);
            }
        }

        // 找出需要的固定牌
        CardData diamond10 = CardPools_UnallocatedPool.First(x => x.Leap == CardSuit.Cube && x.Value == CardValue.Ten);
        CardData heart2 = CardPools_UnallocatedPool.First(x => x.Leap == CardSuit.RedPeach && x.Value == CardValue.Two);
        CardData heartQ = CardPools_UnallocatedPool.First(x => x.Leap == CardSuit.RedPeach && x.Value == CardValue.Q);
        CardData spadeK = CardPools_UnallocatedPool.First(x => x.Leap == CardSuit.BlackPeach && x.Value == CardValue.K);
        CardData heartA = CardPools_UnallocatedPool.First(x => x.Leap == CardSuit.RedPeach && x.Value == CardValue.One);
        CardData clubJ = CardPools_UnallocatedPool.First(x => x.Leap == CardSuit.BlackFlower && x.Value == CardValue.J);

        // 从牌池中移除固定牌
        CardPools_UnallocatedPool.Remove(diamond10);
        CardPools_UnallocatedPool.Remove(heart2);
        CardPools_UnallocatedPool.Remove(heartQ);
        CardPools_UnallocatedPool.Remove(spadeK);
        CardPools_UnallocatedPool.Remove(heartA);
        CardPools_UnallocatedPool.Remove(clubJ);

        // 剩余牌随机打乱
        CardPools_UnallocatedPool = CardPools_UnallocatedPool.OrderBy(x => UnityEngine.Random.value).ToList();

        List<CardData> CardPools_Sink = new List<CardData>();
        List<CardData> CardPools_NoSink = new List<CardData>();

        // 分配沉底牌和不沉底牌（保持原有规则）
        bool SinkColorRed = UnityEngine.Random.value < .5f;
        for (int i = 1; i < 13; i++)
        {
            var matchingCards = CardPools_UnallocatedPool.Where(x => x.UpCod() == SinkColorRed && (int)x.Value == i).ToList();
            if (matchingCards.Count > 0)
            {
                CardPools_Sink.Add(matchingCards[0]);
                CardPools_UnallocatedPool.Remove(matchingCards[0]);
            }
            SinkColorRed = !SinkColorRed;
        }

        // 剩余牌中 A和K不沉底
        for (int i = 0; i < CardPools_UnallocatedPool.Count; i++)
        {
            if (CardPools_UnallocatedPool[i].Value == CardValue.One || CardPools_UnallocatedPool[i].Value == CardValue.K)
            {
                CardPools_NoSink.Add(CardPools_UnallocatedPool[i]);
                CardPools_UnallocatedPool.RemoveAt(i);
                i--;
            }
        }

        // 剩余牌中 9张牌沉底
        for (int i = 0; i < 9 && CardPools_UnallocatedPool.Count > 0; i++)
        {
            CardPools_Sink.Add(CardPools_UnallocatedPool[0]);
            CardPools_UnallocatedPool.RemoveAt(0);
        }

        // 剩余牌不沉底
        CardPools_NoSink.AddRange(CardPools_UnallocatedPool);

        // 打乱顺序
        CardPools_Sink = CardPools_Sink.OrderBy(x => UnityEngine.Random.value).ToList();
        CardPools_NoSink = CardPools_NoSink.OrderBy(x => UnityEngine.Random.value).ToList();

        // 生成下部接龙区 沉底的牌
        int usedSinkCards = 0;
        PinkPeck.Clear();
        for (int i = 0; i < 7; i++)
        {
            List<CardData> cards = new List<CardData>();
            for (int j = 0; j < i; j++)
            {
                if (usedSinkCards + j < CardPools_Sink.Count)
                    cards.Add(CardPools_Sink[usedSinkCards + j]);
            }
            PinkPeck.Add(cards);
            usedSinkCards += i;
        }

        // 删除已使用的沉底牌
        if (usedSinkCards > 0)
            CardPools_Sink.RemoveRange(0, Math.Min(usedSinkCards, CardPools_Sink.Count));

        // 生成下部接龙区 不沉底的牌（固定牌放在指定位置）
        // 第一列底部：红桃2
        PinkPeck[0].Add(heart2);
        heart2.UpHighOf = true;

        // 第二列底部：红桃Q
        PinkPeck[1].Add(heartQ);
        heartQ.UpHighOf = true;

        // 第三列底部：黑桃K
        PinkPeck[2].Add(spadeK);
        spadeK.UpHighOf = true;

        // 第四列底部：红桃A
        PinkPeck[3].Add(heartA);
        heartA.UpHighOf = true;

        // 第五列底部：梅花J
        PinkPeck[4].Add(clubJ);
        clubJ.UpHighOf = true;

        // 其他列使用随机牌
        for (int i = 5; i < 7; i++)
        {
            if (CardPools_NoSink.Count > 0)
            {
                CardPools_NoSink[0].UpHighOf = true;
                PinkPeck[i].Add(CardPools_NoSink[0]);
                CardPools_NoSink.RemoveAt(0);
            }
        }

        // 生成上部牌堆 背面朝上（最上面是方块10）
        OfPram_HighPink.Clear();
        OfPram_HighPink.Add(diamond10); // 最上面一张牌是方块10

        // 添加剩余牌
        for (int i = 0; i < 23 && CardPools_NoSink.Count > 0; i++)
        {
            OfPram_HighPink.Add(CardPools_NoSink[0]);
            CardPools_NoSink.RemoveAt(0);
        }

        // 创建牌对象
        Vector2[] Sum_PinkPeck = KickSwear.Instance.Sum_PinkPeck.Select(x => (Vector2)x.position).ToArray();
        for (int i = 0; i < PinkPeck.Count && i < Sum_PinkPeck.Length; i++)
        {
            for (int j = 0; j < PinkPeck[i].Count; j++)
            {
                Keel card = DugoutKeel(PinkPeck[i][j]);
                card.transform.DOMove(Sum_PinkPeck[i] + new Vector2(0, -j * Inn_Y), 0.5f).SetDelay(j * 0.05f).OnStart(() =>
                {
                    LeafyLeg.FarBefriend().HaulGuinea(LeafyJoke.UIMusic.SFX_CardSet);
                });
            }
        }

        Vector2 Sum_OfPram_HighPink = KickSwear.Instance.Sum_OfPram_HighPink.position;
        for (int i = 0; i < OfPram_HighPink.Count; i++)
        {
            Keel card = DugoutKeel(OfPram_HighPink[i]);
            card.transform.DOMove(Sum_OfPram_HighPink, 0.5f).SetDelay(i * 0.03f).OnStart(() =>
            {
                LeafyLeg.FarBefriend().HaulGuinea(LeafyJoke.UIMusic.SFX_CardSet);
            });
        }
        // 不需要设置奖励牌
        OfPram_StarveKeelProse.Clear();
    }
    #endregion
}

public enum CardSuit { BlackPeach, RedPeach, BlackFlower, Cube } // 黑桃，红桃，梅花，方片
public enum CardValue { One, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, J, Q, K } // 1-13

/// <summary> 纸牌数据 </summary>
[System.Serializable]
public class CardData
{
    public CardSuit Leap; // 花色
    public CardValue Value; // 牌值
    public bool UpHighOf; // 是否正面朝上
    public bool UpOfPeckComputer; // 是否发放过上部接龙区奖励

    public CardData(CardSuit suit, CardValue value, bool isFaceUp, bool isUpAreaRewarded)
    {
        Leap = suit;
        Value = value;
        UpHighOf = isFaceUp;
        UpOfPeckComputer = isUpAreaRewarded;
    }

    public bool UpCod() // 是否是红桃或方片
    {
        return Leap == CardSuit.RedPeach || Leap == CardSuit.Cube;
    }
}

// ========== 撤销功能 ========== 
[System.Serializable]
public class KlondikeGameState
{
    public List<CardData> OfPram_HighOf;
    public List<CardData> OfPram_HighPink;
    public List<List<CardData>> OfPeck;
    public List<List<CardData>> PinkPeck;
    // 新增：保存所有牌的UI状态
    public List<CardUIState> KeelUIJobber;
    // 新增：保存操作次数和游戏时间
    public int SlumWaist;
    public float KickMold;
}
// 牌的UI状态
[System.Serializable]
public class CardUIState
{
    public CardSuit Leap;
    public CardValue Value;
    public bool UpHighOf;
    public bool UpOfPeckComputer; // 是否发放过上部接龙区奖励
    public string QuasarStew; // 父物体名
    public Vector3 Citation;
    public CardUIState(Keel card)
    {
        Leap = card.Line.Leap;
        Value = card.Line.Value;
        UpHighOf = card.Line.UpHighOf;
        UpOfPeckComputer = card.Line.UpOfPeckComputer;
        QuasarStew = card.transform.parent != null ? card.transform.parent.name : "";
        Citation = card.transform.position;
    }
    public bool UpAideKeel(Keel card)
    {
        return card.Line.Leap == Leap && card.Line.Value == Value;
    }
}
