module("UICoachBackup",package.seeall);

require "Game/CoachData"

local coachBackupSettings = {
	prefabName = "CoachBackup",
	prefab = nil,

	coachAvatarPrefabName = "CoachAvatar",
	coachAvatarPrefab = nil,
}

local coachBackupUI = nil;
local uiScrollView = nil;
local uiContainer = nil;

local coachAvatars = {};

function ShowCoachBackup(willShow,windowComponent,rootNode,depth)
	if(not willShow)then
		--disactivate it
		if(coachBackupUI ~= nil)then
			GameObjectSetActive(coachBackupUI.transform,false);
		end
	else
		--show it 
		if(coachBackupUI == nil)then

			--Get prefabs
			coachBackupSettings.prefab = windowComponent:GetPrefab(coachBackupSettings.prefabName);
			coachBackupSettings.coachAvatarPrefab = windowComponent:GetPrefab(coachBackupSettings.coachAvatarPrefabName);

			--generate ui and initialize it
			coachBackupUI = GameObjectInstantiate(coachBackupSettings.prefab);
			coachBackupUI.transform.parent = rootNode;
	    	coachBackupUI.transform.localPosition = Vector3.zero;
	    	coachBackupUI.transform.localScale = Vector3.one; 	

			BindUI();
			RegisterMsg();
		end
		if(coachBackupUI ~= nil)then
			GameObjectSetActive(coachBackupUI.transform,true);
		end
		SetInfo(depth);
	end	
end

function BindUI()
	uiScrollView = TransformFindChild(coachBackupUI.transform,"uiScrollView");
	uiContainer = TransformFindChild(uiScrollView,"uiContainer");
end

function SetInfo(depth)
	if(depth~=nil)then
		UIHelper.SetPanelDepth(uiScrollView,depth);
	end
	--delete old avatars
	DestroyUIListItemGameObjects(coachAvatars);
	--create new avatars
	local coachTableData = CoachData.GetCoachTable();

	if(not IsTableEmpty(coachTableData))then
		local finalTableData = SortCoachTableData(coachTableData);
		CreateUIListItemGameObjects(uiContainer, finalTableData, coachBackupSettings.coachAvatarPrefab, OnInitializeAvatar);
	end

	UIHelper.RepositionGrid(uiContainer);
	UIHelper.RefreshPanel(uiScrollView);
end

function SortCoachTableData(coachTableData)	
	local retTable = {};
	if(coachTableData ~= nil)then
		local tableToCull = {};
		for k,v in pairs(coachTableData) do
			tableToCull[k] = v;
		end

		--remove coaches activated
		for k,v in pairs(CoachData.GetCoachTeamTable()) do
			--find key v, and remove it
			tableToCull[v] = nil;
		end

		--make a list
		for k,v in pairs(tableToCull) do
			table.insert(retTable,v);
		end

		table.sort(retTable,function (a,b)
			local coachConfigDataA = CoachData.GetCoachConfigData(a.id);
			local coachConfigDataB = CoachData.GetCoachConfigData(b.id);
			if(coachConfigDataA.compose>coachConfigDataB.compose)then
				return true;
			elseif(coachConfigDataA.compose == coachConfigDataB.compose)then
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
			else
				return false;
			end
		end);
	end
	return retTable;
end

