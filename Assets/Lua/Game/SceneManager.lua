module("SceneManager",package.seeall);

local lobbySceneOnEnter = nil;
local lobbySceneOnLeave = nil;
--local battleSceneWatcher = nil;

local Scenes = 
{
	StartScene = {
		name = "Start",
	},
	BattleScene = {
		name = "GameAI",
	},
	TransitionScene = {
		name = "EmptyTransition",
	}		
}

local lastSceneName = nil;

function RegisterWatcher(lobbyOnEnter, lobbyOnLeave)
    lobbySceneOnEnter = lobbyOnEnter;
    lobbySceneOnLeave = lobbyOnLeave;
    --battleSceneWatcher = battle;
end

--==========change scene interfaces==========
function LoadLobbyScene()
	Util.LoadLevel(Scenes.TransitionScene.name);
end

function ReturnLobbyScene()
	Util.LoadLevel(Scenes.TransitionScene.name);
    --battleSceneWatcher.OnLeave();
end

function LoadBattleScene()
	Util.LoadLevel(Scenes.BattleScene.name);
    lobbySceneOnLeave();
end
--==========change scene interfaces==========

function OnLoadScene(newSceneName)
	if (newSceneName == Scenes.TransitionScene.name) then
        if (lastSceneName ~= Scenes.BattleScene.name) then
            lobbySceneOnEnter();
            RegisterWatcher(LobbySceneManager.OnEnterScene, LobbySceneManager.OnLeaveScene);
            LobbySceneManager.CreateScene();
        end
    elseif (newSceneName == Scenes.BattleScene.name) then
        WindowMgr.CloseWindow(LuaConst.Const.UILoading);
        WindowMgr.ShowWindow("UIBattle");
    end
    
	lastSceneName = newSceneName;
end