--region *.lua
--Date
--此文件由[BabeLua]插件自动生成


-- 比赛结束成功界面
--endregion

module("UIBattleResultS", package.seeall)


require "Common/UnityCommonScript"
--require "UILua/UITeamLegend"
require "UILua/UILobbyScript"
require "UILua/UIUpgradeCoach"
require "UILua/UIUpgradePlayer"
require "UILua/UIItemTips"

local strCenterBottom = "SuccessPart/Bottom/";

local window = nil;
local windowComponent = nil;
local windowSuccess = nil;
local m_item = nil;
local m_scrollView = nil;
local m_grid = nil;

local spr_iconLeft = nil;
local spr_iconRight = nil;
local lbl_scoreLeft = nil;
local lbl_scoreRight = nil;
local lbl_clubNameLeft = nil;
local lbl_clubNameRight = nil;
local lbl_clubLvLeft = nil;
local lbl_clubLvRight = nil;

local playerExp = nil;
local lbl_playerExp = nil;
local coachExp = nil;
local lbl_coachExp = nil;
local money = nil;
local lbl_money = nil;
local honor = nil;
local lbl_honor = nil;

local btn_detail = nil;
local btn_again = nil;
local btn_continue = nil;
local m_currBattleType = nil;

local m_rewardItemTb = {};
local m_rewardItemNumTb = {};
local m_starsLightTb = {};

enum_BattleResultType = {
    TeamLegend=1,
    PeakRoad=2,
    TimeRaid=3,
    Ladder=4,
    PVELeague=5
};

local Callback = nil;
function OnStart(gameObject, params)
    Callback = params.Callback;
    m_currBattleType = params.BattleResultType;
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");
    BindUI();

    RefreshCommonResult(params);
    if m_currBattleType == enum_BattleResultType.TeamLegend then
        RefreshBattleResult_TL(params);
    elseif m_currBattleType == enum_BattleResultType.PeakRoad then
        RefreshBattleResult_PR(params);
    elseif m_currBattleType == enum_BattleResultType.TimeRaid then
        RefreshBattleResult_TR(params);
    elseif m_currBattleType == enum_BattleResultType.Ladder then
        RefreshBattleResult_LA(params);
    elseif m_currBattleType == enum_BattleResultType.PVELeague then
        RefreshBattleResult_PVELeague(params);
    else
        print("BattleResultType == nil");
    end

end

-- --data_
-- --myUI      {icon,score,name,lv,coachExp,playerExp,money}
-- --enemyUI   {icon,score,name,lv,coachExp,playerExp,money}
-- --otherUI   {starsRoot,buttonStat,buttonAgain,buttonContinue,}
-- function OnInitResultUI(data_,myUI,enemyUI,otherUI)
--     -- body
-- end

function BindUI()
    local transform = window.transform;

    windowSuccess = TransformFindChild(transform,"SuccessPart");

    m_item = windowComponent:GetPrefab("Item");
    m_grid = TransformFindChild(transform,"SuccessPart/Reward/RewardItem/ScrollViewPanel/Grid");
    m_scrollView = TransformFindChild(transform,"SuccessPart/Reward/RewardItem/ScrollViewPanel");

    spr_iconLeft = TransformFindChild(transform,"SuccessPart/ClubLeft/Icon/Icon");
    spr_iconRight = TransformFindChild(transform,"SuccessPart/ClubRight/Icon/Icon");
    lbl_scoreLeft = TransformFindChild(transform,"SuccessPart/Center/Score/lbl_left");
    lbl_scoreRight = TransformFindChild(transform,"SuccessPart/Center/Score/lbl_right");
    lbl_clubNameLeft = TransformFindChild(transform,"SuccessPart/ClubLeft/lbl_clubName");
    lbl_clubNameRight = TransformFindChild(transform,"SuccessPart/ClubRight/lbl_clubName");
    lbl_clubLvLeft = TransformFindChild(transform,"SuccessPart/ClubLeft/Lv/lbl_lv");
    lbl_clubLvRight = TransformFindChild(transform,"SuccessPart/ClubRight/Lv/lbl_lv");

    playerExp = TransformFindChild(transform,"SuccessPart/Reward/PlayerExp");
    lbl_playerExp = TransformFindChild(transform,"SuccessPart/Reward/PlayerExp/Num");
    coachExp = TransformFindChild(transform,"SuccessPart/Reward/CoachExp");
    lbl_coachExp = TransformFindChild(transform,"SuccessPart/Reward/CoachExp/Num");
    money = TransformFindChild(transform,"SuccessPart/Reward/Money");
    lbl_money = TransformFindChild(transform,"SuccessPart/Reward/Money/Num");
    honor = TransformFindChild(transform,"SuccessPart/Reward/Honor");
    lbl_honor = TransformFindChild(transform,"SuccessPart/Reward/Honor/Num");

    btn_detail = TransformFindChild(transform,strCenterBottom.."BtnDetail");
    Util.AddClick(btn_detail.gameObject,BtnDetail);
    btn_again = TransformFindChild(transform,strCenterBottom.."BtnAgain");
    Util.AddClick(btn_again.gameObject,BtnAgain);
    btn_continue = TransformFindChild(transform,strCenterBottom.."BtnContinue");
    Util.AddClick(btn_continue.gameObject,BtnContinue);

    m_starsLightTb = {};
    for i=1,3 do
        local light = TransformFindChild(transform,"SuccessPart/Center/Stars/Star"..i.."/light");
        table.insert(m_starsLightTb,light);
    end
    
    -- Different Exp Type
    if m_currBattleType == enum_BattleResultType.PeakRoad then
        UIHelper.SetLabelTxt(TransformFindChild(transform,"SuccessPart/Reward/CoachExp/Label"),Util.LocalizeString("UIBattleResult_CoachExp"));
    else
        UIHelper.SetLabelTxt(TransformFindChild(transform,"SuccessPart/Reward/CoachExp/Label"),Util.LocalizeString("UIBattleResult_UserExp"));
    end
