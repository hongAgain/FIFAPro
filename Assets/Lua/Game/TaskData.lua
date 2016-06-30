module("TaskData", package.seeall)

require "Common/UnityCommonScript"
require "Game/GameMainScript"
--require "Game/DataSystemScript"
require "Config"
--require "UILua/UITaskItemManager"

TaskProgress = 
{
    InProgress = 2,
    Complete = 1,
    GotReward = 3
}

local dailyTaskInfoDelegateFunc = nil
local dailyTaskSubmitDelegateFunc = nil
local dailyTaskAwardDelegateFunc = nil
local achieveTaskInfoDelegateFunc = nil
local achieveTaskSubmitDelegateFunc = nil

function OnInit()
    DataSystemScript.RegisterMsgHandler(MsgID.tb.DailyTaskInfo, OnReqDailyTaskInfo)
    DataSystemScript.RegisterMsgHandler(MsgID.tb.DailyTaskSubmit, OnReqDailyTaskSubmit)
    DataSystemScript.RegisterMsgHandler(MsgID.tb.DailyTaskAward, OnReqDailyTaskAward)
    DataSystemScript.RegisterMsgHandler(MsgID.tb.AchieveTaskInfo, OnReqAchieveTaskInfo)
    DataSystemScript.RegisterMsgHandler(MsgID.tb.AchieveTaskSubmit, OnReqAchieveTaskSubmit)
end

function OnRelease()
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.DailyTaskInfo, OnReqDailyTaskInfo)
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.DailyTaskSubmit, OnReqDailyTaskSubmit)
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.DailyTaskAward, OnReqDailyTaskAward)
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.AchieveTaskInfo, OnReqAchieveTaskInfo)
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.AchieveTaskSubmit, OnReqAchieveTaskSubmit)
end

function RequestDailyTaskInfo( delegatefunc )
    dailyTaskInfoDelegateFunc = delegatefunc
    DataSystemScript.RequestWithParams(LuaConst.Const.DailyTaskInfo, nil, MsgID.tb.DailyTaskInfo)
end
function RequestDailyTaskSubmit( parameters, delegatefunc )
    dailyTaskSubmitDelegateFunc = delegatefunc
    DataSystemScript.RequestWithParams(LuaConst.Const.DailyTaskSubmit, parameters, MsgID.tb.DailyTaskSubmit)
end
function RequestDailyTaskAward( parameters, delegatefunc )
    dailyTaskAwardDelegateFunc = delegatefunc
    DataSystemScript.RequestWithParams(LuaConst.Const.DailyTaskAward, parameters, MsgID.tb.DailyTaskAward)
end
function RequestAchieveTaskInfo( delegatefunc )
    achieveTaskInfoDelegateFunc = delegatefunc
    DataSystemScript.RequestWithParams(LuaConst.Const.AchieveTaskInfo, nil, MsgID.tb.AchieveTaskInfo)
end
function RequestAchieveTaskSubmit( parameters, delegatefunc )
    achieveTaskSubmitDelegateFunc = delegatefunc
    DataSystemScript.RequestWithParams(LuaConst.Const.AchieveTaskSubmit, parameters, MsgID.tb.AchieveTaskSubmit)
end

function OnReqDailyTaskInfo(_code, _data)
    if _code == nil then
        if (dailyTaskInfoDelegateFunc ~= nil) then
            dailyTaskInfoDelegateFunc(_data)
            dailyTaskInfoDelegateFunc = nil
        end
    end
end

function OnReqDailyTaskSubmit(_code, _data)
    if _code == nil then
        if (dailyTaskSubmitDelegateFunc ~= nil) then
            dailyTaskSubmitDelegateFunc(_data)
            dailyTaskSubmitDelegateFunc = nil
        end
    end
end

function OnReqDailyTaskAward(_code, _data)
    if _code == nil then
        if (dailyTaskAwardDelegateFunc ~= nil) then
            dailyTaskAwardDelegateFunc(_data)
            dailyTaskAwardDelegateFunc = nil
        end
    end
end

function OnReqAchieveTaskInfo(_code, _data)
    if _code == nil then
        if (achieveTaskInfoDelegateFunc ~= nil) then
            achieveTaskInfoDelegateFunc(_data)
            achieveTaskInfoDelegateFunc = nil
        end
    end
end

function OnReqAchieveTaskSubmit(_code, _data)
    if _code == nil then
        if (achieveTaskSubmitDelegateFunc ~= nil) then
            achieveTaskSubmitDelegateFunc(_data)
            achieveTaskSubmitDelegateFunc = nil
        end
    end
end

--table 遍历的迭代器，已key值排序遍历
function pairsByKeys( t )
    local a = {}      
    for n,_ in pairs(t) do          
        a[#a+1] = tonumber(n)      
    end      
    table.sort(a)      
    local i = 0      
    return function()          
        i = i + 1          
        return a[i], t[tostring(a[i])]      
    end 
end

function pairsByProgress(items)
    local a = {}
    for k,v in pairs(items) do
        a[#a+1] = {v.progress,tonumber(k)}
    end
    local func = function(x,y)
        if x[1] == y[1] then
            return x[2] < y[2]
        else
            return x[1] < y[1]
        end
    end
    table.sort(a,func)
    local i = 0
    return function ()
        i= i+1
        if a[i] == nil then
            return nil,nil
        else
            local key = tostring(a[i][2])
            return key,items[key]
        end
    end
end

function RenameItemsByOrder(items)
    local idx = 1
    for k,v in pairsByProgress(items) do
        v.transform.name = tostring(idx)
        if UITaskItemManager ~= nil then
            UITaskItemManager.SetTaskItemBG(v,idx)
        end
        idx = idx + 1
    end
end
