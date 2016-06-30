module("UIPVELeagueScheduleEndingBox", package.seeall)
require "UILua/UIIconItem"

local endingBox = nil;
local windowComponent = nil;

function CreateEndingBox(containerTransform, windowcomponent, endingInfo)
    windowComponent = windowcomponent;

    if (endingBox ~= nil) then
        if (not GameObjectActiveSelf(endingBox)) then
            GameObjectSetActive(endingBox.transform, true);
        end
        return;
    end

    local endingBoxPrefab = windowComponent:GetPrefab("UIPVELeagueScheduleEndingBox");
    endingBox = GameObjectInstantiate(endingBoxPrefab);
    local endingBoxTransform = endingBox.transform;
    endingBoxTransform.parent = containerTransform;
    endingBoxTransform.localPosition = Vector3.zero;
    endingBoxTransform.localScale = Vector3.one;

    local labelDesc   = TransformFindChild(endingBoxTransform, "Box/BGTop/Desc");
    local labelWin    = TransformFindChild(endingBoxTransform, "Box/ResultInfoNode/WinValue");
    local labelDraw   = TransformFindChild(endingBoxTransform, "Box/ResultInfoNode/DrawValue");
    local labelLose   = TransformFindChild(endingBoxTransform, "Box/ResultInfoNode/LoseValue");
    local labelPoint  = TransformFindChild(endingBoxTransform, "Box/ResultInfoNode/PointValue");
    local unlockNode  = TransformFindChild(endingBoxTransform, "Box/BGBottom/UnlockNode");
    local unlockValue = TransformFindChild(unlockNode, "Value");
    local endingBoxRewardGrid = TransformFindChild(endingBoxTransform, "Box/BGBottom/RewardNode/Reward");
    local btnConfirm  = TransformFindChild(endingBoxTransform, "Box/ConfirmButton");
    local btnClose    = TransformFindChild(endingBoxTransform, "Box/CloseButton");

    UIHelper.SetLabelTxt(labelDesc, "您的球队获得了本次联赛第"..endingInfo.rank.."名");
    UIHelper.SetLabelTxt(labelWin, endingInfo.winNum);
    UIHelper.SetLabelTxt(labelDraw, endingInfo.drawNum);
    UIHelper.SetLabelTxt(labelLose, endingInfo.loseNum);
    UIHelper.SetLabelTxt(labelPoint, endingInfo.point);
    if (endingInfo.unlock == nil) then
        GameObjectSetActive(unlockNode, false);
    else
        UIHelper.SetLabelTxt(unlockValue, endingInfo.unlock);
    end

    local rewardLength = table.getn(endingInfo.rewards);
    local param = { scale = 0.4, offsetDepth = 5 };
    for j = 2, rewardLength, 2 do
        UIIconItem.CreateRewardIconItem(
            endingBoxRewardGrid,
            nil,
            { endingInfo.rewards[j-1], tonumber(endingInfo.rewards[j]) },
            param
        );
    end

    UIHelper.RepositionGrid(endingBoxRewardGrid);

    Util.AddClick(btnConfirm.gameObject, OnClickBtnConfirm);
    Util.AddClick(btnClose.gameObject, OnClickBtnClose);
end

function OnClickBtnConfirm()
    JumpToChooseLeague();
end

function OnClickBtnClose()
    JumpToChooseLeague();
end

function JumpToChooseLeague()
    windowComponent:Close();
    local function AfterRequestPVELeagueInfo(data_)
        WindowMgr.ShowWindow(LuaConst.Const.UIPVELeague, data_);
    end
    PVELeagueData.RequestPVELeagueInfo(AfterRequestPVELeagueInfo);
end

function OnDestroy()
    endingBox = nil;
    windowComponent = nil;
end
