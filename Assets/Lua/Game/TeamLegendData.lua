--region NewFile_1.lua
--Author : hongpeng.zhao
--Date   : 2015/3/27
--此文件由[BabeLua]插件自动生成



--endregion

module("TeamLegendData", package.seeall)

require "Game/CombatData"

e_raidDiff = {Normal="Normal",Elite="Elite"};

local m_currRaidDiff = nil;
local m_currCountry = nil;
local m_clickCountry = 1;
local m_clickZone = nil;
local m_currCountryLimit = nil;
local m_levelTempoStars = nil;
local m_levelTempo = nil;
local m_challengeLevelId = 1;
local m_bAgain = nil;
local m_bEvent = nil;
local m_bNoFight = nil;
local m_bPlayLevelFx = nil;

local m_zoneTb = {};
local m_countryLevelTb = {};
local m_countryLevelTbJY = {};
local m_levelDataTb = {};
local rewardItemTb = {};

local callBack = nil;
local callBackOther = nil;
function OnInit()
    DataSystemScript.RegisterMsgHandler(MsgID.tb.RaidInfo, OnReqRaidInfo);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.RaidFastResult, OnReqRaidFastResult);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.RaidStart,OnReqRaidStart);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.RaidResult,OnReqRaidResult);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.RaidLots,OnReqRaidLots);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.RaidEnter,OnReqRaidEnter);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.RaidStarAward,OnReqRaidStarAward);

    DataSystemScript.RegisterMsgHandler(MsgID.tb.RaidInfoJY, OnReqRaidInfoJY);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.RaidStartJY,OnReqRaidStartJY);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.RaidResultJY,OnReqRaidResultJY);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.RaidLotsJY,OnReqRaidLotsJY);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.RaidFastResultJY, OnReqRaidFastResultJY);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.RaidEnterJY, OnReqRaidEnterJY);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.RaidStarAwardJY,OnReqRaidStarAwardJY);

    DoSortZone();
    DoSortLevel();
    DoSortLevelJY();
    DoPreviewReward();
    m_currRaidDiff = e_raidDiff.Normal;
end

function OnRelease()
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.RaidInfo, OnReqRaidInfo);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.RaidFastResult, OnReqRaidFastResult);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.RaidStart,OnReqRaidStart);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.RaidResult,OnReqRaidResult);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.RaidLots,OnReqRaidLots);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.RaidEnter,OnReqRaidEnter);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.RaidStarAward,OnReqRaidStarAward);

    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.RaidInfoJY, OnReqRaidInfoJY);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.RaidStartJY,OnReqRaidStartJY);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.RaidResultJY,OnReqRaidResultJY);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.RaidLotsJY,OnReqRaidLotsJY);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.RaidFastResultJY, OnReqRaidFastResultJY);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.RaidEnterJY, OnReqRaidEnterJY);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.RaidStarAwardJY,OnReqRaidStarAwardJY);
end

--/////////////////////////////////Request
function ReqRaidEnter(callBack_)
    callBack = callBack_;

	local dictPrams = {};
	DataSystemScript.RequestWithParams(LuaConst.Const.ReqRaidEnterUrl, dictPrams, MsgID.tb.RaidEnter);
end

function ReqRaidInfo(mapId_, callBack_)
    callBackOther = callBack_;

	local dictPrams = {};
	dictPrams['id'] = mapId_;
--    dictPrams['type'] = type_;
	DataSystemScript.RequestWithParams(LuaConst.Const.ReqRaidInfoUrl, dictPrams, MsgID.tb.RaidInfo);
end

function ReqRaidStart(mapId_, callBack_)
    callBack = callBack_;

	local dictPrams = {};
	dictPrams['id'] = mapId_;
	DataSystemScript.RequestWithParams(LuaConst.Const.ReqRaidStartUrl, dictPrams, MsgID.tb.RaidStart);
end

function ReqRaidResult(mapId_,score_,actList_,sign_, callBack_)
    callBackOther = callBack_;

	local dictPrams = {};
	dictPrams['id'] = mapId_;
    dictPrams['score'] = score_;
    dictPrams['act'] = actList_;
    dictPrams['sign'] = sign_;
	DataSystemScript.RequestWithParams(LuaConst.Const.ReqRaidResultUrl, dictPrams, MsgID.tb.RaidResult);
end

function ReqRaidLots(mapId_, callBack_)
    callBack = callBack_;

	local dictPrams = {};
	dictPrams['id'] = mapId_;
	DataSystemScript.RequestWithParams(LuaConst.Const.ReqRaidLotsUrl, dictPrams, MsgID.tb.RaidLots);
