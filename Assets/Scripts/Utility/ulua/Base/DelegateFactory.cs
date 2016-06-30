using System;
using UnityEngine;
using System.Collections.Generic;
using LuaInterface;

public static class DelegateFactory
{
	delegate Delegate DelegateValue(LuaFunction func);
	static Dictionary<Type, DelegateValue> dict = new Dictionary<Type, DelegateValue>();

	[NoToLuaAttribute]
	public static void Register(IntPtr L)
	{
		dict.Add(typeof(Action<GameObject>), new DelegateValue(Action_GameObject));
		dict.Add(typeof(Action), new DelegateValue(Action));
		dict.Add(typeof(UnityEngine.Events.UnityAction), new DelegateValue(UnityEngine_Events_UnityAction));
		dict.Add(typeof(Application.LogCallback), new DelegateValue(Application_LogCallback));
		dict.Add(typeof(GUI.WindowFunction), new DelegateValue(GUI_WindowFunction));
		dict.Add(typeof(UIEventListener.VoidDelegate), new DelegateValue(UIEventListener_VoidDelegate));
		dict.Add(typeof(UIEventListener.BoolDelegate), new DelegateValue(UIEventListener_BoolDelegate));
		dict.Add(typeof(UIEventListener.FloatDelegate), new DelegateValue(UIEventListener_FloatDelegate));
		dict.Add(typeof(UIEventListener.VectorDelegate), new DelegateValue(UIEventListener_VectorDelegate));
		dict.Add(typeof(UIEventListener.ObjectDelegate), new DelegateValue(UIEventListener_ObjectDelegate));
		dict.Add(typeof(UIEventListener.KeyCodeDelegate), new DelegateValue(UIEventListener_KeyCodeDelegate));
		dict.Add(typeof(OnRequestResp), new DelegateValue(OnRequestResp));
		dict.Add(typeof(Comparison<UIBaseWindowLua>), new DelegateValue(Comparison_UIBaseWindowLua));
		dict.Add(typeof(OnResponse), new DelegateValue(OnResponse));
	}

	[NoToLuaAttribute]
	public static Delegate CreateDelegate(Type t, LuaFunction func)
	{
		DelegateValue create = null;

		if (!dict.TryGetValue(t, out create))
		{
			Debugger.LogError("Delegate {0} not register", t.FullName);
			return null;
		}
		return create(func);
	}

	public static Delegate Action_GameObject(LuaFunction func)
	{
		Action<GameObject> d = (param0) =>
		{
			int top = func.BeginPCall();
			IntPtr L = func.GetLuaState();
			LuaScriptMgr.Push(L, param0);
			func.PCall(top, 1);
			func.EndPCall(top);
		};
		return d;
	}

	public static Delegate Action(LuaFunction func)
	{
		Action d = () =>
		{
			func.Call();
		};
		return d;
	}

	public static Delegate UnityEngine_Events_UnityAction(LuaFunction func)
	{
		UnityEngine.Events.UnityAction d = () =>
		{
			func.Call();
		};
		return d;
	}

	public static Delegate Application_LogCallback(LuaFunction func)
	{
		Application.LogCallback d = (param0, param1, param2) =>
		{
			int top = func.BeginPCall();
			IntPtr L = func.GetLuaState();
			LuaScriptMgr.Push(L, param0);
			LuaScriptMgr.Push(L, param1);
			LuaScriptMgr.Push(L, param2);
			func.PCall(top, 3);
			func.EndPCall(top);
		};
		return d;
	}

	public static Delegate GUI_WindowFunction(LuaFunction func)
	{
		GUI.WindowFunction d = (param0) =>
		{
			int top = func.BeginPCall();
			IntPtr L = func.GetLuaState();
			LuaScriptMgr.Push(L, param0);
			func.PCall(top, 1);
			func.EndPCall(top);
		};
		return d;
	}

	public static Delegate UIEventListener_VoidDelegate(LuaFunction func)
	{
		UIEventListener.VoidDelegate d = (param0) =>
		{
			int top = func.BeginPCall();
			IntPtr L = func.GetLuaState();
			LuaScriptMgr.Push(L, param0);
			func.PCall(top, 1);
			func.EndPCall(top);
		};
		return d;
	}

	public static Delegate UIEventListener_BoolDelegate(LuaFunction func)
	{
		UIEventListener.BoolDelegate d = (param0, param1) =>
		{
			int top = func.BeginPCall();
			IntPtr L = func.GetLuaState();
			LuaScriptMgr.Push(L, param0);
			LuaScriptMgr.Push(L, param1);
			func.PCall(top, 2);
			func.EndPCall(top);
		};
		return d;
	}

	public static Delegate UIEventListener_FloatDelegate(LuaFunction func)
	{
		UIEventListener.FloatDelegate d = (param0, param1) =>
		{
			int top = func.BeginPCall();
			IntPtr L = func.GetLuaState();
			LuaScriptMgr.Push(L, param0);
			LuaScriptMgr.Push(L, param1);
			func.PCall(top, 2);
			func.EndPCall(top);
		};
		return d;
	}

	public static Delegate UIEventListener_VectorDelegate(LuaFunction func)
	{
		UIEventListener.VectorDelegate d = (param0, param1) =>
		{
			int top = func.BeginPCall();
			IntPtr L = func.GetLuaState();
			LuaScriptMgr.Push(L, param0);
			LuaScriptMgr.Push(L, param1);
			func.PCall(top, 2);
			func.EndPCall(top);
		};
		return d;
	}

	public static Delegate UIEventListener_ObjectDelegate(LuaFunction func)
	{
		UIEventListener.ObjectDelegate d = (param0, param1) =>
		{
			int top = func.BeginPCall();
			IntPtr L = func.GetLuaState();
			LuaScriptMgr.Push(L, param0);
			LuaScriptMgr.Push(L, param1);
			func.PCall(top, 2);
			func.EndPCall(top);
		};
		return d;
	}

	public static Delegate UIEventListener_KeyCodeDelegate(LuaFunction func)
	{
		UIEventListener.KeyCodeDelegate d = (param0, param1) =>
		{
			int top = func.BeginPCall();
			IntPtr L = func.GetLuaState();
			LuaScriptMgr.Push(L, param0);
			LuaScriptMgr.Push(L, param1);
			func.PCall(top, 2);
			func.EndPCall(top);
		};
		return d;
	}

	public static Delegate OnRequestResp(LuaFunction func)
	{
		OnRequestResp d = (param0, param1) =>
		{
			int top = func.BeginPCall();
			IntPtr L = func.GetLuaState();
			LuaScriptMgr.Push(L, param0);
			LuaScriptMgr.PushObject(L, param1);
			func.PCall(top, 2);
			func.EndPCall(top);
		};
		return d;
	}

	public static Delegate Comparison_UIBaseWindowLua(LuaFunction func)
	{
		Comparison<UIBaseWindowLua> d = (param0, param1) =>
		{
			int top = func.BeginPCall();
			IntPtr L = func.GetLuaState();
			LuaScriptMgr.Push(L, param0);
			LuaScriptMgr.Push(L, param1);
			func.PCall(top, 2);
			object[] objs = func.PopValues(top);
			func.EndPCall(top);
			return (int)objs[0];
		};
		return d;
	}

	public static Delegate OnResponse(LuaFunction func)
	{
		OnResponse d = (param0) =>
		{
			int top = func.BeginPCall();
			IntPtr L = func.GetLuaState();
			LuaScriptMgr.PushVarObject(L, param0);
			func.PCall(top, 1);
			func.EndPCall(top);
		};
		return d;
	}

	public static void Clear()
	{
		dict.Clear();
	}

}
