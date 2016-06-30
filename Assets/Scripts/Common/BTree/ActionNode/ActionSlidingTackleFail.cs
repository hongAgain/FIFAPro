using Common;
using Common.Tables;

namespace BehaviourTree
{
    public class ActionSlidingTackleFail : ActionBasicFailAndIdle
    {
        public ActionSlidingTackleFail()
        {
            Name = "SlidingTackleFail";
            DisplayName = "行为:铲断失败";
            NodeType = "ActionSlidingTackleFail";
        }
                
        public override void Activate(BTDatabase kDatabase)
        {
            base.Activate(kDatabase);
            EnteringState = EPlayerState.Sliding_Tackle_Failed;
//            AttackerFinalState = EPlayerState.Avoid_Sliding_Tackle_Success;
            EAS_DefendingFail = EAniState.Ground_Tackle_Failed;
            EAS_IdleAfterFail = EAniState.Idle;
            timeToIdle = TableManager.Instance.AIConfig.GetItem("slide_fail_idle").Value;
            defendingVelocityRate = 1d;
        }

        protected override void OnIdleStart()
        {
            m_kPlayer.Team.InformRefreshCloseMark();
        }
    }
}