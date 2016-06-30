

using Common;
using BehaviourTree;
using System.Collections.Generic;

public enum EBattleCommentType
{
    BCT_Shoot_Success = 1,          // 射门成功   // 1,2,3,4,5,6
    BCT_Shoot_Failed,               // 射门失败
    BCT_Shoot_Inside,               // 射正       // 12,13
    BCT_Shoot_Outside,              // 射偏       // 17
    BCT_Dribble,                    // 带球       // 18,19,20,21,22
    BCT_Passball,                   // 传球       // 23,24
    BCT_CatchBall,                  // 球员接球   // 25
    BCT_HeadGoal,                   // 头球       // 14
    BCT_Goalkeeper,                 // 守门员扑球 // 15,16 
    BCT_Block,                      // 拦截       // 26
    BCT_Breakthrough,               // 突破       // 27,28
    BCT_Intercept,                  // 抢截       // 29,30
    BCT_Slide,                      // 抢断       // 31,32
    BCT_Head,                       // 争顶       // 33,34
    BCT_PlayerSP,                   // 球员体力     // 48
    BCT_PlayerSkill,                // 球员技能     // 49                   // No Verify


    BCT_Max,                        // 3秒内没有触发任何事件 //   35-47
}

public class BattleCommentMgr
{
    public static BattleCommentMgr Instance
    {
        get 
        { 
            if (m_instance == null)
            {
                m_instance = new BattleCommentMgr();
                m_instance.InitCommonEvent();
            }
            return m_instance;
        }
    }

    public void Reset()
    {
        if (m_instance != null)
        {
            m_instance = null;
        }
    }
    private static BattleCommentMgr m_instance;
    private  BattleCommentMgr() 
    {
    }

    private float m_lastEventTime;
    private string m_lastSubCommenID;
    public string LastSubCommentID
    {
        set { m_lastSubCommenID = value; }
    }
    private LLTeam m_reaTeam;
    private LLTeam m_blueTeam;
    private delegate bool CommonEventHander();
    private List<CommonEventHander> commonEventList = new List<CommonEventHander>();
    private void InitCommonEvent()
    {
        m_reaTeam = LLDirector.Instance.Scene.RedTeam;
        m_blueTeam = LLDirector.Instance.Scene.BlueTeam;

        commonEventList.Add(OnCtrRate);
        commonEventList.Add(OnGoalDiff);
        commonEventList.Add(OnEnter80Min);
        commonEventList.Add(OnNoGoal);
        commonEventList.Add(OnCurrDraw);
        commonEventList.Add(OnTotalGoal);
        commonEventList.Add(OnBattlePower);
    }
    // Public Comment API 
    public void PostMessage(EBattleCommentType kType,LLUnit kUnit, params object[] kParams)
    {
        switch(kType)
        {
            case EBattleCommentType.BCT_Shoot_Success:
                OnShootSuccessMsg(kUnit);
                break;
            case EBattleCommentType.BCT_Shoot_Failed:
                OnShootFailedMsg(kUnit);
                break;
            case EBattleCommentType.BCT_Shoot_Inside:
                OnShootInside(kUnit, kParams);
                break;
            case EBattleCommentType.BCT_Shoot_Outside:
                OnShootOutside(kUnit, kParams);
                break;
            case EBattleCommentType.BCT_Dribble:
                OnDribble(kUnit);
                break;
            case EBattleCommentType.BCT_Passball:
                OnPassball(kUnit, kParams);
                break;
            case EBattleCommentType.BCT_CatchBall:
                OnCatchBall(kUnit);
                break;
            case EBattleCommentType.BCT_HeadGoal:
                OnHeadGoal(kUnit);
                break;
            case EBattleCommentType.BCT_Goalkeeper:
                OnGoalkeeperEvent(kUnit);
                break;
            case EBattleCommentType.BCT_Block:
                OnBlock(kUnit);
                break;
            case EBattleCommentType.BCT_Breakthrough:
                OnBreakthrough(kUnit);
                break;
            case EBattleCommentType.BCT_Intercept:
                OnIntercept(kUnit);
                break;
            case EBattleCommentType.BCT_Slide:
                OnSlide(kUnit);
                break;
            case EBattleCommentType.BCT_Head:
                OnHeadGoal(kUnit);
                break;
            case EBattleCommentType.BCT_PlayerSP:
                OnPlayerSP(kUnit);
                break;
            case EBattleCommentType.BCT_PlayerSkill:
                OnPlayerSkill(kUnit);
                break;
            default:
                break;
        }
    }

