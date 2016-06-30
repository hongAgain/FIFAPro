module("UILadderMatchScript", package.seeall)

require "Common/UnityCommonScript"
require "Game/Role"

--child module
require "UILua/UILadderRankInfoManager"
require "UILua/UILadderPrizeInfoManager"
require "UILua/UILadderRankingsInfoManager"
require "UILua/UILadderMatchRecordBox"
-- require "UILua/UILadderMatchingBox"
require "UILua/UILadderMatchQualifierHint"
require "UILua/UIShop"
require "UILua/UIBattleResultS"
require "UILua/UIBattleResultF"
require "UILua/UIBattleResultD"



local ladderSettings = {
	strBGMusic = "BG_LadderMatch",
	DailyJoinRequest = 5,
	strLocalizeLadderRule = "LadderRules",
	QualifierMatchSum = 3,
	strLocalizeRankReached = "RankReached",
	strDailyBonusAhead = "LadderDailyBonusAhead",
	strDailyBonusSum = "LadderDailyBonusSum"

};

--player info
local playerIcon = nil;
local playerName = nil;
local playerLevel = nil;
local ladderHonorIcon = nil;
local ladderHonorPoint = nil;

local buttonHint = nil;
local buttonRecords = nil;
local buttonShop = nil;
local buttonMatch = nil;

local uiInfoRoot = nil;
local uiPopupRoot = nil;

local window = nil;
local windowComponent = nil;

function OnStart(gameObject, params)
    print("UILadderMatchScript.lua .. OnStart");
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");

    --fetch data then set UI
    BindUI();   
	SetInfo();
	InitializeState();
	SaveLocalKeys();
    AudioMgr.Instance():PlayBGMusic(ladderSettings.strBGMusic);
end

function BindUI()
	local transform = window.transform;

	playerIcon = TransformFindChild(transform,"PlayerInfo/IconRoot/Icon");
	playerName = TransformFindChild(transform,"PlayerInfo/PlayerName");
	playerLevel = TransformFindChild(transform,"PlayerInfo/PlayerLevel");
	ladderHonorIcon = TransformFindChild(transform,"PlayerInfo/Honor/Icon");
	ladderHonorPoint = TransformFindChild(transform,"PlayerInfo/Honor/Point");

	buttonHint = TransformFindChild(transform,"ButtonHint");
	buttonRecords = TransformFindChild(transform,"ButtonRecords");
	buttonShop = TransformFindChild(transform,"ButtonShop");
	buttonMatch = TransformFindChild(transform,"ButtonMatch");

	AddOrChangeClickParameters(buttonHint.gameObject,OnClickHint,nil);
	AddOrChangeClickParameters(buttonRecords.gameObject,OnClickRecords,nil);
	AddOrChangeClickParameters(buttonShop.gameObject,OnClickShop,nil);
	AddOrChangeClickParameters(buttonMatch.gameObject,OnClickMatch,nil);

	uiInfoRoot = TransformFindChild(transform,"InfoRoot");
	uiPopupRoot = TransformFindChild(transform,"PopupRoot");
end

function SetInfo()
	local infoData = PVPMsgManager.GetPVPData(MsgID.tb.LadderInfo);
	--set player info
	Util.SetUITexture(playerIcon,LuaConst.Const.ClubIcon,Role.GetRoleIcon().."_2", true);
	UIHelper.SetLabelTxt(playerName,Role.Get_name());
	UIHelper.SetLabelTxt(playerLevel,"Lv."..Role.Get_lv());
	-- UIHelper.SetSpriteName(ladderHonorIcon,);

    if(infoData.daily.join < ladderSettings.DailyJoinRequest)then
    	UIHelper.SetLabelTxt(ladderHonorPoint,GetLocalizedString(ladderSettings.strDailyBonusAhead, (ladderSettings.DailyJoinRequest - infoData.daily.join), infoData.daily.joinAward, infoData.daily.join, ladderSettings.DailyJoinRequest));
    else
    	UIHelper.SetLabelTxt(ladderHonorPoint,GetLocalizedString(ladderSettings.strDailyBonusSum, infoData.daily.joinAward));
    end

	--set other info
	if(NeedShowRankAnime())then
		UILadderRankInfoManager.CreateRankInfo(uiInfoRoot,windowComponent,2,GetLastLadderInfoLevel());
	else	
		UILadderRankInfoManager.CreateRankInfo(uiInfoRoot,windowComponent,1);		
	end	
	UILadderPrizeInfoManager.CreatePrizeInfo(uiInfoRoot,windowComponent);
	UILadderRankingsInfoManager.CreateRankingsInfo(uiInfoRoot,windowComponent);
