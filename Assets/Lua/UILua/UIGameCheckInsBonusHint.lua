module("UIGameCheckInsBonusHint", package.seeall)

require "Common/UnityCommonScript"
require "Game/GameMainScript"
require "Game/Role"

require "Game/GameCheckInsData"
--require "UILua/UIGameCheckInsBox"

local hintBox = nil;

local uiButtonClose = nil;
local uiButtonRecharge = nil;
local uiLabelHintDesc = nil;

local windowComponent = nil;

function CreateHintBox( containerTransform, windowcomponent, vipLv )
	-- body
    windowComponent = windowcomponent;
	if(hintBox == nil) then
		--instantiate one
		local hintBoxPrefab = windowComponent:GetPrefab(UIGameCheckInsBox.CheckInSettings.prefabNameCheckInBonusHintBox);
		--instantiate prefab and initialize it
		hintBox = GameObjectInstantiate(hintBoxPrefab);
		hintBox.transform.parent = containerTransform;
    	hintBox.transform.localPosition = Vector3.zero;
    	hintBox.transform.localScale = Vector3.one;

    	uiButtonRecharge = TransformFindChild(hintBox.transform,"ButtonReCharge");
		Util.AddClick(uiButtonRecharge.gameObject,OnClickRecharge);
    	uiButtonClose = TransformFindChild(hintBox.transform,"ButtonClose");
		Util.AddClick(uiButtonClose.gameObject,OnClickClose);
		uiLabelHintDesc = TransformFindChild(hintBox.transform,"HintDesc");
	end
	--active it
	if(not GameObjectActiveSelf(hintBox)) then
    	GameObjectSetActive(hintBox.transform,true);
    end
	UIHelper.SetLabelTxt(uiLabelHintDesc,GetLocalizedString(UIGameCheckInsBox.CheckInSettings.strCheckInBonusHint,vipLv));
end

function OnClickRecharge()
    OnClickClose();
	-- WindowMgr.ShowWindow(LuaConst.Const.UIRecharge);
    WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { "充值功能紧张开发中，敬请期待！" });
end

function OnClickClose()
	if(GameObjectActiveSelf(hintBox)) then
    	GameObjectSetActive(hintBox.transform,false);
    end
end

function OnDestroy()
	hintBox = nil;
	uiButtonClose = nil;
	uiButtonRecharge = nil;
	uiLabelHintDesc = nil;
	windowComponent = nil;
end
