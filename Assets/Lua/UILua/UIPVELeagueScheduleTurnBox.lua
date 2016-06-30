module("UIPVELeagueScheduleTurnBox", package.seeall)

require "UILua/UIIconItem"

local turnBox = nil;
local windowComponent = nil;
local containerTransform = nil

local resultInfoPrefab = nil;
local rankInfoPrefab = nil;
local turnBoxPrefab = nil;

local btnClose = nil;
local block = nil;
local resultScrollView = nil;
local resultContainer  = nil;
local rankScrollView   = nil;
local rankContainer    = nil;

local colorWhite = Color.New(1, 1, 1, 1);
local rank1Color = Color.New(217/255, 186/255, 104/255, 1);
local rank2Color = Color.New(1, 1, 1, 1);

local leagueClubConfig = PVELeagueData.GetLeagueClubConfig();

local myClubName  = Role.Get_name();
local myClubIcon  = Role.GetRoleIcon();

local upgrade = nil;

function CreateTurnBox(containerTransformP, windowComponentP, currentTurn, turnInfo, clubPointSorted, clubRank, upgradeP)
    containerTransform = containerTransformP;
    windowComponent = windowComponentP;

    upgrade = upgradeP;

    if (turnBox ~= nil) then
        if (not GameObjectActiveSelf(turnBox)) then
            GameObjectSetActive(turnBox.transform, true);
            SetResultUI(turnInfo, resultContainer);
            SetRankUI(currentTurn, clubPointSorted, rankContainer, rankScrollView, clubRank);
            return;
        end
    end

    resultInfoPrefab    = windowComponent:GetPrefab("UIPVELeagueScheduleResultInfo");
    rankInfoPrefab      = windowComponent:GetPrefab("UIPVELeagueScheduleRankInfo");
    turnBoxPrefab       = windowComponent:GetPrefab("UIPVELeagueScheduleTurnBox");
    turnBox = GameObjectInstantiate(turnBoxPrefab);
    local turnBoxTransform = turnBox.transform;
    turnBoxTransform.parent = containerTransform;
    turnBoxTransform.localPosition = Vector3.zero;
    turnBoxTransform.localScale = Vector3.one;

    btnClose         = TransformFindChild(turnBoxTransform, "CloseButton");
    block            = TransformFindChild(turnBoxTransform, "Block");
    resultScrollView = TransformFindChild(turnBoxTransform, "LeftPanel/uiScrollView");
    resultContainer  = TransformFindChild(resultScrollView, "uiContainer");
    rankScrollView   = TransformFindChild(turnBoxTransform, "RightPanel/uiScrollView");
    rankContainer    = TransformFindChild(rankScrollView, "uiContainer");

    Util.AddClick(btnClose.gameObject, OnClickBtnClose);
    Util.AddClick(block.gameObject, CloseBox);

    SetResultUI(turnInfo, resultContainer);
    SetRankUI(currentTurn, clubPointSorted, rankContainer, rankScrollView, clubRank);
end

function SetResultUI(turnInfo, resultContainer)
    UIHelper.DestroyGrid(resultContainer);

    local turnInfoLength = table.getn(turnInfo);
    local resultInfoGameObject = nil;
    local resultInfo = nil;
    local homeName = nil;
    local awayName = nil;
    local homeIcon = nil;
    local awayIcon = nil;
    local score = nil;
    local bg = nil;
    for i = 1, turnInfoLength, 1 do
        resultInfoGameObject = GameObjectInstantiate(resultInfoPrefab);
        resultInfo = resultInfoGameObject.transform;
        resultInfo.parent = resultContainer;
        resultInfo.localPosition = Vector3.zero;
        resultInfo.localScale = Vector3.one;

        homeName = TransformFindChild(resultInfo, "HomeName");
        awayName = TransformFindChild(resultInfo, "AwayName");
        homeIcon = TransformFindChild(resultInfo, "HomeIcon");
        awayIcon = TransformFindChild(resultInfo, "AwayIcon");
        score    = TransformFindChild(resultInfo, "Score");
        bg       = TransformFindChild(resultInfo, "BG");

        UIHelper.SetLabelTxt(homeName, turnInfo[i].name[1]);
        UIHelper.SetLabelTxt(awayName, turnInfo[i].name[2]);
        Util.SetUITexture(homeIcon, LuaConst.Const.ClubIcon, turnInfo[i].icon[1].."_2", true);
        Util.SetUITexture(awayIcon, LuaConst.Const.ClubIcon, turnInfo[i].icon[2].."_2", true);
        homeIcon.localScale = Vector3.New(0.28, 0.28, 0.28);
        awayIcon.localScale = Vector3.New(0.28, 0.28, 0.28);
        UIHelper.SetLabelTxt(score, tostring(turnInfo[i].s[1]).."-"..tostring(turnInfo[i].s[2]));

        if (i % 2 == 0) then
            GameObjectSetActive(bg, false);
        end

        if (turnInfo[i].whoAmI == 0) then
            UIHelper.SetWidgetColor(homeName, colorWhite);
            UIHelper.SetWidgetColor(awayName, colorWhite);
        elseif (turnInfo[i].whoAmI == 1) then
            UIHelper.SetWidgetColor(awayName, colorWhite);
        elseif (turnInfo[i].whoAmI == 2) then
            UIHelper.SetWidgetColor(homeName, colorWhite);
        end
    end

    UIHelper.RepositionGrid(resultContainer);
    UIHelper.RefreshPanel(resultScrollView);
