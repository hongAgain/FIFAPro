using Common;
using Common.Log;
using Common.Tables;

namespace BehaviourTree
{
    public class ActionMarkWithoutBall : BTAction
    {
        public ActionMarkWithoutBall()
        {
            Name = "MarkWithoutBall";
            DisplayName = "行为:无球盯防";
            NodeType = "ActionMarkWithoutBall";
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
            if (m_kPlayer.State == EPlayerState.CloseMark_WithoutBall)
            {
                m_kPlayer.SetAniState(EAniState.Mark);
                m_kPlayer.SetRoteAngle(MathUtil.GetAngle(m_kPlayer.GetPosition(), m_kPlayer.Opponent.GetPosition()));
            }
        }
        
        protected override BTResult Execute(double fTime)
        {
            if (m_kPlayer.State != EPlayerState.CloseMark_WithoutBall)
                return BTResult.Failed;

            if(IsPositionValid())
            {
                m_kPlayer.SetRoteAngle(MathUtil.GetAngle(m_kPlayer.GetPosition(), m_kPlayer.Opponent.GetPosition()));
                return BTResult.Running;
            }
            else
            {
                //turn into ToMarkWithoutBall state
                m_kPlayer.SetState(EPlayerState.ToCloseMark_WithoutBall);
                return BTResult.Failed;
            }
        }

        private bool IsPositionValid()
        {
            return m_kPlayer.Team.IsInMarkWithoutBallArea(m_kPlayer.Opponent,m_kPlayer);
        }
        
        protected override void Exit()
        {
            m_kPlayer = null;
        }
    }
}