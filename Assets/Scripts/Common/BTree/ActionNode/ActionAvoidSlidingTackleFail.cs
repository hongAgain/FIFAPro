using Common;
using Common.Tables;

namespace BehaviourTree
{
    public class ActionAvoidSlidingTackleFail : ActionBasicAvoidTackle
    {   
        public ActionAvoidSlidingTackleFail()
        {
            Name = "AvoidSlidingTackleFail";
            DisplayName = "行为:躲过铲断失败";
            NodeType = "ActionAvoidSlidingTackleFail";
        }
        
        public override void Activate(BTDatabase kDatabase)
        {
            base.Activate(kDatabase);
            EnteringState = EPlayerState.Avoid_Sliding_Tackle_Failed;
            LeavingState = EPlayerState.IdleAfterFail;
            EAS_AvoidTackle = EAniState.Stop_Ground_Tackle_Failed;
            ETSCDT_DelayType = ETeamStateChangeDelayedType.DELAYED_ATTACK;
            needClearDelayType = false;
        }
        
        protected override void OnAvoidingOver()
        {
            m_kPlayer.TimeToIdleAfterFail = TableManager.Instance.AIConfig.GetItem ("slide_success_idle").Value;
        }
    }
}