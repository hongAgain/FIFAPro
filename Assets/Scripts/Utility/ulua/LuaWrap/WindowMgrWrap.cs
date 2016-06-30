using System;
using UnityEngine;
using LuaInterface;

public class WindowMgrWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("MoveTransformNodeDelta", MoveTransformNodeDelta),
			new LuaMethod("MoveTransformNodeAbsolute", MoveTransformNodeAbsolute),
			new LuaMethod("ShowMsgBox", ShowMsgBox),
			new LuaMethod("ShowSystemWindow", ShowSystemWindow),
			new LuaMethod("ShowWindow", ShowWindow),
			new LuaMethod("CloseWindow", CloseWindow),
			new LuaMethod("RmvActiveWindow", RmvActiveWindow),
			new LuaMethod("AdjustLayer", AdjustLayer),
			new LuaMethod("AlignOnCenter", AlignOnCenter),
			new LuaMethod("FindUILayer", FindUILayer),
			new LuaMethod("BlockInput", BlockInput),
			new LuaMethod("UnLockInput", UnLockInput),
			new LuaMethod("BlockUISystemInput", BlockUISystemInput),
			new LuaMethod("UnLockUISystemInput", UnLockUISystemInput),
			new LuaMethod("Recycle", Recycle),
			new LuaMethod("ActiveUICamera", ActiveUICamera),
			new LuaMethod("Create3DUI", Create3DUI),
			new LuaMethod("New", _CreateWindowMgr),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("UI_LAYER", get_UI_LAYER, null),
			new LuaField("UI3D_LAYER", get_UI3D_LAYER, null),
			new LuaField("UISystem_LAYER", get_UISystem_LAYER, null),
			new LuaField("mSharedBundle", get_mSharedBundle, set_mSharedBundle),
			new LuaField("UIParent", get_UIParent, null),
			new LuaField("UISystemParent", get_UISystemParent, null),
			new LuaField("UICamTrans", get_UICamTrans, null),
			new LuaField("UICamSystemTrans", get_UICamSystemTrans, null),
			new LuaField("UICam3DTrans", get_UICam3DTrans, null),
		};

		LuaScriptMgr.RegisterLib(L, "WindowMgr", typeof(WindowMgr), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateWindowMgr(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			WindowMgr obj = new WindowMgr();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: WindowMgr.New");
		}

		return 0;
	}

	static Type classType = typeof(WindowMgr);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_UI_LAYER(IntPtr L)
	{
		LuaScriptMgr.Push(L, WindowMgr.UI_LAYER);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_UI3D_LAYER(IntPtr L)
	{
		LuaScriptMgr.Push(L, WindowMgr.UI3D_LAYER);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_UISystem_LAYER(IntPtr L)
	{
		LuaScriptMgr.Push(L, WindowMgr.UISystem_LAYER);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_mSharedBundle(IntPtr L)
	{
		LuaScriptMgr.Push(L, WindowMgr.mSharedBundle);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_UIParent(IntPtr L)
	{
		LuaScriptMgr.Push(L, WindowMgr.UIParent);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_UISystemParent(IntPtr L)
	{
		LuaScriptMgr.Push(L, WindowMgr.UISystemParent);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_UICamTrans(IntPtr L)
	{
		LuaScriptMgr.Push(L, WindowMgr.UICamTrans);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_UICamSystemTrans(IntPtr L)
	{
		LuaScriptMgr.Push(L, WindowMgr.UICamSystemTrans);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_UICam3DTrans(IntPtr L)
	{
		LuaScriptMgr.Push(L, WindowMgr.UICam3DTrans);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_mSharedBundle(IntPtr L)
	{
		WindowMgr.mSharedBundle = (AssetBundle)LuaScriptMgr.GetUnityObject(L, 3, typeof(AssetBundle));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int MoveTransformNodeDelta(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		float arg0 = (float)LuaScriptMgr.GetNumber(L, 1);
		float arg1 = (float)LuaScriptMgr.GetNumber(L, 2);
		float arg2 = (float)LuaScriptMgr.GetNumber(L, 3);
		WindowMgr.MoveTransformNodeDelta(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int MoveTransformNodeAbsolute(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		float arg0 = (float)LuaScriptMgr.GetNumber(L, 1);
		float arg1 = (float)LuaScriptMgr.GetNumber(L, 2);
		float arg2 = (float)LuaScriptMgr.GetNumber(L, 3);
		WindowMgr.MoveTransformNodeAbsolute(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowMsgBox(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		LuaTable arg1 = LuaScriptMgr.GetLuaTable(L, 2);
		WindowMgr.ShowMsgBox(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowSystemWindow(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		LuaTable arg1 = LuaScriptMgr.GetLuaTable(L, 2);
		WindowMgr.ShowSystemWindow(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowWindow(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			string arg0 = LuaScriptMgr.GetLuaString(L, 1);
			WindowMgr.ShowWindow(arg0);
			return 0;
		}
		else if (count == 2)
		{
			string arg0 = LuaScriptMgr.GetLuaString(L, 1);
			LuaTable arg1 = LuaScriptMgr.GetLuaTable(L, 2);
			WindowMgr.ShowWindow(arg0,arg1);
			return 0;
		}
		else if (count == 3)
		{
			UIBaseWindowLua arg0 = (UIBaseWindowLua)LuaScriptMgr.GetUnityObject(L, 1, typeof(UIBaseWindowLua));
			string arg1 = LuaScriptMgr.GetLuaString(L, 2);
			LuaTable arg2 = LuaScriptMgr.GetLuaTable(L, 3);
			WindowMgr.ShowWindow(arg0,arg1,arg2);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: WindowMgr.ShowWindow");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CloseWindow(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		WindowMgr.CloseWindow(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RmvActiveWindow(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UIBaseWindowLua arg0 = (UIBaseWindowLua)LuaScriptMgr.GetUnityObject(L, 1, typeof(UIBaseWindowLua));
		WindowMgr.RmvActiveWindow(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AdjustLayer(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		WindowMgr.AdjustLayer();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AlignOnCenter(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		UIBaseWindowLua arg0 = (UIBaseWindowLua)LuaScriptMgr.GetUnityObject(L, 1, typeof(UIBaseWindowLua));
		Comparison<UIBaseWindowLua> arg1 = null;
		LuaTypes funcType2 = LuaDLL.lua_type(L, 2);

		if (funcType2 != LuaTypes.LUA_TFUNCTION)
		{
			 arg1 = (Comparison<UIBaseWindowLua>)LuaScriptMgr.GetNetObject(L, 2, typeof(Comparison<UIBaseWindowLua>));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 2);
			arg1 = (param0, param1) =>
			{
				int top = func.BeginPCall();
				LuaScriptMgr.Push(L, param0);
				LuaScriptMgr.Push(L, param1);
				func.PCall(top, 2);
				object[] objs = func.PopValues(top);
				func.EndPCall(top);
				return (int)objs[0];
			};
		}

		WindowMgr.AlignOnCenter(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int FindUILayer(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UIBaseWindowLua arg0 = (UIBaseWindowLua)LuaScriptMgr.GetUnityObject(L, 1, typeof(UIBaseWindowLua));
		int o = WindowMgr.FindUILayer(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int BlockInput(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		WindowMgr.BlockInput();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int UnLockInput(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		WindowMgr.UnLockInput();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int BlockUISystemInput(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		WindowMgr.BlockUISystemInput();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int UnLockUISystemInput(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		WindowMgr.UnLockUISystemInput();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Recycle(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UIBaseWindowLua arg0 = (UIBaseWindowLua)LuaScriptMgr.GetUnityObject(L, 1, typeof(UIBaseWindowLua));
		WindowMgr.Recycle(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ActiveUICamera(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		bool arg0 = LuaScriptMgr.GetBoolean(L, 1);
		WindowMgr.ActiveUICamera(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Create3DUI(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		GameObject o = WindowMgr.Create3DUI(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}
}

