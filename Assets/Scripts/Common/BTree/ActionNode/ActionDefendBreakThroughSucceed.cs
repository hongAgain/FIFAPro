using Common;
namespace BehaviourTree
{
    public class ActionDefendBreakThroughSucceed : ActionBasicRobBall
    {
        public ActionDefendBreakThroughSucceed()
        {
            Name = "DefendBreakThroughSucceed";
            DisplayName = "行为:阻止突破成功";
            NodeType = "ActionDefendBreakThroughSucceed";
        }
        
        public override void Activate(BTDatabase kDatabase)
        {
            base.Activate(kDatabase);
            EnteringState = EPlayerState.Defend_Break_Through_Success;
//            AttackerFinalState = EPlayerState.Break_Through_Failed;
            EAS_Defending = EAniState.Stop_BreakThrough_Success;
            ETSCDT_DelayType = ETeamStateChangeDelayedType.DELAYED_DEFENCE;
            defendingVelocityRate = 0d;
        }
    }
}
