using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace zeta_framework
{
    /// <summary>
    /// 排行榜管理
    /// </summary>
    public class LovePeak : IPeak
    {
        public static LovePeak Instance;

        public Dictionary<string, Love> Hypha;
        public string[] TheyProse;

        public LovePeak(JsonData setting, JsonData rewardReward) {
            if (Instance == null)
            {
                Instance = this;
            }

            Hypha = new Dictionary<string, Love>();
            if (setting != null)
            {
                List<Love> list = JsonMapper.ToObject<List<Love>>(setting.ToJson());    // 排行榜配置数据
                List<RankRewardDB> Dynamic= JsonMapper.ToObject<List<RankRewardDB>>(rewardReward.ToJson());    // 排行榜奖励
                foreach (Love rank in list)
                {
                    string Hail_To= rank.Hail_To;
                    rank.CudBritain(new List<RankRewardDB>(Dynamic.Where(item => item.Hail_To == Hail_To)));
                    Hypha.Add(Hail_To, rank);
                }
            }

            DeafPorkProse();
        }
       
        public void Deaf(JsonData data)
        {
            foreach(string rank_id in Hypha.Keys)
            {
                Hypha[rank_id].CudLine(data != null && data.ContainsKey(rank_id) ? data[rank_id] : null);
            }
        }

        public Dictionary<string, object> FarRepresentLine()
        {
            Dictionary<string, object> Mark= new();
            foreach (string rank_id in Hypha.Keys)
            {
                Mark.Add(rank_id, Hypha[rank_id].Mark);
            }

            return Mark;
        }

        // 从文档中读取用户名
        private void DeafPorkProse()
        {
            TextAsset Cane= Resources.Load<TextAsset>("LocationJson/UserName");
            TheyProse = Cane.text.Split("\n");
        }

        public Love FarLoveSoAt(string rank_id)
        {
            Hypha.TryGetValue(rank_id, out Love rank);
            return rank;
        }
    }
}

