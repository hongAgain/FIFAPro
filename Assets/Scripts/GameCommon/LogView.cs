using UnityEngine;
using System.Collections.Generic;

public class LogView : MonoBehaviour
{
    private bool[] LogLevelFilter = new bool[3] { true, true, true };
    private Vector2 scrollPos = Vector2.zero;

    private struct LogData
    {
        public string mLog;
        public LogType mLT;

        public LogData(string log, LogType lt)
        {
            mLog = log;
            mLT = lt;
        }
    }

    private static List<LogData> mLogs = new List<LogData>();

    void OnGUI()
    {
        LogLevelFilter[0] = GUILayout.Toggle(LogLevelFilter[0], "Normal");
        LogLevelFilter[1] = GUILayout.Toggle(LogLevelFilter[1], "WARN");
        LogLevelFilter[2] = GUILayout.Toggle(LogLevelFilter[2], "ERROR");

        scrollPos = GUILayout.BeginScrollView(scrollPos);
        foreach (var log in mLogs)
        {
            if (log.mLT == LogType.Log && LogLevelFilter[0])
            {
                GUI.color = Color.white;
                GUILayout.Label(log.mLog);
            }
            else if (log.mLT == LogType.Warning && LogLevelFilter[1])
            {
                GUI.color = Color.yellow;
                GUILayout.Label(log.mLog);
            }
            else if (log.mLT == LogType.Error && LogLevelFilter[2])
            {
                GUI.color = Color.red;
                GUILayout.Label(log.mLog);
            }
        }

        GUILayout.EndScrollView();

        GUI.color = Color.white;
        
        if (GUILayout.Button("Clear Log"))
            mLogs.Clear();

        if (GUILayout.Button("Close LogView"))
            WindowMgr.CloseWindow("UILogView");
    }

    public static void Log(string s)
    {
        mLogs.Add(new LogData(s, LogType.Log));
    }
    
    public static void Warn(string s)
    {
        mLogs.Add(new LogData(s, LogType.Warning));
    }
    
    public static void Error(string s)
    {
        mLogs.Add(new LogData(s, LogType.Error));
    }
}