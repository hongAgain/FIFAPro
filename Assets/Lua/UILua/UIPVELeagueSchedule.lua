module("UIPVELeagueSchedule", package.seeall)

require "Game/Role"
require "Game/CheckUpgradeMgr"
require "UILua/UIPVELeague"
require "UILua/UIPrepareMatch"
require "UILua/UIBattleResultS"
require "UILua/UIPVELeagueScheduleLeaveBox"
require "UILua/UIPVELeagueScheduleRewardBox"
require "UILua/UIPVELeagueScheduleEndingBox"
require "UILua/UIPVELeagueScheduleTurnBox"
require "UILua/UIIconItem"

local window = nil;
local windowComponent = nil;

local dateNodePrefab = nil;
local rankInfoPrefab = nil;

-- Set by StoreLeagueScheduleInfo()
local leagueScheduleInfo = nil;    -- 服务端传回数据
local currentSeasonNum = nil;
local currentSeasonScheduleInfo = nil;

-- Set by SetConfig()
local leagueConfig = nil;    -- 配置数据
local currentSeasonConfig = nil;
local currentSeasonStartMonth = nil;
local whatDayIsItTheFirstDayOfTheSeason = nil;
local leagueClubConfig = nil;
local leagueTeamConfig = nil;
local leagueRewardConfig = nil;
local currentSeasonRewardConfig = nil;

-- Set by SetScheduleInfo
local seasonTurnsDaySchedule = {};    -- 每轮的比赛时间；key为轮数，value为比赛时间（距赛季第一天的天数）
local seasonTurnsDayScheduleInvert = {};
local seasonMaxTurn = 0;
local seasonMaxMonth = nil;
local seasonTurnsOpponent = {}; -- 每轮的对手；key为轮数，value为对手
local seasonTurnsScore = {};    -- 自己比赛的每轮比分
local seasonTurnsPosition = {}; -- 每轮的主客场情况；key为轮数，value为主客场：1主场，2客场
local clubPoint = {};           -- 每轮的积分
local clubPointSorted = {};     -- 排序后的积分，格式为 {{{ id=key, point=value }, { id=key, point=value }...} } 按降序排列
local clubRank = {};            -- 每轮的排名 {{ "1"=6, "2"=4... }...}, value 是排名
local clubGoalDiff = {};        -- 每轮的净胜球
local clubWinNum = {};          -- 每轮的胜场数
local clubDrawNum = {};         -- 每轮的平场数
local clubLoseNum = {};         -- 每轮的负场数
local clubGoal = {};            -- 每轮的进球数
local clubLoseGoal = {};        -- 每轮的失球数

local currentTurn = nil;
local currentMonth = nil;    -- 从开始月递增，数值可能超过12，用GetRealMonthNum()得到1~12的月份

local rank1Color = Color.New(217/255, 186/255, 104/255, 1);
local rank2Color = Color.New(1, 1, 1, 1);

local popupRoot = nil;
local dateScrollView = nil;
local dateContainer = nil;
local monthLabel = nil;
local rankScrollView = nil;
local rankContainer = nil;
local descLabel = nil;

local teamLeftNameLabel = nil;
local teamLeftLVLabel = nil;
local teamLeftIcon = nil;
local teamLeftPowerLabel = nil;

local teamRightNameLabel = nil;
local teamRightLVLabel = nil;
local teamRightIcon = nil;
local teamRightPowerLabel = nil;

local rewardBox = nil;

local monthDateNum = {
    [1] = 31,
    [2] = 28,
    [3] = 31,
    [4] = 30,
    [5] = 31,
    [6] = 30,
    [7] = 31,
    [8] = 31,
    [9] = 30,
    [10] = 31,
    [11] = 30,
    [12] = 31
};

-- Set by SetMyInfo
local myClubName = nil;
local myClubIcon = nil;
local myClubLv = nil;
local myClubPower = nil;

function OnStart(gameObject, params)
    print("UIPVELeagueSchedule.lua .. OnStart");
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");
    dateNodePrefab = windowComponent:GetPrefab("UIPVELeagueScheduleDateNode");
    rankInfoPrefab = windowComponent:GetPrefab("UIPVELeagueScheduleRankInfo");

	UIIconItem.Init(windowComponent);

    StoreLeagueScheduleInfo(params);

    BindUI();
    SetInfo();
end

