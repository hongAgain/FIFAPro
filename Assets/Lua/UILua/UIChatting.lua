module("UIChatting",package.seeall)

require "Common/UnityCommonScript"
require "Game/ChattingData"
require "Game/HintManager"


local chatSettings = {
	playerChatItemLeftPrefabName = "PlayerChatItemLeft",
	playerChatItemLeftPrefab = nil,
	playerChatItemRightPrefabName = "PlayerChatItemRight",
	playerChatItemRightPrefab = nil,
	SystemChatItemLeftPrefabName = "SystemChatItem",
	SystemChatItemLeftPrefab = nil,
	TimeItemPrefabName = "TimeItem",
	TimeItemPrefab = nil,

	ConfirmCostPrefabName = "ConfirmCost",
	ConfirmCostPrefab = nil,

	CostChances = "CostChances",

	SystemChatPlayerNameStylePreFix = "[ffffff][u]",
	SystemChatPlayerNameStyleSufFix = "[-][/u]",
	SystemChatContentStylePreFix = "[ffde00]",
	SystemChatContentStyleSufFix = "[-]",
	SystemChatItemNameStylePreFix = "[6ca165][u]",
	SystemChatItemNameStyleSufFix = "[-][/u]",

	ColorChannelSelected = Color.New(255/255,255/255,255/255,255/255),
	ColorChannelNormal = Color.New(125/255,128/255,137/255,255/255),

	initMsgNumToReq = 20,
	pollMsgNumToReq = 5,

	scrollViewPos = NewVector3(0,-30,0),
	anchorDefaultPos = 200,
	anchorStartLimit = -320,
	anchorShiftOffset = 204,
	timeInterval = 60000,

	--chinese takes 3times length
	-- msgLengthLimitMax = 300;

	buttonNameShortCut = {},
	strSpeakToWhom = "SpeakToWhom",
	strNoReceiverSelected = "NoReceiverSelected",
	strFreeChances = "FreeChances",


}

local channelButtons = {};

local contentInput = nil;
local contentInputBlock = nil;

local paymentHint = nil;
local channelHint = nil;
local receiverHint = nil;

local buttonClose = nil;
local buttonSendMsg = nil;

local currentChannel = nil;
local currentReceiver = {};
local currentItemTime = nil;

local pollingTimerID = nil;

local uiScrollView = nil;
local uiContainer = nil;
local currentPosAnchor = 0;
local chatItems = {};

local window = nil;
local windowComponent = nil;

function OnStart(gameObject, params)
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");

	PrepareLocalData();
    BindUI();

    ChattingData.RegisterOnPaymentInfoChanged(OnPaymentInfoChanged);

	SwitchChannel((params and params.targetChannelID) or 0);
	if(params~=nil and params.DefaultTargetPlayerUID~=nil and params.DefaultTargetPlayerName~=nil)then
		SetReceiver(params.DefaultTargetPlayerUID,params.DefaultTargetPlayerName,params.targetChannelID == 2);
    end
    --prepare for polling
	StartPolling();
end

function OnShowUpdate(params)
	print("OnShowUpdate:"..params.targetChannelID);
	SwitchChannel((params and params.targetChannelID) or 0);
	if(params~=nil and params.DefaultTargetPlayerUID~=nil and params.DefaultTargetPlayerName~=nil)then
		SetReceiver(params.DefaultTargetPlayerUID,params.DefaultTargetPlayerName,params.targetChannelID == 2);
    end
end

function PrepareLocalData()
	chatSettings.buttonNameShortCut = {};
	chatSettings.buttonNameShortCut[0] = GetLocalizedString("ChannelWorldShorCut");
	chatSettings.buttonNameShortCut[1] = GetLocalizedString("ChannelGuildShorCut");
	chatSettings.buttonNameShortCut[2] = GetLocalizedString("ChannelPrivateShorCut");
end

