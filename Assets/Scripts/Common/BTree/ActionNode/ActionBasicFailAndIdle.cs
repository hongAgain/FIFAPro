using Common;
using Common.Log;
namespace BehaviourTree
{
    public class ActionBasicFailAndIdle : BTAction
    {
        private enum EState
        {
            None,
            DefendingFail,
            IdleAfterFail,
            Finished
        }
        
        protected LLPlayer m_kPlayer = null;
        private EState eState = EState.None;
        private double timeSpentForIdle = 0d;

        //derived classes should initialize these :
        protected EPlayerState EnteringState = EPlayerState.PS_END;
//        protected EPlayerState AttackerFinalState = EPlayerState.PS_END;
        protected EAniState EAS_DefendingFail = EAniState.EAS_NULL;
        protected EAniState EAS_IdleAfterFail = EAniState.EAS_NULL;
        protected ETeamStateChangeDelayedType ETSCDT_DelayType = ETeamStateChangeDelayedType.NONE;
        protected double timeToIdle = 1d;
        protected double defendingVelocityRate = 0d;

        private Vector3D defendingDir = new Vector3D ();

        
        public ActionBasicFailAndIdle()
        {
            Name = "BasicFailAndIdle";
            DisplayName = "行为:行为失败后待机";
            NodeType = "ActionBasicFailAndIdle";
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
//                if(m_kPlayer.Team.State == ETeamState.TS_DEFEND && m_kPlayer.MarkingStatus == EMarkStatus.MARKWITHBALL)
//                {
//                    LogManager.Instance.GreenLog("NodeType: "+NodeType+" - "+m_kPlayer.PlayerBaseInfo.HeroID);
//                }

                Initialize();
                eState = EState.DefendingFail;
                m_kPlayer.StateChangeDelayedType = ETSCDT_DelayType;
                m_kPlayer.SetRoteAngle(MathUtil.GetAngle(m_kPlayer.GetPosition(),m_kPlayer.Opponent.GetPosition()));
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
            switch(eState)
            {
                case EState.DefendingFail:
                    OnDefendingFail(fTime);
                    break;
                case EState.IdleAfterFail:
                    OnIdleAfterFail(fTime);
                    break;
                case EState.Finished:
                    return BTResult.Success;
                default:
                    return BTResult.Failed;
            }
            return BTResult.Running;
        }

        protected virtual void Initialize()
        {
            m_kPlayer.SetRoteAngle(MathUtil.GetAngle(m_kPlayer.GetPosition(), m_kPlayer.Opponent.GetPosition()));
            defendingDir = MathUtil.GetDir(m_kPlayer.GetPosition(), m_kPlayer.Opponent.GetPosition());
        }

        protected virtual void PlayAnime()
        {
            m_kPlayer.SetAniState(EAS_DefendingFail);
        }
        
        private void OnDefendingFail(double fTime)
        {
            if(m_kPlayer.AniFinish)
            {
//                LogManager.Instance.GreenLog("NodeType: "+NodeType+" AniFinish - "+m_kPlayer.PlayerBaseInfo.HeroID);
                eState = EState.IdleAfterFail;
                m_kPlayer.SetAniState(EAS_IdleAfterFail);
                OnIdleStart();
                return;
            }
            // move with speed : defendingVelocity
//            m_kPlayer.Position += MathUtil.GetDirFormAngle(m_kPlayer.RotateAngle) * m_kPlayer.BaseVelocity * defendingVelocityRate * fTime;
//            m_kPlayer.Position += defendingDir * m_kPlayer.BaseVelocity * defendingVelocityRate * fTime;
        }
        
        private void OnIdleAfterFail(double fTime)
        {
            //idle for some time
            timeSpentForIdle += fTime;
            if (timeSpentForIdle >= timeToIdle)
            {
//                LogManager.Instance.GreenLog("NodeType: "+NodeType+" IdleFinish - "+m_kPlayer.PlayerBaseInfo.HeroID);
                //set next state for safety concern
                m_kPlayer.SetState(EPlayerState.HomePos);
                m_kPlayer.ResetDelayedState();
                eState = EState.Finished;
            }
        }

        protected virtual void OnIdleStart()
        {

        }
        
        protected override void Exit()
        {
//            m_kPlayer.StateChangeDelayedType = ETeamStateChangeDelayedType.NONE;
            m_kPlayer = null;
            eState = EState.None;
            timeSpentForIdle = 0d;
        }
    }
}