function StoreLeagueScheduleInfo(info)
    leagueScheduleInfo = info;
    currentSeasonNum = leagueScheduleInfo.s;
    currentSeasonScheduleInfo = leagueScheduleInfo[currentSeasonNum];
end

function BindUI()
    local transform = window.transform;
    popupRoot      = TransformFindChild(transform, "PopupRoot");
    dateScrollView = TransformFindChild(transform, "LeftPanel/DateBlocks/uiScrollView");
    dateContainer  = TransformFindChild(dateScrollView, "uiContainer");
    monthLabel     = TransformFindChild(transform, "LeftPanel/MonthNode/Label");
    rankScrollView = TransformFindChild(transform, "RightPanel/uiScrollView");
    rankContainer  = TransformFindChild(rankScrollView, "uiContainer");
    descLabel      = TransformFindChild(transform, "RightPanel/Desc");

    teamLeftNameLabel   = TransformFindChild(transform, "LeftPanel/VSNode/TeamLeftName");
    teamLeftLVLabel     = TransformFindChild(transform, "LeftPanel/VSNode/TeamLeftLV");
    teamLeftIcon        = TransformFindChild(transform, "LeftPanel/VSNode/TeamLeftIcon");
    teamLeftPowerLabel  = TransformFindChild(transform, "LeftPanel/PowerNode/PowerLeft");
    teamRightNameLabel  = TransformFindChild(transform, "LeftPanel/VSNode/TeamRightName");
    teamRightLVLabel    = TransformFindChild(transform, "LeftPanel/VSNode/TeamRightLV");
    teamRightIcon       = TransformFindChild(transform, "LeftPanel/VSNode/TeamRightIcon");
    teamRightPowerLabel = TransformFindChild(transform, "LeftPanel/PowerNode/PowerRight");

    local leaveBtn         = TransformFindChild(transform, "LeftPanel/BottomGradient/LeaveButton");
    local rewardBtn        = TransformFindChild(transform, "LeftPanel/BottomGradient/RewardButton");
    local startBtn         = TransformFindChild(transform, "LeftPanel/BottomGradient/StartButton");
    local nextMonthBtn     = TransformFindChild(transform, "LeftPanel/MonthNode/RightArrow");
    local previousMonthBtn = TransformFindChild(transform, "LeftPanel/MonthNode/LeftArrow");
    Util.AddClick(leaveBtn.gameObject, OnClickBtnLeave);
    Util.AddClick(rewardBtn.gameObject, OnClickBtnReward);
    Util.AddClick(startBtn.gameObject, OnClickBtnStart);
    Util.AddClick(nextMonthBtn.gameObject, OnClickBtnNextMonth);
    Util.AddClick(previousMonthBtn.gameObject, OnClickBtnPreviousMonth);
end

function OnClickBtnLeave()
    UIPVELeagueScheduleLeaveBox.CreateLeaveBox(popupRoot, windowComponent);
end

function OnClickBtnReward()
    UIPVELeagueScheduleRewardBox.CreateRewardBox(popupRoot, windowComponent, currentSeasonRewardConfig);
end

function OnClickBtnStart()
    CheckUpgradeMgr.SetBackup();
    PVELeagueData.ClickChallengeTips();
end

function OnClickBtnNextMonth()
    local nextMonth = currentMonth + 1;
    if (nextMonth > seasonMaxMonth) then
        return;
    end

    currentMonth = nextMonth;
    SetDateBlocksUI();
end

function OnClickBtnPreviousMonth()
    local previousMonth = currentMonth - 1;
    if (previousMonth < currentSeasonStartMonth) then
        return;
    end

    currentMonth = previousMonth;
    SetDateBlocksUI();
end

function SetInfo()
    SetConfig();
    SetMyInfo();
    SetScheduleInfo();
    SetRankUI();
    SetVersusUI();
    SetDateBlocksUI();
end

function SetConfig()
    leagueConfig     = PVELeagueData.GetLeagueConfig();
    currentSeasonConfig = leagueConfig[currentSeasonNum];
    currentSeasonStartMonth = currentSeasonConfig["month_start"];
    whatDayIsItTheFirstDayOfTheSeason = currentSeasonConfig["id"] % 7;
    leagueClubConfig = PVELeagueData.GetLeagueClubConfig();
    leagueTeamConfig = PVELeagueData.GetLeagueTeamConfig();
    leagueRewardConfig = PVELeagueData.GetLeagueRewardConfig();
    currentSeasonRewardConfig = {};
    -- currentSeasonRewardConfig
    for i, v in pairs(leagueRewardConfig) do
        if (v.id == currentSeasonNum) then
            currentSeasonRewardConfig[i] = v;
        end
    end
