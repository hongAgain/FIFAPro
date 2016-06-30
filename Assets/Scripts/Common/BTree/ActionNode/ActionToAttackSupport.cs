using Common;

namespace BehaviourTree
{
    public class ActionToAttackSupport : ActionRunToTargetPos
    {
        public ActionToAttackSupport()
        {
            Name = "ToAttackSupport";
            DisplayName = "行为:跑向进攻接应区域";
            NodeType = "ActionToAttackSupport";
        }        
        
        protected override void InitializeParams()
        {
            base.InitializeParams();
            enteringState = EPlayerState.ToAttackSupport;
        }
        
        protected override bool NeedRun()
        {
            //set target position
            if (m_kPlayer.Team.UpdateAttackSupportPos(m_kPlayer))
            {
                return m_kPlayer.GetPosition().Distance(m_kPlayer.TargetPos) > 0.25d;
            }
            else
            {
                m_kPlayer.SetState(EPlayerState.HomePos);
                return false;
            }
        }
        
        protected override void InitPlayer_Enter()
        {
            base.InitPlayer_Enter();
            //set target position
            if (m_kPlayer.Team.UpdateAttackSupportPos(m_kPlayer))
            {
                m_kPlayer.SetRoteAngle(MathUtil.GetAngle(m_kPlayer.GetPosition(), m_kPlayer.TargetPos));
            }
        }

        protected override void UpdateTargetPos_Execute()
        {
            m_kPlayer.SetRoteAngle(MathUtil.GetAngle(m_kPlayer.GetPosition(), m_kPlayer.TargetPos));
        }
        
        protected override void OnArrived_Execute()
        {
            //EPS state is changed here
            m_kPlayer.SetState(EPlayerState.AttackSupport);
        }
    }
}