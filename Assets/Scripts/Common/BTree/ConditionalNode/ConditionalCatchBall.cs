using Common;

namespace BehaviourTree
{

    //判断是否符合中场开球的条件
    public class ConditionalCatchBall : BTConditional
    {
        public ConditionalCatchBall()
        {
            Name = "Catch Ball";
            DisplayName = "条件:接球状态";
            NodeType = "ConditionalCatchBall";
        }
        public override bool Check()
        {
            if (null == m_kPlayer)
            {
                int iID = m_kDatabase.GetDataID(BTConstant.Player);
                m_kPlayer = m_kDatabase.GetData<LLPlayer>(iID);
            }

            if (null == m_kPlayer)
                return false;

            if (EPlayerState.Catch_GroundBall == m_kPlayer.State)
                return true;
            return false;
        }

        private LLPlayer m_kPlayer;
    }
}