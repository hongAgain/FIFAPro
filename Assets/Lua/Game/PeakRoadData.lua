--region NewFile_1.lua
--Author : hongpeng.zhao
--Date   : 2015/4/17
--此文件由[BabeLua]插件自动生成



--endregion


module("PeakRoadData", package.seeall)

require "Common/UnityCommonScript"
require "Common/CommonScript"
require "Game/GameMainScript"
require "Config"
require "Game/CombatData"
require  "Game/Role"


local MaxFloor = nil;
local MaxGroup = nil;
local m_currLevelId = nil;
local m_bRefrshPR = nil;
local m_bRefrshPRSub = nil;

local callBackFun = nil;
local callBackFunOther = nil;
local m_raidDFRankTb = {};

local m_resetTimes = nil;
local m_costTimes = 0;
local m_maxYestoday = 0;
local m_maxFloorIndex = 3;
local m_currFloorIndex = 3;
local m_todayMaxFloor = 0;
function OnInit()
    DataSystemScript.RegisterMsgHandler(MsgID.tb.RaidDFInfo, OnReqRaidDFInfo);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.RaidDFStart, OnReqRaidDFStart);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.RaidDFResult, OnReqRaidDFResult);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.RaidDFFastResult, OnReqRaidDFFastResult);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.RaidDFReset, OnReqRaidDFReset);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.RaidDFSort, OnReqRaidDFRank);

    CalcCountDF();
    SortRaidDF();
end

function OnRelease()
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.RaidDFInfo, OnReqRaidDFInfo);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.RaidDFStart, OnReqRaidDFStart);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.RaidDFResult, OnReqRaidDFResult);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.RaidDFFastResult, OnReqRaidDFFastResult);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.RaidDFReset, OnReqRaidDFReset);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.RaidDFSort, OnReqRaidDFRank);
end

function ReqRaidDFInfo(callBack_)
    callBackFun = callBack_;

	local dictPrams = {};
	DataSystemScript.RequestWithParams(LuaConst.Const.ReqRaidDFInfoUrl, dictPrams, MsgID.tb.RaidDFInfo);
end

function ReqRaidDFStart(id_,callBack_)
    callBackFun = callBack_;

	local dictPrams = {};
    dictPrams['id'] = id_;
	DataSystemScript.RequestWithParams(LuaConst.Const.ReqRaidDFStartUrl, dictPrams, MsgID.tb.RaidDFStart);
end

function ReqRaidDFResult(id_,score_,actList_,sign_,callBack_)
    callBackFunOther = callBack_;
	local dictPrams = {};
    dictPrams['id'] = id_;
    dictPrams['score'] = score_;
    dictPrams['act'] = actList_;
    dictPrams['sign'] = sign_;
	DataSystemScript.RequestWithParams(LuaConst.Const.ReqRaidDFResultUrl, dictPrams, MsgID.tb.RaidDFResult);

end

function ReqRaidDFFastResult(callBack_)
    callBackFun = callBack_;

	local dictPrams = {};
	DataSystemScript.RequestWithParams(LuaConst.Const.ReqRaidDFFastResultUrl, dictPrams, MsgID.tb.RaidDFFastResult);
end

function ReqRaidDFReset(callBack_)
    callBackFun = callBack_;

	local dictPrams = {};
	DataSystemScript.RequestWithParams(LuaConst.Const.ReqRaidDFResetUrl, dictPrams, MsgID.tb.RaidDFReset);
end

function ReqRaidDFRank(page_,size_,callBack_)
    callBackFun = callBack_;

	local dictPrams = {};
    dictPrams['page'] = page_;
    dictPrams['size'] = size_;
	DataSystemScript.RequestWithParams(LuaConst.Const.ReqRaidDFSortUrl, dictPrams, MsgID.tb.RaidDFSort);
end

-- Resopone
function OnReqRaidDFInfo(code_, data_)
    if code_ == nil then
        if callBackFun ~= nil then
            SetPeakRoad(data_);
            callBackFun(data_);
            callBackFun = nil;
        end
    end
end


function OnReqRaidDFStart(code_, data_)
    if code_ == nil then       
        if callBackFun ~= nil then
            callBackFun(data_);
            callBackFun = nil;
        end
    end
end

