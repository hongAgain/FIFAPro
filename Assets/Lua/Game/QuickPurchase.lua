module("QuickPurchase", package.seeall)

local callBackFun = nil

function OnInit()
	DataSystemScript.RegisterMsgHandler(MsgID.tb.BuyEnergy,OnBuyEnergyCB)
	-- DataSystemScript.RegisterMsgHandler(MsgID.tb.BuyGold,OnBuyGoldCB)
end

function OnRelease()
	DataSystemScript.UnRegisterMsgHandler(MsgID.tb.BuyEnergy, OnBuyEnergyCB)
end

function ReqBuyEnergy( _callBack )
	callBackFun = _callBack
	local dictParams = {}
	DataSystemScript.RequestWithParams(LuaConst.Const.BuyEnergyUrl,dictParams,MsgID.tb.BuyEnergy)
end

function OnBuyEnergyCB( _code,_data )
	if _code == nil then
		if callBackFun ~= nil then
			callBackFun(_data)
			callBackFun = nil
		end
	end
end