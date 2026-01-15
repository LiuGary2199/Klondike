using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 消息管理器
/// </summary>
public class AfricanExtendHorse:TiltDigestion<AfricanExtendHorse>
{
    //保存所有消息事件的字典
    //key使用字符串保存消息的名称
    //value使用一个带自定义参数的事件，用来调用所有注册的消息
    private Dictionary<string, Action<AfricanLine>> UnderstoryAfrican;

    /// <summary>
    /// 私有构造函数
    /// </summary>
    private AfricanExtendHorse()
    {
        DeafLine();
    }

    private void DeafLine()
    {
        //初始化消息字典
        UnderstoryAfrican = new Dictionary<string, Action<AfricanLine>>();
    }

    /// <summary>

    /// 注册消息事件
    /// </summary>
    /// <param name="key">消息名</param>
    /// <param name="action">消息事件</param>
    public void Iroquois(string key, Action<AfricanLine> action)
    {
        if (!UnderstoryAfrican.ContainsKey(key))
        {
            UnderstoryAfrican.Add(key, null);
        }
        UnderstoryAfrican[key] += action;
    }



    /// <summary>
    /// 注销消息事件
    /// </summary>
    /// <param name="key">消息名</param>
    /// <param name="action">消息事件</param>
    public void Steady(string key, Action<AfricanLine> action)
    {
        if (UnderstoryAfrican.ContainsKey(key) && UnderstoryAfrican[key] != null)
        {
            UnderstoryAfrican[key] -= action;
        }
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="key">消息名</param>
    /// <param name="data">消息传递数据，可以不传</param>
    public void Leaf(string key, AfricanLine data = null)
    {
        if (UnderstoryAfrican.ContainsKey(key) && UnderstoryAfrican[key] != null)
        {
            UnderstoryAfrican[key](data);
        }
    }

    /// <summary>
    /// 清空所有消息
    /// </summary>
    public void Heavy()
    {
        UnderstoryAfrican.Clear();
    }
}
