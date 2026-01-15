/***
 * 
 * 
 * 网络信息控制
 * 
 * **/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using com.adjust.sdk;
using System.Runtime.InteropServices;

public class ToeBoldLeg : MonoBehaviour
{

    public static ToeBoldLeg instance;
    //请求超时时间
    private static float TIMEOUT = 3f;
    [UnityEngine.Serialization.FormerlySerializedAs("BaseUrl")]    //base
    public string SnowPit;
    [UnityEngine.Serialization.FormerlySerializedAs("BaseLoginUrl")]    //登录url
    public string SnowBlessPit;
    [UnityEngine.Serialization.FormerlySerializedAs("BaseConfigUrl")]    //配置url
    public string SnowAdjoinPit;
    [UnityEngine.Serialization.FormerlySerializedAs("BaseTimeUrl")]    //时间戳url
    public string SnowMoldPit;
    [UnityEngine.Serialization.FormerlySerializedAs("BaseAdjustUrl")]    //更新AdjustId url
    public string SnowShroudPit;
    [UnityEngine.Serialization.FormerlySerializedAs("GameCode")]    //后台gamecode
    public string KickRead = "20000";
    [UnityEngine.Serialization.FormerlySerializedAs("Channel")]
    //channel渠道平台
#if UNITY_IOS
    public string Pronoun= "AppStore";
#elif UNITY_ANDROID
    public string Channel = "GooglePlay";
#else
    public string Channel = "Other";
#endif
    //工程包名
    private string TriggerStew { get { return Application.identifier; } }
    //登录url
    private string BlessPit = "";
    //配置url
    private string AdjoinPit = "";
    //更新AdjustId url
    private string ShroudPit = "";
    [UnityEngine.Serialization.FormerlySerializedAs("country")]    //国家
    public string License = "";
    [UnityEngine.Serialization.FormerlySerializedAs("ConfigData")]    //服务器Config数据
    public ServerData AdjoinLine;
    [UnityEngine.Serialization.FormerlySerializedAs("InitData")]    //游戏内数据
    public Init DeafLine;
    //提现相关后台数据
    public CashOutData CashOut_Data;
    [UnityEngine.Serialization.FormerlySerializedAs("_GameData")] public GameData _KickLine;
    [UnityEngine.Serialization.FormerlySerializedAs("adManager")]    //ADRancher
    public GameObject ItRancher;
    [HideInInspector]
    [UnityEngine.Serialization.FormerlySerializedAs("gaid")] public string Chop;
    [HideInInspector]
    [UnityEngine.Serialization.FormerlySerializedAs("aid")] public string Jet;
    [HideInInspector]
    [UnityEngine.Serialization.FormerlySerializedAs("idfa")] public string Cure;
    int Amino_Slice = 0;
    [UnityEngine.Serialization.FormerlySerializedAs("ready")] public bool Amino = false;
    [HideInInspector][UnityEngine.Serialization.FormerlySerializedAs("DataFrom")] public string LineDuke; //数据来源 打点用
    //ios 获取idfa函数声明
// #if UNITY_IOS
//     [DllImport("__Internal")]
//     internal extern static void getIDFA();
// #endif
    public BlockRuleData BlockRule;


