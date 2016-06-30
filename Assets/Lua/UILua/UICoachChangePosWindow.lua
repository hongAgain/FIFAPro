module("UICoachChangePosWindow",package.seeall);

require "Game/CoachData"

local avatarPrefabName = "CoachChangeListAvatar";
local avatarPrefab = nil;

local buttonClose = nil;
local buttonChange = nil;
local coachAvatar = nil;
local coachEmpty = nil;

local coachListAvatars = {};

local uiScrollView = nil;
local uiContainer = nil;

local window = nil;
local windowComponent = nil;

local targetPosID = nil;
local targetCoachID = nil;
local targetCoachAvatarIndex = nil;

local msgRegisteredKey = nil;

function OnStart(gameObject,params)

	RegisterMsg();
	window = gameObject;
	windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");

	targetPosID = params.posID;

	BindUI();
	SetInfo();
end

function BindUI()
	buttonClose = TransformFindChild(window.transform,"ButtonClose");
	buttonChange = TransformFindChild(window.transform,"ButtonChange");

	coachEmpty = TransformFindChild(window.transform,"CoachEmpty");
	coachAvatar = {};
	coachAvatar.transform = TransformFindChild(window.transform,"CoachAvatar");
	coachAvatar.gameObject = coachAvatar.transform.gameObject;
	coachAvatar.name = TransformFindChild(coachAvatar.transform,"Name");
	coachAvatar.icon = TransformFindChild(coachAvatar.transform,"IconRoot/Icon");
	
	coachAvatar.rank = TransformFindChild(coachAvatar.transform,"RankRoot/Rank");
	coachAvatar.rankColor = TransformFindChild(coachAvatar.transform,"RankColor");
	coachAvatar.bgRankColor = TransformFindChild(coachAvatar.transform,"BGRankColor");
	
	coachAvatar.ability = TransformFindChild(coachAvatar.transform,"Ability");
	coachAvatar.reputation = TransformFindChild(coachAvatar.transform,"Reputation");
	coachAvatar.dou = TransformFindChild(coachAvatar.transform,"DoU");
	coachAvatar.skill = TransformFindChild(coachAvatar.transform,"Skill");
	GameObjectSetActive(coachAvatar.skill,false);

	uiScrollView = TransformFindChild(window.transform,"uiScrollView");
	uiContainer = TransformFindChild(uiScrollView.transform,"uiContainer");

	AddOrChangeClickParameters(buttonClose.gameObject,OnClickButtonClose,nil);
	AddOrChangeClickParameters(buttonChange.gameObject,OnClickButtonChangePos,nil);
end

function SetInfo()

	--delete old avatars
	DestroyUIListItemGameObjects(coachListAvatars);

	--prepare data
	local coachListToCreate = GetOptionCoachList();
	if(not IsTableEmpty(coachListToCreate))then
		GameObjectSetActive(coachAvatar.transform,true);
		GameObjectSetActive(coachEmpty.transform,false);		
		--check prefab
		if(avatarPrefab == nil)then
			avatarPrefab = windowComponent:GetPrefab(avatarPrefabName);
		end
		--create new avatars
		CreateUIListItemGameObjects(uiContainer, coachListToCreate, avatarPrefab, OnInitializeCoachListAvatar);
		UIHelper.RepositionGrid(uiContainer);
		UIHelper.RefreshPanel(uiScrollView);

		--select the first coach in the list
		SelectCoach(coachListToCreate[1].id,1);
		UIHelper.SetButtonActive(buttonChange,true,true);
	else
		GameObjectSetActive(coachEmpty.transform,true);
		GameObjectSetActive(coachAvatar.transform,false);
		UIHelper.SetButtonActive(buttonChange,false,true);
	end
end

