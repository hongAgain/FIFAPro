module("LobbySceneManager",package.seeall)

require "Common/UnityCommonScript"
require "Common/CommonScript"
require "Game/Hero"
require "Game/WeatherManager"

local lobbySceneSettings = {
	ScenePrefabName = "Scene_LobbyCourt",
    
    TeamPlayerPos = {
    	Vector3.New(	8.171,	-0.86,	1.47),
    	Vector3.New(	8.215,	-0.86,	0.731),
    	Vector3.New(	8.24,	-0.86,	0.14),
    	Vector3.New(	8.281,	-0.86,	-0.528),
    	Vector3.New(	8.217,	-0.86,	-1.185),
    	Vector3.New(	8.22,	-0.86,	-1.77),

    	Vector3.New(	7.71,	-0.86,	1.149),
    	Vector3.New(	7.82,	-0.86,	0.537),
    	Vector3.New(	7.8,	-0.86,	-0.23),
    	Vector3.New(	7.832,	-0.86,	-1.008),
    	Vector3.New(	7.73,	-0.86,	-1.62)
	},
	TeamPlayerRot = {
    	Vector3.New(	0,	-110.8512,	0),
    	Vector3.New(	0,	-107.14,	0),
    	Vector3.New(	0,	-96.38,	0),
    	Vector3.New(	0,	-88.26,	0),
    	Vector3.New(	0,	-82.30,	0),
    	Vector3.New(	0,	-71.99695,	0),

    	Vector3.New(	0,	-110.95,	0),
    	Vector3.New(	0,	-103.0266,	0),
    	Vector3.New(	0,	-90,	0),
    	Vector3.New(	0,	-79.59811,	0),
    	Vector3.New(	0,	-67.55246,	0)

	},
	AnimName = {
		"POSE_01",
		"POSE_02",
		"POSE_03",
		"POSE_04",
		"POSE_05",
		"POSE_06",

		"POSE_07",
		"POSE_08",
		"POSE_09",
		"POSE_10",
		"POSE_11"
	},
	SpecialAnimName = {
		"POSE_13",
		"",
		"",
		"POSE_12",
		"",
		"POSE_12",

		"",
		"",
		"",
		"",
		""
	},
	LightSettings = {
		Sunny = {
			AmbientLight = Color.New(186/255,186/255,186/255,255/255),
			SceneLight = Color.New(231/255,228/255,217/255,255/255),
			SceneLightRotation = NewVector3(13.88414,84.44347,123.7691),
			SceneLightIntensity = 0.52,
			UseFogFlag = false
		},
		Rainy = {
			AmbientLight = Color.New(143/255,155/255,180/255,255/255),
			SceneLight = Color.New(255/255,255/255,240/255,255/255),
			SceneLightRotation = NewVector3(22.49339,84.04855,122.4064),
			SceneLightIntensity = 0.46,
			UseFogFlag = false
		}
	}
}

local lobbyScene = nil;
local lobbyTeamPlayers = {};

local weatherNodeSunny = nil;
local weatherNodeRainy = nil;
local lightNode = nil;

function CreateScene()
	DelayDeal.EnqueueEvent(CreateByStep, 3);
end

function CreateByStep(step)
	if (step == 0) then
        lobbyScene = Util.CreateLevelState(lobbySceneSettings.ScenePrefabName);
    elseif (step == 1) then
		CreateTeamPlayers();
	elseif (step == 2) then
		InitializeScene();
	end
end

function InitializeScene()
	BindNode();
	Util.DisableLightMap();
	-- process weather
	local weather = WeatherManager.GetWeather();
	if(weather == WeatherManager.weatherType.Sunny)then
		OnSunnyDay();
	elseif(weather == WeatherManager.weatherType.Rainy)then
		OnRainyDay();
	end
end

function BindNode()
	weatherNodeSunny = TransformFindChild(lobbyScene.transform,"WeatherNodeSunny");
	weatherNodeRainy = TransformFindChild(lobbyScene.transform,"WeatherNodeRainy");
	lightNode = TransformFindChild(lobbyScene.transform,"Light");
end

function OnSunnyDay()
	GameObjectSetActive(weatherNodeSunny,true);
	GameObjectSetActive(weatherNodeRainy,false);
	lightNode.localEulerAngles = lobbySceneSettings.LightSettings.Sunny.SceneLightRotation;
	Util.SetLight(lightNode,lobbySceneSettings.LightSettings.Sunny.SceneLight,lobbySceneSettings.LightSettings.Sunny.SceneLightIntensity);
	Util.SetAmbientLight(lobbySceneSettings.LightSettings.Sunny.AmbientLight);
	Util.SetFog(lobbySceneSettings.LightSettings.Sunny.UseFogFlag);
end

