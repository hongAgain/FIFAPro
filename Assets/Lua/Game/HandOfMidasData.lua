module("HandOfMidasData", package.seeall)

local buyHOMidasCB = nil
local getHOMidasInfoCB = nil

function OnInit()
	DataSystemScript.RegisterMsgHandler(MsgID.tb.BuyHandOfMidas, OnBuyHandOfMidas)
	DataSystemScript.RegisterMsgHandler(MsgID.tb.HandOfMidasInfo, OnGetHandOfMidasInfo)
end

function OnRelease()
	DataSystemScript.UnRegisterMsgHandler(MsgID.tb.BuyHandOfMidas, OnBuyHandOfMidas)
	DataSystemScript.UnRegisterMsgHandler(MsgID.tb.HandOfMidasInfo, OnGetHandOfMidasInfo)
end

function ReqGetHandOfMidasInfo( cb )
	getHOMidasInfoCB = cb
	DataSystemScript.RequestWithParams(LuaConst.Const.HandOfMidasInfo, nil, MsgID.tb.HandOfMidasInfo)
end
function OnGetHandOfMidasInfo( code, data )
	if code == nil then
		getHOMidasInfoCB(data)
		getHOMidasInfoCB = nil
	end
end
function ReqBuyHandOfMidas( cb )
	buyHOMidasCB = cb
	DataSystemScript.RequestWithParams(LuaConst.Const.BuyHandOfMidas, nil, MsgID.tb.BuyHandOfMidas)
end
function OnBuyHandOfMidas( code, data )
	if code == nil then
		buyHOMidasCB(data)
		buyHOMidasCB = nil
	end
end