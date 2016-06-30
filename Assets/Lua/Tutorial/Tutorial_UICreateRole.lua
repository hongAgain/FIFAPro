module("Tutorial_UICreateRole", package.seeall)

local root = nil;

function Start()
    LuaTutorial.RegisterTempOnOpenWindowCB("UICreateRole", Step1);
    WindowMgr.ShowWindow(LuaConst.Const.UICreateRole, { startIdx = 1 });
end

function Step1(gameObject)
    root = gameObject;
    LuaTutorial.ReadConfig("1");
    Tutorial.Instance():WatchClick(Step2, false);
end

function Step2()
    local nextBtn = root.transform:Find("Step1(Clone)/BR/Button");
    Tutorial.Instance():Hide(false);
    Tutorial.Instance():FocusAt(nextBtn.gameObject, false);
    Tutorial.Instance():WatchClick(Step3, true);
end

function Step3()
    LuaTutorial.ReadConfig("2");
    Tutorial.Instance():WatchClick(Step4, false);
end

function Step4()
    DataSystemScript.RegisterHookMsgHandler(MsgID.tb.InitRole, Step5);
    Tutorial.Instance():Hide(false);
    local nextBtn = root.transform:Find("Step1(Clone)/BR/Button");
    nextBtn:SendMessage("OnClick");
end

function Step5()
    DataSystemScript.UnRegisterHookMsgHandler(MsgID.tb.InitRole, Step5);
    LuaTutorial.ReadConfig("3");
    Tutorial.Instance():WatchClick(Step6, false);
end

function Step6()
    Tutorial.Instance():Hide(false);
end
