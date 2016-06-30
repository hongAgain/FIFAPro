using System;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class RoleHelperWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("ChangeUniform", ChangeUniform),
			new LuaMethod("ChangeUniformImmidiately", ChangeUniformImmidiately),
			new LuaMethod("Play", Play),
			new LuaMethod("New", _CreateRoleHelper),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("ball", get_ball, set_ball),
		};

		LuaScriptMgr.RegisterLib(L, "RoleHelper", typeof(RoleHelper), regs, fields, typeof(MonoBehaviour));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateRoleHelper(IntPtr L)
	{
		LuaDLL.luaL_error(L, "RoleHelper class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(RoleHelper);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ball(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RoleHelper obj = (RoleHelper)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name ball");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index ball on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.ball);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_ball(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		RoleHelper obj = (RoleHelper)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name ball");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index ball on a nil value");
			}
		}

		obj.ball = (GameObject)LuaScriptMgr.GetUnityObject(L, 3, typeof(GameObject));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ChangeUniform(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		RoleHelper obj = (RoleHelper)LuaScriptMgr.GetUnityObjectSelf(L, 1, "RoleHelper");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		obj.ChangeUniform(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ChangeUniformImmidiately(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		RoleHelper obj = (RoleHelper)LuaScriptMgr.GetUnityObjectSelf(L, 1, "RoleHelper");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		obj.ChangeUniformImmidiately(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Play(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		RoleHelper obj = (RoleHelper)LuaScriptMgr.GetUnityObjectSelf(L, 1, "RoleHelper");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		bool arg1 = LuaScriptMgr.GetBoolean(L, 3);
		obj.Play(arg0,arg1);
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

