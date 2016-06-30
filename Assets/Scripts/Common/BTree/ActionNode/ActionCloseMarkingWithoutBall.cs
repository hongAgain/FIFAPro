//using Common;
//using Common.Tables;
//namespace BehaviourTree
//{
//    /// <summary>
//    /// deprecated
//    /// </summary>
//    public class ActionCloseMarkingWithoutBall : ActionRuntoTarget
//    {
//        //无球盯防状态
//        protected enum EState
//        {
//            RunToMarkPos = 0, // 跑向盯防位置
//            MarkState,  // 盯防状态
//            ExitMark , // 退出盯防状态
//
//        }
//        public ActionCloseMarkingWithoutBall()
//        {
//            Name = "CloseMarkingWithoutBall";
//            DisplayName = "行为:无球盯防行为";
//            NodeType = "ActionCloseMarkingWithoutBall";
//        }
//
//        public override void Activate(BTDatabase kDatabase)
//        {
//            base.Activate(kDatabase);
//            AICfgItem kItem = TableManager.Instance.AIConfig.GetItem("def_radius_atk");
//            m_dDefRadAtk = kItem.Value;
//            kItem = TableManager.Instance.AIConfig.GetItem("def_radius_def_min");
//            m_dDefRadDefMin = kItem.Value;
//            kItem = TableManager.Instance.AIConfig.GetItem("def_radius_def_max");
//            m_dDefRadDefMax = kItem.Value;
//        }
//        protected override void Enter()
//        {
//            base.Enter();
//            m_kState = EState.RunToMarkPos;
//        }
//
//        protected override BTResult Execute(double dTime)
//        {
//            if (null == m_kPlayer)
//                return BTResult.Failed;
//
//            if (EPlayerState.PS_CLOSEMARK_WITHOUT_BALL != m_kPlayer.State)
//            {
//                return BTResult.Failed;
//            }
//
//            // 无盯防对象
//            if (null == m_kPlayer.Opponent)
//            {
//                m_kPlayer.SetState(EPlayerState.Default);
//                return BTResult.Failed;
//            }
//
//            switch(m_kState)
//            {
//                case EState.RunToMarkPos:
//                    return RuntoMarkPosState(dTime);
//                case EState.MarkState:
//                    return OnMarkState(dTime);
//                case EState.ExitMark:
//                    m_kPlayer.SetState(EPlayerState.Default);
//                    return BTResult.Failed;
//                default:
//                    break;
//            }
//            return BTResult.Failed;
//        }
//
//        protected override void Exit()
//        {
//            m_kPlayer.Opponent = null;
//            base.Exit();
//        }
//
//        protected BTResult RuntoMarkPosState(double dTime)
//        {
//            Vector3D kDir = m_kPlayer.Team.Goal.GoalPos - m_kPlayer.Opponent.Position;
//            kDir = kDir.Normalize();
//            m_kPlayer.TargetPos = m_kPlayer.Opponent.Position + kDir * m_dDefRadAtk;
//            BTResult kRet = RunToTarget(dTime);
//            if (BTResult.Success == kRet)
//            {
//                m_kState = EState.MarkState;
//                m_kPlayer.SetAniState(EAniState.EAS_Defend);
//            }
//            m_kPlayer.RotateAngle = MathUtil.GetAngle(m_kPlayer.Position, m_kPlayer.Opponent.Position);
//            return BTResult.Running;
//        }
//
//        protected BTResult OnMarkState(double dTime)
//        {
//            // 检查是否出了盯防区域 
//            if (m_kPlayer.Opponent.RegionID > BTConstant.Instance.AttackMaxRegionID || m_kPlayer.Opponent.RegionID < BTConstant.Instance.AttackMinRegionID)
//            {
//                if (m_kPlayer.Opponent.Opponent == m_kPlayer)
//                {
//                    m_kPlayer.Opponent.Opponent = null;
//                }
//                m_kPlayer.Opponent = null;
//                m_kState = EState.ExitMark;
//                return BTResult.Running;
//            }
//
//            // 检查球员是否出了盯防的某个区域
//            double dLen = m_kPlayer.Position.Distance(m_kPlayer.Opponent.Position);
//
//            if (dLen < m_dDefRadDefMin || dLen > m_dDefRadDefMax)
//            {
//                m_kPlayer.SetAniState(EAniState.EAS_DefendRun);
//                m_kState = EState.RunToMarkPos;
//            }
//            m_kPlayer.RotateAngle = MathUtil.GetAngle(m_kPlayer.Position, m_kPlayer.Opponent.Position);
//            return BTResult.Running;
//        }
//
//        private EState m_kState;
//
//        protected double m_dDefRadAtk = 0;
//        protected double m_dDefRadDefMin = 0;
//        protected double m_dDefRadDefMax = 0;
//    }
//}