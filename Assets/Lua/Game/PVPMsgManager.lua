module("PVPMsgManager", package.seeall);

--require "Game/DataSystemScript"

local RegisteredMsgs = {
    MsgID.tb.LadderInfo,
    MsgID.tb.LadderJoin,
    MsgID.tb.LadderResult,
    MsgID.tb.LadderQuit,
    MsgID.tb.LadderSort,
    MsgID.tb.LadderadvAward,
    MsgID.tb.LadderwinAward,

    MsgID.tb.LeagueInfo,
    MsgID.tb.LeagueGroup,
    MsgID.tb.LeagueJoin,

    MsgID.tb.DailyCupInfo,
    MsgID.tb.DailyCupJoin,
    MsgID.tb.DailyCupMass,
    MsgID.tb.DailyCupAl,
    MsgID.tb.DailyCupAward,
    MsgID.tb.DailyCupGam,
    MsgID.tb.DailyCupGl,
    MsgID.tb.DailyCupGAward
}

local PVPDataTable = {};
local PVPLocalDataTable = {};

local PVPOnReqMsgTable = {};
local PVPOnReqMsgFailedTable = {};

-- local PVPReqPreProcessTable = {};
local PVPReqDoNotShowAnimFlag = {};
local PVPReqPostProcessTable = {};


local PVPDelegateTable = {};
local PVPFailedDelegateTable = {};

local LadderSeasonData = nil;
local LadderEnemyInfo = {};
local isLadderWaitLocked = nil;
local LadderSeasonKeyData = nil;
local LeagueTimeTable = nil;
local DailyCupLocalDataTable = nil;

function OnInit()
    RegisterMsgs();
end

function OnRelease()
    UnregisterMsgs();
end

function RegisterMsgs()
    --common delegates
    for k,v in pairs(RegisteredMsgs) do
        PVPOnReqMsgTable[v] = function (code_, data_)
            OnRequestMsgs(v, code_, data_);
        end
        DataSystemScript.RegisterMsgHandler(v, PVPOnReqMsgTable[v]);

        PVPOnReqMsgFailedTable[v] = function (code_, data_)
            OnRequestMsgsFailed(v, code_, data_);
        end
        DataSystemScript.RegisterFailedMsgHandler(v, PVPOnReqMsgFailedTable[v]);
    end

    -- --unique delegates
    -- PVPReqPreProcessTable[MsgID.tb.LadderWait] = function ()
    --     if(IsLadderWaitRequestLocked()) then
    --         return;
    --     end
    --     LockLadderWaitRequest(true);
    -- end

    -- PVPReqDoNotShowAnimFlag[MsgID.tb.LadderWait] = true;

    -- PVPReqPostProcessTable[MsgID.tb.LadderWait] = function ()
    --     LockLadderWaitRequest(false);
    -- end
end

function UnregisterMsgs()
    for k,v in pairs(RegisteredMsgs) do
        DataSystemScript.UnRegisterMsgHandler(v, PVPOnReqMsgTable[v]);
        DataSystemScript.UnRegisterFailedMsgHandler(v, PVPOnReqMsgFailedTable[v]);
    end
end

--every single delegate is for single use only, multi-watcher not supported
function RequestPVPMsg( msgId, protocolConst, paramDict, delegateFunc, delegateFunconFailed )
    -- if(PVPReqPreProcessTable~={} and PVPReqPreProcessTable[msgId]~=nil) then
    --     PVPReqPreProcessTable[msgId]();
    -- end    

    PVPDelegateTable[msgId] = delegateFunc;
    PVPFailedDelegateTable[msgId] = delegateFunconFailed;
    DataSystemScript.RequestWithParams(protocolConst, paramDict, msgId, PVPReqDoNotShowAnimFlag[msgId]);
end

function OnRequestMsgs(msgId_, code_, data_)
    if (data_ == nil) then
        print("MSG ID:"..msgId_..",data is null");
    end

    if(PVPReqPostProcessTable~={} and PVPReqPostProcessTable[msgId_]~=nil) then
        PVPReqPostProcessTable[msgId_]();
    end    

    PVPDataTable[msgId_] = data_;
    if (PVPDelegateTable[msgId_] ~= nil) then
        PVPDelegateTable[msgId_]();
    end
end

function OnRequestMsgsFailed(msgId_, code_, data_)
    print("OnRequestMsgsFailed:"..msgId_);

    if(PVPReqPostProcessTable~={} and PVPReqPostProcessTable[msgId_]~=nil) then
        PVPReqPostProcessTable[msgId_]();
    end    
    
    PVPDataTable[msgId_] = data_;
    if (PVPFailedDelegateTable[msgId_] ~= nil) then
        PVPFailedDelegateTable[msgId_]();
    end
