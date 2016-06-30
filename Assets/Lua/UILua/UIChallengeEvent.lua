--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion

module("UIChallengeEvent", package.seeall)

require "UILua/UIPrepareMatch"
require "Game/CheckUpgradeMgr"
require "UILua/UIItemIcon"

local window = nil;
local windowComponent = nil;

local lbl_title = nil;
local lbl_cost = nil;
local lbl_intro = nil;
local lbl_playerExp = nil;
local lbl_teamExp = nil;
local lbl_money = nil;
local lbl_sweepTimes = nil;

local m_scrollView;
local m_grid = nil;
local m_challengeM = nil;
local lbl_challengeM = nil;
local m_challengeDataTb = {};

local itemScale = Vector3.one * 86 / 180;

function OnStart(gameObject, params)
    m_challengeDataTb = params;
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");

    BindUI();

end

function BindUI()
    local transform = window.transform;

    lbl_title = TransformFindChild(transform, "ChallengeContent/Label1");
    lbl_cost = TransformFindChild(transform, "ChallengeContent/Label2/LabelEnergy");
    lbl_intro = TransformFindChild(transform, "ChallengeContent/Label3");
    lbl_playerExp = TransformFindChild(transform, "ChallengeContent/Exp/PlayerExp");
    lbl_teamExp = TransformFindChild(transform, "ChallengeContent/Exp/TeamExp");
    lbl_money = TransformFindChild(transform, "ChallengeContent/Exp/Money");
    lbl_sweepTimes = TransformFindChild(transform, "BtnSweepL/Label");
    lbl_challengeM = TransformFindChild(transform, "BtnChallengeM/Label");
    m_scrollView = TransformFindChild(transform, "ChallengeContent/Item/Scroll View");
    m_grid = TransformFindChild(transform, "ChallengeContent/Item/Scroll View/Grid");



    m_challengeM = TransformFindChild(transform, "BtnChallengeM");
    Util.AddClick(m_challengeM.gameObject, BtnChallengeM);

    local close = TransformFindChild(transform, "BtnClose").gameObject;
    Util.AddClick(close, BtnClose);


    InitData();
end

function InitData()
    local levelId = m_challengeDataTb.levelId; -- Level Id

    UIHelper.SetLabelTxt(lbl_title,Config.GetProperty(Config.RaidLevelTable(),tostring(levelId),'name'));
    UIHelper.SetLabelTxt(lbl_intro,Config.GetProperty(Config.RaidLevelTable(),tostring(levelId),'profile'));
    UIHelper.SetLabelTxt(lbl_cost,Config.GetProperty(Config.RaidLevelTable(),tostring(levelId),'Power'));

    UIHelper.SetLabelTxt(lbl_playerExp,string.format(Util.LocalizeString("UITeamLegend_TeamExp"),Config.GetProperty(Config.RaidLevelTable(),tostring(levelId),'UExp')));
    UIHelper.SetLabelTxt(lbl_teamExp,string.format(Util.LocalizeString("UITeamLegend_PlayerExp"),Config.GetProperty(Config.RaidLevelTable(),tostring(levelId),'HExp')));
    UIHelper.SetLabelTxt(lbl_money,Config.GetProperty(Config.RaidLevelTable(),tostring(levelId),'money'));

    if TeamLegendData.GetBNoFight() then
        UIHelper.SetLabelTxt(lbl_challengeM,Util.LocalizeString("UIPeakRoad_GetReward"));
     else
        UIHelper.SetLabelTxt(lbl_challengeM,Util.LocalizeString("UIPeakRoad_Challenge"));
     end

     InitDropItem();
end

function InitDropItem()
    local m_item = Util.GetGameObject("UIItemIcon");
    local dropItemTb = Config.GetProperty(Config.RaidLevelTable(),tostring(m_challengeDataTb.levelId),'drop_display');
    
    for k,v in pairs(dropItemTb) do
        if v ~= nil and string.len(v) ~= 0 then
            local clone = InstantiatePrefab(m_item, m_grid);
            clone.name = v;
            Util.AddClick(clone, BtnDropItem);
            --Util.SetUITexture(TransformFindChild(clone.transform, "Icon/Icon"), LuaConst.Const.ItemIcon, Config.GetProperty(Config.ItemTable(),v,'icon'), true);
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
function BtnChallengeM(args)
    CheckUpgradeMgr.SetBackup();
    local cost = TeamLegendData.GetLevelPower(TeamLegendData.GetChallengeLevelId());

    if not TeamLegendData.IsHaveChallengeTims() then
         WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("RaidNoTimes")  });
         return;
    end
    
    if Role.Get_power() >= tonumber(cost) then
        if TeamLegendData.GetBNoFight() then
            StartFight();
        else
            UIPrepareMatch.RegisterBtnMatch(StartFight);
            local tb = {};
            tb.PrepareMatchType = UIPrepareMatch.enum_PrepareMatchType.TeamLegend;

            UIPrepareMatch.TryOpen(tb);
        end

    else
        WindowMgr.ShowWindow(LuaConst.Const.UIQuickPurchase,{"Energy"});
        --WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("NoEnoughtPower")  });
    end