end

function NeedShowRankAnime()
	local infoData = PVPMsgManager.GetPVPData(MsgID.tb.LadderInfo);
	local localData = Config.GetTemplate(Config.LadderLevel());
	if(infoData.info.chance == 0)then
		local lastChance = 0;
		local lastLevel = 0;
		local keyData = PVPMsgManager.GetLadderSeasonKeyData();
		if(HasLastLadderInfoChance())then
			lastChance = GetLastLadderInfoChance();
		end
		if(HasLastLadderInfoLevel())then
			lastLevel = GetLastLadderInfoLevel();
		end

		if(lastChance == 1 and (lastLevel < infoData.info.level ))then
			--qualifier successed 
			if(localData[tostring(lastLevel)].type ~= localData[tostring(infoData.info.level)].type)then
				return true;
			end
		elseif(lastChance == -1 and (lastLevel > infoData.info.level))then
			--religation failed
			if(localData[tostring(lastLevel)].type ~= localData[tostring(infoData.info.level)].type)then
				return true;
			end
		end
	end
	return false;
end

function InitializeState()
	local infoData = PVPMsgManager.GetPVPData(MsgID.tb.LadderInfo);
	local localData = Config.GetTemplate(Config.LadderLevel());

	if(infoData.info.chance == 1 and infoData.info.result[1]+infoData.info.result[2] == 0) then
		--just started qualifier match
		UILadderMatchQualifierHint.CreateQualifierHint(uiPopupRoot,windowComponent,1,0,0,0,nil);
	elseif(infoData.info.chance == -1 and infoData.info.result[1]+infoData.info.result[2] == 0) then
		--just started relegation match
		UILadderMatchQualifierHint.CreateQualifierHint(uiPopupRoot,windowComponent,4,0,0,0,nil);
	elseif(infoData.info.chance == 0)then
		local lastChance = 0;
		local lastLevel = 0;
		local keyData = PVPMsgManager.GetLadderSeasonKeyData();
		if(HasLastLadderInfoChance())then
			lastChance = GetLastLadderInfoChance();
		end
		if(HasLastLadderInfoLevel())then
			lastLevel = GetLastLadderInfoLevel();
		end

		local qualifierMatchNum = 0;		
		if(infoData.info.logs ~= nil) then
			local qualifierMatchSum = #infoData.info.logs;
			qualifierMatchNum = math.min(3,(infoData.info.result[1]+infoData.info.result[2]),qualifierMatchSum); 
		end

		local logResultTabel = GetQualifierMatchResults(qualifierMatchNum);
		local qualifierType = -1;
		local qualifierOnOver = nil;

		local ShowAnime = function ()
			ShowQualifierAnime(lastLevel,infoData.info.level);
		end

		if(lastChance == 1)then
			--finished a qualifier match
			if(lastLevel == infoData.info.level)then
				--qualifier failed 
				qualifierType = 3;
			elseif(lastLevel < infoData.info.level )then
				--qualifier successed 
				qualifierType = 2;
				if(localData[tostring(lastLevel)].type ~= localData[tostring(infoData.info.level)].type)then
					qualifierOnOver = ShowAnime;
				end
			end
			UILadderMatchQualifierHint.CreateQualifierHint(uiPopupRoot,windowComponent,qualifierType,logResultTabel[1],logResultTabel[2],logResultTabel[3],qualifierOnOver);
		elseif(lastChance == -1)then
			--finished a religation match
			if(lastLevel > infoData.info.level)then
				--religation failed  
				qualifierType = 6;
				if(localData[tostring(lastLevel)].type ~= localData[tostring(infoData.info.level)].type)then
					qualifierOnOver = ShowAnime;
				end
			elseif(lastLevel == infoData.info.level )then
				--religation successed 
				qualifierType = 5;
			end
			UILadderMatchQualifierHint.CreateQualifierHint(uiPopupRoot,windowComponent,qualifierType,logResultTabel[1],logResultTabel[2],logResultTabel[3],qualifierOnOver);
		end
	end
	
end

function HasLastLadderInfoChance()
	return Util.HasKey("LadderInfoChance_"..PVPMsgManager.GetLadderSeasonKeyData());
