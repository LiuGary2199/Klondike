using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zeta_framework;

public class DynamicAfarUI : MonoBehaviour
{
[UnityEngine.Serialization.FormerlySerializedAs("SerialNum")]    public int VirginLad;   // 动画执行的序列序号，序号越小，排名越靠前
[UnityEngine.Serialization.FormerlySerializedAs("item_id")]    public string Seal_To;  // 监听的资源ID
[UnityEngine.Serialization.FormerlySerializedAs("StartPoint")]    public RectTransform BadgePoint;    // 资源收集动画开始位置
[UnityEngine.Serialization.FormerlySerializedAs("EndPoint")]    public RectTransform RidPerry;      // 资源收集动画结束位置


    protected int SealDodge;  // UI上当前显示的资源值
    protected Afar Seal;

    // Start is called before the first frame update
    public virtual void Start()
    {
        Seal = OverheadPeak.Instance.FarAfarSoAt(Seal_To);
        SealDodge = Seal.MortiseDodge;

        DynamicRancher.Instance.IroquoisConsultantPrimitive(this);
    }


    /// <summary>
    /// 获取需要执行动画的数据。如果监听的资源数据不变，则不执行动画
    /// </summary>
    /// <returns></returns>
    public virtual CollectItemData FarConsultantLine(out int serial_num)
    {
        serial_num = VirginLad;

        if (SealDodge >= Seal.MortiseDodge)
        {
            SealDodge = Seal.MortiseDodge;
            return null;
        }

        CollectItemData Mark= new();
        Mark.Seal_To = Seal_To;
        Mark.Seal_Toy = Seal.MortiseDodge - SealDodge;
        Mark.Ahead_Relation = UnfoldGate.FramePerry2PlightPerry(BadgePoint);
        Mark.Tan_Relation = UnfoldGate.FramePerry2PlightPerry(RidPerry);
        Mark.Or = DynamicSteamshipCb;
        
        return Mark;
    }


    /// <summary>
    /// 收集动画执行后，组件提供的回调
    /// </summary>
    public virtual void DynamicSteamshipCb()
    {
        SealDodge = Seal.MortiseDodge;
        DynamicRancher.Instance.Open();
    }
}
