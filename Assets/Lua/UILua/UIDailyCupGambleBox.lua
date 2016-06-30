module("UIDailyCupGambleBox",package.seeall);

require "Config"
require "Common/UnityCommonScript"
require "Game/GameMainScript"
require "Game/Role"
require "Game/ItemSys"
require "Game/Hero"

--require "UILua/UIDailyCupScript"
require "UILua/UIDailyCupGamPlayerList"
require "UILua/UIDailyCupGamConfirmBox"

gambleSettings = {
	GamBoxPrefabName = "DailyCupGamBox",
	GamIconDefault = "Default",
	GamNameDefault = "DailyCupGamPlayerName",
	ButtonColorNormal = Color.New(125/255,128/255,137/255,255/255),
	ButtonColorChosen = Color.New(108/255,161/255,101/255,255/255),
	ButtonContentNormal = "Support",
	ButtonContentChosen = "DailyCupGamSupport",
	GamPlayerDesc = "DailyCupGamPlayerDesc",
	GamConfirmBoxPrefabName = "DailyCupGamConfirmBox"
}

local gamBox = nil;

local buttonConfirm = nil;
local buttonConfirmContent = nil;
local buttonCancel = nil;
local buttonClose = nil;

local buttonGamTarget = {};

local players = {};

local labelDeadLine = nil;

local GambleParameters = {
	-- hid=nil,
	-- cid=nil,
	-- win=nil
};
local LastGamParameters = {
	-- cid = nil,
	-- win=nil
}

local delegateOnGamble = nil;
local delegateOnSelectGamPlayer = nil;

local uiPopupRoot = nil;

local windowComponent = nil;

function CreateGambleBox(containerTransform,windowcomponent,depth,matchID,deadLineTime,users,defaultPlayer,onGamble,onSelectGamPlayer)
	if(gamBox == nil) then
		--instantiate one
		local gamBoxPrefab = windowcomponent:GetPrefab(gambleSettings.GamBoxPrefabName);
		--instantiate prefab and initialize it
		gamBox = GameObjectInstantiate(gamBoxPrefab);
		gamBox.transform.parent = containerTransform;
    	gamBox.transform.localPosition = Vector3.zero;
    	gamBox.transform.localScale = Vector3.one;

    	buttonConfirm = TransformFindChild(gamBox.transform,"ButtonConfirm");
    	buttonConfirmContent = TransformFindChild(buttonConfirm,"Label");
    	Util.AddClick(buttonConfirm.gameObject,OnClickConfirm);
    	-- UIHelper.SetButtonActive(buttonConfirm,false,true);
    	SetButtonConfirmActive(false);
    	buttonCancel = TransformFindChild(gamBox.transform,"ButtonCancel");
    	Util.AddClick(buttonCancel.gameObject,OnClickCancel);
    	buttonClose = TransformFindChild(gamBox.transform,"ButtonClose");
    	Util.AddClick(buttonClose.gameObject,OnClickClose);

    	buttonGamTarget = {};
    	buttonGamTarget.button = TransformFindChild(gamBox.transform,"ButtonGam");
    	buttonGamTarget.icon = TransformFindChild(buttonGamTarget.button,"IconRoot/Icon");
    	buttonGamTarget.name = TransformFindChild(buttonGamTarget.button,"Name");
    	buttonGamTarget.defaultIcon = TransformFindChild(buttonGamTarget.button,"DefaultIcon");
    	Util.AddClick(buttonGamTarget.button.gameObject,OnClickGamTarget);
    	
    	players = {};

    	for i=1,2 do
    		
    		players[i] = {};
    		players[i].transform = TransformFindChild(gamBox.transform,"Player"..i);
    		players[i].name = TransformFindChild(players[i].transform,"Name");
    		players[i].icon = TransformFindChild(players[i].transform,"IconRoot/Icon");
    		players[i].isMyTeam = TransformFindChild(players[i].transform,"IsMyTeam");

    		players[i].buttonSupport = TransformFindChild(players[i].transform,"ButtonSupport");
    		players[i].buttonContent = TransformFindChild(players[i].buttonSupport,"Label");
    		AddOrChangeClickParameters(players[i].transform.gameObject,OnClickPlayerButton,{playerIndex = i});
    		AddOrChangeClickParameters(players[i].buttonSupport.gameObject,OnClickSupport,{playerIndex = i});
    	end

    	labelDeadLine = TransformFindChild(gamBox.transform,"DeadLine");

    	uiPopupRoot = TransformFindChild(gamBox.transform,"PopupRoot");
	end
	--active it
	if(not GameObjectActiveSelf(gamBox)) then
    	GameObjectSetActive(gamBox.transform,true);
    end

	delegateOnGamble = onGamble;
	delegateOnSelectGamPlayer = onSelectGamPlayer;

	windowComponent = windowcomponent;

	UIHelper.SetPanelDepth(uiPopupRoot,depth+1);

	--set info
	SetInfo(matchID,deadLineTime,users,defaultPlayer);
end

