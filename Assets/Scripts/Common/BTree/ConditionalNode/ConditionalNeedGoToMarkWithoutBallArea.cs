using Common;
using Common.Tables;

namespace BehaviourTree
{
    public class ConditionalNeedGoToMarkWithoutBallArea : BTConditional
    {
        private LLPlayer m_kPlayer = null;

        public ConditionalNeedGoToMarkWithoutBallArea()
        {
            Name = "NeedGetCloseToMarkWithoutBall";
            DisplayName = "条件:需要贴近被盯防的非持球人";
            NodeType = "ConditionalNeedGetCloseToMarkWithoutBall";
        }
        
        public override void Activate(BTDatabase kDatabase)
        {    
            base.Activate(kDatabase);
            if (null == m_kPlayer)
            {
                int iID = m_kDatabase.GetDataID(BTConstant.Player);
                m_kPlayer = m_kDatabase.GetData<LLPlayer>(iID);
            }
            m_kPlayer.MinDistanceToKeep = TableManager.Instance.AIConfig.GetItem("def_radius_def_min").Value;
            m_kPlayer.MaxDistanceToKeep = TableManager.Instance.AIConfig.GetItem("def_radius_def_max").Value;
        }
        
        public override bool Check()
        {
            if (m_kPlayer == null || m_kPlayer.Opponent == null)
            {
                return false;
            }
            double currentDist = m_kPlayer.GetPosition().Distance(m_kPlayer.Opponent.GetPosition());
            return (currentDist < m_kPlayer.MinDistanceToKeep && m_kPlayer.MaxDistanceToKeep < currentDist);
        }
    }
}