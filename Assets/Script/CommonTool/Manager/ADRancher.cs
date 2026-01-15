using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using com.adjust.sdk;
using LitJson;

public class ADRancher : MonoBehaviour
{
    [UnityEngine.Serialization.FormerlySerializedAs("MAX_SDK_KEY")] public string MAX_SDK_KEY = "";
    [UnityEngine.Serialization.FormerlySerializedAs("MAX_REWARD_ID")] public string MAX_REWARD_ID = "";
    [UnityEngine.Serialization.FormerlySerializedAs("MAX_INTER_ID")] public string MAX_INTER_ID = "";
    [UnityEngine.Serialization.FormerlySerializedAs("isTest")]
    public bool ByLife = false;
    public static ADRancher Befriend { get; private set; }

    private int CivilSpeckle;   // 广告加载失败后，重新加载广告次数
    private bool ByAsphaltIt;     // 是否正在播放广告，用于判断切换前后台时是否增加计数

    public int FeedHaulMoldShudder { get; private set; }   // 距离上次广告的时间间隔
    public int Hominid101 { get; private set; }     // 定时插屏(101)计数器
    public int Hominid102 { get; private set; }     // NoThanks插屏(102)计数器
    public int Hominid103 { get; private set; }     // 后台回前台插屏(103)计数器

    private string ValleyAnymoreStew;
    private Action<bool> ValleyTestSortGossip;    // 激励视频回调
    private bool ValleyDepress;     // 激励视频是否成功收到奖励
    private string ValleyDodge;     // 激励视频的打点

    private string QuantifiableAnymoreStew;
    private int QuantifiableJoke;      // 当前播放的插屏类型，101/102/103
    private string QuantifiableDodge;     // 插屏广告的的打点
    public bool VagueMoldExpansionist { get; private set; } // 定时插屏暂停播放

    private List<Action<ADType>> adDisgustWholesale;    // 广告播放完成回调列表，用于其他系统广告计数（例如商店看广告任务）

    private long UnspecifiedCaputCollision;     // 切后台时的时间戳
    private Ad_CustomData StarveItPotionLine; //激励视频自定义数据
    private Ad_CustomData ExpansionistItPotionLine; //插屏自定义数据

    private void Awake()
    {
        Befriend = this;
    }

    private void OnEnable()
    {
        VagueMoldExpansionist = false;
        ByAsphaltIt = false;
        FeedHaulMoldShudder = 999;  // 初始时设置一个较大的值，不阻塞插屏广告
        ValleyDepress = false;

        // Android平台将Adjust的adid传给Max；iOS将randomKey传给Max
#if UNITY_ANDROID
        MaxSdk.SetSdkKey(FarFarmerLine.FactoryDES(MAX_SDK_KEY));
        // 将adjust id 传给Max
        string adjustId = ShedLineRancher.FarStench(CAdjoin.Or_ShroudYawn);
        if (string.IsNullOrEmpty(adjustId))
        {
            adjustId = Adjust.getAdid();
        }
        if (!string.IsNullOrEmpty(adjustId))
        {
            MaxSdk.SetUserId(adjustId);
            MaxSdk.InitializeSdk();
            ShedLineRancher.CudStench(CAdjoin.Or_ShroudYawn, adjustId);
        }
        else
        {
            StartCoroutine(NssShroudYawn());
        }
#else
        MaxSdk.SetSdkKey(FarFarmerLine.FactoryDES(MAX_SDK_KEY));
        MaxSdk.SetUserId(ShedLineRancher.FarStench(CAdjoin.Or_FramePorkAt));
        MaxSdk.InitializeSdk();
#endif

        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) =>
        {
            // 打开调试模式
            //MaxSdk.ShowMediationDebugger();

            SimplisticComputerEat();
            MaxSdk.SetCreativeDebuggerEnabled(true);

            // 每秒执行一次计数
            InvokeRepeating(nameof(BeliefSaving), 1, 1);
        };
    }

    IEnumerator NssShroudYawn()
    {
        int i = 0;
        while (i < 5)
        {
            yield return new WaitForSeconds(1);
            if (UnfoldGate.UpSchist())
            {
                MaxSdk.SetUserId(ShedLineRancher.FarStench(CAdjoin.Or_FramePorkAt));
                MaxSdk.InitializeSdk();
                yield break;
            }
            else
            {
                string adjustId = Adjust.getAdid();
                if (!string.IsNullOrEmpty(adjustId))
                {
                    MaxSdk.SetUserId(adjustId);
                    MaxSdk.InitializeSdk();
                    ShedLineRancher.CudStench(CAdjoin.Or_ShroudYawn, adjustId);
                    yield break;
                }
            }
            i++;
        }
        if (i == 5)
        {
            MaxSdk.SetUserId(ShedLineRancher.FarStench(CAdjoin.Or_FramePorkAt));
            MaxSdk.InitializeSdk();
        }
    }

    public void SimplisticComputerEat()
    {
        // Attach callback
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
        MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
        MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnInterstitialRevenuePaidEvent;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;

        // Load the first rewarded ad
        RockComputerIt();

        // Load the first interstitial
        RockExpansionist();
    }

    private void RockComputerIt()
    {
        MaxSdk.LoadRewardedAd(MAX_REWARD_ID);
    }

    private void RockExpansionist()
    {
        MaxSdk.LoadInterstitial(MAX_INTER_ID);
    }

    private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is ready for you to show. MaxSdk.IsRewardedAdReady(adUnitId) now returns 'true'.

        // Reset retry attempt
        CivilSpeckle = 0;
        ValleyAnymoreStew = adInfo.NetworkName;

        StarveItPotionLine = new Ad_CustomData();
        StarveItPotionLine.user_id = CashOutManager.FarBefriend().Data.UserID;
        StarveItPotionLine.version = Application.version;
        StarveItPotionLine.request_id = CashOutManager.FarBefriend().EcpmRequestID();
        StarveItPotionLine.vendor = adInfo.NetworkName;
    }

    private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Rewarded ad failed to load 
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds).

        CivilSpeckle++;
        double retryDelay = Math.Pow(2, Math.Min(6, CivilSpeckle));

        Invoke(nameof(RockComputerIt), (float)retryDelay);
    }

    private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
