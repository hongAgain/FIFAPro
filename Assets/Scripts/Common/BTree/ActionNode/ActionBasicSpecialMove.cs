//using Common;
//using Common.Log;
//
//namespace BehaviourTree
//{
//    /// <summary>
//    /// deprecated
//    /// </summary>
//    public class ActionBasicSpecialMove : ActionRuntoTarget_Legacy
//	{		
//        protected enum MoveStatus
//		{
//			None,			
//			Preparation,
//			GettingPosition,
//			Ready
//		}	
//		protected MoveStatus internalStatus = MoveStatus.None;
//
//		public ActionBasicSpecialMove()
//		{
//			Name = "BasicSpecialMove";
//			DisplayName = "行为:特殊行为基类";
//			NodeType = "ActionBasicSpecialMove";
//		}
//
//        public override void Activate(BTDatabase kDatabase)
//        {
//            base.Activate(kDatabase);
//        }
//		
//		protected override void Enter()
//		{
//            base.Enter();
//			internalStatus = MoveStatus.Preparation;
//		}
//		
//		protected override BTResult Execute(double fTime)
//		{
//            if (EnteringState != EPlayerState.PS_END && m_kPlayer.State != EnteringState)
//            {
//                return BTResult.Failed;			
//            }
//            switch (internalStatus) 
//			{
//			case MoveStatus.Preparation:
//				return OnPreparation(fTime);
//			case MoveStatus.GettingPosition:
//				return OnGettingPosition(fTime);
//			case MoveStatus.Ready:
//				return OnReady(fTime);
//			default:
//				return BTResult.Failed;
//			}
//		}
//		
//		protected virtual BTResult OnPreparation(double fTime)
//		{
//			internalStatus = MoveStatus.GettingPosition;
//			return BTResult.Running;
//		}
//		
//		protected virtual BTResult OnGettingPosition(double fTime)
//		{
//			internalStatus = MoveStatus.Ready;
//			return BTResult.Running;			
//		}
//		
//		protected virtual BTResult OnReady(double fTime)
//		{
//			internalStatus = MoveStatus.None;
//			return BTResult.Success;			
//		}
//
//        protected override void OnPositionValid(Vector3D nextPosition)
//        {
//            m_kPlayer.SetPosition(nextPosition);
//        }
//		
//		protected override void Exit()
//		{
//            base.Exit();
//			internalStatus = MoveStatus.None;
//		}
//	}
//}