function OnInitializeCoachListAvatar( randomIndex, key, value, cloneGameObject )
	coachListAvatars[randomIndex] = {};
	coachListAvatars[randomIndex].gameObject = cloneGameObject;
	coachListAvatars[randomIndex].transform = cloneGameObject.transform;
	coachListAvatars[randomIndex].transform.localPosition = Vector3.zero;
	coachListAvatars[randomIndex].gameObject.name = string.format("%03d",tonumber(key));
	--bind ui
	coachListAvatars[randomIndex].Icon = TransformFindChild(coachListAvatars[randomIndex].transform,"IconRoot/Icon");
	coachListAvatars[randomIndex].Rank = TransformFindChild(coachListAvatars[randomIndex].transform,"RankRoot/Rank");
	coachListAvatars[randomIndex].RankColor = TransformFindChild(coachListAvatars[randomIndex].transform,"RankColor");
	coachListAvatars[randomIndex].BGRankColor = TransformFindChild(coachListAvatars[randomIndex].transform,"BGRankColor");
	coachListAvatars[randomIndex].Name = TransformFindChild(coachListAvatars[randomIndex].transform,"Name");
	coachListAvatars[randomIndex].Reputation = TransformFindChild(coachListAvatars[randomIndex].transform,"Repu");
	coachListAvatars[randomIndex].PosIcon = TransformFindChild(coachListAvatars[randomIndex].transform,"PosRoot/Pos");
	coachListAvatars[randomIndex].SelectedNode = TransformFindChild(coachListAvatars[randomIndex].transform,"SelectedNode");
	
	--followings are not needed
	GameObjectSetActive(coachListAvatars[randomIndex].SelectedNode,false);
	--set pos icon
	local posID = CoachData.GetCoachPosID(value.id);
	if(posID~=nil)then
		local posIconName = CoachData.GetCoachPosIcon(posID);
		if(posIconName~=nil)then
			GameObjectSetActive(coachListAvatars[randomIndex].PosIcon,true);
			UIHelper.SetSpriteName(coachListAvatars[randomIndex].PosIcon,posIconName);
		else
			GameObjectSetActive(coachListAvatars[randomIndex].PosIcon,false);
		end
	else
		GameObjectSetActive(coachListAvatars[randomIndex].PosIcon,false);
	end
	--value is {id,lv,exp,adv}
	local coachConfigData = CoachData.GetCoachConfigData(value.id);
	if(coachConfigData~=nil)then
		Util.SetUITexture(coachListAvatars[randomIndex].Icon, LuaConst.Const.CoachHeadIcon, coachConfigData.head, true);
		UIHelper.SetSpriteName(coachListAvatars[randomIndex].Rank,coachConfigData.rating);
		UIHelper.SetWidgetColor(coachListAvatars[randomIndex].RankColor,CoachData.GetCoachRankColor(value.id));
		UIHelper.SetWidgetColor(coachListAvatars[randomIndex].BGRankColor,CoachData.GetCoachBGRankColor(value.id));
		UIHelper.SetWidgetColor(coachListAvatars[randomIndex].Name,CoachData.GetCoachNameColor(value.id));
		UIHelper.SetLabelTxt(coachListAvatars[randomIndex].Name,CoachData.GetCoachNameWithSuffix(value.id));
		UIHelper.SetLabelTxt(coachListAvatars[randomIndex].Reputation,CoachData.GetCoachReputationIs(value.id));
		
		AddOrChangeClickParameters(coachListAvatars[randomIndex].gameObject,OnClickListAvatar,{coachid = value.id,avatarindex = randomIndex});	
	else
		print("coach :"..value.id.." not found");
	end

	UIHelper.SetDragScrollViewTarget(coachListAvatars[randomIndex].transform,uiScrollView);
end

