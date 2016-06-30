module("UIGameCheckInsItemTips", package.seeall)

require "Game/GameCheckInsData"

local itemTipsBox = nil;

local uiLabelItemName = nil;
local uiLabelItemNum = nil;
local uiLabelItemHaveNum = nil;
local uiLabelItemDesc = nil;
local uiLabelItemHowToGet = nil;
local uiSpriteItemIcon = nil;

local windowComponent = nil;

function CreateItemTipsBox( containerTransform, windowcomponent, name, icon, num, haveNum, desc, dayindex )
	-- body
    windowComponent = windowcomponent;
	if(itemTipsBox == nil) then
		--instantiate one
		local getItemBoxPrefab = windowComponent:GetPrefab(UIGameCheckInsBox.CheckInSettings.prefabNameCheckInsItemTips);
		--instantiate prefab and initialize it
		itemTipsBox = GameObjectInstantiate(getItemBoxPrefab);
		itemTipsBox.transform.parent = containerTransform;
    	itemTipsBox.transform.localPosition = Vector3.zero;
    	itemTipsBox.transform.localScale = Vector3.one;

		uiLabelItemName = TransformFindChild(itemTipsBox.transform, "Name");
		uiLabelItemNum = TransformFindChild(itemTipsBox.transform, "Num");
        uiLabelItemHaveNum = TransformFindChild(itemTipsBox.transform, "HaveNum");
		uiLabelItemDesc = TransformFindChild(itemTipsBox.transform, "Desc");
		uiLabelItemHowToGet = TransformFindChild(itemTipsBox.transform, "HowToGet");
		uiSpriteItemIcon = TransformFindChild(itemTipsBox.transform, "IconRoot/Icon");
	end
	--active it
	if(not GameObjectActiveSelf(itemTipsBox)) then
		print("Open Box");
    	GameObjectSetActive(itemTipsBox.transform,true);
    end

	UIHelper.SetLabelTxt(uiLabelItemName, name);
	UIHelper.SetLabelTxt(uiLabelItemNum, "x"..num);
    UIHelper.SetLabelTxt(uiLabelItemHaveNum, GetLocalizedString("HaveNum", haveNum));
	UIHelper.SetLabelTxt(uiLabelItemDesc, desc);
	UIHelper.SetLabelTxt(uiLabelItemHowToGet, GetLocalizedString(UIGameCheckInsBox.CheckInSettings.strCheckInHowToGet, dayindex));
    Util.SetUITexture(uiSpriteItemIcon, LuaConst.Const.ItemIcon, icon, true);
end

function OnDestroy()
	itemTipsBox = nil;
	uiLabelItemName = nil;
	uiLabelItemNum = nil;
    uiLabelItemHaveNum = nil;
	uiLabelItemDesc = nil;
	uiLabelItemHowToGet = nil;
	uiSpriteItemIcon = nil;
	windowComponent = nil;
end
