module("UIPrivateChat",package.seeall);

require "Game/Role"
require "Game/FormationCMP"

local playerData = nil;

local icon = nil;
local name = nil;
local level = nil;
-- local power = nil;
local vip = nil;

local formationName = nil;

local buttonStartChat = nil;
local buttonClose = nil;

local avatarRoot = nil;
local uiAvatarPrefab = nil;
local avatars = {};

local window = nil;
local windowComponent = nil;


function OnStart(gameObject, params)
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");

    BindUI();
    local AfterRequest = function (data)
    	playerData = data;
    	SetInfo();
    end
    Role.GetTargetRoleData(params.targetUID,AfterRequest);
end

function OnShowUpdate(params)
	local AfterRequest = function (data)
    	playerData = data;
    	SetInfo();
    end
    Role.GetTargetRoleData(params.targetUID,AfterRequest);
end

function BindUI()
    local transform = window.transform;

	icon = TransformFindChild(transform,"Head/IconRoot/Icon");
	name = TransformFindChild(transform,"Head/Name");
	level = TransformFindChild(transform,"Head/Level");
	power = TransformFindChild(transform,"Head/Power");
	GameObjectSetActive(power,false);
	vip = TransformFindChild(transform,"Head/Vip");

	formationName = TransformFindChild(transform,"FormationName");

	buttonStartChat = TransformFindChild(transform,"ButtonPrivateChat");
	buttonClose = TransformFindChild(transform,"ButtonClose");
	AddOrChangeClickParameters(buttonStartChat.gameObject,OnClickButtonStartPrivateChat,nil);
	AddOrChangeClickParameters(buttonClose.gameObject,OnClickButtonClose,nil);

	avatarRoot = TransformFindChild(transform,"UIAvatarRoot");    
end

function SetInfo()
	-- body
	Util.SetUITexture(icon,LuaConst.Const.ClubIcon,playerData.icon.."_2",true);
	UIHelper.SetLabelTxt(name,playerData.name);
	UIHelper.SetLabelTxt(level,"Lv."..playerData.lv);
	-- UIHelper.SetLabelTxt(power,playerData.power);
	UIHelper.SetLabelTxt(vip,"vip"..playerData.vip);

	UIHelper.SetLabelTxt(formationName,Config.GetTemplate(Config.positionTB)[tostring(playerData.team.id)].name);

	if(not IsTableEmpty(playerData.team.hero))then

		DestroyUIListItemGameObjects(avatars);
    	uiAvatarPrefab = Util.GetGameObject("UIAvatar_Formation");

    	local avatarIndex = 1;
    	for k,v in pairs(playerData.team.hero) do
    		local clone = GameObjectInstantiate(uiAvatarPrefab);
	        clone.transform.parent = avatarRoot;
	        clone.transform.name = v;
	        clone.transform.localPosition = Vector3.zero;
	        clone.transform.localScale = Vector3.one;

	        avatars[avatarIndex] = clone.transform;
	        avatarIndex = avatarIndex+1;
    	end
    end

	local formationFriend = FormationCMP.New();
    formationFriend:InitFormation(tostring(playerData.team.id),avatars,FormationCMP.enum_CMPType.Type1,playerData.team.hero);
end


function OnClickButtonStartPrivateChat()
    WindowMgr.ShowWindow(LuaConst.Const.UIChatting, { 
    	targetChannelID = 2,
    	DefaultTargetPlayerName = playerData.name,
    	DefaultTargetPlayerUID = playerData.uid });
end

function OnClickButtonClose()
	windowComponent:Close();
end

function OnDestroy()
	playerData = nil;
	icon = nil;
	name = nil;
	level = nil;
	-- power = nil;
	vip = nil;
	formationName = nil;
	buttonStartChat = nil;
	buttonClose = nil;
	avatarRoot = nil;
	uiAvatarPrefab = nil;
	avatars = {};
	window = nil;
	windowComponent = nil;
end