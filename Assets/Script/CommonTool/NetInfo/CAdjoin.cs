/**
 * 
 * 常量配置
 * 
 * 
 * **/
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAdjoin
{
    #region 常量字段
    //登录url
    public const string BlessPit= "/api/client/user/getId?gameCode=";
    //配置url
    public const string AdjoinPit= "/api/client/config?gameCode=";
    //时间戳url
    public const string MoldPit= "/api/client/common/current_timestamp?gameCode=";
    //更新AdjustId url
    public const string ShroudPit= "/api/client/user/setAdjustId?gameCode=";
    #endregion

    #region 本地存储的字符串
    /// <summary>
    /// 本地用户id (string)
    /// </summary>
    public const string Or_FramePorkAt= "sv_LocalUserId";
    /// <summary>
    /// 本地服务器id (string)
    /// </summary>
    public const string Or_FrameFleshyAt= "sv_LocalServerId";
    /// <summary>
    /// 是否是新用户玩家 (bool)
    /// </summary>
    public const string Or_UpGunVictim= "sv_IsNewPlayer";

    /// <summary>
    /// 签到次数 (int)
    /// </summary>
    public const string Or_ScoreGlandFarWaist= "sv_DailyBounsGetCount";
    /// <summary>
    /// 签到最后日期 (int)
    /// </summary>
    public const string Or_ScoreGlandWard= "sv_DailyBounsDate";
    /// <summary>
    /// 新手引导完成的步数
    /// </summary>
    public const string Or_GunPorkBoat= "sv_NewUserStep";
    /// <summary>
    /// 金币余额
    /// </summary>
    public const string Or_CastSlit= "sv_GoldCoin";
    /// <summary>
    /// 累计金币总数
    /// </summary>
    public const string Or_GenerationCastSlit= "sv_CumulativeGoldCoin";
    /// <summary>
    /// 钻石/现金余额
    /// </summary>
    public const string Or_Goods= "sv_Token";
    /// <summary>
    /// 累计钻石/现金总数
    /// </summary>
    public const string Or_GenerationGoods= "sv_CumulativeToken";
    /// <summary>
    /// 钻石Amazon
    /// </summary>
    public const string Or_Entity= "sv_Amazon";
    /// <summary>
    /// 累计Amazon总数
    /// </summary>
    public const string Or_GenerationEntity= "sv_CumulativeAmazon";
    /// <summary>
    /// 游戏总时长
    /// </summary>
    public const string Or_EndowKickMold= "sv_TotalGameTime";
    /// <summary>
    /// 第一次获得钻石奖励
    /// </summary>
    public const string Or_BefitFarGoods= "sv_FirstGetToken";
    /// <summary>
    /// 是否已显示评级弹框
    /// </summary>
    public const string Or_PulBindMaskSwear= "sv_HasShowRatePanel";
    /// <summary>
    /// 累计Roblox奖券总数
    /// </summary>
    public const string Or_GenerationOctopus= "sv_CumulativeLottery";
    /// <summary>
    /// 已经通过一次的关卡(int array)
    /// </summary>
    public const string Or_PretendPassMember= "sv_AlreadyPassLevels";
    /// <summary>
    /// 新手引导
    /// </summary>
    public const string Or_GunPorkBoatJobber= "sv_NewUserStepFinish";
    public const string Or_Jolt_Weave_Slice= "sv_task_level_count";
    // 是否第一次使用过slot
    public const string Or_BefitDish= "sv_FirstSlot";
    /// <summary>
    /// adjust adid
    /// </summary>
    public const string Or_ShroudYawn= "sv_AdjustAdid";

    /// <summary>
    /// 广告相关 - trial_num
    /// </summary>
    public const string Or_It_First_Toy= "sv_ad_trial_num";
    /// <summary>
    /// 看广告总次数
    /// </summary>
    public const string Or_March_It_Toy= "sv_total_ad_num";

    #endregion

    #region 监听发送的消息

    /// <summary>
    /// 有窗口打开
    /// </summary>
    public static string We_FluffyPlus= "mg_WindowOpen";
    /// <summary>
    /// 窗口关闭
    /// </summary>
    public static string We_FluffyClose= "mg_WindowClose";
    /// <summary>
    /// 关卡结算时传值
    /// </summary>
    public static string We_ui_Inconsistency= "mg_ui_levelcomplete";
    /// <summary>
    /// 增加金币
    /// </summary>
    public static string We_Me_Miocene= "mg_ui_addgold";
    /// <summary>
    /// 增加钻石/现金
    /// </summary>
    public static string We_Me_Tapestry= "mg_ui_addtoken";
    /// <summary>
    /// 增加amazon
    /// </summary>
    public static string We_Me_Partition= "mg_ui_addamazon";

    /// <summary>
    /// 游戏暂停/继续
    /// </summary>
    public static string We_KickDistant= "mg_GameSuspend";

    /// <summary>
    /// 游戏资源数量变化
    /// </summary>
    public static string We_AfarUranus_= "mg_ItemChange_";

    /// <summary>
    /// 活动状态变更
    /// </summary>
    public static string We_ColonizeWidowUranus_= "mg_ActivityStateChange_";

    /// <summary>
    /// 关卡最大等级变更
    /// </summary>
    public static string We_DodgeGelDodgeUranus= "mg_LevelMaxLevelChange";

    #endregion

    #region 动态加载资源的路径

    // 金币图片
    public static string Dirt_CastSlit_Review= "Art/Tex/UI/jiangli1";
    // 钻石图片
    public static string Dirt_Goods_Review_Mosaic= "Art/Tex/UI/jiangli4";

    #endregion
}

