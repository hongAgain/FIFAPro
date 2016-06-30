module("UICoachScout",package.seeall);

require "CoachData"

local coachScoutSettings = {
	foregroundPrefabName = "CoachScoutInfo",
	foregroundPrefab = nil,
	backgroundPrefabName = "CoachScoutBackground",
	backgroundPrefab = nil,

	strLocalizeRepuIs = "CoachReputationis",
	strLocalizeSkillIs = "CoachSkill",

	avatarCameraLayer = 9,

	totalTimeToShow = 5,
	timerID = nil,
};

local buttonClose = nil;

local targetCoachAnime = nil;
local targetCoachID = nil;

local coachAvatar = nil;
local coachScoutForeground = nil;
local coachScoutBackground = nil;

local window = nil;
local windowComponent = nil;

function OnStart(gameObject,params)
	window = gameObject;
	windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");

	targetCoachID = params.targetCoachID;
	targetCoachAnime = params.targetCoachAnime;

	BindUI();
	SetInfo();
end

function BindUI()
	buttonClose = TransformFindChild(window.transform,"ButtonClose");
	AddOrChangeClickParameters(buttonClose.gameObject,CloseScene,nil);
end

function SetInfo()
	CreateCoachAvatar();
end

function CreateCoachAvatar()
	CreateForeground();
	CreateBackground();
	CommonScript.CreatePerson(
		targetCoachID, 
		LuaConst.Const.CreateRolePose, 
		targetCoachAnime, 
		OnInitScoutCoach,
		"CoachScoutAnime", 
		LuaConst.Const.PlayerTypeCoach
		);
end

function CreateForeground()
	if(coachScoutForeground == nil)then
		if(coachScoutSettings.foregroundPrefab==nil)then
			coachScoutSettings.foregroundPrefab = windowComponent:GetPrefab(coachScoutSettings.foregroundPrefabName);
		end
		coachScoutForeground = {};
		coachScoutForeground.gameObject = WindowMgr.Create3DUI(coachScoutSettings.foregroundPrefab);
		coachScoutForeground.transform = coachScoutForeground.gameObject.transform;
		coachScoutForeground.Name = TransformFindChild(coachScoutForeground.transform,"Name");
		coachScoutForeground.Rank = TransformFindChild(coachScoutForeground.transform,"Rank");
		coachScoutForeground.Reputation = TransformFindChild(coachScoutForeground.transform,"Reputation");
		coachScoutForeground.Skill = TransformFindChild(coachScoutForeground.transform,"Skill");

		GameObjectSetActive(coachScoutForeground.Skill,false);
	end

	UIHelper.SetLabelTxt(coachScoutForeground.Name,CoachData.GetCoachNameWithSuffix(targetCoachID));
	UIHelper.SetSpriteName(coachScoutForeground.Rank,CoachData.GetCoachRankRating(targetCoachID));
	UIHelper.SetLabelTxt(coachScoutForeground.Reputation,GetLocalizedString(coachScoutSettings.strLocalizeRepuIs)..CoachData.GetCoachReputation(targetCoachID));
	-- UIHelper.SetLabelTxt(coachScoutForeground.Skill,GetLocalizedString(coachScoutSettings.strLocalizeSkillIs)..CoachData.GetCoachSkill(targetCoachID));

	GameObjectSetActive(coachScoutForeground.transform,false);
end

function ShowForeground(willShow)
	GameObjectSetActive(coachScoutForeground.transform,willShow);
end

function CreateBackground()
	if(coachScoutBackground == nil)then
		if(coachScoutSettings.backgroundPrefab==nil)then
			coachScoutSettings.backgroundPrefab = windowComponent:GetPrefab(coachScoutSettings.backgroundPrefabName);
		end
		coachScoutBackground = {};
		coachScoutBackground.gameObject = GameObjectInstantiate(coachScoutSettings.backgroundPrefab);
		coachScoutBackground.transform = coachScoutBackground.gameObject.transform;
		coachScoutBackground.transform.localPosition = Vector3.New(0, 0, -17.7);
	    coachScoutBackground.transform.localScale = Vector3.New(2, 2, 1);
	end

	GameObjectSetActive(coachScoutBackground.transform,false);
end

function ShowBackground(willShow)
	GameObjectSetActive(coachScoutBackground.transform,willShow);
	if(willShow)then
		coachScoutBackground.gameObject.animation:Play();
	end
end

function OnInitScoutCoach(go)
	coachAvatar = go;
    SetLayer(coachAvatar, coachScoutSettings.avatarCameraLayer);
    coachAvatar.transform.position = Vector3.New(0.485, -0.833, -17.929);
    coachAvatar.transform.localEulerAngles = Vector3.New(-3.637207, -153.4755, -0.6422119);
    coachAvatar.transform.localScale = Vector3.New(0.99, 0.99, 0.99);
    
    ShowForeground(true);
    ShowBackground(true);

    coachScoutSettings.timerID = LuaTimer.AddTimer(false, coachScoutSettings.totalTimeToShow, CloseScene);
end

function CloseScene()
	if(coachScoutSettings.timerID~=nil)then
		--unregister timerid
		LuaTimer.RmvTimer(coachScoutSettings.timerID);
		coachScoutSettings.timerID = nil;
	end

	--play closing animations 

    ShowForeground(false);
    ShowBackground(false);
	GameObjectSetActive(coachAvatar.transform,false);

	Close();
end

function Close()
	windowComponent:Close();
end

function OnDestroy()
	--destroy avatar	
	if(coachAvatar~=nil)then
		GameObjectDestroy(coachAvatar);
		coachAvatar = nil;
	end
	--destory background
	if(coachScoutBackground~=nil)then
		GameObjectDestroy(coachScoutBackground.gameObject);
		coachScoutBackground = nil;
	end
	--destroy foreground
	if(coachScoutForeground~=nil)then
		GameObjectDestroy(coachScoutForeground.gameObject);
		coachScoutForeground = nil;
	end

	if(coachScoutSettings.timerID~=nil)then
		--unregister timerid
		LuaTimer.RmvTimer(coachScoutSettings.timerID);
		coachScoutSettings.timerID = nil;
	end
	coachScoutSettings.foregroundPrefab = nil;
	coachScoutSettings.backgroundPrefab = nil;

	buttonClose = nil;

	targetCoachAnime = nil;
	targetCoachID = nil;

	coachAvatar = nil;
	coachScoutForeground = nil;
	coachScoutBackground = nil;

	window = nil;
	windowComponent = nil;
end