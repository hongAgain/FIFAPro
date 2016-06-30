--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion

module("UIPlayerPot", package.seeall)

local window = nil;
local windowComponent = nil;
local windowWaitingPot = nil;

local m_potData = nil;
local m_potType = nil;
local m_potId = nil; -- playerId
local m_callback = nil;
local m_noAutoBtnMoney = nil;
local m_noAutoBtnSure = nil;
local spr_btnMoney = nil;
local lbl_type = nil;
local lbl_more = nil;
local m_scrollView = nil;
local m_autoBtnStart = nil;
local m_autoBtnSure = nil;
local m_selectType = nil;
local m_selectMore = nil;
local m_lastTargetLv = nil;



local m_attrName = {};
local m_attrValue = {};
local m_attrStatus = {};
local m_selectCondition = {};

local tfItemTb = {}
local timerId = nil;
local yHeight = 246;
local potDataMsg = {};
local beforeAttr1Pot = {};
function OnStart(gameObject, params)
    m_potType = params.PotType;
    m_potData = params.PotData;
    m_potId = params.PotId;
    m_callback = params.Callback;
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");

    BindUI();
    WindowMgr.AdjustLayer();
end

function BindUI()
    local transform = window.transform;

    m_lastTargetLv = PlayerPotData.GetTargetPotLv(m_potId);
    Util.SetUITexture(TransformFindChild(transform,"tex_bg"),LuaConst.Const.PlayerSystem,"Container_bg_2", true);
    Util.AddClick(TransformFindChild(transform, "BtnClose").gameObject, BtnClose);

    if m_potType == PlayerPotData.enum_PotType.AutoPot then
        BindUIAutoPot();
    else
        BindUINoAutoPot();
    end
end

function BindUINoAutoPot()
    local clone = InstantiatePrefab(windowComponent:GetPrefab("NoAutoPot"),window.transform).transform;
    local title,icon;
    if m_potType == PlayerPotData.enum_PotType.Normal then
        title = Util.LocalizeString("UIPot_NormalPot");
        icon = "Icon_Money";
    elseif m_potType == PlayerPotData.enum_PotType.Rmb then
        title = Util.LocalizeString("UIPot_RmbPot");
        icon = "Icon_RMB";
    elseif m_potType == PlayerPotData.enum_PotType.King then
        title = Util.LocalizeString("UIPot_KingPot");
        icon = "Icon_RMB";
    end
    UIHelper.SetLabelTxt(TransformFindChild(window.transform,"LabelTitle"),title);
    UIHelper.SetLabelTxt(TransformFindChild(clone,"AutoPotItem/LabelTitle"),title);
    UIHelper.SetSpriteNameNoPerfect(TransformFindChild(clone,"BtnStartPot/LabelMoney/Sprite"),icon);

    m_attrName = {};
    m_attrValue = {};
    m_attrStatus = {};
    for i=1,6 do
        table.insert(m_attrName,TransformFindChild(clone,"AutoPotItem/"..i));
        table.insert(m_attrValue,TransformFindChild(clone,"AutoPotItem/"..i.."/Num"));
        table.insert(m_attrStatus,TransformFindChild(clone,"AutoPotItem/"..i.."/Sprite"));
    end

    m_noAutoBtnMoney = TransformFindChild(clone,"BtnStartPot/LabelMoney")
    m_noAutoBtnSure =  TransformFindChild(clone, "BtnSurePot");

    Util.AddClick(m_noAutoBtnSure.gameObject, BtnNoAutoSure);
    Util.AddClick(TransformFindChild(clone, "BtnStartPot").gameObject, BtnNoAutoPotStart);   

    RefreshNoAutoPot();
