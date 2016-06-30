module("UIPVELeagueScheduleRewardBox", package.seeall)

require "UILua/UIIconItem"

local rewardBox = nil;
local windowComponent = nil;

function CreateRewardBox(containerTransform, windowcomponent, currentSeasonRewardConfig)
    windowComponent = windowcomponent;

    if (rewardBox ~= nil) then
        if (not GameObjectActiveSelf(rewardBox)) then
            GameObjectSetActive(rewardBox.transform, true);
        end
        return;
    end

    local rewardBoxPrefab = windowComponent:GetPrefab("UIPVELeagueScheduleRewardBox");
    rewardBox = GameObjectInstantiate(rewardBoxPrefab);
    local rewardBoxTransform = rewardBox.transform;
    rewardBoxTransform.parent = containerTransform;
    rewardBoxTransform.localPosition = Vector3.zero;
    rewardBoxTransform.localScale = Vector3.one;

    local btnConfirm = TransformFindChild(rewardBoxTransform, "Box/ConfirmButton");
    local btnClose   = TransformFindChild(rewardBoxTransform, "Box/CloseButton");

    Util.AddClick(btnConfirm.gameObject, OnClickBtnConfirm);
    Util.AddClick(btnClose.gameObject, OnClickBtnClose);

    local rewardBoxRewardGrid1 = TransformFindChild(rewardBoxTransform, "Box/BGTop/Reward1");
    local rewardBoxRewardGrid2 = TransformFindChild(rewardBoxTransform, "Box/BGTop/Reward2");
    local rewardBoxRewardGrid3 = TransformFindChild(rewardBoxTransform, "Box/BGTop/Reward3");
    local rewardLength = 0;
    local halfRewardLength = 0;
    local rewards = nil;
    local param = { scale = 0.4, offsetDepth = 5 };
    for i, v in pairs(currentSeasonRewardConfig) do
        rewards = v.settlement_reward;
        rewardLength = table.getn(rewards);
        halfRewardLength = rewardLength / 2;
        if (v.start == 1) then
            for j = 1, halfRewardLength, 1 do
                UIIconItem.CreateRewardIconItem(rewardBoxRewardGrid1, nil, { rewards[2*j-1], tonumber(rewards[2*j]) }, param);
            end
        elseif (v.start == 2) then
            for j = 1, halfRewardLength, 1 do
                UIIconItem.CreateRewardIconItem(rewardBoxRewardGrid2, nil, { rewards[2*j-1], tonumber(rewards[2*j]) }, param);
            end
        elseif (v.start == 3) then
            for j = 1, halfRewardLength, 1 do
                UIIconItem.CreateRewardIconItem(rewardBoxRewardGrid3, nil, { rewards[2*j-1], tonumber(rewards[2*j]) }, param);
            end
        end
    end
    UIHelper.RepositionGrid(rewardBoxRewardGrid1);
    UIHelper.RepositionGrid(rewardBoxRewardGrid2);
    UIHelper.RepositionGrid(rewardBoxRewardGrid3);
end

function OnClickBtnConfirm()
    if (GameObjectActiveSelf(rewardBox)) then
        GameObjectSetActive(rewardBox.transform, false);
    end
end

function OnClickBtnClose()
    if (GameObjectActiveSelf(rewardBox)) then
        GameObjectSetActive(rewardBox.transform, false);
    end
end

function OnDestroy()
    rewardBox = nil;
    windowComponent = nil;
end
