using Common;
using Common.Tables;
namespace BehaviourTree
{
    public class ActionHeadingTackleToShoot : ActionBasicHeadingToShoot
    {
        private Vector3D m_kTargetPos = new Vector3D ();

        public ActionHeadingTackleToShoot()
        {
            Name = "HeadingTackleToShoot";
            DisplayName = "行为:防守方高空争顶成功，并头球射门";
            NodeType = "ActionHeadingTackleToShoot";
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
			delayType = ETeamStateChangeDelayedType.DELAYED_DEFENCE;
		}
    }
}