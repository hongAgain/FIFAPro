module("UILadderRankInfoManager", package.seeall)

require "Common/UnityCommonScript"
require "Game/PVPMsgManager"

local ladderRankInfoSettings = {
	LeagueRankInfoUIName = "LadderRankInfo",
	LeagueRankInfoUIPrefab = nil,

	RankInfoNodePrefabName = "RankInfoNode",
	RankInfoNodePrefab = nil,
	RankQualifierNodePrefabName = "RankQualifierNode",
	RankQualifierNodePrefab = nil,
	RankRankingsNodePrefabName = "RankRankingsNode",
	RankRankingsNodePrefab = nil,

	strWinAndLose = "WinAndLose",
	strMatchHolding = "MatchHolding",
	strQualifierTitle = "Qualifier";
	strRelegationTitle = "Relegation";
	
	strWin = "Win",
	strTie = "Tie",
	strLoss = "Loss",

	WinColor = Color.New(159/255,255/255,159/255,255/255),
	TieColor = Color.New(255/255,255/255,255/255,255/255),
	LoseColor = Color.New(255/255,102/255,102/255,255/255),

	DotColorActive = Color.New(2/255,136/255,207/255,255/255),
	DotColorDisActive = Color.New(33/255,35/255,37/255,255/255),

	RankNameColorActive = Color.New(255/255,222/255,90/255,255/255),
	RankNameColorDisactive = Color.New(171/255,173/255,185/255,255/255),
	RankBGColorActive = Color.New(188/255,124/255,37/255,102/255),
	RankBGColorDisactive = Color.New(255/255,255/255,255/255,51/255),

	ChildRankNameColorActive = Color.New(49/255,44/255,36/255,255/255),
	ChildRankNameColorDisactive = Color.New(147/255,147/255,147/255,255/255),
	ChildRankBGColorActive = Color.New(255/255,208/255,42/255,255/255),
	ChildRankBGColorDisactive = Color.New(52/255,45/255,37/255,255/255)
}

local leagueRankInfoUI = nil;
local uiScrollView = nil;
local uiContainer = nil;
local infoListItems = {};
local dots = {};

local rankInfoNode = {
	transform = nil,
	RankName = nil,
	RankPoint = nil,
	RankWinningPercent = nil,
	RankRankings = nil,
	RankRecord = nil,
	RankIcon = nil
};
local rankRankingsNode = {
	transform = nil,
	Ranks = {},
	SelectedNode = nil,
	RankLevel = {}
};
local RankQualifierNode = {
	transform = nil,
	Title = nil,
	Description = nil,
	CurrentRank = nil,
	TargetRank = nil,
	Results = nil
};



function CreateRankInfo( containerTransform, windowcomponent, defaultIndex, defaultRankLevel )
	if(leagueRankInfoUI == nil) then
		--Get prefabs
		ladderRankInfoSettings.LeagueRankInfoUIPrefab = windowcomponent:GetPrefab(ladderRankInfoSettings.LeagueRankInfoUIName);
		
		ladderRankInfoSettings.RankInfoNodePrefab = windowcomponent:GetPrefab(ladderRankInfoSettings.RankInfoNodePrefabName);
		ladderRankInfoSettings.RankQualifierNodePrefab = windowcomponent:GetPrefab(ladderRankInfoSettings.RankQualifierNodePrefabName);
		ladderRankInfoSettings.RankRankingsNodePrefab = windowcomponent:GetPrefab(ladderRankInfoSettings.RankRankingsNodePrefabName);

		--generate ui and initialize it
		leagueRankInfoUI = GameObjectInstantiate(ladderRankInfoSettings.LeagueRankInfoUIPrefab);
		leagueRankInfoUI.transform.parent = containerTransform;
    	leagueRankInfoUI.transform.localPosition = Vector3.zero;
    	leagueRankInfoUI.transform.localScale = Vector3.one;

		-- BindUI(depth);
		BindUI(UIHelper.GetPanelDepth(containerTransform)+1);
	end

	--active it
	if(not GameObjectActiveSelf(leagueRankInfoUI)) then
    	GameObjectSetActive(leagueRankInfoUI.transform,true);
    end

	SetInfo(defaultIndex or 1, defaultRankLevel);