function OnReqRaidDFResult(code_, data_)
    if code_ == nil then
        if callBackFunOther ~= nil then
            callBackFunOther(data_);
            callBackFunOther = nil;
        end
    end
end

function OnReqRaidDFFastResult(code_, data_)
    if code_ == nil then
        if callBackFun ~= nil then
            SetFastResult(data_);
            callBackFun(data_);
            callBackFun = nil;
        end
    end
end

function OnReqRaidDFReset(code_, data_)
    if code_ == nil then
        if callBackFun ~= nil then
            SetResetPeakRoad(data_);
            callBackFun(data_);
            callBackFun = nil;
        end
    end
end

function OnReqRaidDFRank(code_,data_)
    if code_ == nil then
        m_raidDFRankTb = {};
        for k,v in pairs(data_) do
            table.insert(m_raidDFRankTb,v);
        end
        table.sort(m_raidDFRankTb,function(x,y) return x.sort < y.sort end );
    end

    if callBackFun ~= nil then
        callBackFun(data_);
        callBackFun = nil;
    end
end
-- Send Msg Function
function OnBtnReset(callback_)
    local ResetResult = function()
        if GetCurrFloorIndex()+1 >= MaxFloor then
            UIPeakRoadScript.SetCurrFloor(math.ceil(MaxFloor/5));
        else
            UIPeakRoadScript.SetCurrFloor(math.ceil((GetCurrFloorIndex()+1)/5*3))
        end
        callback_();
    end;

    local OnSureReset = function()
        if ItemSys.GetItemData(LuaConst.Const.GB).num >= tonumber(Config.GetTemplate(Config.BaseTable())["rPRCost"]) then
            ReqRaidDFReset(ResetResult);
        else
            WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("UIPeakRoad_NoEnoughtRMB")  });
        end
    end;

    local vip = 0; -- Reserve Default 
    local maxRestTimes = tonumber(Config.GetProperty(Config.RaidDFVipTable(),tostring(vip),'num'));
    if GetResetTimes() >= maxRestTimes then
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("UIPeakRoad_NoResetTimes")  });
        return;
    end

    WindowMgr.ShowWindow(LuaConst.Const.UIPeakRoadTips,{TipsType = UIPeakRoadTips.enum_PeakRoadTips.TipsReset,Callback = OnSureReset});
end


function ClickChallengeTips()
    if GetCostTimes() >= 3 then
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("UIPeakRoad_NoChallengeTimes")  });
    else
        local OnMatch = function()
            ReqRaidDFStart(GetCurrFloorIndex()+1,OnChallengeStart);
        end;

        UIPrepareMatch.RegisterBtnMatch(OnMatch);
        UIPrepareMatch.TryOpen({PrepareMatchType = UIPrepareMatch.enum_PrepareMatchType.PeakRoad});    
    end
    
end

function OnChallengeStart(data_)
    if data_ ~= nil then
         CombatData.FillData(data_,CombatData.COMBAT_TYPE.PEAKROAD,OnReqRaidDFMatchResult);  
    end
end
--Request  Client Match Score
function OnReqRaidDFMatchResult()
    local tb = CombatData.GetResultTable();
    local m_quit = tb['Giveup'];
    if not m_quit then
        m_meScore = tb['HomeScore'];
        m_enemyScore = tb['AwayScore'];
        local score = tostring(m_meScore .. ":".. m_enemyScore);
        local actionList = tostring(tb['PVEData'])
        local signMD5 = tb['md5'];
    
        ReqRaidDFResult(GetCurrFloorIndex()+1,score,actionList,signMD5,OnChallengeResult);
    end

end

function OnChallengeResult(data_)
    if data_ ~= nil then
        data_.BattleResultType = UIBattleResultS.enum_BattleResultType.PeakRoad;
        data_.Callback = OnCloseResult;
        if tonumber(data_['score'][1]) >tonumber(data_['score'][2]) then
            m_currFloorIndex = m_currFloorIndex + 1;
            if m_currFloorIndex > m_maxFloorIndex then
              m_maxFloorIndex = m_currFloorIndex
            end
            if m_currFloorIndex >= MaxFloor then
                WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("UIPeakRoad_PeakAllFinish")  });
                m_currFloorIndex = MaxFloor;
            end

            if m_currFloorIndex%15 == 0 then 
                BtnRightArrow()
            end

            WindowMgr.ShowWindow(LuaConst.Const.UIBattleResultS,data_);
        else
            WindowMgr.ShowWindow(LuaConst.Const.UIBattleResultF,data_);
            m_costTimes = m_costTimes + 1;
        end
    end

    SetBRefreshPR(true);
    SetBRefreshPRSub(true);
