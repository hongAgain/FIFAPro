using Common;
using Common.Log;
namespace BehaviourTree
{
    public class ActionBasicRobBall : BTAction
    {
        
        private enum EState
        {
            None,
            Defending,
            AfterGetBall
        }
        
        protected LLPlayer m_kPlayer = null;
        private EState eState = EState.None;

        //derived classes should initialize these :
        protected EPlayerState EnteringState = EPlayerState.PS_END;
//        protected EPlayerState AttackerFinalState = EPlayerState.PS_END;
        protected EAniState EAS_Defending = EAniState.EAS_NULL;
        protected ETeamStateChangeDelayedType ETSCDT_DelayType = ETeamStateChangeDelayedType.NONE;
        protected double defendingVelocityRate = 0d;

//        protected Vector3D defendingDir = new Vector3D();
        
        public ActionBasicRobBall()
        {
            Name = "BasicRobBall";
            DisplayName = "行为:断球成功基类";
            NodeType = "ActionBasicRobBall";
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
                Initialize();
                eState = EState.Defending;
                m_kPlayer.StateChangeDelayedType = ETSCDT_DelayType;
                PlayAnime();
            } 
            else
                eState = EState.None;
        }
        
        protected override BTResult Execute(double fTime)
        {
            if (m_kPlayer.State != EnteringState)
            {
                return BTResult.Failed;
            }
            
//            Move(fTime);
            switch(eState)
            {
                case EState.Defending:
                    OnDefending(fTime);
                    break;
                case EState.AfterGetBall:
                    return OnAfterGetBall(fTime);
                default:
                    return BTResult.Failed;
            }
            return BTResult.Running;
        }
        
        private void OnDefending(double fTime)
        {
            if(CheckGetBall())
            {
//                LogManager.Instance.YellowLog("NodeType:"+NodeType+" - "+m_kPlayer.PlayerBaseInfo.HeroID+" Ball In");
                eState = EState.AfterGetBall;                
                OnBallIn();
                return;
            }
        }

        protected virtual void Initialize()
        {
            m_kPlayer.SetRoteAngle(MathUtil.GetAngle(m_kPlayer.GetPosition(), m_kPlayer.Opponent.GetPosition()));
//            defendingDir = MathUtil.GetDir(m_kPlayer.GetPosition(), m_kPlayer.Opponent.GetPosition());
        }

        protected virtual void PlayAnime()
        {
            //for safety confern
            m_kPlayer.SetAniState(EAniState.Mark_Ball);
            m_kPlayer.SetAniState(EAS_Defending);
        }

        protected virtual bool CheckGetBall()
        {
            return m_kPlayer.IsBallIn;
        }
        
        protected virtual void OnBallIn()
        {
            BallVisableMeassage _ballMsg = new BallVisableMeassage(m_kPlayer.Opponent, false);
            MessageDispatcher.Instance.SendMessage(_ballMsg);

            m_kPlayer.SetBallCtrl(true);
            m_kPlayer.Team.BallController = m_kPlayer;

            //change on next frame
            m_kPlayer.Team.Scene.RequireTeamStateChange(ETeamStateChangeType.TSCT_INTERCEPT, true);
//            LogManager.Instance.RedLog(m_kPlayer.Opponent.TeamType+" "+m_kPlayer.Opponent.PlayerBaseInfo.HeroID+"'s Ball stolen");
        }

        protected virtual void OnAnimeFinish()
        {
//			m_kPlayer.SetState (EPlayerState.Catch_GroundBall);

        }

        protected virtual BTResult OnAfterGetBall(double fTime)
        {
            if (m_kPlayer.State != EnteringState)
                return BTResult.Failed;
            if (m_kPlayer.AniFinish)
            {             
//                LogManager.Instance.YellowLog("NodeType:"+NodeType+" - "+m_kPlayer.PlayerBaseInfo.HeroID+" OnAfterGetBall");
                OnAnimeFinish();
                m_kPlayer.ResetDelayedState();
                return BTResult.Success;
            }
            return BTResult.Running;
        }

//        private void Move(double fTime)
//        {
//            // move with speed : defendingVelocity
//            Vector3D _v = defendingDir * m_kPlayer.BaseVelocity * defendingVelocityRate * fTime+m_kPlayer.GetPosition();
//            m_kPlayer.SetPosition(_v);
//
//            m_kPlayer.MoveToPos(,fTime);
//        }
        
        protected override void Exit()
        {
            m_kPlayer = null;
            eState = EState.None;
        }
    }
}