end

function SetRankUI(currentTurn, clubPointSorted, rankContainer, rankScrollView, clubRank)
    UIHelper.DestroyGrid(rankContainer);

    local rankInfoGameObject = nil;
    local rankInfo = nil;
    local tempClubIcon = nil;
    local tempRank = nil;
    local tempName = nil;
    local tempPoint = nil;
    local arrowLevel = nil;
    local arrowUp = nil;
    local arrowDown = nil;
    local rankTurn = currentTurn - 1;
    if (rankTurn == 0) then
        rankTurn = 1;
    end
    local previousTurn = rankTurn - 1;
    for i, v in pairs(clubPointSorted[rankTurn]) do
        rankInfoGameObject = GameObjectInstantiate(rankInfoPrefab);
        rankInfo = rankInfoGameObject.transform;
        rankInfo.parent = rankContainer;
        rankInfo.localPosition = Vector3.zero;
        rankInfo.localScale = Vector3.one;
        tempClubIcon = TransformFindChild(rankInfo, "ClubIcon");
        tempRank     = TransformFindChild(rankInfo, "Rank");
        tempName     = TransformFindChild(rankInfo, "Name");
        tempPoint    = TransformFindChild(rankInfo, "Point");
        arrowLevel   = TransformFindChild(rankInfo, "ArrowLevel");
        arrowUp      = TransformFindChild(rankInfo, "ArrowUp");
        arrowDown    = TransformFindChild(rankInfo, "ArrowDown");

        UIHelper.SetLabelTxt(tempRank, i);
        if (v.id == "0") then
            SetClubIcon(tempClubIcon, myClubIcon);
            UIHelper.SetLabelTxt(tempName, myClubName);
        else
            SetClubIcon(tempClubIcon, leagueClubConfig[v.id].icon);
            UIHelper.SetLabelTxt(tempName, leagueClubConfig[v.id].name);
        end
        UIHelper.SetLabelTxt(tempPoint, v.point);

        if (i == 1) then
            UIHelper.SetLabelFontSize(tempRank, 22);
            UIHelper.SetLabelFontSize(tempName, 22);
            UIHelper.SetLabelFontSize(tempPoint, 22);
            UIHelper.SetWidgetColor(tempRank, rank1Color);
            UIHelper.SetWidgetColor(tempName, rank1Color);
            UIHelper.SetWidgetColor(tempPoint, rank1Color);
        elseif (i <= 4) then
            UIHelper.SetWidgetColor(tempRank, rank2Color);
            UIHelper.SetWidgetColor(tempName, rank2Color);
            UIHelper.SetWidgetColor(tempPoint, rank2Color);
        end

        -- 上升下降图标
        if (previousTurn ~= 0) then
            if (i < clubRank[previousTurn][v.id]) then
                GameObjectSetActive(arrowUp, true);
                GameObjectSetActive(arrowLevel, false);
            elseif (i > clubRank[previousTurn][v.id]) then
                GameObjectSetActive(arrowDown, true);
                GameObjectSetActive(arrowLevel, false);
            end
        end
    end
    UIHelper.RepositionGrid(rankContainer);
    UIHelper.RefreshPanel(rankScrollView);
end

function SetClubIcon(uiClubIcon, iconNum)
    GameObjectSetActive(uiClubIcon.gameObject, true);
    Util.SetUITexture(uiClubIcon, LuaConst.Const.ClubIcon, tostring(iconNum).."_2", true);
    uiClubIcon.localScale = Vector3.New(0.18, 0.18, 0.18);
end

function OnClickBtnClose()
    CloseBox();
end

function CloseBox()
    if (GameObjectActiveSelf(turnBox)) then
        GameObjectSetActive(turnBox.transform, false);
    end
    if (upgrade ~= nil) then
        print('season end');
        UIPVELeagueSchedule.ShowEndingBox(upgrade);
    end
end

function OnDestroy()
    turnBox = nil;
    windowComponent = nil;

    resultInfoPrefab = nil;
    rankInfoPrefab = nil;
    turnBoxPrefab = nil;

    btnClose = nil;
    block = nil;
    resultScrollView = nil;
    resultContainer  = nil;
    rankScrollView   = nil;
    rankContainer    = nil;
end
