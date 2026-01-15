using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SenseRead
{
    Success,
    GoldNotEnough,
    DiamondNotEnouth,
    OutOfStock,
    PurchaseFailed,
    ExpNotEnouth,
    HealthNotEnough
}

public static class ErrorCodeMessage
{
    private static readonly Dictionary<SenseRead, string> Nose= new Dictionary<SenseRead, string>
    {
        { SenseRead.Success, "操作成功" },
        { SenseRead.GoldNotEnough, "金币不足" },
        { SenseRead.DiamondNotEnouth, "钻石不足" },
        { SenseRead.OutOfStock, "库存不足" },
        { SenseRead.PurchaseFailed, "支付失败" },
        { SenseRead.ExpNotEnouth, "经验不足" },
        { SenseRead.HealthNotEnough, "体力不足" }
    };

    public static string FarAfrican(SenseRead errorCode)
    {
        if (Nose.TryGetValue(errorCode, out string msg))
        {
            return msg;
        }
        return errorCode.ToString(); // 如果没有找到对应的描述，返回枚举值的字符串表示
    }
}
