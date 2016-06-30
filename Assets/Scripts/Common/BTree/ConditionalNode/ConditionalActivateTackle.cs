using Common;
using Common.Tables;

namespace BehaviourTree
{
    //judge whether will activate slide/block tackle
    public class ConditionalActivateTackle : BTConditional
    {
        private LLPlayer m_kPlayer = null;

        public ConditionalActivateTackle()
        {
            Name = "ActivateTackle";
            DisplayName = "条件:触发抢截/铲断";
            NodeType = "ConditionalActivateTackle";
        }

        public override void Activate(BTDatabase kDatabase)
        {
            base.Activate(kDatabase);
            if (m_kPlayer == null)
            {
                int iID = m_kDatabase.GetDataID(BTConstant.Player);
                m_kPlayer = m_kDatabase.GetData<LLPlayer>(iID);
            }
        }

        //judgement will be based on region
        public override bool Check()
        {
            if (null == m_kPlayer)
                return false;
            return m_kPlayer.Team.CheckTacklesInCloseMark(m_kPlayer.Opponent,m_kPlayer);
        }
    }
}