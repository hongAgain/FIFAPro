namespace BehaviourTree
{
    public class ActionSetBallTarget : BTAction
    {
        public ActionSetBallTarget()
        {
            Name = "SetBallTarget";
            DisplayName = "行为:设置球的目标落点";
            NodeType = "ActionSetBallTarget";
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
            //选择下一个控球球员
            if (null == m_kTeam)
                return BTResult.Success;
            LLPlayer kPlayer = m_kTeam.SelectBallController();
            Vector3D kTargetPos = kPlayer.GetPosition();
            int iID = m_kDatabase.GetDataID(BTConstant.BallTargetPos);
            m_kDatabase.SetData<Vector3D>(iID, kTargetPos);
            m_kTeam.Scene.Ball.SetTarget(kTargetPos);
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
        
    }
}