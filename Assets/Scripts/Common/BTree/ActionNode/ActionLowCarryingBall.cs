//using Common;
//using System;
//using Common.Tables;
//
//namespace BehaviourTree
//{
//    /// <summary>
//    /// deprecated
//    /// </summary>
//    public class ActionLowCarryingBall : ActionRuntoTarget
//    {
//        protected enum EState
//        {
//            WaitForHit = 0, // 等待踢球
//            DribbleBall,    // 带球
//        }
//        
//        private int lastRegionID = 0;
//
//        public ActionLowCarryingBall()
//        {
//            Name = "LowCarryingBall";
//            DisplayName = "行为:低速带球";
//            NodeType = "ActionLowCarryingBall";
//        }
//
//        public override void Activate(BTDatabase kDatabase)
//        {
//            base.Activate(kDatabase);
//            mDribbleMax = (int)TableManager.Instance.AIConfig.GetItem("dribble_dis_max").Value;
//            mDribbleMin = (int)TableManager.Instance.AIConfig.GetItem("dribble_dis_min").Value;
//            m_dSpeedScale = TableManager.Instance.AIConfig.GetItem("speed_rate_run_dribble").Value;
//        }
//
//        protected override void Enter()
//        {
//            base.Enter();
//            LLTeam kTeam = m_kPlayer.Team;
//            LLScene kScene = kTeam.Scene;
//            m_kBall = kScene.Ball;
//            m_kGoalPos = kTeam.Opponent.Goal.GoalPos;
//
//            double fDribbleAngle = MathUtil.GetAngle(m_kBall.Position, m_kGoalPos);
//
//            m_kPlayer.KAniData.targetPos = m_kPlayer.TargetPos;
//            if (Math.Abs(fDribbleAngle - m_kPlayer.RotateAngle) > 90)
//                m_kPlayer.SetAniState(EAniState.LowDribble);
//            else
//                m_kPlayer.SetAniState(EAniState.LowDribble);
//            m_kState = EState.WaitForHit;
//
//            m_kDribbleDir = m_kPlayer.Team.SelectDribbleDir();
//            m_kPlayer.Velocity = m_kPlayer.BaseVelocity * m_dSpeedScale;
//        }
//
//        protected BTResult OnDribbleBall(double dTime)
//        {
//            if (m_kBall.ArrivedTargetPos())
//            {
//                m_kPlayer.SetState(EPlayerState.ActionSelect);
//                return BTResult.Success;
//            }
//            else
//            {
//                // 移动
//                m_kPlayer.Position = m_kPlayer.Position + m_kDribbleDir * m_kPlayer.Velocity * dTime;
//
//                m_kBall.Position = m_kPlayer.Position + m_kDribbleDir * 2;
//                ChangeBallPosMessage kMsg = new ChangeBallPosMessage(m_kBall.Position, m_kBall);
//                MessageDispatcher.Instance.SendMessage(kMsg);
//
//                //check region change
//                CheckRegionChange();
//            }
//            return BTResult.Running;
//        }
//
//        private void CheckRegionChange()
//        {
//            if (lastRegionID != m_kPlayer.RegionID)
//            {
//                m_kPlayer.Team.InformATeamRegionChanged(lastRegionID,m_kPlayer.RegionID);
//                lastRegionID = m_kPlayer.RegionID;
//            }
//        }
//
//        protected BTResult OnWaitForHit(double dTime)
//        {
//            
//            if (m_kPlayer.IsBallIn || m_kPlayer.ContinueDribble)
//            {
//                //(0.064,0.121,0.778)
//                // 计算球运动的方向
//                Vector3D kLen = m_kDribbleDir * FIFARandom.GetRandomValue(mDribbleMin, mDribbleMax + 1);
//                m_kBall.Position = m_kPlayer.Position;
//                m_kBall.Position += new Vector3D(0.064, 0.121, 0.778);
//                m_kBall.SetTarget(m_kBall.Position + kLen);
//                m_kState = EState.DribbleBall;
//            }
//            return BTResult.Running;
//        }
//        protected override BTResult Execute(double dTime)
//        {
//            if (EPlayerState.NormalDribble != m_kPlayer.State)
//            {
//                return BTResult.Failed;
//            }
//
//            if (false == m_kPlayer.IsCtrlBall)
//            {
//                return BTResult.Failed;
//            }
//            m_kPlayer.RotateAngle = MathUtil.GetAngle(m_kPlayer.TargetPos, m_kPlayer.Position);
//            switch (m_kState)
//            {
//                case EState.WaitForHit:
//                    return OnWaitForHit(dTime);
//                case EState.DribbleBall:
//                    return OnDribbleBall(dTime);
//                default:
//                    break;
//            }
//            return BTResult.Failed;
//        }
//
//        protected void UpdatePlayerOri()
//        {
//            m_kPlayer.RotateAngle = MathUtil.GetAngle(m_kBall.Position, m_kGoalPos);
//        }
//        protected override void Exit()
//        {
//            m_kPlayer = null;
//        }
//        protected EState m_kState;
//        private Vector3D m_kGoalPos ;
//        private LLBall m_kBall = null;
//
//        private int mDribbleMin;
//        private int mDribbleMax;
//        private double mVelocity = 0;
//        private Vector3D m_kDribbleDir;
//        private double m_dSpeedScale = 1.0;
//    }
//}