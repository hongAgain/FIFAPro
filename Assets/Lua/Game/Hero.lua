module("Hero", package.seeall)

require "Common/CommonScript"
require "Game/HeroData"
require "Game/AutoReqMsg"

local mHero = {};

local m_mainTeamId = nil;
local m_mainTeamHeroId = nil;

local m_heroDataSub = {};
local m_heroData = {};
local m_heroDataBackup = nil;
local m_heroAmonut = 0;
local effects = nil;

local m_heroMaxStar = 5;
local m_heroMaxQuality = 10;


local scoutConfig = nil;

local callBack = nil;
local heroDefaultTable = {};
CommonScript.SetDefault(heroDefaultTable, "training", {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0});

function OnInit()
	DataSystemScript.RegisterMsgHandler(MsgID.tb.ReqHeroBreakMsg, OnReqHeroBreak);
	DataSystemScript.RegisterMsgHandler(MsgID.tb.ReqHeroAdvMsg, OnHeroAdv);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.ReqHeroProMsg, OnHeroPro);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.ReqHeroDisMsg, OnHeroDis);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.ReqHeroPinfoMsg,OnHeroPInfo);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.ReqHeroPotMsg,OnHeroPot);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.ReqHeroPotsMsg,OnHeroPots);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.ReqHeroCpotMsg,OnHeroCPot);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.ReqHeroRpotMsg,OnHeroRPot);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.ReqHeroTeMsg,OnHeroTe);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.SaveTeam, OnHandleSaveTeam);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.ScoutConfig, OnGetScoutConfig);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.ScoutOne, OnScoutOne);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.ScoutTen, OnScoutTen);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.ReqHeroSkillLvUpMsg,OnHeroSkillLvUp);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.ReqHeroSRefreshMsg,OnHeroSRefresh);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.ReqHeroCSRefreshMsg,OnHeroCSRefresh);
end


function OnRelease()
	DataSystemScript.UnRegisterMsgHandler(MsgID.tb.ReqHeroBreakMsg, OnReqHeroBreak);
	DataSystemScript.UnRegisterMsgHandler(MsgID.tb.ReqHeroAdvMsg, OnHeroAdv);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.ReqHeroProMsg, OnHeroPro);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.ReqHeroDisMsg, OnHeroDis);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.ReqHeroPinfoMsg,OnHeroPInfo);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.ReqHeroPotMsg,OnHeroPot);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.ReqHeroPotsMsg,OnHeroPots);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.ReqHeroCpotMsg,OnHeroCPot);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.ReqHeroRpotMsg,OnHeroRPot);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.ReqHeroTeMsg,OnHeroTe);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.SaveTeam, OnHandleSaveTeam);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.ScoutConfig, OnGetScoutConfig);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.ScoutOne, OnScoutOne);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.ScoutTen, OnScoutTen);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.ReqHeroSkillLvUpMsg,OnHeroSkillLvUp);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.ReqHeroSRefreshMsg,OnHeroSRefresh);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.ReqHeroCSRefreshMsg,OnHeroCSRefresh);
end


function SetTeamData(team)
    m_mainTeamId = team['id'].."";
    m_mainTeamHeroId = team['hero'];
    effects = team['group'] or {};
end

function SetHeroData(id_,value_)
    if value_ ~= nil then
        HeroData.CalcAttr(value_);
        if m_heroData[id_] ~= nil then
            m_heroData[id_].ClearAttr();
        end
    end

    m_heroData[id_] = value_
    ReHeroDataSub();
end

-- Message
function OnReqHeroData(data_)
    if data_ ~= nil then
        for k,v in pairs(data_) do
            setmetatable(v, heroDefaultTable);
            HeroData.CalcAttr(v);

            table.insert(m_heroDataSub,v);
            m_heroData[k] = v;
            m_heroAmonut = m_heroAmonut + 1;
        end
    end

end

function ReqHeroBreak(id,callBack_)
    callBack = callBack_;

	local dictPrams = {};
	dictPrams['id'] = id;
	DataSystemScript.RequestWithParams(LuaConst.Const.ReqHeroBreakUrl, dictPrams, MsgID.tb.ReqHeroBreakMsg);
