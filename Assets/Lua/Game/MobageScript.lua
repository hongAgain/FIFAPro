module("MobageScript", package.seeall)

require "Game/MsgID"
require "Game/DataSystemScript"
require "Common/ktjson"
require "Common/UnityCommonScript"
require "Game/Login"

local mUserId = nil;
local oauth_token = nil;
local oauth_token_secret = nil;
local verifier = nil;
local mPlatform = nil;

function OnInit(platform)
	DataSystemScript.RegisterMsgHandler(MsgID.tb.Mobage_TempAuth, OnTempAuth);
	DataSystemScript.RegisterMsgHandler(MsgID.tb.Mobage_PayCreate, OnPayCreate);
	DataSystemScript.RegisterMsgHandler(MsgID.tb.Mobage_PaySubmit, OnPaySubmit);
    
    mPlatform = platform;
end

function SetUserId(userId)
    mUserId = userId;
end

function GetTempAuth()
    local dic = {};
    dic["userid"] = mUserId;
    DataSystemScript.RequestWithParams(LuaConst.Const.MobageCNCredential, dic, MsgID.tb.Mobage_TempAuth);
end

function OnTempAuth(code, data)
	oauth_token = data['oauth_token'];
	oauth_token_secret = data['oauth_token_secret'];

	local hash = Util.GetHashTable();
	Util.FillHashTable(hash, 'Method', 'Auth');
	Util.FillHashTable(hash, 'oauth_token', oauth_token);
	SDKMgr.CallSDK(hash);
end

function AuthOnSuccess(jsontxt)
    --TODO Mobage BUG
    mVerifier = jsontxt;
    --[[if (IsEditor()) then
        local tb = json.decode(jsontxt);
        mVerifier = tb['verifier'];
    elseif (IsMobile()) then
        mVerifier = jsontxt;
    else
        print("Cannot handle platform "..mPlatform);
    end]]--
end

function DispatchEvent(hashtable)
    local method = hashtable['Method'];
    if (method == "Login") then
        VerifyAuth();
    elseif (method == "BuyItem") then
        local itemId = hashtable["itemId"];
        BuyItem(itemId);
    else
        warn("Encounter method: "..method.." do nothing!");
    end
end

function VerifyAuth()
    local dic = {};
    dic['userid'] = mUserId;
    dic['token'] = oauth_token;
    dic['secret'] = oauth_token_secret;
    dic['verifier'] = mVerifier;
    if (mPlatform == "WindowsEditor") then
        dic['device_id'] = 0;
    elseif (mPlatform == "IPhonePlayer") then
        dic['device_id'] = 8;
    elseif (mPlatform == "Android") then
        dic['device_id'] = 16;
    else
        dic['device_id'] = 32;
    end
    

    DataSystemScript.RequestWithParams(LuaConst.Const.MobageCNLogin, dic, MsgID.tb.VerifyAuth);
end

function BuyItem(id)
    local dic = {};
    dic['id'] = id;
    DataSystemScript.RequestWithParams(LuaConst.Const.MobageCNPayCreate, dic, MsgID.tb.Mobage_PayCreate);
end

function OnPayCreate(code, data)
	local hash = Util.GetHashTable();
	Util.FillHashTable(hash, 'Method', 'ContinueTransaction')
	Util.FillHashTable(hash, 'id', data);

	SDKMgr.CallSDK(hash);
end

function PaySubmit(transactionId)
	local dic = {};
	dic['id'] = transactionId;
	DataSystemScript.RequestWithParams(LuaConst.Const.MobageCNPaySubmit, dic, MsgID.tb.Mobage_PaySubmit);
end

function OnPaySubmit(code, data)

end
 
function ShowWindow()
    WindowMgr.ShowWindow(LuaConst.Const.UILoginServerNoSDK, { DoEnterGame = VerifyAuth });
    Login.SetOnGetLatestServer(nil);
end

function ReqSrvList()
    Login.ReqSrvList();
    Login.ReqDefaultSrvId('mobageCN', mUserId);
   
    Login.SetOnGetLatestServer(ShowWindow);
end

function Get_oauth_token()
	return oauth_token;
end

function Get_oauth_token_secret()
	return oauth_token_secret;
end

--[[function IsEditor()
    return mPlatform == "OSXEditor" or mPlatform == "WindowsEditor";
end

function IsMobile()
    return mPlatform == "IPhonePlayer" or mPlatform == "Android";
end]]--