end

function BindUI(depth)

	uiScrollView = TransformFindChild(leagueRankInfoUI.transform,"uiScrollView");
	uiContainer = TransformFindChild(uiScrollView,"uiContainer");
	UIHelper.SetPanelDepth(uiScrollView,depth);
	UIHelper.SetDragScrollViewTarget(TransformFindChild(leagueRankInfoUI.transform,"DragScrollView"),uiScrollView);

 	UIHelper.AddDragOnStarted(uiScrollView,OnDragStarted);
	UIHelper.AddDragOnFinish(uiScrollView,OnDragFinish);

	dots = {}
	for i=1,2 do
		dots[i] = TransformFindChild(leagueRankInfoUI.transform,"Dot"..i);
	end
	
	local infoData = PVPMsgManager.GetPVPData(MsgID.tb.LadderInfo);

	--destroy old
	for k,v in pairs(infoListItems) do
		GameObjectSetActive(v.transform,false);
		GameObjectDestroy(v.gameObject);
        infoListItems[k] = nil;
	end
	infoListItems = {};
	
	--generate two element for list
	if(infoData.info.chance == 0) then
		--generate rankinfonode for the first
		infoListItems[1] = {};
		infoListItems[1].gameObject = GameObjectInstantiate(ladderRankInfoSettings.RankInfoNodePrefab);
		infoListItems[1].transform = infoListItems[1].gameObject.transform;
		infoListItems[1].transform.name = 1;
		infoListItems[1].transform.parent = uiContainer;
    	infoListItems[1].transform.localPosition = Vector3.zero;
    	infoListItems[1].transform.localScale = Vector3.one;

    	-- bind ui for rankInfoNode
		rankInfoNode = {};
		rankInfoNode.transform = infoListItems[1].transform;
		rankInfoNode.RankName = TransformFindChild(rankInfoNode.transform,"RankName");
		rankInfoNode.RankPoint = TransformFindChild(rankInfoNode.transform,"RankPoint");
		rankInfoNode.RankWinningPercent = TransformFindChild(rankInfoNode.transform,"RankWinningPercent");
		rankInfoNode.RankRankings = TransformFindChild(rankInfoNode.transform,"RankRankings");
		rankInfoNode.RankRecord = TransformFindChild(rankInfoNode.transform,"RankRecord");
		rankInfoNode.RankIcon = TransformFindChild(rankInfoNode.transform,"IconRoot/Icon");

	else
		--generate qualifiernode for the first
		infoListItems[1] = {};
		infoListItems[1].gameObject = GameObjectInstantiate(ladderRankInfoSettings.RankQualifierNodePrefab);
		infoListItems[1].transform = infoListItems[1].gameObject.transform;
		infoListItems[1].transform.name = 1;
		infoListItems[1].transform.parent = uiContainer;
    	infoListItems[1].transform.localPosition = Vector3.zero;
    	infoListItems[1].transform.localScale = Vector3.one;

    	-- bind ui for RankQualifierNode
		RankQualifierNode = {};
		RankQualifierNode.transform = infoListItems[1].transform;
		RankQualifierNode.Title = TransformFindChild(RankQualifierNode.transform,"Title");
		RankQualifierNode.Description = TransformFindChild(RankQualifierNode.transform,"Description");
		RankQualifierNode.CurrentRank = TransformFindChild(RankQualifierNode.transform,"CurrentRank");
		RankQualifierNode.TargetRank = TransformFindChild(RankQualifierNode.transform,"TargetRank");
		RankQualifierNode.Results = {};
		for i=1,3 do
			RankQualifierNode.Results[i] = {};
			RankQualifierNode.Results[i].transform = TransformFindChild(RankQualifierNode.transform,"Result"..i);
			RankQualifierNode.Results[i].Result = TransformFindChild(RankQualifierNode.Results[i].transform,"Result");
			RankQualifierNode.Results[i].Score = TransformFindChild(RankQualifierNode.Results[i].transform,"Score");
		end

	end

	--generate rankRankingsNode for the second
	infoListItems[2] = {};
	infoListItems[2].gameObject = GameObjectInstantiate(ladderRankInfoSettings.RankRankingsNodePrefab);
	infoListItems[2].transform = infoListItems[2].gameObject.transform;
	infoListItems[2].transform.name = 2;
	infoListItems[2].transform.parent = uiContainer;
	infoListItems[2].transform.localPosition = Vector3.zero;
	infoListItems[2].transform.localScale = Vector3.one;	

	-- rankRankingsNode
	rankRankingsNode = {};
	rankRankingsNode.transform = infoListItems[2].transform;
	rankRankingsNode.Ranks = {};
	for i=1,6 do
		rankRankingsNode.Ranks[i] = {};
		rankRankingsNode.Ranks[i].transform = TransformFindChild(rankRankingsNode.transform,"Rank"..i);
		rankRankingsNode.Ranks[i].IconRoot = TransformFindChild(rankRankingsNode.Ranks[i].transform,"IconRoot");
		rankRankingsNode.Ranks[i].Icon = TransformFindChild(rankRankingsNode.Ranks[i].IconRoot,"Icon");
		rankRankingsNode.Ranks[i].Name = TransformFindChild(rankRankingsNode.Ranks[i].transform,"Name");
	end

	rankRankingsNode.SelectedNode = TransformFindChild(rankRankingsNode.transform,"SelectedNode");
	rankRankingsNode.RankLevel = {};
	for i=1,3 do
		rankRankingsNode.RankLevel[i] = {};
		rankRankingsNode.RankLevel[i].transform = TransformFindChild(rankRankingsNode.SelectedNode,"RankLevel"..i);
		rankRankingsNode.RankLevel[i].Frame = TransformFindChild(rankRankingsNode.RankLevel[i].transform,"Frame");
		rankRankingsNode.RankLevel[i].Label = TransformFindChild(rankRankingsNode.RankLevel[i].transform,"Label");
	end

	--re-sort grid
 	-- UIHelper.RepositionGrid(uiContainer,uiScrollView);
    -- UIHelper.RefreshPanel(uiScrollView);
