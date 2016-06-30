--region NewFile_1.lua
--Author : hongpeng.zhao
--Date   : 2015/3/25
--此文件由[BabeLua]插件自动生成



--endregion

module("UIPrepareMatch", package.seeall)


require "Common/UnityCommonScript"
require "Common/CommonScript"
require "Game/FormationCMP"
require "UILua/UILoading"

local strCenterTop = "CenterPanel/TopPart/";
local strCenterC = "CenterPanel/CenterPart/";
local strCenterBottom = "CenterPanel/BottomPart/";
local strTopLeft = "TopPanel/TopLeft/ClubeLeft/";
local strTopRight = "TopPanel/TopRight/ClubeRight/";


local window = nil;
local windowComponent = nil;

local lbl_friendMatchScore = nil;
local lbl_enemyMatchScore = nil;
local lbl_friendFormName = nil;
local lbl_enemyFormName = nil;

local m_friendFormRoot = nil;
local m_enemyFormRoot = nil;
local spr_friendFormBg = nil;
local spr_enemyFormBg = nil;
local spr_friendMatchScore = nil;
local spr_enemyMatchScore = nil;

local m_enemyName = nil;
local m_enemyLv = nil;
local m_enemyIcon = nil;
local m_enemyTeam = nil;
local m_enemyScore = nil;
local m_enemyIconTb = {};

local m_beforeMatchData = {};
local m_tfAvatarFriend = {};
local m_tfAvatarEnemy = {};

local callbackMatch = nil;
local m_currPrepareMatchType = nil;
local m_bRefreshForm = nil;
local m_bFight = nil;

enum_PrepareMatchType = {
    TeamLegend=1,
    PeakRoad=2,
    PVELeague=3,
    PVPLADDER=4
};
local enemyInfoTb = {};
function OnStart(gameObject, params)
    if params.PrepareMatchType == enum_PrepareMatchType.TeamLegend then
        m_currPrepareMatchType = enum_PrepareMatchType.TeamLegend;
    elseif params.PrepareMatchType == enum_PrepareMatchType.PeakRoad then
        m_currPrepareMatchType = enum_PrepareMatchType.PeakRoad;
    elseif params.PrepareMatchType == enum_PrepareMatchType.PVELeague then
        m_currPrepareMatchType = enum_PrepareMatchType.PVELeague;
    elseif params.PrepareMatchType == enum_PrepareMatchType.PVPLADDER then
        m_currPrepareMatchType = enum_PrepareMatchType.PVPLADDER;
    else
        m_currPrepareMatchType = nil;
    end

    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");
    BindUI();
    m_bFight = false;
end

function RegisterBtnMatch(args_)
    callbackMatch = args_;
end

-- name lv icon team score
function RegisterEnemyInfo(args_)
    enemyInfoTb = CommonScript.DeepCopy(args_);
end
function BindUI()
    local transform = window.transform;

    m_friendFormRoot = TransformFindChild(transform,"LeftPanel/FriendFormRoot");
    spr_friendFormBg = TransformFindChild(transform,"LeftPanel/FriendFormation/Bg");
    lbl_friendFormName = TransformFindChild(transform,"LeftPanel/FriendFormation/Label");
    m_enemyFormRoot = TransformFindChild(transform,"RightPanel/EnemyFormRoot");
    spr_enemyFormBg = TransformFindChild(transform,"RightPanel/EnemyFormation/Bg");
    lbl_enemyFormName = TransformFindChild(transform,"RightPanel/EnemyFormation/Label");

    lbl_friendMatchScore = TransformFindChild(transform,strCenterTop.."BattleScore/lbl_left");
    lbl_enemyMatchScore = TransformFindChild(transform,strCenterTop.."BattleScore/lbl_right");
    spr_friendMatchScore = TransformFindChild(transform,"CenterPanel/TopPart/Circle/LeftFor");
    spr_enemyMatchScore = TransformFindChild(transform,"CenterPanel/TopPart/Circle/RightFor");


    local btnFormation = TransformFindChild(transform,strCenterC.."Button1").gameObject;
    Util.AddClick(btnFormation,BtnFormation);
    local btnTactics = TransformFindChild(transform,strCenterC.."Button2").gameObject;
    Util.AddClick(btnTactics,BtnTactics);
    local btnFight = TransformFindChild(transform,strCenterBottom.."Button1").gameObject;
    Util.AddClick(btnFight,BtnFight);
    local btnReturn = TransformFindChild(transform,strCenterBottom.."Button2").gameObject;
    Util.AddClick(btnReturn,BtnReturn);


    RefreshPrepar();

end

