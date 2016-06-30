module ("UITimeRaidDifficultyBox", package.seeall);

require "Common/UnityCommonScript"
require "Config"
require "Game/Role"
require "Game/Hero"
require "Game/TimeRaidData"

--local normalShaderPath = "";
--local grayShaderPath = "";

local difficultyBoxPrefabName = "UITimeRaidDifficultyBox";
local difficultyBox = nil;

local uiLabelChallengeNum = nil;
local uiLabelEnergycost = nil;

local timeRaidLocalDataTable = nil;

local delegateOnSelectDifficulty = nil;

local currentRaidClass = nil;
local windowComponent = nil;

function CreateDifficultyBox( containerTransform, window, raidclass, delegateonselectdifficulty )
	-- body

	if(difficultyBox == nil) then
        windowComponent = window;
		--instantiate one
		local difficultyBoxPrefab = windowComponent:GetPrefab(difficultyBoxPrefabName);
		--instantiate prefab and initialize it
		difficultyBox = GameObjectInstantiate(difficultyBoxPrefab);
		difficultyBox.transform.parent = containerTransform;
    	difficultyBox.transform.localPosition = Vector3.zero;
    	difficultyBox.transform.localScale = Vector3.one;

    	delegateOnSelectDifficulty = delegateonselectdifficulty;

    	uiLabelChallengeNum = TransformFindChild(difficultyBox.transform,"ChallengeNumToday/Label");
    	uiLabelEnergycost = TransformFindChild(difficultyBox.transform,"EnergyCost/Label");

		Util.AddClick(TransformFindChild(difficultyBox.transform,"ButtonClose").gameObject,OnClickClose);
	end

	--active it
	if(not GameObjectActiveSelf(difficultyBox)) then
    	GameObjectSetActive(difficultyBox.transform,true);
    end

	--set info
	SetInfo(raidclass);
end

function SetInfo(raidclass)
	--calc datas
	timeRaidNetInfoData = TimeRaidData.Get_TimeRaidInfoData();
	timeRaidLocalDataTable = Config.GetTemplate(Config.RaidDSTable());
	currentRaidClass = raidclass;
	local realIdBase = (currentRaidClass - 1) * LuaConst.Const.TimeRaidNrByType;
	if (timeRaidNetInfoData[tostring(realIdBase + 1)] == nil) then
		timeRaidNetInfoData[tostring(realIdBase + 1)] = 0;
	end
	if (timeRaidNetInfoData[tostring(realIdBase + 2)] == nil) then 
		timeRaidNetInfoData[tostring(realIdBase + 2)] = 0;
	end
	if (timeRaidNetInfoData[tostring(realIdBase + 3)] == nil) then 
		timeRaidNetInfoData[tostring(realIdBase + 3)] = 0;
	end
    
	local challengeNum = 	timeRaidNetInfoData[tostring(realIdBase+1)]+
							timeRaidNetInfoData[tostring(realIdBase+2)]+
							timeRaidNetInfoData[tostring(realIdBase+3)];
                            
	local energyCost = Config.GetProperty(Config.RaidDSTable(), tostring(realIdBase + 1), "Power");
	UIHelper.SetLabelTxt(uiLabelEnergycost, energyCost);

	--set info
    local maxChallengeTimes = Config.GetProperty(Config.RaidDSVipTable(), tostring(Role.Get_vip()), "num");
    local challengeTimesLeft = math.clamp(maxChallengeTimes - challengeNum, 0, maxChallengeTimes);
	UIHelper.SetLabelTxt(uiLabelChallengeNum, string.format(Util.LocalizeString("UITimeRaid_Times"), challengeTimesLeft));
    
    local _max = LuaConst.Const.TimeRaidType * LuaConst.Const.TimeRaidNrByType;
	for i = 1, _max do
		local v = timeRaidLocalDataTable[tostring(i)];
		if (v.class == tostring(currentRaidClass)) then
			--this class
			local innerId = i - (currentRaidClass - 1) * LuaConst.Const.TimeRaidNrByType;
			local difficultyButton = TransformFindChild(difficultyBox.transform, "Scroll View/DifficultyButton"..innerId);
            local difficultyLabel = TransformFindChild(difficultyBox.transform, string.format("Scroll View/DifficultyButton%d/Label", innerId));

			if( Role.Get_lv() >= v.unlock_lv) then
				--unlocked
				AddOrChangeClickParameters(difficultyButton.gameObject, OnClickDifficultyButton, { id = v.id, available = (challengeNum < maxChallengeTimes ) } );
				--colorize the button
                UIHelper.GrayWidget(difficultyButton, false);
                UIHelper.SetWidgetColor(difficultyLabel, "win_w_24");
--				GameObjectSetActive(difficultyLv, false);
			else
				--show unlock level
				Util.RmvEventListener(difficultyButton.gameObject);
                UIHelper.GrayWidget(difficultyButton, true);
				--discolorize the button
                UIHelper.SetWidgetColor(difficultyLabel, Color.New(80 / 255, 80 / 255, 80 / 255, 1));
--				UIHelper.SetLabelTxt(difficultyLv, v.unlock_lv);
--				GameObjectSetActive(difficultyLv, true);
			end
		end
	end
end

function RefreshInfo()
	if(currentRaidClass == nil) then
		print("currentRaidClass is nil, but you are trying to refresh it, this shouldn't happen");
		return;
	end

	--active it
	if(difficultyBox == nil) then
    	return;
    end
	--set info
	SetInfo(currentRaidClass);
end

function OnClickDifficultyButton(go)
	local listener = UIHelper.GetUIEventListener(go);
	if (listener ~= nil and listener.parameter.id ~= nil) then
		if(listener.parameter.available)then
			function onChallenge()
	            if (delegateOnSelectDifficulty ~= nil) then
	                delegateOnSelectDifficulty(listener.parameter.id);
	            end
	            OnClickClose();
	        end
	        
	        WindowMgr.ShowWindow(LuaConst.Const.UIChallengeTimeRaid, { listener.parameter.id, { OnChallenge = onChallenge } });
		else
			WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { GetLocalizedString("UITimeRaidTimesRunOut") });
			return;
		end        
	else
		print("listener or difficulty id is nil");
	end
end

function OnClickClose()
	if(difficultyBox == nil) then
    	return;
    end
	if(GameObjectActiveSelf(difficultyBox)) then
    	GameObjectSetActive(difficultyBox.transform,false);
    end
end

function OnDestroy()
	difficultyBox = nil;
	uiLabelChallengeNum = nil;
	uiLabelEnergycost = nil;
	timeRaidLocalDataTable = nil;
	delegateOnSelectDifficulty = nil;
    windowComponent = nil;
end