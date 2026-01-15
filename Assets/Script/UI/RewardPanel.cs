using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Spine.Unity;
using DG.Tweening;

/// <summary> 通用奖励面板 </summary>
public class RewardPanel : BaseUIForms
{
    public GameObject Gold;
    public Text GoldText;
    float GoldReward;
    public GameObject Cash;
    public Text CashText;
    float CashReward;
    public GameObject Prop_Tip;
    public Text Prop_Tip_Text;
    int Prop_TipReward;
    public GameObject Prop_Undo;
    public Text Prop_Undo_Text;
    int Prop_UndoReward;
    public SlotGroup SlotGroup;
    public Button SlotBtn;
    public Button GetBtn;
    public UnityAction FinishEvent;
    public GameObject RewardSpine;
    public List<GameObject> RewardShowList;
    Coroutine DelayShowGetBtn;
    string IsWatchedAD;
    string EventID;

    public override void Display(object uiFormParams)
    {
        base.Display(uiFormParams);

    }
    void Start()
    {
        SlotBtn.onClick.AddListener(() =>
        {
            ADManager.Instance.playRewardVideo((ok) =>
            {
                if (ok)
                {
                    TimeManager.GetInstance().StopDelay(DelayShowGetBtn);
                    SlotBtn.gameObject.SetActive(false);
                    GetBtn.gameObject.SetActive(false);
                    SlotGroup.slot(0, (multi) =>
                    {
                        if (GoldReward > 0)
                        {
                            float OldGoldReward = GoldReward;
                            GoldReward = GoldReward * multi;
                            AnimationController.ChangeNumber(OldGoldReward, GoldReward, 0.1f, GoldText, null);
                        }
                        if (CashReward > 0)
                        {
                            float OldCashReward = CashReward;
                            CashReward = CashReward * multi;
                            AnimationController.ChangeNumber(OldCashReward, CashReward, 0.1f, CashText, null);
                        }
                        if (Prop_TipReward > 0)
                        {
                            Prop_TipReward = (int)(Prop_TipReward * multi);
                            AnimationController.ChangeNumber(0, Prop_TipReward, 0.1f, Prop_Tip_Text, null);
                        }
                        if (Prop_UndoReward > 0)
                        {
                            Prop_UndoReward = (int)(Prop_UndoReward * multi);
                            AnimationController.ChangeNumber(0, Prop_UndoReward, 0.1f, Prop_Undo_Text, null);
                        }
                        TimeManager.GetInstance().Delay(1.5f, () =>
                        {
                            IsWatchedAD = "1";
                            GetRewardAndSendEventAndClose();
                        });
                    });
                }
            }, GetEventIndex());
        });
        GetBtn.onClick.AddListener(() =>
        {
            IsWatchedAD = "2";
            GetRewardAndSendEventAndClose();
        });
    }