#if UNITY_IOS
        LeafyLeg.FarBefriend().HeLeafyFactor = !LeafyLeg.FarBefriend().HeLeafyFactor;
        Time.timeScale = 0;
#endif
    }

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad failed to display. AppLovin recommends that you load the next ad.
        RockComputerIt();
        ByAsphaltIt = false;
    }

    private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {

    }

    private void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is hidden. Pre-load the next ad
#if UNITY_IOS
        Time.timeScale = 1;
        LeafyLeg.FarBefriend().HeLeafyFactor = !LeafyLeg.FarBefriend().HeLeafyFactor;
#endif

        ByAsphaltIt = false;
        RockComputerIt();
        if (ValleyDepress)
        {
            ValleyDepress = false;
            ValleyTestSortGossip?.Invoke(true);

            TulipItHaulDepress(ADType.Rewarded);
            WhimInuitRemove.FarBefriend().LeafInuit("9007", ValleyDodge);
        }
        else
        {
            //rewardCallBackAction?.Invoke(false);
        }
        AdSceneManager.Instance.PlaySDK();
        // 上报ecpm
        CashOutManager.FarBefriend().ReportEcpm(adInfo, StarveItPotionLine.request_id, "REWARD");
    }

    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        // The rewarded ad displayed and the user should receive the reward.
        ValleyDepress = true;
    }

    private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo info)
    {
        // Ad revenue paid. Use this callback to track user revenue.
        //从MAX获取收入数据
        var adRevenue = new AdjustAdRevenue(AdjustConfig.AdjustAdRevenueSourceAppLovinMAX);
        adRevenue.setRevenue(info.Revenue, "USD");
        adRevenue.setAdRevenueNetwork(info.NetworkName);
        adRevenue.setAdRevenueUnit(info.AdUnitIdentifier);
        adRevenue.setAdRevenuePlacement(info.Placement);

        //发回收入数据给自己后台
        string countryCodeByMAX = MaxSdk.GetSdkConfiguration().CountryCode; // "US" for the United States, etc - Note: Do not confuse this with currency code which is "USD"
        WhimInuitRemove.FarBefriend().LeafInuit("9008", info.Revenue.ToString(), countryCodeByMAX);

        //带广告收入的漏传策略
        ShroudDeafRancher.Instance.PegItWaist(countryCodeByMAX, info.Revenue);

        string adjustAdid = ShroudDeafRancher.Instance.FarShroudYawn();
        //发回收入数据给Adjust
        if (!string.IsNullOrEmpty(adjustAdid))
        {
            Adjust.trackAdRevenue(adRevenue);
            UnityEngine.Debug.Log("Max to Adjust (rewarded), adUnitId:" + adUnitId + ", revenue:" + info.Revenue + ", network:" + info.NetworkName + ", unit:" + info.AdUnitIdentifier + ", placement:" + info.Placement);
        }

        // 发回收入数据给Mintegral
        if (!string.IsNullOrEmpty(adjustAdid))
        {
#if UNITY_ANDROID || UNITY_IOS
            MBridgeRevenueParamsEntity mBridgeRevenueParamsEntity = new MBridgeRevenueParamsEntity(MBridgeRevenueParamsEntity.ATTRIBUTION_PLATFORM_ADJUST, adjustAdid);
            ///MaxSdkBase.AdInfo类型的adInfo
            mBridgeRevenueParamsEntity.SetMaxAdInfo(info);
            MBridgeRevenueManager.Track(mBridgeRevenueParamsEntity);
            UnityEngine.Debug.Log(nameof(MBridgeRevenueManager) + "~Rewarded revenue:" + info.Revenue);
#endif
        }
    }

    private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is ready for you to show. MaxSdk.IsInterstitialReady(adUnitId) now returns 'true'

        // Reset retry attempt
        CivilSpeckle = 0;
        QuantifiableAnymoreStew = adInfo.NetworkName;

        ExpansionistItPotionLine = new Ad_CustomData();
        ExpansionistItPotionLine.user_id = CashOutManager.FarBefriend().Data.UserID;
        ExpansionistItPotionLine.version = Application.version;
        ExpansionistItPotionLine.request_id = CashOutManager.FarBefriend().EcpmRequestID();
        ExpansionistItPotionLine.vendor = adInfo.NetworkName;
    }

    private void OnInterstitialLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Interstitial ad failed to load 
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds)

        CivilSpeckle++;
        double retryDelay = Math.Pow(2, Math.Min(6, CivilSpeckle));

        Invoke(nameof(RockExpansionist), (float)retryDelay);
    }

    private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
