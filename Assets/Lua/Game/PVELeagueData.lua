module("PVELeagueData", package.seeall);

local leagueConfig = nil;
local leagueClubConfig = nil;
local leagueTeamConfig = nil;
local leagueRewardConfig = nil;
local leagueNPCConfig = nil;

local infoData = nil;
local currentSeasonNum = nil;
local maxTurn = 0;
local currentTurn = nil;
local currentOpponent = nil;
local currentMatchIndex = nil;

local battleResultData = nil;
local upgrade = nil;

local infoDelegateFunc = nil;
local selectDelegateFunc = nil;
local quitDelegateFunc = nil;
local battleStartDelegateFunc = nil;
local battleResultDelegateFunc = nil;

function OnInit()
    DataSystemScript.RegisterMsgHandler(MsgID.tb.PVELeagueInfo, OnReqPVELeagueInfo);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.PVELeagueSelect, OnReqPVELeagueSelect);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.PVELeagueQuit, OnReqPVELeagueQuit);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.PVELeagueBattleStart, OnReqPVELeagueBattleStart);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.PVELeagueBattleResult, OnReqPVELeagueBattleResult);
end

function OnRelease()
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.PVELeagueInfo, OnReqPVELeagueInfo);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.PVELeagueSelect, OnReqPVELeagueSelect);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.PVELeagueQuit, OnReqPVELeagueQuit);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.PVELeagueBattleStart, OnReqPVELeagueBattleStart);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.PVELeagueBattleResult, OnReqPVELeagueBattleResult);
end

function RequestPVELeagueInfo(delegatefunc)
    infoDelegateFunc = delegatefunc;
    DataSystemScript.RequestWithParams(LuaConst.Const.PVELeagueInfo, nil, MsgID.tb.PVELeagueInfo);
end

function RequestPVELeagueSelect(id, delegatefunc)
    selectDelegateFunc = delegatefunc;
    local params = {};
    params['id'] = id;
    DataSystemScript.RequestWithParams(LuaConst.Const.PVELeagueSelect, params, MsgID.tb.PVELeagueSelect);
end

function RequestPVELeagueQuit(delegatefunc)
    quitDelegateFunc = delegatefunc;
    DataSystemScript.RequestWithParams(LuaConst.Const.PVELeagueQuit, nil, MsgID.tb.PVELeagueQuit);
end

function RequestPVELeagueBattleStart(id, callback)
    battleStartDelegateFunc = callback;
    local params = {};
    params['id'] = id;
    DataSystemScript.RequestWithParams(LuaConst.Const.PVELeagueBattleStart, params, MsgID.tb.PVELeagueBattleStart);
end

function RequestPVELeagueBattleResult(id, score, actList, sign, callback)
    print('RequestPVELeagueBattleResult..')
    battleResultDelegateFunc = callback;
    local params = {};
    params['id'] = id;
    params['score'] = score;
    params['act'] = actList;
    params['sign'] = sign;
    DataSystemScript.RequestWithParams(LuaConst.Const.PVELeagueBattleResult, params, MsgID.tb.PVELeagueBattleResult);
end

function OnReqPVELeagueInfo(code_, data_)
    print('OnReqPVELeagueInfo');
    infoData = data_;
    currentSeasonNum = infoData.s;

    SetCurrentInfo();

    if (infoDelegateFunc ~= nil) then
        infoDelegateFunc(data_);
        infoDelegateFunc = nil;
    end
end

function OnReqPVELeagueSelect(code_, data_)
    print('OnReqPVELeagueSelect');
    infoData = data_;
    currentSeasonNum = infoData.s;

    SetCurrentInfo();

    if (selectDelegateFunc ~= nil) then
        selectDelegateFunc(data_);
        selectDelegateFunc = nil;
    end
end

function OnReqPVELeagueQuit(code_, data_)
    if (quitDelegateFunc ~= nil) then
        quitDelegateFunc(data_);
        quitDelegateFunc = nil;
    end
end

function OnReqPVELeagueBattleStart(code_, data_)
    if (battleStartDelegateFunc ~= nil) then
        battleStartDelegateFunc(data_);
        battleStartDelegateFunc = nil;
    end
end

function OnReqPVELeagueBattleResult(code_, data_)
    print('OnReqPVELeagueBattleResult..');
    if (battleResultDelegateFunc ~= nil) then
        battleResultDelegateFunc(data_);
        battleResultDelegateFunc = nil;
    end
end

function SetCurrentInfo()
    if (infoData[currentSeasonNum] ~= nil) then
        currentMatchIndex = infoData[currentSeasonNum].s + 1;
        currentTurn = infoData[currentSeasonNum].l[currentMatchIndex].t;
        local matchList = infoData[currentSeasonNum].l;
        local matchListLength = table.getn(matchList);
        local tempClub = nil;
        for i = 1, matchListLength, 1 do
            if (matchList[i].t > maxTurn) then
                maxTurn = matchList[i].t;
            end
            if (matchList[i].t == currentTurn) then
                if (matchList[i].f == 1) then
                    for j = 1, 2, 1 do
                        tempClub = matchList[i].u[j];
                        if (tempClub ~= "0") then
                            currentOpponent = tempClub;
                        end
                    end
                end
            end
        end
    end
