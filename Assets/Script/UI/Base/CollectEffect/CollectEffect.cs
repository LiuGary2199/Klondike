using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zeta_framework;

public class CollectEffect : BaseUIForms
{
    public GameObject ItemPrefab;
    public Transform ParentContainer;

    /// <summary>
    /// 执行收集动画
    /// </summary>
    /// <param name="dataList"></param>
    /// <param name="delay"></param>
    /// <param name="cb"></param>
    public void PlayCollectAnimation(List<CollectItemData> dataList, Action cb = null)
    {
        CleanParent();

        StartAnimation(dataList, cb);
    }

    private void CleanParent()
    {
        for (int i = ParentContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(ParentContainer.GetChild(i).gameObject);
        }
    }

    private void StartAnimation(List<CollectItemData> rewards, Action cb)
    {
        for (int i = 0; i < rewards.Count; i++)
        {
            int t = i;
            GameObject itemObj = CreateItemObj(rewards[i]);
            AddAnimation(itemObj, rewards[i].start_position, rewards[i].end_position, rewards[i].item_num, () =>
            {
                rewards[t].cb?.Invoke();
                if (t == rewards.Count - 1)
                {
                    cb?.Invoke();
                }
                Destroy(itemObj);
            });
        }
    }

    private void AddAnimation(GameObject item, Vector2 startPos, Vector2 endPos, int iconNum = 5, Action cb = null)
    {
        if (iconNum == 0)
        {
            cb?.Invoke();
        }
        else
        {
            AnimationController.GoldMoveBest(item.transform.Find("Icon").gameObject, iconNum, startPos, endPos, () =>
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
    private GameObject CreateItemObj(CollectItemData data)
    {
        GameObject itemObj = Instantiate(ItemPrefab, ParentContainer);
        GameObject icon = itemObj.transform.Find("Icon").gameObject;
        Item item = ResourceCtrl.Instance.GetItemById(data.item_id);
        icon.GetComponent<Image>().sprite = item.Icon;
        icon.SetActive(false);
        itemObj.GetComponent<RectTransform>().position = CommonUtil.ScreenPoint2LocalPoint(itemObj.GetComponent<RectTransform>() , data.start_position);
        return itemObj;
    }

}

/// <summary>
/// 需要执行动画的数据
/// </summary>
public class CollectItemData
{
    public string item_id;          // 资源ID
    public int item_num;            // 变化数值
    public Vector2 start_position;  // 动画开始位置
    public Vector2 end_position;    // 动画结束位置
    public Action cb;

    public CollectItemData() { }

    public CollectItemData(string item_id, int item_num, Vector2 start_position, Vector2 end_position, Action cb)
    {
        this.item_id = item_id;
        this.item_num = item_num;
        this.start_position = start_position;
        this.end_position = end_position;
        this.cb = cb;
    }

    public CollectItemData(string item_id, int item_num, RectTransform start_rt, RectTransform end_rt, Action cb)
    {
        this.item_id = item_id;
        this.item_num = item_num;
        this.start_position = CommonUtil.LocalPoint2ScreenPoint(start_rt);
        this.end_position = CommonUtil.LocalPoint2ScreenPoint(end_rt);
        this.cb = cb;
    }
}

