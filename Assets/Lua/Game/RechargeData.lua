module("RechargeData", package.seeall)

require "Common/UnityCommonScript"

local buyMonthlyCard = nil

function OnInit()
    DataSystemScript.RegisterMsgHandler(MsgID.tb.BuyMonthlyCard, OnReqBuyMonthlyCard)
end
function OnRelease()
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.BuyMonthlyCard, OnReqBuyMonthlyCard)	
end

function RequrestBuyMonthlyCard( cb )
    buyMonthlyCard = cb
    DataSystemScript.RequestWithParams(LuaConst.Const.BuyMonthlyCard, nil, MsgID.tb.BuyMonthlyCard)
end
function OnReqBuyMonthlyCard( _code, _data )
    print("购买月卡")
end