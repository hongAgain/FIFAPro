module("UILogView", package.seeall)

require "Common/UnityCommonScript"

local window = nil;
local windowComponent = nil;

function OnStart(gameObject, params)
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");
end

function OnDestroy()
end

function OnShow()
end

function OnHide()
end