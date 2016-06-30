//using Common;
//using System;
//using Common.Tables;
//
//namespace BehaviourTree
//{
//    /// <summary>
//    ///  deprecated
//    /// </summary>
//    public class ActionInterceptBall : BTAction
//    {
//        enum EState
//        {
//            EnterIntercepted = 0,    
//            Intercept,
//            RunToTarget,
//            InterceptedSuccess,
//            InterceptedFailed,
//            Max
//        }
//        public ActionInterceptBall()
//        {
//            Name = "InterceptBall";
//            DisplayName = "行为:抢截";
//            NodeType = "ActionInterceptBall";
//        }
//
//        protected override void Enter()
//        {
//            if (null == m_kPlayer)
//            {
//                int iID = m_kDatabase.GetDataID(BTConstant.Player);
//                m_kPlayer = m_kDatabase.GetData<LLPlayer>(iID);
//            }
//            if (null != m_kPlayer)
//                m_kTeam = m_kPlayer.Team;
//            m_kState = EState.EnterIntercepted;
//        }
//
//        protected override BTResult Execute(double dTime)
//        {
//            if (EPlayerState.PS_INTERCEPT_BALL != m_kPlayer.State)
//            {
//                return BTResult.Failed;
//            }
//
//            switch(m_kState)
//            {
//                case EState.EnterIntercepted:
//                    return EnterInterceptedState(dTime);
//                case EState.Intercept:
//                    return InterceptState(dTime);
//                case EState.InterceptedSuccess:
//                    return InterceptedSuccessState();
//                case EState.RunToTarget:
//                    return OnRunToTargetState(dTime);
//                case EState.InterceptedFailed:
//                    return InterceptedFailedState();
//                default:
//                    break;
//            }
//            return BTResult.Success;
//        }
//
//        protected BTResult OnRunToTargetState(double dTime)
//        {
//            double fDist = m_kPlayer.Position.Distance(m_kTeam.Scene.Ball.Position);
//            double ds = m_kPlayer.Velocity * dTime * 2;
//
//            if (fDist > m_kPlayer.Velocity)
//            {
//                // 跑向位置
//                Vector3D kDir = m_kTeam.Scene.Ball.Position - m_kPlayer.Position;
//                kDir = kDir.Normalize();
//                m_kPlayer.Position = m_kPlayer.Position + kDir * ds;
//                m_kPlayer.RotateAngle = MathUtil.GetAngle(m_kPlayer.Position, m_kPlayer.TargetPos);
//            }
//            else
//                m_kState = EState.InterceptedSuccess;
//
//            return BTResult.Running;
//        }
//
//        protected BTResult EnterInterceptedState(double dTime)
//        {
//            // 1.跑向球的位置
//            // 实施抢断
//            //var settleFactor = TableManager.Instance.SettlementFactorTable.GetItem("tackle");
//            //double data1 = m_kPlayer.Data.intercept * settleFactor.factor1 - m_kPlayer.Team.Opponent.BallController.Data.control * settleFactor.factor3;
//            //double data2 = TableManager.Instance.SensitivityFactorTable.GetItem(m_kPlayer.Data.lv.ToString()).tackle;
//            //var rate = data1 / data2 + settleFactor.basic_value;
//            //rate = Math.Min(1, Math.Max(settleFactor.basic_value * 0.1, rate));
//            double rate = 0.5;
//            if (FIFARandom.GetRandomValue(0,1) <= rate)
//            {
//                m_kState = EState.RunToTarget;
//            }
//            else
//            {
//                m_kState = EState.InterceptedFailed;
//            }
//
//            return BTResult.Running;
//            
//        }
//
//        protected BTResult InterceptState(Double dTime)
//        {
//            double fDist = m_kPlayer.Position.Distance(m_kTeam.Scene.Ball.Position);
//            double ds = m_kPlayer.Velocity * dTime;
//            if (fDist > ds)
//            {
//                // 跑向位置
//                Vector3D kDir = m_kTeam.Scene.Ball.Position - m_kPlayer.Position;
//                kDir = kDir.Normalize();
//                m_kPlayer.Position = m_kPlayer.Position + kDir * ds;
//                m_kPlayer.RotateAngle = MathUtil.GetAngle(m_kPlayer.Position, m_kTeam.Scene.Ball.Position);
//                return BTResult.Running;
//            }
//            else
//            {
//                m_kPlayer.SetAniState(EAniState.EAS_Intercept);
//                m_kPlayer.SetBallCtrl(true);
//                m_kPlayer.ManMarkWithBall = false;
//                m_kTeam.BallController = m_kPlayer;
//                m_kTeam.Scene.RequireTeamStateChange(ETeamStateChangeType.TSCT_INTERCEPT, true);
//            }
//            return BTResult.Success;
//            
//        }
//        protected BTResult InterceptedSuccessState()
//        {
//            if (null != m_kTeam.Opponent.BallController)
//                m_kTeam.Opponent.BallController.AIEnable = false;
//            m_kTeam.Scene.Ball.SetTarget(m_kTeam.Scene.Ball.Position);
//            m_kTeam.Scene.Ball.CanMove = true;
//            m_kState = EState.Intercept;
//            return BTResult.Running;
//        }
//
//        protected BTResult InterceptedFailedState()
//        {
//            // 抢失败
//            if (null != m_kPlayer.Opponent)
//                m_kPlayer.Opponent.Opponent = null;
//            m_kPlayer.Opponent = null;
//            m_kPlayer.SetState(EPlayerState.Default);
//            return BTResult.Failed;
//        }
//
//        protected override void Exit()
//        {
//            m_kPlayer = null;
//        }
//
//        private LLTeam m_kTeam = null;
//        private LLPlayer m_kPlayer = null;
//        private EState m_kState;
//        
//    }
//}