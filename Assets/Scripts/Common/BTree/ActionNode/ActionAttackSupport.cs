using Common;
using Common.Log;
using Common.Tables;

namespace BehaviourTree
{
    public class ActionAttackSupport : BTAction
    {
        public ActionAttackSupport()
        {
            Name = "AttackSupport";
            DisplayName = "行为:接应";
            NodeType = "ActionAttackSupport";
        }
        private LLPlayer m_kPlayer = null;
        
        public override void Activate(BTDatabase kDatabase)
        {
            base.Activate(kDatabase);
        }
        protected override void Enter()
        {
            if (null == m_kPlayer)
            {
                int iID = m_kDatabase.GetDataID(BTConstant.Player);
                m_kPlayer = m_kDatabase.GetData<LLPlayer>(iID);
            }
            if (m_kPlayer.State == EPlayerState.AttackSupport)
            {
                m_kPlayer.SetAniState(EAniState.Mark);
                m_kPlayer.SetRoteAngle(MathUtil.GetAngle(m_kPlayer.GetPosition(), m_kPlayer.Team.BallController.GetPosition()));
            }
        }
        
        protected override BTResult Execute(double fTime)
        {
            if (m_kPlayer.State != EPlayerState.AttackSupport)
                return BTResult.Failed;

            if (m_kPlayer.Team.UpdateAttackSupportPos(m_kPlayer))
            {
                if(IsPositionValid())
                {
                    m_kPlayer.SetRoteAngle(MathUtil.GetAngle(m_kPlayer.GetPosition(), m_kPlayer.Team.BallController.GetPosition()));
                    return BTResult.Running;
                }
                else
                {
                    //turn into ToMarkWithoutBall state
                    m_kPlayer.SetState(EPlayerState.ToAttackSupport);
                    return BTResult.Failed;
                }
            }
            else
            {
                m_kPlayer.SetState(EPlayerState.HomePos);
                return BTResult.Failed;
            }
        }
        
        private bool IsPositionValid()
        {
            return m_kPlayer.GetPosition().Distance(m_kPlayer.TargetPos) < 4d;
        }
        
        protected override void Exit()
        {
            m_kPlayer = null;
        }
    }
}