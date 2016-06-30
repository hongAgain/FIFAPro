module("ShopConfirmBox",package.seeall)

require "Common/UnityCommonScript"
require "Config"

local ShopConfirmSettings = {
	LadderShopConfirmBoxPrefabName = "ShopConfirmBox",
	LadderShopConfirmBoxPrefab = nil,

	strHaveNum = "HaveNum"

}

local shopConfirmBoxUI = nil;

local buttonClose = nil;
local buttonConfirm = nil;

local icon = nil;
local name = nil;
local haveNum = nil;

local buyNum = nil;
local buttonMinus = nil;
local buttonMinusMulti = nil;
local buttonPlus = nil;
local buttonPlusMulti = nil;

local priceIcon = nil;
local totalPrice = nil;

local dataParameter = {};
local dataBuyNum = 0;

local delegateOnBuy = nil;

-- {
-- 	item=listItems[randomIndex],
-- 	data=value,
-- 	index = randomIndex
-- }
function CreateShopConfirmBox(containerTransform, windowcomponent, parameter, onBuy)
	if(shopConfirmBoxUI == nil)then
		ShopConfirmSettings.LadderShopConfirmBoxPrefab = windowcomponent:GetPrefab(ShopConfirmSettings.LadderShopConfirmBoxPrefabName);
		
		shopConfirmBoxUI = GameObjectInstantiate(ShopConfirmSettings.LadderShopConfirmBoxPrefab);
		shopConfirmBoxUI.transform.parent = containerTransform;
    	shopConfirmBoxUI.transform.localPosition = Vector3.zero;
    	shopConfirmBoxUI.transform.localScale = Vector3.one;

    	BindUI();
	end
	--active it
	if(not GameObjectActiveSelf(shopConfirmBoxUI)) then
    	GameObjectSetActive(shopConfirmBoxUI.transform,true);
    end
    delegateOnBuy = onBuy;
	SetInfo(parameter);
end

function BindUI()
	
	buttonConfirm = TransformFindChild(shopConfirmBoxUI.transform,"ButtonConfirm");
	buttonClose = TransformFindChild(shopConfirmBoxUI.transform,"ButtonClose");
	AddOrChangeClickParameters(buttonConfirm.gameObject,OnClickConfirm,nil);
	AddOrChangeClickParameters(buttonClose.gameObject,OnClickClose,nil);

	icon = TransformFindChild(shopConfirmBoxUI.transform,"IconRoot/Icon");
	name = TransformFindChild(shopConfirmBoxUI.transform,"Name");
	haveNum = TransformFindChild(shopConfirmBoxUI.transform,"HaveNum");

	buyNum = TransformFindChild(shopConfirmBoxUI.transform,"BuyNum");

	buttonPlus = TransformFindChild(shopConfirmBoxUI.transform,"ButtonPlus1");
	buttonPlusMulti = TransformFindChild(shopConfirmBoxUI.transform,"ButtonPlus10");
	buttonMinus = TransformFindChild(shopConfirmBoxUI.transform,"ButtonMinus1");
	buttonMinusMulti = TransformFindChild(shopConfirmBoxUI.transform,"ButtonMinus10");

	UIHelper.AddPressRepeating(buttonPlus.gameObject,OnClickPlus1);
	UIHelper.AddPressRepeating(buttonPlusMulti.gameObject,OnClickPlus10);
	UIHelper.AddPressRepeating(buttonMinus.gameObject,OnClickMinus1);
	UIHelper.AddPressRepeating(buttonMinusMulti.gameObject,OnClickMinus10);

	totalPrice = TransformFindChild(shopConfirmBoxUI.transform,"Price");
	priceIcon = TransformFindChild(shopConfirmBoxUI.transform,"PriceIconRoot/PriceIcon");
end


-- dataList[count].ID = v.item_id;
-- dataList[count].Name = itemDict[v.item_id].name;
-- dataList[count].Icon = itemDict[v.item_id].icon;
-- dataList[count].Num = v.num;
-- dataList[count].ConditionNum = v.step;
-- dataList[count].Condition = string.format(conditionSuffix,v.step);
-- dataList[count].PriceIcon = itemDict[currencyID].icon;
-- dataList[count].SinglePrice = tonumber(v.sub);
-- dataList[count].IsSoldOut = false;


function SetInfo(parameter)
	-- body
	dataParameter = parameter;

	Util.SetUITexture(icon, LuaConst.Const.ItemIcon, dataParameter.data.Icon, true);
	UIHelper.SetLabelTxt(name,dataParameter.data.Name);
	UIHelper.SetLabelTxt(haveNum,
		GetLocalizedString(ShopConfirmSettings.strHaveNum,ItemSys.GetItemData(dataParameter.data.ID).num));

	dataBuyNum = 1
	UIHelper.SetLabelTxt(buyNum,dataBuyNum);
	UIHelper.SetLabelTxt(totalPrice,dataParameter.data.SinglePrice*dataBuyNum);
	Util.SetUITexture(priceIcon, LuaConst.Const.ItemIcon, dataParameter.data.PriceIcon, true);

	CheckButtons();
end

function OnClickConfirm()
	if(delegateOnBuy~=nil)then
		delegateOnBuy(dataBuyNum);
	end
end

function OnClickClose()
	Close();
end

function OnClickPlus1()
	AddBuyNum(1);
	CheckButtons();
end

function OnClickPlus10()
	AddBuyNum(10);
	CheckButtons();
end

function OnClickMinus1()
	AddBuyNum(-1);
	CheckButtons();
end

function OnClickMinus10()
	AddBuyNum(-10);
	CheckButtons();
end

function AddBuyNum(delta)	
	dataBuyNum = dataBuyNum + delta;

	if(dataBuyNum > dataParameter.data.Num)then
		dataBuyNum = dataParameter.data.Num;
	end
	if(dataBuyNum < 1)then
		dataBuyNum = 1;
	end

	UIHelper.SetLabelTxt(buyNum,dataBuyNum);
	UIHelper.SetLabelTxt(totalPrice,dataParameter.data.SinglePrice*dataBuyNum);
end

function CheckButtons()
	if(dataBuyNum>=dataParameter.data.Num)then
		GameObjectSetActive(buttonPlus,false);
	else
		GameObjectSetActive(buttonPlus,true);
	end

	-- if(dataBuyNum+10>dataParameter.data.Num)then
	if(dataBuyNum>=dataParameter.data.Num)then
		GameObjectSetActive(buttonPlusMulti,false);
	else
		GameObjectSetActive(buttonPlusMulti,true);
	end

	if(dataBuyNum<=1)then
		GameObjectSetActive(buttonMinus,false);
	else
		GameObjectSetActive(buttonMinus,true);
	end

	-- if(dataBuyNum-10<1)then
	if(dataBuyNum<=1)then
		GameObjectSetActive(buttonMinusMulti,false);
	else
		GameObjectSetActive(buttonMinusMulti,true);
	end
end

function Close()
	if(GameObjectActiveSelf(shopConfirmBoxUI)) then
    	GameObjectSetActive(shopConfirmBoxUI.transform,false);
    end
end

function OnDestroy()
	shopConfirmBoxUI = nil;
	buttonClose = nil;
	buttonConfirm = nil;
	icon = nil;
	name = nil;
	haveNum = nil;
	buyNum = nil;
	buttonMinus = nil;
	buttonMinusMulti = nil;
	buttonPlus = nil;
	buttonPlusMulti = nil;
	totalPrice = nil;
	dataParameter = {};
	dataBuyNum = 0;
	delegateOnBuy = nil;
end