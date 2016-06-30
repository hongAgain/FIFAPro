using System;
using LuaInterface;

public class Common_Tables_TableWordFilterWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("Init", Init),
			new LuaMethod("Clear", Clear),
			new LuaMethod("FilterText", FilterText),
			new LuaMethod("ParseContent", ParseContent),
			new LuaMethod("New", _CreateCommon_Tables_TableWordFilter),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("Instance", get_Instance, null),
			new LuaField("FilteredWords", get_FilteredWords, null),
		};

		LuaScriptMgr.RegisterLib(L, "Common.Tables.TableWordFilter", typeof(Common.Tables.TableWordFilter), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateCommon_Tables_TableWordFilter(IntPtr L)
	{
		LuaDLL.luaL_error(L, "Common.Tables.TableWordFilter class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(Common.Tables.TableWordFilter);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Instance(IntPtr L)
	{
		LuaScriptMgr.PushObject(L, Common.Tables.TableWordFilter.Instance);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_FilteredWords(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Common.Tables.TableWordFilter obj = (Common.Tables.TableWordFilter)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name FilteredWords");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index FilteredWords on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.FilteredWords);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Init(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Common.Tables.TableWordFilter obj = (Common.Tables.TableWordFilter)LuaScriptMgr.GetNetObjectSelf(L, 1, "Common.Tables.TableWordFilter");
		obj.Init();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Clear(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Common.Tables.TableWordFilter obj = (Common.Tables.TableWordFilter)LuaScriptMgr.GetNetObjectSelf(L, 1, "Common.Tables.TableWordFilter");
		obj.Clear();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int FilterText(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Common.Tables.TableWordFilter obj = (Common.Tables.TableWordFilter)LuaScriptMgr.GetNetObjectSelf(L, 1, "Common.Tables.TableWordFilter");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		bool o = obj.FilterText(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ParseContent(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Common.Tables.TableWordFilter obj = (Common.Tables.TableWordFilter)LuaScriptMgr.GetNetObjectSelf(L, 1, "Common.Tables.TableWordFilter");
		bool o = obj.ParseContent();
		LuaScriptMgr.Push(L, o);
		return 1;
	}
}

