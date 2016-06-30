module("UIPVELeagueScheduleLeaveBox", package.seeall)

local leaveBox = nil;
local windowComponent = nil;

function CreateLeaveBox(containerTransform, windowcomponent)
    windowComponent = windowcomponent;

    print('leaveBox: '..tostring(leaveBox));
    if (leaveBox ~= nil) then
        if (not GameObjectActiveSelf(leaveBox)) then
            GameObjectSetActive(leaveBox.transform, true);
        end
        return;
    end

    local leaveBoxPrefab = windowComponent:GetPrefab("UIPVELeagueScheduleLeaveBox");
    leaveBox = GameObjectInstantiate(leaveBoxPrefab);
    local leaveBoxTransform = leaveBox.transform;
    leaveBoxTransform.parent = containerTransform;
    leaveBoxTransform.localPosition = Vector3.zero;
    leaveBoxTransform.localScale = Vector3.one;

    local btnConfirm = TransformFindChild(leaveBoxTransform, "Box/ConfirmButton");
    local btnCancel  = TransformFindChild(leaveBoxTransform, "Box/CancelButton");
    local btnClose   = TransformFindChild(leaveBoxTransform, "Box/CloseButton");

    Util.AddClick(btnConfirm.gameObject, OnClickBtnConfirm);
    Util.AddClick(btnCancel.gameObject, OnClickBtnCancel);
    Util.AddClick(btnClose.gameObject, OnClickBtnClose);
end

function OnClickBtnConfirm()
    local function AfterRequestPVELeagueInfo(data_)
        WindowMgr.ShowWindow(LuaConst.Const.UIPVELeague, data_);
    end
    local function AfterRequestQuitLeague()
        windowComponent:Close();
        PVELeagueData.RequestPVELeagueInfo(AfterRequestPVELeagueInfo);
    end
    PVELeagueData.RequestPVELeagueQuit(AfterRequestQuitLeague);
end

function OnClickBtnCancel()
    if (GameObjectActiveSelf(leaveBox)) then
        GameObjectSetActive(leaveBox.transform, false);
    end
end

function OnClickBtnClose()
    if (GameObjectActiveSelf(leaveBox)) then
        GameObjectSetActive(leaveBox.transform, false);
    end
end

function OnDestroy()
    leaveBox = nil;
    windowComponent = nil;
end
