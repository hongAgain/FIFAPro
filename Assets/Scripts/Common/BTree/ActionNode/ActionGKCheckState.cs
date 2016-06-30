using Common;
using System;
using Common.Tables;
namespace BehaviourTree
{
    public class ActionGKCheckState : BTAction
    {
        private LLGoalKeeper m_kPlayer = null;
        private LLBall ball = null;

        public ActionGKCheckState()
        {
            Name = "GKCheckState";
            DisplayName = "门将状态检查";
            NodeType = "ActionGKCheckState";
        }

        protected override void Enter()
        {
            int iID = m_kDatabase.GetDataID(BTConstant.Player);
            m_kPlayer = m_kDatabase.GetData<LLGoalKeeper>(iID);
            m_kTeam = m_kPlayer.Team;
            m_kBall = m_kPlayer.Team.Scene.Ball;
        }

        protected override BTResult Execute(double fTime)
        {
            if (null == m_kBall || null == m_kTeam)
                return BTResult.Failed;
            switch(m_kTeam.State)
            {
                case ETeamState.TS_ATTACK:
                    m_kPlayer.GKState = EGKState.GS_DEFAULT;
                    break;
                case ETeamState.TS_DEFEND:
                    if (ETeamColor.Team_Red == m_kTeam.TeamColor)
                    {
                        if (m_kBall.GetPosition().Z > 0)
                            m_kPlayer.GKState = EGKState.GS_DEFAULT;
                        else
                            m_kPlayer.GKState = EGKState.GS_GUARD;
                    }
                    else
                    {
                        if (m_kBall.GetPosition().Z > 0)
                            m_kPlayer.GKState = EGKState.GS_GUARD;
                        else
                            m_kPlayer.GKState = EGKState.GS_DEFAULT;
                    }
                    break;
            }

            return BTResult.Success;
        }

        private LLTeam m_kTeam = null;
        private LLBall m_kBall = null;
    }
}