using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Spine.Unity;
using DG.Tweening;

/// <summary> 通用奖励面板 </summary>
public class StarveSwear : SnowUIPlace
{
[UnityEngine.Serialization.FormerlySerializedAs("Gold")]    public GameObject Cast;
[UnityEngine.Serialization.FormerlySerializedAs("GoldText")]    public Text CastMoss;
    float CastStarve;
[UnityEngine.Serialization.FormerlySerializedAs("Cash")]    public GameObject Note;
[UnityEngine.Serialization.FormerlySerializedAs("CashText")]    public Text NoteMoss;
    float NoteStarve;
[UnityEngine.Serialization.FormerlySerializedAs("Prop_Tip")]    public GameObject Genu_May;
[UnityEngine.Serialization.FormerlySerializedAs("Prop_Tip_Text")]    public Text Genu_May_Moss;
    int Genu_MayStarve;
[UnityEngine.Serialization.FormerlySerializedAs("Prop_Undo")]    public GameObject Genu_Note;
[UnityEngine.Serialization.FormerlySerializedAs("Prop_Undo_Text")]    public Text Genu_Note_Moss;
    int Genu_NoteStarve;
[UnityEngine.Serialization.FormerlySerializedAs("SlotGroup")]    public DishPerry DishPerry;
[UnityEngine.Serialization.FormerlySerializedAs("SlotBtn")]    public Button DishGem;
[UnityEngine.Serialization.FormerlySerializedAs("GetBtn")]    public Button FarGem;
[UnityEngine.Serialization.FormerlySerializedAs("FinishEvent")]    public UnityAction JobberInuit;
[UnityEngine.Serialization.FormerlySerializedAs("RewardSpine")]    public GameObject StarveDepot;
[UnityEngine.Serialization.FormerlySerializedAs("RewardShowList")]    public List<GameObject> StarveBindTrip;
    Coroutine AwardBindFarGem;
    string UpSunriseAD;
    string InuitID;

    public override void Display(object uiFormParams)
    {
        base.Display(uiFormParams);

    }
    void Start()
    {
        DishGem.onClick.AddListener(() =>
        {
            ADRancher.Befriend.LionStarveAlder((ok) =>
            {
                if (ok)
                {
                    MoldRancher.FarBefriend().FloeAward(AwardBindFarGem);
                    DishGem.gameObject.SetActive(false);
                    FarGem.gameObject.SetActive(false);
                    DishPerry.None(0, (multi) =>
                    {
                        if (CastStarve > 0)
                        {
                            float OldGoldReward = CastStarve;
                            CastStarve = CastStarve * multi;
                            SteamshipRevolution.UranusFormal(OldGoldReward, CastStarve, 0.1f, CastMoss, null);
                        }
                        if (NoteStarve > 0)
                        {
                            float OldCashReward = NoteStarve;
                            NoteStarve = NoteStarve * multi;
                            SteamshipRevolution.UranusFormal(OldCashReward, NoteStarve, 0.1f, NoteMoss, null);
                        }
                        if (Genu_MayStarve > 0)
                        {
                            Genu_MayStarve = (int)(Genu_MayStarve * multi);
                            SteamshipRevolution.UranusFormal(0, Genu_MayStarve, 0.1f, Genu_May_Moss, null);
                        }
                        if (Genu_NoteStarve > 0)
                        {
                            Genu_NoteStarve = (int)(Genu_NoteStarve * multi);
                            SteamshipRevolution.UranusFormal(0, Genu_NoteStarve, 0.1f, Genu_Note_Moss, null);
                        }
                        MoldRancher.FarBefriend().Award(1.5f, () =>
                        {
                            UpSunriseAD = "1";
                            FarStarveBusLeafInuitBusFirst();
                        });
                    });
                }
            }, FarInuitDodge());
        });
        FarGem.onClick.AddListener(() =>
        {
            UpSunriseAD = "2";
            FarStarveBusLeafInuitBusFirst();
            ADRancher.Befriend.IfCoronaPegWaist();
        });
    }

    public void Deaf(RewardData ColdDate, RewardData CashDate, UnityAction FinishEvent, string EventID = "")
    {
        this.JobberInuit = FinishEvent;
        this.InuitID = EventID;
        Genu_MayStarve = 0;
        Genu_NoteStarve = 0;
        Genu_May.SetActive(false);
        Genu_Note.SetActive(false);
        CastStarve = ColdDate != null ? ColdDate.num : 0;
        NoteStarve = CashDate != null ? CashDate.num : 0;
        if (UnfoldGate.UpMound())
            NoteStarve = 0;
        Cast.SetActive(false);
        Note.SetActive(false);
        if (ColdDate != null && ColdDate.num > 0)
        {
            Cast.SetActive(true);
            SteamshipRevolution.UranusFormal(0, ColdDate.num, 0.1f, CastMoss, null);
        }
        if (CashDate != null && CashDate.num > 0)
        {
            Note.SetActive(true);
            SteamshipRevolution.UranusFormal(0, CashDate.num, 0.1f, NoteMoss, null);
        }
        DishPerry.RateMedal();
        DishGem.gameObject.SetActive(true);
        FarGem.gameObject.SetActive(false);
        AwardBindFarGem = MoldRancher.FarBefriend().Award(2, () =>
        {
            FarGem.gameObject.SetActive(true);
            FarGem.transform.localScale = new Vector3(0, 0, 0);
            FarGem.transform.DOScale(1, 0.2f).SetEase(Ease.OutBack);
        });
        LeafyLeg.FarBefriend().HaulGuinea(LeafyJoke.UIMusic.SFX_BigWin);
    }

