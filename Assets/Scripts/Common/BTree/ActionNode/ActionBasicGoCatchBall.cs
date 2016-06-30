using Common;
using Common.Tables;

namespace BehaviourTree
{
    /// <summary>
    /// Action basic go catch ball. TargetPos is set by LLTeam.PassBall();
    /// </summary>
    public class ActionBasicGoCatchBall : BTAction
    {
        public ActionBasicGoCatchBall()
        {
            Name = "BasicGoCatchBall";
            DisplayName = "行为:成功接球基类";
            NodeType = "ActionBasicGoCatchBall";
        } 

        private enum InternalState
        {
            None,
            BeforeCatch,
            OnBallIn,
            Catching,
            OnBallOut,
            WaitForEnd
        }
        private InternalState iState = InternalState.None;

        protected LLPlayer m_kPlayer = null;    

        //params need initialize
        protected double runSpeedRate = 1d;
        protected EAniState catchAnime = EAniState.Catch_GroundBall;
        protected EPlayerState enteringState = EPlayerState.Catch_GroundBall;
        protected EPlayerState leavingState = EPlayerState.PS_END;        
        protected bool hasBallIn = true;
        protected bool hasBallOut = false;

		protected bool clearDelayTypeOnFinish = true;
        protected ETeamStateChangeDelayedType delayType = ETeamStateChangeDelayedType.NONE;
        
        public override void Activate(BTDatabase kDatabase)
        {
            base.Activate(kDatabase);
            InitializeParams();
        }
        
        protected override void Enter()
        {
            if (null == m_kPlayer)
            {
                int iID = m_kDatabase.GetDataID(BTConstant.Player);
                m_kPlayer = m_kDatabase.GetData<LLPlayer>(iID);
            }
            if (m_kPlayer.State == enteringState)
            {
                InitializePlayer();
                InitializeAniParams();
                m_kPlayer.SetAniState(catchAnime);

                if(!hasBallIn)
                {
                    iState = InternalState.WaitForEnd;
                }
                else
                {
                    iState = InternalState.BeforeCatch;
                }
            }
        }
        
        protected override BTResult Execute(double dTime)
        {
            if( m_kPlayer == null || m_kPlayer.State != enteringState )
                return BTResult.Failed;
            switch(iState)
            {
                case InternalState.BeforeCatch:
                    if(m_kPlayer.IsBallIn)
                    {
                        iState = InternalState.OnBallIn;
                    }
                    break;
                case InternalState.OnBallIn:                    
                    m_kPlayer.Team.BallController = m_kPlayer;
                    if (delayType != ETeamStateChangeDelayedType.NONE)
                    {
                        // get ctrl of ball
//                        m_kPlayer.Opponent.SetBallCtrl(false);
                        m_kPlayer.SetBallCtrl(true);
                        
                        //change on next frame
                        m_kPlayer.Team.Scene.RequireTeamStateChange(ETeamStateChangeType.TSCT_INTERCEPT, true);
                    }
                    OnBallIn();
                    if(hasBallOut)
                    {
                        iState = InternalState.Catching;
                    }
                    else
                    {
                        iState = InternalState.WaitForEnd;
                    }
                    break;
                case InternalState.Catching:
                    if(m_kPlayer.IsBallOut)
                    {
                        iState = InternalState.OnBallOut;
                    }
                    break;
                case InternalState.OnBallOut:
                    OnBallOut();
                    iState = InternalState.WaitForEnd;
                    break;
                case InternalState.WaitForEnd:
                    if(IsAniFinished())
                    {
                        OnAniFinish();
                        if(leavingState != EPlayerState.PS_END)
                        {
                            m_kPlayer.SetState(leavingState);
                            if (delayType != ETeamStateChangeDelayedType.NONE)
                                m_kPlayer.NextState = leavingState;
                        }
						if (clearDelayTypeOnFinish) 
						{
                            m_kPlayer.ResetDelayedState();
						}
                        return BTResult.Success;
                    }
                    break;
                default:
                    return BTResult.Failed;
            }
            return BTResult.Running;
        }
        
        protected override void Exit()
		{
            m_kPlayer = null;
        }
        
        #region ====== virtual functions ======
        protected virtual void InitializeParams() {}
        protected virtual void InitializeAniParams() {}        
        protected virtual void InitializePlayer()
        {
            m_kPlayer.StateChangeDelayedType = delayType;
            m_kPlayer.Velocity = m_kPlayer.BaseVelocity * runSpeedRate;
            m_kPlayer.SetRoteAngle(MathUtil.GetAngle(m_kPlayer.GetPosition(), m_kPlayer.Team.Scene.Ball.TargetPos));
            //idle before catching
            m_kPlayer.SetAniState(EAniState.Idle);
        }

        protected virtual void OnBallIn()
        {            

        }

        protected virtual void OnBallOut()
        {
            //pass or shoot

        }

        protected virtual bool IsAniFinished()
        {
            return m_kPlayer.AniFinish;
        }

        protected virtual void OnAniFinish()
        {

        }
        #endregion
    }
}
