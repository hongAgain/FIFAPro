--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion

module("PlayerInfoData", package.seeall)


local m_lastFillLv = nil;
local m_lastFillAmount = nil;

local m_timerId = nil;
local m_currPlayerId = nil;
local m_lastClickTime = 0;
local MaxLv = Config.GetTemplate(Config.BaseTable())["mlv"];
function InitPlayerInfoData()
    m_lastFillLv = GetCurrHeroLv();
    m_lastFillAmount = GetCurrFillAmount();
    m_lastClickTime = 0;
end

function GetCurrHeroData()

    return Hero.GetHeroData2Id(GetCurrPlayerId());
end

function GetCurrHeroLv()
    
    return GetCurrHeroData()['lv'];
end

function GetCurrPlayerId()

    return m_currPlayerId;
end

function SetCurrPlayerId(playerId_)
    m_currPlayerId = playerId_;
end
function GetCurrFillAmount()
    return GetCurrExp()/GetCurrMaxExp();
end

function GetCurrExp()
    local lv = GetCurrHeroLv();
    if lv < 2 then
        return GetCurrHeroData()['exp'];
    else
        return GetCurrHeroData()['exp'] - Config.GetProperty(Config.LevelTable(), tostring(lv),"HExp");
    end

end

function GetCurrMaxExp()
    local lv = GetCurrHeroLv();
    if lv < 2 then
        return Config.GetProperty(Config.LevelTable(), "2","HExp") - Config.GetProperty(Config.LevelTable(), "1","HExp")
    else
        if tonumber(MaxLv) > lv then
            return Config.GetProperty(Config.LevelTable(), tostring(lv+1),"HExp") - Config.GetProperty(Config.LevelTable(), tostring(lv),"HExp");
        else
            return Config.GetProperty(Config.LevelTable(), tostring(lv),"HExp") - Config.GetProperty(Config.LevelTable(), tostring(lv-1),"HExp");
        end
    end

end

function GetLv2Exp(totalExp_)
    for i=1,MaxLv do
        if totalExp_ < tonumber(Config.GetProperty(Config.LevelTable(), tostring(i),"HExp")) then
            return i-1;
        end
    end

    return MaxLv;
end

function SetLastClickTime(args_)
    m_lastClickTime = args_;
end

function GetLastClickTime()
    return m_lastClickTime;
end

function OnDestroy()


end