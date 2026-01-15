using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimyRancher : MonoBehaviour
{
    public static LimyRancher instance;

    private bool Amino = false;

    private void Awake()
    {
        instance = this;
    }

    public void AbutDeaf()
    {
        bool isNewPlayer = !PlayerPrefs.HasKey(CAdjoin.Or_UpGunVictim + "Bool") || ShedLineRancher.FarGray(CAdjoin.Or_UpGunVictim);
        ShroudDeafRancher.Instance.DeafShroudLine(isNewPlayer);
        if (isNewPlayer)
        {
            // 新用户
            ShedLineRancher.CudGray(CAdjoin.Or_UpGunVictim, false);
        }

        LeafyLeg.FarBefriend().HaulHe(LeafyJoke.SceneMusic.Sound_BGM);

        UIRancher.FarBefriend().BindUIPlace(nameof(KickSwear));

        KickLineRancher.FarBefriend().DeafKickLine();

        Amino = true;

        WhimInuitRemove.FarBefriend().LeafInuit("1001");
    }

    //切前后台也检测屏蔽 防止游戏中途更改手机状态
    private void OnApplicationFocus(bool focusStatus)
    {
        if (focusStatus)
            AndroidBlockCheck();
    }
    // 安卓平台特殊屏蔽规则 被屏蔽玩家显示提示 阻止进入
    public bool AndroidBlockCheck()
    {
        // if (Application.platform == RuntimePlatform.Android)
        // {
        //     string BlockReason = UnfoldGate.AndroidBlockCheck();
        //     string Info = "";
        //     if (BlockReason == "Vpn")
        //         Info = "Please turn off your VPN, restart the game and try again.";
        //     else if (BlockReason == "Developer")
        //         Info = "Please switch off Developer Option, restart the game and try again.";
        //     else if (BlockReason == "Usb")
        //         Info = "Please switch off USB debugging, restart the game and try again.";
        //     else if (BlockReason == "SimCard")
        //         Info = "Please check if the SIM card is inserted, then restart the game and try again.";
        //     if (!string.IsNullOrEmpty(BlockReason))
        //     {
        //         UIRancher.FarBefriend().BindUIPlace(nameof(BlockPanel)).GetComponent<BlockPanel>().ShowInfo(Info);
        //         return true;
        //     }
        // }
        return false;
    }

}
