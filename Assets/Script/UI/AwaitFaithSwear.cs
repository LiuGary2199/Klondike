using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Security.Cryptography;

/// <summary> 转盘 </summary>
public class AwaitFaithSwear : SnowUIPlace
{
[UnityEngine.Serialization.FormerlySerializedAs("SpinBtn")]    public Button PrayGem;
[UnityEngine.Serialization.FormerlySerializedAs("BigWheel")]    public Transform NotFaith;
[UnityEngine.Serialization.FormerlySerializedAs("BigWheelItem")]    public Transform[] NotFaithAfar;
[UnityEngine.Serialization.FormerlySerializedAs("SmallWheel")]    public Transform WorryFaith;
[UnityEngine.Serialization.FormerlySerializedAs("SmallWheelText")]    public Text[] WorryFaithMoss;
[UnityEngine.Serialization.FormerlySerializedAs("Pointer")]    public Transform Rooster;
[UnityEngine.Serialization.FormerlySerializedAs("WheelItem")]    public GameObject FaithAfar;
[UnityEngine.Serialization.FormerlySerializedAs("ShowList")]    public List<GameObject> BindTrip;
[UnityEngine.Serialization.FormerlySerializedAs("WheelEffect")]    public GameObject FaithGuinea;
[UnityEngine.Serialization.FormerlySerializedAs("WheelEffect_1")]    public GameObject FaithGuinea_1;
[UnityEngine.Serialization.FormerlySerializedAs("WheelCurve")]    public AnimationCurve FaithTepee;
    List<RewardData> StarveLineTrip= new List<RewardData>();
    List<float> MedalTrip= new List<float>();
    RewardData UrnLine;
    int NotDodge;
    int WorryDodge;
    float Medal;


    private void Start()
    {
        PrayGem.onClick.AddListener(Need);
    }

    public override void Display(object uiFormParams)
    {
        base.Display(uiFormParams);
        PrayGem.gameObject.SetActive(true);
        StarveLineTrip.Clear();
        MedalTrip.Clear();
        //大转盘
        List<SlotItem> BigWheelMultiList = new List<SlotItem>();
        for (int i = 0; i < 12; i++)
        {
            SlotItem Seal= new SlotItem();
            Seal.weight = ToeBoldLeg.instance._KickLine.wheel_reward_weight_group[i].weight;
            Seal.multi = ToeBoldLeg.instance._KickLine.wheel_reward_weight_group[i].num;
            BigWheelMultiList.Add(Seal);
        }
        NotDodge = KickLineRancher.FarBefriend().FarDodgeSoRevert(BigWheelMultiList);
        for (int i = 0; i < 12; i++)
        {
            //原始数据
            RewardData Line= ToeBoldLeg.instance._KickLine.wheel_reward_weight_group[i];
            //对奖励数值进行数量随机
            List<RewardData> DataList = new List<RewardData>() { Line };
            Line = KickLineRancher.FarBefriend().FarStarveLineSoRevertBusDelay(DataList);
            StarveLineTrip.Add(Line);
        }
        UrnLine = StarveLineTrip[NotDodge];
        for (int i = 0; i < 12; i++)
        {
            NotFaithAfar[i].Find("Text").GetComponent<Text>().text = StarveLineTrip[i].num.ToString();
            NotFaithAfar[i].Find("金币").gameObject.SetActive(StarveLineTrip[i].type == RewardType.Gold);
            NotFaithAfar[i].Find("钻石").gameObject.SetActive(StarveLineTrip[i].type == RewardType.Cash);
            NotFaithAfar[i].Find("提示").gameObject.SetActive(StarveLineTrip[i].type == RewardType.Tip);
            NotFaithAfar[i].Find("撤销").gameObject.SetActive(StarveLineTrip[i].type == RewardType.Undo);
        }
        //小转盘
        WorryDodge = 0;
        if (UrnLine.type == RewardType.Gold)
        {
            WorryDodge = KickLineRancher.FarBefriend().FarDodgeSoRevert(ToeBoldLeg.instance._KickLine.wheel_reward_multi.gold);
            Medal = (float)ToeBoldLeg.instance._KickLine.wheel_reward_multi.gold[WorryDodge].multi;
            for (int i = 0; i < 4; i++)
                WorryFaithMoss[i].text = "x" + ToeBoldLeg.instance._KickLine.wheel_reward_multi.gold[i].multi.ToString();
        }
        else if (UrnLine.type == RewardType.Cash)
        {
            WorryDodge = KickLineRancher.FarBefriend().FarDodgeSoRevert(ToeBoldLeg.instance._KickLine.wheel_reward_multi.cash);
            Medal = (float)ToeBoldLeg.instance._KickLine.wheel_reward_multi.cash[WorryDodge].multi;
            for (int i = 0; i < 4; i++)
                WorryFaithMoss[i].text = "x" + ToeBoldLeg.instance._KickLine.wheel_reward_multi.cash[i].multi.ToString();
        }
        else if (UrnLine.type == RewardType.Tip)
        {
            WorryDodge = KickLineRancher.FarBefriend().FarDodgeSoRevert(ToeBoldLeg.instance._KickLine.wheel_reward_multi.tip);
            Medal = (float)ToeBoldLeg.instance._KickLine.wheel_reward_multi.tip[WorryDodge].multi;
            for (int i = 0; i < 4; i++)
                WorryFaithMoss[i].text = "x" + ToeBoldLeg.instance._KickLine.wheel_reward_multi.tip[i].multi.ToString();
        }
        else if (UrnLine.type == RewardType.Undo)
        {
            WorryDodge = KickLineRancher.FarBefriend().FarDodgeSoRevert(ToeBoldLeg.instance._KickLine.wheel_reward_multi.undo);
            Medal = (float)ToeBoldLeg.instance._KickLine.wheel_reward_multi.undo[WorryDodge].multi;
            for (int i = 0; i < 4; i++)
                WorryFaithMoss[i].text = "x" + ToeBoldLeg.instance._KickLine.wheel_reward_multi.undo[i].multi.ToString();
        }
        //最终奖励
        UrnLine.num *= Medal;
        LeafyLeg.FarBefriend().HaulGuinea(LeafyJoke.UIMusic.SFX_LuckyWheelOpen);
        print("奖励类型:" + UrnLine.type + "  , 奖励倍率:" + Medal + "  , 奖励数量:" + UrnLine.num);

        AwaitFaithBindLug();
    }

