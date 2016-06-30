module("UILadderRankingsInfoManager", package.seeall)

require "Common/UnityCommonScript"
require "Game/PVPMsgManager"

local ladderRankingsSettings = {
	LadderRankingsInfoUIPrefabName = "LadderRankingsInfo",
	LadderRankingsInfoUIPrefab = nil,

	itemPrefabName = "LadderRankingsItem",
	itemPrefab = nil,

	strSeasonName = "SeasonName",
	strSeasonDuration = "SeasonDuration",

	itemColorFirst = Color.New(255/255,171/255,28/255,255/255),
	itemColorOther = Color.New(171/255,173/255,185/255,255/255),
	bgColorOdd = Color.New(28/255,28/255,33/255,153/255),
	bgColorEven = Color.New(28/255,28/255,33/255,0/255),
	itemIconFirst = "Test_Grade_6",
	itemIconSecond = "Test_Grade_4",
	itemIconOther = "Test_Grade_1",

	defaultNum = 20,

}

local ladderRankingsUI = nil;

local title = nil;
local duration = nil;

local rankingsItems = {};

local uiScrollView = nil;
local uiContainer = nil;


function CreateRankingsInfo( containerTransform, windowcomponent )
	-- --local rankingsData = PVPMsgManager.Get_LadderSortData();
	-- local rankingsData = PVPMsgManager.GetPVPData(MsgID.tb.LadderSort);
	-- if(rankingsData == nil or rankingsData == {})then

		local AfterRequest = function()
			RealInit(containerTransform, windowcomponent);
		end

		-- PVPMsgManager.RequestLadderSort({size = ladderRankingsSettings.defaultNum},AfterRequest, AfterRequest);
		PVPMsgManager.RequestPVPMsg(MsgID.tb.LadderSort, LuaConst.Const.LadderSort, {size = ladderRankingsSettings.defaultNum}, AfterRequest, AfterRequest );
	-- else
	-- 	RealInit(containerTransform, windowcomponent);
	-- end
end

function RealInit(containerTransform, windowcomponent)
	if(ladderRankingsUI == nil) then
		--Get prefabs
		ladderRankingsSettings.LadderRankingsInfoUIPrefab = windowcomponent:GetPrefab(ladderRankingsSettings.LadderRankingsInfoUIPrefabName);
		ladderRankingsSettings.itemPrefab = windowcomponent:GetPrefab(ladderRankingsSettings.itemPrefabName);
		
		--generate ui and initialize it
		ladderRankingsUI = GameObjectInstantiate(ladderRankingsSettings.LadderRankingsInfoUIPrefab);
		ladderRankingsUI.transform.parent = containerTransform;
    	ladderRankingsUI.transform.localPosition = Vector3.zero;
    	ladderRankingsUI.transform.localScale = Vector3.one;

		BindUI(UIHelper.GetPanelDepth(containerTransform)+1);
	end

	--active it
	if(not GameObjectActiveSelf(ladderRankingsUI)) then
    	GameObjectSetActive(ladderRankingsUI.transform,true);
    end

	SetInfo();
end

function RefreshInfo()
	if(ladderRankingsUI ~= nil) then
		local AfterRequest = function()
			RealInit(containerTransform, windowcomponent);
		end
		-- PVPMsgManager.RequestLadderSort({size = ladderRankingsSettings.defaultNum},SetInfo, nil);
		PVPMsgManager.RequestPVPMsg(MsgID.tb.LadderSort, LuaConst.Const.LadderSort, {size = ladderRankingsSettings.defaultNum}, AfterRequest, AfterRequest );
	end
end

function BindUI(depth)
	title = TransformFindChild(ladderRankingsUI.transform,"Title");
	duration = TransformFindChild(ladderRankingsUI.transform,"Duration");
	uiScrollView = TransformFindChild(ladderRankingsUI.transform,"uiScrollView");
	uiContainer = TransformFindChild(uiScrollView,"uiContainer");
	UIHelper.SetDragScrollViewTarget(TransformFindChild(ladderRankingsUI.transform,"DragScrollView"),uiScrollView);
	UIHelper.SetPanelDepth(uiScrollView,depth);
end

