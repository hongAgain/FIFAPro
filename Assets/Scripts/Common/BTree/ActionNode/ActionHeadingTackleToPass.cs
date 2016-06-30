using Common;
using Common.Tables;
namespace BehaviourTree
{
	public class ActionHeadingTackleToPass : ActionBasicGoCatchBall
    {
        public ActionHeadingTackleToPass()
        {
            Name = "HeadingTackleToPass";
            DisplayName = "行为:防守方高空争顶成功，并头球传球";
            NodeType = "ActionHeadingTackleToPass";
        }

        private LLPlayer targetPlayer = null;
        private Vector3D originalTargetPos = new Vector3D ();

		protected override void InitializeParams()
		{
			runSpeedRate = TableManager.Instance.AIConfig.GetItem("speed_rate_run").Value;
			catchAnime = EAniState.HeadRob_Pass;
			enteringState = EPlayerState.Heading_Tackle_ToPass;
			leavingState = EPlayerState.HomePos;
			
			hasBallIn = true;
			hasBallOut = true;
			clearDelayTypeOnFinish = true;
			delayType = ETeamStateChangeDelayedType.DELAYED_DEFENCE;
		}
		
		protected override void InitializePlayer()
		{
			base.InitializePlayer ();
			//find next player to pass
			targetPlayer = m_kPlayer.Team.SelectPlayerForHeadingPass(m_kPlayer,true);
            originalTargetPos = targetPlayer.GetPosition();
		}
		
		protected override void InitializeAniParams()
		{
            m_kPlayer.KAniData.targetPos = m_kPlayer.Team.Scene.Ball.TargetGroundPos;
			m_kPlayer.KAniData.targetBallOutPos = originalTargetPos;
			m_kPlayer.KAniData.ballFlyingTime = m_kPlayer.Team.Scene.Ball.FlyingTime;
            m_kPlayer.KAniData.playerSpeed = m_kPlayer.Velocity;
            m_kPlayer.KAniData.headRobAvil = true;
		}
		
		protected override void OnBallOut()
		{
			if (targetPlayer != m_kPlayer)
			{
				//pass the ball
                m_kPlayer.Team.PassBall(m_kPlayer, targetPlayer, EBallMoveType.HeadingToHighLob,true,false);
			}
			else
			{
				//usually you will not get into this state with no one else to pass
				//forced to control back the ball
				leavingState = EPlayerState.ActionSelect;
				//weird isn't it?
			}
		}
    }
}