namespace BehaviourTree
{

    //判断是否符合中场开球的条件
    public class ConditionalMidField : BTConditional
    {
        public ConditionalMidField()
        {
            Name = "ConditionalMidField";
            DisplayName = "条件:中场开球";
            NodeType = "ConditionalMidField";
        }

        public override void Activate(BTDatabase kDatabase)
        {
            base.Activate(kDatabase);
            if(null != m_kDatabase)
            {
                int iID = kDatabase.GetDataID(BTConstant.Team);
                m_kTeam = kDatabase.GetData<LLTeam>(iID);
            }
            
        }
        public override bool Check()
        {
            if (null == m_kTeam)
                return false;

            if (EGameState.GS_MIDKICK == m_kTeam.Scene.GameState)
                return true;
            return false;
        }

        private LLTeam m_kTeam = null;
    }
}