function SelectCoach(coachid,coachAvatarIndex)
	if(targetCoachAvatarIndex ~= nil and coachListAvatars[targetCoachAvatarIndex] ~= nil)then
		--set old one disactive
		GameObjectSetActive(coachListAvatars[targetCoachAvatarIndex].SelectedNode,false);
	end
	targetCoachID = coachid;
	targetCoachAvatarIndex = coachAvatarIndex;

	--set current
	if(targetCoachAvatarIndex ~= nil and coachListAvatars[targetCoachAvatarIndex] ~= nil)then		
		--set new one active
		GameObjectSetActive(coachListAvatars[targetCoachAvatarIndex].SelectedNode,true);
		local coachConfigData = CoachData.GetCoachConfigData(targetCoachID);
		local coachUserData = CoachData.GetCoachUserData(targetCoachID);
		if(coachUserData~=nil)then			
			Util.SetUITexture(coachAvatar.icon,LuaConst.Const.CoachHeadIcon,coachConfigData.head,true);
			UIHelper.SetSpriteName(coachAvatar.rank,coachConfigData.rating);
			UIHelper.SetLabelTxt(coachAvatar.name,CoachData.GetCoachNameWithSuffix(targetCoachID));

			UIHelper.SetWidgetColor(coachAvatar.rankColor,CoachData.GetCoachRankColor(targetCoachID));
			UIHelper.SetWidgetColor(coachAvatar.bgRankColor,CoachData.GetCoachBGRankColor(targetCoachID));
			UIHelper.SetWidgetColor(coachAvatar.name,CoachData.GetCoachNameColor(targetCoachID));
			local abilityTable = CoachData.GetCoachAbilityTable(targetCoachID);
			local abilityString = "";
			for i,v in ipairs(abilityTable) do
				abilityString = abilityString..v;
				if(i%2==0)then
					abilityString = abilityString.. "\n";
				else
					abilityString = abilityString.. " ";
				end
			end
			UIHelper.SetLabelTxt(coachAvatar.ability,abilityString);
			local DoUPercent = CoachData.GetCoachDoUPercent(targetCoachID);
			UIHelper.SetLabelTxt(coachAvatar.dou,GetLocalizedString("CoachDoUis")..string.format("%.1f",DoUPercent*100).."%");
			UIHelper.SetLabelTxt(coachAvatar.reputation,CoachData.GetCoachReputationIs(targetCoachID));
			-- UIHelper.SetLabelTxt(coachAvatar.skill,"紧张开发中");
		end
	end
end

function GetOptionCoachList()
	local currentCoachTeam = CoachData.GetCoachTeamTable();
	local allCoachData = CoachData.GetCoachTable();

	--get all coaches
	local filtered = {};
	for k,v in pairs(allCoachData) do
		filtered[k] = v;
	end
	--delete current coach at targetPosID
	if(targetPosID~=nil and currentCoachTeam[targetPosID]~=nil)then
		filtered[currentCoachTeam[targetPosID]] = nil;
	end

	--make a sorted list
	local ret = {};
	for k,v in pairs(filtered) do
		table.insert(ret,v);
	end
	table.sort( ret, function (a,b)		
		if(a.adv>b.adv)then
			return true;
		elseif(a.adv==b.adv)then
			if(a.lv>b.lv)then
				return true;
			elseif(a.lv>b.lv)then
				return tonumber(a.id)<tonumber(b.id);
			else
				return false
			end
		else
			return false;
		end
	end);
	return ret;
end

function OnClickListAvatar(go)
	local listener = UIHelper.GetUIEventListener(go);
	if(listener~=nil and listener.parameter~=nil )then
		SelectCoach(listener.parameter.coachid,listener.parameter.avatarindex);
	end  
end

function OnClickButtonChangePos()
	local currentCoachTeam = CoachData.GetCoachTeamTable();

	local targetCoachTeam = {};
	local swapCoachID = "0";
	local swapPosID = nil;
	for k,v in pairs(currentCoachTeam) do
		if(v == targetCoachID)then
			--save it
			swapPosID = k;
		elseif(k == targetPosID)then
			swapCoachID = v;
		end
		targetCoachTeam[k] = v;
	end
	targetCoachTeam[targetPosID] = targetCoachID;
	if(swapPosID~=nil)then
		targetCoachTeam[swapPosID] = swapCoachID;
	end

	--req replace msg
	CoachData.ReqCoachReplace(targetCoachTeam);
end

function OnClickButtonClose()
	Close();
end

function OnReqCoachReplace()
	Close();
end

function RegisterMsg()
	msgRegisteredKey = CoachData.RegisterDelegatesOnReqSuccess( MsgID.tb.CoachReplace, OnReqCoachReplace );
end

function UnregisterMsg()
	if(msgRegisteredKey~=nil)then
		CoachData.UnregisterDelegatesOnReqSuccess( MsgID.tb.CoachReplace, msgRegisteredKey );
		msgRegisteredKey = nil;
	end
end

function Close()
	windowComponent:Close();
end

function OnDestroy()
	UnregisterMsg();

	avatarPrefab = nil;
	buttonClose = nil;
	buttonChange = nil;
	coachAvatar = nil;
	coachEmpty = nil;
	coachListAvatars = {};
	uiScrollView = nil;
	uiContainer = nil;
	window = nil;
	windowComponent = nil;
	targetPosID = nil;
	targetCoachID = nil;
	targetCoachAvatarIndex = nil;
	msgRegisteredKey = nil;
end