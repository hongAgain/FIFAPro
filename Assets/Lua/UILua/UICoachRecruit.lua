module("UICoachRecruit",package.seeall);

require "Game/CoachData"

local coachRecruitSettings = {
	prefabName = "CoachRecruit",
	prefab = nil,

	coachAvatarPrefabName = "CoachAvatar",
	coachAvatarPrefab = nil,

	recruitGlowFadeInTime = 0.5,
	recruitGlowFadeOutTime = 0.25,
	coachAnimationName = "CoachSpecialIdle",
}

local msgRegisterKey = nil;

local coachRecruitUI = nil;
local uiScrollView = nil;
local uiContainer = nil;

local currentCoachID = nil;
local currentCoachIndex = nil;

local coachAvatars = {};

function ShowCoachRecruit(willShow,windowComponent,rootNode,depth)
	if(not willShow)then
		--disactivate it
		if(coachRecruitUI ~= nil)then
			GameObjectSetActive(coachRecruitUI.transform,false);
		end
	else
		--show it 
		if(coachRecruitUI == nil)then

			--Get prefabs
			coachRecruitSettings.prefab = windowComponent:GetPrefab(coachRecruitSettings.prefabName);
			coachRecruitSettings.coachAvatarPrefab = windowComponent:GetPrefab(coachRecruitSettings.coachAvatarPrefabName);

			--generate ui and initialize it
			coachRecruitUI = GameObjectInstantiate(coachRecruitSettings.prefab);
			coachRecruitUI.transform.parent = rootNode;
	    	coachRecruitUI.transform.localPosition = Vector3.zero;
	    	coachRecruitUI.transform.localScale = Vector3.one; 	

			BindUI();
			RegisterMsg();
		end
		if(coachRecruitUI ~= nil)then
			GameObjectSetActive(coachRecruitUI.transform,true);
		end
		SetInfo(depth);
	end	
end

function BindUI()
	uiScrollView = TransformFindChild(coachRecruitUI.transform,"uiScrollView");
	uiContainer = TransformFindChild(uiScrollView,"uiContainer");
end

function SetInfo(depth)
	if(depth~=nil)then
		UIHelper.SetPanelDepth(uiScrollView,depth);
	end
	--delete old avatars
	DestroyUIListItemGameObjects(coachAvatars);
	--create new avatars
	local finalTableData = SortCoachTableData();
	if(not IsTableEmpty(finalTableData))then
		CreateUIListItemGameObjects(uiContainer, finalTableData, coachRecruitSettings.coachAvatarPrefab, OnInitializeAvatar);
	end

	UIHelper.RepositionGrid(uiContainer);
	UIHelper.RefreshPanel(uiScrollView);
end

function RefreshCollectInfo()
	for k,v in pairs(coachAvatars) do
		local numNeed = v.data.compose;
		local numHave = ItemSys.GetItemData(tostring(v.data.soul)).num;
		if(numHave >= numNeed)then
			--show button
			GameObjectSetActive(v.ButtonRecruit,true);
			GameObjectSetActive(v.CollectProgressBar,false);
		else
			--show progress
			GameObjectSetActive(v.ButtonRecruit,false);
			GameObjectSetActive(v.CollectProgressBar,true);
			UIHelper.SetProgressBar(v.CollectProgressBar,numHave/numNeed);
			UIHelper.SetLabelTxt(v.CollectProgressNum,numHave.."/"..numNeed);
		end
	end
end

function SortCoachTableData(coachTableData)	

	local allConfigData = CoachData.GetCoachConfigTableAvailable();
	local coachTableData = CoachData.GetCoachTable();

	local retTable = {};
	for k,v in pairs(allConfigData) do
		if(coachTableData[tostring(v.id)] == nil)then
			--add it if you dont have this coach
			table.insert(retTable,v);
		end
	end
	table.sort(retTable,function (a,b)
		local numFullA = ItemSys.GetItemData(tostring(a.soul)).num >= a.compose;
		local numFullB = ItemSys.GetItemData(tostring(b.soul)).num >= b.compose;

		if(numFullA and not numFullB)then
			return true;
		else
			if(numFullA == numFullB)then
				if(a.compose > b.compose)then
					return true;
				else
					if(tostring(a.id)<tostring(b.id))then
						return true;
					else
						return false;
					end
				end
			else
				return false;
			end
		end
	end);
	return retTable;
end