end

function OnReqHeroBreak(code, data_)
    if code == nil then
        m_heroData[PlayerInfoData.GetCurrPlayerId()].ClearAttr();
       if callBack ~= nil then
            callBack(data_);
            callBack = nil;
       end
    end

end

function ReqHeroAdv(id,callBack_)
    callBack = callBack_;

	local dictPrams = {};
	dictPrams['id'] = id;
	DataSystemScript.RequestWithParams(LuaConst.Const.ReqHeroAdvUrl, dictPrams, MsgID.tb.ReqHeroAdvMsg);
end

function OnHeroAdv(code, data_)
    if code == nil then
        m_heroData[PlayerInfoData.GetCurrPlayerId()].ClearAttr();
       if callBack ~= nil then
            callBack(data_);
            callBack = nil;
       end
    end
end

function ReqHeroPro(playerId_,callBack_)
    callBack = callBack_;

	local dictPrams = {};
	dictPrams['id'] = playerId_;
	DataSystemScript.RequestWithParams(LuaConst.Const.ReqHeroProUrl, dictPrams, MsgID.tb.ReqHeroProMsg);
end

function OnHeroPro(code, data_)
    if code == nil then
       if callBack ~= nil then
            callBack(data_);
            callBack = nil;
       end

        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("UIPlayerList_MakeChipSuccess") });
    end
end

function ReqHeroDis(strPlayerId_,callBack_)
    callBack = callBack_;

	local dictPrams = {};
	dictPrams['ids'] = strPlayerId_;
	DataSystemScript.RequestWithParams(LuaConst.Const.ReqHeroDisUrl, dictPrams, MsgID.tb.ReqHeroDisMsg);
end

function OnHeroDis(code, data_)
    if code == nil then
       if callBack ~= nil then
            callBack(data_);
            callBack = nil;
       end

        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("UIPlayerList_DisPlayer") });
    end
end

function ReqHeroPinfo(strPlayerId_,callBack_)
    callBack = callBack_;

	local dictPrams = {};
	dictPrams['id'] = strPlayerId_;
	DataSystemScript.RequestWithParams(LuaConst.Const.ReqHeroPinfoUrl, dictPrams, MsgID.tb.ReqHeroPinfoMsg);
end

function OnHeroPInfo(code, data_)
    if code == nil then
       if callBack ~= nil then
            callBack(data_);
            callBack = nil;
       end
    end
end

function ReqHeroPot(strPlayerId_,pay_,callBack_)
    callBack = callBack_;

	local dictPrams = {};
	dictPrams['id'] = strPlayerId_;
    dictPrams['pay'] = pay_;
	DataSystemScript.RequestWithParams(LuaConst.Const.ReqHeroPotUrl, dictPrams, MsgID.tb.ReqHeroPotMsg);
end

function OnHeroPot(code, data_)
    if code == nil then
       if callBack ~= nil then
            callBack(data_);
            callBack = nil;
       end
    end
end
function ReqHeroPots(strPlayerId_,pay_,num_,opt_,callBack_)
    callBack = callBack_;

	local dictPrams = {};
	dictPrams['id'] = strPlayerId_;
    dictPrams['pay'] = pay_;
    dictPrams['num'] = num_;
    dictPrams['opt'] = opt_;
	DataSystemScript.RequestWithParams(LuaConst.Const.ReqHeroPotsUrl, dictPrams, MsgID.tb.ReqHeroPotsMsg);
end
function OnHeroPots(code, data_)
    if code == nil then
       if callBack ~= nil then
            callBack(data_);
            callBack = nil;
       end
    end
end

function ReqHeroCpot(strPlayerId_,callBack_)
    callBack = callBack_;

	local dictPrams = {};
	dictPrams['id'] = strPlayerId_;
	DataSystemScript.RequestWithParams(LuaConst.Const.ReqHeroCpotUrl, dictPrams, MsgID.tb.ReqHeroCpotMsg);
