-- module("UILadderMatchingBox", package.seeall);

-- require "Common/UnityCommonScript"


-- local matchBoxPrefabName = "LadderMatchingBox";
-- local matchBox = nil;

-- local matchBoxPollingData = nil;
-- local matchBoxMatchingState = nil;
-- local matchPollingTimeMax = 30;
-- local matchPollingInterval = 6;
-- local matchPollingCount = nil;

-- local lastPollTimerID = nil;
-- local lastCDTimerID = nil;
-- local currentCDTime = nil;
-- local matchStartLock = nil;

-- local matchTimeCountDown = nil;
-- local matchBoxButtonCancel = nil;

-- -- local delegateOnClickClose = nil;
-- -- local delegateOnClickCancel = nil;
-- local delegateOnMatchSuccess = nil;
-- local delegateOnMatchFailed = nil;

-- function CreateMatchingBox( containerTransform, windowComponent, delegateOnSuccess, delegateOnFailed )
-- 	-- body
-- 	if(matchBox == nil) then
-- 		--instantiate one
-- 		local matchBoxPrefab = windowComponent:GetPrefab(matchBoxPrefabName);
-- 		--instantiate prefab and initialize it
-- 		matchBox = GameObjectInstantiate(matchBoxPrefab);
-- 		matchBox.transform.parent = containerTransform;
--     	matchBox.transform.localPosition = Vector3.zero;
--     	matchBox.transform.localScale = Vector3.one;

--     	-- delegateOnClickClose = delegateOnClose;
--     	-- delegateOnClickCancel = delegateOnCancel;
--     	delegateOnMatchSuccess = delegateOnSuccess;
--     	delegateOnMatchFailed = delegateOnFailed;

--     	matchTimeCountDown = TransformFindChild(matchBox.transform,"TimeCountDown");

--     	matchBoxButtonCancel = TransformFindChild(matchBox.transform,"ButtonCancel");

-- 		Util.AddClick(matchBoxButtonCancel.gameObject,OnClickCancel);

-- 	end

-- 	--active it
-- 	 if(not GameObjectActiveSelf(matchBox)) then
--     	GameObjectSetActive(matchBox.transform,true);
--     end

-- 	--set info
-- 	currentCDTime = matchPollingTimeMax;
-- 	matchStartLock = false;
-- 	matchPollingCount = 0;
--     UIHelper.SetLabelTxt(matchTimeCountDown,"00:30");

--     StartPolling();
--     StartCountDown();
-- end


-- -- =========================== CountDown functions ===========================
-- function StartCountDown()
-- 	lastCDTimerID = LuaTimer.AddTimer(false,-1,UpdateCountDown);
-- end

-- function UpdateCountDown()
-- 	--count down total seconds
-- 	currentCDTime = math.max(0,currentCDTime - 1);
-- 	if(currentCDTime == 0) then
-- 		StopCountDown();
-- 	end
-- 	local currentMinute = string.format("%02d",math.floor(currentCDTime / 60));
-- 	local currentSecond = string.format("%02d",currentCDTime % 60);
-- 	UIHelper.SetLabelTxt(matchTimeCountDown,currentMinute..":"..currentSecond);
-- end

-- function StopCountDown()
-- 	if(lastCDTimerID ~= nil) then
-- 		LuaTimer.RmvTimer(lastCDTimerID);
-- 	end
-- end

-- -- =========================== CountDown functions ===========================


-- -- =========================== polling functions ===========================
-- function StartPolling()
-- 	lastPollTimerID = LuaTimer.AddTimer(false,matchPollingInterval,DoPolling);	
-- end

-- function DoPolling()
-- 	-- print("Debug print ===== DoPolling");

-- 	matchPollingCount = matchPollingCount + 1;
-- 	if(matchPollingCount >= matchPollingTimeMax/matchPollingInterval) then
-- 		--no more polling
-- 		StopPolling();
-- 		-- PVPMsgManager.RequestLadderWait(OnPollingReturn,nil);
-- 		PVPMsgManager.RequestPVPMsg(MsgID.tb.LadderWait, LuaConst.Const.LadderWait, nil, OnPollingReturn, nil );
-- 		return;
-- 	end
-- 	-- the recent state is in queue, continue polling
-- 	if(matchBoxMatchingState == nil or matchBoxMatchingState == 1) then
-- 		-- PVPMsgManager.RequestLadderWait(OnPollingReturn,nil);
-- 		PVPMsgManager.RequestPVPMsg(MsgID.tb.LadderWait, LuaConst.Const.LadderWait, nil, OnPollingReturn, nil );
-- 		StartPolling();
-- 	end
-- end

