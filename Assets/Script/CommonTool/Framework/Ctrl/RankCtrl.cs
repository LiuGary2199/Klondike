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
    public class RankCtrl : ICtrl
    {
        public static RankCtrl Instance;

        public Dictionary<string, Rank> ranks;
        public string[] userNames;

        public RankCtrl(JsonData setting, JsonData rewardReward) {
            if (Instance == null)
            {
                Instance = this;
            }

            ranks = new Dictionary<string, Rank>();
            if (setting != null)
            {
                List<Rank> list = JsonMapper.ToObject<List<Rank>>(setting.ToJson());    // 排行榜配置数据
                List<RankRewardDB> rewards = JsonMapper.ToObject<List<RankRewardDB>>(rewardReward.ToJson());    // 排行榜奖励
                foreach (Rank rank in list)
                {
                    string rank_id = rank.rank_id;
                    rank.SetRewards(new List<RankRewardDB>(rewards.Where(item => item.rank_id == rank_id)));
                    ranks.Add(rank_id, rank);
                }
            }

            InitUserNames();
        }
       
        public void Init(JsonData data)
        {
            foreach(string rank_id in ranks.Keys)
            {
                ranks[rank_id].SetData(data != null && data.ContainsKey(rank_id) ? data[rank_id] : null);
            }
        }

        public Dictionary<string, object> GetSerializeData()
        {
            Dictionary<string, object> data = new();
            foreach (string rank_id in ranks.Keys)
            {
                data.Add(rank_id, ranks[rank_id].data);
            }

            return data;
        }

        // 从文档中读取用户名
        private void InitUserNames()
        {
            TextAsset text = Resources.Load<TextAsset>("LocationJson/UserName");
            userNames = text.text.Split("\n");
        }

        public Rank GetRankById(string rank_id)
        {
            ranks.TryGetValue(rank_id, out Rank rank);
            return rank;
        }
    }
}

