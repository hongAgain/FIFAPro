module("LevelFundData", package.seeall);

local LevelFundInfoData  = nil;
local LevelFundBuyData   = nil;
local LevelFundRepayData = nil;

local LevelFundInfoDelegateFunc  = nil;
local LevelFundBuyDelegateFunc   = nil;
local LevelFundRepayDelegateFunc = nil;

function OnInit()
    DataSystemScript.RegisterMsgHandler(MsgID.tb.LevelFundInfo, OnReqLevelFundInfo);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.LevelFundBuy, OnReqLevelFundBuy);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.LevelFundRepay, OnReqLevelFundRepay);
end

function OnRelease()
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.LevelFundInfo, OnReqLevelFundInfo);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.LevelFundBuy, OnReqLevelFundBuy);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.LevelFundRepay, OnReqLevelFundRepay);
end

function RequestLevelFundInfo(params, delegateFunc)
    LevelFundInfoDelegateFunc = delegateFunc;
    DataSystemScript.RequestWithParams(LuaConst.Const.LevelFundInfo, params, MsgID.tb.LevelFundInfo);
end
function RequestLevelFundBuy(params, delegateFunc)
    LevelFundBuyDelegateFunc = delegateFunc;
    DataSystemScript.RequestWithParams(LuaConst.Const.LevelFundBuy, params, MsgID.tb.LevelFundBuy);
end
function RequestLevelFundRepay(params, delegateFunc)
    LevelFundRepayDelegateFunc = delegateFunc;
    DataSystemScript.RequestWithParams(LuaConst.Const.LevelFundRepay, params, MsgID.tb.LevelFundRepay);
end

function OnReqLevelFundInfo(code_, data_)
    LevelFundInfoData = data_;
    if (LevelFundInfoDelegateFunc ~= nil) then
        LevelFundInfoDelegateFunc();
        LevelFundInfoDelegateFunc = nil;
    end
end
function OnReqLevelFundBuy(code_, data_)
    LevelFundBuyData = data_;
    if (LevelFundBuyDelegateFunc ~= nil) then
        LevelFundBuyDelegateFunc();
        LevelFundBuyDelegateFunc = nil;
    end
end
function OnReqLevelFundRepay(code_, data_)
    LevelFundRepayData = data_;
    if (LevelFundRepayDelegateFunc ~= nil) then
        LevelFundRepayDelegateFunc();
        LevelFundRepayDelegateFunc = nil;
    end
end

function Get_LevelFundInfoData()
    return LevelFundInfoData;
end
function Get_LevelFundBuyData()
    return LevelFundBuyData;
end
function Get_LevelFundRepayData()
    return LevelFundRepayData;
end