-- function OnPollingReturn()

-- 	--refresh state
-- 	-- matchBoxPollingData = PVPMsgManager.Get_LadderWaitData();
-- 	matchBoxPollingData = PVPMsgManager.GetPVPData(MsgID.tb.LadderWait);
-- 	matchBoxMatchingState = matchBoxPollingData.state;

-- 	if(matchBoxMatchingState == 2 or matchBoxMatchingState == 3) then
-- 		--start match or finish match
-- 		SetMatchLock(true);
-- 		StopPolling();
-- 		StopCountDown();
-- 		if(delegateOnMatchSuccess~=nil) then
-- 			delegateOnMatchSuccess(matchBoxPollingData);
-- 		end
-- 	elseif(matchBoxMatchingState == 4 or IsFinalPoll()) then
-- 		QuitLadder();
-- 	else
-- 		SetMatchLock(false);
-- 	end

-- end

-- function QuitLadder()
-- 	-- PVPMsgManager.RequestLadderQuit(AfterQuitLadder,QuitLadder);
-- 	PVPMsgManager.RequestPVPMsg(MsgID.tb.LadderQuit, LuaConst.Const.LadderQuit, nil, AfterQuitLadder, QuitLadder );
-- end

-- function AfterQuitLadder()
-- 	SetMatchLock(false);
-- 	CloseMatchingBox();
-- 	-- if(not IsFinalPoll()) then
-- 		if(delegateOnMatchFailed~=nil) then
-- 			delegateOnMatchFailed();
-- 		end
-- 	-- end
-- end

-- function StopPolling()
-- 	if(lastPollTimerID ~= nil) then
-- 		LuaTimer.RmvTimer(lastPollTimerID);
-- 	end
-- end

-- function IsFinalPoll()
-- 	return (matchPollingCount >= matchPollingTimeMax/matchPollingInterval);
-- end
-- -- =========================== polling functions ===========================

-- function SetMatchLock(willLock)
-- 	matchStartLock = willLock;
-- end

-- function IsMatchStarting()
-- 	return matchStartLock;
-- end

-- -- =========================== event functions ===========================
-- -- function OnClickClose()
-- -- 	if(IsMatchStarting()) then
-- -- 		return;
-- -- 	end;
-- -- 	-- CloseMatchingBox();
-- -- 	QuitLadder(false);
-- -- end

-- function OnClickCancel()
-- 	if(IsMatchStarting()) then
-- 		return;
-- 	end;
-- 	-- CloseMatchingBox();
-- 	QuitLadder(false);
-- end
-- -- =========================== event functions ===========================
-- function CloseMatchingBox()
-- 	StopPolling();
-- 	StopCountDown();
-- 	if(GameObjectActiveSelf(matchBox)) then
--     	GameObjectSetActive(matchBox.transform,false);
--     end
-- 	GameObjectDestroy(matchBox);
-- 	ResetToNull();	
-- end

-- function ResetToNull()
-- 	matchBox = nil;
-- 	matchBoxPollingData = nil;
-- 	matchBoxMatchingState = nil;
-- 	matchPollingTimeMax = 30;
-- 	matchPollingInterval = 6;
-- 	matchPollingCount = nil;
-- 	lastPollTimerID = nil;
-- 	lastCDTimerID = nil;
-- 	currentCDTime = nil;
-- 	matchStartLock = nil;
-- 	matchTimeCountDown = nil;
-- 	matchBoxButtonCancel = nil;
-- 	delegateOnMatchSuccess = nil;
-- 	delegateOnMatchFailed = nil;
-- end

-- function OnDestroy()
-- 	StopPolling();
-- 	StopCountDown();
-- 	ResetToNull();
-- end