module("UIGameCheckInsBox", package.seeall)

require "Common/UnityCommonScript"
require "Game/GameMainScript"
require "Game/ItemSys"
require "Game/GameCheckInsData"
require "UILua/UIGameCheckInsBonusHint"
require "UILua/UIGameCheckInsGetItemBox"
require "UILua/UIGameCheckInsItemTips"

CheckInSettings = {
	prefabNameCheckInBox = "UIGameCheckInsBox",
	prefabNameCheckInItem = "UIGameCheckInsItem",
	prefabNameCheckInBonusHintBox = "UIGameCheckInsBonusHint",
	prefabNameCheckInGetItemBox = "UIGameCheckInsGetItemBox",
	prefabNameCheckInsItemTips = "UIGameCheckInsItemTips",
	strCheckInBonus = "CheckInBonus",
	strCheckInRules = "CheckInRules",
	strCheckInBonusHint = "CheckInBonusHint",
	strCheckInHowToGet = "CheckInHowToGet",
	sprNameSignGotFull = "Sign1",
	sprNameSignGotHalf = "Sign2"
};

local checkInBox = nil;

local uiButtonCheckInToday = nil;
local uiButtonClose = nil;
local uiButtonHint = nil;
local uiLabelCheckInTodayContent = nil;
local uiLabelMonth = nil;
local uiLabelCheckInSum = nil;

local uiScrollView = nil;
local uiContainer = nil;
local uiCheckInItemList = {};
local uiPopupRoot = nil;

local windowComponent = nil;

local localItemDict = nil;

function CreateCheckInBox( containerTransform, windowcomponent, listDepth )
	windowComponent = windowcomponent;
	if (checkInBox == nil) then
		--instantiate one
		local checkInBoxPrefab = windowComponent:GetPrefab(CheckInSettings.prefabNameCheckInBox);
		--instantiate prefab and initialize it
		checkInBox = GameObjectInstantiate(checkInBoxPrefab);
		checkInBox.transform.parent = containerTransform;
        checkInBox.transform.localPosition = NewVector3(0, -10, 0);
    	checkInBox.transform.localScale = Vector3.one;
        UIHelper.SetPanelDepth(checkInBox.transform, 3);

    	uiButtonCheckInToday       = TransformFindChild(checkInBox.transform, "ButtonCheckInToday");
    	uiButtonClose              = TransformFindChild(checkInBox.transform, "ButtonClose");
		uiButtonHint               = TransformFindChild(checkInBox.transform, "ButtonHint");
        uiLabelCheckInTodayContent = TransformFindChild(uiButtonCheckInToday, "Content");
		Util.AddClick(uiButtonCheckInToday.gameObject, OnClickCheckInToday);
		Util.AddClick(uiButtonClose.gameObject, OnClickClose);
		Util.AddClick(uiButtonHint.gameObject, OnClickHint);

		uiLabelMonth = TransformFindChild(checkInBox.transform,"Month");
		uiLabelCheckInSum = TransformFindChild(checkInBox.transform,"CheckInSum");

		uiScrollView = TransformFindChild(checkInBox.transform,"CheckInList");
		uiContainer = TransformFindChild(checkInBox.transform,"CheckInList/Container");

		uiPopupRoot = TransformFindChild(checkInBox.transform,"PopupRoot");
	end
	--active it
	if(not GameObjectActiveSelf(checkInBox)) then
    	GameObjectSetActive(checkInBox.transform,true);
    end

	UIHelper.SetPanelDepth(uiScrollView, listDepth);
	UIHelper.SetPanelDepth(uiPopupRoot, listDepth+1);
	--set info
	SetInfo();
end