function OnRainyDay()
	GameObjectSetActive(weatherNodeSunny,false);
	GameObjectSetActive(weatherNodeRainy,true);	
	lightNode.localEulerAngles = lobbySceneSettings.LightSettings.Rainy.SceneLightRotation;
	Util.SetLight(lightNode,lobbySceneSettings.LightSettings.Rainy.SceneLight,lobbySceneSettings.LightSettings.Rainy.SceneLightIntensity);
	Util.SetAmbientLight(lobbySceneSettings.LightSettings.Rainy.AmbientLight);
	Util.SetFog(lobbySceneSettings.LightSettings.Rainy.UseFogFlag);
end

function IsSceneAvailable()
	if (lobbyScene ~= nil and lobbyScene.transform ~= nil) then
		return true;
	end
	return false;
end

function ShowScene(willShow)
   	GameObjectSetActive(lobbyScene.transform,willShow);
   	if(willShow)then
   		local weather = WeatherManager.GetWeather();
		if(weather == WeatherManager.weatherType.Sunny)then
			Util.DisableLightMap();
			Util.SetLight(lightNode,lobbySceneSettings.LightSettings.Sunny.SceneLight,lobbySceneSettings.LightSettings.Sunny.SceneLightIntensity);
			Util.SetAmbientLight(lobbySceneSettings.LightSettings.Sunny.AmbientLight);
			Util.SetFog(lobbySceneSettings.LightSettings.Sunny.UseFogFlag);
		elseif(weather == WeatherManager.weatherType.Rainy)then
			Util.DisableLightMap();
			Util.SetLight(lightNode,lobbySceneSettings.LightSettings.Rainy.SceneLight,lobbySceneSettings.LightSettings.Rainy.SceneLightIntensity);
			Util.SetAmbientLight(lobbySceneSettings.LightSettings.Rainy.AmbientLight);
			Util.SetFog(lobbySceneSettings.LightSettings.Rainy.UseFogFlag);
		end

		for i,v in ipairs(lobbyTeamPlayers) do
			Util.StartAnimAtRandomTime(lobbyTeamPlayers[i].animNode,lobbySceneSettings.AnimName[i],true);

			if(lobbySceneSettings.SpecialAnimName[i]~="")then
		    	--add a special anim randomly played		    	
		    	lobbyTeamPlayers[i].timerID = LuaTimer.AddTimer(false,math.Random(15, 30),function() PlaySpecialAnime(i);end);
		    end
		end
	else
		for i,v in ipairs(lobbySceneSettings.SpecialAnimName) do
			if(v~="" and lobbyTeamPlayers[i]~=nil and lobbyTeamPlayers[i].timerID~=nil)then
				LuaTimer.RmvTimer(lobbyTeamPlayers[i].timerID);
			end
		end
	end
end

function CreateTeamPlayers()
	local MainHeroID = Hero.MainTeamHeroId();
    local tb = CommonScript.DeepCopy(MainHeroID);
    Util.CreateLobbyPlayer("48004", tb);
end

function InitLobbyPlayers(tb)
	local MainHeroID = Hero.MainTeamHeroId();
	for i = 1, 11 do
        go = tb["Go_0"..i];
        go.transform.parent = lobbyScene.transform;
        go.name = MainHeroID[i];
        go.transform.localPosition = lobbySceneSettings.TeamPlayerPos[i];
        go.transform.localEulerAngles = lobbySceneSettings.TeamPlayerRot[i];
        
        lobbyTeamPlayers[i] = {};
        lobbyTeamPlayers[i].transform = go.transform;
        lobbyTeamPlayers[i].gameObject = go;
        lobbyTeamPlayers[i].animNode = go.transform:Find("Animation");
        --play animation from a random start
        Util.StartAnimAtRandomTime(lobbyTeamPlayers[i].animNode,lobbySceneSettings.AnimName[i], true);

        if (lobbySceneSettings.SpecialAnimName[i] ~= "") then
            --add a special anim randomly played
            lobbyTeamPlayers[i].timerID = LuaTimer.AddTimer(false,math.Random(15, 40),function() PlaySpecialAnime(i);end);
        end
	end
end

function PlaySpecialAnime(i)
	Util.PlayAnimation(lobbyTeamPlayers[i].animNode,lobbySceneSettings.SpecialAnimName[i],false);
	LuaTimer.RmvTimer(lobbyTeamPlayers[i].timerID);
	lobbyTeamPlayers[i].timerID = LuaTimer.AddTimer(false,math.Random(45, 90),function() PlaySpecialAnime(i);end);
end

function OnDestroy()
	print("LobbySceneDestroyed")

	for i,v in ipairs(lobbySceneSettings.SpecialAnimName) do
		if(v~="" and lobbyTeamPlayers[i]~=nil and lobbyTeamPlayers[i].timerID~=nil)then
			LuaTimer.RmvTimer(lobbyTeamPlayers[i].timerID);
		end
	end
end

function OnEnterScene()
    WindowMgr.ShowWindow(LuaConst.Const.UILobby);
    WindowMgr.ShowWindow(LuaConst.Const.UINews);
end

function OnLeaveScene()
	lobbyScene = nil;
    lobbyTeamPlayers = {};
end