end

function ResetUIBindings()
	--destroy old
	for k,v in pairs(infoListItems) do
		GameObjectSetActive(v.transform,false);
		GameObjectDestroy(v.gameObject);
        infoListItems[k] = nil;
	end
	infoListItems = {};
	
	local infoData = PVPMsgManager.GetPVPData(MsgID.tb.LadderInfo);
	--generate two element for list
	if(infoData.info.chance == 0) then
		--generate rankinfonode for the first
		infoListItems[1] = {};
		infoListItems[1].gameObject = GameObjectInstantiate(ladderRankInfoSettings.RankInfoNodePrefab);
		infoListItems[1].transform = infoListItems[1].gameObject.transform;
		infoListItems[1].transform.name = 1;
		infoListItems[1].transform.parent = uiContainer;
    	infoListItems[1].transform.localPosition = Vector3.zero;
    	infoListItems[1].transform.localScale = Vector3.one;

    	-- bind ui for rankInfoNode
		rankInfoNode = {};
		rankInfoNode.transform = infoListItems[1].transform;
		rankInfoNode.RankName = TransformFindChild(rankInfoNode.transform,"RankName");
		rankInfoNode.RankPoint = TransformFindChild(rankInfoNode.transform,"RankPoint");
		rankInfoNode.RankWinningPercent = TransformFindChild(rankInfoNode.transform,"RankWinningPercent");
		rankInfoNode.RankRankings = TransformFindChild(rankInfoNode.transform,"RankRankings");
		rankInfoNode.RankRecord = TransformFindChild(rankInfoNode.transform,"RankRecord");
		rankInfoNode.RankIcon = TransformFindChild(rankInfoNode.transform,"IconRoot/Icon");

	else
		--generate qualifiernode for the first
		infoListItems[1] = {};
		infoListItems[1].gameObject = GameObjectInstantiate(ladderRankInfoSettings.RankQualifierNodePrefab);
		infoListItems[1].transform = infoListItems[1].gameObject.transform;
		infoListItems[1].transform.name = 1;
		infoListItems[1].transform.parent = uiContainer;
    	infoListItems[1].transform.localPosition = Vector3.zero;
    	infoListItems[1].transform.localScale = Vector3.one;

    	-- bind ui for RankQualifierNode
		RankQualifierNode = {};
		RankQualifierNode.transform = infoListItems[1].transform;
		RankQualifierNode.Title = TransformFindChild(RankQualifierNode.transform,"Title");
		RankQualifierNode.Description = TransformFindChild(RankQualifierNode.transform,"Description");
		RankQualifierNode.CurrentRank = TransformFindChild(RankQualifierNode.transform,"CurrentRank");
		RankQualifierNode.TargetRank = TransformFindChild(RankQualifierNode.transform,"TargetRank");
		RankQualifierNode.Results = {};
		for i=1,3 do
			RankQualifierNode.Results[i] = {};
			RankQualifierNode.Results[i].transform = TransformFindChild(RankQualifierNode.transform,"Result"..i);
			RankQualifierNode.Results[i].Result = TransformFindChild(RankQualifierNode.Results[i].transform,"Result");
			RankQualifierNode.Results[i].Score = TransformFindChild(RankQualifierNode.Results[i].transform,"Score");
		end

	end

	--generate rankRankingsNode for the second
	infoListItems[2] = {};
	infoListItems[2].gameObject = GameObjectInstantiate(ladderRankInfoSettings.RankRankingsNodePrefab);
	infoListItems[2].transform = infoListItems[2].gameObject.transform;
	infoListItems[2].transform.name = 2;
	infoListItems[2].transform.parent = uiContainer;
	infoListItems[2].transform.localPosition = Vector3.zero;
	infoListItems[2].transform.localScale = Vector3.one;	

	-- rankRankingsNode
	rankRankingsNode = {};
	rankRankingsNode.transform = infoListItems[2].transform;
	rankRankingsNode.Ranks = {};
	for i=1,6 do
		rankRankingsNode.Ranks[i] = {};
		rankRankingsNode.Ranks[i].transform = TransformFindChild(rankRankingsNode.transform,"Rank"..i);
		rankRankingsNode.Ranks[i].IconRoot = TransformFindChild(rankRankingsNode.Ranks[i].transform,"IconRoot");
		rankRankingsNode.Ranks[i].Icon = TransformFindChild(rankRankingsNode.Ranks[i].IconRoot,"Icon");
		rankRankingsNode.Ranks[i].Name = TransformFindChild(rankRankingsNode.Ranks[i].transform,"Name");
	end

	rankRankingsNode.SelectedNode = TransformFindChild(rankRankingsNode.transform,"SelectedNode");
	rankRankingsNode.RankLevel = {};
	for i=1,3 do
		rankRankingsNode.RankLevel[i] = {};
		rankRankingsNode.RankLevel[i].transform = TransformFindChild(rankRankingsNode.SelectedNode,"RankLevel"..i);
		rankRankingsNode.RankLevel[i].Frame = TransformFindChild(rankRankingsNode.RankLevel[i].transform,"Frame");
		rankRankingsNode.RankLevel[i].Label = TransformFindChild(rankRankingsNode.RankLevel[i].transform,"Label");
	end

	-- re-sort grid
 	-- UIHelper.RepositionGrid(uiContainer,uiScrollView);
  --   UIHelper.RefreshPanel(uiScrollView);
