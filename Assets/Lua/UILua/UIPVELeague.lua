module("UIPVELeague", package.seeall)

require "Common/UnityCommonScript"
require "UILua/UIPVELeagueJoinBox"

local window = nil;
local windowComponent = nil;

local leagueClassBtnPrefab = nil;
local leagueBtnPrefab = nil;
local popupRoot = nil;
local uiScrollView = nil;
local uiContainer = nil;
local uiLeagueScrollView = nil;
local uiLeagueContainer = nil;

local leagueConfig = nil;
local leagueConfigMaxIdNum = 0;
local leagueClassCount = 0;

local leagueScheduleInfo = nil;
local currentLeagueNum = nil;
local leagueOpen = {};           -- 联赛是否开启：e.g. { "1"=true, "2"=false }
local leagueClass = {};          -- 联赛类index：e.g. { "初级联赛"="1", "地区联赛"="2" }
local leagueClassInvert = {};    -- e.g. { "1"="初级联赛", "2"="地区联赛" }
local leagueClassOpen = {};      -- 联赛类是否开启：e.g. { "初级联赛"=true, "地区联赛"=false }
local minLeagueClassOpen = math.huge;
local leagueClassLeagues = {};

local uiLeagueClassList = {};

function OnStart(gameObject, params)
    print("UIPVELeague.lua .. OnStart");
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");
    leagueClassBtnPrefab = windowComponent:GetPrefab("UIPVELeagueButton");
    leagueBtnPrefab = windowComponent:GetPrefab("UIPVELeagueLV2Button");

    leagueScheduleInfo = params;
    currentLeagueNum = tonumber(leagueScheduleInfo.s);

    local transform = window.transform;
    popupRoot          = TransformFindChild(transform, "PopupRoot");
    uiScrollView       = TransformFindChild(transform, "MainUIRoot/UIScrollView");
    uiContainer        = TransformFindChild(uiScrollView, "UIContainer");
    uiLeagueScrollView = TransformFindChild(transform, "MainUIRoot/UILeagueScrollView");
    uiLeagueContainer  = TransformFindChild(uiLeagueScrollView, "UILeagueContainer");

    SetInfo();
end

function SetInfo()
    leagueConfig = Config.GetTemplate(Config.PVELeague());

    SetLeagueInfo();
    SetButtons();

    UIHelper.RepositionGrid(uiContainer, uiScrollView);
    UIHelper.RefreshPanel(uiScrollView);
end

function SetLeagueInfo()
    -- 计算 leagueOpen
    local tempKeyNum = nil;
    for k, v in pairs(leagueConfig) do
        tempKeyNum = tonumber(k);
        if (tempKeyNum > leagueConfigMaxIdNum) then
            leagueConfigMaxIdNum = tempKeyNum;
        end

        if (v.class_id > currentLeagueNum) then
            leagueOpen[k] = false;
        else
            leagueOpen[k] = true;
        end
    end
    -- 初始化 leagueClass, leagueClassInvert, leagueClassOpen, leagueClassLeagues
    local leagueInfo = nil;
    local tempLeagueClassNum = nil;
    for i = 1, leagueConfigMaxIdNum, 1 do
        leagueInfo = leagueConfig[tostring(i)];
        if (leagueInfo ~= nil) then
            if (leagueClass[leagueInfo.className] == nil) then
                leagueClassCount = leagueClassCount + 1;
                leagueClass[leagueInfo.className] = tostring(leagueClassCount);
                leagueClassInvert[tostring(leagueClassCount)] = leagueInfo.className;

                leagueClassOpen[leagueInfo.className] = false;
            end

            if (leagueOpen[leagueInfo.id] == true) then
                leagueClassOpen[leagueInfo.className] = true;
                tempLeagueClassNum = tonumber(leagueClass[leagueInfo.className]);
                if (tempLeagueClassNum < minLeagueClassOpen) then
                    minLeagueClassOpen = tempLeagueClassNum;
                end
            end

            if (leagueClassLeagues[leagueInfo.className] == nil) then
                leagueClassLeagues[leagueInfo.className] = {};
            end
            table.insert(leagueClassLeagues[leagueInfo.className], leagueInfo.id);
        end
    end
end

