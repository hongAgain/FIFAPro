using Common;
namespace BehaviourTree
{
    public class ActionAvoidSlidingTackleSuccess : ActionBasicAvoidTackle
    {   
        public ActionAvoidSlidingTackleSuccess()
        {
            Name = "AvoidSlidingTackleSuccess";
            DisplayName = "行为:躲过铲断成功";
            NodeType = "ActionAvoidSlidingTackleSuccess";
        }
        
        public override void Activate(BTDatabase kDatabase)
        {
            base.Activate(kDatabase);
            EnteringState = EPlayerState.Avoid_Sliding_Tackle_Success;
            LeavingState = EPlayerState.ActionSelect;
            EAS_AvoidTackle = EAniState.Stop_Ground_Tackle_Success;
            ETSCDT_DelayType = ETeamStateChangeDelayedType.NONE;
            needClearDelayType = true;
        }

        protected override void OnAvoidingOver()
        {
            m_kPlayer.NextPossibleDribbleDir = new int[] {0,1,-1};
        }
    }
}