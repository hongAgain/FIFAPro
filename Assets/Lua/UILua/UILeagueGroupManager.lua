module("UILeagueGroupManager", package.seeall)

require "Game/PVPMsgManager"
--require "UILua/UILeagueMatchScript"
require "UILua/UICircleListManager"

uiPrefabs = {
	GroupListName = "LeagueGroupList",
	GroupList = nil
}

local groupListUI = nil;

-- 1-256
local currentGroupItemIndex = nil;
-- 1-4 ： C B A S
local currentGroupItemlevel = nil;

local levelItems = {};
local uiScrollView = nil;
local uiContainer = nil;
local uiDragScrollView = nil;
local circleListManager = nil;

local delegateOnClickLevel = nil;
local delegateOnClickGroup = nil;

local groupSettings = {
	GroupScrollViewPos = NewVector3(-241,-85,0),
	GroupItemContainerOffset = 0,
	GroupItemContainerCellHeight = 50,
	LevelNormalColor = Color.New(255/255,255/255,255/255,255/255),
	LevelSelectedColor = Color.New(102/255,204/255,255/255,255/255),
	GroupItemNamePrefix = "LeagueGroupItemName"
}

function CreateGroups( containerTransform, windowcomponent, delegateonclicklevel, delegateonclickgroup, depth )

	if(groupListUI == nil) then
		--Get prefabs
		uiPrefabs.GroupList = windowcomponent:GetPrefab(uiPrefabs.GroupListName);

		--generate ui and initialize it
		groupListUI = GameObjectInstantiate(uiPrefabs.GroupList);
		groupListUI.transform.parent = containerTransform;
    	groupListUI.transform.localPosition = Vector3.zero;
    	groupListUI.transform.localScale = Vector3.one; 	

		delegateOnClickLevel = delegateonclicklevel;
		delegateOnClickGroup = delegateonclickgroup;
		--bind ui
		levelItems = {};
		--1c 2b 3a 4s
		for i=1,4 do
			levelItems[i] = {};
			levelItems[i].transform = TransformFindChild(groupListUI.transform,"Group"..i);
			levelItems[i].gameObject = levelItems[i].transform.gameObject;
			levelItems[i].level = TransformFindChild(levelItems[i].transform,"GroupLevel");
			levelItems[i].selectedSprite = TransformFindChild(levelItems[i].transform,"GroupSelected");

			AddOrChangeClickParameters(levelItems[i].gameObject,OnClickLevel,{levelID = i});
		end
		uiScrollView = TransformFindChild(groupListUI.transform,"uiScrollView");
		uiContainer = TransformFindChild(uiScrollView,"uiContainer");
		uiDragScrollView = TransformFindChild(groupListUI.transform,"DragScrollView");
		
		UIHelper.SetPanelDepth(uiScrollView,depth+1);
		UIHelper.SetDragScrollViewTarget(uiDragScrollView,uiScrollView);
	end

	--active it
	if(not GameObjectActiveSelf(groupListUI)) then
    	GameObjectSetActive(groupListUI,true);
    end

	-- SetGroups(true);
	SetGroups();
end

-- function SetGroups(needResetPos)
function SetGroups(needResetPos)

	--prepare data
	local infoData = PVPMsgManager.GetPVPData(MsgID.tb.LeagueInfo);
	local groupData = PVPMsgManager.GetPVPData(MsgID.tb.LeagueGroup);
	if(IsTableEmpty(groupData))then
		return;
	end
	local groupSum = math.pow(4,(5-groupData.level));

	if(currentGroupItemlevel~=groupData.level) then		
		--create Levels
		currentGroupItemlevel = groupData.level;
		currentGroupItemIndex = groupData.group;
		--set group Buttons
		for i=1,4 do
			if(i == currentGroupItemlevel) then
				UIHelper.SetWidgetColor(levelItems[i].level,groupSettings.LevelSelectedColor);
				GameObjectSetActive(levelItems[i].selectedSprite,true);
			else
				UIHelper.SetWidgetColor(levelItems[i].level,groupSettings.LevelNormalColor);
				GameObjectSetActive(levelItems[i].selectedSprite,false);
			end
		end
		--create group items
		local groupItemDatas = {};
		for i=1,groupSum do
			groupItemDatas[i] = {groupID = i};
		end
		if(circleListManager==nil)then
			circleListManager = UICircleListManager.New();
		end
		circleListManager:CreateUICircleList(uiScrollView,
			uiContainer,
			groupItemDatas,
			{
				OnCreateItem = OnCreateGroupItem,
				--( { item=items[randomIndex], data=value } )
				OnClickItem = OnClickGroupItem,
				OnSelectByDrag = OnClickGroupItem
			},
			nil,
			groupData.group);
	elseif(needResetPos)then
		--still show these same groups
		print("outside Reposition all !!");
		if(currentGroupItemIndex ~= groupData.group) then
			currentGroupItemIndex = groupData.group;
		end
   		circleListManager:ResetPosition(currentGroupItemIndex);
	end
end

--(randomIndex, key, value, items[randomIndex].itemName);
function OnCreateGroupItem( randomIndex, key, value, itemNameTrans )
	--set the item name
	UIHelper.SetLabelTxt(itemNameTrans,GetLocalizedString(groupSettings.GroupItemNamePrefix,value.groupID));
end

--( { item=items[randomIndex], data=value } )
function OnClickGroupItem( params )
	
	-- print("name"..params.item.transform.name);
	-- print("id"..params.data.groupID);
	if(params.data ~= nil)then		
		if(delegateOnClickGroup~=nil)then
			delegateOnClickGroup(params.data.groupID);
		end
	end	
end

function SetContainerPos(SelectedIndex)	
	return NewVector3(0,groupSettings.GroupItemContainerOffset+(SelectedIndex-1)*groupSettings.GroupItemContainerCellHeight,0);
end

function OnClickLevel(go)

	-- local groupData = PVPMsgManager.Get_LeagueGroupData();
	local groupData = PVPMsgManager.GetPVPData(MsgID.tb.LeagueGroup);
	if(IsTableEmpty(groupData))then
		return;
	end

	local listener = UIHelper.GetUIEventListener(go);
	if(listener.parameter.levelID ~=nil) then
		if(delegateOnClickLevel~=nil)then
			delegateOnClickLevel(listener.parameter.levelID);
		end
	end
end

function OnDestroy()
	print("..OnDestroy UILeagueGroupManager");
	if(circleListManager~=nil)then
		circleListManager:OnDestroy();
	end
	
	uiPrefabs = {
		GroupListName = "LeagueGroupList",
		GroupList = nil
	}
	groupListUI = nil;
	-- 1-256
	currentGroupItemIndex = nil;
	-- 1-4 ： C B A S
	currentGroupItemlevel = nil;
	levelItems = {};
	uiScrollView = nil;
	uiContainer = nil;
	circleListManager = nil;
	windowComponent = nil;
	uiDragScrollView = nil;
	delegateOnClickLevel = nil;
	delegateOnClickGroup = nil;
	recenterTimerID = nil;
	effectTimerID = nil;
end





