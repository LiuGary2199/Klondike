using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Coffee.UIExtensions;
using Spine;
using Spine.Unity;
using UnityEngine.SocialPlatforms.Impl;

/// <summary> 克朗代克纸牌游戏主界面 </summary>
public class GamePanel : BaseUIForms
{
    public static GamePanel Instance;

    [Header("UI相关")]
    public Text GoldText;
    public GameObject GoldIcon;
    public Text CashText;
    public GameObject CashIcon;
    public Text ScoreText;
    public Text MoveCountText;
    public Text TimeText;
    public GameObject FinishPanel;
    public GameObject WinPanel;
    public GameObject WinSpine;
    public Transform[] Stars; //星星
    public List<GameObject> ShowList;
    public GameObject MagicWand;
    public Transform[] StarsStartTF;
    public GameObject StarsEffect;
    public GameObject Fx_Show;
    public GameObject AddScoreItem;
    public List<GameObject> starEffect;
    float Reward_Gold;
    public Text RewardText_Gold;
    float Reward_Cash;
    public Text RewardText_Cash;
    public SlotGroup HSlotGroup; //横向slot奖励翻倍
    public Button HSlotBtn; //横向slot奖励翻倍按钮
    public GameObject LosePanel;
    public Button ReviveBtn;
    public Button Win_ReStartBtn;
    public Button Lose_ReStartBtn;
    public Button SettingBtn;
    Coroutine DelayShowGetBtn;
    public LittleSlot _LittleSlot;
    public Image LuckyWheel_Fill;
    public FlyBubble _FlyBubble;

    [Header("玩法相关")]
    // 牌堆父物体
    public Transform CardParent_UpDeck;
    public Transform CardParent_UpArea;
    public Transform CardParent_DownArea;
    public Transform CardParent_Drag;

    public Transform Pos_CardBorn; //牌生成位置
    public Transform Pos_UpDeck_FaceUp; // 上部牌堆 正面朝上 位置
    public Transform Pos_UpDeck_FaceDown; // 上部牌堆 背面朝上 位置
    public List<Transform> Pos_UpArea = new List<Transform>(); // 上部接龙区 4格位置
    public List<Transform> Pos_DownArea = new List<Transform>(); // 下部接龙区 7格位置
    public Button UpDeck_FlipBtn;
    public UIParticle[] UpArea_RewardEffect; //上部接龙区奖励特效
    public UIParticle UpDeck_RewardEffect; //上部牌堆奖励特效
    // 提示动画相关
    public Button CardTipBtn;
    public Transform CardTip;
    private bool isShowingTip = false;
    public Text TipMessageText;
    public GameObject TipMessagePanel;
    int Prop_TipCount;

    public Button UndoBtn; //撤销按钮
    public GameObject Mask; //屏蔽操作遮罩
    int Prop_UndoCount;

    int PropPrice;


    void Awake()
    {
        Instance = this;

        // 初始化提示
        CardTip.gameObject.SetActive(false);
        isShowingTip = false;
        if (TipMessagePanel != null)
            TipMessagePanel.SetActive(false);

        GoldText.text = GameDataManager.GetInstance().GetGold().ToString();
        CashText.text = GameDataManager.GetInstance().GetCash().ToString();
        ScoreText.text = GameDataManager.GetInstance().GetScore().ToString();

        // 初始化操作次数和时间显示
        if (MoveCountText != null)
            MoveCountText.text = "0";
        if (TimeText != null)
            TimeText.text = "00:00";

        //奖励翻倍
        HSlotBtn.onClick.AddListener(PlayHSlot);
        //复活
        ReviveBtn.onClick.AddListener(() =>
        {
            ADManager.Instance.playRewardVideo((ok) =>
            {
                if (ok)
                {
                    Klondike_Manager.Instance.Revive();
                    FinishPanel.SetActive(false);
                    PostEventScript.GetInstance().SendEvent("1005", "1", Klondike_Manager.Instance.GetDownAreaFaceDownCardNum_ForEvent(), TimeText.text);
                }
            }, "2");
        });
        //胜利后 拿奖励 重开游戏
        Win_ReStartBtn.onClick.AddListener(() =>
        {
            PostEventScript.GetInstance().SendEvent("1004", "2", GameDataManager.GetInstance().GetScore().ToString(), TimeText.text);
            HSlotBtn.gameObject.SetActive(false);
            Win_ReStartBtn.gameObject.SetActive(false);
            GameDataManager.GetInstance().AddGold(Reward_Gold);
            UpdateGold();
            Klondike_Manager.Instance.ReStart();
            FinishPanel.SetActive(false);
        });
        //失败后 重开游戏
        Lose_ReStartBtn.onClick.AddListener(() =>
        {
            PostEventScript.GetInstance().SendEvent("1005", "2", Klondike_Manager.Instance.GetDownAreaFaceDownCardNum_ForEvent(), TimeText.text);
            Klondike_Manager.Instance.ReStart();
            FinishPanel.SetActive(false);
        });

        SettingBtn.onClick.AddListener(() =>
        {
            OpenUIForm(nameof(SettingPanel));
        });
    }

