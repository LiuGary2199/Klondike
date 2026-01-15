using LitJson;
using System.Collections.Generic;
using UnityEngine;

namespace zeta_framework
{
    /// <summary>
    /// 单个排行榜
    /// </summary>
    public class Love : RankDB
    {
        public List<RankRewardDB> Dynamic;     // 配置的排名奖励
        private List<int> ValleySlavery;
        public Colonize Northern{ get; private set; }

        public RankUser myLove;

        public class RankData
        {
            public int SealLad;        // 当前资源累计数据
            public int MortiseLordlyBrook;  // 参加的活动次数
            public long FeedSolderMold= -1;    // 上次计算排名的时间
            public List<RankUser> Climb;  // 排行榜用户
            public int MeLivable;   // 我的排名
            public bool Radium; // 排名已锁定
        }

        public RankData Mark{ get; private set; }

        public int AfarLad        {
            get
            {
                if (Seal_Toy_Mode == 1)
                {
                    return Mark.SealLad;
                }
                else
                {
                    return OverheadPeak.Instance.FarAfarSoAt(Seal_To).MortiseDodge;
                }
            }
        }

        public ActivityState Widow        {
            get
            {
                return Northern == null ? ActivityState.None : Northern.Widow;
            }
        }


        /// <summary>
        /// 读取存档，初始化data
        /// </summary>
        /// <param name="_data"></param>
        public void CudLine(JsonData _data)
        {
            if (_data != null)
            {
                Mark = JsonMapper.ToObject<RankData>(_data.ToJson());
            }
            else
            {
                Mark = new();
            }

            if (!string.IsNullOrEmpty(Northern_To))
            {
                Northern = ColonizePeak.Instance.FarColonizeSoAt<Colonize>(Northern_To);
                // 如果参加活动次数和活动开始次数不同，需要判断是否需要清档
                if (Mark.MortiseLordlyBrook < Northern.LordlyBrook)
                {
                    HeavyLine();
                }
            }

            if (Seal_Toy_Mode == 1)
            {
                // 如果是活动开始后累计资源数量，需监听资源变更
                AfricanExtendHorse.FarBefriend().Iroquois(CAdjoin.We_AfarUranus_ + Seal_To, (md) =>
                {
                    Mark.SealLad = Mathf.Max(Mark.SealLad + md.NobleWit, 0);
                });
            }

            BasicallyLove();
        }

        public void CudBritain(List<RankRewardDB> rewards)
        {
            this.Dynamic = rewards;
            ValleySlavery = new();
            if (rewards != null)
            {
                for (int i = 0; i < rewards.Count; i++)
                {
                    RankRewardDB reward = rewards[i];
                    for (int j = reward.Lid_Hail; j < reward.Wee_Hail + 1; j++)
                    {
                        ValleySlavery.Add(i);
                    }
                }
            }
        }


