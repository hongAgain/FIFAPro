module("UIDailyCupMyGamBox",package.seeall);

require "Config"
require "Common/UnityCommonScript"
require "Game/GameMainScript"
require "Game/Role"
require "Game/ItemSys"
require "Game/Hero"

--require "UILua/UIDailyCupScript"
require "UILua/UIGetItemScript"

local myGamBox = nil;

local myGamBoxPrefabName = "DailyCupMyGamBox"
local gamItemPrefabName = "MyGambleItem"
-- local gamDatePrefabName = "DateItem"
local buttonConfirm = nil;
local buttonConfirmContent = nil;
local buttonClose = nil;

local uiScrollView = nil;
local uiContainer = nil;
local items = {};

local windowComponent = nil;

function CreateMyGamBox(containerTransform,windowcomponent,depth)
	if(myGamBox == nil) then
		--instantiate one
		local myGamBoxPrefab = windowcomponent:GetPrefab(myGamBoxPrefabName);
		--instantiate prefab and initialize it
		myGamBox = GameObjectInstantiate(myGamBoxPrefab);
		myGamBox.transform.parent = containerTransform;
    	myGamBox.transform.localPosition = Vector3.zero;
    	myGamBox.transform.localScale = Vector3.one;

    	buttonConfirm = TransformFindChild(myGamBox.transform,"ButtonConfirm");
    	buttonConfirmContent = TransformFindChild(buttonConfirm,"Label");
    	buttonClose = TransformFindChild(myGamBox.transform,"ButtonClose");
    	Util.AddClick(buttonConfirm.gameObject,OnClickGetReward);
    	Util.AddClick(buttonClose.gameObject,OnClickClose);

    	uiScrollView = TransformFindChild(myGamBox.transform,"uiScrollView");
    	uiContainer = TransformFindChild(uiScrollView,"uiContainer");
	end
	--active it
	if (not GameObjectActiveSelf(myGamBox)) then
    	GameObjectSetActive(myGamBox.transform,true);
    end

	windowComponent = windowcomponent;
	UIHelper.SetPanelDepth(uiScrollView,depth+1);

	--set info
	SetInfo();
end

