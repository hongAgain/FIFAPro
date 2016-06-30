module("Tutorial_UIFormation", package.seeall)

local uiHead = nil;
local obj1 = nil;
local obj2 = nil;
local pos1 = nil;
local pos2 = nil;

function Start()
    SceneManager.RegisterWatcher(OnEnterScene, LobbySceneManager.OnLeaveScene);
end

function OnEnterScene()
    LuaTutorial.RegisterTempOnOpenWindowCB("UILobby", Step1);
    WindowMgr.ShowWindow("UILobby");
end

function Step1()
    uiHead = GameObject.Find("UIHead(Clone)").transform;
    uiHead.localPosition = Vector3.zero;
    LuaTutorial.ReadConfig("8");
    Tutorial.Instance():WatchClick(Step2, false);
end

function Step2()
    local menuBtn = uiHead:Find("TopCenter/Btn - Menu").gameObject;
    Tutorial.Instance():FocusAt(menuBtn, true);
    LuaTutorial.ReadConfig("9");
    Tutorial.Instance():WatchClick(Step3, false);
    Tutorial.Instance():SetFinger(Vector2.zero, false, false, 45);
end

function Step3()
    Tutorial.Instance().transform.localPosition = Vector3.New(-320, 0, 0);
    LuaTutorial.ReadConfig("10");
    LuaTimer.AddTimer(false, 1, Step4);
end

function Step4()
    local formationBtn = uiHead:Find("LeftCenter/SideMenu/Button4").gameObject;
    Tutorial.Instance():FocusAt(formationBtn, true);
    Tutorial.Instance():WatchClick(Step5, false);
    Tutorial.Instance():SetFinger(Vector2.zero, false, false, 45);
    uiHead = nil;
end

function Step5()
    Tutorial.Instance():RevertFocus();
    Tutorial.Instance().transform.localPosition = Vector3.zero;
    LuaTutorial.RegisterTempOnOpenWindowCB("UIFormation", Step6);
end

local root = nil;
function Step6(gameObject)
    root = gameObject;
    local alternatesBtn = gameObject.transform:Find("Field/Mover/Pull_Btn");
    Tutorial.Instance():FocusAt(alternatesBtn.gameObject, true);
    LuaTutorial.ReadConfig("11");
    Tutorial.Instance():WatchClick(Step7, false);
    Tutorial.Instance():SetFinger(Vector2.zero, false, false, -45);
end

function Step7()
    Tutorial.Instance():RevertFocus();
    LuaTimer.AddTimer(false, 0.3, Step8);
end

function Step8()
    obj1 = root.transform:Find("Field/Scroll View - HeroList/Mover/UIAvatar 001");
    Tutorial.Instance():FocusAt(obj1.gameObject, true);
    LuaTutorial.ReadConfig("12");
    Tutorial.Instance():WatchDragDrop(DragStart, DragEnd, Step9);
    
    obj2 = root.transform:Find("Field/FormationRoot/1");
    Tutorial.Instance():WatchAnotherDragDrop(obj2.gameObject, DragStart, DragEnd, Step9);
    
    pos1 = obj1.localPosition;
    pos2 = obj2.localPosition;
end

function DragStart(gameObject)
end

function DragEnd(gameObject)
    obj1.localPosition = pos1;
    obj2.localPosition = pos2;
end

function Step9()
    LuaTutorial.SetStep(4);
    Tutorial.Instance():RevertFocus();
    Tutorial.Instance():ReleaseFocus(obj2.gameObject);
    LuaTutorial.ReadConfig("13");
    Tutorial.Instance():WatchClick(Step10, false);
end

function Step10()
    root:SendMessage("Close");
    root = nil;
    
    local uilobby = GameObjectFind("UI Root/UIAttach").transform:Find("UILobby");
    Tutorial_UITeamLegend.Step1(uilobby);
end