    private bool OnPlayerSkill(LLUnit kUnit,params object[] kParams)
    {
        if (null == kUnit || null == kParams)
            return false;

        bool bFlag = (bool)(kParams[0]);

        if (true == bFlag) // 释放技能
        {
            SendPostMessage("skill_action", kUnit); // 49           
            return true;
        }

        return false;
    }
    private bool OnPlayerSP(LLUnit kUnit)
    {
        if (null == kUnit)
            return false;

        int playerId = 0;
        float minSP = 1;
        foreach(var p in kUnit.Team.BallMarkList)
        {
            float spRate = (float)p.PlayerBaseInfo.Energy / (float)p.PlayerBaseInfo.Attri.stamina;
            if (spRate < minSP && spRate < 0.6)
            {
                minSP = spRate;
                playerId = p.PlayerBaseInfo.PlayerID;
            }
        }

        if (playerId == kUnit.PlayerBaseInfo.PlayerID)
        {
            SendPostMessage("lowest_physical", kUnit); // 48           
            return true;
        }

        return false;
    }
    private bool OnBattlePower()
    {
        int meScore = m_reaTeam.TeamInfo.FightScore;
        int oppScore = m_blueTeam.TeamInfo.FightScore;

        if (meScore > oppScore)
        {
            SendPostMessage("highest_value", m_reaTeam.PlayerList[0] as LLUnit); // 46            
            return true;
        }
        else
        {
            SendPostMessage("lowest_value", m_reaTeam.PlayerList[0] as LLUnit); // 47           
            return true;
        }
    }
    private bool OnTotalGoal()
    {
        int totalScore = m_reaTeam.TeamInfo.Score + m_blueTeam.TeamInfo.Score;

        if (totalScore > 2)
        {
            SendPostMessage("total_goal_2", m_reaTeam.PlayerList[0] as LLUnit); // 43          
            return true;
        }
        else if (totalScore > 4)
        {
            SendPostMessage("total_goal_4", m_reaTeam.PlayerList[0] as LLUnit); // 44            
            return true;
        }
        else if (totalScore > 6)
        {
            SendPostMessage("total_goal_6", m_reaTeam.PlayerList[0] as LLUnit);// 45          
            return true;
        }

        return false;
    }
    private bool OnCurrDraw()
    {
        if (m_reaTeam.TeamInfo.Score == m_blueTeam.TeamInfo.Score)
        {
            SendPostMessage("draw", null); // 42         
            return true;
        }

        return false;
    }
    private bool OnNoGoal()
    {
        //if (true)
        //{
        //    BattleTextMessage kMsg = new BattleTextMessage("No_goal_30", kUnit); // 41
        //    MessageDispatcher.Instance.PostMessage(kMsg);
        //    return true;
        //}

        return false;
    }
    private bool OnEnter80Min()
    {
        int meScore = m_reaTeam.TeamInfo.Score;
        int oppScore = m_blueTeam.TeamInfo.Score;

        if (meScore < oppScore)
        {
            SendPostMessage("enter_80_minutes", m_reaTeam.PlayerList[0] as LLUnit); // 38           
            return true;
        }

        return false;
    }
    private bool OnGoalDiff()
    {
        int meScore = m_reaTeam.TeamInfo.Score;
        int oppScore = m_blueTeam.TeamInfo.Score;

        if (meScore - oppScore > 3)
        {
            SendPostMessage("goal_difference_3", m_reaTeam.PlayerList[0] as LLUnit); // 36          
            return true;
        }
        else if (meScore - oppScore > 5)
        {
            SendPostMessage("goal_difference_5", m_reaTeam.PlayerList[0] as LLUnit); // 37           
            return true;
        }

        return false;
    }
    private bool OnCtrRate()
    {
        float ctrRate = m_reaTeam.TeamInfo.CtrlBallTime / (m_reaTeam.TeamInfo.CtrlBallTime + m_blueTeam.TeamInfo.CtrlBallTime);

        if (ctrRate > 0.65f)
        {
            SendPostMessage("control_rate_65", m_reaTeam.PlayerList[0] as LLUnit); // 35         
            return true;
        }
        else if (ctrRate < 0.35f)
        {
            SendPostMessage("control_rate_35", m_reaTeam.PlayerList[0] as LLUnit); // 39           
            return true;
        }
        else
        {
            SendPostMessage("control_rate_65_35", m_reaTeam.PlayerList[0] as LLUnit); // 40           
            return true;
        }
    }
    private void OnHeadGoal(LLUnit kUnit, params object[] kParams)
    {
        if (null == kUnit || null == kParams)
            return;

        bool bFlag = (bool)(kParams[0]);

        if (true == bFlag) // 争顶成功
        {
            SendPostMessage("head_success", kUnit); // 33           
        }
        else
        {
            SendPostMessage("head_fail", kUnit); // 34           
        }
    }
    private void OnSlide(LLUnit kUnit, params object[] kParams)
    {
        if (null == kUnit || null == kParams)
            return;

        bool bFlag = (bool)(kParams[0]);

        if (true == bFlag) // 抢断成功
        {
            SendPostMessage("slide_success", kUnit); // 31           
        }
        else
        {
            SendPostMessage("slide_fail", kUnit); // 32           
        }
    }
    private void OnIntercept(LLUnit kUnit, params object[] kParams)
    {
        if (null == kUnit || null == kParams)
            return;

        bool bFlag = (bool)(kParams[0]);

        if (true == bFlag) // 抢截成功
        {
            SendPostMessage("intercept_success", kUnit); // 29           
        }
        else
        {
            SendPostMessage("intercept_fail", kUnit); // 30           
        }
    }
    private void OnBreakthrough(LLUnit kUnit, params object[] kParams)
    {
        if (null == kUnit || null == kParams)
            return;

        bool bFlag = (bool)(kParams[0]);

        if (true == bFlag) // 突破成功
        {
            SendPostMessage("breakthrough_success", kUnit); // 27            
        }
        else
        {
            SendPostMessage("breakthrough_fail", kUnit); // 28          
        }
    }