function SetInfo()
	local infoData = GameCheckInsData.Get_InfoData();
	local localData = GameCheckInsData.Get_LocalData();

    localItemDict = Config.GetTemplate(Config.ItemTable());

	if(localData == nil) then
		WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { "本地的本月签到数据有误！" });
		OnClickClose();
		return;
	end

	UIHelper.SetLabelTxt(uiLabelMonth,Util.GetLocalMonth());
	UIHelper.SetLabelTxt(uiLabelCheckInSum,infoData.n);

	--delete old items
	for k,v in pairs(uiCheckInItemList) do
        GameObjectSetActive(v.gameObject,false);
        GameObjectDestroy(v.gameObject);
        uiCheckInItemList[k] = nil;
    end
    uiCheckInItemList = {};

	local CheckInItemPrefab = windowComponent:GetPrefab(CheckInSettings.prefabNameCheckInItem);

	--create new items
	for i,v in ipairs(localData) do
		--generate one
		uiCheckInItemList[i] = {};
		uiCheckInItemList[i].gameObject = GameObjectInstantiate(CheckInItemPrefab);
		uiCheckInItemList[i].transform = uiCheckInItemList[i].gameObject.transform;
		uiCheckInItemList[i].transform.parent = uiContainer;
		uiCheckInItemList[i].transform.localPosition = Vector3.zero;
		uiCheckInItemList[i].transform.localScale = Vector3.one;

		uiCheckInItemList[i].Button     = uiCheckInItemList[i].transform;
		uiCheckInItemList[i].Icon       = TransformFindChild(uiCheckInItemList[i].transform, "IconRoot/Icon");
		uiCheckInItemList[i].Num        = TransformFindChild(uiCheckInItemList[i].transform, "Num");
		uiCheckInItemList[i].Mark       = TransformFindChild(uiCheckInItemList[i].transform, "Mark");
		uiCheckInItemList[i].VipMark    = TransformFindChild(uiCheckInItemList[i].transform, "VipMark");
		uiCheckInItemList[i].VipMarkSpr = TransformFindChild(uiCheckInItemList[i].transform, "VipMark/VipMarkSpr");
		uiCheckInItemList[i].Mask       = TransformFindChild(uiCheckInItemList[i].transform, "Mask");
        uiCheckInItemList[i].TodayMask  = TransformFindChild(uiCheckInItemList[i].transform, "TodayMask");

		local listener = Util.AddClick(uiCheckInItemList[i].Button.gameObject,OnClickCheckInItem);
		listener.parameter = {dayIndex = i};

		UIHelper.SetDragScrollViewTarget(uiCheckInItemList[i].Button,uiScrollView);
        local vItemNum = tonumber(v.item);
        if (vItemNum >= 50000 and vItemNum < 70000) then
            -- Hero
            Util.SetUITexture(uiCheckInItemList[i].Icon, LuaConst.Const.PlayerHeadIcon, HeroData.GetHeroIcon(v.item), false);
        elseif (vItemNum >= 150000 and vItemNum < 170000) then
            -- Hero fragment
            Util.SetUITexture(uiCheckInItemList[i].Icon, LuaConst.Const.PlayerHeadIcon, HeroData.GetHeroIcon(tostring(vItemNum - 100000)), false);
        else
            if (localItemDict[v.item]) then
                Util.SetUITexture(uiCheckInItemList[i].Icon, LuaConst.Const.ItemIcon, localItemDict[v.item].icon, true);
            end
        end
		UIHelper.SetLabelTxt(uiCheckInItemList[i].Num,"x"..v.num);
		UpdateCheckInItem( infoData, localData, i );
	end

	UIHelper.RepositionGrid(uiContainer,uiScrollView);
    UIHelper.RefreshPanel(uiScrollView);

end

function UpdateCheckInItem(infoData, localData, index)
	local indexToday = nil;
	if (infoData.s == 0) then
		--not signed today
		indexToday = infoData.n + 1;
	else
		--signed today
		indexToday = infoData.n;
	end
	--set vip mark
	if (localData[index].double == 1) then
		GameObjectSetActive(uiCheckInItemList[index].VipMark.gameObject, true);
		UIHelper.SetLabelTxt(uiCheckInItemList[index].VipMark, GetLocalizedString(CheckInSettings.strCheckInBonus, localData[index].vip));
	else
		GameObjectSetActive(uiCheckInItemList[index].VipMark.gameObject, false);
	end
	--set button status
	if(index < indexToday) then
		GameObjectSetActive(uiCheckInItemList[index].Mask,true);
		GameObjectSetActive(uiCheckInItemList[index].Mark,true);
		-- UIHelper.SetSpriteName(uiCheckInItemList[index].Mark,CheckInSettings.sprNameSignGotFull);
	elseif(index == indexToday) then
        GameObjectSetActive(uiCheckInItemList[index].TodayMask, true);
		if(infoData.s == 0) then
			GameObjectSetActive(uiCheckInItemList[index].Mask,false);
			GameObjectSetActive(uiCheckInItemList[index].Mark,false);
			UIHelper.SetButtonActive(uiButtonCheckInToday,true,true);
		elseif(infoData.s == 1 and localData[index].double == 1) then
			GameObjectSetActive(uiCheckInItemList[index].Mask,false);
			GameObjectSetActive(uiCheckInItemList[index].Mark,true);
			-- UIHelper.SetSpriteName(uiCheckInItemList[index].Mark,CheckInSettings.sprNameSignGotHalf);
			UIHelper.SetButtonActive(uiButtonCheckInToday,true,true);
		else
			GameObjectSetActive(uiCheckInItemList[index].Mask,true);
			GameObjectSetActive(uiCheckInItemList[index].Mark,true);
			-- UIHelper.SetSpriteName(uiCheckInItemList[index].Mark,CheckInSettings.sprNameSignGotFull);
			UIHelper.SetButtonActive(uiButtonCheckInToday,false,true);
            UIHelper.SetButtonSpriteName(uiButtonCheckInToday, "CommonBtnGray");
            UIHelper.SetWidgetColor(uiLabelCheckInTodayContent, Color.black);
		end
	else
		--future items
		GameObjectSetActive(uiCheckInItemList[index].Mask,false);
		GameObjectSetActive(uiCheckInItemList[index].Mark,false);
	end