function SetInfo(matchID,deadLineTime,users,defaultPlayer)

    --default settings
	-- UIHelper.SetButtonActive(buttonConfirm,false,true);
	SetButtonConfirmActive(false);

	GambleParameters = {};
	GambleParameters.hid = nil;
	GambleParameters.cid = matchID;

	if(GambleParameters.cid == LastGamParameters.cid)then
		GambleParameters.win = LastGamParameters.win;
	else
		GambleParameters.win = nil;
	end

	local timeNum = tonumber(deadLineTime);
	UIHelper.SetLabelTxt(labelDeadLine,math.floor(timeNum/100)..":"..(timeNum%100));

	for i=1,2 do
		Util.SetUITexture(players[i].icon, LuaConst.Const.ClubIcon, tostring(users[i].icon).."_2", true);
		UIHelper.SetLabelTxt(players[i].name,users[i].name);
		GameObjectSetActive(players[i].isMyTeam.gameObject,(users[i].uid == Role.Get_uid()));
		if(GambleParameters.win == i)then
			UIHelper.SetWidgetColor(players[i].buttonSupport,gambleSettings.ButtonColorChosen);
			UIHelper.SetWidgetColor(players[i].buttonContent,gambleSettings.ButtonColorChosen);
			UIHelper.SetLabelTxt(players[i].buttonContent,GetLocalizedString(gambleSettings.ButtonContentChosen));
		else
			UIHelper.SetWidgetColor(players[i].buttonSupport,gambleSettings.ButtonColorNormal);
			UIHelper.SetWidgetColor(players[i].buttonContent,gambleSettings.ButtonColorNormal);
			UIHelper.SetLabelTxt(players[i].buttonContent,GetLocalizedString(gambleSettings.ButtonContentNormal));
		end
	end

	if(defaultPlayer~=nil)then
		GameObjectSetActive(buttonGamTarget.icon,true);
		GameObjectSetActive(buttonGamTarget.defaultIcon,false);

		GambleParameters.hid = defaultPlayer.id;
		Util.SetUITexture(buttonGamTarget.icon, LuaConst.Const.PlayerHeadIcon, HeroData.GetHeroIcon(GambleParameters.hid), true);
		UIHelper.SetLabelTxt(buttonGamTarget.name,HeroData.GetHeroName(GambleParameters.hid));

		CheckParameters();
	else
		GameObjectSetActive(buttonGamTarget.icon,false);
		GameObjectSetActive(buttonGamTarget.defaultIcon,true);

		Util.SetUITexture(buttonGamTarget.icon, LuaConst.Const.PlayerHeadIcon, gambleSettings.GamIconDefault, true);
    	UIHelper.SetLabelTxt(buttonGamTarget.name,GetLocalizedString(gambleSettings.GamNameDefault));
	end
end

function OnClickGamTarget()
	WindowMgr.ShowWindow(LuaConst.Const.UIDailyCupGamPlayerList,{delegateOnSelectPlayer = delegateOnSelectGamPlayer});
end

function OnClickPlayerButton(go)
	-- show formation info

end

function OnClickSupport(go)
	local listener = UIHelper.GetUIEventListener(go);
	if( listener.parameter.playerIndex ~= nil ) then		
		GambleParameters.win = listener.parameter.playerIndex;
		for i=1,2 do
			if(i == listener.parameter.playerIndex) then
				UIHelper.SetWidgetColor(players[i].buttonSupport,gambleSettings.ButtonColorChosen);
				UIHelper.SetWidgetColor(players[i].buttonContent,gambleSettings.ButtonColorChosen);
				UIHelper.SetLabelTxt(players[i].buttonContent,GetLocalizedString(gambleSettings.ButtonContentChosen));
			else
				UIHelper.SetWidgetColor(players[i].buttonSupport,gambleSettings.ButtonColorNormal);
				UIHelper.SetWidgetColor(players[i].buttonContent,gambleSettings.ButtonColorNormal);
				UIHelper.SetLabelTxt(players[i].buttonContent,GetLocalizedString(gambleSettings.ButtonContentNormal));
			end
		end
		LastGamParameters = {};
		LastGamParameters.cid = GambleParameters.cid;
		LastGamParameters.win = GambleParameters.win;
		CheckParameters();
	end
end

function CheckParameters()
	if (GambleParameters~=nil 
			and GambleParameters.hid ~= nil 
			and	GambleParameters.cid ~= nil 
			and GambleParameters.win ~= nil)then
    	-- UIHelper.SetButtonActive(buttonConfirm,true,true);
    	SetButtonConfirmActive(true);
	end
end

function SetButtonConfirmActive(willActivate)
	UIHelper.SetButtonActive(buttonConfirm,willActivate,true);
	if(willActivate)then
		UIHelper.SetWidgetColor(buttonConfirmContent,Color.New(207/255,251/255,255/255,255/255));
	else
		UIHelper.SetWidgetColor(buttonConfirmContent,Color.New(128/255,128/255,128/255,255/255));
	end
end

function OnClickCancel()
	Close();
end

function OnClickClose()
	Close();
end

function OnClickConfirm()	

	UIDailyCupGamConfirmBox.CreateGambleConfirmBox(uiPopupRoot,
		windowComponent,
		gambleSettings.GamConfirmBoxPrefabName,
		HeroData.GetHeroIcon(GambleParameters.hid),
		GetLocalizedString(gambleSettings.GamPlayerDesc,HeroData.GetHeroName(GambleParameters.hid),1),
		function ()
			-- body
			local AfterReq = function ()
				if(delegateOnGamble~=nil)then
					delegateOnGamble();
				end
				Close();
			end
			-- PVPMsgManager.RequestDailyCupGam(GambleParameters,AfterReq);
			PVPMsgManager.RequestPVPMsg(MsgID.tb.DailyCupGam, LuaConst.Const.DailyCupGam, GambleParameters, AfterReq, nil );
		end,
		nil);
end

function Close()
	if (GameObjectActiveSelf(gamBox)) then
    	GameObjectSetActive(gamBox.transform,false);
    end
end

function OnDestroy()

	UIDailyCupGamConfirmBox.OnDestroy();

	gamBox = nil;
	buttonConfirm = nil;
	buttonCancel = nil;
	buttonClose = nil;
	buttonGamTarget = {};
	players = {};
	labelDeadLine = nil;
	GambleParameters = {};
	delegateOnGamble = nil;
end