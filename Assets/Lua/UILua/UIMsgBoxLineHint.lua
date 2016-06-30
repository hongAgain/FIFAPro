module("UIMsgBoxLineHint", package.seeall)

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

	local msgBox = {};
	local mt =
	{
	 	window = nil,
		windowComponent = nil,

		Init = function (self)
			self.window = gameObject;
			self.windowComponent = GetComponentInChildren(self.window, "UIBaseWindowLua");            

			local transform = self.window.transform;
			local label = TransformFindChild(transform, "Content");
			UIHelper.SetLabelTxt(label, luaTables[1]);

			local Close = function ()	            
				self.windowComponent:Close();
			end

			LuaTimer.AddTimer(false, 2, Close);
		end
	};
	mt.__index = mt;
	
	setmetatable(msgBox, mt);

    instanceTable[gameObject] = msgBox;
	return msgBox;

end