end

function OnDragStarted()
	--disable center on child here
	Util.EnableScript(uiContainer.gameObject,"UICenterOnChild",false);
end

function OnDragFinish()
	--enable center on child here
	Util.EnableScript(uiContainer.gameObject,"UICenterOnChild",true);
	ChangeDots();
end

function ChangeDots()
	local centeredInfoItem = UIHelper.CenterOnRecenter(uiContainer);
	local activeDotIndex = tonumber(centeredInfoItem.transform.name);
	--set first dot active
	UIHelper.SetWidgetColor(dots[activeDotIndex],ladderRankInfoSettings.DotColorActive);
	UIHelper.SetWidgetColor(dots[3-activeDotIndex],ladderRankInfoSettings.DotColorDisActive);
end

function SetInfo(defaultIndex, defaultRankLevel)

	local infoData = PVPMsgManager.GetPVPData(MsgID.tb.LadderInfo);
	local localData = Config.GetTemplate(Config.LadderLevel());

	--set first dot active
	UIHelper.SetWidgetColor(dots[1],ladderRankInfoSettings.DotColorActive);
	UIHelper.SetWidgetColor(dots[2],ladderRankInfoSettings.DotColorDisActive);

	if(infoData.info.chance == 0) then
		--init rankinfo node		
		UIHelper.SetLabelTxt(rankInfoNode.RankName,localData[tostring(infoData.info.level)].name);
		UIHelper.SetLabelTxt(rankInfoNode.RankPoint,infoData.info.score-localData[tostring(infoData.info.level)].minScore);
		local rankings = nil;
		if(infoData.sort == nil)then
			rankings = "10000+";
		else
			rankings = tonumber(infoData.sort)+1;
		end
		UIHelper.SetLabelTxt(rankInfoNode.RankRankings,rankings);
		UIHelper.SetLabelTxt(rankInfoNode.RankRecord,GetLocalizedString(ladderRankInfoSettings.strWinAndLose,infoData.info.total[1],infoData.info.total[2],infoData.info.total[3]));
		local winRate=0;
		local totalNum = (infoData.info.total[1]+infoData.info.total[2]+infoData.info.total[3]);
		if(totalNum==0) then
			winRate = 0;
		else
			winRate = 100*infoData.info.total[1]/(infoData.info.total[1]+infoData.info.total[2]+infoData.info.total[3]);
		end
		UIHelper.SetLabelTxt(rankInfoNode.RankWinningPercent,string.format("%.1f",winRate).."%");
		Util.SetUITexture(rankInfoNode.RankIcon,
			LuaConst.Const.CupLadder,
			localData[tostring(infoData.info.level)].cupIconName,
			true);
	else
		if(infoData.info.chance == 1) then
			--init qualifier match
			UIHelper.SetLabelTxt(RankQualifierNode.Title,GetLocalizedString(ladderRankInfoSettings.strQualifierTitle));
			UIHelper.SetLabelTxt(RankQualifierNode.Description,GetLocalizedString(ladderRankInfoSettings.strMatchHolding)..GetLocalizedString(ladderRankInfoSettings.strQualifierTitle));
			UIHelper.SetLabelTxt(RankQualifierNode.CurrentRank,localData[tostring(infoData.info.level)].name);
			UIHelper.SetLabelTxt(RankQualifierNode.TargetRank,localData[tostring(infoData.info.level+1)].name);
		elseif(infoData.info.chance == -1) then
			--init relegation match
			UIHelper.SetLabelTxt(RankQualifierNode.Title,GetLocalizedString(ladderRankInfoSettings.strRelegationTitle));
			UIHelper.SetLabelTxt(RankQualifierNode.Description,GetLocalizedString(ladderRankInfoSettings.strMatchHolding)..GetLocalizedString(ladderRankInfoSettings.strRelegationTitle));
			UIHelper.SetLabelTxt(RankQualifierNode.CurrentRank,localData[tostring(infoData.info.level)].name);
			UIHelper.SetLabelTxt(RankQualifierNode.TargetRank,localData[tostring(infoData.info.level-1)].name);
		end

		local qualifierMatchNum = 0;
		local qualifierMatchSum = #infoData.info.logs;
		if(infoData.info.logs ~= nil) then
			qualifierMatchNum = math.min(3,(infoData.info.result[1]+infoData.info.result[2]),qualifierMatchSum); 
		else
		 	qualifierMatchNum = 0;
		end
		
		for i=1,3 do
			local currentIndex = qualifierMatchNum-i;
			local logIndex = qualifierMatchSum - currentIndex;			
			if(logIndex <= 0 or logIndex > qualifierMatchSum) then
				--disable result
				GameObjectSetActive(RankQualifierNode.Results[i].transform,false);
			else
				--enable result
				GameObjectSetActive(RankQualifierNode.Results[i].transform,true);

				UIHelper.SetLabelTxt(RankQualifierNode.Results[i].Score,infoData.info.logs[logIndex].goal[1]..":"..infoData.info.logs[logIndex].goal[2]);
				if(infoData.info.logs[logIndex].goal[1] > infoData.info.logs[logIndex].goal[2]) then
					--you won
					UIHelper.SetLabelTxt(RankQualifierNode.Results[i].Result,GetLocalizedString(ladderRankInfoSettings.strWin));
					UIHelper.SetWidgetColor(RankQualifierNode.Results[i].Result,ladderRankInfoSettings.WinColor);
				elseif(infoData.info.logs[logIndex].goal[1] < infoData.info.logs[logIndex].goal[2]) then
					--you lose
					UIHelper.SetLabelTxt(RankQualifierNode.Results[i].Result,GetLocalizedString(ladderRankInfoSettings.strLoss));
					UIHelper.SetWidgetColor(RankQualifierNode.Results[i].Result,ladderRankInfoSettings.LoseColor);
				else
					--tie
					UIHelper.SetLabelTxt(RankQualifierNode.Results[i].Result,GetLocalizedString(ladderRankInfoSettings.strTie));
					UIHelper.SetWidgetColor(RankQualifierNode.Results[i].Result,ladderRankInfoSettings.TieColor);
				end
			end
		end		
	end
	SetRankingsNode(infoData.info.level);

	UIHelper.SetGridPosition(
		uiContainer,
		uiScrollView,
		NewVector3(-193,14.3,0),
		NewVector3(557*(1-defaultIndex),0,0),
		false);
   	UIHelper.RefreshPanel(uiScrollView);
