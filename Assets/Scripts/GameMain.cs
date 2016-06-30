using UnityEngine;
using System.Collections.Generic;
using Common.Log;
using System.Collections;
using Common;
using Common.Tables;

public class GameMain : MonoBehaviour
{
    public static GameMain Instance
    {
        get
        {
            return gameMain;
        }
    }
    public string ScriptName = "Game/GameMainScript";

    public DataSystem DataSystem
    {
        get { return dataSystem; }
    }

    void Awake()
    {
        if (gameMain != null)
        {
            Destroy(gameObject);
        }
        else
        {
            gameMain = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {
        StartCoroutine(InitGameMain());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
        }
    }

    void OnDestroy()
    {
        if (dataSystem != null)
        {
            dataSystem.Release();
        }
    }

    private IEnumerator InitGameMain()
    {

        gameObject.AddComponent<LuaTimer>();
        gameObject.AddComponent<LuaServerTime>();
//        gameObject.AddComponent<GMCommond>();
        gameObject.AddComponent<FPSCounter>();
        gameObject.AddComponent<ListNetWorkHandler>();
        gameObject.AddComponent<UIEventMgr>();
        gameObject.AddComponent<SceneMgr>();
        gameObject.AddComponent<TestinMgr>();
        gameObject.AddComponent<DataSystem>();
        dataSystem = gameObject.GetComponent<DataSystem>();
        yield return null;
        scriptInstaller.Install(LuaScriptMgr.Instance, Util.GetLuaFilePath(ScriptName));
        LuaScriptMgr.Instance.CallLuaFunction(Util.GetLuaScriptModuleName(ScriptName) + "OnStart");
        dataSystem.Init();
        yield return null;
        SDKMgr.Start();
        yield return null;
    }

    ScriptInstaller scriptInstaller = new ScriptInstaller();
    DataSystem dataSystem = null;

    static private GameMain gameMain = null;
}