using System;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class UILuaTweenWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("Begin", Begin),
			new LuaMethod("New", _CreateUILuaTween),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
		};

		LuaScriptMgr.RegisterLib(L, "UILuaTween", typeof(UILuaTween), regs, fields, typeof(UITweener));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateUILuaTween(IntPtr L)
	{
		LuaDLL.luaL_error(L, "UILuaTween class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(UILuaTween);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Begin(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		float arg1 = (float)LuaScriptMgr.GetNumber(L, 2);
		LuaFunction arg2 = LuaScriptMgr.GetLuaFunction(L, 3);
        LuaFunction arg3 = LuaScriptMgr.GetLuaFunction(L, 4);
		UILuaTween o = UILuaTween.Begin(arg0,arg1,arg2,arg3);
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