end

function SetRankingsNode(rankLevel)

	local localData = Config.GetTemplate(Config.LadderLevel());
	local rankIndex = math.floor(rankLevel/3)+1;
	local rankABC = rankLevel%3+1;
	for i=1,6 do		
		Util.SetUITexture(rankRankingsNode.Ranks[i].Icon,
			LuaConst.Const.CupLadder,
			localData[tostring((i-1)*3)].cupIconName,
			true);
		UIHelper.SetLabelTxt(rankRankingsNode.Ranks[i].Name,localData[tostring((i-1)*3)].type);

		if(rankIndex == i)then
			UIHelper.SetWidgetColor(rankRankingsNode.Ranks[i].Name,ladderRankInfoSettings.RankNameColorActive);
		else
			UIHelper.SetWidgetColor(rankRankingsNode.Ranks[i].Name,ladderRankInfoSettings.RankNameColorDisactive);
		end
	end
	rankRankingsNode.SelectedNode.localPosition = NewVector3( rankRankingsNode.Ranks[rankIndex].transform.localPosition.x, -35, 0 );
	if(rankABC == 6)then
		for i=1,3 do
			GameObjectSetActive(rankRankingsNode.RankLevel[i].transform,false);
		end
	else
		for i=1,3 do
			GameObjectSetActive(rankRankingsNode.RankLevel[i].transform,true);
			if(i == rankABC)then
				UIHelper.SetWidgetColor(rankRankingsNode.RankLevel[i].Frame,ladderRankInfoSettings.ChildRankBGColorActive);
				UIHelper.SetWidgetColor(rankRankingsNode.RankLevel[i].Label,ladderRankInfoSettings.ChildRankNameColorActive);
			else
				UIHelper.SetWidgetColor(rankRankingsNode.RankLevel[i].Frame,ladderRankInfoSettings.ChildRankBGColorDisactive);
				UIHelper.SetWidgetColor(rankRankingsNode.RankLevel[i].Label,ladderRankInfoSettings.ChildRankNameColorDisactive);

			end
		end
	end	