end

function GetPVPData(msgId_)
    if(PVPDataTable==nil or IsTableEmpty(PVPDataTable))then
        return nil;
    end
    return PVPDataTable[msgId_];
end

function SetLadderSeasonData(data)
    LadderSeasonData = data;
end

function GetLadderSeasonData()
    return LadderSeasonData;
end

function GetLadderSeasonKeyData()
     if(LadderSeasonKeyData~=nil and LadderSeasonKeyData.seasonStartYear~=nil and LadderSeasonKeyDataseasonIndexThisYear~=nil)then
        return LadderSeasonKeyDataseasonStartYear.."_"..LadderSeasonKeyDataseasonIndexThisYear.."_"..Role.Get_oid();
    else

        local infoData = PVPMsgManager.GetPVPData(MsgID.tb.LadderInfo);
        --datas used to calculate
        local currentSeason = infoData.serv.sign;
        local daysInPerSeason = infoData.serv.cycle;
        local daysBeforeSeasonStart = (currentSeason-1)*daysInPerSeason;
        local serverStartTimeStamp = Login.GetSrvInfo(DataSystemScript.GetRegionId()).time;
        local seasonStartDayOfYear =    Util.GetDayInYearAfterAddDay(serverStartTimeStamp,daysBeforeSeasonStart);

        LadderSeasonKeyData = {};
        LadderSeasonKeyData.seasonStartYear =           Util.GetYearAfterAddDay(serverStartTimeStamp,daysBeforeSeasonStart);
        LadderSeasonKeyData.seasonIndexThisYear =       math.floor((seasonStartDayOfYear-1)/daysInPerSeason) + 1;
        return LadderSeasonKeyData.seasonStartYear.."_"..LadderSeasonKeyData.seasonIndexThisYear.."_"..Role.Get_oid();
    end
end

function Get_LeagueTimeTable()
    if(LeagueTimeTable == nil or IsTableEmpty(LeagueTimeTable)) then
        LeagueTimeTable = Config.GetTemplate(Config.LeagueTimeTable());
    end
    return LeagueTimeTable;
end

function Get_DialyCupLocalData(jsonName)
    if(DailyCupLocalDataTable == nil)then
        DailyCupLocalDataTable = {};
    end
    if( DailyCupLocalDataTable[jsonName] == nil) then
        DailyCupLocalDataTable[jsonName] = Config.GetTemplate(jsonName);
    end
    return DailyCupLocalDataTable[jsonName];
end

-- =========================== Getter functions ===========================

function UpdateLadderadvAwardLogs( ranklevel )
    if(PVPDataTable[MsgID.tb.LadderInfo]~=nil)then
        table.insert(PVPDataTable[MsgID.tb.LadderInfo].info.advlog,ranklevel);
    end
end

function LockLadderWaitRequest(willLock)
    isLadderWaitLocked = willLock;
end

function IsLadderWaitRequestLocked()
    return isLadderWaitLocked;
end

function UpdateLeagueInfoData(level)
    if(PVPDataTable[MsgID.tb.LeagueInfo]~=nil) then
        --my match is available in next season
        PVPDataTable[MsgID.tb.LeagueInfo].level = level;
    end
end

function GetLadderRule()
    local ruleData = Config.GetTemplate(Config.LadderRule());
    if(ruleData==nil or ruleData["1"]==nil or ruleData["1"].content==nil)then
        return "未配置天梯规则";
    end
    return ruleData["1"].content;
end

function GetLeagueRule()
    local ruleData = Config.GetTemplate(Config.LeagueRule());
    local rules = GetLocalizedString("LeagueRules");
    local ruleList = {};
    for k,v in pairs(ruleData) do
        ruleList[tonumber(v.id)] = v.content;
    end
    for i,v in ipairs(ruleList) do
        rules = rules.."\n"..i..","..v;
    end
    return rules;
end

function GetDailyCupRule()
    local ruleData = Config.GetTemplate(Config.DailyCupRule());
    local rules = GetLocalizedString("DailyCupRules");
    local ruleList = {};
    for k,v in pairs(ruleData) do
        ruleList[tonumber(v.id)] = v.content;
    end
    for i,v in ipairs(ruleList) do
        rules = rules.."\n"..i..","..v;
    end
    return rules;
end

function SetLadderEnemyInfo(name,lv,icon,formationID,heroTable)

    local sumPower = 0;
    local iconTable = {};
    for k,v in pairs(heroTable) do
        sumPower = sumPower+tonumber(v.ap);
        iconTable[v.pos+1] = v.id;
        print(v.id);
    end

    LadderEnemyInfo = {
        Name = name,
        Level = lv,
        Icon=icon,
        FormationID = formationID,
        Power = sumPower,
        IconTable = iconTable
    };
