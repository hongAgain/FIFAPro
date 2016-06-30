//using System;
//using Common;
//using Common.Tables;
//
//
//namespace BehaviourTree
//{
//    /// <summary>
//    /// deprecated
//    /// </summary>
//    public class ActionBlockTackledThinking : BTAction
//    {
//        public ActionBlockTackledThinking()
//        {
//            Name = "BlockTackledThinking";
//            DisplayName = "行为:被抢截后思考行为";
//            NodeType = "ActionBlockTackledThinking";
//        }
//        public override void Activate(BTDatabase kDatabase)
//        {
//            base.Activate(kDatabase);
//            m_dTackledSuccessTime = TableManager.Instance.AIConfig.GetItem("tackled_success_delay").Value;
//        }
//        protected override void Enter()
//        {
//            if(null == m_kPlayer)
//            {
//                int iID = m_kDatabase.GetDataID(BTConstant.Player);
//                m_kPlayer = m_kDatabase.GetData<LLPlayer>(iID);
//            }
//            m_dElpaseTime = 0;
//            if (EPlayerState.PS_INTERCEPTED_THINKING == m_kPlayer.State)
//                m_kPlayer.SetAniState(EAniState.EAS_InteceptThinking);
//        }
//
//        protected override BTResult Execute(double fTime)
//        {
//            if (null == m_kPlayer)
//                return BTResult.Failed;
//
//            if (EPlayerState.PS_INTERCEPTED_THINKING != m_kPlayer.State)
//            {
//                return BTResult.Failed;
//            }
//            if (EPlayerState.PS_INTERCEPTED_THINKING == m_kPlayer.State)
//                m_kPlayer.SetAniState(EAniState.EAS_InteceptThinking);
//
//            m_dElpaseTime += fTime;
//
//            if (m_dElpaseTime < m_dTackledSuccessTime)
//                return BTResult.Running;
//
//            m_kPlayer.SetState(EPlayerState.Default);
//            return BTResult.Success;
//        }
//
//        protected override void Exit()
//        {
//            m_kPlayer = null;
//        }
//
//        private LLPlayer m_kPlayer = null;
//        private Double m_dElpaseTime = 0;
//        private Double m_dTackledSuccessTime;
//    }
//}