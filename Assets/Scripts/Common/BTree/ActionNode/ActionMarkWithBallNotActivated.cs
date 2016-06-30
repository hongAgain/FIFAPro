using Common;
namespace BehaviourTree
{
	/// <summary>
	/// default action for Close-Mark-With-Ball
	/// </summary>
    public class ActionMarkWithBallNotActivated : BTAction
    {
        private LLPlayer m_kPlayer = null;
        public ActionMarkWithBallNotActivated()
        {
            Name = "MarkWithBallNotActivated";
            DisplayName = "行为:未触发盯防，通知进攻方强制出球";
            NodeType = "ActionMarkWithBallNotActivated";
        }

        protected override void Enter()
        {
            if (m_kPlayer == null)
            {
                int iID = m_kDatabase.GetDataID(BTConstant.Player);
                m_kPlayer = m_kDatabase.GetData<LLPlayer>(iID);
            }  
//			if (m_kPlayer.State != EPlayerState.CloseMark_WithBall_NotActivated)
//				m_kPlayer.SetState (EPlayerState.CloseMark_WithBall_NotActivated);
        }
        
        protected override BTResult Execute(double fTime)
        {
			if (m_kPlayer == null) 
			{
				return BTResult.Failed;
			}

            if (m_kPlayer.State != EPlayerState.CloseMark_WithBall_NotActivated)
                return BTResult.Failed;

            //inform ballController force lowSpeedDribble
            m_kPlayer.MarkingStatus = EMarkStatus.NONE;
            m_kPlayer.SetState(EPlayerState.HomePos);

            //MarkingStatus and state may change
            m_kPlayer.Team.Opponent.InformForcedBallOut();
            return BTResult.Success;
        }
        
        protected override void Exit()
        {
            m_kPlayer = null;
        }
    }
}