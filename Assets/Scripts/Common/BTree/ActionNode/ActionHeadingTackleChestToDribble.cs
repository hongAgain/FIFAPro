using Common;
using Common.Tables;
namespace BehaviourTree
{
    public class ActionHeadingTackleChestToDribble : ActionBasicGoCatchBall
    {
        public ActionHeadingTackleChestToDribble()
        {
            Name = "HeadingTackleChestToDribble";
            DisplayName = "行为:防守方高空争顶成功，胸部停球并转移到脚下";
            NodeType = "ActionHeadingTackleChestToDribble";
        }
        
        protected override void InitializeParams()
        {
            runSpeedRate = TableManager.Instance.AIConfig.GetItem("speed_rate_run").Value;
            catchAnime = EAniState.HeadRob2_Stop;
            enteringState = EPlayerState.Heading_Tackle_ChestToDribble;
            leavingState = EPlayerState.NormalDribble;
            
            hasBallIn = true;
            hasBallOut = false;
            clearDelayTypeOnFinish = true;
            delayType = ETeamStateChangeDelayedType.DELAYED_DEFENCE;
        }
        
        protected override void InitializeAniParams()
        {
            m_kPlayer.KAniData.targetPos = m_kPlayer.Team.Scene.Ball.TargetGroundPos;
            m_kPlayer.KAniData.ballFlyingTime = m_kPlayer.Team.Scene.Ball.FlyingTime;
            m_kPlayer.KAniData.playerSpeed = m_kPlayer.Velocity;
        }
    }
}