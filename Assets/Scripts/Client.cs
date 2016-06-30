using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Common;
using Common.Tables;
using System.Diagnostics;
using Common.Log;

public class Client : MonoBehaviour
{
    public static Client Instance;
    
    void Awake()
    {
        Instance = this;
        m_kUIRoot3D     = GameObject.FindGameObjectWithTag("UIRoot3D");
        m_kUI3DCamera   = GameObject.FindGameObjectWithTag("3DEffectCamera");
        m_kSkyBoxCamera = GameObject.FindGameObjectWithTag("MainCamera");
        m_kUIRoot = GameObject.FindGameObjectWithTag("UIRoot");
        m_kUIAttach = UIRoot.transform.Find("UIAttach").gameObject;
        m_kUIHUD = UIRoot.transform.Find("HUDPanel").gameObject;
        m_kGameStart = UIAttach.transform.Find("GameStart").gameObject;
        m_kUpdateLabel = m_kGameStart.transform.Find("Label").gameObject.GetComponent<UILabel>();
        m_kProgressBarBG = m_kGameStart.transform.Find("ProgressBarBG").gameObject.GetComponent<UISprite>();
        m_kProgressBarFront = m_kGameStart.transform.Find("ProgressBarFront").gameObject.GetComponent<UISprite>();

        DontDestroyOnLoad(transform.gameObject);
        DontDestroyOnLoad(m_kUIRoot);
        DontDestroyOnLoad(m_kUIRoot3D);
        DontDestroyOnLoad(m_kUI3DCamera);
 //       DontDestroyOnLoad(m_kSkyBoxCamera);
        Application.RegisterLogCallback(HandleLog);

        Application.targetFrameRate = 60;
    }
	void Start ()
	{
        StartCoroutine(InitClient());        
	}

    void Update()
    {
        if (null != m_kLuaMgr)
            m_kLuaMgr.Update();
        LogManager.Instance.LogEnable = m_LogEnable;
        MessageDispatcher.Instance.Update();
    }

    private IEnumerator InitClient()
    {
        LogManager.Instance.Log("Initialize start...");
        gameObject.AddComponent<ResourceManager>();
        gameObject.AddComponent<AudioMgr>();
        gameObject.AddComponent<AvatarManager>();
        m_kScreenLogView = gameObject.AddComponent<ScreenLogView>();
        m_kMsgProc = new MessageProcessor(MessageDispatcher.Instance);
        yield return null;
		InitABRes();
		while (false == m_bDownladFinished)
			yield return null;
		
		LogManager.Instance.Log("m_strURL First:"+m_strURL);
		m_kLuaMgr = new LuaScriptMgr();
        m_kLuaMgr.Start();

        yield return null;
        IEnumerator it = LoadStaticData();
        while (it.MoveNext())
            yield return it.Current;

        GameObject.Destroy(m_kGameStart);
        new GameObject("GameMain", typeof(GameMain));
        yield return null;
    }

    [Conditional(Macro.USE_ASSETBUNDLE)]
    private void InitABRes()
    {
        m_bDownladFinished = false;
        StartCoroutine(DownloadABRes());
    }
    private IEnumerator DownloadABRes()
    {
        m_strURL = Versioned.PathPrefix();

        InitReleaseURL();

        InitEditorURL();
		
		LogManager.Instance.Log("m_strURL Second:"+m_strURL);
		
		m_kLocal.ReadFileData(FileList.SavePath());

        IEnumerator it = DownloadFileList();
        while (it.MoveNext())
        {
            yield return it.Current;
        }
        yield return null;

        var kUpdateList = m_kLocal.Compare(m_kLatest);
        if (kUpdateList.Count > 0)
        {
            it = DownloadAssets(kUpdateList);
            while (it.MoveNext())
            {
                yield return it.Current;
            }
            yield return null;
        }
        yield return null;

        AssetMgr.OnGetRes onDownloadSharedAsset = delegate(AssetBundle assetBundle)
        {
            WindowMgr.mSharedBundle = assetBundle;
            m_kUpdateLabel.trueTypeFont = assetBundle.Load("msyh", typeof(Font)) as Font;
        };
        AssetMgr.LoadAsset("UI/SharedAssets.assetbundle",null,onDownloadSharedAsset,null);
        yield return null;

        m_bDownladFinished = true;

        yield return null;
    }

    private IEnumerator LoadStaticData()
    {
		m_kUpdateLabel.text = "资源加载中";

        yield return null;

        Localization.language = ResourceManager.Instance.LoadText("LanguageVersion");
        TextAsset kLocalization = ResourceManager.Instance.Load("","Localization", ResType.ResType_Text, true) as TextAsset;
        Localization.LoadCSV(kLocalization);
        yield return null;

        LogManager.Instance.Log("Initialize Table Module ...");
        IEnumerator it = TableManager.Instance.InitTables();
        while (it.MoveNext())
        {
            yield return it.Current;
        }
        LogManager.Instance.Log("Initialize Table Module End...");

        ResourceManager.onLoadFinished kCallback = delegate(object[] kArgs)
        {
            LuaScriptMgr.Instance.CallLuaFunction("Config.OnConfigTemplateLoad", kArgs[0] as string, kArgs[1] as string);
        };
        ResourceManager.Instance.LoadAllTables(kCallback);
        yield return null;
    }


