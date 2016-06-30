using Common;
using System;
using Common.Tables;
namespace BehaviourTree
{
    public class ActionGKGuard : ActionGKMove
    {

        

        public ActionGKGuard()
        {
            Name = "GKGuard";
            DisplayName = "门将警戒";
            NodeType = "ActionGKGuard";
        }

        protected override BTResult Execute(double dTime)
        {
            if (EGKState.GS_GUARD != m_kPlayer.GKState)
                return BTResult.Failed;
            return base.Execute(dTime);
        }

        protected override BTResult OnCalcAniState()
        {
            // 计算门将面向与目标点位置的夹角
            double dGKAngle = m_kPlayer.GetRotAngle();
            double dTargetAngle = MathUtil.GetAngle(m_kPlayer.GetPosition(), m_kPlayer.HomePos);
            double dAngleDelta = Math.Abs(dGKAngle - dTargetAngle);

            LLTeam kTeam = m_kPlayer.Team;
            switch (kTeam.TeamColor)
            {
                case ETeamColor.Team_Red:
                    if (dAngleDelta < 15)
                    {
                        if (m_kPlayer.GetPosition().Z <= m_kPlayer.HomePos.Z)
                            m_kPlayer.SetAniState(EAniState.GK_FrontWalk);
                        else
                            m_kPlayer.SetAniState(EAniState.GK_BackWalk);
                    }
                    else
                    {
                        if (m_kPlayer.GetPosition().X <= m_kPlayer.HomePos.X)
                            m_kPlayer.SetAniState(EAniState.GK_RightWalk);
                        else
                            m_kPlayer.SetAniState(EAniState.GK_LeftWalk);
                    }
                    break;
                case ETeamColor.Team_Blue:
                    if (dAngleDelta < 15)
                    {
                        if (m_kPlayer.GetPosition().Z >= m_kPlayer.HomePos.Z)
                            m_kPlayer.SetAniState(EAniState.GK_FrontWalk);
                        else
                            m_kPlayer.SetAniState(EAniState.GK_BackWalk);
                    }
                    else
                    {
                        if (m_kPlayer.GetPosition().X >= m_kPlayer.HomePos.X)
                            m_kPlayer.SetAniState(EAniState.GK_RightWalk);
                        else
                            m_kPlayer.SetAniState(EAniState.GK_LeftWalk);
                    }
                    break;
            }

            m_kState = EState.MoveToTarget;
            return BTResult.Running;
        }
    }
}