end

function ReqRaidFastResult(levelId_,times_,callBack_)
    callBack = callBack_;

	local dictPrams = {};
	dictPrams['id'] = levelId_;
    dictPrams['lots'] = times_;
	DataSystemScript.RequestWithParams(LuaConst.Const.ReqRaidFastResultUrl, dictPrams, MsgID.tb.RaidFastResult);
end

function ReqRaidStarAward(mapId_,num_,callBack_)
    callBack = callBack_;

	local dictPrams = {};
	dictPrams['id'] = mapId_;
    dictPrams['num'] = num_;
	DataSystemScript.RequestWithParams(LuaConst.Const.ReqRaidStarAwardUrl, dictPrams, MsgID.tb.RaidStarAward);
end

function ReqRaidEnterJY(callBack_)
    callBack = callBack_;

	local dictPrams = {};
	DataSystemScript.RequestWithParams(LuaConst.Const.ReqRaidEnterUrlJY, dictPrams, MsgID.tb.RaidEnterJY);
end

function ReqRaidInfoJY(mapId_,callBack_)
    callBackOther = callBack_;

	local dictPrams = {};
	dictPrams['id'] = mapId_;
--    dictPrams['type'] = type_;
	DataSystemScript.RequestWithParams(LuaConst.Const.ReqRaidInfoUrlJY, dictPrams, MsgID.tb.RaidInfoJY);
end

function ReqRaidStartJY(mapId_, callBack_)
    callBack = callBack_;

	local dictPrams = {};
	dictPrams['id'] = mapId_;
	DataSystemScript.RequestWithParams(LuaConst.Const.ReqRaidStartUrlJY, dictPrams, MsgID.tb.RaidStartJY);
end

function ReqRaidResultJY(mapId_,score_,actList_,sign_, callBack_)
    callBackOther = callBack_;

	local dictPrams = {};
	dictPrams['id'] = mapId_;
    dictPrams['score'] = score_;
    dictPrams['act'] = actList_;
    dictPrams['sign'] = sign_;
	DataSystemScript.RequestWithParams(LuaConst.Const.ReqRaidResultUrlJY, dictPrams, MsgID.tb.RaidResultJY);
end

function ReqRaidLotsJY(mapId_, callBack_)
    callBack = callBack_;

	local dictPrams = {};
	dictPrams['id'] = mapId_;
	DataSystemScript.RequestWithParams(LuaConst.Const.ReqRaidLotsUrlJY, dictPrams, MsgID.tb.RaidLotsJY);
end

function ReqRaidFastResultJY(levelId_,times_,callBack_)
    callBack = callBack_;

	local dictPrams = {};
	dictPrams['id'] = levelId_;
    dictPrams['lots'] = times_;
	DataSystemScript.RequestWithParams(LuaConst.Const.ReqRaidFastResultUrlJY, dictPrams, MsgID.tb.RaidFastResultJY);
end

function ReqRaidStarAwardJY(mapId_,num_,callBack_)
    callBack = callBack_;

	local dictPrams = {};
	dictPrams['id'] = mapId_;
    dictPrams['num'] = num_;
	DataSystemScript.RequestWithParams(LuaConst.Const.ReqRaidStarAwardUrlJY, dictPrams, MsgID.tb.RaidStarAwardJY);
end

--//////////////////////////////Response
function OnReqRaidEnter(code_, data_)
    if code_ == nil then
        SetCountryData(data_);
        if callBack ~= nil then
            callBack(data_);
            callBack = nil;
        end
    end
end

function OnReqRaidInfo(code_, data_)
    if code_ == nil then
        SetLevelData(data_);
        if callBackOther ~= nil then
            callBackOther(data_);
            callBackOther = nil;
        end
    end
end

function OnReqRaidStart(code_, data_)
    if code_ == nil then
        if callBack ~= nil then
            callBack(data_);
            callBack = nil;
        end
    end
end

function OnReqRaidResult(code_, data_)
    if code_ == nil then
        if callBackOther ~= nil then
            callBackOther(data_);
            callBackOther = nil;
        end
    end
end

function OnReqRaidLots(code_, data_)
    if code_ == nil then
        if callBack ~= nil then
            callBack(data_);
            callBack = nil;
        end
    end
end

function OnReqRaidFastResult(code_, data_)
    if code_ == nil then
        if callBack ~= nil then
            callBack(data_);
            callBack = nil;
        end
    end
