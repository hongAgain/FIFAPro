using System;
using UnityEngine;
using LuaInterface;

public class UIHelperWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("SetLabelTxt", SetLabelTxt),
			new LuaMethod("GrayWidget", GrayWidget),
			new LuaMethod("SetLabelFontSize", SetLabelFontSize),
			new LuaMethod("SetLabelKey", SetLabelKey),
			new LuaMethod("SetLabelOmitTail", SetLabelOmitTail),
			new LuaMethod("EnableBtn", EnableBtn),
			new LuaMethod("EnableButton", EnableButton),
			new LuaMethod("InputTxt", InputTxt),
			new LuaMethod("SetInputText", SetInputText),
			new LuaMethod("SetProgressBar", SetProgressBar),
			new LuaMethod("GetProgressBar", GetProgressBar),
			new LuaMethod("InstantiateStars", InstantiateStars),
			new LuaMethod("InstantiateGrid", InstantiateGrid),
			new LuaMethod("ReInstantiateGrid", ReInstantiateGrid),
			new LuaMethod("RepositionGrid", RepositionGrid),
			new LuaMethod("SortByNumericNameDesc", SortByNumericNameDesc),
			new LuaMethod("GridSortByNumericName", GridSortByNumericName),
			new LuaMethod("ResetScroll", ResetScroll),
			new LuaMethod("ResetScrollGrid", ResetScrollGrid),
			new LuaMethod("SetGridPosition", SetGridPosition),
			new LuaMethod("DestroyGrid", DestroyGrid),
			new LuaMethod("RefreshPanel", RefreshPanel),
			new LuaMethod("SetPanelDepth", SetPanelDepth),
			new LuaMethod("GetMaxDepthOfPanelInChildren", GetMaxDepthOfPanelInChildren),
			new LuaMethod("GetPanelDepth", GetPanelDepth),
			new LuaMethod("AddDragOnStarted", AddDragOnStarted),
			new LuaMethod("AddDragOnFinish", AddDragOnFinish),
			new LuaMethod("IsOverDragged", IsOverDragged),
			new LuaMethod("SpringPanelBegin", SpringPanelBegin),
			new LuaMethod("CenterOnRecenter", CenterOnRecenter),
			new LuaMethod("OnClickScrollView", OnClickScrollView),
			new LuaMethod("OnReleaseScrollView", OnReleaseScrollView),
			new LuaMethod("OnClickChildToCenterOn", OnClickChildToCenterOn),
			new LuaMethod("OnCenterItem", OnCenterItem),
			new LuaMethod("ClipAreaCenterLocalPos", ClipAreaCenterLocalPos),
			new LuaMethod("GetPanelSize", GetPanelSize),
			new LuaMethod("GetUIGridCellSize", GetUIGridCellSize),
			new LuaMethod("EnableWidget", EnableWidget),
			new LuaMethod("IsSpriteInAtlas", IsSpriteInAtlas),
			new LuaMethod("SetSpriteName", SetSpriteName),
			new LuaMethod("SetSpriteNameNoPerfect", SetSpriteNameNoPerfect),
			new LuaMethod("SetWidgetColor", SetWidgetColor),
			new LuaMethod("SetButtonSpriteName", SetButtonSpriteName),
			new LuaMethod("SetSpritePolygonRate", SetSpritePolygonRate),
			new LuaMethod("SetSpriteFillAmount", SetSpriteFillAmount),
			new LuaMethod("SetButtonActive", SetButtonActive),
			new LuaMethod("SetBoxCollider", SetBoxCollider),
			new LuaMethod("AdjustDepth", AdjustDepth),
			new LuaMethod("SetDepth", SetDepth),
			new LuaMethod("WidthOfWidget", WidthOfWidget),
			new LuaMethod("HeightOfWidget", HeightOfWidget),
			new LuaMethod("SizeOfWidget", SizeOfWidget),
			new LuaMethod("SetSizeOfWidget", SetSizeOfWidget),
			new LuaMethod("LabelSize", LabelSize),
			new LuaMethod("AddToggle", AddToggle),
			new LuaMethod("SetToggleState", SetToggleState),
			new LuaMethod("GetToggleState", GetToggleState),
			new LuaMethod("SetDragScrollViewTarget", SetDragScrollViewTarget),
			new LuaMethod("CalculateWidgetBoundsSize", CalculateWidgetBoundsSize),
			new LuaMethod("Floating", Floating),
			new LuaMethod("AlphaTweening", AlphaTweening),
			new LuaMethod("TweenPositionBegin", TweenPositionBegin),
			new LuaMethod("TweenPositionPlayForward", TweenPositionPlayForward),
			new LuaMethod("TweenPositionPlayReverse", TweenPositionPlayReverse),
			new LuaMethod("SetTweenPositionOnFinish", SetTweenPositionOnFinish),
			new LuaMethod("SetTweenerTime", SetTweenerTime),
			new LuaMethod("SetCirclePos", SetCirclePos),
			new LuaMethod("OnDragEndCircle", OnDragEndCircle),
			new LuaMethod("OnDragCircle", OnDragCircle),
			new LuaMethod("OnDragStartCircle", OnDragStartCircle),
			new LuaMethod("AddPressRepeating", AddPressRepeating),
			new LuaMethod("AddDragOver", AddDragOver),
			new LuaMethod("SetAutoActiveCout", SetAutoActiveCout),
			new LuaMethod("SwitchSideMenuIn", SwitchSideMenuIn),
			new LuaMethod("SwitchSideMenuOut", SwitchSideMenuOut),
			new LuaMethod("ScrollClippedPanelTo", ScrollClippedPanelTo),
			new LuaMethod("PlayUIRainDropScreenAnime", PlayUIRainDropScreenAnime),
			new LuaMethod("FadeUIWidgetColor", FadeUIWidgetColor),
			new LuaMethod("FadeUIWidgetColorTo", FadeUIWidgetColorTo),
			new LuaMethod("SetPanelAlpha", SetPanelAlpha),
			new LuaMethod("GetUIEventListener", GetUIEventListener),
			new LuaMethod("SetScrollViewRestrict", SetScrollViewRestrict),
			new LuaMethod("SetPopupListItem", SetPopupListItem),
			new LuaMethod("AddPopupListDelegate", AddPopupListDelegate),
			new LuaMethod("AddPopupListItem", AddPopupListItem),
			new LuaMethod("New", _CreateUIHelper),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
		};

		LuaScriptMgr.RegisterLib(L, "UIHelper", typeof(UIHelper), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateUIHelper(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			UIHelper obj = new UIHelper();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: UIHelper.New");
		}

		return 0;
	}

	static Type classType = typeof(UIHelper);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetLabelTxt(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		UIHelper.SetLabelTxt(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GrayWidget(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		bool arg1 = LuaScriptMgr.GetBoolean(L, 2);
		UIHelper.GrayWidget(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetLabelFontSize(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		float arg1 = (float)LuaScriptMgr.GetNumber(L, 2);
		UIHelper.SetLabelFontSize(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetLabelKey(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		UIHelper.SetLabelKey(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetLabelOmitTail(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		UIHelper.SetLabelOmitTail(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int EnableBtn(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		bool arg1 = LuaScriptMgr.GetBoolean(L, 2);
		UIHelper.EnableBtn(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int EnableButton(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		bool arg1 = LuaScriptMgr.GetBoolean(L, 2);
		UIHelper.EnableButton(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int InputTxt(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		string o = UIHelper.InputTxt(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetInputText(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		UIHelper.SetInputText(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetProgressBar(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		float arg1 = (float)LuaScriptMgr.GetNumber(L, 2);
		UIHelper.SetProgressBar(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetProgressBar(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		float o = UIHelper.GetProgressBar(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int InstantiateStars(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 2);
		int arg2 = (int)LuaScriptMgr.GetNumber(L, 3);
		UIHelper.InstantiateStars(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int InstantiateGrid(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		Transform arg1 = (Transform)LuaScriptMgr.GetUnityObject(L, 2, typeof(Transform));
		int arg2 = (int)LuaScriptMgr.GetNumber(L, 3);
		int arg3 = (int)LuaScriptMgr.GetNumber(L, 4);
		UIHelper.InstantiateGrid(arg0,arg1,arg2,arg3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ReInstantiateGrid(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		Transform arg1 = (Transform)LuaScriptMgr.GetUnityObject(L, 2, typeof(Transform));
		int arg2 = (int)LuaScriptMgr.GetNumber(L, 3);
		bool arg3 = LuaScriptMgr.GetBoolean(L, 4);
		UIHelper.ReInstantiateGrid(arg0,arg1,arg2,arg3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RepositionGrid(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
			UIHelper.RepositionGrid(arg0);
			return 0;
		}
		else if (count == 2)
		{
			Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
			Transform arg1 = (Transform)LuaScriptMgr.GetUnityObject(L, 2, typeof(Transform));
			UIHelper.RepositionGrid(arg0,arg1);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: UIHelper.RepositionGrid");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SortByNumericNameDesc(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		Transform arg1 = (Transform)LuaScriptMgr.GetUnityObject(L, 2, typeof(Transform));
		int o = UIHelper.SortByNumericNameDesc(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GridSortByNumericName(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		UIHelper.GridSortByNumericName(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ResetScroll(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		UIHelper.ResetScroll(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ResetScrollGrid(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		Transform arg1 = (Transform)LuaScriptMgr.GetUnityObject(L, 2, typeof(Transform));
		UIHelper.ResetScrollGrid(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetGridPosition(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 5);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		Transform arg1 = (Transform)LuaScriptMgr.GetUnityObject(L, 2, typeof(Transform));
		Vector3 arg2 = LuaScriptMgr.GetVector3(L, 3);
		Vector3 arg3 = LuaScriptMgr.GetVector3(L, 4);
		bool arg4 = LuaScriptMgr.GetBoolean(L, 5);
		UIHelper.SetGridPosition(arg0,arg1,arg2,arg3,arg4);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DestroyGrid(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		UIHelper.DestroyGrid(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RefreshPanel(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		UIHelper.RefreshPanel(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetPanelDepth(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 2);
		UIHelper.SetPanelDepth(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetMaxDepthOfPanelInChildren(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		int o = UIHelper.GetMaxDepthOfPanelInChildren(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetPanelDepth(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		int o = UIHelper.GetPanelDepth(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddDragOnStarted(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		object arg1 = LuaScriptMgr.GetVarObject(L, 2);
		UIHelper.AddDragOnStarted(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddDragOnFinish(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		object arg1 = LuaScriptMgr.GetVarObject(L, 2);
		UIHelper.AddDragOnFinish(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsOverDragged(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 5);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		bool arg1 = LuaScriptMgr.GetBoolean(L, 2);
		bool arg2 = LuaScriptMgr.GetBoolean(L, 3);
		bool arg3 = LuaScriptMgr.GetBoolean(L, 4);
		bool arg4 = LuaScriptMgr.GetBoolean(L, 5);
		bool o = UIHelper.IsOverDragged(arg0,arg1,arg2,arg3,arg4);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SpringPanelBegin(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		Vector3 arg1 = LuaScriptMgr.GetVector3(L, 2);
		float arg2 = (float)LuaScriptMgr.GetNumber(L, 3);
		UIHelper.SpringPanelBegin(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CenterOnRecenter(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		Transform o = UIHelper.CenterOnRecenter(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnClickScrollView(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		Transform arg1 = (Transform)LuaScriptMgr.GetUnityObject(L, 2, typeof(Transform));
		UIHelper.OnClickScrollView(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnReleaseScrollView(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		UIHelper.OnReleaseScrollView(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnClickChildToCenterOn(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		Transform arg1 = (Transform)LuaScriptMgr.GetUnityObject(L, 2, typeof(Transform));
		UIHelper.OnClickChildToCenterOn(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnCenterItem(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		LuaFunction arg1 = LuaScriptMgr.GetLuaFunction(L, 2);
		UIHelper.OnCenterItem(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ClipAreaCenterLocalPos(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		Vector3 o = UIHelper.ClipAreaCenterLocalPos(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetPanelSize(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		Vector2 o = UIHelper.GetPanelSize(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetUIGridCellSize(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		Vector2 o = UIHelper.GetUIGridCellSize(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int EnableWidget(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(Transform), typeof(bool)))
		{
			Transform arg0 = (Transform)LuaScriptMgr.GetLuaObject(L, 1);
			bool arg1 = LuaDLL.lua_toboolean(L, 2);
			UIHelper.EnableWidget(arg0,arg1);
			return 0;
		}
		else if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(GameObject), typeof(bool)))
		{
			GameObject arg0 = (GameObject)LuaScriptMgr.GetLuaObject(L, 1);
			bool arg1 = LuaDLL.lua_toboolean(L, 2);
			UIHelper.EnableWidget(arg0,arg1);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: UIHelper.EnableWidget");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsSpriteInAtlas(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		bool o = UIHelper.IsSpriteInAtlas(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetSpriteName(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		UIHelper.SetSpriteName(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetSpriteNameNoPerfect(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		UIHelper.SetSpriteNameNoPerfect(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetWidgetColor(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(Transform), typeof(string)))
		{
			Transform arg0 = (Transform)LuaScriptMgr.GetLuaObject(L, 1);
			string arg1 = LuaScriptMgr.GetString(L, 2);
			UIHelper.SetWidgetColor(arg0,arg1);
			return 0;
		}
		else if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(Transform), typeof(LuaTable)))
		{
			Transform arg0 = (Transform)LuaScriptMgr.GetLuaObject(L, 1);
			Color arg1 = LuaScriptMgr.GetColor(L, 2);
			UIHelper.SetWidgetColor(arg0,arg1);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: UIHelper.SetWidgetColor");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetButtonSpriteName(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		UIHelper.SetButtonSpriteName(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetSpritePolygonRate(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 2);
		float arg2 = (float)LuaScriptMgr.GetNumber(L, 3);
		UIHelper.SetSpritePolygonRate(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetSpriteFillAmount(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		float arg1 = (float)LuaScriptMgr.GetNumber(L, 2);
		UIHelper.SetSpriteFillAmount(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetButtonActive(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		bool arg1 = LuaScriptMgr.GetBoolean(L, 2);
		bool arg2 = LuaScriptMgr.GetBoolean(L, 3);
		UIHelper.SetButtonActive(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetBoxCollider(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		bool arg1 = LuaScriptMgr.GetBoolean(L, 2);
		UIHelper.SetBoxCollider(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AdjustDepth(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 2);
		UIHelper.AdjustDepth(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetDepth(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 2);
		UIHelper.SetDepth(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int WidthOfWidget(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		int o = UIHelper.WidthOfWidget(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int HeightOfWidget(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		int o = UIHelper.HeightOfWidget(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SizeOfWidget(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		Vector2 o = UIHelper.SizeOfWidget(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetSizeOfWidget(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		Vector2 arg1 = LuaScriptMgr.GetVector2(L, 2);
		UIHelper.SetSizeOfWidget(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LabelSize(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		Vector2 o = UIHelper.LabelSize(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddToggle(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		object arg1 = LuaScriptMgr.GetVarObject(L, 2);
		UIHelper.AddToggle(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetToggleState(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		bool arg1 = LuaScriptMgr.GetBoolean(L, 2);
		UIHelper.SetToggleState(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetToggleState(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		bool o = UIHelper.GetToggleState(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetDragScrollViewTarget(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		Transform arg1 = (Transform)LuaScriptMgr.GetUnityObject(L, 2, typeof(Transform));
		UIHelper.SetDragScrollViewTarget(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CalculateWidgetBoundsSize(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		bool arg1 = LuaScriptMgr.GetBoolean(L, 2);
		Vector3 o = UIHelper.CalculateWidgetBoundsSize(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Floating(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		Vector3 arg1 = LuaScriptMgr.GetVector3(L, 2);
		Vector3 arg2 = LuaScriptMgr.GetVector3(L, 3);
		float arg3 = (float)LuaScriptMgr.GetNumber(L, 4);
		UIHelper.Floating(arg0,arg1,arg2,arg3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AlphaTweening(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		float arg1 = (float)LuaScriptMgr.GetNumber(L, 2);
		float arg2 = (float)LuaScriptMgr.GetNumber(L, 3);
		float arg3 = (float)LuaScriptMgr.GetNumber(L, 4);
		UIHelper.AlphaTweening(arg0,arg1,arg2,arg3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int TweenPositionBegin(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		float arg1 = (float)LuaScriptMgr.GetNumber(L, 2);
		Vector3 arg2 = LuaScriptMgr.GetVector3(L, 3);
		UIHelper.TweenPositionBegin(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int TweenPositionPlayForward(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		bool arg1 = LuaScriptMgr.GetBoolean(L, 2);
		UIHelper.TweenPositionPlayForward(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int TweenPositionPlayReverse(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		bool arg1 = LuaScriptMgr.GetBoolean(L, 2);
		UIHelper.TweenPositionPlayReverse(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetTweenPositionOnFinish(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		object arg1 = LuaScriptMgr.GetVarObject(L, 2);
		UIHelper.SetTweenPositionOnFinish(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetTweenerTime(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		float arg2 = (float)LuaScriptMgr.GetNumber(L, 3);
		float arg3 = (float)LuaScriptMgr.GetNumber(L, 4);
		UIHelper.SetTweenerTime(arg0,arg1,arg2,arg3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetCirclePos(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 2);
		UIHelper.SetCirclePos(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnDragEndCircle(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		object arg1 = LuaScriptMgr.GetVarObject(L, 2);
		UIHelper.OnDragEndCircle(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnDragCircle(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		object arg1 = LuaScriptMgr.GetVarObject(L, 2);
		UIHelper.OnDragCircle(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnDragStartCircle(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		object arg1 = LuaScriptMgr.GetVarObject(L, 2);
		UIHelper.OnDragStartCircle(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddPressRepeating(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		LuaFunction arg1 = LuaScriptMgr.GetLuaFunction(L, 2);
		UIEventListener o = UIHelper.AddPressRepeating(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddDragOver(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		LuaFunction arg1 = LuaScriptMgr.GetLuaFunction(L, 2);
		UIEventListener o = UIHelper.AddDragOver(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetAutoActiveCout(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 2);
		UIHelper.SetAutoActiveCout(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SwitchSideMenuIn(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		LuaFunction arg0 = LuaScriptMgr.GetLuaFunction(L, 1);
		UIHelper.SwitchSideMenuIn(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SwitchSideMenuOut(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		LuaFunction arg0 = LuaScriptMgr.GetLuaFunction(L, 1);
		UIHelper.SwitchSideMenuOut(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ScrollClippedPanelTo(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		Vector3 arg1 = LuaScriptMgr.GetVector3(L, 2);
		float arg2 = (float)LuaScriptMgr.GetNumber(L, 3);
		LuaFunction arg3 = LuaScriptMgr.GetLuaFunction(L, 4);
		UIHelper.ScrollClippedPanelTo(arg0,arg1,arg2,arg3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PlayUIRainDropScreenAnime(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		Vector3 arg1 = LuaScriptMgr.GetVector3(L, 2);
		UIHelper.PlayUIRainDropScreenAnime(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int FadeUIWidgetColor(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 5);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		Color arg1 = LuaScriptMgr.GetColor(L, 2);
		Color arg2 = LuaScriptMgr.GetColor(L, 3);
		float arg3 = (float)LuaScriptMgr.GetNumber(L, 4);
		LuaFunction arg4 = LuaScriptMgr.GetLuaFunction(L, 5);
		UIHelper.FadeUIWidgetColor(arg0,arg1,arg2,arg3,arg4);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int FadeUIWidgetColorTo(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		Color arg1 = LuaScriptMgr.GetColor(L, 2);
		float arg2 = (float)LuaScriptMgr.GetNumber(L, 3);
		UIHelper.FadeUIWidgetColorTo(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetPanelAlpha(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		float arg1 = (float)LuaScriptMgr.GetNumber(L, 2);
		UIHelper.SetPanelAlpha(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetUIEventListener(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1 && LuaScriptMgr.CheckTypes(L, 1, typeof(Transform)))
		{
			Transform arg0 = (Transform)LuaScriptMgr.GetLuaObject(L, 1);
			UIEventListener o = UIHelper.GetUIEventListener(arg0);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 1 && LuaScriptMgr.CheckTypes(L, 1, typeof(GameObject)))
		{
			GameObject arg0 = (GameObject)LuaScriptMgr.GetLuaObject(L, 1);
			UIEventListener o = UIHelper.GetUIEventListener(arg0);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: UIHelper.GetUIEventListener");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetScrollViewRestrict(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		bool arg1 = LuaScriptMgr.GetBoolean(L, 2);
		UIHelper.SetScrollViewRestrict(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetPopupListItem(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		object arg1 = LuaScriptMgr.GetVarObject(L, 2);
		UIHelper.SetPopupListItem(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddPopupListDelegate(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		object arg1 = LuaScriptMgr.GetVarObject(L, 2);
		UIHelper.AddPopupListDelegate(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddPopupListItem(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		UIHelper.AddPopupListItem(arg0,arg1);
		return 0;
	}
}

