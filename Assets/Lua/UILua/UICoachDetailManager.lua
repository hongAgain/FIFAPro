module("UICoachDetailManager",package.seeall);

require "Game/CoachData"

local coachDetailSettings = {
	prefabName = "DetailNode",
	prefab = nil,
}

local rootNode = nil;
local windowComponent = nil;
local delegateOnClose = nil;

local coachDetailNode = nil;

local coachIcon = nil;
local coachRank = nil;
local coachRankColor = nil;
local coachBGRankColor = nil;

local coachName = nil;
local coachDescription = nil;
local coachReputation = nil;
local coachDoU = nil;
local coachBuff = nil;
local coachSkill = nil;
local coachPos = nil;

local buttonConfirm = nil;

function Initialize(rootnode,windowcomponent,onclose)
	rootNode = rootnode;
	windowComponent = windowcomponent;
	delegateOnClose = onclose;
end

function ShowCoachDetail(willShow,coachid)
	if(coachid == nil or not willShow)then
		--disactivate it
		if(coachDetailNode ~= nil)then
			GameObjectSetActive(coachDetailNode.transform,false);
		end
	else
		--show it 
		if(coachDetailNode == nil)then
			--instantiate one
			if(coachDetailSettings.prefab==nil)then
				coachDetailSettings.prefab = windowComponent:GetPrefab(coachDetailSettings.prefabName);
			end
			--instantiate prefab and initialize it
			coachDetailNode = GameObjectInstantiate(coachDetailSettings.prefab);
			coachDetailNode.transform.parent = rootNode;
	    	coachDetailNode.transform.localPosition = Vector3.zero;
	    	coachDetailNode.transform.localScale = Vector3.one;

			BindUI();
		end
		if(coachDetailNode ~= nil)then
			GameObjectSetActive(coachDetailNode.transform,true);
		end
		SetInfo(coachid);
	end
end

function BindUI()
	coachIcon = TransformFindChild(coachDetailNode.transform,"IconRoot/Icon");
	coachRank = TransformFindChild(coachDetailNode.transform,"RankRoot/Rank");
	coachRankColor = TransformFindChild(coachDetailNode.transform,"RankColor");
	coachBGRankColor = TransformFindChild(coachDetailNode.transform,"BGRankColor");

	coachName = TransformFindChild(coachDetailNode.transform,"Name");
	coachDescription = TransformFindChild(coachDetailNode.transform,"Description");
	coachReputation = TransformFindChild(coachDetailNode.transform,"Repu");
	coachDoU = TransformFindChild(coachDetailNode.transform,"DoU");
	coachBuff = TransformFindChild(coachDetailNode.transform,"Buff");
	coachSkill = TransformFindChild(coachDetailNode.transform,"Skill");
	coachPos = TransformFindChild(coachDetailNode.transform,"Pos");
	GameObjectSetActive(coachSkill,false);

	buttonConfirm = TransformFindChild(coachDetailNode.transform,"ButtonConfirm");
	AddOrChangeClickParameters(buttonConfirm.gameObject,OnClickButtonConfirm,nil);
end

function SetInfo(coachid)
	local coachHeroData = CoachData.GetCoachUserData(coachid);
	local coachConfigData = CoachData.GetCoachConfigData(coachid);

	Util.SetUITexture(coachIcon, LuaConst.Const.CoachHeadIcon, coachConfigData.head, true);

	UIHelper.SetSpriteName(coachRank,coachConfigData.rating);
	UIHelper.SetWidgetColor(coachRankColor,CoachData.GetCoachRankColor(coachid));
	UIHelper.SetWidgetColor(coachBGRankColor,CoachData.GetCoachBGRankColor(coachid));

	UIHelper.SetLabelTxt(coachName,CoachData.GetCoachNameWithSuffix(coachid));
	UIHelper.SetWidgetColor(coachName,CoachData.GetCoachNameColor(coachid));
	UIHelper.SetLabelTxt(coachDescription,CoachData.GetCoachDescription(coachid));
	UIHelper.SetLabelTxt(coachDoU,string.format("%.1f",CoachData.GetCoachDoUPercent(coachid)*100).."%");
	local abilityTable = CoachData.GetCoachAbilityTable(coachid);
	local abilityString = "";
	for i,v in ipairs(abilityTable) do
		abilityString = abilityString..v;
		if(i%3==0)then
			abilityString = abilityString.. "\n";
		else
			abilityString = abilityString.. " ";
		end
	end
	UIHelper.SetLabelTxt(coachBuff,abilityString);
	UIHelper.SetLabelTxt(coachReputation,CoachData.GetCoachReputation(coachid));
	UIHelper.SetLabelTxt(coachPos,CoachData.GetCoachPosName(coachid));
	-- UIHelper.SetLabelTxt(coachSkill,"紧张开发中，敬请期待");
end

function OnClickButtonConfirm()
	if(delegateOnClose~=nil)then
		delegateOnClose();
	end
end

function OnDestroy()
	coachDetailSettings.prefab = nil
	rootNode = nil;
	windowComponent = nil;
	delegateOnClose = nil;
	coachDetailNode = nil;
	coachIcon = nil;
	coachRank = nil;
	coachRankColor = nil;
	coachBGRankColor = nil;
	coachName = nil;
	coachReputation = nil;
	coachDescription = nil;
	coachDoU = nil;
	coachBuff = nil;
	coachSkill = nil;
	coachPos = nil;
	buttonConfirm = nil;
end