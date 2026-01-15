using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using System;

/// <summary> 克朗代克纸牌玩法管理 </summary>
public class Klondike_Manager : MonoBehaviour
{
    public static Klondike_Manager Instance;

    public Card CardPrefab;
    public Sprite[] SuitSprites; //7个花色 黑桃 红桃 梅花 方片 J Q K 
    [HideInInspector] public string[] ValueTexts = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
    public float Gap_Y = .4f; //接龙时上下两张牌的间隔
    [HideInInspector] public List<CardData> UpDeck_FaceUp = new List<CardData>(); // 上部牌堆 正面朝上
    [HideInInspector] public List<CardData> UpDeck_FaceDown = new List<CardData>(); // 上部牌堆 背面朝上
    [HideInInspector] public List<List<CardData>> UpArea = new List<List<CardData>>(); // 上部接龙区 4组
    [HideInInspector] public List<List<CardData>> DownArea = new List<List<CardData>>(); // 下部接龙区 7组
    float DragDistanceCheck = .5f; //判断两张牌之间的距离 用以在拖动操作时检测是否可以叠放
    Dictionary<CardData, Card> cardDataToCardMap = new Dictionary<CardData, Card>(); // 性能优化：CardData到Card的映射
    private const string SaveKey = "klondike_save"; // 存档key
    private Stack<KlondikeGameState> undoStack = new Stack<KlondikeGameState>(); // 撤销栈
    [Range(0f, 1f)] public float SmartFlipProbability = 1f; // 智能换牌概率（0~1），如0.8表示80%概率执行智能换牌
    [HideInInspector] public bool IsWinCollecting = false; // 是否在赢牌收集阶段
    private float timeCounter = 0f; // 时间计数器
    private const int TimePenaltyInterval = 15; // 时间惩罚间隔
    private const int TimePenaltyScore = -2; // 时间惩罚分数
    private int lastScorePenaltyTime = 0; // 上次时间惩罚时间
    private int moveCount = 0; // 操作次数统计
    private float gameStartTime = 0f; // 游戏开始时间
    List<string> UpDeck_RewardCardNames = new List<string>(); //上部牌堆奖励牌
    bool IsTriggerSpecial_ScratchCard = false; //是否触发小游戏 刮刮卡
    bool IsTriggerSpecial_LuckyWheel = false; //是否触发小游戏 幸运轮盘
    bool IsTriggerSpecial_Money = false; //是否触发小游戏 现金奖励
    bool IsTriggerSpecial_Slot = false; //是否触发小游戏 老虎机
    Coroutine WaitLittleGameIE;
    [HideInInspector] public int LuckyWheelCount;
    bool IsPlayedSpecialSlot = false; //是否已经玩过老虎机 每局游戏只出一次
    Vector3 AutoMoveCardAddScoreEffectPos;


