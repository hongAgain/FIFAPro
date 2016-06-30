module("UIShopSubTitleManager",package.seeall);

require "Common/UnityCommonScript"


local subtitleSetting = {
	ListPrefabName = "ShopSubTitleManager",
	ListPrefab = nil,

	ItemPrefabName = "ShopSubTitleItem",
	ItemPrefab = nil,

	ColorNormal = Color.New(1,1,1,1),
	ColorSelected = Color.New(102/255,204/255,255/255,255/255),
	ScrollViewPos = NewVector3(0,139.3,0)
}

local subTitleList = nil;
local listItems = {};
local currentIndex = nil;

local uiScrollView = nil;
local uiContainer = nil;
local buttonRArrow = nil;
local buttonLArrow = nil;


local delegateOnSelect = nil;

local centerItem = nil;

function CreateSubTitleList(containerTransform,windowComponent,depth,dataList,delegateonselect)
	
	if(subTitleList == nil)then
		subtitleSetting.ListPrefab = windowComponent:GetPrefab(subtitleSetting.ListPrefabName);
		subtitleSetting.ItemPrefab = windowComponent:GetPrefab(subtitleSetting.ItemPrefabName);
		--instantiate prefab and initialize it
		subTitleList = GameObjectInstantiate(subtitleSetting.ListPrefab);
		subTitleList.transform.parent = containerTransform;
    	subTitleList.transform.localPosition = Vector3.zero;
    	subTitleList.transform.localScale = Vector3.one;

    	delegateOnSelect = delegateonselect;
	end

	--active it
	if(not GameObjectActiveSelf(subTitleList)) then
    	GameObjectSetActive(subTitleList.transform,true);
    end

	--set info
	BindUI(depth);
	SetInfo(dataList);

	return subTitleList;
end

function RefreshSubTitleList(dataList)
	--active it
	ShowManager(true);
	SetInfo(dataList);
end

function BindUI(depth)
	uiScrollView = TransformFindChild(subTitleList.transform,"uiScrollView");
	uiContainer = TransformFindChild(uiScrollView,"uiContainer");
	buttonLArrow = TransformFindChild(subTitleList.transform,"ButtonLArrow");
	buttonRArrow = TransformFindChild(subTitleList.transform,"ButtonRArrow");

	UIHelper.SetPanelDepth(uiScrollView,depth);
	AddOrChangeClickParameters(buttonLArrow.gameObject,OnClickButtonLeftArrow,nil);
	AddOrChangeClickParameters(buttonRArrow.gameObject,OnClickButtonRightArrow,nil);
end

function SetInfo(dataList)
	--destroy old 
	DestroyUIListItemGameObjects(listItems);
	--create new
	CreateUIListItemGameObjects(uiContainer, dataList, subtitleSetting.ItemPrefab, OnInitItem);

	--select first one as default
	if(dataList~=nil and dataList~={})then
		currentIndex = 1;
		UIHelper.SetWidgetColor(listItems[currentIndex].NameLabel,subtitleSetting.ColorSelected);
	end
	--set some events
	-- UIHelper.AddDragOnStarted(uiScrollView,OnDragStarted);
	-- UIHelper.AddDragOnFinish(uiScrollView,OnDragFinish);
	UIHelper.SetGridPosition(
		uiContainer,
		uiScrollView,
		subtitleSetting.ScrollViewPos,
		SetContainerPos(0),
		false);
   	UIHelper.RefreshPanel(uiScrollView);
end

function SetContainerPos(columnNum)
	return NewVector3(-353.5+(columnNum)*141,0,0);
end

function OnInitItem(randomIndex, key, value, cloneGameObject)
	--bind ui
	listItems[randomIndex] = {};
	listItems[randomIndex].gameObject = cloneGameObject;
	listItems[randomIndex].transform = cloneGameObject.transform;
	listItems[randomIndex].transform.name = string.format("%03d",tonumber(key));
	listItems[randomIndex].NameLabel = TransformFindChild(listItems[randomIndex].transform,"Name");

	UIHelper.SetLabelTxt(listItems[randomIndex].NameLabel,value.titleName);

	--add click event
	AddOrChangeClickParameters(
		listItems[randomIndex].gameObject,
		OnClickItem,
		{
			item=listItems[randomIndex],
			data=value,
			index = randomIndex
		});
	UIHelper.SetDragScrollViewTarget(listItems[randomIndex].transform,uiScrollView);
