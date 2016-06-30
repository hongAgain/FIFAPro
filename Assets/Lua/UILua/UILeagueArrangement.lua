module("UILeagueArrangement", package.seeall)

--child module
require "Game/PVPMsgManager"
--require "UILua/UILeagueMatchScript"

local uiPrefabs = {
	LeagueArrangesUIName = "LeagueArrangementUI",
	LeagueArrangesUI = nil,

	LeagueDayArrangesPageName = "LeagueDayArrangesPage",
	LeagueDayArrangesPage = nil
}

local leagueArrangementUI = nil;

local leagueDayArrangesPages = {};

local weekDayDots = {};
local uiScrollView = nil;
local uiContainer = nil;

local groupData = nil;
local groupTime = nil;
local currentGroupPlayers = {};
local sortedMatchTimeKey = {};
local selectedDay = nil;
local delegateOnReplayMatch = nil;

local leagueArrangesSettings = {

	LeagueDayArrangesOffsetX = 600,
	LeagueDayArrangesOffsetY = -30,
	WeekDayDotSelectedColor = Color.New(102/255,204/255,255/255,255/255),
	WeekDayDotNormalColor = Color.New(63/255,67/255,78/255,255/255),
	WeekDayPrefix = "WeekDay",
	MatchItemColor1 = Color.New(204/255,204/255,241/255,12/255),
	MatchItemColor2 = Color.New(0/255,0/255,0/255,0/255),
	MatchItemSelectedColor = Color.New(25/255,48/255,70/255,204/255),
	ScrollViewPos = NewVector3(172,-42,0)

}


function CreateLeagueArrangementUI( containerTransform, windowComponent, delegateonreplaymatch, depth )
	if(leagueArrangementUI == nil) then
		--Get prefabs
		uiPrefabs.LeagueArrangesUI = windowComponent:GetPrefab(uiPrefabs.LeagueArrangesUIName);
		uiPrefabs.LeagueDayArrangesPage = windowComponent:GetPrefab(uiPrefabs.LeagueDayArrangesPageName);

		--generate ui and initialize it
		leagueArrangementUI = GameObjectInstantiate(uiPrefabs.LeagueArrangesUI);
		leagueArrangementUI.transform.parent = containerTransform;
    	leagueArrangementUI.transform.localPosition = Vector3.zero;
    	leagueArrangementUI.transform.localScale = Vector3.one; 	

		delegateOnReplayMatch = delegateonreplaymatch;
		BindUI(depth);		
	end

	--active it
	if(not GameObjectActiveSelf(leagueArrangementUI)) then
    	GameObjectSetActive(leagueArrangementUI.transform,true);
    end

	--set info  0-6 : sunday - saturday
	selectedDay = Util.GetLocalDayInWeek();
	if(selectedDay == 0) then 
		selectedDay = 7;
	end
	ProcessTimeData();
	ProcessGroupData();
	SetInfo(selectedDay);
end

function RefreshLeagueArrangeUI()
	if(leagueArrangementUI == nil or (not GameObjectActiveSelf(leagueArrangementUI))) then
		return;
	end
    if(selectedDay == nil) then
    	selectedDay = Util.GetLocalDayInWeek();
    end
    if(selectedDay == 0) then 
		selectedDay = 7;
	end
	ProcessGroupData();
	ResetInfo(selectedDay);
end

function BindUI(depth)
	weekDayDots = {};
	for i=1,7 do
		weekDayDots[i]={};
		weekDayDots[i].transform = TransformFindChild(leagueArrangementUI.transform,"Bottom/Dot"..i);
	end

	uiScrollView = TransformFindChild(leagueArrangementUI.transform,"uiScrollView");
	uiContainer = TransformFindChild(leagueArrangementUI.transform,"uiScrollView/uiContainer");

	UIHelper.SetPanelDepth(uiScrollView,depth+1);
	UIHelper.SetDragScrollViewTarget(TransformFindChild(leagueArrangementUI.transform,"DragScrollView"),uiScrollView);

	UIHelper.AddDragOnStarted(uiScrollView,OnDragStarted);
	UIHelper.AddDragOnFinish(uiScrollView,OnDragFinish);

end

function ProcessTimeData()
	--sort time table by match time id, result:[minID,...,maxID]
	groupTime = PVPMsgManager.Get_LeagueTimeTable();
	sortedMatchTimeKey = {};
	for k,v in pairs(groupTime) do
		table.insert(sortedMatchTimeKey,v.id);
	end
	table.sort(sortedMatchTimeKey);	
end

function ProcessGroupData()
	--sort players by index
	-- groupData = PVPMsgManager.Get_LeagueGroupData();
	groupData = PVPMsgManager.GetPVPData(MsgID.tb.LeagueGroup);
	currentGroupPlayers = {};
	if(IsTableEmpty(groupData))then
		return;
	end
	for k,v in pairs(groupData.Player) do
		currentGroupPlayers[v.i] = v;
	end
