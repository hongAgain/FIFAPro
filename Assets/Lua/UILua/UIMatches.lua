module("UIMatches",package.seeall);

local willOpenCareer = true;

local uiScrollView = nil;
local uiContainer = nil;
local scrollViewPos = NewVector3(0,-46,0);
local gridSizeWidth = 944;

local buttonEpicRoad = nil;
local buttonTimeRaid = nil;
--local buttonEuroLeagueMatch = nil;
--local buttonTeamChallenge = nil;

local buttonLadderMatch = nil;
--local buttonLeagueMatch = nil;
local buttonDailyCup = nil;
--local buttonWorldCup = nil;
--local buttonWIFIBattle = nil;

local dots = {};
local dotColorNormal = Color.New(63/255,67/255,78/255,255/255);
local dotColorSelected = Color.New(102/255,204/255,255/255,255/255);

local tweenerColorFadeIn = Color.New(39/255,97/255,155/255,128/255);
local tweenerColorFadeOut = Color.New(39/255,97/255,155/255,0/255);

local window = nil;
local windowComponent = nil;
local title1 = nil;
local title2 = nil;

function OnStart(gameObject, params)
    print("UIMatches.lua .. OnStart");
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");
    --fetch data then set UI    
    BindUI();

    willOpenCareer = params.willOpenCareer;
    SetInfo();
end

function BindUI()
	local transform = window.transform;
	
	uiScrollView = TransformFindChild(transform,"uiScrollView");
	uiContainer = TransformFindChild(uiScrollView,"uiContainer");
    
    buttonTeamLegend = {};
	buttonTeamLegend.transform = TransformFindChild(uiContainer,"1/ButtonTeamLegend");
	buttonTeamLegend.gameObject = buttonTeamLegend.transform.gameObject;
	buttonTeamLegend.tweener = TransformFindChild(buttonTeamLegend.transform,"ColorTweenLayer");
	AddOrChangeClickParameters(buttonTeamLegend.gameObject,OnClickEntry,{tweener = buttonTeamLegend.tweener,onClick = OnClickTeamLegend});

	buttonEpicRoad = {};
	buttonEpicRoad.transform = TransformFindChild(uiContainer,"1/ButtonEpicRoad");
	buttonEpicRoad.gameObject = buttonEpicRoad.transform.gameObject;
	buttonEpicRoad.tweener = TransformFindChild(buttonEpicRoad.transform,"ColorTweenLayer");
	AddOrChangeClickParameters(buttonEpicRoad.gameObject,OnClickEntry,{tweener = buttonEpicRoad.tweener,onClick = OnClickEpicRoad});

	buttonTimeRaid = {};
	buttonTimeRaid.transform = TransformFindChild(uiContainer,"1/ButtonTimeRaid");
	buttonTimeRaid.gameObject = buttonTimeRaid.transform.gameObject;
	buttonTimeRaid.tweener = TransformFindChild(buttonTimeRaid.transform,"ColorTweenLayer");
	AddOrChangeClickParameters(buttonTimeRaid.gameObject,OnClickEntry,{tweener = buttonTimeRaid.tweener,onClick = OnClickTimeRaid});

	--[[buttonEuroLeagueMatch = {};
	buttonEuroLeagueMatch.transform = TransformFindChild(uiContainer,"1/ButtonEuroLeagueMatch");
	buttonEuroLeagueMatch.gameObject = buttonEuroLeagueMatch.transform.gameObject;
	buttonEuroLeagueMatch.tweener = TransformFindChild(buttonEuroLeagueMatch.transform,"ColorTweenLayer");
	AddOrChangeClickParameters(buttonEuroLeagueMatch.gameObject,OnClickEntry,{tweener = buttonEuroLeagueMatch.tweener,onClick = OnClickEuropeLeague});]]--
	
	--[[buttonTeamChallenge = {};
	buttonTeamChallenge.transform = TransformFindChild(uiContainer,"1/ButtonTeamChallenge");
	buttonTeamChallenge.gameObject = buttonTeamChallenge.transform.gameObject;
	buttonTeamChallenge.tweener = TransformFindChild(buttonTeamChallenge.transform,"ColorTweenLayer");
	AddOrChangeClickParameters(buttonTeamChallenge.gameObject,OnClickEntry,{tweener = buttonTeamChallenge.tweener,onClick = OnClickTeamChallenge});]]--

	buttonLadderMatch = {};
	buttonLadderMatch.transform = TransformFindChild(uiContainer,"2/ButtonLadderMatch");
	buttonLadderMatch.gameObject = buttonLadderMatch.transform.gameObject;
	buttonLadderMatch.tweener = TransformFindChild(buttonLadderMatch.transform,"ColorTweenLayer");
	AddOrChangeClickParameters(buttonLadderMatch.gameObject,OnClickEntry,{tweener = buttonLadderMatch.tweener,onClick = OnClickLadderMatch});

	--[[buttonLeagueMatch = {};
	buttonLeagueMatch.transform = TransformFindChild(uiContainer,"2/ButtonLeagueMatch");
	buttonLeagueMatch.gameObject = buttonLeagueMatch.transform.gameObject;
	buttonLeagueMatch.tweener = TransformFindChild(buttonLeagueMatch.transform,"ColorTweenLayer");
	AddOrChangeClickParameters(buttonLeagueMatch.gameObject,OnClickEntry,{tweener = buttonLeagueMatch.tweener,onClick = OnClickLeagueMatch});]]--

	buttonDailyCup = {};
	buttonDailyCup.transform = TransformFindChild(uiContainer,"2/ButtonDailyCup");
	buttonDailyCup.gameObject = buttonDailyCup.transform.gameObject;
	buttonDailyCup.tweener = TransformFindChild(buttonDailyCup.transform,"ColorTweenLayer");
	AddOrChangeClickParameters(buttonDailyCup.gameObject,OnClickEntry,{tweener = buttonDailyCup.tweener,onClick = OnClickDailyCup});

	--[[buttonWorldCup = {};
	buttonWorldCup.transform = TransformFindChild(uiContainer,"2/ButtonWorldCup");
	buttonWorldCup.gameObject = buttonWorldCup.transform.gameObject;
	buttonWorldCup.tweener = TransformFindChild(buttonWorldCup.transform,"ColorTweenLayer");
	AddOrChangeClickParameters(buttonWorldCup.gameObject,OnClickEntry,{tweener = buttonWorldCup.tweener,onClick = OnClickWorldCup});]]--

	--[[buttonWIFIBattle = {};
	buttonWIFIBattle.transform = TransformFindChild(uiContainer,"2/ButtonWIFIBattle");
	buttonWIFIBattle.gameObject = buttonWIFIBattle.transform.gameObject;
	buttonWIFIBattle.tweener = TransformFindChild(buttonWIFIBattle.transform,"ColorTweenLayer");
	AddOrChangeClickParameters(buttonWIFIBattle.gameObject,OnClickEntry,{tweener = buttonWIFIBattle.tweener,onClick = OnClickWifiBattle});]]--

	dots = {};
	dots[1] = TransformFindChild(transform,"Dot1");
	dots[2] = TransformFindChild(transform,"Dot2");

	UIHelper.AddDragOnStarted(uiScrollView,OnDragStarted);
	UIHelper.AddDragOnFinish(uiScrollView,OnDragFinish);
    UIHelper.OnCenterItem(uiContainer, OnCenter);
    
    title1 = TransformFindChild(transform, "Container/Title1");
    title2 = TransformFindChild(transform, "Container/Title2");
