module("TimeRaidData", package.seeall)

--require "DataSystemScript"
require "Common/CommonScript"
require "Game/HeroData"
require "Game/AutoReqMsg"

local TimeRaidInfoData = {};
local TimeRaidStartData = {};
local TimeRaidResultData = {};
local TimeRaidFastResultData = {};

local timeRaidLocalDataTable = {};

local TimeRaidInfoDelegateFunc = nil;
local TimeRaidStartDelegateFunc = nil;
local TimeRaidResultDelegateFunc = nil;
local TimeRaidFastResultDelegateFunc = nil;

local enemyInfo = {};

function OnInit()
    DataSystemScript.RegisterMsgHandler(MsgID.tb.TimeRaidInfo, OnReqTimeRaidInfo);
	DataSystemScript.RegisterMsgHandler(MsgID.tb.TimeRaidStart, OnReqTimeRaidStart);
	DataSystemScript.RegisterMsgHandler(MsgID.tb.TimeRaidResult, OnReqTimeRaidResult);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.TimeRaidFastResult, OnReqTimeRaidFastResult);

    InitInfoData();
end

function InitInfoData()
    TimeRaidInfoData.mt = {};
    TimeRaidInfoData.mt.__index = function (table,key)
        -- body
        if(TimeRaidInfoData[key]==nil) then
            return 0;
        else
            return TimeRaidInfoData[key];
        end
    end
    setmetatable(TimeRaidInfoData,TimeRaidInfoData.mt);
end

function GetPropertyInInfoData(key)
    return TimeRaidInfoData[key];
end

function OnRelease()
	DataSystemScript.UnRegisterMsgHandler(MsgID.tb.TimeRaidInfo, OnReqTimeRaidInfo);
	DataSystemScript.UnRegisterMsgHandler(MsgID.tb.TimeRaidStart, OnReqTimeRaidStart);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.TimeRaidResult, OnReqTimeRaidResult);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.TimeRaidFastResult, OnReqTimeRaidFastResult);
end

function RequestTimeRaidInfo( delegatefunc )
	TimeRaidInfoDelegateFunc = delegatefunc;
	DataSystemScript.RequestWithParams(LuaConst.Const.TimeRaidInfo, nil, MsgID.tb.TimeRaidInfo);
end

function RequestTimeRaidStart( parameters, delegatefunc )
	TimeRaidStartDelegateFunc = delegatefunc;
	DataSystemScript.RequestWithParams(LuaConst.Const.TimeRaidStart, parameters, MsgID.tb.TimeRaidStart);
end

function RequestTimeRaidResult( parameters, delegatefunc )
	TimeRaidResultDelegateFunc = delegatefunc;
	DataSystemScript.RequestWithParams(LuaConst.Const.TimeRaidResult, parameters, MsgID.tb.TimeRaidResult);
end

function RequestTimeRaidFastResult( parameters, delegatefunc )
	TimeRaidFastResultDelegateFunc = delegatefunc;
	DataSystemScript.RequestWithParams(LuaConst.Const.TimeRaidFastResult, parameters, MsgID.tb.TimeRaidFastResult);
end

function OnReqTimeRaidInfo(code_, data_)
	print(".. OnReqTimeRaidInfo!!!");
    TimeRaidInfoData = data_;
    if (TimeRaidInfoDelegateFunc ~= nil) then
        TimeRaidInfoDelegateFunc();
        TimeRaidInfoDelegateFunc = nil;
    end
end

function OnReqTimeRaidStart(code_, data_)
	print(".. OnReqTimeRaidStart!!!");
    TimeRaidStartData = data_;
    if (TimeRaidStartDelegateFunc ~= nil) then
        TimeRaidStartDelegateFunc();
        TimeRaidStartDelegateFunc = nil;
    end
end

function OnReqTimeRaidResult(code_, data_)
	print(".. OnReqTimeRaidResult!!!");
    TimeRaidResultData = data_;
    if (TimeRaidResultDelegateFunc ~= nil) then
        TimeRaidResultDelegateFunc();
        TimeRaidResultDelegateFunc = nil;
    end
end

function OnReqTimeRaidFastResult(code_, data_)
	print(".. OnReqTimeRaidFastResult!!!");
    TimeRaidFastResultData = data_;
    if (TimeRaidFastResultDelegateFunc ~= nil) then
        TimeRaidFastResultDelegateFunc();
        TimeRaidFastResultDelegateFunc = nil;
    end
end

function Get_TimeRaidInfoData()
    return TimeRaidInfoData;
end

function Get_TimeRaidStartData()
    return TimeRaidStartData;
end

function Get_TimeRaidResultData()
    return TimeRaidResultData;
end

function Get_TimeRaidFastResultData()
    return TimeRaidFastResultData;
end

function Get_TimeRaidLocalData()
    if(timeRaidLocalDataTable == nil) then
        timeRaidLocalDataTable = Config.GetTemplate(Config.RaidDSTable());
    end
    return timeRaidLocalDataTable;
end

function Update_TimeRaidInfoData(idJustChallenged)
    if(TimeRaidInfoData ~= nil and TimeRaidInfoData[tostring(idJustChallenged)] ~= nil) then
        TimeRaidInfoData[tostring(idJustChallenged)] = TimeRaidInfoData[tostring(idJustChallenged)]+1;
    end
end

function GetTimeRaidRule()
    local ruleData = Config.GetTemplate(Config.TimeRaidRule());
    if(ruleData==nil or ruleData["1"]==nil or ruleData["1"].content==nil)then
        return "未配置定时副本规则";
    end
    return ruleData["1"].content;
end

function SetEnemyInfo( name,lv,icon )
    enemyInfo = {Name = name,Level = lv,Icon = icon};
end

function GetEnemyInfo()
    return enemyInfo.Name,enemyInfo.Level,enemyInfo.Icon;
end