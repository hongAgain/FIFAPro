module("UIHeadScript", package.seeall)

require "Common/UnityCommonScript"
require "Common/CommonScript"
require "Game/Role"
require "Game/ItemSys"
require "Game/SynSys"
require "UILua/UITeamSettingBox"

local window = nil;
local mode1_Root = nil;
local mode2_Root = nil;
local mode3_Root = nil;
local tr_Root = nil;
local tr_Anchor = nil;
local popupRoot = nil;

--local windowTb = {};
local timerIdTb = {};

local lbl_gold = nil;
local lbl_diamond = nil;
local lbl_cost = nil;
local lbl_costTime = nil; 

local clubName = nil;
local clubIcon = nil;
local expBar = nil;
local clubVIP = nil;
local lvl = nil;

local m_timerId = nil;
local m_currCost = nil;
local m_currCostTime = nil;
local ui = nil;

local bgBlock = nil;

local sideMenuButtons = {}
local sideMenuSelectedIndex = nil;
local buttonMenu = nil;

local ColorSelectedName = Color.New(102/255, 204/255, 255/255, 255/255);
local ColorNormalName = Color.New(92/255, 91/255, 105/255, 255/255);
--add 1 for normal, 2 for selected
local buttonIconNamePrefix = {
    "Icon_Home",
    "Icon_Player",
    "Icon_Coach",
    "Icon_Tactics",
    "Icon_Bag",
    "Icon_Task",
    "Icon_Ranking",
}
--used for comparing
local buttonTitleKeys = {
    "UITitleKeyLobby",
    "UITitleKeyPlayerList",
    "UITitleKeyCoach",
    "UITitleKeyFormation",
    "UITitleKeyBag",
    "UITitleKeyTask",
    "UITitleKeyRankList"
}

function OnStart(gameObject, params)
    window = gameObject;
    --table.insert(windowTb,window);

    BindUI();
    SynSys.RegisterCallback("item", RefreshMoney);
end

