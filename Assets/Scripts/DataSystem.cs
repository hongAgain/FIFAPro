using UnityEngine;
using System.Collections.Generic;

public class DataSystem: MonoBehaviour{    public string ScriptName = "Game/DataSystemScript";
    public ListNetWorkHandler netWorkHandler = null;
    public void Awake()
    {
        Instance = this;
    }
    public void Init()
    {
        if (null == scriptInstaller)
            scriptInstaller = new ScriptInstaller();
        scriptInstaller.Install(LuaScriptMgr.Instance, Util.GetLuaFilePath(ScriptName));
        LuaScriptMgr.Instance.CallLuaFunction(Util.GetLuaScriptModuleName(ScriptName) + "OnInit");

        scriptInstaller = new ScriptInstaller();
        scriptInstaller.Install(LuaScriptMgr.Instance, Util.GetLuaFilePath("Game/ModuleMgr"));
        LuaScriptMgr.Instance.CallLuaFunction(Util.GetLuaScriptModuleName("Game/ModuleMgr") + "OnInit");

        netWorkHandler = gameObject.GetComponent<ListNetWorkHandler>();
        netWorkHandler.SetHandler(OnNetData);
        netWorkHandler.SetOnNoReq(OnNoReq);
    }	    public void Release()
    {
        LuaScriptMgr.Instance.CallLuaFunction(Util.GetLuaScriptModuleName("Game/ModuleMgr") + "OnRelease");			}

    public void OnNetData(WWW www)
    {
		CheckResponseHeader (www);
        LuaScriptMgr.Instance.CallLuaFunction(Util.GetLuaScriptModuleName(ScriptName) + "OnHttpResp", www.error, www.text);    }

    public void DoPost(string url)
    {
		netWorkHandler.Request(url,wwwHeader);
    }    public string GetUrlPrefixAndRegion()
    {
        object[] objs = LuaScriptMgr.Instance.CallLuaFunction(Util.GetLuaScriptModuleName(ScriptName) + "GetUrlPrefixAndRegion");
        return (string)objs[0];
    }	ScriptInstaller scriptInstaller = null;

    void OnNoReq()
    {
        LuaScriptMgr.Instance.CallLuaFunction(Util.GetLuaScriptModuleName(ScriptName) + "OnNoReq");
    }

	Dictionary<string,string> wwwHeader = new Dictionary<string, string> ();
	string headerCookie = "";
	private void CheckResponseHeader(WWW www)
	{
		if (www.responseHeaders.ContainsKey ("SET-COOKIE"))
		{
			headerCookie = www.responseHeaders ["SET-COOKIE"];
			
//			Debug.LogError("test--------"+ headerCookie);
			if(headerCookie.Contains(@"; "))
			{
				headerCookie = headerCookie.Substring(0,headerCookie.IndexOf(@"; "));
			}
//			Debug.LogError("test--------"+ headerCookie);
			wwwHeader ["Cookie"] = headerCookie;
		}
	}

    public static DataSystem Instance = null;}