    private IEnumerator DownloadFileList()
    {
        while (true)
        {
            WWW www = null;
            var url = m_strURL + FileList.SAVE_FILE_NAME;
            www = new WWW(url);

            yield return www;

            if (string.IsNullOrEmpty(www.error))
            {
                if (www.assetBundle != null)
                {
                    www.assetBundle.Unload(true);
                }

                m_kLatest.FillData(www.text);

                break;
            }
            else
            {
                LogView.Error(string.Format("Download [" + www.url + "] encounter some error. Try again!.\nError: " + www.error));
            }
        }
        yield return null;
    }
    
    private IEnumerator DownloadAssets(List<string> list)
    {
        var allNr = list.Count;
        m_kUpdateLabel.text = "0/" + allNr.ToString();
        m_kProgressBarFront.width = 0;

        while (list.Count > 0)
        {
            var a = list[0];
            list.RemoveAt(0);

            WWW www = null;
			var url = "";
			url = m_strURL + a;
            www = new WWW(url);

            LogView.Log(www.url);
            yield return www;

            if (string.IsNullOrEmpty(www.error))
            {
                if (www.assetBundle != null)
                {
                    int idx = m_kLatest.m_FileDataList[a].File.LastIndexOf('/');
                    if (idx == -1)
                    {
                        idx = m_kLatest.m_FileDataList[a].File.LastIndexOf('\\');
                    }
                    if (idx != -1)
                    {
                        string fullPath = FileList.SavePath() + m_kLatest.m_FileDataList[a].File.Substring(0, idx);

                        if (Directory.Exists(fullPath) == false)
                        {
                            Directory.CreateDirectory(fullPath);
                        }
                    }
                    File.WriteAllBytes(FileList.SavePath() + m_kLatest.m_FileDataList[a].File, www.bytes);

                    www.assetBundle.Unload(true);

                    m_kLocal.Update(a, m_kLatest.m_FileDataList[a]);
                }

                var downloaded = allNr - list.Count;
                m_kUpdateLabel.text = downloaded.ToString() + "/" + allNr.ToString();
                m_kProgressBarFront.width = m_kProgressBarBG.width * downloaded / allNr;
            }
            else
            {
                LogView.Error(string.Format("Download [" + www.url + "] encounter some error. Try again!.\nError: " + www.error));
                list.Insert(0, a);
            }
        }
    }

    private void HandleLog(string condition, string stackTrace, LogType type)
    {
        if (type == LogType.Exception || type == LogType.Error )
        {
            string strMsg = condition + stackTrace;
            LogManager.Instance.LogError(strMsg);
            //LogManager.Instance.LogError(condition);
            //LogManager.Instance.LogError(stackTrace);
            //CloseLog();

            //if (m_bShowException)
            //    StatisticManager.Instance.ShowException(condition, stackTrace);

        }
    }

    [Conditional(Macro.RELEASE)]
    protected void InitReleaseURL()
    {
        m_strURL = "http://172.21.186.68" + Versioned.PlatformPath;
    }

    [Conditional("UNITY_EDITOR"), Conditional("UNITY_IPHONE")]
    protected void InitEditorURL()
    {
        m_strURL = "file://" + Versioned.PathPrefix();
    }


    public GameObject UIRoot
    {
        get { return m_kUIRoot; }
    }

    public GameObject UIRoot3D
    {
        get { return m_kUIRoot3D; }
    }

    public GameObject UIAttach
    {
        get { return m_kUIAttach; }
    }

    public GameObject UIHUD
    {
        get { return m_kUIHUD; }
    }

    public GameObject UI3DCamera
    {
        get { return m_kUI3DCamera; }
    }

    public GameObject SkyBoxCamera
    {
        get { return m_kSkyBoxCamera;}
    }

    public ScreenLogView ScreenLogView
    {
        get { return m_kScreenLogView; }
    }

    public bool ScreenLogEnable
    {
        get { return m_ScreenLogEnable; }
        set { m_ScreenLogEnable = value; }
    }

    private bool m_bDownladFinished = true;
    private LuaScriptMgr m_kLuaMgr = null;
    private FileList m_kLocal = new FileList();
    private FileList m_kLatest = new FileList();
    private string m_strURL;

    private ScreenLogView m_kScreenLogView;
    private GameObject m_kUIRoot;
    private GameObject m_kUIRoot3D;
    private GameObject m_kUIAttach;
    private GameObject m_kUIHUD;
    private GameObject m_kUI3DCamera;
    private GameObject m_kSkyBoxCamera;
    private GameObject m_kGameStart;
    private MessageProcessor m_kMsgProc = null;
    private UILabel m_kUpdateLabel;
    private UISprite m_kProgressBarBG;
    private UISprite m_kProgressBarFront;
    [SerializeField]private bool m_ScreenLogEnable = true;
    [SerializeField]private bool m_LogEnable = true;
}

public class Macro
{
    public const string RELEASE = "RELEASE";
    public const string USE_ASSETBUNDLE = "USE_ASSETBUNDLE";
    public const string FIFA_CLIENT = "FIFA_CLIENT";
}