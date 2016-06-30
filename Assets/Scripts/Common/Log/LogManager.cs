namespace Common.Log
{
    public partial class LogManager
    {
        public readonly static LogManager Instance = new LogManager();
        private FileLogActor m_kFileActor = null;
        private LogProcessor m_kLogProcessor = new LogProcessor();
        #region Public Functions
        private LogManager()
        {
            InitClientActor();
        }

        public void UnInit()
        {
            m_kLogProcessor.UnRegisterLogActor(m_kFileActor);
            m_kFileActor.UnInit();
        }

        public void Log(string text)
        {
            Log(text, null);
        }

        public void Log(string strFormat, params object[] args)
        {
            if (false == m_bLogEnable)
                return;
            m_kLogProcessor.LOG_INF(strFormat, args);
        }

        public void LogWarning(string text)
        {
            LogWarning(text, null);
        }

        public void LogWarning(string strFormat, params object[] args)
        {
            if (false == m_bLogEnable)
                return;
            m_kLogProcessor.LOG_WRN(strFormat, args);
        }

        public void LogError(string text)
        {
            LogError(text, null);
        }
        public void LogError(string strFormat, params object[] args)
        {
            if (false == m_bLogEnable)
                return;
            m_kLogProcessor.LOG_CRI(strFormat, args);
        }
        public void RedLog(string strFormat)
        {
            RedLog(strFormat, null);
        }
        public void RedLog(string strFormat, params object[] args)
        {
            ColorLog("red", strFormat, args);
        }
        public void YellowLog(string strFormat)
        {
            YellowLog(strFormat, null);
        }
        public void YellowLog(string strFormat, params object[] args)
        {
            ColorLog("yellow", strFormat, args);
        }
        public void GreenLog(string strFormat)
        {
            GreenLog(strFormat, null);
        }
        public void GreenLog(string strFormat, params object[] args)
        {
            ColorLog("green", strFormat, args);
        }
        public void BlackLog(string strFormat)
        {
            BlackLog(strFormat, null);
        }
        public void BlackLog(string strFormat, params object[] args)
        {
            ColorLog("black", strFormat, args);
        }

        public void ColorLog(string strColor, string strFormat, params object[] args)
        {
            if (false == m_bLogEnable)
                return;
            m_kLogProcessor.LOG_INF(
                string.Format("<color={0}>{1}</color>", strColor, strFormat), args);
        }
        #endregion
        #region Property

        public bool LogEnable
        {
            get { return m_bLogEnable; }
            set { m_bLogEnable = value; }
        }

        public bool WriteFileEnable
        {
            get { return m_bWriteFileLog; }
            set { m_bWriteFileLog = value; }
        }

        #endregion Property
        private bool m_bLogEnable = true;
        private bool m_bWriteFileLog = true;
    }


}
