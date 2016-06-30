module("ModuleMgr", package.seeall)

require "Game/Role"
require "Game/Login"
require "Game/GameMailData"
require "Game/LuaConst"
require "Game/UnitTest"
require "Game/ItemSys"
require "Game/Hero"
require "Game/TeamLegendData"
require "Game/MsgID"
require "Game/PVPMsgManager"
require "Game/PeakRoadData"
require "Game/TimeRaidData"
require "Game/ShopData"
require "Game/GameNewsData"
require "Game/TaskData"
require "Game/GameCheckInsData"
require "Game/FriendData"
require "Game/ChattingData"
require "Game/HintManager"
require "Game/CoachData"
require "Game/QuickPurchase"
require "Game/DailyDataSys"
require "Game/LevelFundData"
require "Tutorial/LuaTutorial"
require "Game/ActivityData"
require "Game/LobbySceneManager"
require "Game/RankListData"
require "Game/HandOfMidasData"
require "Game/RechargeData"
require "Game/PVELeagueData"
require "Game/CarnivalData"

function OnInit()
    math.randomseed(os.time());
    --consume the first num which is not really randomized
    math.random();

	LuaConst.OnInit();
	Role.OnInit();
	Login.OnInit();
	ItemSys.OnInit();
	GameMailData.OnInit();
	Hero.OnInit();
    TeamLegendData.OnInit();
	UnitTest.OnInit();
	PVPMsgManager.OnInit();
    PeakRoadData.OnInit();
    TimeRaidData.OnInit();
    ShopData.OnInit();
	GameNewsData.OnInit();
	TaskData.OnInit();
	GameCheckInsData.OnInit();
    FriendData.OnInit();
    ChattingData.OnInit();
    HintManager.OnInit();
    CoachData.OnInit();
    QuickPurchase.OnInit();
    LevelFundData.OnInit();
    ActivityData.OnInit();
    RankListData.OnInit();
    HandOfMidasData.OnInit();
    RechargeData.OnInit();
    PVELeagueData.OnInit();
    CarnivalData.OnInit()

    LuaTutorial.OnInit();
end

function OnRelease()

end