end

function OnHeroCPot(code, data_)
    if code == nil then
        if callBack ~= nil then
            callBack(data_);
            callBack = nil;
        end
--        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("SaveSuccess")  });
    end
end

function ReqHeroRpot(strPlayerId_,pay_,callBack_)
    callBack = callBack_;

	local dictPrams = {};
	dictPrams['id'] = strPlayerId_;
    dictPrams['pay'] = pay_;
	DataSystemScript.RequestWithParams(LuaConst.Const.ReqHeroRpotUrl, dictPrams, MsgID.tb.ReqHeroRpotMsg);
end

function OnHeroRPot(code, data_)
    if code == nil then
       if callBack ~= nil then
            callBack(data_);
            callBack = nil;
       end
       WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("ResetSuccess") });
    end
end

function ReqHeroTe(id_,callBack_)
    callBack = callBack_;

	local dictPrams = {};
	dictPrams['id'] = id_;
	DataSystemScript.RequestWithParams(LuaConst.Const.ReqHeroTeUrl, dictPrams, MsgID.tb.ReqHeroTeMsg);
end

function ReqHeroSkillLvUp(id_,index_,lv_,callBack_)
    callBack = callBack_;

	local dictPrams = {};
	dictPrams['id'] = id_; -- playerId
    dictPrams['idx'] = index_;
    dictPrams['lv'] = lv_;
	DataSystemScript.RequestWithParams(LuaConst.Const.ReqHeroSkillLvUpUrl, dictPrams, MsgID.tb.ReqHeroSkillLvUpMsg);
end

function OnHeroSkillLvUp(code,data_)
    if code == nil then
       if callBack ~= nil then
            callBack(data_);
            callBack = nil;
       end
    end
end

function ReqHeroSRefresh(id_,num_,lock_,callBack_)
    callBack = callBack_;

	local dictPrams = {};
	dictPrams['id'] = id_; -- player id
    dictPrams['num'] = num_;
    dictPrams['lock'] = lock_;
	DataSystemScript.RequestWithParams(LuaConst.Const.ReqHeroSRefreshUrl, dictPrams, MsgID.tb.ReqHeroSRefreshMsg);
end

function OnHeroSRefresh(code,data_)
    if code == nil then
       if callBack ~= nil then
            callBack(data_);
            callBack = nil;
       end
    end
end

function ReqHeroCSRefresh(id_,callBack_)
    callBack = callBack_;

	local dictPrams = {};
	dictPrams['id'] = id_;
	DataSystemScript.RequestWithParams(LuaConst.Const.ReqHeroCSRefreshUrl, dictPrams, MsgID.tb.ReqHeroCSRefreshMsg);
end

function OnHeroCSRefresh(id_,callBack_)
    if code == nil then
       if callBack ~= nil then
            callBack(data_);
            callBack = nil;
       end
    end
end

function OnHeroTe(code, data_)
    if code == nil then
       if callBack ~= nil then
            callBack(data_);
            callBack = nil;
       end
    end
end

-- Logic

function SetHeroDataBackup()
    m_heroDataBackup = {};
    m_heroDataBackup = CommonScript.DeepCopy(m_heroData);
end

function GetHeroDataBackup()
    return m_heroDataBackup or m_heroData;
end

function GetHeroData()
   return m_heroData;
end

function SetFormId(value)
    m_mainTeamId = value;
end

function GetFormId()
    return m_mainTeamId;
end

function OnHandleSaveTeam()
    print('Handle Save Team Do Nothing!');
    --WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("success") });
end

local function SortByLevel(a, b)
    if (a.lv > b.lv) then
        return 1;
    elseif (a.lv == b.lv) then
        return 0;
    else
        return -1;
    end
end

local function SortByStar(a, b)
    return 0;
end

function GetHeroMaxStars()
    return m_heroMaxStar;
end

function GetHeroMaxQuality()
    return m_heroMaxQuality;
end

