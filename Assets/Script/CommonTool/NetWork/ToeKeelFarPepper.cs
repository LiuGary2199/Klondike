/***
 * 
 * 网络请求的get对象
 * 
 * **/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class ToeKeelFarPepper 
{
    //get的url
    public string Pit;
    //get成功的回调
    public Action<UnityWebRequest> FarDepress;
    //get失败的回调
    public Action FarGobi;
    public ToeKeelFarPepper(string url,Action<UnityWebRequest> success,Action fail)
    {
        Pit = url;
        FarDepress = success;
        FarGobi = fail;
    }
   
}
