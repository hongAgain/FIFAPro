using UnityEngine;
using System.Collections;

public class TestinMgr : MonoBehaviour
{
    private static TestinMgr m_instance;
    public static TestinMgr Instance
    {
        get
        {
            if (m_instance == null)
                Common.Log.LogManager.Instance.LogError("Init TestinMgr Fail!!!");
            return m_instance;
        }
    }

    void Start()
    {
        m_instance = this;
#if UNITY_ANDROID
        TestinAgentHelper.Init();
#elif UNITY_IPHONE
        CrashMasterHelper.Init();
#endif
    }


    public void TestinSetUserInfo(string userInfo_)
    {
#if UNITY_ANDROID
        TestinAgentHelper.SetUserInfo(userInfo_);
#elif UNITY_IPHONE
        CrashMasterHelper.SetUserInfo(userInfo_);
#endif

    }

    public void TestinLeaveBreadcrumb(string breadcrumb_)
    {
#if UNITY_ANDROID
        TestinAgentHelper.leaveBreadcrumb(breadcrumb_);
#elif UNITY_IPHONE
        CrashMasterHelper.leaveBreadcrumb(breadcrumb_);
#endif
    }
    

    public void TestinExceptionLog(System.Exception erro_)
    {
#if UNITY_ANDROID
        TestinAgentHelper.LogHandledException(erro_);     
#elif UNITY_IPHONE
        CrashMasterHelper.LogHandledException(erro_);
#endif

    }



}
