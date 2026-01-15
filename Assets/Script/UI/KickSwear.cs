using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Coffee.UIExtensions;
using Spine.Unity;

/// <summary> 克朗代克纸牌游戏主界面 </summary>
public class KickSwear : SnowUIPlace
{
    public static KickSwear Instance;

    [Header("UI相关")]
    [UnityEngine.Serialization.FormerlySerializedAs("GoldText")] public Text CastMoss;
    [UnityEngine.Serialization.FormerlySerializedAs("GoldIcon")] public GameObject CastPart;
    [UnityEngine.Serialization.FormerlySerializedAs("CashText")] public Text NoteMoss;
    [UnityEngine.Serialization.FormerlySerializedAs("CashIcon")] public GameObject NotePart;
    [UnityEngine.Serialization.FormerlySerializedAs("ScoreText")] public Text DenseMoss;
    [UnityEngine.Serialization.FormerlySerializedAs("MoveCountText")] public Text SlumWaistMoss;
    [UnityEngine.Serialization.FormerlySerializedAs("TimeText")] public Text MoldMoss;
    [UnityEngine.Serialization.FormerlySerializedAs("FinishPanel")] public GameObject FinishSwear;
    [UnityEngine.Serialization.FormerlySerializedAs("WinPanel")] public GameObject UrnSwear;
    [UnityEngine.Serialization.FormerlySerializedAs("WinSpine")] public GameObject UrnDepot;
    [UnityEngine.Serialization.FormerlySerializedAs("Stars")] public Transform[] Taint; //星星
    [UnityEngine.Serialization.FormerlySerializedAs("ShowList")] public List<GameObject> BindTrip;
    [UnityEngine.Serialization.FormerlySerializedAs("MagicWand")] public GameObject CarveNeon;
    [UnityEngine.Serialization.FormerlySerializedAs("StarsStartTF")] public Transform[] TaintBadgeTF;
    [UnityEngine.Serialization.FormerlySerializedAs("StarsEffect")] public GameObject TaintGuinea;
    [UnityEngine.Serialization.FormerlySerializedAs("Fx_Show")] public GameObject Me_Bind;
    [UnityEngine.Serialization.FormerlySerializedAs("AddScoreItem")] public GameObject PegDenseAfar;
    [UnityEngine.Serialization.FormerlySerializedAs("starEffect")] public List<GameObject> GermGuinea;
    float Starve_Cast;
    [UnityEngine.Serialization.FormerlySerializedAs("RewardText_Gold")] public Text StarveMoss_Cast;
    float Starve_Note;
    [UnityEngine.Serialization.FormerlySerializedAs("RewardText_Cash")] public Text StarveMoss_Note;
    [UnityEngine.Serialization.FormerlySerializedAs("HSlotGroup")] public DishPerry HDishPerry; //横向slot奖励翻倍
    [UnityEngine.Serialization.FormerlySerializedAs("HSlotBtn")] public Button HDishGem; //横向slot奖励翻倍按钮
    [UnityEngine.Serialization.FormerlySerializedAs("LosePanel")] public GameObject SoapSwear;
    public SkeletonGraphic LoseSpine;
    public List<GameObject> LoseShowList;
    [UnityEngine.Serialization.FormerlySerializedAs("ReviveBtn")] public Button GrahamGem;
    [UnityEngine.Serialization.FormerlySerializedAs("Win_ReStartBtn")] public Button Urn_HeBadgeGem;
    [UnityEngine.Serialization.FormerlySerializedAs("Lose_ReStartBtn")] public Button Soap_HeBadgeGem;
    [UnityEngine.Serialization.FormerlySerializedAs("SettingBtn")] public Button GradualGem;
    Coroutine AwardBindFarGem;
    [UnityEngine.Serialization.FormerlySerializedAs("_LittleSlot")] public LittleSlot _HiccupDish;
    [UnityEngine.Serialization.FormerlySerializedAs("LuckyWheel_Fill")] public Image AwaitFaith_Gene;
    [UnityEngine.Serialization.FormerlySerializedAs("_FlyBubble")] public SacAvenue _SacAvenue;

