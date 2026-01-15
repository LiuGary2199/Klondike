using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using zeta_framework;

/// <summary>
/// 数据管理器
/// </summary>

public class LineRancher : MonoBehaviour
{
    public static LineRancher Instance;
[UnityEngine.Serialization.FormerlySerializedAs("gameSetting")]
    public KickGradualPeak AbutGradual; // 游戏配置
[UnityEngine.Serialization.FormerlySerializedAs("level")]    public DodgePeak Weave;         // 关卡
[UnityEngine.Serialization.FormerlySerializedAs("resource")]    public OverheadPeak Ironwork;   // 资源
[UnityEngine.Serialization.FormerlySerializedAs("itemGroup")]    public AfarPerryPeak SealPerry; // 资源组
[UnityEngine.Serialization.FormerlySerializedAs("shop")]    public SodaPeak Pont;           // 商店
[UnityEngine.Serialization.FormerlySerializedAs("expBox")]    public SetTugPeak UseTug;       // 宝箱
[UnityEngine.Serialization.FormerlySerializedAs("skin")]    public MoatPeak Dash;           // 皮肤商店
[UnityEngine.Serialization.FormerlySerializedAs("health")]    public WanderPeak Carpet;       // 体力
[UnityEngine.Serialization.FormerlySerializedAs("activity")]    public ColonizePeak Northern;   // 活动
    public LovePeak rank;   // 排行榜

    private void Start()
    {
        // 初始化游戏配置和存档
        //Deaf();
    }

    public void Deaf()
    {
        Instance = this;

        // 初始化配置
        TextAsset Cane= Resources.Load<TextAsset>("LocationJson/GameSetting");
        JsonData setting = JsonMapper.ToObject(Cane.text);
        AbutGradual = new KickGradualPeak(setting["GameSetting"]);
        Weave = new DodgePeak();
        Ironwork = JsonMapper.ToObject<OverheadPeak>(setting["Afar"].ToJson());
        SealPerry = new AfarPerryPeak(setting["AfarPerry"]);
        Pont = new SodaPeak(setting["Soda"]);
        UseTug = new SetTugPeak(setting["SetTug"]);
        Dash = new MoatPeak(setting["Moat"]);
        Carpet = new WanderPeak();
        Northern = JsonMapper.ToObject<ColonizePeak>(setting["Colonize"].ToJson());
        Northern.DugoutSewColonize(setting);
        rank = new LovePeak(setting["Love"], setting["RankReward"]); ;

        // 读取存档
        string keepin = ShedLineRancher.FarStench("sv_framework_data");
        JsonData savedData = string.IsNullOrEmpty(keepin) ? new JsonData() : JsonMapper.ToObject(keepin);
        Weave.Deaf(savedData.ContainsKey("level") ? savedData["level"] : null);
        Ironwork.Deaf(savedData.ContainsKey("resource") ? savedData["resource"] : null);
        Pont.Deaf(savedData.ContainsKey("shop") ? savedData["shop"] : null);
        UseTug.Deaf(savedData.ContainsKey("exp_box") ? savedData["exp_box"] : null);
        Dash.Deaf(savedData.ContainsKey("skin") ? savedData["skin"] : null);
        Carpet.Deaf(savedData.ContainsKey("health") ? savedData["health"] : null);
        Northern.Deaf(savedData.ContainsKey("activity") ? savedData["activity"] : null);
        rank.Deaf(savedData.ContainsKey("rank") ? savedData["rank"] : null);

#if UNITY_EDITOR
        // 展示初始数据
        Debug.Log("数据初始化完成");
        ShedLine();
#endif

        //InvokeRepeating(nameof(HandleInterval), 3, 1);
    }

    /// <summary>
    /// 存档
    /// </summary>
    public void ShedLine()
    {
        //Debug.Log("Before data save: " + ShedLineRancher.GetString("sv_framework_data"));
        Dictionary<string, Dictionary<string, object>> Mark= new()
        {
            { "level", Weave.FarRepresentLine() },
            { "resource", Ironwork.FarRepresentLine() },
            { "shop", Pont.FarRepresentLine() },
            { "exp_box", UseTug.FarRepresentLine() },
            { "skin", Dash.FarRepresentLine() },
            { "health", Carpet.FarRepresentLine() },
            { "activity", Northern.FarRepresentLine() },
            { "rank", rank.FarRepresentLine() }
        };

        string saveDataStr = JsonMapper.ToJson(Mark);
        if (!saveDataStr.Equals(ShedLineRancher.FarStench("sv_framework_data")))
        {
            ShedLineRancher.CudStench("sv_framework_data", saveDataStr);
        }
        //Debug.Log("After data save:" + JsonMapper.ToJson(data));
    }

    /// <summary>
    /// 每秒执行的函数，处理例如更新活动状态等
    /// </summary>
    private void CrayonDemolish()
    {
        Northern.SolderColonizeWidow();

        Carpet.CalcVoyagerWander();
    }

}