function OnInitializeAvatar(randomIndex, key, value, cloneGameObject)	
	coachAvatars[randomIndex] = {};
	coachAvatars[randomIndex].data = value;
	coachAvatars[randomIndex].gameObject = cloneGameObject;
	coachAvatars[randomIndex].transform = cloneGameObject.transform;
	coachAvatars[randomIndex].transform.localPosition = Vector3.zero;
	coachAvatars[randomIndex].gameObject.name = string.format("%03d",tonumber(key));
	--bind ui
	coachAvatars[randomIndex].Icon = TransformFindChild(coachAvatars[randomIndex].transform,"IconRoot/Icon");
	coachAvatars[randomIndex].Rank = TransformFindChild(coachAvatars[randomIndex].transform,"RankRoot/Rank");
	coachAvatars[randomIndex].RankColor = TransformFindChild(coachAvatars[randomIndex].transform,"RankColor");
	coachAvatars[randomIndex].BGRankColor = TransformFindChild(coachAvatars[randomIndex].transform,"BGRankColor");

	coachAvatars[randomIndex].RecruitGlow = TransformFindChild(coachAvatars[randomIndex].transform,"RecruitGlow");

	coachAvatars[randomIndex].NameNode = TransformFindChild(coachAvatars[randomIndex].transform,"NameNode");
	coachAvatars[randomIndex].Name = TransformFindChild(coachAvatars[randomIndex].NameNode,"Name");
	coachAvatars[randomIndex].Tag = TransformFindChild(coachAvatars[randomIndex].NameNode,"Tag");
	coachAvatars[randomIndex].Reputation = TransformFindChild(coachAvatars[randomIndex].NameNode,"Repu");

	coachAvatars[randomIndex].RecruitNode = TransformFindChild(coachAvatars[randomIndex].transform,"RecruitNode");
	coachAvatars[randomIndex].ButtonRecruit = TransformFindChild(coachAvatars[randomIndex].RecruitNode,"ButtonRecruit");
	coachAvatars[randomIndex].CollectProgressBar = TransformFindChild(coachAvatars[randomIndex].RecruitNode,"CollectProgress");
	coachAvatars[randomIndex].CollectProgressNum = TransformFindChild(coachAvatars[randomIndex].CollectProgressBar,"Num");
	coachAvatars[randomIndex].ButtonObtainPieces = TransformFindChild(coachAvatars[randomIndex].RecruitNode,"ButtonObtainPieces");
	
	coachAvatars[randomIndex].ButtonNode = TransformFindChild(coachAvatars[randomIndex].transform,"ButtonNode");	
	coachAvatars[randomIndex].ButtonMore = TransformFindChild(coachAvatars[randomIndex].transform,"ButtonMore");

	--followings are not needed
	GameObjectSetActive(coachAvatars[randomIndex].ButtonNode,false);
	GameObjectSetActive(coachAvatars[randomIndex].Tag,false);
	GameObjectSetActive(coachAvatars[randomIndex].ButtonMore,false);
	GameObjectSetActive(coachAvatars[randomIndex].Reputation,false);

	--set ui	
	--value is {id,lv,exp,adv}
	Util.SetUITexture(coachAvatars[randomIndex].Icon, LuaConst.Const.CoachHeadIcon, value.head, true);

	UIHelper.SetSpriteName(coachAvatars[randomIndex].Rank,value.rating);
	UIHelper.SetWidgetColor(coachAvatars[randomIndex].RankColor,CoachData.GetCoachRankColor(value.id));
	UIHelper.SetWidgetColor(coachAvatars[randomIndex].BGRankColor,CoachData.GetCoachBGRankColor(value.id));
	UIHelper.SetWidgetColor(coachAvatars[randomIndex].RecruitGlow,CoachData.GetCoachRecruitGlowColor(value.id));

	UIHelper.SetLabelTxt(coachAvatars[randomIndex].Name,CoachData.GetCoachNameWithSuffix(value.id));
	UIHelper.SetWidgetColor(coachAvatars[randomIndex].Name,CoachData.GetCoachNameColor(value.id));

	-- UIHelper.SetLabelTxt(coachAvatars[randomIndex].Reputation,CoachData.GetCoachReputationIs(value.id));

	local numNeed = value.compose;
	local numHave = ItemSys.GetItemData(tostring(value.soul)).num;

	if(numHave >= numNeed)then
		--show button
		GameObjectSetActive(coachAvatars[randomIndex].ButtonRecruit,true);
		GameObjectSetActive(coachAvatars[randomIndex].CollectProgressBar,false);
		GameObjectSetActive(coachAvatars[randomIndex].ButtonObtainPieces,false);
	else
		--show progress
		GameObjectSetActive(coachAvatars[randomIndex].ButtonRecruit,false);
		GameObjectSetActive(coachAvatars[randomIndex].CollectProgressBar,true);
		GameObjectSetActive(coachAvatars[randomIndex].ButtonObtainPieces,true);
		UIHelper.SetProgressBar(coachAvatars[randomIndex].CollectProgressBar,numHave/numNeed);
		UIHelper.SetLabelTxt(coachAvatars[randomIndex].CollectProgressNum,numHave.."/"..numNeed);
	end
	AddOrChangeClickParameters(coachAvatars[randomIndex].ButtonRecruit.gameObject,OnClickRecruit,{targetCoachid = value.id,targetCoachIndex = randomIndex});
	AddOrChangeClickParameters(coachAvatars[randomIndex].ButtonObtainPieces.gameObject,OnClickObtainPieces,{targetCoachid = value.id,targetPieceid = tostring(value.soul)});