end

function SetInfo(dayID)
	--destroy old items
	DestroyUIListItemGameObjects(leagueDayArrangesPages);
	leagueDayArrangesPages = {};
	--generate new items
	for i=1,7 do
		--bind ui
		leagueDayArrangesPages[i] = {};
		leagueDayArrangesPages[i].gameObject = GameObjectInstantiate(uiPrefabs.LeagueDayArrangesPage);
		leagueDayArrangesPages[i].gameObject.name = i;
		leagueDayArrangesPages[i].transform = leagueDayArrangesPages[i].gameObject.transform;
		leagueDayArrangesPages[i].transform.parent = uiContainer;
    	leagueDayArrangesPages[i].transform.localPosition = Vector3.zero;
    	leagueDayArrangesPages[i].transform.localScale = Vector3.one;

    	leagueDayArrangesPages[i].title = TransformFindChild(leagueDayArrangesPages[i].transform,"Title");
    	UIHelper.SetLabelTxt(leagueDayArrangesPages[i].title,GetLocalizedString(leagueArrangesSettings.WeekDayPrefix..i));

		leagueDayArrangesPages[i].uiMatchItems = {};
		for j=1,8 do
			leagueDayArrangesPages[i].uiMatchItems[j] = {};
			leagueDayArrangesPages[i].uiMatchItems[j].transform = TransformFindChild(leagueDayArrangesPages[i].transform,"MatchItem"..j);
			leagueDayArrangesPages[i].uiMatchItems[j].Time = TransformFindChild(leagueDayArrangesPages[i].uiMatchItems[j].transform,"Time");
			leagueDayArrangesPages[i].uiMatchItems[j].Name1 = TransformFindChild(leagueDayArrangesPages[i].uiMatchItems[j].transform,"Name1");
			leagueDayArrangesPages[i].uiMatchItems[j].Name2 = TransformFindChild(leagueDayArrangesPages[i].uiMatchItems[j].transform,"Name2");
			leagueDayArrangesPages[i].uiMatchItems[j].Score1 = TransformFindChild(leagueDayArrangesPages[i].uiMatchItems[j].transform,"Score1");
			leagueDayArrangesPages[i].uiMatchItems[j].Score2 = TransformFindChild(leagueDayArrangesPages[i].uiMatchItems[j].transform,"Score2");
			leagueDayArrangesPages[i].uiMatchItems[j].ButtonReplay = TransformFindChild(leagueDayArrangesPages[i].uiMatchItems[j].transform,"ButtonReplay");
			leagueDayArrangesPages[i].uiMatchItems[j].ItemBG = TransformFindChild(leagueDayArrangesPages[i].uiMatchItems[j].transform,"ItemBG");
		end
		-- leagueDayArrangesPages[i].dragScrollView = TransformFindChild(leagueDayArrangesPages[i].transform,"DragScrollView");
		-- UIHelper.SetDragScrollViewTarget(leagueDayArrangesPages[i].dragScrollView,uiScrollView);
		
		--set match info for this day i
		SetSingleInfo(i,dayID);
	end
	UIHelper.SetGridPosition(
		uiContainer,
		uiScrollView,
		leagueArrangesSettings.ScrollViewPos,
		SetContainerPos(dayID),
		false);
   	UIHelper.RefreshPanel(uiScrollView);
end

function SetContainerPos(dayID)	
	return NewVector3(
		(1-dayID)*leagueArrangesSettings.LeagueDayArrangesOffsetX,
		leagueArrangesSettings.LeagueDayArrangesOffsetY,
		0);
end

function ResetInfo(dayID)
	--reset info for pages
	for i=1,7 do
		--set matches for this day i
		SetSingleInfo(i,dayID);
	end
	UIHelper.SetGridPosition(
		uiContainer,
		uiScrollView,
		leagueArrangesSettings.ScrollViewPos,
		SetContainerPos(dayID),
		false);
   	UIHelper.RefreshPanel(uiScrollView);
end

