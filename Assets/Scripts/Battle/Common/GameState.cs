using BehaviourTree;
using Common;
using Common.Tables;
using System;
using System.Collections.Generic;

public enum EGameState
{
    GS_INIT = 0,
    GS_PAUSE,
    GS_RUNNING,
    GS_MIDKICK = GS_RUNNING,    // 中场开球
    GS_SHOOT,                   // 射门事件
    GS_HALFEND,                 // 半场结束
    GS_FREEKICK,
    GS_CORNER,
    GS_NORMAL,
    GS_FIX_PASS,               // 开球后的固定状态，只能传球给其后方的人
    GS_CELEBRATION,            //射门之后庆祝演示动画//
    GS_END,
}

public class IGameState
{
    public virtual void OnEnter()
    {
     
    }

    public virtual void OnExit()
    {
    }

    public virtual void OnUpdate(float fTime)
    {
        #region 注释
        /*
         * 更新流程: 场景更新--
         *                  |
         *                  |----红队球队信息更新--
         *                                      |
         *                                      |--后卫线更新
         *                                      |
         *                                      |--球员所属格式ID更新
         *                                      |
         *                                      |--球队AI更新
         *                  |
         *                  |----蓝队球队信息更新
         *                                      |
         *                                      |--后卫线更新
         *                                      |
         *                                      |--球员所属格式ID更新
         *                                      |
         *                                      |--球队AI更新
         *                  |
         *                  |----红队球员信息更新--
         *                                      |
         *                                      |--球员朝向更新
         *                                      |--球员AI更新
         *                  |
         *                  |----蓝队球员信息更新
         *                                      |
         *                                      |--球员朝向更新
         *                                      |--球员AI更新
         */
        #endregion

        if (null != m_kScene.BlueTeam)
            m_kScene.BlueTeam.Update(fTime);
        if (null != m_kScene.RedTeam)
            m_kScene.RedTeam.Update(fTime);
        if (null != m_kScene.Ball)
            m_kScene.Ball.Update(fTime);
        if (null != m_kScene.BlueTeam)
            m_kScene.BlueTeam.TeamPlayerUpdate(fTime);
        if (null != m_kScene.RedTeam)
            m_kScene.RedTeam.TeamPlayerUpdate(fTime);
    }

    public EGameState State
    {
        get { return m_kGameState; }
        set { m_kGameState = value; }
    }


    protected EGameState m_kGameState;
    protected LLScene m_kScene = null;
}

public class GameStatePause : IGameState
{
    public GameStatePause(LLScene kScene)
    {
        m_kScene = kScene;
        m_kGameState = EGameState.GS_PAUSE;
    }

    public override void OnUpdate(float fTime)
    {
        
    }
}

public class GameStateMidKick : IGameState
{
    private enum EInnerState
    {
        WaitForTime = 0,
        Normal,
    }

    public GameStateMidKick(LLScene kScene)
    {
        m_kScene = kScene;
        m_kGameState = EGameState.GS_MIDKICK;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        if (null == m_kScene)
            return;
        if (null != m_kScene.Ball)
        {
            m_kScene.Ball.SetPosition(Vector3D.zero);
            m_kScene.Ball.SetTarget(new Vector3D(0, 0, 0));
        }

        if (null != m_kScene.RedTeam)
        {
            m_kScene.RedTeam.ResetToMidKick();
        }
            
        if (null != m_kScene.BlueTeam)
        {
            m_kScene.BlueTeam.ResetToMidKick();
        }

        // 设置球在控球球员脚下
        m_kScene.RedTeam.GoalKeeper.GKState = EGKState.GS_DEFAULT;
        m_kScene.BlueTeam.GoalKeeper.GKState = EGKState.GS_DEFAULT;

        m_kScene.BlueTeam.SetTeamPlayerAIState(false);
        m_kScene.RedTeam.SetTeamPlayerAIState(false);
        RefreshHomePositionMessage _redmsg = new RefreshHomePositionMessage(m_kScene);
        RefreshHomePositionMessage _blueMsg = new RefreshHomePositionMessage(m_kScene);
        MessageDispatcher.Instance.SendMessage(_redmsg);
        MessageDispatcher.Instance.SendMessage(_blueMsg);
        m_fElapseTime = 0;
        m_kInnerState = EInnerState.WaitForTime;
    }
    public override void OnExit()
    {
        base.OnExit();
    }

