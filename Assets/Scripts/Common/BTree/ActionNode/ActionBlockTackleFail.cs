using Common;
using Common.Tables;

namespace BehaviourTree
{
    public class ActionBlockTackleFail : ActionBasicFailAndIdle
    {
        public ActionBlockTackleFail()
        {
            Name = "BlockTackleFail";
            DisplayName = "行为:抢截失败";
            NodeType = "ActionBlockTackleFail";
        }
        
        public override void Activate(BTDatabase kDatabase)
        {
            base.Activate(kDatabase);
            EnteringState = EPlayerState.Block_Tackle_Failed;
//            AttackerFinalState = EPlayerState.Avoid_Block_Tackle_Success;
            EAS_DefendingFail = EAniState.Ground_Snatch_Failed;
            EAS_IdleAfterFail = EAniState.Idle;
            timeToIdle = TableManager.Instance.AIConfig.GetItem("tackle_fail_idle").Value;
            defendingVelocityRate = 0d;
        }

        protected override void OnIdleStart()
        {
            m_kPlayer.Team.InformRefreshCloseMark();
        }
    }
}