end

function GetLastLadderInfoChance()
	return Util.GetInt("LadderInfoChance_"..PVPMsgManager.GetLadderSeasonKeyData());
end

function HasLastLadderInfoLevel()
	return Util.HasKey("LadderInfoLevel_"..PVPMsgManager.GetLadderSeasonKeyData());
end

function GetLastLadderInfoLevel()
	return Util.GetInt("LadderInfoLevel_"..PVPMsgManager.GetLadderSeasonKeyData());
end

function SaveLocalKeys()
	local infoData = PVPMsgManager.GetPVPData(MsgID.tb.LadderInfo);
	--save key locally
	Util.SetInt("LadderInfoChance_"..PVPMsgManager.GetLadderSeasonKeyData(),infoData.info.chance);
	Util.SetInt("LadderInfoLevel_"..PVPMsgManager.GetLadderSeasonKeyData(),infoData.info.level);
end

function ShowQualifierAnime(formerRank,latterRank)

	local AnimeStep3 = function ()
		--turn on current medal light
		UILadderRankInfoManager.FadeInRankMedal(latterRank,AfterFade);
	end	

	local AnimeStep2 = function ()
		--fade out latter medal		
		UILadderRankInfoManager.FadeOutRankMedal(latterRank,AfterFade);

		--show animation msg box
		local localData = Config.GetTemplate(Config.LadderLevel());
		WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxQualifierAnimation,{
			iconName = localData[tostring(latterRank)].cupIconName, 
			description = GetLocalizedString(ladderSettings.strLocalizeRankReached,localData[tostring(latterRank)].type), 
			youWon = formerRank<latterRank, 
			onOver = AnimeStep3});
	end

	local AnimeStep1 = function ()
		--turn off last medal light
		UILadderRankInfoManager.FadeOutRankSelected(formerRank,AnimeStep2);
	end

	--start animes
	AnimeStep1();	
end

function AfterFade()
	-- body
end

function GetQualifierMatchResults( requestNum )
	local infoData = PVPMsgManager.GetPVPData(MsgID.tb.LadderInfo);
	if(IsTableEmpty(infoData.info.logs)) then
		return;
	end	

	local logNum = #infoData.info.logs;
	local resultTable = {};

	--generate 1 to req num
	for i=logNum,logNum-requestNum+1,-1 do
		if(infoData.info.logs[i]~=nil) then
			local currentIndex = logNum-i+1;
			if(infoData.info.logs[i].goal[1]>infoData.info.logs[i].goal[2]) then
				resultTable[currentIndex] = 1;
			elseif(infoData.info.logs[i].goal[1]<infoData.info.logs[i].goal[2]) then
				resultTable[currentIndex] = -1;
			else 
				resultTable[currentIndex] = 0;
			end
		end
	end
	if(requestNum<3)then
		for i=requestNum+1,3 do
			resultTable[i] = nil;
		end
	end
	return resultTable;
end

function OnClickHint()
	WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxScrollText, { PVPMsgManager.GetLadderRule() });
end

function OnClickRecords()
	-- body
	UILadderMatchRecordBox.CreateRecordBox( uiPopupRoot, windowComponent, UIHelper.GetPanelDepth(uiPopupRoot)+1 );
end

function OnClickShop()
    WindowMgr.ShowWindow(LuaConst.Const.UIShop,{shopType=UIShopSettings.ShopType.LadderShop});
end

function RefreshUIInfo()
	local AfterRequesting = function ()
	    local infoData = PVPMsgManager.GetPVPData(MsgID.tb.LadderInfo);
	    if(infoData.daily.join < ladderSettings.DailyJoinRequest)then
			UIHelper.SetLabelTxt(ladderHonorPoint,GetLocalizedString(ladderSettings.strDailyBonusAhead, (ladderSettings.DailyJoinRequest - infoData.daily.join), infoData.daily.joinAward, infoData.daily.join, ladderSettings.DailyJoinRequest));
	    else
			UIHelper.SetLabelTxt(ladderHonorPoint,GetLocalizedString(ladderSettings.strDailyBonusSum, infoData.daily.joinAward));
	    end

		--set other info
		if(NeedShowRankAnime())then
			UILadderRankInfoManager.RefreshInfo(2,GetLastLadderInfoLevel());
		else	
			UILadderRankInfoManager.RefreshInfo(1);
		end	
		UILadderPrizeInfoManager.RefreshInfo();
		UILadderRankingsInfoManager.RefreshInfo();

		InitializeState();
		SaveLocalKeys();
    end
    -- PVPMsgManager.RequestLadderInfo(AfterRequesting);
    PVPMsgManager.RequestPVPMsg( MsgID.tb.LadderInfo, LuaConst.Const.LadderInfo, nil, AfterRequesting, nil );
