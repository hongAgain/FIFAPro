using Common;

namespace BehaviourTree
{

    //判断是否符合中场开球的条件
    public class ConditionalDefendTeamState : BTConditional
    {
        public ConditionalDefendTeamState()
        {
            Name = "DefendTeamState";
            DisplayName = "条件:防守状态";
            NodeType = "ConditionalDefendTeamState";
        }
        public override void Activate(BTDatabase kDatabase)
        {
            base.Activate(kDatabase);
            int iID = m_kDatabase.GetDataID(BTConstant.Team);
            m_kTeam = m_kDatabase.GetData<LLTeam>(iID);
            
            iID = m_kDatabase.GetDataID(BTConstant.Player);
            m_kPlayer = m_kDatabase.GetData<LLPlayer>(iID);
        }

        public override bool Check()
        {
            if (m_kPlayer != null)
            {
                if(m_kPlayer.StateChangeDelayedType == ETeamStateChangeDelayedType.DELAYED_DEFENCE)
                    return true;
                if(m_kPlayer.StateChangeDelayedType == ETeamStateChangeDelayedType.DELAYED_ATTACK)
                    return false;
            }
            if (ETeamState.TS_DEFEND == m_kTeam.State)
                return true;
            return false;
        }
        private LLPlayer m_kPlayer = null;
        private LLTeam m_kTeam = null;
    }
}