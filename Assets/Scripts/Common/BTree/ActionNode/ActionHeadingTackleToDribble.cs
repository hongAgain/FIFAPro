using Common;
using Common.Tables;
namespace BehaviourTree
{
    public class ActionHeadingTackleToDribble : ActionBasicCatchToDribble
    {
        public ActionHeadingTackleToDribble()
        {
            Name = "HeadingTackleToDribble";
            DisplayName = "行为:防守方高空争顶成功，并转移到脚下";
            NodeType = "ActionHeadingTackleToDribble";
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
			delayType = ETeamStateChangeDelayedType.DELAYED_DEFENCE;
		}	
    }
}