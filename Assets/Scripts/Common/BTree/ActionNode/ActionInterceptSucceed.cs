using Common;
namespace BehaviourTree
{
    public class ActionInterceptSucceed : ActionBasicRobBall
    {
        public ActionInterceptSucceed()
        {
            Name = "InterceptSucceed";
            DisplayName = "行为:拦截成功";
            NodeType = "ActionInterceptSucceed";
        }
        
        public override void Activate(BTDatabase kDatabase)
        {
            base.Activate(kDatabase);
            EnteringState = EPlayerState.Intercept_Success;
            //            AttackerFinalState = EPlayerState.Avoid_Block_Tackle_Failed;
            EAS_Defending = EAniState.Ground_Block;
            ETSCDT_DelayType = ETeamStateChangeDelayedType.DELAYED_DEFENCE;
            defendingVelocityRate = 0d;
        }

        protected override void PlayAnime()
        {
            //for safety confern
            m_kPlayer.SetAniState(EAniState.Idle);
            m_kPlayer.SetAniState(EAS_Defending);
        }
    }
}