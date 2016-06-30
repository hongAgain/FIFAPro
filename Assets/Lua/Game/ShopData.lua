module("ShopData", package.seeall)

--require "DataSystemScript"
require "Common/CommonScript"
require "Game/HeroData"
require "Game/AutoReqMsg"

local DiamondShopInfoData = nil;
local DiamondShopBuyData = nil;
local EpicShopBuyData = nil;
local LadderShopBuyData = nil;

local DiamondShopInfoDelegateFunc = nil;
local DiamondShopBuyDelegateFunc = nil;
local EpicShopBuyDelegateFunc = nil;
local LadderShopBuyDelegateFunc = nil;

function OnInit()
    DataSystemScript.RegisterMsgHandler(MsgID.tb.DiamondShopInfo, OnReqDiamondShopInfo);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.DiamondShopBuy, OnReqDiamondShopBuy);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.RaidDFBuyItem, OnReqEpicShopBuy);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.LadderBuyItem, OnReqLadderShopBuy);

end

function OnRelease()
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.DiamondShopInfo, OnReqDiamondShopInfo);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.DiamondShopBuy, OnReqDiamondShopBuy);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.RaidDFBuyItem, OnReqEpicShopBuy);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.LadderBuyItem, OnReqLadderShopBuy);

end

--no need to update every time you get into the shop
function RequestDiamondShopInfo( delegatefunc )

    DiamondShopInfoDelegateFunc = delegatefunc;
    -- if(DiamondShopInfoData == nil) then
        DataSystemScript.RequestWithParams(LuaConst.Const.DiamondShopInfo, nil, MsgID.tb.DiamondShopInfo);
    -- else
    --     if (DiamondShopInfoDelegateFunc ~= nil) then
    --         DiamondShopInfoDelegateFunc();
    --         DiamondShopInfoDelegateFunc = nil;
    --     end
    -- end
end

function RequestDiamondShopBuy( parameters, delegatefunc )
    DiamondShopBuyDelegateFunc = delegatefunc;
    DataSystemScript.RequestWithParams(LuaConst.Const.DiamondShopBuy, parameters, MsgID.tb.DiamondShopBuy);
end

function RequestEpicShopBuy( parameters, delegatefunc )
    EpicShopBuyDelegateFunc = delegatefunc;
    DataSystemScript.RequestWithParams(LuaConst.Const.ReqRaidDFBuyItemUrl, parameters, MsgID.tb.RaidDFBuyItem);
end

function RequestLadderShopBuy( paramdict, delegatefunc )
    LadderShopBuyDelegateFunc = delegatefunc;
    DataSystemScript.RequestWithParams(LuaConst.Const.LadderBuyItem, paramdict, MsgID.tb.LadderBuyItem);
end

function OnReqDiamondShopInfo(code_, data_)
    -- print(".. OnReqDiamondShopInfo!!!");
    DiamondShopInfoData = data_;
    if (DiamondShopInfoDelegateFunc ~= nil) then
        DiamondShopInfoDelegateFunc();
        DiamondShopInfoDelegateFunc = nil;
    end
end

function OnReqDiamondShopBuy(code_, data_)
	-- print(".. OnReqDiamondShopBuy!!!");
    DiamondShopBuyData = data_;
    if (DiamondShopBuyDelegateFunc ~= nil) then
        DiamondShopBuyDelegateFunc();
        DiamondShopBuyDelegateFunc = nil;
    end
end

function OnReqEpicShopBuy(code_, data_)
    -- print("..OnReqRaidDFBuyItem!!!");
    EpicShopBuyData = data_;
    if (EpicShopBuyDelegateFunc ~= nil) then
        EpicShopBuyDelegateFunc();
        EpicShopBuyDelegateFunc = nil;
    end
end

function OnReqLadderShopBuy(code_,data_)
    -- print(".. OnReqLadderwinAward!!!");
    LadderShopBuyData = data_;
    if (LadderShopBuyDelegateFunc ~= nil) then
        LadderShopBuyDelegateFunc();
        LadderShopBuyDelegateFunc = nil;
    end
end

function Get_DiamondShopInfoData()
    return DiamondShopInfoData;
end

function Get_DiamondShopBuyData()
    return DiamondShopBuyData;
end

function Get_EpicShopBuyData()
    return EpicShopBuyData;
end

function Get_LadderShopBuyData()
    return LadderShopBuyData;
end

function Refresh_DiamondShopInfoData( id, wn, dn )
    -- body
    DiamondShopInfoData[tostring(id)].wn = wn;
    DiamondShopInfoData[tostring(id)].dn = dn;
end