using Common;
using Common.Tables;

namespace BehaviourTree
{
    public class ActionGetCloseToMarkWithBall : ActionRunToTargetPos
    {        
        double markAreaRadius = 3d;

        public ActionGetCloseToMarkWithBall()
        {
            Name = "GetCloseToMarkWithBall";
            DisplayName = "行为:跑向盯防区域";
            NodeType = "ActionGetCloseToMarkWithBall";
        }        

        protected override void InitializeParams()
        {
            base.InitializeParams();
            enteringState = EPlayerState.CloseMark_WithBall_GetCloser;
            markAreaRadius = TableManager.Instance.AIConfig.GetItem("mark_position_deviate").Value;
        }

        protected override bool NeedRun()
        {
            return !m_kPlayer.Team.CheckInMarkBallArea(m_kPlayer,m_kPlayer.Opponent);
        }

        protected override void InitPlayer_Enter()
        {
            base.InitPlayer_Enter();
            //set target position
            m_kPlayer.TargetPos = m_kPlayer.Opponent.TargetPos;
            m_kPlayer.SetRoteAngle(MathUtil.GetAngle(m_kPlayer.GetPosition(), m_kPlayer.TargetPos));
        }
        
        protected override void UpdateTargetPos_Execute()
        {
            //set target position
//            m_kPlayer.TargetPos = m_kPlayer.Opponent.TargetPos;
            m_kPlayer.TargetPos = m_kPlayer.Opponent.GetPosition()
                +MathUtil.GetDir(m_kPlayer.Opponent.GetPosition(),m_kPlayer.Opponent.TargetPos)
                *System.Math.Min(m_kPlayer.Opponent.GetPosition().Distance(m_kPlayer.Opponent.TargetPos),markAreaRadius);
            m_kPlayer.SetRoteAngle(MathUtil.GetAngle(m_kPlayer.GetPosition(), m_kPlayer.TargetPos));
        }
        
        protected override void OnArrived_Execute()
        {
            m_kPlayer.SetRoteAngle(MathUtil.GetAngle(m_kPlayer.GetPosition(), m_kPlayer.Opponent.GetPosition()));
            //EPS state is changed here
            m_kPlayer.Team.CheckDefensiveEvent(m_kPlayer.Opponent,m_kPlayer);
        }
    }
}