    private void Start()
    {
        Klondike_Manager.Instance.Init();
        UpDeck_FlipBtn.onClick.AddListener(Klondike_Manager.Instance.UpDeck_Filp);
        CardTipBtn.onClick.AddListener(UseProp_Tip);
        UndoBtn.onClick.AddListener(UseProp_Undo);
        PropPrice = NetInfoMgr.instance._GameData.prop_price;
        UpdateProp();
        InvokeRepeating(nameof(ShowFlyBubble), 0, 60);
    }
    void ShowFlyBubble()
    {
        _FlyBubble.Fly();
    }

    #region 提示逻辑
    public void ShowCardTip()
    {
        if (isShowingTip)
        {
            // 如果正在显示提示，先隐藏当前提示
            HideAllTips();
            return;
        }

        // 执行4步提示逻辑
        StartCoroutine(TipLogic());
    }

    private IEnumerator TipLogic()
    {
        isShowingTip = true;

        // 步骤a：检查是否可以移动到上部接龙区
        var tipResult = Klondike_Manager.Instance.CheckTipStepA();
        if (tipResult.hasTip)
        {
            ShowTipAnimation(tipResult.sourceCard, tipResult.targetPosition);
            ShowTipMessage(tipResult.tipMessage);
            yield break;
        }

        // 步骤b：检查下部接龙区内部连接
        tipResult = Klondike_Manager.Instance.CheckTipStepB();
        if (tipResult.hasTip)
        {
            ShowTipAnimation(tipResult.sourceCard, tipResult.targetPosition);
            ShowTipMessage(tipResult.tipMessage);
            yield break;
        }

        // 步骤c：检查上部翻牌区是否有可移动的牌
        tipResult = Klondike_Manager.Instance.CheckTipStepC();
        if (tipResult.hasTip)
        {
            ShowTipAnimation(tipResult.sourceCard, tipResult.targetPosition);
            ShowTipMessage(tipResult.tipMessage);
            yield break;
        }

        // 步骤d：提示翻牌
        tipResult = Klondike_Manager.Instance.CheckTipStepD();
        if (tipResult.hasTip)
        {
            ShowTipAnimation(tipResult.sourceCard, tipResult.targetPosition);
            ShowTipMessage(tipResult.tipMessage);
            yield break;
        }

        // 没有找到任何提示
        ShowTipMessage("没有找到可用的提示");
        yield return new WaitForSeconds(1.5f);
        HideTipMessage();
        Debug.Log("没有找到可用的提示");
        isShowingTip = false;
    }

