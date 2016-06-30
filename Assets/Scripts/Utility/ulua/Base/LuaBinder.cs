using System;
using System.Collections.Generic;

public static class LuaBinder
{
	public static List<string> wrapList = new List<string>();
	public static void Bind(IntPtr L, string type = null)
	{
		if (type == null || wrapList.Contains(type)) return;
		wrapList.Add(type); type += "Wrap";
		switch (type) {
			case "AnimationWrap": AnimationWrap.Register(L); break;
			case "AppConstWrap": AppConstWrap.Register(L); break;
			case "ApplicationWrap": ApplicationWrap.Register(L); break;
			case "AssetBundleWrap": AssetBundleWrap.Register(L); break;
			case "AudioMgrWrap": AudioMgrWrap.Register(L); break;
			case "BehaviourWrap": BehaviourWrap.Register(L); break;
			case "CameraWrap": CameraWrap.Register(L); break;
			case "CoachHelperWrap": CoachHelperWrap.Register(L); break;
			case "Common_Log_LogManagerWrap": Common_Log_LogManagerWrap.Register(L); break;
			case "Common_Tables_TableManagerWrap": Common_Tables_TableManagerWrap.Register(L); break;
			case "Common_Tables_TableWordFilterWrap": Common_Tables_TableWordFilterWrap.Register(L); break;
			case "ComponentWrap": ComponentWrap.Register(L); break;
			case "DataSystemWrap": DataSystemWrap.Register(L); break;
			case "DelayDealWrap": DelayDealWrap.Register(L); break;
			case "GameMainWrap": GameMainWrap.Register(L); break;
			case "GameObjectWrap": GameObjectWrap.Register(L); break;
			case "GUILayoutWrap": GUILayoutWrap.Register(L); break;
			case "GUIWrap": GUIWrap.Register(L); break;
			case "LuaServerTimeWrap": LuaServerTimeWrap.Register(L); break;
			case "MonoBehaviourWrap": MonoBehaviourWrap.Register(L); break;
			case "NetWorkHandlerWrap": NetWorkHandlerWrap.Register(L); break;
			case "ObjectWrap": ObjectWrap.Register(L); break;
			case "RectWrap": RectWrap.Register(L); break;
			case "RoleHelperWrap": RoleHelperWrap.Register(L); break;
			case "SDKMgrWrap": SDKMgrWrap.Register(L); break;
			case "System_ObjectWrap": System_ObjectWrap.Register(L); break;
			case "TimeWrap": TimeWrap.Register(L); break;
			case "TransformWrap": TransformWrap.Register(L); break;
			case "TutorialWrap": TutorialWrap.Register(L); break;
			case "TweenAlphaWrap": TweenAlphaWrap.Register(L); break;
			case "TweenColorWrap": TweenColorWrap.Register(L); break;
			case "TweenPositionWrap": TweenPositionWrap.Register(L); break;
			case "TweenScaleWrap": TweenScaleWrap.Register(L); break;
			case "UIBaseWindowLuaWrap": UIBaseWindowLuaWrap.Register(L); break;
			case "UIBasicSpriteWrap": UIBasicSpriteWrap.Register(L); break;
			case "UIEventListenerWrap": UIEventListenerWrap.Register(L); break;
			case "UIHelperWrap": UIHelperWrap.Register(L); break;
			case "UILuaTweenWrap": UILuaTweenWrap.Register(L); break;
			case "UIRectWrap": UIRectWrap.Register(L); break;
			case "UIScoutAnimWrap": UIScoutAnimWrap.Register(L); break;
			case "UISpriteWrap": UISpriteWrap.Register(L); break;
			case "UIWidgetWrap": UIWidgetWrap.Register(L); break;
			case "UtilWrap": UtilWrap.Register(L); break;
			case "WindowMgrWrap": WindowMgrWrap.Register(L); break;
		}
	}
}
