//using Common;
//namespace BehaviourTree
//{
//    /// <summary>
//    /// deprecated
//    /// </summary>
//    public class ActionFollowTarget : ActionRuntoTarget
//    {
//        public ActionFollowTarget()
//        {
//            Name = "FollowTarget";
//            DisplayName = "行为:跟随";
//            NodeType = "ActionFollowTarget";
//        }
//
//        protected override BTResult Execute(double dTime)
//        {
//            if (null == m_kPlayer)
//                return BTResult.Failed;
//
//            if (EPlayerState.PS_FOLLOWING_TARGET != m_kPlayer.State)
//            {
//                return BTResult.Failed;
//            }
//                
//            //m_kPlayer.State = EPlayerMainState.PS_CLOSEMARKING_ENTERING;
//            BTResult kRet = RunToTarget(dTime);
//            if (BTResult.Success == kRet)
//            {
////                if(true == m_kPlayer.ManMarkWithBall)
////                    m_kPlayer.SetState(EPlayerState.PS_CLOSEMARKING_ENTERING);
//            }
//
//            m_kPlayer.RotateAngle = MathUtil.GetAngle(m_kPlayer.Position, m_kPlayer.Team.Scene.Ball.Position);
//            return kRet;
//        }
//
//    }
//}