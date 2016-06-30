using Common;
namespace BehaviourTree
{

    //判断是否符合中场开球的条件
    public class ConditionalCarryingBall : BTConditional
    {
        public ConditionalCarryingBall()
        {
            Name = "CarryingBall";
            DisplayName = "条件:是否处于带球状态";
            NodeType = "ConditionalCarryingBall";
        }

        public override void Activate(BTDatabase kDatabase)
        {
            base.Activate(kDatabase);
            int iID = m_kDatabase.GetDataID(BTConstant.Player);
            m_kPlayer = m_kDatabase.GetData<LLPlayer>(iID);
        }

        public override bool Check()
        {
            if (m_kPlayer.State == EPlayerState.NormalDribble)
            {
                return true;
            }
            return false;
        }


        private LLPlayer m_kPlayer = null;
    }
}