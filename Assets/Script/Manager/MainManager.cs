using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager instance;

    private bool ready = false;

    private void Awake()
    {
        instance = this;
    }

    

    public void gameInit()
    {
        bool isNewPlayer = !PlayerPrefs.HasKey(CConfig.sv_IsNewPlayer + "Bool") || SaveDataManager.GetBool(CConfig.sv_IsNewPlayer);
        AdjustInitManager.Instance.InitAdjustData(isNewPlayer);
        if (isNewPlayer)
        {
            // 新用户
            SaveDataManager.SetBool(CConfig.sv_IsNewPlayer, false);
        }

        MusicMgr.GetInstance().PlayBg(MusicType.SceneMusic.Sound_BGM);

        UIManager.GetInstance().ShowUIForms(nameof(GamePanel));

        GameDataManager.GetInstance().InitGameData();

        ready = true;

        PostEventScript.GetInstance().SendEvent("1001");
    }

}