end

function OnReqRaidStarAward(code_, data_)
    if code_ == nil then
        if callBack ~= nil then
            callBack(data_);
            callBack = nil;
        end
    end
end

function OnReqRaidEnterJY(code_, data_)
    if code_ == nil then
        SetCountryData(data_);
        if callBack ~= nil then
            callBack(data_);
            callBack = nil;
        end
    end
end

function OnReqRaidInfoJY(code_, data_)
    if code_ == nil then
        SetLevelData(data_);
        if callBackOther ~= nil then
            callBackOther(data_);
            callBackOther = nil;
        end
    end
end

function OnReqRaidStartJY(code_, data_)
    if code_ == nil then
        if callBack ~= nil then
            callBack(data_);
            callBack = nil;
        end
    end
end

function OnReqRaidResultJY(code_, data_)
    if code_ == nil then
        if callBackOther ~= nil then
            callBackOther(data_);
            callBackOther = nil;
        end
    end
end

function OnReqRaidLotsJY(code_, data_)
    if code_ == nil then
        if callBack ~= nil then
            callBack(data_);
            callBack = nil;
        end
    end
end

function OnReqRaidFastResultJY(code_, data_)
    if code_ == nil then
        if callBack ~= nil then
            callBack(data_);
            callBack = nil;
        end
    end

end

function OnReqRaidStarAwardJY(code_, data_)
    if code_ == nil then
        if callBack ~= nil then
            callBack(data_);
            callBack = nil;
        end
    end

end

-- Raid Data
function OpenTeamLegend(args_)
    if args_ == e_raidDiff.Normal then
        local function OnReqInfo(data_)
        WindowMgr.ShowWindow(LuaConst.Const.UITeamLegend,data_);
        end

        m_currRaidDiff = e_raidDiff.Normal;
        TeamLegendData.ReqRaidEnter(OnReqInfo);
    elseif args_ == e_raidDiff.Elite then
        local function OnReqInfoJY(data_)
            WindowMgr.ShowWindow(LuaConst.Const.UITeamLegend,data_);
        end

        m_currRaidDiff = e_raidDiff.Elite;
        TeamLegendData.ReqRaidEnterJY(OnReqInfoJY);
    end

end

-- Unpdate CountryData
function SetCountryData(data_)
    if data_ ~= nil then
        if m_currRaidDiff == e_raidDiff.Normal then
            SetCurrCountry(data_['class']+1)
        elseif m_currRaidDiff == e_raidDiff.Elite then
            SetCurrCountry(data_['class']+1)
            m_currCountryLimit = data_['limit']; -- 0 Next Map Lock // 1 Next Map Unlock
        end
    end
end

-- Update LevelData 
function SetLevelData(data_)
    m_levelTempoStars = 0;
    m_levelTempo = 0;
    m_levelDataTb = {};
    m_levelDataTb = data_;
    if m_levelDataTb.raid ~= nil then
        for k,v in pairs(m_levelDataTb.raid) do
            if not(tonumber(v.star) == 0 and GetIsEvent(k) == 0) then
                m_levelTempoStars = m_levelTempoStars + v.star;
                m_levelTempo = m_levelTempo + 1;
            end
        end
    end

    -- Finish Level
    if GetClickCountry() == GetCurrCountry() and  m_levelTempo == GetRaidLevelLength(GetClickCountry()) then
        SetCurrCountry(GetCurrCountry() + 1);      
    end

end

function GetRaidDiff()
    return m_currRaidDiff;
end

function GetLevelTempoStars()
    return tonumber(m_levelTempoStars);
end

function GetLevelTempo()  -- No LevelId  Is Level Index
    return tonumber(m_levelTempo);
end

function GetClickZone()
    if m_clickZone == nil or m_clickZone == 0 then
        m_clickZone = 1;
    end

    return tonumber(m_clickZone);
end
function GetCurrZone()
    local zone = Config.GetProperty(Config.RaidClassTable(),tostring(GetCurrCountry()),'zone');
    
    return tonumber(zone);
end

function GetClickCountry()
    return tonumber(m_clickCountry);
end

function GetCurrCountry()   -- last country id
    return tonumber(m_currCountry);
end

function GetCurrCountryLimit()
    return tonumber(m_currCountryLimit);
end
function GetChallengeLevelId()
    return tonumber(m_challengeLevelId);
end
function SetCurrCountry(args_)
    m_currCountry = args_;
end
function SetClickCountry(args_)
    m_clickCountry = args_;
