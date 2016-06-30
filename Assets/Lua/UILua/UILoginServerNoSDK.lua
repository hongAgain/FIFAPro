module ("UILoginServerNoSDK", package.seeall)


require "Common/CommonScript"
require "Common/UnityCommonScript"
require "Game/GameMainScript"
require "Game/DataSystemScript"

require "UILua/UILoginServerList"

--BGM name
local strBGMusic = "BG_LoginSelectServer";

-- strings for localizing label contents
local strLocalizeServerIndexSufix = "UILogin_Region";
local strLocalizeNewUserServerName = "NewUserServerName";

-- paths for looking up ui widgets under root
local root = nil;
local hierarchyPathDefaulServerNode = "Root/Sprite/DefaultServer/";
local hierarchyPathPopupRoot = "PopupRoot";

--root Gameobject this script is hooked up to
local window = nil;
--UIBaseWindowLua component on window
local windowComponent = nil;

local latestLoginData = nil;

local uiButtonShowServerList = nil;
local uiServerIndex = nil;
local uiServerName = nil;
local uiButtonEnterGame = nil;
--local uiLabelClickToShowServList = nil;
local uiPopupRoot = nil;

local doEnterGame = nil;

function OnStart(gameObject, params)
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");
    doEnterGame = params["DoEnterGame"];
    latestLoginData = Login.GetLatestLoginServerRole();
    BindUI();
    AudioMgr.Instance():PlayBGMusic(strBGMusic);
end

function BindUI()
	local transform = window.transform;

	uiButtonShowServerList  = TransformFindChild(transform, hierarchyPathDefaulServerNode.."ShowServerListButton");
	uiServerIndex           = TransformFindChild(transform, hierarchyPathDefaulServerNode.."ServerIndex");
	uiServerName            = TransformFindChild(transform, hierarchyPathDefaulServerNode.."ServerName");
	uiButtonEnterGame       = TransformFindChild(transform, "Root/ButtonEnterGame");
    root                    = TransformFindChild(transform, "Root");
	--uiLabelClickToShowServList = TransformFindChild(transform,hierarchyPathDefaulServerNode.."ClickToShowServList");
	uiPopupRoot             = TransformFindChild(transform, hierarchyPathPopupRoot);

	if (latestLoginData == nil or latestLoginData.sname == nil) then
		--new user
		RefreshSelectedServer(false, nil, nil);
	else
		RefreshSelectedServer(true, latestLoginData.sid, latestLoginData.sname);
	end

	Util.AddClick(uiButtonShowServerList.gameObject,ShowServerList);
	Util.AddClick(uiButtonEnterGame.gameObject,EnterGame);
end

function ShowServerList()
	
    local AfterRequesting = function ()
		GameObjectSetActive(root, false);
		UILoginServerList.CreateSelectServerListBox( uiPopupRoot, windowComponent, OnChooseServer);
		Login.SetOnGetSrvList(nil);
	end
	Login.SetOnGetSrvList(AfterRequesting);
	Login.ReqSrvList();
end

function OnChooseServer(host , sid , sname)    
	-- print("host:"..host);
 --    print("sid:"..sid);
 --    print("sname:"..sname);

    DataSystemScript.SetUrlPrefixAndRegion(host, sid);
	RefreshSelectedServer( true , sid, sname );
    
    GameObjectSetActive(root, true);
end

function EnterGame()
    windowComponent:Close();
    doEnterGame();
end

function RefreshSelectedServer( hasSelectedServer , sid, sname )
	-- body
	GameObjectSetActive(uiServerIndex.gameObject, hasSelectedServer);
	--GameObjectSetActive(uiLabelClickToShowServList.gameObject,hasSelectedServer);
	GameObjectSetActive(uiButtonEnterGame.gameObject, hasSelectedServer);
	if(hasSelectedServer) then 
		UIHelper.SetLabelTxt(uiServerIndex, sid..Util.LocalizeString(strLocalizeServerIndexSufix));
		UIHelper.SetLabelTxt(uiServerName, sname);
	else
		UIHelper.SetLabelTxt(uiServerName, Util.LocalizeString(strLocalizeNewUserServerName));
	end
end

function OnDestroy()
	UILoginServerList.OnDestroy();
end