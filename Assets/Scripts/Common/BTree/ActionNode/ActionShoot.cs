using Common;
using Common.Log;
using Common.Tables;
using System;
using System.Collections.Generic;

namespace BehaviourTree
{
    public class ActionShoot : BTAction
    {
        protected enum EState
        {
            Shoot = 0,
            WaitAniBallIn,
            WaitAniBallOut,
        }
        public ActionShoot()
        {
            Name = "Shoot";
            DisplayName = "行为:射门";
            NodeType = "ActionShoot";
        }

        public override void Activate(BTDatabase kDatabase)
        {
            base.Activate(kDatabase);
            m_dLongDist = TableManager.Instance.AIConfig.GetItem("long_distance").Value;
        }

        protected override void Enter()
        {
            if (null == m_kPlayer)
            {
                int iID = m_kDatabase.GetDataID(BTConstant.Player);
                m_kPlayer = m_kDatabase.GetData<LLPlayer>(iID);
            }
            m_kState = EState.Shoot;
            m_dElapseTime = 0;
            m_bStartSkill = false;
            m_bFarShoot = false;
        }

        /// <summary>
        /// Called every frame if the action node is active.
        /// </summary>
        protected override BTResult Execute(double dTime)
        {
            switch (m_kState)
            {
                case EState.Shoot:
                    return OnShootState();
                case EState.WaitAniBallIn:
                    return OnWaitBallIn(dTime);
                case EState.WaitAniBallOut:
                    return OnWaitBallOut(dTime);
                default:
                    break;
            }
            return BTResult.Failed;
        }

        protected override void Exit()
        {
            m_kPlayer = null;
        }

        protected BTResult OnShootState()
        {
            LLTeam kTeam = m_kPlayer.Team;
            LLTeam kOPTeam = kTeam.Opponent;
            kTeam.ShootData.Valid = true;
            kOPTeam.ShootData.Valid = false;
            kOPTeam.ShootData.IsHeadShoot = false;
            kTeam.ShootData.Player = m_kPlayer;
            // 判断是否是远射
            double dDist = m_kPlayer.GetPosition().Distance(kOPTeam.Goal.GoalPos);
            if (dDist > m_dLongDist)
            {
                kOPTeam.ShootData.FarShoot = true;
                m_kPlayer.CastSkill(EEventType.ET_FarShoot);
            }
            else
            {
                kOPTeam.ShootData.FarShoot = false;
                m_kPlayer.CastSkill(EEventType.ET_Shoot);
            }
            // 计算射门点
            Vector3D kGoalPos;
            bool bRotAngle;
			m_kNumericSettler.Caculate(m_kPlayer);
            double dValPr = 0;
            if (m_kNumericSettler.Valid)
                dValPr = m_kNumericSettler.ShootRandVal;
            else
                dValPr = FIFARandom.GetRandomValue(0, 1);
            bool bNumericSettlerResult = (m_kNumericSettler.Valid && dValPr <= m_kNumericSettler.ShootInsidePr);
            bool bRetVal = kOPTeam.Goal.GetShootPoint(m_kPlayer.GetPosition(), m_kPlayer.GetRotAngle(), bNumericSettlerResult, out kGoalPos, out bRotAngle);
            if (bRetVal == true)
            {
                LLDirector.Instance.WaitForFirstHalfEnd = true;
                kOPTeam.ShootData.ShootSuccessPr = m_kNumericSettler.ShootSuccessPr;
                kOPTeam.ShootData.InSideGoal = true;
                return OnShootInSide(kGoalPos, bRotAngle);
            }
            else
            {
                kOPTeam.ShootData.ShootSuccessPr = m_kNumericSettler.ShootSuccessPr;
                kOPTeam.ShootData.InSideGoal = false;
                return OnShootOutSide(kGoalPos, bRotAngle);
            }
        }

        protected BTResult OnWaitBallIn(double dTime)
        {
            if (false == m_kPlayer.IsBallIn)
                return BTResult.Running;
            m_kState = EState.WaitAniBallOut;
            return BTResult.Running;
        }