end
function BindUIAutoPot()
    local clone = InstantiatePrefab(windowComponent:GetPrefab("AutoPot"),window.transform).transform;
    windowWaitingPot = TransformFindChild(clone, "WaitingPot");
    m_selectType = 0;
    m_selectMore = 50; -- 50
    m_selectCondition = {1,1,1,1,1,1,1};
    lbl_type = TransformFindChild(clone, "PotType/Type");
    lbl_more = TransformFindChild(clone, "PotMore/Num");
    m_scrollView = TransformFindChild(clone,"ResultRoot/ScrollViewPanel");
    m_noAutoBtnMoney = TransformFindChild(clone,"LabelMoney")
    spr_btnMoney = TransformFindChild(clone,"LabelMoney/Sprite")
    UIHelper.SetLabelTxt(TransformFindChild(window.transform,"LabelTitle"),Util.LocalizeString("UIPot_AutoPot"));
    m_autoBtnStart = TransformFindChild(clone,"BtnStartPot")
    m_autoBtnSure =  TransformFindChild(clone, "BtnSurePot");
    Util.AddClick(m_autoBtnStart.gameObject, BtnAutoStart);
    Util.AddClick(m_autoBtnSure.gameObject, BtnAutoSure);
    Util.AddClick(TransformFindChild(clone, "PotType/Left").gameObject, BtnTypeLeft);
    Util.AddClick(TransformFindChild(clone, "PotType/Right").gameObject, BtnTypeRight);
    Util.AddClick(TransformFindChild(clone, "PotMore/Left").gameObject, BtnMoreLeft);
    Util.AddClick(TransformFindChild(clone, "PotMore/Right").gameObject, BtnMoreRight);
    for i=0,6 do
        Util.AddClick(TransformFindChild(clone, "LabelTips/"..i).gameObject, BtnCondition);
        if i == 0 then
            UIHelper.SetLabelTxt(TransformFindChild(clone, "LabelTips/"..i),Util.LocalizeString("UIPot_AllAttr"));
        else
            UIHelper.SetLabelTxt(TransformFindChild(clone, "LabelTips/"..i),HeroData.GetAttr1Name(m_potId,i));
        end
    end

    for i=1,6 do
        UIHelper.SetLabelTxt(TransformFindChild(clone,"WaitingPot/"..i),HeroData.GetAttr1Name(m_potId,i));
        UIHelper.SetLabelTxt(TransformFindChild(clone,"WaitingPot/"..i.."/Num"),HeroData.GetAttr1Pot(m_potId,i));
    end

    RefreshAutoPot();
end

function RefreshNoAutoPot()
    UIHelper.SetLabelTxt(m_noAutoBtnMoney,PlayerPotData.GetPotCost(m_potType,1));
    for i=1,#m_attrName do
        local index = HeroData.GetAttr1Index(m_potId,i);
        UIHelper.SetLabelTxt(m_attrName[i],HeroData.GetAttr1Name(m_potId,i));
        if m_potData["pot"][index] == 0 then
            UIHelper.SetLabelTxt(m_attrValue[i],HeroData.GetAttr1Pot(m_potId,i));
        elseif m_potData["pot"][index] > 0 then
            UIHelper.SetLabelTxt(m_attrValue[i],HeroData.GetAttr1Pot(m_potId,i).."      +"..m_potData["pot"][index]);
        else
            UIHelper.SetLabelTxt(m_attrValue[i],HeroData.GetAttr1Pot(m_potId,i).."      "..m_potData["pot"][index]);
        end
        
        if m_potData["pot"][index] > 0 then
            UIHelper.SetSpriteName(m_attrStatus[i],"Common_arrow_up")
        elseif m_potData["pot"][index] < 0 then
            UIHelper.SetSpriteName(m_attrStatus[i],"Common_arrow_down")
        else
            UIHelper.SetSpriteName(m_attrStatus[i]," ");
        end
    end

end

function BtnNoAutoPotStart()
    if PlayerPotData.IsEnoughMoney(m_potType,1) then
        NoAutoPot();
    end
end