#if UNITY_IOS
        LeafyLeg.FarBefriend().HeLeafyFactor = !LeafyLeg.FarBefriend().HeLeafyFactor;
        Time.timeScale = 0;
#endif
    }

    private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad failed to display. AppLovin recommends that you load the next ad.
        RockExpansionist();
        ByAsphaltIt = false;
    }

    private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnInterstitialRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo info)
    {
        //从MAX获取收入数据
        var adRevenue = new AdjustAdRevenue(AdjustConfig.AdjustAdRevenueSourceAppLovinMAX);
        adRevenue.setRevenue(info.Revenue, "USD");
        adRevenue.setAdRevenueNetwork(info.NetworkName);
        adRevenue.setAdRevenueUnit(info.AdUnitIdentifier);
        adRevenue.setAdRevenuePlacement(info.Placement);

        //发回收入数据给自己后台
        string countryCodeByMAX = MaxSdk.GetSdkConfiguration().CountryCode; // "US" for the United States, etc - Note: Do not confuse this with currency code which is "USD"
        WhimInuitRemove.FarBefriend().LeafInuit("9108", info.Revenue.ToString(), countryCodeByMAX);

        //带广告收入的漏传策略
        ShroudDeafRancher.Instance.PegItWaist(countryCodeByMAX, info.Revenue);

        //发回收入数据给Adjust
        if (!string.IsNullOrEmpty(ShroudDeafRancher.Instance.FarShroudYawn()))
        {
            Adjust.trackAdRevenue(adRevenue);
            UnityEngine.Debug.Log("Max to Adjust (interstitial), adUnitId:" + adUnitId + ", revenue:" + info.Revenue + ", network:" + info.NetworkName + ", unit:" + info.AdUnitIdentifier + ", placement:" + info.Placement);
        }

        // 发回收入数据给Mintegral
        string adjustAdid = ShroudDeafRancher.Instance.FarShroudYawn();
        if (!string.IsNullOrEmpty(adjustAdid))
        {
#if UNITY_ANDROID || UNITY_IOS
            MBridgeRevenueParamsEntity mBridgeRevenueParamsEntity = new MBridgeRevenueParamsEntity(MBridgeRevenueParamsEntity.ATTRIBUTION_PLATFORM_ADJUST, adjustAdid);
            ///MaxSdkBase.AdInfo类型的adInfo
            mBridgeRevenueParamsEntity.SetMaxAdInfo(info);
            MBridgeRevenueManager.Track(mBridgeRevenueParamsEntity);
            UnityEngine.Debug.Log(nameof(MBridgeRevenueManager) + "~Interstitial revenue:" + info.Revenue);
#endif
        }
    }

    private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is hidden. Pre-load the next ad.
#if UNITY_IOS
        Time.timeScale = 1;
        LeafyLeg.FarBefriend().HeLeafyFactor = !LeafyLeg.FarBefriend().HeLeafyFactor;
