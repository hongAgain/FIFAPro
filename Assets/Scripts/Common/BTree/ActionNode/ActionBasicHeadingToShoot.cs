using Common;
using Common.Tables;
namespace BehaviourTree
{
    public class ActionBasicHeadingToShoot : ActionBasicGoCatchBall 
    {        
        private Vector3D m_kTargetPos = new Vector3D ();

        public ActionBasicHeadingToShoot()
        {
            Name = "BasicHeadingToShoot";
            DisplayName = "行为:接高球转射门基类";
            NodeType = "ActionBasicHeadingToShoot";
        } 
                
        protected override void InitializeAniParams()
        {
            m_kPlayer.KAniData.targetPos = m_kPlayer.Team.Scene.Ball.TargetGroundPos;
            m_kPlayer.KAniData.targetBallOutPos = m_kPlayer.Team.Opponent.Goal.GoalPos;
            m_kPlayer.KAniData.ballFlyingTime = m_kPlayer.Team.Scene.Ball.FlyingTime;
            m_kPlayer.KAniData.playerSpeed = m_kPlayer.Velocity;
            m_kPlayer.KAniData.headRobAvil = true;
        }
        
        protected override void OnBallOut()
        {
            LLTeam kOPTeam = m_kPlayer.Team.Opponent;
            kOPTeam.ShootData.IsHeadShoot = true;
            m_kPlayer.CastSkill(EEventType.ET_HeadShoot);
            // 计算射门点
            Vector3D kGoalPos;
            bool bRotAngle;
            double dValPr = FIFARandom.GetRandomValue(0, 1);
            
            NSShoot m_kNumericSettler = m_kPlayer.Team.NS_Shoot;
            m_kNumericSettler.Caculate(m_kPlayer);
            bool bNumericSettlerResult = (dValPr <= m_kNumericSettler.ShootInsidePr && m_kNumericSettler.Valid == true);
            bool bRetVal = kOPTeam.Goal.GetShootPoint(m_kPlayer.GetPosition(), m_kPlayer.GetRotAngle(), bNumericSettlerResult, out kGoalPos, out bRotAngle, true);
            if (bRetVal == true)
            {
                kOPTeam.ShootData.InSideGoal = true;
                OnShootInSide(kGoalPos, bRotAngle);
            }
            else
            {
                kOPTeam.ShootData.ShootSuccessPr = m_kNumericSettler.ShootSuccessPr;
                kOPTeam.ShootData.InSideGoal = false;
                OnShootOutSide(kGoalPos, bRotAngle);
            }
            
            
            
            LLTeam kTeam = m_kPlayer.Team;
            LLTeam kOpponentTeam = m_kPlayer.Team.Opponent;
            kTeam.Scene.Ball.SetTarget(m_kTargetPos, EBallMoveType.Shooting);
            kOpponentTeam.GoalKeeper.GKState = EGKState.GS_SAVE;
            m_kPlayer.SetBallCtrl(false);
            kTeam.Scene.SetState(EGameState.GS_SHOOT);
        }
        
        
        
        
        protected void OnShootOutSide(Vector3D kGoalPos, bool bRotAngle)
        {
            LLTeam kOPTeam = m_kPlayer.Team.Opponent;
            kOPTeam.ShootData.GoalPos = kGoalPos;
            kOPTeam.ShootData.Player = m_kPlayer;
            m_kTargetPos = kGoalPos;
            
            double goalDepth = TableManager.Instance.BattleInfoTable.GetItem("GoalDepth").Value;
            Vector3D targetPosFromPlayerNormalized = (m_kTargetPos - m_kPlayer.GetPosition()).normalized;
            m_kTargetPos = m_kTargetPos + targetPosFromPlayerNormalized * (goalDepth / System.Math.Abs(targetPosFromPlayerNormalized.Z));
            
            BattleCommentMgr.Instance.PostMessage(EBattleCommentType.BCT_HeadGoal, m_kPlayer);
        }
        
        // 射正
        protected void OnShootInSide(Vector3D kGoalPos,bool bRotAngle)
        {
            LLTeam kOPTeam = m_kPlayer.Team.Opponent;
            kOPTeam.ShootData.GoalPos = kGoalPos;
            kOPTeam.ShootData.Player = m_kPlayer;
            m_kTargetPos = kGoalPos;
            
            double goalDepth = TableManager.Instance.BattleInfoTable.GetItem("GoalDepth").Value;
            Vector3D targetPosFromPlayerNormalized = (m_kTargetPos - m_kPlayer.GetPosition()).normalized;
            m_kTargetPos = m_kTargetPos + targetPosFromPlayerNormalized * (goalDepth / System.Math.Abs(targetPosFromPlayerNormalized.Z));
            
            BattleCommentMgr.Instance.PostMessage(EBattleCommentType.BCT_HeadGoal, m_kPlayer);
        }
    }
}
