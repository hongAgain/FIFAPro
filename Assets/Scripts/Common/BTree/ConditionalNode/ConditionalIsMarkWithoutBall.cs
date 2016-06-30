using Common;

namespace BehaviourTree
{
    //judge whether this player is marking without ball
    public class ConditionalIsMarkWithoutBall : BTConditional
    {
        private LLPlayer m_kPlayer = null;

        public ConditionalIsMarkWithoutBall()
        {
            Name = "IsMarkWithoutBall";
            DisplayName = "条件:无球盯防";
            NodeType = "ConditionalIsMarkWithoutBall";
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

        public override bool Check()
        {
            if (null == m_kPlayer)
                return false;
            return m_kPlayer.MarkingStatus == EMarkStatus.MARKWITHOUTBALL && m_kPlayer.Opponent != null;
        }
    }
}