#endif
        RockExpansionist();

        TulipItHaulDepress(ADType.Interstitial);
        WhimInuitRemove.FarBefriend().LeafInuit("9107", QuantifiableDodge);
        // 上报ecpm
        AdSceneManager.Instance.PlaySDK();
        CashOutManager.FarBefriend().ReportEcpm(adInfo, ExpansionistItPotionLine.request_id, "INTER");
    }


    /// <summary>
    /// 播放激励视频广告
    /// </summary>
    /// <param name="callBack"></param>
    /// <param name="index"></param>
    public void LionStarveAlder(Action<bool> callBack, string index)
    {
        if (ByLife)
        {
            callBack(true);
            TulipItHaulDepress(ADType.Rewarded);
            return;
        }

        bool rewardVideoReady = MaxSdk.IsRewardedAdReady(MAX_REWARD_ID);
        ValleyTestSortGossip = callBack;
        if (rewardVideoReady)
        {
            AdSceneManager.Instance.StopSDK();
            // 打点
            ValleyDodge = index;
            WhimInuitRemove.FarBefriend().LeafInuit("9002", index);
            ByAsphaltIt = true;
            ValleyDepress = false;
            string placement = index + "_" + ValleyAnymoreStew;
            StarveItPotionLine.placement_id = placement;
            MaxSdk.ShowRewardedAd(MAX_REWARD_ID, placement, JsonMapper.ToJson(StarveItPotionLine));
        }
        else
        {
            GroomRancher.FarBefriend().BindGroom("No ads right now, please try it later.");
            // rewardCallBackAction(false);
        }
    }

    /// <summary>
    /// 播放插屏广告
    /// </summary>
    /// <param name="index"></param>
    public void LionExpansionistIt(int index)
    {
        if (index == 101 || index == 102 || index == 103)
        {
            UnityEngine.Debug.LogError("广告点位不允许为101、102、103");
            return;
        }

        LionExpansionist(index);
    }

    /// <summary>
    /// 播放插屏广告
    /// </summary>
    /// <param name="index">101/102/103</param>
    /// <param name="customIndex">用户自定义点位</param>
    private void LionExpansionist(int index, int customIndex = 0)
    {
        QuantifiableJoke = index;

        if (ByAsphaltIt)
        {
            return;
        }

        // 当用户过关数 < trial_MaxNum时，不弹插屏广告
        // int sv_trialNum = ShedLineRancher.FarWit(CAdjoin.Or_It_First_Toy);
        // int trial_MaxNum = int.Parse(ToeBoldLeg.instance.AdjoinLine.trial_MaxNum);
        // if (sv_trialNum < trial_MaxNum)
        // {
        //     return;
        // }

        // 时间间隔低于阈值，不播放广告
        if (FeedHaulMoldShudder < int.Parse(ToeBoldLeg.instance.AdjoinLine.inter_freq))
        {
            return;
        }

        if (ByLife)
        {
            TulipItHaulDepress(ADType.Interstitial);
            return;
        }

        bool interstitialVideoReady = MaxSdk.IsInterstitialReady(MAX_INTER_ID);
        if (interstitialVideoReady)
        {
            AdSceneManager.Instance.StopSDK();
            ByAsphaltIt = true;
            // 打点
            string point = index.ToString();
            if (customIndex > 0)
            {
                point += customIndex.ToString().PadLeft(2, '0');
            }
            QuantifiableDodge = point;
            WhimInuitRemove.FarBefriend().LeafInuit("9102", point);
            string placement = point + "_" + QuantifiableAnymoreStew;
            ExpansionistItPotionLine.placement_id = placement;
            MaxSdk.ShowInterstitial(MAX_INTER_ID, placement, JsonMapper.ToJson(ExpansionistItPotionLine));
        }
    }

    /// <summary>
    /// 每秒更新一次计数器 - 101计数器 和 时间间隔计数器
    /// </summary>
    private void BeliefSaving()
    {
        FeedHaulMoldShudder++;

        int relax_interval = int.Parse(ToeBoldLeg.instance.AdjoinLine.relax_interval);
        // 计时器阈值设置为0或负数时，关闭广告101；
        // 播放广告期间不计数；
        if (relax_interval <= 0 || ByAsphaltIt)
        {
            return;
        }
        else
        {
            Hominid101++;
            if (Hominid101 >= relax_interval && !VagueMoldExpansionist)
            {
                LionExpansionist(101);
            }
        }
    }

    /// <summary>
    /// NoThanks插屏 - 102
    /// </summary>
    public void IfCoronaPegWaist(int customIndex = 0)
    {
        // 用户行为累计次数计数器阈值设置为0或负数时，关闭广告102
        int nextlevel_interval = int.Parse(ToeBoldLeg.instance.AdjoinLine.nextlevel_interval);
        if (nextlevel_interval <= 0)
        {
            return;
        }
        else
        {
            Hominid102 = ShedLineRancher.FarWit("NoThanksCount") + 1;
            ShedLineRancher.CudWit("NoThanksCount", Hominid102);
            if (Hominid102 >= nextlevel_interval)
            {
                LionExpansionist(102, customIndex);
            }
        }
    }

    /// <summary>
    /// 前后台切换计数器 - 103
    /// </summary>
    /// <param name="pause"></param>
    private void OnApplicationPause(bool pause)
    {
        if (!pause)
        {
            // 切回前台
            if (!ByAsphaltIt)
            {
                // 前后台切换时，播放间隔计数器需要累加切到后台的时间
                if (UnspecifiedCaputCollision > 0)
                {
                    FeedHaulMoldShudder += (int)(WardGate.Voyager() - UnspecifiedCaputCollision);
                    UnspecifiedCaputCollision = 0;
                }
                // 后台切回前台累计次数，后台配置为0或负数，关闭该广告
                int inter_b2f_count = int.Parse(ToeBoldLeg.instance.AdjoinLine.inter_b2f_count);
                if (inter_b2f_count <= 0)
                {
                    return;
                }
                else
                {
                    Hominid103++;
                    if (Hominid103 >= inter_b2f_count)
                    {
                        LionExpansionist(103);
                    }
                }
            }
        }
        else
        {
            // 切到后台
            UnspecifiedCaputCollision = WardGate.Voyager();
        }
    }

    /// <summary>
    /// 暂停定时插屏播放 - 101
    /// </summary>
    public void CaputMoldExpansionist()
    {
        VagueMoldExpansionist = true;
    }

    /// <summary>
    /// 恢复定时插屏播放 - 101
    /// </summary>
    public void ReputeMoldExpansionist()
    {
        VagueMoldExpansionist = false;
    }

    /// <summary>
    /// 更新游戏的TrialNum
    /// </summary>
    /// <param name="num"></param>
    public void SolderRallyLad(int num)
    {
        ShedLineRancher.CudWit(CAdjoin.Or_It_First_Toy, num);
    }

    /// <summary>
    /// 注册看广告的回调事件
    /// </summary>
    /// <param name="callback"></param>
    public void IroquoisHaulVariance(Action<ADType> callback)
    {
        if (adDisgustWholesale == null)
        {
            adDisgustWholesale = new List<Action<ADType>>();
        }

        if (!adDisgustWholesale.Contains(callback))
        {
            adDisgustWholesale.Add(callback);
        }
    }

    /// <summary>
    /// 广告播放成功后，执行看广告回调事件
    /// </summary>
    private void TulipItHaulDepress(ADType adType)
    {
        ByAsphaltIt = false;
        // 播放间隔计数器清零
        FeedHaulMoldShudder = 0;
        // 插屏计数器清零
        if (adType == ADType.Interstitial)
        {
            // 计数器清零
            if (QuantifiableJoke == 101)
            {
                Hominid101 = 0;
            }
            else if (QuantifiableJoke == 102)
            {
                Hominid102 = 0;
                ShedLineRancher.CudWit("NoThanksCount", 0);
            }
            else if (QuantifiableJoke == 103)
            {
                Hominid103 = 0;
            }
        }

        // 看广告总数+1
        ShedLineRancher.CudWit(CAdjoin.Or_March_It_Toy + adType.ToString(), ShedLineRancher.FarWit(CAdjoin.Or_March_It_Toy + adType.ToString()) + 1);
        // 真提现任务 
        if (adType == ADType.Rewarded)
            CashOutManager.FarBefriend().AddTaskValue("Ad", 1);

        // 回调
        if (adDisgustWholesale != null && adDisgustWholesale.Count > 0)
        {
            foreach (Action<ADType> callback in adDisgustWholesale)
            {
                callback?.Invoke(adType);
            }
        }
    }

    /// <summary>
    /// 获取总的看广告次数
    /// </summary>
    /// <returns></returns>
    public int FarEndowItLad(ADType adType)
    {
        return ShedLineRancher.FarWit(CAdjoin.Or_March_It_Toy + adType.ToString());
    }
}

public enum ADType { Interstitial, Rewarded }

[System.Serializable]
public class Ad_CustomData //广告自定义数据
{
    public string user_id; //用户id
    public string version; //版本号
    public string request_id; //请求id
    public string vendor; //渠道
    public string placement_id; //广告位id
}