function BindUI()
	local transform = window.transform;	

    BindChannelButton("ButtonChannelWorld",0);
    BindChannelButton("ButtonChannelGuild",1);
	-- channelButtons[1] = {};
	-- channelButtons[1].transform = TransformFindChild(window.transform,"ButtonChannelGuild");
 --    channelButtons[1].gameObject = channelButtons[1].transform.gameObject;
 --    channelButtons[1].title = TransformFindChild(channelButtons[1].transform,"Title");
 --    channelButtons[1].selected = TransformFindChild(channelButtons[1].transform,"Selected");
 --    GameObjectSetActive(channelButtons[1].selected,false);
	-- UIHelper.SetWidgetColor(channelButtons[1].title,chatSettings.ColorChannelNormal);
    BindChannelButton("ButtonChannelPrivate",2);
	contentInput = TransformFindChild(transform,"ContentInput");
	contentInputBlock = TransformFindChild(transform,"ContentInputBlock");

	paymentHint = {};
	paymentHint.CostTimeHint = {};
	paymentHint.CostTimeHint.transform = TransformFindChild(transform,"Payment/CostTimeHint");
	paymentHint.CostTimeHint.price = TransformFindChild(paymentHint.CostTimeHint.transform,"Price");
	paymentHint.CostTimeHint.icon = TransformFindChild(paymentHint.CostTimeHint.transform,"IconRoot/Icon");
	GameObjectSetActive(paymentHint.CostTimeHint.icon,false);
	paymentHint.CostTimeHint.tempIcon = TransformFindChild(paymentHint.CostTimeHint.transform,"TempIcon");
	paymentHint.FreeTimeHint = TransformFindChild(transform,"Payment/FreeTimeHint");
	GameObjectSetActive(paymentHint.CostTimeHint.transform,false);
	GameObjectSetActive(paymentHint.FreeTimeHint.transform,false);

	channelHint = TransformFindChild(transform,"ChannelHint");
	receiverHint = TransformFindChild(transform,"ReceiverHint");
	buttonClose = TransformFindChild(transform,"ButtonClose");
	buttonSendMsg = TransformFindChild(transform,"ButtonSend");
	uiScrollView = TransformFindChild(transform,"uiScrollView");
	uiContainer = TransformFindChild(uiScrollView,"uiContainer");

	AddOrChangeClickParameters(contentInputBlock.gameObject,OnClickContentInputBlock,nil);
	AddOrChangeClickParameters(buttonSendMsg.gameObject,OnClickButtonSend,nil);
	AddOrChangeClickParameters(buttonClose.gameObject,OnClickButtonClose,nil);
	currentPosAnchor = 0;
end

function BindChannelButton(buttonName,channelid)
	channelButtons[channelid] = {};
	channelButtons[channelid].transform = TransformFindChild(window.transform,buttonName);
    channelButtons[channelid].gameObject = channelButtons[channelid].transform.gameObject;
    channelButtons[channelid].title = TransformFindChild(channelButtons[channelid].transform,"Title");
    channelButtons[channelid].selected = TransformFindChild(channelButtons[channelid].transform,"Selected");
    AddOrChangeClickParameters(channelButtons[channelid].gameObject,OnClickChannel,{channelID = channelid});
    GameObjectSetActive(channelButtons[channelid].selected,false);
end

function SetChannelActive(channelid,willActive)
	if(channelButtons[channelid]~=nil and channelButtons[channelid]~={} and channelButtons[channelid].selected~=nil)then
		GameObjectSetActive(channelButtons[channelid].selected,willActive);
		if(willActive)then
			UIHelper.SetWidgetColor(channelButtons[channelid].title,chatSettings.ColorChannelSelected);
		else
			UIHelper.SetWidgetColor(channelButtons[channelid].title,chatSettings.ColorChannelNormal);
		end
	end
end

function StartPolling()
	pollingTimerID = LuaTimer.AddTimer(true,-10,DoPolling);
end

function DoPolling()
	local AfterRequest = function ()
		SetInfo()
	end
	ChattingData.RequestMsgList( {channel = currentChannel,num = chatSettings.pollMsgNumToReq}, AfterRequest, true );
end

function StopPolling()
	if(pollingTimerID~=nil)then
		LuaTimer.RmvTimer(pollingTimerID);
	end
end

function ClearMsg()
	--clear what you said
	UIHelper.SetInputText(contentInput,"");
end

