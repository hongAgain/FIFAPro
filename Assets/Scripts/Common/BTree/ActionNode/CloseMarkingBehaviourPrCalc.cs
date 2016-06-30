using Common;
using System;
using Common.Tables;
namespace BehaviourTree
{
    /// <summary>
    /// deprecated
    /// </summary>
//    public class CloseMarkingBehaviourPrCalc : BTAction
//    {
//
//        protected enum EState
//        {
//            CalcState = 0,
//            FollowState,
//            RuntoPos,
//            InterceptState,
//        }
//        public CloseMarkingBehaviourPrCalc()
//        {
//            Name = "CloseMarkingBehaviourPrCalc";
//            DisplayName = "盯防后事件概率计算";
//            NodeType = "CloseMarkingBehaviourPrCalc";
//        }
//
//        public override void Activate(BTDatabase kDatabase)
//        {
//            base.Activate(kDatabase);
//            AICfgItem kItem = TableManager.Instance.AIConfig.GetItem("safe_area_tackler");
//            m_iSafePr = (int)kItem.Value;
//            kItem = TableManager.Instance.AIConfig.GetItem("dager_area_tackler");
//            m_iDangerPr = (int)kItem.Value;
//        }
//
//        protected override void Enter()
//        {
//            if (null == m_kPlayer)
//            {
//                int iID = m_kDatabase.GetDataID(BTConstant.Player);
//                m_kPlayer = m_kDatabase.GetData<LLPlayer>(iID);
//            }
//            if (null != m_kPlayer)
//                m_kTeam = m_kPlayer.Team;
//            m_kState = EState.CalcState;
//        }
//
//        /// <summary>
//        /// Called every frame if the action node is active.
//        /// </summary>
//        protected override BTResult Execute(double fTime)
//        {
//            if (EPlayerState.PS_CLOSEMARK_WITH_BALL != m_kPlayer.State)
//                return BTResult.Failed;
//
//            switch(m_kState)
//            {
//                case EState.CalcState:
//                    return OnCalcState();
//                case EState.FollowState:
//                    return OnFollowTargetState();
//                case EState.RuntoPos:
//                    return OnRunToPosState(fTime);
//                case EState.InterceptState:
//                    return OnInteceptState();
//                default:
//                    break;
//            }
//
//            return BTResult.Failed;
//        }
//
//        protected BTResult OnFollowTargetState()
//        {
//            m_kPlayer.SetState(EPlayerState.PS_FOLLOWING_TARGET);//跟随状态
//            return BTResult.Success;
//        }
//
//        // 截球 状态  
//        protected BTResult OnInteceptState()
//        {
//            float fPr = (float)FIFARandom.GetRandomValue(0,1);
//            if(fPr < GlobalBattleInfo.Instance.SnatchPr)
//                m_kPlayer.SetState(EPlayerState.PS_INTERCEPT_BALL);  
//            else
//                m_kPlayer.SetState(EPlayerState.Default);//跟随状态
//            return BTResult.Success; 
//        }
//
//        protected override void Exit()
//        {
//            m_kPlayer = null;
//        }
//
//        protected BTResult OnCalcState()
//        {
//            int iRegionID = m_kPlayer.RegionID;
//            if(1 <= iRegionID && iRegionID <= 10)
//            {
//                // 危险区
//                FIFARandom.Select(new int[] { m_iSafePr, 100- m_iSafePr }, new FIFARandom.OnSelect[] { FollowFunc, SnatchFunc });
//            }
//            else
//            {
//                // 非危险区
//                FIFARandom.Select(new int[] { m_iDangerPr, 100 - m_iDangerPr }, new FIFARandom.OnSelect[] { FollowFunc, SnatchFunc });
//            }
//            return BTResult.Running;
//        }
//
//        protected void FollowFunc()
//        {
//            m_kState = EState.FollowState;
//        }
//
//        protected void SnatchFunc()
//        {
//            m_kPlayer.SetAniState(EAniState.EAS_Defend);
//            m_kState = EState.RuntoPos;
//        }
//        protected BTResult OnRunToPosState(double dTime)
//        {
//            LLBall kBall = m_kTeam.Scene.Ball;
//            if (false == kBall.ArrivedTargetPos())
//                return BTResult.Running;
//            m_kPlayer.SetAniState(EAniState.EAS_DefendRun);
//            m_kState = EState.InterceptState;
//            return BTResult.Running;
//        }
//        protected EState m_kState;
//        private LLTeam m_kTeam = null;
//        private LLPlayer m_kPlayer = null;
//        private int m_iSafePr = 0;
//        private int m_iDangerPr = 0;
//    }
}