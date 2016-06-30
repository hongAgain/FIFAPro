module("UIPVELeagueJoinBox", package.seeall)

local joinBox = nil;
local windowComponent = nil;

function CreateJoinBox(containerTransform, windowcomponent, selectedLeagueInfo)
    windowComponent = windowcomponent;

    print('joinBox: '..tostring(joinBox));
    if (joinBox ~= nil) then
        if (not GameObjectActiveSelf(joinBox)) then
            AddOrChangeClickParameters(
                TransformFindChild(joinBox.transform, "Box/ConfirmButton").gameObject,
                OnClickBtnConfirm,
                { id=selectedLeagueInfo.id }
            );
            UIHelper.SetLabelTxt(
                TransformFindChild(joinBox.transform, "Box/BG/Desc"),
                selectedLeagueInfo.name
            );
            GameObjectSetActive(joinBox.transform, true);
        end
        return;
    end

    local joinBoxPrefab = windowComponent:GetPrefab("UIPVELeagueJoinBox");
    joinBox = GameObjectInstantiate(joinBoxPrefab);
    local joinBoxTransform = joinBox.transform;
    joinBoxTransform.parent = containerTransform;
    joinBoxTransform.localPosition = Vector3.zero;
    joinBoxTransform.localScale = Vector3.one;

    local btnConfirm = TransformFindChild(joinBoxTransform, "Box/ConfirmButton");
    local btnCancel  = TransformFindChild(joinBoxTransform, "Box/CancelButton");
    local btnClose   = TransformFindChild(joinBoxTransform, "Box/CloseButton");
    local labelDesc  = TransformFindChild(joinBoxTransform, "Box/BG/Desc");

    AddOrChangeClickParameters(btnConfirm.gameObject, OnClickBtnConfirm, { id=selectedLeagueInfo.id });
    Util.AddClick(btnCancel.gameObject, OnClickBtnCancel);
    Util.AddClick(btnClose.gameObject, OnClickBtnClose);
    UIHelper.SetLabelTxt(labelDesc, selectedLeagueInfo.name);
end

function OnClickBtnConfirm(go)
    local listener = UIHelper.GetUIEventListener(go);
    if (listener == nil or listener.parameter == nil) then
        return;
    end
    local paramId = listener.parameter.id;
    if (paramId == nil) then
        return;
    end

    local function AfterRequestJoin(data)
        WindowMgr.ShowWindow(LuaConst.Const.UIPVELeagueSchedule, data);
        windowComponent:Close();
        OnDestroy();
    end
    PVELeagueData.RequestPVELeagueSelect(paramId, AfterRequestJoin);
end

function OnClickBtnCancel()
    if (GameObjectActiveSelf(joinBox)) then
        GameObjectSetActive(joinBox.transform, false);
    end
end

function OnClickBtnClose()
    if (GameObjectActiveSelf(joinBox)) then
        GameObjectSetActive(joinBox.transform, false);
    end
end

function OnDestroy()
    joinBox = nil;
    windowComponent = nil;
end
