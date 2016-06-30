//using Common;
//namespace BehaviourTree
//{
//    /// <summary>
//    /// not used
//    /// </summary>
//    public class ActionKeepDistance : BTAction
//    {
//        private LLPlayer m_kPlayer = null;
//        private LLPlayer targetPlayer = null;
//        private double minDist = 4d;
//        private double maxDist = 6d;
//        private double runRate = 1d;
//
//        public ActionKeepDistance()
//        {
//            Name = "KeepDistance";
//            DisplayName = "行为:与目标玩家保持距离";
//            NodeType = "ActionKeepDistance";
//        }
//
//        protected override void Enter()
//        {
//            if (m_kPlayer == null)
//            {
//                int iID = m_kDatabase.GetDataID(BTConstant.Player);
//                m_kPlayer = m_kDatabase.GetData<LLPlayer>(iID);
//            }
//           
//            targetPlayer = m_kPlayer.Opponent;
//            minDist = m_kPlayer.MinDistanceToKeep;
//            maxDist = m_kPlayer.MaxDistanceToKeep;
//           
//            m_kPlayer.SetAniState(EAniState.Walk);
//            m_kPlayer.Velocity = m_kPlayer.BaseVelocity * runRate;
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
//        protected virtual BTResult RunToTarget(double dTime)
//        {            
//            // run and keep in range
//            double currentDist = m_kPlayer.GetPosition().Distance(targetPlayer.GetPosition());
//            double ds = m_kPlayer.Velocity * dTime;
//            if (currentDist > maxDist)
//            {
//                Vector3D kDir = MathUtil.GetDir(m_kPlayer.GetPosition(), targetPlayer.GetPosition());
//                if (currentDist - ds > maxDist)
//                {
//                    //too far, go closer
//                    Vector3D _v = m_kPlayer.GetPosition() + kDir * ds;
//                    m_kPlayer.SetPosition(_v);
////                    m_kPlayer.RotateAngle = MathUtil.GetAngle(m_kPlayer.Position, targetPlayer.Position);
//                }
//                else
//                {
//                    //reached
//                    Vector3D _v =targetPlayer.GetPosition() + kDir*maxDist;
//                    m_kPlayer.SetPosition(_v);
//                    return BTResult.Success;
//                }
//            } 
//            else if (currentDist < minDist)
//            {
//                Vector3D kDir = MathUtil.GetDir(targetPlayer.GetPosition(), m_kPlayer.GetPosition());
//                if (currentDist + ds < minDist)
//                {
//                    //too close, go further
//                    Vector3D _v = m_kPlayer.GetPosition() + kDir * ds;
//                    m_kPlayer.SetPosition(_v);
////                    m_kPlayer.RotateAngle = MathUtil.GetAngle(targetPlayer.Position, m_kPlayer.Position);
//                }
//                else
//                {
//                    //reached
//                    Vector3D _v = targetPlayer.GetPosition() + kDir * minDist;
//                    m_kPlayer.SetPosition(_v);
//                    return BTResult.Success;
//                }
//            }
//            else
//            {
//                return BTResult.Success;
//            }
//            return BTResult.Running;
//        }
//                
//        protected override void Exit()
//        {
//            m_kPlayer = null;
//            targetPlayer = null;
//        }
//    }
//}