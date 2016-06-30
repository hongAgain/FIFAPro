using System;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class TutorialWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("Instance", Instance),
			new LuaMethod("FocusAt", FocusAt),
			new LuaMethod("WatchAnotherDragDrop", WatchAnotherDragDrop),
			new LuaMethod("FocusFullScreen", FocusFullScreen),
			new LuaMethod("ActiveArrow", ActiveArrow),
			new LuaMethod("SetArrow", SetArrow),
			new LuaMethod("SetFinger", SetFinger),
			new LuaMethod("ReleaseFocus", ReleaseFocus),
			new LuaMethod("RevertFocus", RevertFocus),
			new LuaMethod("Highlight", Highlight),
			new LuaMethod("HandleAlignment", HandleAlignment),
			new LuaMethod("WatchClick", WatchClick),
			new LuaMethod("WatchDragDrop", WatchDragDrop),
			new LuaMethod("Hide", Hide),
			new LuaMethod("ShowTalk", ShowTalk),
			new LuaMethod("New", _CreateTutorial),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("blockInput", get_blockInput, set_blockInput),
			new LuaField("transparent", get_transparent, set_transparent),
			new LuaField("arrow", get_arrow, set_arrow),
			new LuaField("frame", get_frame, set_frame),
			new LuaField("finger", get_finger, set_finger),
			new LuaField("hand", get_hand, set_hand),
			new LuaField("dragFXRoot", get_dragFXRoot, set_dragFXRoot),
			new LuaField("dragLine", get_dragLine, set_dragLine),
			new LuaField("dragHand", get_dragHand, set_dragHand),
			new LuaField("offset", get_offset, set_offset),
			new LuaField("talk1", get_talk1, set_talk1),
			new LuaField("talk2", get_talk2, set_talk2),
			new LuaField("talk3", get_talk3, set_talk3),
		};

		LuaScriptMgr.RegisterLib(L, "Tutorial", typeof(Tutorial), regs, fields, typeof(MonoBehaviour));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateTutorial(IntPtr L)
	{
		LuaDLL.luaL_error(L, "Tutorial class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(Tutorial);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_blockInput(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Tutorial obj = (Tutorial)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name blockInput");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index blockInput on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.blockInput);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_transparent(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Tutorial obj = (Tutorial)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name transparent");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index transparent on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.transparent);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_arrow(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Tutorial obj = (Tutorial)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name arrow");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index arrow on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.arrow);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_frame(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Tutorial obj = (Tutorial)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name frame");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index frame on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.frame);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_finger(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Tutorial obj = (Tutorial)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name finger");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index finger on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.finger);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_hand(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Tutorial obj = (Tutorial)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name hand");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index hand on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.hand);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_dragFXRoot(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Tutorial obj = (Tutorial)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name dragFXRoot");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index dragFXRoot on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.dragFXRoot);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_dragLine(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Tutorial obj = (Tutorial)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name dragLine");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index dragLine on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.dragLine);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_dragHand(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Tutorial obj = (Tutorial)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name dragHand");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index dragHand on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.dragHand);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_offset(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Tutorial obj = (Tutorial)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name offset");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index offset on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.offset);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_talk1(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Tutorial obj = (Tutorial)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name talk1");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index talk1 on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.talk1);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_talk2(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Tutorial obj = (Tutorial)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name talk2");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index talk2 on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.talk2);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_talk3(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Tutorial obj = (Tutorial)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name talk3");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index talk3 on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.talk3);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_blockInput(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Tutorial obj = (Tutorial)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name blockInput");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index blockInput on a nil value");
			}
		}

		obj.blockInput = (GameObject)LuaScriptMgr.GetUnityObject(L, 3, typeof(GameObject));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_transparent(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Tutorial obj = (Tutorial)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name transparent");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index transparent on a nil value");
			}
		}

		obj.transparent = (UITexture)LuaScriptMgr.GetUnityObject(L, 3, typeof(UITexture));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_arrow(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Tutorial obj = (Tutorial)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name arrow");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index arrow on a nil value");
			}
		}

		obj.arrow = (UISprite)LuaScriptMgr.GetUnityObject(L, 3, typeof(UISprite));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_frame(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Tutorial obj = (Tutorial)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name frame");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index frame on a nil value");
			}
		}

		obj.frame = (UISprite)LuaScriptMgr.GetUnityObject(L, 3, typeof(UISprite));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_finger(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Tutorial obj = (Tutorial)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name finger");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index finger on a nil value");
			}
		}

		obj.finger = (UISprite)LuaScriptMgr.GetUnityObject(L, 3, typeof(UISprite));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_hand(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Tutorial obj = (Tutorial)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name hand");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index hand on a nil value");
			}
		}

		obj.hand = (GameObject)LuaScriptMgr.GetUnityObject(L, 3, typeof(GameObject));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_dragFXRoot(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Tutorial obj = (Tutorial)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name dragFXRoot");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index dragFXRoot on a nil value");
			}
		}

		obj.dragFXRoot = (GameObject)LuaScriptMgr.GetUnityObject(L, 3, typeof(GameObject));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_dragLine(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Tutorial obj = (Tutorial)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name dragLine");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index dragLine on a nil value");
			}
		}

		obj.dragLine = (UISprite)LuaScriptMgr.GetUnityObject(L, 3, typeof(UISprite));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_dragHand(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Tutorial obj = (Tutorial)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name dragHand");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index dragHand on a nil value");
			}
		}

		obj.dragHand = (GameObject)LuaScriptMgr.GetUnityObject(L, 3, typeof(GameObject));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_offset(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Tutorial obj = (Tutorial)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name offset");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index offset on a nil value");
			}
		}

		obj.offset = (Transform)LuaScriptMgr.GetUnityObject(L, 3, typeof(Transform));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_talk1(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Tutorial obj = (Tutorial)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name talk1");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index talk1 on a nil value");
			}
		}

		obj.talk1 = (Tutorial_NPCSpeech)LuaScriptMgr.GetNetObject(L, 3, typeof(Tutorial_NPCSpeech));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_talk2(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Tutorial obj = (Tutorial)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name talk2");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index talk2 on a nil value");
			}
		}

		obj.talk2 = (Tutorial_NPCSpeech)LuaScriptMgr.GetNetObject(L, 3, typeof(Tutorial_NPCSpeech));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_talk3(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Tutorial obj = (Tutorial)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name talk3");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index talk3 on a nil value");
			}
		}

		obj.talk3 = (Tutorial_Speech)LuaScriptMgr.GetNetObject(L, 3, typeof(Tutorial_Speech));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Instance(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		Tutorial o = Tutorial.Instance();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int FocusAt(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		Tutorial obj = (Tutorial)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Tutorial");
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 2, typeof(GameObject));
		bool arg1 = LuaScriptMgr.GetBoolean(L, 3);
		obj.FocusAt(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int WatchAnotherDragDrop(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 5);
		Tutorial obj = (Tutorial)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Tutorial");
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 2, typeof(GameObject));
		LuaFunction arg1 = LuaScriptMgr.GetLuaFunction(L, 3);
		LuaFunction arg2 = LuaScriptMgr.GetLuaFunction(L, 4);
		LuaFunction arg3 = LuaScriptMgr.GetLuaFunction(L, 5);
		obj.WatchAnotherDragDrop(arg0,arg1,arg2,arg3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int FocusFullScreen(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Tutorial obj = (Tutorial)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Tutorial");
		obj.FocusFullScreen();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ActiveArrow(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Tutorial obj = (Tutorial)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Tutorial");
		bool arg0 = LuaScriptMgr.GetBoolean(L, 2);
		obj.ActiveArrow(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetArrow(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		Tutorial obj = (Tutorial)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Tutorial");
		Vector2 arg0 = LuaScriptMgr.GetVector2(L, 2);
		bool arg1 = LuaScriptMgr.GetBoolean(L, 3);
		float arg2 = (float)LuaScriptMgr.GetNumber(L, 4);
		obj.SetArrow(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetFinger(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 5);
		Tutorial obj = (Tutorial)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Tutorial");
		Vector2 arg0 = LuaScriptMgr.GetVector2(L, 2);
		bool arg1 = LuaScriptMgr.GetBoolean(L, 3);
		bool arg2 = LuaScriptMgr.GetBoolean(L, 4);
		float arg3 = (float)LuaScriptMgr.GetNumber(L, 5);
		obj.SetFinger(arg0,arg1,arg2,arg3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ReleaseFocus(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Tutorial obj = (Tutorial)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Tutorial");
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 2, typeof(GameObject));
		obj.ReleaseFocus(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RevertFocus(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Tutorial obj = (Tutorial)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Tutorial");
		obj.RevertFocus();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Highlight(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		Tutorial obj = (Tutorial)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Tutorial");
		Vector3 arg0 = LuaScriptMgr.GetVector3(L, 2);
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 3);
		int arg2 = (int)LuaScriptMgr.GetNumber(L, 4);
		obj.Highlight(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int HandleAlignment(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		Tutorial obj = (Tutorial)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Tutorial");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 3);
		obj.HandleAlignment(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int WatchClick(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		Tutorial obj = (Tutorial)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Tutorial");
		LuaFunction arg0 = LuaScriptMgr.GetLuaFunction(L, 2);
		bool arg1 = LuaScriptMgr.GetBoolean(L, 3);
		obj.WatchClick(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int WatchDragDrop(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		Tutorial obj = (Tutorial)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Tutorial");
		LuaFunction arg0 = LuaScriptMgr.GetLuaFunction(L, 2);
		LuaFunction arg1 = LuaScriptMgr.GetLuaFunction(L, 3);
		LuaFunction arg2 = LuaScriptMgr.GetLuaFunction(L, 4);
		obj.WatchDragDrop(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Hide(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Tutorial obj = (Tutorial)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Tutorial");
		bool arg0 = LuaScriptMgr.GetBoolean(L, 2);
		obj.Hide(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowTalk(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 5);
		Tutorial obj = (Tutorial)LuaScriptMgr.GetUnityObjectSelf(L, 1, "Tutorial");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		string arg1 = LuaScriptMgr.GetLuaString(L, 3);
		string arg2 = LuaScriptMgr.GetLuaString(L, 4);
		Vector4 arg3 = LuaScriptMgr.GetVector4(L, 5);
		obj.ShowTalk(arg0,arg1,arg2,arg3);
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

