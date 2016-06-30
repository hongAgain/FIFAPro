using System;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class NetWorkHandlerWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("Request", Request),
			new LuaMethod("New", _CreateNetWorkHandler),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
		};

		LuaScriptMgr.RegisterLib(L, "NetWorkHandler", typeof(NetWorkHandler), regs, fields, typeof(MonoBehaviour));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateNetWorkHandler(IntPtr L)
	{
		LuaDLL.luaL_error(L, "NetWorkHandler class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(NetWorkHandler);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Request(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 3 && LuaScriptMgr.CheckTypes(L, 1, typeof(NetWorkHandler), typeof(string), typeof(string)))
		{
			NetWorkHandler obj = (NetWorkHandler)LuaScriptMgr.GetUnityObjectSelf(L, 1, "NetWorkHandler");
			string arg0 = LuaScriptMgr.GetString(L, 2);
			string arg1 = LuaScriptMgr.GetString(L, 3);
			obj.Request(arg0,arg1);
			return 0;
		}
		else if (count == 3 && LuaScriptMgr.CheckTypes(L, 1, typeof(NetWorkHandler), typeof(string), typeof(OnRequestResp)))
		{
			NetWorkHandler obj = (NetWorkHandler)LuaScriptMgr.GetUnityObjectSelf(L, 1, "NetWorkHandler");
			string arg0 = LuaScriptMgr.GetString(L, 2);
			OnRequestResp arg1 = null;
			LuaTypes funcType3 = LuaDLL.lua_type(L, 3);

			if (funcType3 != LuaTypes.LUA_TFUNCTION)
			{
				 arg1 = (OnRequestResp)LuaScriptMgr.GetLuaObject(L, 3);
			}
			else
			{
				LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 3);
				arg1 = (param0, param1) =>
				{
					int top = func.BeginPCall();
					LuaScriptMgr.Push(L, param0);
					LuaScriptMgr.PushObject(L, param1);
					func.PCall(top, 2);
					func.EndPCall(top);
				};
			}

			obj.Request(arg0,arg1);
			return 0;
		}
		else if (count == 5 && LuaScriptMgr.CheckTypes(L, 1, typeof(NetWorkHandler), typeof(string), typeof(Dictionary<string,string>), typeof(string), typeof(string)))
		{
			NetWorkHandler obj = (NetWorkHandler)LuaScriptMgr.GetUnityObjectSelf(L, 1, "NetWorkHandler");
			string arg0 = LuaScriptMgr.GetString(L, 2);
			Dictionary<string,string> arg1 = (Dictionary<string,string>)LuaScriptMgr.GetLuaObject(L, 3);
			string arg2 = LuaScriptMgr.GetString(L, 4);
			string arg3 = LuaScriptMgr.GetString(L, 5);
			obj.Request(arg0,arg1,arg2,arg3);
			return 0;
		}
		else if (count == 5 && LuaScriptMgr.CheckTypes(L, 1, typeof(NetWorkHandler), typeof(string), typeof(Dictionary<string,string>), typeof(string), typeof(OnRequestResp)))
		{
			NetWorkHandler obj = (NetWorkHandler)LuaScriptMgr.GetUnityObjectSelf(L, 1, "NetWorkHandler");
			string arg0 = LuaScriptMgr.GetString(L, 2);
			Dictionary<string,string> arg1 = (Dictionary<string,string>)LuaScriptMgr.GetLuaObject(L, 3);
			string arg2 = LuaScriptMgr.GetString(L, 4);
			OnRequestResp arg3 = null;
			LuaTypes funcType5 = LuaDLL.lua_type(L, 5);

			if (funcType5 != LuaTypes.LUA_TFUNCTION)
			{
				 arg3 = (OnRequestResp)LuaScriptMgr.GetLuaObject(L, 5);
			}
			else
			{
				LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 5);
				arg3 = (param0, param1) =>
				{
					int top = func.BeginPCall();
					LuaScriptMgr.Push(L, param0);
					LuaScriptMgr.PushObject(L, param1);
					func.PCall(top, 2);
					func.EndPCall(top);
				};
			}

			obj.Request(arg0,arg1,arg2,arg3);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: NetWorkHandler.Request");
		}

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

