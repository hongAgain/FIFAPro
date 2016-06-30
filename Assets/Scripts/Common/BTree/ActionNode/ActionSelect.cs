using Common;
using System;
using Common.Tables;
using Common.Log;

namespace BehaviourTree
{
    public class ActionSelect : BTAction
    {
        private double longDistancePass = 18d;
        public ActionSelect()
        {
            Name = "ActionSelect";
            DisplayName = "行为:行为选择";
            NodeType = "ActionSelect";
        }

        public override void Activate(BTDatabase kDatabase)
        {
            base.Activate(kDatabase);
            longDistancePass = TableManager.Instance.AIConfig.GetItem("long_distance_pass").Value;
        }

        protected override void Enter()
        {
            int iID = m_kDatabase.GetDataID(BTConstant.Player);
            m_kPlayer = m_kDatabase.GetData<LLPlayer>(iID);
            m_kScene = m_kPlayer.Team.Scene;
        }

        /// <summary>
        /// Called every frame if the action node is active.
        /// </summary>
        protected override BTResult Execute(double fTime)
        {
            if (EPlayerState.ActionSelect != m_kPlayer.State)
                return BTResult.Failed;

            switch(m_kScene.GameState)
            {
                case EGameState.GS_FIX_PASS:
                    m_kPlayer.SetState(EPlayerState.PassBall);  // 对人传球
                    return BTResult.Success;
                default:         
                    break;
            }
            BehaviourSelect();// 行为选择
            return BTResult.Success;
        }

        protected void BehaviourSelect()
        {
            int iRegionID = m_kPlayer.RegionID;
            if (0 == iRegionID)//球员没找到所属区域
            {
                m_kPlayer.SetState(EPlayerState.NormalDribble);  // 带球
                LogManager.Instance.RedLog("Error: Player is out of pitch");
                return;
            }

            if (m_kPlayer.Team.Scene.GameState == EGameState.GS_FIX_PASS)
            {
                PassBallFunc();
                LogManager.Instance.ColorLog("#ff8000ff","EGameState.GS_FIX_PASS");
            }
            else
            {
                EventAfterStoppingItem kItem = TableManager.Instance.EvtAfterStopTable.GetItem(iRegionID);
                int iDribblePr = kItem.DribblePr;       // 带球概率
                int iPassPr = kItem.PassPr;             // 传球概率
                int iShootPr = kItem.ShootPr;           // 射门概率
                                
                double randomResult = FIFARandom.GetRandomValue(0,100);
                if (randomResult <= iDribblePr)
                {
                    DribbleFunc();
                    LogManager.Instance.ColorLog("#ff8000ff","Region : "+m_kPlayer.RegionID+" | Dribble randomResult: "+randomResult+" -> iDribblePr: 0-"+iDribblePr+" | iPassPr: "+iDribblePr+"-"+(iDribblePr+iPassPr)+" | iShootPr: "+(iDribblePr+iPassPr)+"-"+(iDribblePr+iPassPr+iShootPr));
                } 
                else if (randomResult <= iDribblePr + iPassPr)
                {
                    m_kPlayer.Team.PlanToPass(m_kPlayer,false);
                    LogManager.Instance.ColorLog("#ff8000ff","Region : "+m_kPlayer.RegionID+" | Pass randomResult: "+randomResult+" -> iDribblePr: 0-"+iDribblePr+" | iPassPr: "+iDribblePr+"-"+(iDribblePr+iPassPr)+" | iShootPr: "+(iDribblePr+iPassPr)+"-"+(iDribblePr+iPassPr+iShootPr));
                } 
                else// if (randomResult < iDribblePr + iPassPr + iShootPr)
                {
                    ShootFunc();
                    LogManager.Instance.ColorLog("#ff8000ff","Region : "+m_kPlayer.RegionID+" | Shoot randomResult: "+randomResult+" -> iDribblePr: 0-"+iDribblePr+" | iPassPr: "+iDribblePr+"-"+(iDribblePr+iPassPr)+" | iShootPr: "+(iDribblePr+iPassPr)+"-"+(iDribblePr+iPassPr+iShootPr));
                } 
            }
        }

        private void PassBallFunc()
        {
            if (null == m_kPlayer)
            {
                return;
            }
            m_kPlayer.SetState(EPlayerState.PassBall);
        }

        private void DribbleFunc()
        {
            if (null == m_kPlayer)
            {
                return;
            }
            m_kPlayer.SetState(EPlayerState.NormalDribble);
        }

        private void ShootFunc()
        {
            if (null == m_kPlayer)
            {
                return;
            }
            m_kPlayer.SetState(EPlayerState.Shoot);
        }

        /// <summary>
        /// Called when the action node finishes.
        /// </summary>
        protected override void Exit()
        {
            m_kPlayer = null;
        }

        private LLPlayer m_kPlayer = null;
        private LLScene m_kScene = null;
    }
}