//using Common;
//namespace BehaviourTree
//{
//	/// <summary>
//	/// deprecated
//	/// </summary>
//    public class ActionCatchGroundBallToIdle : ActionBasicAfterCatchBall {
//        
//        public ActionCatchGroundBallToIdle()
//        {
//            Name = "CatchGroundBallToIdle";
//            DisplayName = "行为:接地面球并停球";
//            NodeType = "ActionCatchGroundBallToIdle";
//        } 
//        
//        public override void Activate(BTDatabase kDatabase)
//        {
//            base.Activate(kDatabase);
//            enteringState = EPlayerState.Catch_GroundBall_ToIdle;
//            eAS_CatchingAnime = EAniState.Catch_GroundBall;
////            catchingVelocityRate = 0d;
//            hasBallOutFlag = false;
//        }
//
//        protected override void OnAfterCatch(double fTime)
//        {
//            m_kPlayer.SetState(EPlayerState.ActionSelect);
//        }
//
//        protected override void PlayAnime()
//        {
//            m_kPlayer.KAniData.targetPos = m_kPlayer.Team.Scene.Ball.TargetPos;
//            m_kPlayer.SetAniState(eAS_CatchingAnime);
//        }
//    }
//}
