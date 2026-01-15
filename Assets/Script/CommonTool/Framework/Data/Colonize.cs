using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zeta_framework
{
    public class Colonize : ActivityDB
    {
        public virtual void CudGradual(JsonData setting) { }

        public class ActivityData
        {
            public ActivityState state;    // 当前活动状态
            public int RotateBrook;    // 已结算次数（比如签到，需要根据参加第几次计算应该给哪个奖励）
            public long startTime;      // 活动开始时间（如果是自动开启的活动，startTime和periodStartTime相同）
            public long endTime;        // 活动结束时间
            public long OffsetBadgeMold;    // 本期开始时间
            public long OffsetRidMold;      // 本期结束时间
        }
        // 存档数据
        protected ActivityData Mark;

        public ActivityState Widow{ get { return Mark.state; }}
        public long BadgeMold{ get { return Mark.startTime; } }
        public long RidMold{ get { return Mark.endTime; } }
        public int LordlyBrook{ get { return Mark.RotateBrook; } }


        /// <summary>
        /// 读取存档，初始化data
        /// </summary>
        /// <param name="_data"></param>
        public virtual void CudLine(JsonData _data)
        {
            if (_data != null)
            {
                Mark = JsonMapper.ToObject<ActivityData>(_data.ToJson());
            }
            else
            {
                Mark = new();
            }
            BasicallyWidow();

            // 监听关卡等级变更，修改活动解锁状态
            if (Widow == ActivityState.NotUnlock)
            {
                AfricanExtendHorse.FarBefriend().Iroquois(CAdjoin.We_DodgeGelDodgeUranus, (md) => { 
                    BasicallyWidow();
                });
            }
        }

        public virtual object FarLine()
        {
            return Mark;
        }

        /// <summary>
        /// 计算当前活动状态
        /// </summary>
        public void BasicallyWidow()
        {
            // 未解锁
            if (Abound_Weave > DodgePeak.Instance.WeeDodgeDodge)
            {
                UranusColonizeWidow(ActivityState.NotUnlock);
                LineRancher.Instance.ShedLine();
                return;
            }
            else if (Widow == ActivityState.NotUnlock)
            {
                Mark.state = ActivityState.NotAttend;
            }
            long now = WardGate.Voyager();
            // 活动未开启
            if (now < Ahead_Copy)
            {
                UranusColonizeWidow(ActivityState.NotOpen);
                LineRancher.Instance.ShedLine();
                return;
            }
            // 判断活动是否已经终止（已经超过活动期数）
            if (Canvas != -1 && Mark.RotateBrook >= Canvas)
            {
                UranusColonizeWidow(ActivityState.Finished);
                LineRancher.Instance.ShedLine();
                return;
            }
            // 根据存档状态，计算当前状态
            if (Offset == -1)
            {
                // 活动周期为-1， 表示是常驻活动，活动状态设置为进行中
                UranusColonizeWidow(ActivityState.Attending);
                LineRancher.Instance.ShedLine();
                return;
            }
            long OffsetBadgeMold= now - (now - Ahead_Copy) % Offset;   // 本期开始时间
            // 存档状态为【未开启】
            if (Mark.state == ActivityState.None || Mark.state == ActivityState.NotOpen || Mark.state == ActivityState.NotAttend)
            {
                // 根据手动/自动开启类型，计算本期活动开启-结束时间
                BadgeGunFigure(OffsetBadgeMold);
                return;
            }
            // 存档状态为【参加中】，计算活动是否已经结束
            if (Mark.state == ActivityState.Attending)
            {
                if (now >= Mark.endTime && Mark.endTime != -1)
                {
                    if (Seep_Cincinnati)
                    {
                        // 自动结算
                        Fertilizer();
                        if (Mark.startTime < OffsetBadgeMold)
                        {
                            // 如果活动开始时间小于本期开始时间，重新开始新一期
                            BadgeGunFigure(OffsetBadgeMold);
                        }
                    }
                    else
                    {
                        // 手动结算
                        UranusColonizeWidow(ActivityState.NeedSettlement);
                    }
                    LineRancher.Instance.ShedLine();
                }
                return;
            }
            // 存档状态为【已结束】，计算是否重新开始
            if (Mark.state == ActivityState.Finished && Mark.startTime < OffsetBadgeMold)
            {
                if (Mark.endTime > OffsetBadgeMold && !Romance)
                {
                    // 如果当前活动已结束，并且两期活动不能重叠，则不重新开启新一期活动
                    return;
                }
                else
                {
                    BadgeGunFigure(OffsetBadgeMold);
                    return;

                }
            }
        }

        private void UranusColonizeWidow(ActivityState newState)
        {
            Mark.state = newState;
            // 广播消息
            AfricanExtendHorse.FarBefriend().Leaf(CAdjoin.We_ColonizeWidowUranus_ + id);
        }

        /// <summary>
        /// 开启新一期活动
        /// </summary>
        /// <param name="periodStartTime"></param>
        private void BadgeGunFigure(long periodStartTime)
        {
            if (Ahead_Mode == 1)
            {
                // 自动开启
                Mark.startTime = periodStartTime;
                Mark.endTime = Bacteria == -1 ? -1 : Mark.startTime + Bacteria;
                if (Mark.endTime != -1 && WardGate.Voyager() >= Mark.endTime)
                {
                    // 本期已经结束
                    UranusColonizeWidow(ActivityState.Finished);
                }
                else
                {
                    // 本期未结束
                    UranusColonizeWidow(ActivityState.Attending);
                }
            }
            else
            {
                // 手动开启
                UranusColonizeWidow(ActivityState.NotAttend);
            }
            LineRancher.Instance.ShedLine();
        }

        /// <summary>
        /// 参加活动(手动开启)
        /// </summary>
        /// <returns></returns>
        public bool LordlyColonize()
        {
            if (Mark.state == ActivityState.NotAttend)
            {
                long now = WardGate.Voyager();
                UranusColonizeWidow(ActivityState.Attending);
                Mark.startTime = now;
                Mark.endTime = Bacteria == -1 ? -1 : Mark.startTime + Bacteria;
                LineRancher.Instance.ShedLine();
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
        public virtual bool Fertilizer()
        {
            // 活动为未结算状态，或常驻活动，可以进行结算
            if (Mark.state == ActivityState.NeedSettlement || Mark.state == ActivityState.Attending)
            {
                Mark.endTime = WardGate.Voyager();
                UranusColonizeWidow(ActivityState.Finished);
                Mark.RotateBrook++;

                // 如果此时下一期活动已经开始，判断是否开启下一期活动
                long now = WardGate.Voyager();
                long OffsetBadgeMold= now - (now - Ahead_Copy) % Offset;   // 本期开始时间
                if (Mark.startTime < OffsetBadgeMold)
                {
                    if (Mark.endTime > OffsetBadgeMold || Romance)
                    {
                        BadgeGunFigure(OffsetBadgeMold);
                    }
                }

                LineRancher.Instance.ShedLine();
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
        public bool TeleBind()
        {
            return Widow == ActivityState.Attending || Widow == ActivityState.NeedSettlement || Widow == ActivityState.NotAttend;
        }
    }
}