function RefreshPrepar()
    local transform = window.transform;
    if m_currPrepareMatchType == enum_PrepareMatchType.TeamLegend then
        m_enemyName,m_enemyLv,m_enemyIcon,m_enemyTeam,m_enemyScore,m_enemyIconTb = TeamLegendData.GetCurrEnemyNameLvInfo();
    elseif m_currPrepareMatchType == enum_PrepareMatchType.PeakRoad then
        m_enemyName,m_enemyLv,m_enemyIcon,m_enemyTeam,m_enemyScore,m_enemyIconTb = PeakRoadData.GetCurrEnemyNameLvInfo();
    elseif m_currPrepareMatchType == enum_PrepareMatchType.PVELeague then
        m_enemyName,m_enemyLv,m_enemyIcon,m_enemyTeam,m_enemyScore,m_enemyIconTb = PVELeagueData.GetCurrEnemyNameLvInfo();
    elseif m_currPrepareMatchType == enum_PrepareMatchType.PVPLADDER then
        m_enemyName,m_enemyLv,m_enemyIcon,m_enemyTeam,m_enemyScore,m_enemyIconTb = PVPMsgManager.GetLadderEnemyInfo();
    else
        m_enemyName = enemyInfoTb.name;
        m_enemyLv = enemyInfoTb.lv;
        m_enemyIcon = enemyInfoTb.icon
        m_enemyTeam = enemyInfoTb.team;
        m_enemyScore = enemyInfoTb.score or -2;
    end

    local lbl_clubNameLeft = TransformFindChild(transform,strTopLeft.."lbl_clubeName");
    UIHelper.SetLabelTxt(lbl_clubNameLeft,Role.Get_name());
    local lbl_lvLeft = TransformFindChild(transform,strTopLeft.."Lv/Label");
    UIHelper.SetLabelTxt(lbl_lvLeft,"Lv.".. Role.Get_lv());
    local teamLogo = TransformFindChild(transform,strTopLeft.."Icon/Icon");
    Util.SetUITexture(teamLogo,LuaConst.Const.ClubIcon,Role.GetRoleIcon(), true);

    local lbl_clubNameRight = TransformFindChild(transform,strTopRight.."lbl_clubeName");
    local lbl_lvRight = TransformFindChild(transform,strTopRight.."Lv/Label");
    local teamLogo = TransformFindChild(transform,strTopRight.."Icon/Icon");
    UIHelper.SetLabelTxt(lbl_clubNameRight,(m_enemyName or "Name"));
    UIHelper.SetLabelTxt(lbl_lvRight,"Lv.".. (m_enemyLv or "-1"));
    Util.SetUITexture(teamLogo,LuaConst.Const.ClubIcon,GetEnemyIcon(), true);

    -----------------------------------------------------------------------------
    UIHelper.DestroyGrid(m_friendFormRoot);
    UIHelper.DestroyGrid(m_enemyFormRoot);

    local formId = Hero.GetFormId();
    local teamIdTb = Hero.MainTeamHeroId();
    -- local posPrefab = windowComponent:GetPrefab("UIAvatar");
    local posPrefab = Util.GetGameObject("UIAvatar_Formation");


    m_tfAvatarFriend = {};
    local friendIconTb = {};
    for i = 1, #teamIdTb do
        local clone = AddChild(posPrefab, m_friendFormRoot);
        UIHelper.AdjustDepth(clone, 10);
        clone.name = i;
        m_tfAvatarFriend[i] = clone.transform;
        table.insert(friendIconTb,HeroData.GetHeroIcon(teamIdTb[i]));
    end

    local formationFriend = FormationCMP.New();
    formationFriend:InitFormation(nil,m_tfAvatarFriend,FormationCMP.enum_CMPType.Type1,friendIconTb);

    local teamLengthEnemy = 11;
    local enemyTeamId = 1;
    if m_enemyTeam ~= nil then
        enemyTeamId = m_enemyTeam;
    end
    print("enemyTeamId: "..enemyTeamId);
    m_tfAvatarEnemy = {};
    for i = 1, teamLengthEnemy do
        local clone = AddChild(posPrefab, m_enemyFormRoot);
        UIHelper.AdjustDepth(clone, 10);
        clone.name = i;
        m_tfAvatarEnemy[i] = clone.transform;
    end

    local formationEnemy = FormationCMP.New();
    formationEnemy:InitFormation(tostring(enemyTeamId),m_tfAvatarEnemy,FormationCMP.enum_CMPType.Type1,m_enemyIconTb);

    UIHelper.SetLabelTxt(lbl_friendMatchScore,HeroData.GetTeamBattleScore());
    UIHelper.SetLabelTxt(lbl_enemyMatchScore,m_enemyScore);
    UIHelper.SetSpriteFillAmount(spr_friendMatchScore,HeroData.GetTeamBattleScore()/100000);
    UIHelper.SetSpriteFillAmount(spr_enemyMatchScore,m_enemyScore/100000);

    UIHelper.SetLabelTxt(lbl_friendFormName,Config.GetTemplate(Config.positionTB)[formId].name);
    UIHelper.SetLabelTxt(lbl_enemyFormName,Config.GetTemplate(Config.positionTB)[tostring(enemyTeamId)].name);
end

function BtnFormation()
    print("BtnFormation");
end

function BtnTactics()
    print("BtnTactics");
    WindowMgr.ShowWindow(LuaConst.Const.UIFormation);
    m_bRefreshForm = true;
end


function BtnFight()
    m_bFight = true;
    ExitUIPrepareMatch();
end

function BtnReturn()
--    TryClose();
    m_bFight = false;
    ExitUIPrepareMatch();
end

-- logic
function GetEnemyIcon()
    local icon = m_enemyIcon;
    if icon == nil or string.len(icon) == 0 then
        icon = 'Default';
    end

    return icon;
end

function TryOpen(tb_)
    if tb_ == nil then
        tb_ = {};
        tb_.PrepareMatchType = nil;
    end

    if window == nil then
        WindowMgr.ShowWindow(LuaConst.Const.UIPrepareMatch,tb_);
    elseif windowComponent ~= nil then
        m_bRefreshForm = true;
        WindowMgr.ShowWindow(LuaConst.Const.UIPrepareMatch,tb_);
    end

end

function TryClose()
    if windowComponent ~= nil then
        windowComponent:Hide();
    end
end

function OnShow()
    print("UIPrepareMatch...OnShow");
    if m_bRefreshForm then
        RefreshPrepar();
        m_bRefreshForm = nil;
    end

end

function OnHide()
--    print("UIPrepareMatch...OnHide");

end

function OnDestroy()
    window = nil;
    windowComponent = nil;

    if callbackMatch ~= nil and m_bFight then
        callbackMatch();
        callbackMatch = nil;
    end
end

function ExitUIPrepareMatch()
    windowComponent:Close();
    WindowMgr.ShowWindow(LuaConst.Const.UILoading);
end