end

function SetSelectedWidget( targetRank )
	--set the selected widget
	local rankIndex = math.floor(targetRank/3)+1;
	rankRankingsNode.SelectedNode.localPosition = NewVector3( rankRankingsNode.Ranks[rankIndex].transform.localPosition.x, -35, 0 );
	
	local rankABC = targetRank%3+1;
	if(rankABC == 6)then
		for i=1,3 do
			GameObjectSetActive(rankRankingsNode.RankLevel[i].transform,false);
		end
	else
		for i=1,3 do
			GameObjectSetActive(rankRankingsNode.RankLevel[i].transform,true);
			if(i == rankABC)then
				UIHelper.SetWidgetColor(rankRankingsNode.RankLevel[i].Frame,ladderRankInfoSettings.ChildRankBGColorActive);
				UIHelper.SetWidgetColor(rankRankingsNode.RankLevel[i].Label,ladderRankInfoSettings.ChildRankNameColorActive);
			else
				UIHelper.SetWidgetColor(rankRankingsNode.RankLevel[i].Frame,ladderRankInfoSettings.ChildRankBGColorDisactive);
				UIHelper.SetWidgetColor(rankRankingsNode.RankLevel[i].Label,ladderRankInfoSettings.ChildRankNameColorDisactive);

			end
		end
	end	
