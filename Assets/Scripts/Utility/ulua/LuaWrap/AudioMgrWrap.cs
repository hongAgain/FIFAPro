using System;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class AudioMgrWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("Instance", Instance),
			new LuaMethod("PlayBGMusic", PlayBGMusic),
			new LuaMethod("PauseBGMusic", PauseBGMusic),
			new LuaMethod("DestroyBGMusic", DestroyBGMusic),
			new LuaMethod("New", _CreateAudioMgr),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("IsMuteMusic", get_IsMuteMusic, set_IsMuteMusic),
			new LuaField("IsMuteSound", get_IsMuteSound, set_IsMuteSound),
			new LuaField("CurrBGMusicName", get_CurrBGMusicName, null),
			new LuaField("MusicVolume", get_MusicVolume, set_MusicVolume),
			new LuaField("SoundVolume", get_SoundVolume, set_SoundVolume),
		};

		LuaScriptMgr.RegisterLib(L, "AudioMgr", typeof(AudioMgr), regs, fields, typeof(MonoBehaviour));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateAudioMgr(IntPtr L)
	{
		LuaDLL.luaL_error(L, "AudioMgr class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(AudioMgr);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsMuteMusic(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioMgr obj = (AudioMgr)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsMuteMusic");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsMuteMusic on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsMuteMusic);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsMuteSound(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioMgr obj = (AudioMgr)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsMuteSound");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsMuteSound on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsMuteSound);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_CurrBGMusicName(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioMgr obj = (AudioMgr)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name CurrBGMusicName");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index CurrBGMusicName on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.CurrBGMusicName);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_MusicVolume(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioMgr obj = (AudioMgr)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name MusicVolume");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index MusicVolume on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.MusicVolume);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_SoundVolume(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioMgr obj = (AudioMgr)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name SoundVolume");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index SoundVolume on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.SoundVolume);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_IsMuteMusic(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioMgr obj = (AudioMgr)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsMuteMusic");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsMuteMusic on a nil value");
			}
		}

		obj.IsMuteMusic = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_IsMuteSound(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioMgr obj = (AudioMgr)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsMuteSound");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsMuteSound on a nil value");
			}
		}

		obj.IsMuteSound = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_MusicVolume(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioMgr obj = (AudioMgr)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name MusicVolume");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index MusicVolume on a nil value");
			}
		}

		obj.MusicVolume = (float)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_SoundVolume(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		AudioMgr obj = (AudioMgr)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name SoundVolume");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index SoundVolume on a nil value");
			}
		}

		obj.SoundVolume = (float)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Instance(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		AudioMgr o = AudioMgr.Instance();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PlayBGMusic(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		AudioMgr obj = (AudioMgr)LuaScriptMgr.GetUnityObjectSelf(L, 1, "AudioMgr");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		obj.PlayBGMusic(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PauseBGMusic(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		AudioMgr obj = (AudioMgr)LuaScriptMgr.GetUnityObjectSelf(L, 1, "AudioMgr");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		bool arg1 = LuaScriptMgr.GetBoolean(L, 3);
		obj.PauseBGMusic(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DestroyBGMusic(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		AudioMgr obj = (AudioMgr)LuaScriptMgr.GetUnityObjectSelf(L, 1, "AudioMgr");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		obj.DestroyBGMusic(arg0);
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