    private void OnBlock(LLUnit kUnit)
    {
        if (null == kUnit)
            return;

        SendPostMessage("block", kUnit); // 26       
    }
    private void OnGoalkeeperEvent(LLUnit kUnit, params object[] kParams)
    {
        if (null == kUnit || null == kParams)
            return;

        bool bFlag = (bool)(kParams[0]);

        if (true == bFlag) // 守门员扑球成功
        {
            SendPostMessage("keeper_save1", kUnit); // 15          
        }
        else
        {
            SendPostMessage("keeper_save2", kUnit); // 16            
        }
    }

    private void OnHeadGoal(LLUnit kUnit)
    {
        if (null == kUnit)
            return;

        SendPostMessage("head_goal", kUnit); // 14      
    }

    private void OnCatchBall(LLUnit kUnit)
    {
        if (null == kUnit)
            return;

        SendPostMessage("catch_ball", kUnit);// 25      
    }

    private void OnPassball(LLUnit kUnit, params object[] kParams)
    {
        if (null == kUnit || null == kParams)
            return;

        bool bFlag = (bool)(kParams[0]);

        if(true == bFlag)
        {
            // 传地面球
            SendPostMessage("pass1", kUnit); // 23          
        }
        else
        {
            // 传高空球
            SendPostMessage("pass2", kUnit); // 24          
        }
    }
    private void OnDribble(LLUnit kUnit)
    {
        if (null == kUnit)
            return;
        if (kUnit.RegionID > 20)
        {
            //在后场带球
            SendPostMessage("dribble1", kUnit); // 18           
        }
        else if ((kUnit.RegionID >= 12 && kUnit.RegionID <= 14)||(kUnit.RegionID >= 17 && kUnit.RegionID <= 19))
        {
            //在前场带球（12-14区域，17-19区域）
            SendPostMessage("dribble2", kUnit);  // 19
        }
        else if ((kUnit.RegionID >= 4 && kUnit.RegionID <= 4) || (kUnit.RegionID >= 7 && kUnit.RegionID <= 9))
        {
            //在前场带球（2-4区域，7-9区域）
            SendPostMessage("dribble4", kUnit);  // 21          
        }
        else if (kUnit.RegionID == 11 || kUnit.RegionID == 15 
            || kUnit.RegionID == 16 || kUnit.RegionID == 20)
        {
            //在前场带球（11,15,16,20区域）
            SendPostMessage("dribble3", kUnit);  // 20  
        }
        else if (kUnit.RegionID == 1 || kUnit.RegionID == 5
            || kUnit.RegionID == 6 || kUnit.RegionID == 10)
        {
            //在前场带球（1,5,6,10区域）
            SendPostMessage("dribble5", kUnit); // 22           
        }
    }

