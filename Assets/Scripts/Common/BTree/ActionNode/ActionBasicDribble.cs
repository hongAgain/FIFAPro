using Common;
using Common.Tables;

namespace BehaviourTree
{
    public class ActionBasicDribble : BTAction
    {
        private enum EState
        {
            None,
            Dribbling,
            Finished
        }

        protected LLBall m_kBall = null;
        protected LLPlayer m_kPlayer = null;
        private EState eState = EState.None;

        //derived classes should initialize these :
        protected EPlayerState EnteringState = EPlayerState.PS_END;
        protected EAniState EAS_DribbleAnime = EAniState.NormalDribbl;
        protected double dribbleVelocityRate = 0d;    

        private Vector3D m_kDribbleDir = Vector3D.zero;
        private int lastRegionID = -1;

        private double DistRandomMax = 10d;
        private double DistRandomMin = 5d;

        private double BallDistInDribble = 0.4d;

        public ActionBasicDribble()
        {
            Name = "BasicDribble";
            DisplayName = "行为:带球基类";
            NodeType = "ActionBasicDribble";
        }
        
        protected override void Enter()
        {
            if (null == m_kPlayer)
            {
                int iID = m_kDatabase.GetDataID(BTConstant.Player);
                m_kPlayer = m_kDatabase.GetData<LLPlayer>(iID);
            }
            if (m_kPlayer.State == EnteringState)
            {
                BattleCommentMgr.Instance.PostMessage(EBattleCommentType.BCT_Dribble, m_kPlayer);
                InitializeDribble();
                eState = EState.Dribbling;
                PlayAnime();
            }
            else
            {
                eState = EState.None;
            }
        }
        
        protected override BTResult Execute(double fTime)
        {
            if (m_kPlayer.State != EnteringState)
            {
                return BTResult.Failed;
            }
            Move(fTime);
            switch(eState)
            {
                case EState.Dribbling:
                    break;
                case EState.Finished:
                    m_kPlayer.SetState(EPlayerState.ActionSelect);
                    return BTResult.Success;
                default:
                    return BTResult.Failed;
            }
            return BTResult.Running;
        }

        protected virtual void InitializeDribble()
        {
            DistRandomMax = TableManager.Instance.AIConfig.GetItem("dribble_dis_max").Value;
            DistRandomMin = TableManager.Instance.AIConfig.GetItem("dribble_dis_min").Value;

            m_kBall = m_kPlayer.Team.Scene.Ball;

            m_kDribbleDir = m_kPlayer.Team.SelectDribbleDir(m_kPlayer.NextPossibleDribbleDir);
            m_kPlayer.NextPossibleDribbleDir = null;

            lastRegionID = m_kPlayer.RegionID;
            m_kPlayer.TargetPos = m_kPlayer.GetPosition() + m_kDribbleDir * FIFARandom.GetRandomValue(DistRandomMin, DistRandomMax);

            m_kPlayer.Velocity = m_kPlayer.BaseVelocity * dribbleVelocityRate;
        }
        
        protected virtual void PlayAnime()
        {
            m_kPlayer.KAniData.targetPos = m_kPlayer.TargetPos;
            m_kPlayer.SetAniState(EAS_DribbleAnime);
        }

        private void Move(double fTime)
        {
            double nextMoveDist = m_kPlayer.Velocity * fTime;
            if (m_kPlayer.GetPosition().Distance(m_kPlayer.TargetPos) < nextMoveDist)
            {
                //arrived
                m_kPlayer.SetPosition(m_kPlayer.TargetPos);
                eState = EState.Finished;
            }
            else
            {
                // move
                m_kPlayer.MoveToPos(m_kPlayer.TargetPos,fTime);
            }

            m_kBall.SetPosition(m_kPlayer.GetPosition() + m_kDribbleDir * BallDistInDribble);
            ChangeBallPosMessage kMsg = new ChangeBallPosMessage(m_kBall.GetPosition(), m_kBall);
            MessageDispatcher.Instance.SendMessage(kMsg);
            
            //check region change
            CheckRegionChange();
        }

        private void CheckRegionChange()
        {
            if (lastRegionID != m_kPlayer.RegionID)
            {
                m_kPlayer.Team.InformDribbleRegionChanged(lastRegionID,m_kPlayer.RegionID);
                lastRegionID = m_kPlayer.RegionID;
            }
        }
        
        protected override void Exit()
        {
            m_kPlayer = null;
            eState = EState.None;
        }
    }
}