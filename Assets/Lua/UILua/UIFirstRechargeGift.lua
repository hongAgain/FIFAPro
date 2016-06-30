module("UIFirstRechargeGift", package.seeall)

local windowComponent = nil;

function Init(transform, component, actId)
    local btn = TransformFindChild(transform, "Btn");
    Util.AddClick(btn.gameObject, GotoRecharge);
end

function OnShow()

end

function OnHide()

end

function OnDestroy()

end

function GotoRecharge()
    WindowMgr.ShowWindow(LuaConst.Const.UIRecharge);
    -- UIActivity.Close();
    UIActivity.TryCloseUI();
end