function SetInfo()
	--get data for channel currentChannelID
	local needRefreshScrollView = false;	

	local msgDict = ChattingData.GetChannelMsgList(currentChannel);
	local msgList = {};
	for k,v in pairs(msgDict) do
		if(v.needCreate)then

			if(not needRefreshScrollView)then
				needRefreshScrollView = true;
			end
			table.insert(msgList,v);
		end
	end
	table.sort(msgList,function (a,b)
		return a.time<b.time;
	end);

	for i,v in ipairs(msgList) do

		--check time
		if(currentItemTime==nil or v.time - currentItemTime > chatSettings.timeInterval)then
			if(chatSettings.TimeItemPrefab == nil)then
				chatSettings.TimeItemPrefab = windowComponent:GetPrefab(chatSettings.TimeItemPrefabName);
			end
			local timeitemKey = v.time.."timeStamp";
			chatItems[timeitemKey] = {};
	        --instantiate
	        local clone = GameObjectInstantiate(chatSettings.TimeItemPrefab);
	        clone.transform.parent = uiContainer;
	        clone.transform.localPosition = NewVector3(0,currentPosAnchor,0);
	        clone.transform.localScale = Vector3.one;

	        chatItems[timeitemKey].gameObject = clone;
	        chatItems[timeitemKey].transform = clone.transform;
	        chatItems[timeitemKey].chatData = v.time;

	        UIHelper.SetLabelTxt(chatItems[timeitemKey].transform,os.date("%H:%M",v.time/1000));

    		currentPosAnchor = currentPosAnchor - 40;
			currentItemTime = v.time;
		end
		--set item
		if(v.type == 1)then
			--this is a broadcast
			if(chatSettings.SystemChatItemLeftPrefab == nil)then
				chatSettings.SystemChatItemLeftPrefab = windowComponent:GetPrefab(chatSettings.SystemChatItemLeftPrefabName);
			end
			local chatItem = {};
	        --instantiate
	        local clone = GameObjectInstantiate(chatSettings.SystemChatItemLeftPrefab);
	        clone.transform.parent = uiContainer;
	        clone.transform.localPosition = NewVector3(0,currentPosAnchor,0);
	        clone.transform.localScale = Vector3.one;

	        chatItem.gameObject = clone;
	        chatItem.transform = clone.transform;
	        chatItem.chatData = v;

			chatItem.textBG = TransformFindChild(chatItem.transform,"textBG");
	        chatItem.Content = TransformFindChild(chatItem.transform,"Content");
	        --set ui info
	        SetItemBGAndPosAnchor(chatItem.Content, chatItem.textBG, ProcessBroadCastMsg(v.id,v.args,v.uinfo));
            chatItems[v.time] = chatItem;
		elseif(v.type == 2)then
			local chatItem = {};
			--this is a chatting msg
			if(v.sender == Role.Get_uid())then
				--this is mine
				if(chatSettings.playerChatItemRightPrefab == nil)then
					chatSettings.playerChatItemRightPrefab = windowComponent:GetPrefab(chatSettings.playerChatItemRightPrefabName);
				end				
		        --instantiate
		        local clone = GameObjectInstantiate(chatSettings.playerChatItemRightPrefab);
		        clone.transform.parent = uiContainer;
		        clone.transform.localPosition = NewVector3(0,currentPosAnchor,0);
		        clone.transform.localScale = Vector3.one;

		        chatItem.gameObject = clone;
		        chatItem.transform = clone.transform;
		        chatItem.chatData = v;
		        chatItem.ToWhom = TransformFindChild(chatItem.transform,"ToWhom");
		        chatItem.Icon = TransformFindChild(chatItem.transform,"IconRoot/Icon");
		        chatItem.Vip = TransformFindChild(chatItem.transform,"Vip");
		        chatItem.Content = TransformFindChild(chatItem.transform,"Content");
		        -- chatItem.ButtonIcon = TransformFindChild(chatItem.transform,"ButtonIcon");
				chatItem.textBG = TransformFindChild(chatItem.transform,"textBG");
			else
				--this is others
				if(chatSettings.playerChatItemLeftPrefab == nil)then
					chatSettings.playerChatItemLeftPrefab = windowComponent:GetPrefab(chatSettings.playerChatItemLeftPrefabName);
				end
				--instantiate
		        local clone = GameObjectInstantiate(chatSettings.playerChatItemLeftPrefab);
		        clone.transform.parent = uiContainer;
		        clone.transform.localPosition = NewVector3(0,currentPosAnchor,0);
		        clone.transform.localScale = Vector3.one;

		        chatItem.gameObject = clone;
		        chatItem.transform = clone.transform;
		        chatItem.chatData = v;
		        chatItem.Name = TransformFindChild(chatItem.transform,"Name");
		        chatItem.Icon = TransformFindChild(chatItem.transform,"IconRoot/Icon");
		        chatItem.Vip = TransformFindChild(chatItem.transform,"Vip");
		        chatItem.Content = TransformFindChild(chatItem.transform,"Content");
		        chatItem.ButtonIcon = TransformFindChild(chatItem.transform,"ButtonIcon");
				chatItem.textBG = TransformFindChild(chatItem.transform,"textBG");		        
			end
            chatItems[v.time] = chatItem;
			--set ui info		        
	        SetChattingItemInfo(chatItem, v);
		end
	end
    SetContainerPos();
    if(needRefreshScrollView)then
    	UIHelper.ResetScroll(uiScrollView);
    end
    UIHelper.RefreshPanel(uiScrollView);
