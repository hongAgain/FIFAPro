using Common;
namespace BehaviourTree
{
    public class ActionIdle : BTAction
    {
        public ActionIdle()
        {
            Name = "Idle";
            DisplayName = "行为:待机状态";
            NodeType = "ActionIdle";

        }

        protected override void Enter()
        {
            if (null == m_kPlayer)
            {
                int iID = m_kDatabase.GetDataID(BTConstant.Player);
                m_kPlayer = m_kDatabase.GetData<LLPlayer>(iID);
            }
            if (EPlayerState.Idle == m_kPlayer.State)
            {
                m_kPlayer.SetAniState(EAniState.Idle);
                m_kPlayer.MarkingStatus = EMarkStatus.NONE;
            }
                
        }

        protected override BTResult Execute(double fTime)
        {
            if (EPlayerState.Idle == m_kPlayer.State)
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