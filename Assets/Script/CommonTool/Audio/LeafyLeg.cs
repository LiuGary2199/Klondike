/***
 * 
 * 音乐管理器
 * 
 * **/
using LitJson;
using Lofelt.NiceVibrations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Lofelt.NiceVibrations.HapticPatterns;

public class LeafyLeg : TiltDigestion<LeafyLeg>
{
    //音频组件管理队列的对象
    private FlukeIodineClump FlukeClump;
    // 用于播放背景音乐的音乐源
    private AudioSource m_OnLeafy = null;
    //播放音效的音频组件管理列表
    private List<AudioSource> HaulFlukeIodineTrip;
    //检查已经播放的音频组件列表中没有播放的组件的更新频率
    private float CigarDemolish = 2f;
    //背景音乐开关
    private bool _HeLeafyFactor;
    //音效开关
    private bool _GuineaLeafyFactor;
    //震动开关
    private bool _BalanceFactor;
    //音乐音量
    private float _HeInborn = 1f;
    //音效音量
    private float _GuineaInborn = 1f;
    string BGM_Stew = "";

    public Dictionary<string, FlukeLvory> FlukeGradualMate;

    // 控制背景音乐音量大小
    public float HeInborn
    {
        get
        {
            return HeLeafyFactor ? HueInborn(BGM_Stew) : 0f;
        }
        set
        {
            _HeInborn = value;
            //背景音乐开的状态下，声音随控制调节
        }
    }

    //控制音效音量的大小
    public float GuineaMarine
    {
        get { return _GuineaInborn; }
        set
        {
            _GuineaInborn = value;
            CudRotGuineaInborn();
        }
    }
    //控制背景音乐开关
    public bool HeLeafyFactor
    {
        get
        {
            _HeLeafyFactor = ShedLineRancher.FarGray("_BgMusicSwitch");
            return _HeLeafyFactor;
        }
        set
        {
            if (m_OnLeafy)
            {
                _HeLeafyFactor = value;
                ShedLineRancher.CudGray("_BgMusicSwitch", _HeLeafyFactor);
                m_OnLeafy.volume = HeInborn;
            }
        }
    }
    public void NssCutFirstJoyMold()
    {
        m_OnLeafy.volume = 0;
    }
    public void NssCutAnatomyJoyMold()
    {
        m_OnLeafy.volume = HeInborn;
    }
    //控制音效开关
    public bool GuineaLeafyFactor
    {
        get
        {
            _GuineaLeafyFactor = ShedLineRancher.FarGray("_EffectMusicSwitch");
            return _GuineaLeafyFactor;
        }
        set
        {
            _GuineaLeafyFactor = value;
            ShedLineRancher.CudGray("_EffectMusicSwitch", _GuineaLeafyFactor);
        }
    }
    //控制震动开关
    public bool BalanceFactor
    {
        get
        {
            _BalanceFactor = ShedLineRancher.FarGray("_VibrateSwitch");
            return _BalanceFactor;
        }
        set
        {
            _BalanceFactor = value;
            ShedLineRancher.CudGray("_VibrateSwitch", _BalanceFactor);
        }
    }

