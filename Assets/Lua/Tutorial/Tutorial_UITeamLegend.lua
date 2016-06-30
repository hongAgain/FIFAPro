module("Tutorial_UITeamLegend", package.seeall)

function Start()
    SceneManager.RegisterWatcher(OnEnterScene, LobbySceneManager.OnLeaveScene);
end

function OnEnterScene()
    LuaTutorial.RegisterTempOnOpenWindowCB("UILobby", Step1);
    WindowMgr.ShowWindow("UILobby");
end

function Step1(gameObject)
    local pveBtn = gameObject.transform:Find("RightNode/ButtonCareer").gameObject;
    Tutorial.Instance():FocusAt(pveBtn, true);
    LuaTutorial.ReadConfig("14");
    Tutorial.Instance():WatchClick(Step2, false);
    Tutorial.Instance():SetFinger(Vector2.zero, false, false, 0);
end

function Step2()
    LuaTutorial.RegisterTempOnOpenWindowCB("UIMatches", Step3);
end

function Step3(gameObject)
    local pveBtn = gameObject.transform:Find("uiScrollView/uiContainer/1/ButtonTeamLegend").gameObject;
    Tutorial.Instance():FocusAt(pveBtn, true);
    Tutorial.Instance():WatchClick(Step4, false);
    Tutorial.Instance():SetFinger(Vector2.zero, false, false, 0);
end

function Step4()
    LuaTutorial.RegisterTempOnOpenWindowCB("UITeamLegend", Step5);
end

function Step5(gameObject)
    local goBtn = gameObject.transform:Find("LeftPanel/Left/Go").gameObject;
    Tutorial.Instance():FocusAt(goBtn, true);
    LuaTutorial.ReadConfig("15");
    Tutorial.Instance():WatchClick(Step6, true);
    Tutorial.Instance():SetFinger(Vector2.zero, false, false, 0);
end

function Step6()
    TeamLegendData.ReqRaidInfo(TeamLegendData.GetClickCountry(), Step7);
end

function Step7()
    LuaTutorial.RegisterTempOnOpenWindowCB("UILevelMap", Step8);
    WindowMgr.ShowWindow("UILevelMap");
end

function Step8(gameObject)
    local point = gameObject.transform:Find("Center/ScrollViewPanel/SpriteRoot/1");
    Tutorial.Instance():FocusAt(point.gameObject, true);
    LuaTutorial.ReadConfig("16");
    Tutorial.Instance():WatchClick(Step9, false);
    Tutorial.Instance():SetFinger(Vector2.zero, false, false, 0);
end

function Step9()
    LuaTutorial.RegisterTempOnOpenWindowCB("UIChallengeTL", Step10);
end

function Step10(gameObject)
    local challengeBtn = gameObject.transform:Find("BtnChallengeM");
    Tutorial.Instance():FocusAt(challengeBtn.gameObject, true);
    LuaTutorial.ReadConfig("17");
    Tutorial.Instance():WatchClick(Step11, false);
    Tutorial.Instance():SetFinger(Vector2.New(0, 120), false, true, 0);
end

function Step11()
    LuaTutorial.RegisterTempOnOpenWindowCB("UIPrepareMatch", Step12);
end

function Step12(gameObject)
    local beginMatchBtn = gameObject.transform:Find("CenterPanel/BottomPart/Button1");
    Tutorial.Instance():FocusAt(beginMatchBtn.gameObject, true);
    LuaTutorial.ReadConfig("18");
    Tutorial.Instance():WatchClick(Step13, false);
    Tutorial.Instance():SetFinger(Vector2.New(0, 120), false, true, 0);
end

function Step13()
    LuaTutorial.SetStep(5);
    Tutorial.Instance():Hide(false);
    LuaTutorial.RegisterTempOnOpenWindowCB("UIBattleResultS", Step14);
end

function Step14(gameObject)
    local continueBtn = gameObject.transform:Find("SuccessPart/Bottom/BtnContinue");
    Tutorial.Instance():FocusAt(continueBtn.gameObject, true);
    LuaTutorial.ReadConfig("19");
    Tutorial.Instance():WatchClick(Step15, false);
    Tutorial.Instance():SetFinger(Vector2.New(0, 120), false, true, 0);
end

function Step15()
    Tutorial_UICreateRole2.OnEnterScene();
end
