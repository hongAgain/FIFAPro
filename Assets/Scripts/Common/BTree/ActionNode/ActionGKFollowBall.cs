using Common;
using System;
using Common.Tables;
namespace BehaviourTree
{


    public class ActionGKFollowBall : ActionGKMove
    {
        public ActionGKFollowBall()
        {
            Name = "GKFollowBall";
            DisplayName = "行为:跟随球移动";
            NodeType = "ActionGKFollowBall";
        }



        protected override BTResult Execute(double dTime)
        {
            if (m_kPlayer.GKState != EGKState.GS_DEFAULT)
                return BTResult.Failed;
            return base.Execute(dTime);
        }

        protected override BTResult OnCalcAniState()
        {
            m_kPlayer.SetRoteAngle(MathUtil.GetAngle(m_kPlayer.GetPosition(), m_kTargetPos));
            m_kPlayer.SetAniState(EAniState.Walk);
            m_kState = EState.MoveToTarget;
            return BTResult.Running;
        }
    }
}