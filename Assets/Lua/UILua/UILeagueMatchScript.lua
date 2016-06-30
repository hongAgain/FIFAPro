module("UILeagueMatchScript", package.seeall)

require "Config"
require "Common/UnityCommonScript"
require "Common/Color"
require "Game/GameMainScript"
require "Game/ItemSys"
require "Game/Hero"
require "Game/PVPMsgManager"

--child module
require "UILua/UILeagueGroupManager"
require "UILua/UILeagueArrangement"
require "UILua/UILeagueRankings"
require "UILua/UILeaguePrizes"

--BGM name
local strBGMusic = "BG_LeagueMatch";

--league info
local uiLeagueInfo = {
	PlayerName = nil,
	PlayerLevel = nil,
	PlayerIcon = nil,
	LeagueName = nil,
	LeagueDuration = nil,
	LeagueTime = nil,
	TimerID = nil
}

--common buttons
local uiButtons = {
	Hint = nil,
	MyLeague = nil,
	MyLeagueContent = nil,
	Award = nil,
	TabSelectedLine = nil,
	ArrangesTab = nil,
	RankingsTab = nil
}

--nodes
local window = nil;
local windowComponent = nil;
local mainUIRoot = nil;
local awardUIRoot = nil;
local uiPopupRoot = nil;
local mainUIRootDepth = nil;
local popupRootDepth = nil;

local leagueSettings = {
	LeagueTitle = "LeagueTitle",
	LeagueDuration = "LeagueDuration",
	LeagueRules = "LeagueRules",
	MyLeagueSignUp = "SignUp",
	MyLeagueSignedUp = "SignedUp",
	MyLeagueShowDetail = "MyLeagueMatch",
	MyLeagueBanned = "SignBanned",
	MatchNotStarted = "MatchNotStarted",
	TabNormalColor = Color.New(171/255,173/255,185/255,255/255),
	TabSelectedColor = Color.New(255/255,255/255,255/255,255/255),
}

function OnStart(gameObject, params)
    print("UILeagueMatchScript.lua .. OnStart");
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");

    BindUI();
    SetCommonInfo();

    UILeagueGroupManager.CreateGroups(mainUIRoot,windowComponent,OnClickLevel,OnClickGroup,mainUIRootDepth);
    ShowDefaultTab();
    AudioMgr.Instance():PlayBGMusic(strBGMusic);
end

function BindUI()
	local transform = window.transform;

	--buttons under main ui
	uiButtons = {};
	uiButtons.Hint = TransformFindChild(transform,"MainUIRoot/Buttons/ButtonHint");
	uiButtons.MyLeague = TransformFindChild(transform,"MainUIRoot/Buttons/ButtonMyLeague");
	uiButtons.MyLeagueContent = TransformFindChild(uiButtons.MyLeague,"Content");
	uiButtons.Award = TransformFindChild(transform,"MainUIRoot/Buttons/ButtonAward");
	uiButtons.TabSelectedLine = TransformFindChild(transform,"MainUIRoot/Buttons/SelectedLine");
	uiButtons.ArrangesTab = TransformFindChild(transform,"MainUIRoot/Buttons/ButtonArranges");
	uiButtons.RankingsTab = TransformFindChild(transform,"MainUIRoot/Buttons/ButtonRankings");
	AddOrChangeClickParameters(uiButtons.Hint.gameObject,OnClickHint,nil);
	AddOrChangeClickParameters(uiButtons.MyLeague.gameObject,OnClickMyLeague,nil);
	AddOrChangeClickParameters(uiButtons.Award.gameObject,OnClickAward,nil);
	AddOrChangeClickParameters(uiButtons.ArrangesTab.gameObject,OnClickArrangesTab,nil);
	AddOrChangeClickParameters(uiButtons.RankingsTab.gameObject,OnClickRankingsTab,nil);

	--league info
	uiLeagueInfo = {};
	uiLeagueInfo.PlayerName = TransformFindChild(transform,"MainUIRoot/PlayerDetail/Name");
	uiLeagueInfo.PlayerLevel = TransformFindChild(transform,"MainUIRoot/PlayerDetail/Level");
	uiLeagueInfo.PlayerIcon = TransformFindChild(transform,"MainUIRoot/PlayerDetail/IconRoot/Icon");
	uiLeagueInfo.LeagueName = TransformFindChild(transform,"MainUIRoot/LeagueDetail/Name");
	uiLeagueInfo.LeagueDuration = TransformFindChild(transform,"MainUIRoot/LeagueDetail/Duration");
	uiLeagueInfo.LeagueTime = TransformFindChild(transform,"MainUIRoot/LeagueDetail/Time");
	uiLeagueInfo.TimerID = LuaTimer.AddTimer(true,-1,SetTimeInfo);

	--nodes
	mainUIRoot = TransformFindChild(transform,"MainUIRoot");
	awardUIRoot = TransformFindChild(transform,"AwardUIRoot");
	uiPopupRoot = TransformFindChild(transform,"PopupRoot");
	mainUIRootDepth = UIHelper.GetMaxDepthOfPanelInChildren(mainUIRoot);
	popupRootDepth = UIHelper.GetMaxDepthOfPanelInChildren(uiPopupRoot);
