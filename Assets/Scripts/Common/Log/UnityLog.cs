using UnityEngine;

namespace Common.Log
{
    public class UnityLogActor : ILogActor
    {
        public override string GetName()
        {
            return "UnityLog";
        }

        public override bool Log(string strLogText)
        {
            if (strLogText.IndexOf("[INF]") >= 0)
            {
                Debug.Log(strLogText);
            }
            else if (strLogText.IndexOf("[WRN]") >= 0)
            {
                Debug.LogWarning(strLogText);
            }
            else if (strLogText.IndexOf("[DBG]") >= 0)
            {
                Debug.LogWarning(strLogText);
            }
            else if (strLogText.IndexOf("[CRI]") >= 0)
            {
                Debug.LogError(strLogText);
            }
            else
            {
                Debug.LogError(strLogText);
            }
            return true;
        }
    }


    public class ScreenLogActor : ILogActor
    {
        public override string GetName()
        {
            return "ScreenActor";
        }

        public override bool Log(string strLogText)
        {
            
            if (null == Client.Instance || false == Client.Instance.ScreenLogEnable
                || null == Client.Instance.ScreenLogView)
                return true;
            if (strLogText.IndexOf("[CRI]") >= 0)
            {
                Client.Instance.ScreenLogView.Log(strLogText);
            }
            return true;
        }

        public override void UnInit()
        {
        }
    }

}