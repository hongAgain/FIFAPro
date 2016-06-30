using Common;
using Common.Log;
using Common.Tables;

namespace BehaviourTree
{
    public class ActionToMarkWithoutBall : BTAction
    {
        private LLPlayer m_kPlayer = null;
        private double minDist = 4d;
        private double maxDist = 6d;
        private double runRate = 1d;

        private bool needRecalculateFlag = false;
        
        public ActionToMarkWithoutBall()
        {
            Name = "ToMarkWithoutBall";
            DisplayName = "行为:跑向无球盯防位置";
            NodeType = "ActionToMarkWithoutBall";
        }
                
        public override void Activate(BTDatabase kDatabase)
        {
            base.Activate(kDatabase);
            minDist = TableManager.Instance.AIConfig.GetItem("def_radius_def_min").Value;
            maxDist = TableManager.Instance.AIConfig.GetItem("def_radius_def_max").Value;
            runRate = TableManager.Instance.AIConfig.GetItem("speed_rate_run").Value;
        }
        
        protected override void Enter()
        {
            if (m_kPlayer == null)
            {
                int iID = m_kDatabase.GetDataID(BTConstant.Player);
                m_kPlayer = m_kDatabase.GetData<LLPlayer>(iID);
            }
            if (m_kPlayer.State == EPlayerState.ToCloseMark_WithoutBall)
            {                
                m_kPlayer.TargetPos = GenerateTargetPos();
                m_kPlayer.Velocity = m_kPlayer.BaseVelocity * runRate;
                if(!IsPositionCloseEnough())
                    m_kPlayer.SetAniState(EAniState.NormalRun);
            }
        }
        
        protected override BTResult Execute(double dTime)
        {
            if(m_kPlayer == null || m_kPlayer.State != EPlayerState.ToCloseMark_WithoutBall)
                return BTResult.Failed;
            return RunToTarget(dTime);
        }
        
        protected virtual BTResult RunToTarget(double dTime)
        {                       
            m_kPlayer.TargetPos = GenerateTargetPos();
            if (IsPositionValid(dTime))
            {                
                //face opponent, turn into mark without ball
                m_kPlayer.SetPosition(m_kPlayer.TargetPos);
                m_kPlayer.SetRoteAngle(MathUtil.GetAngle(m_kPlayer.GetPosition(), m_kPlayer.Opponent.GetPosition()));
                m_kPlayer.SetState(EPlayerState.CloseMark_WithoutBall);
                return BTResult.Success;
            }
            else
            {
                //run to target position
                m_kPlayer.MoveToPos(m_kPlayer.TargetPos,dTime);
                m_kPlayer.SetRoteAngle(MathUtil.GetAngle(m_kPlayer.GetPosition(), m_kPlayer.TargetPos));
            }
            return BTResult.Running;
        }

        private bool IsPositionCloseEnough()
        {
            return m_kPlayer.GetPosition().Distance(m_kPlayer.TargetPos) < 0.02d;
        }

        private bool IsPositionValid(double dTime)
        {
            return m_kPlayer.GetPosition().Distance(m_kPlayer.TargetPos) < m_kPlayer.Velocity * dTime;
        }

        private Vector3D GenerateTargetPos()
        {
            double distRemain = m_kPlayer.Opponent.GetPosition().Distance(m_kPlayer.Team.Goal.GoalPos);
            if (distRemain < maxDist)
            {                
//                LogManager.Instance.YellowLog("ToCloseMark_WithoutBall To Near");
                //stay near opponent
                return (0.8d * m_kPlayer.Opponent.GetPosition() + 0.2d * m_kPlayer.Team.Goal.GoalPos);
            }
            else
            {
                return m_kPlayer.Team.GetMarkWithoutBallPos(m_kPlayer.Opponent.GetPosition());
            }
        }
        
        protected override void Exit()
        {
            m_kPlayer = null;
        }
    }
}