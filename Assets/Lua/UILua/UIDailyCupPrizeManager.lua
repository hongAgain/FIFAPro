module("UIDailyCupPrizeManager",package.seeall);

local prizeManagerSettings = {
	DailyCupPrizePrefabName = "DailyCupPrizeInfo",
	DailyCupPrizePrefab = nil,

	SeaElectionPrizePrefabName = "SeaElectionPrizeItem",
	SeaElectionPrizePrefab = nil,
	SeaElectionPrizeLength = 80;

	KnockOutPrizePrefabName = "KnockOutPrizeItem",
	KnockOutPrizePrefab = nil,
	KnockOutPrizeLength = 170,

	RoundPrizePrefabName = "RoundPrizePrefabItem",
	RoundPrizePrefab = nil,

	roundNum = "RoundNum",
	availableKnockOutTitle = "DailyCupPrizeAvailableKnockOut",
	availableSeaElectionTitle = "DailyCupPrizeAvailableSeaElection"

};

local dailyCupPrizeUI = nil;

local knockOutPrizes = {};

local seaElectionScrollView = nil;
local seaElectionContainer = nil;
local seaElectionPrizes = {};

local availableScrollView = nil;
local availableContainer = nil;
local availablePrizes = {};
local availablePrizeCurrentLength = 0;

local buttonGet = nil;
local buttonClose = nil;
local uiPopupRoot = nil;

function CreateDailyCupPrizeBox( containerTransform, windowcomponent, depth )
	local AfterReqReward = function ()
        RealCreate( containerTransform, windowcomponent, depth );
	end
	-- PVPMsgManager.RequestDailyCupAl(nil,AfterReqReward);
	PVPMsgManager.RequestPVPMsg(MsgID.tb.DailyCupAl, LuaConst.Const.DailyCupAl, nil, AfterReqReward, nil );
end

function RealCreate( containerTransform, windowcomponent, depth )
	if(dailyCupPrizeUI == nil) then
		--Get prefabs
		prizeManagerSettings.DailyCupPrizePrefab = windowcomponent:GetPrefab(prizeManagerSettings.DailyCupPrizePrefabName);
		
		prizeManagerSettings.SeaElectionPrizePrefab = windowcomponent:GetPrefab(prizeManagerSettings.SeaElectionPrizePrefabName);
		prizeManagerSettings.KnockOutPrizePrefab = windowcomponent:GetPrefab(prizeManagerSettings.KnockOutPrizePrefabName);
		prizeManagerSettings.RoundPrizePrefab = windowcomponent:GetPrefab(prizeManagerSettings.RoundPrizePrefabName);

		--generate ui and initialize it
		dailyCupPrizeUI = GameObjectInstantiate(prizeManagerSettings.DailyCupPrizePrefab);
		dailyCupPrizeUI.transform.parent = containerTransform;
    	dailyCupPrizeUI.transform.localPosition = Vector3.zero;
    	dailyCupPrizeUI.transform.localScale = Vector3.one;

		BindUI(depth);
	end

	--active it
	if(not GameObjectActiveSelf(dailyCupPrizeUI)) then
    	GameObjectSetActive(dailyCupPrizeUI.transform,true);
    end

	SetInfo();
end

function BindUI(depth)
	local transform = dailyCupPrizeUI.transform;

	knockOutPrizes = {};
    local iconPrefab = Util.GetGameObject("UIItemIcon");
	for i = 1, 4 do
        local root = TransformFindChild(transform, "KnockOutPrize/RankPrize"..i);
        local clone = AddChild(iconPrefab, root);
        clone.transform.localPosition = Vector3.zero;
        local itemIcon = UIItemIcon.New(clone);
        itemIcon:SetSize("win_wb_20", Vector3.one * 86 / 180);
		knockOutPrizes[i] = itemIcon;
	end

	seaElectionScrollView = TransformFindChild(transform,"SeaElectionPrize/PrizeList");
	seaElectionContainer = TransformFindChild(seaElectionScrollView,"Container");

	availableScrollView = TransformFindChild(transform,"AvailablePrize/PrizeList");
	availableContainer = TransformFindChild(availableScrollView,"Container");

	buttonGet = TransformFindChild(transform,"GetAllButton");
	AddOrChangeClickParameters(buttonGet.gameObject,OnClickButtonGet,nil);

	buttonClose = TransformFindChild(transform,"CloseButton");
	AddOrChangeClickParameters(buttonClose.gameObject,OnClickCloseButton,nil);


	uiPopupRoot = TransformFindChild(transform,"PopupRoot");

	UIHelper.SetPanelDepth(seaElectionScrollView,depth+1);
	UIHelper.SetPanelDepth(availableScrollView,depth+1);
	UIHelper.SetPanelDepth(uiPopupRoot,depth+2);
end

