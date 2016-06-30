using Common;
using Common.Tables;

namespace BehaviourTree
{
    public class ActionDefendBreakThroughFail : ActionBasicFailAndIdle
    {
        public ActionDefendBreakThroughFail()
        {
            Name = "DefendBreakThroughFail";
            DisplayName = "行为:阻止突破失败";
            NodeType = "ActionDefendBreakThroughFail";
        }

        public override void Activate(BTDatabase kDatabase)
        {
            base.Activate(kDatabase);
            EnteringState = EPlayerState.Defend_Break_Through_Failed;
//            AttackerFinalState = EPlayerState.Break_Through_Success;
            EAS_DefendingFail = EAniState.Stop_BreakThrough_Failed;
            EAS_IdleAfterFail = EAniState.Idle;
            timeToIdle = TableManager.Instance.AIConfig.GetItem("breakthrough_success_idle").Value;
            defendingVelocityRate = 0d;
        }

        protected override void OnIdleStart()
        {
            m_kPlayer.Team.InformRefreshCloseMark();
        }
    }
}