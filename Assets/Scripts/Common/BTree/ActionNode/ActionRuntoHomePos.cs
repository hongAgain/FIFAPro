using Common;
using Common.Log;
using Common.Tables;

namespace BehaviourTree
{
    public class ActionRuntoHomePos : BTAction
    {
        public ActionRuntoHomePos()
        {
            Name = "RuntoHomePos";
            DisplayName = "行为:球员归位";
            NodeType = "ActionRuntoHomePos";
        }

        public override void Activate(BTDatabase kDatabase)
        {
            base.Activate(kDatabase);
            m_dHomePosRadius = TableManager.Instance.AIConfig.GetItem("homeposition_radius").Value;
            mDistanceRunWalk = TableManager.Instance.AIConfig.GetItem("distance_run_walk").Value;
            mRunRate = TableManager.Instance.AIConfig.GetItem("speed_rate_run").Value;
            mWalkRate = TableManager.Instance.AIConfig.GetItem("speed_rate_walk").Value;
        }

        protected override void Enter()
        {
            if (null == m_kPlayer)
            {
                int iID = m_kDatabase.GetDataID(BTConstant.Player);
                m_kPlayer = m_kDatabase.GetData<LLPlayer>(iID);
            }
            if (null != m_kPlayer)
                m_kTeam = m_kPlayer.Team;

            if (m_kPlayer.State == EPlayerState.HomePos)
            {
                if (m_kPlayer.GetPosition().Distance(m_kPlayer.HomePos) > m_dHomePosRadius)
                {
                    //                LogManager.Instance.BlackLog(NodeType+" run1 "+EAniState.Walk);
                    m_kPlayer.SetAniState(EAniState.Walk);
                    m_bRunable = true;
                } else
                {
                    //                if (EPlayerState.HomePos != m_kPlayer.State)
                    //                {
                    //                    
                    ////                    LogManager.Instance.BlackLog(NodeType+" run2 "+EAniState.Walk);
                    //                    m_kPlayer.SetAniState(EAniState.Walk);
                    //                }
                    //                else
                    m_kPlayer.SetAniState(EAniState.Idle);
                    m_kPlayer.SetRoteAngle(MathUtil.GetAngle(m_kPlayer.GetPosition(), m_kPlayer.Team.Scene.Ball.GetPosition()));
                    m_bRunable = false;
                }
                //            m_kPlayer.SetState(EPlayerState.HomePos);

                m_kPlayer.Velocity = m_kPlayer.BaseVelocity;
            }
        }

        protected override BTResult Execute(double fTime)
        {
            if (m_kPlayer == null || false == m_bRunable)
                return BTResult.Failed;
            
            if (m_kPlayer.State != EPlayerState.HomePos)
                return BTResult.Failed;

            BTResult kRet = BTResult.Running;
            
            // 球员的朝向是向着目标点的
            double fDist = m_kPlayer.GetPosition().Distance(m_kPlayer.HomePos);
            if (fDist >= mDistanceRunWalk)
                m_kPlayer.Velocity = m_kPlayer.BaseVelocity * mRunRate;
            else
                m_kPlayer.Velocity = m_kPlayer.BaseVelocity * mWalkRate;

            if (false == m_bRunable && fDist > 2.5f)
            {
                RefreshHomePositionMessage _msg = new RefreshHomePositionMessage(m_kTeam.Scene);
                MessageDispatcher.Instance.SendMessage(_msg);
                m_bRunable = true;
                return BTResult.Success;
            }
            double ds = m_kPlayer.Velocity * fTime;
            if (fDist > ds)
            {
//                LogManager.Instance.BlackLog(NodeType+" run3 "+EAniState.Walk);
                m_kPlayer.SetAniState(EAniState.Walk);
                m_kPlayer.MoveToPos(m_kPlayer.HomePos,fTime);
                m_kPlayer.SetRoteAngle(MathUtil.GetAngle(m_kPlayer.GetPosition(), m_kPlayer.HomePos));
            }
            else
            {
                m_kPlayer.ForceHomePostionClose = false;
                m_bRunable = false;
                m_kPlayer.SetPosition(m_kPlayer.HomePos);
                kRet = BTResult.Success;
            }

            return kRet;
        }

        protected override void Exit()
        {
            m_kPlayer = null;
        }

        private LLTeam m_kTeam = null;
        private LLPlayer m_kPlayer = null;
        private bool m_bRunable = true;
        private double m_dHomePosRadius = 0;
        private double mDistanceRunWalk = 0;
        private double mRunRate = 1d;
        private double mWalkRate = 1d;
    }
}