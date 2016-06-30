using Common;
namespace BehaviourTree
{
    public class ActionBlockTackleSucceed : ActionBasicRobBall
    {
        public ActionBlockTackleSucceed()
        {
            Name = "BlockTackleSucceed";
            DisplayName = "行为:抢截成功";
            NodeType = "ActionBlockTackleSucceed";
        }

        public override void Activate(BTDatabase kDatabase)
        {
            base.Activate(kDatabase);
            EnteringState = EPlayerState.Block_Tackle_Success;
//            AttackerFinalState = EPlayerState.Avoid_Block_Tackle_Failed;
            EAS_Defending = EAniState.Ground_Snatch;
            ETSCDT_DelayType = ETeamStateChangeDelayedType.DELAYED_DEFENCE;
            defendingVelocityRate = 0d;
        }
    }
}