end

function SetInfo()
	if(willOpenCareer)then
		UIHelper.SetGridPosition(
			uiContainer,
			uiScrollView,
			scrollViewPos,
			SetContainerPos(1),
			false);

		UIHelper.SetWidgetColor(dots[1].transform,dotColorSelected);
		UIHelper.SetWidgetColor(dots[2].transform,dotColorNormal);
        title1.gameObject:SetActive(true);
        title2.gameObject:SetActive(false);
	else
		UIHelper.SetGridPosition(
			uiContainer,
			uiScrollView,
			scrollViewPos,
			SetContainerPos(2),
			false);
		UIHelper.SetWidgetColor(dots[2].transform,dotColorSelected);
		UIHelper.SetWidgetColor(dots[1].transform,dotColorNormal);
        title1.gameObject:SetActive(false);
        title2.gameObject:SetActive(true);
	end
	
	UIHelper.RefreshPanel(uiScrollView);
end

function SetContainerPos(dotIndex)
	return NewVector3((1-dotIndex)*gridSizeWidth,0,0);
end

function OnDragStarted()
	--disable center on child here
	Util.EnableScript(uiContainer.gameObject,"UICenterOnChild",false);
end

function OnDragFinish()
	--enable center on child here
	Util.EnableScript(uiContainer.gameObject,"UICenterOnChild",true);
	RefreshDots();
end

function RefreshDots()
	local m_centerObject = UIHelper.CenterOnRecenter(uiContainer);
	local selectedDot = tonumber(m_centerObject.name);

	for i=1,2 do
		if(selectedDot == i)then
			UIHelper.SetWidgetColor(dots[i].transform,dotColorSelected);
		else
			UIHelper.SetWidgetColor(dots[i].transform,dotColorNormal);
		end
	end
end

function TweenTheTweener(tweener,onTweenFinished)
	FadeTweenerIn(tweener,
		function ()
			FadeTweenerOut(tweener,onTweenFinished);
		end
	);
end

function FadeTweenerIn(tweener,onFinished)
	UIHelper.FadeUIWidgetColor(tweener,tweenerColorFadeOut,tweenerColorFadeIn,0.3,onFinished);
