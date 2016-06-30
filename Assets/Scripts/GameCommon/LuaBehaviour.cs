using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LuaBehaviour : MonoBehaviour
{
    public string luaScriptName = "";
	public List<string> luaScriptParams = new List<string> ();

    private bool mStarted = false;
	// Use this for initialization
	void Start ()
	{
	    if (mStarted == false)
        {
            mStarted = true;
            new ScriptInstaller().Install(LuaScriptMgr.Instance, "UILua/" + luaScriptName);
            if (luaScriptParams.Count > 0)
            {
                //			object[] objParams = luaScriptParams.ToArray ();
                LuaScriptMgr.Instance.CallLuaFunction(luaScriptName + ".Start", gameObject, luaScriptParams.ToArray());
            }
            else
                LuaScriptMgr.Instance.CallLuaFunction(luaScriptName + ".Start", gameObject);
        }
	}
	
	// Update is called once per frame
	void Update ()
	{
	    LuaScriptMgr.Instance.CallLuaFunction(luaScriptName + ".Update", Time.deltaTime);
	}

    void OnDestroy()
    {
		LuaScriptMgr.Instance.CallLuaFunction(luaScriptName + ".OnDestroy", gameObject);
	}

    void OnEnable()
    {
        if (mStarted)
        {
            LuaScriptMgr.Instance.CallLuaFunction(luaScriptName + ".OnEnable", gameObject);
        }
    }

    void OnDisable()
    {
        LuaScriptMgr.Instance.CallLuaFunction(luaScriptName + ".OnDisable", gameObject);
    }
}