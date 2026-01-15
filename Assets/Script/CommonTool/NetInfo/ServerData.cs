using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//登录服务器返回数据
public class RootData
{
    public int code { get; set; }
    public string msg { get; set; }
    public ServerData data { get; set; }
}
//用户登录信息
public class ServerUserData
{
    public int code { get; set; }
    public string msg { get; set; }
    public int data { get; set; }
}
//服务器的数据
public class ServerData
{
    public string init { get; set; }
    public string version { get; set; }

    public string apple_pie { get; set; }
    public string inter_b2f_count { get; set; }
    public string inter_freq { get; set; }
    public string relax_interval { get; set; }
    public string trial_MaxNum { get; set; }
    public string nextlevel_interval { get; set; }
    public string adjust_init_rate_act { get; set; }
    public string adjust_init_act_position { get; set; }
    public string adjust_init_adrevenue { get; set; }
    public string soho_shop { get; set; }

    public string game_data { get; set; }

    public string fall_down { get; set; }
    public string HeiNameList { get; set; } //IP黑名单列表
    public string LocationList { get; set; } //黑位置列表
    public string HeiCity { get; set; } //城市黑名单列表
}
public class Init
{
    public List<SlotItem> slot_group { get; set; }

    public double[] cash_random { get; set; }
    public MultiGroup[] cash_group { get; set; }
    public MultiGroup[] gold_group { get; set; }
    public MultiGroup[] amazon_group { get; set; }
}

public class GameData
{
    public int prop_price; //使用道具价格
    public List<RewardData> uparea_collectcard_data_list; //上部接龙区 收集奖励
    public List<RewardData> uparea_filp_data_list; //上部翻牌区 翻开奖励
    public double uparea_filp_reward_rate; //上翻牌区 翻开牌触发奖励概率
    public List<SpecialGameMulti> downarea_specialgame_weight_group; //下部接龙区 翻开牌触发特殊小游戏的权重  ScratchCard LuckyWheelCount Money Null
    public List<RewardData> downarea_filp_money_data_list; //下部接龙区 翻开牌触发直接发钱奖励
    public int scratch_win_max_count; //刮刮卡最大获奖数量
    public List<RewardData> scratch_data_list; //刮刮卡奖励
    public List<RewardData> wheel_reward_weight_group; //转盘奖励
    public List<RewardData> slots_data_list; //小slot奖励
    public WheelMultis wheel_reward_multi; //转盘奖励倍率
    public int levelcomplete_reward_count; //关卡完成奖励数量
    public int[] star_score; //星级所需分数
    public int[] star_reward;
    public int[] star_reward_gold; //星级奖励金币
    public int[] star_reward_cash; //星级奖励现金
    public List<RewardData> flybubble_data_list; //飞行气泡奖励
}

public class SlotItem
{
    public double multi { get; set; }
    public int weight { get; set; }
}

public class MultiGroup
{
    public int max { get; set; }
    public int multi { get; set; }
}

public class RewardData
{
    public int weight { get; set; }
    public RewardType type { get; set; }
    public float num { get; set; }
    public int numMax { get; set; }
    public int numMin { get; set; }
}

public class WheelMultis
{
    public List<SlotItem> cash;
    public List<SlotItem> gold { get; set; }
    public List<SlotItem> tip;
    public List<SlotItem> undo;
}

public class SpecialGameMulti
{
    public string name { get; set; }
    public int weight { get; set; }
}

public class UserRootData
{
    public int code { get; set; }
    public string msg { get; set; }
    public string data { get; set; }
}

public class LocationData
{
    public double X;
    public double Y;
    public double Radius;
}

public class UserInfoData
{
    public double lat;
    public double lon;
    public string query; //ip地址
    public string regionName; //地区名称
    public string city; //城市名称
    public bool IsHaveApple; //是否有苹果
}

