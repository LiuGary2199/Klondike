/**
 * 
 * 左右滑动的页面视图
 * 
 * ***/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class TaleOval : MonoBehaviour,IBeginDragHandler,IEndDragHandler
{
[UnityEngine.Serialization.FormerlySerializedAs("rect")]    //scrollview
    public ScrollRect Cash;
    //求出每页的临界角，页索引从0开始
    List<float> AilTrip= new List<float>();
[UnityEngine.Serialization.FormerlySerializedAs("isDrag")]    //是否拖拽结束
    public bool ByWaxy= false;
    bool RoleSlum= true;
    //滑动的起始坐标  
    float PotatoSuccessive= 0;
    float AheadWaxySuccessive;
    float startTime = 0f;
[UnityEngine.Serialization.FormerlySerializedAs("smooting")]    //滑动速度  
    public float Catalyze= 1f;
[UnityEngine.Serialization.FormerlySerializedAs("sensitivity")]    public float Counterpart= 0.3f;
[UnityEngine.Serialization.FormerlySerializedAs("OnPageChange")]    //页面改变
    public Action<int> DyTaleUranus;
    //当前页面下标
    int MortiseTaleDodge= -1;
    void Start()
    {
        Cash = this.GetComponent<ScrollRect>();
        float horizontalLength = Cash.content.rect.width - this.GetComponent<RectTransform>().rect.width;
        AilTrip.Add(0);
        for(int i = 1; i < Cash.content.childCount - 1; i++)
        {
            AilTrip.Add(GetComponent<RectTransform>().rect.width * i / horizontalLength);
        }
        AilTrip.Add(1);
    }

    
    void Update()
    {
        if(!ByWaxy && !RoleSlum)
        {
            startTime += Time.deltaTime;
            float t = startTime * Catalyze;
            Cash.horizontalNormalizedPosition = Mathf.Lerp(Cash.horizontalNormalizedPosition, PotatoSuccessive, t);
            if (t >= 1)
            {
                RoleSlum = true;
            }
        }
        
    }
    /// <summary>
    /// 设置页面的index下标
    /// </summary>
    /// <param name="index"></param>
    void CudTaleDodge(int index)
    {
        if (MortiseTaleDodge != index)
        {
            MortiseTaleDodge = index;
            if (DyTaleUranus != null)
            {
                DyTaleUranus(index);
            }
        }
    }
    /// <summary>
    /// 开始拖拽
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        ByWaxy = true;
        AheadWaxySuccessive = Cash.horizontalNormalizedPosition;
    }
    /// <summary>
    /// 拖拽结束
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        float posX = Cash.horizontalNormalizedPosition;
        posX += ((posX - AheadWaxySuccessive) * Counterpart);
        posX = posX < 1 ? posX : 1;
        posX = posX > 0 ? posX : 0;
        int index = 0;
        float offset = Mathf.Abs(AilTrip[index] - posX);
        for(int i = 0; i < AilTrip.Count; i++)
        {
            float temp = Mathf.Abs(AilTrip[i] - posX);
            if (temp < offset)
            {
                index = i;
                offset = temp;
            }
        }
        CudTaleDodge(index);
        PotatoSuccessive = AilTrip[index];
        ByWaxy = false;
        startTime = 0f;
        RoleSlum = false;
    }
}
