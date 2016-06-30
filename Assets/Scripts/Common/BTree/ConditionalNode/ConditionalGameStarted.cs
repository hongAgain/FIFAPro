namespace BehaviourTree
{

    //判断是否已开始游戏
    public class ConditionalGameStarted : BTConditional
    {
        public ConditionalGameStarted()
        {
            Name = "GameStarted";
            DisplayName = "条件:开始比赛";
            NodeType = "ConditionalGameStarted";
        }

        public override void Activate(BTDatabase kDatabase)
        {
            base.Activate(kDatabase);
            if(null == m_kDatabase)
                return;
            int iID = m_kDatabase.GetDataID(BTConstant.Player);
            LLPlayer kPlayer = m_kDatabase.GetData<LLPlayer>(iID);
            if(null != kPlayer && null != kPlayer.Team)
                m_kScene = kPlayer.Team.Scene;
        }
        public override bool Check()
        {
            if(null == m_kScene)
            {
                int iID = m_kDatabase.GetDataID(BTConstant.Player);
                LLPlayer kPlayer = m_kDatabase.GetData<LLPlayer>(iID);
                if (null != kPlayer && null != kPlayer.Team)
                    m_kScene = kPlayer.Team.Scene;
            }
            if (null == m_kScene)
                return false;
            if (EGameState.GS_RUNNING <= m_kScene.GameState)
                return true;
            return true;
        }

        private LLScene m_kScene = null;

    }
}