function SetButtons()
    local tempClassName = nil;
    local tempClassItem = {};
    for i = 1, leagueClassCount, 1 do
        tempClassName = leagueClassInvert[tostring(i)];
        tempClassItem = {};
        tempClassItem.status = 1; -- 1: locked, 2: normal, 3: complete
        tempClassItem.isHover = false;

        if (leagueClassOpen[tempClassName] == false) then
            tempClassItem.status = 1;
        else
            tempClassItem.status = 2;
        end
        -- TODO: check complete
        if (i == minLeagueClassOpen) then
            tempClassItem.isHover = true;
        end

        tempClassItem.leagueClassBtnGameObject = GameObjectInstantiate(leagueClassBtnPrefab);
        tempClassItem.leagueClassBtn = tempClassItem.leagueClassBtnGameObject.transform;
        tempClassItem.leagueClassBtn.parent = uiContainer;
        tempClassItem.leagueClassBtn.localScale = Vector3.one;
        tempClassItem.leagueClassBtn.localPosition = Vector3.zero;

        tempClassItem.leagueClassBtnName                = TransformFindChild(tempClassItem.leagueClassBtn, "Name");
        tempClassItem.leagueClassBtnDesc                = TransformFindChild(tempClassItem.leagueClassBtn, "Desc");
        tempClassItem.leagueClassBtnNormal              = TransformFindChild(tempClassItem.leagueClassBtn, "ButtonNormal");
        tempClassItem.leagueClassBtnNormalTick          = TransformFindChild(tempClassItem.leagueClassBtnNormal, "Tick");
        tempClassItem.leagueClassBtnNormalName          = TransformFindChild(tempClassItem.leagueClassBtnNormal, "Name");
        tempClassItem.leagueClassBtnNormalNameHighLight = TransformFindChild(tempClassItem.leagueClassBtnNormal, "NameHighLight");
        tempClassItem.leagueClassBtnHover               = TransformFindChild(tempClassItem.leagueClassBtnNormal, "ButtonHover");
        tempClassItem.leagueClassBtnHoverName           = TransformFindChild(tempClassItem.leagueClassBtnHover, "Name");

        UIHelper.SetDragScrollViewTarget(tempClassItem.leagueClassBtn, uiScrollView);
        AddOrChangeClickParameters(tempClassItem.leagueClassBtnGameObject, OnClickLeagueClassBtn, { id=i });

        UIHelper.SetLabelTxt(tempClassItem.leagueClassBtnName, tempClassName);
        if (i == 1) then
            UIHelper.SetLabelTxt(tempClassItem.leagueClassBtnDesc, "");
        else
            UIHelper.SetLabelTxt(tempClassItem.leagueClassBtnDesc, leagueClassInvert[tostring(i-1)].."通过后开启");
        end
        UIHelper.SetLabelTxt(tempClassItem.leagueClassBtnNormalName, tempClassName);
        UIHelper.SetLabelTxt(tempClassItem.leagueClassBtnNormalNameHighLight, tempClassName);
        UIHelper.SetLabelTxt(tempClassItem.leagueClassBtnHoverName, tempClassName);

        uiLeagueClassList[i] = tempClassItem;
    end
    RefreshClassButtonStatus();
    RefreshLeagueButtons(leagueClassInvert[tostring(minLeagueClassOpen)]);
end

function OnClickLeagueClassBtn(go)
    local listener = UIHelper.GetUIEventListener(go);
    if (listener == nil or listener.parameter == nil) then
        return;
    end
    local paramId = listener.parameter.id;
    if (paramId == nil) then
        return;
    end
    if (uiLeagueClassList[paramId].status == 1 or uiLeagueClassList[paramId].isHover == true) then
        return;
    end
    for i = 1, leagueClassCount, 1 do
        uiLeagueClassList[i].isHover = false;
    end
    uiLeagueClassList[paramId].isHover = true;
    RefreshClassButtonStatus();
    local className = leagueClassInvert[tostring(paramId)];
    RefreshLeagueButtons(className);
end

function OnClickLeagueBtn(go)
    local listener = UIHelper.GetUIEventListener(go);
    if (listener == nil or listener.parameter == nil) then
        return;
    end
    local paramId = listener.parameter.id;
    if (paramId == nil) then
        return;
    end

    UIPVELeagueJoinBox.CreateJoinBox(popupRoot, windowComponent, leagueConfig[paramId]);
