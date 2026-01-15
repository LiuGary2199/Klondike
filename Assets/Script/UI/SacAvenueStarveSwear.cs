using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;

/// <summary> 飞行气泡奖励 </summary>
public class SacAvenueStarveSwear : SnowUIPlace
{
[UnityEngine.Serialization.FormerlySerializedAs("Gold")]    public GameObject Cast;
[UnityEngine.Serialization.FormerlySerializedAs("GoldText")]    public Text CastMoss;
    float CastStarve;
[UnityEngine.Serialization.FormerlySerializedAs("Cash")]    public GameObject Note;
[UnityEngine.Serialization.FormerlySerializedAs("CashText")]    public Text NoteMoss;
    float NoteStarve;
[UnityEngine.Serialization.FormerlySerializedAs("GiveupBtn")]    public Button VioletGem;
[UnityEngine.Serialization.FormerlySerializedAs("GetBtn")]    public Button FarGem;
[UnityEngine.Serialization.FormerlySerializedAs("RewardSpine")]    public GameObject StarveDepot;
[UnityEngine.Serialization.FormerlySerializedAs("RewardShowList")]    public List<GameObject> StarveBindTrip;


    private void Start()
    {
        FarGem.onClick.AddListener(() =>
        {
            ADRancher.Befriend.LionStarveAlder((ok) =>
            {
                if (ok)
                {
                    if (CastStarve > 0)
                    {
                        KickLineRancher.FarBefriend().PegCast(CastStarve);
                        KickSwear.Instance.SolderCast();
                        WhimInuitRemove.FarBefriend().LeafInuit("1017", "1", CastStarve.ToString());
                    }
                    if (NoteStarve > 0)
                    {
                        KickLineRancher.FarBefriend().PegNote(NoteStarve);
                        KickSwear.Instance.SolderNote();
                        WhimInuitRemove.FarBefriend().LeafInuit("1017", "1", NoteStarve.ToString());
                    }
                    FirstUIMode(nameof(SacAvenueStarveSwear));
                }
            }, "");
        });
        VioletGem.onClick.AddListener(() =>
        {
            FirstUIMode(nameof(SacAvenueStarveSwear));
            ADRancher.Befriend.IfCoronaPegWaist();
        });
    }

    public override void Display(object uiFormParams)
    {
        base.Display(uiFormParams);
        CastStarve = 0;
        NoteStarve = 0;
        Cast.SetActive(false);
        Note.SetActive(false);
        RewardData Line= KickLineRancher.FarBefriend().FarStarveLineSoRevertBusDelay(ToeBoldLeg.instance._KickLine.flybubble_data_list);
        if (Line.type == RewardType.Gold)
        {
            CastStarve = Line.num;
            CastMoss.text = CastStarve.ToString();
            Cast.SetActive(true);
        }
        else if (Line.type == RewardType.Cash)
        {
            NoteStarve = Line.num;
            NoteMoss.text = NoteStarve.ToString();
            Note.SetActive(true);
        }
    }

    void DeafStarveLug()
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
