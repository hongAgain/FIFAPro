module("UIGameCheckInsGetItemBox", package.seeall)

-- require "UILua/UIGameCheckInsBox"

local getItemBox = nil;

local uiButtonClose = nil;
local uiButtonGetItem = nil;

local uiLabelItemName = nil;
local uiLabelItemNum = nil;
local uiSpriteItemIcon = nil;

local windowComponent = nil;

function CreateGetItemBox( containerTransform, windowcomponent, name,icon,num )
	-- body
    windowComponent = windowcomponent;
	if(getItemBox == nil) then
		--instantiate one
		local getItemBoxPrefab = windowComponent:GetPrefab(UIGameCheckInsBox.CheckInSettings.prefabNameCheckInGetItemBox);
		--instantiate prefab and initialize it
		getItemBox = GameObjectInstantiate(getItemBoxPrefab);
		getItemBox.transform.parent = containerTransform;
    	getItemBox.transform.localPosition = Vector3.zero;
    	getItemBox.transform.localScale = Vector3.one;

    	uiButtonGetItem = TransformFindChild(getItemBox.transform,"ButtonGetItem");
		Util.AddClick(uiButtonGetItem.gameObject,OnClickGet);
    	uiButtonClose = TransformFindChild(getItemBox.transform,"ButtonClose");
		Util.AddClick(uiButtonClose.gameObject,OnClickClose);
		uiLabelItemName = TransformFindChild(getItemBox.transform,"Name");
		uiLabelItemNum = TransformFindChild(getItemBox.transform,"Num");
		uiSpriteItemIcon = TransformFindChild(getItemBox.transform,"Icon");
	end
	--active it
	if(not GameObjectActiveSelf(getItemBox)) then
    	GameObjectSetActive(getItemBox.transform,true);
    end
	-- UIHelper.SetLabelTxt(uiLabelItemName,name);
	UIHelper.SetLabelTxt(uiLabelItemName,"不要忘了配置道具信息");
	UIHelper.SetLabelTxt(uiLabelItemNum,"x"..num);
	-- UIHelper.SetLabelTxt(uiSpriteItemIcon,icon);
end

function OnClickGet()
	OnClickClose();
end

function OnClickClose()
	if(GameObjectActiveSelf(getItemBox)) then
    	GameObjectSetActive(getItemBox.transform,false);
    end
end

function OnDestroy()
	getItemBox = nil;
	uiButtonClose = nil;
	uiButtonGetItem = nil;
	uiLabelItemName = nil;
	uiLabelItemNum = nil;
	uiSpriteItemIcon = nil;
	windowComponent = nil;
end
