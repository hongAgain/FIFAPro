module("UIInputNameScript", package.seeall)

require "Game/Login"
require "Common/UnityCommonScript"

local window = nil;
local windowComponent = nil;
local input = nil;

function OnStart(gameObject, params)
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");
    BindUI();
end

function OnDestroy()
end

function BindUI()
    local transform = window.transform;

    input = TransformFindChild(transform, "Sprite/Input");
    UIHelper.SetInputText(input,Util.GetString("UNTest"));

    local login = TransformFindChild(transform, "Button");
    Util.AddClick(login.gameObject, ReqSrvList);
end

function ReqSrvList()
    NoSDK.Set_OID(UIHelper.InputTxt(input));
    Util.SetString("UNTest",UIHelper.InputTxt(input));
    windowComponent:Close();
end