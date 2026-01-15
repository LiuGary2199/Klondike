using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using zeta_framework;

/// <summary>
/// 发奖动画统一管理
/// </summary>

[Serializable]
public class CollectEndTransform
{
    [FormerlySerializedAs("item_id")]
    public string item_id;
    [HideInInspector]
    public Vector2 EndPosition;
    [FormerlySerializedAs("CollectItemUI")]
    public CollectItemUI CollectItemUI;
}

public class CollectManager : MonoBehaviour
{
    public static CollectManager Instance;

    private  List<List<CollectItemData>> collectSequence;   // 本次需要执行的动画数据列表

    private CollectEffect collectEffect;    // CollectEffect Panel

    private int callBackNum;    // 需要等待的UI回调次数（比如当前同时执行3个发奖动画，需要等到3次UI回调，才能执行下一步）
    private int receivedCallbackNum;

    private Dictionary<string, CollectItemUI> collectComponentDict; // 资源收集动画结束位置转成字典，方便读取
    private Action cb;  // 完整收集动画后的回调

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

    }

    /// <summary>
    /// 将需要执行收集动画的组件，注册到列表中
    /// </summary>
    /// <param name="component"></param>
    public void RegisterCollectionComponent(CollectItemUI component)
    {
        // 收集动画组件字典
        collectComponentDict ??= new Dictionary<string, CollectItemUI>();
        if (!collectComponentDict.ContainsKey(component.item_id))
        {
            collectComponentDict.Add(component.item_id, component);
        }
    }

    /// <summary>
    /// 执行下一个收集动画
    /// </summary>
    public void Next()
    {
        if (collectSequence.Count == 0)
        {
            UIManager.GetInstance().CloseOrReturnUIForms("CollectEffect");
            cb?.Invoke();
            return;
        }

        receivedCallbackNum++;
        if (receivedCallbackNum >= callBackNum)
        {
            // 执行下一组动画
            List<CollectItemData> dataList = collectSequence[0];
            callBackNum = dataList.Count;
            receivedCallbackNum = 0;
            // 如果动画没有设置结束位置，从“收集动画结束位置字典”中寻找是否设置了资源的结束位置
            dataList.ForEach(itemData => {
                if (itemData.end_position == Vector2.zero && collectComponentDict.ContainsKey(itemData.item_id))
                {
                    itemData.end_position = CommonUtil.LocalPoint2ScreenPoint(collectComponentDict[itemData.item_id].EndPoint);
                }
                if (itemData.cb == null)
                {
                    if (collectComponentDict.ContainsKey(itemData.item_id))
                    {
                        itemData.cb = collectComponentDict[itemData.item_id].CollectAnimationCb;
                    }
                    else
                    {
                        itemData.cb = Next;
                    }
                }
            });
            collectEffect.PlayCollectAnimation(dataList);

            collectSequence.RemoveAt(0);
        }
    }

    /// <summary>
    /// 插入一个动画
    /// </summary>
    /// <param name="data"></param>
    public void InsertSequence(List<CollectItemData> data)
    {
        collectSequence.Insert(0, data);
    }

    /// <summary>
    /// 从注册的组件中查看是否需要开启动画，开始收集动画
    /// </summary>
    public void PlayCollectAnimation(Action _cb = null)
    {
        cb = _cb;
        if (collectComponentDict == null || collectComponentDict.Count == 0)
        {
            return;
        }
        // 从各组件中，获取需要执行收集动画的数据
        Dictionary<int, List<CollectItemData>> collectionDict = new();
        foreach(CollectItemUI component in collectComponentDict.Values) {
            CollectItemData componentCollectData = component.GetCollectionData(out int serialNum);
            if (serialNum > 0 && componentCollectData != null)
            {
                if (!collectionDict.ContainsKey(serialNum))
                {
                    collectionDict.Add(serialNum, new());
                }
                collectionDict[serialNum].Add(componentCollectData);
            }
        }

        if (collectionDict.Count == 0)
        {
            return;
        }

        collectSequence = new();
        List<int> keys = new List<int>(collectionDict.Keys);
        keys.Sort();
        keys.ForEach(key =>
        {
            collectSequence.Add(collectionDict[key]);
        });

        OpenCollectEffectPanel();
    }

    public void PlayCollectAnimation(List<ItemGroup> list, Action _cb = null)
    {
        if (list == null || list.Count == 0)
        {
            return;
        }
        List<CollectItemData> dataList = new();
        list.ForEach(itemGroup => {
            CollectItemData data = new(itemGroup.item_id, itemGroup.item_num, Vector2.zero, Vector2.zero, null);
            dataList.Add(data);
        });
        PlayCollectAnimation(dataList, _cb);
    }

    public void PlayCollectAnimation(List<CollectItemData> dataList, Action _cb = null)
    {
        cb = _cb;
        collectSequence = new();
        collectSequence.Add(dataList);

        OpenCollectEffectPanel();
    }

    private void OpenCollectEffectPanel()
    {
        GameObject panel = UIManager.GetInstance().ShowUIForms("CollectEffect");
        collectEffect = panel.GetComponent<CollectEffect>();

        callBackNum = 0;
        Next();
    }
}