end

function GetLadderEnemyInfo()
    return LadderEnemyInfo.Name,LadderEnemyInfo.Level,LadderEnemyInfo.Icon,LadderEnemyInfo.FormationID,LadderEnemyInfo.Power,LadderEnemyInfo.IconTable;
end


-- ========================================= legacy codes =========================================

-- local LadderInfoData = nil;      -- {serv = {sign,days,start},
--                              -- info = {score,level,chance,result},
--                              -- wait = {state,user,goal,result]},
--                              -- msgid};
-- local LadderJoinData = nil;  --0/1?
-- local LadderWaitData = nil;      --{state,user,goal,result,msgid};
-- local LadderQuitData = nil;  --0/1?
-- local LadderSortData = nil;      --this is a list
-- local LadderadvAwardData = nil; 
-- local LadderwinAwardData = nil;

-- local LeagueInfoData = nil;
-- local LeagueGroupData = nil;
-- local LeagueJoinData = nil;

-- local DailyCupInfoData = nil;
-- local DailyCupJoinData = nil;
-- local DailyCupMassData = nil;
-- local DailyCupAlData = nil;
-- local DailyCupAwardData = nil;
-- local DailyCupGamData = nil;
-- local DailyCupGlData = nil;
-- local DailyCupGAwardData = nil;

-- local LadderInfoDelegateFunc = nil;
-- local LadderJoinDelegateFunc = nil;
-- local LadderWaitDelegateFunc = nil;
-- local LadderQuitDelegateFunc = nil;
-- local LadderSortDelegateFunc = nil;
-- local LadderadvAwardDelegateFunc = nil;
-- local LadderwinAwardDelegateFunc = nil;

-- local LeagueInfoDelegateFunc = nil;
-- local LeagueGroupDelegateFunc = nil;
-- local LeagueJoinDelegateFunc = nil;

-- local DailyCupInfoDelegateFunc = nil;
-- local DailyCupJoinDelegateFunc = nil;
-- local DailyCupMassDelegateFunc = nil;
-- local DailyCupAlDelegateFunc = nil;
-- local DailyCupAwardDelegateFunc = nil;
-- local DailyCupGamDelegateFunc = nil;
-- local DailyCupGlDelegateFunc = nil;
-- local DailyCupGAwardDelegateFunc = nil;

-- --delegate functions on msging failed, for ladder events
-- local LadderInfoFailedDelegateFunc = nil;
-- local LadderJoinFailedDelegateFunc = nil;
-- local LadderWaitFailedDelegateFunc = nil;
-- local LadderQuitFailedDelegateFunc = nil;
-- local LadderSortFailedDelegateFunc = nil;
-- local LadderadvAwardFailedDelegateFunc = nil;
-- local LadderwinAwardFailedDelegateFunc = nil;

-- local DailyCupInfoFailedDelegateFunc = nil;