end
function SetClickZone(args_)
    m_clickZone = args_;
end

function SetChallengeLevelId(args_)
    m_challengeLevelId = args_;
end
function SetRaidDiff(args_)
    m_currRaidDiff = args_;
end

function IsUnlockFinishCountry(countryId_)
    local bUnlock = false;
    local bFinish = false;
    if m_currRaidDiff == e_raidDiff.Normal then
        if countryId_ < GetCurrCountry() then
            bUnlock = true;
            bFinish = true;
        elseif countryId_ == GetCurrCountry() then
            bUnlock = true;
        end
    elseif m_currRaidDiff == e_raidDiff.Elite then
        if countryId_ < GetCurrCountry() then
            bUnlock = true;
            bFinish = true;
        elseif countryId_ == GetCurrCountry() then
            if GetCurrCountryLimit() == 1 then -- Next Map Unlock
                bUnlock = true;
            end
        end
    end
    
    return bUnlock,bFinish;
end

function IsFinishCountry()
    local bUnlock = IsUnlockFinishCountry(GetClickCountry());
    local levelId = nil;
    local openLv = nil;
    if m_currRaidDiff == e_raidDiff.Normal then
        levelId = GetLevelId(GetClickCountry(),1);
        openLv = tonumber(Config.GetProperty(Config.RaidLevelTable(),tostring(levelId),'lv'));
    elseif m_currRaidDiff == e_raidDiff.Elite then
        levelId = GetLevelId(GetClickCountry(),1);
        openLv = tonumber(Config.GetProperty(Config.RaidJYLevelTable(),tostring(levelId),'lv'));
    end

    if openLv ~= nil and Role.Get_lv() < openLv then
       local str = string.format(Util.LocalizeString("UITeamLegend_RaidOpenLv"),openLv);
       WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, {str});
       return false; 
    end


    if m_currRaidDiff == e_raidDiff.Elite then
        if not bUnlock then
            WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, {Util.LocalizeString("UITeamLegend_RaidLimitElite")});
            return false; 
        end
    end

    if GetClickCountry() > GetCurrCountry() then        
        local strTips = Config.GetProperty(Config.RaidClassTable(),tostring(GetClickCountry()),'country');
        if m_currRaidDiff == e_raidDiff.Elite then
            strTips = Config.GetProperty(Config.RaidClassTable(),tostring(GetClickCountry()),'ecountry');
        end

       local str = string.format(Util.LocalizeString("UITeamLegend_RaidLockTips"),strTips);
       WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, {str});
       return false; 
    end

    return true;
end

function IsFinishStars(levelId_)
    local bFinish = false;
    local star = -1;
    if m_levelDataTb.raid ~= nil and m_levelDataTb.raid[tostring(levelId_)] ~= nil then
        bFinish = true;
        star = m_levelDataTb.raid[tostring(levelId_)].star;
    else
        if levelId_ - GetLevelId(GetClickCountry(),1) == m_levelTempo then
            bFinish = true;
        end
    end

    return bFinish,star;
end

function IsGetReward(index_)
    local bGet = false;
    if m_levelDataTb.award ~= nil then
        for k,v in pairs(m_levelDataTb.award) do
            if tonumber(v) == tonumber(index_) then
                bGet = true;
            end
        end
    end

    return bGet;
end

function GetStarNum(args_)
    local num = 0;
    if m_levelDataTb.raid ~= nil and m_levelDataTb.raid[tostring(args_)] ~= nil then
       num = m_levelDataTb.raid[tostring(args_)].star;
    end

    return num;
end

function GetSweepNum()
    local numItem = ItemSys.GetItemData(LuaConst.Const.Sweep).num
    local cost = 0;
    if GetRaidDiff() == e_raidDiff.Normal then
        cost = Config.GetProperty(Config.RaidLevelTable(),tostring(GetCurrLevelId()),'Power');
    elseif GetRaidDiff() == e_raidDiff.Elite then
        cost = Config.GetProperty(Config.RaidJYLevelTable(),tostring(GetCurrLevelId()),'Power');
    end
 
    local numCost = math.floor(Role.Get_power()/cost);
    local num = math.min(numCost,numItem);
    if num > 10 then
        num = 10;
    end
    if GetRaidDiff() == e_raidDiff.Elite then
        if m_levelDataTb.raid ~= nil and m_levelDataTb.raid[tostring(GetCurrLevelId())] ~= nil then
            local times = m_levelDataTb.raid[tostring(GetCurrLevelId())].num;
            num = math.min(num,(3-times));
            if num < 0 then
                num = 0;
            end
        end
    end

    return num;
