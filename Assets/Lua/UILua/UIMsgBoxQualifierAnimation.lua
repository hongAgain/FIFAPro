module("UIMsgBoxQualifierAnimation",package.seeall);

require "Common/UnityCommonScript"

local instanceTable = {};
local mt = {__mode = "k" };
setmetatable(instanceTable, mt);

function OnStart(gameObject, luaTables)
	local msgBox = New(gameObject, luaTables);
    msgBox:Init();
end

function OnDestroy(gameObject)
	instanceTable[gameObject] = nil;
end

--luaTables:{iconName, description, youWon, onOver}
function New(gameObject, luaTables)

	local msgBox = { releaseFlag = false, windowComponent = nil };
	
	function msgBox:Init()
		self.windowComponent = GetComponentInChildren(gameObject, "UIBaseWindowLua");			
        local transform = gameObject.transform;

        local animeNode = TransformFindChild(transform,"AnimeNode");

        local icon = TransformFindChild(animeNode,"IconRoot/Icon");
        local desc = TransformFindChild(animeNode,"Description");
    	Util.SetUITexture(icon, LuaConst.Const.CupLadder, luaTables.iconName, true);    
		UIHelper.SetLabelTxt(desc, luaTables.description);

		local topLight = TransformFindChild(animeNode,"TopLight");
		local coverLight = TransformFindChild(animeNode,"IconRoot/CoverLight");
		GameObjectSetActive(topLight, luaTables.youWon);
		GameObjectSetActive(coverLight, luaTables.youWon);

		local delegateOnOver = luaTables.onOver;

		local Close = function ()
			if(delegateOnOver~=nil)then
				delegateOnOver();
			end
            if (self.releaseFlag) then
                return;
            end
			self.windowComponent:Close();
            self.windowComponent = nil;
            self.releaseFlag = true;
		end
		LuaTimer.AddTimer(false, 3, Close);
	end

    instanceTable[gameObject] = msgBox;
	return msgBox;
end