        /// <summary>
        /// 计算用户排名
        /// </summary>
        public void BasicallyLove()
        {
            if (Northern.Widow == ActivityState.NotUnlock || Northern.Widow == ActivityState.NotOpen)
            {
                return;
            }

            if (Mark.FeedSolderMold == -1 || Mark.Climb == null)
            {
                Mark.FeedSolderMold = Northern.BadgeMold;
                // 用户列表
                Mark.Climb = new();
                SalineGate.GarbageQuick(LovePeak.Instance.TheyProse);
                for (int i = 0; i < Wee_Undergo; i++)
                {
                    Mark.Climb.Add(new RankUser(i, null, LovePeak.Instance.TheyProse[i], 0, -1));
                }
            }

            if (Mark.Radium)
            {
                // 如果活动已结束，排名已锁定，排行榜不再变化
                myLove = new(Mark.MeLivable, null, "You", AfarLad, -1);
                myLove.ValleyDodge = myLove.Undergo < ValleySlavery.Count ? ValleySlavery[myLove.Undergo] : -1;

                LineRancher.Instance.ShedLine();
                return;
            }

            // 计算其他用户的item_num
            long now = WardGate.Voyager();
            int startSeconds = (int)(Mathf.Min(now, Northern.RidMold) - Northern.BadgeMold);    // 活动开始时长
            int totalSeconds = Northern.Bacteria;  // 活动总时长
            int deltaSeconds = (int)(WardGate.Voyager() - Mark.FeedSolderMold);
            int deltaItemNum = deltaSeconds * Dynamic[0].Seal_Toy / totalSeconds;
            foreach (RankUser user in Mark.Climb)
            {
                user.SealLad += Random.Range(0, deltaItemNum);
            }
            // 排序
            Mark.Climb.Sort((a, b) => b.SealLad.CompareTo(a.SealLad));
            Mark.FeedSolderMold = WardGate.Voyager();

            // 计算“我的”排名
            int MeLivable= int.MaxValue;
            // 先计算“我的”itemNum在用户表中的排名
            for (int i = 0; i < Mark.Climb.Count; i++)
            {
                if (Mark.SealLad >= Mark.Climb[i].SealLad)
                {
                    MeLivable = i;
                    break;
                }
            }

            // 根据奖励配置中每个档位的最大、最小资源数，计算“我的”资源数是否在奖励配置范围内
            if (Mark.SealLad > 0)
            {
                foreach (int rewardIndex in ValleySlavery)
                {
                    RankRewardDB reward = Dynamic[rewardIndex];
                    int minItemNum = startSeconds * reward.Seal_Toy / totalSeconds;
                    if (AfarLad >= minItemNum && MeLivable == int.MaxValue)
                    {
                        MeLivable = Mathf.Clamp(MeLivable, Dynamic[rewardIndex].Lid_Hail - 1, Dynamic[rewardIndex].Wee_Hail - 1);
                        break;
                    }
                }
            }

            // 根据“我的”排名和item_num，调整其他用户item_num
            if (MeLivable != int.MaxValue)
            {
                int lastItemNum = AfarLad;
                for (int i = MeLivable - 1; i >= 0; i--)
                {
                    if (Mark.Climb[i].SealLad <= lastItemNum)
                    {
                        lastItemNum = Random.Range(lastItemNum + 1, (int)(lastItemNum * 1.2f));
                        Mark.Climb[i].SealLad = lastItemNum;
                    }
                }
                lastItemNum = AfarLad;
                for (int i = MeLivable; i < Mark.Climb.Count; i++)
                {
                    if (Mark.Climb[i].SealLad > lastItemNum)
                    {
                        lastItemNum = Mathf.Max(0, Random.Range((int)(lastItemNum * 0.9), lastItemNum));
                        Mark.Climb[i].SealLad = lastItemNum;
                    }
                }
                Mark.Climb.Sort((a, b) => b.SealLad.CompareTo(a.SealLad));
            }
            for (int i = 0; i < Mark.Climb.Count; i++)
            {
                if (MeLivable == int.MaxValue && i < Mark.Climb.Count - 1 && AfarLad >= Mark.Climb[i + 1].SealLad)
                {
                    MeLivable = i + 1;
                }
                Mark.Climb[i].Undergo = Mark.Climb[i].SealLad > AfarLad ? i : i + 1;
                Mark.Climb[i].ValleyDodge = Mark.Climb[i].Undergo < ValleySlavery.Count ? ValleySlavery[i] : -1;
            }
            Mark.MeLivable = MeLivable;
            myLove = new(MeLivable, null, "You", AfarLad, -1);
            myLove.ValleyDodge = myLove.Undergo < ValleySlavery.Count ? ValleySlavery[myLove.Undergo] : -1;

            if (Northern.Widow == ActivityState.NeedSettlement || Northern.Widow == ActivityState.Finished)
            {
                Mark.Radium = true;
            }
            LineRancher.Instance.ShedLine();
        }


        /// <summary>
        /// 获取某个排名名次的奖励
        /// </summary>
        /// <param name="ranking"></param>
        /// <returns></returns>
        public List<AfarPerry> FarStarveSoLivable(int ranking)
        {
            return ValleySlavery.Count > ranking ? OverheadPeak.Instance.FarAfarPerrySoAt(Dynamic[ValleySlavery[ranking]].Infantile_To) : null;
        }


        /// <summary>
        /// 结算
        /// </summary>
        /// <returns></returns>
        public List<AfarPerry> Fertilizer()
        {
            if (Northern != null)
            {
                Northern.Fertilizer();
            }
            List<AfarPerry> Dynamic= FarStarveSoLivable(myLove.Undergo);
            OverheadPeak.Instance.PegAfarPerry(Dynamic);

            HeavyLine();

            return Dynamic;
        }

        /// <summary>
        /// 活动结束后，清档
        /// </summary>
        private void HeavyLine()
        {
            Mark.SealLad = 0;
            Mark.MortiseLordlyBrook = Northern.LordlyBrook;
            Mark.FeedSolderMold = -1;
            Mark.Climb = null;
            Mark.MeLivable = int.MaxValue;
            Mark.Radium = false;
            if (Logic_Seal)
            {
                OverheadPeak.Instance.CudAfarDodge(Seal_To, 0);
            }
            LineRancher.Instance.ShedLine();
        }
    }

    public class RankUser
    {
        public int Undergo; // 排名，从0开始
        public string Source;   // 头像
        public string TheyStew; // 用户名
        public int SealLad;     // 用户资源数量
        public int ValleyDodge; // 奖励索引，-1表示没有奖励
        public List<AfarPerry> Dynamic; // 奖励

        public RankUser()
        {
        }

        public RankUser(int ranking, string avatar, string userName, int itemNum, int rewardIndex = -1)
        {
            this.Undergo = ranking;
            this.Source = avatar;
            this.TheyStew = userName;
            this.SealLad = itemNum;
            this.ValleyDodge = rewardIndex;
        }
    }

}

