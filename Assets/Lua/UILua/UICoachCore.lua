module("UICoachCore",package.seeall);

require "Game/CoachData"


local coachCoreSettings = {
	prefabName = "CoachCore",
	prefab = nil,
	reputationPrefix = "CoachReputationis",
	skillPrefix = "CoachSkill",
}

local coachCoreUI = nil;
local coaches = {
	CoachMain = 
	{
		id = "9",
		hierachyName = "CoachMain",
	},
	CoachAttack = 
	{
		id = "1",
		hierachyName = "CoachAttack",
	},
	CoachDefence = 
	{
		id = "2",
		hierachyName = "CoachDefence",
	},
	CoachOrganize = 
	{
		id = "3",
		hierachyName = "CoachOrganize",
	},
	CoachGoalKeeper = 
	{
		id = "4",
		hierachyName = "CoachGoalKeeper",
	},
}

function ShowCoachCore(willShow,windowComponent,rootNode)
	if(not willShow)then
		--disactivate it
		if(coachCoreUI ~= nil)then
			GameObjectSetActive(coachCoreUI.transform,false);
		end
	else
		--show it 
		if(coachCoreUI == nil)then

			--Get prefabs
			coachCoreSettings.prefab = windowComponent:GetPrefab(coachCoreSettings.prefabName);

			--generate ui and initialize it
			coachCoreUI = GameObjectInstantiate(coachCoreSettings.prefab);
			coachCoreUI.transform.parent = rootNode;
	    	coachCoreUI.transform.localPosition = Vector3.zero;
	    	coachCoreUI.transform.localScale = Vector3.one; 	

			BindUI();
			RegisterMsg();
		end
		if(coachCoreUI ~= nil)then
			GameObjectSetActive(coachCoreUI.transform,true);
		end
		SetInfo();
	end
end

function BindUI()
	for k,v in pairs(coaches) do
		v.transform = TransformFindChild(coachCoreUI.transform,v.hierachyName);

		v.CoachAvatar = TransformFindChild(v.transform,"CoachAvatar");
		v.Icon = TransformFindChild(v.CoachAvatar,"IconRoot/Icon");
		v.Rank = TransformFindChild(v.CoachAvatar,"RankRoot/Rank");
		v.RankColor = TransformFindChild(v.CoachAvatar,"RankColor");
		v.BGRankColor = TransformFindChild(v.CoachAvatar,"BGRankColor");

		v.ButtonMore = TransformFindChild(v.CoachAvatar,"ButtonMore");

		v.NameNode = TransformFindChild(v.CoachAvatar,"NameNode");
		v.Name = TransformFindChild(v.NameNode,"Name");
		v.Repu = TransformFindChild(v.NameNode,"Repu");
		v.DoU = TransformFindChild(v.NameNode,"DoU");
		if(v.id == "9")then
			v.Skill = TransformFindChild(v.NameNode,"Skill");		
			GameObjectSetActive(v.Skill,false);		
		end

		v.ButtonNode = TransformFindChild(v.CoachAvatar,"ButtonNode");
		v.ButtonTrain = TransformFindChild(v.ButtonNode,"ButtonTrain");
		v.ButtonChange = TransformFindChild(v.ButtonNode,"ButtonChange");

		v.CoachEmpty = TransformFindChild(v.transform,"CoachEmpty");
		v.ButtonActivate = TransformFindChild(v.CoachEmpty,"ButtonActivate");

	end
end

function SetInfo()
	--fetch data
	local coachTableData = CoachData.GetCoachTable();
	local coachTeamData = CoachData.GetCoachTeamTable();
	local coachConfigData = Config.GetTemplate(Config.Coach());
	local coachAdvConfigData = Config.GetTemplate(Config.CoachAdv());

	--set ui info
	for k,v in pairs(coaches) do
		local coachID = coachTeamData[v.id];
		if(coachID == nil or coachID == "0")then
			--disactive avatar
			GameObjectSetActive(v.CoachEmpty,true);
			GameObjectSetActive(v.CoachAvatar,false);
			AddOrChangeClickParameters(v.ButtonActivate.gameObject,OnClickButtonActivate,{posid = v.id});	
		else
			--show avatar
			GameObjectSetActive(v.CoachAvatar,true);
			GameObjectSetActive(v.CoachEmpty,false);

			local coachData = coachConfigData[coachID];
			if(coachData~=nil)then
				Util.SetUITexture(v.Icon, LuaConst.Const.CoachHeadIcon, coachData.head, true);

				UIHelper.SetSpriteName(v.Rank,coachData.rating);				
				UIHelper.SetWidgetColor(v.RankColor,CoachData.GetCoachRankColor(coachID));
				UIHelper.SetWidgetColor(v.BGRankColor,CoachData.GetCoachBGRankColor(coachID));

				-- UIHelper.SetLabelTxt(v.Name,coachData.name..CoachData.GetCoachRankNameSuffix(coachID));
				UIHelper.SetLabelTxt(v.Name,CoachData.GetCoachNameWithSuffix(coachID));
				UIHelper.SetWidgetColor(v.Name,CoachData.GetCoachNameColor(coachID));

				UIHelper.SetLabelTxt(v.Repu,CoachData.GetCoachReputationIs(coachID));
				UIHelper.SetLabelTxt(v.DoU,CoachData.GetCoachDoUPercentIs(coachID));
				-- if(v.id == "9")then
				-- 	UIHelper.SetLabelTxt(v.Skill,GetLocalizedString(coachCoreSettings.skillPrefix).."技能紧张开发中");	
				-- end

				AddOrChangeClickParameters(v.ButtonMore.gameObject,OnClickButtonMore,{buttonNode = v.ButtonNode});
				AddOrChangeClickParameters(v.ButtonTrain.gameObject,OnClickButtonTrain,{coachid = coachID});
				AddOrChangeClickParameters(v.ButtonChange.gameObject,OnClickButtonChange,{posid = v.id,coachid = coachID});		
			
				GameObjectSetActive(v.ButtonNode,false);		
			else
				print("coach :"..coachID.." not found");
			end
		end
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

function OnClickButtonChange(go)
	local listener = UIHelper.GetUIEventListener(go);
	if(listener~=nil and listener.parameter~=nil )then
		WindowMgr.ShowWindow(LuaConst.Const.UICoachChangePosWindow,{posID = listener.parameter.posid});
	end   
end

function OnClickButtonActivate(go)
	local listener = UIHelper.GetUIEventListener(go);
	if(listener~=nil and listener.parameter~=nil )then
		WindowMgr.ShowWindow(LuaConst.Const.UICoachChangePosWindow,{posID = listener.parameter.posid});
	end   
end

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
	coachCoreSettings.prefab = nil;
	coachCoreUI = nil;

	coaches = {
		CoachMain = 
		{
			id = "9",
			hierachyName = "CoachMain",
		},
		CoachAttack = 
		{
			id = "1",
			hierachyName = "CoachAttack",
		},
		CoachDefence = 
		{
			id = "2",
			hierachyName = "CoachDefence",
		},
		CoachOrganize = 
		{
			id = "3",
			hierachyName = "CoachOrganize",
		},
		CoachGoalKeeper = 
		{
			id = "4",
			hierachyName = "CoachGoalKeeper",
		},
	}
end