end

--name,icon,num,desc,dayindex
function OnClickCheckInItem(go)
	--show details
	local listener = UIHelper.GetUIEventListener(go);
	if (listener ~= nil and listener.parameter.dayIndex ~= nil) then
		local localData = GameCheckInsData.Get_LocalData();
		local id = localData[listener.parameter.dayIndex].item;
        local idNum = tonumber(id);

        if (idNum >= 50000 and idNum < 70000) then
            local haveHeroNum = 0;
            if (Hero.IsContainPlayer(id)) then
                haveHeroNum = 1;
            end
            UIGameCheckInsItemTips.CreateItemTipsBox(
                uiPopupRoot,
                windowComponent,
                HeroData.GetHeroName(id),
                HeroData.GetHeroIcon(id),
                localData[listener.parameter.dayIndex].num,
                haveHeroNum,
                HeroData.GetHeroDesc(id),
                listener.parameter.dayIndex
            );
        elseif (idNum >= 150000 and idNum < 170000) then
            UIGameCheckInsItemTips.CreateItemTipsBox(
                uiPopupRoot,
                windowComponent,
                localItemDict[id].name,
                HeroData.GetHeroIcon(tostring(idNum - 100000)),
                localData[listener.parameter.dayIndex].num,
                ItemSys.GetItemData(id).num,
                localItemDict[id].desc,
                listener.parameter.dayIndex
            );
		elseif (localItemDict[id] == nil or IsTableEmpty(localItemDict[id])) then
            UIGameCheckInsItemTips.CreateItemTipsBox(
                uiPopupRoot,
                windowComponent,
                "本地item.json缺少道具："..id,
                "缺数据",
                localData[listener.parameter.dayIndex].num,
                ItemSys.GetItemData(id).num,
                "缺数据",
                listener.parameter.dayIndex
            );
		else
			UIGameCheckInsItemTips.CreateItemTipsBox(
                uiPopupRoot,
                windowComponent,
                localItemDict[id].name,
                localItemDict[id].icon,
                localData[listener.parameter.dayIndex].num,
                ItemSys.GetItemData(id).num,
                localItemDict[id].desc,
                listener.parameter.dayIndex
            );
		end
	else

	end
end

function OnClickCheckInToday()
	local infoData = GameCheckInsData.Get_InfoData();
	local localData = GameCheckInsData.Get_LocalData();
	if(infoData.s == 0 or (localData[infoData.n].double == 1 and Role.Get_vip() >= localData[infoData.n].vip) )then
		local AfterRequestCheckInToday = function ()
			-- update local data
			GameCheckInsData.Update_InfoData();
			--find today's index
			local dayIndex = nil;
			if(infoData.s==0)then
				--not signed today
				dayIndex = infoData.n+1;
			else
				--signed today
				dayIndex = infoData.n;
			end
			--update today's item
			UpdateCheckInItem( infoData, localData, dayIndex );
			UIHelper.SetLabelTxt(uiLabelCheckInSum,infoData.n);

            -- update getItem number
            local checkInData = GameCheckInsData.Get_CheckInData();
            local itemNum = localData[infoData.n].num;
            if (checkInData[2] - checkInData[1] == 2) then
                itemNum = 2 * itemNum;
            end
			ShowGetItemBox(localData[infoData.n].item, localData[infoData.n].item, itemNum);
		end
		GameCheckInsData.RequestCheckInToday(AfterRequestCheckInToday);
	elseif(infoData.s == 1 and (localData[infoData.n].double == 1 and Role.Get_vip() < localData[infoData.n].vip) )then
		UIGameCheckInsBonusHint.CreateHintBox(uiPopupRoot,windowComponent,localData[infoData.n].vip);
	end
end

function ShowGetItemBox(name,icon,num)
	UIGameCheckInsGetItemBox.CreateGetItemBox(uiPopupRoot,windowComponent,name,icon,num);
end

function OnClickHint()
	print("===== > OnClickHint : ");
	WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirmCheckInRule, { GetLocalizedString(CheckInSettings.strCheckInRules) });
end

function OnClickClose()
	if(GameObjectActiveSelf(checkInBox)) then
    	GameObjectSetActive(checkInBox.transform,false);
    end
end

function OnDestroy()

	UIGameCheckInsBonusHint.OnDestroy();
	UIGameCheckInsGetItemBox.OnDestroy();
	UIGameCheckInsItemTips.OnDestroy();

	checkInBox = nil;
	uiButtonCheckInToday = nil;
	uiButtonClose = nil;
	uiButtonHint = nil;
	uiLabelMonth = nil;
	uiLabelCheckInSum = nil;
	uiScrollView = nil;
	uiContainer = nil;
	uiCheckInItemList = {};
	uiPopupRoot = nil;
	windowComponent = nil;
end
