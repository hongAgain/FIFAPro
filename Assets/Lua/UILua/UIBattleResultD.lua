--region *.lua
--Date
--此文件由[BabeLua]插件自动生成


-- 比赛结束 平局界面
--endregion

module("UIBattleResultD", package.seeall)


local window = nil;
local windowComponent = nil;

local enemyName = nil;
local enemyLv = nil;
local enemyIcon = nil;
function OnStart(gameObject, params)
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");
    BindUI();

    if params.BattleResultType == UIBattleResultS.enum_BattleResultType.PeakRoad then
        enemyName,enemyLv,enemyIcon = PeakRoadData.GetCurrEnemyNameLvInfo();      
    elseif params.BattleResultType == UIBattleResultS.enum_BattleResultType.TeamLegend then
        enemyName,enemyLv,enemyIcon = TeamLegendData.GetEnemyNameLv(tostring(TeamLegendData.GetCurrLevelId()));
    elseif params.BattleResultType == UIBattleResultS.enum_BattleResultType.TimeRaid then
        enemyName,enemyLv,enemyIcon = TimeRaidData.GetEnemyInfo();
    elseif params.BattleResultType == UIBattleResultS.enum_BattleResultType.Ladder then
        enemyName,enemyLv,enemyIcon = PVPMsgManager.GetLadderEnemyInfo();
    end

    InitUIData(params);
end

function BindUI()
    local transform = window.transform;

    Util.AddClick(TransformFindChild(transform,"DrawPart/Bottom/BtnDetail").gameObject,BtnDetail);
    Util.AddClick(TransformFindChild(transform,"DrawPart/Bottom/BtnAgain").gameObject,BtnAgain);
    Util.AddClick(TransformFindChild(transform,"DrawPart/Bottom/BtnContinue").gameObject,BtnContinue);
end

function InitUIData(data_)
    local transform = window.transform;
    local meScore = data_['score'][1];
    local enemyScore = data_['score'][2];

    UIHelper.SetLabelTxt(TransformFindChild(transform,"DrawPart/Center/Score/lbl_left"),meScore);
    UIHelper.SetLabelTxt(TransformFindChild(transform,"DrawPart/Center/Score/lbl_right"),enemyScore);
    UIHelper.SetLabelTxt(TransformFindChild(transform,"DrawPart/ClubLeft/lbl_clubName"),Role.Get_name());
    UIHelper.SetLabelTxt(TransformFindChild(transform,"DrawPart/ClubLeft/Lv/lbl_lv"),"Lv.".. Role.Get_lv());
    UIHelper.SetLabelTxt(TransformFindChild(transform,"DrawPart/ClubRight/lbl_clubName"),enemyName);
    UIHelper.SetLabelTxt(TransformFindChild(transform,"DrawPart/ClubRight/Lv/lbl_lv"),"Lv.".. enemyLv);
    Util.SetUITexture(TransformFindChild(transform,"DrawPart/ClubRight/Icon/Icon"), LuaConst.Const.ClubIcon, tostring(enemyIcon), true);
    Util.SetUITexture(TransformFindChild(transform,"DrawPart/ClubLeft/Icon/Icon"), LuaConst.Const.ClubIcon, Role.GetRoleIcon(), true);

    UIHelper.SetLabelTxt(TransformFindChild(transform,"DrawPart/Reward/PlayerExp/Num"),"+ ".. data_['HExp']);
    UIHelper.SetLabelTxt(TransformFindChild(transform,"DrawPart/Reward/CoachExp/Num"),"+ ".. data_['UExp']);
    UIHelper.SetLabelTxt(TransformFindChild(transform,"DrawPart/Reward/Money/Num"),"+ ".. data_['money']);
end


function BtnDetail()

end

function BtnAgain()
    if IsValidClick() then
        -- Application.LoadLevel("train");
        SceneManager.ReturnLobbyScene();

        ExitUIBattleResultS();
    end
end

function BtnContinue()
    if IsValidClick() then
        -- Application.LoadLevel("train");
        SceneManager.ReturnLobbyScene();

        ExitUIBattleResultS();
    end
end

function OnDestroy()

end

function ExitUIBattleResultS()
    windowComponent:Close(); 
end

