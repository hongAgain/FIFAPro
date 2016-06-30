module("UIActFreePower", package.seeall)

require "Common/CommonScript"
--显示数据来源于静态配置表
--ui
local cdLbl = nil
local getBtn = nil
local disableBtn = nil
local timeInternal = nil

local toNextTime = 0
local timeTick = nil
local timeTable = {}
local getIndex = 0
local isCanGet = false
local nextGetDurTime = 0

local maxPower = 600

--like bind ui
function Init(transform,winComponent)
    getBtn = TransformFindChild(transform, "EnableBtn")
    disableBtn = TransformFindChild(transform, "DisableBtn")
    cdLbl = TransformFindChild(transform, "CD")
    timeInternal = TransformFindChild(transform, "TimeInternal")
    Util.AddClick(getBtn.gameObject, GetFreePower)
    local tb = Config.GetTemplate(Config.FreePowerTable())
    for k,v in pairs(tb) do
        local st = v.STime:split(':')
        local et = v.ETime:split(':')
        timeTable[tonumber(k)] = {STime = st,ETime = et, SSecond = HMStoSecond(st), ESecond = HMStoSecond(et)}
    end
    local timeStr = nil
    for i,v in ipairs(timeTable) do
        --local v = timeTable[tostring(i)]
        local sTime = string.format("%02d:%02d", v.STime[1], v.STime[2])
        local eTime = string.format("%02d:%02d", v.ETime[1], v.ETime[2])
        if timeStr == nil then
            timeStr = sTime.." - "..eTime
        else
            local str = timeStr.."\n"..sTime.." - "..eTime
            timeStr = str
        end
    end 
    UIHelper.SetLabelTxt(timeInternal,timeStr)
    DataSystemScript.RegisterMsgHandler(MsgID.tb.FreePowerLogs, OnGetFreePowerData)
    DataSystemScript.RegisterMsgHandler(MsgID.tb.FreePowerFetch, OnFreePowerFetch)
end

function HMStoSecond( hms )
    return hms[1]*3600 + hms[2]*60+hms[3]
end

function OnShow()
    SetBtnActive(false)
    local param = {}
    --param["type"] = 'freePower'
    DataSystemScript.RequestWithParams(LuaConst.Const.FreePowerLogs, param, MsgID.tb.FreePowerLogs)
end

function OnHide()
end

function OnDestroy()
    if timeTick~=nil then
        LuaTimer.RmvTimer(timeTick)
        timeTick = nil
    end
    
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.FreePowerLogs, OnGetFreePowerData)
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.FreePowerFetch, OnFreePowerFetch)
end

function OnTimer()
    if (toNextTime >= 0) then
        isCanGet = false
        if(cdLbl ~= nil or GameObjectActiveSelf(cdLbl.gameObject)) then
            local h = math.floor(toNextTime / 3600)
            local m = math.floor((toNextTime - h * 3600) / 60)
            local s = toNextTime % 60

            UIHelper.SetLabelTxt(cdLbl, string.format("%02d:%02d:%02d", h, m, s))
        end
        --SetBtnActive(false)
        toNextTime = toNextTime - 1
    else
        if not isCanGet then
            isCanGet = true
            SetBtnActive(true)
        end
        nextGetDurTime = nextGetDurTime - 1
        if nextGetDurTime < 0 then
            LuaTimer.RmvTimer(timeTick)
            timeTick = nil
            DataSystemScript.RequestWithParams(LuaConst.Const.FreePowerLogs, {}, MsgID.tb.FreePowerLogs)
        end
    end
end

function GetFreePower()
    if ItemSys.GetItemData(LuaConst.Const.Power).num >=maxPower then
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { Util.LocalizeString("UIFreePowerLimit")})
        return
    end
    SetBtnActive(false)
    local param = {}
    GetCurrentIndex()
    param['id'] = getIndex
    DataSystemScript.RequestWithParams(LuaConst.Const.FreePowerFetch, param, MsgID.tb.FreePowerFetch)
end