function NoAutoPot()
    local OnPot = function(data_)
        m_potData = data_;
        RefreshNoAutoPot();
        GameObjectSetActive(m_noAutoBtnSure,true);
    end;

    Hero.ReqHeroPot(m_potId,m_potType,OnPot);
end

function BtnNoAutoSure()
    local OnSurePot = function()
        for i=1,#m_attrName do
            UIHelper.SetLabelTxt(m_attrName[i],HeroData.GetAttr1Name(m_potId,i));
            UIHelper.SetLabelTxt(m_attrValue[i],HeroData.GetAttr1Pot(m_potId,i));
            UIHelper.SetSpriteName(m_attrStatus[i]," ");
        end
--        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("UIPot_SaveSuccess") });
        GameObjectSetActive(m_noAutoBtnSure,false);
        CheckFinishTarget();
    end;

    Hero.ReqHeroCpot(m_potId,OnSurePot);
end

function CheckFinishTarget()
    if m_lastTargetLv < PlayerPotData.GetTargetPotLv(m_potId) then
        WindowMgr.ShowWindow(LuaConst.Const.UIPotTips,{PlayerId=m_potId,
                                                        PotType=PlayerPotData.enum_PotType.TargetFinish,
                                                        PotLv=m_lastTargetLv});
        m_lastTargetLv = PlayerPotData.GetTargetPotLv(m_potId)
    end
end

-----------  AutoPot
function RefreshAutoPot()
    UIHelper.SetLabelTxt(lbl_more,m_selectMore);

    RefreshAutoMoney();
end

function BtnTypeLeft()
    if m_selectType == 0 then
        m_selectType = 2;
    else
        m_selectType = m_selectType - 1;
    end
    RefreshAutoMoney();
end
function BtnTypeRight()
    if m_selectType == 3 then
        m_selectType = 0;
    else
        m_selectType = m_selectType + 1;
    end
    RefreshAutoMoney();
end
function BtnMoreLeft()
    if m_selectMore == 1 then
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("UIPot_MinTimes") });
        return;
    end
    m_selectMore = m_selectMore - 1;
    UIHelper.SetLabelTxt(lbl_more,m_selectMore);
    RefreshAutoMoney();
end
function BtnMoreRight()
    if m_selectMore == 50 then
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("UIPot_MaxTimes") });
        return;
    end
    m_selectMore = m_selectMore + 1;
    UIHelper.SetLabelTxt(lbl_more,m_selectMore);
    RefreshAutoMoney();
end
function BackupAttr1Pot()
    beforeAttr1Pot = {};
    for i=1,6 do
        local index = HeroData.GetAttr1Index(m_potId,i);
        table.insert(beforeAttr1Pot,HeroData.GetAttr1Pot(m_potId,index));
    end
end
function BtnAutoStart()
    if not PlayerPotData.IsEnoughMoney(PlayerPotData.GetEnum2Num(m_selectType),m_selectMore) then
        return;
    end

    local OnPots = function(data_)
        GameObjectSetActive(windowWaitingPot,false);
        GameObjectSetActive(m_autoBtnStart,false);
        GameObjectSetActive(m_autoBtnSure,true);
        InitAutoPotItem(data_);
    end;

    local opt = "";
    for i=1,#m_selectCondition do
        if m_selectCondition[i] ~= 0 then
            opt = opt.. HeroData.GetAttr1Index(m_potId,i-1)..",";
        end
    end
    print("type/moer: "..m_selectType .."/".. m_selectMore)
    BackupAttr1Pot();
    Hero.ReqHeroPots(m_potId,m_selectType,m_selectMore,opt,OnPots);
