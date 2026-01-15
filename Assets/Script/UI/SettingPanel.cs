using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BaseUIForms
{
    public Button Sound_Button;
    public Button Music_Button;
    public Button Vibrate_Button;
    public Button AutoBtn;
    public Button Continue_Button;
    public Button ReStartBtn;


    public override void Display(object uiFormParams)
    {
        base.Display(uiFormParams);
        Sound_Button.transform.Find("ON").gameObject.SetActive(MusicMgr.GetInstance().EffectMusicSwitch);
        Sound_Button.transform.Find("OFF").gameObject.SetActive(!MusicMgr.GetInstance().EffectMusicSwitch);
        Music_Button.transform.Find("ON").gameObject.SetActive(MusicMgr.GetInstance().BgMusicSwitch);
        Music_Button.transform.Find("OFF").gameObject.SetActive(!MusicMgr.GetInstance().BgMusicSwitch);
        Vibrate_Button.transform.Find("ON").gameObject.SetActive(MusicMgr.GetInstance().VibrateSwitch);
        Vibrate_Button.transform.Find("OFF").gameObject.SetActive(!MusicMgr.GetInstance().VibrateSwitch);
        AutoBtn.transform.Find("ON").gameObject.SetActive(Klondike_Manager.Instance.AutoCollectSwitch);
        AutoBtn.transform.Find("OFF").gameObject.SetActive(!Klondike_Manager.Instance.AutoCollectSwitch);
    }
    // Start is called before the first frame update
    void Start()
    {
        Continue_Button.onClick.AddListener(() =>
        {
            CloseUIForm(GetType().Name);
        });

        Music_Button.onClick.AddListener(() =>
        {
            MusicMgr.GetInstance().BgMusicSwitch = !MusicMgr.GetInstance().BgMusicSwitch;
            Music_Button.transform.Find("ON").gameObject.SetActive(MusicMgr.GetInstance().BgMusicSwitch);
            Music_Button.transform.Find("OFF").gameObject.SetActive(!MusicMgr.GetInstance().BgMusicSwitch);
        });
        Sound_Button.onClick.AddListener(() =>
        {
            MusicMgr.GetInstance().EffectMusicSwitch = !MusicMgr.GetInstance().EffectMusicSwitch;
            Sound_Button.transform.Find("ON").gameObject.SetActive(MusicMgr.GetInstance().EffectMusicSwitch);
            Sound_Button.transform.Find("OFF").gameObject.SetActive(!MusicMgr.GetInstance().EffectMusicSwitch);
        });
        Vibrate_Button.onClick.AddListener(() =>
        {
            MusicMgr.GetInstance().VibrateSwitch = !MusicMgr.GetInstance().VibrateSwitch;
            Vibrate_Button.transform.Find("ON").gameObject.SetActive(MusicMgr.GetInstance().VibrateSwitch);
            Vibrate_Button.transform.Find("OFF").gameObject.SetActive(!MusicMgr.GetInstance().VibrateSwitch);
        });
        AutoBtn.onClick.AddListener(() =>
        {
            Klondike_Manager.Instance.AutoCollectSwitch = !Klondike_Manager.Instance.AutoCollectSwitch;
            AutoBtn.transform.Find("ON").gameObject.SetActive(Klondike_Manager.Instance.AutoCollectSwitch);
            AutoBtn.transform.Find("OFF").gameObject.SetActive(!Klondike_Manager.Instance.AutoCollectSwitch);
            if (Klondike_Manager.Instance.AutoCollectSwitch)
                ToastManager.GetInstance().ShowToast("Move cards to the foundatin automatically");
            else
                ToastManager.GetInstance().ShowToast("Stop moving cards to the foundatin automatically");
        });
        ReStartBtn.onClick.AddListener(() =>
        {
            Klondike_Manager.Instance.ReStart();
            CloseUIForm(GetType().Name);
        });
    }

}
