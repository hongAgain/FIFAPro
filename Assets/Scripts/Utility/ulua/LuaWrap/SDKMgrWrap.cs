using System;
using System.Collections;
using LuaInterface;

public class SDKMgrWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("Start", Start),
			new LuaMethod("CallSDK", CallSDK),
			new LuaMethod("New", _CreateSDKMgr),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
		};

		LuaScriptMgr.RegisterLib(L, "SDKMgr", typeof(SDKMgr), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateSDKMgr(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			SDKMgr obj = new SDKMgr();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: SDKMgr.New");
		}

		return 0;
	}

	static Type classType = typeof(SDKMgr);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Start(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		SDKMgr.Start();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CallSDK(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			Hashtable arg0 = (Hashtable)LuaScriptMgr.GetNetObject(L, 1, typeof(Hashtable));
			SDKMgr.CallSDK(arg0);
			return 0;
		}
		else if (count == 2)
		{
			Hashtable arg0 = (Hashtable)LuaScriptMgr.GetNetObject(L, 1, typeof(Hashtable));
			OnResponse arg1 = null;
			LuaTypes funcType2 = LuaDLL.lua_type(L, 2);

			if (funcType2 != LuaTypes.LUA_TFUNCTION)
			{
				 arg1 = (OnResponse)LuaScriptMgr.GetNetObject(L, 2, typeof(OnResponse));
			}
			else
			{
				LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 2);
				arg1 = (param0) =>
				{
					int top = func.BeginPCall();
					LuaScriptMgr.PushVarObject(L, param0);
					func.PCall(top, 1);
					func.EndPCall(top);
				};
			}

			SDKMgr.CallSDK(arg0,arg1);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: SDKMgr.CallSDK");
		}

		return 0;
	}
}

