module("LuaConst", package.seeall)


Const = {};

function SetConst(k,v)
	if Const[k] ~= nil then
		print("repeat key" ..k);
	end
	Const[k] = v;
end


function OnInit()
	SetConst('HTTP_PREFIX','http://119.15.139.144/S');
    
    SetConst("UExp",                          "1");  --球队经验
    SetConst("HExp",                          "2");  --球员经验
    SetConst("CExp",                          "3");  --教练经验
    SetConst("SB",                            "4");  --游戏币 金币
    SetConst("GB",                            "5");  --RMB 钻石
    SetConst("Power",                         "6");  --体力
    SetConst("Honor",                         "7");  --荣誉 天梯荣誉值
    SetConst("CostDF",                        "8");  --巅峰印记
    SetConst("CContract",                     "9");  --教练合同
    SetConst("HContract",                     "10");  --球员合同
    SetConst("UnionCoin",                     "11");  -- 联盟币
    SetConst("QuizCoin",                      "12");  -- 竞猜币
    SetConst("CoachFame",                     "14")   --教练名声


    SetConst("ExpCard1",                    "20001");  --小球员经验卡
    SetConst("ExpCard2",                    "20002");  --中球员经验卡
    SetConst("ExpCard3",                    "20003");  --大球员经验卡
    SetConst("SkillCard",                   "20015");  --技能卡
    SetConst("TeachCard",                   "20019");  --训话卡
    SetConst("Sweep",                       "20021");  --扫荡券
    SetConst("SkillTicket",                 "20033");  --技能升级券
    SetConst("SkillStone",                  "20039");  --技能洗练石
    
    SetConst("Train",                       "10019");  --特训卡
    


	-----msg url config------------
	SetConst('ReqEmailListUrl',             '/mail/rows');
	SetConst('ReqEmailDetailUrl',           '/mail/info');
    SetConst('ReqEmailDeletelUrl',          '/mail/del');
    SetConst('ReqEmailGetAttrUrl',          '/mail/attr');

	SetConst('ReqHeroBreakUrl',             '/hero/slv');
	SetConst('ReqHeroAdvUrl',               '/hero/adv');
    SetConst('ReqHeroProUrl',               '/hero/pro');
    SetConst('ReqHeroDisUrl',               '/hero/dis');
    SetConst('ReqHeroPinfoUrl',             '/hero/pinfo');
    SetConst('ReqHeroPotUrl',               '/hero/pot');
    SetConst('ReqHeroCpotUrl',              '/hero/cPot');
    SetConst('ReqHeroPotsUrl',              '/hero/pots')
    SetConst('ReqHeroRpotUrl',              '/hero/rPot');
    SetConst('ReqHeroTeUrl',                '/hero/te');
    SetConst('ReqHeroSkillLvUpUrl',         '/hero/skillLvUp');
    SetConst('ReqHeroSRefreshUrl',          '/hero/sRefresh');
    SetConst('ReqHeroCSRefreshUrl',         '/hero/csRefresh');
    SetConst('GameNews',                    '/game/news');
    -- SetConst('GameMail',      '/raid/info');


    SetConst('ReqRaidInfoUrl',              '/raid/info');
    SetConst('ReqRaidStartUrl',             '/raid/start');
    SetConst('ReqRaidResultUrl',            '/raid/result');
    SetConst('ReqRaidLotsUrl',              '/raid/lots');
    SetConst('ReqRaidFastResultUrl',        '/raid/fastResult');
    SetConst('ReqRaidEnterUrl',             '/raid/enter');
    SetConst('ReqRaidStarAwardUrl',         '/raid/starAward');
    SetConst('ReqRaidInfoUrlJY',            '/raidjy/info');
    SetConst('ReqRaidStartUrlJY',           '/raidjy/start');
    SetConst('ReqRaidResultUrlJY',          '/raidjy/result');
    SetConst('ReqRaidLotsUrlJY',            '/raidjy/lots');
    SetConst('ReqRaidFastResultUrlJY',      '/raidjy/fastResult');
    SetConst('ReqRaidEnterUrlJY',           '/raidjy/enter');
    SetConst('ReqRaidStarAwardUrlJY',       '/raidjy/starAward');

    SetConst('ReqRaidDFInfoUrl',            '/raiddf/info');
    SetConst('ReqRaidDFStartUrl',           '/raiddf/start');
    SetConst('ReqRaidDFResultUrl',          '/raiddf/result');
    SetConst('ReqRaidDFFastResultUrl',      '/raiddf/fastResult');
    SetConst('ReqRaidDFResetUrl',           '/raiddf/reset');
    SetConst('ReqRaidDFBuyItemUrl',         '/raiddf/buyItem');
    SetConst('ReqRaidDFSortUrl',            '/raiddf/sort');

    SetConst('ReqFriendAddUrl',             '/friend/add');
    SetConst('ReqFriendAcceptUrl',          '/friend/accept');
    SetConst('ReqFriendRefuseUrl',          '/friend/reject');
    SetConst('ReqFriendListUrl',            '/friend/fl');
    SetConst('ReqFriendApplyUrl',           '/friend/fal');
    SetConst('ReqFriendGivePowerUrl',       '/friend/gp');
    SetConst('ReqFriendDelUrl',             '/friend/del');
    SetConst('ReqFriendPowerListUrl',       '/friend/rpl');
    SetConst('ReqFriendGetPowerUrl',        '/friend/rp');
    SetConst('ReqFriendGetAllPowerUrl',     '/friend/fastrp');
    SetConst('ReqFriendPKUrl',              '/friend/pk');
    SetConst('ReqFriendRecommendUrl',       '/friend/rfl');

	SetConst('MobageCNCredential',          '/mobageCN/credential');
	SetConst('MobageCNLogin',               '/mobageCN/login');
	SetConst('MobageCNPayCreate',           '/mobageCN/payCreate');
	SetConst('MobageCNPaySubmit',           '/mobageCN/paySubmit');


	SetConst('ServerList',                  '/game/server');

    SetConst('FreeLogin',                   '/debug/login');

    SetConst('DefaultSrv',                  '/user/role');
	SetConst('RoleInit',                    '/user/init');
    SetConst('RoleInit1',                   '/user/init1');
    SetConst('RoleData',                    '/user/info');
    SetConst('RoleChangeIcon',              '/user/ci');
    SetConst('RoleChangeName',              '/user/cn');

	SetConst('UseItem',                     '/item/use');
	SetConst('SellItem',                    '/item/sell');

    SetConst('SaveTeam',                    '/team/save');

    SetConst('ScoutConfig',                 '/hotel/rows');
    SetConst('ScoutOne',                    '/hotel/start');

    SetConst('Exchange',                    '/game/code');

    SetConst('ActivityInfo',                '/active/info')
    SetConst('ActivityData',                '/active/data');
    SetConst('FreePowerLogs',               '/freePower/logs');
    SetConst('FreePowerFetch',              '/freePower/fetch');
    SetConst('SevenLoginInfo',              '/event/initSevenLoginPanel')
    SetConst('SevenLoginReward',            '/event/gainRewardForSL')
    SetConst('AccLoginInfo',                '/event/initAccLoginPanel')
    SetConst('AccLoginReward',              '/event/gainRewardForAL')
    SetConst('LevelGiftInfo',               '/event/initLvUpRewardPanel')
    SetConst('LevelGiftReward',             '/event/gainRewardForLU')
    SetConst('LevelRankReward',             '/event/initLvRankRewardPanel')
    SetConst('ActiveFetch',                 '/active/fetch')
    SetConst('ActiveAd',                    '/active/ad')

    SetConst('LadderInfo',                  '/ladder/info');
    SetConst('LadderJoin',                  '/ladder/join');
    -- SetConst('LadderWait',                  '/ladder/wait');
    SetConst('LadderResult',                '/ladder/result');
    SetConst('LadderQuit',                  '/ladder/quit');
    SetConst('LadderSort',                  '/ladder/sort');
    SetConst('LadderadvAward',              '/ladder/advAward');
    SetConst('LadderwinAward',              '/ladder/winAward');
    SetConst('LadderBuyItem',               '/ladder/buyItem');

    SetConst('TimeRaidInfo',                '/raidds/info');
    SetConst('TimeRaidStart',               '/raidds/start');
    SetConst('TimeRaidResult',              '/raidds/result');
    SetConst('TimeRaidFastResult',          '/raidds/fastResult');
    SetConst('LeagueInfo',                  '/league/info');
    SetConst('LeagueGroup',                 '/league/groupData');
    SetConst('LeagueJoin',                  '/league/join');

    SetConst('DailyCupInfo',                '/cup/info');
    SetConst('DailyCupJoin',                '/cup/join');
    SetConst('DailyCupMass',                '/cup/mass');
    SetConst('DailyCupAl',                  '/cup/al');
    SetConst('DailyCupAward',               '/cup/award');
    SetConst('DailyCupGam',                 '/cup/gam');
    SetConst('DailyCupGl',                  '/cup/gl');
    SetConst('DailyCupGAward',              '/cup/gaward');

    SetConst('DiamondShopInfo',             '/shop/info');
    SetConst('DiamondShopBuy',              '/shop/buy');
    SetConst('DailyTaskInfo',               '/dayTask/info');
    SetConst('DailyTaskSubmit',             '/dayTask/submit');
    SetConst('DailyTaskAward',              '/dayTask/award');
    SetConst('AchieveTaskInfo',             '/task/info');
    SetConst('AchieveTaskSubmit',           '/task/submit');

    SetConst('GameCheckInsInfo',            '/sign/info');
    SetConst('GameCheckInsCheck',           '/sign/signOn');

    SetConst('GetBroadCast',                '/broadcast/getBdList');
    SetConst('GetChatMsgList',              '/chat/getMsgList');
    SetConst('SendChatMsg',                 '/chat/sendMsg');

    SetConst('GetFuncTips',                 '/funcTips/getFuncTips');

    SetConst('CoachReplace',                '/coach/replace');
    SetConst('CoachAdv',                    '/coach/adv');
    SetConst('CoachPro',                    '/coach/pro');
    SetConst('CoachInfo',                   '/coach/getCoachInfo')
    SetConst('CoachFameInfo',               '/coach/getFameRewardInfo')
    SetConst('CoachFameReward',             '/coach/gainFameReward')
    SetConst('CoachSkillInfo',              '/coach/getSkillList')
    SetConst('CSUnlockSlot',                '/coach/unlockSkillSlot')
    SetConst('CSUpgrade',                   '/coach/upgradeSkill')
    SetConst('CSLoadSkill',                 '/coach/loadSkill')
    SetConst('CoachTalentInfo',             '/coach/getTalentPageInfo')
    SetConst('CTUpgradeSkill',              '/coach/upgradeTalentSkill')
    SetConst('CTResetPoint',                '/coach/resetTalentPage')
    SetConst('CTUnlockPage',                '/coach/unlockTalentPage')
    SetConst('CTEnablePage',                '/coach/enableTalentPage')

    SetConst('LevelFundInfo',               '/invest/data');
    SetConst('LevelFundBuy',                '/invest/buy');
    SetConst('LevelFundRepay',              '/invest/repay');
    SetConst('BuyMonthlyCard',              '/invest/mc')
    
    SetConst('BuyEnergyUrl',                '/shop/buyPower');
    SetConst('BuyGoldUrl',                  '/shop/buySb');
    
    SetConst('SetTutorialStep',             '/guide/setGuideStep');

    SetConst('RankList',                    '/rank/rankList');

    SetConst('HandOfMidasInfo',             '/shop/openMidas')
    SetConst('BuyHandOfMidas',              '/shop/buyMidas')

    SetConst('PVELeagueInfo',               '/club/info');
    SetConst('PVELeagueSelect',             '/club/select');
    SetConst('PVELeagueQuit',               '/club/quit');
    SetConst('PVELeagueBattleStart',        '/club/start');
    SetConst('PVELeagueBattleResult',       '/club/result');

    SetConst('CarnivalInfo',                '/task/carnival')
    SetConst('CarnivalSubmit',              '/task/csubmit')
    SetConst('GetCPointAward',              '/task/cpointAward')
    SetConst('GetCShop',                    '/task/cshop')

	----------------------UI Name----------------------
	SetConst('UISelectServer',              'UISelectServer');
    -- SetConst('UISelectServerNoSDK', 'UISelectServerNoSDK');
    SetConst('UILoginServerNoSDK',          'UILoginServerNoSDK');
    SetConst('UILoginServerList',           'UILoginServerList');

	SetConst('UICreateRole',                'UICreateRole');
	SetConst('UIWaiting',                   'UIWaiting');
	SetConst('UILoading',                   'UILoading');
	SetConst('UIBag',                       'UIBag');
    SetConst('UIChatting',                  'UIChatting');
    SetConst('UIPrivateChat',               'UIPrivateChat');

	SetConst('UIFormation',                 'UIFormation');
    SetConst('UIForceQuitGame',             'UIForceQuitGame');
    SetConst('UILobby',                     'UILobby');
    SetConst('UIMatches',                   'UIMatches');
    SetConst('UIPlayerInfoBase',            'UIPlayerInfoBase');
    SetConst('UIPlayerInfo',                'UIPlayerInfo');
    SetConst('UIPlayerInfoAttribute',       'UIAttribute');
    SetConst('UIPlayerInfoAdavenced',       'UIAdvanced');
    SetConst('UIPlayerInfoBreak',           'UIBreak');
    SetConst('UIPlayerInfoSkill',           'UISkill');
    SetConst('UISkillPot',                  'UISkillPot');
    SetConst('UIPot',                       'UIPot');
    SetConst('UIPotTips',                   'UIPotTips');
    SetConst('UIPlayerPot',                 'UIPlayerPot');
    SetConst('UIUpgradePotion',             'UIUpgradePotion');
    SetConst('UIOriginTips',                'UIOriginTips');
    SetConst('UIHead',                      'UIHead');
    SetConst('UITeamSettingBox',            'UITeamSettingBox');
    SetConst('UITeamIconList',              'UITeamIconList');
    SetConst('UIPlayerList',                'UIPlayerList');
    SetConst('UIPlayerListFilter',          'UIPlayerListFilter');
    SetConst('UITrainTalk',                 'UITrainTalk');
    SetConst('UIScout',                     'UIScout');
    SetConst('UIRecharge',                  'UIRecharge');
    SetConst('UITeamLegend',                'UITeamLegend');
    SetConst('UILevelMap',                  'UILevelMap');
    SetConst('UIChallengeTL',               'UIChallengeTL');
    SetConst('UIChallengeEvent',            'UIChallengeEvent');
    SetConst('UIPrepareBattle',             'UIPrepareBattle');
    SetConst('UIPrepareMatch',              'UIPrepareMatch');
    SetConst('UIBattleResultS',             'UIBattleResultS');
    SetConst('UIBattleResultF',             'UIBattleResultF');
    SetConst('UIBattleResultD',             'UIBattleResultD');
    SetConst('UIUpgradeCoach',              'UIUpgradeCoach');
    SetConst('UIUpgradePlayer',             'UIUpgradePlayer');
    SetConst('UIBattleDetail',              'UIBattleDetail');
    SetConst('UIGetItem',                   'UIGetItem');
    SetConst('UIPeakRoad',                  'UIPeakRoad');
    SetConst('UIPeakRoadSub',               'UIPeakRoadSub');
    SetConst('UIPeakRoadRank',              'UIPeakRoadRank');
    SetConst('UIPeakRoadTips',              'UIPeakRoadTips');
    SetConst('UISweepResult',               'UISweepResult');
    SetConst('UIFriend',                    'UIFriend');
    SetConst('UIFriendRecommend',           'UIFriendRecommend');
    SetConst('UIFriendTips',                'UIFriendTips');
    SetConst('UIFriendView',                'UIFriendView');
    SetConst('FriendListItem',              'FriendList');
    SetConst('FriendPowerItem',             'FriendPower');
    SetConst('FriendMatchItem',             'FriendMatch');
    SetConst('FriendApplyItem',             'FriendApply');
    SetConst('FriendRecommendItem',         'FriendRecommend');
    SetConst('UISelectFormation',           'UISelectFormation');
    SetConst('UIActivity',                  'UIActivity');
    SetConst('UILadderMatch',               'UILadderMatch');
    SetConst('UILadderPrizeInfoManager',    'UILadderPrizeInfoManager');
    SetConst('UILadderRankInfoManager',     'UILadderRankInfoManager');
    SetConst('UILadderRankingsInfoManager', 'UILadderRankingsInfoManager');
    SetConst('UILadderMatchQualifierHint',  'UILadderMatchQualifierHint');
    SetConst('UILadderMatchingBox',         'UILadderMatchingBox');
    SetConst('UILadderMatchRecordBox',      'UILadderMatchRecordBox');
    SetConst('UILadderMatchShareBox',       'UILadderMatchShareBox');
    SetConst('UISell',                      'UISell');
    SetConst('UITimeRaid',                  'UITimeRaid');
    SetConst('UITimeRaidDifficultyBox',     'UITimeRaidDifficultyBox');
    SetConst('UIGameMail',                  'UIGameMail');
    SetConst('UIGameMailDetailBox',         'UIGameMailDetailBox');
    SetConst('UINews',                      'UINews');
    SetConst('UITask',                      'UITask');
    SetConst('UITaskItem',                  'UITaskItem');
    SetConst('UITaskDaily',                 'UITaskDaily');
    SetConst('UITaskAchieve',               'UITaskAchieve');
    SetConst('UITaskProgressItem',          'UITaskProgressItem');
    SetConst('TaskData',                    'TaskData');
    SetConst('UILeagueMatch',               'UILeagueMatch');
    SetConst('UIPVELeague',                 'UIPVELeague');
    SetConst('UIPVELeagueSchedule',         'UIPVELeagueSchedule');

    SetConst('UIDailyCup',                  'UIDailyCup');
    SetConst('UIDailyCupGamPlayerList',     'UIDailyCupGamPlayerList');
    SetConst('UIDailyCupGamPlayerFilterBox','UIDailyCupGamPlayerFilterBox');

    SetConst('UIShop',                      'UIShop');
    SetConst('ShopConfirmBox',              'ShopConfirmBox');

    SetConst('UIGameCheckInsBox',           'UIGameCheckInsBox');
    SetConst('UIGameCheckInsItem',          'UIGameCheckInsItem');
    SetConst('UIGameCheckInsBonusHint',     'UIGameCheckInsBonusHint');
    SetConst('UIGameCheckInsItemTips',      'UIGameCheckInsItemTips');
    SetConst('UIGameCheckInsGetItemBox',    'UIGameCheckInsGetItemBox');

    SetConst('UICoach'                      ,'UICoach');
    SetConst('UICoachDetailWindow'          ,'UICoachDetailWindow');
    SetConst('UICoachTeamPowerUp'           ,'UICoachTeamPowerUp');
    SetConst('UICoachChangePosWindow'       ,'UICoachChangePosWindow');
    SetConst('CoachChangeListAvatar'        ,'CoachChangeListAvatar');
    SetConst('UICoachScout'                 ,'UICoachScout');

	SetConst('UIMsgBoxConfirm',             'UIMsgBoxConfirm');
    SetConst('UIMsgBoxConfirmCheckInRule',  'UIMsgBoxConfirmCheckInRule');
    SetConst('UIMsgBoxHint',                'UIMsgBoxHint');
    SetConst('UIMsgBoxLineHint',            'UIMsgBoxLineHint');
    SetConst('UIMsgBoxScrollText',          'UIMsgBoxScrollText');
    SetConst('UIMsgBoxAdaptive',            'UIMsgBoxAdaptive');
    SetConst('UIMsgBoxQualifierAnimation',  'UIMsgBoxQualifierAnimation');

    SetConst('UIItemTips',                  'UIItemTips');

    SetConst('UIChallengeTimeRaid',         'UIChallengeTimeRaid');

    SetConst('UILogView',                   'UILogView');
    -- SetConst('UIContinuousReqTest','UIContinuousReqTest');
    SetConst('UISkillInfo',                 'UISkillInfo')
    SetConst('UIRankList',                  'UIRankList')
    SetConst('UIListFilter',                "UIListFilter")
    SetConst('UIQuickPurchase',             "UIQuickPurchase");
    SetConst('UIHandOfMidas',               "UIHandOfMidas")

    SetConst('UICarnival',                  'UICarnival')
    SetConst('UICarnivalPopWin',            'UICarnivalPopWin')

    ---------------------- Rules ----------------------------------
    SetConst('LadderRule',                  'ladder');
    SetConst('LeagueRule',                  'league');
    SetConst('DailyCupRule',                'dailycup');
    SetConst('TimeRaidRule',                'timeraid');
    SetConst('EpicRoadRule',                'epicroad');
    SetConst('TimeRaidType',                3);
    SetConst('TimeRaidNrByType',            6);

    ----------------------Texture----------------------------------
    SetConst('UniformIcon',                 "UniformIcon");
    SetConst('FlagIcon',                    "FlagIcon");
    SetConst('ClubIcon',                    "ClubIcon");
    SetConst('PlayerHeadIcon',              "PlayerHeadIcon");
    SetConst('ItemIcon',                    "ItemIcon");
    SetConst('SkillIcon',                   "SkillIcon")
    SetConst('ClubIcon190',                 "ClubIcon190");
    SetConst('CupDF',                       "CupDF");
    SetConst('CupLadder',                   "CupLadder");
    SetConst('CoachHeadIcon',               "CoachHeadIcon");
    SetConst('PlayerSystem',                "PlayerSystem");
    SetConst('PveMapFx',                    "PveMapFx");
    SetConst('PvePlayerFx',                 "PvePlayerFx");
    SetConst('PveMapB',                     "PveMapB");
    SetConst('PveMapS',                     "PveMapS");
    SetConst('ActivityImg',                 "Activity")

    ---------------------GameObject--------------------------------
    SetConst('UIAvatar',                    0);

    ---------------------CreatePersonType--------------------------
    SetConst('PlayerTypePerson',            0);
    SetConst('PlayerTypeLobby',             1);
    SetConst('PlayerTypeCoach',             2);


    ---------------------Animation Clip----------------------------
    SetConst('CreateRolePose',              0);
    SetConst('LobbyPose',                   1);
    SetConst('Scout',                       2);
    SetConst('PlayerInfoBase',              3)

    ---------------------MD5 Salt-----------------------------------
    SetConst('MD5Key',          'FifaPro007.sz.h');

end
