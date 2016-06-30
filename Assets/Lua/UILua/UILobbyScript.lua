module("UILobbyScript", package.seeall)

require "Common/UnityCommonScript"
require "UILua/RainScreenEffect"
require "UILua/UIGameCheckInsBox"
require "UILua/UIMatches"
require "UILua/UIShop"
require "Game/WeatherManager"
require "Game/LobbySceneManager"
require("Game/HintManager")

local lobby_strBGMusic = "BG_Lobby";

local buttonActivity = nil;
local buttonShop = nil;
local buttonGamble = nil;
local buttonRecharge = nil;
local buttonMail = nil;
local buttonRank = nil;
local buttonFriend = nil;
local buttonCheckIn = nil;
local buttonAnnouncement = nil;

local buttonLeague = nil;
local buttonCareer = nil;
local buttonCompetition = nil;
local buttonGuild = nil;

local buttonChat = nil;

local weatherNodeRainy = nil;

local window = nil;
local windowComponent = nil;

function OnStart(gameObject, params)
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");
    BindUI();
    SetInfo();
    ReqLobbyHintData();
end

function ReqLobbyHintData()
    HintManager.RequestToGetFuncTips();
end

function BindUI()    
    local transform = window.transform;
    
    buttonCarnival = TransformFindChild(transform, "BottomNode/ButtonCarnival");
    buttonActivity = TransformFindChild(transform,"BottomNode/ButtonActivity");
    buttonShop = TransformFindChild(transform,"BottomNode/ButtonShop");
    buttonGamble = TransformFindChild(transform,"BottomNode/ButtonGamble");
    buttonRecharge = TransformFindChild(transform,"BottomNode/ButtonRecharge");
    --buttonRank = TransformFindChild(transform,"BottomNode/ButtonRank");
    buttonMail = TransformFindChild(transform,"BottomNode/OtherPanel/ButtonMail");
    buttonFriend = TransformFindChild(transform,"BottomNode/OtherPanel/ButtonFriend");
    buttonCheckIn = TransformFindChild(transform,"BottomNode/OtherPanel/ButtonCheckIn");
    buttonAnnouncement = TransformFindChild(transform,"BottomNode/OtherPanel/ButtonAnnouncement");

    buttonLeague = TransformFindChild(transform,"RightNode/ButtonLeague");
    buttonCareer = TransformFindChild(transform,"RightNode/ButtonCareer");
    buttonCompetition = TransformFindChild(transform,"RightNode/ButtonCompetition");
    buttonGuild = TransformFindChild(transform,"RightNode/ButtonGuild");

    buttonChat = TransformFindChild(transform,"Broadcast/TopPanel/ButtonChat");

    weatherNodeRainy = TransformFindChild(transform,"WeatherNodeRainy");
end

function SetInfo()
    AddOrChangeClickParameters(buttonCarnival.gameObject, OnClickBtnCarnival, nil);
    AddOrChangeClickParameters(buttonActivity.gameObject,OnClickBtnActivity,nil);
    AddOrChangeClickParameters(buttonShop.gameObject,OnClickBtnShop,nil);
    AddOrChangeClickParameters(buttonGamble.gameObject,OnClickBtnGamble,nil);
    AddOrChangeClickParameters(buttonRecharge.gameObject,OnClickBtnRecharge,nil);
    --AddOrChangeClickParameters(buttonRank.gameObject,OnClickBtnRank,nil);
    AddOrChangeClickParameters(buttonMail.gameObject,OnClickBtnMail,nil);
    AddOrChangeClickParameters(buttonFriend.gameObject,OnClickBtnFriend,nil);
    AddOrChangeClickParameters(buttonCheckIn.gameObject,OnClickBtnCheckIn,nil);
    AddOrChangeClickParameters(buttonAnnouncement.gameObject,OnClickBtnAnnouncement,nil);

    AddOrChangeClickParameters(buttonLeague.gameObject,OnClickLeague,nil);
    AddOrChangeClickParameters(buttonCareer.gameObject,OnClickBtnCareer,nil);
    AddOrChangeClickParameters(buttonCompetition.gameObject,OnClickBtnCompetition,nil);
    -- AddOrChangeClickParameters(buttonGuild.gameObject,OnClickBtnGuild,nil);
    AddOrChangeClickParameters(buttonGuild.gameObject,BtnScout,nil);

    AddOrChangeClickParameters(buttonChat.gameObject,OnClickBtnChat,nil);

    local weather = WeatherManager.GetWeather();
    if(weather == WeatherManager.weatherType.Sunny)then
        OnSunnyDay();
    elseif(weather == WeatherManager.weatherType.Rainy)then
        OnRainyDay();
    end
