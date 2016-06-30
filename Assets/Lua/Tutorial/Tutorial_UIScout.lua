module("Tutorial_UIScout", package.seeall)

local root = nil;

function Start()
    SceneManager.RegisterWatcher(OnEnterScene, LobbySceneManager.OnLeaveScene);
end

function OnEnterScene()
    LuaTutorial.RegisterTempOnOpenWindowCB("UILobby", Step1);
    WindowMgr.ShowWindow("UILobby");
end

function Step1(gameObject)
    local scoutBtn = gameObject.transform:Find("RightNode/ButtonGuild");
    Tutorial.Instance():FocusAt(scoutBtn.gameObject, true);
    LuaTutorial.ReadConfig("4");
    Tutorial.Instance():WatchClick(Step2, true);
    Tutorial.Instance():SetFinger(Vector2.zero, false, false, 0);
end

function Step2()
    LuaTutorial.RegisterTempOnOpenWindowCB("UIScout", Step3);
    WindowMgr.ShowWindow("UIScout");
end

function Step3(gameObject)
    root = gameObject;
    
    local rmbLookBtn = root.transform:Find("Root/Scout2/ScoutMover/Block1/Btn - Look");
    Tutorial.Instance():FocusAt(rmbLookBtn.gameObject, true);
    LuaTutorial.ReadConfig("5");
    Tutorial.Instance():WatchClick(Step4, false);
    Tutorial.Instance():SetFinger(Vector2.New(0, 120), false, true, 0);
end

function Step4()
    Tutorial.Instance():RevertFocus();
    
    LuaTimer.AddTimer(false, 0.6, Step5);
end

function Step5()
    local rmbBtn = root.transform:Find("Root/Scout2/ScoutMover/Block2/Btn - Buy1");
    Tutorial.Instance():FocusAt(rmbBtn.gameObject, true);
    LuaTutorial.ReadConfig("6");
    Tutorial.Instance():WatchClick(Step6, false);
    Tutorial.Instance():SetFinger(Vector2.zero, false, false, 0);
end

function Step6()
    Tutorial.Instance():Hide(true);
    LuaTutorial.SetStep(3);
    DataSystemScript.RegisterHookMsgHandler(MsgID.tb.ScoutOne, Step7);
end

function Step7()
    DataSystemScript.UnRegisterHookMsgHandler(MsgID.tb.ScoutOne, Step7);
    LuaTimer.AddTimer(false, 8, Step8);
end

function Step8()
    LuaTutorial.ReadConfig("7");
    Tutorial.Instance():WatchClick(Step9, false);
end

function Step9()
    root:SendMessage("Close");
    Tutorial_UIFormation.Step1();
    root = nil;
end