end

function SetContainerPos() 
    if(currentPosAnchor == nil or currentPosAnchor > chatSettings.anchorStartLimit)then
    	uiContainer.transform.localPosition = NewVector3(0, chatSettings.anchorDefaultPos, 0);
    else
        uiContainer.transform.localPosition = NewVector3(0, -(chatSettings.anchorShiftOffset + currentPosAnchor), 0);
    end    
end

function ProcessBroadCastMsg(bcid, args, uinfo)
    local str = Config.GetProperty(Config.broadcastTB, bcid, "content") or "";
    if(args==nil or IsTableEmpty(args)) then
        return str;
    else 
    	for i,v in ipairs(args) do
	        local param = v;
	        if (uinfo ~= nil and uinfo[v]~=nil) then
	            param = "[FFFFFF](VIP"..uinfo[v].vip..")[-]"..uinfo[v].name;
	        end
	        str = string.gsub(str, "{"..i.."}", param);
	    end
		return str;
    end
end

function SetChattingItemInfo(uiChatItem, chatData)
	if(uiChatItem.ToWhom ~= nil)then
		if(chatData.receiver_name~=nil and chatData.receiver_name~="")then
			GameObjectSetActive(uiChatItem.ToWhom,true);
			UIHelper.SetLabelTxt(uiChatItem.ToWhom,GetLocalizedString(chatSettings.strSpeakToWhom,chatData.receiver_name));
		else
			GameObjectSetActive(uiChatItem.ToWhom,false);
		end
	elseif(uiChatItem.Name ~= nil)then
        UIHelper.SetLabelTxt(uiChatItem.Name,chatData.sender_name);
	end
	Util.SetUITexture(uiChatItem.Icon,LuaConst.Const.ClubIcon,Role.GetClubIconType2(chatData.sender_icon),true);
	UIHelper.SetLabelTxt(uiChatItem.Vip,"vip"..chatData.sender_vip);

	if(uiChatItem.ButtonIcon~=nil)then
		AddOrChangeClickParameters(uiChatItem.ButtonIcon.gameObject,OnClickHeadIcon,{targetUID = chatData.sender});
	end
    SetItemBGAndPosAnchor(uiChatItem.Content, uiChatItem.textBG, chatData.msg);
end

function SetItemBGAndPosAnchor( uiContent, uiBG, msgContent )
    UIHelper.SetLabelTxt(uiContent,msgContent);
    local textSize = UIHelper.LabelSize(uiContent);
    UIHelper.SetSizeOfWidget(uiBG,Vector2.New(textSize.x+30,textSize.y+20));
    currentPosAnchor = currentPosAnchor - (47 + textSize.y + 8);
end

