module("UIDailyCupScript",package.seeall);

require "Config"
require "Common/UnityCommonScript"
require "Game/Role"

require "UILua/UIDailyCupFilterRecordBox"
require "UILua/UIDailyCupGambleBox"
require "UILua/UIDailyCupMyGamBox"
require "UILua/UIDailyCupPrizeManager"

local cupGroups = nil;
local winnerGroups = nil;
local finalGroups = nil;
local championPlayer = nil;

local timeProgress = nil;
local selectedInfo = nil;

local lastSelectedGameID = nil;
local lastSelectedHostSelectedIcon = nil;
local lastSelectedGuestSelectedIcon = nil;
local dailyCupTitle = nil;
local dailyCupTime = nil;
local dailyCupTimerID = nil;

local buttonMyGam = nil;
local buttonReward = nil;
local buttonFilter = nil;
local buttonSignUp = nil;
local buttonSignUpTitle = nil;

local uiPopupRoot = nil;

local strBGMusic = "";
local window = nil;
local windowComponent = nil;

local lastGamParamData = nil;
local defaultPlayerData = nil;
local needOpenGamBox = false;

dailyCupSettings = {
	CurrentSign = nil,
	DailyCupTitle = "DailyCupTitle",
	DailyCupRule = "DailyCupRules",
	MatchNotAvailable = "DailyCupMatchNotAvailable",
	SignUpTitle = "SignUp",
	SignedUpTitle = "SignedUp",
	SignUpHintMsg = "DailyCupSignUpHintMsg",
	SignedUpHintMsg = "DailyCupSignedUpHintMsg",
	FilterTitle = "DailyCupFilterTitle",
	FilterRoundNum = "DailyCupFilterRoundNum",
	FilterNoRoundNum = "DailyCupFilterCalculating",
	RoundNum = "RoundNum",
	MonthAndDay = "MonthAndDay",
	MatchFinished = "MatchFinished",
	MatchNotFinished = "MatchNotFinished",
	DailyCupGamSupport = "DailyCupGamSupport",
	DailyCupGamWon = "DailyCupGamWon",
	DailyCupGamLose = "DailyCupGamLose",
	DailyCupGamColorWon = Color.New(159/255, 255/255, 159/255, 255/255),
	DailyCupGamColorLose = Color.New(159/255, 102/255, 102/255, 255/255),
	TimeColorActive = Color.New(102/255,204/255,255/255,255/255),
	TimeColorDisactive = Color.New(171/255,173/255,185/255,255/255)
}

function OnStart(gameObject, params)
    print("UIDailyCupScript.lua .. OnStart");
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");

    BindUI();
    SetInfo();
    if(needOpenGamBox)then
    	if( lastGamParamData~=nil and lastGamParamData.id ~= nil ) then
    		print("on open");
			UIDailyCupGambleBox.CreateGambleBox(uiPopupRoot,windowComponent,UIHelper.GetPanelDepth(uiPopupRoot),
				lastGamParamData.id,
				lastGamParamData.deadLineTime,
				lastGamParamData.users,
				defaultPlayerData,
				lastGamParamData.onGamble,
				SetDefaultPlayerData);
		end
    end
    AudioMgr.Instance():PlayBGMusic(strBGMusic);
end


