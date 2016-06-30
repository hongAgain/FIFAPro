using Common;
namespace BehaviourTree
{

    public class ActionGKKickOff : BTAction
    {
        protected enum EState
        {
            WaitAni = 0,
            Kick,
            WaitAniFinished,
        }
        private LLPlayer m_kPlayer = null;
        private LLGoalKeeper m_GK = null;

        private Vector3D originalTargetPos = new Vector3D ();

        public ActionGKKickOff()
        {
            Name = "GKKickOff";
            DisplayName = "行为:开球";
            NodeType = "ActionGKKickOff";
        }

        protected override void Enter()
        {
            int iID = m_kDatabase.GetDataID(BTConstant.Player);
            m_GK = m_kDatabase.GetData<LLGoalKeeper>(iID);
            m_GK.SetBallCtrl(false);
            FIFARandom.OnSelect onNear = delegate()
            {
                m_kPlayer = m_GK.Team.SelectBallController();
                if(null != m_kPlayer)
                {
                    m_kPlayer.SetBallCtrl(false);
                    originalTargetPos = m_kPlayer.GetPosition();
                    m_GK.KAniData.targetPos = m_kPlayer.GetPosition();
                    m_GK.SetAniState(EAniState.GK_KickBall);
                }
            };
            FIFARandom.OnSelect onFar = delegate()
            {
                m_kPlayer = m_GK.Team.SelectBallController();
                if (m_kPlayer != null)
                {
                    m_kPlayer.SetBallCtrl(false);
                    originalTargetPos = m_kPlayer.GetPosition();
                    m_GK.KAniData.targetPos = m_kPlayer.GetPosition();
                    m_GK.SetAniState(EAniState.GK_ThrowBall);
                }
            };

            var rate1 = 0.5f;//TableManager.Instance.AIConfig.GKThrowPL;
            var rate2 = 0.5f;//TableManager.Instance.AIConfig.GKBigKickPL;

            FIFARandom.Select(new double[] { rate1, rate2 }, new FIFARandom.OnSelect[] { onNear, onFar });
            m_kState = EState.WaitAni;
        }

        private BTResult OnWaitAniState()
        {
            if (m_GK.IsBallOut)
            {
           //     m_GK.Team.Scene.Ball.SetTarget(m_kPlayer.Position);
                m_kState = EState.Kick;
                m_dElapseTime = 0;
            }
            return BTResult.Running;
        }

        private BTResult OnKickState(double dTime)
        {
            //m_dElapseTime += dTime;
            //if (m_dElapseTime < 1)
            //    return BTResult.Running;
            if (m_kPlayer != null)
            {
//                m_kPlayer.SetBallCtrl(true);
                m_GK.Team.Scene.Ball.SetTarget(m_kPlayer.GetPosition());
                m_GK.Team.PassBall(m_GK, m_kPlayer, EBallMoveType.GroundPass, true);
                //m_kPlayer.SetState(EPlayerState.Catch_GroundBall);
                m_kPlayer.Team.Scene.SetState(EGameState.GS_NORMAL);

            }

            m_kState = EState.WaitAniFinished; 
            return BTResult.Running;
        }


        private BTResult OnWaitAniFinished(double dTime)
        {
            if (false == m_GK.AniFinish)
                return BTResult.Running;
            m_GK.GKState = EGKState.GS_DEFAULT;
            return BTResult.Success;

        }
        protected override BTResult Execute(double dTime)
        {
            switch(m_kState)
            {
                case EState.WaitAni:
                    return OnWaitAniState();
                case EState.Kick:
                    return OnKickState(dTime);
                case EState.WaitAniFinished:
                    return OnWaitAniFinished(dTime);
                default:
                    break;
            }
            return BTResult.Failed;
            //if (mDelay >= 0)
            //{
            //    mDelay -= fTime;
            //}

            //if (mDelay <= 0f && resetFlag == false)
            //{
            //   // m_GK.Team.Scene.Ball.Position = mTouchPos + m_GK.Position;
            //    m_GK.Team.Scene.Ball.SetTarget(m_kPlayer.Position);

            //    resetFlag = true;
            //    resetDelayT = 1d;
            //}

            //if (resetFlag == true)
            //{
            //    resetDelayT -= fTime;
            //    if (resetDelayT <= 0)
            //    {
            //        m_GK.GKState = EGKState.GS_DEFAULT;
            //        resetFlag = false;
            //        if (m_kPlayer != null)
            //        {
            //            m_kPlayer.SetBallCtrl(true);
            //            m_GK.Team.Scene.Ball.SetTarget(m_kPlayer.Position);
            //            m_kPlayer.SetState(EPlayerMainState.PS_WAITING_FOR_BALL_ARRIVED);
            //            m_kPlayer.Team.Scene.SetState(EGameState.TSS_NORMAL);
            //        }
            //        return BTResult.Success;
            //    }
            //}

            //return BTResult.Running;
        }

        protected override void Exit()
        {
            m_kPlayer = null;
            m_GK = null;
        }

        protected EState m_kState;
        private double m_dElapseTime = 0;
    }
}