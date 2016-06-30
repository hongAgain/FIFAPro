module("UICoachDoUManager",package.seeall);

require "Game/CoachData"

coachDoUSettings = {
	prefabName = "DoUNode",
	prefab = nil,

	expItemBasicID = "20021",
	expItemAdvancedID = "20022",
	expItemEpicID = "20023",
}

local rootNode = nil;
local windowComponent = nil;

local coachDoUNode = nil;

local icon = nil;
local rank = nil;
local rankColor = nil;
local bgRankColor = nil;
local name = nil;
local reputation = nil;
local ability = nil;
local skill = nil;

local currentDoUCircle = nil;
local currentDoUNum = nil;
local buttonUseItemBasic = {};
local buttonUseItemAdvanced = {};
local buttonUseItemEpic = {};

local currentCoachID = nil;

function Initialize(rootnode,windowcomponent)
	rootNode = rootnode;
	windowComponent = windowcomponent;
end

function ShowCoachDoU(willShow,coachid)
	if(coachid == nil or not willShow)then
		--disactivate it
		if(coachDoUNode ~= nil)then
			GameObjectSetActive(coachDoUNode.transform,false);
		end
	else
		--show it 
		if(coachDoUNode == nil)then
			--instantiate one
			if(coachDoUSettings.prefab==nil)then
				coachDoUSettings.prefab = windowComponent:GetPrefab(coachDoUSettings.prefabName);
			end
			--instantiate prefab and initialize it
			coachDoUNode = GameObjectInstantiate(coachDoUSettings.prefab);
			coachDoUNode.transform.parent = rootNode;
	    	coachDoUNode.transform.localPosition = Vector3.zero;
	    	coachDoUNode.transform.localScale = Vector3.one;

			BindUI();

			RegisterMsg();
		end
		if(coachDoUNode ~= nil)then
			GameObjectSetActive(coachDoUNode.transform,true);
		end
		currentCoachID = coachid;
		SetInfo();
	end
end

function BindUI()
	icon = TransformFindChild(coachDoUNode.transform,"IconRoot/Icon");
	rank = TransformFindChild(coachDoUNode.transform,"RankRoot/Rank");
	rankColor = TransformFindChild(coachDoUNode.transform,"RankColor");
	bgRankColor = TransformFindChild(coachDoUNode.transform,"BGRankColor");
	name = TransformFindChild(coachDoUNode.transform,"Name");
	reputation = TransformFindChild(coachDoUNode.transform,"Repu");
	ability = TransformFindChild(coachDoUNode.transform,"Buff");
	skill = TransformFindChild(coachDoUNode.transform,"Skill");
	GameObjectSetActive(skill,false);


	currentDoUCircle = TransformFindChild(coachDoUNode.transform,"DoUProgress");
	currentDoUNum = TransformFindChild(currentDoUCircle,"Progress");
	buttonUseItemBasic = {};
	buttonUseItemBasic.transform = TransformFindChild(coachDoUNode.transform,"ButtonBasicExp");
	buttonUseItemBasic.gameObject = buttonUseItemBasic.transform.gameObject;
	buttonUseItemBasic.num = TransformFindChild(buttonUseItemBasic.transform,"Num");
	buttonUseItemAdvanced = {};
	buttonUseItemAdvanced.transform = TransformFindChild(coachDoUNode.transform,"ButtonAdvancedExp");
	buttonUseItemAdvanced.gameObject = buttonUseItemAdvanced.transform.gameObject;
	buttonUseItemAdvanced.num = TransformFindChild(buttonUseItemAdvanced.transform,"Num");
	buttonUseItemEpic = {};
	buttonUseItemEpic.transform = TransformFindChild(coachDoUNode.transform,"ButtonEpicExp");
	buttonUseItemEpic.gameObject = buttonUseItemEpic.transform.gameObject;
	buttonUseItemEpic.num = TransformFindChild(buttonUseItemEpic.transform,"Num");
	AddOrChangeClickParameters(buttonUseItemBasic.gameObject,OnClickUseExpItem,{itemid = coachDoUSettings.expItemBasicID});
	AddOrChangeClickParameters(buttonUseItemAdvanced.gameObject,OnClickUseExpItem,{itemid = coachDoUSettings.expItemAdvancedID});
	AddOrChangeClickParameters(buttonUseItemEpic.gameObject,OnClickUseExpItem,{itemid = coachDoUSettings.expItemEpicID});