end

function OnCloseResult()
    UIChallengeTL.BtnClose();
    UIPeakRoadSub.RefreshSub();
end

function OnPromotion(callbackPromotion_)
    local OnFastResult = function()
        if GetCurrFloorIndex()+1 >= MaxFloor then
            UIPeakRoadScript.SetCurrFloor( math.ceil(MaxFloor/(5)) );
        else
            UIPeakRoadScript.SetCurrFloor( math.ceil((GetCurrFloorIndex()+1)/(5*3)) );
        end  
        callbackPromotion_();
    end

    local OnSurePro = function()
        local cost = Config.GetTemplate(Config.BaseTable())["pPRCost"];    
        if ItemSys.GetItemData(LuaConst.Const.GB).num >= tonumber(cost) then
            ReqRaidDFFastResult(OnFastResult);
        else
            WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("UIPeakRoad_NoEnoughtRMB")  });
        end
    end

    if IsFast() then
        WindowMgr.ShowWindow(LuaConst.Const.UIPeakRoadTips,{TipsType = UIPeakRoadTips.enum_PeakRoadTips.TipsPro,Callback = OnSurePro});
    else
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("UIPeakRoad_NoFast")  });
    end
end


-- Update PeakRoad Data
function SetPeakRoad(data_)
    m_resetTimes = data_['rn'];
    m_costTimes = data_['tn'];
    m_maxYestoday = data_['ys'];
    m_maxFloorIndex = data_['top'];
    m_currFloorIndex = data_['ts'];
    m_todayMaxFloor = data_['tt'];

end

function SetResetPeakRoad(data_)
    m_resetTimes = data_['rn'];
    m_costTimes = data_['tn'];
    m_currFloorIndex = data_['ts'];

end

function SetFastResult(data_)
    m_currFloorIndex = data_['ts'];
end

function GetResetTimes()
    return m_resetTimes;
end
function GetCostTimes()
    return m_costTimes;
end
function GetMaxYestoday()
    return m_maxYestoday;
end
function GetMaxFloorIndex()
    return m_maxFloorIndex;
end
function GetCurrFloorIndex()
    return m_currFloorIndex;
end
function GetTodayMaxFloor()
    return m_todayMaxFloor;
end
function SetTodayMaxFloor(floor_)
    m_todayMaxFloor = floor_;
end
-- Logic

-- 15(match) = 3(cup) x 5(floor)
function CalcCountDF()
    MaxFloor = 0;
    local tb = Config.GetTemplate(Config.RaidDFTable());
    for k,v in pairs(tb) do
        MaxFloor = MaxFloor + 1;
    end

    MaxGroup = math.floor(MaxFloor/15);
end

function IsFast()
    local step = math.floor(GetMaxFloorIndex()/3);
    if step*3  > GetCurrFloorIndex() then
        return true;
    end

    return false;
end

function GetRaidDFRankTb()
    return m_raidDFRankTb;
end

local m_groupTb = {};
function SortRaidDF()
    local tempGroup = 1;
    local tempGroupTb1 = {};
    local tempGroupTb2 = {};
    local tempGroupTb3 = {};
    local tempGroupTb4 = {};
    local tempGroupTb5 = {};
    local tempGroupTb6 = {};
    local tb = Config.GetTemplate(Config.RaidDFTable());
    for k,v in pairs(tb) do
        if tonumber(v['group']) == 1 then
            table.insert(tempGroupTb1,v);
        elseif tonumber(v['group']) == 2 then
            table.insert(tempGroupTb2,v);
        elseif tonumber(v['group']) == 3 then
            table.insert(tempGroupTb3,v);
        elseif tonumber(v['group']) == 4 then
            table.insert(tempGroupTb4,v);
        elseif tonumber(v['group']) == 5 then
            table.insert(tempGroupTb5,v);
        elseif tonumber(v['group']) == 6 then
            table.insert(tempGroupTb6,v);
        end
    end
    table.insert(m_groupTb,tempGroupTb1);
    table.insert(m_groupTb,tempGroupTb2);
    table.insert(m_groupTb,tempGroupTb3);
    table.insert(m_groupTb,tempGroupTb4);
    table.insert(m_groupTb,tempGroupTb5);
    table.insert(m_groupTb,tempGroupTb6);