    [Header("玩法相关")]
    [UnityEngine.Serialization.FormerlySerializedAs("CardParent_UpDeck")]    // 牌堆父物体
    public Transform KeelQuasar_OfPram;
    [UnityEngine.Serialization.FormerlySerializedAs("CardParent_UpArea")] public Transform KeelQuasar_OfPeck;
    [UnityEngine.Serialization.FormerlySerializedAs("CardParent_DownArea")] public Transform KeelQuasar_PinkPeck;
    [UnityEngine.Serialization.FormerlySerializedAs("CardParent_Drag")] public Transform KeelQuasar_Waxy;
    [UnityEngine.Serialization.FormerlySerializedAs("Pos_CardBorn")]
    public Transform Sum_KeelAvid; //牌生成位置
    [UnityEngine.Serialization.FormerlySerializedAs("Pos_UpDeck_FaceUp")] public Transform Sum_OfPram_HighOf; // 上部牌堆 正面朝上 位置
    [UnityEngine.Serialization.FormerlySerializedAs("Pos_UpDeck_FaceDown")] public Transform Sum_OfPram_HighPink; // 上部牌堆 背面朝上 位置
    [UnityEngine.Serialization.FormerlySerializedAs("Pos_UpArea")] public List<Transform> Sum_OfPeck = new List<Transform>(); // 上部接龙区 4格位置
    [UnityEngine.Serialization.FormerlySerializedAs("Pos_DownArea")] public List<Transform> Sum_PinkPeck = new List<Transform>(); // 下部接龙区 7格位置
    [UnityEngine.Serialization.FormerlySerializedAs("UpDeck_FlipBtn")] public Button OfPram_BarbGem;
    [UnityEngine.Serialization.FormerlySerializedAs("UpArea_RewardEffect")] public UIParticle[] OfPeck_StarveGuinea; //上部接龙区奖励特效
    [UnityEngine.Serialization.FormerlySerializedAs("UpDeck_RewardEffect")] public UIParticle OfPram_StarveGuinea; //上部牌堆奖励特效
    [UnityEngine.Serialization.FormerlySerializedAs("CardTipBtn")]    // 提示动画相关
    public Button KeelMayGem;
    [UnityEngine.Serialization.FormerlySerializedAs("CardTip")] public Transform KeelMay;
    private bool ByAsphaltMay = false;
    [UnityEngine.Serialization.FormerlySerializedAs("TipMessageText")] public Text MayAfricanMoss;
    [UnityEngine.Serialization.FormerlySerializedAs("TipMessagePanel")] public GameObject MayAfricanSwear;
    int Genu_MayWaist;
    [UnityEngine.Serialization.FormerlySerializedAs("UndoBtn")]
    public Button UndoGem; //撤销按钮
    [UnityEngine.Serialization.FormerlySerializedAs("Mask")] public GameObject Verb; //屏蔽操作遮罩
    int Genu_NoteWaist;
    int GenuBread;
    [UnityEngine.Serialization.FormerlySerializedAs("CashOutBtn")]
    public Transform NoteRedGem;


