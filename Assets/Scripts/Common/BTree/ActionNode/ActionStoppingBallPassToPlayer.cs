using Common;
using Common.Tables;
namespace BehaviourTree
{
    public class ActionStoppingBallPassToPlayer : BTAction
    {
        enum EState
        {
            Normal = 0,
            PassBall,
            WaitForBallHit,
            AfterBallHit,
            WaitForFinish,
            End
        }
        public ActionStoppingBallPassToPlayer()
        {
            Name = "StoppingBallPassToPlayer";
            DisplayName = "行为:停球传人";
            NodeType = "ActionStoppingBallPassToPlayer";
        }

        protected override void Enter()
        {
            if (null == m_kPlayer)
            {
                int iID = m_kDatabase.GetDataID(BTConstant.Player);
                m_kPlayer = m_kDatabase.GetData<LLPlayer>(iID);
            }
            if (null != m_kPlayer && EPlayerState.PassBall == m_kPlayer.State)
            {
                m_kTeam = m_kPlayer.Team;
                m_kBall = m_kTeam.Scene.Ball;
                m_kState = EState.Normal;
            }
        }

        /// <summary>
        /// Called every frame if the action node is active.
        /// </summary>
        protected override BTResult Execute(double fTime)
        {
            if (m_kPlayer == null)
                return BTResult.Failed;
            if (EPlayerState.PassBall != m_kPlayer.State )
                return BTResult.Failed;
            switch(m_kState)
            {
                case EState.Normal:
                    return NormalState();
                case EState.PassBall:
                    return PassBallState();
                case EState.WaitForBallHit:
                    return WaitBallHitState();
                case EState.AfterBallHit:
                    return AfterBallHitState();
                case EState.WaitForFinish:
                    return WaitForFinish();
                default:
                    break;
            }
            
            return BTResult.Failed;
        }

        protected BTResult NormalState()
        {
            // 球队选择可以接球的球员
            m_kSelectedPlayer = m_kTeam.SelectBallController();
            if (null != m_kSelectedPlayer)
            {
                if (m_kSelectedPlayer == m_kTeam.BallController || m_kSelectedPlayer == m_kPlayer)
                {
                    m_kPlayer.SetState(EPlayerState.NormalDribble);
//                    m_kSelectedPlayer = null;
                    return BTResult.Success;
                }
                else
                {
                    m_OriginalTargetPos = m_kSelectedPlayer.GetPosition();
                    m_kBall.CanMove = false;
                    m_kState = EState.PassBall;

                    if (m_kPlayer.GetPosition().Distance(m_kSelectedPlayer.GetPosition()) >= TableManager.Instance.AIConfig.GetItem("long_distance_pass").Value)
                    {
                        moveType = EBallMoveType.HighLobPass;
//                        //turn first for high lob pass
//                        m_kPlayer.RotateAngle = MathUtil.GetAngle(m_kPlayer.Position,m_kSelectedPlayer.Position);
                    }
                    else
                    {
                        moveType = EBallMoveType.GroundPass;
                    }

                    return BTResult.Running;
                }
            }
            return BTResult.Failed;
        }

        protected BTResult PassBallState()
        {
            m_kPlayer.KAniData.targetPos = m_kSelectedPlayer.GetPosition();

            if(moveType == EBallMoveType.HighLobPass)
            {
                m_kPlayer.SetAniState(EAniState.PassBall_High);
            }
            else
            {
                m_kPlayer.SetAniState(EAniState.PassBall_Floor);
            }
            m_kState = EState.WaitForBallHit;
            return BTResult.Running;
        }

        protected BTResult WaitBallHitState()
        {
            if (m_kPlayer.IsBallOut)
                m_kState = EState.AfterBallHit;
            return BTResult.Running;
        }

        protected BTResult AfterBallHitState()
        {
            Common.Log.LogManager.Instance.RedLog("PassBall BallOut");
            if(m_OriginalTargetPos == null)
                m_OriginalTargetPos = m_kSelectedPlayer.GetPosition();
            m_kPlayer.Team.PassBall(m_kPlayer,m_kSelectedPlayer,moveType,true);
            m_kState = EState.WaitForFinish;
            return BTResult.Running;
        }

        protected BTResult WaitForFinish()
        {
            if (m_kPlayer.AniFinish)
            {
                Common.Log.LogManager.Instance.RedLog("PassBall AniFinish");
                m_kPlayer.SetState(EPlayerState.PassBallIdle);
                return BTResult.Success;
            }
            return BTResult.Running;
        }

        protected override void Exit()
        {
            m_kPlayer = null;
            m_kTeam = null;
            m_kSelectedPlayer = null;
        }
        private LLPlayer m_kPlayer = null;
        private LLTeam m_kTeam = null;
        private Vector3D m_OriginalTargetPos = new Vector3D ();
        private EBallMoveType moveType = EBallMoveType.GroundPass;
        private LLPlayer m_kSelectedPlayer = null;
        private LLBall m_kBall = null;
        private EState m_kState=EState.Normal;
    }
}