    private void ShowTipAnimation(Card sourceCard, Vector3 targetPosition)
    {
        Vector3 StartPos = Vector3.zero;
        Vector3 EndPos = Vector3.zero;
        // 设置提示动画的起始和结束位置
        if (sourceCard != null)
        {
            StartPos = sourceCard.transform.position;
            EndPos = targetPosition;
            CardTip.transform.Find("花色").GetComponent<Image>().sprite = Klondike_Manager.Instance.GetSuitSprite(sourceCard.Data);
            CardTip.transform.Find("Text").GetComponent<Text>().text = Klondike_Manager.Instance.ValueTexts[(int)sourceCard.Data.Value];
            CardTip.transform.Find("Text").GetComponent<Text>().color = sourceCard.Data.IsRed() ? Color.red : Color.black;
            CardTip.transform.Find("卡背").gameObject.SetActive(false);
        }
        else
        {
            // 对于翻牌提示，使用固定的起始位置
            StartPos = Pos_UpDeck_FaceDown.position;
            EndPos = targetPosition;
            CardTip.transform.Find("卡背").gameObject.SetActive(true);
        }

        CardTip.gameObject.SetActive(true);
        CardTip.position = StartPos;
        CardTip.DOKill();
        CardTip.DOMove(EndPos, 1).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
    }

    public void HideCardTip()
    {
        CardTip.DOKill();
        CardTip.gameObject.SetActive(false);
    }

    // 隐藏所有提示（在玩家操作时调用）
    public void HideAllTips()
    {
        HideCardTip();
        HideTipMessage();
        isShowingTip = false;
    }

    private void ShowTipMessage(string message)
    {
        print("提示: " + message);
        if (TipMessagePanel != null && TipMessageText != null)
        {
            TipMessageText.text = message;
            TipMessagePanel.SetActive(true);
        }
    }

    private void HideTipMessage()
    {
        if (TipMessagePanel != null)
        {
            TipMessagePanel.SetActive(false);
        }
    }
    #endregion

    #region  道具
    public void UpdateProp() //更新道具
    {
        int Gold = (int)GameDataManager.GetInstance().GetGold();

        Prop_TipCount = GameDataManager.GetInstance().GetProp_Tip();
        if (Prop_TipCount > 0)
        {
            CardTipBtn.transform.Find("角标/广告").gameObject.SetActive(false);
            CardTipBtn.transform.Find("角标/金币").gameObject.SetActive(false);
            CardTipBtn.transform.Find("角标/数量Text").gameObject.SetActive(true);
            CardTipBtn.transform.Find("角标/数量Text").GetComponent<Text>().text = Prop_TipCount.ToString();
        }
        else if (Gold >= PropPrice)
        {
            CardTipBtn.transform.Find("角标/广告").gameObject.SetActive(false);
            CardTipBtn.transform.Find("角标/金币").gameObject.SetActive(true);
            CardTipBtn.transform.Find("角标/金币/Text").GetComponent<Text>().text = PropPrice.ToString();
            CardTipBtn.transform.Find("角标/数量Text").gameObject.SetActive(false);
        }
        else
        {
            CardTipBtn.transform.Find("角标/广告").gameObject.SetActive(true);
            CardTipBtn.transform.Find("角标/金币").gameObject.SetActive(false);
            CardTipBtn.transform.Find("角标/数量Text").gameObject.SetActive(false);
        }

        Prop_UndoCount = GameDataManager.GetInstance().GetProp_Undo();
        if (Prop_UndoCount > 0)
        {
            UndoBtn.transform.Find("角标/广告").gameObject.SetActive(false);
            UndoBtn.transform.Find("角标/金币").gameObject.SetActive(false);
            UndoBtn.transform.Find("角标/数量Text").gameObject.SetActive(true);
            UndoBtn.transform.Find("角标/数量Text").GetComponent<Text>().text = Prop_UndoCount.ToString();
        }
        else if (Gold >= PropPrice)
        {
            UndoBtn.transform.Find("角标/广告").gameObject.SetActive(false);
            UndoBtn.transform.Find("角标/金币").gameObject.SetActive(true);
            UndoBtn.transform.Find("角标/金币/Text").GetComponent<Text>().text = PropPrice.ToString();
            UndoBtn.transform.Find("角标/数量Text").gameObject.SetActive(false);
        }
        else
        {
            UndoBtn.transform.Find("角标/广告").gameObject.SetActive(true);
            UndoBtn.transform.Find("角标/金币").gameObject.SetActive(false);
            UndoBtn.transform.Find("角标/数量Text").gameObject.SetActive(false);
        }
    }