function SetInfo()

	DestroyUIListItemGameObjects(items);
	items = {};

	--sort data
	-- local listData = PVPMsgManager.Get_DailyCupGlData();
	local listData = PVPMsgManager.GetPVPData(MsgID.tb.DailyCupGl);

	if(listData==nil or listData.list == nil)then
		return;
	end

	if(HasNoAvailableGamResult(listData))then
		SetConfirmButtonActive(false);
	else
		SetConfirmButtonActive(true);
	end

	local sortedList = {};
	local index = 1;
	for k,v in pairs(listData.list) do
		sortedList[index] = v;
		index = index + 1;
	end
	table.sort(sortedList,
		function (a,b)
			local astring = a.d:split("-");
			local ay = tonumber(astring[1]);
			local am = tonumber(astring[2]);
			local ad = tonumber(astring[3]);

			local bstring = b.d:split("-");
			local by = tonumber(bstring[1]);
			local bm = tonumber(bstring[2]);
			local bd = tonumber(bstring[3]);

			if(ay < by) then
				return true;
			elseif(ay == by) then
				if(am < bm) then
					return true;
				elseif(am == bm) then
					if(ad < bd) then
						return true;
					elseif(ad == bd) then
						if(a.r < b.r) then
							return true;
						end
					end
				end
			end
			return false;
	end);

	local lastDate = nil;
	local lastYPos = 0;
	local lastIndex = 1;

	local gamItemPrefab = windowComponent:GetPrefab(gamItemPrefabName);
	-- local gamDatePrefab = windowComponent:GetPrefab(gamDatePrefabName);

	for i,v in ipairs(sortedList) do
		-- if(lastDate~=v.d)then
		-- 	-- add date title here, and record latest
		-- 	items[lastIndex] = {};
		-- 	items[lastIndex].gameObject = GameObjectInstantiate(gamDatePrefab);
		-- 	items[lastIndex].transform = items[lastIndex].gameObject.transform;
		-- 	items[lastIndex].transform.parent = uiContainer;
		--  items[lastIndex].transform.localPosition = NewVector3(0,-lastYPos,0);
		--  items[lastIndex].transform.localScale = Vector3.one;

		--  local dateStrings = v.d:split("-");
		--  UIHelper.SetLabelTxt(items[lastIndex].transform,
		--    GetLocalizedString(UIDailyCupScript.dailyCupSettings.MonthAndDay,dateStrings[2],dateStrings[3]));

		-- 	lastDate = v.d;
		-- 	lastYPos = lastYPos+30;
		-- 	lastIndex = lastIndex+1;
		-- end
		-- if( v.f == 0 and v.result == v.win )then
			--won the gamble and prize not gained status, add this item
			items[lastIndex] = {};
			items[lastIndex].gameObject = GameObjectInstantiate(gamItemPrefab);
			items[lastIndex].transform = items[lastIndex].gameObject.transform;
			items[lastIndex].transform.parent = uiContainer;
	    	items[lastIndex].transform.localPosition = NewVector3(0,-lastYPos,0);
	    	items[lastIndex].transform.localScale = Vector3.one;

	    	Util.SetUITexture(
                TransformFindChild(items[lastIndex].transform, "ButtonGam/IconRoot/Icon"),
                LuaConst.Const.PlayerHeadIcon,
                HeroData.GetHeroIcon(tostring(v.hid)),
                true);
                
	    	UIHelper.SetLabelTxt(TransformFindChild(items[lastIndex].transform,"ButtonGam/Name"),HeroData.GetHeroName(tostring(v.hid)));
	    	
	    	local dateStrings = v.d:split("-");
	    	UIHelper.SetLabelTxt(TransformFindChild(items[lastIndex].transform,"Date"),
	    		GetLocalizedString(UIDailyCupScript.dailyCupSettings.MonthAndDay,dateStrings[2],dateStrings[3]));
	    	local uiStatus = TransformFindChild(items[lastIndex].transform,"Status");
	    	if(v.result==0)then
	    		--not finished
	    		UIHelper.SetLabelTxt(uiStatus,
	    			GetLocalizedString(UIDailyCupScript.dailyCupSettings.MatchNotFinished));
	    		UIHelper.SetWidgetColor(uiStatus,Color.New(1,1,1,1));
	    	else
	    		--finished
				UIHelper.SetLabelTxt(uiStatus,
					GetLocalizedString(UIDailyCupScript.dailyCupSettings.MatchFinished));
	    		UIHelper.SetWidgetColor(uiStatus,Color.New(0.5,0.5,0.5,1));
	    	end
	    	for i=1,2 do
	    		local player = TransformFindChild(items[lastIndex].transform,"Player"..i);
	    		local userinfo = listData.user[v.user[i]];
	    		local name = TransformFindChild(player,"Name");
	    		local icon = TransformFindChild(player,"IconRoot/Icon");
	    		local isMyTeam = TransformFindChild(player,"IsMyTeam");
	    		local score = TransformFindChild(player,"Score");
	    		local result = TransformFindChild(player,"Result");

	    		UIHelper.SetLabelTxt(name,userinfo.name);
	    		Util.SetUITexture(icon, LuaConst.Const.ClubIcon, tostring(userinfo.icon).."_2", true);
	    		GameObjectSetActive(isMyTeam,(userinfo._id==Role.Get_uid()));

	    		if(v.result==0)then
	    			--not finished
					GameObjectSetActive(score,false);	    
	    			if(v.win==i)then
	    				--gam on this player
	    				GameObjectSetActive(result,true);
	    				UIHelper.SetWidgetColor(result,Color.New(52/255,255/255,31/255,1));	    	
	    				UIHelper.SetLabelTxt(result,GetLocalizedString(UIDailyCupScript.dailyCupSettings.DailyCupGamSupport));
	    			else
	    				GameObjectSetActive(result,false);
	    			end
	    		else
	    			--finished
	    			UIHelper.SetLabelTxt(score,v.score[i]);
	    			if(v.win == i)then
		    			--i gammed on this player
		    			GameObjectSetActive(result,true);
		    			if(v.win == v.result)then
		    				--won the gam
							UIHelper.SetWidgetColor(result,UIDailyCupScript.dailyCupSettings.DailyCupGamColorWon);
		    				UIHelper.SetLabelTxt(result,GetLocalizedString(UIDailyCupScript.dailyCupSettings.DailyCupGamWon));	    			
		    			else
		    				--lose the gam
							UIHelper.SetWidgetColor(result,UIDailyCupScript.dailyCupSettings.DailyCupGamColorLose);
		    				UIHelper.SetLabelTxt(result,GetLocalizedString(UIDailyCupScript.dailyCupSettings.DailyCupGamLose));
		    			end
						
	    				else
	    				GameObjectSetActive(result,false);
		    		end
	    		end
	    	end

	    	lastYPos = lastYPos+150;
			lastIndex = lastIndex+1;
		-- end
	end

    -- UIHelper.RepositionGrid(uiContainer,uiScrollView);
    UIHelper.ResetScroll(uiScrollView);
    UIHelper.RefreshPanel(uiScrollView);
end

function HasNoAvailableGamResult(listData)
	for k,v in pairs(listData.list) do
		if(v.result ~= nil and v.result ~= 0)then
			if(v.win == v.result)then
				return false;
			end
		end
	end
	return true;
end

function SetConfirmButtonActive(willActivate)
	if(willActivate)then
		UIHelper.SetWidgetColor(buttonConfirm,Color.New(108/255,161/255,101/255,255/255));
		UIHelper.SetWidgetColor(buttonConfirmContent,Color.New(108/255,161/255,101/255,255/255));
	else
		UIHelper.SetWidgetColor(buttonConfirm,Color.New(125/255,128/255,137/255,255/255));
		UIHelper.SetWidgetColor(buttonConfirmContent,Color.New(125/255,128/255,137/255,255/255));
	end
	UIHelper.SetBoxCollider(buttonConfirm,willActivate);
end

function OnClickGetReward()
	local AfterShownList = function () Close(); end
	local AfterReq = function ()
		WindowMgr.ShowWindow(LuaConst.Const.UIGetItem,{
                m_itemTb = PVPMsgManager.GetPVPData(MsgID.tb.DailyCupGAward),
                OnClose = AfterShownList
                });
		AfterShownList();
	end
	-- PVPMsgManager.RequestDailyCupGAward(nil,AfterReq);
    PVPMsgManager.RequestPVPMsg(MsgID.tb.DailyCupGAward, LuaConst.Const.DailyCupGAward, nil, AfterReq, nil );
end

function OnClickClose()
	Close();
end

function Close()
	if(GameObjectActiveSelf(myGamBox)) then
    	GameObjectSetActive(myGamBox.transform,false);
    end
end

function OnDestroy()
	myGamBox = nil;
	buttonConfirm = nil;
	buttonClose = nil;
	uiScrollView = nil;
	uiContainer = nil;
	items = {};
	windowComponent = nil;
end