end
function IsFastBattle()
    local levelId = GetCurrLevelId();
    local numItem = ItemSys.GetItemData(LuaConst.Const.Sweep).num
    local cost = 0;
    if GetRaidDiff() == e_raidDiff.Normal then
        cost = Config.GetProperty(Config.RaidLevelTable(),tostring(GetCurrLevelId()),'Power');
    elseif GetRaidDiff() == e_raidDiff.Elite then
        cost = Config.GetProperty(Config.RaidJYLevelTable(),tostring(GetCurrLevelId()),'Power');
    end
    local numCost = math.floor(Role.Get_power()/cost);

    if numItem < 1 then
         WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("UITeamLegend_RaidNoSweep")  });
         return false;
    end
    if numCost < 1 then
         WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("UITeamLegend_RaidNoPower")  });
         return false;
    end
    if not IsHaveChallengeTims() then
         WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("UITeamLegend_RaidNoTimes")  });
         return false;
    end
    if m_levelDataTb.raid ~= nil and m_levelDataTb.raid[tostring(levelId)] ~= nil and m_levelDataTb.raid[tostring(levelId)].star >= 3 then
        return true;
    else
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("UITeamLegend_Need3Stars")  });
        return false;
    end

    return true;
end
function IsHaveChallengeTims()
    if GetRaidDiff() == e_raidDiff.Elite then
        local levelId = GetCurrLevelId();
        if m_levelDataTb.raid ~= nil and m_levelDataTb.raid[tostring(levelId)] ~= nil then
            local times = m_levelDataTb.raid[tostring(levelId)].num;
            if times >= 3 then
                return false;
            end
        end
    end

    return true;
end

function IsContainAward(index_)
    local bContain = false;
    if m_levelDataTb.award ~= nil then
        for k,v in pairs(m_levelDataTb.award) do
            if tonumber(v) == tonumber(index_) then
                bContain = true;
            end
        end
    end

    return bContain;
end
-- Raid Config
function SetBNoFight(b_)
    m_bNoFight = b_;
end
function SetBAgain(b_)
    m_bAgain = b_;
    SetPlayLevelFx(not b_)
end
function SetBEvent(b_)
    m_bEvent = b_;
end
function SetPlayLevelFx(b_)
    if GetCurrLevelId() > GetLevelIdTop() then
        m_bPlayLevelFx = b_;
    else
        m_bPlayLevelFx = false;
    end
end
function SetClubIcon(spr_icon_,levelId_)
    local clubIconName = Config.GetProperty(Config.RaidLevelTable(),tostring(levelId_),'icon');
    if GetRaidDiff() == e_raidDiff.Elite then
        clubIconName = Config.GetProperty(Config.RaidJYLevelTable(),tostring(levelId_),'icon');
    end

    Util.SetUITexture(spr_icon_, LuaConst.Const.ClubIcon, clubIconName, false);
end

function SetCountryIcon(spr_icon_,countryId_)
    local countryIconName = Config.GetProperty(Config.RaidClassTable(),tostring(countryId_),'icon');

    Util.SetUITexture(spr_icon_, LuaConst.Const.FlagIcon, countryIconName, true);
end
function GetBNoFight()
    return m_bNoFight;
end
function GetBAgain()
    return m_bAgain;
end
function GetBEvent()
    return m_bEvent;
end
function GetBPlayLevelFx()
   return m_bPlayLevelFx;
end
function GetRaidClassLength()
    local lenght = 0;
    local raidClassTb = Config.GetTemplate(Config.RaidClassTable());
    for k,v in pairs(raidClassTb) do
        lenght = lenght +1;
    end

    return lenght;
end

function GetIconPos(levelId_)
    local pos = {};
    if GetRaidDiff() == e_raidDiff.Elite then
        pos = Config.GetProperty(Config.RaidJYLevelTable(),tostring(levelId_),'pos');
    else
        pos = Config.GetProperty(Config.RaidLevelTable(),tostring(levelId_),'pos');
    end

    return pos;
end

function GetIsBoss(levelId_)
    local isBoss = false;
    if GetRaidDiff() == e_raidDiff.Elite then
        isBoss = Config.GetProperty(Config.RaidJYLevelTable(),tostring(levelId_),'boss');
    else
        isBoss = Config.GetProperty(Config.RaidLevelTable(),tostring(levelId_),'boss');
    end
    
    return isBoss;
end