function SetInfo()
	local infoData = PVPMsgManager.GetPVPData(MsgID.tb.LadderInfo);
	local localData = Config.GetTemplate(Config.LadderLevel());

	--datas used to calculate
	local currentSeason = infoData.serv.sign;
	local daysInPerSeason = infoData.serv.cycle;
	local currentDayInThisSeason = infoData.serv.days;
	local isCurrentSeasonAvailable = (infoData.serv.start == 1);
	local daysBeforeSeasonStart = (currentSeason-1)*daysInPerSeason;
	local daysBeforeSeasonFinish = (currentSeason)*daysInPerSeason-1;

	-- print("debug -------- DataSystemScript.GetRegionId() "..DataSystemScript.GetRegionId());

	local serverStartTimeStamp = Login.GetSrvInfo(DataSystemScript.GetRegionId()).time;
	--datas needed to be shown
	local seasonStartYear = 		Util.GetYearAfterAddDay(serverStartTimeStamp,daysBeforeSeasonStart);
	local seasonStartMonth = 		Util.GetMonthAfterAddDay(serverStartTimeStamp,daysBeforeSeasonStart);
	local seasonStartDayOfMonth = 	Util.GetDayInMonthAfterAddDay(serverStartTimeStamp,daysBeforeSeasonStart);
	local seasonStartDayOfYear = 	Util.GetDayInYearAfterAddDay(serverStartTimeStamp,daysBeforeSeasonStart);
	local seasonFinishMonth = 		Util.GetMonthAfterAddDay(serverStartTimeStamp,daysBeforeSeasonFinish);
	local seasonFinishDayOfMonth = 	Util.GetDayInMonthAfterAddDay(serverStartTimeStamp,daysBeforeSeasonFinish);
	local seasonDaysLeft = daysInPerSeason - currentDayInThisSeason;
	seasonIndexThisYear = math.floor((seasonStartDayOfYear-1)/daysInPerSeason) + 1;

	UIHelper.SetLabelTxt(title,GetLocalizedString(ladderRankingsSettings.strSeasonName,seasonStartYear,seasonIndexThisYear));
	UIHelper.SetLabelTxt(duration,
					GetLocalizedString(ladderRankingsSettings.strSeasonDuration,
						seasonStartMonth,seasonStartDayOfMonth,
						seasonFinishMonth,seasonFinishDayOfMonth,seasonDaysLeft));


	-- local rankingsData = PVPMsgManager.Get_LadderSortData();
	local rankingsData = PVPMsgManager.GetPVPData(MsgID.tb.LadderSort);
	if(rankingsData == nil or IsTableEmpty(rankingsData))then
		
	else		
		--destroy old items
		DestroyUIListItemGameObjects(rankingsItems)

		local rankingListItemDatas = {};
	    --resort the dictionary
	    for k,v in pairs(rankingsData) do
	    	rankingListItemDatas[v.sort+1] = v;
	    end

		--generate new items
		for i,v in ipairs(rankingListItemDatas) do

			local clone = GameObjectInstantiate(ladderRankingsSettings.itemPrefab);
	        clone.transform.parent = uiContainer;
	        clone.transform.name = string.format("%03d",i);
	        clone.transform.localPosition = Vector3.zero;
	        clone.transform.localScale = Vector3.one;

	        rankingsItems[i] = {};
	        rankingsItems[i].gameObject = clone;
			rankingsItems[i].transform = clone.transform
			rankingsItems[i].BG = TransformFindChild(rankingsItems[i].transform,"BG");
			rankingsItems[i].Rankings = TransformFindChild(rankingsItems[i].transform,"Rankings");
			rankingsItems[i].Name = TransformFindChild(rankingsItems[i].transform,"Name");
			rankingsItems[i].Rank = TransformFindChild(rankingsItems[i].transform,"Rank");
			rankingsItems[i].Point = TransformFindChild(rankingsItems[i].transform,"Point");
			rankingsItems[i].PointTitle = TransformFindChild(rankingsItems[i].transform,"Point/Label");
			rankingsItems[i].Icon = TransformFindChild(rankingsItems[i].transform,"IconRoot/Icon");

			--set info
			UIHelper.SetLabelTxt(rankingsItems[i].Rankings,i);
	        UIHelper.SetLabelTxt(rankingsItems[i].Name,v.name);
	        local RealPoint = 0;
	        local RealRank = nil;
	        local RealIconName = nil;
	        for k,r in pairs(localData) do
	        	if(r.minScore <= v.val and v.val <= r.maxScore)then
	        		RealPoint = v.val-r.minScore;
	        		RealRank = r.name;
	        		RealIconName = r.cupIconName;
	        		break;
	        	end
	        end
			UIHelper.SetLabelTxt(rankingsItems[i].Point,RealPoint);
	        UIHelper.SetLabelTxt(rankingsItems[i].Rank,RealRank);
		
			--set style
			if(i%2 == 1)then
				UIHelper.SetWidgetColor(rankingsItems[i].BG,ladderRankingsSettings.bgColorOdd);				
			else
				UIHelper.SetWidgetColor(rankingsItems[i].BG,ladderRankingsSettings.bgColorEven);			
			end
			
			if(i==1)then
				UIHelper.SetWidgetColor(rankingsItems[i].Name,ladderRankingsSettings.itemColorFirst);
				UIHelper.SetWidgetColor(rankingsItems[i].Rankings,ladderRankingsSettings.itemColorFirst);
				UIHelper.SetWidgetColor(rankingsItems[i].Rank,ladderRankingsSettings.itemColorFirst);
				UIHelper.SetWidgetColor(rankingsItems[i].Point,ladderRankingsSettings.itemColorFirst);
				UIHelper.SetWidgetColor(rankingsItems[i].PointTitle,ladderRankingsSettings.itemColorFirst);				
			else				
				UIHelper.SetWidgetColor(rankingsItems[i].Name,ladderRankingsSettings.itemColorOther);
				UIHelper.SetWidgetColor(rankingsItems[i].Rankings,ladderRankingsSettings.itemColorOther);
				UIHelper.SetWidgetColor(rankingsItems[i].Rank,ladderRankingsSettings.itemColorOther);
				UIHelper.SetWidgetColor(rankingsItems[i].Point,ladderRankingsSettings.itemColorOther);
				UIHelper.SetWidgetColor(rankingsItems[i].PointTitle,ladderRankingsSettings.itemColorOther);				
			end
			Util.SetUITexture(rankingsItems[i].Icon,
				LuaConst.Const.CupLadder,
				RealIconName,
				true);
		end

		-- UIHelper.SetGridPosition(
		-- 	uiContainer,
		-- 	uiScrollView,
		-- 	NewVector3(283,-82.9,0),
		-- 	NewVector3(0,206.3,0),
		-- 	true);
		-- UIHelper.RefreshPanel(uiScrollView);
		UIHelper.RepositionGrid(uiContainer,uiScrollView);
    	UIHelper.RefreshPanel(uiScrollView);
	end
end

function OnDestroy()
	ladderRankingsUI = nil;
	title = nil;
	duration = nil;
	rankingsItems = {};
	uiScrollView = nil;
	uiContainer = nil;
end