end

function GetGroupName(index_)
    local name = m_groupTb[index_][1].groupName;

    return name;
end

local m_currFloorClubTb = {};
local m_currClubTb = {};
function InitCurrFloorClub(floor_)
    m_currFloorClubTb = {};
    local id = (floor_-1)*15; -- 3*5
    local currId = 0;
    for j=1,5 do
        local clubTb = {};
        for i=1,3 do
            local tb = {};
            currId = tostring(id + i + (j-1)*3);
            tb.id = currId;
            tb.class = Config.GetProperty(Config.RaidDFTable(),currId,"class");
            tb.group = Config.GetProperty(Config.RaidDFTable(),currId,"group");
            tb.pos = Config.GetProperty(Config.RaidDFTable(),currId,"pos");
            tb.name = Config.GetProperty(Config.RaidDFTable(),currId,"name");
            tb.unlock_lv = Config.GetProperty(Config.RaidDFTable(),currId,"unlock_lv");
            tb.clubName = Config.GetProperty(Config.RaidDFTeamTable(),currId,"name");
            tb.clubLv = Config.GetProperty(Config.RaidDFTeamTable(),currId,"lv");
            tb.clubIcon = Config.GetProperty(Config.RaidDFTeamTable(),currId,"icon");
            table.insert(clubTb,tb);
        end
        table.insert(m_currFloorClubTb,clubTb);
    end
end

function InitCurrClub(index_)
    local classId = m_currFloorClubTb[index_][1].class;

    local nameTb = Config.GetProperty(Config.RobotTeamTable(),classId,'name');
    local iconTb = Config.GetProperty(Config.RobotTeamTable(),classId,'icon');
    local myPos = tonumber(m_currFloorClubTb[index_][1].pos);
    local robotTb = {};
    local myTb = {};
    for i=1,4 do
       local tb = {};
       tb.clubName = nameTb[i];
       tb.clubIcon = iconTb[i];
       table.insert(robotTb,tb);
    end
    myTb.me = true;
    myTb.clubName = Role.Get_name();
    myTb.clubIcon = Role.GetRoleIcon();
    function CommonClub()
       table.insert(m_currClubTb,robotTb[2]);
       robotTb[3].success = true
       table.insert(m_currClubTb,robotTb[3]);

       m_currFloorClubTb[index_][3].success = true
       table.insert(m_currClubTb,m_currFloorClubTb[index_][3]);
       table.insert(m_currClubTb,robotTb[4]);

       local tb = CommonScript.DeepCopy(robotTb[3]);
       tb.success = false;
       table.insert(m_currClubTb,tb);
       table.insert(m_currClubTb,m_currFloorClubTb[index_][3]);

       table.insert(m_currClubTb,m_currFloorClubTb[index_][3]);
    end
    function Club1()
       myTb.success = true;
       table.insert(m_currClubTb,myTb);
       table.insert(m_currClubTb,m_currFloorClubTb[index_][1]);

       m_currFloorClubTb[index_][2].success = true;
       table.insert(m_currClubTb,robotTb[1]);
       table.insert(m_currClubTb,m_currFloorClubTb[index_][2]);

       table.insert(m_currClubTb,myTb);
       local tb = CommonScript.DeepCopy(m_currFloorClubTb[index_][2]);
       tb.success = false;
       table.insert(m_currClubTb,tb);

       table.insert(m_currClubTb,myTb);
    end
    function Club2()
       myTb.success = true;
       table.insert(m_currClubTb,m_currFloorClubTb[index_][1]);
       table.insert(m_currClubTb,myTb);

       m_currFloorClubTb[index_][2].success = true
       table.insert(m_currClubTb,m_currFloorClubTb[index_][2]);
       table.insert(m_currClubTb,robotTb[1]);

       table.insert(m_currClubTb,myTb);
       local tb = CommonScript.DeepCopy(m_currFloorClubTb[index_][2]);
       tb.success = false;
       table.insert(m_currClubTb,tb);

       table.insert(m_currClubTb,myTb);
    end
    function Club3()
       m_currFloorClubTb[index_][1].success = true;
       table.insert(m_currClubTb,robotTb[1]);
       table.insert(m_currClubTb,m_currFloorClubTb[index_][1]);

       myTb.success = true;
       table.insert(m_currClubTb,myTb);
       table.insert(m_currClubTb,m_currFloorClubTb[index_][2]);

       local tb = CommonScript.DeepCopy(m_currFloorClubTb[index_][1]);
       tb.success = false;
       table.insert(m_currClubTb,tb);
       table.insert(m_currClubTb,myTb);

       table.insert(m_currClubTb,myTb);
    end
    function Club4()
       m_currFloorClubTb[index_][1].success = true;
       table.insert(m_currClubTb,m_currFloorClubTb[index_][1]);
       table.insert(m_currClubTb,robotTb[1]);

       myTb.success = true;
       table.insert(m_currClubTb,m_currFloorClubTb[index_][2]);
       table.insert(m_currClubTb,myTb);

       local tb = CommonScript.DeepCopy(m_currFloorClubTb[index_][1]);
       tb.success = false;
       table.insert(m_currClubTb,tb);
       table.insert(m_currClubTb,myTb);

       table.insert(m_currClubTb,myTb);
    end

    m_currClubTb = {};
    if myPos == 1 then
       Club1();
       CommonClub();
    elseif myPos == 2 then
       Club2();
       CommonClub();
     elseif myPos == 3 then
       Club3();
       CommonClub();
     elseif myPos == 4 then
       Club4();
       CommonClub();
     elseif myPos == 5 then
       CommonClub();
       Club1();
     elseif myPos == 6 then
       CommonClub();
       Club2();
     elseif myPos == 7 then
       CommonClub();
       Club3();
     elseif myPos == 8 then
       CommonClub();
       Club4();
    end
        
