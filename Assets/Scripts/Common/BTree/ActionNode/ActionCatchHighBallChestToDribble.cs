using Common;
using Common.Tables;
namespace BehaviourTree
{
	public class ActionCatchHighBallChestToDribble : ActionBasicGoCatchBall {
        
        public ActionCatchHighBallChestToDribble()
        {
            Name = "CatchHighBallChestToDribble";
            DisplayName = "行为:接高球转为带球";
            NodeType = "ActionCatchHighBallChestToDribble";
        } 

		protected override void InitializeParams()
		{
			runSpeedRate = TableManager.Instance.AIConfig.GetItem("speed_rate_run").Value;
			catchAnime = EAniState.HeadRob2_Stop;
			enteringState = EPlayerState.Heading_Tackle_ChestToDribble;
			leavingState = EPlayerState.NormalDribble;
			
			hasBallIn = true;
			hasBallOut = false;
			clearDelayTypeOnFinish = true;
			delayType = ETeamStateChangeDelayedType.NONE;
		}
		
		protected override void InitializeAniParams()
		{
            m_kPlayer.KAniData.targetPos = m_kPlayer.Team.Scene.Ball.TargetGroundPos;
			m_kPlayer.KAniData.ballFlyingTime = m_kPlayer.Team.Scene.Ball.FlyingTime;
			m_kPlayer.KAniData.playerSpeed = m_kPlayer.Velocity;
		}
    }
}