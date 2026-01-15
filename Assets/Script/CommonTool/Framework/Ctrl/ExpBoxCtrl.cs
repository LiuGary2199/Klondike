using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 经验宝箱
/// </summary>
namespace zeta_framework
{

    public class ExpBoxCtrl : ICtrl
    {
        public static ExpBoxCtrl Instance;

        public Dictionary<string, ExpBox> boxes;    // key:宝箱id

        public ExpBoxCtrl(JsonData setting)
        {
            if (Instance == null)
            {
                Instance = this;
            }

            boxes = new();
            if (setting != null)
            {
                List<ExpBoxDB> boxList = JsonMapper.ToObject<List<ExpBoxDB>>(setting.ToJson());
                Dictionary<string, List<ExpBoxDB>> boxSettings = new();
                boxList.ForEach(box =>
                {
                    string key = box.box_id;
                    if (!boxes.ContainsKey(key))
                    {
                        boxes.Add(key, new ExpBox());
                    }
                    if (!boxSettings.ContainsKey(key))
                    {
                        boxSettings.Add(key, new List<ExpBoxDB>());
                    }
                    boxSettings[key].Add(box);
                });
                foreach(string key in boxes.Keys)
                {
                    boxes[key].SetSettingData(boxSettings[key]);
                }
            }
        }

        public void Init(JsonData data)
        {
            foreach (string box_id in boxes.Keys)
            {
                boxes[box_id].SetData(data != null && data.ContainsKey(box_id) ? data[box_id] : null);
            }
        }

        public Dictionary<string, object> GetSerializeData()
        {
            Dictionary<string, object> data = new();
            foreach (string box_id in boxes.Keys)
            {
                data.Add(box_id, boxes[box_id].data);
            }

            return data;
        }


        public ExpBox GetBoxDataById(string box_id)
        {
            boxes.TryGetValue(box_id, out ExpBox data);
            return data;
        }
    }

}