end

function SetInfo()
	SetCoachInfo();
	SetExpItemInfo();
end

function SetCoachInfo()
	local coachConfigData = CoachData.GetCoachConfigData(currentCoachID);
	local coachUserData = CoachData.GetCoachUserData(currentCoachID);

	Util.SetUITexture(icon,LuaConst.Const.CoachHeadIcon,coachConfigData.head,true);
	UIHelper.SetSpriteName(rank,coachConfigData.rating);
	UIHelper.SetLabelTxt(name,CoachData.GetCoachNameWithSuffix(currentCoachID));
	
	UIHelper.SetWidgetColor(rankColor,CoachData.GetCoachRankColor(currentCoachID));
	UIHelper.SetWidgetColor(bgRankColor,CoachData.GetCoachBGRankColor(currentCoachID));
	UIHelper.SetWidgetColor(name,CoachData.GetCoachNameColor(currentCoachID));

	local DoUPercent = CoachData.GetCoachDoUPercent(currentCoachID);
	UIHelper.SetSpriteFillAmount(currentDoUCircle,DoUPercent);
	UIHelper.SetLabelTxt(currentDoUNum,string.format("%.1f",DoUPercent*100).."%");

	local abilityTable = CoachData.GetCoachAbilityTable(currentCoachID);
	local abilityString = "";
	for i,v in ipairs(abilityTable) do
		abilityString = abilityString..v;
		if(i%2==0)then
			abilityString = abilityString.. "\n";
		else
			abilityString = abilityString.. " ";
		end
	end
	UIHelper.SetLabelTxt(ability,abilityString);
	UIHelper.SetLabelTxt(reputation,CoachData.GetCoachReputation(currentCoachID));
	-- UIHelper.SetLabelTxt(skill,"紧张开发中，敬请期待");
end

function SetExpItemInfo()
	local basicItemData = ItemSys.GetItemData(coachDoUSettings.expItemBasicID);
	UIHelper.SetLabelTxt(buttonUseItemBasic.num,"x"..basicItemData.num);
	UIHelper.EnableBtn(buttonUseItemBasic.gameObject,(basicItemData.num>0));

	local advancedItemData = ItemSys.GetItemData(coachDoUSettings.expItemAdvancedID)
	UIHelper.SetLabelTxt(buttonUseItemAdvanced.num,"x"..advancedItemData.num);
	UIHelper.EnableBtn(buttonUseItemAdvanced.gameObject,(advancedItemData.num>0));

	local epicItemData = ItemSys.GetItemData(coachDoUSettings.expItemEpicID)
	UIHelper.SetLabelTxt(buttonUseItemEpic.num,"x"..epicItemData.num);
	UIHelper.EnableBtn(buttonUseItemEpic.gameObject,(epicItemData.num>0));	
end

function OnClickUseExpItem(go)
	local listener = UIHelper.GetUIEventListener(go);
	if(listener~=nil and listener.parameter~=nil )then
		ItemSys.UseItem(listener.parameter.itemid,1,currentCoachID);
	end
end

function OnUpdateCoach()
	SetCoachInfo();
end

function OnUpdateItem()
	SetExpItemInfo();
end

function RegisterMsg()
	SynSys.RegisterCallback("item", OnUpdateItem);
	SynSys.RegisterCallback("coach", OnUpdateCoach);
end

function UnregisterMsg()	
    SynSys.UnRegisterCallback("item", OnUpdateItem);
    SynSys.UnRegisterCallback("coach", OnUpdateCoach);
end

function OnDestroy()
	UnregisterMsg();
	coachDoUSettings.prefab = nil;
	rootNode = nil;
	windowComponent = nil;
	coachDoUNode = nil;
	icon = nil;
	rank = nil;
	rankColor = nil;
	bgRankColor = nil;
	name = nil;
	reputation = nil;
	ability = nil;
	skill = nil;
	currentDoUCircle = nil;
	currentDoUNum = nil;
	buttonUseItemBasic = {};
	buttonUseItemAdvanced = {};
	buttonUseItemEpic = {};
	currentCoachID = nil;
end