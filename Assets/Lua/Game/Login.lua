module("Login", package.seeall)


require "Game/DataSystemScript"
require "Game/MsgID"
require "Game/PageComponent"
require "Common/CommonScript"
require "Game/SceneManager"

local srvList = {};
local srvSum = nil;

local pageModule = nil;

local onPageChangedDel = {};

local onGetSrvList = nil;
local onGetLatestServer = nil;

local defaultSrvId = nil;

local defaultServerRoles = nil;
NR_PER_PAGE = 15;

function OnInit()
	DataSystemScript.RegisterMsgHandler(MsgID.tb.ServerList, OnServerList);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.DefaultSrv, OnDefaultSrv);
    
    pageModule = PageComponent.New(OnPageChanged);
end

function OnServerList(errcode, data)
	local page = data["page"];
	local size = data["size"];
	local total = data["total"];

    srvSum = total;
    
	local maxPage = math.floor(total / size);
	if (total - maxPage * size > 0) then
		maxPage = maxPage + 1;
	end

    pageModule:SetMax(maxPage);

	if (total == 0) then
		print("Have no server alive!");
	else
		local rows = data["rows"];

		-- if (srvList[page] == nil) then
			srvList[page] = {};
		-- end

        for k,v in ipairs(rows) do
            local srvInfo = CommonScript.DeepCopy(v);
            srvInfo.best = srvInfo.best or 0;
            srvInfo.__index = srvInfo;
            
	        function srvInfo.GetStrInfo ()
	            local statStr = "";

                local id = srvInfo.id;
                local name = srvInfo.name;
                local status = srvInfo.stat;
                local best = srvInfo.best;

	            if (status == 0) then
	                statStr = Util.LocalizeString("testSrv");
	            elseif (status == 1) then
	                statStr = Util.LocalizeString("formalSrv");
	            elseif (status == 2) then
	                statStr = Util.LocalizeString("weihu");
	            end

	            local str = id..Util.LocalizeString("qu").." "..name.."     ("..statStr..")";
        
                if (best == 1) then
                    str = str .. "[sup]" .. Util.LocalizeString("tuijian") .. "[/sup]";
                end
	            return str;
	        end
    
            table.insert(srvList[page], srvInfo);
        end

        if (onGetSrvList ~= nil) then
            onGetSrvList();
        end
        
        for _,v in ipairs(onPageChangedDel) do
            v();
        end
	end
end

function OnDefaultSrv(code, data)
    -- local recentLoginSrv = data[1];
    -- defaultSrvId = recentLoginSrv.sid;
    -- DataSystemScript.SetRegionId(defaultSrvId);
    -- pageModule.cur = math.ceil(defaultSrvId / 10);
    -- ReqSrvList();
    if (data.role == nil or IsTableEmpty(data.role)) then
        --set default values and the newest server id record
        defaultServerRoles = nil;
        -- defaultServerRoles[1] = {  id = nil,
        --                         sid = data.snum,
        --                         sname = nil,
        --                         uid = nil,
        --                         lv = 1,
        --                         lastTime = nil,
        --                         name = nil};
        defaultSrvId = data.snum;
    else
        defaultSrvId = data.role[1].sid;
        defaultServerRoles = data.role;
        
        local page = math.floor((data.role[1].sid - 1) / NR_PER_PAGE) + 1;
        if (srvList[page] == nil) then
            srvList[page] = {};
        end
        
        local srvInfo = {
            _id = data.role[1].sid,
            name = data.role[1].sname,
            best = 1,
            host = "",
            time = data.role[1].time,
            -- time = 1426809600000,
            id = data.role[1].sid
        };
        table.insert(srvList[page], srvInfo);
    end

 

    -- print("Remember to fix this bug");


    DataSystemScript.SetRegionId(defaultSrvId);
    pageModule.cur = math.ceil(defaultSrvId / NR_PER_PAGE);
    if (onGetLatestServer ~= nil) then 
        onGetLatestServer();
    end
end

function GetDefaultSrvInfo()
    local list = srvList[pageModule.cur];
    
    for _, v in ipairs(list) do
        if (v.id == defaultSrvId) then
            return v.GetStrInfo();
        end
    end
    
    return nil;
end

function GetLatestLoginServerRole()
    -- body
    if(defaultServerRoles == nil)then
        return nil;
    end
    return defaultServerRoles[1];
end

function GetRecentLoginServerRole()
    -- body
    return defaultServerRoles;
end

function GetServerSum()
    return srvSum;
end

function GetCurPage()
    return pageModule.cur;
end

function GetMaxPageNum()
    return pageModule.imax;
end

function GetCurPageSrvInfo()
	return srvList[pageModule.cur];
end

function RegisterPageChanged(callback)
	table.insert(onPageChangedDel, callback);
end

function UnRegisterPageChanged(callback)
	for k,v in pairs(onPageChangedDel) do
		if (v == callback) then
			table.remove(onPageChangedDel, k);
			break;
		end
	end
end

function SetOnGetSrvList(callback)
    onGetSrvList = callback;
end

function SetOnGetLatestServer( callback )
    -- body
    onGetLatestServer = callback;
end

function ReqSrvList()
    local dic = {};
    dic['page'] = pageModule.cur;
    dic['size'] = NR_PER_PAGE;
    DataSystemScript.RequestWithParams(LuaConst.Const.ServerList, dic, MsgID.tb.ServerList);
end

function ReqDefaultSrvId(pf, oid)
    local dic = {};
    dic['pf'] = pf;
    dic['oid'] = oid;
    DataSystemScript.RequestWithParams(LuaConst.Const.DefaultSrv, dic, MsgID.tb.DefaultSrv);
end

function SwitchToPrevPage()
    pageModule:PrevPage();
end

function SwitchToNextPage()
    pageModule:NextPage();
end

function SwitchToPage(targetPage)
    OnPageChanged(targetPage);
end

function OnPageChanged(curPage)
    if (srvList[curPage] == nil) then
        ReqSrvList();
    else
        for _,v in ipairs(onPageChangedDel) do
            v();
        end
    end
end

function GetSrvInfo(srvId)
   local page = math.ceil(srvId / NR_PER_PAGE);
   local idx = srvId % NR_PER_PAGE;
   -- print("debug ------ page".. page);
   -- print("debug ------ idx".. idx);

   return srvList[page][idx];
end