-- function OnInit()
 --    DataSystemScript.RegisterMsgHandler(MsgID.tb.LadderInfo, OnReqLadderInfo);
    -- DataSystemScript.RegisterMsgHandler(MsgID.tb.LadderJoin, OnReqLadderJoin);
    -- DataSystemScript.RegisterMsgHandler(MsgID.tb.LadderWait, OnReqLadderWait);
 --    DataSystemScript.RegisterMsgHandler(MsgID.tb.LadderQuit, OnReqLadderQuit);
 --    DataSystemScript.RegisterMsgHandler(MsgID.tb.LadderSort, OnReqLadderSort);
 --    DataSystemScript.RegisterMsgHandler(MsgID.tb.LadderadvAward, OnReqLadderadvAward);
 --    DataSystemScript.RegisterMsgHandler(MsgID.tb.LadderwinAward, OnReqLadderwinAward);

 --    DataSystemScript.RegisterMsgHandler(MsgID.tb.LeagueInfo, OnReqLeagueInfo);
 --    DataSystemScript.RegisterMsgHandler(MsgID.tb.LeagueGroup, OnReqLeagueGroup);
 --    DataSystemScript.RegisterMsgHandler(MsgID.tb.LeagueJoin, OnReqLeagueJoin);
  
 --    DataSystemScript.RegisterMsgHandler( MsgID.tb.DailyCupInfo,    OnReqDailyCupInfo );
 --    DataSystemScript.RegisterMsgHandler( MsgID.tb.DailyCupJoin,    OnReqDailyCupJoin );
 --    DataSystemScript.RegisterMsgHandler( MsgID.tb.DailyCupMass,    OnReqDailyCupMass );
 --    DataSystemScript.RegisterMsgHandler( MsgID.tb.DailyCupAl,      OnReqDailyCupAl );
 --    DataSystemScript.RegisterMsgHandler( MsgID.tb.DailyCupAward,   OnReqDailyCupAward );
 --    DataSystemScript.RegisterMsgHandler( MsgID.tb.DailyCupGam,     OnReqDailyCupGam );
 --    DataSystemScript.RegisterMsgHandler( MsgID.tb.DailyCupGl,      OnReqDailyCupGl );
 --    DataSystemScript.RegisterMsgHandler( MsgID.tb.DailyCupGAward,  OnReqDailyCupGAward );

 --    DataSystemScript.RegisterFailedMsgHandler(MsgID.tb.LadderInfo, OnReqLadderInfoFailed);
 --    DataSystemScript.RegisterFailedMsgHandler(MsgID.tb.LadderJoin, OnReqLadderJoinFailed);
 --    DataSystemScript.RegisterFailedMsgHandler(MsgID.tb.LadderWait, OnReqLadderWaitFailed);
 --    DataSystemScript.RegisterFailedMsgHandler(MsgID.tb.LadderQuit, OnReqLadderQuitFailed);
 --    DataSystemScript.RegisterFailedMsgHandler(MsgID.tb.LadderSort, OnReqLadderSortFailed);
 --    DataSystemScript.RegisterFailedMsgHandler(MsgID.tb.LadderadvAward, OnReqLadderadvAwardFailed);
 --    DataSystemScript.RegisterFailedMsgHandler(MsgID.tb.LadderwinAward, OnReqLadderwinAwardFailed);

 --    DataSystemScript.RegisterFailedMsgHandler(MsgID.tb.DailyCupInfo, OnReqDailyCupInfoFailed);
-- end

-- function OnRelease()
    -- DataSystemScript.UnRegisterMsgHandler(MsgID.tb.LadderInfo, OnReqLadderInfo);
    -- DataSystemScript.UnRegisterMsgHandler(MsgID.tb.LadderJoin, OnReqLadderJoin);
 --    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.LadderWait, OnReqLadderWait);
 --    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.LadderQuit, OnReqLadderQuit);
 --    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.LadderSort, OnReqLadderSort);
 --    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.LadderadvAward, OnReqLadderadvAward);
 --    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.LadderwinAward, OnReqLadderwinAward);

 --    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.LeagueInfo, OnReqLeagueInfo);
 --    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.LeagueGroup, OnReqLeagueGroup);
 --    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.LeagueJoin, OnReqLeagueJoin);

 --    DataSystemScript.UnRegisterMsgHandler( MsgID.tb.DailyCupInfo,    OnReqDailyCupInfo );
 --    DataSystemScript.UnRegisterMsgHandler( MsgID.tb.DailyCupJoin,    OnReqDailyCupJoin );
 --    DataSystemScript.UnRegisterMsgHandler( MsgID.tb.DailyCupMass,    OnReqDailyCupMass );
 --    DataSystemScript.UnRegisterMsgHandler( MsgID.tb.DailyCupAl,      OnReqDailyCupAl );
 --    DataSystemScript.UnRegisterMsgHandler( MsgID.tb.DailyCupAward,   OnReqDailyCupAward );
 --    DataSystemScript.UnRegisterMsgHandler( MsgID.tb.DailyCupGam,     OnReqDailyCupGam );
 --    DataSystemScript.UnRegisterMsgHandler( MsgID.tb.DailyCupGl,      OnReqDailyCupGl );
 --    DataSystemScript.UnRegisterMsgHandler( MsgID.tb.DailyCupGAward,  OnReqDailyCupGAward );

 --    DataSystemScript.UnRegisterFailedMsgHandler(MsgID.tb.LadderInfo, OnReqLadderInfoFailed);
 --    DataSystemScript.UnRegisterFailedMsgHandler(MsgID.tb.LadderJoin, OnReqLadderJoinFailed);
 --    DataSystemScript.UnRegisterFailedMsgHandler(MsgID.tb.LadderWait, OnReqLadderWaitFailed);
 --    DataSystemScript.UnRegisterFailedMsgHandler(MsgID.tb.LadderQuit, OnReqLadderQuitFailed);
 --    DataSystemScript.UnRegisterFailedMsgHandler(MsgID.tb.LadderSort, OnReqLadderSortFailed);
 --    DataSystemScript.UnRegisterFailedMsgHandler(MsgID.tb.LadderadvAward, OnReqLadderadvAwardFailed);
 --    DataSystemScript.UnRegisterFailedMsgHandler(MsgID.tb.LadderwinAward, OnReqLadderwinAwardFailed);
    
 --    DataSystemScript.UnRegisterFailedMsgHandler(MsgID.tb.DailyCupInfo, OnReqDailyCupInfoFailed);
