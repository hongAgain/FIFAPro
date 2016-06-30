namespace BehaviourTree
{

    //判断是否符合中场开球的条件
    public class ConditionalTeamStateChange : BTConditional
    {
        public ConditionalTeamStateChange()
        {
            Name = "ConditionalTeamStateChange";
            DisplayName = "条件:攻防转换";
            NodeType = "ConditionalTeamStateChange";
        }
        public override void Activate(BTDatabase kDatabase)
        {
            base.Activate(kDatabase);
            int iID = m_kDatabase.GetDataID(BTConstant.Team);
            m_kTeam = m_kDatabase.GetData<LLTeam>(iID);
        }

        public override bool Check()
        {
            if (null == m_kTeam)
            {
                int iID = m_kDatabase.GetDataID(BTConstant.Team);
                m_kTeam = m_kDatabase.GetData<LLTeam>(iID);
            }
            //if (TeamState.TS_ATTACK == m_kTeam.State)
            //    return true;
            
            return false;
        }

        private LLTeam m_kTeam = null;
    }
}