end

function ShowDefaultTab()
	-- Show daily tab by default
	OnClickArrangesTab();
end

function SetCommonInfo()
	-- local infoData = PVPMsgManager.Get_LeagueInfoData();
	local infoData = PVPMsgManager.GetPVPData(MsgID.tb.LeagueInfo);

	--set season info
	UIHelper.SetLabelTxt(uiLeagueInfo.LeagueName,GetLocalizedString(leagueSettings.LeagueTitle,infoData.sign));
	--datas used to calculate
	local currentSeason = infoData.sign;
	local daysInPerSeason = infoData.Cycle;
	local currentDayInThisSeason = infoData.day;
	local serverStartTimeStamp = Login.GetSrvInfo(DataSystemScript.GetRegionId()).time;
	-- 0-6 sunday - saturday
	local serverstartWeekDay = Util.GetDayInWeekFromTimeStamp(serverStartTimeStamp);
	local dayOffset = 7-(serverstartWeekDay+6)%7;
	local daysBeforeSeasonStart = (currentSeason-1)*daysInPerSeason+dayOffset;
	local daysBeforeSeasonFinish = (currentSeason)*daysInPerSeason-1+dayOffset;

	--datas needed to be shown
	local seasonStartYear = 		Util.GetYearAfterAddDay(serverStartTimeStamp,daysBeforeSeasonStart);
	local seasonStartMonth = 		Util.GetMonthAfterAddDay(serverStartTimeStamp,daysBeforeSeasonStart);
	local seasonStartDayOfMonth = 	Util.GetDayInMonthAfterAddDay(serverStartTimeStamp,daysBeforeSeasonStart);
	local seasonStartDayOfYear = 	Util.GetDayInYearAfterAddDay(serverStartTimeStamp,daysBeforeSeasonStart);
	local seasonFinishMonth = 		Util.GetMonthAfterAddDay(serverStartTimeStamp,daysBeforeSeasonFinish);
	local seasonFinishDayOfMonth = 	Util.GetDayInMonthAfterAddDay(serverStartTimeStamp,daysBeforeSeasonFinish);
	local seasonDaysLeft = 			daysInPerSeason - currentDayInThisSeason;
	-- UIHelper.SetLabelTxt(uiLeagueInfo.LeagueDuration,GetLocalizedString(leagueSettings.LeagueDuration,
	-- 																	seasonStartMonth,
	-- 																	seasonStartDayOfMonth,
	-- 																	seasonFinishMonth,
	-- 																	seasonFinishDayOfMonth,
	-- 																	seasonDaysLeft));
	UIHelper.SetLabelTxt(uiLeagueInfo.LeagueDuration,seasonStartMonth.."/"..seasonStartDayOfMonth.."-"..seasonFinishMonth.."/"..seasonFinishDayOfMonth);
	
	SetTimeInfo();
	
	--set myleague button, infoData.level：1-4 concrete leve, 0 not signed up, -1 banned, -2 already signed up
	if(infoData.level == 0) then
		UIHelper.SetLabelTxt(uiButtons.MyLeagueContent,GetLocalizedString(leagueSettings.MyLeagueSignUp));
		UIHelper.SetButtonActive(uiButtons.MyLeague,true,true);		
	elseif(infoData.level == -1)then
		UIHelper.SetLabelTxt(uiButtons.MyLeagueContent,GetLocalizedString(leagueSettings.MyLeagueBanned));	
		UIHelper.SetButtonActive(uiButtons.MyLeague,false,true);
	elseif(infoData.level == -2)then
		UIHelper.SetLabelTxt(uiButtons.MyLeagueContent,GetLocalizedString(leagueSettings.MyLeagueSignedUp));
		UIHelper.SetButtonActive(uiButtons.MyLeague,false,true);
	else
		UIHelper.SetLabelTxt(uiButtons.MyLeagueContent,GetLocalizedString(leagueSettings.MyLeagueShowDetail));
		UIHelper.SetButtonActive(uiButtons.MyLeague,true,true);
	end
	AddOrChangeClickParameters(uiButtons.MyLeague.gameObject,OnClickMyLeague,{status = infoData.level});

	--set player info
	UIHelper.SetLabelTxt(uiLeagueInfo.PlayerName,Role.Get_name());
	UIHelper.SetLabelTxt(uiLeagueInfo.PlayerLevel,"Lv."..Role.Get_lv());
	Util.SetUITexture(uiLeagueInfo.PlayerIcon,LuaConst.Const.ClubIcon,Role.GetRoleIcon().."_2", true);
					
