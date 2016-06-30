module("UICoachTeamPowerUp",package.seeall);

require "Game/CoachData"

local titleSum = nil;
local content = nil;

local window = nil;
local windowComponent = nil;

function OnStart(gameObject,params)
	window = gameObject;
	windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");

	BindUI();
	SetInfo();
end

function BindUI()
	titleSum = TransformFindChild(window.transform,"Sum");
	content = TransformFindChild(window.transform,"Content");

	local buttonOK = TransformFindChild(window.transform,"ButtonOK");
	local buttonClose = TransformFindChild(window.transform,"ButtonClose");

	AddOrChangeClickParameters(buttonOK.gameObject,Close,nil);
	AddOrChangeClickParameters(buttonClose.gameObject,Close,nil);
end

function SetInfo()
	-- local abilitySum = CoachData.GetCoachTeamAbilitySumUp();
	local abilitySum = CoachData.GetCoachTeamPowerUp();
	UIHelper.SetLabelTxt(titleSum,abilitySum);

	local abilityContent = CoachData.GetCoachTeamAbilityStringTable();
	local abilityString = "";
	for i,v in ipairs(abilityContent) do
		if(i==1)then
			abilityString = abilityString..v;
		else
			abilityString = abilityString.."\n"..v;
		end
	end
	UIHelper.SetLabelTxt(content,abilityString);
end

function Close()
	windowComponent:Close();
end

function OnDestroy()
	titleSum = nil;
	content = nil;
	window = nil;
	windowComponent = nil;
end