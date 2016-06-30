
using Common;
using Common.Tables;
using System.Collections.Generic;

namespace BehaviourTree
{
    public class ActionGKSave : BTAction
    {
        enum EShootResult
        {
            Outside = 1,        // 射偏
            Inside              // 射正
        }

        enum EGKSaveState
        {
            // 射正相关动作
            CalcCanSave= 1,     // 数值对抗，表示是否可以扑中
            SaveFailed,         // 扑救失败
            SaveSuccess,        // 扑救成功
            AfterSuccess,       // 扑球成功后的延时
            AfterFailed,        // 门将扑球失败后的延时
            // 射偏相关动作
            ShootOutside,
            SaveShootOutside,      // 射偏后的动作
            AfterShootOutside
        }

        public ActionGKSave()
        {
            Name = "GKSave";
            DisplayName = "行为:跟随球移动";
            NodeType = "ActionGKSave";
        }

        public override void Activate(BTDatabase kDatabase)
        {
            base.Activate(kDatabase);
//            m_dNearSpeed = TableManager.Instance.AIConfig.GetItem("ShortShoot_speed").Value;
//            m_dFarSpeed = TableManager.Instance.AIConfig.GetItem("LongShoot_speed").Value;
        }
        protected override void Enter()
        {
            int iID = m_kDatabase.GetDataID(BTConstant.Player);
            m_kPlayer = m_kDatabase.GetData<LLGoalKeeper>(iID);
            m_kBall = m_kPlayer.Team.Scene.Ball;

            if (m_kPlayer.Team.ShootData.InSideGoal)
            {
                m_kShootResult = EShootResult.Inside;
                m_kGKSaveState = EGKSaveState.CalcCanSave;
            }
            else
            {
                m_kShootResult = EShootResult.Outside;
                m_kGKSaveState = EGKSaveState.ShootOutside;
            }
            m_kPlayer.CastSkill(EEventType.ET_GKSave);
        }

        
        protected override BTResult Execute(double dTime)
        {

            switch(m_kGKSaveState)
            {
                case EGKSaveState.CalcCanSave:
                    OnCalcCanSave();
                    break;
                case EGKSaveState.SaveFailed:
                    if (m_kPlayer.IsBallIn)
                    {
                        m_kGKSaveState = EGKSaveState.AfterFailed;
                        m_dTime = 0; 
                    }
                    break;
                case EGKSaveState.SaveSuccess:
                    if (m_kPlayer.IsBallIn)
                    {
                        m_dTime = 0; 
                        m_kGKSaveState = EGKSaveState.AfterSuccess;
						//m_kPlayer.Team.Opponent.ShootData.Player.SetAniState();

					    EAniState shootPlayerState;
					    if (m_kPlayer.Team.Opponent.ShootData.FarShoot == true) {
						    shootPlayerState = EAniState.Shoot_Far_Failed;
					    } else {
						    shootPlayerState = EAniState.Shoot_Near_Failed;
						}
					    if (m_kPlayer.Team.ShootData.IsHeadShoot == true) {
						    shootPlayerState = EAniState.HeadRob_Shoot_Failed;
					    }
					    m_kPlayer.Team.ShootData.Player.SetAniState(shootPlayerState);
                    }
                    break;
                case EGKSaveState.AfterFailed:
                    m_dTime += dTime;
                    if (m_dTime >= 2)
                    {
                        m_kPlayer.GKState = EGKState.GS_DEFAULT;
                        m_kPlayer.Team.Scene.RequireTeamStateChange(ETeamStateChangeType.TSCT_SHOOT, true);
                        m_kPlayer.Team.Scene.SetState(EGameState.GS_CELEBRATION);
                        return BTResult.Success;
                    }
                    break;
                case EGKSaveState.AfterSuccess:
                    m_dTime += dTime;
                    if (m_dTime >= 2)
                    {
                        m_kPlayer.Team.Scene.RequireTeamStateChange(ETeamStateChangeType.TSCT_SHOOT, true);
                        m_kPlayer.GKState = EGKState.GS_KICKOFF;
                        return BTResult.Success;
                    }
                    break;
                case EGKSaveState.ShootOutside:
                    // 设置ani
                    OnCalcShootOut();
                    break;
                case EGKSaveState.SaveShootOutside:
                    if (m_kPlayer.IsBallIn)
                    {
                        m_kGKSaveState = EGKSaveState.AfterShootOutside;
                        m_dTime = 0;
                    }
                    break;
                case EGKSaveState.AfterShootOutside:
                    m_dTime += dTime;
                    if (m_dTime >= 2)
                    {
                        m_kPlayer.GKState = EGKState.GS_DEFAULT;
                        m_kPlayer.Team.Scene.RequireTeamStateChange(ETeamStateChangeType.TSCT_SHOOT, true);
                        m_kPlayer.Team.Scene.SetState(EGameState.GS_MIDKICK);
                        return BTResult.Success;
                    }
                    break;
                default:
                    break;
            }
            return BTResult.Running;
        }

