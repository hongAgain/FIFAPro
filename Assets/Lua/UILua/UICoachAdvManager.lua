module("UICoachAdvManager",package.seeall);

require "Game/CoachData"

coachAdvSettings = {
	prefabName = "AdvNode",
	prefab = nil,

	normalAvatarPosition = NewVector3(-250,81,0),
	finalAvatarPosition = NewVector3(0,81,0),
}

local rootNode = nil;
local windowComponent = nil;

local coachAdvNode = nil;

local coachAvatarFormer = {};
local coachAvatarLatter = {};

local advArrow = nil;
local resNeed = nil;
local resLeft = nil;
local buttonAdv = nil;

local currentCoachID = nil;

local msgRegisterKey = nil;


function Initialize(rootnode,windowcomponent)
	rootNode = rootnode;
	windowComponent = windowcomponent;
end

function ShowCoachAdv(willShow,coachid)
	if(coachid == nil or not willShow)then
		--disactivate it
		if(coachAdvNode ~= nil)then
			GameObjectSetActive(coachAdvNode.transform,false);
		end
	else
		--show it 
		if(coachAdvNode == nil)then
			--instantiate one
			if(coachAdvSettings.prefab==nil)then
				coachAdvSettings.prefab = windowComponent:GetPrefab(coachAdvSettings.prefabName);
			end
			--instantiate prefab and initialize it
			coachAdvNode = GameObjectInstantiate(coachAdvSettings.prefab);
			coachAdvNode.transform.parent = rootNode;
	    	coachAdvNode.transform.localPosition = Vector3.zero;
	    	coachAdvNode.transform.localScale = Vector3.one;

			BindUI();

			RegisterMsg();
		end

		if(coachAdvNode ~= nil)then
			GameObjectSetActive(coachAdvNode.transform,true);
		end
		currentCoachID = coachid;
		SetInfo();
	end
end

function BindUI()
	coachAvatarFormer = {};
	coachAvatarFormer.transform = TransformFindChild(coachAdvNode.transform,"CoachAvatarBefore");
	coachAvatarFormer.gameObject = coachAvatarFormer.transform.gameObject;
	coachAvatarFormer.icon = TransformFindChild(coachAvatarFormer.transform,"IconRoot/Icon");
	coachAvatarFormer.rank = TransformFindChild(coachAvatarFormer.transform,"RankRoot/Rank");
	coachAvatarFormer.rankColor = TransformFindChild(coachAvatarFormer.transform,"RankColor");
	coachAvatarFormer.BGRankColor = TransformFindChild(coachAvatarFormer.transform,"BGRankColor");
	coachAvatarFormer.name = TransformFindChild(coachAvatarFormer.transform,"NameNode/Name");
	coachAvatarFormer.reputation = TransformFindChild(coachAvatarFormer.transform,"NameNode/Repu");
	coachAvatarFormer.ability = TransformFindChild(coachAvatarFormer.transform,"Buff");
	coachAvatarFormer.skill = TransformFindChild(coachAvatarFormer.transform,"Skill");

	coachAvatarLatter = {};
	coachAvatarLatter.transform = TransformFindChild(coachAdvNode.transform,"CoachAvatarAfter");
	coachAvatarLatter.gameObject = coachAvatarLatter.transform.gameObject;
	coachAvatarLatter.icon = TransformFindChild(coachAvatarLatter.transform,"IconRoot/Icon");
	coachAvatarLatter.rank = TransformFindChild(coachAvatarLatter.transform,"RankRoot/Rank");
	coachAvatarLatter.rankColor = TransformFindChild(coachAvatarLatter.transform,"RankColor");
	coachAvatarLatter.BGRankColor = TransformFindChild(coachAvatarLatter.transform,"BGRankColor");
	coachAvatarLatter.name = TransformFindChild(coachAvatarLatter.transform,"NameNode/Name");
	coachAvatarLatter.reputation = TransformFindChild(coachAvatarLatter.transform,"NameNode/Repu");
	coachAvatarLatter.ability = TransformFindChild(coachAvatarLatter.transform,"Buff");
	coachAvatarLatter.skill = TransformFindChild(coachAvatarLatter.transform,"Skill");

	advArrow = TransformFindChild(coachAdvNode.transform,"DevideLine");
	resNeed = TransformFindChild(coachAdvNode.transform,"ResNeed");
	resLeft = TransformFindChild(coachAdvNode.transform,"AdvRes");
	buttonAdv = TransformFindChild(coachAdvNode.transform,"ButtonConfirmAdv");
	AddOrChangeClickParameters(buttonAdv.gameObject,OnClickButtonAdv,nil);

	--unwanted
	GameObjectSetActive(coachAvatarFormer.skill,false);
	GameObjectSetActive(coachAvatarLatter.skill,false);
end