-- end

-- =========================== Request functions ===========================

-- function RequestLadderInfo( delegatefunc, delegatefunconfailed )
-- 	LadderInfoDelegateFunc = delegatefunc;
--     LadderInfoFailedDelegateFunc = delegatefunconfailed;
-- 	DataSystemScript.RequestWithParams(LuaConst.Const.LadderInfo, nil, MsgID.tb.LadderInfo);
-- end

-- function RequestLadderJoin( delegatefunc, delegatefunconfailed )
-- 	LadderJoinDelegateFunc = delegatefunc;
--     LadderJoinFailedDelegateFunc = delegatefunconfailed;
-- 	DataSystemScript.RequestWithParams(LuaConst.Const.LadderJoin, nil, MsgID.tb.LadderJoin);
-- end

-- function RequestLadderWait( delegatefunc, delegatefunconfailed )
--     if(IsLadderWaitRequestLocked()) then
--         return;
--     end

--     LockLadderWaitRequest(true);

-- 	LadderWaitDelegateFunc = delegatefunc;
--     LadderWaitFailedDelegateFunc = delegatefunconfailed;
-- 	DataSystemScript.RequestWithParams(LuaConst.Const.LadderWait, nil, MsgID.tb.LadderWait);
-- end

-- function RequestLadderQuit( delegatefunc, delegatefunconfailed )
-- 	LadderQuitDelegateFunc = delegatefunc;
--     LadderQuitFailedDelegateFunc = delegatefunconfailed;
-- 	DataSystemScript.RequestWithParams(LuaConst.Const.LadderQuit, nil, MsgID.tb.LadderQuit);
-- end

-- function RequestLadderSort( paramdict, delegatefunc, delegatefunconfailed )
-- 	LadderSortDelegateFunc = delegatefunc;
--     LadderSortFailedDelegateFunc = delegatefunconfailed;
-- 	DataSystemScript.RequestWithParams(LuaConst.Const.LadderSort, paramdict, MsgID.tb.LadderSort);
-- end

-- function RequestLadderadvAward( paramdict, delegatefunc, delegatefunconfailed )
--     LadderadvAwardDelegateFunc = delegatefunc;
--     LadderadvAwardFailedDelegateFunc = delegatefunconfailed;
--     DataSystemScript.RequestWithParams(LuaConst.Const.LadderadvAward, paramdict, MsgID.tb.LadderadvAward);
-- end

-- function RequestLadderwinAward( paramdict, delegatefunc, delegatefunconfailed )
--     LadderwinAwardDelegateFunc = delegatefunc;
--     LadderwinAwardFailedDelegateFunc = delegatefunconfailed;
--     DataSystemScript.RequestWithParams(LuaConst.Const.LadderwinAward, paramdict, MsgID.tb.LadderwinAward);
-- end

-- function RequestLeagueInfo( paramdict, delegatefunc )
--     LeagueInfoDelegateFunc = delegatefunc;
--     DataSystemScript.RequestWithParams(LuaConst.Const.LeagueInfo, paramdict, MsgID.tb.LeagueInfo);
-- end

-- function RequestLeagueGroup( paramdict, delegatefunc )
--     LeagueGroupDelegateFunc = delegatefunc;
--     DataSystemScript.RequestWithParams(LuaConst.Const.LeagueGroup, paramdict, MsgID.tb.LeagueGroup);
-- end

-- function RequestLeagueJoin( paramdict, delegatefunc )
--     LeagueJoinDelegateFunc = delegatefunc;
--     DataSystemScript.RequestWithParams(LuaConst.Const.LeagueJoin, paramdict, MsgID.tb.LeagueJoin);
-- end

-- function RequestDailyCupInfo( paramdict, delegatefunc, delegatefunconfailed )
--     DailyCupInfoDelegateFunc = delegatefunc;
--     DailyCupInfoFailedDelegateFunc = delegatefunconfailed;
--     DataSystemScript.RequestWithParams(LuaConst.Const.DailyCupInfo, paramdict, MsgID.tb.DailyCupInfo);
-- end

-- function RequestDailyCupJoin( paramdict, delegatefunc )
--     DailyCupJoinDelegateFunc = delegatefunc;
--     DataSystemScript.RequestWithParams(LuaConst.Const.DailyCupJoin, paramdict, MsgID.tb.DailyCupJoin);
-- end

-- function RequestDailyCupMass( paramdict, delegatefunc )
--     DailyCupMassDelegateFunc = delegatefunc;
--     DataSystemScript.RequestWithParams(LuaConst.Const.DailyCupMass, paramdict, MsgID.tb.DailyCupMass);
-- end