        private void OnCalcShootOut()
        {
            m_kPlayer.KAniData.targetPos = m_kPlayer.Team.ShootData.GoalPos; 
            m_kPlayer.SetAniState(EAniState.GK_Save_Failed);// 扑救失败的动画状态
            m_kGKSaveState = EGKSaveState.SaveShootOutside;
        }

        // 计算是否可以扑中球
        private void OnCalcCanSave()
        {
            //Vector3D goalCenter = m_kPlayer.Team.Goal.ShootPos;
            //double d1 = (mBall.Position - goalCenter).Length();
            //double d2 = (mBall.Position - m_kPlayer.Position).Length();
            //double d3 = (goalCenter - mBall.TargetPos).Length();
            //double d4 = d3 * d2 / d1;
            //m_kPlayer.SavePoint = (mBall.TargetPos - goalCenter).Normalize() * d4 + m_kPlayer.Position;
            //m_kPlayer.SetBallCtrl(true);
            //m_kPlayer.SetAniState(EAniState.GK_HorizontalSaveSuccess, m_kPlayer.SavePoint);


            ShootData kData = m_kPlayer.Team.ShootData;
            if (kData.FarShoot) {
                m_kPlayer.KAniData.ballFlyingTime = kData.Player.GetPosition().Distance(kData.GoalPos) / m_kBall.Velocity;
			} else {
                m_kPlayer.KAniData.ballFlyingTime = kData.Player.GetPosition().Distance(kData.GoalPos) / m_kBall.Velocity;
			}
            m_kPlayer.KAniData.targetPos = kData.GoalPos;

            double dValPr = 0;
            if (kData.NSValid)
                dValPr = kData.GKSaveRandVal;
            else
                dValPr = FIFARandom.GetRandomValue(0, 1);
            bool _FLAG = dValPr < kData.ShootSuccessPr;
            if (_FLAG)
            {
                // 射门成功
                m_kPlayer.Team.Opponent.TeamInfo.Score += 1;
                kData.Player.PlayerBaseInfo.Score += 1;
                m_kPlayer.KAniData.targetPos = kData.GoalPos;
                m_kPlayer.SetAniState(EAniState.GK_Save_Failed);// 扑救失败的动画状态
                m_kGKSaveState = EGKSaveState.SaveFailed;
                BattleCommentMgr.Instance.PostMessage(EBattleCommentType.BCT_Shoot_Success, kData.Player);
				m_kBall.SetTarget(m_kBall.TargetPos + m_kBall.TargetPos.normalized * 2);
            }
            else
            {
                LLDirector.Instance.WaitForFirstHalfEnd = false;
                // 射门失败
                //  m_kPlayer.SetAniState()// 扑救失败的动画状态
                m_kPlayer.KAniData.targetPos = kData.GoalPos;
                m_kPlayer.SetAniState(EAniState.GK_Save_Success);// 扑救成功的动画状态
                m_kGKSaveState = EGKSaveState.SaveSuccess;
                BattleCommentMgr.Instance.PostMessage(EBattleCommentType.BCT_Shoot_Failed, kData.Player);
            }

            //// 添加数值对抗
            //List<LLUnit> kUnitList = new List<LLUnit>();
            //kUnitList.Add(kData.Player);
            //DataValidManager.Instance.AddValidData(EActionType.ActionID_GKSave, m_kPlayer, kUnitList, true);
        }
        private EShootResult m_kShootResult = EShootResult.Outside;
        private LLGoalKeeper m_kPlayer = null;
        private LLBall m_kBall = null;
        private bool mAfterDelay = false;
        private double m_dTime = 0f;
        private EGKSaveState m_kGKSaveState = EGKSaveState.CalcCanSave;
        private double m_dFarSpeed = 0;
        private double m_dNearSpeed = 0;
    }
}