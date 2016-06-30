--region NewFile_1.lua
--Author : hongpeng.zhao
--Date   : 2015/1/23
--此文件由[BabeLua]插件自动生成



--endregion
module("UIForceQuitGame", package.seeall)

require "Common/UnityCommonScript"


local window = nil;
local windowComponent = nil;
local quitGameTips = nil;

function OnStart(gameObject, params)
    print("OnStart..UIForceQuitGame!!!")
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");
    BindUI();
end

function OnDestroy()
end

function BindUI()
    local transform = window.transform;

    quitGameTips = TransformFindChild(transform, "QuitTips/Label");

    local btnQuitGame = TransformFindChild(transform, "QuitGame");
    Util.AddClick(btnQuitGame.gameObject, QuitGame);
end

function QuitGame()
    windowComponent:ForceQuitGame();
end
