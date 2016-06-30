using Common;

namespace BehaviourTree
{

    //判断是否符合中场开球的条件
    public class ConditionalStoppingBallPassToPlayer : BTConditional
    {
        public ConditionalStoppingBallPassToPlayer()
        {
            Name = "StoppingBallPassToPlayer";
            DisplayName = "条件:进攻状态";
            NodeType = "ConditionalStoppingBallPassToPlayer";
        }

        public override void Activate(BTDatabase kDatabase)
        {
            base.Activate(kDatabase);
            int iID = m_kDatabase.GetDataID(BTConstant.Player);
            m_kPlayer = m_kDatabase.GetData<LLPlayer>(iID);
        }

        public override bool Check()
        {
            if (m_kPlayer.State == EPlayerState.PassBall)
                return true;
            return false;
        }

        private LLPlayer m_kPlayer = null;
    }
}