function BindUI()
	local transform = window.transform;

	dailyCupTitle = TransformFindChild(transform,"CommonNode/Title");
	dailyCupTime = TransformFindChild(transform,"CommonNode/Time");
	dailyCupTimerID = LuaTimer.AddTimer(true,-1,SetTimeInfo);

	--buttons
	Util.AddClick(TransformFindChild(transform,"ButtonHint").gameObject,OnClickHint);
	buttonMyGam = TransformFindChild(transform,"ButtonMyGam");
	buttonReward = TransformFindChild(transform,"ButtonReward");
	buttonFilter = TransformFindChild(transform,"ButtonFilter");
	buttonSignUp = TransformFindChild(transform,"ButtonSignUp");
	buttonSignUpTitle = TransformFindChild(buttonSignUp,"Label");
	AddOrChangeClickParameters(buttonMyGam.gameObject,OnClickMyGam,{});
	AddOrChangeClickParameters(buttonReward.gameObject,OnClickReward,{});
	AddOrChangeClickParameters(buttonFilter.gameObject,OnClickFilter,{});
	AddOrChangeClickParameters(buttonSignUp.gameObject,OnClickSignUp,{});
	
	--4 cup groups
	cupGroups = {};
	for i=1,4 do
		cupGroups[i] = {};
		cupGroups[i].players = {};
		cupGroups[i].Lines = {};
		for j=1,2 do
			cupGroups[i].players[j] = {};
			cupGroups[i].players[j].transform = TransformFindChild(transform,"CupGroup"..i.."/CupPlayer"..j);
			cupGroups[i].players[j].gameObject = cupGroups[i].players[j].transform.gameObject;
			cupGroups[i].players[j].Button = TransformFindChild(cupGroups[i].players[j].transform,"IconBG");
			cupGroups[i].players[j].Icon = TransformFindChild(cupGroups[i].players[j].transform,"IconRoot/Icon");
			cupGroups[i].players[j].Selected = TransformFindChild(cupGroups[i].players[j].transform,"Selected");
			cupGroups[i].players[j].IsMyTeam = TransformFindChild(cupGroups[i].players[j].transform,"IsMyTeam");

			cupGroups[i].Lines[j] = TransformFindChild(transform,"CupGroup"..i.."/PlayerLine"..j);

			GameObjectSetActive(cupGroups[i].players[j].Selected.gameObject,false);
			GameObjectSetActive(cupGroups[i].players[j].Icon.gameObject,false);
			GameObjectSetActive(cupGroups[i].players[j].IsMyTeam.gameObject,false);
			GameObjectSetActive(cupGroups[i].Lines[j].gameObject,false);
		end
	end
	--2 winner groups
	winnerGroups = {};
	for i=1,2 do
		winnerGroups[i] = {};
		winnerGroups[i].players = {};
		winnerGroups[i].Lines = {};
		for j=1,2 do
			winnerGroups[i].players[j] = {};
			winnerGroups[i].players[j].transform = TransformFindChild(transform,"WinnerGroup"..i.."/CupPlayer"..j);
			winnerGroups[i].players[j].gameObject = winnerGroups[i].players[j].transform.gameObject;
			winnerGroups[i].players[j].Button = TransformFindChild(winnerGroups[i].players[j].transform,"IconBG");
			winnerGroups[i].players[j].Icon = TransformFindChild(winnerGroups[i].players[j].transform,"IconRoot/Icon");
			winnerGroups[i].players[j].Selected = TransformFindChild(winnerGroups[i].players[j].transform,"Selected");
			winnerGroups[i].players[j].IsMyTeam = TransformFindChild(winnerGroups[i].players[j].transform,"IsMyTeam");
			winnerGroups[i].players[j].ButtonGam = TransformFindChild(winnerGroups[i].players[j].transform,"ButtonGam");
			winnerGroups[i].players[j].Supported = TransformFindChild(winnerGroups[i].players[j].transform,"Supported");

			winnerGroups[i].Lines[j] = TransformFindChild(transform,"WinnerGroup"..i.."/PlayerLine"..j);

			GameObjectSetActive(winnerGroups[i].players[j].Icon.gameObject,false);
			GameObjectSetActive(winnerGroups[i].players[j].Selected.gameObject,false);
			GameObjectSetActive(winnerGroups[i].players[j].IsMyTeam.gameObject,false);
			GameObjectSetActive(winnerGroups[i].players[j].ButtonGam.gameObject,false);
			GameObjectSetActive(winnerGroups[i].players[j].Supported.gameObject,false);
			GameObjectSetActive(winnerGroups[i].Lines[j].gameObject,false);
		end
	end
	finalGroup = {};
	finalGroup.players = {};
	finalGroup.Lines = {};
	for j=1,2 do
		finalGroup.players[j] = {};
		finalGroup.players[j].transform = TransformFindChild(transform,"FinalGroup/CupPlayer"..j);
		finalGroup.players[j].gameObject = finalGroup.players[j].transform.gameObject;
		finalGroup.players[j].Button = TransformFindChild(finalGroup.players[j].transform,"IconBG");
		finalGroup.players[j].Selected = TransformFindChild(finalGroup.players[j].transform,"Selected");
		finalGroup.players[j].Icon = TransformFindChild(finalGroup.players[j].transform,"IconRoot/Icon");
		finalGroup.players[j].IsMyTeam = TransformFindChild(finalGroup.players[j].transform,"IsMyTeam");
		finalGroup.players[j].ButtonGam = TransformFindChild(finalGroup.players[j].transform,"ButtonGam");
		finalGroup.players[j].Supported = TransformFindChild(finalGroup.players[j].transform,"Supported");

		finalGroup.Lines[j] = TransformFindChild(transform,"FinalGroup/PlayerLine"..j);

		GameObjectSetActive(finalGroup.players[j].Icon.gameObject,false);
		GameObjectSetActive(finalGroup.players[j].Selected.gameObject,false);
		GameObjectSetActive(finalGroup.players[j].IsMyTeam.gameObject,false);
		GameObjectSetActive(finalGroup.players[j].ButtonGam.gameObject,false);
		GameObjectSetActive(finalGroup.players[j].Supported.gameObject,false);
		GameObjectSetActive(finalGroup.Lines[j].gameObject,false);
	end

	championPlayer = {};
	championPlayer.transform = TransformFindChild(transform,"Champion");
	championPlayer.gameObject = championPlayer.transform.gameObject;
	championPlayer.Button = TransformFindChild(championPlayer.transform,"IconBG");
	championPlayer.Icon = TransformFindChild(championPlayer.transform,"IconRoot/Icon");
	championPlayer.Selected = TransformFindChild(championPlayer.transform,"Selected");
	championPlayer.IsMyTeam = TransformFindChild(championPlayer.transform,"IsMyTeam");
	championPlayer.ButtonGam = TransformFindChild(championPlayer.transform,"ButtonGam");
	championPlayer.Supported = TransformFindChild(championPlayer.transform,"Supported");

	AddOrChangeClickParameters(championPlayer.Button.gameObject,OnClickMatch,
	{
		isChampion = true
	});

	GameObjectSetActive(championPlayer.Icon.gameObject,false);
	GameObjectSetActive(championPlayer.Selected.gameObject,false);
	GameObjectSetActive(championPlayer.IsMyTeam.gameObject,false);
	GameObjectSetActive(championPlayer.ButtonGam.gameObject,false);
	GameObjectSetActive(championPlayer.Supported.gameObject,false);

	timeProgress = {};
	timeProgress.timeFirstRound1 = TransformFindChild(transform,"TimeLine/TimeFirstRound1");
	timeProgress.timeFirstRound2 = TransformFindChild(transform,"TimeLine/TimeFirstRound2");
	timeProgress.timeSecondRound1 = TransformFindChild(transform,"TimeLine/TimeSecondRound1");
	timeProgress.timeSecondRound2 = TransformFindChild(transform,"TimeLine/TimeSecondRound2");
	timeProgress.timeFinalRound = TransformFindChild(transform,"TimeLine/TimeFinalRound");
	UIHelper.SetWidgetColor(timeProgress.timeFirstRound1,dailyCupSettings.TimeColorDisactive);
	UIHelper.SetWidgetColor(timeProgress.timeFirstRound2,dailyCupSettings.TimeColorDisactive);
	UIHelper.SetWidgetColor(timeProgress.timeSecondRound1,dailyCupSettings.TimeColorDisactive);
	UIHelper.SetWidgetColor(timeProgress.timeSecondRound2,dailyCupSettings.TimeColorDisactive);
	UIHelper.SetWidgetColor(timeProgress.timeFinalRound,dailyCupSettings.TimeColorDisactive);

	selectedInfo = {};
	selectedInfo.transform = TransformFindChild(transform,"SelectedInfo");
	selectedInfo.gameObject = selectedInfo.transform.gameObject;
	selectedInfo.Icon1 = TransformFindChild(selectedInfo.transform,"IconRoot1/Icon");
	selectedInfo.Icon2 = TransformFindChild(selectedInfo.transform,"IconRoot2/Icon");
	selectedInfo.Name1 = TransformFindChild(selectedInfo.transform,"Name1");
	selectedInfo.Name2 = TransformFindChild(selectedInfo.transform,"Name2");
	GameObjectSetActive(selectedInfo.gameObject,false);

	uiPopupRoot = TransformFindChild(transform,"PopupRoot");
