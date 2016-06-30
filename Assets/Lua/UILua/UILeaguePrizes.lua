module("UILeaguePrizes",package.seeall);

require "Config"
require "PVPMsgManager"
require "UILeagueMatchScript"

local leaguePrizesUI = nil;

-- local titleLeagueName = nil;
-- local titleLeagueRank = nil;

local buttonClose = nil;
local buttonGetAll = nil;

local leaguePrizeList = nil;
local leaguePrizeContainer = nil;
local leaguePrizeDrag = nil;
local leaguePrizeItems = {};

local availablePrizeList = nil;
local availablePrizeContainer = nil;
local availablePrizeDrag = nil;
local availablePrizeItems = {};

local leaguePrizeSettings = {
	LeaguePrizesPrefabName = "LeagueMatchPrizes",
	LeaguePrizesPrefab = nil,
	LeagueLevelPrizeItemPrefabName = "LeagueLevelPrizeItem",
	LeagueLevelPrizeItemPrefab = nil,

	LeagueDailyPrizeItemPrefabName = "LeagueDailyPrizeItem",
	LeagueDailyPrizeItemPrefab = nil,
	LeagueSeasonPrizeItemPrefabName = "LeagueSeasonPrizeItem",
	LeagueSeasonPrizeItemPrefab = nil


}

local leagueLevelPrizes = {};

function CreatePrizeUI(containerTransform, windowcomponent, depth)
	if(leaguePrizesUI == nil) then
		--Get prefabs
		-- print("load prefab");
		leaguePrizeSettings.LeaguePrizesPrefab = windowcomponent:GetPrefab(leaguePrizeSettings.LeaguePrizesPrefabName);
		leaguePrizeSettings.LeagueLevelPrizeItemPrefab = windowcomponent:GetPrefab(leaguePrizeSettings.LeagueLevelPrizeItemPrefabName);
		leaguePrizeSettings.LeagueDailyPrizeItemPrefab = windowcomponent:GetPrefab(leaguePrizeSettings.LeagueDailyPrizeItemPrefabName);
		leaguePrizeSettings.LeagueSeasonPrizeItemPrefab = windowcomponent:GetPrefab(leaguePrizeSettings.LeagueSeasonPrizeItemPrefabName);

		-- print("load prefab");
		--generate ui and initialize it
		leaguePrizesUI = GameObjectInstantiate(leaguePrizeSettings.LeaguePrizesPrefab);
		leaguePrizesUI.transform.parent = containerTransform;
    	leaguePrizesUI.transform.localPosition = Vector3.zero;
    	leaguePrizesUI.transform.localScale = Vector3.one; 	

		--bind ui
		BindUI(depth);
	end

	--active it
	if(not GameObjectActiveSelf(leaguePrizesUI)) then
    	GameObjectSetActive(leaguePrizesUI,true);
    end

	SetInfo();
end

function BindUI(depth)
	
		-- print("load prefab");
	-- titleLeagueName = TransformFindChild(leaguePrizesUI.transform,"Title/LeagueName");
	-- titleLeagueRank = TransformFindChild(leaguePrizesUI.transform,"Title/LeagueRank");
	
	buttonClose = TransformFindChild(leaguePrizesUI.transform,"CloseButton");
	buttonGetAll = TransformFindChild(leaguePrizesUI.transform,"GetAllButton");

	AddOrChangeClickParameters(buttonClose.gameObject,OnClickClose,nil);
	AddOrChangeClickParameters(buttonGetAll.gameObject,OnClickGetAllPrize,nil);

	leaguePrizeList = TransformFindChild(leaguePrizesUI.transform,"LeaguePrizeList");
	leaguePrizeContainer = TransformFindChild(leaguePrizeList,"Container");
	leaguePrizeDrag = TransformFindChild(leaguePrizesUI.transform,"DragLeaguePrizeList");
	UIHelper.SetPanelDepth(leaguePrizeList,depth+1);
	UIHelper.SetDragScrollViewTarget(leaguePrizeDrag,leaguePrizeList);	
	leaguePrizeItems = {};

	availablePrizeList = TransformFindChild(leaguePrizesUI.transform,"AvailablePrizeList");
	availablePrizeContainer = TransformFindChild(leaguePrizeList,"Container");
	availablePrizeDrag = TransformFindChild(leaguePrizesUI.transform,"DragAvailablePrizeList");
	UIHelper.SetPanelDepth(availablePrizeList,depth+1);
	UIHelper.SetDragScrollViewTarget(availablePrizeDrag,availablePrizeList);
	availablePrizeItems = {};


end

