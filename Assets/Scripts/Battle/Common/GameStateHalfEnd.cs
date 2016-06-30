using BehaviourTree;
using Common;
using Common.Tables;
using System;
using System.Collections.Generic;

public class GameStateHalfEnd : IGameState
{
    private enum EInnerState
    {
        Begin_State = 0,
        Wait_Begin_FirstHalfEnd_Ani,
        Wait_Begin_SecondHalfBegin_Ani,
        Wait_One_Frame,
        Normal,
    }
    public GameStateHalfEnd(LLScene kScene)
    {
        m_kScene = kScene;
        m_kGameState = EGameState.GS_HALFEND;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        LLDirector.Instance.EnableCalcTime = false;
        m_dElapseTime = 0;
        m_kInnerState = EInnerState.Begin_State;

        // 改变球员的状态为normal
        for (int i = 0; i < m_kScene.BlueTeam.PlayerList.Count; i++)
            m_kScene.BlueTeam.PlayerList[i].SetState(EPlayerState.Idle);

        for (int i = 0; i < m_kScene.RedTeam.PlayerList.Count; i++)
            m_kScene.RedTeam.PlayerList[i].SetState(EPlayerState.Idle);
    }

    public override void OnExit()
    {
        LLDirector.Instance.EnableCalcTime = true;
        base.OnExit();
    }

    public override void OnUpdate(float fTime)
    {
        base.OnUpdate(fTime);
        m_dElapseTime += fTime;
        switch (m_kInnerState)
        {
            case EInnerState.Begin_State:
                {
                    CreateBattleDescMessage kMsg = new CreateBattleDescMessage(Localization.Get("FirstHalfEnd"));
                    MessageDispatcher.Instance.SendMessage(kMsg);
                    m_kInnerState = EInnerState.Wait_Begin_FirstHalfEnd_Ani;
                }
                break;
            case EInnerState.Wait_Begin_FirstHalfEnd_Ani:
                if (m_dElapseTime > m_dAniTime)
                {
                    DestroyBattleDescMessage kMsg = new DestroyBattleDescMessage();
                    MessageDispatcher.Instance.SendMessage(kMsg);
                    m_kInnerState = EInnerState.Wait_One_Frame;
                    
                }
                break;
            case EInnerState.Wait_One_Frame:
                {
                    m_dElapseTime = 0;
                    if (null != m_kScene.Ball)
                    {
                        m_kScene.Ball.SetPosition(Vector3D.zero);
                        m_kScene.Ball.SetTarget(Vector3D.zero);

                        ChangeBallPosMessage kBallPosMsg = new ChangeBallPosMessage(m_kScene.Ball.GetPosition(), m_kScene.Ball);
                        MessageDispatcher.Instance.SendMessage(kBallPosMsg);
                    }
                    if (null != m_kScene.RedTeam && null != m_kScene.BlueTeam)
                    {
                        switch (m_kScene.HomeTeamColor)
                        {
                            case ETeamColor.Team_Red:
                                m_kScene.RedTeam.State = ETeamState.TS_DEFEND;
                                m_kScene.BlueTeam.State = ETeamState.TS_ATTACK;
                                break;
                            case ETeamColor.Team_Blue:
                                m_kScene.RedTeam.State = ETeamState.TS_ATTACK;
                                m_kScene.BlueTeam.State = ETeamState.TS_DEFEND;
                                break;
                        }
                        m_kScene.RedTeam.ResetToMidKick();
                        m_kScene.BlueTeam.ResetToMidKick();
                        // 改变球员的状态为normal
                        //for (int i = 0; i < m_kScene.BlueTeam.PlayerList.Count; i++)
                        //    m_kScene.BlueTeam.PlayerList[i].SetState(EPlayerState.Idle);

                        //for (int i = 0; i < m_kScene.RedTeam.PlayerList.Count; i++)
                        //    m_kScene.RedTeam.PlayerList[i].SetState(EPlayerState.Idle);
                    }
                   

                    CreateBattleDescMessage kMsg = new CreateBattleDescMessage(Localization.Get("SecondHalfBegin"));
                    MessageDispatcher.Instance.SendMessage(kMsg);
                    m_kInnerState = EInnerState.Wait_Begin_SecondHalfBegin_Ani; 
                }
                break;
            case EInnerState.Wait_Begin_SecondHalfBegin_Ani:
                if (m_dElapseTime > m_dAniTime)
                {
                    DestroyBattleDescMessage kMsg = new DestroyBattleDescMessage();
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

    private double m_dAniTime = 1.7f;
    private double m_dCameraTime = 2;
    private double m_dElapseTime = 0;
    private EInnerState m_kInnerState;
}