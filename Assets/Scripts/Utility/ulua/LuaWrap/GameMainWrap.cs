using System;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class GameMainWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("New", _CreateGameMain),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("ScriptName", get_ScriptName, set_ScriptName),
			new LuaField("Instance", get_Instance, null),
			new LuaField("DataSystem", get_DataSystem, null),
		};

		LuaScriptMgr.RegisterLib(L, "GameMain", typeof(GameMain), regs, fields, typeof(MonoBehaviour));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateGameMain(IntPtr L)
	{
		LuaDLL.luaL_error(L, "GameMain class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(GameMain);

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
		GameMain obj = (GameMain)o;

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
	static int get_Instance(IntPtr L)
	{
		LuaScriptMgr.Push(L, GameMain.Instance);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_DataSystem(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		GameMain obj = (GameMain)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name DataSystem");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index DataSystem on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.DataSystem);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_ScriptName(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		GameMain obj = (GameMain)o;

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

