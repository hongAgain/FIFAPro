using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Common;

public class SelectPlayerGUI : MonoBehaviour
{


    void Start()
    {
        if (instance == null)
            instance = this;
    }
    public void OnGUI()
    {

        if (m_players == null && m_players.Count == 0)
            return;
        GUI.Box(new Rect(10, 200, Screen.width / 2 - 20, Screen.height / 2 - 10), "选择球员比分信息");
        GUILayout.BeginArea(new Rect(30, 220, Screen.width / 2 - 20, Screen.height / 2));
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        GUILayout.Label(string.Format("球员ID", ""));
        GUILayout.Label(string.Format("距离得分", ""));
        GUILayout.Label(string.Format("战术得分", ""));
        GUILayout.Label(string.Format("接球者位置得分", ""));
        GUILayout.Label(string.Format("接球难度", ""));
        GUILayout.Label(string.Format("出球难度", ""));
        GUILayout.Label(string.Format("总得分", ""));

        if (m_bIsShow)
        {
            GUILayout.EndHorizontal();
            for (int i = 0; i < m_players.Count; i++)
            {
                if (m_players[i].Socore != null)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(string.Format("{0}", m_players[i].PlayerBaseInfo.HeroID));
                    GUILayout.Label(string.Format("{0}", m_players[i].Socore.m_disScore));
                    GUILayout.Label(string.Format("{0}", m_players[i].Socore.m_StageScore + m_players[i].Socore.m_DepthScore));
                    GUILayout.Label(string.Format("{0}", m_players[i].Socore.m_posScore));
                    GUILayout.Label(string.Format("{0}", m_players[i].Socore.m_getScore));
                    GUILayout.Label(string.Format("{0}", m_players[i].Socore.m_passScore));
                    GUILayout.Label(string.Format("{0}", m_players[i].GeterScore));
                    GUILayout.EndHorizontal();
                }

            }

            GUILayout.EndVertical();
            GUILayout.BeginHorizontal();
            GUILayout.Label(string.Format("接球球员ID为：{0}", m_Splayer.PlayerBaseInfo.HeroID));
            GUILayout.Label(string.Format("传球球员ID为：{0}", m_Pplayer.PlayerBaseInfo.HeroID));
            GUILayout.EndVertical();
        }
        GUILayout.EndArea();

    }

    public void BeginShow(List<LLPlayer> _players, LLPlayer _Splayer,LLPlayer _Pplayer)
    {
        m_players.Clear();
        m_players = _players;
        m_Splayer = _Splayer;
        m_Pplayer = _Pplayer;
        m_bIsShow = true;
    }
    private List<LLPlayer> m_players = new List<LLPlayer>();
    private LLPlayer m_Splayer = null;
    private LLPlayer m_Pplayer = null;

    public bool m_bIsShow = false;

    public static SelectPlayerGUI instance = null;

}