function GetRaidLevelLength(countryId_)
    local lenght = 0;
    if GetRaidDiff() == e_raidDiff.Elite then
        local raidLevelTb = Config.GetTemplate(Config.RaidJYLevelTable());
        for k,v in pairs(raidLevelTb) do
            if v.class == tostring(countryId_) then
                lenght = lenght + 1;
            end
        end
    else
        local raidLevelTb = Config.GetTemplate(Config.RaidLevelTable());
        for k,v in pairs(raidLevelTb) do
            if v.class == tostring(countryId_) then
                lenght = lenght + 1;
            end
        end
    end

    return lenght;
end

function GetEnemyNameLv(teamId_)
    local name = Config.GetProperty(Config.RaidTeamTable(),teamId_,'name');
    local lv = Config.GetProperty(Config.RaidTeamTable(),teamId_,'lv');
    local icon = Config.GetProperty(Config.RaidTeamTable(),teamId_,'icon');
    local team = Config.GetProperty(Config.RaidTeamTable(),teamId_,'team');
    local score = Config.GetProperty(Config.RaidTeamTable(),teamId_,'score');
    local formIconTb = {};
    local heroTb = Config.GetProperty(Config.RaidTeamTable(),teamId_,'hero');
    for i=2,#heroTb,2 do
        local kItem = TableManager.RaidNpcTbl:GetItem(heroTb[i])
        if null ~= kItem then
            table.insert(formIconTb,kItem.PlayerIconID);
           -- table.insert(formIconTb,Config.GetProperty(Config.RaidNpcTable(),heroTb[i],'playericon'));
        end
    end

    return name,lv,icon,team,score,formIconTb;
end

function GetCurrEnemyNameLvInfo()
    if GetRaidDiff() == e_raidDiff.Elite then
        return GetEnemyNameLvJY(tostring(GetCurrLevelId()));
    else
        return GetEnemyNameLv(tostring(GetCurrLevelId()));
    end
end

function GetEnemyTeam(teamId_)
    return Config.GetProperty(Config.RaidTeamTable(),teamId_,'team');
end

function GetEnemyTeamLength(teamId_)
    local heroTb = Config.GetProperty(Config.RaidTeamTable(),teamId_,'hero');
    local size = 0;
    for k,v in pairs(heroTb) do
       size = size + 1;
    end

    return math.floor(size/6);
end

function GetEnemyNameLvJY(teamId_)
    local name = Config.GetProperty(Config.RaidJYTeamTable(),teamId_,'name');
    local lv = Config.GetProperty(Config.RaidJYTeamTable(),teamId_,'lv');
    local icon = Config.GetProperty(Config.RaidJYTeamTable(),teamId_,'icon');
    local team = Config.GetProperty(Config.RaidJYTeamTable(),teamId_,'team');
    local score = Config.GetProperty(Config.RaidJYTeamTable(),teamId_,'score');
    local formIconTb = {};
    local heroTb = Config.GetProperty(Config.RaidJYTeamTable(),teamId_,'hero');
    for i=2,#heroTb,2 do
        table.insert(formIconTb,Config.GetProperty(Config.RaidJYNpcTable(),heroTb[i],'playericon'));
    end

    return name,lv,icon,team,score,formIconTb;
end

function GetEnemyTeamJY(teamId_)
    return Config.GetProperty(Config.RaidJYTeamTable(),teamId_,'team');
end

function GetEnemyTeamLenghtJY(teamId_)
    local heroTb = Config.GetProperty(Config.RaidJYTeamTable(),teamId_,'hero');
    local size = 0;
    for k,v in pairs(heroTb) do
       size = size + 1;
    end

    return math.floor(size/6);
end
function GetLevelIdTop()
    return GetLevelTempo();
end

function GetCurrLevelId()
    return GetChallengeLevelId();
end

function GetLevelId(countryId_,index_)
    if GetRaidDiff() == e_raidDiff.Elite then
        return m_countryLevelTbJY[countryId_][index_];
    else
        return m_countryLevelTb[countryId_][index_];
    end
end

function GetLevelDropDisplay(levelId_)
    if GetRaidDiff() == e_raidDiff.Elite then
        return Config.GetProperty(Config.RaidJYLevelTable(),tostring(levelId_),'drop_display');
    else
        return Config.GetProperty(Config.RaidLevelTable(),tostring(levelId_),'drop_display');
    end
end 

