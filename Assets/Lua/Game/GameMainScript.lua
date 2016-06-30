module("GameMainScript", package.seeall)

require("Common/UnityCommonScript")

gameAppGo = nil;
instance = nil;
dataSystem = nil;

function OnStart()
	gameAppGo = GameObjectFind("GameMain");
	instance = gameAppGo:GetComponent("GameMain");
	dataSystem = instance.DataSystem;
	print("GameMain: OnStart");
 --   LogManager:RedLog("11111{0}","aaaa");
end

function SetState(state)
	print("GameMain: SetState" .. state);
end