end

function SetMyInfo()
    myClubName  = Role.Get_name();
    myClubIcon  = Role.GetRoleIcon();
    myClubLv    = Role.Get_lv();
    myClubPower = HeroData.GetTeamBattleScore();
end

function SetScheduleInfo()
    seasonTurnsDaySchedule = {};
    seasonTurnsDayScheduleInvert = {};
    seasonMaxTurn = 0;
    seasonMaxMonth = nil;
    seasonTurnsOpponent = {};
    seasonTurnsScore = {};
    seasonTurnsPosition = {};
    clubPoint = {};
    clubPointSorted = {};
    clubRank = {};
    clubGoalDiff = {};
    clubWinNum = {};
    clubDrawNum = {};
    clubLoseNum = {};
    clubGoal = {};
    clubLoseGoal = {};
    currentTurn = nil;
    currentMonth = nil;

    local matchList = currentSeasonScheduleInfo.l;
    local matchListLength = table.getn(matchList);
    local tempT = nil;
    local tempDaysFromSeasonBegin = nil;
    local tempClub = nil;
    for i = 1, matchListLength, 1 do
        tempT = matchList[i].t;
        -- seasonMaxTurn
        if (tempT > seasonMaxTurn) then
            seasonMaxTurn = tempT;
        end
        -- 初始化 seasonTurnsDaySchedule 和 seasonTurnsDayScheduleInvert
        if (seasonTurnsDaySchedule[tempT] == nil) then
            tempDaysFromSeasonBegin = GetMatchDaysFromFirstDayOfSeason(tempT);
            seasonTurnsDaySchedule[tempT] = tempDaysFromSeasonBegin;
            seasonTurnsDayScheduleInvert[tempDaysFromSeasonBegin] = tempT;
        end
        -- 初始化 seasonTurnsOpponent 和 seasonTurnsPosition
        if (matchList[i].f == 1) then
            -- seasonTurnsScore
            seasonTurnsScore[tempT] = matchList[i].s;
            for j = 1, 2, 1 do
                tempClub = matchList[i].u[j];
                if (tempClub ~= "0") then
                    seasonTurnsOpponent[tempT] = tempClub;
                    seasonTurnsPosition[tempT] = 3 - j;    -- j 是对手主客场，3-j 是自己主客场
                end
            end
        end
        -- 初始化 clubPoint, clubGoalDiff, clubWinNum, clubDrawNum, clubLoseNum, clubGoal, clubLoseGoal
        for j = 1, 2, 1 do
            tempClub = matchList[i].u[j];
            if (clubPoint[tempT] == nil) then
                clubPoint[tempT] = {};
                clubGoalDiff[tempT] = {};
                clubWinNum[tempT] = {};
                clubDrawNum[tempT] = {};
                clubLoseNum[tempT] = {};
                clubGoal[tempT] = {};
                clubLoseGoal[tempT] = {};
            end
            if (clubPoint[tempT][tempClub] == nil) then
                clubPoint[tempT][tempClub] = 0;
                clubGoalDiff[tempT][tempClub] = 0;
                clubWinNum[tempT][tempClub] = 0;
                clubDrawNum[tempT][tempClub] = 0;
                clubLoseNum[tempT][tempClub] = 0;
                clubGoal[tempT][tempClub] = 0;
                clubLoseGoal[tempT][tempClub] = 0;
            end
        end
    end

    local tempMatch = nil;
    local tempGoalDiff = nil;
    for i = 1, matchListLength, 1 do
        tempMatch = matchList[i];
        if (tempMatch.o == 0) then
            break;
        end
        tempT = tempMatch.t;
        tempGoalDiff = tempMatch.s[1] - tempMatch.s[2];
        -- 计算 clubPoint, clubWinNum, clubDrawNum, clubLoseNum
        if (tempGoalDiff > 0) then
            for j = tempT, seasonMaxTurn, 1 do
                clubPoint[j][tempMatch.u[1]]   = clubPoint[j][tempMatch.u[1]] + 3;
                clubWinNum[j][tempMatch.u[1]]  = clubWinNum[j][tempMatch.u[1]] + 1;
                clubLoseNum[j][tempMatch.u[2]] = clubLoseNum[j][tempMatch.u[2]] + 1;
            end
        elseif (tempGoalDiff < 0) then
            for j = tempT, seasonMaxTurn, 1 do
                clubPoint[j][tempMatch.u[2]]   = clubPoint[j][tempMatch.u[2]] + 3;
                clubLoseNum[j][tempMatch.u[1]] = clubLoseNum[j][tempMatch.u[1]] + 1;
                clubWinNum[j][tempMatch.u[2]]  = clubWinNum[j][tempMatch.u[2]] + 1;
            end
        else
            for j = tempT, seasonMaxTurn, 1 do
                clubPoint[j][tempMatch.u[1]]    = clubPoint[j][tempMatch.u[1]] + 1;
                clubPoint[j][tempMatch.u[2]]    = clubPoint[j][tempMatch.u[2]] + 1;
                clubDrawNum[j][tempMatch.u[1]]  = clubDrawNum[j][tempMatch.u[1]] + 1;
                clubDrawNum[j][tempMatch.u[2]]  = clubDrawNum[j][tempMatch.u[2]] + 1;
            end
        end
        -- 计算 clubGoal, clubLoseGoal, clubGoalDiff
        for j = tempT, seasonMaxTurn, 1 do
            clubGoal[j][tempMatch.u[1]] = clubGoal[j][tempMatch.u[1]] + tempMatch.s[1];
            clubGoal[j][tempMatch.u[2]] = clubGoal[j][tempMatch.u[2]] + tempMatch.s[2];
            clubLoseGoal[j][tempMatch.u[1]] = clubLoseGoal[j][tempMatch.u[1]] + tempMatch.s[2];
            clubLoseGoal[j][tempMatch.u[2]] = clubLoseGoal[j][tempMatch.u[2]] + tempMatch.s[1];
        end
        if (tempGoalDiff ~= 0) then
            for j = tempT, seasonMaxTurn, 1 do
                clubGoalDiff[j][tempMatch.u[1]] = clubGoalDiff[j][tempMatch.u[1]] + tempGoalDiff;
                clubGoalDiff[j][tempMatch.u[2]] = clubGoalDiff[j][tempMatch.u[2]] - tempGoalDiff;
            end
        end
    end

    -- 计算 clubPointSorted
    for turn, turnPoints in pairs(clubPoint) do
        clubPointSorted[turn] = {};
        for club, point in pairs(turnPoints) do
            table.insert(clubPointSorted[turn], { id = club, point = point, goalDiff = clubGoalDiff[turn][club] });
        end
    end
    local function sortFunc(a, b)
        local pointDiff = a.point - b.point;
        if (pointDiff ~= 0) then
            return a.point > b.point;
        end
        -- 积分相等
        local goalDiff = a.goalDiff - b.goalDiff;
        if (goalDiff ~= 0) then
            return a.goalDiff > b.goalDiff;
        end
        -- 积分净胜球都相等
        local aName = nil;
        local bName = nil;
        if (a.id == "0") then
            aName = myClubName;
        else
            aName = leagueClubConfig[a.id].name;
        end
        if (b.id == "0") then
            bName = myClubName;
        else
            bName = leagueClubConfig[b.id].name;
        end
        if (aName < bName) then
            return true;
        end
        return false;
    end
    -- 计算 clubRank
    for turn, turnPoints in pairs(clubPointSorted) do
        table.sort(turnPoints, sortFunc);
        clubRank[turn] = {};
        for rank, info in pairs(turnPoints) do
            clubRank[turn][info.id] = rank;
        end
    end

    -- seasonMaxMonth
    seasonMaxMonth = GetTurnMonth(seasonMaxTurn);
    -- currentTurn
    currentTurn = currentSeasonScheduleInfo.l[currentSeasonScheduleInfo.s + 1].t;
    if (currentTurn > seasonMaxTurn) then
        currentTurn = seasonMaxTurn;
    end
    -- currentMonth
    currentMonth = GetTurnMonth(currentTurn);