end

function SetInfo()
	-- local infoData = PVPMsgManager.Get_DailyCupInfoData();
	local infoData = PVPMsgManager.GetPVPData(MsgID.tb.DailyCupInfo);
	dailyCupSettings.CurrentSign = infoData.sign;
	UIHelper.SetLabelTxt(dailyCupTitle,GetLocalizedString(dailyCupSettings.DailyCupTitle,dailyCupSettings.CurrentSign));
	
	SetTimeInfo();

	if(infoData.reg == nil or infoData.reg == false or infoData.reg == 0 or infoData.reg <= infoData.sign) then
		--not signed up for next, clicking on the button will request for signing up
		UIHelper.SetLabelTxt(buttonSignUpTitle,GetLocalizedString(dailyCupSettings.SignUpTitle));
		AddOrChangeClickParameters(buttonSignUp.gameObject,OnClickSignUp,{signedUp = false});
	else
		--already signed up, clicking on the button will show sth
		UIHelper.SetLabelTxt(buttonSignUpTitle,GetLocalizedString(dailyCupSettings.SignedUpTitle));
		AddOrChangeClickParameters(buttonSignUp.gameObject,OnClickSignUp,{signedUp = true});
	end

	local roundFlag = 0;
	if(infoData.game == nil or infoData.status == 0 or infoData.status == 1) then
		--set all to default if no game data available
		AddOrChangeClickParameters(buttonFilter.gameObject,OnClickFilter,{noData = true});
	else
		--set groups
		local hasGamedFlag = false;
		for k,v in pairs(infoData.game) do

			if(v.type == 2 and v.round == 0) then
				--set cupGroup info
				local groupIndex = ( tonumber(v.team) - 1 ) * 2 + v.gid;
				for i=1,2 do
					local playerData = infoData.users[v.user[i]];
					GameObjectSetActive(cupGroups[groupIndex].players[i].Icon.gameObject,true);
					GameObjectSetActive(cupGroups[groupIndex].players[i].IsMyTeam.gameObject,playerData.uid == Role.Get_uid());
					Util.SetUITexture(cupGroups[groupIndex].players[i].Icon,LuaConst.Const.ClubIcon,tostring(playerData.icon).."_1", true);
					AddOrChangeClickParameters(cupGroups[groupIndex].players[i].Button.gameObject,OnClickMatch,
						{
							isChampion = false,
							gameID = v._id,
							hostID = infoData.users[v.user[1]].uid,
							hostName = infoData.users[v.user[1]].name,
							hostIcon = infoData.users[v.user[1]].icon,
							hostScore = v.score[1],
							hostSelectedTrans = cupGroups[groupIndex].players[1].Selected,
							guestID = infoData.users[v.user[2]].uid,
							guestName = infoData.users[v.user[2]].name,
							guestIcon = infoData.users[v.user[2]].icon,
							guestScore = v.score[2],
							guestSelectedTrans = cupGroups[groupIndex].players[2].Selected
						});
				end		
				if(v.state == 2) then
					--set result
					GameObjectSetActive(cupGroups[groupIndex].Lines[v.win].gameObject,true);
					GameObjectSetActive(winnerGroups[tonumber(v.team)].players[tonumber(v.gid)].ButtonGam,false);

					--firstRoundOver
					if(roundFlag < 1)then
						roundFlag = 1;
					end
				elseif(v.gam == 1)then
					GameObjectSetActive(winnerGroups[tonumber(v.team)].players[tonumber(v.gid)].ButtonGam,false);
					GameObjectSetActive(winnerGroups[tonumber(v.team)].players[tonumber(v.gid)].Supported,true);
					hasGamedFlag = true;
				else
					--set ButtonGam Active
					GameObjectSetActive(winnerGroups[tonumber(v.team)].players[tonumber(v.gid)].ButtonGam,true);
					AddOrChangeClickParameters(winnerGroups[tonumber(v.team)].players[tonumber(v.gid)].ButtonGam.gameObject,OnClickButtonGam,
					{
						id=v._id,
						deadLineTime = Config.GetProperty(Config.CupGamTime(),"0","end"),
						users = {infoData.users[v.user[1]],infoData.users[v.user[2]]},
						onGamble=function ()
							GameObjectSetActive(winnerGroups[tonumber(v.team)].players[tonumber(v.gid)].Supported,true);	
							DisableButtonGams();
						end
					});
				end
				
			elseif(v.type == 2 and v.round == 1)then
				--set winnerGroup info
				local groupIndex = tonumber(v.team);
				-- local groupIndex = (v.team);
				local realPlayerIndex = nil;
				for i=1,2 do
					realPlayerIndex = GetPlayerCupGroupID(v.user[i]);
					local playerData = infoData.users[v.user[i]];
					GameObjectSetActive(winnerGroups[groupIndex].players[realPlayerIndex].Icon.gameObject,true);
					GameObjectSetActive(winnerGroups[groupIndex].players[realPlayerIndex].IsMyTeam.gameObject,playerData.uid == Role.Get_uid());
					Util.SetUITexture(winnerGroups[groupIndex].players[realPlayerIndex].Icon,LuaConst.Const.ClubIcon,tostring(playerData.icon).."_1", true);
					AddOrChangeClickParameters(winnerGroups[groupIndex].players[realPlayerIndex].Button.gameObject,OnClickMatch,
						{
							isChampion = false,
							gameID = v._id,
							hostID = infoData.users[v.user[1]].uid,
							hostName = infoData.users[v.user[1]].name,
							hostIcon = infoData.users[v.user[1]].icon,
							hostScore = v.score[1],
							hostSelectedTrans = winnerGroups[groupIndex].players[GetPlayerCupGroupID(v.user[1])].Selected,
							guestID = infoData.users[v.user[2]].uid,
							guestName = infoData.users[v.user[2]].name,
							guestIcon = infoData.users[v.user[2]].icon,
							guestScore = v.score[2],
							guestSelectedTrans = winnerGroups[groupIndex].players[3-GetPlayerCupGroupID(v.user[1])].Selected
						});
					end
				if(v.state == 2) then
					--set result
					local winnerIndex = v.win;
					if(realPlayerIndex ~= 2)then
						winnerIndex = 2-(v.win+1)%2;
					end
					GameObjectSetActive(winnerGroups[groupIndex].Lines[winnerIndex].gameObject,true);
					GameObjectSetActive(finalGroup.players[tonumber(v.team)].ButtonGam,false);

					--SecondRoundOver
					if(roundFlag < 2)then
						roundFlag = 2;
					end
				elseif(v.gam == 1)then
					GameObjectSetActive(finalGroup.players[tonumber(v.team)].ButtonGam,false);
					GameObjectSetActive(finalGroup.players[tonumber(v.team)].Supported,true);
					hasGamedFlag = true;
				else
					GameObjectSetActive(finalGroup.players[tonumber(v.team)].ButtonGam,true);
					AddOrChangeClickParameters(finalGroup.players[tonumber(v.team)].ButtonGam.gameObject,OnClickButtonGam,
					{
						id=v._id,
						deadLineTime = Config.GetProperty(Config.CupGamTime(),"1","end"),
						users = {infoData.users[v.user[1]],infoData.users[v.user[2]]},
						onGamble = function ()
							GameObjectSetActive(finalGroup.players[tonumber(v.team)].Supported,true);
							DisableButtonGams();
						end
					});
				end
				
			elseif(v.type == 3) then
				--set finalGroup info
				local realPlayerIndex = nil;
				for i=1,2 do
					realPlayerIndex = GetPlayerWinnerGroupID(v.user[i]);
					local playerData = infoData.users[v.user[i]];
					GameObjectSetActive(finalGroup.players[realPlayerIndex].Icon.gameObject,true);
					GameObjectSetActive(finalGroup.players[realPlayerIndex].IsMyTeam.gameObject,playerData.uid == Role.Get_uid());
					Util.SetUITexture(finalGroup.players[realPlayerIndex].Icon,LuaConst.Const.ClubIcon,tostring(playerData.icon).."_1", true);
					AddOrChangeClickParameters(finalGroup.players[realPlayerIndex].Button.gameObject,OnClickMatch,
						{
							isChampion = false,
							gameID = v._id,
							hostID = infoData.users[v.user[1]].uid,
							hostName = infoData.users[v.user[1]].name,
							hostIcon = infoData.users[v.user[1]].icon,
							hostScore = v.score[1],
							hostSelectedTrans = finalGroup.players[GetPlayerWinnerGroupID(v.user[1])].Selected,
							guestID = infoData.users[v.user[2]].uid,
							guestName = infoData.users[v.user[2]].name,
							guestIcon = infoData.users[v.user[2]].icon,
							guestScore = v.score[2],
							guestSelectedTrans = finalGroup.players[3-GetPlayerWinnerGroupID(v.user[1])].Selected
						});
				end
				if(v.state == 2) then
					--set set champion
					local winnerIndex = v.win;
					if(realPlayerIndex ~= 2)then
						winnerIndex = 2-(v.win+1)%2;
					end
					GameObjectSetActive(finalGroup.Lines[winnerIndex].gameObject,true);
					local championData = infoData.users[v.user[winnerIndex]];
					GameObjectSetActive(championPlayer.Icon.gameObject,true);
					Util.SetUITexture(championPlayer.Icon,LuaConst.Const.ClubIcon,tostring(championData.icon).."_1", true);
					GameObjectSetActive(championPlayer.IsMyTeam.gameObject,championData.uid == Role.Get_uid());

					--FinalRoundOver
					if(roundFlag < 3)then
						roundFlag = 3;
					end
				elseif(v.gam == 1)then
					GameObjectSetActive(championPlayer.ButtonGam,false);
					GameObjectSetActive(championPlayer.Supported,true);
					hasGamedFlag = true;
				else
					GameObjectSetActive(championPlayer.ButtonGam,true);
					AddOrChangeClickParameters(championPlayer.ButtonGam.gameObject,OnClickButtonGam,
					{
						id=v._id,
						deadLineTime = Config.GetProperty(Config.CupGamTime(),"2","end"),
						users = {infoData.users[v.user[1]],infoData.users[v.user[2]]},
						onGamble=function ()
							GameObjectSetActive(championPlayer.Supported,true);
							DisableButtonGams();
						end
					});

				end
			end
		end
		if(hasGamedFlag)then
			DisableButtonGams();
		end
	end

	if(roundFlag == 0)then
		--show first round
		UIHelper.SetWidgetColor(timeProgress.timeFirstRound1,dailyCupSettings.TimeColorActive);
		UIHelper.SetWidgetColor(timeProgress.timeFirstRound2,dailyCupSettings.TimeColorActive);
	elseif(roundFlag == 1)then
		--show second round
		UIHelper.SetWidgetColor(timeProgress.timeSecondRound1,dailyCupSettings.TimeColorActive);
		UIHelper.SetWidgetColor(timeProgress.timeSecondRound2,dailyCupSettings.TimeColorActive);	
	elseif(roundFlag == 2)then
		--show final round	
		UIHelper.SetWidgetColor(timeProgress.timeFinalRound,dailyCupSettings.TimeColorActive);
	end
