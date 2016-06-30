module("ChattingData",package.seeall);

--require "Game/DataSystemScript"

local msgDataDict = {};
local sendMsgData = nil;
local chatPaymentInfo = {
	chatCostItemId=nil,
	chatCostItemNum=nil,
	chatRemainFreeNum=nil
};

local delegateOnReqMsgList = nil;
local delegateOnReqSendMsg = nil;
local delegateOnPaymentInfoChanged = nil;

local currentChannelID = nil;
local msgReqLock = false;

function OnInit()
    DataSystemScript.RegisterMsgHandler(MsgID.tb.GetChatMsgList, OnReqMsgList);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.SendChatMsg, OnReqSendMsg);

    DataSystemScript.RegisterFailedMsgHandler(MsgID.tb.GetChatMsgList, OnReqMsgListFailed);
end

function OnRelease()
	DataSystemScript.UnRegisterMsgHandler(MsgID.tb.GetChatMsgList, OnReqMsgList);
	DataSystemScript.UnRegisterMsgHandler(MsgID.tb.SendChatMsg, OnReqSendMsg);

    DataSystemScript.UnRegisterFailedMsgHandler(MsgID.tb.GetChatMsgList, OnReqMsgListFailed);
end

function RegisterOnPaymentInfoChanged(delegateFunc)
	delegateOnPaymentInfoChanged = delegateFunc;
end

function UnRegisterOnPaymentInfoChanged()
	delegateOnPaymentInfoChanged = nil;
end

function RequestMsgList( parameters, delegatefunc, doNotShowAnimeFlag )
	if(msgReqLock)then
		return;
	end
	msgReqLock = true;

	currentChannelID = parameters.channel;

    delegateOnReqMsgList = delegatefunc;
    DataSystemScript.RequestWithParams(LuaConst.Const.GetChatMsgList, parameters, MsgID.tb.GetChatMsgList,doNotShowAnimeFlag);
end

function RequestSendMsg( parameters, delegatefunc )

	if(msgReqLock)then
		return;
	end
	msgReqLock = true;

    delegateOnReqSendMsg = delegatefunc;
    DataSystemScript.RequestWithParams(LuaConst.Const.SendChatMsg, parameters, MsgID.tb.SendChatMsg, true);
end

function OnReqMsgList(code_, data_)
	msgReqLock = false;
	
	MergeListData(data_.msgList or {});
	
    if (delegateOnReqMsgList ~= nil) then
        delegateOnReqMsgList();
    end
    UpdatePaymentInfo(data_.chatCostItemId,data_.chatCostItemNum,data_.chatRemainFreeNum);
end

function OnReqMsgListFailed(code_, data_)
	msgReqLock = false;
end

function OnReqSendMsg(code_, data_)
	msgReqLock = false;
    sendMsgData = data_;
    MergeListData(data_.msgList or {});

    if (delegateOnReqSendMsg ~= nil) then
        delegateOnReqSendMsg();
    end
    UpdatePaymentInfo(data_.chatCostItemId,data_.chatCostItemNum,data_.chatRemainFreeNum);
end

------------tool functions------------
function AddMyLocalMsg(channelID,timeStamp,myMsg,receiverID)
	local data = {
		seq = nil,
		msg = myMsg,
		sender = Role.Get_uid(),
		receiver = receiverID,
		time = timeStamp,
		sender_name = Role.Get_name(),
		sender_icon = Role.Get_icon(),
		sender_lv = Role.Get_lv(),
		sender_vip = Role.Get_vip(),

		receiver_name = nil,
		receiver_icon = nil,
		receiver_lv = nil,
		receiver_vip = nil
	};
	data.type = 2;
	msgDataDict[currentChannelID][Role.Get_uid()..timeStamp] = data;
end

function UpdatePaymentInfo(chatCostItemId,chatCostItemNum,chatRemainFreeNum)
	if(chatPaymentInfo.chatCostItemId ~= chatCostItemId 
		or chatPaymentInfo.chatCostItemNum ~= chatCostItemNum
		or chatPaymentInfo.chatRemainFreeNum ~= chatRemainFreeNum)then

		chatPaymentInfo.chatCostItemId=chatCostItemId;
		chatPaymentInfo.chatCostItemNum=chatCostItemNum;
		chatPaymentInfo.chatRemainFreeNum=chatRemainFreeNum;

		if(delegateOnPaymentInfoChanged~=nil)then
			delegateOnPaymentInfoChanged();
		end
	end	
end

function MergeListData(data_)
	if (msgDataDict[currentChannelID]==nil)then
		msgDataDict[currentChannelID] = {};
	end
	--set all old
	for k,v in pairs(msgDataDict[currentChannelID]) do
		v.needCreate = false;
	end
	--check new data
    local empty = {};
	for k, v in pairs(data_) do
		if (msgDataDict[currentChannelID][v.time] ~= nil and msgDataDict[currentChannelID][v.time] ~= empty)then
			--old data
			v.needCreate = false;
		else
			--new data
			v.needCreate = true;
		end
		msgDataDict[currentChannelID][v.time] = v;
	end
end

function SetListDataNeedCreate()
	--reset flags

	if(currentChannelID~=nil)then
		for k,v in pairs(msgDataDict[currentChannelID]) do
			v.needCreate = true;
		end
	end
end

function GetChannelMsgList(channelID)
	return msgDataDict[channelID];
end

function GetSendMsgData()
	return sendMsgData;
end

function GetPaymentInfo()
	return chatPaymentInfo or {};
end