function BindUI()

    local transform = window.transform;

    lbl_gold = TransformFindChild(transform, "TopCenter/TR/Gold/lbl_gold");    
    lbl_diamond = TransformFindChild(transform, "TopCenter/TR/Diamond/lbl_diamond");    
    lbl_cost = TransformFindChild(transform, "TopCenter/TR/Cost/lbl_cost");
    lbl_costTime = TransformFindChild(transform, "TopCenter/TR/Cost/lbl_costTime");
    UIHelper.SetLabelTxt(lbl_gold, Role.GetSB());
    UIHelper.SetLabelTxt(lbl_diamond, Role.GetGB());
    UIHelper.SetLabelTxt(lbl_cost, ItemSys.GetItemData(LuaConst.Const.Power).num .."/"..GetMaxPower());

    Util.AddClick(TransformFindChild(transform, "TopCenter/TR/Gold/BtnGold").gameObject, BtnGold);    
    Util.AddClick(TransformFindChild(transform, "TopCenter/TR/Diamond/BtnDiamond").gameObject, BtnDiamond);    
    Util.AddClick(TransformFindChild(transform, "TopCenter/TR/Cost/BtnCost").gameObject, BtnCost);    
    Util.AddClick(TransformFindChild(transform, "TopCenter/TR/Cost/BtnCostTips").gameObject, BtnCostTips);
    
    mode1_Root = TransformFindChild(transform, "TopCenter/TL-1").gameObject;
    mode2_Root = TransformFindChild(transform, "TopCenter/TL-2").gameObject;
    mode3_Root = TransformFindChild(transform, "TopCenter/TL-3").gameObject;
    tr_Root = TransformFindChild(transform, "TopCenter/TR").gameObject;
    tr_Anchor = TransformFindChild(transform, "TopCenter/TRAnchor");
    popupRoot = TransformFindChild(transform,"PopupRoot");

    function Close()
        if (ui ~= nil) then
            print("ui is not null");
            ui:Close();
        end
    end

    label_title1 = TransformFindChild(transform, "TopCenter/TL-2/Label - Title");
    label_title2 = TransformFindChild(transform, "TopCenter/TL-3/Label - Title");    
    btn_Return1 = TransformFindChild(transform, "TopCenter/TL-2/Btn - Return");
    btn_Return2 = TransformFindChild(transform, "TopCenter/TL-3/Btn - Return");    
    Util.AddClick(btn_Return1.gameObject, Close);
    Util.AddClick(btn_Return2.gameObject, Close);

    local buttonTeamSettings = TransformFindChild(transform,"TopCenter/TL-1/ButtonTeamSettings");
    AddOrChangeClickParameters(buttonTeamSettings.gameObject,OnClickBtnTeamSettings,nil);
    
    clubName = TransformFindChild(transform, "TopCenter/TL-1/Label - ClubName");    
    clubIcon = TransformFindChild(transform, "TopCenter/TL-1/IconRoot/Sprite-ClubIcon");
    clubVIP = TransformFindChild(transform, "TopCenter/TL-1/Vip");
    lvl = TransformFindChild(transform, "TopCenter/TL-1/Label - Level");
    expBar = TransformFindChild(transform,"TopCenter/TL-1/Exp");

    UIHelper.SetLabelTxt(clubName, Role.Get_name()); 
    SetIcon();
    UIHelper.SetLabelTxt(clubVIP,"vip"..Role.Get_vip());
    UIHelper.SetLabelTxt(lvl, string.format("Lv.%d", Role.Get_lv()));
    SetExpProgressBar();

    bgBlock = TransformFindChild(transform,"BGBlock");
    AddOrChangeClickParameters(bgBlock.gameObject,OnClickBlockBG,nil);
    GameObjectSetActive(bgBlock,false);

    buttonMenu = TransformFindChild(transform,"TopCenter/Btn - Menu");
    AddOrChangeClickParameters(buttonMenu.gameObject,OnClickBtnMenu,nil);

    sideMenuButtons = {};
    for i=1,7 do
        sideMenuButtons[i] = {};
        sideMenuButtons[i].transform = TransformFindChild(transform,"LeftCenter/SideMenu/Button"..i);
        sideMenuButtons[i].gameObject = sideMenuButtons[i].transform.gameObject;

        sideMenuButtons[i].Icon = TransformFindChild(sideMenuButtons[i].transform,"Icon");
        sideMenuButtons[i].Name = TransformFindChild(sideMenuButtons[i].transform,"Name");
        --sideMenuButtons[i].Hint = TransformFindChild(sideMenuButtons[i].transform,"Hint");
        sideMenuButtons[i].Selected = TransformFindChild(sideMenuButtons[i].transform,"Selected");

        UIHelper.SetSpriteName(sideMenuButtons[i].Icon,buttonIconNamePrefix[i]..1);
        UIHelper.SetLabelTxt(sideMenuButtons[i].Name,GetLocalizedString(buttonTitleKeys[i]));
        UIHelper.SetWidgetColor(sideMenuButtons[i].Name,ColorNormalName);
        --GameObjectSetActive(sideMenuButtons[i].Hint,false);
        GameObjectSetActive(sideMenuButtons[i].Selected,false);
    end

    AddOrChangeClickParameters(sideMenuButtons[1].gameObject,OnClickBtnLobby,nil);
    AddOrChangeClickParameters(sideMenuButtons[2].gameObject,OnClickBtnPlayer,nil);
    AddOrChangeClickParameters(sideMenuButtons[3].gameObject,OnClickBtnCoach,nil);
    AddOrChangeClickParameters(sideMenuButtons[4].gameObject,OnClickBtnTactics,nil);
    AddOrChangeClickParameters(sideMenuButtons[5].gameObject,OnClickBtnBag,nil);
    AddOrChangeClickParameters(sideMenuButtons[6].gameObject,OnClickBtnTask,nil);
    AddOrChangeClickParameters(sideMenuButtons[7].gameObject,OnClickBtnRank,nil);
    
    InitCost();
    UpdateCost();
    m_timerId = LuaTimer.AddTimer(false,-1,UpdateCost);
    table.insert(timerIdTb,m_timerId);
    
    Role.RegisterNameAndIconChanged(OnIconChanged, OnNameChanged);
end

function SetExpProgressBar()
    local lvNow = Role.Get_lv();
    local nowExp = Role.Get_exp();
    local lvDict = Config.GetTemplate(Config.LevelTable());
    local minExp = nil;
    local maxExp = nil;
    local value = 1;
    if(lvDict[tostring(lvNow)]~=nil)then
        minExp = lvDict[tostring(lvNow)].UExp;
    end
    if(lvDict[tostring(lvNow+1)]~=nil)then
        maxExp = lvDict[tostring(lvNow+1)].UExp;
    end
    if(minExp~=nil and maxExp~=nil)then
        value = (nowExp-minExp)/(maxExp-minExp);
    end
    UIHelper.SetProgressBar(expBar,value);
