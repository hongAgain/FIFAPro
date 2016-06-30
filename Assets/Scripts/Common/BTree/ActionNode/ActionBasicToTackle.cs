using Common;

namespace BehaviourTree
{
    /// <summary>
    /// Action basic to tackle. targetPos and Rotation is set by LLTeam.ActivateDefenceEvent();
    /// </summary>
    public class ActionBasicToTackle : ActionRunToTargetPos
    {        
        protected double minDistToDefend = 2d;
        protected double maxTimeToTrack = 3d;
        private double timeSpentToTrack = 0d;       

        public ActionBasicToTackle()
        {
            Name = "BasicToTackle";
            DisplayName = "行为:发动地面断球行为基类";
            NodeType = "ActionBasicToTackle";
        }

        #region ====== override functions ======
        protected override void InitializeParams()
        {
            timeSpentToTrack = 0;
            runAnime = EAniState.Mark_Ball;
        }

        protected override bool NeedRun()
        {
            double distance = m_kPlayer.GetPosition().Distance(m_kPlayer.Opponent.GetPosition());
            return distance >= minDistToDefend;
        }

        protected override void UpdateTargetPos_Execute()
        {
            m_kPlayer.TargetPos = m_kPlayer.Opponent.GetPosition();
        }

        //run to my m_kPlayer.TargetPos, until we say its valid
        protected override void RunToTargetPos_Execute(double dTime)
        {
            timeSpentToTrack+=dTime;
            if(timeSpentToTrack >= maxTimeToTrack)
            {
                //tackle no more
                m_kPlayer.SetState(EPlayerState.CloseMark_WithBall_NotActivated);
                return;
            }
            
            if (NeedRun())
            {
                //move
                m_kPlayer.MoveToPos(m_kPlayer.TargetPos,dTime);
            }
            else
            {
                //arrived
                Vector3D _v = m_kPlayer.Opponent.GetPosition() + minDistToDefend * MathUtil.GetDir(m_kPlayer.Opponent.GetPosition(), m_kPlayer.GetPosition());
                m_kPlayer.SetPosition(_v);
                iState = InternalState.ARRIVED;
            }
        }
        #endregion
    }
}