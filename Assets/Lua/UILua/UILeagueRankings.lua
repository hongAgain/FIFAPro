module("UILeagueRankings", package.seeall)

--child module
require "Game/PVPMsgManager"
require "UILua/UILeagueRankingCalculator"



local leagueRankingSettings = {

	prefabNameLeagueRankings = "LeagueRankingsUI",
	NameColor = {
		orange = Color.New(255/255,186/255,104/255,255/255),
		white = Color.New(255/255,255/255,255/255,255/255),
		gray = Color.New(125/255,128/255,137/255,255/255),
		blue = Color.New(102/255,204/255,255/255,255/255)
	},
	BGColor = {
		gray = Color.New(204/255,204/255,241/255,12/255),
		black = Color.New(0/255,0/255,0/255,0/255),
		blue = Color.New(25/255,48/255,70/255,204/255)
	}
};

leagueRankingSettings.NameColor.__index = function (table,key)
	if( key == 0 )then
		return leagueRankingSettings.NameColor.blue;
	elseif( key == 1 )then	
		return leagueRankingSettings.NameColor.orange;
	elseif( key > 1 and key < 5 )then
		return leagueRankingSettings.NameColor.white;
	elseif( key > 4 and key < 9 )then
		return leagueRankingSettings.NameColor.gray;
	end
end
setmetatable(leagueRankingSettings.NameColor,leagueRankingSettings.NameColor);

leagueRankingSettings.BGColor.__index = function (table,key)
	if( key == 0 )then
		return leagueRankingSettings.BGColor.blue;
	elseif( key % 2 == 0 )then
		return leagueRankingSettings.BGColor.gray;
	else
		return leagueRankingSettings.BGColor.black;
	end
end
setmetatable(leagueRankingSettings.BGColor,leagueRankingSettings.BGColor);



local leagueRankingsUI = nil;
local rankingItems = {};
local pointRankData = nil;


function CreateLeagueRankingsUI( containerTransform, windowComponent )
	if(leagueRankingsUI == nil) then
		--instantiate one
		local leagueRankingsUIPrefab = windowComponent:GetPrefab(leagueRankingSettings.prefabNameLeagueRankings);
		--instantiate prefab and initialize it
		leagueRankingsUI = GameObjectInstantiate(leagueRankingsUIPrefab);
		leagueRankingsUI.transform.parent = containerTransform;
    	leagueRankingsUI.transform.localPosition = Vector3.zero;
    	leagueRankingsUI.transform.localScale = Vector3.one;
 		BindUI();
	end
	--active it
	if(not GameObjectActiveSelf(leagueRankingsUI)) then
    	GameObjectSetActive(leagueRankingsUI.transform,true);
    end
	--set info
	SetInfo();
end

function RefreshLeagueRankingsUI()
	if(leagueRankingsUI == nil) then
		return;
	end
	if(not GameObjectActiveSelf(leagueRankingsUI)) then
    	return;
    end
    RefreshTaskUI();
end

function BindUI()

	rankingItems = {}
	for i=1,8 do
		rankingItems[i] = {};
		rankingItems[i].transform = TransformFindChild(leagueRankingsUI.transform,"RankItem"..i);

		rankingItems[i].Rank = TransformFindChild(rankingItems[i].transform,"Rank");
		rankingItems[i].Team = TransformFindChild(rankingItems[i].transform,"Team");
		rankingItems[i].Num = TransformFindChild(rankingItems[i].transform,"Num");
		rankingItems[i].Point = TransformFindChild(rankingItems[i].transform,"Point");
		rankingItems[i].Win = TransformFindChild(rankingItems[i].transform,"Win");
		rankingItems[i].Tie = TransformFindChild(rankingItems[i].transform,"Tie");
		rankingItems[i].Lose = TransformFindChild(rankingItems[i].transform,"Lose");
		rankingItems[i].Goal = TransformFindChild(rankingItems[i].transform,"Goal");
		rankingItems[i].Conceded = TransformFindChild(rankingItems[i].transform,"Conceded");
		rankingItems[i].GoalDifference = TransformFindChild(rankingItems[i].transform,"GoalDifference");
		rankingItems[i].ItemBG = TransformFindChild(rankingItems[i].transform,"ItemBG");
	end
	-- UIHelper.SetPanelDepth(uiScrollView,depth+1);
end

function SetInfo()
	-- if(pointRankData == nil) then
		pointRankData = UILeagueRankingCalculator.CalculateData();
	-- end

	if(pointRankData == nil)then
		for i=1,8 do
			UIHelper.SetLabelTxt(rankingItems[i].Rank,i);
			UIHelper.SetLabelTxt(rankingItems[i].Team,"");
			UIHelper.SetLabelTxt(rankingItems[i].Num,"");
			UIHelper.SetLabelTxt(rankingItems[i].Point,"");
			UIHelper.SetLabelTxt(rankingItems[i].Win,"");
			UIHelper.SetLabelTxt(rankingItems[i].Tie,"");
			UIHelper.SetLabelTxt(rankingItems[i].Lose,"");
			UIHelper.SetLabelTxt(rankingItems[i].Goal,"");
			UIHelper.SetLabelTxt(rankingItems[i].Conceded,"");
			UIHelper.SetLabelTxt(rankingItems[i].GoalDifference,"");				
			UIHelper.SetWidgetColor(rankingItems[i].Team,leagueRankingSettings.NameColor[i]);
			UIHelper.SetWidgetColor(rankingItems[i].ItemBG,leagueRankingSettings.BGColor[i]);			
		end
	else
		for i=1,8 do
			UIHelper.SetLabelTxt(rankingItems[i].Rank,i);
			UIHelper.SetLabelTxt(rankingItems[i].Team,pointRankData[i].name);
			UIHelper.SetLabelTxt(rankingItems[i].Num,pointRankData[i].num);
			UIHelper.SetLabelTxt(rankingItems[i].Point,pointRankData[i].point);
			UIHelper.SetLabelTxt(rankingItems[i].Win,pointRankData[i].win);
			UIHelper.SetLabelTxt(rankingItems[i].Tie,pointRankData[i].tie);
			UIHelper.SetLabelTxt(rankingItems[i].Lose,pointRankData[i].lose);
			UIHelper.SetLabelTxt(rankingItems[i].Goal,pointRankData[i].goal);
			UIHelper.SetLabelTxt(rankingItems[i].Conceded,pointRankData[i].conceded);
			UIHelper.SetLabelTxt(rankingItems[i].GoalDifference,pointRankData[i].goalDiff);

			if( pointRankData[i].uid ~= nil and pointRankData[i].uid == Role.Get_uid())then
				UIHelper.SetWidgetColor(rankingItems[i].Team,leagueRankingSettings.NameColor[0]);
				UIHelper.SetWidgetColor(rankingItems[i].ItemBG,leagueRankingSettings.BGColor[0]);
			else
				UIHelper.SetWidgetColor(rankingItems[i].Team,leagueRankingSettings.NameColor[i]);
				UIHelper.SetWidgetColor(rankingItems[i].ItemBG,leagueRankingSettings.BGColor[i]);
			end		
		end
	end	
end

function RefreshTaskUI()
	SetInfo();
end

function CloseLeagueRankingsUI()
	if (leagueRankingsUI == nil) then
		return;
	end
	if (GameObjectActiveSelf(leagueRankingsUI)) then
    	GameObjectSetActive(leagueRankingsUI.transform,false);
    end
end

function OnDestroy()	
	leagueRankingsUI = nil;
	rankingItems = {};
	pointRankData = nil;
end