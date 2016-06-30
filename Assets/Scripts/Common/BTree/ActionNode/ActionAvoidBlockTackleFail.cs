using Common;
using Common.Tables;

namespace BehaviourTree
{
    public class ActionAvoidBlockTackleFail : ActionBasicAvoidTackle
    {   
        public ActionAvoidBlockTackleFail()
        {
            Name = "AvoidBlockTackleFail";
            DisplayName = "行为:躲过抢截失败";
            NodeType = "ActionAvoidBlockTackleFail";
        }
        
        public override void Activate(BTDatabase kDatabase)
        {
            base.Activate(kDatabase);
            EnteringState = EPlayerState.Avoid_Block_Tackle_Failed;
            LeavingState = EPlayerState.IdleAfterFail;
            EAS_AvoidTackle = EAniState.Stop_Ground_Snatch_Failed;
            ETSCDT_DelayType = ETeamStateChangeDelayedType.DELAYED_ATTACK;
            needClearDelayType = false;
        }
        
        protected override void OnAvoidingOver()
        {
            m_kPlayer.TimeToIdleAfterFail = TableManager.Instance.AIConfig.GetItem ("tackle_success_idle").Value;
        }
    }
}