    void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;
    }

    public void Init()
    {
        LuckyWheelCount = SaveDataManager.GetInt("LuckyWheelCount");
        GamePanel.Instance.SetLuckyWheelFill();
        IsPlayedSpecialSlot = SaveDataManager.GetBool("IsPlayedSpecialSlot");

        // bool IsGuide = SaveDataManager.GetBool("IsGuide");
        // if (!IsGuide)
        // {
        //     StartNewGame_Guide();
        //     return;
        // }

        if (!LoadGame()) //加载存档
            StartNewGame(); // 没有存档则新开一局
    }

    void Update()
    {
        // 更新游戏时间显示
        if (GamePanel.Instance != null)
        {
            GamePanel.Instance.UpdateGameTime(GetGameTime());
        }

        // 每15秒减2分
        if (GamePanel.Instance != null)
        {
            timeCounter += Time.deltaTime;
            int penaltyTimes = (int)(timeCounter / TimePenaltyInterval);
            if (penaltyTimes > lastScorePenaltyTime)
            {
                GamePanel.Instance.AddScore(TimePenaltyScore);
                lastScorePenaltyTime = penaltyTimes;
            }
        }

        //测试
        if (Input.GetKeyDown(KeyCode.R))
        {
            GamePanel.Instance.Finish(true);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UIManager.GetInstance().ShowUIForms(nameof(LuckyWheelPanel));
            //GamePanel.Instance.PlayLittleSlot();
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            AnimationController.MagicWand(GamePanel.Instance.MagicWand, new Vector3(0, 0, 0), new Vector3(1, 1, 1));
        }
    }

    // 增加操作次数
    private void AddMoveCount()
    {
        moveCount++;
        if (GamePanel.Instance != null)
            GamePanel.Instance.UpdateMoveCount(moveCount);
    }

    // 获取当前游戏时间（秒）
    public float GetGameTime()
    {
        return Time.time - gameStartTime;
    }

    public Sprite GetSuitSprite(CardData card, bool IsLittle = false) //获取花色图片
    {
        if (IsLittle)
            return SuitSprites[(int)card.Suit];
        else if (card.Value == CardValue.J)
            return SuitSprites[4];
        else if (card.Value == CardValue.Q)
            return SuitSprites[5];
        else if (card.Value == CardValue.K)
            return SuitSprites[6];
        else
            return SuitSprites[(int)card.Suit];
    }

    public string GetCardDisplayName(CardData card) //获取牌的显示名称
    {
        string suitName = "";
        switch (card.Suit)
        {
            case CardSuit.BlackPeach: suitName = "♠"; break;
            case CardSuit.RedPeach: suitName = "♥"; break;
            case CardSuit.BlackFlower: suitName = "♣"; break;
            case CardSuit.Cube: suitName = "♦"; break;
        }
        return suitName + ValueTexts[(int)card.Value];
    }

    public string GetDownAreaFaceDownCardNum_ForEvent() //获取下部接龙区背面朝上的牌数 打点用
    {
        int Num = 0;
        for (int i = 0; i < DownArea.Count; i++)
        {
            for (int j = 0; j < DownArea[i].Count; j++)
            {
                if (!DownArea[i][j].IsFaceUp)
                    Num++;
            }
        }
        return Num.ToString();
    }

    public void GetUpAreaReward(int index) //获取上部接龙区奖励
    {
        RewardData Data = GameDataManager.GetInstance().GetRewardDataByWeightAndRange(NetInfoMgr.instance._GameData.uparea_collectcard_data_list);
        if (Data.type == RewardType.Gold)
        {
            GameDataManager.GetInstance().AddGold(Data.num);
            GamePanel.Instance.UpdateGold();
            PostEventScript.GetInstance().SendEvent("1008", "1", Data.num.ToString());
        }
        else if (Data.type == RewardType.Cash)
        {
            GameDataManager.GetInstance().AddCash(Data.num);
            GamePanel.Instance.UpdateCash();

            PostEventScript.GetInstance().SendEvent("1008", "2", Data.num.ToString());
        }
        GamePanel.Instance.ShowUpAreaRewardEffect(index, Data.type);
    }

    public void GetUpDeckReward() //获取上部牌堆奖励
    {
        RewardData Data = GameDataManager.GetInstance().GetRewardDataByWeightAndRange(NetInfoMgr.instance._GameData.uparea_filp_data_list);
        if (Data.type == RewardType.Gold)
        {
            GameDataManager.GetInstance().AddGold(Data.num);
            GamePanel.Instance.UpdateGold();
            PostEventScript.GetInstance().SendEvent("1010", "1", Data.num.ToString());
        }
        else if (Data.type == RewardType.Cash)
        {
            GameDataManager.GetInstance().AddCash(Data.num);
            GamePanel.Instance.UpdateCash();
            PostEventScript.GetInstance().SendEvent("1010", "2", Data.num.ToString());
        }
        GamePanel.Instance.ShowUpDeckRewardEffect(Data.type);
    }
    #region  初始化游戏
    void StartNewGame() // 初始化游戏 根据规则控制难度
    {
        GamePanel.Instance.ClearScore();
        // 重置操作次数和时间
        IsPlayedSpecialSlot = false;
        SaveDataManager.SetBool("IsPlayedSpecialSlot", false);
        moveCount = 0;
        gameStartTime = Time.time;
        if (GamePanel.Instance != null)
        {
            GamePanel.Instance.UpdateMoveCount(moveCount);
            GamePanel.Instance.UpdateGameTime(0);
        }
        //初始化上部接龙区
        UpArea.Clear();
        for (int i = 0; i < 4; i++)
            UpArea.Add(new List<CardData>());
        // 初始布置
        // a、随机下部接龙区域 7列每列分别有1~7张牌
        // b、剩余牌收起，放置上部翻牌区
        // c、3张A和K从来不沉底（沉底的牌是一组2~13(红黑交叉的13张)+2~13中可重复的9张）
        List<CardData> CardPools_UnallocatedPool = new List<CardData>(); //未分配的牌52张 等所有牌生成结束后再分入沉底或不沉底牌池
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 13; j++)
            {
                CardData data = new CardData((CardSuit)i, (CardValue)j, false, false);
                CardPools_UnallocatedPool.Add(data);
            }
        }
        CardPools_UnallocatedPool = CardPools_UnallocatedPool.OrderBy(x => UnityEngine.Random.value).ToList();// 未分配的牌随机打乱顺序
        List<CardData> CardPools_Sink = new List<CardData>(); // 沉底牌池 在下部接龙区 被盖住的牌
        List<CardData> CardPools_NoSink = new List<CardData>(); // 不沉底的牌 在上部牌堆 或者 下部接龙区最底下被翻开的牌
        //随机选一个初始颜色 这组牌沉底  （此时沉底共12张 不沉底0张）
        bool SinkColorRed = UnityEngine.Random.value < .5f;
        for (int i = 1; i < 13; i++)
        {
            CardData SinkData = CardPools_UnallocatedPool.Select(x => x).Where(x => x.IsRed() == SinkColorRed && (int)x.Value == i).First();
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
            DownArea.Add(cards);
            usedSinkCards += i;
        }
        // 删除已使用的沉底牌
        CardPools_Sink.RemoveRange(0, usedSinkCards);
        // 生成下部接龙区 不沉底的牌
        for (int i = 0; i < 7; i++)
        {
            CardPools_NoSink[0].IsFaceUp = true;
            DownArea[i].Add(CardPools_NoSink[0]);
            CardPools_NoSink.RemoveAt(0);
        }
        Vector2[] Pos_DownArea = GamePanel.Instance.Pos_DownArea.Select(x => (Vector2)x.position).ToArray();
        for (int i = 0; i < DownArea.Count && i < Pos_DownArea.Length; i++)
        {
            for (int j = 0; j < DownArea[i].Count; j++)
            {
                Card card = CreateCard(DownArea[i][j]);
                card.transform.DOMove(Pos_DownArea[i] + new Vector2(0, -j * Gap_Y), 0.5f).SetDelay(j * 0.05f).OnStart(() =>
                {
                    if (i % 2 == 0)
                        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.SFX_CardSet);
                });
            }
        }

        // 生成上部牌堆 背面朝上
        for (int i = 0; i < 24; i++)
        {
            UpDeck_FaceDown.Add(CardPools_NoSink[0]);
            CardPools_NoSink.RemoveAt(0);
        }
        Vector2 Pos_UpDeck_FaceDown = GamePanel.Instance.Pos_UpDeck_FaceDown.position;
        for (int i = 0; i < UpDeck_FaceDown.Count; i++)
        {
            Card card = CreateCard(UpDeck_FaceDown[i]);
            card.transform.DOMove(Pos_UpDeck_FaceDown, 0.5f).SetDelay(i * 0.03f).OnStart(() =>
                {
                    if (i % 2 == 0)
                        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.SFX_CardSet);
                });
        }

        //上部牌堆区 随机些牌记录为奖励牌
        int UpDeck_RewardCard_Count = 0;
        for (int i = 0; i < UpDeck_FaceDown.Count; i++)
        {
            //根据概率确定奖励牌数量
            if (UnityEngine.Random.value < NetInfoMgr.instance._GameData.uparea_filp_reward_rate)
                UpDeck_RewardCard_Count++;
        }
        List<string> TempCardName = new List<string>();
        for (int i = 0; i < UpDeck_FaceDown.Count; i++)
            TempCardName.Add(GetCardDisplayName(UpDeck_FaceDown[i]));
        TempCardName = TempCardName.OrderBy(x => UnityEngine.Random.value).ToList();
        for (int i = 0; i < UpDeck_RewardCard_Count; i++)
            UpDeck_RewardCardNames.Add(TempCardName[i]);
        string UpDeck_RewardCard_Save = string.Join(",", UpDeck_RewardCardNames);
        SaveDataManager.SetString("UpDeck_RewardCardNames", UpDeck_RewardCard_Save);
        TimeManager.GetInstance().Delay(1, () => { GamePanel.Instance.HideMask(); });

        //第二局游戏开始时 弹好评
        if (SaveDataManager.GetBool("WaitRateUs"))
        {
            UIManager.GetInstance().ShowUIForms(nameof(RateUsPanel));
            SaveDataManager.SetBool("WaitRateUs", false);
        }
    }

    //生成牌
    Card CreateCard(CardData data)
    {
        Card card = Instantiate(CardPrefab);
        // 判断初始归属区域
        if (DownArea.Any(area => area.Contains(data)))
            card.transform.SetParent(GamePanel.Instance.CardParent_DownArea);
        else if (UpDeck_FaceDown.Contains(data) || UpDeck_FaceUp.Contains(data))
            card.transform.SetParent(GamePanel.Instance.CardParent_UpDeck);
        else if (UpArea.Any(area => area.Contains(data)))
            card.transform.SetParent(GamePanel.Instance.CardParent_UpArea);
        else
            card.transform.SetParent(GamePanel.Instance.CardParent_UpDeck);
        card.transform.localScale = Vector3.one;
        card.transform.position = GamePanel.Instance.Pos_CardBorn.position;
        card.Init(data);

        // 建立映射关系
        cardDataToCardMap[data] = card;
        return card;
    }
    #endregion

    #region  翻牌接龙等逻辑
    //上部牌堆翻牌
    public void UpDeck_Filp()
    {
        SaveStateForUndo(); // 撤销快照
        AddMoveCount(); // 增加操作次数
        // 隐藏提示
        GamePanel.Instance.HideAllTips();
        if (UpDeck_FaceDown.Count > 0)
        {
            var cardData = UpDeck_FaceDown.Last();
            UpDeck_FaceDown.RemoveAt(UpDeck_FaceDown.Count - 1);
            cardData.IsFaceUp = true;
            UpDeck_FaceUp.Add(cardData);
            // UI
            var cardObj = GetCardObj(cardData);
            if (cardObj != null)
            {
                cardObj.transform.SetParent(GamePanel.Instance.CardParent_UpDeck);
                cardObj.transform.SetAsLastSibling();
                // 添加翻牌动画
                StartCoroutine(FlipDeckCardAnimation(cardObj, cardData));

                if (UpDeck_RewardCardNames.Contains(cardObj.transform.name))
                {
                    GetUpDeckReward();
                    UpDeck_RewardCardNames.Remove(cardObj.transform.name);
                    string UpDeck_RewardCard_Save = string.Join(",", UpDeck_RewardCardNames);
                    SaveDataManager.SetString("UpDeck_RewardCardNames", UpDeck_RewardCard_Save);
                }
            }
        }
        else if (UpDeck_FaceDown.Count == 0 && UpDeck_FaceUp.Count > 0)
        {
            // 全部回收
            StartCoroutine(RecycleAllFaceUpCards());
        }
        SaveGame();
    }

    // 翻牌动画
    private IEnumerator FlipDeckCardAnimation(Card cardObj, CardData cardData)
    {
        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.SFX_CardSwitch);
        GamePanel.Instance.ShowMask();
        // 第一阶段：旋转到90度
        cardObj.transform.DOMove(GamePanel.Instance.Pos_UpDeck_FaceUp.position, 0.2f).SetEase(Ease.OutQuad);
        cardObj.transform.DORotate(new Vector3(0, 90, 0), 0.15f).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(0.15f);
        // 在90度时切换牌面显示
        cardObj.Init(cardData);
        // 第二阶段：旋转回0度
        cardObj.transform.DORotate(Vector3.zero, 0.15f).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(0.15f);
        GamePanel.Instance.HideMask();
        CheckGameEnd();
    }

    // 回收所有正面朝上的牌
    private IEnumerator RecycleAllFaceUpCards()
    {
        GamePanel.Instance.ShowMask();
        for (int i = UpDeck_FaceUp.Count - 1; i >= 0; i--)
        {
            var cardData = UpDeck_FaceUp[i];
            cardData.IsFaceUp = false;
            UpDeck_FaceDown.Add(cardData);

            // UI
            var cardObj = GetCardObj(cardData);
            if (cardObj != null)
            {
                cardObj.transform.SetParent(GamePanel.Instance.CardParent_UpDeck);
                cardObj.transform.SetAsLastSibling();

                // 添加回收动画
                cardObj.transform.DOMove(GamePanel.Instance.Pos_UpDeck_FaceDown.position, 0.2f).SetEase(Ease.InQuad);
                cardObj.transform.DORotate(new Vector3(0, 180, 0), 0.2f).SetEase(Ease.InOutQuad)
                    .OnComplete(() =>
                    {
                        cardObj.Init(cardData);
                        cardObj.transform.DORotate(Vector3.zero, 0.1f).SetEase(Ease.OutQuad);
                    });
                MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.SFX_CardSwitch);
                yield return new WaitForSeconds(0.05f); // 添加延迟，让回收过程更流畅
            }
        }
        UpDeck_FaceUp.Clear();
        GamePanel.Instance.HideMask();
        SaveGame();
        CheckGameEnd();
    }

    // 获取拖动的牌（包括下方所有牌）
    public List<Card> GetDraggingCards(Card topCard)
    {
        // 下部接龙区连续正面朝上的牌
        for (int i = 0; i < DownArea.Count; i++)
        {
            var area = DownArea[i];
            for (int j = 0; j < area.Count; j++)
            {
                if (GetCardObj(area[j]) == topCard)
                {
                    List<Card> result = new List<Card>();
                    // 从当前牌开始，收集所有连续正面朝上的牌
                    for (int k = j; k < area.Count; k++)
                    {
                        if (area[k].IsFaceUp)
                        {
                            Card c = GetCardObj(area[k]);
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
        for (int i = 0; i < UpArea.Count; i++)
        {
            var area = UpArea[i];
            if (area.Count > 0 && GetCardObj(area.Last()) == topCard)
                return new List<Card> { topCard };
        }
        // 支持上部牌堆正面区 只允许单张
        if (UpDeck_FaceUp.Count > 0 && GetCardObj(UpDeck_FaceUp.Last()) == topCard)
            return new List<Card> { topCard };
        return new List<Card> { topCard };
    }

    // 判断能否叠放到目标牌
    public bool CanStackOn(Card movingCard, Card targetCard)
    {
        // 下部区域：红黑交错，递减
        for (int i = 0; i < DownArea.Count; i++)
        {
            var area = DownArea[i];
            if (area.Count > 0 && GetCardObj(area.Last()) == targetCard)
            {
                bool colorDiff = movingCard.Data.IsRed() != targetCard.Data.IsRed();
                bool valueDiff = (int)movingCard.Data.Value == (int)targetCard.Data.Value - 1;
                //print("颜色是否不同： " + colorDiff);
                //print("牌值是否递减： " + valueDiff + " 拖动牌：" + movingCard.Data.Value + " 目标牌：" + targetCard.Data.Value);
                return colorDiff && valueDiff;
            }
        }
        // 上部接龙区：同花色，递增
        for (int i = 0; i < UpArea.Count; i++)
        {
            var area = UpArea[i];
            if (area.Count > 0 && GetCardObj(area.Last()) == targetCard)
            {
                bool sameSuit = movingCard.Data.Suit == targetCard.Data.Suit;
                bool valueInc = (int)movingCard.Data.Value == (int)targetCard.Data.Value + 1;
                //print("颜色是否相同： " + sameSuit);
                //print("牌值是否递增： " + valueInc + " 拖动牌：" + movingCard.Data.Value + " 目标牌：" + targetCard.Data.Value);
                return sameSuit && valueInc;
            }
        }
        return false;
    }

    // 移动一叠牌到目标牌下方
    public bool MoveCardStack(Card movingCard, Card targetCard)
    {
        SaveStateForUndo(); // 撤销快照
        AddMoveCount(); // 增加操作次数
        bool result = false;
        bool fromUpDeckFaceUp = false;
        bool fromUpArea = false;
        if (DownArea.Any(area => area.Contains(targetCard.Data)))
        {
            var source = FindCardSource(movingCard);
            if (source.fromCol == -1) return false;
            List<CardData> movingList;
            if (source.fromUpDeckFaceUp)
            {
                movingList = new List<CardData> { UpDeck_FaceUp[source.fromIdx] };
                UpDeck_FaceUp.RemoveAt(source.fromIdx);
                fromUpDeckFaceUp = true;
            }
            else if (source.fromUpArea)
            {
                movingList = new List<CardData> { UpArea[source.fromUpAreaIdx].Last() };
                UpArea[source.fromUpAreaIdx].RemoveAt(UpArea[source.fromUpAreaIdx].Count - 1);
                fromUpArea = true;
            }
            else
            {
                movingList = DownArea[source.fromCol].GetRange(source.fromIdx, DownArea[source.fromCol].Count - source.fromIdx);
                DownArea[source.fromCol].RemoveRange(source.fromIdx, DownArea[source.fromCol].Count - source.fromIdx);
            }
            DownArea[DownArea.FindIndex(area => area.Contains(targetCard.Data))].AddRange(movingList);
            // UI移动
            Vector3 AddScoreEffectPos = Vector3.zero;
            for (int k = 0; k < movingList.Count; k++)
            {
                Card cardObj = GetCardObj(movingList[k]);
                cardObj.transform.SetParent(GamePanel.Instance.CardParent_DownArea);
                cardObj.transform.SetAsLastSibling();
                Vector3 targetPos = GamePanel.Instance.Pos_DownArea[DownArea.FindIndex(area => area.Contains(targetCard.Data))].position + new Vector3(0, -(DownArea[DownArea.FindIndex(area => area.Contains(targetCard.Data))].Count - movingList.Count + k) * Gap_Y);
                cardObj.transform.DOMove(targetPos, 0.3f).SetEase(Ease.OutQuad);
                MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.SFX_CardLink);
                if (k == 0)
                    AddScoreEffectPos = targetPos;
            }
            if (!source.fromUpDeckFaceUp && !source.fromUpArea)
                CheckFlipAfterMove(source.fromCol);
            CheckGameEnd();
            result = true;
            if (fromUpDeckFaceUp)
                GamePanel.Instance.AddScore(5, AddScoreEffectPos);
            if (fromUpArea)
                GamePanel.Instance.AddScore(-10, AddScoreEffectPos);
        }
        SaveGame();
        return result;
    }

    // 获取空下部区域索引（通过距离检测）
    public int GetEmptyDownAreaIndexUnderMouse(List<Card> draggingCards)
    {
        for (int i = 0; i < GamePanel.Instance.Pos_DownArea.Count; i++)
        {
            if (DownArea[i].Count == 0)
            {
                Vector3 targetPos = GamePanel.Instance.Pos_DownArea[i].position;
                // 检查拖动牌列表中是否有牌距离目标位置足够近
                foreach (var card in draggingCards)
                {
                    float distance = Vector3.Distance(card.transform.position, targetPos);
                    if (distance <= DragDistanceCheck)
                        return i;
                }
            }
        }
        return -1;
    }

    // 能否移动到空下部区域（只能K）
    public bool CanMoveToEmptyDownArea(Card movingCard)
    {
        return movingCard.Data.Value == CardValue.K;
    }
    // 移动到空下部区域
    public bool MoveCardStackToEmptyDownArea(Card movingCard, int areaIdx)
    {
        SaveStateForUndo(); // 撤销快照
        AddMoveCount(); // 增加操作次数
        bool result = false;
        var source = FindCardSource(movingCard);
        if (source.fromCol == -1) return false;
        List<CardData> movingList;
        bool fromUpArea = false;
        if (source.fromUpDeckFaceUp)
        {
            movingList = new List<CardData> { UpDeck_FaceUp[source.fromIdx] };
            UpDeck_FaceUp.RemoveAt(source.fromIdx);
        }
        else if (source.fromUpArea)
        {
            movingList = new List<CardData> { UpArea[source.fromUpAreaIdx].Last() };
            UpArea[source.fromUpAreaIdx].RemoveAt(UpArea[source.fromUpAreaIdx].Count - 1);
            fromUpArea = true;
        }
        else
        {
            movingList = DownArea[source.fromCol].GetRange(source.fromIdx, DownArea[source.fromCol].Count - source.fromIdx);
            DownArea[source.fromCol].RemoveRange(source.fromIdx, DownArea[source.fromCol].Count - source.fromIdx);
        }
        DownArea[areaIdx].AddRange(movingList);
        Vector3 AddScoreEffectPos = Vector3.zero;
        for (int k = 0; k < movingList.Count; k++)
        {
            Card cardObj = GetCardObj(movingList[k]);
            cardObj.transform.SetParent(GamePanel.Instance.CardParent_DownArea);
            cardObj.transform.SetAsLastSibling();
            Vector3 targetPos = GamePanel.Instance.Pos_DownArea[areaIdx].position + new Vector3(0, -(DownArea[areaIdx].Count - movingList.Count + k) * Gap_Y);
            cardObj.transform.DOMove(targetPos, 0.3f).SetEase(Ease.OutQuad);
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.SFX_CardLink);
            if (k == 0)
                AddScoreEffectPos = targetPos;
        }
        if (!source.fromUpDeckFaceUp && !source.fromUpArea)
            CheckFlipAfterMove(source.fromCol);
        CheckGameEnd();
        result = true;
        SaveGame();
        if (fromUpArea)
            GamePanel.Instance.AddScore(-10, AddScoreEffectPos);
        return result;
    }

    // 获取上部接龙区索引（通过距离检测）
    public int GetUpAreaIndexUnderMouse(List<Card> draggingCards)
    {
        for (int i = 0; i < GamePanel.Instance.Pos_UpArea.Count; i++)
        {
            Vector3 targetPos = GamePanel.Instance.Pos_UpArea[i].position;
            // 检查拖动牌列表中是否有牌距离目标位置足够近
            foreach (var card in draggingCards)
            {
                float distance = Vector3.Distance(card.transform.position, targetPos);
                if (distance <= DragDistanceCheck)
                    return i;
            }
        }
        return -1;
    }
    // 能否移动到上部接龙区
    public bool CanMoveToUpArea(Card movingCard, int upAreaIdx)
    {
        // 上部接龙区只能放置单张牌
        var draggingCards = GetDraggingCards(movingCard);
        if (draggingCards.Count > 1)
            return false;

        var area = UpArea[upAreaIdx];
        if (area.Count == 0)
            return movingCard.Data.Value == CardValue.One; // 只能A
        var last = area.Last();
        return movingCard.Data.Suit == last.Suit && (int)movingCard.Data.Value == (int)last.Value + 1;
    }
    // 复活专用：能否移动到上部接龙区（不检查拖动牌数量）
    public bool CanMoveToUpAreaForRevive(Card movingCard, int upAreaIdx)
    {
        var area = UpArea[upAreaIdx];
        if (area.Count == 0)
            return movingCard.Data.Value == CardValue.One; // 只能A
        var last = area.Last();
        return movingCard.Data.Suit == last.Suit && (int)movingCard.Data.Value == (int)last.Value + 1;
    }
    // 移动到上部接龙区
    public bool MoveCardToUpArea(Card movingCard, int upAreaIdx)
    {
        SaveStateForUndo(); // 撤销快照
        AddMoveCount(); // 增加操作次数
        bool result = false;
        var source = FindCardSource(movingCard);
        if (source.fromCol == -1) return false;
        CardData cardData;
        int addScore = 0;
        if (source.fromUpDeckFaceUp)
        {
            cardData = UpDeck_FaceUp[source.fromIdx];
            UpDeck_FaceUp.RemoveAt(source.fromIdx);
            addScore = 15;
        }
        else if (source.fromUpArea)
        {
            cardData = UpArea[source.fromUpAreaIdx].Last();
            UpArea[source.fromUpAreaIdx].RemoveAt(UpArea[source.fromUpAreaIdx].Count - 1);
            // 拖回下部接龙区时减分
            addScore = 0;
        }
        else
        {
            cardData = DownArea[source.fromCol][source.fromIdx];
            DownArea[source.fromCol].RemoveAt(source.fromIdx);
            addScore = 10;
        }
        UpArea[upAreaIdx].Add(cardData);
        var cardObj2 = GetCardObj(cardData);
        cardObj2.transform.SetParent(GamePanel.Instance.CardParent_UpArea);
        cardObj2.transform.SetAsLastSibling();
        Vector3 targetPos = GamePanel.Instance.Pos_UpArea[upAreaIdx].position;
        cardObj2.transform.DOMove(targetPos, 0.3f).SetEase(Ease.OutQuad);
        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.SFX_CardCollect);

        // 检查是否是第一次收入上部接龙区  检测是否触发slot
        CheckUpAreaRewardedAndCheckSlot(cardData, upAreaIdx);

        if (!source.fromUpDeckFaceUp && !source.fromUpArea)
            CheckFlipAfterMove(source.fromCol);
        CheckGameEnd();
        // 触发自动收牌功能
        if (AutoCollectSwitch)
            StartCoroutine(AutoCollectCardsCoroutine());
        result = true;
        SaveGame();
        if (addScore > 0)
            GamePanel.Instance.AddScore(addScore, targetPos);
        return result;
    }

    // 移动后自动翻牌
    private void CheckFlipAfterMove(int fromCol)
    {
        var area = DownArea[fromCol];
        if (area.Count > 0 && !area.Last().IsFaceUp)
        {
            // 智能换牌逻辑：优先把缺的牌翻出来
            TrySmartFlipCard(fromCol);
            area.Last().IsFaceUp = true;
            var cardObj = GetCardObj(area.Last());
            if (cardObj != null)
            {
                // 添加翻牌旋转动画
                StartCoroutine(FlipCardAnimation(cardObj, area.Last()));

                //翻牌后判断是否触发小游戏
                string SpecialType = GameDataManager.GetInstance().GetSpecialGameTypeByWeight(NetInfoMgr.instance._GameData.downarea_specialgame_weight_group);
                if (SpecialType != "Null")
                {
                    TimeManager.GetInstance().Delay(0.15f, () =>
                    {
                        cardObj.ShowSpecial(SpecialType);
                        if (SpecialType == "ScratchCard" && !IsTriggerSpecial_ScratchCard)
                        {
                            IsTriggerSpecial_ScratchCard = true;
                            PostEventScript.GetInstance().SendEvent("1009", "1");
                        }
                        else if (SpecialType == "LuckyWheel")
                        {
                            LuckyWheelCount++;
                            SaveDataManager.SetInt("LuckyWheelCount", LuckyWheelCount);
                            if (LuckyWheelCount >= 4 && !IsTriggerSpecial_LuckyWheel)
                                IsTriggerSpecial_LuckyWheel = true;
                            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.SFX_LuckyWheelCollection);
                            PostEventScript.GetInstance().SendEvent("1009", "2");
                        }
                        else if (SpecialType == "Money" && !IsTriggerSpecial_Money)
                        {
                            IsTriggerSpecial_Money = true;
                            PostEventScript.GetInstance().SendEvent("1009", "3");
                        }
                    });
                }
            }
        }
    }

    // 翻牌旋转动画
    private IEnumerator FlipCardAnimation(Card cardObj, CardData cardData)
    {
        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.SFX_CardOpen);
        GamePanel.Instance.ShowMask();
        // 第一阶段：旋转到90度
        cardObj.transform.DORotate(new Vector3(0, 90, 0), 0.15f).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(0.15f);
        // 在90度时切换牌面显示
        cardObj.Init(cardData);
        // 第二阶段：旋转回0度
        cardObj.transform.DORotate(Vector3.zero, 0.15f).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(0.15f);
        GamePanel.Instance.HideMask();
        CheckGameEnd();
    }

    //触发小游戏
    public void TriggerSpecialGame()
    {
        if (WaitLittleGameIE != null)
            TimeManager.GetInstance().StopDelay(WaitLittleGameIE);

        if (GamePanel.Instance.FinishPanel.activeSelf
        || GamePanel.Instance._LittleSlot.gameObject.activeSelf
        || (UIManager.GetInstance().GetPanelByName(nameof(ScratchPanel)) != null && UIManager.GetInstance().GetPanelByName(nameof(ScratchPanel)).gameObject.activeSelf)
        || (UIManager.GetInstance().GetPanelByName(nameof(LuckyWheelPanel)) != null && UIManager.GetInstance().GetPanelByName(nameof(LuckyWheelPanel)).gameObject.activeSelf)
        || (UIManager.GetInstance().GetPanelByName(nameof(RewardPanel)) != null && UIManager.GetInstance().GetPanelByName(nameof(RewardPanel)).gameObject.activeSelf))
            return;

        if (IsTriggerSpecial_Slot)
        {
            GamePanel.Instance.ShowMask();
            IsPlayedSpecialSlot = true;
            SaveDataManager.SetBool("IsPlayedSpecialSlot", true);
            WaitLittleGameIE = TimeManager.GetInstance().Delay(1, () =>
            {
                IsTriggerSpecial_Slot = false;
                GamePanel.Instance.HideMask();
                GamePanel.Instance.PlayLittleSlot();
                WaitLittleGameIE = null;
            });
        }
        else if (IsTriggerSpecial_ScratchCard)
        {
            GamePanel.Instance.ShowMask();
            WaitLittleGameIE = TimeManager.GetInstance().Delay(1, () =>
            {
                IsTriggerSpecial_ScratchCard = false;
                GamePanel.Instance.HideMask();
                UIManager.GetInstance().ShowUIForms(nameof(ScratchPanel));
                WaitLittleGameIE = null;
            });
        }
        else if (IsTriggerSpecial_LuckyWheel)
        {
            GamePanel.Instance.ShowMask();
            WaitLittleGameIE = TimeManager.GetInstance().Delay(1.5f, () =>
            {
                LuckyWheelCount = 0;
                SaveDataManager.SetInt("LuckyWheelCount", LuckyWheelCount);
                GamePanel.Instance.SetLuckyWheelFill();
                IsTriggerSpecial_LuckyWheel = false;
                GamePanel.Instance.HideMask();
                UIManager.GetInstance().ShowUIForms(nameof(LuckyWheelPanel));
                WaitLittleGameIE = null;
            });
        }
        else if (IsTriggerSpecial_Money)
        {
            GamePanel.Instance.ShowMask();
            WaitLittleGameIE = TimeManager.GetInstance().Delay(1, () =>
            {
                IsTriggerSpecial_Money = false;
                GamePanel.Instance.HideMask();
                RewardData Date = GameDataManager.GetInstance().GetRewardDataByWeightAndRange(NetInfoMgr.instance._GameData.downarea_filp_money_data_list);
                if (Date.type == RewardType.Gold)
                    UIManager.GetInstance().ShowUIForms(nameof(RewardPanel)).GetComponent<RewardPanel>().Init(Date, null, null, "1011");
                else if (Date.type == RewardType.Cash)
                    UIManager.GetInstance().ShowUIForms(nameof(RewardPanel)).GetComponent<RewardPanel>().Init(null, Date, null, "1011");
                WaitLittleGameIE = null;
            });
        }
        else
        {
            CheckGameEnd();
        }
    }

    // 获取CardData对应的Card对象
    private Card GetCardObj(CardData data)
    {
        cardDataToCardMap.TryGetValue(data, out Card card);
        return card;
    }
    // 提取公共的查找逻辑
    private (int fromCol, int fromIdx, bool fromUpDeckFaceUp, bool fromUpArea, int fromUpAreaIdx) FindCardSource(Card movingCard)
    {
        int fromCol = -1, fromIdx = -1;
        bool fromUpDeckFaceUp = false;
        bool fromUpArea = false;
        int fromUpAreaIdx = -1;

        // 从下部接龙区查找
        for (int c = 0; c < DownArea.Count; c++)
        {
            int f = DownArea[c].FindIndex(x => GetCardObj(x) == movingCard);
            if (f != -1) { fromCol = c; fromIdx = f; break; }
        }
        // 从上部牌堆正面区查找
        if (fromCol == -1)
        {
            int f = UpDeck_FaceUp.FindIndex(x => GetCardObj(x) == movingCard);
            if (f != -1) { fromCol = -2; fromIdx = f; fromUpDeckFaceUp = true; }
        }
        // 从上部接龙区查找
        if (fromCol == -1)
        {
            for (int u = 0; u < UpArea.Count; u++)
            {
                if (UpArea[u].Count > 0 && GetCardObj(UpArea[u].Last()) == movingCard)
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
    public void CheckGameEnd()
    {
        if (GamePanel.Instance.FinishPanel.activeSelf
        || GamePanel.Instance._LittleSlot.gameObject.activeSelf
        || (UIManager.GetInstance().GetPanelByName(nameof(ScratchPanel)) != null && UIManager.GetInstance().GetPanelByName(nameof(ScratchPanel)).gameObject.activeSelf)
        || (UIManager.GetInstance().GetPanelByName(nameof(LuckyWheelPanel)) != null && UIManager.GetInstance().GetPanelByName(nameof(LuckyWheelPanel)).gameObject.activeSelf)
        || (UIManager.GetInstance().GetPanelByName(nameof(RewardPanel)) != null && UIManager.GetInstance().GetPanelByName(nameof(RewardPanel)).gameObject.activeSelf))
            return;

        // 先检查小游戏，如果有小游戏待触发，则延迟游戏结束检查
        if (IsTriggerSpecial_ScratchCard || IsTriggerSpecial_LuckyWheel || IsTriggerSpecial_Money || IsTriggerSpecial_Slot)
        {
            TriggerSpecialGame();
            return;
        }

        // 检查胜利条件
        if (CheckWin())
            return;



        //检查是否所有下部接龙区的牌都已翻开
        bool allCardsFaceUp = true;
        for (int i = 0; i < DownArea.Count; i++)
        {
            for (int j = 0; j < DownArea[i].Count; j++)
            {
                if (!DownArea[i][j].IsFaceUp)
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
            StartCoroutine(Win_AutoCollect());
            return;
        }

        // 检查游戏是否卡死
        if (IsGameStuck())
        {
            GamePanel.Instance.Finish(false);
            return;
        }
    }

    // 胜利判定
    bool CheckWin()
    {
        int count = 0;
        for (int i = 0; i < UpArea.Count; i++)
        {
            if (UpArea[i].Count == 13)
                count++;
        }
        if (count == 4)
        {
            Debug.Log("胜利！");
            GamePanel.Instance.Finish(true);
            return true;
        }
        return false;
    }

    // 检查游戏是否卡死
    bool IsGameStuck()
    {
        // 检查是否还有任何可能的移动
        // 1. 检查上部接龙区移动
        if (CheckTipStepA().hasTip) return false;

        // 2. 检查下部接龙区内部连接
        if (CheckTipStepB().hasTip) return false;

        // 3. 检查上部翻牌区移动
        if (CheckTipStepC().hasTip) return false;

        // 4. 检查是否可以翻牌
        if (CheckTipStepD().hasTip) return false;

        // 如果所有检查都失败，游戏确实卡住了
        return true;
    }

    // 胜利后自动收牌（如果所有牌都已翻开）
    private IEnumerator Win_AutoCollect()
    {
        if (IsWinCollecting) yield break;
        IsWinCollecting = true;
        GamePanel.Instance.ShowMask();
        Debug.Log("检测到所有牌都已翻开，开始自动收牌...");

        // 先把上部翻牌区背面朝上的牌全部移到正面区（视觉上不需要动画）
        for (int i = UpDeck_FaceDown.Count - 1; i >= 0; i--)
        {
            CardData cardData = UpDeck_FaceDown[i];
            Card cardObj = GetCardObj(cardData);
            if (cardObj != null)
                cardObj.BackImage.SetActive(false);
            UpDeck_FaceUp.Add(cardData);
        }
        UpDeck_FaceDown.Clear();

        bool hasCollected;
        do
        {
            hasCollected = false;
            // 1. 检查下部接龙区
            for (int i = 0; i < DownArea.Count; i++)
            {
                if (DownArea[i].Count > 0)
                {
                    CardData cardData = DownArea[i].Last();
                    Card cardObj = GetCardObj(cardData);
                    for (int k = 0; k < UpArea.Count; k++)
                    {
                        if (CanMoveToUpArea(cardObj, k))
                        {
                            DownArea[i].RemoveAt(DownArea[i].Count - 1);
                            UpArea[k].Add(cardData);

                            // 检查是否是第一次收入上部接龙区  检测是否触发slot
                            CheckUpAreaRewardedAndCheckSlot(cardData, k);

                            // UI动画
                            if (cardObj != null)
                            {
                                cardObj.transform.SetParent(GamePanel.Instance.CardParent_UpArea);
                                cardObj.transform.SetAsLastSibling();
                                Vector3 targetPos = GamePanel.Instance.Pos_UpArea[k].position;
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
            if (!hasCollected && UpDeck_FaceUp.Count > 0)
            {
                // 遍历上部翻牌区的所有牌，从最上面开始检查
                for (int i = UpDeck_FaceUp.Count - 1; i >= 0; i--)
                {
                    CardData cardData = UpDeck_FaceUp[i];
                    Card cardObj = GetCardObj(cardData);
                    for (int k = 0; k < UpArea.Count; k++)
                    {
                        if (CanMoveToUpArea(cardObj, k))
                        {
                            UpDeck_FaceUp.RemoveAt(i);
                            UpArea[k].Add(cardData);

                            // 检查是否是第一次收入上部接龙区  检测是否触发slot
                            CheckUpAreaRewardedAndCheckSlot(cardData, k);

                            if (cardObj != null)
                            {
                                cardObj.transform.SetParent(GamePanel.Instance.CardParent_UpArea);
                                cardObj.transform.SetAsLastSibling();
                                Vector3 targetPos = GamePanel.Instance.Pos_UpArea[k].position;
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
        } while (hasCollected);

        IsWinCollecting = false;
        //GamePanel.Instance.HideMask();

        // 等待所有动画完成后，统一触发小游戏和结算
        yield return new WaitForSeconds(0.2f);
        CheckGameEnd();
    }
    #endregion

    #region  相应点击牌自动移动
    // 自动移动单张牌（优先上部接龙区，其次下部接龙区）
    public bool AutoMoveCard(Card card)
    {
        // 优先尝试移动到上部接龙区
        for (int i = 0; i < UpArea.Count; i++)
        {
            if (CanMoveToUpArea(card, i))
            {
                return MoveCardToUpArea(card, i);
            }
        }
        // 尝试移动到下部接龙区
        for (int i = 0; i < DownArea.Count; i++)
        {
            if (DownArea[i].Count > 0)
            {
                Card targetCard = GetCardObj(DownArea[i].Last());
                if (CanStackOn(card, targetCard))
                {
                    return MoveCardStack(card, targetCard);
                }
            }
        }
        // 尝试移动到空的下部接龙区（如果是K）
        for (int i = 0; i < DownArea.Count; i++)
        {
            if (DownArea[i].Count == 0 && CanMoveToEmptyDownArea(card))
            {
                return MoveCardStackToEmptyDownArea(card, i);
            }
        }
        return false;
    }

    // 自动移动一组牌到下部接龙区尾部
    public bool AutoMoveCardStack(Card topCard)
    {
        var draggingCards = GetDraggingCards(topCard);
        if (draggingCards.Count <= 1) return false; // 只有单张牌不处理
        // 查找最佳目标位置
        int bestTargetCol = -1;
        Card bestTargetCard = null;
        // 优先查找能接上的牌
        for (int i = 0; i < DownArea.Count; i++)
        {
            if (DownArea[i].Count > 0)
            {
                Card targetCard = GetCardObj(DownArea[i].Last());
                if (CanStackOn(topCard, targetCard))
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
            return MoveCardStack(topCard, bestTargetCard);
        }
        // 尝试移动到空的下部接龙区（如果是K）
        for (int i = 0; i < DownArea.Count; i++)
        {
            if (DownArea[i].Count == 0 && CanMoveToEmptyDownArea(topCard))
            {
                return MoveCardStackToEmptyDownArea(topCard, i);
            }
        }
        return false;
    }
    #endregion

    #region  提示

    // 步骤a：检查下部接龙区和上部翻牌区的龙尾牌是否可以移动到上部接龙区
    public TipResult CheckTipStepA()
    {
        // 检查下部接龙区的龙尾牌
        for (int i = 0; i < DownArea.Count; i++)
        {
            if (DownArea[i].Count > 0)
            {
                CardData tailCard = DownArea[i].Last();
                if (tailCard.IsFaceUp)
                {
                    Card cardObj = GetCardObj(tailCard);
                    for (int j = 0; j < UpArea.Count; j++)
                    {
                        if (CanMoveToUpArea(cardObj, j))
                        {
                            Vector3 targetPos = GamePanel.Instance.Pos_UpArea[j].position;
                            return new TipResult(true, cardObj, targetPos, $"可以将{GetCardDisplayName(tailCard)}移动到上部接龙区");
                        }
                    }
                }
            }
        }

        // 检查上部翻牌区的牌
        if (UpDeck_FaceUp.Count > 0)
        {
            CardData faceUpCard = UpDeck_FaceUp.Last();
            Card cardObj = GetCardObj(faceUpCard);
            for (int j = 0; j < UpArea.Count; j++)
            {
                if (CanMoveToUpArea(cardObj, j))
                {
                    Vector3 targetPos = GamePanel.Instance.Pos_UpArea[j].position;
                    return new TipResult(true, cardObj, targetPos, $"可以将{GetCardDisplayName(faceUpCard)}移动到上部接龙区");
                }
            }
        }

        return new TipResult(false, null, Vector3.zero);
    }

    // 步骤b：检查下部接龙区内部连接
    public TipResult CheckTipStepB()
    {
        // 遍历所有下部接龙区，查找可以连接的牌
        for (int i = 0; i < DownArea.Count; i++)
        {
            if (DownArea[i].Count == 0) continue;

            // 查找当前区域的龙尾牌
            CardData tailCard = DownArea[i].Last();
            if (!tailCard.IsFaceUp) continue;

            Card tailCardObj = GetCardObj(tailCard);

            // 查找其他区域的龙头牌（最上面的牌）
            for (int j = 0; j < DownArea.Count; j++)
            {
                if (i == j || DownArea[j].Count == 0) continue;

                // 查找j区域中连续正面朝上的牌组
                for (int k = 0; k < DownArea[j].Count; k++)
                {
                    if (DownArea[j][k].IsFaceUp)
                    {
                        Card headCardObj = GetCardObj(DownArea[j][k]);
                        if (CanStackOn(headCardObj, tailCardObj))
                        {
                            Vector3 targetPos = tailCardObj.transform.position;
                            return new TipResult(true, headCardObj, targetPos, $"可以将{GetCardDisplayName(DownArea[j][k])}连接到{GetCardDisplayName(tailCard)}");
                        }
                        break; // 只检查第一个正面朝上的牌
                    }
                }
            }
        }

        // 检查K牌是否可以移动到空的下部接龙区
        int bestMoveScore = 0;
        TipResult bestKMove = new TipResult(false, null, Vector3.zero);

        for (int i = 0; i < DownArea.Count; i++)
        {
            if (DownArea[i].Count == 0) continue; // 跳过空区域

            // 查找当前区域中不在龙头的K牌
            for (int j = 0; j < DownArea[i].Count - 1; j++) // 不检查最后一张牌（龙头）
            {
                if (DownArea[i][j].IsFaceUp && DownArea[i][j].Value == CardValue.K)
                {
                    // 检查从K牌开始到区域末尾是否都是连续正面朝上的牌
                    bool canMove = true;
                    int moveCount = 0;
                    for (int checkIdx = j; checkIdx < DownArea[i].Count; checkIdx++)
                    {
                        if (!DownArea[i][checkIdx].IsFaceUp)
                        {
                            canMove = false;
                            break;
                        }
                        moveCount++;
                    }

                    if (canMove && moveCount > bestMoveScore)
                    {
                        // 检查是否有空的下部接龙区可以移动
                        for (int k = 0; k < DownArea.Count; k++)
                        {
                            if (DownArea[k].Count == 0)
                            {
                                // 只有当K牌不是龙头时才提示移动（避免K牌在空位时的错误提示）
                                if (j > 0)
                                {
                                    Card kCardObj = GetCardObj(DownArea[i][j]);
                                    Vector3 targetPos = GamePanel.Instance.Pos_DownArea[k].position;
                                    bestKMove = new TipResult(true, kCardObj, targetPos, $"可以将K牌及其下方{moveCount}张牌移动到空的下部接龙区");
                                    bestMoveScore = moveCount;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        if (bestKMove.hasTip)
        {
            return bestKMove;
        }

        return new TipResult(false, null, Vector3.zero);
    }

    // 步骤c：检查上部翻牌区未翻开的牌是否可以移动
    public TipResult CheckTipStepC()
    {
        // 检查上部翻牌区是否有牌可以移动
        if (UpDeck_FaceUp.Count > 0)
        {
            CardData faceUpCard = UpDeck_FaceUp.Last();
            Card cardObj = GetCardObj(faceUpCard);

            // 检查是否可以移动到下部接龙区
            for (int i = 0; i < DownArea.Count; i++)
            {
                if (DownArea[i].Count > 0)
                {
                    Card targetCard = GetCardObj(DownArea[i].Last());
                    if (CanStackOn(cardObj, targetCard))
                    {
                        Vector3 targetPos = targetCard.transform.position;
                        return new TipResult(true, cardObj, targetPos, $"可以将{GetCardDisplayName(faceUpCard)}移动到下部接龙区");
                    }
                }
                else if (CanMoveToEmptyDownArea(cardObj))
                {
                    Vector3 targetPos = GamePanel.Instance.Pos_DownArea[i].position;
                    return new TipResult(true, cardObj, targetPos, $"可以将{GetCardDisplayName(faceUpCard)}移动到空的下部接龙区");
                }
            }
        }

        return new TipResult(false, null, Vector3.zero);
    }

    // 步骤d：提示翻牌
    public TipResult CheckTipStepD()
    {
        // 检查是否需要翻牌
        if (UpDeck_FaceDown.Count > 0)
        {
            // 还有牌可以翻
            Vector3 targetPos = GamePanel.Instance.Pos_UpDeck_FaceUp.position;
            return new TipResult(true, null, targetPos, "点击翻牌按钮翻开新牌");
        }
        else if (UpDeck_FaceUp.Count > 0)
        {
            // 优化：遍历所有正面朝上的牌，判断是否有能移动的
            bool hasMovable = false;
            CardData movableCard = null;
            Vector3 movableTarget = Vector3.zero;
            string moveMsg = "";
            foreach (var cardData in UpDeck_FaceUp)
            {
                Card cardObj = GetCardObj(cardData);
                // 检查能否移动到下部接龙区
                for (int i = 0; i < DownArea.Count; i++)
                {
                    if (DownArea[i].Count > 0)
                    {
                        Card targetCard = GetCardObj(DownArea[i].Last());
                        if (CanStackOn(cardObj, targetCard))
                        {
                            hasMovable = true;
                            movableCard = cardData;
                            movableTarget = targetCard.transform.position;
                            moveMsg = $"可以将{GetCardDisplayName(cardData)}移动到下部接龙区";
                            break;
                        }
                    }
                    else if (CanMoveToEmptyDownArea(cardObj))
                    {
                        hasMovable = true;
                        movableCard = cardData;
                        movableTarget = GamePanel.Instance.Pos_DownArea[i].position;
                        moveMsg = $"可以将{GetCardDisplayName(cardData)}移动到空的下部接龙区";
                        break;
                    }
                }
                if (hasMovable) break;
                // 检查能否移动到上部接龙区
                for (int j = 0; j < UpArea.Count; j++)
                {
                    if (CanMoveToUpArea(cardObj, j))
                    {
                        hasMovable = true;
                        movableCard = cardData;
                        movableTarget = GamePanel.Instance.Pos_UpArea[j].position;
                        moveMsg = $"可以将{GetCardDisplayName(cardData)}移动到上部接龙区";
                        break;
                    }
                }
                if (hasMovable) break;
            }
            if (hasMovable)
            {
                // 有可移动的牌，提示玩家继续操作
                return new TipResult(true, GetCardObj(movableCard), movableTarget, moveMsg);
            }
            else
            {
                // 所有正面朝上的牌都无法移动，才提示死局
                Vector3 stuckPos = GamePanel.Instance.Pos_UpDeck_FaceDown.position;
                return new TipResult(false, null, stuckPos, "游戏无法继续，建议重新开始");
            }
        }
        else
        {
            // 没有牌了，检查是否胜利
            if (CheckWin())
                return new TipResult(true, null, Vector3.zero, "恭喜！游戏胜利！");
            else
                return new TipResult(false, null, Vector3.zero, "游戏结束");
        }
    }
    #endregion

    #region 自动收牌功能
    public bool AutoCollectSwitch //自动收集开关
    {
        get { return SaveDataManager.GetBool("AutoCollectSwitch"); }
        set { SaveDataManager.SetBool("AutoCollectSwitch", value); }
    }

    private IEnumerator AutoCollectCardsCoroutine()
    {
        GamePanel.Instance.ShowMask();
        yield return new WaitForSeconds(0.1f);

        bool hasCollected;
        int maxIterations = 100;
        int currentIteration = 0;

        do
        {
            hasCollected = false;
            currentIteration++;

            // 1. 检查下部接龙区的龙尾牌
            for (int i = 0; i < DownArea.Count; i++)
            {
                if (DownArea[i].Count > 0)
                {
                    CardData tailCard = DownArea[i].Last();
                    if (tailCard.IsFaceUp)
                    {
                        Card cardObj = GetCardObj(tailCard);
                        for (int j = 0; j < UpArea.Count; j++)
                        {
                            if (CanMoveToUpArea(cardObj, j))
                            {
                                MoveCardToUpAreaInternal(cardObj, j);
                                GamePanel.Instance.AddScore(10, AutoMoveCardAddScoreEffectPos);
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
            if (!hasCollected && UpDeck_FaceUp.Count > 0)
            {
                CardData faceUpCard = UpDeck_FaceUp.Last();
                Card cardObj = GetCardObj(faceUpCard);
                for (int j = 0; j < UpArea.Count; j++)
                {
                    if (CanMoveToUpArea(cardObj, j))
                    {
                        MoveCardToUpAreaInternal(cardObj, j);
                        GamePanel.Instance.AddScore(15, AutoMoveCardAddScoreEffectPos);
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
                for (int i = 0; i < DownArea.Count; i++)
                {
                    for (int j = 0; j < DownArea[i].Count; j++)
                    {
                        if (!DownArea[i][j].IsFaceUp)
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
                    while (UpDeck_FaceUp.Count > 0)
                    {
                        CardData faceUpCard = UpDeck_FaceUp.Last();
                        Card cardObj = GetCardObj(faceUpCard);
                        bool moved = false;
                        for (int j = 0; j < UpArea.Count; j++)
                        {
                            if (CanMoveToUpArea(cardObj, j))
                            {
                                MoveCardToUpAreaInternal(cardObj, j);
                                GamePanel.Instance.AddScore(15, AutoMoveCardAddScoreEffectPos);
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

        } while (hasCollected);

        // 检查游戏结束条件
        GamePanel.Instance.HideMask();
        yield return new WaitForSeconds(0.2f);
        CheckGameEnd();
    }

    // 内部移动方法，不触发自动收牌（避免递归）
    private bool MoveCardToUpAreaInternal(Card movingCard, int upAreaIdx)
    {
        var source = FindCardSource(movingCard);
        if (source.fromCol == -1) return false;

        CardData cardData;
        if (source.fromUpDeckFaceUp)
        {
            cardData = UpDeck_FaceUp[source.fromIdx];
            UpDeck_FaceUp.RemoveAt(source.fromIdx);
        }
        else if (source.fromUpArea)
        {
            cardData = UpArea[source.fromUpAreaIdx].Last();
            UpArea[source.fromUpAreaIdx].RemoveAt(UpArea[source.fromUpAreaIdx].Count - 1);
        }
        else
        {
            cardData = DownArea[source.fromCol][source.fromIdx];
            DownArea[source.fromCol].RemoveAt(source.fromIdx);
        }
        UpArea[upAreaIdx].Add(cardData);
        var cardObj2 = GetCardObj(cardData);
        cardObj2.transform.SetParent(GamePanel.Instance.CardParent_UpArea);
        cardObj2.transform.SetAsLastSibling();

        // 检查是否是第一次收入上部接龙区  检测是否触发slot
        CheckUpAreaRewardedAndCheckSlot(cardData, upAreaIdx);

        // 添加收牌动画
        Vector3 targetPos = GamePanel.Instance.Pos_UpArea[upAreaIdx].position;
        AutoMoveCardAddScoreEffectPos = targetPos;
        cardObj2.transform.DOMove(targetPos, 0.3f).SetEase(Ease.OutQuad);
        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.SFX_CardCollect);

        if (!source.fromUpDeckFaceUp && !source.fromUpArea)
            CheckFlipAfterMove(source.fromCol);

        return true;
    }
    #endregion

    #region 撤销功能
    private void SaveStateForUndo()
    {
        KlondikeGameState state = new KlondikeGameState();
        state.UpDeck_FaceUp = DeepCopyCardDataList(UpDeck_FaceUp);
        state.UpDeck_FaceDown = DeepCopyCardDataList(UpDeck_FaceDown);
        state.UpArea = DeepCopyCardDataListList(UpArea);
        state.DownArea = DeepCopyCardDataListList(DownArea);
        // 保存所有牌的UI状态
        state.CardUIStates = new List<CardUIState>();
        foreach (var card in cardDataToCardMap.Values)
        {
            if (card != null)
                state.CardUIStates.Add(new CardUIState(card));
        }
        // 保存操作次数和游戏时间
        state.MoveCount = moveCount;
        state.GameTime = GetGameTime();
        undoStack.Push(state);
    }
    private List<CardData> DeepCopyCardDataList(List<CardData> src)
    {
        List<CardData> copy = new List<CardData>();
        foreach (var c in src)
            copy.Add(new CardData(c.Suit, c.Value, c.IsFaceUp, c.IsUpAreaRewarded));
        return copy;
    }
    private List<List<CardData>> DeepCopyCardDataListList(List<List<CardData>> src)
    {
        List<List<CardData>> copy = new List<List<CardData>>();
        foreach (var l in src)
            copy.Add(DeepCopyCardDataList(l));
        return copy;
    }

    public void Undo()
    {
        if (!CanUndo())
            return;
        AddMoveCount(); // 增加操作次数
        var state = undoStack.Pop();
        // 先恢复数据（不刷新UI）
        UpDeck_FaceUp = DeepCopyCardDataList(state.UpDeck_FaceUp);
        UpDeck_FaceDown = DeepCopyCardDataList(state.UpDeck_FaceDown);
        UpArea = DeepCopyCardDataListList(state.UpArea);
        DownArea = DeepCopyCardDataListList(state.DownArea);
        // 恢复操作次数和游戏时间
        moveCount = state.MoveCount;
        float savedGameTime = state.GameTime;
        gameStartTime = Time.time - savedGameTime;

        // 更新UI显示
        if (GamePanel.Instance != null)
        {
            GamePanel.Instance.UpdateMoveCount(moveCount);
            GamePanel.Instance.UpdateGameTime(savedGameTime);
        }

        // 播放撤销动画
        StartCoroutine(PlayUndoAnimation(state.CardUIStates));
        SaveGame();
    }
    // 撤销动画协程
    private IEnumerator PlayUndoAnimation(List<CardUIState> targetStates)
    {
        GamePanel.Instance.ShowMask();
        int animCount = 0;
        foreach (var target in targetStates)
        {
            // 找到当前对应的Card对象
            Card card = null;
            foreach (var c in cardDataToCardMap.Values)
            {
                if (c != null && c.Data.Suit == target.Suit && c.Data.Value == target.Value)
                {
                    card = c;
                    break;
                }
            }
            if (card == null) continue;

            // 位置不同，移动动画
            if ((card.transform.position - target.Position).sqrMagnitude > 0.01f)
            {
                animCount++;
                card.transform.DOMove(target.Position, 0.3f).SetEase(Ease.OutQuad).OnComplete(() => { animCount--; });
            }

            // 父物体不同，动画后SetParent
            if (card.transform.parent == null || card.transform.parent.name != target.ParentName)
            {
                animCount++;
                card.transform.DOMove(target.Position, 0.3f).SetEase(Ease.OutQuad).OnComplete(() =>
                {
                    // 检查card是否已被销毁
                    if (card != null)
                    {
                        // 查找目标父物体
                        Transform newParent = FindParentByName(target.ParentName);
                        if (newParent != null) card.transform.SetParent(newParent);
                    }
                    animCount--;
                });
            }

            // 正反面不同，翻转动画
            if (card.Data.IsFaceUp != target.IsFaceUp)
            {
                animCount++;
                StartCoroutine(UndoFlipCardAnim(card, target.IsFaceUp, () => { animCount--; }));
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
        RefreshAllCards();
        GamePanel.Instance.HideMask();
        CheckGameEnd();
    }
    // 查找父物体
    private Transform FindParentByName(string name)
    {
        if (GamePanel.Instance.CardParent_DownArea.name == name) return GamePanel.Instance.CardParent_DownArea;
        if (GamePanel.Instance.CardParent_UpArea.name == name) return GamePanel.Instance.CardParent_UpArea;
        if (GamePanel.Instance.CardParent_UpDeck.name == name) return GamePanel.Instance.CardParent_UpDeck;
        if (GamePanel.Instance.CardParent_Drag.name == name) return GamePanel.Instance.CardParent_Drag;
        return null;
    }
    // 撤销翻牌动画
    private IEnumerator UndoFlipCardAnim(Card card, bool toFaceUp, System.Action onEnd)
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
        card.Data.IsFaceUp = toFaceUp;
        card.Init(card.Data);

        // 旋转回0度
        card.transform.DORotate(Vector3.zero, 0.15f).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(0.15f);

        onEnd?.Invoke();
    }
    // 刷新所有牌并带动画
    private void RefreshAllCards()
    {
        // 清理所有Card对象
        foreach (var card in cardDataToCardMap.Values)
        {
            if (card != null)
                Destroy(card.gameObject);
        }
        cardDataToCardMap.Clear();
        // 重新初始化UpArea和DownArea，防止为空
        if (UpArea == null || UpArea.Count != 4)
        {
            UpArea = new List<List<CardData>>();
            for (int i = 0; i < 4; i++) UpArea.Add(new List<CardData>());
        }
        if (DownArea == null || DownArea.Count != 7)
        {
            DownArea = new List<List<CardData>>();
            for (int i = 0; i < 7; i++) DownArea.Add(new List<CardData>());
        }
        List<Card> allCards = new List<Card>();
        // 下部接龙区
        for (int i = 0; i < DownArea.Count; i++)
        {
            for (int j = 0; j < DownArea[i].Count; j++)
            {
                Card card = CreateCard(DownArea[i][j]);
                card.transform.SetParent(GamePanel.Instance.CardParent_DownArea);
                card.transform.localScale = Vector3.one;
                card.transform.position = GamePanel.Instance.Pos_DownArea[i].position + new Vector3(0, -j * Gap_Y);
                card.Init(DownArea[i][j]);
                allCards.Add(card);
            }
        }
        // 上部接龙区
        for (int i = 0; i < UpArea.Count; i++)
        {
            for (int j = 0; j < UpArea[i].Count; j++)
            {
                Card card = CreateCard(UpArea[i][j]);
                card.transform.SetParent(GamePanel.Instance.CardParent_UpArea);
                card.transform.localScale = Vector3.one;
                card.transform.position = GamePanel.Instance.Pos_UpArea[i].position;
                card.Init(UpArea[i][j]);
                allCards.Add(card);
            }
        }
        // 上部牌堆
        foreach (var c in UpDeck_FaceDown)
        {
            Card card = CreateCard(c);
            card.transform.SetParent(GamePanel.Instance.CardParent_UpDeck);
            card.transform.localScale = Vector3.one;
            card.transform.position = GamePanel.Instance.Pos_UpDeck_FaceDown.position;
            card.Init(c);
            allCards.Add(card);
        }
        foreach (var c in UpDeck_FaceUp)
        {
            Card card = CreateCard(c);
            card.transform.SetParent(GamePanel.Instance.CardParent_UpDeck);
            card.transform.localScale = Vector3.one;
            card.transform.position = GamePanel.Instance.Pos_UpDeck_FaceUp.position;
            card.Init(c);
            allCards.Add(card);
        }
        StartCoroutine(DelayHideMaskAfterAnim(0.2f));
    }
    private IEnumerator DelayHideMaskAfterAnim(float delay)
    {
        yield return new WaitForSeconds(delay);
        GamePanel.Instance.HideMask();
    }
    //是否可以撤销
    public bool CanUndo()
    {
        return undoStack.Count > 0;
    }
    #endregion

    #region 存档方法
    public void SaveGame()
    {
        if (UpArea == null)
            UpArea = new List<List<CardData>>();
        if (DownArea == null)
            DownArea = new List<List<CardData>>();
        KlondikeGameState state = new KlondikeGameState();
        state.UpDeck_FaceUp = DeepCopyCardDataList(UpDeck_FaceUp);
        state.UpDeck_FaceDown = DeepCopyCardDataList(UpDeck_FaceDown);
        state.UpArea = DeepCopyCardDataListList(UpArea);
        state.DownArea = DeepCopyCardDataListList(DownArea);
        state.CardUIStates = new List<CardUIState>();
        // 保存操作次数和游戏时间
        state.MoveCount = moveCount;
        state.GameTime = GetGameTime();
        // 按照实际牌堆顺序生成CardUIStates
        // 1. 下部接龙区（从下到上，从左到右）
        for (int i = 0; i < DownArea.Count; i++)
        {
            for (int j = 0; j < DownArea[i].Count; j++)
            {
                Card card = GetCardObj(DownArea[i][j]);
                if (card != null)
                    state.CardUIStates.Add(new CardUIState(card));
            }
        }
        // 2. 上部接龙区（从左到右）
        for (int i = 0; i < UpArea.Count; i++)
        {
            for (int j = 0; j < UpArea[i].Count; j++)
            {
                Card card = GetCardObj(UpArea[i][j]);
                if (card != null)
                    state.CardUIStates.Add(new CardUIState(card));
            }
        }
        // 3. 上部牌堆背面区（从下到上）
        for (int i = 0; i < UpDeck_FaceDown.Count; i++)
        {
            Card card = GetCardObj(UpDeck_FaceDown[i]);
            if (card != null)
                state.CardUIStates.Add(new CardUIState(card));
        }
        // 4. 上部牌堆正面区（从下到上）
        for (int i = 0; i < UpDeck_FaceUp.Count; i++)
        {
            Card card = GetCardObj(UpDeck_FaceUp[i]);
            if (card != null)
                state.CardUIStates.Add(new CardUIState(card));
        }
        string json = JsonUtility.ToJson(state);
        //print(json);
        SaveDataManager.SetString(SaveKey, json);
    }
    // 读档方法
    public bool LoadGame()
    {
        if (!PlayerPrefs.HasKey(SaveKey))
            return false;
        string json = SaveDataManager.GetString(SaveKey);
        print(json);
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
        UpDeck_FaceUp = DeepCopyCardDataList(state.UpDeck_FaceUp);
        UpDeck_FaceDown = DeepCopyCardDataList(state.UpDeck_FaceDown);
        // 兼容旧存档：如果UpArea/DownArea为空，则用CardUIStates重建
        if ((state.UpArea == null || state.UpArea.Count == 0) && (state.DownArea == null || state.DownArea.Count == 0) && state.CardUIStates != null)
        {
            RebuildAreaFromCardUIStates(state.CardUIStates);
        }
        else
        {
            UpArea = DeepCopyCardDataListList(state.UpArea);
            DownArea = DeepCopyCardDataListList(state.DownArea);
        }
        if (UpDeck_FaceUp == null) UpDeck_FaceUp = new List<CardData>();
        if (UpDeck_FaceDown == null) UpDeck_FaceDown = new List<CardData>();
        if (UpArea == null) UpArea = new List<List<CardData>>();
        if (DownArea == null) DownArea = new List<List<CardData>>();

        //读取上部牌堆奖励牌
        string UpDeck_RewardCard_Save = SaveDataManager.GetString("UpDeck_RewardCardNames");
        if (!string.IsNullOrEmpty(UpDeck_RewardCard_Save))
            UpDeck_RewardCardNames = UpDeck_RewardCard_Save.Split(',').ToList();

        // 读取操作次数和游戏时间
        moveCount = state.MoveCount;
        float savedGameTime = state.GameTime;
        gameStartTime = Time.time - savedGameTime; // 调整开始时间以保持游戏时间

        // 更新UI显示
        if (GamePanel.Instance != null)
        {
            GamePanel.Instance.UpdateMoveCount(moveCount);
            GamePanel.Instance.UpdateGameTime(savedGameTime);
        }

        // 使用动画生成牌，类似StartNewGame的效果
        StartCoroutine(LoadGameWithAnimation(state.CardUIStates));
        return true;
    }
    // 读档时使用动画生成牌
    private IEnumerator LoadGameWithAnimation(List<CardUIState> cardUIStates)
    {
        GamePanel.Instance.ShowMask();

        // 清理所有Card对象
        foreach (var card in cardDataToCardMap.Values)
        {
            if (card != null)
                Destroy(card.gameObject);
        }
        cardDataToCardMap.Clear();

        // 重新初始化UpArea和DownArea，防止为空
        if (UpArea == null || UpArea.Count != 4)
        {
            UpArea = new List<List<CardData>>();
            for (int i = 0; i < 4; i++) UpArea.Add(new List<CardData>());
        }
        if (DownArea == null || DownArea.Count != 7)
        {
            DownArea = new List<List<CardData>>();
            for (int i = 0; i < 7; i++) DownArea.Add(new List<CardData>());
        }

        int cardIndex = 0;

        // 1. 下部接龙区（从下到上，从左到右）
        for (int i = 0; i < DownArea.Count; i++)
        {
            for (int j = 0; j < DownArea[i].Count; j++)
            {
                Card card = CreateCard(DownArea[i][j]);
                card.transform.SetParent(GamePanel.Instance.CardParent_DownArea);
                card.transform.localScale = Vector3.one;
                card.transform.position = GamePanel.Instance.Pos_CardBorn.position;
                card.Init(DownArea[i][j]);

                Vector3 targetPos = GamePanel.Instance.Pos_DownArea[i].position + new Vector3(0, -j * Gap_Y);
                card.transform.DOMove(targetPos, 0.5f).SetDelay(cardIndex * 0.05f).OnStart(() =>
                {
                    if (i % 2 == 0)
                        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.SFX_CardSet);
                });
                cardIndex++;
            }
        }

        // 2. 上部接龙区（从左到右）
        for (int i = 0; i < UpArea.Count; i++)
        {
            for (int j = 0; j < UpArea[i].Count; j++)
            {
                Card card = CreateCard(UpArea[i][j]);
                card.transform.SetParent(GamePanel.Instance.CardParent_UpArea);
                card.transform.localScale = Vector3.one;
                card.transform.position = GamePanel.Instance.Pos_CardBorn.position;
                card.Init(UpArea[i][j]);

                Vector3 targetPos = GamePanel.Instance.Pos_UpArea[i].position;
                card.transform.DOMove(targetPos, 0.5f).SetDelay(cardIndex * 0.05f).OnStart(() =>
                {
                    if (i % 2 == 0)
                        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.SFX_CardSet);
                });
                cardIndex++;
            }
        }

        // 3. 上部牌堆背面区（从下到上）
        for (int i = 0; i < UpDeck_FaceDown.Count; i++)
        {
            Card card = CreateCard(UpDeck_FaceDown[i]);
            card.transform.SetParent(GamePanel.Instance.CardParent_UpDeck);
            card.transform.localScale = Vector3.one;
            card.transform.position = GamePanel.Instance.Pos_CardBorn.position;
            card.Init(UpDeck_FaceDown[i]);

            Vector3 targetPos = GamePanel.Instance.Pos_UpDeck_FaceDown.position;
            card.transform.DOMove(targetPos, 0.5f).SetDelay(cardIndex * 0.03f).OnStart(() =>
                {
                    if (i % 2 == 0)
                        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.SFX_CardSet);
                });
            cardIndex++;
        }

        // 4. 上部牌堆正面区（从下到上）
        for (int i = 0; i < UpDeck_FaceUp.Count; i++)
        {
            Card card = CreateCard(UpDeck_FaceUp[i]);
            card.transform.SetParent(GamePanel.Instance.CardParent_UpDeck);
            card.transform.localScale = Vector3.one;
            card.transform.position = GamePanel.Instance.Pos_CardBorn.position;
            card.Init(UpDeck_FaceUp[i]);

            Vector3 targetPos = GamePanel.Instance.Pos_UpDeck_FaceUp.position;
            card.transform.DOMove(targetPos, 0.5f).SetDelay(cardIndex * 0.03f).OnStart(() =>
                {
                    if (i % 2 == 0)
                        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.SFX_CardSet);
                });
            cardIndex++;
        }

        // 等待所有动画完成
        float totalDelay = cardIndex * 0.05f + 0.5f;
        yield return new WaitForSeconds(totalDelay);

        GamePanel.Instance.HideMask();
    }
    // 新增：根据CardUIStates自动推断并重建UpArea/DownArea
    private void RebuildAreaFromCardUIStates(List<CardUIState> cardUIStates)
    {
        // 先清空
        UpArea = new List<List<CardData>>();
        DownArea = new List<List<CardData>>();
        for (int i = 0; i < 4; i++) UpArea.Add(new List<CardData>());
        for (int i = 0; i < 7; i++) DownArea.Add(new List<CardData>());
        // 遍历所有CardUIState
        foreach (var state in cardUIStates)
        {
            CardData data = new CardData(state.Suit, state.Value, state.IsFaceUp, state.IsUpAreaRewarded);
            if (state.ParentName.Contains("上部接龙区"))
            {
                // 归入UpArea，按位置y排序
                int idx = 0;
                float minDist = float.MaxValue;
                for (int i = 0; i < 4; i++)
                {
                    float dist = Mathf.Abs(state.Position.x - GamePanel.Instance.Pos_UpArea[i].position.x);
                    if (dist < minDist) { minDist = dist; idx = i; }
                }
                UpArea[idx].Add(data);
            }
            else if (state.ParentName.Contains("下部接龙区"))
            {
                int idx = 0;
                float minDist = float.MaxValue;
                for (int i = 0; i < 7; i++)
                {
                    float dist = Mathf.Abs(state.Position.x - GamePanel.Instance.Pos_DownArea[i].position.x);
                    if (dist < minDist) { minDist = dist; idx = i; }
                }
                DownArea[idx].Add(data);
            }
            // 其他区域（如上部牌堆）不处理
        }
    }
    #endregion

    #region 智能换牌
    // 智能换牌：下部接龙区未翻开的牌在翻开前，优先把场上缺的牌翻出来
    private void TrySmartFlipCard(int col)
    {
        // 概率控制
        if (UnityEngine.Random.value >= SmartFlipProbability) return;
        var area = DownArea[col];
        if (area.Count == 0) return;
        if (area.Last().IsFaceUp) return;

        // 1. 收集所有下部接龙区未翻开的牌
        List<(int col, int idx, CardData card)> unflipped = new List<(int, int, CardData)>();
        for (int c = 0; c < DownArea.Count; c++)
            for (int i = 0; i < DownArea[c].Count; i++)
                if (!DownArea[c][i].IsFaceUp)
                    unflipped.Add((c, i, DownArea[c][i]));

        // 2. 优先级1：能接在下部接龙区的牌
        foreach (var entry in unflipped)
        {
            if (CanStackOnAnyDownArea(entry.card, entry.col))
            {
                if (entry.col != col || entry.idx != DownArea[col].Count - 1)
                    SwapUnflippedCard(col, DownArea[col].Count - 1, entry.col, entry.idx);
                return;
            }
        }
        // 3. 优先级2：能接在上部接龙区的牌
        foreach (var entry in unflipped)
        {
            if (CanStackOnAnyUpArea(entry.card))
            {
                if (entry.col != col || entry.idx != DownArea[col].Count - 1)
                    SwapUnflippedCard(col, DownArea[col].Count - 1, entry.col, entry.idx);
                return;
            }
        }
        // 4. 否则不换
    }
    // 交换两张未翻开的牌的位置
    private void SwapUnflippedCard(int colA, int idxA, int colB, int idxB)
    {
        // 交换数据
        var tmp = DownArea[colA][idxA];
        DownArea[colA][idxA] = DownArea[colB][idxB];
        DownArea[colB][idxB] = tmp;

        // 交换UI位置和父物体（无动画，玩家无感知）
        var cardA = GetCardObj(DownArea[colA][idxA]);
        var cardB = GetCardObj(DownArea[colB][idxB]);
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
    private bool CanStackOnAnyDownArea(CardData card, int excludeCol)
    {
        for (int i = 0; i < DownArea.Count; i++)
        {
            if (i == excludeCol) continue;
            var area = DownArea[i];
            if (area.Count == 0) continue;
            var top = area.Last();
            var cardObj = GetCardObj(card);
            var topObj = GetCardObj(top);
            if (cardObj != null && topObj != null && CanStackOn(cardObj, topObj)) return true;
        }
        return false;
    }

    // 能否接在上部接龙区
    private bool CanStackOnAnyUpArea(CardData card)
    {
        var cardObj = GetCardObj(card);
        if (cardObj == null) return false;
        for (int i = 0; i < UpArea.Count; i++)
        {
            if (CanMoveToUpArea(cardObj, i)) return true;
        }
        return false;
    }
    #endregion

    #region  结算
    // 检查是否是第一次收入上部接龙区  检测是否触发slot
    void CheckUpAreaRewardedAndCheckSlot(CardData cardData, int upIdx)
    {
        if (!cardData.IsUpAreaRewarded)
        {
            cardData.IsUpAreaRewarded = true;
            GetUpAreaReward(upIdx);

            //判断是否触发小游戏 slot
            if (!IsPlayedSpecialSlot)
            {
                int SevenCount = 0;
                for (int l = 0; l < UpArea.Count; l++)
                {
                    for (int j = 0; j < UpArea[l].Count; j++)
                    {
                        if (UpArea[l][j].Value == CardValue.Seven)
                            SevenCount++;
                    }
                }
                if (SevenCount >= 4 && !IsTriggerSpecial_Slot)
                    IsTriggerSpecial_Slot = true;
            }
        }
    }

    public void ReStart() //重新开始
    {
        ClearGame();
        StartNewGame();
    }
    public void ClearGame() //清空场景 清空存档 销毁场景中物体 
    {
        // 清理所有Card对象
        foreach (var card in cardDataToCardMap.Values)
        {
            if (card != null)
                Destroy(card.gameObject);
        }
        cardDataToCardMap.Clear();

        // 清空所有数据
        UpDeck_FaceUp.Clear();
        UpDeck_FaceDown.Clear();
        UpArea.Clear();
        DownArea.Clear();

        // 清空撤销栈和其他状态
        undoStack.Clear();
        timeCounter = 0f;
        lastScorePenaltyTime = 0;
        moveCount = 0;
        gameStartTime = Time.time;

        // 清理存档
        if (PlayerPrefs.HasKey(SaveKey))
        {
            PlayerPrefs.DeleteKey(SaveKey);
        }
    }

    public void Revive() //复活
    {
        AddMoveCount(); // 增加操作次数
        StartCoroutine(ReviveCoroutine());
        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.SFX_Wand);
    }
    private IEnumerator ReviveCoroutine()
    {
        GamePanel.Instance.ShowMask();
        bool hasRevived = false;

        // i. 遍历所有下部接龙区盖着的牌，检查能否收入上部接龙区
        for (int col = 0; col < DownArea.Count; col++)
        {
            for (int idx = 0; idx < DownArea[col].Count; idx++)
            {
                var cardData = DownArea[col][idx];
                if (!cardData.IsFaceUp)
                {
                    cardData.IsFaceUp = true;
                    Card cardObj = GetCardObj(cardData);
                    for (int upIdx = 0; upIdx < UpArea.Count; upIdx++)
                    {
                        if (CanMoveToUpAreaForRevive(cardObj, upIdx))
                        {
                            // 复活：收入上部接龙区
                            DownArea[col].RemoveAt(idx);
                            UpArea[upIdx].Add(cardData);
                            // 检查是否是第一次收入上部接龙区  检测是否触发slot
                            CheckUpAreaRewardedAndCheckSlot(cardData, upIdx);

                            if (cardObj != null)
                            {
                                cardObj.transform.SetParent(GamePanel.Instance.CardParent_UpArea);
                                cardObj.transform.SetAsLastSibling();
                                Vector3 targetPos = GamePanel.Instance.Pos_UpArea[upIdx].position;
                                cardObj.transform.DOMove(targetPos, 0.3f).SetEase(Ease.OutQuad);
                                cardObj.Init(cardData);
                                AnimationController.MagicWand(GamePanel.Instance.MagicWand, cardObj.transform.position, targetPos);
                            }
                            print($"复活成功 - 收入上部接龙区：{GetCardDisplayName(cardData)} 移动到上部接龙区：{upIdx}");
                            hasRevived = true;
                            yield return new WaitForSeconds(0.35f);
                            break;
                        }
                    }
                    if (hasRevived) break;
                    cardData.IsFaceUp = false; // 恢复原状态
                }
            }
            if (hasRevived) break;
        }

        // ii. 如果没有收入上部接龙区，检查能否在下部接龙区接龙
        if (!hasRevived)
        {
            for (int col = 0; col < DownArea.Count; col++)
            {
                for (int idx = 0; idx < DownArea[col].Count; idx++)
                {
                    var cardData = DownArea[col][idx];
                    if (!cardData.IsFaceUp)
                    {
                        cardData.IsFaceUp = true;
                        Card cardObj = GetCardObj(cardData);

                        // 检查能否接龙到其他下部接龙区
                        for (int targetCol = 0; targetCol < DownArea.Count; targetCol++)
                        {
                            if (targetCol == col) continue; // 跳过自己

                            if (DownArea[targetCol].Count > 0)
                            {
                                Card targetCard = GetCardObj(DownArea[targetCol].Last());
                                if (CanStackOn(cardObj, targetCard))
                                {
                                    // 复活：接龙到下部接龙区
                                    DownArea[col].RemoveAt(idx);
                                    DownArea[targetCol].Add(cardData);
                                    if (cardObj != null)
                                    {
                                        cardObj.transform.SetParent(GamePanel.Instance.CardParent_DownArea);
                                        cardObj.transform.SetAsLastSibling();
                                        Vector3 targetPos = GamePanel.Instance.Pos_DownArea[targetCol].position + new Vector3(0, -(DownArea[targetCol].Count - 1) * Gap_Y);
                                        cardObj.transform.DOMove(targetPos, 0.3f).SetEase(Ease.OutQuad);
                                        cardObj.Init(cardData);
                                        AnimationController.MagicWand(GamePanel.Instance.MagicWand, cardObj.transform.position, targetPos);
                                    }
                                    print($"复活成功 - 下部接龙：{GetCardDisplayName(cardData)} 移动到下部接龙区：{targetCol}");
                                    hasRevived = true;
                                    yield return new WaitForSeconds(0.35f);
                                    break;
                                }
                            }
                            else if (cardData.Value == CardValue.K)
                            {
                                // 如果是K牌，可以移动到空的下部接龙区
                                DownArea[col].RemoveAt(idx);
                                DownArea[targetCol].Add(cardData);
                                if (cardObj != null)
                                {
                                    cardObj.transform.SetParent(GamePanel.Instance.CardParent_DownArea);
                                    cardObj.transform.SetAsLastSibling();
                                    Vector3 targetPos = GamePanel.Instance.Pos_DownArea[targetCol].position;
                                    cardObj.transform.DOMove(targetPos, 0.3f).SetEase(Ease.OutQuad);
                                    cardObj.Init(cardData);
                                    AnimationController.MagicWand(GamePanel.Instance.MagicWand, cardObj.transform.position, targetPos);
                                }
                                print($"复活成功 - K牌移动：{GetCardDisplayName(cardData)} 移动到空的下部接龙区：{targetCol}");
                                hasRevived = true;
                                yield return new WaitForSeconds(0.35f);
                                break;
                            }
                        }
                        if (hasRevived) break;
                        cardData.IsFaceUp = false; // 恢复原状态
                    }
                }
                if (hasRevived) break;
            }
        }

        if (!hasRevived)
        {
            print("复活失败：没有找到可移动的牌");
        }

        SaveGame();
        RefreshAllCards();
        GamePanel.Instance.HideMask();
        yield return new WaitForSeconds(0.2f);
        CheckGameEnd();
    }
    #endregion

    #region 新手引导
    void StartNewGame_Guide() // 新手引导专用初始化方法
    {
        GamePanel.Instance.ClearScore();
        // 重置操作次数和时间
        IsPlayedSpecialSlot = false;
        SaveDataManager.SetBool("IsPlayedSpecialSlot", false);
        moveCount = 0;
        gameStartTime = Time.time;
        if (GamePanel.Instance != null)
        {
            GamePanel.Instance.UpdateMoveCount(moveCount);
            GamePanel.Instance.UpdateGameTime(0);
        }
        //初始化上部接龙区
        UpArea.Clear();
        for (int i = 0; i < 4; i++)
            UpArea.Add(new List<CardData>());
        // 初始布置
        List<CardData> CardPools_UnallocatedPool = new List<CardData>();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 13; j++)
            {
                CardData data = new CardData((CardSuit)i, (CardValue)j, false, false);
                CardPools_UnallocatedPool.Add(data);
            }
        }

        // 找出需要的固定牌
        CardData diamond10 = CardPools_UnallocatedPool.First(x => x.Suit == CardSuit.Cube && x.Value == CardValue.Ten);
        CardData heart2 = CardPools_UnallocatedPool.First(x => x.Suit == CardSuit.RedPeach && x.Value == CardValue.Two);
        CardData heartQ = CardPools_UnallocatedPool.First(x => x.Suit == CardSuit.RedPeach && x.Value == CardValue.Q);
        CardData spadeK = CardPools_UnallocatedPool.First(x => x.Suit == CardSuit.BlackPeach && x.Value == CardValue.K);
        CardData heartA = CardPools_UnallocatedPool.First(x => x.Suit == CardSuit.RedPeach && x.Value == CardValue.One);
        CardData clubJ = CardPools_UnallocatedPool.First(x => x.Suit == CardSuit.BlackFlower && x.Value == CardValue.J);

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
            var matchingCards = CardPools_UnallocatedPool.Where(x => x.IsRed() == SinkColorRed && (int)x.Value == i).ToList();
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
        DownArea.Clear();
        for (int i = 0; i < 7; i++)
        {
            List<CardData> cards = new List<CardData>();
            for (int j = 0; j < i; j++)
            {
                if (usedSinkCards + j < CardPools_Sink.Count)
                    cards.Add(CardPools_Sink[usedSinkCards + j]);
            }
            DownArea.Add(cards);
            usedSinkCards += i;
        }

        // 删除已使用的沉底牌
        if (usedSinkCards > 0)
            CardPools_Sink.RemoveRange(0, Math.Min(usedSinkCards, CardPools_Sink.Count));

        // 生成下部接龙区 不沉底的牌（固定牌放在指定位置）
        // 第一列底部：红桃2
        DownArea[0].Add(heart2);
        heart2.IsFaceUp = true;

        // 第二列底部：红桃Q
        DownArea[1].Add(heartQ);
        heartQ.IsFaceUp = true;

        // 第三列底部：黑桃K
        DownArea[2].Add(spadeK);
        spadeK.IsFaceUp = true;

        // 第四列底部：红桃A
        DownArea[3].Add(heartA);
        heartA.IsFaceUp = true;

        // 第五列底部：梅花J
        DownArea[4].Add(clubJ);
        clubJ.IsFaceUp = true;

        // 其他列使用随机牌
        for (int i = 5; i < 7; i++)
        {
            if (CardPools_NoSink.Count > 0)
            {
                CardPools_NoSink[0].IsFaceUp = true;
                DownArea[i].Add(CardPools_NoSink[0]);
                CardPools_NoSink.RemoveAt(0);
            }
        }

        // 生成上部牌堆 背面朝上（最上面是方块10）
        UpDeck_FaceDown.Clear();
        UpDeck_FaceDown.Add(diamond10); // 最上面一张牌是方块10

        // 添加剩余牌
        for (int i = 0; i < 23 && CardPools_NoSink.Count > 0; i++)
        {
            UpDeck_FaceDown.Add(CardPools_NoSink[0]);
            CardPools_NoSink.RemoveAt(0);
        }

        // 创建牌对象
        Vector2[] Pos_DownArea = GamePanel.Instance.Pos_DownArea.Select(x => (Vector2)x.position).ToArray();
        for (int i = 0; i < DownArea.Count && i < Pos_DownArea.Length; i++)
        {
            for (int j = 0; j < DownArea[i].Count; j++)
            {
                Card card = CreateCard(DownArea[i][j]);
                card.transform.DOMove(Pos_DownArea[i] + new Vector2(0, -j * Gap_Y), 0.5f).SetDelay(j * 0.05f).OnStart(() =>
                {
                    MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.SFX_CardSet);
                });
            }
        }

        Vector2 Pos_UpDeck_FaceDown = GamePanel.Instance.Pos_UpDeck_FaceDown.position;
        for (int i = 0; i < UpDeck_FaceDown.Count; i++)
        {
            Card card = CreateCard(UpDeck_FaceDown[i]);
            card.transform.DOMove(Pos_UpDeck_FaceDown, 0.5f).SetDelay(i * 0.03f).OnStart(() =>
            {
                MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.SFX_CardSet);
            });
        }
        // 不需要设置奖励牌
        UpDeck_RewardCardNames.Clear();
    }
    #endregion
}

public enum CardSuit { BlackPeach, RedPeach, BlackFlower, Cube } // 黑桃，红桃，梅花，方片
public enum CardValue { One, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, J, Q, K } // 1-13

/// <summary> 纸牌数据 </summary>
[System.Serializable]
public class CardData
{
    public CardSuit Suit; // 花色
    public CardValue Value; // 牌值
    public bool IsFaceUp; // 是否正面朝上
    public bool IsUpAreaRewarded; // 是否发放过上部接龙区奖励

    public CardData(CardSuit suit, CardValue value, bool isFaceUp, bool isUpAreaRewarded)
    {
        Suit = suit;
        Value = value;
        IsFaceUp = isFaceUp;
        IsUpAreaRewarded = isUpAreaRewarded;
    }

    public bool IsRed() // 是否是红桃或方片
    {
        return Suit == CardSuit.RedPeach || Suit == CardSuit.Cube;
    }
}

// ========== 撤销功能 ========== 
[System.Serializable]
public class KlondikeGameState
{
    public List<CardData> UpDeck_FaceUp;
    public List<CardData> UpDeck_FaceDown;
    public List<List<CardData>> UpArea;
    public List<List<CardData>> DownArea;
    // 新增：保存所有牌的UI状态
    public List<CardUIState> CardUIStates;
    // 新增：保存操作次数和游戏时间
    public int MoveCount;
    public float GameTime;
}
// 牌的UI状态
[System.Serializable]
public class CardUIState
{
    public CardSuit Suit;
    public CardValue Value;
    public bool IsFaceUp;
    public bool IsUpAreaRewarded; // 是否发放过上部接龙区奖励
    public string ParentName; // 父物体名
    public Vector3 Position;
    public CardUIState(Card card)
    {
        Suit = card.Data.Suit;
        Value = card.Data.Value;
        IsFaceUp = card.Data.IsFaceUp;
        IsUpAreaRewarded = card.Data.IsUpAreaRewarded;
        ParentName = card.transform.parent != null ? card.transform.parent.name : "";
        Position = card.transform.position;
    }
    public bool IsSameCard(Card card)
    {
        return card.Data.Suit == Suit && card.Data.Value == Value;
    }
}
