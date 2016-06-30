module("CarnivalData", package.seeall)

require "Common/CommonScript"

local userRegistTime = nil
local endTime = nil
local remainDays = 7

local getCarnivalInfoCB = nil
local carnivalSubmitCB = nil
local getCPointAwardCB = nil
local getCSpGiftCB = nil

function OnInit()
	DataSystemScript.RegisterMsgHandler(MsgID.tb.CarnivalInfo, OnGetCarnivalInfo)
	DataSystemScript.RegisterMsgHandler(MsgID.tb.CarnivalSubmit, OnSubmitCarnival)
	DataSystemScript.RegisterMsgHandler(MsgID.tb.GetCPointAward, OnGetCPointAward)
	DataSystemScript.RegisterMsgHandler(MsgID.tb.GetCShop, OnBuyCSpGift)
end

function OnRelease()
	DataSystemScript.UnRegisterMsgHandler(MsgID.tb.CarnivalInfo, OnGetCarnivalInfo)
	DataSystemScript.UnRegisterMsgHandler(MsgID.tb.CarnivalSubmit, OnSubmitCarnival)
	DataSystemScript.UnRegisterMsgHandler(MsgID.tb.GetCPointAward, OnGetCPointAward)
	DataSystemScript.UnRegisterMsgHandler(MsgID.tb.GetCShop, OnBuyCSpGift)
end

function SetGegistTime(registTime)
	userRegistTime = math.modf(registTime/1000) --时间戳ms
	local t = os.date("*t", userRegistTime)
	local daySecond = 24*3600
	local registSecond = CommonScript.DayHMS2Second({t.hour,t.min,t.sec})
	endTime = userRegistTime + daySecond * remainDays - registSecond - 1
end
function GetRemainTime()
	local curTime = math.modf(Util.GetTime()/1000)
	return endTime - curTime
end

function ReqCarnivalInfo(cb)
	getCarnivalInfoCB = cb
	DataSystemScript.RequestWithParams(LuaConst.Const.CarnivalInfo, nil, MsgID.tb.CarnivalInfo)
end
function OnGetCarnivalInfo(code, data)
	if code == nil then
		getCarnivalInfoCB(data)
		getCarnivalInfoCB = nil
	end
end

function ReqSubmitCarnival( cb, id )
	carnivalSubmitCB = cb
	local param = {}
	param.id = id
	DataSystemScript.RequestWithParams(LuaConst.Const.CarnivalSubmit, param, MsgID.tb.CarnivalSubmit)
end
function OnSubmitCarnival( code, data )
	if code == nil then
		carnivalSubmitCB(data)
		carnivalSubmitCB = nil
	end
end

function ReqGetCPointAward( cb, id)
	getCPointAwardCB = cb
	local param = {}
	param.id = id
	DataSystemScript.RequestWithParams(LuaConst.Const.GetCPointAward, param, MsgID.tb.GetCPointAward)
end
function OnGetCPointAward( code, data )
	if code == nil then
		getCPointAwardCB(data)
		getCPointAwardCB = nil
	end
end

function ReqBuyCSpGift( cb, id )
	getCSpGiftCB = cb
	local param = {}
	param.id = id
	DataSystemScript.RequestWithParams(LuaConst.Const.GetCShop, param, MsgID.tb.GetCShop)
end
function OnBuyCSpGift( code, data )
	if code == nil then
		getCSpGiftCB(data)
		getCSpGiftCB = nil
	end
end