end

function SetRankUI()
    UIHelper.SetLabelTxt(descLabel, "第"..currentTurn.."轮（总"..seasonMaxTurn.."轮前两名可晋级）");

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

function SetVersusUI()
    local turn = currentTurn;
    local currentOpponent = seasonTurnsOpponent[turn];
    local currentOpponentClub = leagueClubConfig[currentOpponent];
    local currentOpponentTeam = leagueTeamConfig[tostring(currentOpponentClub.team)];
    if (seasonTurnsPosition[turn] == 1) then
        UIHelper.SetLabelTxt(teamLeftNameLabel, myClubName);
        UIHelper.SetLabelTxt(teamLeftLVLabel, "Lv."..myClubLv);
        UIHelper.SetLabelTxt(teamLeftPowerLabel, myClubPower);
        Util.SetUITexture(teamLeftIcon, LuaConst.Const.ClubIcon, tostring(myClubIcon).."_2", true);
        teamLeftIcon.localScale = Vector3.New(0.316, 0.316, 0.316);

        UIHelper.SetLabelTxt(teamRightNameLabel, currentOpponentClub.name);
        UIHelper.SetLabelTxt(teamRightLVLabel, "Lv."..currentOpponentTeam.lv);
        UIHelper.SetLabelTxt(teamRightPowerLabel, currentOpponentTeam.score);
        Util.SetUITexture(teamRightIcon, LuaConst.Const.ClubIcon, tostring(currentOpponentClub.icon).."_2", true);
        teamRightIcon.localScale = Vector3.New(0.316, 0.316, 0.316);
    end
    if (seasonTurnsPosition[turn] == 2) then
        UIHelper.SetLabelTxt(teamRightNameLabel, myClubName);
        UIHelper.SetLabelTxt(teamRightLVLabel, "Lv."..myClubLv);
        UIHelper.SetLabelTxt(teamRightPowerLabel, myClubPower);
        Util.SetUITexture(teamRightIcon, LuaConst.Const.ClubIcon, tostring(myClubIcon).."_2", true);
        teamRightIcon.localScale = Vector3.New(0.316, 0.316, 0.316);

        UIHelper.SetLabelTxt(teamLeftNameLabel, currentOpponentClub.name);
        UIHelper.SetLabelTxt(teamLeftLVLabel, "Lv."..currentOpponentTeam.lv);
        UIHelper.SetLabelTxt(teamLeftPowerLabel, currentOpponentTeam.score);
        Util.SetUITexture(teamLeftIcon, LuaConst.Const.ClubIcon, tostring(currentOpponentClub.icon).."_2", true);
        teamLeftIcon.localScale = Vector3.New(0.316, 0.316, 0.316);
    end
