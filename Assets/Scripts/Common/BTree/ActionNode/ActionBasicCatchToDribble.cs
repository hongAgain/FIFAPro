using Common;
using Common.Tables;
namespace BehaviourTree
{
    public class ActionBasicCatchToDribble : ActionBasicGoCatchBall 
    {
        private Vector3D dribbleFinishPos = new Vector3D ();

        public ActionBasicCatchToDribble()
        {
            Name = "BasicCatchToDribble";
            DisplayName = "行为:接高球转为带球基类";
            NodeType = "ActionBasicCatchToDribble";
        }         

        protected override void InitializePlayer()
        {
            base.InitializePlayer();
//            m_kPlayer.NextPossibleDribbleDir = null;
//            dribbleFinishPos = m_kPlayer.Position + m_kPlayer.Team.SelectDribbleDir(m_kPlayer.NextPossibleDribbleDir) * FIFARandom.GetRandomValue(2,3);
        }
        
        protected override void InitializeAniParams()
        {
            m_kPlayer.KAniData.targetPos = m_kPlayer.Team.Scene.Ball.TargetGroundPos;
            m_kPlayer.KAniData.ballFlyingTime = m_kPlayer.Team.Scene.Ball.FlyingTime;
            m_kPlayer.KAniData.playerSpeed = m_kPlayer.Velocity;
        }
        
//        protected override bool IsAniFinished()
//        {            
//            return m_kPlayer.Position.Distance(dribbleFinishPos) < 0.5d;
//        }
    }
}