function GetLevelUExp(levelId_)
    if GetRaidDiff() == e_raidDiff.Elite then
        return Config.GetProperty(Config.RaidJYLevelTable(),tostring(levelId_),'UExp');
    else
        return Config.GetProperty(Config.RaidLevelTable(),tostring(levelId_),'UExp');
    end
end 

function GetLevelHExp(levelId_)
    if GetRaidDiff() == e_raidDiff.Elite then
        return Config.GetProperty(Config.RaidJYLevelTable(),tostring(levelId_),'HExp');
    else
        return Config.GetProperty(Config.RaidLevelTable(),tostring(levelId_),'HExp');
    end
end 

function GetLevelMoney(levelId_)
    if GetRaidDiff() == e_raidDiff.Elite then
        return Config.GetProperty(Config.RaidJYLevelTable(),tostring(levelId_),'money');
    else
        return Config.GetProperty(Config.RaidLevelTable(),tostring(levelId_),'money');
    end
end 

function GetLevelPower(levelId_)
    if GetRaidDiff() == e_raidDiff.Elite then
        return Config.GetProperty(Config.RaidJYLevelTable(),tostring(levelId_),'Power');
    else
        return Config.GetProperty(Config.RaidLevelTable(),tostring(levelId_),'Power');
    end
end

function GetIsEvent(levelId_)
    local event = 0; -- 0(match)  >0(Event)
    if GetRaidDiff() == e_raidDiff.Normal then
        event = tonumber( Config.GetProperty(Config.RaidLevelTable(),tostring(levelId_),'event') )
    end

    return event;
end

function GetLevelName(levelId_)
    if GetRaidDiff() == e_raidDiff.Elite then
        return Config.GetProperty(Config.RaidJYLevelTable(),tostring(levelId_),'name');
    else
        return Config.GetProperty(Config.RaidLevelTable(),tostring(levelId_),'name');
    end

end

function GetLevelProfile(levelId_)
    if GetRaidDiff() == e_raidDiff.Elite then
        return Config.GetProperty(Config.RaidJYLevelTable(),tostring(levelId_),'profile');
    else
        return Config.GetProperty(Config.RaidLevelTable(),tostring(levelId_),'profile');
    end

end

function GetZoneTb()
    return m_zoneTb;
end

function GetCountry2Zone(zoneIndex_)
    return m_zoneTb[zoneIndex_];
end

function DoSortZone()
    local zoneTb = Config.GetTemplate(Config.RaidClassTable());
    for i=1,5 do    -- 5 zone
        local tb = {};
        for k,v in pairs(zoneTb) do
            if v.zone == i then
                table.insert(tb,v.id); 
            end
        end
        table.sort(tb,function(x,y) return x < y end)
        m_zoneTb[i] = tb; -- zone id -> country id
    end
end

function DoSortLevel()
    local countryLength = GetRaidClassLength();
    local raidLevelTb = Config.GetTemplate(Config.RaidLevelTable());
    
    for i=1,countryLength do
        local levelTb = {};
        for k,v in pairs(raidLevelTb) do
            if tonumber(v.class) == i then
                table.insert(levelTb,tonumber(v.id));
            end
        end

        table.sort(levelTb,function(x,y) return x < y end)
        m_countryLevelTb[i] = levelTb;
     end
end

function DoSortLevelJY()
    local countryLength = GetRaidClassLength();
    local raidLevelTb = Config.GetTemplate(Config.RaidJYLevelTable());
    
    for i=1,countryLength do
        local levelTb = {};
        for k,v in pairs(raidLevelTb) do
            if tonumber(v.class) == i then
                table.insert(levelTb,tonumber(v.id));
            end
        end

        table.sort(levelTb,function(x,y) return x < y end)
        m_countryLevelTbJY[i] = levelTb;

     end
end
function DoPreviewReward()
    local raidStarAwardTb = Config.GetTemplate(Config.RaidStarAwrad());

    rewardItemTb = {};
    for k,v in pairs(raidStarAwardTb) do
        local tbClass = {};
        for j=1,#v do
            local tbItem = {};
            for k=1,#v[j].item,2 do
                local tb = {};
                if string.len(v[j].item[k]) ~= 0 then 
                    tb.id = v[j].item[k];
                    tb.num = v[j].item[k+1];
                    table.insert(tbItem,tb);
                end
            end
            table.insert(tbClass,tbItem);
        end
        table.insert(rewardItemTb,tbClass);
    end           
end

function GetPreviewRewardTb()
    return rewardItemTb;
