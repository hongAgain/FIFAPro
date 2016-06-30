using Common;
namespace BehaviourTree
{
    public class ActionWaitForKickOff : BTAction
    {
        public ActionWaitForKickOff()
        {
            Name = "WaitForKickOff";
            DisplayName = "行为:等待开球";
            NodeType = "ActionWaitForKickOff";
        }

        protected override void Enter()
        {
            if (null == m_kPlayer)
            {
                int iID = m_kDatabase.GetDataID(BTConstant.Player);
                m_kPlayer = m_kDatabase.GetData<LLPlayer>(iID);
            }
            // 动画进入等待开球状态
            if (EPlayerState.PS_WAITFORKICKOFF == m_kPlayer.State)
                    m_kPlayer.SetAniState(EAniState.Idle);
        }

        /// <summary>
        /// Called every frame if the action node is active.
        /// </summary>
        protected override BTResult Execute(double fTime)
        {
            if (m_kPlayer.Team.Scene.GameState == EGameState.GS_MIDKICK)
            { 
                if(EPlayerState.PS_WAITFORKICKOFF == m_kPlayer.State)
                    return BTResult.Running;
                return BTResult.Success;
            }
            else
            { 
                if (m_kPlayer.State == EPlayerState.PS_WAITFORKICKOFF)
                {
                    m_kPlayer.SetState(EPlayerState.HomePos);
                    return BTResult.Success;
                }
                return BTResult.Success;
            }
        }

        /// <summary>
        /// Called when the action node finishes.
        /// </summary>
        protected override void Exit()
        {
            m_kPlayer = null;
        }
        private LLPlayer m_kPlayer = null;
    }
}