    void UseProp_Tip() //使用提示
    {
        Prop_TipCount = GameDataManager.GetInstance().GetProp_Tip();
        int Gold = (int)GameDataManager.GetInstance().GetGold();
        if (Prop_TipCount > 0)
        {
            GameDataManager.GetInstance().AddProp_Tip(-1);
            UpdateProp();
            ShowCardTip();
            PostEventScript.GetInstance().SendEvent("1006", "1");
        }
        else if (Gold >= PropPrice)
        {
            GameDataManager.GetInstance().AddGold(-PropPrice);
            UpdateGold();
            UpdateProp();
            ShowCardTip();
            PostEventScript.GetInstance().SendEvent("1006", "2");
        }
        else
        {
            ADManager.Instance.playRewardVideo((ok) =>
            {
                if (ok)
                {
                    ShowCardTip();
                    PostEventScript.GetInstance().SendEvent("1006", "3");
                }
            }, "8");
        }
    }
    void UseProp_Undo() //使用撤销
    {
        if (!Klondike_Manager.Instance.CanUndo())
        {
            ToastManager.GetInstance().ShowToast("There are no undo operations");
            return;
        }

        Prop_UndoCount = GameDataManager.GetInstance().GetProp_Undo();
        int Gold = (int)GameDataManager.GetInstance().GetGold();
        if (Prop_UndoCount > 0)
        {
            GameDataManager.GetInstance().AddProp_Undo(-1);
            UpdateProp();
            Klondike_Manager.Instance.Undo();
            PostEventScript.GetInstance().SendEvent("1007", "1");
        }
        else if (Gold >= PropPrice)
        {
            GameDataManager.GetInstance().AddGold(-PropPrice);
            UpdateGold();
            UpdateProp();
            Klondike_Manager.Instance.Undo();
            PostEventScript.GetInstance().SendEvent("1007", "2");
        }
        else
        {
            ADManager.Instance.playRewardVideo((ok) =>
            {
                if (ok)
                {
                    Klondike_Manager.Instance.Undo();
                    PostEventScript.GetInstance().SendEvent("1007", "3");
                }
            }, "9");
        }
    }
    #endregion

    public void ShowMask()
    {
        if (Mask != null) Mask.SetActive(true);
    }
    public void HideMask()
    {
        if (Mask != null) Mask.SetActive(false);
    }

    public void AddScore(int score, Vector3 Pos = default(Vector3)) // 更新分数
    {
        int oldScore = GameDataManager.GetInstance().GetScore();
        int newScore = Mathf.Max(0, oldScore + score);
        GameDataManager.GetInstance().SetScore(newScore);
        AnimationController.ChangeNumber(oldScore, newScore, 0.1f, ScoreText, null);
        AddScoreItem.GetComponentInChildren<Text>().text = score > 0 ? "+" + score : score.ToString();
        if (Pos == default(Vector3))
            Pos = ScoreText.transform.position;
        AnimationController.AddScore(Pos);
    }
    public void ClearScore() // 清空分数
    {
        GameDataManager.GetInstance().SetScore(0);
        ScoreText.text = "0";
    }

    public void UpdateGold() // 更新金币
    {
        AnimationController.ChangeNumber(float.Parse(GoldText.text, System.Globalization.CultureInfo.CurrentCulture), GameDataManager.GetInstance().GetGold(), 0.1f, GoldText, null);

        //金币变化影响道具角标
        UpdateProp();
    }

    public void UpdateCash() // 更新现金
    {
        AnimationController.ChangeNumber(float.Parse(CashText.text, System.Globalization.CultureInfo.CurrentCulture), GameDataManager.GetInstance().GetCash(), 0.1f, CashText, null);
    }

    public void UpdateMoveCount(int count)  // 更新操作次数
    {
        if (MoveCountText != null)
        {
            MoveCountText.text = count.ToString();
        }
    }

