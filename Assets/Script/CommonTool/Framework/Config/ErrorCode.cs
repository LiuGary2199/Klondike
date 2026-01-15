using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ErrorCode
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
    private static readonly Dictionary<ErrorCode, string> msgs = new Dictionary<ErrorCode, string>
    {
        { ErrorCode.Success, "操作成功" },
        { ErrorCode.GoldNotEnough, "金币不足" },
        { ErrorCode.DiamondNotEnouth, "钻石不足" },
        { ErrorCode.OutOfStock, "库存不足" },
        { ErrorCode.PurchaseFailed, "支付失败" },
        { ErrorCode.ExpNotEnouth, "经验不足" },
        { ErrorCode.HealthNotEnough, "体力不足" }
    };

    public static string GetMessage(ErrorCode errorCode)
    {
        if (msgs.TryGetValue(errorCode, out string msg))
        {
            return msg;
        }
        return errorCode.ToString(); // 如果没有找到对应的描述，返回枚举值的字符串表示
    }
}
