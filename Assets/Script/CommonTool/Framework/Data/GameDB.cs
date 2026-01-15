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
	public string value_type; // 属性类型
	public string comment; // 注释
}

public class ItemDB
{
	public string id; // 资源ID(名称)
	public string value_type; // 属性类型
	public string comment; // 注释
	public string icon; // 图标
	public int defaultValue; // 默认值
	public int minValue; // 最小值
	public int maxValue; // 最大值
	public int type; // 资源类型(1、消耗类、2、经验类)
}

public class ItemGroupDB
{
	public string id; // 资源组ID
	public string item_id; // 资源ID
	public int item_num; // 资源数量
}

public class ShopDB
{
	public string id; // 商店ID
	public string itemgroup_id; // 对应ItemGroup表中的id
	public string gp_pid; // GooglePlay的pid
	public string ios_pid; // AppStore的pid
	public string shop_icon; // 商品图标
	public string title; // 商品名称
	public int purchase_type; // 购买类型：1:现金；2:金币;3:钻石
	public double price; // 价格
	public bool is_show; // 是否在商店中展示
	public int num; // 数量（每日限购）
}

public class ExpBoxDB
{
	public string box_id; // 宝箱类型/活动id
	public int level; // 经验宝箱等级
	public string exp_key; // 升级所需资源
	public int exp_value; // 升级所需资源值
	public string itemgroup_id; // 奖励(对应ItemGroup表的id)
	public string item_id; // 奖励(对应Item表的id)
	public int item_value; // 奖励值
}

public class SkinDB
{
	public string item_id; // 皮肤对应的itemID
	public string skin_type; // 皮肤分类
	public int unlock_type; // 解锁类型，1:经验自动解锁;2:金币购买;3:现金购买;4:自定义解锁
	public string unlock_value; // 解锁条件值
}

public class ActivityDB
{
	public string id; // 活动名称
	public string value_type; // 属性类型
	public string comment; // 注释
	public int unlock_level; // 解锁关卡
	public int start_time; // 第一次活动开始时间的时间戳
	public int duration; // 活动的持续时间（秒）
	public int period; // 每期活动开始时间的间隔（秒）
	public int phases; // 活动的期数（-1:无限期）
	public int start_type; // 开始方式
	public string prefab; // 活动图标
	public string panel; // 活动prefab
	public bool auto_settlement; // 活动结束自动结算
	public bool overlap; // 两期活动是否可重叠
	public string auto_open_time; // 自动打开弹窗时机
	public int auto_open_priority; // 弹出打开优先级
}

public class ActivityDailyGiftDB
{
	public int day; // 第几天
	public string itemgroup_id; // 奖励资源组id
	public string item_id; // 奖励资源id
	public int item_num; // 奖励数量
}

public class ActivityEndlessTreasureDB
{
	public string id; // ID
	public string itemgroup_id; // 奖励资源组id
	public string item_id; // 奖励资源id
	public int item_num; // 奖励数量
	public string shop_id; // 商店ID
	public string color; // UI背景色
}

public class RankDB
{
	public string rank_id; // 榜单ID
	public string activity_id; // 活动ID
	public string item_id; // 榜单资源ID
	public int item_num_type; // 资源累计类型
	public bool clear_item; // 清榜后是否清空资源
	public int max_ranking; // 榜单显示前n名
}

public class RankRewardDB
{
	public string rank_id; // 榜单ID
	public int min_rank; // 最小排名
	public int max_rank; // 最大排名
	public string itemgroup_id; // 奖励id
	public int item_num; // 获得奖励所需资源最小数量
}


/// <summary>
/// 1. 资源类为名为'Item'的Sheet中的配置
/// 2. 表格约定：id为属性名称，value_type为属性类型，comment为注释
/// Generate From GameSetting.xlsx -> Sheet[Item]
/// </summary>
public class ResourceDB
{
	public Item gold { get; set; } // 金币
	public Item diamond { get; set; } // 钻石
	public Item health { get; set; } // 体力
	public Item unlimit_health { get; set; } // 无限体力
	public Item exp { get; set; } // 经验
	public Item consecutive_wins { get; set; } // 连胜
	public Item remove_ad { get; set; } // 免广告
	public Item skin_gb_1 { get; set; } // 皮肤1
	public Item goldleaf { get; set; } // 金箔
	public Item star { get; set; } // 星星
}

/// <summary>
/// 1. 资源类为名为'GameSetting'的Sheet中的配置
/// 2. 表格约定：id为属性名称，value_type为属性类型，comment为注释
/// Generate From GameSetting.xlsx -> Sheet[GameSetting]
/// </summary>
public class SettingDB
{
	public int health_recharge_interval { get; set; } // 体力恢复时间间隔
	public int health_cost { get; set; } // 每关体力消耗
	public int last_lv_strategy_Collection { get; set; } // 全部关卡都通过后奖励策略-宝箱（0、不给奖励；1：按最后一关奖励；2：从第一级循环）
	public string GooglePublicKey { get; set; } // 内购 - google公钥
	public string AppleRootCert { get; set; } // 内购 - Apple证书
}

/// <summary>
/// 1. 资源类为名为'Activity'的Sheet中的配置
/// 2. 表格约定：id为属性名称，value_type为属性类型，comment为注释
/// Generate From GameSetting.xlsx -> Sheet[Activity]
/// </summary>
public class ActivityCtrlDB
{
	public ActivityDailyGiftCtrl DailyGift { get; set; } // 签到奖励
	public ActivityRemoveAdCtrl RemoveAd { get; set; } // 去广告
	public Activity GoldBox { get; set; } // 金箔宝箱
	public ActivityEndlessTreasureCtrl EndlessTreasure { get; set; } // 无尽宝藏
	public Activity RankStar { get; set; } // 星星排行榜
}

}
// End of Auto Generated Code