end

function OnSunnyDay()
    GameObjectSetActive(weatherNodeRainy,false);
end

function OnRainyDay()
    GameObjectSetActive(weatherNodeRainy,true);
    RainScreenEffect.CreateRainScreenEffect( weatherNodeRainy, NewVector3(-530,200,0), NewVector3(280,-100,0) );
end

function OnClickBtnRecharge()
    -- WindowMgr.ShowWindow(LuaConst.Const.UIRecharge);
    WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { "充值功能紧张开发中，敬请期待！" });
end

function OnClickBtnGamble()
    WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { "比赛竞猜功能紧张开发中，敬请期待！" });
end

function OnClickBtnGuild()
    WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { "联盟功能紧张开发中，敬请期待！" });
end

function OnClickBtnCareer()
    WindowMgr.ShowWindow(LuaConst.Const.UIMatches,{willOpenCareer = true});
end

function OnClickBtnCompetition()
    WindowMgr.ShowWindow(LuaConst.Const.UIMatches,{willOpenCareer = false});
end

function OnClickBtnShop()
    WindowMgr.ShowWindow(LuaConst.Const.UIShop,{shopType=UIShopSettings.ShopType.DiamondShop});
end

function BtnScout()
    WindowMgr.ShowWindow(LuaConst.Const.UIScout);
end

function OnClickBtnActivity()
    WindowMgr.ShowWindow(LuaConst.Const.UIActivity);
    -- WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { "活动功能紧张开发中，敬请期待！" });
end

function OnClickBtnMail()
    WindowMgr.ShowWindow(LuaConst.Const.UIGameMail);
end

function OnClickBtnCheckIn()
    -- WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { "签到功能紧张开发中，敬请期待！" });
    local AfterRequesting = function ()
        local containerTransform = TransformFindChild(window.transform, "PopupRoot");
        local newsListDepth = UIHelper.GetMaxDepthOfPanelInChildren(containerTransform) + 1;
        UIGameCheckInsBox.CreateCheckInBox( containerTransform, windowComponent, newsListDepth);
    end
    GameCheckInsData.RequestCheckInInfo( AfterRequesting );
end

function OnClickBtnFriend()
    local OnReqFriendList = function (data)
        WindowMgr.ShowWindow(LuaConst.Const.UIFriend, data);
    end        
    FriendData.ReqFriendList(OnReqFriendList, 1, 50);
 end

function OnClickBtnChat()
    WindowMgr.ShowWindow(LuaConst.Const.UIChatting);
end

function OnClickBtnCarnival()
    WindowMgr.ShowWindow(LuaConst.Const.UICarnival);
end

function OnClickBtnAnnouncement()
    WindowMgr.ShowWindow(LuaConst.Const.UINews);
end

function OnClickLeague()
    local function AfterRequesting(data_)
        if (tonumber(data_.s) == 0) then
            WindowMgr.ShowWindow(LuaConst.Const.UIPVELeague, data_);
        else
            WindowMgr.ShowWindow(LuaConst.Const.UIPVELeagueSchedule, data_);
        end
    end
    PVELeagueData.RequestPVELeagueInfo(AfterRequesting);
end

function OnHide()
    if(LobbySceneManager.IsSceneAvailable())then
        LobbySceneManager.ShowScene(false);
    else
        LobbySceneManager.CreateScene();
    end
end

function OnShow()
    if(LobbySceneManager.IsSceneAvailable())then
        LobbySceneManager.ShowScene(true);
    else
        LobbySceneManager.CreateScene();
    end    
    ReqLobbyHintData();
end

function OnDestroy()
    UIGameCheckInsBox.OnDestroy();
    LobbySceneManager.OnDestroy();
    RainScreenEffect.OnDestroy();

    buttonActivity = nil;
    buttonShop = nil;
    buttonGamble = nil;
    buttonRecharge = nil;
    buttonMail = nil;
    buttonRank = nil;
    buttonCheckIn = nil;
    buttonAnnouncement = nil;

    buttonLeague = nil;
    buttonCareer = nil;
    buttonCompetition = nil;
    buttonGuild = nil;

    buttonChat = nil;

    window = nil;
    windowComponent = nil;
end