end

function SetTimeInfo()
	if(uiLeagueInfo~=nil and uiLeagueInfo.LeagueTime~=nil)then
		UIHelper.SetLabelTxt(uiLeagueInfo.LeagueTime,os.date("%X",os.time()));
	end
end

function SelectGroup(needResetPos)
	UILeagueGroupManager.SetGroups(needResetPos);
	UILeagueArrangement.RefreshLeagueArrangeUI();
	UILeagueRankings.RefreshLeagueRankingsUI();
end

function OnClickArrangesTab()
	print("===== > OnClickArrangesTab : ");
	--set button
	UIHelper.SetWidgetColor(uiButtons.ArrangesTab,leagueSettings.TabSelectedColor);
	UIHelper.SetWidgetColor(uiButtons.RankingsTab,leagueSettings.TabNormalColor);
	uiButtons.TabSelectedLine.transform.localPosition = NewVector3(
		uiButtons.ArrangesTab.transform.localPosition.x,
		uiButtons.ArrangesTab.transform.localPosition.y-20,
		uiButtons.ArrangesTab.transform.localPosition.z);
	--set ui
	UILeagueArrangement.CreateLeagueArrangementUI(mainUIRoot,windowComponent,OnClickReplayMatch,mainUIRootDepth);
	UILeagueRankings.CloseLeagueRankingsUI();
end

function OnClickRankingsTab()
	print("===== > OnClickRankingsTab : ");
	--set buttons
	UIHelper.SetWidgetColor(uiButtons.RankingsTab,leagueSettings.TabSelectedColor);
	UIHelper.SetWidgetColor(uiButtons.ArrangesTab,leagueSettings.TabNormalColor);
	uiButtons.TabSelectedLine.transform.localPosition = NewVector3(
		uiButtons.RankingsTab.transform.localPosition.x,
		uiButtons.RankingsTab.transform.localPosition.y-20,
		uiButtons.RankingsTab.transform.localPosition.z);
	--set ui
	UILeagueArrangement.CloseLeagueArrangementUI();
	UILeagueRankings.CreateLeagueRankingsUI(mainUIRoot,windowComponent,mainUIRootDepth);
end

function OnClickReplayMatch(go)
	local listener = UIHelper.GetUIEventListener(go);
	-- id = groupData.AList[matchID].logid,
	-- state = groupData.AList[matchID].state,
	-- hostName = currentGroupPlayers[groupTime[v].day[hostIndex]].name,
	-- guestName = currentGroupPlayers[groupTime[v].day[guestIndex]].name});
	print("===== > OnClickReplayMatch, matchID: "..listener.parameter.id);
	if(listener.parameter.state==0)then
		--not started cannot replay
		WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { GetLocalizedString(leagueSettings.MatchNotStarted); });	
	else
		--replay it
		WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, 
			{ "这里开始重播比赛:"..listener.parameter.id..";主队"..listener.parameter.hostName.."对阵客队"..listener.parameter.guestName });	
	end
end

