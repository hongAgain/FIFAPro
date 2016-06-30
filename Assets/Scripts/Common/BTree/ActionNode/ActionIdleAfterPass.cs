using Common;
using Common.Tables;
namespace BehaviourTree
{
    public class ActionIdleAfterPass : BTAction
    {
        enum EState
        {
            Normal = 0,
            WaitForTime         // 等待思考结果
        }
        public ActionIdleAfterPass()
        {
            Name = "IdleAfterPass";
            DisplayName = "行为:传球后Idle";
            NodeType = "ActionIdleAfterPass";
        }

        public override void Activate(BTDatabase kDatabase)
        {
            base.Activate(kDatabase);
            m_dTimeToWait = TableManager.Instance.AIConfig.GetItem("after_pass_idle").Value;
        }

        protected override void Enter()
        {
            if (null == m_kPlayer)
            {
                int iID = m_kDatabase.GetDataID(BTConstant.Player);
                m_kPlayer = m_kDatabase.GetData<LLPlayer>(iID);
            }            
            if (EPlayerState.PassBallIdle == m_kPlayer.State)
            {
                m_dElapseTime = 0;
                m_kState = EState.Normal;
            }
        }
        protected override BTResult Execute(double dTime)
        {
            if (m_kPlayer == null)
                return BTResult.Failed;
            if (EPlayerState.PassBallIdle != m_kPlayer.State)
                return BTResult.Failed;
            m_dElapseTime += dTime;
            switch(m_kState)
            {
                case EState.Normal:
                    m_kPlayer.SetAniState(EAniState.Idle);
                    m_kState = EState.WaitForTime;
                    return BTResult.Running;
                case EState.WaitForTime:
                    if (m_dElapseTime > m_dTimeToWait)
                    {
                        m_kPlayer.SetState(EPlayerState.HomePos);
                        return BTResult.Success;
                    }
                    else
                        return BTResult.Running;
                default:
                    break;
            }
            return BTResult.Running;
        }

        protected override void Exit()
        {
            m_kPlayer = null;
        }

        private LLPlayer m_kPlayer = null;
        private double m_dElapseTime = 0d;
        private double m_dTimeToWait = 0d;
        private EState m_kState;
    }
}