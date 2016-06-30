using System;
using LuaInterface;

public class Common_Log_LogManagerWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("UnInit", UnInit),
			new LuaMethod("Log", Log),
			new LuaMethod("LogWarning", LogWarning),
			new LuaMethod("LogError", LogError),
			new LuaMethod("RedLog", RedLog),
			new LuaMethod("YellowLog", YellowLog),
			new LuaMethod("GreenLog", GreenLog),
			new LuaMethod("BlackLog", BlackLog),
			new LuaMethod("ColorLog", ColorLog),
			new LuaMethod("New", _CreateCommon_Log_LogManager),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("Instance", get_Instance, null),
			new LuaField("LogEnable", get_LogEnable, set_LogEnable),
			new LuaField("WriteFileEnable", get_WriteFileEnable, set_WriteFileEnable),
		};

		LuaScriptMgr.RegisterLib(L, "Common.Log.LogManager", typeof(Common.Log.LogManager), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateCommon_Log_LogManager(IntPtr L)
	{
		LuaDLL.luaL_error(L, "Common.Log.LogManager class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(Common.Log.LogManager);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Instance(IntPtr L)
	{
		LuaScriptMgr.PushObject(L, Common.Log.LogManager.Instance);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_LogEnable(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Common.Log.LogManager obj = (Common.Log.LogManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name LogEnable");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index LogEnable on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.LogEnable);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_WriteFileEnable(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Common.Log.LogManager obj = (Common.Log.LogManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name WriteFileEnable");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index WriteFileEnable on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.WriteFileEnable);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_LogEnable(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Common.Log.LogManager obj = (Common.Log.LogManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name LogEnable");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index LogEnable on a nil value");
			}
		}

		obj.LogEnable = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_WriteFileEnable(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Common.Log.LogManager obj = (Common.Log.LogManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name WriteFileEnable");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index WriteFileEnable on a nil value");
			}
		}

		obj.WriteFileEnable = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int UnInit(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Common.Log.LogManager obj = (Common.Log.LogManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "Common.Log.LogManager");
		obj.UnInit();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Log(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2)
		{
			Common.Log.LogManager obj = (Common.Log.LogManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "Common.Log.LogManager");
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			obj.Log(arg0);
			return 0;
		}
		else if (LuaScriptMgr.CheckTypes(L, 1, typeof(Common.Log.LogManager), typeof(string)) && LuaScriptMgr.CheckParamsType(L, typeof(object), 3, count - 2))
		{
			Common.Log.LogManager obj = (Common.Log.LogManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "Common.Log.LogManager");
			string arg0 = LuaScriptMgr.GetString(L, 2);
			object[] objs1 = LuaScriptMgr.GetParamsObject(L, 3, count - 2);
			obj.Log(arg0,objs1);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Common.Log.LogManager.Log");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LogWarning(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2)
		{
			Common.Log.LogManager obj = (Common.Log.LogManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "Common.Log.LogManager");
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			obj.LogWarning(arg0);
			return 0;
		}
		else if (LuaScriptMgr.CheckTypes(L, 1, typeof(Common.Log.LogManager), typeof(string)) && LuaScriptMgr.CheckParamsType(L, typeof(object), 3, count - 2))
		{
			Common.Log.LogManager obj = (Common.Log.LogManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "Common.Log.LogManager");
			string arg0 = LuaScriptMgr.GetString(L, 2);
			object[] objs1 = LuaScriptMgr.GetParamsObject(L, 3, count - 2);
			obj.LogWarning(arg0,objs1);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Common.Log.LogManager.LogWarning");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LogError(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2)
		{
			Common.Log.LogManager obj = (Common.Log.LogManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "Common.Log.LogManager");
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			obj.LogError(arg0);
			return 0;
		}
		else if (LuaScriptMgr.CheckTypes(L, 1, typeof(Common.Log.LogManager), typeof(string)) && LuaScriptMgr.CheckParamsType(L, typeof(object), 3, count - 2))
		{
			Common.Log.LogManager obj = (Common.Log.LogManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "Common.Log.LogManager");
			string arg0 = LuaScriptMgr.GetString(L, 2);
			object[] objs1 = LuaScriptMgr.GetParamsObject(L, 3, count - 2);
			obj.LogError(arg0,objs1);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Common.Log.LogManager.LogError");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RedLog(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2)
		{
			Common.Log.LogManager obj = (Common.Log.LogManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "Common.Log.LogManager");
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			obj.RedLog(arg0);
			return 0;
		}
		else if (LuaScriptMgr.CheckTypes(L, 1, typeof(Common.Log.LogManager), typeof(string)) && LuaScriptMgr.CheckParamsType(L, typeof(object), 3, count - 2))
		{
			Common.Log.LogManager obj = (Common.Log.LogManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "Common.Log.LogManager");
			string arg0 = LuaScriptMgr.GetString(L, 2);
			object[] objs1 = LuaScriptMgr.GetParamsObject(L, 3, count - 2);
			obj.RedLog(arg0,objs1);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Common.Log.LogManager.RedLog");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int YellowLog(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2)
		{
			Common.Log.LogManager obj = (Common.Log.LogManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "Common.Log.LogManager");
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			obj.YellowLog(arg0);
			return 0;
		}
		else if (LuaScriptMgr.CheckTypes(L, 1, typeof(Common.Log.LogManager), typeof(string)) && LuaScriptMgr.CheckParamsType(L, typeof(object), 3, count - 2))
		{
			Common.Log.LogManager obj = (Common.Log.LogManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "Common.Log.LogManager");
			string arg0 = LuaScriptMgr.GetString(L, 2);
			object[] objs1 = LuaScriptMgr.GetParamsObject(L, 3, count - 2);
			obj.YellowLog(arg0,objs1);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Common.Log.LogManager.YellowLog");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GreenLog(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2)
		{
			Common.Log.LogManager obj = (Common.Log.LogManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "Common.Log.LogManager");
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			obj.GreenLog(arg0);
			return 0;
		}
		else if (LuaScriptMgr.CheckTypes(L, 1, typeof(Common.Log.LogManager), typeof(string)) && LuaScriptMgr.CheckParamsType(L, typeof(object), 3, count - 2))
		{
			Common.Log.LogManager obj = (Common.Log.LogManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "Common.Log.LogManager");
			string arg0 = LuaScriptMgr.GetString(L, 2);
			object[] objs1 = LuaScriptMgr.GetParamsObject(L, 3, count - 2);
			obj.GreenLog(arg0,objs1);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Common.Log.LogManager.GreenLog");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int BlackLog(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2)
		{
			Common.Log.LogManager obj = (Common.Log.LogManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "Common.Log.LogManager");
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			obj.BlackLog(arg0);
			return 0;
		}
		else if (LuaScriptMgr.CheckTypes(L, 1, typeof(Common.Log.LogManager), typeof(string)) && LuaScriptMgr.CheckParamsType(L, typeof(object), 3, count - 2))
		{
			Common.Log.LogManager obj = (Common.Log.LogManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "Common.Log.LogManager");
			string arg0 = LuaScriptMgr.GetString(L, 2);
			object[] objs1 = LuaScriptMgr.GetParamsObject(L, 3, count - 2);
			obj.BlackLog(arg0,objs1);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Common.Log.LogManager.BlackLog");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ColorLog(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);
		Common.Log.LogManager obj = (Common.Log.LogManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "Common.Log.LogManager");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		string arg1 = LuaScriptMgr.GetLuaString(L, 3);
		object[] objs2 = LuaScriptMgr.GetParamsObject(L, 4, count - 3);
		obj.ColorLog(arg0,arg1,objs2);
		return 0;
	}
}

