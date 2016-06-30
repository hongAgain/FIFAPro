module("UICircleListManager",package.seeall)

require "Common/UnityCommonScript"
require "Common/Color"

local circleListSettings = {
	itemPrefabIDX = 3,
	itemPrefabName = "UICircleListItem",
	colorCenter = Color.New(102/255,204/255,255/255,255/255),
	colorNormal = Color.New(171/255,173/255,185/255,255/255),
	colorEdge = Color.New(125/255,128/255,137/255,255/255),
	fontSizeCenter = 30,
	fontSizeNormal = 22,
	fontSizeEdge = 22,
	itemScales = { 1 , 0.8 	, 0.6 	, 0.4 	, 0.2 	, 0.1 	, 0.1 	, 1 },
	itemOffset = { 0 , 3 	, 14 	, 31 	, 56 	, 88 	, 126 	, 0 },
	itemColors = {
		Color.New(171/255,173/255,185/255,255/255),
		Color.New(171/255,173/255,185/255,255/255),
		Color.New(171/255,173/255,185/255,255/255),
		Color.New(125/255,128/255,137/255,255/255),
		Color.New(125/255,128/255,137/255,255/255),
		Color.New(125/255,128/255,137/255,255/255),
		Color.New(125/255,128/255,137/255,0/255),
		Color.New(125/255,128/255,137/255,0/255)
	},
	itemFontSizes = {22	,22	,22	,22	,22	,22	,22	,22},
	effectNum = 8
}

local ManagerBase = {
	itemPrefab = nil
};

local mt = {};
mt.__index = ManagerBase;

