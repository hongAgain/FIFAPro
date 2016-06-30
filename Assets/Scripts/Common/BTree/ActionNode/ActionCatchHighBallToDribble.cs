using Common;
using Common.Tables;
namespace BehaviourTree
{
    public class ActionCatchHighBallToDribble : ActionBasicCatchToDribble {
        
        public ActionCatchHighBallToDribble()
        {
            Name = "CatchHighBallToDribble";
            DisplayName = "行为:接高球转为带球";
            NodeType = "ActionCatchHighBallToDribble";
        } 

		protected override void InitializeParams()
		{
			runSpeedRate = TableManager.Instance.AIConfig.GetItem("speed_rate_run").Value;
			catchAnime = EAniState.HeadRob_Carry;
			enteringState = EPlayerState.Heading_Tackle_ToDribble;
			leavingState = EPlayerState.NormalDribble;
			
			hasBallIn = true;
			hasBallOut = false;
			clearDelayTypeOnFinish = true;
			delayType = ETeamStateChangeDelayedType.NONE;
		}
    }
}