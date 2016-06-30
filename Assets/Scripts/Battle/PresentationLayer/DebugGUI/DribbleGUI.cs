

using Common;
using UnityEngine;
public class DribbleGUI : MonoBehaviour
{
    public void Start()
    {

    }

    public void Update()
    {
        if (null == LLDirector.Instance.Scene)
            return;

        LLTeam kTeam = LLDirector.Instance.Scene.RedTeam.State == ETeamState.TS_ATTACK ? LLDirector.Instance.Scene.RedTeam : LLDirector.Instance.Scene.BlueTeam;
        m_kData = kTeam.DribblePrData;
    }

    void OnGUI()
    {
        if (null == m_kData)
            return;

        GUI.Box(new Rect(10, 200, Screen.width / 2 - 20, Screen.height - 10), "八向带球调试信息");
        GUILayout.BeginArea(new Rect(10, 220, Screen.width / 2 - 20, Screen.height -10));
            GUILayout.BeginVertical();

                GUILayout.BeginHorizontal();
                GUILayout.Label(string.Format("球员ID:{0}", m_kData.HeroID));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label(string.Format("球员职业:{0}", m_kData.Career));
                GUILayout.EndHorizontal();


                GUILayout.BeginHorizontal();
                GUILayout.Label(string.Format("当前格子ID:{0}", m_kData.RegionID));
                GUILayout.EndHorizontal();

                for (int i = 0;i < m_kData.Score.Count;i++)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(string.Format("方向{0}",i+1));
                    GUILayout.Label(string.Format("职业:{0}  战术:{1}  球员密度:{2}  总得分:{3}", m_kData.CareerScore[i],
                        m_kData.Tactics[i], m_kData.Density[i], m_kData.Score[i]));
                    GUILayout.EndHorizontal();
                }
            GUILayout.EndVertical();
        GUILayout.EndArea();
    }


    private DribblePrData m_kData;
}
