//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using Common;
//using Common.Tables;
//using System.Diagnostics;
//using System;
//using Common.Log;

//public class GameStart : MonoBehaviour
//{
//    public GameObject[] dontDestroyArr;
//    public UILabel updateLabel;
//    public UISprite progressBarBG;
//    public UISprite progressBarFront;
   
//    void Start ()
//    {
//        StartCoroutine(InitClient());        
//    }

//    private IEnumerator InitClient()
//    {
//        foreach (GameObject go in dontDestroyArr)
//        {
//            DontDestroyOnLoad(go);
//        }
//        yield return null;
//        InitABRes();
//        while (false == m_bDownladFinished)
//            yield return null;

//        yield return null;
//        IEnumerator it = LoadStaticData();
//        while (it.MoveNext())
//        {
//            yield return it.Current;
//        }
//        yield return null;
//    }

//    [Conditional(Macro.USE_ASSETBUNDLE)]
//    private void InitABRes()
//    {
//        m_bDownladFinished = false;
//        StartCoroutine(DownloadABRes());
//    }
//    private IEnumerator DownloadABRes()
//    {
//        m_strURL = Versioned.PathPrefix();

//        InitReleaseURL();

//        InitEditorURL();

//        m_kLocal.ReadFileData(FileList.SavePath());

//        IEnumerator it = DownloadFileList();
//        while (it.MoveNext())
//        {
//            yield return it.Current;
//        }
//        yield return null;

//        var kUpdateList = m_kLocal.Compare(m_kLatest);
//        if (kUpdateList.Count > 0)
//        {
//            it = DownloadAssets(kUpdateList);
//            while (it.MoveNext())
//            {
//                yield return it.Current;
//            }
//            yield return null;
//        }
//        yield return null;

//        AssetMgr.OnGetRes onDownloadSharedAsset = delegate(AssetBundle assetBundle)
//        {
//            WindowMgr.mSharedBundle = assetBundle;
//            updateLabel.trueTypeFont = assetBundle.Load("msyh", typeof(Font)) as Font;
//        };
//        AssetMgr.LoadAsset("UI/SharedAssets.assetbundle", onDownloadSharedAsset);
//        yield return null;

//        m_bDownladFinished = true;

//        yield return null;
//    }

//    private IEnumerator LoadStaticData()
//    {
//        dontDestroyArr[0].AddComponent<ResourceManager>();
//        updateLabel.text = "资源加载中";
//        m_kLuaMgr = new LuaScriptMgr();
//        m_kLuaMgr.Start();

//        yield return null;

//        Localization.language = ResourceManager.Instance.LoadText("LanguageVersion");
//        TextAsset kLocalization = ResourceManager.Instance.Load("","Localization", ResType.ResType_Text, true) as TextAsset;
//        Localization.LoadCSV(kLocalization);
//        yield return null;

//        LogManager.Instance.Log("Initialize Table Module ...");
//        IEnumerator it = TableManager.Instance.InitTables();
//        while (it.MoveNext())
//        {
//            yield return it.Current;
//        }
//        LogManager.Instance.Log("Initialize Table Module End...");

//        if (LoadAssetWay.DirectReading())
//        {
//            foreach (var txt in Resources.LoadAll<TextAsset>("Tables/"))
//            {
//                LuaScriptMgr.Instance.CallLuaFunction("Config.OnConfigTemplateLoad", txt.name, txt.text);
//            }
//            Resources.UnloadUnusedAssets();
//            yield return null;
//        }
//        else
//        {
//            AssetHelper.GetAsset cb3 = delegate(AssetBundle ab)
//            {
//                foreach (var obj in ab.LoadAll())
//                {
//                    var txt = obj as TextAsset;
//                    var name = txt.name.Replace(".json", "");
//                    LuaScriptMgr.Instance.CallLuaFunction("Config.OnConfigTemplateLoad", name, txt.text);
//                }
//            };
//            AssetHelper.LoadABImmediate("StaticData.assetbundle", cb3);
//            yield return null;
//        }

//        Destroy(gameObject);
//        dontDestroyArr[0].AddComponent<GameMain>();
//       // new GameObject("GameMain", typeof(GameMain));
//        yield return null;
//    }


//    private IEnumerator DownloadFileList()
//    {
//        while (true)
//        {
//            WWW www = null;
//            var url = m_strURL + FileList.SAVE_FILE_NAME;
//            www = new WWW(url);

//            yield return www;

//            if (String.IsNullOrEmpty(www.error))
//            {
//                if (www.assetBundle != null)
//                {
//                    www.assetBundle.Unload(true);
//                }

//                m_kLatest.FillData(www.text);

//                break;
//            }
//            else
//            {
//                LogView.Error(String.Format("Download [" + www.url + "] encounter some error. Try again!.\nError: " + www.error));
//            }
//        }
//        yield return null;
//    }
    
//    private IEnumerator DownloadAssets(List<String> list)
//    {
//        var allNr = list.Count;
//        updateLabel.text = "0/" + allNr.ToString();
//        progressBarFront.width = 0;

//        while (list.Count > 0)
//        {
//            var a = list[0];
//            list.RemoveAt(0);

//            WWW www = null;
//            var url = "";
//            url = m_strURL + a;
//            www = new WWW(url);

//            LogView.Log(www.url);
//            yield return www;

//            if (String.IsNullOrEmpty(www.error))
//            {
//                if (www.assetBundle != null)
//                {
//                    int idx = m_kLatest.m_FileDataList[a].File.LastIndexOf('/');
//                    if (idx == -1)
//                    {
//                        idx = m_kLatest.m_FileDataList[a].File.LastIndexOf('\\');
//                    }
//                    if (idx != -1)
//                    {
//                        String fullPath = FileList.SavePath() + m_kLatest.m_FileDataList[a].File.Substring(0, idx);

//                        if (Directory.Exists(fullPath) == false)
//                        {
//                            Directory.CreateDirectory(fullPath);
//                        }
//                    }
//                    File.WriteAllBytes(FileList.SavePath() + m_kLatest.m_FileDataList[a].File, www.bytes);

//                    www.assetBundle.Unload(true);

//                    m_kLocal.Update(a, m_kLatest.m_FileDataList[a]);
//                }

//                var downloaded = allNr - list.Count;
//                updateLabel.text = downloaded.ToString() + "/" + allNr.ToString();
//                progressBarFront.width = progressBarBG.width * downloaded / allNr;
//            }
//            else
//            {
//                LogView.Error(String.Format("Download [" + www.url + "] encounter some error. Try again!.\nError: " + www.error));
//                list.Insert(0, a);
//            }
//        }
//    }
    
//    [Conditional(Macro.RELEASE)]
//    protected void InitReleaseURL()
//    {
//        m_strURL = "http://172.21.186.68" + Versioned.PlatformPath;
//    }

//    [Conditional("UNITY_EDITOR"), Conditional("UNITY_IPHONE")]
//    protected void InitEditorURL()
//    {
//        m_strURL = "file://" + Versioned.PathPrefix();
//    }

//    private bool m_bDownladFinished = true;
//    private LuaScriptMgr m_kLuaMgr = null;
//    private FileList m_kLocal = new FileList();
//    private FileList m_kLatest = new FileList();
//    private String m_strURL;
//}

////public class Macro
////{
////    public const string RELEASE         = "RELEASE";
////    public const string USE_ASSETBUNDLE = "USE_ASSETBUNDLE";
////}