end

function OnClickLV2Button()
    JumpToLeagueSchedule();
end

function JumpToLeagueSchedule()
    WindowMgr.ShowWindow(LuaConst.Const.UIPVELeagueSchedule, leagueScheduleInfo);
end

function RefreshClassButtonStatus()
    local tempClassItem = nil;
    for i = 1, leagueClassCount, 1 do
        tempClassItem = uiLeagueClassList[i];
        if (tempClassItem.isHover == true) then
            SetLeagueClassBtnHover(tempClassItem);
        else
            if (tempClassItem.status == 1) then
                SetLeagueClassBtnLocked(tempClassItem);
            elseif (tempClassItem.status == 2) then
                SetLeagueClassBtnNormal(tempClassItem);
            elseif (tempClassItem.status == 3) then
                SetLeagueClassBtnComplete(tempClassItem);
            end
        end
    end
end

function RefreshLeagueButtons(leagueClassName)
    UIHelper.DestroyGrid(uiLeagueContainer);
    for v, k in pairs(leagueClassLeagues[leagueClassName]) do
        local leagueBtnGameObject = GameObjectInstantiate(leagueBtnPrefab);
        local leagueBtn = leagueBtnGameObject.transform;
        leagueBtn.parent = uiLeagueContainer;
        leagueBtn.localScale = Vector3.one;
        leagueBtn.localPosition = Vector3.zero;

        AddOrChangeClickParameters(leagueBtnGameObject, OnClickLeagueBtn, { id=k });

        local btnLabel = TransformFindChild(leagueBtn, "Label");
        UIHelper.SetLabelTxt(btnLabel, leagueConfig[k].name);
    end
    UIHelper.RepositionGrid(uiLeagueContainer, uiLeagueScrollView);
    UIHelper.RefreshPanel(uiLeagueScrollView);
end

function SetLeagueClassBtnNormal(tempClassItem)
    SetLeagueClassBtnLocked(tempClassItem);
    GameObjectSetActive(tempClassItem.leagueClassBtnNormal.gameObject, true);
end

function SetLeagueClassBtnComplete(tempClassItem)
    SetLeagueClassBtnLocked(tempClassItem);
    GameObjectSetActive(tempClassItem.leagueClassBtnNormal.gameObject, true);
    GameObjectSetActive(tempClassItem.leagueClassBtnNormalTick.gameObject, true);
    GameObjectSetActive(tempClassItem.leagueClassBtnNormalName.gameObject, false);
    GameObjectSetActive(tempClassItem.leagueClassBtnNormalNameHighLight.gameObject, true);
end

function SetLeagueClassBtnLocked(tempClassItem)
    GameObjectSetActive(tempClassItem.leagueClassBtnNormal.gameObject, false);
    GameObjectSetActive(tempClassItem.leagueClassBtnNormalTick.gameObject, false);
    GameObjectSetActive(tempClassItem.leagueClassBtnNormalName.gameObject, true);
    GameObjectSetActive(tempClassItem.leagueClassBtnNormalNameHighLight.gameObject, false);
    GameObjectSetActive(tempClassItem.leagueClassBtnHover.gameObject, false);
end

function SetLeagueClassBtnHover(tempClassItem)
    SetLeagueClassBtnLocked(tempClassItem);
    GameObjectSetActive(tempClassItem.leagueClassBtnNormal.gameObject, true);
    GameObjectSetActive(tempClassItem.leagueClassBtnHover.gameObject, true);
end

function OnDestroy()
    UIPVELeagueJoinBox.OnDestroy();

    window = nil;
    windowComponent = nil;

    leagueClassBtnPrefab = nil;
    leagueBtnPrefab = nil;
    popupRoot = nil;
    uiScrollView = nil;
    uiContainer = nil;
    uiLeagueScrollView = nil;
    uiLeagueContainer = nil;

    leagueConfig = nil;
    leagueConfigMaxIdNum = 0;
    leagueClassCount = 0;

    leagueScheduleInfo = nil;
    currentLeagueNum = nil;
    leagueOpen = {};
    leagueClass = {};
    leagueClassInvert = {};
    leagueClassOpen = {};
    minLeagueClassOpen = math.huge;
    leagueClassLeagues = {};

    uiLeagueClassList = {};
end