    void Awake()
    {
        Instance = this;

        // 初始化提示
        KeelMay.gameObject.SetActive(false);
        ByAsphaltMay = false;
        if (MayAfricanSwear != null)
            MayAfricanSwear.SetActive(false);

        CastMoss.text = KickLineRancher.FarBefriend().FarCast().ToString();
        NoteMoss.text = KickLineRancher.FarBefriend().FarNote().ToString();
        DenseMoss.text = KickLineRancher.FarBefriend().FarDense().ToString();

        // 初始化操作次数和时间显示
        if (SlumWaistMoss != null)
            SlumWaistMoss.text = "0";
        if (MoldMoss != null)
            MoldMoss.text = "00:00";

        //奖励翻倍
        HDishGem.onClick.AddListener(HaulHDish);
        //复活
        GrahamGem.onClick.AddListener(() =>
        {
            ADRancher.Befriend.LionStarveAlder((ok) =>
            {
                if (ok)
                {
                    Freezing_Rancher.Instance.Graham();
                    FinishSwear.SetActive(false);
                    WhimInuitRemove.FarBefriend().LeafInuit("1005", "1", Freezing_Rancher.Instance.FarPinkPeckHighPinkKeelLad_HutInuit(), MoldMoss.text);
                }
            }, "2");
        });
        //胜利后 拿奖励 重开游戏
        Urn_HeBadgeGem.onClick.AddListener(() =>
        {
            WhimInuitRemove.FarBefriend().LeafInuit("1004", "2", KickLineRancher.FarBefriend().FarDense().ToString(), MoldMoss.text);
            HDishGem.gameObject.SetActive(false);
            Urn_HeBadgeGem.gameObject.SetActive(false);
            if (Starve_Cast > 0)
                KickLineRancher.FarBefriend().PegCast(Starve_Cast);
            if (Starve_Note > 0)
                KickLineRancher.FarBefriend().PegNote(Starve_Note);
            SolderCast();
            Freezing_Rancher.Instance.HeBadge();
            FinishSwear.SetActive(false);
            ADRancher.Befriend.IfCoronaPegWaist();
        });
        //失败后 重开游戏
        Soap_HeBadgeGem.onClick.AddListener(() =>
        {
            WhimInuitRemove.FarBefriend().LeafInuit("1005", "2", Freezing_Rancher.Instance.FarPinkPeckHighPinkKeelLad_HutInuit(), MoldMoss.text);
            Freezing_Rancher.Instance.HeBadge();
            FinishSwear.SetActive(false);
            ADRancher.Befriend.IfCoronaPegWaist();
        });

        GradualGem.onClick.AddListener(() =>
        {
            PlusUIMode(nameof(GradualSwear));
        });

        if (UnfoldGate.UpMound())
            NoteRedGem.gameObject.SetActive(false);
        else
        {
            MoldRancher.FarBefriend().Award(1, () =>
            {
                bool IsGuideCashOut = ShedLineRancher.FarGray("IsGuideCashOut");
                if (!IsGuideCashOut)
                {
                    PlusUIMode(nameof(InferSwear)).GetComponent<InferSwear>().BindVerb(NoteRedGem);
                    ShedLineRancher.CudGray("IsGuideCashOut", true);
                }
            });
        }

        bool IsLongScreen = ((float)Screen.height / (float)Screen.width) >= 2f;
        if (IsLongScreen)
        {
            KeelMayGem.transform.localPosition = new Vector2(-150, -835);
            KeelMayGem.transform.localScale = Vector3.one;
            UndoGem.transform.localPosition = new Vector2(150, -835);
            UndoGem.transform.localScale = Vector3.one;
            NoteRedGem.transform.localPosition = new Vector2(0, -1010);
            NoteRedGem.transform.localScale = Vector3.one * .8f;
        }
        else
        {
            KeelMayGem.transform.localPosition = new Vector2(-450, -840);
            KeelMayGem.transform.localScale = Vector3.one * .7f;
            UndoGem.transform.localPosition = new Vector2(-300, -840);
            UndoGem.transform.localScale = Vector3.one * .7f;
            NoteRedGem.transform.localPosition = new Vector2(160, -840);
            NoteRedGem.transform.localScale = Vector3.one * .8f;
        }
    }

    private void Start()
    {
        Freezing_Rancher.Instance.Deaf();
        OfPram_BarbGem.onClick.AddListener(Freezing_Rancher.Instance.OfPram_Back);
        KeelMayGem.onClick.AddListener(BogGenu_May);
        UndoGem.onClick.AddListener(BogGenu_Note);
        GenuBread = ToeBoldLeg.instance._KickLine.prop_price;
        SolderGenu();
        if (!UnfoldGate.UpMound())
            InvokeRepeating(nameof(BindSacAvenue), 0, 60);
    }
    void BindSacAvenue()
    {
        _SacAvenue.Sac();
    }

    #region 提示逻辑
    public void BindKeelMay()
    {
        if (ByAsphaltMay)
        {
            // 如果正在显示提示，先隐藏当前提示
            SendRotVole();
            return;
        }

        // 执行4步提示逻辑
        StartCoroutine(MayHorse());
    }

