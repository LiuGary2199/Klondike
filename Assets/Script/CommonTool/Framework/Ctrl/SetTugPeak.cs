using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 经验宝箱
/// </summary>
namespace zeta_framework
{

    public class SetTugPeak : IPeak
    {
        public static SetTugPeak Instance;

        public Dictionary<string, SetTug> Enemy;    // key:宝箱id

        public SetTugPeak(JsonData setting)
        {
            if (Instance == null)
            {
                Instance = this;
            }

            Enemy = new();
            if (setting != null)
            {
                List<ExpBoxDB> boxList = JsonMapper.ToObject<List<ExpBoxDB>>(setting.ToJson());
                Dictionary<string, List<ExpBoxDB>> boxSettings = new();
                boxList.ForEach(box =>
                {
                    string key = box.Leg_To;
                    if (!Enemy.ContainsKey(key))
                    {
                        Enemy.Add(key, new SetTug());
                    }
                    if (!boxSettings.ContainsKey(key))
                    {
                        boxSettings.Add(key, new List<ExpBoxDB>());
                    }
                    boxSettings[key].Add(box);
                });
                foreach(string key in Enemy.Keys)
                {
                    Enemy[key].CudGradualLine(boxSettings[key]);
                }
            }
        }

        public void Deaf(JsonData data)
        {
            foreach (string box_id in Enemy.Keys)
            {
                Enemy[box_id].CudLine(data != null && data.ContainsKey(box_id) ? data[box_id] : null);
            }
        }

        public Dictionary<string, object> FarRepresentLine()
        {
            Dictionary<string, object> Mark= new();
            foreach (string box_id in Enemy.Keys)
            {
                Mark.Add(box_id, Enemy[box_id].Mark);
            }

            return Mark;
        }


        public SetTug FarTugLineSoAt(string box_id)
        {
            Enemy.TryGetValue(box_id, out SetTug data);
            return data;
        }
    }

}