module("LuaTimer", package.seeall)
require('Common/TimerObj')

local activeTimers = {};
setmetatable(activeTimers, {__mode = "k"});

local timerId = 0;
local mTime = 0;	--Time.time
local mRealTimeSinceStartup = 0; --Time.realTimeSinceStart


function Update(time, realTimeSinceStartup)
	
	local delta = time - mTime;
	local realDelta = realTimeSinceStartup - mRealTimeSinceStartup;

	for k, v in pairs(activeTimers) do
		local rmv = false;
		if (v.mIgnoreTimeScale) then
			rmv = v:OnTimePassed(realDelta);
		else
			rmv = v:OnTimePassed(delta);
		end

		if rmv then
			activeTimers[k] = nil;
		end
	end

	
	mTime = time;
	mRealTimeSinceStartup = realTimeSinceStartup;
end

function AddTimer(ignoreTimeScale, duration, OnTimer)
	timerId = tostring(timerId+1);
	newTimer = TimerObj.new(timerId, ignoreTimeScale, duration, OnTimer);
	activeTimers[timerId] = newTimer;
	return timerId;
end

function RmvTimer(id)
    activeTimers[id] = nil;
end