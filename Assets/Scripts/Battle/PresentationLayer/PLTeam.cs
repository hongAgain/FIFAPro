using UnityEngine;
using System.Collections;
using System;
using Common;
using System.Collections.Generic;
using LuaInterface;
using Common.Log;
using System.Text;

// PL 表示presentation layer 表现层
// LL 表示Logical Layer 逻辑层

public class PLTeam : MonoBehaviour
{
    public void Start()
    {
        //gameObject.layer = LayerMask.NameToLayer("SceneFx");
    }
    public void CreatePlayer(LLUnit kUnit,bool bGoalKeeper)
    {
        
        if (null == kUnit)
            return;

        if (m_kPlayerList.ContainsKey(kUnit.PlayerBaseInfo.PlayerID))
            return;
        UInt32 uiHeroID = kUnit.PlayerBaseInfo.HeroID;
        bool bHost = (m_kLLTeam.TeamColor == ETeamColor.Team_Red) ? true : false;
        GameObject kRoleObj = AvatarManager.Instance.CreateRoleObject((int)uiHeroID, "Player", (int)kUnit.PlayerBaseInfo.ClubID, bHost);
        kRoleObj.transform.parent = gameObject.transform;
        kRoleObj.name = ((int)kUnit.PlayerBaseInfo.PlayerID).ToString();
        PLPlayer kPlayer = kRoleObj.AddComponent<PLPlayer>();
        kPlayer.Player = kUnit;
        kPlayer.IsGoalKeeper = bGoalKeeper;
        m_kPlayerList.Add(kUnit.PlayerBaseInfo.PlayerID, kPlayer);
    }

    public void RemovePlayer(LLUnit kUnit, bool bGoalKeeper)
    {
        if (null == kUnit)
            return;
        if (m_kPlayerList.ContainsKey(kUnit.PlayerBaseInfo.PlayerID))
        {
            GameObject.Destroy(m_kPlayerList[kUnit.PlayerBaseInfo.PlayerID].gameObject);
            m_kPlayerList.Remove(kUnit.PlayerBaseInfo.PlayerID);
        }
    }
    public void Update()
    {
        DebugDraw();
        if(false == m_bRunning)
            return;
    }

    private void DebugDraw()
    {
        if (null != DebugGUI.Instance)
        {
            if (true == DebugGUI.Instance.ShowGuardLine)
            {
                DrawGuardLine();
            }
            
            if (true == DebugGUI.Instance.ShowTeamInfo)
            {

                if (m_kLLTeam.TeamColor == ETeamColor.Team_Red)
                    DrawStateInfo("red");
                else
                    DrawStateInfo("green");
            }
            else if (true == DebugGUI.Instance.ShowAttackTeamInfo && ETeamState.TS_ATTACK == m_kLLTeam.State)
            {
                if (m_kLLTeam.TeamColor == ETeamColor.Team_Red)
                    DrawStateInfo("red");
                else
                    DrawStateInfo("green");
            }
            else if (true == DebugGUI.Instance.ShowDefendTeamInfo && ETeamState.TS_DEFEND == m_kLLTeam.State)
            {
                if (m_kLLTeam.TeamColor == ETeamColor.Team_Red)
                    DrawStateInfo("red");
                else
                    DrawStateInfo("green");
            }
        }
    }

    private void DrawStateInfo(String strColor)
    {
        String strMsg = String.Format("球场状态:{0}   球队状态:{1}", m_kLLTeam.Scene.GameState, m_kLLTeam.State.ToString());
        LogManager.Instance.ColorLog(strColor, strMsg);
        foreach (var kItem in m_kPlayerList)
        {
            PLPlayer kPlayer = kItem.Value;
            if (null == kPlayer || null == kPlayer.Player)
                continue;

            if (kPlayer.IsGoalKeeper)
            {
               // LLGoalKeeper kEntity = (LLGoalKeeper)kPlayer.Player;
                //strMsg = String.Format("ID:{0} 状态:{1}", kEntity.HeroID, kEntity.State.ToString());
                //if (kEntity.CtrlBall)
                //    strMsg = String.Format("{0} 控球", strMsg);
            }
            else
            {
                LLPlayer kUnit = (LLPlayer)kPlayer.Player;
                if (EPlayerState.HomePos != kUnit.State)
                {
                    strMsg = String.Format("ID:{0} 状态:{1}", kUnit.PlayerBaseInfo.HeroID, kUnit.State.ToString());
                    if (kUnit.IsCtrlBall)
                        strMsg = String.Format("{0} 控球", strMsg);
                    LogManager.Instance.ColorLog(strColor, strMsg);
                }
            }

        }
    }

    private void DrawGuardLine()
    {
        Color kColor = m_kLLTeam.TeamColor == ETeamColor.Team_Red ? Color.red : Color.blue;
        LLScene kScene = m_kLLTeam.Scene;

        Double fStartX = kScene.Region.Top;
        Double fEndX = kScene.Region.Bottom;
        Vector3 kStartPos = new Vector3((float)fStartX, 1, (float)m_kLLTeam.GuardLine);
        Vector3 kEndPos = new Vector3((float)fEndX, 1, (float)m_kLLTeam.GuardLine);
        for (int i = 0; i < 6; i++)
        {
            UnityEngine.Debug.DrawLine(kStartPos, kEndPos, kColor);
            kStartPos.z += (float)m_kLLTeam.LineDist;
            kEndPos.z += (float)m_kLLTeam.LineDist;
        }
    }

    public PLPlayer GetPlayer(LLUnit kUnit)
    {
        foreach(var kItem in m_kPlayerList)
        {
            PLPlayer kPlayer = kItem.Value;
            if(kPlayer.Player == kUnit)
                return kPlayer;
        }
        return null;
    }

    public LLTeam Team
    {
        get { return m_kLLTeam; }
        set { m_kLLTeam = value; }
    }

    public void Pause()
    {
        m_bRunning = false;
        foreach (var kItem in m_kPlayerList)
        {
            PLPlayer kPlayer = kItem.Value as PLPlayer;
            if (kPlayer)
                kPlayer.Pause();
        }
    }

    public void Resume()
    {
        m_bRunning = true;
        
        foreach(var kItem in m_kPlayerList)
        {
            PLPlayer kPlayer = kItem.Value as PLPlayer;
            if(kPlayer)
                kPlayer.Resume();
        }
    }
   
    private bool m_bRunning = true;        // 用来表示游戏是暂停还是正常运行  仅用在GamePause State
    private LLTeam m_kLLTeam;
    private Dictionary<Int32, PLPlayer> m_kPlayerList = new Dictionary<Int32, PLPlayer>();
}

