using System;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class DelayDealWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("EnqueueEvent", EnqueueEvent),
			new LuaMethod("New", _CreateDelayDeal),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
		};

		LuaScriptMgr.RegisterLib(L, "DelayDeal", typeof(DelayDeal), regs, fields, typeof(MonoBehaviour));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateDelayDeal(IntPtr L)
	{
		LuaDLL.luaL_error(L, "DelayDeal class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(DelayDeal);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int EnqueueEvent(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		LuaFunction arg0 = LuaScriptMgr.GetLuaFunction(L, 1);
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 2);
		DelayDeal.EnqueueEvent(arg0,arg1);
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

