// 初始化示例       
//bool _InitLogProcessor()
//{
//    初始化CLogProcessor
//    CLogProcessor.CreateInstance();
//    if (!CLogProcessor.Instance().Init())
//    {
//        return false;
//    }
//    注册actor，将日志打印至屏幕
//    CScreenLogActor oScreenActor = new CScreenLogActor();
//    CLogProcessor.Instance().RegisterLogActor(oScreenActor);

//    注册actor，将日志打印至文件， 文件名为log_20140101
//    CFileLogActor oFileActor = new CFileLogActor();
//    string strLogFileName = "log_" + CLogProcessor.Instance().GetDatePrefix();
//    bool bResult = oFileActor.Init(ref strLogFileName);
//    if (bResult)
//    {
//        CLogProcessor.Instance().RegisterLogActor(oFileActor);
//    }
//    else
//    {
//        return false;
//    }

//    return true;
//}

//    取消注册actor
//    CLogProcessor.Instance().UnRegisterLogActor(oScreenActor);
//    oFileActor.UnInit();

// 使用示例   
//      CLogProcessor.Instance().LOG_CRI("my log {0}, {1}", "qqqqqq", "222222222");
//      CLogProcessor.Instance().LOG_DBG("my log {0}, {1}", "qqqqqq", "222222222");
//      CLogProcessor.Instance().LOG_WRN("my log {0}, {1}", "qqqqqq", "222222222");
//      CLogProcessor.Instance().LOG_INF("my log {0}, {1}", "qqqqqq", "222222222");

