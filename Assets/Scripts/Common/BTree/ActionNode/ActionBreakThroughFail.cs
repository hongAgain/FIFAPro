using Common;
using Common.Tables;

namespace BehaviourTree
{
    public class ActionBreakThroughFail : ActionBasicAvoidTackle
    {   
        public ActionBreakThroughFail()
        {
            Name = "BreakThroughFail";
            DisplayName = "行为:突破失败";
            NodeType = "ActionBreakThroughFail";
        }
        
        public override void Activate(BTDatabase kDatabase)
        {
            base.Activate(kDatabase);
            EnteringState = EPlayerState.Break_Through_Failed;
            LeavingState = EPlayerState.IdleAfterFail;
            EAS_AvoidTackle = EAniState.Break_Through_Failed;
            ETSCDT_DelayType = ETeamStateChangeDelayedType.DELAYED_ATTACK;
            needClearDelayType = false;
        }

        protected override void OnAvoidingOver()
        {
            m_kPlayer.TimeToIdleAfterFail = TableManager.Instance.AIConfig.GetItem ("breakthrough_fail_idle").Value;
        }
    }
}