function OnClickChannel(go)
	local listener = UIHelper.GetUIEventListener(go);
	if(listener~=nil and listener.parameter~=nil )then
		if(listener.parameter.channelID == 1)then
    		WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { "联盟聊天功能紧张开发中，敬请期待！" });
		else
			SwitchChannel(listener.parameter.channelID);
		end
	end
end

function OnClickHeadIcon(go)
	local listener = UIHelper.GetUIEventListener(go);
	if(listener~=nil and listener.parameter~=nil )then
		WindowMgr.ShowWindow(LuaConst.Const.UIPrivateChat, {targetUID = listener.parameter.targetUID});
	end
end

function OnClickContentInputBlock()
	--show hint
	WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxLineHint, { GetLocalizedString(chatSettings.strNoReceiverSelected) });	
end

function OnClickButtonSend()
	if(UIHelper.InputTxt(contentInput) == "")then
		return;
	end

	if(currentChannel~=nil)then
		local paymentInfo = ChattingData.GetPaymentInfo();
		if(currentChannel~=0 or paymentInfo.chatRemainFreeNum~=nil and paymentInfo.chatRemainFreeNum>0)then
			--still have free chance to talk			
			SendMsg();
		else
			if(chatSettings.ConfirmCostPrefab == nil)then
				chatSettings.ConfirmCostPrefab = windowComponent:GetPrefab(chatSettings.ConfirmCostPrefabName);
			end
			--instantiate
		    local clone = GameObjectInstantiate(chatSettings.ConfirmCostPrefab);
			clone.transform.parent = window.transform;
		    clone.transform.localPosition = Vector3.zero;
		    clone.transform.localScale = Vector3.one;

		    UIHelper.SetLabelTxt(clone.transform,GetLocalizedString(chatSettings.CostChances,paymentInfo.chatCostItemNum));
		    -- local icon = TransformFindChild(clone.transform,"IconRoot/Icon");
		    -- Util.SetUITexture(icon,LuaConst.Const.ItemIcon, paymentInfo.chatCostItemId, true);

			GameObjectSetActive(clone.transform,false);
			--1，content prefab 2，delegate close 3，delegate ok 4，delegate yes 5，delegate no
			WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxAdaptive, { 
				clone,
				true,
				nil,
				SendMsg,
				nil,
				nil });	
		end
	end
end

function SendMsg()
	-- print(UIHelper.InputTxt(contentInput));
	-- local msgToSend = string.sub(UIHelper.InputTxt(contentInput),1,chatSettings.msgLengthLimitMax);
	local msgToSend = UIHelper.InputTxt(contentInput);
    TableWordFilter:FilterText(msgToSend)
    msgToSend = TableWordFilter.FilteredWords
	-- print(msgToSend);
	local AfterRequest = function ()
		ClearMsg();
    	SetInfo();
    end
    if(currentChannel==2)then
		ChattingData.RequestSendMsg({channel = currentChannel, msg = msgToSend, receiverId = currentReceiver.ID},AfterRequest);
    else
		ChattingData.RequestSendMsg({channel = currentChannel, msg = msgToSend },AfterRequest);
    end
end

function OnClickButtonClose()
	Close();
end

function SwitchChannel(channelID)
	if(currentChannel~=channelID)then
		local AfterRequest = function ()
			--set old need create
			ChattingData.SetListDataNeedCreate();

			--set channel info
			SetChannelActive(currentChannel,false);
			SetChannelActive(channelID,true);
			currentChannel = channelID;
			UIHelper.SetLabelTxt(channelHint,chatSettings.buttonNameShortCut[currentChannel]);
			ShowReceiverHint(currentChannel==2);
			--check hint for private chat
			HintCheck();
			SetPaymentInfo();

			--reset params
			DestroyUIListItemGameObjects(chatItems);
			currentPosAnchor = 0;
			currentItemTime = nil;			

			--set chat items
			ClearMsg();
	    	SetInfo();
	    end
	    ChattingData.RequestMsgList({channel = channelID,num = chatSettings.initMsgNumToReq},AfterRequest,false);
	end
end

function HintCheck()
	if(currentChannel == 2)then
		HintManager.CheckHintStatus(UIHintSettings.HintSettings.HavePrivateChatMsgs,false);
	end
