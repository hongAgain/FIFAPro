module("UITotalRecharge", package.seeall)

require "Common/CommonScript"
require "Common/UnityCommonScript"

local windowComponent = nil;
local scrollRoot = nil;
local rechargeNr = nil;
local needRechargeNr = nil;
local sortedTB = nil;

function Init(transform, component, actId)
    windowComponent = component;
    windowComponent:AdjustSelfPanelDepth();
    DataSystemScript.RegisterMsgHandler(MsgID.tb.TotalRecharge, OnGetTotalRechargeData);
    
    scrollRoot = TransformFindChild(transform, "Scroll View");
    rechargeNr = TransformFindChild(transform, "Label - RechargeNr");
    needRechargeNr = TransformFindChild(transform, "Label - NeedRecharge");

    local tb = Config.GetTemplate(Config.PayTotal());
    sortedTB = CommonScript.QuickSort(tb, SortById);

    local have = false;
    for k, v in ipairs(sortedTB) do
        local nr = tonumber(v.id);
        if (Role.Get_rmb() < nr) then
            have = true;
            UIHelper.SetLabelTxt(needRechargeNr, "[FFC400]"..tostring(nr - Role.Get_rmb().."[-]"));
            break;
        end
    end

    if (have == false) then
        UIHelper.SetLabelTxt(needRechargeNr, "");
    end
    
    UIHelper.SetLabelTxt(rechargeNr, tostring(Role.Get_rmb()));
    
    local gotoRecharge = TransformFindChild(transform, "Btn");
    Util.AddClick(gotoRecharge.gameObject, GotoRecharge);
    
    local param = {};
    param["id"] = actId;
    DataSystemScript.RequestWithParams(LuaConst.Const.ActiveData, param, MsgID.tb.TotalRecharge);
end

function OnShow()
end

function OnHide()
end

function OnDestroy()
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.TotalRecharge, OnGetTotalRechargeData);
end

function OnGetTotalRechargeData(code, data)
    local prefab = windowComponent:GetPrefab("IconBG");

    local idx = 0;
    for k, v in ipairs(sortedTB) do
        local clone = AddChild(prefab, scrollRoot);
        local _transform = clone.transform;
        _transform.localPosition = NewVector3(-165 + idx * 110, -15, 0);
        local cost = TransformFindChild(_transform, "Label");
        UIHelper.SetLabelTxt(cost, v.id);
        idx = idx + 1;

        if (idx == #sortedTB) then
            local arrow = TransformFindChild(_transform, "Sprite - Arrow");
            arrow.gameObject:SetActive(false);
        end
    end
end

function SortById(a, b)
    local ida = tonumber(a.id);
    local idb = tonumber(b.id);
    if (ida < idb) then
        return -1;
    elseif (ida == idb) then
        return 0;
    else
        return 1;
    end
end

function GotoRecharge()
    WindowMgr.ShowWindow(LuaConst.Const.UIRecharge);
    -- UIActivity.Close();
    UIActivity.TryCloseUI();
end