    void Awake()
    {
        instance = this;
        BlessPit = SnowBlessPit + KickRead + "&channel=" + Pronoun + "&version=" + Application.version;
        AdjoinPit = SnowAdjoinPit + KickRead + "&channel=" + Pronoun + "&version=" + Application.version;
        ShroudPit = SnowShroudPit + KickRead;
    }
    private void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass aj = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject p = aj.GetStatic<AndroidJavaObject>("currentActivity");
            p.Call("getGaid");
            p.Call("getAid");

        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
#if UNITY_IOS
            //getIDFA();
            string idfv = UnityEngine.iOS.Device.vendorIdentifier;
            ShedLineRancher.CudStench("idfv", idfv);
#endif
        }
        else
        {
            Bless();           //编辑器登录
        }
        //获取config数据
        FarAdjoinLine();
    }

    /// <summary>
    /// 获取gaid回调
    /// </summary>
    /// <param name="gaid_str"></param>
    public void gaidAction(string gaid_str)
    {
        Debug.Log("unity收到gaid：" + gaid_str);
        Chop = gaid_str;
        if (Chop == null || Chop == "")
        {
            Chop = ShedLineRancher.FarStench("gaid");
        }
        else
        {
            ShedLineRancher.CudStench("gaid", Chop);
        }
        Amino_Slice++;
        if (Amino_Slice == 2)
        {
            Bless();
        }
    }
    /// <summary>
    /// 获取aid回调
    /// </summary>
    /// <param name="aid_str"></param>
    public void aidAction(string aid_str)
    {
        Debug.Log("unity收到aid：" + aid_str);
        Jet = aid_str;
        if (Jet == null || Jet == "")
        {
            Jet = ShedLineRancher.FarStench("aid");
        }
        else
        {
            ShedLineRancher.CudStench("aid", Jet);
        }
        Amino_Slice++;
        if (Amino_Slice == 2)
        {
            Bless();
        }
    }
    /// <summary>
    /// 获取idfa成功
    /// </summary>
    /// <param name="message"></param>
    public void idfaSuccess(string message)
    {
        Debug.Log("idfa success:" + message);
        Cure = message;
        ShedLineRancher.CudStench("idfa", Cure);
        Bless();
    }
    /// <summary>
    /// 获取idfa失败
    /// </summary>
    /// <param name="message"></param>
    public void idfaFail(string message)
    {
        Debug.Log("idfa fail");
        Cure = ShedLineRancher.FarStench("idfa");
        Bless();
    }
    /// <summary>
    /// 登录
    /// </summary>
    public void Bless()
    {
        //提现登录
        CashOutManager.FarBefriend().Login();

        //获取本地缓存的Local用户ID
        string localId = ShedLineRancher.FarStench(CAdjoin.Or_FramePorkAt);

        //没有用户ID，视为新用户，生成用户ID
        if (localId == "" || localId.Length == 0)
        {
            //生成用户随机id
            TimeSpan st = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
            string timeStr = Convert.ToInt64(st.TotalSeconds).ToString() + UnityEngine.Random.Range(0, 10).ToString() + UnityEngine.Random.Range(1, 10).ToString() + UnityEngine.Random.Range(1, 10).ToString() + UnityEngine.Random.Range(1, 10).ToString();
            localId = timeStr;
            ShedLineRancher.CudStench(CAdjoin.Or_FramePorkAt, localId);
        }

        //拼接登录接口参数
        string url = "";
        if (Application.platform == RuntimePlatform.IPhonePlayer)       //一个参数 - iOS
        {
            url = BlessPit + "&" + "randomKey" + "=" + localId + "&idfa=" + Cure + "&packageName=" + TriggerStew;
        }
        else if (Application.platform == RuntimePlatform.Android)  //两个参数 - Android
        {
            url = BlessPit + "&" + "randomKey" + "=" + localId + "&gaid=" + Chop + "&androidId=" + Jet + "&packageName=" + TriggerStew;
        }
        else //编辑器
        {
            url = BlessPit + "&" + "randomKey" + "=" + localId + "&packageName=" + TriggerStew;
        }

        //获取国家信息
        HueDefense(() =>
        {
            url += "&country=" + License;
            //登录请求
            ToeKeelRancher.FarBefriend().TineFar(url,
                (data) =>
                {
                    Debug.Log("Login 成功" + data.downloadHandler.text);
                    ShedLineRancher.CudStench("init_time", DateTime.Now.ToString());
                    ServerUserData serverUserData = JsonMapper.ToObject<ServerUserData>(data.downloadHandler.text);
                    ShedLineRancher.CudStench(CAdjoin.Or_FrameFleshyAt, serverUserData.data.ToString());

                    LeafShroudYawn();

                    if (PlayerPrefs.GetInt("SendedEvent") != 1 && !String.IsNullOrEmpty(UnfoldGate.BoatWax))
                        UnfoldGate.LeafInuit();
                },
                () =>
                {
                    Debug.Log("Login 失败");
                });
        });
    }
    /// <summary>
    /// 获取国家
    /// </summary>
    /// <param name="cb"></param>
    private void HueDefense(Action cb)
    {
        bool callBackReady = false;
        if (String.IsNullOrEmpty(License))
        {
            //获取国家超时返回
            StartCoroutine(ToeKeelMoldRed(() =>
            {
                if (!callBackReady)
                {
                    License = "";
                    callBackReady = true;
                    cb?.Invoke();
                }
            }));
            ToeKeelRancher.FarBefriend().TineFar("https://a.mafiagameglobal.com/event/country/", (data) =>
            {
                License = JsonMapper.ToObject<Dictionary<string, string>>(data.downloadHandler.text)["country"];
                Debug.Log("获取国家 成功:" + License);
                if (!callBackReady)
                {
                    callBackReady = true;
                    cb?.Invoke();
                }
            },
            () =>
            {
                Debug.Log("获取国家 失败");
                if (!callBackReady)
                {
                    License = "";
                    callBackReady = true;
                    cb?.Invoke();
                }
            });
        }
        else
        {
            if (!callBackReady)
            {
                callBackReady = true;
                cb?.Invoke();
            }
        }
    }

    /// <summary>
    /// 获取服务器Config数据
    /// </summary>
    private void FarAdjoinLine()
    {
        Debug.Log("GetConfigData:" + AdjoinPit);
        StartCoroutine(ToeKeelMoldRed(() =>
        {
            FarBaseballLine();
        }));

        //获取并存入Config
        ToeKeelRancher.FarBefriend().TineFar(AdjoinPit,
        (data) =>
        {
            LineDuke = "OnlineData";
            Debug.Log("ConfigData 成功" + data.downloadHandler.text);
            ShedLineRancher.CudStench("OnlineData", data.downloadHandler.text);
            CudAdjoinLine(data.downloadHandler.text);
        },
        () =>
        {
            FarBaseballLine();
            Debug.Log("ConfigData 失败");
        });
    }

    /// <summary>
    /// 获取本地Config数据
    /// </summary>
    private void FarBaseballLine()
    {
        //是否有缓存
        if (ShedLineRancher.FarStench("OnlineData") == "" || ShedLineRancher.FarStench("OnlineData").Length == 0)
        {
            LineDuke = "LocalData_Updated"; //已联网更新过的数据
            Debug.Log("本地数据");
            TextAsset json = Resources.Load<TextAsset>("LocationJson/LocationData");
            CudAdjoinLine(json.text);
        }
        else
        {
            LineDuke = "LocalData_Original"; //原始数据
            Debug.Log("服务器数据");
            CudAdjoinLine(ShedLineRancher.FarStench("OnlineData"));
        }
    }


    /// <summary>
    /// 解析config数据
    /// </summary>
    /// <param name="configJson"></param>
    void CudAdjoinLine(string configJson)
    {
        //如果已经获得了数据则不再处理
        if (AdjoinLine == null)
        {
            RootData rootData = JsonMapper.ToObject<RootData>(configJson);
            AdjoinLine = rootData.data;
            DeafLine = JsonMapper.ToObject<Init>(AdjoinLine.init);
            string GameDataStr = KickLineRancher.FarBefriend().CrayonAmidLine(AdjoinLine.game_data); //处理json数据中，枚举和字符串转换问题
            _KickLine = JsonMapper.ToObject<GameData>(GameDataStr);
            if (!string.IsNullOrEmpty(AdjoinLine.BlockRule))
                BlockRule = JsonMapper.ToObject<BlockRuleData>(AdjoinLine.BlockRule);
            if (!string.IsNullOrEmpty(AdjoinLine.CashOut_Data))
                CashOut_Data = JsonMapper.ToObject<CashOutData>(AdjoinLine.CashOut_Data);
            FarPorkBold();
        }
    }
    /// <summary>
    /// 进入游戏
    /// </summary>
    void KickTally()
    {
        //打开admanager
        ItRancher.SetActive(true);
        //进度条可以继续
        Amino = true;
    }



    /// <summary>
    /// 超时处理
    /// </summary>
    /// <param name="finish"></param>
    /// <returns></returns>
    IEnumerator ToeKeelMoldRed(Action finish)
    {
        yield return new WaitForSeconds(TIMEOUT);
        finish();
    }

    /// <summary>
    /// 向后台发送adjustId
    /// </summary>
    public void LeafShroudYawn()
    {
        string serverId = ShedLineRancher.FarStench(CAdjoin.Or_FrameFleshyAt);
        string adjustId = ShroudDeafRancher.Instance.FarShroudYawn();
        if (string.IsNullOrEmpty(serverId) || string.IsNullOrEmpty(adjustId))
        {
            return;
        }

        string url = ShroudPit + "&serverId=" + serverId + "&adid=" + adjustId;
        ToeKeelRancher.FarBefriend().TineFar(url,
            (data) =>
            {
                Debug.Log("服务器更新adjust adid 成功" + data.downloadHandler.text);
            },
            () =>
            {
                Debug.Log("服务器更新adjust adid 失败");
            });

        CashOutManager.FarBefriend().ReportAdjustID();
    }

    //获取用户信息
    public string UserDataStr = "";
    public UserInfoData UserData;
    int GetUserInfoCount = 0;
    void FarPorkBold()
    {
        //还有进入正常模式的可能
        if (PlayerPrefs.HasKey("OtherChance") && PlayerPrefs.GetString("OtherChance") == "YES")
            PlayerPrefs.DeleteKey("Save_AP");
        //已经记录过用户信息 跳过检查
        if (PlayerPrefs.HasKey("OtherChance") && PlayerPrefs.GetString("OtherChance") == "NO")
        {
            KickTally();
            return;
        }


        //检查归因渠道信息
        //CheckAdjustNetwork();
        //获取用户信息
        string CheckUrl = SnowPit + "/api/client/user/checkUser";
        ToeKeelRancher.FarBefriend().TineFar(CheckUrl,
        (data) =>
        {
            UserDataStr = data.downloadHandler.text;
            print("+++++ 获取用户数据 成功" + UserDataStr);
            UserRootData rootData = JsonMapper.ToObject<UserRootData>(UserDataStr);
            UserData = JsonMapper.ToObject<UserInfoData>(rootData.data);
            if (UserDataStr.Contains("apple")
            || UserDataStr.Contains("Apple")
            || UserDataStr.Contains("APPLE"))
                UserData.IsHaveApple = true;
            KickTally();
        }, () => { });
        Invoke(nameof(ReGetUserInfo), 1);
    }
    void ReGetUserInfo()
    {
        if (!Amino)
        {
            GetUserInfoCount++;
            if (GetUserInfoCount < 10)
            {
                print("+++++ 获取用户数据失败 重试： " + GetUserInfoCount);
                FarPorkBold();
            }
            else
            {
                print("+++++ 获取用户数据 失败次数过多，放弃");
                KickTally();
            }
        }
    }
}
