module("UIWaiting", package.seeall)

require "Common/Vector3"
require "Common/UnityCommonScript"


local window = nil;
local windowComponent = nil;
local doNotShowAnime = nil;

function OnStart(gameObject, params)
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");
    doNotShowAnime = params.DoNotShowAnime;
    BindUI();
end

function OnDestroy()
    window = nil;
    windowComponent = nil;
    doNotShowAnime = nil;
end

function BindUI()
    local spritePrefab = windowComponent:GetPrefab("Sprite");
    local windowTransform = window.transform;
    local idx = 0;
    local raduis = 40;
    local minScale = 0.5;
    local maxScale = 1.5;
    if(doNotShowAnime==nil or doNotShowAnime==false)then
        while (idx < 1) do
            local clone = GameObjectInstantiate(spritePrefab);
            clone.name = idx.."";
            local transform = clone.transform;
            transform.parent = windowTransform;

            local scale = Vector3.zero;
            local scaleFactor = 0.5 * (1 - idx / 12) + 2 * idx / 12;
            scale.x = scaleFactor;
            scale.y = scaleFactor;
            scale.z = scaleFactor;

            transform.localScale = scale;
            local min = Vector3.one * minScale;
            local max = Vector3.one * maxScale;
            --UILoadingPoint.Begin(clone, 0.5, max, min);

            local pos = Vector3.zero;

            local radian = idx / 12 * 2 * 3.14159;
            pos.x = -1 * raduis * math.sin(radian);
            pos.y = raduis * math.cos(radian);
            transform.localPosition = pos;
            idx = idx + 1;
        end
    end
end