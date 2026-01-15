//
// Auto Generated Code By excel2json
// https://neil3d.gitee.io/coding/excel2json.html
// 1. 每个 Sheet 形成一个 Struct 定义, Sheet 的名称作为 Struct 的名称
// 2. 表格约定：第一行是变量名称，第二行是变量类型

// Generate From GameSetting.xlsx

using System;
using System.Collections.Generic;

namespace zeta_framework
{

public class GameSettingDB
{
	public string id; // 配置名称
	public string value; // 配置的值
	public string Noble_type; // 属性类型
	public string Willing; // 注释
}

public class ItemDB
{
	public string id; // 资源ID(名称)
	public string Noble_type; // 属性类型
	public string Willing; // 注释
	public string Gram; // 图标
	public int ContendDodge; // 默认值
	public int LidDodge; // 最小值
	public int WeeDodge; // 最大值
	public int type; // 资源类型(1、消耗类、2、经验类)
}

public class ItemGroupDB
{
	public string id; // 资源组ID
	public string Seal_To; // 资源ID
	public int Seal_Toy; // 资源数量
}

public class ShopDB
{
	public string id; // 商店ID
	public string Infantile_To; // 对应ItemGroup表中的id
	public string gp_Bid; // GooglePlay的pid
	public string ios_Bid; // AppStore的pid
	public string Pont_Gram; // 商品图标
	public string Price; // 商品名称
	public int Nobility_Mode; // 购买类型：1:现金；2:金币;3:钻石
	public double Guest; // 价格
	public bool By_show; // 是否在商店中展示
	public int num; // 数量（每日限购）
}

public class ExpBoxDB
{
	public string Leg_To; // 宝箱类型/活动id
	public int Weave; // 经验宝箱等级
	public string Use_Fix; // 升级所需资源
	public int Use_Noble; // 升级所需资源值
	public string Infantile_To; // 奖励(对应ItemGroup表的id)
	public string Seal_To; // 奖励(对应Item表的id)
	public int Seal_Noble; // 奖励值
}

public class SkinDB
{
	public string Seal_To; // 皮肤对应的itemID
	public string Dash_Mode; // 皮肤分类
	public int Abound_Mode; // 解锁类型，1:经验自动解锁;2:金币购买;3:现金购买;4:自定义解锁
	public string Abound_Noble; // 解锁条件值
}

public class ActivityDB
{
	public string id; // 活动名称
	public string Noble_type; // 属性类型
	public string Willing; // 注释
	public int Abound_Weave; // 解锁关卡
	public int Ahead_Copy; // 第一次活动开始时间的时间戳
	public int Bacteria; // 活动的持续时间（秒）
	public int Offset; // 每期活动开始时间的间隔（秒）
	public int Canvas; // 活动的期数（-1:无限期）
	public int Ahead_Mode; // 开始方式
	public string Sweaty; // 活动图标
	public string Hilly; // 活动prefab
	public bool Seep_Cincinnati; // 活动结束自动结算
	public bool Romance; // 两期活动是否可重叠
	public string Seep_Urge_Copy; // 自动打开弹窗时机
	public int Seep_Urge_Fanciful; // 弹出打开优先级
}

public class ActivityDailyGiftDB
{
	public int Jaw; // 第几天
	public string Infantile_To; // 奖励资源组id
	public string Seal_To; // 奖励资源id
	public int Seal_Toy; // 奖励数量
}

public class ActivityEndlessTreasureDB
{
	public string id; // ID
	public string Infantile_To; // 奖励资源组id
	public string Seal_To; // 奖励资源id
	public int Seal_Toy; // 奖励数量
	public string Pont_To; // 商店ID
	public string color; // UI背景色
}

public class RankDB
{
	public string Hail_To; // 榜单ID
	public string Northern_To; // 活动ID
	public string Seal_To; // 榜单资源ID
	public int Seal_Toy_Mode; // 资源累计类型
	public bool Logic_Seal; // 清榜后是否清空资源
	public int Wee_Undergo; // 榜单显示前n名
}

public class RankRewardDB
{
	public string Hail_To; // 榜单ID
	public int Lid_Hail; // 最小排名
	public int Wee_Hail; // 最大排名
	public string Infantile_To; // 奖励id
	public int Seal_Toy; // 获得奖励所需资源最小数量
}


/// <summary>
/// 1. 资源类为名为'Afar'的Sheet中的配置
/// 2. 表格约定：id为属性名称，value_type为属性类型，comment为注释
/// Generate From GameSetting.xlsx -> Sheet[Afar]
/// </summary>
public class ResourceDB
{
	public Afar Pool{ get; set; } // 金币
	public Afar Shatter{ get; set; } // 钻石
	public Afar Carpet{ get; set; } // 体力
	public Afar Outcrop_Carpet{ get; set; } // 无限体力
	public Afar Use{ get; set; } // 经验
	public Afar Anthocyanin_Limb{ get; set; } // 连胜
	public Afar remove_It{ get; set; } // 免广告
	public Afar Dash_gb_1{ get; set; } // 皮肤1
	public Afar Gravitas{ get; set; } // 金箔
	public Afar Germ{ get; set; } // 星星
}

/// <summary>
/// 1. 资源类为名为'GameSetting'的Sheet中的配置
/// 2. 表格约定：id为属性名称，value_type为属性类型，comment为注释
/// Generate From GameSetting.xlsx -> Sheet[GameSetting]
/// </summary>
public class SettingDB
{
	public int Carpet_Paralyze_Collapse{ get; set; } // 体力恢复时间间隔
	public int Carpet_Seem{ get; set; } // 每关体力消耗
	public int Feed_lv_Fountain_Consultant{ get; set; } // 全部关卡都通过后奖励策略-宝箱（0、不给奖励；1：按最后一关奖励；2：从第一级循环）
	public string ReviveEngageCan{ get; set; } // 内购 - google公钥
	public string MoundRootPipe{ get; set; } // 内购 - Apple证书
}

/// <summary>
/// 1. 资源类为名为'Colonize'的Sheet中的配置
/// 2. 表格约定：id为属性名称，value_type为属性类型，comment为注释
/// Generate From GameSetting.xlsx -> Sheet[Colonize]
/// </summary>
public class ActivityCtrlDB
{
	public ColonizeScoreWhipPeak ScoreWhip{ get; set; } // 签到奖励
	public ColonizeSteadyItPeak SteadyIt{ get; set; } // 去广告
	public Colonize CastTug{ get; set; } // 金箔宝箱
	public ColonizeQuitterClearingPeak QuitterClearing{ get; set; } // 无尽宝藏
	public Colonize LoveFord{ get; set; } // 星星排行榜
}

}
// End of Auto Generated Code
