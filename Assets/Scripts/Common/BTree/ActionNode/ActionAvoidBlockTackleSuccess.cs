using Common;
namespace BehaviourTree
{
    public class ActionAvoidBlockTackleSuccess : ActionBasicAvoidTackle
    {   
        public ActionAvoidBlockTackleSuccess()
        {
            Name = "AvoidBlockTackleSuccess";
            DisplayName = "行为:躲过抢截成功";
            NodeType = "ActionAvoidBlockTackleSuccess";
        }
        
        public override void Activate(BTDatabase kDatabase)
        {
            base.Activate(kDatabase);
            EnteringState = EPlayerState.Avoid_Block_Tackle_Success;
            LeavingState = EPlayerState.ActionSelect;
            EAS_AvoidTackle = EAniState.Stop_Ground_Snatch_Success;
            ETSCDT_DelayType = ETeamStateChangeDelayedType.NONE;
            needClearDelayType = true;
        }

        protected override void OnAvoidingOver()
        {
            m_kPlayer.NextPossibleDribbleDir = new int[] {0,1,-1};
        }
    }
}