end

function SetDateBlocksUI()
    SetMonthLabel(currentMonth);

	UIHelper.DestroyGrid(dateContainer);

    local dateNodeGameObject = nil;
    local dateNode = nil;
    local dateBgBtn = nil;
    local dateBlockNumBeforeMonth = GetDateBlockNumBeforeMonth(GetWhatDayIsItTheSpecifiedDay(currentMonth, 1));
    local dateBlockNumOfMonth     = GetDateBlockNumOfMonth(currentMonth);
    local dateBlockNumAfterMonth  = GetDateBlockNumAfterMonth(GetWhatDayIsItTheSpecifiedDay(currentMonth, monthDateNum[GetRealMonthNum(currentMonth)]));
    local tempDays = nil;
    local tempTurn = nil;
    -- 月前 date blocks
    for i = 1, dateBlockNumBeforeMonth, 1 do
        dateNodeGameObject = GameObjectInstantiate(dateNodePrefab);
        dateNode = dateNodeGameObject.transform;
        dateNode.parent = dateContainer;
        dateNode.localPosition = Vector3.zero;
        dateNode.localScale = Vector3.one;
        SetDateBtnOutOfMonth(dateNode);

        dateBgBtn = TransformFindChild(dateNode, "DateBG");
        UIHelper.SetDragScrollViewTarget(dateBgBtn, dateScrollView);
    end
    -- 当月 date blocks
    for i = 1, dateBlockNumOfMonth, 1 do
        dateNodeGameObject = GameObjectInstantiate(dateNodePrefab);
        dateNode = dateNodeGameObject.transform;
        dateNode.parent = dateContainer;
        dateNode.localPosition = Vector3.zero;
        dateNode.localScale = Vector3.one;

        tempDays = GetDaysFromFirstDayOfSeason(currentMonth, i);
        if (tempDays < seasonTurnsDaySchedule[currentTurn]) then
            SetDateBtnPast(dateNode);
        else
            SetDateBtnFuture(dateNode);
        end
        if (tempDays == seasonTurnsDaySchedule[currentTurn]) then
            SetDateBtnCurrent(dateNode);
        end

        tempTurn = seasonTurnsDayScheduleInvert[tempDays];
        if (tempTurn ~= nil) then
            if (tempTurn >= currentTurn) then
                SetClubIcon(
                    TransformFindChild(dateNode, "ClubIcon"),
                    leagueClubConfig[seasonTurnsOpponent[tempTurn]].icon
                );
            else
                UIHelper.SetLabelTxt(
                    TransformFindChild(dateNode, "Score"),
                    tostring(seasonTurnsScore[tempTurn][1])..'-'..tostring(seasonTurnsScore[tempTurn][2])
                );
                local result = seasonTurnsScore[tempTurn][1] - seasonTurnsScore[tempTurn][2];
                if (seasonTurnsPosition[tempTurn] == 2) then
                    result = -result;
                end
                if (result > 0) then
                    UIHelper.SetLabelTxt(TransformFindChild(dateNode, "WinOrLose"), '胜');
                    GameObjectSetActive(TransformFindChild(dateNode, "ResultWin").gameObject, true);
                elseif(result < 0 ) then
                    UIHelper.SetLabelTxt(TransformFindChild(dateNode, "WinOrLose"), '负');
                    GameObjectSetActive(TransformFindChild(dateNode, "ResultLose").gameObject, true);
                else
                    UIHelper.SetLabelTxt(TransformFindChild(dateNode, "WinOrLose"), '平');
                    GameObjectSetActive(TransformFindChild(dateNode, "ResultLevel").gameObject, true);
                end
                GameObjectSetActive(TransformFindChild(dateNode, "Score").gameObject, true);
                GameObjectSetActive(TransformFindChild(dateNode, "WinOrLose").gameObject, true);
            end
        end
        UIHelper.SetLabelTxt(TransformFindChild(dateNode, "Date"), i);

        dateBgBtn = TransformFindChild(dateNode, "DateBG");
        UIHelper.SetDragScrollViewTarget(dateBgBtn, dateScrollView);
    end
    -- 月后 date blocks
    for i = 1, dateBlockNumAfterMonth, 1 do
        dateNodeGameObject = GameObjectInstantiate(dateNodePrefab);
        dateNode = dateNodeGameObject.transform;
        dateNode.parent = dateContainer;
        dateNode.localPosition = Vector3.zero;
        dateNode.localScale = Vector3.one;
        SetDateBtnOutOfMonth(dateNode);

        dateBgBtn = TransformFindChild(dateNode, "DateBG");
        UIHelper.SetDragScrollViewTarget(dateBgBtn, dateScrollView);
    end

    UIHelper.RepositionGrid(dateContainer);
    UIHelper.RefreshPanel(dateScrollView);
