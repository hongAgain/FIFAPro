module("MsgID", package.seeall)

tb = {};

tb.ServerList        = '1001'
tb.DefaultSrv        = '1002'
tb.SetTutorialStep   = '1003'


tb.VerifyAuth 		 = '2001'
tb.InitRole   		 = '2002'
tb.RoleData          = '2003'
tb.InitRole1  		 = '2004'

tb.ChangeIcon 		 = '2010'
tb.ChangeName		 = '2011'


tb.ReqEmailListMsg	 = '2020'
tb.ReqEmailDetailMsg = '2021'
tb.ReqDeleteEmailMsg = '2022'
tb.ReqGetAttrMailMsg = '2023'

tb.ReqHeroBreakMsg   = '2100'
tb.ReqHeroAdvMsg     = '2101'
--tb.ReqHeroMsg        = '2102'
tb.ReqHeroProMsg     = '2103'
tb.ReqHeroDisMsg     = '2104'
tb.ReqHeroPinfoMsg   = '2105'
tb.ReqHeroPotMsg     = '2106'
tb.ReqHeroCpotMsg    = '2107'
tb.ReqHeroRpotMsg    = '2108'
tb.ReqHeroTeMsg      = '2109';
tb.ReqHeroPotsMsg    = '2110'
tb.ReqHeroSkillLvUpMsg = '2111'
tb.ReqHeroSRefreshMsg = '2112'
tb.ReqHeroCSRefreshMsg = '2113'

tb.GameNews			 = '2201'

-- tb.GameMail			 = '2301'
tb.GameCheckInsInfo	 = '2302'
tb.GameCheckInsToday = '2303'

tb.UseItem           = '3011'
tb.SellItem          = '3012'

tb.DiamondShopInfo	 = '3016'
tb.DiamondShopBuy	 = '3017'

tb.SaveTeam          = '3101'

tb.ScoutConfig       = '3201'
tb.ScoutData         = '3202'
tb.ScoutOne          = '3203'
tb.ScoutTen          = '3204'

tb.Exchange          = '3301'

tb.FreePower         = '3401'
tb.ActivityInfo      = '3402'
tb.TotalRecharge     = '3403'
tb.FreePowerLogs     = '3404'
tb.FreePowerFetch    = '3405'
tb.SevenLoginInfo    = '3406'
tb.SevenLoginReward  = '3407'
tb.AccLoginInfo      = '3408'
tb.AccLoginReward    = '3409'
tb.LevelGiftInfo     = '3410'
tb.LevelGiftReward   = '3411'
tb.LevelRankReward   = '3412'
tb.ActiveFetch       = '3413'
tb.ActiveAd          = '3414'


tb.RaidInfo          = '3501'
tb.RaidStart         = '3502'
tb.RaidResult        = '3503'
tb.RaidLots          = '3504'
tb.RaidFastResult    = '3505'
tb.RaidEnter         = '3506'
tb.RaidStarAward     = '3507'

tb.RaidInfoJY        = '3511'
tb.RaidStartJY       = '3512'
tb.RaidResultJY      = '3513'
tb.RaidLotsJY        = '3514'
tb.RaidFastResultJY  = '3515'
tb.RaidEnterJY       = '3516'
tb.RaidStarAwardJY   = '3517'

tb.LadderInfo        = '3601'
tb.LadderJoin        = '3602'
-- tb.LadderWait        = '3603'
tb.LadderResult      = '3603'
tb.LadderQuit        = '3604'
tb.LadderSort		 = '3605'
tb.LadderadvAward    = '3606'
tb.LadderwinAward    = '3607'
tb.LadderBuyItem	 = '3608'

tb.LeagueInfo	 	 = '3651'
tb.LeagueGroup	 	 = '3652'
tb.LeagueJoin 		 = '3653'

tb.DailyCupInfo		 = '3671'
tb.DailyCupJoin		 = '3672'
tb.DailyCupMass		 = '3673'
tb.DailyCupAl		 = '3674'
tb.DailyCupAward	 = '3675'
tb.DailyCupGam		 = '3676'
tb.DailyCupGl		 = '3677'
tb.DailyCupGAward	 = '3678'

tb.TimeRaidInfo		 = '3701';
tb.TimeRaidStart	 = '3702';
tb.TimeRaidResult	 = '3703';
tb.TimeRaidFastResult= '3704';

tb.RaidDFInfo        = '3801'
tb.RaidDFStart       = '3802'
tb.RaidDFResult      = '3803'
tb.RaidDFFastResult  = '3804'
tb.RaidDFReset       = '3805'
tb.RaidDFBuyItem     = '3806'
tb.RaidDFSort        = '3807'

tb.DailyTaskInfo	 = '3901'
tb.DailyTaskSubmit	 = '3902'
tb.DailyTaskAward	 = '3903'
tb.AchieveTaskInfo	 = '3904'
tb.AchieveTaskSubmit = '3905'

tb.FriendAdd         = '4001'
tb.FriendAccept      = '4002'
tb.FriendRefuse      = '4003'
tb.FriendList        = '4004'
tb.FriendApply       = '4005'
tb.FriendGivePower   = '4006'
tb.FriendDel         = '4007'
tb.FriendPowerList   = '4008'
tb.FriendGetPower    = '4009'
tb.FriendGetAllPower = '4010'
tb.FriendPK          = '4011'
tb.FriendRecommend   = '4012'

tb.GetBroadCast      = '4101';
tb.GetChatMsgList	 = '4102';
tb.SendChatMsg	 	 = '4103';

tb.GetFuncTips		 = '4111';

--tb.CoachReplace		 = '4151';
--tb.CoachAdv			 = '4152';
--tb.CoachPro			 = '4153';
tb.CoachInfo         = '4150'
tb.CoachFameInfo     = '4151'
tb.CoachFameReward   = '4152'
tb.CoachSkillInfo    = '4153'
tb.CSUnlockSlot      = '4154'
tb.CSUpgrade         = '4155'
tb.CSLoadSkill       = '4156'
tb.CoachTalentInfo   = '4157'
tb.CTUpgradeSkill    = '4158'
tb.CTResetPoint      = '4159'
tb.CTUnlockPage      = '4160'
tb.CTEnablePage      = '4161'

tb.BuyEnergy         = '4201';
tb.BuyGold           = '4202';
tb.BuyHandOfMidas    = '4203'
tb.HandOfMidasInfo   = '4204'

tb.LevelFundInfo     = '4301';
tb.LevelFundBuy      = '4302';
tb.LevelFundRepay    = '4303';
tb.BuyMonthlyCard    = '4304'

tb.RankList          = '4401';

tb.PVELeagueInfo         = '4501';
tb.PVELeagueSelect       = '4502';
tb.PVELeagueQuit         = '4503';
tb.PVELeagueBattleStart  = '4504';
tb.PVELeagueBattleResult = '4505';

tb.CarnivalInfo      = '4551'
tb.CarnivalSubmit    = '4552'
tb.GetCPointAward    = '4553'
tb.GetCShop          = '4554'

tb.Mobage_TempAuth   = '1'
tb.Mobage_PayCreate  = '2'
tb.Mobage_PaySubmit  = '3'
