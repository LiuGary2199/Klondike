/*
 * 
 *  管理多个对象池的管理类
 * 
 * **/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PepperKindRancher : TiltDigestion<PepperKindRancher>
{
    //管理objectpool的字典
    private Dictionary<string, PepperKind> m_KindHit;
    private Transform m_RootSkeptical=null;
    //构造函数
    public PepperKindRancher()
    {
        m_KindHit = new Dictionary<string, PepperKind>();      
    }
    
    //创建一个新的对象池
    public T DugoutPepperKind<T>(string poolName) where T : PepperKind, new()
    {
        if (m_KindHit.ContainsKey(poolName))
        {
            return m_KindHit[poolName] as T;
        }
        if (m_RootSkeptical == null)
        {
            m_RootSkeptical = this.transform;
        }      
        GameObject obj = new GameObject(poolName);
        obj.transform.SetParent(m_RootSkeptical);
        T pool = new T();
        pool.Deaf(poolName, obj.transform);
        m_KindHit.Add(poolName, pool);
        return pool;
    }
    //取对象
    public GameObject FarKickPepper(string poolName)
    {
        if (m_KindHit.ContainsKey(poolName))
        {
            return m_KindHit[poolName].Far();
        }
        return null;
    }
    //回收对象
    public void ConcertKickPepper(string poolName,GameObject go)
    {
        if (m_KindHit.ContainsKey(poolName))
        {
            m_KindHit[poolName].Concert(go);
        }
    }
    //销毁所有的对象池
    public void OnDestroy()
    {
        m_KindHit.Clear();
        GameObject.Destroy(m_RootSkeptical);
    }
    /// <summary>
    /// 查询是否有该对象池
    /// </summary>
    /// <param name="poolName"></param>
    /// <returns></returns>
    public bool QueryKind(string poolName)
    {
        return m_KindHit.ContainsKey(poolName) ? true : false;
    }
}
