using System;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class UIScoutAnimWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("BeginAnimate", BeginAnimate),
			new LuaMethod("New", _CreateUIScoutAnim),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("delay", get_delay, set_delay),
		};

		LuaScriptMgr.RegisterLib(L, "UIScoutAnim", typeof(UIScoutAnim), regs, fields, typeof(MonoBehaviour));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateUIScoutAnim(IntPtr L)
	{
		LuaDLL.luaL_error(L, "UIScoutAnim class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(UIScoutAnim);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_delay(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UIScoutAnim obj = (UIScoutAnim)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name delay");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index delay on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.delay);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_delay(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UIScoutAnim obj = (UIScoutAnim)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name delay");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index delay on a nil value");
			}
		}

		obj.delay = (float)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int BeginAnimate(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 6);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		LuaTable arg2 = LuaScriptMgr.GetLuaTable(L, 3);
		LuaTable arg3 = LuaScriptMgr.GetLuaTable(L, 4);
		LuaFunction arg4 = LuaScriptMgr.GetLuaFunction(L, 5);
		LuaFunction arg5 = LuaScriptMgr.GetLuaFunction(L, 6);
		UIScoutAnim.BeginAnimate(arg0,arg1,arg2,arg3,arg4,arg5);
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

