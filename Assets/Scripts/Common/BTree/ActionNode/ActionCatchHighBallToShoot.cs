using Common;
using Common.Tables;
namespace BehaviourTree
{
    public class ActionCatchHighBallToShoot : ActionBasicHeadingToShoot {
        
        public ActionCatchHighBallToShoot()
        {
            Name = "CatchHighBallToShoot";
            DisplayName = "行为:接高球转射门";
            NodeType = "ActionCatchHighBallToShoot";
        } 

		protected override void InitializeParams()
		{
			runSpeedRate = TableManager.Instance.AIConfig.GetItem("speed_rate_run").Value;
			catchAnime = EAniState.HeadRob_Shoot;
			enteringState = EPlayerState.Heading_Tackle_ToShoot;
			leavingState = EPlayerState.PS_END;
			
			hasBallIn = true;
			hasBallOut = true;
			clearDelayTypeOnFinish = true;
			delayType = ETeamStateChangeDelayedType.NONE;
		}
    }
}
