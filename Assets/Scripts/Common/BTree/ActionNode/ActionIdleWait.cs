using Common;
namespace BehaviourTree
{
    public class ActionIdleWait : BTAction
    {
        public ActionIdleWait()
        {
            Name = "IdleWait";
            DisplayName = "行为:原地待機";
            NodeType = "ActionIdleWait";
        }


        protected override void Enter()
        {
            if (null == m_kPlayer)
            {
                int iID = m_kDatabase.GetDataID(BTConstant.Player);
                m_kPlayer = m_kDatabase.GetData<LLPlayer>(iID);
            }
            if (EPlayerState.PS_WAIT_IDLE == m_kPlayer.State)
                m_kPlayer.SetAniState(EAniState.Idle);
        }

        protected override BTResult Execute(double fTime)
        {
            if (EPlayerState.PS_WAIT_IDLE == m_kPlayer.State)
                return BTResult.Running;
            return BTResult.Failed;
        }

        protected override void Exit()
        {
            m_kPlayer = null;
        }

        private LLPlayer m_kPlayer = null;
    }
}