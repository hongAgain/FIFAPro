using Common;
using System;
using Common.Tables;
namespace BehaviourTree
{
    public class ActionGKMove : BTAction
    {
        protected enum EState
        {
            CalcAniState = 0,
            MoveToTarget
        }
        public ActionGKMove()
        {
            Name = "GKMove";
            DisplayName = "门将移动";
            NodeType = "ActionGKMove";
        }
        protected override void Enter()
        {
            int iID = m_kDatabase.GetDataID(BTConstant.Player);
            m_kPlayer = m_kDatabase.GetData<LLGoalKeeper>(iID);
            m_kBall = m_kPlayer.Team.Scene.Ball;
            m_kState = EState.CalcAniState;
            m_kTargetPos = m_kPlayer.HomePos;
            m_dHPRadius = TableManager.Instance.AIConfig.GetItem("homeposition_radius_gk").Value;
            if (m_kPlayer.GetPosition().Distance(m_kPlayer.HomePos) > m_dHPRadius)
                m_bRunning = true;
            else
                m_bRunning = false;
        }


        protected override BTResult Execute(double dTime)
        {
            if (false == m_bRunning)
                return BTResult.Failed;
            switch (m_kState)
            {
                case EState.CalcAniState:
                    return OnCalcAniState();
                case EState.MoveToTarget:
                    return OnMoveTarget(dTime);
                default:
                    break;
            }

            return BTResult.Success;
        }

        protected virtual BTResult OnCalcAniState()
        {
            return BTResult.Success;
        }

        protected BTResult OnMoveTarget(double dTime)
        {
            double dDeltaMove = m_kPlayer.Velocity * dTime;
            double dDist = m_kPlayer.GetPosition().Distance(m_kTargetPos);
            if(dDist > dDeltaMove)
            {
                m_kPlayer.MoveToPos(m_kTargetPos,dTime);
                return BTResult.Running;
            }
            m_kPlayer.SetAniState(EAniState.Idle);
            return BTResult.Success;
        }

        protected LLGoalKeeper m_kPlayer = null;
        protected LLBall m_kBall = null;
        protected EState m_kState;
        protected Vector3D m_kTargetPos;
        protected double m_dHPRadius;// home pos 活动半径
        protected bool m_bRunning = false; 
    }
}