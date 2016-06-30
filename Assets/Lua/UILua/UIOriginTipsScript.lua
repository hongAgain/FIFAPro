--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion


module("UIOriginTipsScript", package.seeall)

require "Common/UnityCommonScript"

local window = nil;
local windowComponent = nil;
local m_haveOrigin = nil;
local m_noOrigin = nil;

local lbl_tipsOrigin = nil;

local originTb = {};

enumOriginType = {PlayerChips = 1,AdvItem = 2}
function OnStart(gameObject, params)
    print("UIOriginTipsScript.lua .. OnStart");
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");


    BindUI();

end


function BindUI()
    local transform = window.transform;

    m_haveOrigin = TransformFindChild(transform,"OriginTips/HaveOrigin");
    m_noOrigin = TransformFindChild(transform,"OriginTips/NoOrigin");

    local btnClose = TransformFindChild(transform, "OriginTips/BtnClose").gameObject;
    Util.AddClick(btnClose,BtnClose);

    lbl_tipsOrigin = TransformFindChild(transform,"OriginTips/Label");



    RefreshOrigin();
end

function RefreshOrigin()
    if m_haveOrigin ~= nil and m_noOrigin ~= nil and originTb.originType == enumOriginType.PlayerChips then
        GameObjectSetActive(m_haveOrigin,false);
        GameObjectSetActive(m_noOrigin,true);
        UIHelper.SetLabelTxt(lbl_tipsOrigin,Util.LocalizeString("ChipsOrigin")); 
    elseif m_haveOrigin ~= nil and m_noOrigin ~= nil and originTb.originType == enumOriginType.AdvItem then
        GameObjectSetActive(m_haveOrigin,false);
        GameObjectSetActive(m_noOrigin,true);
        UIHelper.SetLabelTxt(lbl_tipsOrigin,Util.LocalizeString("ItemOrigin")); 
    end

    print("Origin Id: "..originTb.id);
end

function TryOpen(argsTb_)
    originTb = argsTb_;

    WindowMgr.ShowWindow(LuaConst.Const.UIOriginTips);
    WindowMgr.AdjustLayer(); 

    RefreshOrigin();
end


function BtnClose()
    ExitOriginTips();

end


function OnHide()

end

function OnDestroy()
    print("OriginTips OnDestroy");
    m_haveOrigin = nil;
    m_noOrigin = nil;
    lbl_tipsOrigin = nil;

end



function ExitOriginTips()
    windowComponent:Close();

end



