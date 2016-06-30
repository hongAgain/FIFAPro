using Common;
using Common.Tables;

namespace BehaviourTree
{
    public class ActionToIntercept : ActionRunToTargetPos
    {
        protected double minDistToDefend = 2d;

        public ActionToIntercept()
        {
            Name = "ToIntercept";
            DisplayName = "行为:发动拦截";
            NodeType = "ActionToIntercept";
        }        
        
        protected override void InitializeParams()
        {
            base.InitializeParams();
            runSpeedRate = TableManager.Instance.AIConfig.GetItem("speed_rate_block").Value;
            enteringState = EPlayerState.Intercept;
        }
        
        protected override bool NeedRun()
        {
            return (!m_kPlayer.Team.Scene.Ball.ArrivedTargetPos()) && m_kPlayer.GetPosition().Distance(m_kPlayer.TargetPos) >= minDistToDefend;
        }
        
        protected override void InitPlayer_Enter()
        {
            base.InitPlayer_Enter();
            //set target position
            m_kPlayer.SetRoteAngle(MathUtil.GetAngle(m_kPlayer.GetPosition(), m_kPlayer.TargetPos));
        }

        protected override void OnArrived_Execute()
        {
            //check whether intercepting the ball will succeseed
            m_kPlayer.Team.CheckIntercept(m_kPlayer);
        }
    }
}