end

function InitCost()
    m_currCost = Role.Get_power();
    
    local sec = Util.GetTotalSeconds(tonumber(Role.Get_powerTime()));
    local restorCost = math.floor(sec / (5 * 60));
    m_currCostTime = sec - restorCost * (5 * 60);
end

function UpdateCost()
    if m_currCost < GetMaxPower() then
        if m_currCostTime > 0 then
            m_currCostTime = m_currCostTime - 1;
        end

        if m_currCostTime <= 0 then
            m_currCost = m_currCost + 1;
            m_currCostTime = 5*60 -1;
        end
        UIHelper.SetLabelTxt(lbl_costTime,GetStrTime(m_currCostTime));
    else
        UIHelper.SetLabelTxt(lbl_costTime,GetStrTime(0));
    end

    UIHelper.SetLabelTxt(lbl_cost, m_currCost .."/"..GetMaxPower());
end

function RefreshMoney()
    InitCost();

    UIHelper.SetLabelTxt(lbl_gold, Role.GetSB());
    UIHelper.SetLabelTxt(lbl_diamond, Role.GetGB());
end

function BtnGold()
    WindowMgr.ShowWindow(LuaConst.Const.UIHandOfMidas)
    --WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { "点金手功能紧张开发中，敬请期待！" });
end

function BtnDiamond()
    WindowMgr.ShowWindow(LuaConst.Const.UIRecharge);
    --WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { "充值功能紧张开发中，敬请期待！" });
end

function BtnCost()
    --WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { "补充体力功能紧张开发中，敬请期待！" });
    WindowMgr.ShowWindow(LuaConst.Const.UIQuickPurchase,{"Energy"});
end

function BtnCostTips()

end

function OnClickBtnMenu()
    SwitchMenuIn(nil);
    -- GameObjectSetActive(bgBlock,true);
end

function OnClickBtnTeamSettings()
    WindowMgr.ShowWindow(LuaConst.Const.UITeamSettingBox,{delegateOnChangeName = OnNameChanged,delegateOnChangeIcon = OnIconChanged});
    -- UITeamSettingBox.CreateTeamSettingBox(popupRoot, ui,UIHelper.GetPanelDepth(popupRoot)+1, OnNameChanged, OnIconChanged);
end

function OnIconChanged()
    -- Role.SetRoleIcon(clubIcon,Role.GetRoleIcon());
    SetIcon();
end

function SetIcon()
    Util.SetUITexture(clubIcon, LuaConst.Const.ClubIcon, Role.GetRoleIcon().."_2", true);
end

function OnNameChanged()
    UIHelper.SetLabelTxt(clubName, Role.Get_name());
end

function SwitchMenuIn(funcOnOver)
    UIHelper.SwitchSideMenuIn(
        function ()
            if(funcOnOver~=nil)then
                funcOnOver();
            end
        end
    );
    GameObjectSetActive(bgBlock,true); 
end

function SwitchMenuOut(funcOnOver)
    UIHelper.SwitchSideMenuOut(
        function ()
            if(funcOnOver~=nil)then
                funcOnOver();
            end
            GameObjectSetActive(bgBlock,false);
        end
    );    
end

function OnClickBlockBG()
    SwitchMenuOut(nil);
end

function OnClickBtnLobby()
    SetSelected(1,true);
    SetSelected(GetSelectedIndex(),false);
    SwitchMenuOut(function ()
        SetSelected(1,false);
        if(not IsButtonSelected(1))then
            WindowMgr.ShowWindow(LuaConst.Const.UILobby);
        end
    end);
end

function OnClickBtnPlayer()
    SetSelected(2,true);
    SetSelected(GetSelectedIndex(),false);
    SwitchMenuOut(function ()
        SetSelected(2,false);
        if(not IsButtonSelected(2))then
            WindowMgr.ShowWindow(LuaConst.Const.UIPlayerList);
        end
    end);
end

function OnClickBtnCoach()
    SetSelected(3,true);
    SetSelected(GetSelectedIndex(),false);
    SwitchMenuOut(function ()
        SetSelected(3,false);
        if(not IsButtonSelected(3))then
            -- WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { "教练系统紧张开发中，敬请期待！" });            
            WindowMgr.ShowWindow(LuaConst.Const.UICoach);
        end
    end);
end

