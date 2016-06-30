using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIHintScript : MonoBehaviour 
{
	private static int uiHintInstanceKeyCount = 0;

	private string luaScriptName = "UIHint";

	private string scriptModuleName = "";
	private ScriptInstaller scriptInstaller = new ScriptInstaller();

	private int hintInstanceKey = 0;

	private LuaInterface.LuaTable luaParamTable;
	public List<HintParam> _RelatedHints = new List<HintParam> ();
	
	[System.Serializable]
	public class HintParam
	{
		public string hintType = "";
		public List<string> hintParams = new List<string> ();
	}

	// Use this for initialization
	void Start ()
	{
		//load lua script
		scriptModuleName = Util.GetLuaScriptModuleName(luaScriptName);
		scriptInstaller.Install(LuaScriptMgr.Instance, Util.GetLuaFilePath("UILua/" + luaScriptName));

		//get a null table to prepare params
		object[] emptyTable = LuaScriptMgr.Instance.CallLuaFunction (scriptModuleName + "InitializeTable");
		luaParamTable = emptyTable[0] as LuaInterface.LuaTable;
		for (int i = 0; i < _RelatedHints.Count; i++)
		{
		    var subLuaTable = LuaScriptMgr.Instance.CallLuaFunction(scriptModuleName + "InitializeTable")[0];
            LuaInterface.LuaTable paramTable = subLuaTable as LuaInterface.LuaTable;
			for(int j=0; j < _RelatedHints[i].hintParams.Count; j++)
			{
				paramTable[j+1] = _RelatedHints[i].hintParams[j] as object;
			}
//			Debug.LogError(_RelatedHints[i].hintType);
			luaParamTable.Set(_RelatedHints[i].hintType,paramTable as object);
		}
		hintInstanceKey = ++uiHintInstanceKeyCount;

		//send back the prepared luatable
		LuaScriptMgr.Instance.CallLuaFunction (scriptModuleName + "Start", gameObject, hintInstanceKey, luaParamTable);
	}
		
	void OnEnable()
	{
		LuaScriptMgr.Instance.CallLuaFunction(scriptModuleName + "OnEnable", hintInstanceKey);
	}

	void OnDestroy()
	{
		LuaScriptMgr.Instance.CallLuaFunction(scriptModuleName + "OnDestroy", hintInstanceKey);
	}
}
