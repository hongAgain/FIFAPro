using Common;
namespace BehaviourTree
{
    public class ActionBreakThroughSucceed : ActionBasicAvoidTackle
    {   
        public ActionBreakThroughSucceed()
        {
            Name = "BreakThroughSucceed";
            DisplayName = "行为:突破成功";
            NodeType = "ActionBreakThroughSucceed";
        }
        
        public override void Activate(BTDatabase kDatabase)
        {
            base.Activate(kDatabase);
            EnteringState = EPlayerState.Break_Through_Success;
            LeavingState = EPlayerState.ActionSelect;
            EAS_AvoidTackle = EAniState.Break_Through;
            ETSCDT_DelayType = ETeamStateChangeDelayedType.NONE;
            needClearDelayType = true;
        }

        protected override void OnAvoidingOver()
        {
            m_kPlayer.NextPossibleDribbleDir = new int[] {0,1,-1};
        }
    }
}