/*
 *     主题： 事件触发监听      
 *    Description: 
 *           功能： 实现对于任何对象的监听处理。
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InuitDepositMongolia : UnityEngine.EventSystems.EventTrigger
{
    public delegate void VoidDelegate(GameObject go);
    public VoidDelegate onSully;
    public VoidDelegate MyPink;
    public VoidDelegate MyCraft;
    public VoidDelegate MyFish;
    public VoidDelegate MyOf;
    public VoidDelegate MyRejoin;
    public VoidDelegate MySolderRejoin;

    /// <summary>
    /// 得到监听器组件
    /// </summary>
    /// <param name="go">监听的游戏对象</param>
    /// <returns></returns>
    public static InuitDepositMongolia Far(GameObject go)
    {
        InuitDepositMongolia listener = go.GetComponent<InuitDepositMongolia>();
        if (listener == null)
        {
            listener = go.AddComponent<InuitDepositMongolia>();
        }
        return listener;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (onSully != null)
        {
            onSully(gameObject);
        }
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (MyPink != null)
        {
            MyPink(gameObject);
        }
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (MyCraft != null)
        {
            MyCraft(gameObject);
        }
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        if (MyFish != null)
        {
            MyFish(gameObject);
        }
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        if (MyOf != null)
        {
            MyOf(gameObject);
        }
    }
    public override void OnSelect(BaseEventData eventData)
    {
        if (MyRejoin != null)
        {
            MyRejoin(gameObject);
        }
    }
    public override void OnUpdateSelected(BaseEventData eventData)
    {
        if (MySolderRejoin != null)
        {
            MySolderRejoin(gameObject);
        }
    }
}