    private IEnumerator MayHorse()
    {
        ByAsphaltMay = true;

        // 步骤a：检查是否可以移动到上部接龙区
        var tipResult = Freezing_Rancher.Instance.CigarMayBoatA();
        if (tipResult.AgeMay)
        {
            BindMaySteamship(tipResult.CrisisKeel, tipResult.PotatoCitation);
            BindMayAfrican(tipResult.CatAfrican);
            yield break;
        }

        // 步骤b：检查下部接龙区内部连接
        tipResult = Freezing_Rancher.Instance.CigarMayBoatB();
        if (tipResult.AgeMay)
        {
            BindMaySteamship(tipResult.CrisisKeel, tipResult.PotatoCitation);
            BindMayAfrican(tipResult.CatAfrican);
            yield break;
        }

        // 步骤c：检查上部翻牌区是否有可移动的牌
        tipResult = Freezing_Rancher.Instance.CigarMayBoatC();
        if (tipResult.AgeMay)
        {
            BindMaySteamship(tipResult.CrisisKeel, tipResult.PotatoCitation);
            BindMayAfrican(tipResult.CatAfrican);
            yield break;
        }

        // 步骤d：提示翻牌
        tipResult = Freezing_Rancher.Instance.CigarMayBoatD();
        if (tipResult.AgeMay)
        {
            BindMaySteamship(tipResult.CrisisKeel, tipResult.PotatoCitation);
            BindMayAfrican(tipResult.CatAfrican);
            yield break;
        }

        // 没有找到任何提示
        BindMayAfrican("没有找到可用的提示");
        yield return new WaitForSeconds(1.5f);
        SendMayAfrican();
        Debug.Log("没有找到可用的提示");
        ByAsphaltMay = false;
    }

    private void BindMaySteamship(Keel sourceCard, Vector3 targetPosition)
    {
        Vector3 StartPos = Vector3.zero;
        Vector3 EndPos = Vector3.zero;
        // 设置提示动画的起始和结束位置
        if (sourceCard != null)
        {
            StartPos = sourceCard.transform.position;
            EndPos = targetPosition;
            KeelMay.transform.Find("花色").GetComponent<Image>().sprite = Freezing_Rancher.Instance.FarLeapReview(sourceCard.Line);
            KeelMay.transform.Find("Text").GetComponent<Text>().text = Freezing_Rancher.Instance.DodgeVogue[(int)sourceCard.Line.Value];
            KeelMay.transform.Find("Text").GetComponent<Text>().color = sourceCard.Line.UpCod() ? Color.red : Color.black;
            KeelMay.transform.Find("卡背").gameObject.SetActive(false);
        }
        else
        {
            // 对于翻牌提示，使用固定的起始位置
            StartPos = Sum_OfPram_HighPink.position;
            EndPos = targetPosition;
            KeelMay.transform.Find("卡背").gameObject.SetActive(true);
        }

        KeelMay.gameObject.SetActive(true);
        KeelMay.position = StartPos;
        KeelMay.DOKill();
        KeelMay.DOMove(EndPos, 1).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
    }

    public void SendKeelMay()
    {
        KeelMay.DOKill();
        KeelMay.gameObject.SetActive(false);
    }

    // 隐藏所有提示（在玩家操作时调用）
    public void SendRotVole()
    {
        SendKeelMay();
        SendMayAfrican();
        ByAsphaltMay = false;
    }

    private void BindMayAfrican(string message)
    {
        print("提示: " + message);
        if (MayAfricanSwear != null && MayAfricanMoss != null)
        {
            MayAfricanMoss.text = message;
            MayAfricanSwear.SetActive(true);
        }
    }

    private void SendMayAfrican()
    {
        if (MayAfricanSwear != null)
        {
            MayAfricanSwear.SetActive(false);
        }
    }
    #endregion

