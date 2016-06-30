module("TimerObj", package.seeall)

function new(id, ignoreTimeScale, duration, OnTimer)
	local instance = {};

	local mt =
	{
		mElasped = 0,
		mId = id,
		mIgnoreTimeScale = ignoreTimeScale,

		OnTimePassed = function(self, deltaTime)
			local dur = duration;
			if (duration < 0) then
				dur = -duration;
			end

			self.mElasped = self.mElasped + deltaTime;

			if (self.mElasped >= dur) then
				if (OnTimer ~= nil) then
					OnTimer();
				end

				if duration > 0 then
					return true;
				else
					self.mElasped = self.mElasped - dur;
					return false;
				end
			else
				return false;
			end
		end
	};
	mt.__index = mt;

	setmetatable(instance, mt);

	return instance;
end