end

function RefreshCommonResult(data_)
    local meScore = data_['score'][1];
    local enemyScore = data_['score'][2];
    UIHelper.SetLabelTxt(lbl_clubNameLeft,Role.Get_name());
    UIHelper.SetLabelTxt(lbl_clubLvLeft,"Lv.".. Role.Get_lv());
    Util.SetUITexture(spr_iconLeft, LuaConst.Const.ClubIcon, Role.GetRoleIcon(), true);
    UIHelper.SetLabelTxt(lbl_scoreLeft,meScore);
    UIHelper.SetLabelTxt(lbl_scoreRight,enemyScore);

    if data_['HExp'] ~= nil then
        UIHelper.SetLabelTxt(lbl_playerExp,"+ ".. data_['HExp']);
    end
    if data_['UExp'] ~= nil then
        UIHelper.SetLabelTxt(lbl_coachExp,"+ ".. data_['UExp']);
    end
    if data_['money'] ~= nil then
        UIHelper.SetLabelTxt(lbl_money,"+ ".. data_['money']);
    end

    for k,v in pairs(data_['item']) do
        local icon = GameObjectInstantiate(m_item);
        icon.name = v.id;
        icon.transform.parent = m_grid;
        icon.transform.localScale = NewVector3(1,1,1);
        icon.transform.localPosition = NewVector3(0,0,0);
        local lblNum = TransformFindChild(icon.transform,"num");
        local sprIcon = TransformFindChild(icon.transform,"Icon/Icon");
        UIHelper.SetLabelTxt(lblNum,v.num);
        Util.SetUITexture(sprIcon, LuaConst.Const.ItemIcon, Config.GetProperty(Config.ItemTable(),v.id,'icon'), true);
        Util.AddClick(icon,BtnItem);
    end
    UIHelper.RepositionGrid(m_grid,m_scrollView);
end

function RefreshBattleResult_TL(data_)

    GameObjectSetActive(playerExp,true);
    GameObjectSetActive(coachExp,true);
    GameObjectSetActive(money,true);
    GameObjectSetActive(honor,false);

    local enemyName,enemyLv,enemyIcon = TeamLegendData.GetEnemyNameLv(tostring(TeamLegendData.GetCurrLevelId()));
    print("enemyIcon: "..enemyIcon);
    UIHelper.SetLabelTxt(lbl_clubNameRight,enemyName);
    Util.SetUITexture(spr_iconRight, LuaConst.Const.ClubIcon, tostring(enemyIcon), true);
    UIHelper.SetLabelTxt(lbl_clubLvRight,"Lv.".. enemyLv);

    for i=1,3 do
        GameObjectSetActive(m_starsLightTb[i],false);
    end
    for i=1,(data_['score'][1] - data_['score'][2]) do
        GameObjectSetActive(m_starsLightTb[i],true);
    end
    if TeamLegendData.GetBEvent() then
        GameObjectSetActive(btn_again,false);
    end

    local OnCoach = function()
        CheckUpgradeMgr.CheckPlayerUpgrade(nil);
    end;
    CheckUpgradeMgr.CheckCoachUpgrade(OnCoach);
