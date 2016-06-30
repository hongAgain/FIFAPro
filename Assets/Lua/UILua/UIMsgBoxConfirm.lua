module("UIMsgBoxConfirm", package.seeall)

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

--1,content{1,content;2,title;3,confirm btn text;4,cancel btn text}; 2,isOKType; 3,delegate yes or confirm; 4,delegate no if necessary
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

            local function OnClickYes ()
                local cb = luaTables[3];
                if (cb ~= nil) then
                        cb();
                end
                --self is button yes;
                self.windowComponent:Close();
            end;

            local function OnClickNo ()
            	local cb = luaTables[4];
                if (cb ~= nil) then
                        cb();
                end
                --self is button no;
                self.windowComponent:Close();
            end;

			local transform = self.window.transform;

			local label = TransformFindChild(transform, "Content");
			local title = TransformFindChild(transform, "Title")
			local btnOK = TransformFindChild(transform, "ButtonOK");
			local btnYes = TransformFindChild(transform, "ButtonYes");
			local btnNo = TransformFindChild(transform, "ButtonNo");
			local btnOkLbl = TransformFindChild(btnOK, "Label")
			local btnYesLbl = TransformFindChild(btnYes, "Label")
			local btnNoLbl = TransformFindChild(btnNo, "Label")
			local closeBtn = TransformFindChild(transform, "ButtonClose")
			UIHelper.SetLabelTxt(btnOkLbl, Util.LocalizeString("Yes"))
			UIHelper.SetLabelTxt(btnYesLbl, Util.LocalizeString("Yes"))
			UIHelper.SetLabelTxt(btnNoLbl, Util.LocalizeString("No"))
			if type(luaTables[1]) == "table" then
				local info = luaTables[1]
				UIHelper.SetLabelTxt(label, info[1])
				if info[2] ~= nil then
					UIHelper.SetLabelTxt(title, info[2])
				end
				if info[3] ~= nil then
					UIHelper.SetLabelTxt(btnOkLbl, info[3])
					UIHelper.SetLabelTxt(btnYesLbl, info[3])
				end
				if info[4] ~= nil then
					UIHelper.SetLabelTxt(btnNoLbl, info[4])
				end
			else
				UIHelper.SetLabelTxt(label, luaTables[1]);
			end

			Util.AddClick(btnOK.gameObject, OnClickOK);

			Util.AddClick(btnYes.gameObject, OnClickYes);

			Util.AddClick(btnNo.gameObject, OnClickNo);
			Util.AddClick(closeBtn.gameObject, OnClickClose)
			AddOrChangeClickParameters(transform.gameObject, OnClickClose, nil)

			if(luaTables[2]==nil or luaTables[2]==true)then
				--ok type
				GameObjectSetActive(btnOK.gameObject,true);
				GameObjectSetActive(btnYes.gameObject,false);
				GameObjectSetActive(btnNo.gameObject,false);
			else
				--yes no type
				GameObjectSetActive(btnOK.gameObject,false);
				GameObjectSetActive(btnYes.gameObject,true);
				GameObjectSetActive(btnNo.gameObject,true);
			end
		end
	};
	mt.__index = mt;

	setmetatable(msgBox, mt);

    instanceTable[gameObject] = msgBox;
	return msgBox;
end
