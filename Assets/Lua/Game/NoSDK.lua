module("NoSDK", package.seeall)

require "Game/Login"

local OID = nil;

function ShowWindow()
    -- WindowMgr.ShowWindow(LuaConst.Const.UISelectServerNoSDK);
    -- Login.SetOnGetSrvList(nil);
    
    WindowMgr.ShowWindow(LuaConst.Const.UILoginServerNoSDK, { DoEnterGame = OnEnterGame });
    Login.SetOnGetLatestServer(nil);
end

function OnEnterGame()
    local t = {};
    t['oid'] = OID;
    DataSystemScript.RequestWithParams(LuaConst.Const.FreeLogin, t, MsgID.tb.VerifyAuth);
end

function Set_OID(oid)
    OID = oid;
    Login.SetOnGetLatestServer(ShowWindow);
	Login.ReqDefaultSrvId('test', oid);
end

function Get_OID()
    return OID;
end

-- Login.SetOnGetSrvList(ShowWindow);

WindowMgr.ShowWindow("UIInputName");