    public void Need()
    {
        StartCoroutine(StingerSteamship());
        NotFaith.DORotate(new Vector3(0, 0, 360 * 3 - (360 / 12f) * NotDodge), 2f, RotateMode.FastBeyond360).SetDelay(0.2f).SetEase(FaithTepee).OnComplete(() =>
        {
            LeafyLeg.FarBefriend().HaulGuinea(LeafyJoke.UIMusic.SFX_LuckyWheelStop);
        });
        WorryFaith.DORotate(new Vector3(0, 0, -360 * 3 - (360 / 4f) * WorryDodge), 3f, RotateMode.FastBeyond360).SetDelay(0.2f).SetEase(FaithTepee).OnComplete(() =>
        {
            LeafyLeg.FarBefriend().HaulGuinea(LeafyJoke.UIMusic.SFX_LuckyWheelStop);
            FaithGuinea.SetActive(true);
            FaithGuinea_1.SetActive(true);
        });
        PrayGem.gameObject.SetActive(false);
        MoldRancher.FarBefriend().Award(4.5f, () =>
        {
            if (UrnLine.type == RewardType.Gold)
                PlusUIMode(nameof(StarveSwear)).GetComponent<StarveSwear>().Deaf(UrnLine, null, null, "1013");
            else if (UrnLine.type == RewardType.Cash)
                PlusUIMode(nameof(StarveSwear)).GetComponent<StarveSwear>().Deaf(null, UrnLine, null, "1013");
            else if (UrnLine.type == RewardType.Tip)
                PlusUIMode(nameof(StarveSwear)).GetComponent<StarveSwear>().Deaf_Genu((int)UrnLine.num, 0, null, "1013");
            else if (UrnLine.type == RewardType.Undo)
                PlusUIMode(nameof(StarveSwear)).GetComponent<StarveSwear>().Deaf_Genu(0, (int)UrnLine.num, null, "1013");
            FirstUIMode(nameof(AwaitFaithSwear));
        });
        LeafyLeg.FarBefriend().HaulGuinea(LeafyJoke.UIMusic.SFX_LuckyWheelRolling);
    }
    IEnumerator StingerSteamship()
    {
        yield return new WaitForSeconds(0.2f);
        int LoopCount = 24;
        Sequence seq = DOTween.Sequence();
        seq.Append(Rooster.transform.DOLocalRotate(new Vector3(0, 0, -20 + Random.Range(-2f, 2f)), 6f / LoopCount * 0.3f)
            .SetEase(Ease.Linear));
        //seq.Append(Pointer.transform.DOLocalRotate(new Vector3(0, 0, 0), 2f / 15 * 0.7f).SetEase(Ease.Linear));
        seq.AppendCallback(() =>
        {
            //LeafyLeg.GetInstance().PlayEffect(LeafyJoke.UIMusic.wheel_rotate);
            LeafyLeg.FarBefriend().HaulBalance(Lofelt.NiceVibrations.HapticPatterns.PresetType.SoftImpact);
        });
        seq.SetLoops(LoopCount, LoopType.Yoyo);
        seq.SetEase(Ease.InOutSine);
        seq.Play();
    }

    public void AwaitFaithBindLug()
    {
        for (int i = 0; i < BindTrip.Count; i++)
        {
            GameObject Seal= BindTrip[i].gameObject;
            Seal.transform.localScale = new Vector3(0, 0, 0);
        }

        float DelayTime = 0.2f;
        Vector3 EndPos = new Vector3(0, 0, 0);
        FaithAfar.GetComponent<CanvasGroup>().alpha = 0;
        FaithAfar.transform.position = new Vector3(0, EndPos.y + 5, 0);

        FaithAfar.GetComponent<CanvasGroup>().DOFade(1, 0.3f);
        FaithAfar.transform.DOMoveY(EndPos.y, 0.3f).SetEase(Ease.OutBack);
        for (int i = 0; i < BindTrip.Count; i++)
        {
            GameObject Seal= BindTrip[i].gameObject;
            Seal.transform.localScale = new Vector3(0, 0, 0);
            Seal.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack).SetDelay(DelayTime).OnComplete(() =>
            {

            });
            DelayTime += 0.1f;
        }
    }
}