end

function SetTimeInfo()
	if(dailyCupTime~=nil)then
		UIHelper.SetLabelTxt(dailyCupTime,os.date("%X",os.time()));
	end
end

function GetPlayerCupGroupID(userID)
	-- local infoData = PVPMsgManager.Get_DailyCupInfoData();
	local infoData = PVPMsgManager.GetPVPData(MsgID.tb.DailyCupInfo);
	for k,v in pairs(infoData.game) do
		if(v.round == 0 and v.type == 2) then
			if(v.user[1] == userID or v.user[2] == userID) then
				return v.gid;
			end
		end
	end
end

function GetPlayerWinnerGroupID(userID)
	-- local infoData = PVPMsgManager.Get_DailyCupInfoData();
	local infoData = PVPMsgManager.GetPVPData(MsgID.tb.DailyCupInfo);
	for k,v in pairs(infoData.game) do
		if(v.round == 1 and v.type == 2) then
			if(v.user[1] == userID or v.user[2] == userID) then
				return tonumber(v.team);
			end
		end
	end
end

function OnClickMatch(go)
	local listener = UIHelper.GetUIEventListener(go);
	if( listener.parameter ~= nil or listener.parameter ~= {} ) then
		-- parameter has:
		-- isChampion
		-- hostID = infoData.users[v.user[i]].uid,
		-- hostName = infoData.users[v.user[i]].name,
		-- hostIcon = infoData.users[v.user[i]].icon,
		-- hostScore = v.score[1],
		-- hostSelectedTrans = finalGroup.players[realPlayerIndex].Selected,
		-- guestID = infoData.users[v.user[3-i]].uid,
		-- guestName = infoData.users[v.user[3-i]].name,
		-- guestIcon = infoData.users[v.user[3-i]].icon,
		-- guestScore = v.score[2],
		-- guestSelectedTrans = finalGroup.players[3-realPlayerIndex].Selected

		if(listener.parameter.isChampion)then
			GameObjectSetActive(selectedInfo.gameObject,false);
			if(lastSelectedGameID~=nil)then
				GameObjectSetActive(lastSelectedHostSelectedIcon.gameObject,false);
				GameObjectSetActive(lastSelectedGuestSelectedIcon.gameObject,false);
				lastSelectedGameID = nil;
				lastSelectedHostSelectedIcon = nil;
				lastSelectedGuestSelectedIcon = nil;
			end
			return;
		else
			if(lastSelectedGameID ~= listener.parameter.gameID )then
				if(lastSelectedGameID ~= nil)then
					--switch off old ones
					GameObjectSetActive(lastSelectedHostSelectedIcon.gameObject,false);
					GameObjectSetActive(lastSelectedGuestSelectedIcon.gameObject,false);
				end
				lastSelectedGameID = listener.parameter.gameID;
				lastSelectedHostSelectedIcon = listener.parameter.hostSelectedTrans;
				lastSelectedGuestSelectedIcon = listener.parameter.guestSelectedTrans;
			end
			--switch on new ones
			GameObjectSetActive(listener.parameter.hostSelectedTrans.gameObject,true);
			GameObjectSetActive(listener.parameter.guestSelectedTrans.gameObject,true);

			--change selected info	
			GameObjectSetActive(selectedInfo.gameObject,true);
			Util.SetUITexture(selectedInfo.Icon1,LuaConst.Const.ClubIcon,tostring(listener.parameter.hostIcon).."_1", true);
			Util.SetUITexture(selectedInfo.Icon2,LuaConst.Const.ClubIcon,tostring(listener.parameter.guestIcon).."_1", true);
			UIHelper.SetLabelTxt(selectedInfo.Name1,listener.parameter.hostName);
			UIHelper.SetLabelTxt(selectedInfo.Name2,listener.parameter.guestName);
		end		
	end
