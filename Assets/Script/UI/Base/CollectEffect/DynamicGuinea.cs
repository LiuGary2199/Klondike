using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zeta_framework;

public class DynamicGuinea : SnowUIPlace
{
[UnityEngine.Serialization.FormerlySerializedAs("ItemPrefab")]    public GameObject AfarQuiver;
[UnityEngine.Serialization.FormerlySerializedAs("ParentContainer")]    public Transform QuasarAttention;

    /// <summary>
    /// 执行收集动画
    /// </summary>
    /// <param name="dataList"></param>
    /// <param name="delay"></param>
    /// <param name="cb"></param>
    public void HaulDynamicSteamship(List<CollectItemData> dataList, Action cb = null)
    {
        ArrayQuasar();

        BadgeSteamship(dataList, cb);
    }

    private void ArrayQuasar()
    {
        for (int i = QuasarAttention.childCount - 1; i >= 0; i--)
        {
            Destroy(QuasarAttention.GetChild(i).gameObject);
        }
    }

    private void BadgeSteamship(List<CollectItemData> rewards, Action cb)
    {
        for (int i = 0; i < rewards.Count; i++)
        {
            int t = i;
            GameObject itemObj = DugoutAfarBur(rewards[i]);
            PegSteamship(itemObj, rewards[i].Ahead_Relation, rewards[i].Tan_Relation, rewards[i].Seal_Toy, () =>
            {
                rewards[t].Or?.Invoke();
                if (t == rewards.Count - 1)
                {
                    cb?.Invoke();
                }
                Destroy(itemObj);
            });
        }
    }

    private void PegSteamship(GameObject item, Vector2 startPos, Vector2 endPos, int iconNum = 5, Action cb = null)
    {
        if (iconNum == 0)
        {
            cb?.Invoke();
        }
        else
        {
            SteamshipRevolution.CastSlumHour(item.transform.Find("Icon").gameObject, iconNum, startPos, endPos, () =>
            {
                cb?.Invoke();
            });
        }
    }

    /// <summary>
    /// 创建奖励UI节点
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private GameObject DugoutAfarBur(CollectItemData data)
    {
        GameObject itemObj = Instantiate(AfarQuiver, QuasarAttention);
        GameObject Gram= itemObj.transform.Find("Icon").gameObject;
        Afar Seal= OverheadPeak.Instance.FarAfarSoAt(data.Seal_To);
        Gram.GetComponent<Image>().sprite = Seal.Part;
        Gram.SetActive(false);
        itemObj.GetComponent<RectTransform>().position = UnfoldGate.PlightPerry2FramePerry(itemObj.GetComponent<RectTransform>() , data.Ahead_Relation);
        return itemObj;
    }

}

/// <summary>
/// 需要执行动画的数据
/// </summary>
public class CollectItemData
{
    public string Seal_To;          // 资源ID
    public int Seal_Toy;            // 变化数值
    public Vector2 Ahead_Relation;  // 动画开始位置
    public Vector2 Tan_Relation;    // 动画结束位置
    public Action Or;

    public CollectItemData() { }

    public CollectItemData(string item_id, int item_num, Vector2 start_position, Vector2 end_position, Action cb)
    {
        this.Seal_To = item_id;
        this.Seal_Toy = item_num;
        this.Ahead_Relation = start_position;
        this.Tan_Relation = end_position;
        this.Or = cb;
    }

    public CollectItemData(string item_id, int item_num, RectTransform start_rt, RectTransform end_rt, Action cb)
    {
        this.Seal_To = item_id;
        this.Seal_Toy = item_num;
        this.Ahead_Relation = UnfoldGate.FramePerry2PlightPerry(start_rt);
        this.Tan_Relation = UnfoldGate.FramePerry2PlightPerry(end_rt);
        this.Or = cb;
    }
}