    private void OnShootInside(LLUnit kUnit, params object[] kParams)
    {
        if (null == kUnit || null == kParams)
            return;

        bool bFlag = (bool)(kParams[0]);
        if(true == bFlag) // 远射
        {
            SendPostMessage("near_shoot", kUnit); // 12         
        }
        else
        {
            SendPostMessage("far_shoot", kUnit); // 12           
        }
    }

    private void OnShootOutside(LLUnit kUnit, params object[] kParams)
    {
        if (null == kUnit || null == kParams)
            return;
        SendPostMessage("shoot_miss", kUnit); // 17       
    }
    private void OnShootSuccessMsg(LLUnit kUnit)
    {
        if (null == kUnit)
            return;
        // Comment 01
        SendPostMessage("goal", kUnit); // 1      

        // 检查是否扳平比分 Comment 02
        int iOwnerScore = kUnit.Team.TeamInfo.Score;
        int iOpponentScore = kUnit.Team.Opponent.TeamInfo.Score;

        if (0 == iOpponentScore && 1 == iOwnerScore)
        {
            // 在0比0的时候，破门得分
            SendPostMessage("goal_comment5", kUnit); // 6           
        }
        if (iOwnerScore == iOpponentScore)
        {
            //扳平比分
            SendPostMessage("goal_comment1", kUnit);   // 2
        }
        else if(iOwnerScore < iOpponentScore)
        {
            //进球后仍然落后
            SendPostMessage("goal_comment2", kUnit);   // 3  
        }
        else
        {
            if(GlobalBattleInfo.Instance.GetRealWorldTime() > 4800) 
            {
                //80分钟之后进球，并从平局到领先
                SendPostMessage("goal_comment4", kUnit); // 5 
            }
            else
            {
                //进球后反超了比分
                SendPostMessage("goal_comment3", kUnit); // 4                
            }
        }


        // 进球者评论 1,2,3,4,5, //6 No Comment
        if(kUnit.PlayerBaseInfo.Score < 6) // 7-11
        {
            CommonShooter(kUnit.PlayerBaseInfo.Score, kUnit);
        }
    }
    private void CommonShooter(int index_, LLUnit kUnit_)
    {
        System.Text.StringBuilder str = new System.Text.StringBuilder("shooter_comment");
        SendPostMessage(str.Append(index_).ToString(), kUnit_);
    }

    private void OnShootFailedMsg(LLUnit kUnit)
    {
        if (null == kUnit)
            return;

    }

    // ---------------
    public void OnUpdate(float deltaTime_)
    {
        if (m_lastEventTime < 3.0f)
        {
            m_lastEventTime += deltaTime_;
        } 
        else
        {
            OnNoneEvent3s();
        }
    }

    private void OnNoneEvent3s()
    {
        while (true)
        {
            int rate = (int)FIFARandom.GetRandomValue(0, commonEventList.Count - 1);
            if (commonEventList[rate]())
            {
                return;
            }
        }
    }

    private void SendPostMessage(string strID_, LLUnit kUnit_)
    {
        if (IsNoLastStrID(strID_))
        {
            m_lastEventTime = 0;
            BattleTextMessage kMsg = new BattleTextMessage(strID_, kUnit_);
            MessageDispatcher.Instance.PostMessage(kMsg);
        }
    }

    private bool IsNoLastStrID(string strID)
    {
        if (string.Equals(strID, m_lastSubCommenID))
        {
            return false;
        }
        m_lastSubCommenID = strID;
        return true;
    }
}