end

function SetMonthLabel(monthNum)
    UIHelper.SetLabelTxt(monthLabel, tostring(GetRealMonthNum(monthNum)).." 月");
end

function SetDateBtnWin(dateNode)
    local winBg          = TransformFindChild(dateNode, "ResultWin");
    local scoreBg        = TransformFindChild(dateNode, "Score");
    local winOrLoseLabel = TransformFindChild(dateNode, "WinOrLose");
    GameObjectSetActive(winBg.gameObject, true);
    GameObjectSetActive(scoreBg.gameObject, true);
    GameObjectSetActive(winOrLoseLabel.gameObject, true);
    UIHelper.SetLabelTxt(winOrLoseLabel, "胜");
end

function SetDateBtnLose(dateNode)
    local loseBg         = TransformFindChild(dateNode, "ResultLose");
    local scoreBg        = TransformFindChild(dateNode, "Score");
    local winOrLoseLabel = TransformFindChild(dateNode, "WinOrLose");
    GameObjectSetActive(loseBg.gameObject, true);
    GameObjectSetActive(scoreBg.gameObject, true);
    GameObjectSetActive(winOrLoseLabel.gameObject, true);
    UIHelper.SetLabelTxt(winOrLoseLabel, "负");
end

function SetDateBtnDraw(dateNode)
    local drawBg         = TransformFindChild(dateNode, "ResultLevel");
    local scoreBg        = TransformFindChild(dateNode, "Score");
    local winOrLoseLabel = TransformFindChild(dateNode, "WinOrLose");
    GameObjectSetActive(drawBg.gameObject, true);
    GameObjectSetActive(scoreBg.gameObject, true);
    GameObjectSetActive(winOrLoseLabel.gameObject, true);
    UIHelper.SetLabelTxt(winOrLoseLabel, "平");
