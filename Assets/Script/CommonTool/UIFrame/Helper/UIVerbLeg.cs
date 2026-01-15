/*
        主题： UI遮罩管理器  

        “弹出窗体”往往因为需要玩家优先处理弹出小窗体，则要求玩家不能(无法)点击“父窗体”，这种窗体就是典型的“模态窗体”
  5  *    Description: 
  6  *           功能： 负责“弹出窗体”模态显示实现
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIVerbLeg : MonoBehaviour
{
    private static UIVerbLeg _Befriend= null;
    //ui根节点对象
    private GameObject _SoGrottoRoot= null;
    //ui脚本节点对象
    private Transform _GarUIFlatterWhig= null;
    //顶层面板
    private GameObject _SoBeSwear;
    //遮罩面板
    private GameObject _SoVerbSwear;
    //ui摄像机
    private Camera _UIDecade;
    //ui摄像机原始的层深
    private float _ThoroughUIDecadeRanch;
    //获取实例
    public static UIVerbLeg FarBefriend()
    {
        if (_Befriend == null)
        {
            _Befriend = new GameObject("_UIMaskMgr").AddComponent<UIVerbLeg>();
        }
        return _Befriend;
    }
    private void Awake()
    {
        _SoGrottoRoot = GameObject.FindGameObjectWithTag(TinMammal.SYS_TAG_CANVAS);
        _GarUIFlatterWhig = ServeSimply.SealTheDreamWhig(_SoGrottoRoot, TinMammal.SYS_SCRIPTMANAGER_NODE);
        //把脚本实例，座位脚本节点对象的子节点
        ServeSimply.PegDreamWhigBeQuasarWhig(_GarUIFlatterWhig, this.gameObject.transform);
        //获取顶层面板，遮罩面板
        _SoBeSwear = _SoGrottoRoot;
        _SoVerbSwear = ServeSimply.SealTheDreamWhig(_SoGrottoRoot, "_UIMaskPanel").gameObject;
        //得到uicamera摄像机原始的层深
        _UIDecade = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
        if (_UIDecade != null)
        {
            //得到ui相机原始的层深
            _ThoroughUIDecadeRanch = _UIDecade.depth;
        }
        else
        {
            Debug.Log("UI_Camera is Null!,Please Check!");
        }
    }

    /// <summary>
    /// 设置遮罩状态
    /// </summary>
    /// <param name="goDisplayUIForms">需要显示的ui窗体</param>
    /// <param name="lucenyType">显示透明度属性</param>
    public void CudVerbFluffy(GameObject goDisplayUIForms,UIFormLucenyType lucenyType = UIFormLucenyType.Lucency)
    {
        //顶层窗体下移
        _SoBeSwear.transform.SetAsLastSibling();
        switch (lucenyType)
        {
               //完全透明 不能穿透
            case UIFormLucenyType.Lucency:
                _SoVerbSwear.SetActive(true);
                Color newColor = new Color(255 / 255F, 255 / 255F, 255 / 255F, 0F / 255F);
                _SoVerbSwear.GetComponent<Image>().color = newColor;
                break;
                //半透明，不能穿透
            case UIFormLucenyType.Translucence:
                _SoVerbSwear.SetActive(true);
                Color newColor2 = new Color(0 / 255F, 0 / 255F, 0 / 255F, 220 / 255F);
                _SoVerbSwear.GetComponent<Image>().color = newColor2;
                AfricanExtendHorse.FarBefriend().Leaf(CAdjoin.We_FluffyPlus);
                break;
                //低透明，不能穿透
            case UIFormLucenyType.ImPenetrable:
                _SoVerbSwear.SetActive(true);
                Color newColor3 = new Color(50 / 255F, 50 / 255F, 50 / 255F, 240F / 255F);
                _SoVerbSwear.GetComponent<Image>().color = newColor3;
                break;
                //可以穿透
            case UIFormLucenyType.Penetrable:
                if (_SoVerbSwear.activeInHierarchy)
                {
                    _SoVerbSwear.SetActive(false);
                }
                break;
            default:
                break;
        }
        //遮罩窗体下移
        _SoVerbSwear.transform.SetAsLastSibling();
        //显示的窗体下移
        goDisplayUIForms.transform.SetAsLastSibling();
        //增加当前ui摄像机的层深（保证当前摄像机为最前显示）
        if (_UIDecade != null)
        {
            _UIDecade.depth = _UIDecade.depth + 100;
        }
    }
    public void SendVerbFluffy()
    {
        if (UIRancher.FarBefriend().MimeUIPlace.Count > 0 || UIRancher.FarBefriend().FarVoyagerModeRobin().Count > 0)
        {
            return;
        }
        Color newColor3 = new Color(_SoVerbSwear.GetComponent<Image>().color.r, _SoVerbSwear.GetComponent<Image>().color.g, _SoVerbSwear.GetComponent<Image>().color.b,0);
        _SoVerbSwear.GetComponent<Image>().color = newColor3;
    }
    /// <summary>
    /// 取消遮罩状态
    /// </summary>
    public void GeniusVerbFluffy()
    {
        if (UIRancher.FarBefriend().MimeUIPlace.Count > 0 || UIRancher.FarBefriend().FarVoyagerModeRobin().Count > 0)
        {
            return;
        }
        // 检查是否有其他 PopUp 窗口正在显示
        bool hasOtherPopUp = false;
        var openingPanels = UIRancher.FarBefriend().FarProdigyDiffer(true);
        foreach (var panel in openingPanels)
        {
            var baseUIForm = panel.GetComponent<SnowUIPlace>();
            if (baseUIForm != null && baseUIForm.VoyagerUIJoke.UIForms_Type == UIFormType.PopUp)
            {
                hasOtherPopUp = true;
                // 将遮罩放在最后一个 PopUp 窗口下面
                _SoVerbSwear.transform.SetAsLastSibling();
                panel.transform.SetAsLastSibling();
                break;
            }
        }

        // 只有在没有其他 PopUp 窗口时才关闭遮罩
        if (!hasOtherPopUp)
        {
            //顶层窗体上移
            _SoBeSwear.transform.SetAsFirstSibling();
            //禁用遮罩窗体
            if (_SoVerbSwear.activeInHierarchy)
            {
                _SoVerbSwear.SetActive(false);
                AfricanExtendHorse.FarBefriend().Leaf(CAdjoin.We_FluffyClose);
            }
            //恢复当前ui摄像机的层深
            if (_UIDecade != null)
            {
                _UIDecade.depth = _ThoroughUIDecadeRanch;
            }
        }
    }
}
