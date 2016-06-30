using Common;
namespace BehaviourTree
{
    public class ActionSlidingTackleSucceed : ActionBasicRobBall
    {
        public ActionSlidingTackleSucceed()
        {
            Name = "SlidingTackleSucceed";
            DisplayName = "行为:铲断成功";
            NodeType = "ActionSlidingTackleSucceed";
        }
        
        public override void Activate(BTDatabase kDatabase)
        {
            base.Activate(kDatabase);
            EnteringState = EPlayerState.Sliding_Tackle_Success;
//            AttackerFinalState = EPlayerState.Avoid_Sliding_Tackle_Failed;
            EAS_Defending = EAniState.Ground_Tackle;
            ETSCDT_DelayType = ETeamStateChangeDelayedType.DELAYED_DEFENCE;
            defendingVelocityRate = 0d;
        }
    }
}