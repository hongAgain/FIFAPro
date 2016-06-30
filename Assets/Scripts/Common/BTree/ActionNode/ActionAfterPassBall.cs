//using Common;
//namespace BehaviourTree
//{
//    /// <summary>
//    /// not used
//    /// </summary>
//    public class ActionAfterPassBall : BTAction
//    {
//        enum EState
//        {
//            Normal = 0,
//            Wait
//        }
//        public ActionAfterPassBall()
//        {
//            Name = "AfterPassBall";
//            DisplayName = "行为:传球后Idle";
//            NodeType = "ActionAfterPassBall";
//        }
//
//        protected override void Enter()
//        {
//            if (null == m_kPlayer)
//            {
//                int iID = m_kDatabase.GetDataID(BTConstant.Player);
//                m_kPlayer = m_kDatabase.GetData<LLPlayer>(iID);
//            }
//            m_dElapseTime = 0;
//            m_kState = EState.Normal;
//                
//        }
//        protected override BTResult Execute(double dTime)
//        {
//            m_dElapseTime += dTime;
//            switch(m_kState)
//            {
//                case EState.Normal:
//                    m_kPlayer.SetAniState(EAniState.Idle);
//                    m_kState = EState.Wait;
//                    return BTResult.Running;
//                case EState.Wait:
//                    if (m_dElapseTime > 2)
//                    {
//                        m_kPlayer.SetState(EPlayerState.HomePos);
//                        return BTResult.Success;
//                    }
//                    else
//                        return BTResult.Running;
//                default:
//                    break;
//            }
//            return BTResult.Running;
//        }
//
//        protected override void Exit()
//        {
//            m_kPlayer = null;
//        }
//
//        private LLPlayer m_kPlayer = null;
//        private double m_dElapseTime = 0;
//        private EState m_kState;
//    }
//}