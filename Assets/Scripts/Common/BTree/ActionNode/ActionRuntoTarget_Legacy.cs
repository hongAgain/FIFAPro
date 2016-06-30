//using Common;
//using Common.Tables;
//
//namespace BehaviourTree
//{
//    /// <summary>
//    /// deprecated
//    /// </summary>
//    public class ActionRuntoTarget_Legacy : BTAction
//    {
//        public ActionRuntoTarget_Legacy()
//        {
//            Name = "RuntoTarget";
//            DisplayName = "行为:跑向目标位置";
//            NodeType = "ActionRuntoTarget";
//        }
//
//        protected EAniState runningAnime = EAniState.NormalRun;
//        protected EPlayerState EnteringState = EPlayerState.PS_END;
//
//        public override void Activate(BTDatabase kDatabase)
//        {
//            base.Activate(kDatabase);
//            m_dRunRate = TableManager.Instance.AIConfig.GetItem("speed_rate_run").Value;
//            runningAnime = EAniState.NormalRun;
//        }
//        protected override void Enter()
//        {
//            if (null == m_kPlayer)
//            {
//                int iID = m_kDatabase.GetDataID(BTConstant.Player);
//                m_kPlayer = m_kDatabase.GetData<LLPlayer>(iID);
//            }
//            if (EnteringState != EPlayerState.PS_END && m_kPlayer.State == EnteringState)
//            {
//                if (!IsCurrentPositionValid())
//                {
//                    if(runningAnime != EAniState.EAS_NULL)
//                    {
//                        m_kPlayer.KAniData.targetPos = m_kPlayer.TargetPos;
//                        m_kPlayer.SetAniState(runningAnime);
//                        m_kPlayer.Velocity = m_kPlayer.BaseVelocity * m_dRunRate;
//                    }
//                }
//            }
//        }
//
//        protected override BTResult Execute(double dTime)
//        {
//            if(null == m_kPlayer)
//                return BTResult.Failed;
//
//            return RunToTarget(dTime);
//        }
//
//		//run to my target position, until we say its valid
//        protected virtual BTResult RunToTarget(double dTime)
//        {
////            if (IsCurrentPositionValid())
////            {
////                  arrived
////                  OnPositionValid();
////                  return BTResult.Success;
////            }
//            //calculate next position
//            Vector3D nextPosition = m_kPlayer.GetPosition() + MathUtil.GetDir(m_kPlayer.GetPosition(), m_kPlayer.TargetPos) * m_kPlayer.Velocity * dTime;
//			//judge it
//			if (IsPositionValid(dTime))
//            {
//                //arrived
//                OnPositionValid(nextPosition);
//                return BTResult.Success;
//            }
//            else
//            {
//				OnPositionNotValid(nextPosition);
//            }
//            return BTResult.Running;
//        }
//
//		#region ========== determining functions ===========
//        protected virtual bool IsCurrentPositionValid()
//        {
//            return false;
//        }
//
//		protected virtual bool IsPositionValid(double dTime)
//		{
//			//a little problem lies here: 
//			// when you are not turning instantly and not always moving on a straight line, you may get passed 
////			double distRemain = nextPosition.Distance(m_kPlayer.TargetPos);
//			double distMovedNext = m_kPlayer.Velocity * dTime;
//            double distRemain = m_kPlayer.GetPosition().Distance(m_kPlayer.TargetPos);
//
//			return (distRemain <= distMovedNext);
//		}
//
//        protected virtual void OnPositionValid(Vector3D nextPosition)
//		{
//            //set to target position
//            m_kPlayer.SetPosition(m_kPlayer.TargetPos);
////            m_kPlayer.Position = nextPosition;
//		}
//		
//		protected virtual void OnPositionNotValid(Vector3D nextPosition)
//		{
//			//move to next position
//			// 球员的朝向是向着目标点的
//            m_kPlayer.SetPosition(nextPosition);
////			m_kPlayer.RotateAngle = MathUtil.GetAngle(m_kPlayer.Position, m_kPlayer.TargetPos);
//		}
//		#endregion
//       
//        protected override void Exit()
//        {
//            m_kPlayer = null;
//        }
//        private double m_dRunRate = 1d;
//        protected LLPlayer m_kPlayer = null;
//    }
//}