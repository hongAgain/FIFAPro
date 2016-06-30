using System;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class UIBaseWindowLuaWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("HideForDrawCall", HideForDrawCall),
			new LuaMethod("Hide", Hide),
			new LuaMethod("Show", Show),
			new LuaMethod("Close", Close),
			new LuaMethod("DoOpenEffect", DoOpenEffect),
			new LuaMethod("AddChildWindow", AddChildWindow),
			new LuaMethod("RmvChildWindow", RmvChildWindow),
			new LuaMethod("SetFrontOf", SetFrontOf),
			new LuaMethod("AdjustSelfPanelDepth", AdjustSelfPanelDepth),
			new LuaMethod("MoveTo", MoveTo),
			new LuaMethod("MoveBack", MoveBack),
			new LuaMethod("AddPrefab", AddPrefab),
			new LuaMethod("GetPrefab", GetPrefab),
			new LuaMethod("OnTop", OnTop),
			new LuaMethod("New", _CreateUIBaseWindowLua),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("ScriptName", get_ScriptName, set_ScriptName),
			new LuaField("mLuaTables", get_mLuaTables, set_mLuaTables),
			new LuaField("FullScreen", get_FullScreen, set_FullScreen),
			new LuaField("autoCollider", get_autoCollider, set_autoCollider),
			new LuaField("OnDestroyEvent", get_OnDestroyEvent, set_OnDestroyEvent),
			new LuaField("resourcesBarMode", get_resourcesBarMode, set_resourcesBarMode),
			new LuaField("titleKey", get_titleKey, set_titleKey),
			new LuaField("HasScript", get_HasScript, null),
			new LuaField("ChildrenWindow", get_ChildrenWindow, null),
			new LuaField("Father", get_Father, set_Father),
		};

		LuaScriptMgr.RegisterLib(L, "UIBaseWindowLua", typeof(UIBaseWindowLua), regs, fields, typeof(MonoBehaviour));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateUIBaseWindowLua(IntPtr L)
	{
		LuaDLL.luaL_error(L, "UIBaseWindowLua class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(UIBaseWindowLua);

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
		UIBaseWindowLua obj = (UIBaseWindowLua)o;

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
	static int get_mLuaTables(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UIBaseWindowLua obj = (UIBaseWindowLua)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name mLuaTables");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index mLuaTables on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.mLuaTables);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_FullScreen(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UIBaseWindowLua obj = (UIBaseWindowLua)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name FullScreen");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index FullScreen on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.FullScreen);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_autoCollider(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UIBaseWindowLua obj = (UIBaseWindowLua)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name autoCollider");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index autoCollider on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.autoCollider);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_OnDestroyEvent(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UIBaseWindowLua obj = (UIBaseWindowLua)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name OnDestroyEvent");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index OnDestroyEvent on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.OnDestroyEvent);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_resourcesBarMode(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UIBaseWindowLua obj = (UIBaseWindowLua)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name resourcesBarMode");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index resourcesBarMode on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.resourcesBarMode);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_titleKey(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UIBaseWindowLua obj = (UIBaseWindowLua)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name titleKey");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index titleKey on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.titleKey);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_HasScript(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UIBaseWindowLua obj = (UIBaseWindowLua)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name HasScript");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index HasScript on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.HasScript);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ChildrenWindow(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UIBaseWindowLua obj = (UIBaseWindowLua)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name ChildrenWindow");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index ChildrenWindow on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.ChildrenWindow);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Father(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UIBaseWindowLua obj = (UIBaseWindowLua)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Father");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Father on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.Father);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_ScriptName(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UIBaseWindowLua obj = (UIBaseWindowLua)o;

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
	static int set_mLuaTables(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UIBaseWindowLua obj = (UIBaseWindowLua)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name mLuaTables");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index mLuaTables on a nil value");
			}
		}

		obj.mLuaTables = LuaScriptMgr.GetLuaTable(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_FullScreen(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UIBaseWindowLua obj = (UIBaseWindowLua)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name FullScreen");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index FullScreen on a nil value");
			}
		}

		obj.FullScreen = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_autoCollider(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UIBaseWindowLua obj = (UIBaseWindowLua)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name autoCollider");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index autoCollider on a nil value");
			}
		}

		obj.autoCollider = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_OnDestroyEvent(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UIBaseWindowLua obj = (UIBaseWindowLua)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name OnDestroyEvent");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index OnDestroyEvent on a nil value");
			}
		}

		LuaTypes funcType = LuaDLL.lua_type(L, 3);

		if (funcType != LuaTypes.LUA_TFUNCTION)
		{
			obj.OnDestroyEvent = (Action)LuaScriptMgr.GetNetObject(L, 3, typeof(Action));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.ToLuaFunction(L, 3);
			obj.OnDestroyEvent = () =>
			{
				func.Call();
			};
		}
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_resourcesBarMode(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UIBaseWindowLua obj = (UIBaseWindowLua)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name resourcesBarMode");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index resourcesBarMode on a nil value");
			}
		}

		obj.resourcesBarMode = (UIBaseWindowLua.ResourceBar)LuaScriptMgr.GetNetObject(L, 3, typeof(UIBaseWindowLua.ResourceBar));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_titleKey(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UIBaseWindowLua obj = (UIBaseWindowLua)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name titleKey");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index titleKey on a nil value");
			}
		}

		obj.titleKey = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Father(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		UIBaseWindowLua obj = (UIBaseWindowLua)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Father");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Father on a nil value");
			}
		}

		obj.Father = (UIBaseWindowLua)LuaScriptMgr.GetUnityObject(L, 3, typeof(UIBaseWindowLua));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int HideForDrawCall(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UIBaseWindowLua obj = (UIBaseWindowLua)LuaScriptMgr.GetUnityObjectSelf(L, 1, "UIBaseWindowLua");
		obj.HideForDrawCall();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Hide(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UIBaseWindowLua obj = (UIBaseWindowLua)LuaScriptMgr.GetUnityObjectSelf(L, 1, "UIBaseWindowLua");
		obj.Hide();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Show(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UIBaseWindowLua obj = (UIBaseWindowLua)LuaScriptMgr.GetUnityObjectSelf(L, 1, "UIBaseWindowLua");
		obj.Show();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Close(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UIBaseWindowLua obj = (UIBaseWindowLua)LuaScriptMgr.GetUnityObjectSelf(L, 1, "UIBaseWindowLua");
		obj.Close();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DoOpenEffect(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UIBaseWindowLua obj = (UIBaseWindowLua)LuaScriptMgr.GetUnityObjectSelf(L, 1, "UIBaseWindowLua");
		obj.DoOpenEffect();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddChildWindow(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		UIBaseWindowLua obj = (UIBaseWindowLua)LuaScriptMgr.GetUnityObjectSelf(L, 1, "UIBaseWindowLua");
		UIBaseWindowLua arg0 = (UIBaseWindowLua)LuaScriptMgr.GetUnityObject(L, 2, typeof(UIBaseWindowLua));
		obj.AddChildWindow(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RmvChildWindow(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		UIBaseWindowLua obj = (UIBaseWindowLua)LuaScriptMgr.GetUnityObjectSelf(L, 1, "UIBaseWindowLua");
		UIBaseWindowLua arg0 = (UIBaseWindowLua)LuaScriptMgr.GetUnityObject(L, 2, typeof(UIBaseWindowLua));
		obj.RmvChildWindow(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetFrontOf(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		UIBaseWindowLua obj = (UIBaseWindowLua)LuaScriptMgr.GetUnityObjectSelf(L, 1, "UIBaseWindowLua");
		UIBaseWindowLua arg0 = (UIBaseWindowLua)LuaScriptMgr.GetUnityObject(L, 2, typeof(UIBaseWindowLua));
		obj.SetFrontOf(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AdjustSelfPanelDepth(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UIBaseWindowLua obj = (UIBaseWindowLua)LuaScriptMgr.GetUnityObjectSelf(L, 1, "UIBaseWindowLua");
		obj.AdjustSelfPanelDepth();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int MoveTo(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		UIBaseWindowLua obj = (UIBaseWindowLua)LuaScriptMgr.GetUnityObjectSelf(L, 1, "UIBaseWindowLua");
		Vector3 arg0 = LuaScriptMgr.GetVector3(L, 2);
		obj.MoveTo(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int MoveBack(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UIBaseWindowLua obj = (UIBaseWindowLua)LuaScriptMgr.GetUnityObjectSelf(L, 1, "UIBaseWindowLua");
		obj.MoveBack();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddPrefab(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		UIBaseWindowLua obj = (UIBaseWindowLua)LuaScriptMgr.GetUnityObjectSelf(L, 1, "UIBaseWindowLua");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		GameObject arg1 = (GameObject)LuaScriptMgr.GetUnityObject(L, 3, typeof(GameObject));
		obj.AddPrefab(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetPrefab(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		UIBaseWindowLua obj = (UIBaseWindowLua)LuaScriptMgr.GetUnityObjectSelf(L, 1, "UIBaseWindowLua");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		GameObject o = obj.GetPrefab(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnTop(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UIBaseWindowLua obj = (UIBaseWindowLua)LuaScriptMgr.GetUnityObjectSelf(L, 1, "UIBaseWindowLua");
		obj.OnTop();
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