end

function OnClickButtonGam(go)
	local listener = UIHelper.GetUIEventListener(go);
	if( listener.parameter.id ~= nil ) then
		UIDailyCupGambleBox.CreateGambleBox(uiPopupRoot,windowComponent,UIHelper.GetPanelDepth(uiPopupRoot),
			listener.parameter.id,
			listener.parameter.deadLineTime,
			listener.parameter.users,
			nil,
			listener.parameter.onGamble,
			SetDefaultPlayerData);
		lastGamParamData = listener.parameter;
	end
end

function SetDefaultPlayerData(selectedPlayerData)
	--item and data
	defaultPlayerData = selectedPlayerData;
	needOpenGamBox = true;
end

function DisableButtonGams()
	for i=1,2 do		
		for j=1,2 do
			GameObjectSetActive(winnerGroups[i].players[j].ButtonGam.gameObject,false);
		end
		GameObjectSetActive(finalGroup.players[i].ButtonGam.gameObject,false);
	end
	GameObjectSetActive(championPlayer.ButtonGam.gameObject,false);
end

function OnClickMyGam()
	local AfterReq = function ( ... )
		UIDailyCupMyGamBox.CreateMyGamBox(uiPopupRoot,windowComponent,UIHelper.GetPanelDepth(uiPopupRoot));
	end
	-- PVPMsgManager.RequestDailyCupGl(nil,AfterReq);
	PVPMsgManager.RequestPVPMsg(MsgID.tb.DailyCupGl, LuaConst.Const.DailyCupGl, nil, AfterReq, nil );
