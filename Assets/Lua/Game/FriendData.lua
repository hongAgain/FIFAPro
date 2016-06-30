--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion

module("FriendData", package.seeall)

require "Common/UnityCommonScript"
require "Game/GameMainScript"
--require "Game/DataSystemScript"
require "Config"


local callBackFun = nil;
local callBackFunOther = nil;
function OnInit()
    DataSystemScript.RegisterMsgHandler(MsgID.tb.FriendAdd, OnReqFriendAdd);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.FriendAccept, OnReqFriendAccept);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.FriendRefuse, OnReqFriendRefuse);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.FriendList, OnReqFriendList);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.FriendApply, OnReqFriendApply);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.FriendGivePower, OnReqFriendGivePower);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.FriendDel, OnReqFriendDel);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.FriendPowerList, OnReqFriendPowerList);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.FriendGetPower, OnReqFriendGetPower);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.FriendGetAllPower, OnReqFriendGetAllPower);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.FriendPK, OnReqFriendPK);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.FriendRecommend, OnReqFriendRecommend);

end

function OnRelease()
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.FriendAdd, OnReqFriendAdd);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.FriendAccept, OnReqFriendAccept);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.FriendRefuse, OnReqFriendRefuse);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.FriendList, OnReqFriendList);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.FriendApply, OnReqFriendApply);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.FriendGivePower, OnReqFriendGivePower);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.FriendDel, OnReqFriendDel);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.FriendPowerList, OnReqFriendPowerList);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.FriendGetPower, OnReqFriendGetPower);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.FriendGetAllPower, OnReqFriendGetAllPower);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.FriendPK, OnReqFriendPK);

end

-- Request
function ReqFriendAdd(callBack_,id_,name_)
    callBackFun = callBack_;

	local dictPrams = {};
    dictPrams['id'] = id_;
    dictPrams['name'] = name_;
	DataSystemScript.RequestWithParams(LuaConst.Const.ReqFriendAddUrl, dictPrams, MsgID.tb.FriendAdd);
end

function ReqFriendAccept(callBack_,id_)
    callBackFun = callBack_;

	local dictPrams = {};
    dictPrams['id'] = id_;
	DataSystemScript.RequestWithParams(LuaConst.Const.ReqFriendAcceptUrl, dictPrams, MsgID.tb.FriendAccept);
end

function ReqFriendRefuse(callBack_,id_)
    callBackFun = callBack_;

	local dictPrams = {};
    dictPrams['id'] = id_;
	DataSystemScript.RequestWithParams(LuaConst.Const.ReqFriendRefuseUrl, dictPrams, MsgID.tb.FriendRefuse);
end

function ReqFriendList(callBack_,page_,size_)
    callBackFunOther = callBack_;

	local dictPrams = {};
    dictPrams['page'] = page_;
    dictPrams['size'] = size_;
	DataSystemScript.RequestWithParams(LuaConst.Const.ReqFriendListUrl, dictPrams, MsgID.tb.FriendList);
end

function ReqFriendApply(callBack_,page_,size_)
    callBackFun = callBack_;

	local dictPrams = {};
    dictPrams['page'] = page_;
    dictPrams['size'] = size_;
	DataSystemScript.RequestWithParams(LuaConst.Const.ReqFriendApplyUrl, dictPrams, MsgID.tb.FriendApply);
end

function ReqFriendGivePower(callBack_,id_)
    callBackFun = callBack_;

	local dictPrams = {};
    dictPrams['id'] = id_;
	DataSystemScript.RequestWithParams(LuaConst.Const.ReqFriendGivePowerUrl, dictPrams, MsgID.tb.FriendGivePower);
end

function ReqFriendDel(callBack_,id_)
    callBackFun = callBack_;

	local dictPrams = {};
    dictPrams['id'] = id_;
	DataSystemScript.RequestWithParams(LuaConst.Const.ReqFriendDelUrl, dictPrams, MsgID.tb.FriendDel);