function New()
	local manager = {
	uiScrollView = nil,
	uiScrollViewPos = nil,
	uiContainer = nil,
	uiPanelCenterPos = nil,
	ContainerCellWidth = nil,
	ContainerCellHeight = nil,
	
	items = {},
	datas = {},
	centerItem = nil,
	needInitalize = true,	
	recenterTimerIDByDrag = nil,
	recenterTimerIDByClick = nil,
	currentCenterItemData = nil,
	delegates = {
		--(randomIndex, key, value, items[randomIndex].itemName);
		OnCreateItem = nil,
		--( { item=items[randomIndex], data=value } )
		OnClickItem = nil,
		OnSelectByDrag = nil
	}
	};
	setmetatable(manager,mt);

	function manager:CreateUICircleList(listTrans,gridTrans,datatable,delegatetable,startIndex,defaultIndex)	
		manager.uiScrollView = listTrans;
		manager.uiContainer = gridTrans;
		if(manager.uiScrollViewPos == nil)then
			manager.uiScrollViewPos = manager.uiScrollView.localPosition;
		end
		if(manager.ContainerCellWidth == nil or manager.ContainerCellHeight == nil)then
			local cellSize = UIHelper.GetUIGridCellSize(gridTrans);
			manager.ContainerCellWidth = cellSize.x;
			manager.ContainerCellHeight = cellSize.y;
		end
		manager.datas = ToDataList(datatable,startIndex);
		manager.delegates = delegatetable;

		manager.needInitalize = true;
		if(manager.itemPrefab==nil)then
			manager.itemPrefab = Util.GetGameObject(circleListSettings.itemPrefabName);	
		end	
		UIHelper.SetSizeOfWidget(manager.itemPrefab.transform, UIHelper.GetUIGridCellSize(gridTrans))
		StartCreate(defaultIndex);
		-- GameObjectDestroyImmediate(manager.itemPrefab);
		-- manager.itemPrefab = nil;
	end

	-- return a data table with startIndexed item at first pos, key is num
	function ToDataList(dataTable,startIndex)
		if(startIndex == nil or startIndex == 1)then
			return dataTable;
		end

		local finalData = {};
		for k,v in pairs(dataTable) do
			local kNum = tonumber(k);
			if(kNum<startIndex)then
				finalData[tonumber(k)+startIndex] = v;
			else
				finalData[tonumber(k)-startIndex+1] = v;
			end			
		end
		return finalData;
	end

	function manager:GetCenteredObject()
		local centeredItem = UIHelper.CenterOnRecenter(manager.uiContainer);
		if(centeredItem~=nil)then
			local listener = UIHelper.GetUIEventListener(centeredItem);
			if(listener.parameter ~= nil )then
				return (listener.parameter);
			end
		end	
	end

	function StartCreate(defaultIndex)
		--destroy old 
		DestroyUIListItemGameObjects(manager.items);		
		--create new
		CreateUIListItemGameObjects(manager.uiContainer, manager.datas, manager.itemPrefab, OnInitItem);
		--set some events
		UIHelper.AddDragOnStarted(manager.uiScrollView,OnDragStarted);
		UIHelper.AddDragOnFinish(manager.uiScrollView,OnDragFinish);

		-- if(defaultIndex~=nil)then
			UIHelper.SetGridPosition(
				manager.uiContainer,
				manager.uiScrollView,
				manager.uiScrollViewPos,
				SetContainerPos(defaultIndex or 1),
				true);
		-- end
   		UIHelper.RefreshPanel(manager.uiScrollView);
	end

	function manager:ResetPosition(defaultIndex)

		UIHelper.SetGridPosition(
			manager.uiContainer,
			manager.uiScrollView,
			manager.uiScrollViewPos,
			SetContainerPos(defaultIndex or 1),
			true);
   		UIHelper.RefreshPanel(manager.uiScrollView);
	end

	function SetContainerPos(index)
		return NewVector3(0,(index-1)*manager.ContainerCellHeight,0);
	end

	function OnInitItem(randomIndex, key, value, cloneGameObject)
		--bind ui
		manager.items[randomIndex] = {};
		manager.items[randomIndex].gameObject = cloneGameObject;
		manager.items[randomIndex].transform = cloneGameObject.transform;
		manager.items[randomIndex].transform.name = string.format("%03d",tonumber(key));

		manager.items[randomIndex].transformNode = TransformFindChild(manager.items[randomIndex].transform,"TransformNode");
		manager.items[randomIndex].itemName = TransformFindChild(manager.items[randomIndex].transformNode,"GroupName");

		if(manager.delegates.OnCreateItem~=nil)then
			manager.delegates.OnCreateItem(randomIndex, key, value, manager.items[randomIndex].itemName);
		end

		--add click event
		AddOrChangeClickParameters(
			manager.items[randomIndex].gameObject,
			OnClickItem,
			{
				item=manager.items[randomIndex],
				data=value
			});
		UIHelper.SetDragScrollViewTarget(manager.items[randomIndex].transform,manager.uiScrollView);
		Util.InitializeUICircleListItem(manager.items[randomIndex].transform);
	end

	function OnClickItem(go)
		-- body
		manager.centerItem = go.transform;
		UIHelper.OnClickChildToCenterOn(manager.uiScrollView,manager.centerItem);
		RecenterItemListByClick();
	end

	--alternative way to handle bug
	function OnPressed(isPressed)
		manager.centerItem = nil;
		RecenterItemList();
	end

	function OnDragStarted()
		--disable center on child here
		Util.EnableScript(manager.uiContainer.gameObject,"UICenterOnChild",false);
	end

	function OnDragFinish()
		--enable center on child here
		Util.EnableScript(manager.uiContainer.gameObject,"UICenterOnChild",true);
		manager.centerItem = nil;
		RecenterItemListByDrag();
	end

	function RecenterItemListByDrag()
		StopRecenterByDrag();
	    manager.recenterTimerIDByDrag = LuaTimer.AddTimer(false,0.1,OnRecenterFinishByDrag);   
	end

	function RecenterItemListByClick()
		StopRecenterByClick();
	    manager.recenterTimerIDByClick = LuaTimer.AddTimer(false,0.1,OnRecenterFinishByClick);   
	end

	function OnRecenterFinishByDrag()
		StopRecenterByDrag();
		--select target item on drag spring finish
		if(manager.centerItem==nil)then
			manager.centerItem = UIHelper.CenterOnRecenter(manager.uiContainer);
		end
		-- print(manager.centerItem.name);
		if(manager.centerItem~=nil)then
			local listener = UIHelper.GetUIEventListener(manager.centerItem);
			if(listener.parameter.data ~= nil )then
				if( listener.parameter.data ~= manager.currentCenterItemData) then
					if(manager.delegates.OnSelectByDrag~=nil)then
						manager.delegates.OnSelectByDrag(listener.parameter);
					end
				end
				manager.currentCenterItemData = listener.parameter.data;
			end
			manager.centerItem = nil;
		end	
	end

	function OnRecenterFinishByClick()
		StopRecenterByClick();
		--select target item on drag spring finish
		if(manager.centerItem==nil)then
			manager.centerItem = UIHelper.CenterOnRecenter(manager.uiContainer);
		end
		-- print(manager.centerItem.name);
		if(manager.centerItem~=nil)then
			local listener = UIHelper.GetUIEventListener(manager.centerItem);
			if(listener.parameter.data ~= nil )then
				if( listener.parameter.data ~= manager.currentCenterItemData) then
					if(manager.delegates.OnClickItem~=nil)then
						manager.delegates.OnClickItem(listener.parameter);
					end
				end
				manager.currentCenterItemData = listener.parameter.data;
			end
			manager.centerItem = nil;
		end	
	end

	function StopRecenterByDrag()
		if(manager.recenterTimerIDByDrag~=nil)then
			LuaTimer.RmvTimer(manager.recenterTimerIDByDrag);
	        manager.recenterTimerIDByDrag = nil;
		end
	end

	function StopRecenterByClick()
		if(manager.recenterTimerIDByClick~=nil)then
			LuaTimer.RmvTimer(manager.recenterTimerIDByClick);
	        manager.recenterTimerIDByClick = nil;
		end
	end

	function manager:OnDestroy()
		StopRecenterByDrag();
		StopRecenterByClick();
		manager.uiScrollView = nil;
		manager.uiContainer = nil;
		manager.uiPanelCenterPos = nil;
		manager.ContainerCellWidth = nil;
		manager.ContainerCellHeight = nil;
		
		manager.itemPrefab = nil;
		manager.items = {};
		manager.datas = {};
		manager.centerItem = nil;
		manager.needInitalize = true;
		manager.recenterTimerIDByDrag = nil;
		manager.recenterTimerIDByClick = nil;
		manager.currentCenterItemData = nil;
		manager.delegates = {
			--(randomIndex, key, value, items[randomIndex].itemName);
			OnCreateItem = nil,
			--( { item=items[randomIndex], data=value } )
			OnClickItem = nil,
			OnSelectByDrag = nil
		}
	end

	return manager;
end