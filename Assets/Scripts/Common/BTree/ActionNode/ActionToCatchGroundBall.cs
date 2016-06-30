using Common;
using Common.Tables;
namespace BehaviourTree
{
    public class ActionToCatchGroundBall : ActionBasicGoCatchBall
    {
        public ActionToCatchGroundBall()
        {
            Name = "ToCatchGroundBall";
            DisplayName = "行为:准备接地球";
            NodeType = "ActionToCatchGroundBall";
        }

        protected override void InitializeParams()
        {
            runSpeedRate = TableManager.Instance.AIConfig.GetItem("speed_rate_run").Value;
            catchAnime = EAniState.Catch_GroundBall;
            enteringState = EPlayerState.Catch_GroundBall;
            leavingState = EPlayerState.PS_END;
            
            hasBallIn = true;
			hasBallOut = false;
			clearDelayTypeOnFinish = true;
            delayType = ETeamStateChangeDelayedType.NONE;
        }

        protected override void InitializeAniParams()
        {
            m_kPlayer.KAniData.targetPos = m_kPlayer.Team.Scene.Ball.TargetGroundPos;
            m_kPlayer.KAniData.ballFlyingTime = m_kPlayer.Team.Scene.Ball.FlyingTime;
            m_kPlayer.KAniData.playerSpeed = m_kPlayer.Velocity;
        }

        protected override void OnAniFinish()
        {
            //cosider next : dribble/pass/shoot
            m_kPlayer.Team.CheckAfterCatchGroundBall(m_kPlayer);
            
        }
    }
}