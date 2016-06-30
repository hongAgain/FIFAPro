module("UIFirstRechargeDouble", package.seeall)

require "Common/UnityCommonScript"

function Init(transform)
    local btn = TransformFindChild(transform, "Btn");
    Util.AddClick(btn.gameObject, OpenRecharge);
end

function OnShow()
    
end

function OnHide()
    
end

function OnDestroy()
end

function OpenRecharge()
    WindowMgr.ShowWindow(LuaConst.Const.UIRecharge);
    -- UIActivity.Close();
    UIActivity.TryCloseUI();
end