module("UIBattleNameList", package.seeall)

local window = nil;
local windowComponent = nil;

function OnStart(gameObject, params)
	window = gameObject;
	windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");
    BindUI();
end

function OnDestroy()
    window = nil;
    windowComponent = nil;
end

function OnShow()
end

function OnHide()
end

function BindUI()

end

function RefreshItemList()
end

function Close()
    windowComponent:Close();
end