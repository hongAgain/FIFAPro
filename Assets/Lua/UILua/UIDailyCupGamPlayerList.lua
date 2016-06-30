module("UIDailyCupGamPlayerList",package.seeall);

require "Config"
require "Common/UnityCommonScript"
require "Game/GameMainScript"
require "Game/Role"
require "Game/ItemSys"
require "Game/Hero"
require "Game/HeroData"

--require "UILua/UIDailyCupScript"
require "UILua/UICircleListManager"

gamPListSettings = {
	dotActive = "Common_dot_Selected",
	dotDisactive = "Common_dot_bg",
	enumPlayerType = {All=1,Forward=2,Midfielder=3,Defender=4,Goalkeeper=5},

	enumPlayerName = {
		"UIPlayerList_FilterType1",
		"UIPlayerList_FilterType2",
		"UIPlayerList_FilterType3",
		"UIPlayerList_FilterType4",
		"UIPlayerList_FilterType5"
	}
}

local playerItemPrefabName = "DailyCupGamPlayerItem";
local playerItemPrefab = nil;

local uiScrollView = nil;
local uiContainer = nil;
local gamPlayers = {};

local uiCurrentPlayer = {item = nil,data = nil};

local strBGMusic = "";
local window = nil;
local windowComponent = nil;

local delegateOnSelectPlayer = nil;

local circleListManager = nil;
local filterButton = {
	buttonContent = nil
};

function OnStart(gameObject, params)
    print("UIDailyCupGamPlayerList.lua .. OnStart");
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");

    delegateOnSelectPlayer = params.delegateOnSelectPlayer;
    BindUI();
    SetInfo();
    AudioMgr.Instance():PlayBGMusic(strBGMusic);
end

function BindUI()
	local transform = window.transform;

	uiScrollView = TransformFindChild(transform,"uiScrollView");
	uiContainer = TransformFindChild(uiScrollView,"uiContainer");

	filterButton = {};
	filterButton.transform = TransformFindChild(transform,"ButtonSelectFilter");
	filterButton.gameObject = filterButton.transform.gameObject;
	filterButton.buttonContent = TransformFindChild(filterButton.transform,"buttonContent");
	AddOrChangeClickParameters(filterButton.gameObject,OnClickShowFilterBox,nil);	
end

function SetInfo()
	CreatePlayerList(gamPListSettings.enumPlayerType.All);
    UIHelper.SetLabelTxt(filterButton.buttonContent,GetLocalizedString(gamPListSettings.enumPlayerName[gamPListSettings.enumPlayerType.All]));
end

function CreatePlayerList(playerEnumType)
	local playerData = FilterPlayers(playerEnumType);
	--destroy old
	DestroyUIListItemGameObjects(gamPlayers);

	if(playerItemPrefab == nil)then
		playerItemPrefab = windowComponent:GetPrefab(playerItemPrefabName);
	end
	gamPlayers = {};
	--create new
	CreateUIListItemGameObjects(uiContainer, playerData, playerItemPrefab, OnInitListItem)

	UIHelper.RepositionGrid(uiContainer,uiScrollView);
    UIHelper.RefreshPanel(uiScrollView);
end

function FilterPlayers(playerEnumType)
	
	if(playerEnumType == gamPListSettings.enumPlayerType.All)then
		return Hero.GetHeroList();
	end

	local finalPlayerData = {};
	for k,v in pairs(Hero.GetHeroList()) do
		local pos = tonumber(Config.GetProperty(Config.HeroTable(),v.id,"ipos"));
		if(playerEnumType == gamPListSettings.enumPlayerType.Forward)then
			if pos >= 1 and pos <= 9 then
	            table.insert(finalPlayerData,v);
            end
		elseif(playerEnumType == gamPListSettings.enumPlayerType.Midfielder)then
			if pos >= 10 and pos <= 20 then
	            table.insert(finalPlayerData,v);
	        end
		elseif(playerEnumType == gamPListSettings.enumPlayerType.Defender)then
			if pos >= 21 and pos <= 28 then
	            table.insert(finalPlayerData,v);
	        end
		elseif(playerEnumType == gamPListSettings.enumPlayerType.Goalkeeper)then
			if pos == 29 then
	            table.insert(finalPlayerData,v);
	        end
		end
	end
	return finalPlayerData;
end

