module("Tutorial_UICreateRole2", package.seeall)

local root = nil;

function Start()
    SceneManager.RegisterWatcher(OnEnterScene, LobbySceneManager.OnLeaveScene);
end

function OnEnterScene()
    LuaTutorial.RegisterTempOnOpenWindowCB("UICreateRole", Step1);
    WindowMgr.ShowWindow(LuaConst.Const.UICreateRole, { startIdx = 2 });
end

function Step1(gameObject)
    root = gameObject;
    LuaTutorial.ReadConfig("20");
    Tutorial.Instance():WatchClick(Step2, false);
end

function Step2()
    local nextBtn = root.transform:Find("Step2(Clone)/BR/Button");
    Tutorial.Instance():Hide(false);
    Tutorial.Instance():FocusAt(nextBtn.gameObject, false);
    Tutorial.Instance():WatchClick(Step3, false);
end

function Step3()
    local nextBtn = root.transform:Find("Step3(Clone)/Btn - R");
    Tutorial.Instance():FocusAt(nextBtn.gameObject, true);
    LuaTutorial.ReadConfig("21");
    Tutorial.Instance():WatchClick(Step6, false);
end

function Step6()
    LuaTutorial.ReadConfig("22");
    Tutorial.Instance():WatchClick(Step7, false);
end

function Step7()
    DataSystemScript.RegisterHookMsgHandler(MsgID.tb.InitRole1, Step8);
    Tutorial.Instance():Hide(false);
    --LuaTutorial.SetStep(6); --server will do it
end

function Step8()
    DataSystemScript.UnRegisterHookMsgHandler(MsgID.tb.InitRole1, Step8);
    
    LuaTutorial.ReadConfig("23");
    Tutorial.Instance():WatchClick(Step9, false);
end

function Step9()
    Tutorial.Instance():Hide(false);
    root:SendMessage("Close");
    root = nil;
    WindowMgr.ShowWindow(LuaConst.Const.UILobby);
end