function ColorHeroName(quality_,name_)
   local qColor = {
        ["0"]="[ffffff]"..name_.."[-]",
        ["1"]="[4dd92e]"..name_.."[-]",
        ["2"]="[4dd92e]"..name_.."+1[-]",
        ["3"]="[1dc7ff]"..name_.."[-]",
        ["4"]="[1dc7ff]"..name_.."+1[-]",
        ["5"]="[1dc7ff]"..name_.."+2[-]",
        ["6"]="[c470ff]"..name_.."[-]",
        ["7"]="[c470ff]"..name_.."+1[-]",
        ["8"]="[c470ff]"..name_.."+2[-]",
        ["9"]="[c470ff]"..name_.."+3[-]",
        ["10"]="[ff9445]"..name_.."[-]",
        ["11"]="[ff9445]"..name_.."+1[-]",
        ["12"]="[ff9445]"..name_.."+2[-]",
        ["13"]="[ff9445]"..name_.."+3[-]",
        ["14"]="[ff9445]"..name_.."+4[-]",
        ["15"]="[ff3636]"..name_.."[-]"
    }

    if tonumber(quality_) <= m_heroMaxQuality then
        return qColor[tostring(quality_)];
    else
        print("ERROR...Quality...qColor");
        return "";
    end

end

function GetHeroQualityBgColor(quality_)
    local quality = 
    {
        ["0"]=Color.New(191/255,191/255,191/255,1),
        ["1"]=Color.New(0/255,1,122/255,1),
        ["2"]=Color.New(0/255,1,122/255,1),
        ["3"]=Color.New(0/255,196/255,1,1),
        ["4"]=Color.New(0/255,196/255,1,1),
        ["5"]=Color.New(0/255,196/255,1,1),
        ["6"]=Color.New(160/255,72/255,201/255,1),
        ["7"]=Color.New(160/255,72/255,201/255,1),
        ["8"]=Color.New(160/255,72/255,201/255,1),
        ["9"]=Color.New(160/255,72/255,201/255,1),
        ["10"]=Color.New(1,159/255,0,1),
        ["11"]=Color.New(1,159/255,0,1),
        ["12"]=Color.New(1,159/255,0,1),
        ["13"]=Color.New(1,159/255,0,1),
        ["14"]=Color.New(1,159/255,0,1),
        ["15"]=Color.New(226/255,43/255,54/255,1)
    }

    if tonumber(quality_) <= m_heroMaxQuality then
        return quality[tostring(quality_)];
    else
        print("ERROR...Quality..."..quality_);
        return quality["15"];
    end
end
function GetHeroQualityBannerBg(quality_)
-- Color.New(1,1,1,120/255)
    local quality = 
    {
        ["0"]=Color.New(108/255,108/255,108/255,1),
        ["1"]=Color.New(62/255,102/255,63/255,1),
        ["2"]=Color.New(62/255,102/255,63/255,1),
        ["3"]=Color.New(58/255,78/255,97/255,1),
        ["4"]=Color.New(58/255,78/255,97/255,1),
        ["5"]=Color.New(58/255,78/255,97/255,1),
        ["6"]=Color.New(91/255,77/255,113/255,1),
        ["7"]=Color.New(91/255,77/255,113/255,1),
        ["8"]=Color.New(91/255,77/255,113/255,1),
        ["9"]=Color.New(91/255,77/255,113/255,1),
        ["10"]=Color.New(149/255,110/255,43/255,1),
        ["11"]=Color.New(149/255,110/255,43/255,1),
        ["12"]=Color.New(149/255,110/255,43/255,1),
        ["13"]=Color.New(149/255,110/255,43/255,1),
        ["14"]=Color.New(149/255,110/255,43/255,1),
        ["15"]=Color.New(128/255,66/255,66/255,1)
    }

    if tonumber(quality_) <= m_heroMaxQuality then
        return quality[tostring(quality_)];
    else
        print("ERROR...Quality..."..quality_);
        return quality["15"];
    end
end

function GetHeroAmonut()
    return m_heroAmonut;
end

function GetHeroLv2Index(index_)
    if m_heroDataSub[index_] ~= nil then
        return m_heroDataSub[index_]['lv']
    else
        return nil;
    end
