using Common;
using Common.Tables;

namespace BehaviourTree
{
    public class ActionInterceptFail : ActionBasicFailAndIdle
    {
        public ActionInterceptFail()
        {
            Name = "InterceptFail";
            DisplayName = "行为:拦截失败";
            NodeType = "ActionInterceptFail";
        }
        
        public override void Activate(BTDatabase kDatabase)
        {
            base.Activate(kDatabase);
            EnteringState = EPlayerState.Intercept_Failed;
            //            AttackerFinalState = EPlayerState.Avoid_Block_Tackle_Success;
            EAS_DefendingFail = EAniState.Ground_Block_Failed;
            EAS_IdleAfterFail = EAniState.Idle;
            timeToIdle = TableManager.Instance.AIConfig.GetItem("tackle_fail_idle").Value;
            defendingVelocityRate = 0d;
        }

        protected override void Initialize()
        {
            m_kPlayer.SetRoteAngle(MathUtil.GetAngle(m_kPlayer.GetPosition(), m_kPlayer.TargetPos));
//            defendingDir = MathUtil.GetDir(m_kPlayer.Position,m_kPlayer.Opponent.Position);
        }

        protected override void PlayAnime()
        {
            m_kPlayer.SetAniState(EAniState.Idle);
            m_kPlayer.SetAniState(EAS_DefendingFail);
        }
    }
}