end

function SetReceiver(receiverID, receiverName, showIt)
	currentReceiver.ID  = receiverID;
	currentReceiver.Name = receiverName;
	ShowReceiverHint(showIt);
end

function ShowReceiverHint(willShow)
	if(willShow)then
		if(currentReceiver.ID~=nil and currentReceiver.Name~=nil)then
			--have a receiver
			GameObjectSetActive(contentInputBlock,false);
			GameObjectSetActive(receiverHint,true);
			UIHelper.SetLabelTxt(receiverHint,GetLocalizedString(chatSettings.strSpeakToWhom,currentReceiver.Name));
		else
			--set block active
			GameObjectSetActive(contentInputBlock,true);
			GameObjectSetActive(receiverHint,false);
		end
	else
		GameObjectSetActive(contentInputBlock,false);
		GameObjectSetActive(receiverHint,false);
	end	
end

function OnPaymentInfoChanged()	
	if(currentChannel==0)then
		--get payment info data from chattingData
		local paymentInfo = ChattingData.GetPaymentInfo();
		--set ui info
		if(paymentInfo.chatRemainFreeNum~=nil and paymentInfo.chatRemainFreeNum>0)then
			--still have free chance to talk
			GameObjectSetActive(paymentHint.CostTimeHint.transform,false);
			GameObjectSetActive(paymentHint.FreeTimeHint.transform,true);
			UIHelper.SetLabelTxt(paymentHint.FreeTimeHint,GetLocalizedString(chatSettings.strFreeChances,paymentInfo.chatRemainFreeNum));
		else
			GameObjectSetActive(paymentHint.CostTimeHint.transform,true);
			GameObjectSetActive(paymentHint.FreeTimeHint,false);
			-- Util.SetUITexture(paymentHint.CostTimeHint.icon, LuaConst.Const.ItemIcon, paymentInfo.chatCostItemId, true);
			UIHelper.SetLabelTxt(paymentHint.CostTimeHint.price,paymentInfo.chatCostItemNum);
		end
	end
end

function SetPaymentInfo()
	if(currentChannel==0)then
		local paymentInfo = ChattingData.GetPaymentInfo();
		if(paymentInfo.chatRemainFreeNum~=nil and paymentInfo.chatRemainFreeNum>0)then
			--still have free chance to talk
			GameObjectSetActive(paymentHint.CostTimeHint.transform,false);
			GameObjectSetActive(paymentHint.FreeTimeHint.transform,true);
			UIHelper.SetLabelTxt(paymentHint.FreeTimeHint,GetLocalizedString(chatSettings.strFreeChances,paymentInfo.chatRemainFreeNum));
		else
			GameObjectSetActive(paymentHint.CostTimeHint.transform,true);
			GameObjectSetActive(paymentHint.FreeTimeHint,false);
			-- Util.SetUITexture(paymentHint.CostTimeHint.icon, LuaConst.Const.ItemIcon, paymentInfo.chatCostItemId, true);
			UIHelper.SetLabelTxt(paymentHint.CostTimeHint.price,paymentInfo.chatCostItemNum);
		end	
	else
		GameObjectSetActive(paymentHint.CostTimeHint.transform,false);
		GameObjectSetActive(paymentHint.FreeTimeHint.transform,false);
	end
end

function Close()
	StopPolling();
	windowComponent:Close();
end

function OnHide()

end

function OnShow()

end

function OnDestroy()
	StopPolling();
	ChattingData.SetListDataNeedCreate();
    ChattingData.UnRegisterOnPaymentInfoChanged();

	channelButtons = {};
	contentInput = nil;
	contentInputBlock = nil;
	paymentHint = nil;
	channelHint = nil;
	receiverHint = nil;
	buttonClose = nil;
	buttonSendMsg = nil;
	currentChannel = nil;
	-- currentReceiver = {};
	currentItemTime = nil;
	pollingTimerID = nil;
	uiScrollView = nil;
	uiContainer = nil;
	currentPosAnchor = 0;
	chatItems = {};
	window = nil;
	windowComponent = nil;
end