    #region  道具
    public void SolderGenu() //更新道具
    {
        int Cast = (int)KickLineRancher.FarBefriend().FarCast();

        Genu_MayWaist = KickLineRancher.FarBefriend().FarGenu_May();
        if (Genu_MayWaist > 0)
        {
            KeelMayGem.transform.Find("角标/广告").gameObject.SetActive(false);
            KeelMayGem.transform.Find("角标/金币").gameObject.SetActive(false);
            KeelMayGem.transform.Find("角标/数量Text").gameObject.SetActive(true);
            KeelMayGem.transform.Find("角标/数量Text").GetComponent<Text>().text = Genu_MayWaist.ToString();
        }
        else if (Cast >= GenuBread)
        {
            KeelMayGem.transform.Find("角标/广告").gameObject.SetActive(false);
            KeelMayGem.transform.Find("角标/金币").gameObject.SetActive(true);
            KeelMayGem.transform.Find("角标/金币/Text").GetComponent<Text>().text = GenuBread.ToString();
            KeelMayGem.transform.Find("角标/数量Text").gameObject.SetActive(false);
        }
        else
        {
            KeelMayGem.transform.Find("角标/广告").gameObject.SetActive(true);
            KeelMayGem.transform.Find("角标/金币").gameObject.SetActive(false);
            KeelMayGem.transform.Find("角标/数量Text").gameObject.SetActive(false);
        }

        Genu_NoteWaist = KickLineRancher.FarBefriend().FarGenu_Note();
        if (Genu_NoteWaist > 0)
        {
            UndoGem.transform.Find("角标/广告").gameObject.SetActive(false);
            UndoGem.transform.Find("角标/金币").gameObject.SetActive(false);
            UndoGem.transform.Find("角标/数量Text").gameObject.SetActive(true);
            UndoGem.transform.Find("角标/数量Text").GetComponent<Text>().text = Genu_NoteWaist.ToString();
        }
        else if (Cast >= GenuBread)
        {
            UndoGem.transform.Find("角标/广告").gameObject.SetActive(false);
            UndoGem.transform.Find("角标/金币").gameObject.SetActive(true);
            UndoGem.transform.Find("角标/金币/Text").GetComponent<Text>().text = GenuBread.ToString();
            UndoGem.transform.Find("角标/数量Text").gameObject.SetActive(false);
        }
        else
        {
            UndoGem.transform.Find("角标/广告").gameObject.SetActive(true);
            UndoGem.transform.Find("角标/金币").gameObject.SetActive(false);
            UndoGem.transform.Find("角标/数量Text").gameObject.SetActive(false);
        }
    }

    void BogGenu_May() //使用提示
    {
        Genu_MayWaist = KickLineRancher.FarBefriend().FarGenu_May();
        int Cast = (int)KickLineRancher.FarBefriend().FarCast();
        if (Genu_MayWaist > 0)
        {
            KickLineRancher.FarBefriend().PegGenu_May(-1);
            SolderGenu();
            BindKeelMay();
            WhimInuitRemove.FarBefriend().LeafInuit("1006", "1");
        }
        else if (Cast >= GenuBread)
        {
            KickLineRancher.FarBefriend().PegCast(-GenuBread);
            SolderCast();
            SolderGenu();
            BindKeelMay();
            WhimInuitRemove.FarBefriend().LeafInuit("1006", "2");
        }
        else
        {
            ADRancher.Befriend.LionStarveAlder((ok) =>
            {
                if (ok)
                {
                    BindKeelMay();
                    WhimInuitRemove.FarBefriend().LeafInuit("1006", "3");
                }
            }, "8");
        }
    }
    void BogGenu_Note() //使用撤销
    {
        if (!Freezing_Rancher.Instance.TheNote())
        {
            GroomRancher.FarBefriend().BindGroom("There are no undo operations");
            return;
        }

        Genu_NoteWaist = KickLineRancher.FarBefriend().FarGenu_Note();
        int Cast = (int)KickLineRancher.FarBefriend().FarCast();
        if (Genu_NoteWaist > 0)
        {
            KickLineRancher.FarBefriend().PegGenu_Note(-1);
            SolderGenu();
            Freezing_Rancher.Instance.Note();
            WhimInuitRemove.FarBefriend().LeafInuit("1007", "1");
        }
        else if (Cast >= GenuBread)
        {
            KickLineRancher.FarBefriend().PegCast(-GenuBread);
            SolderCast();
            SolderGenu();
            Freezing_Rancher.Instance.Note();
            WhimInuitRemove.FarBefriend().LeafInuit("1007", "2");
        }
        else
        {
            ADRancher.Befriend.LionStarveAlder((ok) =>
            {
                if (ok)
                {
                    Freezing_Rancher.Instance.Note();
                    WhimInuitRemove.FarBefriend().LeafInuit("1007", "3");
                }
            }, "9");
        }
    }
    #endregion

