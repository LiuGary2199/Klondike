using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Sprites;
using UnityEngine;
using static MaxSdkBase;
using static UnityEditor.Progress;
using static zeta_framework.Item;

namespace zeta_framework
{
    /// <summary>
    /// 单个排行榜
    /// </summary>
    public class Rank : RankDB
    {
        public List<RankRewardDB> rewards;     // 配置的排名奖励
        private List<int> rewardIndexes;
        public Activity activity { get; private set; }

        public RankUser myRank;

        public class RankData
        {
            public int itemNum;        // 当前资源累计数据
            public int currentAttendTimes;  // 参加的活动次数
            public long lastUpdateTime = -1;    // 上次计算排名的时间
            public List<RankUser> users;  // 排行榜用户
            public int myRanking;   // 我的排名
            public bool locked; // 排名已锁定
        }

        public RankData data { get; private set; }

        public int ItemNum
        {
            get
            {
                if (item_num_type == 1)
                {
                    return data.itemNum;
                }
                else
                {
                    return ResourceCtrl.Instance.GetItemById(item_id).currentValue;
                }
            }
        }

        public ActivityState State
        {
            get
            {
                return activity == null ? ActivityState.None : activity.State;
            }
        }


        /// <summary>
        /// 读取存档，初始化data
        /// </summary>
        /// <param name="_data"></param>
        public void SetData(JsonData _data)
        {
            if (_data != null)
            {
                data = JsonMapper.ToObject<RankData>(_data.ToJson());
            }
            else
            {
                data = new();
            }

            if (!string.IsNullOrEmpty(activity_id))
            {
                activity = ActivityCtrl.Instance.GetActivityById<Activity>(activity_id);
                // 如果参加活动次数和活动开始次数不同，需要判断是否需要清档
                if (data.currentAttendTimes < activity.AttendTimes)
                {
                    ClearData();
                }
            }

            if (item_num_type == 1)
            {
                // 如果是活动开始后累计资源数量，需监听资源变更
                MessageCenterLogic.GetInstance().Register(CConfig.mg_ItemChange_ + item_id, (md) =>
                {
                    data.itemNum = Mathf.Max(data.itemNum + md.valueInt, 0);
                });
            }

            CalculateRank();
        }

        public void SetRewards(List<RankRewardDB> rewards)
        {
            this.rewards = rewards;
            rewardIndexes = new();
            if (rewards != null)
            {
                for (int i = 0; i < rewards.Count; i++)
                {
                    RankRewardDB reward = rewards[i];
                    for (int j = reward.min_rank; j < reward.max_rank + 1; j++)
                    {
                        rewardIndexes.Add(i);
                    }
                }
            }
        }


