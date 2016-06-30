module("UIMsgBoxScrollText", package.seeall)

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

--1,content; 2,delegate confirm;
function New(gameObject, luaTables)

	local msgBox = {};
	local mt =
	{
	 	window = nil,
		windowComponent = nil,

		Init = function (self)
			self.window = gameObject;
			self.windowComponent = GetComponentInChildren(self.window, "UIBaseWindowLua");

            local function OnClickOK ()
                local cb = luaTables[2];
                if (cb ~= nil) then
					cb();
                end
                self.windowComponent:Close();
            end;

			local transform = self.window.transform;

			local label = TransformFindChild(transform, "ScrollView/Content");
			UIHelper.SetLabelTxt(label, luaTables[1]);

			local btnOK = TransformFindChild(transform, "ButtonOK");
			Util.AddClick(btnOK.gameObject, OnClickOK);			
			
			GameObjectSetActive(btnOK.gameObject,true);
		end
	};
	mt.__index = mt;
	
	setmetatable(msgBox, mt);

    instanceTable[gameObject] = msgBox;
	return msgBox;
end