        protected BTResult OnWaitBallOut(double dTime)
        {
            if (false == m_kPlayer.IsBallOut)
                return BTResult.Running;
            LLTeam kTeam = m_kPlayer.Team;
            LLTeam kOpponentTeam = m_kPlayer.Team.Opponent;
            kTeam.Scene.Ball.SetTarget(m_kTargetPos, EBallMoveType.Shooting);
            kOpponentTeam.GoalKeeper.GKState = EGKState.GS_SAVE;
            m_kPlayer.SetBallCtrl(false);
            kTeam.Scene.SetState(EGameState.GS_SHOOT);
            return BTResult.Success;
        }
        
        protected BTResult OnShootOutSide(Vector3D kGoalPos, bool bRotAngle)
        {
            LLTeam kOPTeam = m_kPlayer.Team.Opponent;
            kOPTeam.ShootData.GoalPos = kGoalPos;
            kOPTeam.ShootData.Player = m_kPlayer;
            m_kState = EState.WaitAniBallIn;
            m_kTargetPos = kGoalPos;

            double goalDepth = TableManager.Instance.BattleInfoTable.GetItem("GoalDepth").Value;
            Vector3D targetPosFromPlayerNormalized = (m_kTargetPos - m_kPlayer.GetPosition()).normalized;
            m_kTargetPos = m_kTargetPos + targetPosFromPlayerNormalized * (goalDepth / Math.Abs(targetPosFromPlayerNormalized.Z));

            m_kPlayer.KAniData.playerSpeed = m_kPlayer.Velocity;
            m_kPlayer.KAniData.shootRotateFlag = bRotAngle;
            m_kPlayer.KAniData.targetPos = kGoalPos;
            if (kOPTeam.ShootData.FarShoot)
                m_kPlayer.SetAniState(EAniState.Shoot_Far);
            else
                m_kPlayer.SetAniState(EAniState.Shoot_Near);
            BattleCommentMgr.Instance.PostMessage(EBattleCommentType.BCT_Shoot_Outside, m_kPlayer, kOPTeam.ShootData.FarShoot);
            return BTResult.Running;
        }
        // 射正
        protected BTResult OnShootInSide(Vector3D kGoalPos,bool bRotAngle)
        {
            LLTeam kOPTeam = m_kPlayer.Team.Opponent;
            kOPTeam.ShootData.GoalPos = kGoalPos;
            kOPTeam.ShootData.Player = m_kPlayer;
            m_kState = EState.WaitAniBallIn;
            m_kTargetPos = kGoalPos;

            double goalDepth = TableManager.Instance.BattleInfoTable.GetItem("GoalDepth").Value;
            Vector3D targetPosFromPlayerNormalized = (m_kTargetPos - m_kPlayer.GetPosition()).normalized;
            m_kTargetPos = m_kTargetPos + targetPosFromPlayerNormalized * (goalDepth / Math.Abs(targetPosFromPlayerNormalized.Z));

            m_kPlayer.KAniData.playerSpeed = m_kPlayer.Velocity;
            m_kPlayer.KAniData.shootRotateFlag = bRotAngle;
            m_kPlayer.KAniData.targetPos = kGoalPos;
            if (kOPTeam.ShootData.FarShoot)
                m_kPlayer.SetAniState(EAniState.Shoot_Far);
            else
                m_kPlayer.SetAniState(EAniState.Shoot_Near);
            BattleCommentMgr.Instance.PostMessage(EBattleCommentType.BCT_Shoot_Inside, m_kPlayer, kOPTeam.ShootData.FarShoot);
            return BTResult.Running;
        }
        
        private LLPlayer m_kPlayer = null;
        protected EState m_kState;
        protected double m_dLongDist = 0;
        private Vector3D m_kTargetPos;
        private List<Vector3D> m_kShootPointList = new List<Vector3D>();
        private double m_dElapseTime = 0;
        private bool m_bStartSkill = false;
        private bool m_bFarShoot = false;
        private NSShoot m_kNumericSettler = new NSShoot();
    }
}