    public void Deaf_Genu(int TipReward, int UndoReward, UnityAction FinishEvent, string EventID = "")
    {
        this.JobberInuit = FinishEvent;
        this.InuitID = EventID;
        CastStarve = 0;
        NoteStarve = 0;
        Genu_MayStarve = 0;
        Genu_NoteStarve = 0;
        Cast.SetActive(false);
        Note.SetActive(false);
        Genu_MayStarve = TipReward;
        Genu_NoteStarve = UndoReward;
        Genu_May.SetActive(false);
        Genu_Note.SetActive(false);
        if (Genu_MayStarve > 0)
        {
            Genu_May.SetActive(true);
            SteamshipRevolution.UranusFormal(0, Genu_MayStarve, 0.1f, Genu_May_Moss, null);
        }
        if (Genu_NoteStarve > 0)
        {
            Genu_Note.SetActive(true);
            SteamshipRevolution.UranusFormal(0, Genu_NoteStarve, 0.1f, Genu_Note_Moss, null);
        }
        DishPerry.RateMedal();
        DishGem.gameObject.SetActive(true);
        FarGem.gameObject.SetActive(false);
        AwardBindFarGem = MoldRancher.FarBefriend().Award(2, () =>
        {
            FarGem.gameObject.SetActive(true);
            FarGem.transform.localScale = new Vector3(0, 0, 0);
            FarGem.transform.DOScale(1, 0.2f).SetEase(Ease.OutBack);
        });
        LeafyLeg.FarBefriend().HaulGuinea(LeafyJoke.UIMusic.SFX_BigWin);
    }

    void FarStarveBusLeafInuitBusFirst()
    {
        if (CastStarve > 0)
        {
            SteamshipRevolution.FreezingFarCast(KickSwear.Instance.CastPart, 7, UIRancher.FarBefriend()._Cup, new Vector2(0, 0), KickSwear.Instance.CastPart.transform.position, () => { });
            KickLineRancher.FarBefriend().PegCast(CastStarve);
            KickSwear.Instance.SolderCast();
        }
        if (NoteStarve > 0)
        {
            SteamshipRevolution.FreezingFarCast(KickSwear.Instance.NotePart, 7, UIRancher.FarBefriend()._Cup, new Vector2(0, 0), KickSwear.Instance.NotePart.transform.position, () => { });
            KickLineRancher.FarBefriend().PegNote(NoteStarve);
            KickSwear.Instance.SolderNote();
        }
        if (Genu_MayStarve > 0)
        {
            KickLineRancher.FarBefriend().PegGenu_May(Genu_MayStarve);
            KickSwear.Instance.SolderGenu();
        }
        if (Genu_NoteStarve > 0)
        {
            KickLineRancher.FarBefriend().PegGenu_Note(Genu_NoteStarve);
            KickSwear.Instance.SolderGenu();
        }
        JobberInuit?.Invoke();
        JobberInuit = null;
        FirstUIMode(nameof(StarveSwear));
        MoldRancher.FarBefriend().Award(1, () => { Freezing_Rancher.Instance.CigarKickRid(true); });

        //打点
        if (InuitID == "1011") //转化货币
        {
            if (CastStarve > 0)
                WhimInuitRemove.FarBefriend().LeafInuit(InuitID, UpSunriseAD, CastStarve.ToString());
            if (NoteStarve > 0)
                WhimInuitRemove.FarBefriend().LeafInuit(InuitID, UpSunriseAD, NoteStarve.ToString());
        }
        else if (InuitID == "1012") //刮刮卡
        {
            WhimInuitRemove.FarBefriend().LeafInuit(InuitID, UpSunriseAD, CastStarve.ToString(), NoteStarve.ToString());
        }
        else if (InuitID == "1013") //转盘
        {
            if (CastStarve > 0)
                WhimInuitRemove.FarBefriend().LeafInuit(InuitID, UpSunriseAD, "1", CastStarve.ToString());
            if (NoteStarve > 0)
                WhimInuitRemove.FarBefriend().LeafInuit(InuitID, UpSunriseAD, "2", NoteStarve.ToString());
            if (Genu_MayStarve > 0)
                WhimInuitRemove.FarBefriend().LeafInuit(InuitID, UpSunriseAD, "3", Genu_MayStarve.ToString());
            if (Genu_NoteStarve > 0)
                WhimInuitRemove.FarBefriend().LeafInuit(InuitID, UpSunriseAD, "4", Genu_NoteStarve.ToString());
        }
    }

    string FarInuitDodge()
    {
        if (InuitID == "1011") //转化货币
            return "3";
        else if (InuitID == "1012") //刮刮卡
            return "4";
        else if (InuitID == "1013") //转盘
            return "5";
        else
            return "0";
    }

    public void DeafStarveLug()
    {
        // 重置骨骼到初始姿态
        SkeletonGraphic ItemSpine = StarveDepot.GetComponent<SkeletonGraphic>();
        ItemSpine.Skeleton.SetToSetupPose();
        ItemSpine.AnimationState.ClearTracks();
        // 强制立即更新状态（重要！）
        ItemSpine.Update(0);
        ItemSpine.AnimationState.SetAnimation(0, "Reward", false);
        SteamshipRevolution.UrnSwearBind(StarveBindTrip);
    }
}