end

function FadeOutRankSelected( targetRank, onOver )
	SetSelectedWidget( targetRank );
	--fade out the selected widget
	UIHelper.FadeUIWidgetColor(
		rankRankingsNode.SelectedNode,
		Color.New(1,1,1,1),
		Color.New(1,1,1,0),
		0.3,
		onOver);
end

function FadeOutRankMedal(targetRank, onOver)
	--fade out the medal
	UIHelper.FadeUIWidgetColor(
		rankRankingsNode.Ranks[targetRank].Icon,
		Color.New(1,1,1,1),
		Color.New(1,1,1,0),
		1,
		onOver);
end

function FadeInRankMedal( targetRank, onOver  )
	SetSelectedWidget( targetRank );

	local timeOfThisPeriod = 0.3;
	--fade in the selected widget
	UIHelper.FadeUIWidgetColor(
		rankRankingsNode.SelectedNode,
		Color.New(1,1,1,0),
		Color.New(1,1,1,1),
		timeOfThisPeriod,
		onOver);

	--fade in the medal
	UIHelper.FadeUIWidgetColor(
		rankRankingsNode.Ranks[targetRank].Icon,
		Color.New(1,1,1,0),
		Color.New(1,1,1,1),
		timeOfThisPeriod,
		nil);

	--move down the medal, y : 80-->0
	rankRankingsNode.Ranks[targetRank].IconRoot.localPosition = NewVector3(0,80,0);
	UIHelper.TweenPositionBegin(rankRankingsNode.Ranks[targetRank].IconRoot,timeOfThisPeriod,NewVector3(0,80,0));
end

function RefreshInfo(defaultIndex, defaultRankLevel)
	if(leagueRankInfoUI ~= nil) then
		ResetUIBindings();
		SetInfo(defaultIndex or 1, defaultRankLevel);
	end
end

function OnDestroy()
	-- body
	leagueRankInfoUI = nil;
	uiScrollView = nil;
	uiContainer = nil;
	infoListItems = {};
	dots = {};
	rankInfoNode = {};
	rankRankingsNode = {};
	RankQualifierNode = {};
end