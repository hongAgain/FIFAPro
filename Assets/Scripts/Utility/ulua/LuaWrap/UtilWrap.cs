using System;
using UnityEngine;
using System.Collections;
using LuaInterface;

public class UtilWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("GetType", GetType),
			new LuaMethod("GetComponentInChildren", GetComponentInChildren),
			new LuaMethod("GetComponent", GetComponent),
			new LuaMethod("GetMissingComponent", GetMissingComponent),
			new LuaMethod("GetComponentsInChildren", GetComponentsInChildren),
			new LuaMethod("GetAllChild", GetAllChild),
			new LuaMethod("Int", Int),
			new LuaMethod("Float", Float),
			new LuaMethod("Long", Long),
			new LuaMethod("Random", Random),
			new LuaMethod("Uid", Uid),
			new LuaMethod("GetTime", GetTime),
			new LuaMethod("GetTimeToString", GetTimeToString),
			new LuaMethod("GetTotalSeconds", GetTotalSeconds),
			new LuaMethod("ShowTestTime", ShowTestTime),
			new LuaMethod("GetLocalYear", GetLocalYear),
			new LuaMethod("GetLocalMonth", GetLocalMonth),
			new LuaMethod("GetLocalDayInWeek", GetLocalDayInWeek),
			new LuaMethod("GetLocalDayInMonth", GetLocalDayInMonth),
			new LuaMethod("GetLocalHourInDay", GetLocalHourInDay),
			new LuaMethod("GetYearFromTimeStamp", GetYearFromTimeStamp),
			new LuaMethod("GetMonthFromTimeStamp", GetMonthFromTimeStamp),
			new LuaMethod("GetDayInWeekFromTimeStamp", GetDayInWeekFromTimeStamp),
			new LuaMethod("GetDayInMonthFromTimeStamp", GetDayInMonthFromTimeStamp),
			new LuaMethod("GetYearAfterAddDay", GetYearAfterAddDay),
			new LuaMethod("GetMonthAfterAddDay", GetMonthAfterAddDay),
			new LuaMethod("GetDayInMonthAfterAddDay", GetDayInMonthAfterAddDay),
			new LuaMethod("GetDayInYearAfterAddDay", GetDayInYearAfterAddDay),
			new LuaMethod("GetPassedTimeMeasurement", GetPassedTimeMeasurement),
			new LuaMethod("Child", Child),
			new LuaMethod("Peer", Peer),
			new LuaMethod("Encode", Encode),
			new LuaMethod("Decode", Decode),
			new LuaMethod("IsNumeric", IsNumeric),
			new LuaMethod("HashToMD5Hex", HashToMD5Hex),
			new LuaMethod("md5", md5),
			new LuaMethod("md5file", md5file),
			new LuaMethod("CompressFile", CompressFile),
			new LuaMethod("DecompressFile", DecompressFile),
			new LuaMethod("Compress", Compress),
			new LuaMethod("Decompress", Decompress),
			new LuaMethod("ClearChild", ClearChild),
			new LuaMethod("GetInt", GetInt),
			new LuaMethod("HasKey", HasKey),
			new LuaMethod("SetInt", SetInt),
			new LuaMethod("GetString", GetString),
			new LuaMethod("SetString", SetString),
			new LuaMethod("RemoveData", RemoveData),
			new LuaMethod("CallGC", CallGC),
			new LuaMethod("CallUnloadUnusedAssets", CallUnloadUnusedAssets),
			new LuaMethod("IsNumber", IsNumber),
			new LuaMethod("GetFileText", GetFileText),
			new LuaMethod("AppContentPath", AppContentPath),
			new LuaMethod("EnableScript", EnableScript),
			new LuaMethod("AddClick", AddClick),
			new LuaMethod("AddPress", AddPress),
			new LuaMethod("ChangeClick", ChangeClick),
			new LuaMethod("AddDragDrop", AddDragDrop),
			new LuaMethod("RmvEventListener", RmvEventListener),
			new LuaMethod("LuaPath", LuaPath),
			new LuaMethod("GetLuaScriptModuleName", GetLuaScriptModuleName),
			new LuaMethod("GetLuaFilePath", GetLuaFilePath),
			new LuaMethod("WorldToNGUIPixel", WorldToNGUIPixel),
			new LuaMethod("WorldToNGUIScale", WorldToNGUIScale),
			new LuaMethod("ScreenToNGUIScale", ScreenToNGUIScale),
			new LuaMethod("GetHashTable", GetHashTable),
			new LuaMethod("FillHashTable", FillHashTable),
			new LuaMethod("MatchURL", MatchURL),
			new LuaMethod("UriEscapeDataString", UriEscapeDataString),
			new LuaMethod("Log", Log),
			new LuaMethod("LogWarning", LogWarning),
			new LuaMethod("LogError", LogError),
			new LuaMethod("MarkAsChanged", MarkAsChanged),
			new LuaMethod("ForceQuitGame", ForceQuitGame),
			new LuaMethod("LocalizeString", LocalizeString),
			new LuaMethod("CreateLobbyPlayer", CreateLobbyPlayer),
			new LuaMethod("CreatePerson", CreatePerson),
			new LuaMethod("CreateCoach", CreateCoach),
			new LuaMethod("CombineSkinnedMeshRenderer", CombineSkinnedMeshRenderer),
			new LuaMethod("PlayAnimation", PlayAnimation),
			new LuaMethod("StartAnimAtRandomTime", StartAnimAtRandomTime),
			new LuaMethod("InitializeUICircleListItem", InitializeUICircleListItem),
			new LuaMethod("SetUITexture", SetUITexture),
			new LuaMethod("GetGameObject", GetGameObject),
			new LuaMethod("ChangeUniform", ChangeUniform),
			new LuaMethod("ChangeUniformImmidiately", ChangeUniformImmidiately),
			new LuaMethod("SetLight", SetLight),
			new LuaMethod("SetAmbientLight", SetAmbientLight),
			new LuaMethod("SetFog", SetFog),
			new LuaMethod("DisableLightMap", DisableLightMap),
			new LuaMethod("LoadLevel", LoadLevel),
			new LuaMethod("ChangeLevelState", ChangeLevelState),
			new LuaMethod("CreateLevelState", CreateLevelState),
			new LuaMethod("ClearLevelState", ClearLevelState),
			new LuaMethod("SyncObjPos", SyncObjPos),
			new LuaMethod("SetJinJieLevel", SetJinJieLevel),
			new LuaMethod("GetLuaBehaviourParamNum", GetLuaBehaviourParamNum),
			new LuaMethod("SetAutoActive", SetAutoActive),
			new LuaMethod("AddChild", AddChild),
			new LuaMethod("DetachHUD", DetachHUD),
			new LuaMethod("AttachHUD", AttachHUD),
			new LuaMethod("Attach2D", Attach2D),
			new LuaMethod("Attach2DBall", Attach2DBall),
			new LuaMethod("TestinExceptionLog", TestinExceptionLog),
			new LuaMethod("SetShaderPropertiesInt", SetShaderPropertiesInt),
			new LuaMethod("SetParentAndAllChildrenToLayer", SetParentAndAllChildrenToLayer),
			new LuaMethod("SetAllChildrenToLayer", SetAllChildrenToLayer),
			new LuaMethod("FormatString", FormatString),
			new LuaMethod("New", _CreateUtil),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("AppContentDataUri", get_AppContentDataUri, null),
			new LuaField("NetAvailable", get_NetAvailable, null),
			new LuaField("IsWifi", get_IsWifi, null),
			new LuaField("isLogin", get_isLogin, null),
		};

		LuaScriptMgr.RegisterLib(L, "Util", typeof(Util), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateUtil(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			Util obj = new Util();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Util.New");
		}

		return 0;
	}

	static Type classType = typeof(Util);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_AppContentDataUri(IntPtr L)
	{
		LuaScriptMgr.PushObject(L, Util.AppContentDataUri);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_NetAvailable(IntPtr L)
	{
		LuaScriptMgr.Push(L, Util.NetAvailable);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsWifi(IntPtr L)
	{
		LuaScriptMgr.Push(L, Util.IsWifi);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_isLogin(IntPtr L)
	{
		LuaScriptMgr.Push(L, Util.isLogin);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetType(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		Type o = Util.GetType(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetComponentInChildren(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		Component o = Util.GetComponentInChildren(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetComponent(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		Component o = Util.GetComponent(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetMissingComponent(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		Component o = Util.GetMissingComponent(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetComponentsInChildren(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		Component[] o = Util.GetComponentsInChildren(arg0,arg1);
		LuaScriptMgr.PushArray(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetAllChild(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		Transform[] o = Util.GetAllChild(arg0);
		LuaScriptMgr.PushArray(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Int(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		float arg0 = (float)LuaScriptMgr.GetNumber(L, 1);
		int o = Util.Int(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Float(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		object arg0 = LuaScriptMgr.GetVarObject(L, 1);
		float o = Util.Float(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Long(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		object arg0 = LuaScriptMgr.GetVarObject(L, 1);
		long o = Util.Long(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Random(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(float), typeof(float)))
		{
			float arg0 = (float)LuaDLL.lua_tonumber(L, 1);
			float arg1 = (float)LuaDLL.lua_tonumber(L, 2);
			float o = Util.Random(arg0,arg1);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(int), typeof(int)))
		{
			int arg0 = (int)LuaDLL.lua_tonumber(L, 1);
			int arg1 = (int)LuaDLL.lua_tonumber(L, 2);
			int o = Util.Random(arg0,arg1);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Util.Random");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Uid(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string o = Util.Uid(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetTime(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		long o = Util.GetTime();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetTimeToString(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		double arg0 = (double)LuaScriptMgr.GetNumber(L, 1);
		string o = Util.GetTimeToString(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetTotalSeconds(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		long arg0 = (long)LuaScriptMgr.GetNumber(L, 1);
		double o = Util.GetTotalSeconds(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowTestTime(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		long arg0 = (long)LuaScriptMgr.GetNumber(L, 1);
		Util.ShowTestTime(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetLocalYear(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		int o = Util.GetLocalYear();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetLocalMonth(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		int o = Util.GetLocalMonth();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetLocalDayInWeek(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		int o = Util.GetLocalDayInWeek();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetLocalDayInMonth(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		int o = Util.GetLocalDayInMonth();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetLocalHourInDay(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		int o = Util.GetLocalHourInDay();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetYearFromTimeStamp(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		double arg0 = (double)LuaScriptMgr.GetNumber(L, 1);
		int o = Util.GetYearFromTimeStamp(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetMonthFromTimeStamp(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		double arg0 = (double)LuaScriptMgr.GetNumber(L, 1);
		int o = Util.GetMonthFromTimeStamp(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetDayInWeekFromTimeStamp(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		double arg0 = (double)LuaScriptMgr.GetNumber(L, 1);
		int o = Util.GetDayInWeekFromTimeStamp(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetDayInMonthFromTimeStamp(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		double arg0 = (double)LuaScriptMgr.GetNumber(L, 1);
		int o = Util.GetDayInMonthFromTimeStamp(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetYearAfterAddDay(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		double arg0 = (double)LuaScriptMgr.GetNumber(L, 1);
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 2);
		int o = Util.GetYearAfterAddDay(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetMonthAfterAddDay(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		double arg0 = (double)LuaScriptMgr.GetNumber(L, 1);
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 2);
		int o = Util.GetMonthAfterAddDay(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetDayInMonthAfterAddDay(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		double arg0 = (double)LuaScriptMgr.GetNumber(L, 1);
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 2);
		int o = Util.GetDayInMonthAfterAddDay(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetDayInYearAfterAddDay(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		double arg0 = (double)LuaScriptMgr.GetNumber(L, 1);
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 2);
		int o = Util.GetDayInYearAfterAddDay(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetPassedTimeMeasurement(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		double arg0 = (double)LuaScriptMgr.GetNumber(L, 1);
		string o = Util.GetPassedTimeMeasurement(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Child(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(Transform), typeof(string)))
		{
			Transform arg0 = (Transform)LuaScriptMgr.GetLuaObject(L, 1);
			string arg1 = LuaScriptMgr.GetString(L, 2);
			GameObject o = Util.Child(arg0,arg1);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(GameObject), typeof(string)))
		{
			GameObject arg0 = (GameObject)LuaScriptMgr.GetLuaObject(L, 1);
			string arg1 = LuaScriptMgr.GetString(L, 2);
			GameObject o = Util.Child(arg0,arg1);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Util.Child");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Peer(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(Transform), typeof(string)))
		{
			Transform arg0 = (Transform)LuaScriptMgr.GetLuaObject(L, 1);
			string arg1 = LuaScriptMgr.GetString(L, 2);
			GameObject o = Util.Peer(arg0,arg1);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(GameObject), typeof(string)))
		{
			GameObject arg0 = (GameObject)LuaScriptMgr.GetLuaObject(L, 1);
			string arg1 = LuaScriptMgr.GetString(L, 2);
			GameObject o = Util.Peer(arg0,arg1);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Util.Peer");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Encode(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string o = Util.Encode(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Decode(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string o = Util.Decode(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsNumeric(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		bool o = Util.IsNumeric(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int HashToMD5Hex(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string o = Util.HashToMD5Hex(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int md5(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string o = Util.md5(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int md5file(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string o = Util.md5file(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CompressFile(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		Util.CompressFile(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DecompressFile(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string o = Util.DecompressFile(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Compress(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string o = Util.Compress(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Decompress(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string o = Util.Decompress(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ClearChild(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		Util.ClearChild(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetInt(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		int o = Util.GetInt(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int HasKey(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		bool o = Util.HasKey(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetInt(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 2);
		Util.SetInt(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetString(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string o = Util.GetString(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetString(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		Util.SetString(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RemoveData(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		Util.RemoveData(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CallGC(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		Util.CallGC();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CallUnloadUnusedAssets(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		Util.CallUnloadUnusedAssets();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsNumber(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		bool o = Util.IsNumber(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetFileText(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string o = Util.GetFileText(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AppContentPath(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		string o = Util.AppContentPath();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int EnableScript(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		bool arg2 = LuaScriptMgr.GetBoolean(L, 3);
		Util.EnableScript(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddClick(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		object arg1 = LuaScriptMgr.GetVarObject(L, 2);
		UIEventListener o = Util.AddClick(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddPress(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		object arg1 = LuaScriptMgr.GetVarObject(L, 2);
		UIEventListener o = Util.AddPress(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ChangeClick(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		object arg1 = LuaScriptMgr.GetVarObject(L, 2);
		UIEventListener o = Util.ChangeClick(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddDragDrop(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 6);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		object arg1 = LuaScriptMgr.GetVarObject(L, 2);
		object arg2 = LuaScriptMgr.GetVarObject(L, 3);
		object arg3 = LuaScriptMgr.GetVarObject(L, 4);
		object arg4 = LuaScriptMgr.GetVarObject(L, 5);
		object arg5 = LuaScriptMgr.GetVarObject(L, 6);
		UIEventListener o = Util.AddDragDrop(arg0,arg1,arg2,arg3,arg4,arg5);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RmvEventListener(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		Util.RmvEventListener(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LuaPath(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string o = Util.LuaPath(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetLuaScriptModuleName(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string o = Util.GetLuaScriptModuleName(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetLuaFilePath(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string o = Util.GetLuaFilePath(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int WorldToNGUIPixel(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Camera arg0 = (Camera)LuaScriptMgr.GetUnityObject(L, 1, typeof(Camera));
		Vector3 arg1 = LuaScriptMgr.GetVector3(L, 2);
		Vector3 o = Util.WorldToNGUIPixel(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int WorldToNGUIScale(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Camera arg0 = (Camera)LuaScriptMgr.GetUnityObject(L, 1, typeof(Camera));
		Vector3 arg1 = LuaScriptMgr.GetVector3(L, 2);
		Vector3 o = Util.WorldToNGUIScale(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ScreenToNGUIScale(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Vector3 arg0 = LuaScriptMgr.GetVector3(L, 1);
		Vector3 o = Util.ScreenToNGUIScale(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetHashTable(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		Hashtable o = Util.GetHashTable();
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int FillHashTable(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		Hashtable arg0 = (Hashtable)LuaScriptMgr.GetNetObject(L, 1, typeof(Hashtable));
		object arg1 = LuaScriptMgr.GetVarObject(L, 2);
		object arg2 = LuaScriptMgr.GetVarObject(L, 3);
		Util.FillHashTable(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int MatchURL(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		bool o = Util.MatchURL(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int UriEscapeDataString(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string o = Util.UriEscapeDataString(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Log(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		object arg0 = LuaScriptMgr.GetVarObject(L, 1);
		Util.Log(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LogWarning(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		object arg0 = LuaScriptMgr.GetVarObject(L, 1);
		Util.LogWarning(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LogError(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		object arg0 = LuaScriptMgr.GetVarObject(L, 1);
		Util.LogError(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int MarkAsChanged(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		Util.MarkAsChanged(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ForceQuitGame(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		Util.ForceQuitGame();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LocalizeString(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string o = Util.LocalizeString(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CreateLobbyPlayer(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		LuaTable arg1 = LuaScriptMgr.GetLuaTable(L, 2);
		Util.CreateLobbyPlayer(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CreatePerson(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 9);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		string arg2 = LuaScriptMgr.GetLuaString(L, 3);
		string arg3 = LuaScriptMgr.GetLuaString(L, 4);
		Color arg4 = LuaScriptMgr.GetColor(L, 5);
		int arg5 = (int)LuaScriptMgr.GetNumber(L, 6);
		string arg6 = LuaScriptMgr.GetLuaString(L, 7);
		LuaFunction arg7 = LuaScriptMgr.GetLuaFunction(L, 8);
		string arg8 = LuaScriptMgr.GetLuaString(L, 9);
		Util.CreatePerson(arg0,arg1,arg2,arg3,arg4,arg5,arg6,arg7,arg8);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CreateCoach(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		LuaFunction arg1 = LuaScriptMgr.GetLuaFunction(L, 2);
		string arg2 = LuaScriptMgr.GetLuaString(L, 3);
		string arg3 = LuaScriptMgr.GetLuaString(L, 4);
		Util.CreateCoach(arg0,arg1,arg2,arg3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CombineSkinnedMeshRenderer(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		LuaFunction arg1 = LuaScriptMgr.GetLuaFunction(L, 2);
		Util.CombineSkinnedMeshRenderer(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PlayAnimation(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		bool arg2 = LuaScriptMgr.GetBoolean(L, 3);
		Util.PlayAnimation(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int StartAnimAtRandomTime(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		bool arg2 = LuaScriptMgr.GetBoolean(L, 3);
		Util.StartAnimAtRandomTime(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int InitializeUICircleListItem(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		Util.InitializeUICircleListItem(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetUITexture(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		string arg2 = LuaScriptMgr.GetLuaString(L, 3);
		bool arg3 = LuaScriptMgr.GetBoolean(L, 4);
		Util.SetUITexture(arg0,arg1,arg2,arg3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetGameObject(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		GameObject o = Util.GetGameObject(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ChangeUniform(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		Util.ChangeUniform(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ChangeUniformImmidiately(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		Util.ChangeUniformImmidiately(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetLight(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		Color arg1 = LuaScriptMgr.GetColor(L, 2);
		float arg2 = (float)LuaScriptMgr.GetNumber(L, 3);
		Util.SetLight(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetAmbientLight(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Color arg0 = LuaScriptMgr.GetColor(L, 1);
		Util.SetAmbientLight(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetFog(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		bool arg0 = LuaScriptMgr.GetBoolean(L, 1);
		Util.SetFog(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DisableLightMap(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		Util.DisableLightMap();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LoadLevel(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		Util.LoadLevel(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ChangeLevelState(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		Vector3 arg1 = LuaScriptMgr.GetVector3(L, 2);
		GameObject o = Util.ChangeLevelState(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CreateLevelState(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		GameObject o = Util.CreateLevelState(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ClearLevelState(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		Util.ClearLevelState();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SyncObjPos(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		GameObject arg1 = (GameObject)LuaScriptMgr.GetUnityObject(L, 2, typeof(GameObject));
		Camera arg2 = (Camera)LuaScriptMgr.GetUnityObject(L, 3, typeof(Camera));
		Camera arg3 = (Camera)LuaScriptMgr.GetUnityObject(L, 4, typeof(Camera));
		Util.SyncObjPos(arg0,arg1,arg2,arg3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetJinJieLevel(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 2);
		Util.SetJinJieLevel(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetLuaBehaviourParamNum(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		int o = Util.GetLuaBehaviourParamNum(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetAutoActive(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 2);
		Util.SetAutoActive(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddChild(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		GameObject arg1 = (GameObject)LuaScriptMgr.GetUnityObject(L, 2, typeof(GameObject));
		GameObject o = Util.AddChild(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DetachHUD(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		Util.DetachHUD(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AttachHUD(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		GameObject arg1 = (GameObject)LuaScriptMgr.GetUnityObject(L, 2, typeof(GameObject));
		GameObject o = Util.AttachHUD(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Attach2D(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		GameObject arg1 = (GameObject)LuaScriptMgr.GetUnityObject(L, 2, typeof(GameObject));
		GameObject o = Util.Attach2D(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Attach2DBall(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		GameObject arg1 = (GameObject)LuaScriptMgr.GetUnityObject(L, 2, typeof(GameObject));
		GameObject o = Util.Attach2DBall(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int TestinExceptionLog(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		Util.TestinExceptionLog(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetShaderPropertiesInt(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		string arg2 = LuaScriptMgr.GetLuaString(L, 3);
		int arg3 = (int)LuaScriptMgr.GetNumber(L, 4);
		Util.SetShaderPropertiesInt(arg0,arg1,arg2,arg3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetParentAndAllChildrenToLayer(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(Transform), typeof(int)))
		{
			Transform arg0 = (Transform)LuaScriptMgr.GetLuaObject(L, 1);
			int arg1 = (int)LuaDLL.lua_tonumber(L, 2);
			Util.SetParentAndAllChildrenToLayer(arg0,arg1);
			return 0;
		}
		else if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(Transform), typeof(string)))
		{
			Transform arg0 = (Transform)LuaScriptMgr.GetLuaObject(L, 1);
			string arg1 = LuaScriptMgr.GetString(L, 2);
			Util.SetParentAndAllChildrenToLayer(arg0,arg1);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Util.SetParentAndAllChildrenToLayer");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetAllChildrenToLayer(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 2);
		Util.SetAllChildrenToLayer(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int FormatString(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string[] objs1 = LuaScriptMgr.GetParamsString(L, 2, count - 1);
		string o = Util.FormatString(arg0,objs1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}
}