end
-------------------------- Start Fight
function StartFight()
    if GetRaidDiff() == e_raidDiff.Normal then
        ReqRaidStart(GetCurrLevelId(),OnRaidStart);
    elseif GetRaidDiff() == e_raidDiff.Elite then
        ReqRaidStartJY(GetCurrLevelId(),OnRaidStartJY);
    end
end

function OnRaidStart(data_)
    if GetBNoFight() then
        OnRaidResult(data_);
    else
        CombatData.FillData(data_,CombatData.COMBAT_TYPE.TEAMLEGEND,OnReqMatchResult);
    end
end

function OnRaidStartJY(data_)
    if TeamLegendData.GetBNoFight() then
        OnRaidResultJY(data_);
    else
        CombatData.FillData(data_,CombatData.COMBAT_TYPE.TEAMLEGEND,OnReqMatchResult);
    end
end

function OnReqMatchResult()
    local tb = CombatData.GetResultTable();
    if not tb['Giveup'] then
        local score = tostring(tb['HomeScore'] .. ":".. tb['AwayScore']);
        local actionList = tostring(tb['PVEData']);
        local signMD5 = tb['md5'];

        if GetRaidDiff() == e_raidDiff.Normal then
            ReqRaidResult(GetCurrLevelId(),score,actionList,signMD5,OnRaidResult);
        elseif GetRaidDiff() == e_raidDiff.Elite then
            ReqRaidResultJY(GetCurrLevelId(),score,actionList,signMD5,OnRaidResultJY);
        end
    end
end
-- match end
function OnRaidResult(data_)
    if data_ ~= nil then
        data_.BattleResultType = UIBattleResultS.enum_BattleResultType.TeamLegend;

        if tonumber(data_['score'][1]) >tonumber(data_['score'][2]) then
            local OnRaidInfo = function()
                UIChallengeTL.BtnClose();
            end;

            local CloseBattleResult = function()
                 if GetBAgain() then
                    ReqRaidInfo(GetClickCountry(),nil);
                 else
                    ReqRaidInfo(GetClickCountry(),OnRaidInfo);
                 end
            end
            data_.Callback = CloseBattleResult;
            
            WindowMgr.ShowWindow(LuaConst.Const.UIBattleResultS,data_);
        else
            WindowMgr.ShowWindow(LuaConst.Const.UIBattleResultF,data_);
        end
    end
end

function OnRaidResultJY(data_)
    if data_ ~= nil then
        data_.BattleResultType = UIBattleResultS.enum_BattleResultType.TeamLegend;

        if tonumber(data_['score'][1]) >tonumber(data_['score'][2]) then
            local OnRaidInfo = function()
                UIChallengeTL.BtnClose();
            end;

            local CloseBattleResult = function()
                if GetBAgain() then
                    ReqRaidInfoJY(GetClickCountry(),nil);
                else
                    ReqRaidInfoJY(GetClickCountry(),OnRaidInfo);
                end
            end
            data_.Callback = CloseBattleResult;

            WindowMgr.ShowWindow(LuaConst.Const.UIBattleResultS,data_);
        else
            WindowMgr.ShowWindow(LuaConst.Const.UIBattleResultF,data_);
        end
    end
end

-- fast
local m_fastTimes = nil;
function SetFastTimes(times_)
    m_fastTimes = times_;
end

function ReqQuickBattle()
    if GetRaidDiff() == e_raidDiff.Normal then
        ReqRaidFastResult(GetCurrLevelId(),1,OnReqFaseResult);
        UISweepResult.SetTitleName(Config.GetProperty(Config.RaidLevelTable(),tostring(GetCurrLevelId()),'name'));
    elseif GetRaidDiff() == e_raidDiff.Elite then
        ReqRaidFastResultJY(GetCurrLevelId(),1,OnReqFaseResult);
        UISweepResult.SetTitleName(Config.GetProperty(Config.RaidJYLevelTable(),tostring(GetCurrLevelId()),'name'));
    end

end

function OnReqFaseResult(data_)
    if data_ ~= nil then
        m_fastTimes = m_fastTimes -1;
        
        if m_fastTimes > 0 then
            UISweepResult.TryOpen(data_,UISweepResult.enumSweepType.TeamLegend,OnCloseSweepUI);
            LuaTimer.AddTimer(false,1,ReqQuickBattle);
        else
            UISweepResult.TryOpen(data_,UISweepResult.enumSweepType.None,OnCloseSweepUI);
        end
    end
end

-- Reset TeamLegend Data
function ResetData()
    local m_clickCountry = 1;
    local m_clickZone = 1;
    local m_challengeLevelId = 1;

end
