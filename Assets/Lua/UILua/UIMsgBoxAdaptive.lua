module("UIMsgBoxAdaptive",package.seeall);

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

--1，content prefab 2,isOKType 3，delegate close 4，delegate ok 5，delegate yes 6，delegate no
function New(gameObject, luaTables)

	local msgBox = {};
	local mt =
	{
	 	window = nil,
		windowComponent = nil,

		Init = function (self)
			self.window = gameObject;
			self.windowComponent = GetComponentInChildren(self.window, "UIBaseWindowLua");            

			local OnClickClose = function ()
				if (luaTables[3] ~= nil) then
					luaTables[3]();
                end
				self.windowComponent:Close();
			end

			local OnClickOK = function ()
                if (luaTables[4] ~= nil) then
					luaTables[4]();
                end
				self.windowComponent:Close();
			end

			local OnClickYes = function ()
				if (luaTables[5] ~= nil) then
					luaTables[5]();
                end
				self.windowComponent:Close();
			end

			local OnClickNo = function ()
				if (luaTables[6] ~= nil) then
					luaTables[6]();
                end
				self.windowComponent:Close();
			end

			local transform = self.window.transform;
			local contentRoot = TransformFindChild(transform, "ContentRoot");
			local buttonClose = TransformFindChild(transform,"ButtonClose");
			local buttonOK = TransformFindChild(transform,"ButtonOK");
			local buttonYes = TransformFindChild(transform,"ButtonYes");
			local buttonNo = TransformFindChild(transform,"ButtonNo");
			AddOrChangeClickParameters(buttonClose.gameObject,OnClickClose,nil);
			AddOrChangeClickParameters(buttonOK.gameObject,OnClickOK,nil);
			AddOrChangeClickParameters(buttonYes.gameObject,OnClickYes,nil);
			AddOrChangeClickParameters(buttonNo.gameObject,OnClickNo,nil);

			if(luaTables[1]~=nil)then
				GameObjectSetActive(luaTables[1].transform,true);
				luaTables[1].transform.parent = contentRoot;
			end		

			if(luaTables[2]==true)then
				--ok type
				GameObjectSetActive(buttonOK.gameObject,true);
				GameObjectSetActive(buttonYes.gameObject,false);
				GameObjectSetActive(buttonNo.gameObject,false);
			else
				--yes no type
				GameObjectSetActive(buttonOK.gameObject,false);
				GameObjectSetActive(buttonYes.gameObject,true);
				GameObjectSetActive(buttonNo.gameObject,true);
			end	
		end
	};
	mt.__index = mt;	
	setmetatable(msgBox, mt);
    instanceTable[gameObject] = msgBox;
	return msgBox;
end