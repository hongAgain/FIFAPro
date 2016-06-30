using System;
using System.Collections;
using LuaInterface;

public class Common_Tables_TableManagerWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("InitTables", InitTables),
			new LuaMethod("GetProperty", GetProperty),
			new LuaMethod("New", _CreateCommon_Tables_TableManager),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("Instance", get_Instance, null),
			new LuaField("PosCfgTbl", get_PosCfgTbl, null),
			new LuaField("EvtAfterStopTable", get_EvtAfterStopTable, null),
			new LuaField("BattleInfoTable", get_BattleInfoTable, null),
			new LuaField("HeroTbl", get_HeroTbl, null),
			new LuaField("CoachTable", get_CoachTable, null),
			new LuaField("AIConfig", get_AIConfig, null),
			new LuaField("GroundAreaTable", get_GroundAreaTable, null),
			new LuaField("PassTimeTable", get_PassTimeTable, null),
			new LuaField("ShootTimeTbl", get_ShootTimeTbl, null),
			new LuaField("SensitivityFactorTbl", get_SensitivityFactorTbl, null),
			new LuaField("SettlementFactorTbl", get_SettlementFactorTbl, null),
			new LuaField("AttackTacticalConfig", get_AttackTacticalConfig, null),
			new LuaField("DistanceDecayTbl", get_DistanceDecayTbl, null),
			new LuaField("InterceptCoefficientDataConfig", get_InterceptCoefficientDataConfig, null),
			new LuaField("MarkCoefficientDataConfig", get_MarkCoefficientDataConfig, null),
			new LuaField("TacticalPosCoefficientDataConfig", get_TacticalPosCoefficientDataConfig, null),
			new LuaField("MidKickOffPosTableConfig", get_MidKickOffPosTableConfig, null),
			new LuaField("HomePositionZDataConfig", get_HomePositionZDataConfig, null),
			new LuaField("AniDataConfig", get_AniDataConfig, null),
			new LuaField("AniCombineConfig", get_AniCombineConfig, null),
			new LuaField("AniBeahaviorConfig", get_AniBeahaviorConfig, null),
			new LuaField("AniStateLayerConfig", get_AniStateLayerConfig, null),
			new LuaField("SkillTbl", get_SkillTbl, null),
			new LuaField("BaseFxTbl", get_BaseFxTbl, null),
			new LuaField("CameraEffectTbl", get_CameraEffectTbl, null),
			new LuaField("GhostEffectTbl", get_GhostEffectTbl, null),
			new LuaField("RaidNpcTbl", get_RaidNpcTbl, null),
			new LuaField("BattleTextTbl", get_BattleTextTbl, null),
			new LuaField("BattleTextCondTbl", get_BattleTextCondTbl, null),
			new LuaField("ConfrontationBasicTbl", get_ConfrontationBasicTbl, null),
			new LuaField("EnergyTbl", get_EnergyTbl, null),
			new LuaField("FormationTbl", get_FormationTbl, null),
			new LuaField("SkillAppearTbl", get_SkillAppearTbl, null),
			new LuaField("hotSpot", get_hotSpot, null),
			new LuaField("defenceDensity", get_defenceDensity, null),
			new LuaField("BattlePositionConfig", get_BattlePositionConfig, null),
		};

		LuaScriptMgr.RegisterLib(L, "Common.Tables.TableManager", typeof(Common.Tables.TableManager), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateCommon_Tables_TableManager(IntPtr L)
	{
		LuaDLL.luaL_error(L, "Common.Tables.TableManager class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(Common.Tables.TableManager);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Instance(IntPtr L)
	{
		LuaScriptMgr.PushObject(L, Common.Tables.TableManager.Instance);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_PosCfgTbl(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Common.Tables.TableManager obj = (Common.Tables.TableManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name PosCfgTbl");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index PosCfgTbl on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.PosCfgTbl);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_EvtAfterStopTable(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Common.Tables.TableManager obj = (Common.Tables.TableManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name EvtAfterStopTable");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index EvtAfterStopTable on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.EvtAfterStopTable);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_BattleInfoTable(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Common.Tables.TableManager obj = (Common.Tables.TableManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name BattleInfoTable");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index BattleInfoTable on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.BattleInfoTable);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_HeroTbl(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Common.Tables.TableManager obj = (Common.Tables.TableManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name HeroTbl");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index HeroTbl on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.HeroTbl);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_CoachTable(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Common.Tables.TableManager obj = (Common.Tables.TableManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name CoachTable");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index CoachTable on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.CoachTable);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_AIConfig(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Common.Tables.TableManager obj = (Common.Tables.TableManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name AIConfig");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index AIConfig on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.AIConfig);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_GroundAreaTable(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Common.Tables.TableManager obj = (Common.Tables.TableManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name GroundAreaTable");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index GroundAreaTable on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.GroundAreaTable);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_PassTimeTable(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Common.Tables.TableManager obj = (Common.Tables.TableManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name PassTimeTable");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index PassTimeTable on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.PassTimeTable);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ShootTimeTbl(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Common.Tables.TableManager obj = (Common.Tables.TableManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name ShootTimeTbl");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index ShootTimeTbl on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.ShootTimeTbl);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_SensitivityFactorTbl(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Common.Tables.TableManager obj = (Common.Tables.TableManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name SensitivityFactorTbl");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index SensitivityFactorTbl on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.SensitivityFactorTbl);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_SettlementFactorTbl(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Common.Tables.TableManager obj = (Common.Tables.TableManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name SettlementFactorTbl");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index SettlementFactorTbl on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.SettlementFactorTbl);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_AttackTacticalConfig(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Common.Tables.TableManager obj = (Common.Tables.TableManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name AttackTacticalConfig");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index AttackTacticalConfig on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.AttackTacticalConfig);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_DistanceDecayTbl(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Common.Tables.TableManager obj = (Common.Tables.TableManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name DistanceDecayTbl");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index DistanceDecayTbl on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.DistanceDecayTbl);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_InterceptCoefficientDataConfig(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Common.Tables.TableManager obj = (Common.Tables.TableManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name InterceptCoefficientDataConfig");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index InterceptCoefficientDataConfig on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.InterceptCoefficientDataConfig);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_MarkCoefficientDataConfig(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Common.Tables.TableManager obj = (Common.Tables.TableManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name MarkCoefficientDataConfig");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index MarkCoefficientDataConfig on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.MarkCoefficientDataConfig);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_TacticalPosCoefficientDataConfig(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Common.Tables.TableManager obj = (Common.Tables.TableManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name TacticalPosCoefficientDataConfig");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index TacticalPosCoefficientDataConfig on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.TacticalPosCoefficientDataConfig);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_MidKickOffPosTableConfig(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Common.Tables.TableManager obj = (Common.Tables.TableManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name MidKickOffPosTableConfig");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index MidKickOffPosTableConfig on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.MidKickOffPosTableConfig);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_HomePositionZDataConfig(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Common.Tables.TableManager obj = (Common.Tables.TableManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name HomePositionZDataConfig");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index HomePositionZDataConfig on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.HomePositionZDataConfig);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_AniDataConfig(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Common.Tables.TableManager obj = (Common.Tables.TableManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name AniDataConfig");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index AniDataConfig on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.AniDataConfig);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_AniCombineConfig(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Common.Tables.TableManager obj = (Common.Tables.TableManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name AniCombineConfig");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index AniCombineConfig on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.AniCombineConfig);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_AniBeahaviorConfig(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Common.Tables.TableManager obj = (Common.Tables.TableManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name AniBeahaviorConfig");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index AniBeahaviorConfig on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.AniBeahaviorConfig);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_AniStateLayerConfig(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Common.Tables.TableManager obj = (Common.Tables.TableManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name AniStateLayerConfig");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index AniStateLayerConfig on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.AniStateLayerConfig);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_SkillTbl(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Common.Tables.TableManager obj = (Common.Tables.TableManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name SkillTbl");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index SkillTbl on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.SkillTbl);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_BaseFxTbl(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Common.Tables.TableManager obj = (Common.Tables.TableManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name BaseFxTbl");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index BaseFxTbl on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.BaseFxTbl);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_CameraEffectTbl(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Common.Tables.TableManager obj = (Common.Tables.TableManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name CameraEffectTbl");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index CameraEffectTbl on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.CameraEffectTbl);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_GhostEffectTbl(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Common.Tables.TableManager obj = (Common.Tables.TableManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name GhostEffectTbl");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index GhostEffectTbl on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.GhostEffectTbl);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_RaidNpcTbl(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Common.Tables.TableManager obj = (Common.Tables.TableManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name RaidNpcTbl");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index RaidNpcTbl on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.RaidNpcTbl);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_BattleTextTbl(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Common.Tables.TableManager obj = (Common.Tables.TableManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name BattleTextTbl");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index BattleTextTbl on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.BattleTextTbl);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_BattleTextCondTbl(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Common.Tables.TableManager obj = (Common.Tables.TableManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name BattleTextCondTbl");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index BattleTextCondTbl on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.BattleTextCondTbl);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ConfrontationBasicTbl(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Common.Tables.TableManager obj = (Common.Tables.TableManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name ConfrontationBasicTbl");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index ConfrontationBasicTbl on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.ConfrontationBasicTbl);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_EnergyTbl(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Common.Tables.TableManager obj = (Common.Tables.TableManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name EnergyTbl");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index EnergyTbl on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.EnergyTbl);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_FormationTbl(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Common.Tables.TableManager obj = (Common.Tables.TableManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name FormationTbl");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index FormationTbl on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.FormationTbl);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_SkillAppearTbl(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Common.Tables.TableManager obj = (Common.Tables.TableManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name SkillAppearTbl");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index SkillAppearTbl on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.SkillAppearTbl);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_hotSpot(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Common.Tables.TableManager obj = (Common.Tables.TableManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name hotSpot");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index hotSpot on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.hotSpot);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_defenceDensity(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Common.Tables.TableManager obj = (Common.Tables.TableManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name defenceDensity");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index defenceDensity on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.defenceDensity);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_BattlePositionConfig(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Common.Tables.TableManager obj = (Common.Tables.TableManager)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name BattlePositionConfig");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index BattlePositionConfig on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.BattlePosTbl);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int InitTables(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Common.Tables.TableManager obj = (Common.Tables.TableManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "Common.Tables.TableManager");
		IEnumerator o = obj.InitTables();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetProperty(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		Common.Tables.TableManager obj = (Common.Tables.TableManager)LuaScriptMgr.GetNetObjectSelf(L, 1, "Common.Tables.TableManager");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		string arg1 = LuaScriptMgr.GetLuaString(L, 3);
		string arg2 = LuaScriptMgr.GetLuaString(L, 4);
		object o = obj.GetProperty(arg0,arg1,arg2);
		LuaScriptMgr.PushVarObject(L, o);
		return 1;
	}
}