function SetSingleInfo(i,dayID)
	--set matches for this day i
	local guestIndex = i * 2;
	local hostIndex = guestIndex - 1;

	for j,v in ipairs(sortedMatchTimeKey) do
		local matchtime = tonumber(v);
		local matchID = i..v;

		if(IsTableEmpty(currentGroupPlayers))then
			UIHelper.SetLabelTxt(leagueDayArrangesPages[i].uiMatchItems[j].Time,math.floor(matchtime/100)..":"..(matchtime%100));
			UIHelper.SetLabelTxt(leagueDayArrangesPages[i].uiMatchItems[j].Name1,"");
			UIHelper.SetLabelTxt(leagueDayArrangesPages[i].uiMatchItems[j].Name2,"");
			--set bg color
			if(j==1 or j==3 or j==6 or j==8)then
				UIHelper.SetWidgetColor(leagueDayArrangesPages[i].uiMatchItems[j].ItemBG,leagueArrangesSettings.MatchItemColor1);
			else
				UIHelper.SetWidgetColor(leagueDayArrangesPages[i].uiMatchItems[j].ItemBG,leagueArrangesSettings.MatchItemColor2);
			end	
			UIHelper.SetLabelTxt(leagueDayArrangesPages[i].uiMatchItems[j].Score1,"0");
			UIHelper.SetLabelTxt(leagueDayArrangesPages[i].uiMatchItems[j].Score2,"0");
			GameObjectSetActive(leagueDayArrangesPages[i].uiMatchItems[j].ButtonReplay.gameObject,false);			
		else
			UIHelper.SetLabelTxt(leagueDayArrangesPages[i].uiMatchItems[j].Time,math.floor(matchtime/100)..":"..(matchtime%100));
			UIHelper.SetLabelTxt(leagueDayArrangesPages[i].uiMatchItems[j].Name1,currentGroupPlayers[groupTime[v].day[hostIndex]].name);
			UIHelper.SetLabelTxt(leagueDayArrangesPages[i].uiMatchItems[j].Name2,currentGroupPlayers[groupTime[v].day[guestIndex]].name);
			--set bg color
			if(Role.Get_uid() == currentGroupPlayers[groupTime[v].day[hostIndex]].uid or 
				Role.Get_uid() == currentGroupPlayers[groupTime[v].day[guestIndex]].uid ) then
					UIHelper.SetWidgetColor(leagueDayArrangesPages[i].uiMatchItems[j].ItemBG,leagueArrangesSettings.MatchItemSelectedColor);
			else
				if(j==1 or j==3 or j==6 or j==8)then
					UIHelper.SetWidgetColor(leagueDayArrangesPages[i].uiMatchItems[j].ItemBG,leagueArrangesSettings.MatchItemColor1);
				else
					UIHelper.SetWidgetColor(leagueDayArrangesPages[i].uiMatchItems[j].ItemBG,leagueArrangesSettings.MatchItemColor2);
				end
			end

			if(groupData.AList[matchID].state == 0)then
				--not started yet
				UIHelper.SetLabelTxt(leagueDayArrangesPages[i].uiMatchItems[j].Score1,"0");
				UIHelper.SetLabelTxt(leagueDayArrangesPages[i].uiMatchItems[j].Score2,"0");
				GameObjectSetActive(leagueDayArrangesPages[i].uiMatchItems[j].ButtonReplay.gameObject,false);
			else
				--already started
				UIHelper.SetLabelTxt(leagueDayArrangesPages[i].uiMatchItems[j].Score1,groupData.AList[matchID].score[1]);
				UIHelper.SetLabelTxt(leagueDayArrangesPages[i].uiMatchItems[j].Score2,groupData.AList[matchID].score[2]);
				GameObjectSetActive(leagueDayArrangesPages[i].uiMatchItems[j].ButtonReplay.gameObject,true);
				AddOrChangeClickParameters(leagueDayArrangesPages[i].uiMatchItems[j].ButtonReplay.gameObject,delegateOnReplayMatch,
									{	id = groupData.AList[matchID].logid,
										state = groupData.AList[matchID].state,
										hostName = currentGroupPlayers[groupTime[v].day[hostIndex]].name,
										guestName = currentGroupPlayers[groupTime[v].day[guestIndex]].name});
			end
		end		
	end

	if(dayID == i)then
		UIHelper.SetWidgetColor(weekDayDots[i].transform,leagueArrangesSettings.WeekDayDotSelectedColor);
	else
		UIHelper.SetWidgetColor(weekDayDots[i].transform,leagueArrangesSettings.WeekDayDotNormalColor);
	end
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
	selectedDay = tonumber(m_centerObject.name);

	for i=1,7 do
		if(selectedDay == i)then
			UIHelper.SetWidgetColor(weekDayDots[i].transform,leagueArrangesSettings.WeekDayDotSelectedColor);
		else
			UIHelper.SetWidgetColor(weekDayDots[i].transform,leagueArrangesSettings.WeekDayDotNormalColor);
		end
	end
end

function CloseLeagueArrangementUI()
	if(leagueArrangementUI == nil) then
		return;
	end
	if(GameObjectActiveSelf(leagueArrangementUI)) then
    	GameObjectSetActive(leagueArrangementUI.transform,false);
    end
end

function OnDestroy()
	uiPrefabs = {
		LeagueArrangesUIName = "LeagueArrangementUI",
		LeagueArrangesUI = nil,
		LeagueDayArrangesPageName = "LeagueDayArrangesPage",
		LeagueDayArrangesPage = nil
	}
	leagueArrangementUI = nil;
	leagueDayArrangesPages = {};
	weekDayDots = {};
	uiScrollView = nil;
	uiContainer = nil;
	groupData = nil;
	groupTime = nil;
	currentGroupPlayers = {};
	sortedMatchTimeKey = {};
	delegateOnReplayMatch = nil;
end