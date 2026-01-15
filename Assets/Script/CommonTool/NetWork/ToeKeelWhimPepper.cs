/**
 * 
 * 网络请求的post对象
 * 
 * ***/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class ToeKeelWhimPepper 
{
    //post请求地址
    public string URL;
    //post的数据表单
    public WWWForm Mode;
    //post成功回调
    public Action<UnityWebRequest> WhimDepress;
    //post失败回调
    public Action WhimGobi;
    public ToeKeelWhimPepper(string url,WWWForm  form,Action<UnityWebRequest> success,Action fail)
    {
        URL = url;
        Mode = form;
        WhimDepress = success;
        WhimGobi = fail;
    }
}
