using Common;
using Common.Tables;
namespace BehaviourTree
{
	public class ActionIdleAfterFail : BTAction
	{
		private LLPlayer m_kPlayer = null;
		private double m_dElapseTime = 0d;
		private double m_dTimeToWait = 1d;

		public ActionIdleAfterFail()
		{
			Name = "IdleAfterFail";
			DisplayName = "行为:行为失败后Idle";
			NodeType = "ActionIdleAfterFail";
		}

		protected override void Enter()
		{
			if (m_kPlayer == null)
			{
				int iID = m_kDatabase.GetDataID(BTConstant.Player);
				m_kPlayer = m_kDatabase.GetData<LLPlayer>(iID);
			}            
			if ( m_kPlayer.State == EPlayerState.IdleAfterFail )
			{
				m_dElapseTime = 0d;
				m_kPlayer.SetAniState(EAniState.Idle);
			}
		}
		protected override BTResult Execute(double dTime)
		{
			if (m_kPlayer == null || m_kPlayer.State != EPlayerState.IdleAfterFail)
				return BTResult.Failed;
			m_dElapseTime += dTime;
			if (m_dElapseTime > m_dTimeToWait) 
			{
				m_kPlayer.SetState (EPlayerState.HomePos);
                m_kPlayer.ResetDelayedState();
				return BTResult.Success;
			}
			return BTResult.Running;
		}

		protected override void Exit()
		{
			m_kPlayer.TimeToIdleAfterFail = 0d;

//			m_kPlayer.StateChangeDelayedType = ETeamStateChangeDelayedType.NONE;
			m_kPlayer = null;
		}
	}
}