-- function RequestDailyCupAl( paramdict, delegatefunc )
--     DailyCupAlDelegateFunc = delegatefunc;
--     DataSystemScript.RequestWithParams(LuaConst.Const.DailyCupAl, paramdict, MsgID.tb.DailyCupAl);
-- end

-- function RequestDailyCupAward( paramdict, delegatefunc )
--     DailyCupAwardDelegateFunc = delegatefunc;
--     DataSystemScript.RequestWithParams(LuaConst.Const.DailyCupAward, paramdict, MsgID.tb.DailyCupAward);
-- end

-- function RequestDailyCupGam( paramdict, delegatefunc )
--     DailyCupGamDelegateFunc = delegatefunc;
--     DataSystemScript.RequestWithParams(LuaConst.Const.DailyCupGam, paramdict, MsgID.tb.DailyCupGam);
-- end

-- function RequestDailyCupGl( paramdict, delegatefunc )
--     DailyCupGlDelegateFunc = delegatefunc;
--     DataSystemScript.RequestWithParams(LuaConst.Const.DailyCupGl, paramdict, MsgID.tb.DailyCupGl);
-- end

-- function RequestDailyCupGAward( paramdict, delegatefunc )
--     DailyCupGAwardDelegateFunc = delegatefunc;
--     DataSystemScript.RequestWithParams(LuaConst.Const.DailyCupGAward, paramdict, MsgID.tb.DailyCupGAward);
-- end
-- =========================== Request functions ===========================


-- =========================== Recieve functions ===========================

--on req ladder msgs
-- function OnReqLadderInfo(code_, data_)
--     print(".. OnReqLadderInfo!!!");
--     if (data_ == nil) then
--     	print("LadderInfoData is null");
--     end
--     LadderInfoData = data_;
--     if (LadderInfoDelegateFunc ~= nil) then
--     	print("Delegate called");
--         LadderInfoDelegateFunc();
--         -- LadderInfoDelegateFunc = nil;
--     else
--         print("Delegate not called");       
--     end
-- end

-- function OnReqLadderJoin(code_, data_)
--     print(".. OnReqLadderJoin!!!");
--     if (data_ == nil) then
--     	print("LadderJoinData is null");
--     end
--     LadderJoinData = data_;
--     if (LadderJoinDelegateFunc ~= nil) then
--     	LadderJoinDelegateFunc();
--         -- LadderJoinDelegateFunc = nil;
--     end
-- end

-- function OnReqLadderWait(code_, data_)
--     print(".. OnReqLadderWait!!!");

--     LockLadderWaitRequest(false);

--     if (data_ == nil) then
--     	print("LadderWaitData is null");
--     end
--     LadderWaitData = data_;
--     if (LadderWaitDelegateFunc ~= nil) then
--     	LadderWaitDelegateFunc();
--         -- LadderWaitDelegateFunc = nil;
--     end
-- end

-- function OnReqLadderQuit(code_, data_)
--     print(".. OnReqLadderQuit!!!");
--     if (data_ == nil) then
--     	print("LadderQuitData is null");
--     end
--     LadderQuitData = data_;
--     if (LadderQuitDelegateFunc ~= nil) then
--     	LadderQuitDelegateFunc();
--         -- LadderQuitDelegateFunc = nil;
--     end
-- end

-- function OnReqLadderSort(code_, data_)
--     print(".. OnReqLadderSort!!!");
--     if (data_ == nil) then
--     	print("LadderSortData is null");
--     end
--     LadderSortData = data_;
--     if (LadderSortDelegateFunc ~= nil) then
--     	LadderSortDelegateFunc();
--         -- LadderSortDelegateFunc = nil;
--     end
-- end

-- function OnReqLadderadvAward(code_, data_)
--     print(".. OnReqLadderadvAward!!!");
--     if (data_ == nil) then
--         print("LadderadvAwardData is null");
--     end
--     LadderadvAwardData = data_;
--     if (LadderadvAwardDelegateFunc ~= nil) then
--         LadderadvAwardDelegateFunc();
--         -- LadderadvAwardDelegateFunc = nil;
--     end
-- end

-- function OnReqLadderwinAward(code_, data_)
--     print(".. OnReqLadderwinAward!!!");
--     if (data_ == nil) then
--         print("LadderwinAwardData is null");
--     end
--     LadderwinAwardData = data_;
--     if (LadderwinAwardDelegateFunc ~= nil) then
--         LadderwinAwardDelegateFunc();
--         -- LadderwinAwardDelegateFunc = nil;
--     end
-- end

