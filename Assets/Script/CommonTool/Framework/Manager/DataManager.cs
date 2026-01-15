using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using zeta_framework;

/// <summary>
/// 数据管理器
/// </summary>

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    public GameSettingCtrl gameSetting; // 游戏配置
    public LevelCtrl level;         // 关卡
    public ResourceCtrl resource;   // 资源
    public ItemGroupCtrl itemGroup; // 资源组
    public ShopCtrl shop;           // 商店
    public ExpBoxCtrl expBox;       // 宝箱
    public SkinCtrl skin;           // 皮肤商店
    public HealthCtrl health;       // 体力
    public ActivityCtrl activity;   // 活动
    public RankCtrl rank;   // 排行榜

    private void Start()
    {
        // 初始化游戏配置和存档
        Init();
    }

    public void Init()
    {
        Instance = this;

        // 初始化配置
        TextAsset text = Resources.Load<TextAsset>("LocationJson/GameSetting");
        JsonData setting = JsonMapper.ToObject(text.text);
        gameSetting = new GameSettingCtrl(setting["GameSetting"]);
        level = new LevelCtrl();
        resource = JsonMapper.ToObject<ResourceCtrl>(setting["Item"].ToJson());
        itemGroup = new ItemGroupCtrl(setting["ItemGroup"]);
        shop = new ShopCtrl(setting["Shop"]);
        expBox = new ExpBoxCtrl(setting["ExpBox"]);
        skin = new SkinCtrl(setting["Skin"]);
        health = new HealthCtrl();
        activity = JsonMapper.ToObject<ActivityCtrl>(setting["Activity"].ToJson());
        activity.CreateSubActivity(setting);
        rank = new RankCtrl(setting["Rank"], setting["RankReward"]); ;

        // 读取存档
        string keepin = SaveDataManager.GetString("sv_framework_data");
        JsonData savedData = string.IsNullOrEmpty(keepin) ? new JsonData() : JsonMapper.ToObject(keepin);
        level.Init(savedData.ContainsKey("level") ? savedData["level"] : null);
        resource.Init(savedData.ContainsKey("resource") ? savedData["resource"] : null);
        shop.Init(savedData.ContainsKey("shop") ? savedData["shop"] : null);
        expBox.Init(savedData.ContainsKey("exp_box") ? savedData["exp_box"] : null);
        skin.Init(savedData.ContainsKey("skin") ? savedData["skin"] : null);
        health.Init(savedData.ContainsKey("health") ? savedData["health"] : null);
        activity.Init(savedData.ContainsKey("activity") ? savedData["activity"] : null);
        rank.Init(savedData.ContainsKey("rank") ? savedData["rank"] : null);

#if UNITY_EDITOR
        // 展示初始数据
        Debug.Log("数据初始化完成");
        SaveData();
#endif

        //InvokeRepeating(nameof(HandleInterval), 3, 1);
    }

    /// <summary>
    /// 存档
    /// </summary>
    public void SaveData()
    {
        //Debug.Log("Before data save: " + SaveDataManager.GetString("sv_framework_data"));
        Dictionary<string, Dictionary<string, object>> data = new()
        {
            { "level", level.GetSerializeData() },
            { "resource", resource.GetSerializeData() },
            { "shop", shop.GetSerializeData() },
            { "exp_box", expBox.GetSerializeData() },
            { "skin", skin.GetSerializeData() },
            { "health", health.GetSerializeData() },
            { "activity", activity.GetSerializeData() },
            { "rank", rank.GetSerializeData() }
        };

        string saveDataStr = JsonMapper.ToJson(data);
        if (!saveDataStr.Equals(SaveDataManager.GetString("sv_framework_data")))
        {
            SaveDataManager.SetString("sv_framework_data", saveDataStr);
        }
        //Debug.Log("After data save:" + JsonMapper.ToJson(data));
    }

    /// <summary>
    /// 每秒执行的函数，处理例如更新活动状态等
    /// </summary>
    private void HandleInterval()
    {
        activity.UpdateActivityState();

        health.CalcCurrentHealth();
    }

}