function OnInitializeAvatar(randomIndex, key, value, cloneGameObject)	
	coachAvatars[randomIndex] = {};
	coachAvatars[randomIndex].gameObject = cloneGameObject;
	coachAvatars[randomIndex].transform = cloneGameObject.transform;
	coachAvatars[randomIndex].transform.localPosition = Vector3.zero;
	coachAvatars[randomIndex].gameObject.name = string.format("%03d",tonumber(key));
	--bind ui
	coachAvatars[randomIndex].Icon = TransformFindChild(coachAvatars[randomIndex].transform,"IconRoot/Icon");
	coachAvatars[randomIndex].Rank = TransformFindChild(coachAvatars[randomIndex].transform,"RankRoot/Rank");
	coachAvatars[randomIndex].RankColor = TransformFindChild(coachAvatars[randomIndex].transform,"RankColor");
	coachAvatars[randomIndex].BGRankColor = TransformFindChild(coachAvatars[randomIndex].transform,"BGRankColor");

	coachAvatars[randomIndex].NameNode = TransformFindChild(coachAvatars[randomIndex].transform,"NameNode");
	coachAvatars[randomIndex].Name = TransformFindChild(coachAvatars[randomIndex].NameNode,"Name");
	coachAvatars[randomIndex].Tag = TransformFindChild(coachAvatars[randomIndex].NameNode,"Tag");
	coachAvatars[randomIndex].Reputation = TransformFindChild(coachAvatars[randomIndex].NameNode,"Repu");

	coachAvatars[randomIndex].RecruitNode = TransformFindChild(coachAvatars[randomIndex].transform,"RecruitNode");

	coachAvatars[randomIndex].ButtonNode = TransformFindChild(coachAvatars[randomIndex].transform,"ButtonNode");
	coachAvatars[randomIndex].ButtonTrain = TransformFindChild(coachAvatars[randomIndex].ButtonNode,"ButtonTrain");
	coachAvatars[randomIndex].ButtonChange = TransformFindChild(coachAvatars[randomIndex].ButtonNode,"ButtonChange");
	coachAvatars[randomIndex].ButtonBG = TransformFindChild(coachAvatars[randomIndex].ButtonNode,"ButtonNodeBG");
	
	coachAvatars[randomIndex].ButtonMore = TransformFindChild(coachAvatars[randomIndex].transform,"ButtonMore");

	--little tweaks
	coachAvatars[randomIndex].ButtonTrain.transform.localPosition = NewVector3(0,-50,0);
	GameObjectSetActive(coachAvatars[randomIndex].RecruitNode,false);
	-- GameObjectSetActive(coachAvatars[randomIndex].Tag,false);
	GameObjectSetActive(coachAvatars[randomIndex].ButtonChange,false);
	GameObjectSetActive(coachAvatars[randomIndex].ButtonBG,false);
	GameObjectSetActive(coachAvatars[randomIndex].ButtonMore,false);

	--set ui	
	--value is {id,lv,exp,adv}
	local coachConfigData = CoachData.GetCoachConfigData(value.id);
	if(coachConfigData~=nil)then
		Util.SetUITexture(coachAvatars[randomIndex].Icon, LuaConst.Const.CoachHeadIcon, coachConfigData.head, true);

		UIHelper.SetSpriteName(coachAvatars[randomIndex].Rank,coachConfigData.rating);
		UIHelper.SetWidgetColor(coachAvatars[randomIndex].RankColor,CoachData.GetCoachRankColor(value.id));
		UIHelper.SetWidgetColor(coachAvatars[randomIndex].BGRankColor,CoachData.GetCoachBGRankColor(value.id));

		UIHelper.SetLabelTxt(coachAvatars[randomIndex].Name,CoachData.GetCoachNameWithSuffix(value.id));
		UIHelper.SetWidgetColor(coachAvatars[randomIndex].Name,CoachData.GetCoachNameColor(value.id));

		UIHelper.SetLabelTxt(coachAvatars[randomIndex].Tag,CoachData.GetCoachPosName(value.id));

		UIHelper.SetLabelTxt(coachAvatars[randomIndex].Reputation,CoachData.GetCoachReputationIs(value.id));
		
		AddOrChangeClickParameters(coachAvatars[randomIndex].ButtonMore.gameObject,OnClickButtonMore,{buttonNode = coachAvatars[randomIndex].ButtonNode});
		AddOrChangeClickParameters(coachAvatars[randomIndex].ButtonTrain.gameObject,OnClickButtonTrain,{coachid = value.id});
		-- AddOrChangeClickParameters(v.ButtonChange.gameObject,OnClickButtonChange,{coachid = coachID});
	
		-- GameObjectSetActive(coachAvatars[randomIndex].ButtonNode,false);		
	else
		print("coach :"..value.id.." not found");
	end
end

function OnClickButtonMore(go)
	local listener = UIHelper.GetUIEventListener(go);
	if(listener~=nil and listener.parameter~=nil )then
		GameObjectSetActive(listener.parameter.buttonNode,not GameObjectActiveSelf(listener.parameter.buttonNode.gameObject));
	end	
end

function OnClickButtonTrain(go)
	local listener = UIHelper.GetUIEventListener(go);
	if(listener~=nil and listener.parameter~=nil )then
		WindowMgr.ShowWindow(LuaConst.Const.UICoachDetailWindow, {openTab=1, coachID = listener.parameter.coachid});
	end    
end

-- function OnClickButtonChange(go)
-- 	-- body
-- end

function OnUpdateCoach()
	SetInfo();
end

function RegisterMsg()
	SynSys.RegisterCallback("coach", OnUpdateCoach);
end

function UnregisterMsg()	
    SynSys.UnRegisterCallback("coach", OnUpdateCoach);
end

function OnDestroy()
	UnregisterMsg();

	coachBackupSettings.prefab = nil;
	coachBackupSettings.coachAvatarPrefab = nil;

	coachBackupUI = nil;
	uiScrollView = nil;
	uiContainer = nil;

	coachAvatars = {};
end