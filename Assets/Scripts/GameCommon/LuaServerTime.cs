using UnityEngine;
using System;
using System.Collections;
using Common.Log;

public class LuaServerTime : MonoBehaviour
{
    public static DateTime TIME1970 = new DateTime(1970, 1, 1, 0, 0, 0).ToLocalTime();
    private static DateTime m_now;
    private static float m_syncTimeStamp;
    private static DateTime m_syncDateTime;

    public static LuaServerTime Instance;
    public static DateTime Now { get { return m_now; } }


    void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        m_now = DateTime.Now;
        m_syncTimeStamp = Time.realtimeSinceStartup;
        m_syncDateTime = m_now;


        InvokeRepeating("UpdateTime", 0.5f * UnityEngine.Random.value, 0.5f);
        UpdateTime();
    }


    void UpdateTime()
    {
        float deltaTime = Time.realtimeSinceStartup - m_syncTimeStamp;
        m_now = m_syncDateTime.AddSeconds(deltaTime);
    }

    public static void SynServerTime(double timestamp_)
    {
        LogManager.Instance.Log("SynServerTime: " + timestamp_);
        m_now = Time19702DateTime(timestamp_);
        m_syncTimeStamp = Time.realtimeSinceStartup;
        m_syncDateTime = m_now;
    }


    public static DateTime Time19702DateTime(double timestamp_)
    {
        return TIME1970.AddMilliseconds(timestamp_);
    }

    public static double DateTime2Time1970(DateTime dt_)
    {
        return (dt_ - TIME1970).TotalMilliseconds;
    }

    //void OnGUI()
    //{
    //    GUI.Label(new Rect(Screen.width - 100, Screen.height - 100, 100, 100), m_now.ToString());
    //    GUI.Label(new Rect(Screen.width - 100, Screen.height - 50, 100, 100), DateTime.Now.ToString());
    //}
}