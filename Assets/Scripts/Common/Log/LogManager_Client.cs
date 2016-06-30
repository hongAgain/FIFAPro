using System.Diagnostics;

namespace Common.Log
{
    public partial class LogManager
    {
        [Conditional("FIFA_CLIENT")]
        private void InitClientActor()
        {
            uint dwLogLevel = (uint)ELogLevel.ELL_INF + (uint)ELogLevel.ELL_WRN + (uint)ELogLevel.ELL_CRI;
            if (!m_kLogProcessor.Init(dwLogLevel))
            {
                UnityEngine.Debug.LogError("LogManager is initialized failed.");
            }
            UnityLogActor unityLog = new UnityLogActor();
            m_kLogProcessor.RegisterLogActor(unityLog);
            ScreenLogActor kScreenLog = new ScreenLogActor();
            m_kLogProcessor.RegisterLogActor(kScreenLog);

            string log = string.Format("{0}/log_{1}.log", UnityEngine.Application.persistentDataPath, m_kLogProcessor.GetDatePrefix());
            m_kFileActor = new FileLogActor(log);
            m_kLogProcessor.RegisterLogActor(m_kFileActor);
        }
    }
}
