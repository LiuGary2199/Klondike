using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
public class WhimInuitRemove : TiltDigestion<WhimInuitRemove>
{
    public string version = "1.2";
    public string KickRead = ToeBoldLeg.instance.KickRead;
    //channel
#if UNITY_IOS
    private string Pronoun = "AppStore";
#elif UNITY_ANDROID
    private string Channel = "GooglePlay";
#else
    private string Channel = "GooglePlay";
#endif


    private void OnApplicationPause(bool pause)
    {
        WhimInuitRemove.FarBefriend().RiftKickManeuver();
    }

    public Text Cane;

    protected override void Awake()
    {
        base.Awake();

        version = Application.version;
        StartCoroutine(nameof(SeepAfrican));
    }
    IEnumerator SeepAfrican()
    {
        while (true)
        {
            yield return new WaitForSeconds(120f);
            WhimInuitRemove.FarBefriend().RiftKickManeuver();
        }
    }
    private void Start()
    {
        if (ShedLineRancher.FarWit("event_day") != DateTime.Now.Day && ShedLineRancher.FarStench("user_servers_id").Length != 0)
        {
            ShedLineRancher.CudWit("event_day", DateTime.Now.Day);
        }
    }
    public void LeafIfSureInuit(string event_id)
    {
        LeafInuit(event_id);
    }
    public void RiftKickManeuver(List<string> valueList = null)
    {
        if (ShedLineRancher.FarEmbryo(CAdjoin.Or_GenerationCastSlit) == 0)
        {
            ShedLineRancher.CudEmbryo(CAdjoin.Or_GenerationCastSlit, ShedLineRancher.FarEmbryo(CAdjoin.Or_CastSlit));
        }
        if (ShedLineRancher.FarEmbryo(CAdjoin.Or_GenerationGoods) == 0)
        {
            ShedLineRancher.CudEmbryo(CAdjoin.Or_GenerationGoods, ShedLineRancher.FarEmbryo(CAdjoin.Or_Goods));
        }
        if (valueList == null)
        {
            valueList = new List<string>() {
                ShedLineRancher.FarEmbryo(CAdjoin.Or_Goods).ToString(), //现金余额
                ShedLineRancher.FarEmbryo(CAdjoin.Or_GenerationGoods).ToString(), //累计现金
                ShedLineRancher.FarEmbryo(CAdjoin.Or_CastSlit).ToString(), //金币余额
                ShedLineRancher.FarEmbryo(CAdjoin.Or_GenerationCastSlit).ToString(),//累计金币
            };
        }

        if (ShedLineRancher.FarStench(CAdjoin.Or_FrameFleshyAt) == null)
        {
            return;
        }
        WWWForm wwwForm = new WWWForm();
        wwwForm.AddField("gameCode", KickRead);
        wwwForm.AddField("userId", ShedLineRancher.FarStench(CAdjoin.Or_FrameFleshyAt));

        wwwForm.AddField("gameVersion", version);

        wwwForm.AddField("channel", Pronoun);

        for (int i = 0; i < valueList.Count; i++)
        {
            wwwForm.AddField("resource" + (i + 1), valueList[i]);
        }



        StartCoroutine(LeafWhim(ToeBoldLeg.instance.SnowPit + "/api/client/game_progress", wwwForm,
        (error) =>
        {
            Debug.Log(error);
        },
        (message) =>
        {
            //Debug.Log(message);
        }));
    }
    public void LeafInuit(string event_id, string p1 = null, string p2 = null, string p3 = null)
    {
        if (Cane != null)
        {
            if (int.Parse(event_id) < 9100 && int.Parse(event_id) >= 9000)
            {
                if (p1 == null)
                {
                    p1 = "";
                }
                Cane.text += "\n" + DateTime.Now.ToString() + "id:" + event_id + "  p1:" + p1;
            }
        }
        if (ShedLineRancher.FarStench(CAdjoin.Or_FrameFleshyAt) == null)
        {
            ToeBoldLeg.instance.Bless();
            return;
        }
        WWWForm wwwForm = new WWWForm();
        wwwForm.AddField("gameCode", KickRead);
        wwwForm.AddField("userId", ShedLineRancher.FarStench(CAdjoin.Or_FrameFleshyAt));
        //Debug.Log("userId:" + ShedLineRancher.GetString(CAdjoin.sv_LocalServerId));
        wwwForm.AddField("version", version);
        //Debug.Log("version:" + version);
        wwwForm.AddField("channel", Pronoun);
        //Debug.Log("channel:" + channal);
        wwwForm.AddField("operateId", event_id);
        //Debug.Log("打点 事件ID:" + event_id + " 参数1:" + p1 + " 参数2:" + p2 + " 参数3:" + p3);


        if (p1 != null)
        {
            wwwForm.AddField("params1", p1);
        }
        if (p2 != null)
        {
            wwwForm.AddField("params2", p2);
        }
        if (p3 != null)
        {
            wwwForm.AddField("params3", p3);
        }
        StartCoroutine(LeafWhim(ToeBoldLeg.instance.SnowPit + "/api/client/log", wwwForm,
        (error) =>
        {
            Debug.Log(error);
        },
        (message) =>
        {
            //Debug.Log(message);
        }));
    }
    IEnumerator LeafWhim(string _url, WWWForm wwwForm, Action<string> fail, Action<string> success)
    {
        //Debug.Log(SerializeDictionaryToJsonString(dic));
        using UnityWebRequest request = UnityWebRequest.Post(_url, wwwForm);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isNetworkError)
        {
            fail(request.error);
            TanPortend();
        }
        else
        {
            success(request.downloadHandler.text);
            TanPortend();
        }
    }
    private void TanPortend()
    {
        StopCoroutine("SendGet");
    }


}