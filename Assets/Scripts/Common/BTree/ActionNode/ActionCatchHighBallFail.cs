using Common;
using Common.Tables;

namespace BehaviourTree
{
	public class ActionCatchHighBallFail : ActionBasicGoCatchBall
    {
        public ActionCatchHighBallFail()
        {
            Name = "CatchHighBallFail";
            DisplayName = "行为:进攻方接头球失败";
            NodeType = "ActionCatchHighBallFail";
        }
				
		protected override void InitializeParams()
		{
			runSpeedRate = TableManager.Instance.AIConfig.GetItem("speed_rate_run").Value;
			catchAnime = EAniState.Head_Tackle_Failed;
			enteringState = EPlayerState.Heading_Tackle_Failed;
			//fail and idle
			leavingState = EPlayerState.IdleAfterFail;
			
			hasBallIn = false;
			hasBallOut = false;
			clearDelayTypeOnFinish = false;
			delayType = ETeamStateChangeDelayedType.DELAYED_ATTACK;
		}
		
		protected override void InitializeAniParams()
		{
            m_kPlayer.KAniData.targetPos = m_kPlayer.Team.Scene.Ball.TargetGroundPos;
			m_kPlayer.KAniData.ballFlyingTime = m_kPlayer.Team.Scene.Ball.FlyingTime;
			m_kPlayer.KAniData.playerSpeed = m_kPlayer.Velocity;
		}

		protected override void InitializePlayer()
		{
			base.InitializePlayer ();
			m_kPlayer.TimeToIdleAfterFail = TableManager.Instance.AIConfig.GetItem ("after_head_shoot_idle").Value;
		}  

		protected virtual void OnAniFinish()
		{
			m_kPlayer.SetBallCtrl (false);
		}
    }
}