end

--------------------------------logic

function StartFight()
    if TeamLegendData.GetRaidDiff() == TeamLegendData.e_raidDiff.Normal then
        TeamLegendData.ReqRaidStart(TeamLegendData.GetCurrLevelId(),OnRaidStart);
    elseif TeamLegendData.GetRaidDiff() == TeamLegendData.e_raidDiff.Elite then
        TeamLegendData.ReqRaidStartJY(TeamLegendData.GetCurrLevelId(),OnRaidStartJY);
    end
end

function OnRaidStart(data_)
    if TeamLegendData.GetBNoFight() then
        OnRaidResult(data_);
    else
        CombatData.FillData(data_,CombatData.COMBAT_TYPE.TEAMLEGEND,OnReqRaidResult);
    end
end

function OnRaidStartJY(data_)
    if TeamLegendData.GetBNoFight() then
        OnRaidResultJY(data_);
    else
        CombatData.FillData(data_,CombatData.COMBAT_TYPE.TEAMLEGEND,OnReqRaidResult);
    end
end

function OnReqRaidResult()
    local tb = CombatData.GetResultTable();
    local m_quit = tb['Giveup'];
    if not m_quit then
        m_meScore = tb['HomeScore'];
        m_enemyScore = tb['AwayScore'];
        local actionList = tostring(tb['PVEData'])
        local score = tostring(m_meScore .. ":".. m_enemyScore);
        local signMD5 = tb['md5'];
        print("Battle Score: "..score);
    
        if TeamLegendData.GetRaidDiff() == TeamLegendData.e_raidDiff.Normal then
            TeamLegendData.ReqRaidResult(TeamLegendData.GetCurrLevelId(),score,actionList,signMD5,OnRaidResult);
        elseif TeamLegendData.GetRaidDiff() == TeamLegendData.e_raidDiff.Elite then
            TeamLegendData.ReqRaidResultJY(TeamLegendData.GetCurrLevelId(),score,actionList,signMD5,OnRaidResultJY);
        end
    end
    
end


function OnRaidResult(data_) -- Event Result
    if data_ ~= nil then
        if TeamLegendData.GetBNoFight() then

            local OnRaidInfo = function()
                BtnClose();
            end;

            local CloseBattleResult = function()
                TeamLegendData.ReqRaidInfo(TeamLegendData.GetCurrCountry(),OnRaidInfo);
                TeamLegendData.SetPlayLevelFx(true);
            end
            WindowMgr.ShowWindow(LuaConst.Const.UIGetItem,{
                m_itemTb = data_,
                titleName = Util.LocalizeString("UITeamLegend_FinishLevel"),
                getButtonName = nil,
                OnClose = CloseBattleResult
                });
        else
            data_.BattleResultType = UIBattleResultS.enum_BattleResultType.TeamLegend;
            if tonumber(data_['score'][1]) >tonumber(data_['score'][2]) then
                local OnRaidInfo = function()
                    BtnClose();
                end;

                local CloseBattleResult = function()
                     TeamLegendData.ReqRaidInfo(TeamLegendData.GetCurrCountry(),OnRaidInfo);
                end
                data_.Callback = CloseBattleResult;

                WindowMgr.ShowWindow(LuaConst.Const.UIBattleResultS,data_);
            else
                WindowMgr.ShowWindow(LuaConst.Const.UIBattleResultF,data_);
            end
        end
    end

end

function OnRaidResultJY(data_)
    if data_ ~= nil then
         if TeamLegendData.GetBNoFight() then

            local OnRaidInfo = function()
                BtnClose();
            end;

            local CloseBattleResult = function()
                TeamLegendData.ReqRaidInfoJY(TeamLegendData.GetCurrCountry(),OnRaidInfo);
            end
          
            WindowMgr.ShowWindow(LuaConst.Const.UIGetItem,{
                m_itemTb = data_,
                titleName = Util.LocalizeString("UITeamLegend_FinishLevel"),
                getButtonName = nil,
                OnClose = CloseBattleResult
                });
        else
            data_.BattleResultType = UIBattleResultS.enum_BattleResultType.TeamLegend;
            if tonumber(data_['score'][1]) >tonumber(data_['score'][2]) then
                local OnRaidInfo = function()
                    BtnClose();
                end;

                 local CloseBattleResult = function()
                     TeamLegendData.ReqRaidInfoJY(TeamLegendData.GetCurrCountry(),OnRaidInfo);
                end
                data_.Callback = CloseBattleResult;

                WindowMgr.ShowWindow(LuaConst.Const.UIBattleResultS,data_);
            else
                WindowMgr.ShowWindow(LuaConst.Const.UIBattleResultF,data_);
            end
        end
    end

end

function BtnClose(args)
    windowComponent:Close();
end



function OnDestroy()
    window = nil;
    windowComponent = nil;

end



