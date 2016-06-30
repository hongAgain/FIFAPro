using Common;
using System;
using Common.Tables;
namespace BehaviourTree
{
    /// <summary>
    /// deprecated
    /// </summary>
//    public class ActionCloseMarkingBall : ActionRuntoTarget
//    {
//        public ActionCloseMarkingBall()
//        {
//            Name = "CloseMarkingBall";
//            DisplayName = "行为:跑向有球盯防位置";
//            NodeType = "ActionCloseMarkingBall";
//        }
//
//        public override void Activate(BTDatabase kDatabase)
//        {
//            base.Activate(kDatabase);
//            AICfgItem kDefRadiusItem = TableManager.Instance.AIConfig.GetItem("def_radius_dribble");
//            m_dDefDist = kDefRadiusItem.Value;
//        }
//
//
//        protected override void Enter()
//        {
//            base.Enter();
//            m_dElpaseTime = 0;
//            
//        }
//
//        protected override BTResult Execute(double dTime)
//        {
//            if (null == m_kPlayer)
//                return BTResult.Failed;
//            m_dElpaseTime += dTime;
//            if (EPlayerState.PS_CLOSEMARKING_ENTERING != m_kPlayer.State)
//            {
//                return BTResult.Failed;
//            }
//            double dDist = m_kPlayer.Position.Distance(m_kPlayer.Team.Opponent.BallController.Position);
//            if (dDist < m_dDefDist)
//            {
//                m_kPlayer.SetState(EPlayerState.PS_CLOSEMARK_WITH_BALL);
//                m_kPlayer.SetAniState(EAniState.EAS_Defend);
//                m_kPlayer.ManMarkWithBall = true;
//            }
//            else if (BTResult.Success == RunToTarget(dTime))
//            {
//                m_kPlayer.SetState(EPlayerState.PS_CLOSEMARK_WITH_BALL);
//                m_kPlayer.SetAniState(EAniState.EAS_Defend);
//                m_kPlayer.ManMarkWithBall = true;
//
//            }
//            else
//                return BTResult.Running;
//            return BTResult.Success;
//        }
//
//        protected override void Exit()
//        {
//            base.Exit();
//            m_dElpaseTime = 0;
//        }
//        private Double m_dElpaseTime = 0;
//        private double m_dDefDist = 0; 
//    }
}