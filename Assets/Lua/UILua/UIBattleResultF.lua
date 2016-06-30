--region *.lua
--Date
--此文件由[BabeLua]插件自动生成


-- 比赛结束 失败界面
--endregion

module("UIBattleResultF", package.seeall)


require "Common/UnityCommonScript"
require "UILua/UIBattleResultS"
require "UILua/UITeamLegend"


local window = nil;
local windowComponent = nil;


local spr_iconLeft = nil;
local spr_iconRight = nil;
local lbl_scoreLeft = nil;
local lbl_scoreRight = nil;
local lbl_clubNameLeft = nil;
local lbl_clubNameRight = nil;
local lbl_clubLvLeft = nil;
local lbl_clubLvRight = nil;
local btn_detail  = nil;
local btn_again = nil;
local btn_continue = nil;
local m_enemyName = nil;
local m_enemyLv = nil;
local m_enemyIcon = nil; 

local Callback = nil;

-- params-> enemyName enemyLv enemyIcon
function OnStart(gameObject, params)
    Callback = params.Callback;
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");
    BindUI();

    if params.BattleResultType == UIBattleResultS.enum_BattleResultType.PeakRoad then
        SetUI_PR()
        m_enemyName,m_enemyLv,m_enemyIcon = PeakRoadData.GetCurrEnemyNameLvInfo();
    elseif params.BattleResultType == UIBattleResultS.enum_BattleResultType.TeamLegend then
        m_enemyName,m_enemyLv,m_enemyIcon = TeamLegendData.GetEnemyNameLv(tostring(TeamLegendData.GetCurrLevelId()));
    elseif params.BattleResultType == UIBattleResultS.enum_BattleResultType.TimeRaid then
        m_enemyName,m_enemyLv,m_enemyIcon = TimeRaidData.GetEnemyInfo();
    elseif params.BattleResultType == UIBattleResultS.enum_BattleResultType.Ladder then
        m_enemyName,m_enemyLv,m_enemyIcon = PVPMsgManager.GetLadderEnemyInfo();
    elseif params.BattleResultType == UIBattleResultS.enum_BattleResultType.PVELeague then
        m_enemyName,m_enemyLv,m_enemyIcon = PVELeagueData.GetCurrEnemyNameLvInfo();
    else
        m_enemyName = params.enemyName;
        m_enemyLv = params.enemyLv;
        m_enemyIcon = params.enemyIcon;
    end

    RefreshBattleResult(params);

end

function BindUI()
    local transform = window.transform;

    spr_iconLeft = TransformFindChild(transform,"FailPart/ClubLeft/Icon/Icon");
    spr_iconRight = TransformFindChild(transform,"FailPart/ClubRight/Icon/Icon");
    lbl_scoreLeft = TransformFindChild(transform,"FailPart/Center/Score/lbl_left");
    lbl_scoreRight = TransformFindChild(transform,"FailPart/Center/Score/lbl_right");
    lbl_clubNameLeft = TransformFindChild(transform,"FailPart/ClubLeft/lbl_clubName");
    lbl_clubNameRight = TransformFindChild(transform,"FailPart/ClubRight/lbl_clubName");
    lbl_clubLvLeft = TransformFindChild(transform,"FailPart/ClubLeft/Lv/lbl_lv");
    lbl_clubLvRight = TransformFindChild(transform,"FailPart/ClubRight/Lv/lbl_lv");


    btn_detail = TransformFindChild(transform,"FailPart/Bottom/BtnDetail");
    Util.AddClick(btn_detail.gameObject,BtnDetail);
    btn_again = TransformFindChild(transform,"FailPart/Bottom/BtnAgain");
    Util.AddClick(btn_again.gameObject,BtnAgain);
    btn_continue = TransformFindChild(transform,"FailPart/Bottom/BtnContinue");
    Util.AddClick(btn_continue.gameObject,BtnContinue);
    local btnUpgradePlayer = TransformFindChild(transform,"FailPart/Upgrade/UpgradePlayer").gameObject;
    Util.AddClick(btnUpgradePlayer,BtnUpgradePlayer);
    local btnUpgradeCoach = TransformFindChild(transform,"FailPart/Upgrade/UpgradeCoach").gameObject;
    Util.AddClick(btnUpgradeCoach,BtnUpgradeCoach);


end

function RefreshBattleResult(data_)
    local meScore = data_['score'][1];
    local enemyScore = data_['score'][2];
    local stars = meScore - enemyScore;

    if data_.BattleResultType == UIBattleResultS.enum_BattleResultType.PeakRoad then

    elseif data_.BattleResultType == UIBattleResultS.enum_BattleResultType.TeamLegend then

    elseif data_.BattleResultType == UIBattleResultS.enum_BattleResultType.TimeRaid then

    elseif data_.BattleResultType == UIBattleResultS.enum_BattleResultType.Ladder then
        GameObjectSetActive(btn_detail,false);
        GameObjectSetActive(btn_again,false);
    else

    end

    UIHelper.SetLabelTxt(lbl_clubNameLeft,Role.Get_name());
    UIHelper.SetLabelTxt(lbl_clubLvLeft,"Lv.".. Role.Get_lv());
    Util.SetUITexture(spr_iconLeft, LuaConst.Const.ClubIcon, Role.GetRoleIcon(), true);
    UIHelper.SetLabelTxt(lbl_clubNameRight,m_enemyName);
    UIHelper.SetLabelTxt(lbl_clubLvRight,"Lv.".. m_enemyLv);
    Util.SetUITexture(spr_iconRight, LuaConst.Const.ClubIcon, tostring(m_enemyIcon), true);
    UIHelper.SetLabelTxt(lbl_scoreLeft,meScore);
    UIHelper.SetLabelTxt(lbl_scoreRight,enemyScore);

end

function SetUI_PR()
    GameObjectSetActive(btn_detail,false);
    GameObjectSetActive(btn_again,false);
end

function BtnDetail()
    WindowMgr.ShowWindow(LuaConst.Const.UIBattleDetail);
end

function BtnAgain()
    if IsValidClick() then
        -- Application.LoadLevel("train");
        SceneManager.ReturnLobbyScene();

        ExitUIBattleResultF();
    end

end


function BtnContinue()
    if IsValidClick() then
        -- Application.LoadLevel("train");
        SceneManager.ReturnLobbyScene();

        ExitUIBattleResultF();
    end
end

function BtnUpgradePlayer()
--    print("BtnUpgradePlayer");
    local str = Util.LocalizeString("UIBattleResult_UpgradePlayer");
    WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { str });
end

function BtnUpgradeCoach()
--    print("BtnUpgradeCoach");
    local str = Util.LocalizeString("UIBattleResult_UpgradeCaoch");
    WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { str });
end

function OnDestroy()
    if Callback ~= nil then
        Callback();
        Callback = nil;
    end

end

function ExitUIBattleResultF()
    windowComponent:Close();
    
end