function OnClickMyLeague(go)
	print("===== > OnClickMyLeague");
	local listener = UIHelper.GetUIEventListener(go);
	if(listener.parameter.status == -2) then
		--already signed up
		WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { GetLocalizedString("PlsWaitNextSeason"); });	
	elseif(listener.parameter.status == -1)then
		--banned

	elseif(listener.parameter.status == 0)then
		--request to sign up
		local AfterRequestJoin = function()
			-- local joinData = PVPMsgManager.Get_LeagueJoinData();
			local joinData = PVPMsgManager.GetPVPData(MsgID.tb.LeagueJoin);
			PVPMsgManager.UpdateLeagueInfoData(joinData[1].level);

			-- local infoData = PVPMsgManager.Get_LeagueInfoData();
			local infoData = PVPMsgManager.GetPVPData(MsgID.tb.LeagueInfo);
			UIHelper.SetLabelTxt(uiButtons.MyLeagueContent,GetLocalizedString(leagueSettings.MyLeagueSignedUp));
			AddOrChangeClickParameters(uiButtons.MyLeague.gameObject,OnClickMyLeague,{status = infoData.level});
		end
		-- PVPMsgManager.RequestLeagueJoin(nil,AfterRequestJoin);
		PVPMsgManager.RequestPVPMsg(MsgID.tb.LeagueJoin, LuaConst.Const.LeagueJoin,nil, AfterRequestJoin, nil );
	else
		--show detail of my league match
		-- local infoData = PVPMsgManager.Get_LeagueInfoData();
		local infoData = PVPMsgManager.GetPVPData(MsgID.tb.LeagueInfo);
		-- PVPMsgManager.RequestLeagueGroup({sign = infoData.sign,level = infoData.level,group = infoData.group},function () SelectGroup(false);end);		
		PVPMsgManager.RequestPVPMsg(MsgID.tb.LeagueGroup, LuaConst.Const.LeagueGroup, {sign = infoData.sign,level = infoData.level,group = infoData.group}, function () SelectGroup(nil);end, nil );
	end
end

function OnClickAward()
	print("===== > OnClickAward : ");
	UILeaguePrizes.CreatePrizeUI(uiPopupRoot,windowComponent,popupRootDepth);
end

function OnClickLevel( levelID )
	-- local infoData = PVPMsgManager.Get_LeagueInfoData();
	local infoData = PVPMsgManager.GetPVPData(MsgID.tb.LeagueInfo);
	-- PVPMsgManager.RequestLeagueGroup({sign = infoData.sign,level = levelID,group = 1},function () SelectGroup(false);end);
	PVPMsgManager.RequestPVPMsg(MsgID.tb.LeagueGroup, LuaConst.Const.LeagueGroup, {sign = infoData.sign,level = levelID,group = 1}, function () SelectGroup(true);end, nil );
end

function OnClickGroup( groupID )
	-- local infoData = PVPMsgManager.Get_LeagueInfoData();
	-- local groupData = PVPMsgManager.Get_LeagueGroupData();
	local infoData = PVPMsgManager.GetPVPData(MsgID.tb.LeagueInfo);
	local groupData = PVPMsgManager.GetPVPData(MsgID.tb.LeagueGroup);
	-- PVPMsgManager.RequestLeagueGroup({sign = infoData.sign,level = groupData.level,group = groupID},function () SelectGroup(false);end);
	PVPMsgManager.RequestPVPMsg(MsgID.tb.LeagueGroup, LuaConst.Const.LeagueGroup, {sign = infoData.sign,level = groupData.level,group = groupID}, function () SelectGroup(false);end, nil );
end

function OnClickHint()
	WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxScrollText, { PVPMsgManager.GetLeagueRule() });	
	-- WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxScrollText, { GetModuleRules(LuaConst.Const.LeagueRule); });
end

function OnHide()
    print("..OnHide UILeagueMatchScript");
end

function OnShow()
	print("..OnShow UILeagueMatchScript");
end

function OnDestroy()
    print("..OnDestroy UILeagueMatchScript");
	UILeagueArrangement.OnDestroy();
	UILeagueRankings.OnDestroy();
	UILeagueGroupManager.OnDestroy();
	UILeaguePrizes.OnDestroy();

	if(uiLeagueInfo~=nil and uiLeagueInfo.TimerID ~= nil) then
		LuaTimer.RmvTimer(uiLeagueInfo.TimerID);
	end

	uiLeagueInfo = {
		PlayerName = nil,
		PlayerLevel = nil,
		PlayerIcon = nil,
		LeagueName = nil,
		LeagueDuration = nil,
		LeagueTime = nil,
		TimerID = nil
	};
	uiButtons = {
		Hint = nil,
		MyLeague = nil,
		MyLeagueContent = nil,
		Award = nil,
		TabSelectedLine = nil,
		ArrangesTab = nil,
		RankingsTab = nil
	};
	window = nil;
	windowComponent = nil;
	mainUIRoot = nil;
	awardUIRoot = nil;
	uiPopupRoot = nil;
	mainUIRootDepth = nil;
	popupRootDepth = nil;
end