end

function GetBRefreshPR()
    return m_bRefrshPR;
end
function GetBRefreshPRSub()
    return m_bRefrshPRSub;
end
function GetCurrFloorClubTb()
    return m_currFloorClubTb;
end

function GetCurrClubTb()
   return m_currClubTb;
end

function SetCurrLevelId(level_)
    
    m_currLevelId = level_;
end

function SetBRefreshPR(bRefresh_)
    m_bRefrshPR = bRefresh_;
end
function SetBRefreshPRSub(bRefresh_)
    m_bRefrshPRSub = bRefresh_;
end

function GetCurrLevelId()
    return m_currLevelId;
end

function GetCurrEnemyNameLvInfo()
    local name = Config.GetProperty(Config.RaidDFTeamTable(),tostring(m_currLevelId),'name');
    local lv = Config.GetProperty(Config.RaidDFTeamTable(),tostring(m_currLevelId),'lv');
    local icon = Config.GetProperty(Config.RaidDFTeamTable(),tostring(m_currLevelId),'icon');
    local team = Config.GetProperty(Config.RaidDFTeamTable(),tostring(m_currLevelId),'team');
    local score = Config.GetProperty(Config.RaidDFTeamTable(),tostring(m_currLevelId),'score');
    if score == nil then
        score = -1;
    end
    local formIconTb = {};
    local heroTb = Config.GetProperty(Config.RaidDFTeamTable(),tostring(m_currLevelId),'hero');
    for i=2,#heroTb,2 do
        table.insert(formIconTb,Config.GetProperty(Config.RaidDFNpcTable(),heroTb[i],'playericon'));
    end

    return name,lv,icon,team,score,formIconTb;
end

function ResetCost()
    local strCost = string.format(Util.LocalizeString("ResetCostTips"),
          tonumber(Config.GetTemplate(Config.BaseTable())["rPRCost"]),tonumber(Config.GetTemplate(Config.BaseTable())["rPRTimes"]));

    return strCost;
end

function ProCost()
    local strCost = string.format(Util.LocalizeString("PorCostTips"),tonumber(Config.GetTemplate(Config.BaseTable())["pPRCost"]));

    return strCost;
end

function GetMaxFloor()
    return MaxFloor;
end

function GetMaxGroup()
    return MaxGroup
end