function SetInfo()
	-- body

	--load level data from config 

	--
	local fakeprizedata = {
		{
			titleName = "S",
			winNum = "999",
			tieNum = "999",
			loseNum = "999",
			itemID1 = "10001",
			itemID2 = "10001",
			itemID3 = "10001",
		},
		{
			titleName = "A",
			winNum = "888",
			tieNum = "888",
			loseNum = "888",
			itemID1 = "10001",
			itemID2 = "10001",
			itemID3 = "10001",
		},
		{
			titleName = "B",
			winNum = "777",
			tieNum = "777",
			loseNum = "777",
			itemID1 = "10001",
			itemID2 = "10001",
			itemID3 = "10001",
		},
		{
			titleName = "C",
			winNum = "666",
			tieNum = "666",
			loseNum = "666",
			itemID1 = "10001",
			itemID2 = "10001",
			itemID3 = "10001",
		}
	}
	for i,v in ipairs(fakeprizedata) do
		--generate ui and initialize it
		leagueLevelPrizes[i] = {};
		leagueLevelPrizes[i].gameObject = GameObjectInstantiate(leaguePrizeSettings.LeagueLevelPrizeItemPrefab);
		leagueLevelPrizes[i].transform = leagueLevelPrizes[i].gameObject.transform;

		leagueLevelPrizes[i].transform.parent = leaguePrizeContainer;
		leagueLevelPrizes[i].transform.name = string.format("%03d",i);
    	leagueLevelPrizes[i].transform.localScale = Vector3.one;

    	leagueLevelPrizes[i].title = TransformFindChild(leagueLevelPrizes[i].transform,"LeagueLevel");
    	leagueLevelPrizes[i].winNum = TransformFindChild(leagueLevelPrizes[i].transform,"Win/Num");
    	leagueLevelPrizes[i].tieNum = TransformFindChild(leagueLevelPrizes[i].transform,"Tie/Num");
    	leagueLevelPrizes[i].loseNum = TransformFindChild(leagueLevelPrizes[i].transform,"Loss/Num");

    	leagueLevelPrizes[i].icon1 = TransformFindChild(leagueLevelPrizes[i].transform,"IconRoot1/Sprite");
    	leagueLevelPrizes[i].icon2 = TransformFindChild(leagueLevelPrizes[i].transform,"IconRoot2/Sprite");
    	leagueLevelPrizes[i].icon3 = TransformFindChild(leagueLevelPrizes[i].transform,"IconRoot3/Sprite");

    	UIHelper.SetLabelTxt(leagueLevelPrizes[i].title,v.titleName);
    	UIHelper.SetLabelTxt(leagueLevelPrizes[i].winNum,v.winNum);
    	UIHelper.SetLabelTxt(leagueLevelPrizes[i].tieNum,v.tieNum);
    	UIHelper.SetLabelTxt(leagueLevelPrizes[i].loseNum,v.loseNum);

    	-- UIHelper.SetSpriteName(leagueLevelPrizes[i].icon1,v.itemID1);
    	-- UIHelper.SetSpriteName(leagueLevelPrizes[i].icon2,v.itemID2);
    	-- UIHelper.SetSpriteName(leagueLevelPrizes[i].icon3,v.itemID3);
    	-- Util.SetUITexture(leagueLevelPrizes[i].icon1,LuaConst.Const.ItemIcon,v.itemID1, true);
    	-- Util.SetUITexture(leagueLevelPrizes[i].icon2,LuaConst.Const.ItemIcon,v.itemID2, true);
    	-- Util.SetUITexture(leagueLevelPrizes[i].icon3,LuaConst.Const.ItemIcon,v.itemID3, true);
	end



end

function OnClickGetAllPrize()
	-- body
end

function OnClickClose()
	if (leaguePrizesUI == nil) then
		return;
	end
	if (GameObjectActiveSelf(leaguePrizesUI)) then
    	GameObjectSetActive(leaguePrizesUI.transform,false);
    end
end

function OnDestroy()
	leaguePrizesUI = nil;
	-- titleLeagueName = nil;
	-- titleLeagueRank = nil;
	buttonClose = nil;
	buttonGetAll = nil;
	leaguePrizeList = nil;
	leaguePrizeContainer = nil;
	leaguePrizeDrag = nil;
	leaguePrizeItems = {};
	availablePrizeList = nil;
	availablePrizeContainer = nil;
	availablePrizeDrag = nil;
	availablePrizeItems = {};
	leaguePrizeSettings = {
		LeaguePrizesPrefabName = "LeagueMatchPrizes",
		LeaguePrizesPrefab = nil,
		LeagueLevelPrizeItemPrefabName = "LeagueLevelPrizeItem",
		LeagueLevelPrizeItemPrefab = nil,

		LeagueDailyPrizeItemPrefabName = "LeagueDailyPrizeItem",
		LeagueDailyPrizeItemPrefab = nil,
		LeagueSeasonPrizeItemPrefabName = "LeagueSeasonPrizeItem",
		LeagueSeasonPrizeItemPrefab = nil
	}

	leagueLevelPrizes = {};
end