end

function OnClickMatch()
	-- Start match waiting
    local AfterRequesting = function ()
        local joinData = PVPMsgManager.GetPVPData(MsgID.tb.LadderJoin);
        if(joinData ~= nil and joinData ~= {} ) then
 		-- open battle here
			local delegateFinishMatch = function ()
				OnMatchEnd();
			end

			local delegateStartMatch = function ()
				CombatData.FillData(joinData, CombatData.COMBAT_TYPE.PVPLADDER, delegateFinishMatch);
			end

	    	PVPMsgManager.SetLadderEnemyInfo(joinData["P2"].name, joinData["P2"].lv, joinData["P2"].icon, joinData["P2"].id, joinData["P2"].hero);

			UIPrepareMatch.RegisterBtnMatch(delegateStartMatch);
			local tb = {PrepareMatchType = UIPrepareMatch.enum_PrepareMatchType.PVPLADDER};
			UIPrepareMatch.TryOpen(tb);

		else
			WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { "服务器战斗返回数据异常"});
		end
	end

	local AfterRequestFailed = function ()
        print("Debug print ===== AfterRequestFailed");
	end
    PVPMsgManager.RequestPVPMsg(MsgID.tb.LadderJoin, LuaConst.Const.LadderJoin, nil, AfterRequesting, AfterRequestFailed );
end

function OnMatchEnd(joinData)
    local tb = CombatData.GetResultTable();
    local m_quit = tb['Giveup'];
	local m_meScore = tonumber(tb['HomeScore']);
	local m_enemyScore = tonumber(tb['AwayScore']);

	local AfterRequesting = function ()
        local resultData = PVPMsgManager.GetPVPData(MsgID.tb.LadderResult);
        if(resultData ~= nil and resultData ~= {} ) then
			--show match result

			local data_ = {
				UExp = 0,
				HExp = 0,
				money = 0,
				honor = 0,
				item = resultData.item,
				score = {m_meScore,m_enemyScore}
			};
	     	data_.BattleResultType = UIBattleResultS.enum_BattleResultType.Ladder;
	        data_.Callback = RefreshUIInfo;  

			if (m_meScore >m_enemyScore) then
	            WindowMgr.ShowWindow(LuaConst.Const.UIBattleResultS,data_);
	        elseif(m_meScore == m_enemyScore)then
	            WindowMgr.ShowWindow(LuaConst.Const.UIBattleResultD,data_);
	        else
	        	WindowMgr.ShowWindow(LuaConst.Const.UIBattleResultF,data_);
	        end
		else
			WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { "服务器战斗返回数据异常"});
		end
	end

	local AfterRequestFailed = function ()
        print("Debug print ===== AfterRequestFailed");
	end

    if not m_quit then
        local dictPrams = {};
	    dictPrams['score'] = tostring(tb['HomeScore'] .. ":".. tb['AwayScore']);
	    dictPrams['act'] = tostring(tb['PVEData']);
	    dictPrams['sign'] = tb['md5'];
		PVPMsgManager.RequestPVPMsg(MsgID.tb.LadderResult, LuaConst.Const.LadderResult, dictPrams, AfterRequesting, AfterRequestFailed );
    end
end

function OnHide()
    print("..OnHide LadderMatch");
end

function OnShow()
	print("..OnShow LadderMatch");
end

function OnDestroy()
	UILadderRankInfoManager.OnDestroy();
	UILadderPrizeInfoManager.OnDestroy();
	UILadderRankingsInfoManager.OnDestroy();
	UILadderMatchRecordBox.OnDestroy();
	-- UILadderMatchingBox.OnDestroy();
	UILadderMatchQualifierHint.OnDestroy();

	playerIcon = nil;
	playerName = nil;
	playerLevel = nil;
	ladderHonorIcon = nil;
	ladderHonorPoint = nil;

	buttonHint = nil;
	buttonRecords = nil;
	buttonShop = nil;
	buttonMatch = nil;

	uiInfoRoot = nil;
	uiPopupRoot = nil;

	window = nil;
	windowComponent = nil;
end