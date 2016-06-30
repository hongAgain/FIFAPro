using System;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class DataSystemWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("Awake", Awake),
			new LuaMethod("Init", Init),
			new LuaMethod("Release", Release),
			new LuaMethod("OnNetData", OnNetData),
			new LuaMethod("DoPost", DoPost),
			new LuaMethod("GetUrlPrefixAndRegion", GetUrlPrefixAndRegion),
			new LuaMethod("New", _CreateDataSystem),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("ScriptName", get_ScriptName, set_ScriptName),
			new LuaField("netWorkHandler", get_netWorkHandler, set_netWorkHandler),
			new LuaField("Instance", get_Instance, set_Instance),
		};

		LuaScriptMgr.RegisterLib(L, "DataSystem", typeof(DataSystem), regs, fields, typeof(MonoBehaviour));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateDataSystem(IntPtr L)
	{
		LuaDLL.luaL_error(L, "DataSystem class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(DataSystem);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ScriptName(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		DataSystem obj = (DataSystem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name ScriptName");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index ScriptName on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.ScriptName);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_netWorkHandler(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		DataSystem obj = (DataSystem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name netWorkHandler");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index netWorkHandler on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.netWorkHandler);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Instance(IntPtr L)
	{
		LuaScriptMgr.Push(L, DataSystem.Instance);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_ScriptName(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		DataSystem obj = (DataSystem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name ScriptName");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index ScriptName on a nil value");
			}
		}

		obj.ScriptName = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_netWorkHandler(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		DataSystem obj = (DataSystem)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name netWorkHandler");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index netWorkHandler on a nil value");
			}
		}

		obj.netWorkHandler = (ListNetWorkHandler)LuaScriptMgr.GetUnityObject(L, 3, typeof(ListNetWorkHandler));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Instance(IntPtr L)
	{
		DataSystem.Instance = (DataSystem)LuaScriptMgr.GetUnityObject(L, 3, typeof(DataSystem));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Awake(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		DataSystem obj = (DataSystem)LuaScriptMgr.GetUnityObjectSelf(L, 1, "DataSystem");
		obj.Awake();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Init(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		DataSystem obj = (DataSystem)LuaScriptMgr.GetUnityObjectSelf(L, 1, "DataSystem");
		obj.Init();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Release(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		DataSystem obj = (DataSystem)LuaScriptMgr.GetUnityObjectSelf(L, 1, "DataSystem");
		obj.Release();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnNetData(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		DataSystem obj = (DataSystem)LuaScriptMgr.GetUnityObjectSelf(L, 1, "DataSystem");
		WWW arg0 = (WWW)LuaScriptMgr.GetNetObject(L, 2, typeof(WWW));
		obj.OnNetData(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DoPost(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		DataSystem obj = (DataSystem)LuaScriptMgr.GetUnityObjectSelf(L, 1, "DataSystem");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		obj.DoPost(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetUrlPrefixAndRegion(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		DataSystem obj = (DataSystem)LuaScriptMgr.GetUnityObjectSelf(L, 1, "DataSystem");
		string o = obj.GetUrlPrefixAndRegion();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Lua_Eq(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Object arg0 = LuaScriptMgr.GetLuaObject(L, 1) as Object;
		Object arg1 = LuaScriptMgr.GetLuaObject(L, 2) as Object;
		bool o = arg0 == arg1;
		LuaScriptMgr.Push(L, o);
		return 1;
	}
}

