module("UITimeRaid", package.seeall)

require "Config"
require "Common/UnityCommonScript"
require "Game/TimeRaidData"
require "UILua/UIGetItemScript"
require "UILua/UITimeRaidDifficultyBox"
require "UILua/UIPrepareMatch"
require "UILua/UIBattleResultS"
require "UILua/UIBattleResultD"
require "UILua/UIBattleResultF"

local uiRaidCardLeft = nil;
local uiRaidCardCenter = nil;
local uiRaidCardRight = nil;

local uiButtonArrowLeft = nil;
local uiButtonArrowRight = nil;

local uiPopupRoot = nil;

local timeRaidNetStartData = nil;
local timeRaidNetResultData = nil;
local timeRaidNetFastResultData = nil;

local timeRaidLocalDataTable = nil;
local label_weekofday = nil;

local TimeRaidSettings = {
	IconAttack = "Messi",
	IconDefence = "Nesta",
	IconOrganize = "Zidane",
	IconWeekDayActive = Color.New(102 / 255, 204 / 255, 1, 1),
	IconWeekDayDisactive = Color.New(63 / 255, 67 / 255, 78 / 255, 1),
	WeekDaySum = 7,
	LocalTimeToday = nil,
    WeekDayKeys = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" },
    TypeTxtKeys = { "UIPeakRoad_Shoot", "UIPeakRoad_Defend", "UIPeakRoad_Tissue" };
};

local window = nil;
local windowComponent = nil;

function OnStart(gameObject, params)
    print("UILadderMatchScript.lua .. OnStart");
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");

    --fetch data then set UI
    BindUI();
    TimeRaidSettings.LocalTimeToday = tonumber(os.date("%w"));
	ShowWeekDayContent(TimeRaidSettings.LocalTimeToday);
end

function BindUI()
	local transform = window.transform;

    label_weekofday = TransformFindChild(transform, "Label - WeekOfDay");

	uiRaidCardLeft  = TransformFindChild(transform, "RaidAnchorLeft");
	uiRaidCardCenter = TransformFindChild(transform, "RaidAnchorCenter");
	uiRaidCardRight = TransformFindChild(transform, "RaidAnchorRight");
	Util.AddClick(TransformFindChild(uiRaidCardLeft, "Profile").gameObject, OnClickRaid).parameter = 1;
	Util.AddClick(TransformFindChild(uiRaidCardCenter, "Profile").gameObject, OnClickRaid).parameter = 2;
	Util.AddClick(TransformFindChild(uiRaidCardRight, "Profile").gameObject, OnClickRaid).parameter = 3;

	uiPopupRoot = TransformFindChild(transform,"PopupRoot");
end


function OnClickRaid(go)
	local listener = UIHelper.GetUIEventListener(go);
	if(listener~=nil and listener.parameter ~= nil) then
		--listener.parameter 1 2 3
		--show difficulty box
		local timeRaidNetInfoData = TimeRaidData.Get_TimeRaidInfoData();
		local realIdBase = (listener.parameter - 1) * 3;
        
        if(timeRaidNetInfoData[tostring(realIdBase+1)] == nil) then 
			timeRaidNetInfoData[tostring(realIdBase+1)] = 0;
		end
		if(timeRaidNetInfoData[tostring(realIdBase+2)] == nil) then 
			timeRaidNetInfoData[tostring(realIdBase+2)] = 0;
		end
		if(timeRaidNetInfoData[tostring(realIdBase+3)] == nil) then 
			timeRaidNetInfoData[tostring(realIdBase+3)] = 0;
		end
--		local challengeNum = 	timeRaidNetInfoData[tostring(realIdBase+1)]+
--								timeRaidNetInfoData[tostring(realIdBase+2)]+
--								timeRaidNetInfoData[tostring(realIdBase+3)];
--		local energyCost = timeRaidLocalDataTable[tostring(realIdBase+1)].Power;

		UITimeRaidDifficultyBox.CreateDifficultyBox( uiPopupRoot, windowComponent, 
			listener.parameter, OnClickDifficultyButton );
            
        windowComponent:AdjustSelfPanelDepth();
	else
		print("listener or listener.parameter is nil");
	end
end

function OnClickDifficultyButton(rid)	

	local AfterStart = function ()		
		OpenBattle(rid);		
	end

	print("you will start a time raid");
	TimeRaidData.RequestTimeRaidStart( {id = rid} , AfterStart );
end

function OpenBattle(rid)
	-- open battle here
	local combatData = TimeRaidData.Get_TimeRaidStartData();
	if (combatData ~= nil) then

		local delegateFinishMatch = function ()
			OnBattleEnd(rid);
		end

		local delegateStartMatch = function ()
			CombatData.FillData(combatData, CombatData.COMBAT_TYPE.TIMERAID, delegateFinishMatch);
		end

		UIPrepareMatch.RegisterBtnMatch(delegateStartMatch);
		TimeRaidData.SetEnemyInfo( 
			Config.GetProperty(Config.RaidDSTable(),tostring(rid),"name"),
			Config.GetProperty(Config.RaidDSTable(),tostring(rid),"unlock_lv"),
			"Default" );
		-- UIPrepareMatch.RegisterEnemyInfo(
		-- {
		-- 	name=Config.GetProperty(Config.RaidDSTable(),tostring(rid),"name"),
		-- 	lv=Config.GetProperty(Config.RaidDSTable(),tostring(rid),"unlock_lv"),
		-- 	icon="11000",
  --           team=Config.GetProperty(Config.RaidDSTeamTable(), rid, "team"),
  --           score=Config.GetProperty(Config.RaidDSTeamTable(), rid, "score"),
		-- });
		UIPrepareMatch.TryOpen();
	else
		WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { "服务器战斗返回数据异常"});
	end