end
function InstantiatePot()
    local clone = GameObject.Instantiate(windowComponent:GetPrefab("AutoPotItem")).transform;
    clone.parent = m_scrollView;
    clone.localPosition = NewVector3(0,-yHeight,0);
    clone.localScale = Vector3.one;
    table.insert(tfItemTb,clone);
    UIHelper.SetLabelTxt(TransformFindChild(clone,"LabelTitle"),string.format(Util.LocalizeString("UIPot_potTimes"),#tfItemTb));
    if potDataMsg[#tfItemTb]["r"] == 1 then
        GameObjectSetActive(TransformFindChild(clone,"LabelQuit"),false);
    else
        GameObjectSetActive(TransformFindChild(clone,"LabelSave"),false);
    end    
    for i=1,6 do
        local index = HeroData.GetAttr1Index(m_potId,i);
        UIHelper.SetLabelTxt(TransformFindChild(clone,i),HeroData.GetAttr1Name(m_potId,index));
        if potDataMsg[#tfItemTb]["pot"][index] == 0 then
            UIHelper.SetLabelTxt(TransformFindChild(clone,i.. "/Num"),beforeAttr1Pot[i]);
            UIHelper.SetSpriteName(TransformFindChild(clone,i.. "/Sprite")," ");
        elseif potDataMsg[#tfItemTb]["pot"][index] > 0 then
            UIHelper.SetLabelTxt(TransformFindChild(clone,i.. "/Num"),beforeAttr1Pot[i].."      +"..potDataMsg[#tfItemTb]["pot"][index]);
            UIHelper.SetSpriteName(TransformFindChild(clone,i.. "/Sprite"),"Common_arrow_up")
        else
            UIHelper.SetLabelTxt(TransformFindChild(clone,i.. "/Num"),beforeAttr1Pot[i].."      "..potDataMsg[#tfItemTb]["pot"][index]);
            UIHelper.SetSpriteName(TransformFindChild(clone,i.. "/Sprite"),"Common_arrow_down")
        end
    end

end
function InstantiatePotTotal()
    local totalAttr1Pot = {0,0,0,0,0,0,0,0};
    for i=1,#potDataMsg do
        if potDataMsg[i]["r"] == 1 then
            for j=1,#potDataMsg[i]["pot"] do
                totalAttr1Pot[j] = totalAttr1Pot[j] + potDataMsg[i]["pot"][j];
            end
        end
    end

    local clone = GameObject.Instantiate(windowComponent:GetPrefab("AutoPotItem")).transform;
    clone.parent = m_scrollView;
    clone.localPosition = NewVector3(0,-yHeight,0);
    clone.localScale = Vector3.one;
    table.insert(tfItemTb,clone);
    UIHelper.SetLabelTxt(TransformFindChild(clone,"LabelTitle"),Util.LocalizeString("UIPot_potTotal"));
    GameObjectSetActive(TransformFindChild(clone,"LabelQuit"),false);
    GameObjectSetActive(TransformFindChild(clone,"LabelSave"),false);
    UIHelper.SetWidgetColor(TransformFindChild(clone,"LeftBg"),Color.New(80/255,76/255,65/255, 1));
    UIHelper.SetWidgetColor(TransformFindChild(clone,"LeftBg/TitleBg"),Color.New(178/255,150/255,82/255, 1));
   
    for i=1,6 do
        local index = HeroData.GetAttr1Index(m_potId,i);
        UIHelper.SetLabelTxt(TransformFindChild(clone,i),HeroData.GetAttr1Name(m_potId,index));
        if totalAttr1Pot[index] == 0 then
            UIHelper.SetLabelTxt(TransformFindChild(clone,i.. "/Num"),beforeAttr1Pot[i]);
            UIHelper.SetSpriteName(TransformFindChild(clone,i.. "/Sprite")," ");
        elseif totalAttr1Pot[index] > 0 then
            UIHelper.SetLabelTxt(TransformFindChild(clone,i.. "/Num"),beforeAttr1Pot[i].."      +"..totalAttr1Pot[index]);
            UIHelper.SetSpriteName(TransformFindChild(clone,i.. "/Sprite"),"Common_arrow_up")
        else
            UIHelper.SetLabelTxt(TransformFindChild(clone,i.. "/Num"),beforeAttr1Pot[i].."      "..totalAttr1Pot[index]);
            UIHelper.SetSpriteName(TransformFindChild(clone,i.. "/Sprite"),"Common_arrow_down")
        end
    end

    CheckFinishTarget();
end
function InitAutoPotItem(data_)
--    print("//////".. #data_["pots"])
    tfItemTb = {};
    timerId = nil;
    potDataMsg = data_["pots"];

    local CreateItem = function()
        InstantiatePot(); 
           
        for i=1,#tfItemTb do
            UIHelper.SpringPanelBegin(tfItemTb[i],NewVector3(0,(#tfItemTb-i)*yHeight,0),15);
        end 
        if #tfItemTb >= #potDataMsg and timerId ~= nil then
            LuaTimer.RmvTimer(timerId);
            timerId = nil;
            InstantiatePotTotal();
        end

        for i=1,#tfItemTb do
            UIHelper.SpringPanelBegin(tfItemTb[i],NewVector3(0,(#tfItemTb-i)*yHeight,0),15);
        end   
    end;

    timerId = LuaTimer.AddTimer(true,-1,CreateItem);
end

function BtnAutoSure()
    if timerId ~= nil then
        LuaTimer.RmvTimer(timerId);
        timerId = nil;
--        print(#tfItemTb.. "/".. #potDataMsg)
        for i=#tfItemTb+1,#potDataMsg do
            InstantiatePot()
        end
        for i=1,#tfItemTb do
            UIHelper.SpringPanelBegin(tfItemTb[i],NewVector3(0,(#tfItemTb-i)*yHeight,0),15);
        end 

        InstantiatePotTotal();

        for i=1,#tfItemTb do
            UIHelper.SpringPanelBegin(tfItemTb[i],NewVector3(0,(#tfItemTb-i)*yHeight,0),15);
        end  
    end
end

function BtnCondition(object_)
    local check = TransformFindChild(object_.transform, "Check");
    if GameObjectActiveSelf(check.gameObject) then
        GameObjectSetActive(check,false);
        m_selectCondition[object_.name+1] = 0;
    else
        GameObjectSetActive(check,true);
        m_selectCondition[object_.name+1] = 1;
    end

end

function RefreshAutoMoney()
    if m_selectType == 0 then
        UIHelper.SetSpriteNameNoPerfect(spr_btnMoney,"Icon_Money");
        UIHelper.SetLabelTxt(m_noAutoBtnMoney,PlayerPotData.GetPotCost(PlayerPotData.enum_PotType.Normal,m_selectMore));
        UIHelper.SetLabelTxt(lbl_type,Util.LocalizeString("UIPot_NormalPot"));
    elseif m_selectType == 1 then
        UIHelper.SetSpriteNameNoPerfect(spr_btnMoney,"Icon_RMB");
        UIHelper.SetLabelTxt(m_noAutoBtnMoney,PlayerPotData.GetPotCost(PlayerPotData.enum_PotType.Rmb,m_selectMore));
        UIHelper.SetLabelTxt(lbl_type,Util.LocalizeString("UIPot_RmbPot"));
    elseif m_selectType == 2 then
        UIHelper.SetSpriteNameNoPerfect(spr_btnMoney,"Icon_RMB");
        UIHelper.SetLabelTxt(m_noAutoBtnMoney,PlayerPotData.GetPotCost(PlayerPotData.enum_PotType.King,m_selectMore));
        UIHelper.SetLabelTxt(lbl_type,Util.LocalizeString("UIPot_KingPot"));
    end      

end

function BtnClose()
    ExitUIPlayerPot();
end

function OnDestroy()
    if timerId ~= nil then
        LuaTimer.RmvTimer(timerId);
        timerId = nil;
    end

    window = nil;
    windowComponent = nil;
    m_selectCondition = {};
end


function ExitUIPlayerPot()
   windowComponent:Close();
   if m_callback ~= nil then
        m_callback();
    end
end