-- -- on req league msgs
-- function OnReqLeagueInfo(code_, data_)
--     print(".. OnReqLeagueInfo!!!");
--     if (data_ == nil) then
--         print("LeagueInfoData is null");
--     end
--     LeagueInfoData = data_;
--     if (LeagueInfoDelegateFunc ~= nil) then
--         print("Delegate called");
--         LeagueInfoDelegateFunc();
--         -- LeagueInfoDelegateFunc = nil;
--     else

--         print("Delegate not called");
--     end
-- end

-- function OnReqLeagueGroup(code_, data_)
--     print(".. OnReqLeagueGroup!!!");
--     if (data_ == nil) then
--         print("LeagueGroupData is null");
--     end
--     LeagueGroupData = data_;
--     if (LeagueGroupDelegateFunc ~= nil) then
--         print("Delegate called");
--         LeagueGroupDelegateFunc();
--         -- LeagueGroupDelegateFunc = nil;
--     else

--         print("Delegate not called");
--     end
-- end

-- function OnReqLeagueJoin(code_, data_)
--     print(".. OnReqLeagueJoin!!!");
--     if (data_ == nil) then
--         print("LeagueJoinData is null");
--     end
--     LeagueJoinData = data_;
--     if (LeagueJoinDelegateFunc ~= nil) then
--         LeagueJoinDelegateFunc();
--         -- LeagueJoinDelegateFunc = nil;
--     end
-- end

-- -- on req daily cup msgs
-- function OnReqDailyCupInfo(code_, data_)
--     print(".. OnReqDailyCupInfo!!!");
--     if (data_ == nil) then
--         print("DailyCupInfoData is null");
--     end
--     DailyCupInfoData = data_;
--     if (DailyCupInfoDelegateFunc ~= nil) then
--         print("Delegate called");
--         DailyCupInfoDelegateFunc();
--         -- DailyCupInfoDelegateFunc = nil;
--     else

--         print("Delegate not called");
--     end
-- end

-- function OnReqDailyCupJoin(code_, data_)
--     print(".. OnReqDailyCupJoin!!!");
--     if (data_ == nil) then
--         print("DailyCupJoinData is null");
--     end
--     DailyCupJoinData = data_;
--     if (DailyCupJoinDelegateFunc ~= nil) then
--         DailyCupJoinDelegateFunc();
--         -- DailyCupJoinDelegateFunc = nil;
--     end
-- end

-- function OnReqDailyCupMass(code_, data_)
--     print(".. OnReqDailyCupMass!!!");
--     if (data_ == nil) then
--         print("DailyCupMassData is null");
--     end
--     DailyCupMassData = data_;
--     if (DailyCupMassDelegateFunc ~= nil) then
--         DailyCupMassDelegateFunc();
--         -- DailyCupMassDelegateFunc = nil;
--     end
-- end

-- function OnReqDailyCupAl(code_, data_)
--     print(".. OnReqDailyCupAl!!!");
--     if (data_ == nil) then
--         print("DailyCupAlData is null");
--     end
--     DailyCupAlData = data_;
--     if (DailyCupAlDelegateFunc ~= nil) then
--         DailyCupAlDelegateFunc();
--         -- DailyCupAlDelegateFunc = nil;
--     end
-- end

-- function OnReqDailyCupAward(code_, data_)
--     print(".. OnReqDailyCupAward!!!");
--     if (data_ == nil) then
--         print("DailyCupAwardData is null");
--     end
--     DailyCupAwardData = data_;
--     if (DailyCupAwardDelegateFunc ~= nil) then
--         DailyCupAwardDelegateFunc();
--         -- DailyCupAwardDelegateFunc = nil;
--     end
-- end

-- function OnReqDailyCupGam(code_, data_)
--     print(".. OnReqDailyCupGam!!!");
--     if (data_ == nil) then
--         print("DailyCupGamData is null");
--     end
--     DailyCupGamData = data_;
--     if (DailyCupGamDelegateFunc ~= nil) then
--         DailyCupGamDelegateFunc();
--         -- DailyCupGamDelegateFunc = nil;
--     end
-- end

-- function OnReqDailyCupGl(code_, data_)
--     print(".. OnReqDailyCupGl!!!");
--     if (data_ == nil) then
--         print("DailyCupGlData is null");
--     end
--     DailyCupGlData = data_;
--     if (DailyCupGlDelegateFunc ~= nil) then
--         DailyCupGlDelegateFunc();
--         -- DailyCupGlDelegateFunc = nil;
--     end
-- end

-- function OnReqDailyCupGAward(code_, data_)
--     print(".. OnReqDailyCupGAward!!!");
--     if (data_ == nil) then
--         print("DailyCupGAwardData is null");
--     end
--     DailyCupGAwardData = data_;
--     if (DailyCupGAwardDelegateFunc ~= nil) then
--         DailyCupGAwardDelegateFunc();
--         -- DailyCupGAwardDelegateFunc = nil;
--     end
-- end