using System;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Common.Log
{
    public enum ELogLevel
    {
        ELL_BEGIN = 0,
        ELL_DBG,
        ELL_INF = 2,
        ELL_WRN = 4,
        ELL_CRI = 8,
        ELL_COUNT
    };

    public enum ELogType
    {
        ELType_COMMON = 1,
    };

    public class ILogActor
    {
	    public virtual bool Log(string strLogText) { return false; }
	    public virtual string GetName() 
        { 
            string strDefaultName = "defaultname";
            return strDefaultName;
        }

        public virtual void UnInit()
        {}
    };

    public class FileLogActor : ILogActor
    {
        private StreamWriter m_write;

        public FileLogActor(string fileName)
        {
            m_write = new StreamWriter(new FileStream(fileName, FileMode.Append), Encoding.UTF8);
            m_write.AutoFlush = true;
        }

        public override string GetName()
        {
            return "FileActor";
        }

        public override bool Log(string strLogText)
        {
            m_write.WriteLine(strLogText);
            return true;
        }

        public override void UnInit()
        {
            m_write.Close();
            m_write.Dispose();
            return;
        } 
    }

    public class  ILogPrefix
    {
	    public string GetLogPrefix(string strFileName, Int32 nLine, string strFuncName, string strLogLevel) 
        {
           // string strPrefix = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff ") + strLogLevel + " " + strFileName + "(" + nLine + ") : " + strFuncName + " - ";
           string strPrefix = strLogLevel + " " + strFileName + "(" + nLine + ") : " + strFuncName + " - "; 
            return strPrefix;
        }

       
    };

    public class ILogPostfix
    {
        public string GetLogPostfix()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff ");
        }
    }


    public class LogProcessor //: Singleton<LogProcessor>
    {
        //初始化，只能运行在主线程
        
        public bool Init()
        {
            uint dwLogLevel = (uint)ELogLevel.ELL_INF + (uint)ELogLevel.ELL_WRN + (uint)ELogLevel.ELL_CRI;
            return Init(dwLogLevel);
        }

        public bool Init(uint dwLogLevel)
        {
            m_dwLogLevel = dwLogLevel;

            m_oLogLock = new Object();

            m_oLogPrefix = new ILogPrefix();
            m_kLogPostfix = new ILogPostfix();
            m_oVecLogMsgs = new List<string>();
            m_oMapLogActor = new Dictionary<string, ILogActor>();

            m_oMainThread = Thread.CurrentThread;

            //CScreenLogActor oScreenActor = new CScreenLogActor();
            //RegisterLogActor(oScreenActor);

            //FileLogActor oFileActor = new FileLogActor("log_" + GetDatePrefix());
            //RegisterLogActor(oFileActor);

            return true;
        }

        public string GetDatePrefix()
        {
			return DateTime.Now.ToString("yyyyMMdd_HH_mm_ss");
        }

        //只能运行在主线程
        public void UnInit()
        {

        }

        //只能运行在主线程
        public void SetUserLogPrefix(ILogPrefix roLogPrefix)
        {
            m_oLogPrefix = roLogPrefix;
        }

        //只能运行在主线程
        public bool RegisterLogActor(ILogActor  roLogActor)
        {
            m_oMapLogActor[roLogActor.GetName()] = roLogActor;

            return true;
        }

        public bool UnRegisterLogActor(ILogActor roLogActor)
        {
            m_oMapLogActor.Remove(roLogActor.GetName());

            return true;
        }

        public bool LOG_CRI(string strFormat, params object[] args)
        {
            return Log(ELogLevel.ELL_CRI, (Int32)ELogType.ELType_COMMON, false, strFormat, args);
        }

        public bool LOG_DBG(string strFormat, params object[] args)
        {
            return Log(ELogLevel.ELL_DBG, (Int32)ELogType.ELType_COMMON, false, strFormat, args);
        }

        public bool LOG_WRN(string strFormat, params object[] args)
        {
            return Log(ELogLevel.ELL_WRN, (Int32)ELogType.ELType_COMMON, false, strFormat, args);
        }

        public bool LOG_INF(string strFormat, params object[] args)
        {
            return Log(ELogLevel.ELL_INF, (Int32)ELogType.ELType_COMMON, false, strFormat, args);
        }

        public bool Log(ELogLevel eLogLevel, UInt16 wLogType, bool bDirect, string strFormat, params object[] args)
        {
            uint dwLogLevel = (uint)eLogLevel;
            if ( 0 == (dwLogLevel & m_dwLogLevel) )
            {
                return false;
            }

            string strFileName = string.Empty;
            string strFuncName = string.Empty;
            Int32 nLineNumber = 0;

#if UNITY_EDITOR
            Int32 nLayerReq = bDirect ? 1 : 2;
            StackTrace oStackTrace = new StackTrace(true);
            if( oStackTrace.FrameCount >= nLayerReq )
            {
                StackFrame oStackFrame;
                if( !bDirect )
                {
                    oStackFrame = oStackTrace.GetFrame(2);
                }
                else
                {
                    oStackFrame = oStackTrace.GetFrame(1);
                }

                strFileName = oStackFrame.GetFileName();
                Int32 nIndex = strFileName.LastIndexOf('\\');
                if (nIndex != -1 && nIndex < strFileName.Length-1)
                {
                    strFileName = strFileName.Substring(nIndex + 1);
                }

                strFuncName = oStackFrame.GetMethod().Name;
                nLineNumber = oStackFrame.GetFileLineNumber();
            }
#endif

            return _Log(eLogLevel, wLogType, strFileName, nLineNumber, strFuncName, strFormat, args);
        }

        void _ProcessLog(string strLogMsg)
        {
            List<string> oVecLogMsgs;

            lock(m_oLogLock)
            {
                m_oVecLogMsgs.Add(strLogMsg);

                oVecLogMsgs = m_oVecLogMsgs;
                m_oVecLogMsgs = new List<string>();
            }

            foreach(string strMsg in oVecLogMsgs)
            {
                _ActLog(strMsg);
            }
        }
        
        void _ActLog(string strLogMsg)
        {
            foreach( KeyValuePair<string, ILogActor> oElement in m_oMapLogActor  )
            {
                oElement.Value.Log(strLogMsg);
            }
        }

        void _RecordLog(string strLogMsg)
        {
            lock(m_oLogLock)
            {
                m_oVecLogMsgs.Add(strLogMsg);
            }
        }

        string _GetLogLevelString(ELogLevel eLogLevel)
        {
            string strLogMsg = "";

            switch (eLogLevel)
            {
                case ELogLevel.ELL_DBG:
                    strLogMsg = "[DBG]";
                    break;
                case ELogLevel.ELL_CRI:
                    strLogMsg = "<color=red>[CRI]错误</color>";
                    break;
                case ELogLevel.ELL_INF:
                    strLogMsg = "<color=green>[INF]信息</color>";
                    break;
                case ELogLevel.ELL_WRN:
                    strLogMsg = "<color=yellow>[WRN]警告</color>";
                    break;
                default:
                    break;
            }

            return strLogMsg;
        }

        string _GetLogMsg(string strLogPrefix, string strLogPostfix, string strFormat, params object[] args)
        {
            string strFormatLog = "";
            if (null == args || 0 == args.Length)
            {
                strFormatLog = strFormat;
            }
            else
                strFormatLog = string.Format(strFormat, args);

            string strLogMsg = strLogPrefix + " ： \n" + strFormatLog + "\n" + strLogPostfix;

            return strLogMsg;
        }

        bool _Log(ELogLevel eLogLevel, UInt16 wLogType, string strFileName, Int32 nLine, string strFuncName, string strFormat, params object[] args)
        {
            string strLogLevel = _GetLogLevelString(eLogLevel);

            string strLogPrefix = m_oLogPrefix.GetLogPrefix(strFileName, nLine, strFuncName, strLogLevel);

            string strLogPostfix = m_kLogPostfix.GetLogPostfix();
            string strLogMsg = _GetLogMsg(strLogPrefix, strLogPostfix,strFormat, args );

            if(m_oMainThread.Equals(Thread.CurrentThread))
            {
                _ProcessLog(strLogMsg);
            }
            else
            {
                _RecordLog(strLogMsg);
            }

            return true;
        }

        uint m_dwLogLevel;

        Thread m_oMainThread;

        object m_oLogLock;
        List<string> m_oVecLogMsgs;

        Dictionary<string, ILogActor> m_oMapLogActor;
        ILogPrefix m_oLogPrefix;
        ILogPostfix m_kLogPostfix;
    };
}