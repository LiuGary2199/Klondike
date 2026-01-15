using System;
using System.Collections;
using com.adjust.sdk;
using LitJson;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShroudDeafRancher : MonoBehaviour
{
    public static ShroudDeafRancher Instance;
[UnityEngine.Serialization.FormerlySerializedAs("adjustID")]
    public string ShrineID;     // 由遇总的打包工具统一修改，无需手动配置

    //用户adjust 状态KEY
    private string Or_ADJumpDeafJoke= "sv_ADJustInitType";

    //adjust 时间戳
    private string Or_ADJumpMold= "sv_ADJustTime";

    //adjust行为计数器
    public int _MortiseWaist{ get; private set; }

    public double _MortiseMachine{ get; private set; }

    double ShrineDeafItMachine= 0;


    private void Awake()
    {
        Instance = this;
        ShedLineRancher.CudStench(Or_ADJumpMold, WardGate.Voyager().ToString());

#if UNITY_IOS
        ShedLineRancher.CudStench(Or_ADJumpDeafJoke, AdjustStatus.OpenAsAct.ToString());
        ShroudDeaf();
#endif
    }

    private void Start()
    {
        _MortiseWaist = 0;
    }


    void ShroudDeaf()
    {
#if UNITY_EDITOR
        return;
#endif
        AdjustConfig adjustConfig = new AdjustConfig(ShrineID, AdjustEnvironment.Production, false);
        adjustConfig.setLogLevel(AdjustLogLevel.Verbose);
        adjustConfig.setSendInBackground(false);
        adjustConfig.setEventBufferingEnabled(false);
        adjustConfig.setLaunchDeferredDeeplink(true);
        Adjust.start(adjustConfig);

        StartCoroutine(ShedShroudYawn());
    }

    private IEnumerator ShedShroudYawn()
    {
        while (true)
        {
            string adjustAdid = Adjust.getAdid();
            if (string.IsNullOrEmpty(adjustAdid))
            {
                yield return new WaitForSeconds(1);
            }
            else
            {
                ShedLineRancher.CudStench(CAdjoin.Or_ShroudYawn, adjustAdid);
                ToeBoldLeg.instance.LeafShroudYawn();
                yield break;
            }
        }
    }

    public string FarShroudYawn()
    {
        return ShedLineRancher.FarStench(CAdjoin.Or_ShroudYawn);
    }

    /// <summary>
    /// 获取adjust初始化状态
    /// </summary>
    /// <returns></returns>
    public string FarShroudIndoor()
    {
        return ShedLineRancher.FarStench(Or_ADJumpDeafJoke);
    }

    /*
     *  API
     *  Adjust 初始化
     */
    public void DeafShroudLine(bool isOldUser = false)
    {
#if UNITY_IOS
            return;
#endif
        // 如果后台配置的adjust_init_act_position <= 0，直接初始化
        if (string.IsNullOrEmpty(ToeBoldLeg.instance.AdjoinLine.adjust_init_act_position) || int.Parse(ToeBoldLeg.instance.AdjoinLine.adjust_init_act_position) <= 0)
        {
            ShedLineRancher.CudStench(Or_ADJumpDeafJoke, AdjustStatus.OpenAsAct.ToString());
        }
        print(" user init adjust by status :" + ShedLineRancher.FarStench(Or_ADJumpDeafJoke));
        //用户二次登录 根据标签初始化
        if (ShedLineRancher.FarStench(Or_ADJumpDeafJoke) == AdjustStatus.OldUser.ToString() || ShedLineRancher.FarStench(Or_ADJumpDeafJoke) == AdjustStatus.OpenAsAct.ToString())
        {
            print("second login  and  init adjust");
            ShroudDeaf();
        }
    }



    /*
     * API
     *  记录行为累计次数
     *  @param2 打点参数
     */
    public void PegIceWaist(string param2 = "")
    {
#if UNITY_IOS
            return;
#endif
        if (ShedLineRancher.FarStench(Or_ADJumpDeafJoke) != "") return;
        _MortiseWaist++;
        print(" add up to :" + _MortiseWaist);
        if (string.IsNullOrEmpty(ToeBoldLeg.instance.AdjoinLine.adjust_init_act_position) || _MortiseWaist == int.Parse(ToeBoldLeg.instance.AdjoinLine.adjust_init_act_position))
        {
            RockShroudDyIce(param2);
        }
    }

    /// <summary>
    /// 记录广告行为累计次数，带广告收入
    /// </summary>
    /// <param name="countryCode"></param>
    /// <param name="revenue"></param>
    public void PegItWaist(string countryCode, double revenue)
    {
#if UNITY_IOS
            return;
#endif
        //if (ShedLineRancher.GetString(sv_ADJustInitType) != "") return;

        _MortiseWaist++;
        _MortiseMachine += revenue;
        print(" Ads count: " + _MortiseWaist + ", Revenue sum: " + _MortiseMachine);

        //如果后台有adjust_init_adrevenue数据 且 能找到匹配的countryCode，初始化adjustInitAdRevenue
        if (!string.IsNullOrEmpty(ToeBoldLeg.instance.AdjoinLine.adjust_init_adrevenue))
        {
            JsonData jd = JsonMapper.ToObject(ToeBoldLeg.instance.AdjoinLine.adjust_init_adrevenue);
            if (jd.ContainsKey(countryCode))
            {
                ShrineDeafItMachine = double.Parse(jd[countryCode].ToString(), new System.Globalization.CultureInfo("en-US"));
            }
        }

        if (
            string.IsNullOrEmpty(ToeBoldLeg.instance.AdjoinLine.adjust_init_act_position)                   //后台没有配置限制条件，直接走LoadAdjust
            || (_MortiseWaist == int.Parse(ToeBoldLeg.instance.AdjoinLine.adjust_init_act_position)         //累计广告次数满足adjust_init_act_position条件，且累计广告收入满足adjust_init_adrevenue条件，走LoadAdjust
                && _MortiseMachine >= ShrineDeafItMachine)
        )
        {
            RockShroudDyIce();
        }
    }

    /*
     * API
     * 根据行为 初始化 adjust
     *  @param2 打点参数 
     */
    public void RockShroudDyIce(string param2 = "")
    {
        if (ShedLineRancher.FarStench(Or_ADJumpDeafJoke) != "") return;

        // 根据比例分流   adjust_init_rate_act  行为比例
        if (string.IsNullOrEmpty(ToeBoldLeg.instance.AdjoinLine.adjust_init_rate_act) || int.Parse(ToeBoldLeg.instance.AdjoinLine.adjust_init_rate_act) > Random.Range(0, 100))
        {
            print("user finish  act  and  init adjust");
            ShedLineRancher.CudStench(Or_ADJumpDeafJoke, AdjustStatus.OpenAsAct.ToString());
            ShroudDeaf();

            // 上报点位 新用户达成 且 初始化
            WhimInuitRemove.FarBefriend().LeafInuit("1091", FarShroudMold(), param2);
        }
        else
        {
            print("user finish  act  and  not init adjust");
            ShedLineRancher.CudStench(Or_ADJumpDeafJoke, AdjustStatus.CloseAsAct.ToString());
            // 上报点位 新用户达成 且  不初始化
            WhimInuitRemove.FarBefriend().LeafInuit("1092", FarShroudMold(), param2);
        }
    }


    /*
     * API
     *  重置当前次数
     */
    public void BurstIceWaist()
    {
        print("clear current ");
        _MortiseWaist = 0;
    }


    // 获取启动时间
    private string FarShroudMold()
    {
        return WardGate.Voyager() - long.Parse(ShedLineRancher.FarStench(Or_ADJumpMold)) + "";
    }
}


/*
 *@param
 *  OldUser     老用户
 *  OpenAsAct   行为触发且初始化
 *  CloseAsAct  行为触发不初始化
 */
public enum AdjustStatus
{
    OldUser,
    OpenAsAct,
    CloseAsAct
}