end

function GetHeroId2Index(index_)
    if m_heroDataSub[index_] ~= nil then
        return m_heroDataSub[index_]['id']
    else
        return nil;
    end
end


function GetHeroData2Id(playerId_)
--    print("GetHeroData2Id: "..playerId_)

    return m_heroData[playerId_];
end

function IsContainPlayer(playerId_)
    if m_heroData[playerId_] == nil then
        return false;
    else
        return true;
    end

end

function GetHeroList()
    return m_heroDataSub;
end

function  ReHeroDataSub()
    m_heroDataSub = {};
    m_heroAmonut = 0;
    for k,v in pairs(m_heroData) do
        table.insert(m_heroDataSub,v);
        m_heroAmonut = m_heroAmonut + 1;
    end

end
function GetCurrStars(playerId_)
    local star = m_heroData[playerId_]['slv'] + Config.GetProperty(Config.HeroTable(), playerId_, 'islv');

    return star;
end

function GetOriginStars(playerId_)
    local originStar =  Config.GetProperty(Config.HeroTable(), playerId_, 'islv');

    return originStar;
end

function GetCurrQuality(playerId_)

    return m_heroData[playerId_]['adv'];
end

function GetHeroStatusName(playerId_)
    if (m_heroData == nil or m_heroData[playerId_] == nil) then
        return "Common_Status_3";
    end
    
    if m_heroData[playerId_]['stat'] == 1 then
        return "Common_Status_1";
    elseif m_heroData[playerId_]['stat'] == 2 then
        return "Common_Status_2";
    elseif m_heroData[playerId_]['stat'] == 3 then
        return "Common_Status_3";
    elseif m_heroData[playerId_]['stat'] == 4 then
        return "Common_Status_4";
    elseif m_heroData[playerId_]['stat'] == 5 then
        return "Common_Status_5";
    end

    return "Common_Status_1";
end

function MainTeamHeroId()
    return m_mainTeamHeroId;
end

function MainTeamEffects()
    return effects;
end

function IsMainPlayer(playerId_)
   for k,v in pairs(m_mainTeamHeroId) do
       if tonumber(v) == tonumber(playerId_) then
            return true;
       end
   end

   return false;
end
function Swap(a, b)
    local id1 = m_mainTeamHeroId[a];
    local id2 = m_mainTeamHeroId[b];
    
    m_mainTeamHeroId[a] = id2;
    m_mainTeamHeroId[b] = id1;
    
    --print("swap  "..a.."   "..b);
end

function SetInTeam(id, idx)
    local old = m_mainTeamHeroId[idx];
    m_mainTeamHeroId[idx] = id;
end

function SaveTeam()
    local param = {};
    param.id = m_mainTeamId;
    param.hero = table.concat(m_mainTeamHeroId, ',');
    DataSystemScript.RequestWithParams(LuaConst.Const.SaveTeam, param, MsgID.tb.SaveTeam);
end

local refreshScout = nil;
function RegisterRefreshScoutCB(cb)
    refreshScout = cb;
end

function TryGetScoutConfig()
    if (scoutConfig == nil) then
        local auto = AutoReqMsg.New();
        auto:EnqueueMsg(MsgID.tb.ScoutConfig, GetScoutConfig);
        auto:DoMsgReq();
    else
        if (refreshScout ~= nil) then
            refreshScout(scoutConfig);
        end
    end
end

function GetScoutConfig()
    DataSystemScript.RequestWithParams(LuaConst.Const.ScoutConfig, nil, MsgID.tb.ScoutConfig);
end

function OnGetScoutConfig(code, data)
    scoutConfig = data;
    if (refreshScout ~= nil) then
        refreshScout(scoutConfig);
    end
end

animScoutOne = nil;
animScoutTen = nil;

function OnScoutOne(code, data)
    if (animScoutOne ~= nil) then
        animScoutOne(data['add']);
    end
end

function OnScoutTen(code, data)
    if (animScoutTen ~= nil) then
        animScoutTen(data['add']);
    end
    
    --type:"hero" "item"
