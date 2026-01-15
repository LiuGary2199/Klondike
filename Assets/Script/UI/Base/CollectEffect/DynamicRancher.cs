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
    public string Seal_To;
    [HideInInspector]
    public Vector2 RidCitation;
    [FormerlySerializedAs("DynamicAfarUI")]
    public DynamicAfarUI DynamicAfarUI;
}

public class DynamicRancher : MonoBehaviour
{
    public static DynamicRancher Instance;

    private  List<List<CollectItemData>> GaseousInstinct;   // 本次需要执行的动画数据列表

    private DynamicGuinea GaseousGuinea;    // DynamicGuinea Panel

    private int TopiSortLad;    // 需要等待的UI回调次数（比如当前同时执行3个发奖动画，需要等到3次UI回调，才能执行下一步）
    private int PinpointVarianceLad;

    private Dictionary<string, DynamicAfarUI> GaseousPrimitiveMate; // 资源收集动画结束位置转成字典，方便读取
    private Action Or;  // 完整收集动画后的回调

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

    }

    /// <summary>
    /// 将需要执行收集动画的组件，注册到列表中
    /// </summary>
    /// <param name="component"></param>
    public void IroquoisConsultantPrimitive(DynamicAfarUI component)
    {
        // 收集动画组件字典
        GaseousPrimitiveMate ??= new Dictionary<string, DynamicAfarUI>();
        if (!GaseousPrimitiveMate.ContainsKey(component.Seal_To))
        {
            GaseousPrimitiveMate.Add(component.Seal_To, component);
        }
    }

    /// <summary>
    /// 执行下一个收集动画
    /// </summary>
    public void Open()
    {
        if (GaseousInstinct.Count == 0)
        {
            UIRancher.FarBefriend().FirstOrTavernUIPlace("DynamicGuinea");
            Or?.Invoke();
            return;
        }

        PinpointVarianceLad++;
        if (PinpointVarianceLad >= TopiSortLad)
        {
            // 执行下一组动画
            List<CollectItemData> dataList = GaseousInstinct[0];
            TopiSortLad = dataList.Count;
            PinpointVarianceLad = 0;
            // 如果动画没有设置结束位置，从“收集动画结束位置字典”中寻找是否设置了资源的结束位置
            dataList.ForEach(itemData => {
                if (itemData.Tan_Relation == Vector2.zero && GaseousPrimitiveMate.ContainsKey(itemData.Seal_To))
                {
                    itemData.Tan_Relation = UnfoldGate.FramePerry2PlightPerry(GaseousPrimitiveMate[itemData.Seal_To].RidPerry);
                }
                if (itemData.Or == null)
                {
                    if (GaseousPrimitiveMate.ContainsKey(itemData.Seal_To))
                    {
                        itemData.Or = GaseousPrimitiveMate[itemData.Seal_To].DynamicSteamshipCb;
                    }
                    else
                    {
                        itemData.Or = Open;
                    }
                }
            });
            GaseousGuinea.HaulDynamicSteamship(dataList);

            GaseousInstinct.RemoveAt(0);
        }
    }

    /// <summary>
    /// 插入一个动画
    /// </summary>
    /// <param name="data"></param>
    public void AttainInstinct(List<CollectItemData> data)
    {
        GaseousInstinct.Insert(0, data);
    }

    /// <summary>
    /// 从注册的组件中查看是否需要开启动画，开始收集动画
    /// </summary>
    public void HaulDynamicSteamship(Action _cb = null)
    {
        Or = _cb;
        if (GaseousPrimitiveMate == null || GaseousPrimitiveMate.Count == 0)
        {
            return;
        }
        // 从各组件中，获取需要执行收集动画的数据
        Dictionary<int, List<CollectItemData>> collectionDict = new();
        foreach(DynamicAfarUI component in GaseousPrimitiveMate.Values) {
            CollectItemData componentCollectData = component.FarConsultantLine(out int serialNum);
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

        GaseousInstinct = new();
        List<int> keys = new List<int>(collectionDict.Keys);
        keys.Sort();
        keys.ForEach(key =>
        {
            GaseousInstinct.Add(collectionDict[key]);
        });

        PlusDynamicGuineaSwear();
    }

    public void HaulDynamicSteamship(List<AfarPerry> list, Action _cb = null)
    {
        if (list == null || list.Count == 0)
        {
            return;
        }
        List<CollectItemData> dataList = new();
        list.ForEach(itemGroup => {
            CollectItemData Mark= new(itemGroup.Seal_To, itemGroup.Seal_Toy, Vector2.zero, Vector2.zero, null);
            dataList.Add(Mark);
        });
        HaulDynamicSteamship(dataList, _cb);
    }

    public void HaulDynamicSteamship(List<CollectItemData> dataList, Action _cb = null)
    {
        Or = _cb;
        GaseousInstinct = new();
        GaseousInstinct.Add(dataList);

        PlusDynamicGuineaSwear();
    }

    private void PlusDynamicGuineaSwear()
    {
        GameObject Hilly= UIRancher.FarBefriend().BindUIPlace("DynamicGuinea");
        GaseousGuinea = Hilly.GetComponent<DynamicGuinea>();

        TopiSortLad = 0;
        Open();
    }
}