function OnClickBtnTactics()
    SetSelected(4,true);
    SetSelected(GetSelectedIndex(),false);
    SwitchMenuOut(function ()
        SetSelected(4,false);
        if(not IsButtonSelected(4))then
            WindowMgr.ShowWindow(LuaConst.Const.UIFormation);
        end
    end);
end

function OnClickBtnBag()
    SetSelected(5,true);
    SetSelected(GetSelectedIndex(),false);
    SwitchMenuOut(function ()
        SetSelected(5,false);
        if(not IsButtonSelected(5))then
            WindowMgr.ShowWindow(LuaConst.Const.UIBag);
        end
    end);
end

function OnClickBtnTask()
    SetSelected(6,true);
    SetSelected(GetSelectedIndex(),false);
    SwitchMenuOut(function ()
        SetSelected(6,false);
        if(not IsButtonSelected(6))then
            --WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { "任务系统紧张开发中，敬请期待！" });   
            WindowMgr.ShowWindow(LuaConst.Const.UITask);
        end
    end);
end

function OnClickBtnRank()
    SetSelected(7,true);
    SetSelected(GetSelectedIndex(),false);
    SwitchMenuOut(function ()
        SetSelected(7,false);
        if(not IsButtonSelected(7))then
            WindowMgr.ShowWindow(LuaConst.Const.UIRankList);
        end
    end);
end

function GetMaxPower()
    local lv = Role.Get_lv();
    return Config.GetProperty(Config.LevelTable(), tostring(lv),"Power");
end

function GetStrTime(second_)
    local minute = math.floor(second_/60);
    local second = second_ - minute*60;

    return string.format("%02d",minute).." : "..string.format("%02d",second);
end

function RefreshInfo()
    --Exp
    SetExpProgressBar();
    --Energy

    --Level
    UIHelper.SetLabelTxt(lvl, string.format("Lv.%d", Role.Get_lv()));
    --money
    RefreshMoney();
    --vip
    UIHelper.SetLabelTxt(clubVIP,"vip"..Role.Get_vip());
end

function ChangeTRAnchorSize(iWidth)
    if(nil == tr_Anchor ) then
        return;
    end
    local kSize = UIHelper.SizeOfWidget(tr_Anchor);
    kSize.x = iWidth;
    UIHelper.SetSizeOfWidget(tr_Anchor,kSize)
end 

function SwitchMode(mode)
    if (mode == 1) then
        window:SetActive(false);
    elseif (mode == 2) then
        window:SetActive(true);
        mode1_Root:SetActive(true);
        mode2_Root:SetActive(false);
        mode3_Root:SetActive(false);
        ChangeTRAnchorSize(63);
        tr_Root:SetActive(true);
    elseif (mode == 3) then
        window:SetActive(true);
        mode1_Root:SetActive(false);
        mode2_Root:SetActive(true);
        mode3_Root:SetActive(false);
        ChangeTRAnchorSize(260);
        tr_Root:SetActive(true);
    elseif (mode == 4) then
        window:SetActive(true);
        mode1_Root:SetActive(false);
        mode2_Root:SetActive(false);
        mode3_Root:SetActive(true);
        ChangeTRAnchorSize(260);
        tr_Root:SetActive(false);
    end
    RefreshInfo();
end

function SetWindowInfo(originalTitle, titleStr, windowComponent)
    UIHelper.SetLabelTxt(label_title1, titleStr);
    UIHelper.SetLabelTxt(label_title2, titleStr);
    
    ui = windowComponent;

    --check side menu selected status
    for i,v in ipairs(buttonTitleKeys) do
        if(originalTitle == v)then
            SelectSideButton(i);
        end
        SetSelected(i,originalTitle == v);
    end
end

function SelectSideButton(i)
    sideMenuSelectedIndex = i;
end

function GetSelectedIndex()
    return sideMenuSelectedIndex;
end

function IsButtonSelected(index)
    return sideMenuSelectedIndex == index;
end

function SetSelected(i,willSelect)
    if(i==nil)then
        return;
    end
    if(willSelect)then
        UIHelper.SetSpriteName(sideMenuButtons[i].Icon,buttonIconNamePrefix[i]..2);
        UIHelper.SetWidgetColor(sideMenuButtons[i].Name,ColorSelectedName);
    else
        UIHelper.SetSpriteName(sideMenuButtons[i].Icon,buttonIconNamePrefix[i]..1);
        UIHelper.SetWidgetColor(sideMenuButtons[i].Name,ColorNormalName);
    end
    GameObjectSetActive(sideMenuButtons[i].Selected,willSelect);
end
