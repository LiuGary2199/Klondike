using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zeta_framework;

public class HomePanel : BaseUIForms
{
    public static HomePanel Instance;

    public GameObject GoldBar;
    public GameObject DiamondBar;
    public GameObject HealthBar;
    public Button PlayButton;


    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        PlayButton.onClick.AddListener(() => {
            ErrorCode errorCode = LevelCtrl.Instance.StartLevel();
            if (errorCode != ErrorCode.Success)
            {
                ToastManager.GetInstance().ShowToast(ErrorCodeMessage.GetMessage(errorCode));
            }
        });
    }

    /// <summary>
    /// 展示资源收集动画
    /// </summary>
    /// <param name="items"></param>
    public void ShowCollectAnimation(Action cb = null)
    {
        if (!gameObject.activeInHierarchy)
        {
            // 如果是用UIManager的ShowUIForms打开的HomePanel，请修改下面的打开方式
            //GamePanel.Instance.OpenTab("Home");
        }

        CollectManager.Instance.PlayCollectAnimation(cb);
    }

    public void ShowCollectAnimation(List<ItemGroup> list, Action cb = null) {
        if (!gameObject.activeInHierarchy)
        {
            // 如果是用UIManager的ShowUIForms打开的HomePanel，请修改下面的打开方式
            //GamePanel.Instance.OpenTab("Home");
        }

        CollectManager.Instance.PlayCollectAnimation(list, cb);
    }

}
