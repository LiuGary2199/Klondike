using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zeta_framework;

public class CollectItemUI : MonoBehaviour
{
    public int SerialNum;   // 动画执行的序列序号，序号越小，排名越靠前
    public string item_id;  // 监听的资源ID
    public RectTransform StartPoint;    // 资源收集动画开始位置
    public RectTransform EndPoint;      // 资源收集动画结束位置


    protected int itemValue;  // UI上当前显示的资源值
    protected Item item;

    // Start is called before the first frame update
    public virtual void Start()
    {
        item = ResourceCtrl.Instance.GetItemById(item_id);
        itemValue = item.currentValue;

        CollectManager.Instance.RegisterCollectionComponent(this);
    }


    /// <summary>
    /// 获取需要执行动画的数据。如果监听的资源数据不变，则不执行动画
    /// </summary>
    /// <returns></returns>
    public virtual CollectItemData GetCollectionData(out int serial_num)
    {
        serial_num = SerialNum;

        if (itemValue >= item.currentValue)
        {
            itemValue = item.currentValue;
            return null;
        }

        CollectItemData data = new();
        data.item_id = item_id;
        data.item_num = item.currentValue - itemValue;
        data.start_position = CommonUtil.LocalPoint2ScreenPoint(StartPoint);
        data.end_position = CommonUtil.LocalPoint2ScreenPoint(EndPoint);
        data.cb = CollectAnimationCb;
        
        return data;
    }


    /// <summary>
    /// 收集动画执行后，组件提供的回调
    /// </summary>
    public virtual void CollectAnimationCb()
    {
        itemValue = item.currentValue;
        CollectManager.Instance.Next();
    }
}