end

function ClickChallengeTips()
    -- if GetCostTimes() >= 3 then
    --     WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("UIPeakRoad_NoChallengeTimes")  });
    -- else
    local OnMatch = function()
        RequestPVELeagueBattleStart(currentMatchIndex, OnChallengeStart);
    end;

    UIPrepareMatch.RegisterBtnMatch(OnMatch);
    UIPrepareMatch.TryOpen({ PrepareMatchType = UIPrepareMatch.enum_PrepareMatchType.PVELeague });
    -- end
end

function OnChallengeStart(data)
    if (data ~= nil) then
        CombatData.FillData(data, CombatData.COMBAT_TYPE.PVELEAGUE, OnReqRaidMatchResult);
    end
end

function OnReqRaidMatchResult()
    print('OnReqRaidMatchResult');
    local tb = CombatData.GetResultTable();
    local quit = tb['Giveup'];
    if not quit then
        local homeScore = tb['HomeScore'];
        local awayScore = tb['AwayScore'];
        local score = tostring(homeScore..':'..awayScore);
        local actionList = tostring(tb['PVEData']);
        local signMD5 = tb['md5'];
        RequestPVELeagueBattleResult(currentMatchIndex, score, actionList, signMD5, OnChallengeResult);
    end
end

function OnChallengeResult(data_)
    if (data_ ~= nil) then
        data_.BattleResultType = UIBattleResultS.enum_BattleResultType.PVELeague;
        data_.Callback = OnCloseResult;
        battleResultData = data_;
        if (data_['upgrade'] ~= nil) then
            upgrade = data_['upgrade'];
        end
        if (tonumber(data_['score'][1]) > tonumber(data_['score'][2])) then
            WindowMgr.ShowWindow(LuaConst.Const.UIBattleResultS, data_);
        else
            WindowMgr.ShowWindow(LuaConst.Const.UIBattleResultF, data_);
        end
    end
end

function OnCloseResult()
    infoData[currentSeasonNum] = battleResultData.list;
    SetCurrentInfo();

    UIPVELeagueSchedule.RefreshUIFromResult(battleResultData);
    UIPVELeagueSchedule.ShowTurnBox(upgrade);
end

function GetInfoData()
    return infoData;
end

function GetLeagueConfig()
    if (leagueConfig ~= nil) then
        return leagueConfig;
    end

    leagueConfig = Config.GetTemplate(Config.PVELeague());
    return leagueConfig;
end

function GetLeagueClubConfig()
    if (leagueClubConfig ~= nil) then
        return leagueClubConfig;
    end

    leagueClubConfig = Config.GetTemplate(Config.PVELeagueClub());
    return leagueClubConfig;
end

function GetLeagueTeamConfig()
    if (leagueTeamConfig ~= nil) then
        return leagueTeamConfig;
    end

    leagueTeamConfig = Config.GetTemplate(Config.PVELeagueTeam());
    return leagueTeamConfig;
end

function GetLeagueRewardConfig()
    if (leagueRewardConfig ~= nil) then
        return leagueRewardConfig;
    end

    leagueRewardConfig = Config.GetTemplate(Config.PVELeagueReward());
    return leagueRewardConfig;
end

function GetLeagueNPCConfig()
    if (leagueNPCConfig ~= nil) then
        return leagueNPCConfig;
    end

    leagueNPCConfig = Config.GetTemplate(Config.PVELeagueNPC());
    return leagueNPCConfig;
end

function GetCurrEnemyNameLvInfo()
    print('GetCurrEnemyNameLvInfo..');
    local teamIdStr = tostring(leagueClubConfig[currentOpponent].team);
    local name  = leagueClubConfig[currentOpponent].name;
    local lv    = Config.GetProperty(Config.PVELeagueTeam(), teamIdStr, 'lv');
    local icon  = leagueClubConfig[currentOpponent].icon;
    local team  = Config.GetProperty(Config.PVELeagueTeam(), teamIdStr, 'team');
    local score = Config.GetProperty(Config.PVELeagueTeam(), teamIdStr, 'score');
    if score == nil then
        score = -1;
    end
    local formIconTb = {};
    local heroTb = Config.GetProperty(Config.PVELeagueTeam(), teamIdStr, 'hero');
    for i = 2, #heroTb, 2 do
        table.insert(formIconTb, Config.GetProperty(Config.PVELeagueNPC(), heroTb[i], 'playericon'));
    end
    return name, lv, icon, team, score, formIconTb;
end
