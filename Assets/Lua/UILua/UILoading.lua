module("UILoading", package.seeall)

require "Common/Vector3"
require "Common/UnityCommonScript"

local window = nil;
local windowComponent = nil;

function OnStart(gameObject, params)
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");
end

function OnDestroy()
    window = nil;
    windowComponent = nil;
end
