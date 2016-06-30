module("UIMsgBoxHint", package.seeall)

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

function New(gameObject, luaTables)

	local msgBox = { releaseFlag = false, windowComponent = nil };
	
	function msgBox:Init()
		self.windowComponent = GetComponentInChildren(gameObject, "UIBaseWindowLua");
			
        local transform = gameObject.transform;

		local label = TransformFindChild(transform, "Label");
		UIHelper.SetLabelTxt(label, luaTables[1]);

		local Close = function ()
            if (self.releaseFlag) then
                return;
            end

			self.windowComponent:Close();

            self.windowComponent = nil;
            self.releaseFlag = true;
		end
		AddOrChangeClickParameters(gameObject, Close, nil)
		LuaTimer.AddTimer(false, 3, Close);
	end

    instanceTable[gameObject] = msgBox;
	return msgBox;
end