--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion

module("UIChallengeTL", package.seeall)

require "UILua/UIPrepareMatch"
require "UILua/UIPeakRoadScript"
require "UILua/UISweepResult"
require "Game/CheckUpgradeMgr"
require "UILua/UIItemIcon"

local window = nil;
local windowComponent = nil;


local lbl_title = nil;
local lbl_cost = nil;
local lbl_todayTimes = nil;
local lbl_intro = nil;
local lbl_playerExp = nil;
local lbl_teamExp = nil;
local lbl_money = nil;
local lbl_sweepTimes = nil;
local tex_club = nil;

local m_scrollView = nil;
local m_grid = nil;
local m_challengeL = nil;
local m_challengeM = nil;
local m_challengeR = nil;
local m_currChallengeType = nil;

local m_challengeDataTb = {};
enum_ChallengeType = {TeamLegend=1,PeakRoad=2};

local itemScale = Vector3.one * 86 / 180;
function OnStart(gameObject, params)
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");
    
    RefreshData();
    BindUI();
end

function RefreshData()
    m_challengeDataTb = windowComponent.mLuaTables;
    if m_challengeDataTb.ChallengeType == enum_ChallengeType.TeamLegend then
        m_currChallengeType = enum_ChallengeType.TeamLegend;
    elseif m_challengeDataTb.ChallengeType == enum_ChallengeType.PeakRoad then
        m_currChallengeType = enum_ChallengeType.PeakRoad;
    else
        print("ChallengeType == nil");
    end
end

function BindUI()
    local transform = window.transform;

    lbl_title = TransformFindChild(transform, "ChallengeContent/Label1");
    lbl_cost = TransformFindChild(transform, "ChallengeContent/Label2/LabelEnergy");
    lbl_intro = TransformFindChild(transform, "ChallengeContent/Label3");
    lbl_todayTimes = TransformFindChild(transform, "ChallengeContent/LabelToday/Label");
    lbl_playerExp = TransformFindChild(transform, "ChallengeContent/Exp/PlayerExp");
    lbl_teamExp = TransformFindChild(transform, "ChallengeContent/Exp/TeamExp");
    lbl_money = TransformFindChild(transform, "ChallengeContent/Exp/Money");
    tex_club = TransformFindChild(transform, "ChallengeContent/Icon");
    lbl_sweepTimes = TransformFindChild(transform, "BtnSweepL/Label");
    m_scrollView = TransformFindChild(transform, "ChallengeContent/Item/Scroll View");
    m_grid = TransformFindChild(transform, "ChallengeContent/Item/Scroll View/Grid");


    m_challengeL = TransformFindChild(transform, "BtnSweepL");
    Util.AddClick(m_challengeL.gameObject, BtnChallengeL);
    m_challengeM = TransformFindChild(transform, "BtnChallengeM");
    Util.AddClick(m_challengeM.gameObject, BtnChallengeM);
    m_challengeR = TransformFindChild(transform, "BtnChallengeR");
    Util.AddClick(m_challengeR.gameObject, BtnChallengeR);
    Util.AddClick(TransformFindChild(transform, "BtnClose").gameObject, BtnClose);

    RefreshChallengeData();
end

function RefreshChallengeData()
    if m_currChallengeType == enum_ChallengeType.TeamLegend then
        InitDataTL();
    elseif m_currChallengeType == enum_ChallengeType.PeakRoad then
        InitDataPR();
    end
end

function InitDataTL()
    local levelId = m_challengeDataTb.levelId; -- Level Id
    TeamLegendData.SetFastTimes(TeamLegendData.GetSweepNum());

    TeamLegendData.SetClubIcon(tex_club,levelId);
    UIHelper.SetLabelTxt(lbl_title,TeamLegendData.GetLevelName(levelId));
    UIHelper.SetLabelTxt(lbl_intro,TeamLegendData.GetLevelProfile(levelId));
    UIHelper.SetLabelTxt(lbl_cost,TeamLegendData.GetLevelPower(levelId));
    UIHelper.SetLabelTxt(lbl_playerExp,string.format(Util.LocalizeString("UITeamLegend_TeamExp"),TeamLegendData.GetLevelUExp(levelId)));
    UIHelper.SetLabelTxt(lbl_teamExp,string.format(Util.LocalizeString("UITeamLegend_PlayerExp"),TeamLegendData.GetLevelHExp(levelId)));
    UIHelper.SetLabelTxt(lbl_money,TeamLegendData.GetLevelMoney(levelId));
    UIHelper.SetLabelTxt(lbl_todayTimes,m_challengeDataTb.times.."/99");

    if m_challengeDataTb.stars3 >=3 then
       GameObjectSetActive(m_challengeL,true);
       GameObjectSetActive(m_challengeR,true);
       GameObjectSetActive(m_challengeM,false);
       UIHelper.SetLabelTxt(lbl_sweepTimes,string.format(Util.LocalizeString("UITeamLegend_Sweep"),TeamLegendData.GetSweepNum()));
    else
       GameObjectSetActive(m_challengeL,false);
       GameObjectSetActive(m_challengeR,false);
       GameObjectSetActive(m_challengeM,true);
    end

    InitDropItem();