        /// <summary>
        /// 计算用户排名
        /// </summary>
        public void CalculateRank()
        {
            if (activity.State == ActivityState.NotUnlock || activity.State == ActivityState.NotOpen)
            {
                return;
            }

            if (data.lastUpdateTime == -1 || data.users == null)
            {
                data.lastUpdateTime = activity.StartTime;
                // 用户列表
                data.users = new();
                RandomUtil.ShuffleArray(RankCtrl.Instance.userNames);
                for (int i = 0; i < max_ranking; i++)
                {
                    data.users.Add(new RankUser(i, null, RankCtrl.Instance.userNames[i], 0, -1));
                }
            }

            if (data.locked)
            {
                // 如果活动已结束，排名已锁定，排行榜不再变化
                myRank = new(data.myRanking, null, "You", ItemNum, -1);
                myRank.rewardIndex = myRank.ranking < rewardIndexes.Count ? rewardIndexes[myRank.ranking] : -1;

                DataManager.Instance.SaveData();
                return;
            }

            // 计算其他用户的item_num
            long now = DateUtil.Current();
            int startSeconds = (int)(Mathf.Min(now, activity.EndTime) - activity.StartTime);    // 活动开始时长
            int totalSeconds = activity.duration;  // 活动总时长
            int deltaSeconds = (int)(DateUtil.Current() - data.lastUpdateTime);
            int deltaItemNum = deltaSeconds * rewards[0].item_num / totalSeconds;
            foreach (RankUser user in data.users)
            {
                user.itemNum += Random.Range(0, deltaItemNum);
            }
            // 排序
            data.users.Sort((a, b) => b.itemNum.CompareTo(a.itemNum));
            data.lastUpdateTime = DateUtil.Current();

            // 计算“我的”排名
            int myRanking = int.MaxValue;
            // 先计算“我的”itemNum在用户表中的排名
            for (int i = 0; i < data.users.Count; i++)
            {
                if (data.itemNum >= data.users[i].itemNum)
                {
                    myRanking = i;
                    break;
                }
            }

            // 根据奖励配置中每个档位的最大、最小资源数，计算“我的”资源数是否在奖励配置范围内
            if (data.itemNum > 0)
            {
                foreach (int rewardIndex in rewardIndexes)
                {
                    RankRewardDB reward = rewards[rewardIndex];
                    int minItemNum = startSeconds * reward.item_num / totalSeconds;
                    if (ItemNum >= minItemNum && myRanking == int.MaxValue)
                    {
                        myRanking = Mathf.Clamp(myRanking, rewards[rewardIndex].min_rank - 1, rewards[rewardIndex].max_rank - 1);
                        break;
                    }
                }
            }

            // 根据“我的”排名和item_num，调整其他用户item_num
            if (myRanking != int.MaxValue)
            {
                int lastItemNum = ItemNum;
                for (int i = myRanking - 1; i >= 0; i--)
                {
                    if (data.users[i].itemNum <= lastItemNum)
                    {
                        lastItemNum = Random.Range(lastItemNum + 1, (int)(lastItemNum * 1.2f));
                        data.users[i].itemNum = lastItemNum;
                    }
                }
                lastItemNum = ItemNum;
                for (int i = myRanking; i < data.users.Count; i++)
                {
                    if (data.users[i].itemNum > lastItemNum)
                    {
                        lastItemNum = Mathf.Max(0, Random.Range((int)(lastItemNum * 0.9), lastItemNum));
                        data.users[i].itemNum = lastItemNum;
                    }
                }
                data.users.Sort((a, b) => b.itemNum.CompareTo(a.itemNum));
            }
            for (int i = 0; i < data.users.Count; i++)
            {
                if (myRanking == int.MaxValue && i < data.users.Count - 1 && ItemNum >= data.users[i + 1].itemNum)
                {
                    myRanking = i + 1;
                }
                data.users[i].ranking = data.users[i].itemNum > ItemNum ? i : i + 1;
                data.users[i].rewardIndex = data.users[i].ranking < rewardIndexes.Count ? rewardIndexes[i] : -1;
            }
            data.myRanking = myRanking;
            myRank = new(myRanking, null, "You", ItemNum, -1);
            myRank.rewardIndex = myRank.ranking < rewardIndexes.Count ? rewardIndexes[myRank.ranking] : -1;

            if (activity.State == ActivityState.NeedSettlement || activity.State == ActivityState.Finished)
            {
                data.locked = true;
            }
            DataManager.Instance.SaveData();
        }


        /// <summary>
        /// 获取某个排名名次的奖励
        /// </summary>
        /// <param name="ranking"></param>
        /// <returns></returns>
        public List<ItemGroup> GetRewardByRanking(int ranking)
        {
            return rewardIndexes.Count > ranking ? ResourceCtrl.Instance.GetItemGroupById(rewards[rewardIndexes[ranking]].itemgroup_id) : null;
        }


        /// <summary>
        /// 结算
        /// </summary>
        /// <returns></returns>
        public List<ItemGroup> Settlement()
        {
            if (activity != null)
            {
                activity.Settlement();
            }
            List<ItemGroup> rewards = GetRewardByRanking(myRank.ranking);
            ResourceCtrl.Instance.AddItemGroup(rewards);

            ClearData();

            return rewards;
        }

        /// <summary>
        /// 活动结束后，清档
        /// </summary>
        private void ClearData()
        {
            data.itemNum = 0;
            data.currentAttendTimes = activity.AttendTimes;
            data.lastUpdateTime = -1;
            data.users = null;
            data.myRanking = int.MaxValue;
            data.locked = false;
            if (clear_item)
            {
                ResourceCtrl.Instance.SetItemValue(item_id, 0);
            }
            DataManager.Instance.SaveData();
        }
    }

    public class RankUser
    {
        public int ranking; // 排名，从0开始
        public string avatar;   // 头像
        public string userName; // 用户名
        public int itemNum;     // 用户资源数量
        public int rewardIndex; // 奖励索引，-1表示没有奖励
        public List<ItemGroup> rewards; // 奖励

        public RankUser()
        {
        }

        public RankUser(int ranking, string avatar, string userName, int itemNum, int rewardIndex = -1)
        {
            this.ranking = ranking;
            this.avatar = avatar;
            this.userName = userName;
            this.itemNum = itemNum;
            this.rewardIndex = rewardIndex;
        }
    }

}