function GetCurrentIndex()
    local curSecond = GetCurrentSecond()
    getIndex = 0
    for i,v in ipairs(timeTable) do
        if curSecond >= v.SSecond and curSecond <= v.ESecond then
            getIndex = i
            break
        end
    end
end

function GetCurrentSecond()
    local currentTime = math.modf(Util.GetTime()/1000)
    local hour = os.date("%H",currentTime)
    local minute = os.date("%M",currentTime)
    local second = os.date("%S",currentTime)
    local curSecond = HMStoSecond({hour,minute,second})
    return curSecond
end

function OnGetFreePowerData(code,data)
    if code ~= nil then
        return
    end
    local curSecond = GetCurrentSecond()

    for i,v in ipairs(timeTable) do
        if curSecond < v.SSecond then
            toNextTime = GetBetweenSecond(curSecond,i,false)
            nextGetDurTime = GetActiveSecond(curSecond, i, false)
            break
        elseif curSecond < v.ESecond then
            local hasGot = (type(data) == "table") and (data[tostring(i)] ~= nil)
            if hasGot then
                if i == #timeTable then
                    toNextTime = GetBetweenSecond(curSecond,1,true)
                    nextGetDurTime = GetActiveSecond(curSecond, 1, false)
                else
                    toNextTime = GetBetweenSecond(curSecond,i+1,false)
                    nextGetDurTime = GetActiveSecond(curSecond, i + 1, false)
                end
            else
                nextGetDurTime = GetActiveSecond(curSecond, i, true)
                toNextTime = -1
            end
            break
        else 
            if i==#timeTable then
                toNextTime = GetBetweenSecond(curSecond,1,true)
                nextGetDurTime = GetActiveSecond(curSecond, 1, false)
            end
        end

    end

    if timeTick~=nil then
        LuaTimer.RmvTimer(timeTick)
    end
    timeTick = LuaTimer.AddTimer(false,-1,OnTimer)
end

--获取当前时间到特定时间点的间隔时间
function GetBetweenSecond(curSecond,idx,nextDay)
    local sSecond = timeTable[idx].SSecond
    local betweenTime = 0
    if nextDay == false then
        betweenTime =  sSecond - curSecond
    else
        local dayTotelSecond = 24*3600
        betweenTime = dayTotelSecond - curSecond + sSecond
    end
    return betweenTime
end

function GetActiveSecond(curSecond, idx, isIn)
    local sSecond = timeTable[idx].SSecond
    local eSecond = timeTable[idx].ESecond
    if isIn then
        return eSecond - curSecond
    else
        return eSecond - sSecond
    end
end

function OnFreePowerFetch(code, data)
    if getIndex == #timeTable then
        toNextTime = GetBetweenSecond(GetCurrentSecond(),1,true)
        nextGetDurTime = GetActiveSecond(curSecond, 1, false)
    else 
        toNextTime = GetBetweenSecond(GetCurrentSecond(),getIndex+1,false)
        nextGetDurTime = GetActiveSecond(curSecond, getIndex + 1, false)
    end
end

function SetBtnActive(active)
    getBtn.gameObject:SetActive(active)
    disableBtn.gameObject:SetActive(not active)
end

--os.date("%c")
--%a  abbreviated weekday name (e.g., Wed)
--%A  full weekday name (e.g., Wednesday)
--%b  abbreviated month name (e.g., Sep)
--%B  full month name (e.g., September)
--%c  date and time (e.g., 09/16/98 23:48:10)
--%d  day of the month (16) [01-31]
--%H  hour, using a 24-hour clock (23) [00-23]
--%I  hour, using a 12-hour clock (11) [01-12]
--%M  minute (48) [00-59]
--%m  month (09) [01-12]
--%p  either "am" or "pm" (pm)
--%S  second (10) [00-61]
--%w  weekday (3) [0-6 = Sunday-Saturday]
--%x  date (e.g., 09/16/98)
--%X  time (e.g., 23:48:10)
--%Y  full year (1998)
--%y  two-digit year (98) [00-99]
--%%  the character '%'