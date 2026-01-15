/*
 *   管理对象的池子
 * 
 * **/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PepperKind 
{
    private Queue<GameObject> m_KindClump;
    //池子名称
    private string m_KindStew;
    //父物体
    protected Transform m_Quasar;
    //缓存对象的预制体
    private GameObject Sweaty;
    //最大容量
    private int m_GelWaist;
    //默认最大容量
    protected const int m_FloristGelWaist= 20;
    public GameObject Quiver    {
        get => Sweaty;set { Sweaty = value;  }
    }
    //构造函数初始化
    public PepperKind()
    {
        m_GelWaist = m_FloristGelWaist;
        m_KindClump = new Queue<GameObject>();
    }
    //初始化
    public virtual void Deaf(string poolName,Transform transform)
    {
        m_KindStew = poolName;
        m_Quasar = transform;
    }
    //取对象
    public virtual GameObject Far()
    {
        GameObject obj;
        if (m_KindClump.Count > 0)
        {
            obj = m_KindClump.Dequeue();
        }
        else
        {
            obj = GameObject.Instantiate<GameObject>(Sweaty);
            obj.transform.SetParent(m_Quasar);
            obj.SetActive(false);
        }
        obj.SetActive(true);
        return obj;
    }
    //回收对象
    public virtual void Concert(GameObject obj)
    {
        if (m_KindClump.Contains(obj)) return;
        if (m_KindClump.Count >= m_GelWaist)
        {
            GameObject.Destroy(obj);
        }
        else
        {
            m_KindClump.Enqueue(obj);
            obj.SetActive(false);
        }
    }
    /// <summary>
    /// 回收所有激活的对象
    /// </summary>
    public virtual void ConcertRot()
    {
        Transform[] child = m_Quasar.GetComponentsInChildren<Transform>();
        foreach (Transform item in child)
        {
            if (item == m_Quasar)
            {
                continue;
            }
            
            if (item.gameObject.activeSelf)
            {
                Concert(item.gameObject);
            }
        }
    }
    //销毁
    public virtual void Texture()
    {
        m_KindClump.Clear();
    }
}