end

function RefreshBattleResult_PR(data_)

    GameObjectSetActive(playerExp,true);
    GameObjectSetActive(coachExp,true);
    GameObjectSetActive(money,true);
    GameObjectSetActive(honor,false);

    local enemyName,enemyLv,enemyIcon = PeakRoadData.GetCurrEnemyNameLvInfo();
    UIHelper.SetLabelTxt(lbl_clubNameRight,enemyName);
    Util.SetUITexture(spr_iconRight, LuaConst.Const.ClubIcon, tostring(enemyIcon), true);
    UIHelper.SetLabelTxt(lbl_clubLvRight,"Lv.".. enemyLv);

    for i=1,3 do
        GameObjectSetActive(m_starsLightTb[i].parent,false);
    end
    GameObjectSetActive(btn_detail,false);
    GameObjectSetActive(btn_again,false);
end

function RefreshBattleResult_TR(data_)

    GameObjectSetActive(playerExp,true);
    GameObjectSetActive(coachExp,true);
    GameObjectSetActive(money,true);
    GameObjectSetActive(honor,false);

    local enemyName,enemyLv,enemyIcon = TimeRaidData.GetEnemyInfo();
    UIHelper.SetLabelTxt(lbl_clubNameRight,enemyName);
    Util.SetUITexture(spr_iconRight, LuaConst.Const.ClubIcon, tostring(enemyIcon), true);
    UIHelper.SetLabelTxt(lbl_clubLvRight,"Lv.".. enemyLv);

    for i=1,3 do
        GameObjectSetActive(m_starsLightTb[i].parent,false);
    end
    GameObjectSetActive(btn_detail,false);
    GameObjectSetActive(btn_again,false);
end

function RefreshBattleResult_LA(data_)

    GameObjectSetActive(playerExp,false);
    GameObjectSetActive(coachExp,false);
    GameObjectSetActive(money,false);
    GameObjectSetActive(honor,false);

    local enemyName,enemyLv,enemyIcon = PVPMsgManager.GetLadderEnemyInfo();
    UIHelper.SetLabelTxt(lbl_clubNameRight,enemyName);
    Util.SetUITexture(spr_iconRight, LuaConst.Const.ClubIcon, tostring(enemyIcon), true);
    UIHelper.SetLabelTxt(lbl_clubLvRight,"Lv.".. enemyLv);

    --honor
    -- UIHelper.SetLabelTxt(lbl_honor,"+"..data_.honor);


    for i=1,3 do
        GameObjectSetActive(m_starsLightTb[i].parent,false);
    end
    GameObjectSetActive(btn_detail,false);
    GameObjectSetActive(btn_again,false);
end

function RefreshBattleResult_PVELeague(data_)
    GameObjectSetActive(playerExp,true);
    GameObjectSetActive(coachExp,true);
    GameObjectSetActive(money,true);
    GameObjectSetActive(honor,false);

    local enemyName,enemyLv,enemyIcon = PVELeagueData.GetCurrEnemyNameLvInfo();
    UIHelper.SetLabelTxt(lbl_clubNameRight,enemyName);
    Util.SetUITexture(spr_iconRight, LuaConst.Const.ClubIcon, tostring(enemyIcon), true);
    UIHelper.SetLabelTxt(lbl_clubLvRight,"Lv.".. enemyLv);

    for i=1,3 do
        GameObjectSetActive(m_starsLightTb[i].parent,false);
    end
    GameObjectSetActive(btn_detail,false);
    GameObjectSetActive(btn_again,false);
end

function BtnItem(args_)
    WindowMgr.ShowWindow(LuaConst.Const.UIItemTips,{id = args_.name});
end

function BtnDetail()
    WindowMgr.ShowWindow(LuaConst.Const.UIBattleDetail);
end

function BtnAgain()
    if IsValidClick() then
        if m_currBattleType == enum_BattleResultType.TeamLegend then
            TeamLegendData.SetBAgain(true);
        end
        SceneManager.ReturnLobbyScene();
        ExitUIBattleResultS();
    end
end


function BtnContinue()
    if IsValidClick() then
        if m_currBattleType == enum_BattleResultType.TeamLegend then
            TeamLegendData.SetBAgain(false);
        end
        SceneManager.ReturnLobbyScene();
        ExitUIBattleResultS();
    end

end


function OnDestroy()
    if Callback ~= nil then
       Callback();
       Callback = nil;
    end

end

function ExitUIBattleResultS()
    windowComponent:Close();
    
end