    public void BindVerb()
    {
        if (Verb != null) Verb.SetActive(true);
    }
    public void SendVerb()
    {
        if (Verb != null) Verb.SetActive(false);
    }

    public void PegDense(int score, Vector3 Pos = default(Vector3)) // 更新分数
    {
        int oldScore = KickLineRancher.FarBefriend().FarDense();
        int newScore = Mathf.Max(0, oldScore + score);
        KickLineRancher.FarBefriend().CudDense(newScore);
        SteamshipRevolution.UranusFormal(oldScore, newScore, 0.1f, DenseMoss, null);
        if (score > 0)
        {
            if (Pos == default(Vector3))
                Pos = DenseMoss.transform.position;
            PegDenseAfar.GetComponentInChildren<Text>().text = score.ToString();
            SteamshipRevolution.PegDense(Pos);
        }
    }
    public void HeavyDense() // 清空分数
    {
        KickLineRancher.FarBefriend().CudDense(0);
        DenseMoss.text = "0";
    }

    public void SolderCast() // 更新金币
    {
        SteamshipRevolution.UranusFormal(float.Parse(CastMoss.text, System.Globalization.CultureInfo.CurrentCulture), KickLineRancher.FarBefriend().FarCast(), 0.1f, CastMoss, null);

        //金币变化影响道具角标
        SolderGenu();
    }

    public void SolderNote() // 更新现金
    {
        SteamshipRevolution.UranusFormal(float.Parse(NoteMoss.text, System.Globalization.CultureInfo.CurrentCulture), KickLineRancher.FarBefriend().FarNote(), 0.1f, NoteMoss, null);
    }

    public void SolderSlumWaist(int count)  // 更新操作次数
    {
        if (SlumWaistMoss != null)
        {
            SlumWaistMoss.text = count.ToString();
        }
    }