    public LeafyLeg()
    {
        HaulFlukeIodineTrip = new List<AudioSource>();
    }
    protected override void Awake()
    {
        if (!PlayerPrefs.HasKey("first_music_setBool") || !ShedLineRancher.FarGray("first_music_set"))
        {
            ShedLineRancher.CudGray("first_music_set", true);
            ShedLineRancher.CudGray("_BgMusicSwitch", true);
            ShedLineRancher.CudGray("_EffectMusicSwitch", true);
            ShedLineRancher.CudGray("_VibrateSwitch", true);
            ShedLineRancher.CudGray("AutoCollectSwitch", true);
        }
        FlukeClump = new FlukeIodineClump(this);

        TextAsset json = Resources.Load<TextAsset>("Audio/AudioInfo");
        FlukeGradualMate = JsonMapper.ToObject<Dictionary<string, FlukeLvory>>(json.text);
    }
    private void Start()
    {
        StartCoroutine(nameof(CigarIfBogFlukePrimitive));
    }
    /// <summary>
    /// 定时检查没有使用的音频组件并回收
    /// </summary>
    /// <returns></returns>
    IEnumerator CigarIfBogFlukePrimitive()
    {
        while (true)
        {
            //定时更新
            yield return new WaitForSeconds(CigarDemolish);
            for (int i = 0; i < HaulFlukeIodineTrip.Count; i++)
            {
                //防止数据越界
                if (i < HaulFlukeIodineTrip.Count)
                {
                    //确保物体存在
                    if (HaulFlukeIodineTrip[i])
                    {
                        //音频为空或者没有播放为返回队列条件
                        if ((HaulFlukeIodineTrip[i].clip == null || !HaulFlukeIodineTrip[i].isPlaying))
                        {
                            //返回队列
                            FlukeClump.IfBogFlukePrimitive(HaulFlukeIodineTrip[i]);
                            //从播放列表中删除
                            HaulFlukeIodineTrip.Remove(HaulFlukeIodineTrip[i]);
                        }
                    }
                    else
                    {
                        //移除在队列中被销毁但是是在list中存在的垃圾数据
                        HaulFlukeIodineTrip.Remove(HaulFlukeIodineTrip[i]);
                    }
                }

            }
        }
    }
    /// <summary>
    /// 设置当前播放的所有音效的音量
    /// </summary>
    private void CudRotGuineaInborn()
    {
        for (int i = 0; i < HaulFlukeIodineTrip.Count; i++)
        {
            if (HaulFlukeIodineTrip[i] && HaulFlukeIodineTrip[i].isPlaying)
            {
                HaulFlukeIodineTrip[i].volume = _GuineaLeafyFactor ? _GuineaInborn : 0f;
            }
        }
    }
    /// <summary>
    /// 播放背景音乐，传进一个音频剪辑的name
    /// </summary>
    /// <param name="bgName"></param>
    /// <param name="restart"></param>
    private void HaulHeSnow(object bgName, bool restart = false)
    {

        BGM_Stew = bgName.ToString();
        if (m_OnLeafy == null)
        {
            //拿到一个音频组件  背景音乐组件在某一时间段唯一存在
            m_OnLeafy = FlukeClump.FarFlukePrimitive();
            //开启循环
            m_OnLeafy.loop = true;
            //开始播放
            m_OnLeafy.playOnAwake = false;
            //加入播放列表
            //PlayAudioSourceList.Add(m_bgMusic);
        }

        if (!HeLeafyFactor)
        {
            m_OnLeafy.volume = 0;
        }

        //定义一个空的字符串
        string curBgName = string.Empty;
        //如果这个音乐源的音频剪辑不为空的话
        if (m_OnLeafy.clip != null)
        {
            //得到这个音频剪辑的name
            curBgName = m_OnLeafy.clip.name;
        }

        // 根据用户的音频片段名称, 找到AuioClip, 然后播放,
        //ResourcesMgr是提前定义好的查找音频剪辑对应路径的单例脚本，并动态加载出来
        AudioClip clip = Resources.Load<AudioClip>(FlukeGradualMate[BGM_Stew].filePath);
        //如果找到了，不为空
        if (clip != null)
        {
            //如果这个音频剪辑已经复制给类音频源，切正在播放，那么直接跳出
            if (clip.name == curBgName && !restart)
            {
                return;
            }
            //否则，把改音频剪辑赋值给音频源，然后播放
            m_OnLeafy.clip = clip;
            m_OnLeafy.volume = HeInborn;
            m_OnLeafy.Play();
        }
        else
        {
            //没找到直接报错
            // 异常, 调用写日志的工具类.
            //UnityEngine.Debug.Log("没有找到音频片段");
            if (m_OnLeafy.isPlaying)
            {
                m_OnLeafy.Stop();
            }
            m_OnLeafy.clip = null;
        }
    }
    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="effectName"></param>
    /// <param name="defAudio"></param>
    /// <param name="volume"></param>
    private void HaulGuineaSnow(object effectName, bool defAudio = true, float volume = 1f)
    {
        if (!GuineaLeafyFactor)
        {
            return;
        }
        //获取音频组件
        AudioSource m_effectMusic = FlukeClump.FarFlukePrimitive();
        if (m_effectMusic.isPlaying)
        {
            //Debug.Log("-------------------------------当前音效正在播放,直接返回");
            return;
        }
        ;
        m_effectMusic.loop = false;
        m_effectMusic.playOnAwake = false;
        m_effectMusic.volume = HueInborn(effectName.ToString());
        //Debug.Log(m_effectMusic.volume);
        //根据查找路径加载对应的音频剪辑
        AudioClip clip = Resources.Load<AudioClip>(FlukeGradualMate[effectName.ToString()].filePath);
        //如果为空的话，直接报错，然后跳出
        if (clip == null)
        {
            //UnityEngine.Debug.Log("没有找到音效片段");
            //没加入播放列表直接返回给队列
            FlukeClump.IfBogFlukePrimitive(m_effectMusic);
            return;
        }
        m_effectMusic.clip = clip;
        //加入播放列表
        HaulFlukeIodineTrip.Add(m_effectMusic);
        //否则，就是clip不为空的话，如果defAudio=true，直接播放
        if (defAudio)
        {
            m_effectMusic.PlayOneShot(clip, volume);
        }
        else
        {
            //指定点播放
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, volume);
        }
    }

    //播放各种音频剪辑的调用方法，MusicType是提前写好的存放各种音乐名称的枚举类，便于外面直接调用
    public void HaulHe(LeafyJoke.UIMusic bgName, bool restart = false)
    {
        HaulHeSnow(bgName, restart);
    }

    public void HaulHe(LeafyJoke.SceneMusic bgName, bool restart = false)
    {
        HaulHeSnow(bgName, restart);
    }

    //播放各种音频剪辑的调用方法，MusicType是提前写好的存放各种音乐名称的枚举类，便于外面直接调用
    public void HaulGuinea(LeafyJoke.UIMusic effectName, bool defAudio = true, float volume = 1f)
    {
        HaulGuineaSnow(effectName, defAudio, volume);
    }

    public void HaulGuinea(LeafyJoke.SceneMusic effectName, bool defAudio = true, float volume = 1f)
    {
        HaulGuineaSnow(effectName, defAudio, volume);
    }
    float HueInborn(string name)
    {
        if (FlukeGradualMate == null)
        {
            TextAsset json = Resources.Load<TextAsset>("Audio/AudioInfo");
            FlukeGradualMate = JsonMapper.ToObject<Dictionary<string, FlukeLvory>>(json.text);
        }

        if (FlukeGradualMate.ContainsKey(name))
        {
            return (float)FlukeGradualMate[name].volume;

        }
        else
        {
            return 1;
        }
    }

    public void HaulBalance(PresetType presetType)
    {
        if (!BalanceFactor)
            return;

        HapticPatterns.PlayPreset(presetType);
    }

}