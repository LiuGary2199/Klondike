using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 基础UI窗体脚本（父类，其他窗体都继承此脚本）
/// </summary>
public class SnowUIPlace : MonoBehaviour
{
[UnityEngine.Serialization.FormerlySerializedAs("_CurrentUIType")]    //当前（基类）窗口的类型
    public UIJoke _VoyagerUIJoke= new UIJoke();
    [HideInInspector]
[UnityEngine.Serialization.FormerlySerializedAs("close_button")]    public Button Stuff_Notice;
    //属性，当前ui窗体类型
    internal UIJoke VoyagerUIJoke    {
        set
        {
            _VoyagerUIJoke = value;
        }
        get
        {
            return _VoyagerUIJoke;
        }
    }
    protected virtual void Awake()
    {
        SealDreamPegPrimitive(gameObject);
        if (transform.Find("Window/Content/CloseBtn"))
        {
            Stuff_Notice = transform.Find("Window/Content/CloseBtn").GetComponent<Button>();
            Stuff_Notice.onClick.AddListener(() => {
                UIRancher.FarBefriend().FirstOrTavernUIPlace(this.GetType().Name);
            });
        }
        if (_VoyagerUIJoke.UIForms_Type == UIFormType.PopUp)
        {
            gameObject.AddComponent<CanvasGroup>();
        }
        gameObject.name = GetType().Name;
    }


    public static void SealDreamPegPrimitive(GameObject goParent)
    {
        Transform parent = goParent.transform;
        int childCount = parent.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform chile = parent.GetChild(i);
            if (chile.GetComponent<Button>())
            {
                chile.GetComponent<Button>().onClick.AddListener(() => {

                    LeafyLeg.FarBefriend().HaulGuinea(LeafyJoke.UIMusic.Sound_UIButton);
                });
            }
            
            if (chile.childCount > 0)
            {
                SealDreamPegPrimitive(chile.gameObject);
            }
        }
    }

    //页面显示
    public virtual void Display(object uiFormParams)
    {
        //Debug.Log(this.GetType().Name);
        this.gameObject.SetActive(true);
        // 设置模态窗体调用(必须是弹出窗体)
        if (_VoyagerUIJoke.UIForms_Type == UIFormType.PopUp && _VoyagerUIJoke.UIForm_LucencyType != UIFormLucenyType.NoMask)
        {
            UIVerbLeg.FarBefriend().CudVerbFluffy(this.gameObject, _VoyagerUIJoke.UIForm_LucencyType);
        }
        if (_VoyagerUIJoke.UIForms_Type == UIFormType.PopUp)
        {

            //动画添加
            switch (_VoyagerUIJoke.UIForm_animationType)
            {
                case UIFormShowAnimationType.scale:
                    SteamshipRevolution.EkeBind(gameObject, () =>
                    {

                    });
                    break;

            }
            
        }
        //NewUserManager.GetInstance().TriggerEvent(TriggerType.panel_display);
    }
    //页面隐藏（不在栈集合中）
    public virtual void Hidding(System.Action finish = null)
    {
        //if (_CurrentUIType.UIForms_Type == UIFormType.PopUp && _CurrentUIType.UIForm_LucencyType != UIFormLucenyType.NoMask)
        //{
        //    UIVerbLeg.GetInstance().HideMaskWindow();
        //}

        //取消模态窗体调用

        if (_VoyagerUIJoke.UIForms_Type == UIFormType.PopUp)
        {
            switch (_VoyagerUIJoke.UIForm_animationType)
            {
                case UIFormShowAnimationType.scale:
                    SteamshipRevolution.EkeSend(gameObject, () =>
                    {
                        this.gameObject.SetActive(false);
                        if (_VoyagerUIJoke.UIForms_Type == UIFormType.PopUp && _VoyagerUIJoke.UIForm_LucencyType != UIFormLucenyType.NoMask)
                        {
                            UIVerbLeg.FarBefriend().GeniusVerbFluffy();
                        }
                        UIRancher.FarBefriend().BindOpenEkeOf();
                        finish?.Invoke();
                    });
                    break;
                case UIFormShowAnimationType.none:
                    this.gameObject.SetActive(false);
                    if (_VoyagerUIJoke.UIForms_Type == UIFormType.PopUp && _VoyagerUIJoke.UIForm_LucencyType != UIFormLucenyType.NoMask)
                    {
                        UIVerbLeg.FarBefriend().GeniusVerbFluffy();
                    }
                    UIRancher.FarBefriend().BindOpenEkeOf();
                    finish?.Invoke();
                    break;

            }

        }
        else
        {
            this.gameObject.SetActive(false);
            //if (_CurrentUIType.UIForms_Type == UIFormType.PopUp && _CurrentUIType.UIForm_LucencyType != UIFormLucenyType.NoMask)
            //{
            //    UIVerbLeg.GetInstance().CancelMaskWindow();
            //}
            finish?.Invoke();
        }
    }

    public virtual void Hidding()
    {
        Hidding(null);
    }

    //页面重新显示
    public virtual void Redisplay()
    {
        this.gameObject.SetActive(true);
        if (_VoyagerUIJoke.UIForms_Type == UIFormType.PopUp)
        {
            UIVerbLeg.FarBefriend().CudVerbFluffy(this.gameObject, _VoyagerUIJoke.UIForm_LucencyType); 
        }
    }
    //页面冻结（还在栈集合中）
    public virtual void Radius()
    {
        this.gameObject.SetActive(true);
    }

    /// <summary>
    /// 注册按钮事件
    /// </summary>
    /// <param name="buttonName">按钮节点名称</param>
    /// <param name="delHandle">委托，需要注册的方法</param>
    protected void SentenceGibbonPepperInuit(string buttonName,InuitDepositMongolia.VoidDelegate delHandle)
    {
        GameObject goButton = ServeSimply.SealTheDreamWhig(this.gameObject, buttonName).gameObject;
        //给按钮注册事件方法
        if (goButton != null)
        {
            InuitDepositMongolia.Far(goButton).onSully = delHandle;
        }
    }

    /// <summary>
    /// 打开ui窗体
    /// </summary>
    /// <param name="uiFormName"></param>
    protected GameObject PlusUIMode(string uiFormName)
    {
        return UIRancher.FarBefriend().BindUIPlace(uiFormName);
    }

    /// <summary>
    /// 关闭当前ui窗体
    /// </summary>
    protected void FirstUIMode(string uiFormName)
    {
        //处理后的uiform名称
        UIRancher.FarBefriend().FirstOrTavernUIPlace(uiFormName);
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="msgType">消息的类型</param>
    /// <param name="msgName">消息名称</param>
    /// <param name="msgContent">消息内容</param>
    protected void LeafAfrican(string msgType,string msgName,object msgContent)
    {
        KeyValuesUpdate kvs = new KeyValuesUpdate(msgName, msgContent);
        AfricanExtend.LeafAfrican(msgType, kvs);
    }

    /// <summary>
    /// 接受消息
    /// </summary>
    /// <param name="messageType">消息分类</param>
    /// <param name="handler">消息委托</param>
    public void NeutronAfrican(string messageType,AfricanExtend.DelMessageDelivery handler)
    {
        AfricanExtend.PegOatMongolia(messageType, handler);
    }

    /// <summary>
    /// 显示语言
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public string Bind(string id)
    {
        string strResult = string.Empty;
        strResult = AnalysisLeg.FarBefriend().BindMoss(id);
        return strResult;
    }
}