end

function FadeTweenerOut(tweener,onFinished)
	UIHelper.FadeUIWidgetColor(tweener,tweenerColorFadeIn,tweenerColorFadeOut,0.15,onFinished);
end

function OnClickEntry(go)
	local listener = UIHelper.GetUIEventListener(go);
	if(listener~=nil and listener.parameter~=nil)then
		TweenTheTweener(listener.parameter.tweener,listener.parameter.onClick);
	end
end

---------------------------------Careers
function OnClickTeamLegend()
    TeamLegendData.OpenTeamLegend(TeamLegendData.e_raidDiff.Normal);
end

function OnClickEpicRoad()
	local function OnReqInfo(data_)
        WindowMgr.ShowWindow(LuaConst.Const.UIPeakRoad,data_);
    end
    PeakRoadData.ReqRaidDFInfo(OnReqInfo);
end

function OnClickTimeRaid()
    local AfterRequesting = function ()
        WindowMgr.ShowWindow(LuaConst.Const.UITimeRaid);
    end
    TimeRaidData.RequestTimeRaidInfo(AfterRequesting);
end

--[[function OnClickEuropeLeague()
    -- temp begin
    local function AfterRequesting(data_)
        if (tonumber(data_.s) == 0) then
            WindowMgr.ShowWindow(LuaConst.Const.UIPVELeague, data_);
        else
            WindowMgr.ShowWindow(LuaConst.Const.UIPVELeagueSchedule, data_);
        end
    end
    PVELeagueData.RequestPVELeagueInfo(AfterRequesting);
    -- temp end

	-- WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { "紧张开发中，敬请期待！" });
end]]--

--[[function OnClickTeamChallenge()
	WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { "紧张开发中，敬请期待！" });
end]]--

---------------------------------Arenas
function OnClickLadderMatch()
	local AfterRequesting = function ()
        WindowMgr.ShowWindow(LuaConst.Const.UILadderMatch);
    end    
    -- PVPMsgManager.RequestLadderInfo(AfterRequesting);    
    PVPMsgManager.RequestPVPMsg( MsgID.tb.LadderInfo, LuaConst.Const.LadderInfo, nil, AfterRequesting, nil );
end

--[[function OnClickLeagueMatch()
	local AfterRequestGroupData = function ()
        WindowMgr.ShowWindow(LuaConst.Const.UILeagueMatch);
    end
    local AfterRequestInfo = function ()
        -- local infoData = PVPMsgManager.Get_LeagueInfoData();
        local infoData = PVPMsgManager.GetPVPData(MsgID.tb.LeagueInfo);
        -- PVPMsgManager.RequestLeagueGroup({sign = infoData.sign,level = 4,group = 1},AfterRequestGroupData);
    	PVPMsgManager.RequestPVPMsg(MsgID.tb.LeagueGroup, LuaConst.Const.LeagueGroup, {sign = infoData.sign,level = 4,group = 1}, AfterRequestGroupData, nil );
	
	end  
    -- PVPMsgManager.RequestLeagueInfo(nil,AfterRequestInfo);
    PVPMsgManager.RequestPVPMsg(MsgID.tb.LeagueInfo, LuaConst.Const.LeagueInfo, nil, AfterRequestInfo, nil );
end]]--

--[[function OnClickWorldCup()
	print("===== > OnClickWorldCup");
	WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { "紧张开发中，敬请期待！" });
end]]--

function OnClickDailyCup()
    local AfterRequestInfo = function ()
        WindowMgr.ShowWindow(LuaConst.Const.UIDailyCup);
    end  
    -- PVPMsgManager.RequestDailyCupInfo(nil,AfterRequestInfo,AfterRequestInfo);
    PVPMsgManager.RequestPVPMsg(MsgID.tb.DailyCupInfo, LuaConst.Const.DailyCupInfo, nil, AfterRequestInfo, AfterRequestInfo );
end

function OnCenter(centerObj)
    if (centerObj.name == "1") then
        title1.gameObject:SetActive(true);
        title2.gameObject:SetActive(false);
    else
        title1.gameObject:SetActive(false);
        title2.gameObject:SetActive(true);
    end
end

--[[function OnClickWifiBattle()
	WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { "紧张开发中，敬请期待！" });	
end]]--

function OnDestroy()
    window = nil;
    windowComponent = nil;

    willOpenCareer = true;

	uiScrollView = nil;
	uiContainer = nil;
	gridSizeWidth = 944;

	buttonEpicRoad = nil;
	buttonTimeRaid = nil;
	--buttonEuroLeagueMatch = nil;
	--buttonTeamChallenge = nil;

	buttonLadderMatch = nil;
	--buttonLeagueMatch = nil;
	buttonDailyCup = nil;
	--buttonWorldCup = nil;
	--buttonWIFIBattle = nil;

	dots = {};
end