-- --========= functions on msg failed============
-- function OnReqLadderInfoFailed(code_, data_)
--     print(".. OnReqLadderInfoFailed!!!");
--     LadderInfoData = data_;
--     if (LadderInfoFailedDelegateFunc ~= nil) then
--         LadderInfoFailedDelegateFunc();
--         -- LadderInfoFailedDelegateFunc = nil;
--     end
-- end

-- function OnReqLadderJoinFailed(code_, data_)
--     print(".. OnReqLadderJoinFailed!!!");
--     LadderJoinData = data_;
--     if (LadderJoinFailedDelegateFunc ~= nil) then
--         LadderJoinFailedDelegateFunc();
--         -- LadderJoinFailedDelegateFunc = nil;
--     end
-- end

-- function OnReqLadderWaitFailed(code_, data_)
--     print(".. OnReqLadderWaitFailed!!!");
--     LockLadderWaitRequest(false);
--     LadderWaitData = data_;
--     if (LadderWaitFailedDelegateFunc ~= nil) then
--         LadderWaitFailedDelegateFunc();
--         -- LadderWaitFailedDelegateFunc = nil;
--     end
-- end

-- function OnReqLadderQuitFailed(code_, data_)
--     print(".. OnReqLadderQuitFailed!!!");
--     LadderQuitData = data_;
--     if (LadderQuitFailedDelegateFunc ~= nil) then
--         LadderQuitFailedDelegateFunc();
--         -- LadderQuitFailedDelegateFunc = nil;
--     end
-- end

-- function OnReqLadderSortFailed(code_, data_)
--     print(".. OnReqLadderSortFailed!!!");
--     LadderSortData = data_;
--     if (LadderSortFailedDelegateFunc ~= nil) then
--         LadderSortFailedDelegateFunc();
--         -- LadderSortFailedDelegateFunc = nil;
--     end
-- end

-- function OnReqLadderadvAwardFailed(code_, data_)
--     print(".. OnReqLadderadvAwardFailed!!!");
--     LadderadvAwardData = data_;
--     if (LadderadvAwardFailedDelegateFunc ~= nil) then
--         LadderadvAwardFailedDelegateFunc();
--         -- LadderadvAwardFailedDelegateFunc = nil;
--     end
-- end

-- function OnReqLadderwinAwardFailed(code_, data_)
--     print(".. OnReqLadderwinAwardFailed!!!");
--     LadderwinAwardData = data_;
--     if (LadderwinAwardFailedDelegateFunc ~= nil) then
--         LadderwinAwardFailedDelegateFunc();
--         -- LadderwinAwardFailedDelegateFunc = nil;
--     end
-- end

-- function OnReqDailyCupInfoFailed(code_, data_)
--     print(".. OnReqLadderwinAwardFailed!!!");
--     DailyCupInfoData = data_;
--     if (DailyCupInfoFailedDelegateFunc ~= nil) then
--         DailyCupInfoFailedDelegateFunc();
--         -- DailyCupInfoFailedDelegateFunc = nil;
--     end
-- end

-- function Get_LadderInfoData()
--     return LadderInfoData;
-- end

-- function Get_LadderJoinData()
--     return LadderJoinData;
-- end

-- function Get_LadderWaitData()
--     return LadderWaitData;
-- end

-- function Get_LadderQuitData()
--     return LadderQuitData;
-- end

-- function Get_LadderSortData()
--     return LadderSortData;
-- end

-- function Get_LadderadvAwardData()
--     return LadderadvAwardData;
-- end

-- function Get_LeagueInfoData()
--     return LeagueInfoData;
-- end

-- function Get_LeagueGroupData()
--     return LeagueGroupData;
-- end

-- function Get_LeagueJoinData()
--     return LeagueJoinData;
-- end

-- function Get_DailyCupInfoData()
--     return DailyCupInfoData;
-- end

-- function Get_DailyCupJoinData()
--     return DailyCupJoinData;
-- end

-- function Get_DailyCupMassData()
--     return DailyCupMassData;
-- end

-- function Get_DailyCupAlData()
--     return DailyCupAlData;
-- end

-- function Get_DailyCupAwardData()
--     return DailyCupAwardData;
-- end

-- function Get_DailyCupGamData()
--     return DailyCupGamData;
-- end

-- function Get_DailyCupGlData()
--     return DailyCupGlData;
-- end

-- function Get_DailyCupGAwardData()
--     return DailyCupGAwardData;
-- end
-- ========================================= legacy codes =========================================