end

function SetDateBtnCurrent(dateNode)
    local lightBg   = TransformFindChild(dateNode, "ResultLight");
    local dateLabel = TransformFindChild(dateNode, "Date");
    GameObjectSetActive(lightBg.gameObject, true);
    UIHelper.SetWidgetColor(dateLabel, Color.New(102/255, 204/255, 255/255, 255/255));
end

function SetDateBtnPast()
    -- Nothing
end

function SetDateBtnFuture(dateNode)
    local futureBg = TransformFindChild(dateNode, "ResultUp");
    GameObjectSetActive(futureBg.gameObject, true);
end

function SetDateBtnOutOfMonth(dateNode)
    local dateLabel = TransformFindChild(dateNode, "Date");
    GameObjectSetActive(dateLabel.gameObject, false);
end

function SetClubIcon(uiClubIcon, iconNum)
    GameObjectSetActive(uiClubIcon.gameObject, true);
    Util.SetUITexture(uiClubIcon, LuaConst.Const.ClubIcon, tostring(iconNum).."_2", true);
    uiClubIcon.localScale = Vector3.New(0.18, 0.18, 0.18);
end

function ShowTurnBox(upgrade)
    print("ShowTurnBox..");
    local resultTurn = currentTurn - 1;
    local turnInfo = {};
    local myMatchIndex = 0;

    local matchList = currentSeasonScheduleInfo.l;
    local matchListLength = table.getn(matchList);
    local tempTurnInfo = nil;
    for i = 1, matchListLength, 1 do
        if (matchList[i].t == resultTurn) then
            tempTurnInfo = {};
            tempTurnInfo.u = matchList[i].u;
            tempTurnInfo.s = matchList[i].s;
            tempTurnInfo.name = { "", "" };
            tempTurnInfo.icon = { "", "" };
            tempTurnInfo.whoAmI = 0;
            for j = 1, 2, 1 do
                if (tempTurnInfo.u[j] == "0") then
                    tempTurnInfo.name[j] = myClubName;
                    tempTurnInfo.icon[j] = myClubIcon;
                    tempTurnInfo.whoAmI = j;
                else
                    tempTurnInfo.name[j] = leagueClubConfig[tempTurnInfo.u[j]].name;
                    tempTurnInfo.icon[j] = leagueClubConfig[tempTurnInfo.u[j]].icon;
                end
            end
            table.insert(
                turnInfo,
                tempTurnInfo
            );
            if (matchList[i].f == 1) then
                myMatchIndex = table.getn(turnInfo);
            end
        end
    end

    local turnInfoLength = table.getn(turnInfo);
    local myTurnInfo = nil;
    for i = turnInfoLength, 1, -1 do
        if (i == myMatchIndex) then
            myTurnInfo = turnInfo[i];
        end
        if (i < myMatchIndex) then
            turnInfo[i + 1] = turnInfo[i];
        end
    end
    turnInfo[1] = myTurnInfo;
    UIPVELeagueScheduleTurnBox.CreateTurnBox(popupRoot, windowComponent, currentTurn, turnInfo, clubPointSorted, clubRank, upgrade);
end

function ShowEndingBox()
    local endingInfo = {};
    endingInfo.rank    = clubRank[currentTurn]["0"];
    endingInfo.winNum  = clubWinNum[currentTurn]["0"];
    endingInfo.drawNum = clubDrawNum[currentTurn]["0"];
    endingInfo.loseNum = clubLoseNum[currentTurn]["0"];
    endingInfo.point = clubPoint[currentTurn]["0"];
    endingInfo.unlock = nil;
    endingInfo.rewards = nil;
    for i, v in pairs(currentSeasonRewardConfig) do
        if (v["end"] >= endingInfo.rank) then
            endingInfo.rewards = v.settlement_reward;
            break
        end
    end
    UIPVELeagueScheduleEndingBox.CreateEndingBox(popupRoot, windowComponent, endingInfo);
end

-------------
-- 工具函数 --
-------------
function GetRealMonthNum(monthNum)
    return (monthNum + 11) % 12 + 1;
end

function GetDateBlockNumBeforeMonth(whatDayIsItTheFirstDayOfTheMonth)
    return whatDayIsItTheFirstDayOfTheMonth;
