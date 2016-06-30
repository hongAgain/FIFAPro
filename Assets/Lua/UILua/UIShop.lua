module("UIShop",package.seeall);

require "Config"
require "Common/UnityCommonScript"
require "Game/ItemSys"
require "Game/ShopData"
require "UILua/ShopConfirmBox"
require "UILua/UIShopSettings"
require "UILua/UIShopSubTitleManager"
require "UILua/UIShopItemListManager"


local title = nil;
local currency = nil;
local currencyNum = nil;
local buttonLArrow = nil;
local buttonRArrow = nil;

local buttonRefresh = nil;

local subTitleManager = nil;
local itemListManager = nil;

local window = nil;
local windowComponent = nil;
local rootDepth = nil;
local scaleRoot = nil;
local uiPopupRoot = nil;

local shopType = UIShopSettings.ShopType.EpicShop;

function OnStart(gameObject, params)
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");

    BindUI();
    shopType = params.shopType;
    SetInfo();
end

function BindUI()
	local transform = window.transform;

	scaleRoot = TransformFindChild(transform,"ScaleNode");
	title = TransformFindChild(scaleRoot,"CommonNode/Title");
	currency = TransformFindChild(scaleRoot,"CommonNode/Icon");
	currencyNum = TransformFindChild(scaleRoot,"CommonNode/Num");
	buttonLArrow = TransformFindChild(scaleRoot,"CommonNode/ArrowPanel/ButtonLArrow");
	buttonRArrow = TransformFindChild(scaleRoot,"CommonNode/ArrowPanel/ButtonRArrow");
	buttonRefresh = TransformFindChild(scaleRoot,"ButtonRefresh");

	uiPopupRoot = TransformFindChild(transform,"uiPopupRoot");


	AddOrChangeClickParameters(buttonRefresh.gameObject,OnClickRefreshButton,nil);
	AddOrChangeClickParameters(buttonLArrow.gameObject,OnClickButtonLeftArrow,nil);
	AddOrChangeClickParameters(buttonRArrow.gameObject,OnClickButtonRightArrow,nil);
end

function SetInfo()
	RefreshInfo(shopType);
end

function RefreshInfo(type)
	shopType = type;
	local typeSetting = UIShopSettings.ShopSettings[shopType];

	local RealSetInfo = function ()
		UIHelper.SetLabelTxt(title,GetLocalizedString(typeSetting.titleName));
		-- UIHelper.SetSpriteName(currency,typeSetting.currencyID);
		UIHelper.SetLabelTxt(currencyNum,ItemSys.GetItemData(typeSetting.currencyID).num);
		GameObjectSetActive(buttonRefresh,typeSetting.needFreshButton);	

		SetDataLists(typeSetting);
		SetSubTitleList(typeSetting);
		SetItemList(typeSetting);
	end

	if(typeSetting.preRequest ~= nil)then
		typeSetting.preRequest(RealSetInfo);
	else
		RealSetInfo();
	end	
end

function SetDataLists(typeSetting)
	
	if(typeSetting.needSubTitleList)then
		if(typeSetting.subTitleDataList == nil)then
			typeSetting.subTitleDataList = typeSetting.generateSubTitleDataList();
		end
	else
		typeSetting.subTitleDataList = nil;
	end

	if(typeSetting.dataList == nil)then
		typeSetting.dataList = typeSetting.generateDataList(typeSetting.currencyID);
	end

	if(typeSetting.needSubTitleList)then
		if(typeSetting.startIndexList == nil)then
			typeSetting.startIndexList = typeSetting.generateStartIndexList(typeSetting.subTitleDataList,typeSetting.dataList);
		end
	else
		typeSetting.startIndexList = nil;
	end
end

function SetSubTitleList(typeSetting)
	if(typeSetting.needSubTitleList)then
		if(subTitleManager==nil)then
			subTitleManager = UIShopSubTitleManager.CreateSubTitleList(
				scaleRoot,
				windowComponent,
				UIHelper.GetPanelDepth(window.transform)+1,
				typeSetting.subTitleDataList,
				OnSelectSubTitle);
		else
			UIShopSubTitleManager.RefreshSubTitleList(typeSetting.subTitleDataList);
		end
	else
		if(subTitleManager~=nil)then
			UIShopSubTitleManager.ShowManager(false);			
		end
	end
end

function SetItemList(typeSetting)
	-- subItemListManager
	if(itemListManager==nil)then
		itemListManager = UIShopItemListManager.CreateItemList(
			scaleRoot,
			windowComponent,
			UIHelper.GetPanelDepth(window.transform)+1,
			typeSetting.dataList,
			OnSelectListItem,
			OnDragFinishListItem);
	else
		UIShopItemListManager.RefreshItemList(typeSetting.dataList);
	end
end

-- {
-- 	item=listItems[randomIndex],
-- 	data=value,
-- 	index = randomIndex
-- }
function OnSelectSubTitle(params)
	local typeSetting = UIShopSettings.ShopSettings[shopType];
	if(typeSetting.startIndexList~=nil)then
		UIShopItemListManager.ScrollToIndexedItem(typeSetting.startIndexList[params.data.titleData].firstIndex);
	end
end

function OnSelectListItem(params)
	local typeSetting = UIShopSettings.ShopSettings[shopType];
	-- pop up a buying menu
	local OnClickBuying = function (dataBuyNum)
		if(typeSetting.requestToBuy~=nil)then
			typeSetting.requestToBuy({id=params.data.ID,num=dataBuyNum},AfterBuying);
		end
	end
	ShopConfirmBox.CreateShopConfirmBox(uiPopupRoot, windowComponent, params, OnClickBuying);
end

--index of the item on the left( bottom one )
function OnDragFinishListItem(index)
	local typeSetting = UIShopSettings.ShopSettings[shopType];
	if(typeSetting.needSubTitleList)then
		local closestSubTitleFirstIndex = nil;
		local closestSubTitleIndex = nil;
		local typeSetting = UIShopSettings.ShopSettings[shopType];
		if(index>0)then
			--compare firstIndex and get the right titleIndex
			for k,v in pairs(typeSetting.startIndexList) do
				if((v.firstIndex <= index and v.firstIndex > 0) and ( closestSubTitleFirstIndex == nil or v.firstIndex>closestSubTitleFirstIndex))then
					closestSubTitleFirstIndex = v.firstIndex;
					closestSubTitleIndex = v.titleIndex;
				end
			end
		else
			closestSubTitleIndex = 1;
		end

		-- print(closestSubTitleIndex);
		UIShopSubTitleManager.SwitchSelectedIndex(closestSubTitleIndex);
	end
end

function AfterBuying()
	ShopConfirmBox.Close();
end

function OnClickRefreshButton()
	--not used

end

function OnClickButtonLeftArrow()
	RefreshInfo(UIShopSettings.ShopSettings[shopType].prevType);
end

function OnClickButtonRightArrow()
	RefreshInfo(UIShopSettings.ShopSettings[shopType].nextType);
end

function OnHide()

end

function OnShow()

end

function OnDestroy()
	
	UIShopSubTitleManager.OnDestroy();
	UIShopItemListManager.OnDestroy();
	ShopConfirmBox.OnDestroy();

	title = nil;
	currency = nil;
	currencyNum = nil;
	buttonLArrow = nil;
	buttonRArrow = nil;
	buttonRefresh = nil;
	subTitleManager = nil;
	itemListManager = nil;
	window = nil;
	windowComponent = nil;
	rootDepth = nil;
	shopType = nil;

end
