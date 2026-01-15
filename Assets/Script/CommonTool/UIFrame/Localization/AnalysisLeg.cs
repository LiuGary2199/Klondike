/*
 * 
 * 多语言
 * 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalysisLeg 
{
    public static AnalysisLeg _Sugarlike;
    //语言翻译的缓存集合
    private Dictionary<string, string> _HitAnalysisDraft;

    private AnalysisLeg()
    {
        _HitAnalysisDraft = new Dictionary<string, string>();
        //初始化语言缓存集合
        DeafAnalysisDraft();
    }

    /// <summary>
    /// 获取实例
    /// </summary>
    /// <returns></returns>
    public static AnalysisLeg FarBefriend()
    {
        if (_Sugarlike == null)
        {
            _Sugarlike = new AnalysisLeg();
        }
        return _Sugarlike;
    }

    /// <summary>
    /// 得到显示文本信息
    /// </summary>
    /// <param name="lauguageId">语言id</param>
    /// <returns></returns>
    public string BindMoss(string lauguageId)
    {
        string strQueryResult = string.Empty;
        if (string.IsNullOrEmpty(lauguageId)) return null;
        //查询处理
        if(_HitAnalysisDraft!=null && _HitAnalysisDraft.Count >= 1)
        {
            _HitAnalysisDraft.TryGetValue(lauguageId, out strQueryResult);
            if (!string.IsNullOrEmpty(strQueryResult))
            {
                return strQueryResult;
            }
        }
        Debug.Log(GetType() + "/ShowText()/ Query is Null!  Parameter lauguageID: " + lauguageId);
        return null;
    }

    /// <summary>
    /// 初始化语言缓存集合
    /// </summary>
    private void DeafAnalysisDraft()
    {
        //LauguageJSONConfig_En
        //LauguageJSONConfig
        IAdjoinRancher config = new AdjoinRancherSoAmid("LauguageJSONConfig");
        if (config != null)
        {
            _HitAnalysisDraft = config.GutGradual;
        }
    }
}