end


function OnBattleEnd(rid)	
	local tb = CombatData.GetResultTable();
    m_meScore = tonumber(tb['HomeScore']);
    m_enemyScore = tonumber(tb['AwayScore']);
    local scorestr = tostring(m_meScore .. ":".. m_enemyScore);
    local actionList = tostring(tb['PVEData']);
    local signMD5 = tb['md5'];

	local OnCloseBattleResult = function()
        UITimeRaidDifficultyBox.RefreshInfo()
    end

    local AfterResult = function ()
		--update data
    	TimeRaidData.Update_TimeRaidInfoData(rid);

    	--set params
		local data_ = TimeRaidData.Get_TimeRaidResultData();
     	data_.BattleResultType = UIBattleResultS.enum_BattleResultType.TimeRaid;
        data_.Callback = OnCloseBattleResult;        
       
        --open battle result
		if (m_meScore >m_enemyScore) then
            WindowMgr.ShowWindow(LuaConst.Const.UIBattleResultS,data_);
--        elseif(m_meScore == m_enemyScore)then
--            WindowMgr.ShowWindow(LuaConst.Const.UIBattleResultD,data_);
        else
        	WindowMgr.ShowWindow(LuaConst.Const.UIBattleResultF,data_);
        end
	end    
    TimeRaidData.RequestTimeRaidResult({id = rid,score = scorestr,act = actionList ,sign = signMD5},AfterResult);		
end

--CurrentWeekDay is 0-6 : sunday - saturday
function ShowWeekDayContent(CurrentWeekDay)
	UIHelper.SetLabelTxt(label_weekofday, Util.LocalizeString(TimeRaidSettings.WeekDayKeys[CurrentWeekDay + 1]));
	timeRaidLocalDataTable = Config.GetTemplate(Config.RaidDSTable());
	local openningFlag = {	AttackRaidOpen = false,
							DefenceRaidOpen = false,
							OrganizeRaidOpen = false };
    local _max = LuaConst.Const.TimeRaidType * LuaConst.Const.TimeRaidNrByType;
	for i = 1, _max do
		local v = timeRaidLocalDataTable[tostring(i)];
		if (v.open[CurrentWeekDay+1] == 1) then
			--its open on CurrentWeekDay
			if (v.class == "1") then
				--attacking raid
				openningFlag.AttackRaidOpen = true;
			elseif(v.class == "2") then
				--defending raid
				openningFlag.DefenceRaidOpen = true;
			elseif(v.class == "3") then
				--organizing raid
				openningFlag.OrganizeRaidOpen = true;
			end		
		end
	end

    SetRaidData( openningFlag.AttackRaidOpen, uiRaidCardLeft, 1, TimeRaidSettings.IconAttack);
    SetRaidData( openningFlag.DefenceRaidOpen, uiRaidCardCenter, 2, TimeRaidSettings.IconDefence);
    SetRaidData( openningFlag.OrganizeRaidOpen, uiRaidCardRight, 3, TimeRaidSettings.IconOrganize);
end

function SetRaidData( active, raidTransform, class, iconName )
    raidTransform:SendMessage("Gray", active == false);
	if (active) then			
		ActivateRaid(raidTransform, class, iconName);
	else
		DisableRaid(raidTransform, iconName);
	end
    
    if (class == 1) then
        UIHelper.SetSpriteName(TransformFindChild(raidTransform, "Texture"), "Type_bg3");
    elseif (class == 2) then
        UIHelper.SetSpriteName(TransformFindChild(raidTransform, "Texture"), "Type_bg1");
    else
        UIHelper.SetSpriteName(TransformFindChild(raidTransform, "Texture"), "Type_bg2");
    end
    
    UIHelper.SetLabelTxt(TransformFindChild(raidTransform, "Texture/Label"), Util.LocalizeString(TimeRaidSettings.TypeTxtKeys[class]));
    UIHelper.SetLabelTxt(TransformFindChild(raidTransform, "Label - Desc"), Util.LocalizeString(string.format("UITimeRaid_Tips%d", class)));
    UIHelper.SetLabelTxt(TransformFindChild(raidTransform, "Label - Name"), Util.LocalizeString(string.format("UITimeRaid_Type%d", class)));
end

function ActivateRaid( raidTransform , raidclass, iconName)
	--print("ActivateRaid , class : "..raidclass);
	AddOrChangeClickParameters(raidTransform.gameObject, OnClickRaid, { class = raidclass } );
	UIHelper.SetButtonSpriteName(raidTransform, iconName);
end

function DisableRaid( raidTransform, iconName )
	--print("DisableRaid , iconName : "..iconName);	
	Util.RmvEventListener(raidTransform.gameObject);
	UIHelper.SetButtonSpriteName(raidTransform, iconName);
end

function OnHide()
    UITimeRaidDifficultyBox.OnDestroy();
end

function OnShow()
	ShowWeekDayContent(TimeRaidSettings.LocalTimeToday);
end

function OnDestroy()
	UITimeRaidDifficultyBox.OnDestroy();

	uiRaidCardLeft = nil;
	uiRaidCardCenter = nil;
	uiRaidCardRight = nil;
	uiButtonArrowLeft = nil;
	uiButtonArrowRight = nil;
	uiPopupRoot = nil;
	timeRaidNetStartData = nil;
	timeRaidNetResultData = nil;
	timeRaidNetFastResultData = nil;
	timeRaidLocalDataTable = nil;
    label_weekofday = nil;
end