    public void Init(RewardData ColdDate, RewardData CashDate, UnityAction FinishEvent, string EventID = "")
    {
        this.FinishEvent = FinishEvent;
        this.EventID = EventID;
        Prop_TipReward = 0;
        Prop_UndoReward = 0;
        Prop_Tip.SetActive(false);
        Prop_Undo.SetActive(false);
        GoldReward = ColdDate != null ? ColdDate.num : 0;
        CashReward = CashDate != null ? CashDate.num : 0;
        if (CommonUtil.IsApple())
            CashReward = 0;
        Gold.SetActive(false);
        Cash.SetActive(false);
        if (ColdDate != null && ColdDate.num > 0)
        {
            Gold.SetActive(true);
            AnimationController.ChangeNumber(0, ColdDate.num, 0.1f, GoldText, null);
        }
        if (CashDate != null && CashDate.num > 0)
        {
            Cash.SetActive(true);
            AnimationController.ChangeNumber(0, CashDate.num, 0.1f, CashText, null);
        }
        SlotGroup.initMulti();
        SlotBtn.gameObject.SetActive(true);
        GetBtn.gameObject.SetActive(false);
        DelayShowGetBtn = TimeManager.GetInstance().Delay(2, () =>
        {
            GetBtn.gameObject.SetActive(true);
            GetBtn.transform.localScale = new Vector3(0, 0, 0);
            GetBtn.transform.DOScale(1, 0.2f).SetEase(Ease.OutBack);
        });
        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.SFX_BigWin);
    }

    public void Init_Prop(int TipReward, int UndoReward, UnityAction FinishEvent, string EventID = "")
    {
        this.FinishEvent = FinishEvent;
        this.EventID = EventID;
        GoldReward = 0;
        CashReward = 0;
        Prop_TipReward = 0;
        Prop_UndoReward = 0;
        Gold.SetActive(false);
        Cash.SetActive(false);
        Prop_TipReward = TipReward;
        Prop_UndoReward = UndoReward;
        Prop_Tip.SetActive(false);
        Prop_Undo.SetActive(false);
        if (Prop_TipReward > 0)
        {
            Prop_Tip.SetActive(true);
            AnimationController.ChangeNumber(0, Prop_TipReward, 0.1f, Prop_Tip_Text, null);
        }
        if (Prop_UndoReward > 0)
        {
            Prop_Undo.SetActive(true);
            AnimationController.ChangeNumber(0, Prop_UndoReward, 0.1f, Prop_Undo_Text, null);
        }
        SlotGroup.initMulti();
        SlotBtn.gameObject.SetActive(true);
        GetBtn.gameObject.SetActive(false);
        DelayShowGetBtn = TimeManager.GetInstance().Delay(2, () =>
        {
            GetBtn.gameObject.SetActive(true);
            GetBtn.transform.localScale = new Vector3(0, 0, 0);
            GetBtn.transform.DOScale(1, 0.2f).SetEase(Ease.OutBack);
        });
        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.SFX_BigWin);
    }

    void GetRewardAndSendEventAndClose()
    {
        if (GoldReward > 0)
        {
            AnimationController.KlondikeGetGold(GamePanel.Instance.GoldIcon, 7, UIManager.GetInstance()._Top, new Vector2(0, 0), GamePanel.Instance.GoldIcon.transform.position, () => { });
            GameDataManager.GetInstance().AddGold(GoldReward);
            GamePanel.Instance.UpdateGold();
        }
        if (CashReward > 0)
        {
            AnimationController.KlondikeGetGold(GamePanel.Instance.CashIcon, 7, UIManager.GetInstance()._Top, new Vector2(0, 0), GamePanel.Instance.CashIcon.transform.position, () => { });
            GameDataManager.GetInstance().AddCash(CashReward);
            GamePanel.Instance.UpdateCash();
        }
        if (Prop_TipReward > 0)
        {
            GameDataManager.GetInstance().AddProp_Tip(Prop_TipReward);
            GamePanel.Instance.UpdateProp();
        }
        if (Prop_UndoReward > 0)
        {
            GameDataManager.GetInstance().AddProp_Undo(Prop_UndoReward);
            GamePanel.Instance.UpdateProp();
        }
        FinishEvent?.Invoke();
        FinishEvent = null;
        CloseUIForm(nameof(RewardPanel));
        Klondike_Manager.Instance.CheckGameEnd();

        //打点
        if (EventID == "1011") //转化货币
        {
            if (GoldReward > 0)
                PostEventScript.GetInstance().SendEvent(EventID, IsWatchedAD, GoldReward.ToString());
            if (CashReward > 0)
                PostEventScript.GetInstance().SendEvent(EventID, IsWatchedAD, CashReward.ToString());
        }
        else if (EventID == "1012") //刮刮卡
        {
            PostEventScript.GetInstance().SendEvent(EventID, IsWatchedAD, GoldReward.ToString(), CashReward.ToString());
        }
        else if (EventID == "1013") //转盘
        {
            if (GoldReward > 0)
                PostEventScript.GetInstance().SendEvent(EventID, IsWatchedAD, "1", GoldReward.ToString());
            if (CashReward > 0)
                PostEventScript.GetInstance().SendEvent(EventID, IsWatchedAD, "2", CashReward.ToString());
            if (Prop_TipReward > 0)
                PostEventScript.GetInstance().SendEvent(EventID, IsWatchedAD, "3", Prop_TipReward.ToString());
            if (Prop_UndoReward > 0)
                PostEventScript.GetInstance().SendEvent(EventID, IsWatchedAD, "4", Prop_UndoReward.ToString());
        }
    }

    string GetEventIndex()
    {
        if (EventID == "1011") //转化货币
            return "3";
        else if (EventID == "1012") //刮刮卡
            return "4";
        else if (EventID == "1013") //转盘
            return "5";
        else
            return "0";
    }

    public void InitRewardAni()
    {
        // 重置骨骼到初始姿态
        SkeletonGraphic ItemSpine = RewardSpine.GetComponent<SkeletonGraphic>();
        ItemSpine.Skeleton.SetToSetupPose();
        ItemSpine.AnimationState.ClearTracks();
        // 强制立即更新状态（重要！）
        ItemSpine.Update(0);
        ItemSpine.AnimationState.SetAnimation(0, "Reward", false);
        AnimationController.WinPanelShow(RewardShowList);
    }
}
