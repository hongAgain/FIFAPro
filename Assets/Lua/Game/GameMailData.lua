module("GameMailData", package.seeall);


local totalListData = {};
--only used for calculating length at the first time
local listData = nil;
local detailData = {};
local attrData = nil;
local deleteData = nil;

local localDataLengthMax = 100;

local delegateOnReqMailList = nil;
local delegateOnReqMailDetail = nil;
local delegateOnReqMailAttr = nil;
local delegateOnReqMailDelete = nil;

function OnInit()
	DataSystemScript.RegisterMsgHandler(MsgID.tb.ReqEmailListMsg, OnReqEmailList);
	DataSystemScript.RegisterMsgHandler(MsgID.tb.ReqEmailDetailMsg, OnReqEmailDetail);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.ReqGetAttrMailMsg, OnReqAttrEmail);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.ReqDeleteEmailMsg, OnReqDeleteEmail);
end

function OnRelease()
	DataSystemScript.UnRegisterMsgHandler(MsgID.tb.ReqEmailListMsg, OnReqEmailList);
	DataSystemScript.UnRegisterMsgHandler(MsgID.tb.ReqEmailDetailMsg, OnReqEmailDetail);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.ReqGetAttrMailMsg, OnReqAttrEmail);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.ReqDeleteEmailMsg, OnReqDeleteEmail);
end

function RequestGameMailList( parameters, delegatefunc )
    delegateOnReqMailList = delegatefunc;
    DataSystemScript.RequestWithParams(LuaConst.Const.ReqEmailListUrl, parameters, MsgID.tb.ReqEmailListMsg);
end

function RequestGameMailDetail( parameters, delegatefunc )
    delegateOnReqMailDetail = delegatefunc;
    DataSystemScript.RequestWithParams(LuaConst.Const.ReqEmailDetailUrl, parameters, MsgID.tb.ReqEmailDetailMsg);
end

function RequestGameMailAttr( parameters, delegatefunc )
    delegateOnReqMailAttr = delegatefunc;
    DataSystemScript.RequestWithParams(LuaConst.Const.ReqEmailGetAttrUrl, parameters, MsgID.tb.ReqGetAttrMailMsg);
end

function RequestGameMailDelete( parameters, delegatefunc )
    delegateOnReqMailDelete = delegatefunc;
    DataSystemScript.RequestWithParams(LuaConst.Const.ReqEmailDeletelUrl, parameters, MsgID.tb.ReqDeleteEmailMsg);
end

function OnReqEmailList(code_, data_)
    print(".. OnReqEmailList!!!");
    listData = data_;

    MergeListData(listData);

    if (delegateOnReqMailList ~= nil) then
        delegateOnReqMailList();
        delegateOnReqMailList = nil;
    end
end

function OnReqEmailDetail(code_, data_)
    print(".. OnReqEmailDetail!!!");
    detailData[data_._id] = data_;
    if (delegateOnReqMailDetail ~= nil) then
        delegateOnReqMailDetail();
        delegateOnReqMailDetail = nil;
    end
end

function OnReqAttrEmail(code_, data_)
    print(".. OnReqAttrEmail!!!");
    attrData = data_;
    if (delegateOnReqMailAttr ~= nil) then
        delegateOnReqMailAttr();
        delegateOnReqMailAttr = nil;
    end
end

function OnReqDeleteEmail(code_, data_)
    print(".. OnReqDeleteEmail!!!");
    deleteData = data_;
    if (delegateOnReqMailDelete ~= nil) then
        delegateOnReqMailDelete();
        delegateOnReqMailDelete = nil;
    end
end

function Get_TotalListData()
    return totalListData;
end

-- function Get_ListData()
--     return listData;
-- end

function Get_ListDataLength()
    if(listData==nil)then
        return 0;
    end
    return #(listData.rows or {});
end

function HaveUnreadMail()
    if(listData~=nil)then
        for k,v in pairs(listData.rows) do
            if(v.stat==0)then
                return true;
            end
        end
        
        --if no new letter, check remaining letter in list
        return (listData.total > 0 and (listData.start+listData.num-2)<listData.total);
    end
    return false;
end

function ForbidToReqMore()
    local count = 0;
    for k,v in pairs(totalListData) do
        count = count+1;
    end
    return count >= localDataLengthMax;
end

function Get_DetailData()
    return detailData;
end

function Get_AttrData()
    return attrData;
end

function Get_DeleteData()
    return deleteData;
end

function MergeListData(currentListData)
    for k,v in pairs(currentListData.rows) do
        totalListData[v._id] = v;
    end
end

function IsListDataContain( mailID )    
    -- if(listData==nil or listData.rows==nil) then
    --     return false;
    -- end
    -- for k,v in pairs(listData.rows) do
    --     if(v._id == mailID) then
    --         return true;
    --     end
    -- end
    if(totalListData[mailID]~=nil or totalListData[mailID]~={})then
        return true;
    end
    return false;
end

function Get_OneDetailData(mailID)
    if(detailData==nil) then
        return nil;
    end
    for k,v in pairs(detailData) do
        if(v._id == mailID) then
            return v;
        end
    end
    return nil;
end

function UpdateReadStat(mailID)
    -- for k,v in pairs(listData.rows) do
    --     if(v._id == mailID) then
    --         v.stat = 1;
    --         return;
    --     end
    -- end
    if(totalListData[mailID]~=nil or totalListData[mailID]~={})then
        totalListData[mailID].stat = 1;
    end
end

function GetMailAttr( mailID )
    -- if(listData==nil or listData.rows==nil) then
    --     return;
    -- end
    if(detailData[mailID] ~= nil) then
        detailData[mailID].stat = 1;
        detailData[mailID].attr = "";
    end    
    if(totalListData[mailID]~=nil or totalListData[mailID]~={})then
        totalListData[mailID].stat = 1;
        totalListData[mailID].attr = "";
    end
    -- for k,v in pairs(listData.rows) do
    --     if(v._id == mailID) then            
    --         v.stat = 1;
    --         v.attr = "";
    --         return;
    --     end
    -- end
end

function DeleteMailData( mailID, indexID )
	-- if(listData==nil or listData.rows==nil) then
	-- 	return;
	-- end
    if(detailData[mailID] ~= nil) then
        detailData[mailID] = nil;
    end
    if(totalListData[mailID]~=nil or totalListData[mailID]~={})then
        totalListData[mailID] = nil;
    end
	-- for k,v in pairs(listData.rows) do
	-- 	if(v._id == mailID) then			
	-- 		table.remove(listData.rows,indexID);
	-- 		return;
	-- 	end
	-- end
end
