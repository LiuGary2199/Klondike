using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zeta_framework
{
    public class Activity : ActivityDB
    {
        public virtual void SetSetting(JsonData setting) { }

        public class ActivityData
        {
            public ActivityState state;    // 当前活动状态
            public int attendTimes;    // 已结算次数（比如签到，需要根据参加第几次计算应该给哪个奖励）
            public long startTime;      // 活动开始时间（如果是自动开启的活动，startTime和periodStartTime相同）
            public long endTime;        // 活动结束时间
            public long periodStartTime;    // 本期开始时间
            public long periodEndTime;      // 本期结束时间
        }
        // 存档数据
        protected ActivityData data;

        public ActivityState State { get { return data.state; }}
        public long StartTime { get { return data.startTime; } }
        public long EndTime { get { return data.endTime; } }
        public int AttendTimes { get { return data.attendTimes; } }


        /// <summary>
        /// 读取存档，初始化data
        /// </summary>
        /// <param name="_data"></param>
        public virtual void SetData(JsonData _data)
        {
            if (_data != null)
            {
                data = JsonMapper.ToObject<ActivityData>(_data.ToJson());
            }
            else
            {
                data = new();
            }
            CalculateState();

            // 监听关卡等级变更，修改活动解锁状态
            if (State == ActivityState.NotUnlock)
            {
                MessageCenterLogic.GetInstance().Register(CConfig.mg_LevelMaxLevelChange, (md) => { 
                    CalculateState();
                });
            }
        }

        public virtual object GetData()
        {
            return data;
        }

        /// <summary>
        /// 计算当前活动状态
        /// </summary>
        public void CalculateState()
        {
            // 未解锁
            if (unlock_level > LevelCtrl.Instance.maxLevelIndex)
            {
                ChangeActivityState(ActivityState.NotUnlock);
                DataManager.Instance.SaveData();
                return;
            }
            else if (State == ActivityState.NotUnlock)
            {
                data.state = ActivityState.NotAttend;
            }
            long now = DateUtil.Current();
            // 活动未开启
            if (now < start_time)
            {
                ChangeActivityState(ActivityState.NotOpen);
                DataManager.Instance.SaveData();
                return;
            }
            // 判断活动是否已经终止（已经超过活动期数）
            if (phases != -1 && data.attendTimes >= phases)
            {
                ChangeActivityState(ActivityState.Finished);
                DataManager.Instance.SaveData();
                return;
            }
            // 根据存档状态，计算当前状态
            if (period == -1)
            {
                // 活动周期为-1， 表示是常驻活动，活动状态设置为进行中
                ChangeActivityState(ActivityState.Attending);
                DataManager.Instance.SaveData();
                return;
            }
            long periodStartTime = now - (now - start_time) % period;   // 本期开始时间
            // 存档状态为【未开启】
            if (data.state == ActivityState.None || data.state == ActivityState.NotOpen || data.state == ActivityState.NotAttend)
            {
                // 根据手动/自动开启类型，计算本期活动开启-结束时间
                StartNewPeriod(periodStartTime);
                return;
            }
            // 存档状态为【参加中】，计算活动是否已经结束
            if (data.state == ActivityState.Attending)
            {
                if (now >= data.endTime && data.endTime != -1)
                {
                    if (auto_settlement)
                    {
                        // 自动结算
                        Settlement();
                        if (data.startTime < periodStartTime)
                        {
                            // 如果活动开始时间小于本期开始时间，重新开始新一期
                            StartNewPeriod(periodStartTime);
                        }
                    }
                    else
                    {
                        // 手动结算
                        ChangeActivityState(ActivityState.NeedSettlement);
                    }
                    DataManager.Instance.SaveData();
                }
                return;
            }
            // 存档状态为【已结束】，计算是否重新开始
            if (data.state == ActivityState.Finished && data.startTime < periodStartTime)
            {
                if (data.endTime > periodStartTime && !overlap)
                {
                    // 如果当前活动已结束，并且两期活动不能重叠，则不重新开启新一期活动
                    return;
                }
                else
                {
                    StartNewPeriod(periodStartTime);
                    return;

                }
            }
        }

        private void ChangeActivityState(ActivityState newState)
        {
            data.state = newState;
            // 广播消息
            MessageCenterLogic.GetInstance().Send(CConfig.mg_ActivityStateChange_ + id);
        }

        /// <summary>
        /// 开启新一期活动
        /// </summary>
        /// <param name="periodStartTime"></param>
        private void StartNewPeriod(long periodStartTime)
        {
            if (start_type == 1)
            {
                // 自动开启
                data.startTime = periodStartTime;
                data.endTime = duration == -1 ? -1 : data.startTime + duration;
                if (data.endTime != -1 && DateUtil.Current() >= data.endTime)
                {
                    // 本期已经结束
                    ChangeActivityState(ActivityState.Finished);
                }
                else
                {
                    // 本期未结束
                    ChangeActivityState(ActivityState.Attending);
                }
            }
            else
            {
                // 手动开启
                ChangeActivityState(ActivityState.NotAttend);
            }
            DataManager.Instance.SaveData();
        }

        /// <summary>
        /// 参加活动(手动开启)
        /// </summary>
        /// <returns></returns>
        public bool AttendActivity()
        {
            if (data.state == ActivityState.NotAttend)
            {
                long now = DateUtil.Current();
                ChangeActivityState(ActivityState.Attending);
                data.startTime = now;
                data.endTime = duration == -1 ? -1 : data.startTime + duration;
                DataManager.Instance.SaveData();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 结算
        /// </summary>
        public virtual bool Settlement()
        {
            // 活动为未结算状态，或常驻活动，可以进行结算
            if (data.state == ActivityState.NeedSettlement || data.state == ActivityState.Attending)
            {
                data.endTime = DateUtil.Current();
                ChangeActivityState(ActivityState.Finished);
                data.attendTimes++;

                // 如果此时下一期活动已经开始，判断是否开启下一期活动
                long now = DateUtil.Current();
                long periodStartTime = now - (now - start_time) % period;   // 本期开始时间
                if (data.startTime < periodStartTime)
                {
                    if (data.endTime > periodStartTime || overlap)
                    {
                        StartNewPeriod(periodStartTime);
                    }
                }

                DataManager.Instance.SaveData();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 是否需要显示（进行中、未结算、还未参加状态，需要进行显示）
        /// </summary>
        /// <returns></returns>
        public bool NeedShow()
        {
            return State == ActivityState.Attending || State == ActivityState.NeedSettlement || State == ActivityState.NotAttend;
        }
    }
}