    public void UpdateGameTime(float timeInSeconds) // 更新游戏时间显示
    {
        if (TimeText != null)
        {
            int minutes = Mathf.FloorToInt(timeInSeconds / 60);
            int seconds = Mathf.FloorToInt(timeInSeconds % 60);
            TimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void Finish(bool IsWin)
    {
        FinishPanel.gameObject.SetActive(true);
        int[] star_score = NetInfoMgr.instance._GameData.star_score; //星级分数
        int[] star_reward_Gold = NetInfoMgr.instance._GameData.star_reward_gold; //星级奖励
        int[] star_reward_Cash = NetInfoMgr.instance._GameData.star_reward_cash; //星级奖励
        if (IsWin)
        {
            AddScore(NetInfoMgr.instance._GameData.levelcomplete_reward_count, Vector3.zero);
            WinPanel.SetActive(true);
            LosePanel.SetActive(false);
            int StarCount = 0;
            Reward_Gold = 0;
            RewardText_Gold.transform.parent.gameObject.SetActive(false);
            Reward_Cash = 0;
            RewardText_Cash.transform.parent.gameObject.SetActive(false);
            // 动画开始----------------------
            // 星星的轨迹list
            List<Transform> StarList = new List<Transform>(Stars);
            List<Transform> StarStartPosList = new List<Transform>(StarsStartTF);
            // 奖励按钮依次展示
            AnimationController.WinPanelShow(ShowList);
            SkeletonGraphic ItemSpine = WinSpine.GetComponent<SkeletonGraphic>();
            // 重置骨骼到初始姿态
            ItemSpine.Skeleton.SetToSetupPose();
            ItemSpine.AnimationState.ClearTracks();
            // 强制立即更新状态（重要！）
            ItemSpine.Update(0);
            // 过关spine展示
            WinSpine.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(1, "ShowAni", false);
            Fx_Show.SetActive(true);
            if (GameDataManager.GetInstance().GetScore() >= star_score[2])
            {
                AnimationController.WinStar(3, StarList, StarStartPosList, StarList, starEffect, WinPanel.transform);
                StarCount = 3;
                if (star_reward_Gold != null && star_reward_Gold.Length > 0)
                    Reward_Gold = star_reward_Gold[2];
                if (star_reward_Cash != null && star_reward_Cash.Length > 0)
                    Reward_Cash = star_reward_Cash[2];
            }
            else if (GameDataManager.GetInstance().GetScore() >= star_score[1])
            {
                AnimationController.WinStar(2, StarList, StarStartPosList, StarList, starEffect, WinPanel.transform);
                StarCount = 2;
                if (star_reward_Gold != null && star_reward_Gold.Length > 0)
                    Reward_Gold = star_reward_Gold[1];
                if (star_reward_Cash != null && star_reward_Cash.Length > 0)
                    Reward_Cash = star_reward_Cash[1];
            }
            else
            {
                AnimationController.WinStar(1, StarList, StarStartPosList, StarList, starEffect, WinPanel.transform);
                StarCount = 1;
                if (star_reward_Gold != null && star_reward_Gold.Length > 0)
                    Reward_Gold = star_reward_Gold[0];
                if (star_reward_Cash != null && star_reward_Cash.Length > 0)
                    Reward_Cash = star_reward_Cash[0];
            }
            if (CommonUtil.IsApple())
                Reward_Cash = 0;
            HSlotGroup.initMulti();
            HSlotBtn.gameObject.SetActive(true);
            Win_ReStartBtn.gameObject.SetActive(false);
            if (Reward_Gold > 0)
            {
                RewardText_Gold.transform.parent.gameObject.SetActive(true);
                AnimationController.ChangeNumber(0, Reward_Gold, 0.1f, RewardText_Gold, null);
            }
            if (Reward_Cash > 0)
            {
                RewardText_Cash.transform.parent.gameObject.SetActive(true);
                AnimationController.ChangeNumber(0, Reward_Cash, 0.1f, RewardText_Cash, null);
            }
            DelayShowGetBtn = TimeManager.GetInstance().Delay(2, () =>
              {
                  Win_ReStartBtn.gameObject.SetActive(true);
                  Win_ReStartBtn.transform.localScale = new Vector3(0, 0, 0);
                  Win_ReStartBtn.transform.DOScale(1, 0.2f).SetEase(Ease.OutBack);
              });
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.SFX_EndSuccessGame);

            //第一局游戏胜利
            if (!PlayerPrefs.HasKey("WaitRateUsBool"))
                SaveDataManager.SetBool("WaitRateUs", true);
        }
        else
        {
            WinPanel.SetActive(false);
            LosePanel.SetActive(true);
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.SFX_EedLostGame);
        }
    }

