using System;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class CoachHelperWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("PlayAnim", PlayAnim),
			new LuaMethod("InitUniform", InitUniform),
			new LuaMethod("FadeOut", FadeOut),
			new LuaMethod("FadeIn", FadeIn),
			new LuaMethod("New", _CreateCoachHelper),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
		};

		LuaScriptMgr.RegisterLib(L, "CoachHelper", typeof(CoachHelper), regs, fields, typeof(MonoBehaviour));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateCoachHelper(IntPtr L)
	{
		LuaDLL.luaL_error(L, "CoachHelper class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(CoachHelper);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PlayAnim(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		CoachHelper obj = (CoachHelper)LuaScriptMgr.GetUnityObjectSelf(L, 1, "CoachHelper");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		string arg1 = LuaScriptMgr.GetLuaString(L, 3);
		obj.PlayAnim(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int InitUniform(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		CoachHelper obj = (CoachHelper)LuaScriptMgr.GetUnityObjectSelf(L, 1, "CoachHelper");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		obj.InitUniform(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int FadeOut(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		CoachHelper obj = (CoachHelper)LuaScriptMgr.GetUnityObjectSelf(L, 1, "CoachHelper");
		obj.FadeOut();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int FadeIn(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		CoachHelper obj = (CoachHelper)LuaScriptMgr.GetUnityObjectSelf(L, 1, "CoachHelper");
		obj.FadeIn();
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