function OnInitListItem(randomIndex, key, value, cloneGameObject)	
	gamPlayers[randomIndex] = {};
	gamPlayers[randomIndex].gameObject = cloneGameObject;
	gamPlayers[randomIndex].transform = cloneGameObject.transform;
	--bind ui
	gamPlayers[randomIndex].Icon = TransformFindChild(gamPlayers[randomIndex].transform,"IconRoot/Icon");
	gamPlayers[randomIndex].Name = TransformFindChild(gamPlayers[randomIndex].transform,"Name");
	gamPlayers[randomIndex].Num = TransformFindChild(gamPlayers[randomIndex].transform,"Num");
	gamPlayers[randomIndex].Rank = TransformFindChild(gamPlayers[randomIndex].transform,"RankRoot/Rank");
	gamPlayers[randomIndex].Pos = TransformFindChild(gamPlayers[randomIndex].transform,"Pos");
	gamPlayers[randomIndex].Dot = TransformFindChild(gamPlayers[randomIndex].transform,"Dot");
	gamPlayers[randomIndex].ProgressBar = TransformFindChild(gamPlayers[randomIndex].transform,"ProgressBar");
	gamPlayers[randomIndex].FullEffect = TransformFindChild(gamPlayers[randomIndex].transform,"FullEffect");
	gamPlayers[randomIndex].SelectedFrame = TransformFindChild(gamPlayers[randomIndex].transform,"SelectedFrame");

	--set ui
	Util.SetUITexture(gamPlayers[randomIndex].Icon, LuaConst.Const.PlayerHeadIcon, HeroData.GetHeroIcon(tostring(value.id)), true);
	UIHelper.SetLabelTxt(gamPlayers[randomIndex].Name,HeroData.GetHeroName(value.id));

	local pieceID = Config.GetProperty(Config.HeroFragTable(),value.id,"item");
	local heroSlv = Hero.GetCurrStars(value.id);
	if(heroSlv<5)then
		--have up-limit
		local pieceNeeded = Config.GetProperty(Config.HeroSlvTable(),tostring(heroSlv+1),"subSoul");
		UIHelper.SetLabelTxt(gamPlayers[randomIndex].Num,ItemSys.GetItemData(pieceID).num.."/"..pieceNeeded);
		UIHelper.SetProgressBar(gamPlayers[randomIndex].ProgressBar,ItemSys.GetItemData(pieceID).num/pieceNeeded);		
		GameObjectSetActive(gamPlayers[randomIndex].FullEffect.gameObject,ItemSys.GetItemData(pieceID).num>=pieceNeeded);
	else
		--have no up-limit
		UIHelper.SetLabelTxt(gamPlayers[randomIndex].Num,ItemSys.GetItemData(pieceID).num);
		UIHelper.SetProgressBar(gamPlayers[randomIndex].ProgressBar,1);
		GameObjectSetActive(gamPlayers[randomIndex].FullEffect.gameObject,true);
	end
	UIHelper.SetSpriteName(gamPlayers[randomIndex].Rank,HeroData.GetHeroRatingName(value.id));
	UIHelper.SetLabelTxt(gamPlayers[randomIndex].Pos,HeroData.GetHeroPosName(value.id));
	UIHelper.SetSpriteName(gamPlayers[randomIndex].Dot,gamPListSettings.dotDisactive);
	GameObjectSetActive(gamPlayers[randomIndex].SelectedFrame.gameObject,false);

	UIHelper.SetDragScrollViewTarget(gamPlayers[randomIndex].transform,uiScrollView);
	AddOrChangeClickParameters(gamPlayers[randomIndex].gameObject,OnClickPlayer,{item=gamPlayers[randomIndex],data=value});
end

function OnClickPlayer(go)
	if(uiCurrentPlayer~=nil and uiCurrentPlayer.item~=nil)then
		--set off the last selection
		GameObjectSetActive(uiCurrentPlayer.item.SelectedFrame.gameObject,false);
		UIHelper.SetSpriteName(uiCurrentPlayer.item.Dot,gamPListSettings.dotDisactive);
	end
	--set the current one
	local listener = UIHelper.GetUIEventListener(go);
	uiCurrentPlayer.item = listener.parameter.item;
	uiCurrentPlayer.data = listener.parameter.data;
	GameObjectSetActive(uiCurrentPlayer.item.SelectedFrame.gameObject,true);
	UIHelper.SetSpriteName(uiCurrentPlayer.item.Dot,gamPListSettings.dotActive);

	delegateOnSelectPlayer(uiCurrentPlayer.data);

	Close();
end

function OnClickShowFilterBox()
	WindowMgr.ShowWindow(LuaConst.Const.UIDailyCupGamPlayerFilterBox,{delegateOnClickFilter = OnClickFilterBoxItem});
end

function OnClickFilterBoxItem(value)
	UIHelper.SetLabelTxt(filterButton.buttonContent,GetLocalizedString(gamPListSettings.enumPlayerName[value]));
	CreatePlayerList(value);
end

function OnHide()

end

function OnShow()

end

function Close()
	windowComponent:Close();
end

function OnDestroy()
	playerItemPrefab = nil;
	uiScrollView = nil;
	uiContainer = nil;
	gamPlayers = {};
	uiCurrentPlayer = {item = nil,data = nil};
	window = nil;
	windowComponent = nil;
	delegateOnSelectPlayer = nil;
	circleListManager = nil;
	filterButton = {
		buttonContent = nil
	};
end