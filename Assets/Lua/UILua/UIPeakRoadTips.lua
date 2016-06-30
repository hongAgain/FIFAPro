--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion

module("UIPeakRoadTips", package.seeall)

local window = nil;
local windowComponent = nil;
local windowAttr = nil;
local windowPro = nil;
local windowReset = nil;

local m_currTipsType = nil;

enum_PeakRoadTips = {TipsAttr=1,TipsPro=2,TipsReset=3};

local bg = nil

local m_dataTb = {};
local Callback = nil;
function OnStart(gameObject, params)
    m_currTipsType = params.TipsType;
    m_dataTb = params.DataTb;
    Callback = params.Callback;

    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");

    BindUI();
    WindowMgr.AdjustLayer(); 
end

function BindUI()
    local transform = window.transform;

    windowAttr = windowComponent:GetPrefab("UITipsAttr");
    windowPro = windowComponent:GetPrefab("UITipsPro");
    windowReset = windowComponent:GetPrefab("UITipsReset");
    bg = TransformFindChild(transform, "Bg")
    Util.ChangeClick(bg.gameObject, BtnClose)
    if m_currTipsType == enum_PeakRoadTips.TipsAttr then
        BindUIAttr();
    elseif m_currTipsType == enum_PeakRoadTips.TipsReset then
        BindUIReset();
    elseif m_currTipsType == enum_PeakRoadTips.TipsPro then
        BindUIPro();
    end
end

function BindUIAttr()
    local itemAttr = windowComponent:GetPrefab("ItemAttr");
    local tf = InstantiatePrefab(windowAttr,window.transform).transform;
    local scrollViewGrid = TransformFindChild(tf, "ScrollViewPanel");

    for i=1,4 do
        local clone = InstantiatePrefab(itemAttr,scrollViewGrid).transform;
        clone.name = i;
        local str1 = "[7d8089]".. Util.LocalizeString("UIAttr_1Attr"..(2*i-1)).."[-]".."[9fff9f]".."  +".. Config.GetProperty(Config.RaidDFBuffTable(),tostring(m_dataTb.MaxYestoday),'att')[2*i-1].."%[-]";
        local str2 = "[7d8089]".. Util.LocalizeString("UIAttr_1Attr"..(2*i)).."[-]".."[9fff9f]".."  +".. Config.GetProperty(Config.RaidDFBuffTable(),tostring(m_dataTb.MaxYestoday),'att')[2*i].."%[-]";
        UIHelper.SetLabelTxt(TransformFindChild(clone, "1"), str1);
        UIHelper.SetLabelTxt(TransformFindChild(clone, "2"), str2);
    end


    Util.AddClick(TransformFindChild(tf, "BtnSure").gameObject, BtnCancel);
    Util.AddClick(TransformFindChild(tf, "BtnClose").gameObject, BtnClose);
end

function BindUIReset()
    local tf = InstantiatePrefab(windowReset,window.transform).transform;

    UIHelper.SetLabelTxt(TransformFindChild(tf, "Content/lbl_content"), PeakRoadData.ResetCost());
    Util.AddClick(TransformFindChild(tf, "BtnSure").gameObject, BtnSure);
    Util.AddClick(TransformFindChild(tf, "BtnCancel").gameObject, BtnCancel);
    Util.AddClick(TransformFindChild(tf, "BtnClose").gameObject, BtnClose);
end

function BindUIPro()
    local tf = InstantiatePrefab(windowPro,window.transform).transform;

    UIHelper.SetLabelTxt(TransformFindChild(tf, "Content/lbl_content"), PeakRoadData.ProCost());
    Util.AddClick(TransformFindChild(tf, "BtnSure").gameObject, BtnSure);
    Util.AddClick(TransformFindChild(tf, "BtnCancel").gameObject, BtnCancel);
    Util.AddClick(TransformFindChild(tf, "BtnClose").gameObject, BtnClose);
end

function BtnSure()
    if Callback ~= nil then
        Callback();
        Callback = nil;
        BtnClose();
    end

end

function BtnCancel()
    BtnClose();
end

function BtnClose()
    ExitUIPeakRoadTips();
end

function OnDestroy()
    window = nil;
    windowComponent = nil;
end


function ExitUIPeakRoadTips()
   windowComponent:Close();
end