    #region  slot

    void PlayHSlot() //横向slot翻倍
    {
        ADManager.Instance.playRewardVideo((ok) =>
        {
            if (ok)
            {
                TimeManager.GetInstance().StopDelay(DelayShowGetBtn);
                HSlotBtn.gameObject.SetActive(false);
                Win_ReStartBtn.gameObject.SetActive(false);
                HSlotGroup.slot(0, (multi) =>
                {
                    PostEventScript.GetInstance().SendEvent("1004", "1", GameDataManager.GetInstance().GetScore().ToString(), TimeText.text);
                    if (Reward_Gold > 0)
                    {
                        float OldReward = Reward_Gold;
                        Reward_Gold = Reward_Gold * multi;
                        GameDataManager.GetInstance().AddGold(Reward_Gold);
                        AnimationController.ChangeNumber(OldReward, Reward_Gold, 0.1f, RewardText_Gold, null);
                    }
                    if (Reward_Cash > 0)
                    {
                        float OldReward = Reward_Cash;
                        Reward_Cash = Reward_Cash * multi;
                        GameDataManager.GetInstance().AddCash(Reward_Cash);
                        AnimationController.ChangeNumber(OldReward, Reward_Cash, 0.1f, RewardText_Cash, null);
                    }
                    TimeManager.GetInstance().Delay(1, () =>
                    {
                        UpdateGold();
                        UpdateCash();
                        Klondike_Manager.Instance.ReStart();
                        FinishPanel.SetActive(false);
                    });
                });
            }
        }, "1");
    }

    public void PlayLittleSlot() //小slot玩法
    {
        _LittleSlot.gameObject.SetActive(true);
        _LittleSlot.Slot();
        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.SFX_Slots);
        PostEventScript.GetInstance().SendEvent("1014", MoveCountText.text);
    }

    #endregion

    public void ShowUpAreaRewardEffect(int index, RewardType type)
    {
        UpArea_RewardEffect[index].Play();
        if (type == RewardType.Gold)
            AnimationController.KlondikeGetGold(GoldIcon, 3, UIManager.GetInstance()._Top, UpArea_RewardEffect[index].transform.position, GoldIcon.transform.position, null);
        else if (type == RewardType.Cash)
            AnimationController.KlondikeGetGold(CashIcon, 3, UIManager.GetInstance()._Top, UpArea_RewardEffect[index].transform.position, CashIcon.transform.position, null);
    }
    public void ShowUpDeckRewardEffect(RewardType type)
    {
        UpDeck_RewardEffect.Play();
        if (type == RewardType.Gold)
            AnimationController.KlondikeGetGold(GoldIcon, 3, UIManager.GetInstance()._Top, UpDeck_RewardEffect.transform.position, GoldIcon.transform.position, null);
        else if (type == RewardType.Cash)
            AnimationController.KlondikeGetGold(CashIcon, 3, UIManager.GetInstance()._Top, UpDeck_RewardEffect.transform.position, CashIcon.transform.position, null);
    }

    public void SetLuckyWheelFill()
    {
        LuckyWheel_Fill.fillAmount = Klondike_Manager.Instance.LuckyWheelCount / 4f;
    }

    public void MagicWandEffect()
    {

    }
}

// 提示结果结构
public struct TipResult
{
    public bool hasTip;
    public Card sourceCard;
    public Vector3 targetPosition;
    public string tipMessage;

    public TipResult(bool hasTip, Card sourceCard, Vector3 targetPosition, string tipMessage = "")
    {
        this.hasTip = hasTip;
        this.sourceCard = sourceCard;
        this.targetPosition = targetPosition;
        this.tipMessage = tipMessage;
    }
}
