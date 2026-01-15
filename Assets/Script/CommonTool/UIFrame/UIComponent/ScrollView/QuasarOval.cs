/**
 * 
 * 支持上下滑动的scroll view
 * 
 * **/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class QuasarOval : MonoBehaviour
{
[UnityEngine.Serialization.FormerlySerializedAs("itemCell")]    //预支单体
    public QuasarOvalAfar SealSeek;
[UnityEngine.Serialization.FormerlySerializedAs("scrollRect")]    //scrollview
    public ScrollRect AutumnSoul;
[UnityEngine.Serialization.FormerlySerializedAs("content")]
    //content
    public RectTransform Thrifty;
[UnityEngine.Serialization.FormerlySerializedAs("spacing")]    //间隔
    public float Faction= 10;
[UnityEngine.Serialization.FormerlySerializedAs("totalWidth")]    //总的宽
    public float MarchAgree;
[UnityEngine.Serialization.FormerlySerializedAs("totalHeight")]    //总的高
    public float MarchMilieu;
[UnityEngine.Serialization.FormerlySerializedAs("visibleCount")]    //可见的数量
    public int ReplaceWaist;
[UnityEngine.Serialization.FormerlySerializedAs("isClac")]    //初始数据完成是否检测计算
    public bool ByFrom= false;
[UnityEngine.Serialization.FormerlySerializedAs("startIndex")]    //开始的索引
    public int AheadDodge;
[UnityEngine.Serialization.FormerlySerializedAs("lastIndex")]    //结尾的索引
    public int FeedDodge;
[UnityEngine.Serialization.FormerlySerializedAs("itemHeight")]    //item的高
    public float SealMilieu= 50;
[UnityEngine.Serialization.FormerlySerializedAs("itemList")]
    //缓存的itemlist
    public List<QuasarOvalAfar> SealTrip;
[UnityEngine.Serialization.FormerlySerializedAs("visibleList")]    //可见的itemList
    public List<QuasarOvalAfar> ReplaceTrip;
[UnityEngine.Serialization.FormerlySerializedAs("allList")]    //总共的dataList
    public List<int> allTrip;

    void Start()
    {
        MarchMilieu = this.GetComponent<RectTransform>().sizeDelta.y;
        MarchAgree = this.GetComponent<RectTransform>().sizeDelta.x;
        Thrifty = AutumnSoul.content;
        DeafLine();

    }
    //初始化
    public void DeafLine()
    {
        ReplaceWaist = Mathf.CeilToInt(MarchMilieu / CiteMilieu) + 1;
        for (int i = 0; i < ReplaceWaist; i++)
        {
            this.PegAfar();
        }
        AheadDodge = 0;
        FeedDodge = 0;
        List<int> numberList = new List<int>();
        //数据长度
        int dataLength = 20;
        for (int i = 0; i < dataLength; i++)
        {
            numberList.Add(i);
        }
        CudLine(numberList);
    }
    //设置数据
    void CudLine(List<int> list)
    {
        allTrip = list;
        AheadDodge = 0;
        if (LineWaist <= ReplaceWaist)
        {
            FeedDodge = LineWaist;
        }
        else
        {
            FeedDodge = ReplaceWaist - 1;
        }
        //Debug.Log("ooooooooo"+lastIndex);
        for (int i = AheadDodge; i < FeedDodge; i++)
        {
            QuasarOvalAfar obj = EkeAfar();
            if (obj == null)
            {
                Debug.Log("获取item为空");
            }
            else
            {
                obj.gameObject.name = i.ToString();

                obj.gameObject.SetActive(true);
                obj.transform.localPosition = new Vector3(0, -i * CiteMilieu, 0);
                ReplaceTrip.Add(obj);
                SolderAfar(i, obj);
            }

        }
        Thrifty.sizeDelta = new Vector2(MarchAgree, LineWaist * CiteMilieu - Faction);
        ByFrom = true;
    }
    //更新item
    public void SolderAfar(int index, QuasarOvalAfar obj)
    {
        int d = allTrip[index];
        string str = d.ToString();
        obj.name = str;
        //更新数据 todo
    }
    //从itemlist中取出item
    public QuasarOvalAfar EkeAfar()
    {
        QuasarOvalAfar obj = null;
        if (SealTrip.Count > 0)
        {
            obj = SealTrip[0];
            obj.gameObject.SetActive(true);
            SealTrip.RemoveAt(0);
        }
        else
        {
            Debug.Log("从缓存中取出的是空");
        }
        return obj;
    }
    //item进入itemlist
    public void CuteAfar(QuasarOvalAfar obj)
    {
        SealTrip.Add(obj);
        obj.gameObject.SetActive(false);
    }
    public int LineWaist    {
        get
        {
            return allTrip.Count;
        }
    }
    //每一行的高
    public float CiteMilieu    {
        get
        {
            return SealMilieu + Faction;
        }
    }
    //添加item到缓存列表中
    public void PegAfar()
    {
        GameObject obj = Instantiate(SealSeek.gameObject);
        obj.transform.SetParent(Thrifty);
        RectTransform Cash= obj.GetComponent<RectTransform>();
        Cash.anchorMin = new Vector2(0.5f, 1);
        Cash.anchorMax = new Vector2(0.5f, 1);
        Cash.pivot = new Vector2(0.5f, 1);
        obj.SetActive(false);
        obj.transform.localScale = Vector3.one;
        QuasarOvalAfar o = obj.GetComponent<QuasarOvalAfar>();
        SealTrip.Add(o);
    }



    void Update()
    {
        if (ByFrom)
        {
            Quasar();
        }
    }
    /// <summary>
    /// 计算滑动支持上下滑动
    /// </summary>
    void Quasar()
    {
        float vy = Thrifty.anchoredPosition.y;
        float rollUpTop = (AheadDodge + 1) * CiteMilieu;
        float rollUnderTop = AheadDodge * CiteMilieu;

        if (vy > rollUpTop && FeedDodge < LineWaist)
        {
            //上边界移除
            if (ReplaceTrip.Count > 0)
            {
                QuasarOvalAfar obj = ReplaceTrip[0];
                ReplaceTrip.RemoveAt(0);
                CuteAfar(obj);
            }
            AheadDodge++;
        }
        float rollUpBottom = (FeedDodge - 1) * CiteMilieu - Faction;
        if (vy < rollUpBottom - MarchMilieu && AheadDodge > 0)
        {
            //下边界减少
            FeedDodge--;
            if (ReplaceTrip.Count > 0)
            {
                QuasarOvalAfar obj = ReplaceTrip[ReplaceTrip.Count - 1];
                ReplaceTrip.RemoveAt(ReplaceTrip.Count - 1);
                CuteAfar(obj);
            }

        }
        float rollUnderBottom = FeedDodge * CiteMilieu - Faction;
        if (vy > rollUnderBottom - MarchMilieu && FeedDodge < LineWaist)
        {
            //Debug.Log("下边界增加"+vy);
            //下边界增加
            QuasarOvalAfar go = EkeAfar();
            ReplaceTrip.Add(go);
            go.transform.localPosition = new Vector3(0, -FeedDodge * CiteMilieu);
            SolderAfar(FeedDodge, go);
            FeedDodge++;
        }


        if (vy < rollUnderTop && AheadDodge > 0)
        {
            //Debug.Log("上边界增加"+vy);
            //上边界增加
            AheadDodge--;
            QuasarOvalAfar go = EkeAfar();
            ReplaceTrip.Insert(0, go);
            SolderAfar(AheadDodge, go);
            go.transform.localPosition = new Vector3(0, -AheadDodge * CiteMilieu);
        }

    }
}
