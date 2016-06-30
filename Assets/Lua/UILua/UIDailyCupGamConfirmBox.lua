module("UIDailyCupGamConfirmBox",package.seeall);

require "Config"
require "Common/UnityCommonScript"
require "Game/GameMainScript"
require "Game/Role"
require "Game/ItemSys"
require "Game/Hero"

--require "UILua/UIDailyCupGambleBox"

local confirmBox = nil;

local buttonConfirm = nil;
local buttonCancel = nil;
local buttonClose = nil;

local buttonGamTarget = {};

local delegateOnConfirm = nil;
local delegateOnCancel = nil;

function CreateGambleConfirmBox(containerTransform,windowComponent,prefabname,icon,desc,onConfirm,onCancel)
	if(confirmBox == nil) then
		--instantiate one
		local confirmBoxPrefab = windowComponent:GetPrefab(prefabname);
		--instantiate prefab and initialize it
		confirmBox = GameObjectInstantiate(confirmBoxPrefab);
		confirmBox.transform.parent = containerTransform;
    	confirmBox.transform.localPosition = Vector3.zero;
    	confirmBox.transform.localScale = Vector3.one;

    	buttonConfirm = TransformFindChild(confirmBox.transform,"ButtonConfirm");
    	Util.AddClick(buttonConfirm.gameObject,OnClickConfirm);
    	buttonCancel = TransformFindChild(confirmBox.transform,"ButtonCancel");
    	Util.AddClick(buttonCancel.gameObject,OnClickCancel);
    	buttonClose = TransformFindChild(confirmBox.transform,"ButtonClose");
    	Util.AddClick(buttonClose.gameObject,OnClickClose);

    	buttonGamTarget = {};
    	buttonGamTarget.button = TransformFindChild(confirmBox.transform,"ButtonGam");
    	buttonGamTarget.icon = TransformFindChild(buttonGamTarget.button,"IconRoot/Icon");
    	buttonGamTarget.name = TransformFindChild(buttonGamTarget.button,"Name");
    	-- Util.AddClick(buttonGamTarget.button.gameObject,OnClickGamTarget);

	end
	--active it
	if (not GameObjectActiveSelf(confirmBox)) then
    	GameObjectSetActive(confirmBox.transform,true);
    end

	delegateOnConfirm = onConfirm;
	delegateOnCancel = onCancel;

	--set info
	SetInfo(icon,desc);
end

function SetInfo(icon,desc)
	Util.SetUITexture(buttonGamTarget.icon, LuaConst.Const.PlayerHeadIcon, tostring(icon), true);
	UIHelper.SetLabelTxt(buttonGamTarget.name,desc);
end

function OnClickCancel()
	if(delegateOnCancel~=nil)then
		delegateOnCancel();
	end
	Close();
end

function OnClickClose()
	if(delegateOnCancel~=nil)then
		delegateOnCancel();
	end
	Close();
end

function OnClickConfirm()
	if(delegateOnConfirm~=nil)then
		delegateOnConfirm();
	end
end

function Close()
	if(GameObjectActiveSelf(confirmBox)) then
    	GameObjectSetActive(confirmBox.transform,false);
    end
end

function OnDestroy()
	confirmBox = nil;
	buttonConfirm = nil;
	buttonCancel = nil;
	buttonClose = nil;
	buttonGamTarget = {};
	delegateOnConfirm = nil;
	delegateOnCancel = nil;
end