    public override void OnUpdate(float fTime)
    {
     //   base.OnUpdate(fTime);
        m_fElapseTime += fTime;
        switch (m_kInnerState)
        {
            case EInnerState.WaitForTime:
                if (m_fElapseTime > m_fWaitTime)
                {
                    m_kInnerState = EInnerState.Normal;

                    m_kScene.BlueTeam.SetTeamPlayerAIState(true);
                    m_kScene.RedTeam.SetTeamPlayerAIState(true);
                }
                break;
            case EInnerState.Normal:
                base.OnUpdate(fTime);
                break;
        }
    }

    private float m_fElapseTime;
    private float m_fWaitTime = 1;
    private EInnerState m_kInnerState;
}

public class GameStateFixPass : IGameState
{
    public GameStateFixPass(LLScene kScene)
    {
        m_kScene = kScene;
        m_kGameState = EGameState.GS_FIX_PASS;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        // 改变球员的状态为normal
        if (ETeamState.TS_ATTACK == m_kScene.BlueTeam.State)
        {
            m_kAttackTeam = m_kScene.BlueTeam;
        }
        else
        {
            m_kAttackTeam = m_kScene.RedTeam;
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void OnUpdate(float fTime)
    {
        base.OnUpdate(fTime);
        if (ETeamState.TS_ATTACK == m_kScene.BlueTeam.State)
        {
            m_kAttackTeam = m_kScene.BlueTeam;
        }
        else
        {
            m_kAttackTeam = m_kScene.RedTeam;
        }
        if (m_kAttackTeam.MidKickFinished)
        {
            m_kAttackTeam.ChangeGameState(EGameState.GS_NORMAL);
        }
    }

    private LLTeam m_kAttackTeam = null;
    private UInt32 m_uiMaxCnt = 1;
}
public class GameStateNormal : IGameState
{
    public GameStateNormal(LLScene kScene)
    {
        m_kScene = kScene;
        m_kGameState = EGameState.GS_NORMAL;
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnExit()
    {
        base.OnExit();
    }
    public override void OnUpdate(float fTime)
    {
        base.OnUpdate(fTime);
        BattleCommentMgr.Instance.OnUpdate(fTime);
    }

}
public class GameStateCelebration : IGameState
{
    private enum EInnerState
    {
        Begin = 0,
        WaitGoalAni,
        Normal,
    }

    public GameStateCelebration(LLScene kScene)
    {
        m_kScene = kScene;
        m_kGameState = EGameState.GS_CELEBRATION;
    }

    public override void OnEnter()
    {
        m_kInterState = EInnerState.Begin;
        LLDirector.Instance.EnableCalcTime = false;
        m_enterDestoryFlag = false;
        base.OnEnter();
        m_kScene.BlueTeam.SetTeamPlayerAIState(false);
        m_kScene.RedTeam.SetTeamPlayerAIState(false);
       

        if (m_kScene.BlueTeam.ShootData.Valid)
        {
            m_shootData = m_kScene.BlueTeam.ShootData;
            m_CelePlayerList = m_kScene.BlueTeam.PlayerList;
            m_SadPlayerList = m_kScene.RedTeam.PlayerList;
        }
        else
        {
            m_shootData = m_kScene.RedTeam.ShootData;
            m_CelePlayerList = m_kScene.RedTeam.PlayerList;
            m_SadPlayerList = m_kScene.BlueTeam.PlayerList;
        }
        //球员的庆祝还是沮丧//
        if(m_CelePlayerList!=null&&m_CelePlayerList.Count>0)
        {
            for (int i = 0; i < m_CelePlayerList.Count;i++ )
            {
                m_CelePlayerList[i].SetAniState(EAniState.GoalCeleBration);
            }
        }
        if (m_SadPlayerList != null && m_SadPlayerList.Count > 0)
        {
            for (int i = 0; i < m_SadPlayerList.Count; i++)
            {
                m_SadPlayerList[i].SetAniState(EAniState.GoalSad);
            }
        }

        HideBattleUIMessage kHideUIMsg = new HideBattleUIMessage();
        MessageDispatcher.Instance.SendMessage(kHideUIMsg);
        m_dElapseTime = 0;
    }

    public override void OnExit()
    {

        BattleDestoryGoalCeleBrationMsg _msg = new BattleDestoryGoalCeleBrationMsg();
        MessageDispatcher.Instance.SendMessage(_msg);

        HideInGoalUIMessage kInGoalMsg = new HideInGoalUIMessage();
        MessageDispatcher.Instance.SendMessage(kInGoalMsg);

        LLDirector.Instance.WaitForFirstHalfEnd = false;
        ShowBattleUIMessage kMsg = new ShowBattleUIMessage();
        MessageDispatcher.Instance.SendMessage(kMsg);
        LLDirector.Instance.EnableCalcTime = true; 
        m_shootData = null;
        m_kCeleData = null;
    }

    public override void OnUpdate(float fTime)
    {
        m_dElapseTime += fTime;
        switch (m_kInterState)
        {
            case EInnerState.Begin:
                {
                    CreateGoalAniMessage kMsg = new CreateGoalAniMessage();
                    MessageDispatcher.Instance.SendMessage(kMsg);
                    m_dElapseTime = 0;
                    m_kInterState = EInnerState.WaitGoalAni;
                }
                break;
            case EInnerState.WaitGoalAni:
                {
                    if(m_dElapseTime > m_dGoalAniTime)
                    {
                        DestroyGoalAniMessage kMsg = new DestroyGoalAniMessage();
                        MessageDispatcher.Instance.SendMessage(kMsg);

                        ShowInGoalUIMessage kInGoalMsg = new ShowInGoalUIMessage(m_shootData.Player);
                        MessageDispatcher.Instance.SendMessage(kInGoalMsg);
                        m_bShowCelAni = true;
#if !GAME_AI_ONLY
                        if (null != UIBattle.Instance && UIBattle.Instance.Is2DModel)
                            m_bShowCelAni = false;
#endif
                        if(m_bShowCelAni)
                        {
                            //随机获取庆祝动画//
                            int _totalCount = TableManager.Instance.GoalCelebrationTb.m_configs.Count;
                            int _index = (int)FIFARandom.GetRandomValue(1, _totalCount);
                            //                        _index = 6;
                            m_kCeleData = TableManager.Instance.GoalCelebrationTb.GetGoalCelebrationDataById(_index);
                            if (m_kCeleData != null && m_shootData != null)
                            {
                                BattleGoalCeleBrationMsg _Msg = new BattleGoalCeleBrationMsg(m_kCeleData, m_shootData.Player);
                                MessageDispatcher.Instance.SendMessage(_Msg);
                            }
                            m_dElapseTime = 0;
                        }
                        m_kInterState = EInnerState.Normal;
                    }
                }
                break;
            case EInnerState.Normal:
                {
                    base.OnUpdate(fTime);
                    if(false == m_bShowCelAni)
                    {
                        HideInGoalUIMessage kInGoalMsg = new HideInGoalUIMessage();
                        MessageDispatcher.Instance.SendMessage(kInGoalMsg);
                        m_kScene.SetState(EGameState.GS_MIDKICK);
                        m_kScene.BlueTeam.SetTeamPlayerAIState(true);
                        m_kScene.RedTeam.SetTeamPlayerAIState(true);
                    }
                    else if (m_dElapseTime >= m_kCeleData.m_AniTime && null != m_kScene && !m_enterDestoryFlag)
                    {
                        BattleDestoryGoalCeleBrationMsg _msg = new BattleDestoryGoalCeleBrationMsg();
                        MessageDispatcher.Instance.SendMessage(_msg);

                        HideInGoalUIMessage kInGoalMsg = new HideInGoalUIMessage();
                        MessageDispatcher.Instance.SendMessage(kInGoalMsg);
                        m_kScene.SetState(EGameState.GS_MIDKICK);
                        m_kScene.BlueTeam.SetTeamPlayerAIState(true);
                        m_kScene.RedTeam.SetTeamPlayerAIState(true);
                        m_enterDestoryFlag = true;
                    }
                }
                break;
        }
        
    }

    private bool m_bShowCelAni = true;
    private bool m_enterDestoryFlag = false;
    private ShootData m_shootData = null;
    private GoalCelebrationData m_kCeleData = null;
    private double m_dElapseTime = 0;
    private double m_dGoalAniTime = 1.2f;
    private EInnerState m_kInterState;
    private List<LLPlayer> m_CelePlayerList = new List<LLPlayer>();
    private List<LLPlayer> m_SadPlayerList = new List<LLPlayer>();
}