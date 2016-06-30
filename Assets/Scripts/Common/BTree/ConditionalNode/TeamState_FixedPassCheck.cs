namespace BehaviourTree
{

    //判断是否已开始游戏
    public class TeamState_FixedPassCheck : BTConditional
    {
        public TeamState_FixedPassCheck()
        {
            Name = "TeamState_FixedPassCheck";
            DisplayName = "条件:固定传送状态检查";
            NodeType = "TeamState_FixedPassCheck";
        }

        public override void Activate(BTDatabase kDatabase)
        {
            base.Activate(kDatabase);
            if (null == m_kDatabase)
                return;
            int iID = m_kDatabase.GetDataID(BTConstant.Team);
            m_kTeam = m_kDatabase.GetData<LLTeam>(iID);
        }
        public override bool Check()
        {
            if (m_kTeam.Scene.GameState != EGameState.GS_FIX_PASS)
                return false;
            return false;
        }

        private LLTeam m_kTeam = null;
    }
}