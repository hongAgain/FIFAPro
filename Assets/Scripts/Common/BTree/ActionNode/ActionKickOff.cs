using Common;
namespace BehaviourTree
{
    public class ActionKickOff : BTAction
    {
        public ActionKickOff()
        {
            Name = "KickOff";
            DisplayName = "行为:开球";
            NodeType = "ActionKickOff";
        }

        protected override void Enter()
        {
            if (null == m_kPlayer)
            {
                int iID = m_kDatabase.GetDataID(BTConstant.Player);
                m_kPlayer = m_kDatabase.GetData<LLPlayer>(iID);
            }
            if (null != m_kPlayer)
                m_kTeam = m_kPlayer.Team;
        }

        /// <summary>
        /// Called every frame if the action node is active.
        /// </summary>
        protected override BTResult Execute(double fTime)
        {
            BTResult kRet = BTResult.Success;
            if (null == m_kTeam )
            {
                return kRet;
            }
            if(EPlayerState.PS_KICKOFF != m_kPlayer.State)
                return BTResult.Success;
            // 球队选择可以接球的球员
            LLPlayer kPlayer = m_kTeam.SelectBallController();
            if (null != kPlayer)
            {
                originalTargetPos = kPlayer.GetPosition();
                m_kTeam.PassBall(m_kPlayer,kPlayer,EBallMoveType.GroundPass,true);

//                m_kPlayer.SetBallCtrl(false); //传球后,失去控球权
                m_kPlayer.SetState(EPlayerState.HomePos);
            }
            return BTResult.Success;
        }

        /// <summary>
        /// Called when the action node finishes.
        /// </summary>
        protected override void Exit()
        {
            m_kPlayer = null;
        }

        private LLTeam m_kTeam = null;
        private LLPlayer m_kPlayer = null;
        private Vector3D originalTargetPos = new Vector3D ();
    }
}