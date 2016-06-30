module("UIMsgBoxConfirmCheckInRule", package.seeall)

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

--1,content; 2,isOKType; 3,delegate yes or confirm; 4,delegate no if necessary
function New(gameObject, luaTables)

	local msgBox = {};
	local mt =
	{
	 	window = nil,
		windowComponent = nil,

		Init = function (self)
			self.window = gameObject;
			self.windowComponent = GetComponentInChildren(self.window, "UIBaseWindowLua");

            local function OnClickClose ()
                --self is button close;
                self.windowComponent:Close();
            end;

            local function OnClickOK ()
                local cb = luaTables[3];
                if (cb ~= nil) then
                        cb();
                end
                --self is button yes;
                self.windowComponent:Close();
            end;

			local transform = self.window.transform;

			local label = TransformFindChild(transform, "Content");
			UIHelper.SetLabelTxt(label, luaTables[1]);

			local btnOK = TransformFindChild(transform, "ButtonOK");
			Util.AddClick(btnOK.gameObject, OnClickOK);

			local btnClose = TransformFindChild(transform, "ButtonClose");
			Util.AddClick(btnClose.gameObject, OnClickClose);
			local bg = TransformFindChild(transform, "BG/BGBlock")
			Util.AddClick(bg.gameObject, OnClickClose)

            GameObjectSetActive(btnOK.gameObject,true);
		end
	};
	mt.__index = mt;

	setmetatable(msgBox, mt);

    instanceTable[gameObject] = msgBox;
	return msgBox;
end