end

function OnClickReward()
	UIDailyCupPrizeManager.CreateDailyCupPrizeBox( uiPopupRoot, windowComponent, UIHelper.GetPanelDepth(uiPopupRoot) )
end

function OnClickFilter(go)
	local listener = UIHelper.GetUIEventListener(go);
	if(listener ~= nil and listener.parameter ~=nil)then		
		if( listener.parameter.noData == true ) then
			WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { GetLocalizedString(dailyCupSettings.FilterNoRoundNum) });
			return;
		end
	end
	--else do following
	local AfterReqMass = function ()
		UIDailyCupFilterRecordBox.CreateRecordBox(uiPopupRoot,windowComponent,dailyCupSettings.CurrentSign,nil);
	end
	-- PVPMsgManager.RequestDailyCupMass(nil,AfterReqMass);
	PVPMsgManager.RequestPVPMsg(MsgID.tb.DailyCupMass, LuaConst.Const.DailyCupMass, nil, AfterReqMass, nil );
end

function OnClickSignUp(go)
	local listener = UIHelper.GetUIEventListener(go);
	if(listener ~= nil and listener.parameter ~=nil)then
		
		if( listener.parameter.signedUp == true ) then
			--show sth indicating signed up status
			WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { GetLocalizedString(dailyCupSettings.SignedUpHintMsg) });
		else
			--show sth indicating non-signed up status
			local AfterReqJoin = function ()
				--already signed up, clicking on the button will show sth
				WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { GetLocalizedString(dailyCupSettings.SignUpHintMsg,Util.GetLocalMonth(),Util.GetLocalDayInMonth()) });
				UIHelper.SetLabelTxt(buttonSignUpTitle,GetLocalizedString(dailyCupSettings.SignedUpTitle));
				AddOrChangeClickParameters(buttonSignUp.gameObject,OnClickSignUp,{signedUp = true});
			end

			-- PVPMsgManager.RequestDailyCupJoin(nil,AfterReqJoin);
    		PVPMsgManager.RequestPVPMsg(MsgID.tb.DailyCupJoin, LuaConst.Const.DailyCupJoin, nil, AfterReqJoin, nil );
		end
	end
