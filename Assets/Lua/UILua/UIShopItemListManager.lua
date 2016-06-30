module("UIShopItemListManager",package.seeall);

require "Common/UnityCommonScript"

local itemListSetting = {
	ListPrefabName = "ShopItemListManager",
	ListPrefab = nil,

	ItemPrefabName = "ShopItem",
	ItemPrefab = nil,

	ScrollViewPos = NewVector3(0,-80,0)
}

local itemList = nil;
local listItems = {};

local uiScrollView = nil;
local uiContainer = nil;

local delegateOnSelect = nil;
local delegateOnDragFinish = nil;


--datalist structure:{Name,Icon,Num,Condition,PriceIcon,SinglePrice,IsSoldOut}
function CreateItemList(containerTransform,windowComponent,depth,dataList,delegateonselect,delegateondragfinish)
	
	if(itemList == nil)then
		itemListSetting.ListPrefab = windowComponent:GetPrefab(itemListSetting.ListPrefabName);
		itemListSetting.ItemPrefab = windowComponent:GetPrefab(itemListSetting.ItemPrefabName);
		--instantiate prefab and initialize it
		itemList = GameObjectInstantiate(itemListSetting.ListPrefab);
		itemList.transform.parent = containerTransform;
    	itemList.transform.localPosition = Vector3.zero;
    	itemList.transform.localScale = Vector3.one;

    	delegateOnSelect = delegateonselect;
    	delegateOnDragFinish = delegateondragfinish;
	end

	--active it
	if(not GameObjectActiveSelf(itemList)) then
    	GameObjectSetActive(itemList.transform,true);
    end

	--set info
	BindUI(depth);
	SetInfo(dataList);
	return itemList;
end

function RefreshItemList(dataList)
	ShowManager(true);
	SetInfo(dataList);
end

function BindUI(depth)
	uiScrollView = TransformFindChild(itemList.transform,"uiScrollView");
	uiContainer = TransformFindChild(uiScrollView,"uiContainer");

	UIHelper.SetPanelDepth(uiScrollView,depth);
end

function SetInfo(dataList)
	--destroy old 
	DestroyUIListItemGameObjects(listItems);
	--create new
	CreateUIListItemGameObjects(uiContainer, dataList, itemListSetting.ItemPrefab, OnInitItem);
	--set some events
	UIHelper.AddDragOnFinish(uiScrollView,OnDragFinish);

	UIHelper.SetGridPosition(
		uiContainer,
		uiScrollView,
		itemListSetting.ScrollViewPos,
		SetContainerPos(0),
		false);
   	UIHelper.RefreshPanel(uiScrollView);
end

function SetContainerPos(columnNum)
	return NewVector3(315*(columnNum-1),88,0);
end

-- value structure:{Name,Icon,Num,Condition,PriceIcon,SinglePrice,IsSoldOut}
function OnInitItem(randomIndex, key, value, cloneGameObject)
	--bind ui
	listItems[randomIndex] = {};
	listItems[randomIndex].gameObject = cloneGameObject;
	listItems[randomIndex].transform = cloneGameObject.transform;
	listItems[randomIndex].transform.name = string.format("%03d",tonumber(key));

	listItems[randomIndex].Name = TransformFindChild(listItems[randomIndex].transform,"Name");
	listItems[randomIndex].Icon = TransformFindChild(listItems[randomIndex].transform,"IconRoot/icon");
	listItems[randomIndex].Num = TransformFindChild(listItems[randomIndex].transform,"Num");
	listItems[randomIndex].Condition = TransformFindChild(listItems[randomIndex].transform,"Condition");
	listItems[randomIndex].PriceIcon = TransformFindChild(listItems[randomIndex].transform,"Currency");
	listItems[randomIndex].SinglePrice = TransformFindChild(listItems[randomIndex].transform,"Price");

	UIHelper.SetLabelTxt(listItems[randomIndex].Name,value.Name);
	Util.SetUITexture(listItems[randomIndex].Icon,LuaConst.Const.ItemIcon,value.Icon, true);
	UIHelper.SetLabelTxt(listItems[randomIndex].Num,value.Num);
	UIHelper.SetLabelTxt(listItems[randomIndex].Condition,value.Condition);
	-- UIHelper.SetSpriteName(listItems[randomIndex].PriceIcon,value.PriceIcon);
	UIHelper.SetLabelTxt(listItems[randomIndex].SinglePrice,value.SinglePrice);

	--add click event
	AddOrChangeClickParameters(
		listItems[randomIndex].gameObject,
		OnClickItem,
		{
			item = listItems[randomIndex],
			data = value,
			index = randomIndex
		});
	UIHelper.SetDragScrollViewTarget(listItems[randomIndex].transform,uiScrollView);
end

function OnDragFinish()
	if(delegateOnDragFinish~=nil)then
		delegateOnDragFinish(CalculateScrolledIndex());
	end
end

--returns the max index of this column
function CalculateScrolledIndex()
	return (math.floor(uiScrollView.localPosition.x/(-315))+1)*2;
end

function OnClickItem(go)
	local listener = UIHelper.GetUIEventListener(go);
	if(listener~=nil and listener.parameter~=nil)then
		delegateOnSelect(listener.parameter);
	end	
end

--targetIndex start from 1
function ScrollToIndexedItem( targetIndex )
	if(targetIndex~=nil and targetIndex>0)then
		BeforeScroll();
		UIHelper.ScrollClippedPanelTo(uiScrollView,GetScrollPos(math.floor((targetIndex-1)/2)),8,AfterScroll);
	end
end

--targetColumn start from 0
function GetScrollPos(targetColumn)
	return NewVector3(-targetColumn*315,-80,0);
end

function BeforeScroll()

end

function AfterScroll()
	Util.EnableScript(uiScrollView.gameObject,"SpringPanel",false);
end

function ShowManager(willShow)
   	GameObjectSetActive(itemList.transform,willShow);
end

function OnDestroy()
	itemListSetting.ListPrefab = nil;
	itemListSetting.ItemPrefab = nil;
	itemList = nil;
	listItems = {};
	uiScrollView = nil;
	uiContainer = nil;
	delegateOnSelect = nil;
end