end

function InitDataPR()
    local levelId = m_challengeDataTb.levelId; -- Level Id

    UIHelper.SetLabelTxt(lbl_title,Config.GetProperty(Config.RaidDFTeamTable(),tostring(levelId),'name'));
    UIHelper.SetLabelTxt(lbl_intro,Config.GetProperty(Config.RaidDFTeamTable(),tostring(levelId),'profile'));
    Util.SetUITexture(tex_club, LuaConst.Const.ClubIcon, Config.GetProperty(Config.RaidDFTeamTable(),tostring(levelId),'icon'), false);
    GameObjectSetActive(lbl_cost.parent,false);
    GameObjectSetActive(lbl_todayTimes.parent,false);

    UIHelper.SetLabelTxt(lbl_teamExp,string.format(Util.LocalizeString("UIPeakRoad_CoachExp"),Config.GetProperty(Config.RaidDFTable(),tostring(levelId),'CExp')));
    UIHelper.SetLabelTxt(lbl_playerExp,string.format(Util.LocalizeString("UITeamLegend_PlayerExp"),Config.GetProperty(Config.RaidDFTable(),tostring(levelId),'HExp')));
    UIHelper.SetLabelTxt(lbl_money,Config.GetProperty(Config.RaidDFTable(),tostring(levelId),'money'));

    GameObjectSetActive(m_challengeL,false);
    GameObjectSetActive(m_challengeR,false);
    GameObjectSetActive(m_challengeM,true);

    InitDropItem();
end

function InitDropItem()
    local dropItemTb = nil;
    if m_currChallengeType == enum_ChallengeType.TeamLegend then
        dropItemTb = TeamLegendData.GetLevelDropDisplay(m_challengeDataTb.levelId);
    elseif m_currChallengeType == enum_ChallengeType.PeakRoad then
        dropItemTb = Config.GetProperty(Config.RaidDFTable(),tostring(m_challengeDataTb.levelId),'drop_display');
    end
    
    local m_item = Util.GetGameObject("UIItemIcon");

    for k,v in pairs(dropItemTb) do
        if v ~= nil and string.len(v) ~= 0 then
            local clone = InstantiatePrefab(m_item,m_grid);
            clone.name = v;
            Util.AddClick(clone, BtnDropItem);
            local itemIcon = UIItemIcon.New(clone);
            itemIcon:SetSize("win_wb_22", itemScale);
            itemIcon:InitIconOnly(v);
        end
    end
end

function BtnDropItem(args)
    print(args.name);
    WindowMgr.ShowWindow(LuaConst.Const.UIItemTips,{id = args.name});
end

function BtnChallengeL(args)
    CheckUpgradeMgr.SetBackup();

    if m_currChallengeType == enum_ChallengeType.TeamLegend then
        BtnChallengeL_TL();
    end
end

function BtnChallengeM(args)
    BtnChallengeR();
end

function BtnChallengeR(args)
    CheckUpgradeMgr.SetBackup();
    
    if m_currChallengeType == enum_ChallengeType.TeamLegend then
        BtnChallengeR_TL();
    elseif m_currChallengeType == enum_ChallengeType.PeakRoad then
        BtnChallengeR_PR();
    end
end

function BtnChallengeL_TL()
    CheckUpgradeMgr.SetBackup();
    if TeamLegendData.IsFastBattle() then
        TeamLegendData.ReqQuickBattle();
    end
end

function BtnChallengeR_TL()
    local cost = TeamLegendData.GetLevelPower(TeamLegendData.GetChallengeLevelId());

    if not TeamLegendData.IsHaveChallengeTims() then     
         WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("RaidNoTimes")  });
         return;
    end
    
    if Role.Get_power() >= tonumber(cost) then
        UIPrepareMatch.RegisterBtnMatch(TeamLegendData.StartFight);
        local tb = {};
        tb.PrepareMatchType = UIPrepareMatch.enum_PrepareMatchType.TeamLegend;

        UIPrepareMatch.TryOpen(tb);
    else
        WindowMgr.ShowWindow(LuaConst.Const.UIQuickPurchase,{"Energy"});
        --WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("NoEnoughtPower")  });
    end
end
function BtnChallengeR_PR()   
    PeakRoadData.ClickChallengeTips();
end


function OnShow()
    if window ~= nil then
        RefreshData();
        RefreshChallengeData();
    end
end

function BtnClose()
    if windowComponent ~= nil then
        windowComponent:Close();
    end

end

function OnDestroy()
    window = nil;
    windowComponent = nil;
    m_challengeDataTb = {};
end