end

function OnClickItem(go)
	local listener = UIHelper.GetUIEventListener(go);
	if(listener~=nil and listener.parameter~=nil)then
		delegateOnSelect(listener.parameter);
	end
	SwitchSelectedIndex(listener.parameter.index);
end

function SwitchSelectedIndex(index)	
	if(listItems[index]~=nil and listItems[index]~={})then
		print("SwitchSelectedIndex : "..index);
		--unselect current item
		if(currentIndex~=nil)then
			UIHelper.SetWidgetColor(listItems[currentIndex].NameLabel,subtitleSetting.ColorNormal);
		end
		currentIndex = index;
		UIHelper.SetWidgetColor(listItems[currentIndex].NameLabel,subtitleSetting.ColorSelected);

		SetArrowButtons();
	end
end

function OnClickButtonLeftArrow()
	local targetIndex = currentIndex - 1;
	if(listItems[targetIndex]~=nil and listItems[targetIndex]~={})then
		if(currentIndex~=nil)then
			UIHelper.SetWidgetColor(listItems[currentIndex].NameLabel,subtitleSetting.ColorNormal);
		end
		currentIndex = targetIndex
		OnClickItem(listItems[currentIndex].transform);
		UIHelper.SetWidgetColor(listItems[currentIndex].NameLabel,subtitleSetting.ColorSelected);
		--tween to this index
		ScrollToIndexedItem( currentIndex );
		--check to disable button
		SetArrowButtons();
	end
end

function OnClickButtonRightArrow()
	local targetIndex = currentIndex + 1;
	if(listItems[targetIndex]~=nil and listItems[targetIndex]~={})then
		if(currentIndex~=nil)then
			UIHelper.SetWidgetColor(listItems[currentIndex].NameLabel,subtitleSetting.ColorNormal);
		end
		currentIndex = targetIndex
		OnClickItem(listItems[currentIndex].transform);
		UIHelper.SetWidgetColor(listItems[currentIndex].NameLabel,subtitleSetting.ColorSelected);
		--tween to this index
		ScrollToIndexedItem( currentIndex );
		--check to disable button
		SetArrowButtons();
	end
end

function SetArrowButtons()
	if(listItems[currentIndex-1]==nil or IsTableEmpty(listItems[currentIndex-1]))then
		GameObjectSetActive(buttonLArrow,false);
	else
		GameObjectSetActive(buttonLArrow,true);
	end
	if(listItems[currentIndex+1]==nil or IsTableEmpty(listItems[currentIndex+1]))then
		GameObjectSetActive(buttonRArrow,false);
	else
		GameObjectSetActive(buttonRArrow,true);
	end
end

--targetIndex start from 1
function ScrollToIndexedItem( targetIndex )
	if(targetIndex~=nil and targetIndex>0)then
		BeforeScroll();
		UIHelper.ScrollClippedPanelTo(uiScrollView,GetScrollPos(targetIndex-1),8,AfterScroll);
	end
end

--targetColumn start from 0
function GetScrollPos(targetColumn)
	return NewVector3(-targetColumn*141,139.3,0);
end

function BeforeScroll()

end

function AfterScroll()
	Util.EnableScript(uiScrollView.gameObject,"SpringPanel",false);
end

function ShowManager(willShow)
   	GameObjectSetActive(subTitleList.transform,willShow);
end

function OnDestroy()
	subtitleSetting.ListPrefab = nil;
	subtitleSetting.ItemPrefab = nil;
	subTitleList = nil;
	listItems = {};
	currentIndex = nil;
	uiScrollView = nil;
	uiContainer = nil;
	buttonRArrow = nil;
	buttonLArrow = nil;
	delegateOnSelect = nil;
	centerItem = nil;
end