function SetInfo()
	local advInfoData = CoachData.GetCoachAdvInfo(currentCoachID);

	local coachConfigData = CoachData.GetCoachConfigData(currentCoachID);
	local coachUserData = CoachData.GetCoachUserData(currentCoachID);
	if(coachUserData~=nil)then
		--set avatar former
		Util.SetUITexture(coachAvatarFormer.icon,LuaConst.Const.CoachHeadIcon,coachConfigData.head,true);
		UIHelper.SetSpriteName(coachAvatarFormer.rank,coachConfigData.rating);
		UIHelper.SetLabelTxt(coachAvatarFormer.name,CoachData.GetCoachNameWithSuffix(currentCoachID));

		UIHelper.SetWidgetColor(coachAvatarFormer.rankColor,CoachData.GetCoachRankColor(currentCoachID));
		UIHelper.SetWidgetColor(coachAvatarFormer.BGRankColor,CoachData.GetCoachBGRankColor(currentCoachID));
		UIHelper.SetWidgetColor(coachAvatarFormer.name,CoachData.GetCoachNameColor(currentCoachID));
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
		UIHelper.SetLabelTxt(coachAvatarFormer.ability,abilityString);
		UIHelper.SetLabelTxt(coachAvatarFormer.reputation,CoachData.GetCoachReputationIs(currentCoachID));
		-- UIHelper.SetLabelTxt(coachAvatarFormer.skill,"紧张开发中，敬请期待");

		if(CoachData.IsCoachAdvMax(currentCoachID))then
			GameObjectSetActive(coachAvatarLatter.transform,false);
			GameObjectSetActive(advArrow,false);
			GameObjectSetActive(resNeed,false);
			SetButtonActive(false);
			coachAvatarFormer.transform.localPosition = coachAdvSettings.finalAvatarPosition;

		else
			GameObjectSetActive(coachAvatarLatter.transform,true);
			GameObjectSetActive(advArrow,true);
			GameObjectSetActive(resNeed,true);
			SetButtonActive(true);
			coachAvatarFormer.transform.localPosition = coachAdvSettings.normalAvatarPosition;

			--set avatar latter
			Util.SetUITexture(coachAvatarLatter.icon,LuaConst.Const.CoachHeadIcon,coachConfigData.head,true);
			UIHelper.SetSpriteName(coachAvatarLatter.rank,coachConfigData.rating);
			UIHelper.SetLabelTxt(coachAvatarLatter.name,CoachData.GetCoachNameWithSuffix(currentCoachID,coachUserData.adv+1));
			
			UIHelper.SetWidgetColor(coachAvatarLatter.rankColor,CoachData.GetCoachRankColor(currentCoachID,coachUserData.adv+1));
			UIHelper.SetWidgetColor(coachAvatarLatter.BGRankColor,CoachData.GetCoachBGRankColor(currentCoachID,coachUserData.adv+1));
			UIHelper.SetWidgetColor(coachAvatarLatter.name,CoachData.GetCoachNameColor(currentCoachID,coachUserData.adv+1));
			local abilityTable = CoachData.GetCoachAbilityTable(currentCoachID,nil,coachUserData.adv+1);
			local abilityString = "";
			for i,v in ipairs(abilityTable) do
				abilityString = abilityString..v;
				if(i%2==0)then
					abilityString = abilityString.. "\n";
				else
					abilityString = abilityString.. " ";
				end
			end
			UIHelper.SetLabelTxt(coachAvatarLatter.ability,abilityString);
			UIHelper.SetLabelTxt(coachAvatarLatter.reputation,CoachData.GetCoachReputationIs(currentCoachID,coachUserData.adv+1));
			-- UIHelper.SetLabelTxt(coachAvatarLatter.skill,"紧张开发中，敬请期待");

			-- {
			-- 	lvneed = advinfo.lv_need, 
			-- 	name = ItemSys.GetItemName(advinfo.sub[1]);, 
			-- 	id = advinfo.sub[1], 
			-- 	num = advinfo.sub[2], 
			-- 	sum = ItemSys.GetItemData(advinfo.sub[1]).num;
			-- };
			UIHelper.SetLabelTxt(resNeed,advInfoData.name.."[66CCFF]"..advInfoData.num);
		end
		UIHelper.SetLabelTxt(resLeft,advInfoData.name.."[66CCFF]"..advInfoData.sum);
	else
		print("data error: you don't own this coach:"..currentCoachID);
	end
end

function SetButtonActive(willActivate)
	UIHelper.EnableBtn(buttonAdv.gameObject,willActivate);
end

function OnClickButtonAdv()
	--req adv msg
	CoachData.RequestMsg( MsgID.tb.CoachAdv, LuaConst.Const.CoachAdv, {id = currentCoachID} );
end

function OnReqAdv()
	-- refresh ui with currentCoachID
	SetInfo();
end

function RegisterMsg()
	msgRegisterKey = CoachData.RegisterDelegatesOnReqSuccess( MsgID.tb.CoachAdv, OnReqAdv );
end

function UnregisterMsg()
	if(msgRegisterKey~=nil)then
		CoachData.UnregisterDelegatesOnReqSuccess( MsgID.tb.CoachAdv, msgRegisterKey );
		msgRegisterKey = nil;
	end
end

function OnDestroy()
	UnregisterMsg();

	coachAdvSettings.prefab = nil;
	rootNode = nil;
	windowComponent = nil;
	coachAdvNode = nil;
	coachAvatarFormer = {};
	coachAvatarLatter = {};
	advArrow = nil;
	resNeed = nil;
	resLeft = nil;
	buttonAdv = nil;
	currentCoachID = nil;
	msgRegisterKey = nil;
end