    public void SolderKickMold(float timeInSeconds) // 更新游戏时间显示
    {
        if (MoldMoss != null)
        {
            int minutes = Mathf.FloorToInt(timeInSeconds / 60);
            int seconds = Mathf.FloorToInt(timeInSeconds % 60);
            MoldMoss.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void Jobber(bool IsWin)
    {
        FinishSwear.gameObject.SetActive(true);
        int[] star_score = ToeBoldLeg.instance._KickLine.star_score; //星级分数
        int[] star_reward_Gold = ToeBoldLeg.instance._KickLine.star_reward_gold; //星级奖励
        int[] star_reward_Cash = ToeBoldLeg.instance._KickLine.star_reward_cash; //星级奖励
        if (IsWin)
        {
            PegDense(ToeBoldLeg.instance._KickLine.levelcomplete_reward_count, Vector3.zero);
            UrnSwear.SetActive(true);
            SoapSwear.SetActive(false);
            int StarCount = 0;
            Starve_Cast = 0;
            StarveMoss_Cast.transform.parent.gameObject.SetActive(false);
            Starve_Note = 0;
            StarveMoss_Note.transform.parent.gameObject.SetActive(false);
            // 动画开始----------------------
            // 星星的轨迹list
            List<Transform> StarList = new List<Transform>(Taint);
            List<Transform> StarStartPosList = new List<Transform>(TaintBadgeTF);
            // 奖励按钮依次展示
            SteamshipRevolution.UrnSwearBind(BindTrip);
            SkeletonGraphic ItemSpine = UrnDepot.GetComponent<SkeletonGraphic>();
            // 重置骨骼到初始姿态
            ItemSpine.Skeleton.SetToSetupPose();
            ItemSpine.AnimationState.ClearTracks();
            // 强制立即更新状态（重要！）
            ItemSpine.Update(0);
            // 过关spine展示
            UrnDepot.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(1, "ShowAni", false);
            Me_Bind.SetActive(true);
            if (KickLineRancher.FarBefriend().FarDense() >= star_score[2])
            {
                SteamshipRevolution.UrnFord(3, StarList, StarStartPosList, StarList, GermGuinea, UrnSwear.transform);
                StarCount = 3;
                if (star_reward_Gold != null && star_reward_Gold.Length > 0)
                    Starve_Cast = star_reward_Gold[2];
                if (star_reward_Cash != null && star_reward_Cash.Length > 0)
                    Starve_Note = star_reward_Cash[2];
            }
            else if (KickLineRancher.FarBefriend().FarDense() >= star_score[1])
            {
                SteamshipRevolution.UrnFord(2, StarList, StarStartPosList, StarList, GermGuinea, UrnSwear.transform);
                StarCount = 2;
                if (star_reward_Gold != null && star_reward_Gold.Length > 0)
                    Starve_Cast = star_reward_Gold[1];
                if (star_reward_Cash != null && star_reward_Cash.Length > 0)
                    Starve_Note = star_reward_Cash[1];
            }
            else
            {
                SteamshipRevolution.UrnFord(1, StarList, StarStartPosList, StarList, GermGuinea, UrnSwear.transform);
                StarCount = 1;
                if (star_reward_Gold != null && star_reward_Gold.Length > 0)
                    Starve_Cast = star_reward_Gold[0];
                if (star_reward_Cash != null && star_reward_Cash.Length > 0)
                    Starve_Note = star_reward_Cash[0];
            }
            if (UnfoldGate.UpMound())
                Starve_Note = 0;
            HDishPerry.RateMedal();
            HDishGem.gameObject.SetActive(true);
            Urn_HeBadgeGem.gameObject.SetActive(false);
            if (Starve_Cast > 0)
            {
                StarveMoss_Cast.transform.parent.gameObject.SetActive(true);
                SteamshipRevolution.UranusFormal(0, Starve_Cast, 0.1f, StarveMoss_Cast, null);
            }
            if (Starve_Note > 0)
            {
                StarveMoss_Note.transform.parent.gameObject.SetActive(true);
                SteamshipRevolution.UranusFormal(0, Starve_Note, 0.1f, StarveMoss_Note, null);
            }
            AwardBindFarGem = MoldRancher.FarBefriend().Award(2, () =>
              {
                  Urn_HeBadgeGem.gameObject.SetActive(true);
                  Urn_HeBadgeGem.transform.localScale = new Vector3(0, 0, 0);
                  Urn_HeBadgeGem.transform.DOScale(1, 0.2f).SetEase(Ease.OutBack);
              });
            LeafyLeg.FarBefriend().HaulGuinea(LeafyJoke.UIMusic.SFX_EndSuccessGame);

            //第一局游戏胜利
            if (!UnfoldGate.UpMound() && !PlayerPrefs.HasKey("WaitRateUsBool"))
                ShedLineRancher.CudGray("WaitRateUs", true);

            //真提现任务统计
            CashOutManager.FarBefriend().AddTaskValue("WinCount", 1);
        }
        else
        {
            UrnSwear.SetActive(false);
            SoapSwear.SetActive(true);
            Soap_HeBadgeGem.gameObject.SetActive(false);
            MoldRancher.FarBefriend().Award(2.5f, () => { Soap_HeBadgeGem.gameObject.SetActive(true); });
            //SteamshipRevolution.PlusSwear(FinishSwear.GetComponent<Image>(), SoapSwear.transform);
            LeafyLeg.FarBefriend().HaulGuinea(LeafyJoke.UIMusic.SFX_EedLostGame);

            LoseSpine.Skeleton.SetToSetupPose();
            LoseSpine.AnimationState.ClearTracks();
            LoseSpine.Update(0);
            LoseSpine.AnimationState.SetAnimation(0, "ShowAni", false);
            SteamshipRevolution.UrnSwearBind(LoseShowList);
        }
    }

    #region  slot

    void HaulHDish() //横向slot翻倍
    {
        ADRancher.Befriend.LionStarveAlder((ok) =>
        {
            if (ok)
            {
                MoldRancher.FarBefriend().FloeAward(AwardBindFarGem);
                HDishGem.gameObject.SetActive(false);
                Urn_HeBadgeGem.gameObject.SetActive(false);
                HDishPerry.None(0, (multi) =>
                {
                    WhimInuitRemove.FarBefriend().LeafInuit("1004", "1", KickLineRancher.FarBefriend().FarDense().ToString(), MoldMoss.text);
                    if (Starve_Cast > 0)
                    {
                        float OldReward = Starve_Cast;
                        Starve_Cast = Starve_Cast * multi;
                        KickLineRancher.FarBefriend().PegCast(Starve_Cast);
                        SteamshipRevolution.UranusFormal(OldReward, Starve_Cast, 0.1f, StarveMoss_Cast, null);
                    }
                    if (Starve_Note > 0)
                    {
                        float OldReward = Starve_Note;
                        Starve_Note = Starve_Note * multi;
                        KickLineRancher.FarBefriend().PegNote(Starve_Note);
                        SteamshipRevolution.UranusFormal(OldReward, Starve_Note, 0.1f, StarveMoss_Note, null);
                    }
                    MoldRancher.FarBefriend().Award(1, () =>
                    {
                        SolderCast();
                        SolderNote();
                        Freezing_Rancher.Instance.HeBadge();
                        FinishSwear.SetActive(false);
                    });
                });
            }
        }, "1");
    }

    public void HaulHiccupDish() //小slot玩法
    {
        _HiccupDish.gameObject.SetActive(true);
        _HiccupDish.Show();
        //SteamshipRevolution.PlusSwear(_HiccupDish.GetComponent<Image>(), _HiccupDish.transform.GetChild(0));
        MoldRancher.FarBefriend().Award(1f, () =>
        {
            _HiccupDish.Slot();
            LeafyLeg.FarBefriend().HaulGuinea(LeafyJoke.UIMusic.SFX_Slots);
            WhimInuitRemove.FarBefriend().LeafInuit("1014", SlumWaistMoss.text);
        });
    }

    #endregion

    public void BindOfPeckStarveGuinea(int index, RewardType type)
    {
        OfPeck_StarveGuinea[index].Play();
        if (type == RewardType.Gold)
            SteamshipRevolution.FreezingFarCast(CastPart, 3, UIRancher.FarBefriend()._Cup, OfPeck_StarveGuinea[index].transform.position, CastPart.transform.position, null);
        else if (type == RewardType.Cash)
            SteamshipRevolution.FreezingFarCast(NotePart, 3, UIRancher.FarBefriend()._Cup, OfPeck_StarveGuinea[index].transform.position, NotePart.transform.position, null);
    }
    public void BindOfPramStarveGuinea(RewardType type)
    {
        OfPram_StarveGuinea.Play();
        if (type == RewardType.Gold)
            SteamshipRevolution.FreezingFarCast(CastPart, 3, UIRancher.FarBefriend()._Cup, OfPram_StarveGuinea.transform.position, CastPart.transform.position, null);
        else if (type == RewardType.Cash)
            SteamshipRevolution.FreezingFarCast(NotePart, 3, UIRancher.FarBefriend()._Cup, OfPram_StarveGuinea.transform.position, NotePart.transform.position, null);
    }

    public void CudAwaitFaithGene()
    {
        AwaitFaith_Gene.fillAmount = Freezing_Rancher.Instance.AwaitFaithWaist / 4f;
    }

    public void CarveNeonGuinea()
    {

    }
}

// 提示结果结构
public struct TipResult
{
    public bool AgeMay;
    public Keel CrisisKeel;
    public Vector3 PotatoCitation;
    public string CatAfrican;

    public TipResult(bool hasTip, Keel sourceCard, Vector3 targetPosition, string tipMessage = "")
    {
        this.AgeMay = hasTip;
        this.CrisisKeel = sourceCard;
        this.PotatoCitation = targetPosition;
        this.CatAfrican = tipMessage;
    }
}
