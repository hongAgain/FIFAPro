using System;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class LuaServerTimeWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("SynServerTime", SynServerTime),
			new LuaMethod("Time19702DateTime", Time19702DateTime),
			new LuaMethod("DateTime2Time1970", DateTime2Time1970),
			new LuaMethod("New", _CreateLuaServerTime),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("TIME1970", get_TIME1970, set_TIME1970),
			new LuaField("Instance", get_Instance, set_Instance),
			new LuaField("Now", get_Now, null),
		};

		LuaScriptMgr.RegisterLib(L, "LuaServerTime", typeof(LuaServerTime), regs, fields, typeof(MonoBehaviour));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateLuaServerTime(IntPtr L)
	{
		LuaDLL.luaL_error(L, "LuaServerTime class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(LuaServerTime);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_TIME1970(IntPtr L)
	{
		LuaScriptMgr.PushValue(L, LuaServerTime.TIME1970);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Instance(IntPtr L)
	{
		LuaScriptMgr.Push(L, LuaServerTime.Instance);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Now(IntPtr L)
	{
		LuaScriptMgr.PushValue(L, LuaServerTime.Now);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_TIME1970(IntPtr L)
	{
		LuaServerTime.TIME1970 = (DateTime)LuaScriptMgr.GetNetObject(L, 3, typeof(DateTime));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Instance(IntPtr L)
	{
		LuaServerTime.Instance = (LuaServerTime)LuaScriptMgr.GetUnityObject(L, 3, typeof(LuaServerTime));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SynServerTime(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		double arg0 = (double)LuaScriptMgr.GetNumber(L, 1);
		LuaServerTime.SynServerTime(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Time19702DateTime(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		double arg0 = (double)LuaScriptMgr.GetNumber(L, 1);
		DateTime o = LuaServerTime.Time19702DateTime(arg0);
		LuaScriptMgr.PushValue(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DateTime2Time1970(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		DateTime arg0 = (DateTime)LuaScriptMgr.GetNetObject(L, 1, typeof(DateTime));
		double o = LuaServerTime.DateTime2Time1970(arg0);
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

