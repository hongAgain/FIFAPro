using BehaviourTree;
using Common;
using Common.Tables;
using System;
using System.Collections.Generic;

public class GameStateShoot : IGameState
{
    public GameStateShoot(LLScene kScene)
    {
        m_kScene = kScene;
        m_kGameState = EGameState.GS_SHOOT;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        m_kShootPlayer = null;
        // 改变球员的状态为normal
        for (int i = 0; i < m_kScene.BlueTeam.PlayerList.Count; i++)
        {
            if(EPlayerState.Shoot ==  m_kScene.BlueTeam.PlayerList[i].State || EPlayerState.Shoot_Head == m_kScene.BlueTeam.PlayerList[i].State)
            {
                m_kShootPlayer = m_kScene.BlueTeam.PlayerList[i];
                continue;
            }
            else
                m_kScene.BlueTeam.PlayerList[i].SetState(EPlayerState.Idle);
        }
            
        for (int i = 0; i < m_kScene.RedTeam.PlayerList.Count; i++)
        {
            if (EPlayerState.Shoot == m_kScene.RedTeam.PlayerList[i].State || EPlayerState.Shoot_Head == m_kScene.RedTeam.PlayerList[i].State)
            {
                m_kShootPlayer = m_kScene.RedTeam.PlayerList[i];
                continue;
            }
            else
                m_kScene.RedTeam.PlayerList[i].SetState(EPlayerState.Idle);

        }

    }

    public override void OnExit()
    {
        m_kShootPlayer = null;
        base.OnExit();
    }
    public override void OnUpdate(float fTime)
    {
        if(null != m_kShootPlayer)
        {
            if(m_kShootPlayer.AniFinish)
            {
                m_kShootPlayer.SetState(EPlayerState.Idle);
                m_kShootPlayer = null;
            }
        }
        base.OnUpdate(fTime);
    }

    private LLPlayer m_kShootPlayer = null;
}