end

function AutoLineup()
    
    --local proArr = Config.GetProperty(Config.FormTable(), m_mainTeamId, "pro");
    local proArr = TableManager.FormationTbl:GetItem(m_mainTeamId).ProList
    local candidate = { "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" };
    
    local function GetFitHero(fitCondition, sortByPos)
        
        local fitArr = {};
        
        local function Sort(pos)

            local function SortByPower(a, b)
                if (a.power > b.power) then
                    return 1;
                elseif (a.power == b.power) then
                    return 0;
                else
                    return -1;
                end
            end

            local function SortByPowerAtPos(a, b)
                local aPower = a.PowerAtPos(pos);
                local bPower = b.PowerAtPos(pos);
                if (aPower > bPower) then
                    return 1;
                elseif (aPower == bPower) then
                    return 0;
                else
                    return -1;
                end
            end

            if (pos == "default") then
                return CommonScript.QuickSort(fitArr, SortByPower)
            else
                return CommonScript.QuickSort(fitArr, SortByPowerAtPos);
            end
        end
        
        for _, v in ipairs(m_heroDataSub) do

            local continue = true;
            for _, candidateID in ipairs(candidate) do
                if (v.id == candidateID) then
                    continue = false;
                    break;
                end
            end

            if (continue) then
                local configPos = Config.GetProperty(Config.HeroTable(), v.id, "ipos");
                if (fitCondition(configPos)) then
                    table.insert(fitArr, v);
                end
            end
        end
        
        local sorted = Sort(sortByPos);
        
        return sorted;
    end
    
    local firstRoundFit = {};
    for i = 1, proArr.Count do
        
        function EqualPro(configPos)
            return configPos == tostring(proArr[i-1]);
        end
        
        local sorted = GetFitHero(EqualPro, "default");
        if (#sorted > 0) then
            candidate[i] = sorted[1].id;
            firstRoundFit[i] = true;
        else
            firstRoundFit[i] = false;
        end
    end
    --First ergodic
    
    for i = 1, proArr.Count do
        
        if (candidate[i] == "0") then
            
            function LikePro(configPos)
                local num = tonumber(configPos);
                return (num >= 1 and num <= 9 and proArr[i-1] >= 1 and proArr[i-1] <= 9)
                    or (num >= 10 and num <= 20 and proArr[i-1] >= 10 and proArr[i-1] <= 20)
                    or (num >= 21 and num <= 27 and proArr[i-1] >= 21 and proArr[i-1] <= 27);
            end

            local sorted = GetFitHero(LikePro, tostring(proArr[i-1]));
            if (#sorted > 0) then
                candidate[i] = sorted[1].id;
            end
        end
    end
    --Second ergodic
    
    for i = 1, proArr.Count do
        
        if (candidate[i] == "0" or firstRoundFit[i]) then
            
            function AlwaysEqual(configPos)
                return true;
            end

            local pro = tostring(proArr[i-1]);
            local sorted = GetFitHero(AlwaysEqual, pro);
            if (#sorted > 0) then
                if (candidate[i] == "0") then
                    candidate[i] = sorted[1].id;
                else
                    local pow1 = sorted[1].PowerAtPos(pro);
                    local pow2 = GetHeroData2Id(candidate[i]).PowerAtPos(pro);
                    if (pow1 > pow2 * 1.1) then
                        candidate[i] = sorted[1].id;
                    end
                end
            end
        end
    end
    --Third ergodic
    
    local equal = true;
    for i = 1, #m_mainTeamHeroId do
        if (m_mainTeamHeroId[i] ~= candidate[i]) then
            equal = false;
            break;
        end
    end
    if (equal) then
        print("Equal");
    else
--    for k, v in ipairs(m_mainTeamHeroId) do
--        print("Org: ["..k.."] = "..v);
--    end
--    
        m_mainTeamHeroId = candidate;
--    
--    for k, v in ipairs(m_mainTeamHeroId) do
--        print("Now: ["..k.."] = "..v);
--    end
    end
    
    return equal;
end