end

function ReqFriendPowerList(callBack_,page_,size_)
    callBackFun = callBack_;

	local dictPrams = {};
    dictPrams['page'] = page_;
    dictPrams['size'] = size_;
	DataSystemScript.RequestWithParams(LuaConst.Const.ReqFriendPowerListUrl, dictPrams, MsgID.tb.FriendPowerList);
end

function ReqFriendGetPower(callBack_,id_)
    callBackFun = callBack_;

	local dictPrams = {};
    dictPrams['id'] = id_;
	DataSystemScript.RequestWithParams(LuaConst.Const.ReqFriendGetPowerUrl, dictPrams, MsgID.tb.FriendGetPower);
end

function ReqFriendGetAllPower(callBack_,id_)
    callBackFun = callBack_;

	local dictPrams = {};
	DataSystemScript.RequestWithParams(LuaConst.Const.ReqFriendGetAllPowerUrl, dictPrams, MsgID.tb.FriendGetAllPower);
end

function ReqFriendPK(callBack_,id_)
    callBackFun = callBack_;

	local dictPrams = {};
    dictPrams['id'] = id_;
	DataSystemScript.RequestWithParams(LuaConst.Const.ReqFriendPKUrl, dictPrams, MsgID.tb.FriendPK);
end

function ReqFriendRecommend(callBack_)
    callBackFun = callBack_;

	local dictPrams = {};
	DataSystemScript.RequestWithParams(LuaConst.Const.ReqFriendRecommendUrl, dictPrams, MsgID.tb.FriendRecommend);
end


-- Response
function OnReqFriendAdd(code_, data_)
    if code_ == nil then        
        if callBackFun ~= nil then
            callBackFun(data_);
            callBackFun = nil;
        end
    end
end

function OnReqFriendAccept(code_, data_)
    if code_ == nil then        
        if callBackFun ~= nil then
            callBackFun(data_);
            callBackFun = nil;
        end
    end
end

function OnReqFriendRefuse(code_, data_)
    if code_ == nil then        
        if callBackFun ~= nil then
            callBackFun(data_);
            callBackFun = nil;
        end
    end
end

function OnReqFriendList(code_, data_)
    if code_ == nil then        
        if callBackFunOther ~= nil then
            callBackFunOther(data_);
            callBackFunOther = nil;
        end
    end
end

function OnReqFriendApply(code_, data_)
    if code_ == nil then        
        if callBackFun ~= nil then
            callBackFun(data_);
            callBackFun = nil;
        end
    end
end

function OnReqFriendGivePower(code_, data_)
    if code_ == nil then        
        if callBackFun ~= nil then
            callBackFun(data_);
            callBackFun = nil;
        end
    end
end

function OnReqFriendDel(code_, data_)
    if code_ == nil then        
        if callBackFun ~= nil then
            callBackFun(data_);
            callBackFun = nil;
        end
    end
end

function OnReqFriendPowerList(code_, data_)
    if code_ == nil then        
        if callBackFun ~= nil then
            callBackFun(data_);
            callBackFun = nil;
        end
    end
end

function OnReqFriendGetPower(code_, data_)
    if code_ == nil then        
        if callBackFun ~= nil then
            callBackFun(data_);
            callBackFun = nil;
        end
    end
end

function OnReqFriendGetAllPower(code_, data_)
    if code_ == nil then        
        if callBackFun ~= nil then
            callBackFun(data_);
            callBackFun = nil;
        end
    end
end

function OnReqFriendPK(code_, data_)
    if code_ == nil then        
        if callBackFun ~= nil then
            callBackFun(data_);
            callBackFun = nil;
        end
    end
end

function OnReqFriendRecommend(code_, data_)
    if code_ == nil then        
        if callBackFun ~= nil then
            callBackFun(data_);
            callBackFun = nil;
        end
    end
end


