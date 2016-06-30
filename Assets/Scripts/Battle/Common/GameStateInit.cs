using BehaviourTree;
using Common;
using Common.Tables;
using System;
using System.Collections.Generic;

public class GameStateInit : IGameState
{
    private enum EInnerState
    {
        PreBattleUI = 0,
        WaitPreBattleUI,
        Begin_Camera_Ani,
        Wait_Camera_Finished,
        Wait_Begin_Ani,
        Normal,
    }
    public GameStateInit(LLScene kScene)
    {
        m_kScene = kScene;
        m_kGameState = EGameState.GS_INIT;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        if (null != m_kScene.RedTeam)
        {
            m_kScene.RedTeam.ResetToMidKick();
        }
        if (null != m_kScene.BlueTeam)
        {
            m_kScene.BlueTeam.ResetToMidKick();
        }

        m_kScene.BlueTeam.SetTeamPlayerAIState(false);
        m_kScene.RedTeam.SetTeamPlayerAIState(false);

        LLDirector.Instance.EnableCalcTime = false;

        BattleInfoItem kItem = TableManager.Instance.BattleInfoTable.GetItem("camera_kickoff");
        if (null != kItem)
            m_dCameraTime = kItem.Value;
        m_dElapseTime = 0;
        m_kInnerState = EInnerState.PreBattleUI;
    }

    public override void OnExit()
    {
        LLDirector.Instance.EnableCalcTime = true;
        base.OnExit();
    }

    public override void OnUpdate(float fTime)
    {
        m_dElapseTime += fTime;
        switch (m_kInnerState)
        {
            case EInnerState.PreBattleUI:
                {
#if !GAME_AI_ONLY
                    if(null != UIBattle.Instance)
                    {
                        m_dElapseTime = 0;
                        m_kInnerState = EInnerState.WaitPreBattleUI;
                        ShowPreBattleUIMessage kMsg = new ShowPreBattleUIMessage(m_kScene.RedTeam, m_kScene.BlueTeam);
                        MessageDispatcher.Instance.SendMessage(kMsg);
                    }
#else
                    m_kInnerState = EInnerState.WaitPreBattleUI;
#endif
                }

                break;
            case EInnerState.WaitPreBattleUI:
                if (true == m_bSkipPreBattleUI || m_dElapseTime > m_dPreBattleUI)
                {
                    DestroyPreBattleUIMessage kMsg = new DestroyPreBattleUIMessage();
                    MessageDispatcher.Instance.SendMessage(kMsg);
                    m_kInnerState = EInnerState.Begin_Camera_Ani;
                }
                break;
            case EInnerState.Begin_Camera_Ani:
                {
                    CameraAniEnableMessage kMsg = new CameraAniEnableMessage();
                    MessageDispatcher.Instance.SendMessage(kMsg);
                    m_kInnerState = EInnerState.Wait_Camera_Finished;
                }
                break;
            case EInnerState.Wait_Camera_Finished:
                if (m_dElapseTime > m_dCameraTime)
                {
                    m_kInnerState = EInnerState.Wait_Begin_Ani;
                    CreateBattleDescMessage kMsg = new CreateBattleDescMessage(Localization.Get("GameStart"));
                    MessageDispatcher.Instance.SendMessage(kMsg);
                }
                break;
            case EInnerState.Wait_Begin_Ani:
                if (m_dElapseTime > m_dCameraTime+ m_dAniTime)
                {
                    DestroyBattleDescMessage kMsg = new DestroyBattleDescMessage();
                    MessageDispatcher.Instance.SendMessage(kMsg);
                    m_kInnerState = EInnerState.Normal;
                }
                break;
            case EInnerState.Normal:
                m_kScene.SetState(EGameState.GS_MIDKICK);
                break;
            default:
                break;
        }
    }

    public bool SkipPreBattleUI
    {
        get { return m_bSkipPreBattleUI; }
        set { m_bSkipPreBattleUI = value; }
    }

    private double m_dPreBattleUI = 2.0f;
    private double m_dAniTime = 1.7f;
    private double m_dCameraTime = 2;
    private double m_dElapseTime = 0;
    private EInnerState m_kInnerState;
    private bool m_bSkipPreBattleUI = false;
}