end

function OnClickRecruit(go)
	local listener = UIHelper.GetUIEventListener(go);
	if(listener~=nil and listener.parameter~=nil )then
		--recruit this coach
		currentCoachID = listener.parameter.targetCoachid;
		currentCoachIndex = listener.parameter.targetCoachIndex;
		CoachData.RequestMsg( MsgID.tb.CoachPro, LuaConst.Const.CoachPro, {id = listener.parameter.targetCoachid} );
	end
end

function OnClickObtainPieces(go)
	local listener = UIHelper.GetUIEventListener(go);
	if(listener~=nil and listener.parameter~=nil )then
		--recruit this coach
		-- print("Coach:"..listener.parameter.targetCoachid.." Piece:"..listener.parameter.targetPieceid);
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { "这里显示\n碎片："..listener.parameter.targetPieceid.." 的来源："}); 
	end
end

function OnReqPro()
	ShowRecruitAnimation();	
end

function ShowRecruitAnimation()
	local ShowRecruitAvatar = function ()
		--refresh
		OnUpdateCoach();
		--show recruit animation
		WindowMgr.ShowWindow(LuaConst.Const.UICoachScout,{targetCoachID = currentCoachID or "1001", targetCoachAnime = coachRecruitSettings.coachAnimationName});
		currentCoachID = nil;	
	end

	local RecruitGlowFadeOut = function ()
		UIHelper.FadeUIWidgetColor(
			coachAvatars[currentCoachIndex].RecruitGlow, 
			CoachData.GetCoachBGRankColor(currentCoachID), 
			CoachData.GetCoachRecruitGlowColor(currentCoachID), 
			coachRecruitSettings.recruitGlowFadeOutTime,
			ShowRecruitAvatar);
	end

	local RecruitGlowFadeIn = function ()
		UIHelper.FadeUIWidgetColor(
			coachAvatars[currentCoachIndex].RecruitGlow, 
			CoachData.GetCoachRecruitGlowColor(currentCoachID), 
			CoachData.GetCoachBGRankColor(currentCoachID), 
			coachRecruitSettings.recruitGlowFadeInTime, 
			RecruitGlowFadeOut)
	end
	RecruitGlowFadeIn();
end

function OnUpdateCoach()
	SetInfo();
end

function OnUpdateCollectInfo()
	RefreshCollectInfo();
end

function RegisterMsg()
	SynSys.RegisterCallback("item", OnUpdateCollectInfo);
	-- SynSys.RegisterCallback("coach", OnUpdateCoach);
	msgRegisterKey = CoachData.RegisterDelegatesOnReqSuccess( MsgID.tb.CoachPro, OnReqPro );
end

function UnregisterMsg()	
    SynSys.UnRegisterCallback("item", OnUpdateCollectInfo);
    -- SynSys.UnRegisterCallback("coach", OnUpdateCoach);
    if(msgRegisterKey~=nil)then
		CoachData.UnregisterDelegatesOnReqSuccess( MsgID.tb.CoachPro, msgRegisterKey );
		msgRegisterKey = nil;
	end
end

function OnDestroy()
	UnregisterMsg();
	coachRecruitSettings.prefab = nil;
	coachRecruitSettings.coachAvatarPrefab = nil;
	msgRegisterKey = nil;
	coachRecruitUI = nil;
	uiScrollView = nil;
	uiContainer = nil;
	currentCoachID = nil;
	coachAvatars = {};
end