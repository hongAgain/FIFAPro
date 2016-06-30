using System.Collections.Generic;
using UnityEngine;

public class ScreenLogView : MonoBehaviour
{
    void Awake()
    {
        m_kGUIStyle.fontSize = 20;
    }

	// Use this for initialization
	void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, Screen.width, Screen.height - 10));
        GUILayout.BeginVertical();
        GUILayout.Label(string.Format("<color=red>{0}</color>", m_strMsg), m_kGUIStyle);        
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

    public void Log(string strMsg)
    {
        m_strMsg = strMsg;
    }
    private string m_strMsg = "";
    private GUIStyle m_kGUIStyle = new GUIStyle();
}
