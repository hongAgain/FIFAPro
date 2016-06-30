module("DataSystemScript", package.seeall)

require("Common/ktJson")
require("Common/CommonScript")
require("Common/UnityCommonScript")
require("Game/SynSys")
require("Game/HintManager")
--require("Game/ModuleMgr")

local handleMsgTB = {};
local handleFailedMsgTB = {};
local hookHandleMsgTB = {};

local dataSystem = nil;
local fullurl = nil;
local region = nil;
-- local DOLLAR_ID_KEY = "$id";
-- local dollar_id = nil;

local Lock_Streen_Switch = true;

local strLocalizeMsgRequestFailed = "MsgRequestFailed";

function OnInit(dataSys)
	dataSystem = dataSys;
    dataSystem = DataSystem.Instance
--	ModuleMgr.OnInit();
end

function OnRelease()
--	ModuleMgr.OnRelease();
end

function OnHttpResp(wwwError, jsonmsg)
	local tb = json.decode(jsonmsg);
	local msgid = tb['msgid'];
	print("Handle msgid "..msgid);
	print("jsonmsg = "..jsonmsg);

	if (wwwError == nil or (string.len(wwwError) == 0)) then
		--handle jsonmsg
		local code = tb['code'];
		local data = tb['data'];
        local serverTime = tb['time'];
		if (HaveError(code)) then
			error(code);
			--Handle error code
			if (handleFailedMsgTB[msgid] ~= nil) then
				handleFailedMsgTB[msgid](code, data);
			end
			if (code ~= nil) then
                local strMsg = Config.GetProperty(Config.messageTB, code, "name");
                
                if (data ~= nil) then
                    for i = 1, #data do
                        for k, v in pairs(data[i]) do
                            local param = Config.GetProperty(k, tostring(v), "name");
                            strMsg = string.gsub(strMsg, "{"..i.."}", param);
                        end
                    end
                end
				WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { strMsg });
			else
				WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString(strLocalizeMsgRequestFailed) });
			end
		else
            if (tb['cache'] ~= nil) then
                --sync info
                SynSys.Handle(tb['cache']);
                --check hint msg
                HintManager.HandleMsgCache(tb['cache']);
            end
			--check hint msg
            HintManager.HandleMsgContent(data,msgid);
			if (handleMsgTB[msgid] ~= nil) then				
				handleMsgTB[msgid](code, data);
                if (hookHandleMsgTB[msgid] ~= nil) then
                    hookHandleMsgTB[msgid](msgid, data);
                end
			else
				error("Unhandled msgid = "..msgid);
			end
		end
        if serverTime ~= nil then
            LuaServerTime.SynServerTime(serverTime);
        end
	else
		--something err
		error(wwwError);
	end
	msgid = nil;
	tb = nil;
end

function RegisterMsgHandler(id, fun)
	if (handleMsgTB[id] == nil) then
		handleMsgTB[id] = fun;
	else
		error(id.."MsgHandler isn't nil, try another id!");
	end
end

function UnRegisterMsgHandler(id, fun)
	if (handleMsgTB[id] == fun) then
		handleMsgTB[id] = nil;
	end
end

function RegisterFailedMsgHandler(id, fun)
	if (handleFailedMsgTB[id] == nil) then
		handleFailedMsgTB[id] = fun;
	else
		error(id.."FailedMsgHandler isn't nil, try another id!");
	end
end

function UnRegisterFailedMsgHandler(id, fun)
	if (handleFailedMsgTB[id] == fun) then
		handleFailedMsgTB[id] = nil;
	end
end

function RegisterHookMsgHandler(id, fun)
    if (hookHandleMsgTB[id] == nil) then
        hookHandleMsgTB[id] = fun;
    else
		error("Try hook "..id.." isn't nil!");
    end
end

function UnRegisterHookMsgHandler(id, fun)
	if (hookHandleMsgTB[id] == fun) then
		hookHandleMsgTB[id] = nil;
	end
end

function SetUrlPrefixAndRegion(host, srvRegion)
	local urlPrefix = nil;
	if (host ~= "" and host ~= nil) then
		if (Util.MatchURL(host)) then
			urlPrefix = host;
		else
			error('Try use host:'..host..'!\nBut format doesn\'t match!\nUse default'..LuaConst.Const.HTTP_PREFIX);
			urlPrefix = LuaConst.Const.HTTP_PREFIX;
		end
	else
		urlPrefix = LuaConst.Const.HTTP_PREFIX;
	end
	region = srvRegion;
	fullurl = urlPrefix..region;
	print("SetUrlPrifix&Region: "..GetUrlPrefixAndRegion());
end

function GetUrlPrefixAndRegion()
	if (fullurl ~= nil) then
		return fullurl;
	else
		return LuaConst.Const.HTTP_PREFIX..'1';
	end
end

function SetRegionId(srvid)
    region = srvid;
end

function GetRegionId()
    return region;
end

-- function SetDollarId(id)
-- 	dollar_id = id;
-- 	--test unit
-- 	--UnitTest.Test();
-- end

function RequestWithParams(protocol, params, msgid, doNotShowAnimeFlag)

	local url_params = params or {};
	url_params['msgid'] = msgid;
    
	-- if (dollar_id ~= nil) then
	-- 	url_params[DOLLAR_ID_KEY] = dollar_id;
	-- end

	local temp_tb1 = {};
	for k,v in pairs(url_params) do
		local temp_tb2 = {};
		table.insert(temp_tb2, k);
		table.insert(temp_tb2, '=');
		table.insert(temp_tb2, Util.UriEscapeDataString(v));
		local s = table.concat(temp_tb2, '');
		table.insert(temp_tb1, s);
	end

	local url = nil;
	if (#temp_tb1 == 0) then
		url = GetUrlPrefixAndRegion()..protocol;
	else
		url = GetUrlPrefixAndRegion()..protocol..'?';
	end

	local url_params_str = table.concat(temp_tb1, '&');
	url = url..url_params_str;

    print(url)
	--dataSystem:DoPost(url);
    DataSystem.Instance:DoPost(url);

	if (Lock_Streen_Switch) then
		WindowMgr.ShowWindow(LuaConst.Const.UIWaiting,{DoNotShowAnime = doNotShowAnimeFlag});
		WindowMgr.BlockInput();
	end
end

function HaveError(code)
	if (code == nil or string.len(code) == 0) then
		return false;
	else
		return true;
	end
end

function IsMatchGameVersion()
    WindowMgr.ShowWindow(LuaConst.Const.UIForceQuitGame);
end

function TurnOnLockScreenSwitch()
	Lock_Streen_Switch = true;
end

function TurnOffLockScreenSwitch()
	Lock_Streen_Switch = false;
end

function OnNoReq()
	WindowMgr.CloseWindow(LuaConst.Const.UIWaiting);
	WindowMgr.UnLockInput();
end