end

function OnClickHint()
	WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxScrollText, { PVPMsgManager.GetDailyCupRule() });	
	-- WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxScrollText, { GetModuleRules(LuaConst.Const.DailyCupRule); });
end 

function OnHide()
    -- print("..OnHide TimeRaid");
end

function OnShow()
	if(needOpenGamBox)then
    	if( lastGamParamData~=nil and lastGamParamData.id ~= nil ) then
    		print("on OnShow");
			UIDailyCupGambleBox.CreateGambleBox(uiPopupRoot,windowComponent,UIHelper.GetPanelDepth(uiPopupRoot),
				lastGamParamData.id,
				lastGamParamData.deadLineTime,
				lastGamParamData.users,
				defaultPlayerData,
				lastGamParamData.onGamble,
				SetDefaultPlayerData);
		end
    end
end

function OnDestroy()
	UIDailyCupFilterRecordBox.OnDestroy();
	UIDailyCupGambleBox.OnDestroy();
	UIDailyCupMyGamBox.OnDestroy();
	UIDailyCupPrizeManager.OnDestroy();

	if(dailyCupTimerID~=nil) then
		LuaTimer.RmvTimer(dailyCupTimerID);
	end

	cupGroups = nil;
	winnerGroups = nil;
	finalGroups = nil;
	championPlayer = nil;
	timeProgress = nil;
	selectedInfo = nil;
	lastSelectedGameID = nil;
	lastSelectedHostSelectedIcon = nil;
	lastSelectedGuestSelectedIcon = nil;
	dailyCupTitle = nil;
	dailyCupTime = nil;
	dailyCupTimerID = nil;
	buttonMyGam = nil;
	buttonReward = nil;
	buttonFilter = nil;
	buttonSignUp = nil;
	buttonSignUpTitle = nil;
	uiPopupRoot = nil;
	window = nil;
	windowComponent = nil;
	lastGamParamData = nil;
	defaultPlayerData = nil;
	needOpenGamBox = false;
end