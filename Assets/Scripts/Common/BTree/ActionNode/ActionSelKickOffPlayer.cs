namespace BehaviourTree
{
    /// <summary>
    /// Used in TeamAI
    /// </summary>
    public class ActionSelKickOffPlayer : BTAction
    {
        public ActionSelKickOffPlayer()
        {
            Name = "SelKickOffPlayer";
            DisplayName = "行为:选择开球球员";
            NodeType = "ActionSelKickOffPlayer";
        }

        protected override void Enter()
        {
            if (null != m_kDatabase)
            {
                int iID = m_kDatabase.GetDataID(BTConstant.Team);
                m_kTeam = m_kDatabase.GetData<LLTeam>(iID);
            }
        }

        /// <summary>
        /// Called every frame if the action node is active.
        /// </summary>
        protected override BTResult Execute(double fTime)
        {
            if (null == m_kTeam)
                return BTResult.Failed;
            LLPlayer kPlayer = m_kTeam.SelectKickOffPlayer();
            if (null != kPlayer)
                kPlayer.SetState(Common.EPlayerState.Catch_GroundBall);
            m_kTeam.ChangeGameState(EGameState.GS_FIX_PASS);
            return BTResult.Success;
        }

        protected override void Exit()
        {
        }

        private LLTeam m_kTeam = null;
    }
}