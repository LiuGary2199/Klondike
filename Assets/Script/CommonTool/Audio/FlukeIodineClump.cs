/***
 * 
 * AudioSource组件管理(音效，背景音乐除外)
 * 
 * **/
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FlukeIodineClump 
{
    //音乐的管理者
    private GameObject FlukeLeg;
    //音乐组件管理队列
    private List<AudioSource> FlukePrimitiveClump;
    //音乐组件默认容器最大值  
    private int GelWaist= 25;
    public FlukeIodineClump(LeafyLeg audioMgr)
    {
        FlukeLeg = audioMgr.gameObject;
        DeafFlukeIodineClump();
    }
  
    /// <summary>
    /// 初始化队列
    /// </summary>
    private void DeafFlukeIodineClump()
    {
        FlukePrimitiveClump = new List<AudioSource>();
        for(int i = 0; i < GelWaist; i++)
        {
            PegFlukeIodineHutJeanLeg();
        }
    }
    /// <summary>
    /// 给音乐的管理者添加音频组件，同时组件加入队列
    /// </summary>
    private AudioSource PegFlukeIodineHutJeanLeg()
    {
        AudioSource audio = FlukeLeg.AddComponent<AudioSource>();
        FlukePrimitiveClump.Add(audio);
        return audio;
    }
    /// <summary>
    /// 获取一个音频组件
    /// </summary>
    /// <param name="audioMgr"></param>
    /// <returns></returns>
    public AudioSource FarFlukePrimitive()
    {
        if (FlukePrimitiveClump.Count > 0)
        {
            AudioSource audio = FlukePrimitiveClump.Find(t => !t.isPlaying);
            if (audio)
            {
                FlukePrimitiveClump.Remove(audio);
                return audio;
            }
            //队列中没有了，需额外添加
            return PegFlukeIodineHutJeanLeg();
            //直接返回队列中存在的组件
            //return AudioComponentQueue.Dequeue();
        }
        else
        {
            //队列中没有了，需额外添加
            return  PegFlukeIodineHutJeanLeg();
        }
    }
    /// <summary>
    /// 没有被使用的音频组件返回给队列
    /// </summary>
    /// <param name="audio"></param>
    public void IfBogFlukePrimitive(AudioSource audio)
    {
        if (FlukePrimitiveClump.Contains(audio)) return;
        if (FlukePrimitiveClump.Count >= GelWaist)
        {
            GameObject.Destroy(audio);
            //Debug.Log("删除组件");
        }
        else
        {
            audio.clip = null;
            FlukePrimitiveClump.Add(audio);
        }

        //Debug.Log("队列长度是" + AudioComponentQueue.Count);
    }
    
}
