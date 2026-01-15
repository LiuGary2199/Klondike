using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Security.Cryptography;

/// <summary> 转盘 </summary>
public class LuckyWheelPanel : BaseUIForms
{
    public Button SpinBtn;
    public Transform BigWheel;
    public Transform[] BigWheelItem;
    public Transform SmallWheel;
    public Text[] SmallWheelText;
    public Transform Pointer;
    public GameObject WheelItem;
    public List<GameObject> ShowList;
    public GameObject WheelEffect;
    public GameObject WheelEffect_1;
    public AnimationCurve WheelCurve;
    List<RewardData> RewardDataList = new List<RewardData>();
    List<float> MultiList = new List<float>();
    RewardData WinData;
    int BigIndex;
    int SmallIndex;
    float Multi;


    private void Start()
    {
        SpinBtn.onClick.AddListener(spin);
    }

    public override void Display(object uiFormParams)
    {
        base.Display(uiFormParams);
        SpinBtn.gameObject.SetActive(true);
        RewardDataList.Clear();
        MultiList.Clear();
        BigIndex = Random.Range(0, 12);
        SmallIndex = Random.Range(0, 4);
        // if (!SaveDataManager.GetBool("notFirstWheel"))
        // {
        //     SaveDataManager.SetBool("notFirstWheel", true);
        //     bigIndex = 2;
        //     smallIndex = 4;
        //     rewardData = rewardDataList[bigIndex];
        // }
        for (int i = 0; i < 12; i++)
        {
            //原始数据
            RewardData Data = NetInfoMgr.instance._GameData.wheel_reward_weight_group[i];
            //对奖励数值进行数量随机
            List<RewardData> DataList = new List<RewardData>() { Data };
            Data = GameDataManager.GetInstance().GetRewardDataByWeightAndRange(DataList);
            RewardDataList.Add(Data);
        }
        WinData = RewardDataList[BigIndex];
        List<SlotItem> Multis = null;
        if (WinData.type == RewardType.Gold)
            Multis = NetInfoMgr.instance._GameData.wheel_reward_multi.gold;
        else if (WinData.type == RewardType.Cash)
            Multis = NetInfoMgr.instance._GameData.wheel_reward_multi.cash;
        else if (WinData.type == RewardType.Tip)
            Multis = NetInfoMgr.instance._GameData.wheel_reward_multi.tip;
        else if (WinData.type == RewardType.Undo)
            Multis = NetInfoMgr.instance._GameData.wheel_reward_multi.undo;
        for (int i = 0; i < 4; i++)
            MultiList.Add((float)Multis[i].multi);
        for (int i = 0; i < 12; i++)
        {
            BigWheelItem[i].Find("Text").GetComponent<Text>().text = RewardDataList[i].num.ToString();
            BigWheelItem[i].Find("金币").gameObject.SetActive(RewardDataList[i].type == RewardType.Gold);
            BigWheelItem[i].Find("钻石").gameObject.SetActive(RewardDataList[i].type == RewardType.Cash);
            BigWheelItem[i].Find("提示").gameObject.SetActive(RewardDataList[i].type == RewardType.Tip);
            BigWheelItem[i].Find("撤销").gameObject.SetActive(RewardDataList[i].type == RewardType.Undo);
        }
        for (int i = 0; i < 4; i++)
            SmallWheelText[i].text = "x" + MultiList[i].ToString();
        Multi = MultiList[SmallIndex];
        WinData.num *= Multi;
        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.SFX_LuckyWheelOpen);
        print("奖励类型:" + WinData.type + "  , 奖励倍率:" + Multi + "  , 奖励数量:" + WinData.num);

        LuckyWheelShowAni();
    }

    public void spin()
    {
        StartCoroutine(pointerAnimation());
        BigWheel.DORotate(new Vector3(0, 0, 360 * 3 - (360 / 12f) * BigIndex), 2f, RotateMode.FastBeyond360).SetDelay(0.2f).SetEase(WheelCurve).OnComplete(() =>
        {
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.SFX_LuckyWheelStop);
        });
        SmallWheel.DORotate(new Vector3(0, 0, -360 * 3 - (360 / 4f) * SmallIndex), 3f, RotateMode.FastBeyond360).SetDelay(0.2f).SetEase(WheelCurve).OnComplete(() =>
        {
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.SFX_LuckyWheelStop);
            WheelEffect.SetActive(true);
            WheelEffect_1.SetActive(true);
        });
        SpinBtn.gameObject.SetActive(false);
        TimeManager.GetInstance().Delay(4.5f, () =>
        {
            if (WinData.type == RewardType.Gold)
                OpenUIForm(nameof(RewardPanel)).GetComponent<RewardPanel>().Init(WinData, null, null, "1013");
            else if (WinData.type == RewardType.Cash)
                OpenUIForm(nameof(RewardPanel)).GetComponent<RewardPanel>().Init(null, WinData, null, "1013");
            else if (WinData.type == RewardType.Tip)
                OpenUIForm(nameof(RewardPanel)).GetComponent<RewardPanel>().Init_Prop((int)WinData.num, 0, null, "1013");
            else if (WinData.type == RewardType.Undo)
                OpenUIForm(nameof(RewardPanel)).GetComponent<RewardPanel>().Init_Prop(0, (int)WinData.num, null, "1013");
            CloseUIForm(nameof(LuckyWheelPanel));
        });
        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.SFX_LuckyWheelRolling);
    }
    IEnumerator pointerAnimation()
    {
        yield return new WaitForSeconds(0.2f);
        int LoopCount = 24;
        Sequence seq = DOTween.Sequence();
        seq.Append(Pointer.transform.DOLocalRotate(new Vector3(0, 0, -20 + Random.Range(-2f, 2f)), 6f / LoopCount * 0.3f)
            .SetEase(Ease.Linear));
        //seq.Append(Pointer.transform.DOLocalRotate(new Vector3(0, 0, 0), 2f / 15 * 0.7f).SetEase(Ease.Linear));
        seq.AppendCallback(() =>
        {
            //MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.wheel_rotate);
            MusicMgr.GetInstance().PlayVibrate(Lofelt.NiceVibrations.HapticPatterns.PresetType.SoftImpact);
        });
        seq.SetLoops(LoopCount,LoopType.Yoyo);
        seq.SetEase(Ease.InOutSine);
        seq.Play();
    }

    public void LuckyWheelShowAni()
    {
        for (int i = 0; i < ShowList.Count; i++)
        {
            GameObject item = ShowList[i].gameObject;
            item.transform.localScale = new Vector3(0, 0, 0);
        }

        float DelayTime = 0.2f;
        Vector3 EndPos = new Vector3(0, 0, 0);
        WheelItem.GetComponent<CanvasGroup>().alpha = 0;
        WheelItem.transform.position = new Vector3(0, EndPos.y + 5, 0);

        WheelItem.GetComponent<CanvasGroup>().DOFade(1, 0.3f);
        WheelItem.transform.DOMoveY(EndPos.y, 0.3f).SetEase(Ease.OutBack);
        for (int i = 0; i < ShowList.Count; i++)
        {
            GameObject item = ShowList[i].gameObject;
            item.transform.localScale = new Vector3(0, 0, 0);
            item.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack).SetDelay(DelayTime).OnComplete(() =>
            {

            });
            DelayTime += 0.1f;
        }
    }
}