function SetInfo()
	--set info for KnockOutPrize
	local finalData = PVPMsgManager.Get_DialyCupLocalData(Config.CupFinalAward());
	for i=1,4 do
		local dataIndex = tostring(i);
		print("这里需要设置淘汰赛奖励图标");
        knockOutPrizes[i]:Init({id = "", num = finalData[dataIndex].item[2]}, false);
	end

	--set info for SeaElection
	--fetch data
	local seaElectionData = PVPMsgManager.Get_DialyCupLocalData(Config.CupMassAward());
	local seaElectionReSort = {};
	for k,v in pairs(seaElectionData) do
		seaElectionReSort[tonumber(k)+1] = v;
	end

	--destroy old items
    for i,v in ipairs(seaElectionPrizes) do 
        GameObjectSetActive(v.gameObject,false);
        GameObjectDestroy(v.gameObject);
        seaElectionPrizes[i] = nil;
    end
    seaElectionPrizes = {};
    -- local seaElectionPrizePrefab = windowComponent:GetPrefab(prizeManagerSettings.seaElectionPrizePrefabName);
	--create new items
    for i,v in ipairs(seaElectionReSort) do
        seaElectionPrizes[i] = {};
        --instantiate prefab and initialize it
        local clone = GameObjectInstantiate(prizeManagerSettings.RoundPrizePrefab);
        clone.transform.parent = seaElectionContainer;
        clone.transform.localPosition = Vector3.zero;
        clone.transform.localScale = Vector3.one;

        --bind ui
        seaElectionPrizes[i].gameObject = clone;
        seaElectionPrizes[i].transform = clone.transform;
        seaElectionPrizes[i].RoundNum = TransformFindChild(seaElectionPrizes[i].transform,"RoundNum");
        seaElectionPrizes[i].Icon = TransformFindChild(seaElectionPrizes[i].transform,"Icon");
        seaElectionPrizes[i].PrizeNum = TransformFindChild(seaElectionPrizes[i].transform,"PrizeNum");

        --set info
        UIHelper.SetLabelTxt(seaElectionPrizes[i].RoundNum,GetLocalizedString(prizeManagerSettings.roundNum,i));
        UIHelper.SetLabelTxt(seaElectionPrizes[i].PrizeNum,v.num);
    end

    -- UIHelper.RepositionGrid(seaElectionContainer,seaElectionScrollView);

    UIHelper.SetGridPosition(
		seaElectionContainer,
		seaElectionScrollView,
		NewVector3(-204.2,-202.9,0),
		NewVector3(-180,70,0),
		true);

    UIHelper.RefreshPanel(seaElectionScrollView);


	--set info for AvailableData
    -- local finalData = PVPMsgManager.Get_DailyCupAlData();
    local finalData = PVPMsgManager.GetPVPData(MsgID.tb.DailyCupAl);
	--destroy old items
    for i,v in ipairs(availablePrizes) do 
        GameObjectSetActive(v.gameObject,false);
        GameObjectDestroy(v.gameObject);
        availablePrizes[i] = nil;
    end
    availablePrizes = {};

    if(finalData == nil or #finalData == 0)then
    	UIHelper.SetButtonActive(buttonGet,false,true);
    else
    	UIHelper.SetButtonActive(buttonGet,true,true);
	    -- local availablePrizePrefab = windowComponent:GetPrefab(prizeManagerSettings.availablePrizePrefabName);
		--create new items
		local availablePrizeIndex = 1;
	    for i,v in ipairs(finalData) do
	    	for j,x in ipairs(v) do		       
		        --set info
		        local timeStrings = x.d:split("-");
				local month = tostring(timeStrings[2]);
		        local day = tostring(timeStrings[3]);
		        
		        if(x.id==1) then
		        	--Sea Election,prize is money, use seaElectionPrizePrefab

		        	 --instantiate prefab and initialize it
		    		availablePrizes[availablePrizeIndex] = {};
			        local clone = GameObjectInstantiate(prizeManagerSettings.SeaElectionPrizePrefab);
			        clone.transform.parent = availableContainer;
			        clone.transform.localPosition = NewVector3(0, -availablePrizeCurrentLength, 0);
			        clone.transform.localScale = Vector3.one;

			        --bind ui
			        availablePrizes[availablePrizeIndex].gameObject = clone;
			        availablePrizes[availablePrizeIndex].transform = clone.transform;
			        availablePrizes[availablePrizeIndex].Title = TransformFindChild(availablePrizes[availablePrizeIndex].transform,"Title");
			        availablePrizes[availablePrizeIndex].ItemIcon = TransformFindChild(availablePrizes[availablePrizeIndex].transform,"IconRoot/Icon");
			        availablePrizes[availablePrizeIndex].CurrencyNum = TransformFindChild(availablePrizes[availablePrizeIndex].transform,"Num");


		        	GameObjectSetActive(availablePrizes[availablePrizeIndex].ItemIcon,false);
		        	GameObjectSetActive(availablePrizes[availablePrizeIndex].CurrencyNum,true);
		        	UIHelper.SetLabelTxt(availablePrizes[availablePrizeIndex].Title,
		        		GetLocalizedString(prizeManagerSettings.availableSeaElectionTitle,month,day));
		        	--take the only data from x.item
		        	for k,y in pairs(x.item) do
		        		UIHelper.SetLabelTxt(availablePrizes[availablePrizeIndex].CurrencyNum,tostring(y));
		        		break;
		        	end

		        	availablePrizeCurrentLength = availablePrizeCurrentLength+prizeManagerSettings.SeaElectionPrizeLength;
		        elseif(x.id==2) then
		        	--Knock Out, prize is an item with icon, use knockOutPrizePrefab

					--instantiate prefab and initialize it
		    		availablePrizes[availablePrizeIndex] = {};
			        local clone = GameObjectInstantiate(prizeManagerSettings.KnockOutPrizePrefab);
			        clone.transform.parent = availableContainer;
			        clone.transform.localPosition = NewVector3(0, -availablePrizeCurrentLength, 0);
			        clone.transform.localScale = Vector3.one;

			        --bind ui
			        availablePrizes[availablePrizeIndex].gameObject = clone;
			        availablePrizes[availablePrizeIndex].transform = clone.transform;
			        availablePrizes[availablePrizeIndex].Title = TransformFindChild(availablePrizes[availablePrizeIndex].transform,"Title");
			        availablePrizes[availablePrizeIndex].ItemIcon = TransformFindChild(availablePrizes[availablePrizeIndex].transform,"ItemIcon");
			        availablePrizes[availablePrizeIndex].Num = TransformFindChild(availablePrizes[availablePrizeIndex].transform,"Num");


		        	GameObjectSetActive(availablePrizes[availablePrizeIndex].ItemIcon,true);
		        	GameObjectSetActive(availablePrizes[availablePrizeIndex].Num,false);

		        	UIHelper.SetLabelTxt(availablePrizes[availablePrizeIndex].Title,
		        		GetLocalizedString(prizeManagerSettings.availableKnockOutTitle,month,day));
		        	print("Remember To set icon here after itemsystem is done");
		        	-- UIHelper.SetSpriteName(availablePrizes[availablePrizeIndex].ItemIcon,"");
		        	for k,y in pairs(x.item) do
		        		UIHelper.SetLabelTxt(availablePrizes[availablePrizeIndex].Num,"x"..tostring(y));
		        		break;
		        	end

		        	availablePrizeCurrentLength = availablePrizeCurrentLength+prizeManagerSettings.KnockOutPrizeLength;
		        end
		        availablePrizeIndex = availablePrizeIndex+1;
	    	end        
	    end
	end

    UIHelper.ResetScroll(availableScrollView)
    UIHelper.RefreshPanel(availableScrollView);

end

function OnClickButtonGet(go)
	local listener = UIHelper.GetUIEventListener(go);
		--show sth indicating non-signed up status
	local AfterReqGetReward = function ()
		--show item list
		-- WindowMgr.ShowWindow(LuaConst.Const.UIGetItem,{
		--		m_itemTb = PVPMsgManager.GetPVPData(MsgID.tb.DailyCupAward),
		--		OnClose = nil
		--	});

		--destroy old items
	    for i,v in ipairs(availablePrizes) do 
	        GameObjectSetActive(v.gameObject,false);
	        GameObjectDestroy(v.gameObject);
	        availablePrizes[i] = nil;
	    end
	    availablePrizes = {};
	    UIHelper.ResetScroll(availableScrollView)
	    UIHelper.RefreshPanel(availableScrollView);

		UIHelper.SetButtonActive(buttonGet,false,true);
	end
	-- PVPMsgManager.RequestDailyCupAward(nil,AfterReqGetReward);	
	PVPMsgManager.RequestPVPMsg(MsgID.tb.DailyCupAward, LuaConst.Const.DailyCupAward, nil, AfterReqGetReward, nil );
end

function OnClickCloseButton()
	Close();
end

function Close()
	if(GameObjectActiveSelf(dailyCupPrizeUI)) then
    	GameObjectSetActive(dailyCupPrizeUI.transform,false);
    end
end

function OnDestroy()
	dailyCupPrizeUI = nil;
	knockOutPrizes = {};
	seaElectionScrollView = nil;
	seaElectionContainer = nil;
	seaElectionPrizes = {};
	availableScrollView = nil;
	availableContainer = nil;
	availablePrizes = {};
	availablePrizeCurrentLength = 0;
	buttonGet = nil;
	buttonClose = nil;
	uiPopupRoot = nil;
end