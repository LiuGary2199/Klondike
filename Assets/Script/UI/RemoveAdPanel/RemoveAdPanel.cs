using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zeta_framework;

/// <summary>
/// 去广告弹窗
/// </summary>
public class RemoveAdPanel : BaseUIForms
{
    public Button CloseButton;
    public Button BuyButton;
    
    // Start is called before the first frame update
    void Start()
    {
        CloseButton.onClick.AddListener(() => {
            CloseUIForm(GetType().Name);
        });

        BuyButton.onClick.AddListener(() => {
            ActivityRemoveAdCtrl.Instance.Buy((errorCode) => {
                if (errorCode == ErrorCode.Success)
                {
                    HomePanel.Instance.ShowCollectAnimation(ActivityDailyGiftCtrl.Instance.GetRewardByIndex(0));
                    CloseUIForm(GetType().Name);
                }
                else
                {
                    ToastManager.GetInstance().ShowToast(ErrorCodeMessage.GetMessage(errorCode));
                }
            });
        });
    }

    public override void Hidding()
    {
        base.Hidding();
        //ActivityAutoOpenManager.Instance.OpenNext();
    }
}
