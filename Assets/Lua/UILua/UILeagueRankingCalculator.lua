module("UILeagueRankingCalculator",package.seeall);

local PointRankingData = {};
local players = {};
local matchTime = nil;
local groupTime = nil;
local groupData = nil;

function CalculateData()

	--initialize PointRankingData
	for i=1,8 do
		PointRankingData[i] = {};
		PointRankingData[i].uid = nil;
		PointRankingData[i].name = nil;
		PointRankingData[i].num = 0;
		PointRankingData[i].point = 0;
		PointRankingData[i].win = 0;
		PointRankingData[i].lose = 0;
		PointRankingData[i].tie = 0;
		PointRankingData[i].goal = 0;
		PointRankingData[i].conceded = 0;
		PointRankingData[i].goalDiff = 0;
	end
	if(matchTime == nil) then
		--sort match time, result:[minID,...,maxID]
		groupTime = PVPMsgManager.Get_LeagueTimeTable();
		matchTime = {};
		for k,v in pairs(groupTime) do
			table.insert(matchTime,v.id);
		end
		table.sort(matchTime);
	end
	--sorted players
	-- groupData = PVPMsgManager.Get_LeagueGroupData();	
	groupData = PVPMsgManager.GetPVPData(MsgID.tb.LeagueGroup);

	if(IsTableEmpty(groupData))then
		return nil;
	else
		for k,v in pairs(groupData.Player) do
			players[v.i] = v;
		end

		if(groupTime == nil) then
			print("gorupTime is nil");
		end

		if(matchTime == nil) then
			print("matchTime is nil");
		end

		--search through all matches	
		for i=1,7 do			
			local guestDayIndex = i * 2;
			local hostDayIndex = guestDayIndex - 1;
			for j,v in ipairs(matchTime) do
				local matchID = i..v;
				local hostIndex = groupTime[v].day[hostDayIndex];
				local guestIndex = groupTime[v].day[guestDayIndex];
				local matchData = groupData.AList[matchID];
				UpdatePlayerData(hostIndex, matchData.state,matchData.score[1],matchData.score[2]);
				UpdatePlayerData(guestIndex, matchData.state,matchData.score[2],matchData.score[1]);
			end
		end

		for i=1,8 do
			PointRankingData[i].uid = players[i].uid;
			PointRankingData[i].name = players[i].name;
		end

		table.sort(PointRankingData,function (a,b)
			if(a.point > b.point) then
				return true;
			elseif(a.point == b.point) then
				if(a.goalDiff > b.goalDiff) then
					return true;
				elseif(a.goalDiff == b.goalDiff) then
					if(a.goal > b.goal) then
						return true;
					elseif(a.goal == b.goal) then
						-- if() then
						--
						-- elseif() then
						--
						-- end
					end
				end
			end
			return false;
		end);
		return PointRankingData;
	end	
end

function UpdatePlayerData( playerIndex, state, myGoal, myConceded )	
	if(state == 2) then
		PointRankingData[playerIndex].num = PointRankingData[playerIndex].num+1;
		if(myGoal > myConceded) then
			PointRankingData[playerIndex].point = PointRankingData[playerIndex].point+3;
			PointRankingData[playerIndex].win = PointRankingData[playerIndex].win+1;
		elseif(myGoal == myConceded) then
			PointRankingData[playerIndex].point = PointRankingData[playerIndex].point+1;
			PointRankingData[playerIndex].tie = PointRankingData[playerIndex].tie+1;
		else
			PointRankingData[playerIndex].lose = PointRankingData[playerIndex].lose+1;
		end
		PointRankingData[playerIndex].goal = PointRankingData[playerIndex].goal+myGoal;
		PointRankingData[playerIndex].conceded = PointRankingData[playerIndex].conceded+myConceded;
		PointRankingData[playerIndex].goalDiff = PointRankingData[playerIndex].goal - PointRankingData[playerIndex].conceded;
	end
end