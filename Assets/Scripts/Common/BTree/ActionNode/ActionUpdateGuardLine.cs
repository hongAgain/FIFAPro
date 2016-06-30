namespace BehaviourTree
{
    public class ActionUpdateGuardLine : BTAction
    {
        public ActionUpdateGuardLine()
        {
            Name = "UpdateGuardLine";
            DisplayName = "行为:更新基准线";
            NodeType = "ActionUpdateGuardLine";
        }
        protected override void Enter()
        {
            int iID = m_kDatabase.GetDataID(BTConstant.Team);
            m_kTeam = m_kDatabase.GetData<LLTeam>(iID);
        }

        /// <summary>
        /// Called every frame if the action node is active.
        /// </summary>
        protected override BTResult Execute(double fTime)
        {
            if (null != m_kTeam)
                m_kTeam.UpdateGuardLine();            
            return BTResult.Success;
        }

        /// <summary>
        /// Called when the action node finishes.
        /// </summary>
        protected override void Exit()
        {
            m_kTeam = null;
        }

       

        private LLTeam m_kTeam = null;
    }
}