end
function GetDateBlockNumOfMonth(month)
    return monthDateNum[GetRealMonthNum(month)];
end
function GetDateBlockNumAfterMonth(whatDayIsItTheLastDayOfTheMonth)
    return 6 - whatDayIsItTheLastDayOfTheMonth;
end

function GetDaysFromFirstDayOfSeason(month, day)
    local days = 0;
    for i = currentSeasonStartMonth, month - 1, 1 do
        days = days + monthDateNum[GetRealMonthNum(i)];
    end
    days = days + day - 1;
    return days;
end

function GetWhatDayIsItTheSpecifiedDay(month, day)
    local days = GetDaysFromFirstDayOfSeason(month, day);
    return (whatDayIsItTheFirstDayOfTheSeason + days) % 7;
end

function GetMatchDaysFromFirstDayOfSeason(turn)
    local days = 0;
    days = days + 7 * (turn - 1);
    return days;
end

function GetTurnMonth(turn)
    local month = nil;
    local turnDaysFromFirstDay = seasonTurnsDaySchedule[turn];
    local tempMonth = nil;
    local tempDays = 0;
    for i = 0, 11, 1 do    -- 这里假定赛季不会超过一年，写while不安全
        tempMonth = currentSeasonStartMonth + i;
        tempDays = tempDays + monthDateNum[GetRealMonthNum(tempMonth)];
        if (tempDays > turnDaysFromFirstDay) then
            month = tempMonth;
            break;
        end
    end

    return month;
end

function RefreshUIFromResult(battleResultData)
    print('RefreshUIFromResult');
    currentSeasonScheduleInfo = battleResultData.list;
    SetMyInfo();
    SetScheduleInfo();
    SetRankUI();
    SetVersusUI();
    SetDateBlocksUI();
end

function OnDestroy()
    UIPVELeagueScheduleLeaveBox.OnDestroy();
    UIPVELeagueScheduleRewardBox.OnDestroy();
    UIPVELeagueScheduleEndingBox.OnDestroy();

    window = nil;
    windowComponent = nil;

    dateNodePrefab = nil;
    rankInfoPrefab = nil;

    leagueScheduleInfo = nil;    -- 服务端传回数据
    currentSeasonNum = nil;
    currentSeasonScheduleInfo = nil;

    leagueConfig = nil;    -- 配置数据
    currentSeasonConfig = nil;
    currentSeasonStartMonth = nil;
    whatDayIsItTheFirstDayOfTheSeason = nil;
    leagueClubConfig = nil;
    leagueTeamConfig = nil;
    leagueRewardConfig = nil;
    currentSeasonRewardConfig = nil;

    seasonTurnsDaySchedule = {};    -- 每轮的比赛时间；key为轮数，value为比赛时间（距赛季第一天的天数）
    seasonTurnsDayScheduleInvert = {};
    seasonMaxTurn = 0;
    seasonMaxMonth = nil;
    seasonTurnsOpponent = {};    -- 每轮的对手；key为轮数，value为对手
    seasonTurnsScore = {};       -- 自己比赛的每轮比分
    seasonTurnsPosition = {};    -- 每轮的主客场情况；key为轮数，value为主客场：1主场，2客场
    clubPoint = {};              -- 每轮的积分
    clubPointSorted = {};        -- 排序后的积分，格式为 { { id=key, point=value }, { id=key, point=value }... } 按降序排列
    clubGoalDiff = {};           -- 每轮的净胜球
    clubRank = {};
    clubWinNum = {};
    clubDrawNum = {};
    clubLoseNum = {};
    clubGoal = {};
    clubLoseGoal = {};

    currentTurn = nil;
    currentMonth = nil;    -- 从开始月递增，数值可能超过12，用GetRealMonthNum()得到1~12的月份

    popupRoot = nil;
    dateScrollView = nil;
    dateContainer = nil;
    monthLabel = nil;
    rankScrollView = nil;
    rankContainer = nil;
    descLabel = nil;

    teamLeftNameLabel = nil;
    teamLeftLVLabel = nil;
    teamLeftIcon = nil;
    teamLeftPowerLabel = nil;

    teamRightNameLabel = nil;
    teamRightLVLabel = nil;
    teamRightIcon = nil;
    teamRightPowerLabel = nil;

    rewardBox = nil;

    myClubName = nil;
    myClubIcon = nil;
    myClubLv = nil;
    myClubPower = nil;
end
