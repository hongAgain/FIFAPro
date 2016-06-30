module("UILadderMatchShareBox", package.seeall);

require "Common/UnityCommonScript"

local shareBoxPrefabName = "LadderMatchShareBox";
local shareBox = nil;
local shareBoxMatchID = nil;
--uis
local shareBoxMyName = nil
local shareBoxOpponentName = nil;
local shareBoxDescription = nil;

local shareBoxButtonShareToWorld = nil;
local shareBoxButtonShareToGuild = nil;
local shareBoxButtonClose = nil;

local delegateOnClickShareToWorld = nil;
local delegateOnClickShareToGuild = nil;

function CreateShareBox( containerTransform, windowComponent, matchID, myPlayerName, opponentPlayerName, description, delegateOnShareToWorld, delegateOnShareToGuild )
	
	if(shareBox == nil) then
		--instantiate one
		local shareBoxPrefab = windowComponent:GetPrefab(shareBoxPrefabName);
		--instantiate prefab and initialize it
		shareBox = GameObjectInstantiate(shareBoxPrefab);
		shareBox.transform.parent = containerTransform;
    	shareBox.transform.localPosition = Vector3.zero;
    	shareBox.transform.localScale = Vector3.one;

    	delegateOnClickShareToWorld = delegateOnShareToWorld;
    	delegateOnClickShareToGuild = delegateOnShareToGuild;

    	shareBoxMyName = TransformFindChild(shareBox.transform,"MyName");
    	shareBoxOpponentName = TransformFindChild(shareBox.transform,"OpponentName");
	    shareBoxDescription = TransformFindChild(shareBox.transform,"Description");

    	shareBoxButtonShareToWorld = TransformFindChild(shareBox.transform,"ButtonShareToWorldChannel");
    	shareBoxButtonShareToGuild = TransformFindChild(shareBox.transform,"ButtonShareToGuildChannel");
    	shareBoxButtonClose = TransformFindChild(shareBox.transform,"ButtonClose");
		Util.AddClick(shareBoxButtonShareToWorld.gameObject,OnClickShareToWorld);
		Util.AddClick(shareBoxButtonShareToGuild.gameObject,OnClickShareToGuild);
		Util.AddClick(shareBoxButtonClose.gameObject,OnClickCloseButton);
    end
    --SetActive the box
    if(not GameObjectActiveSelf(shareBox)) then
    	GameObjectSetActive(shareBox.transform,true);
    end
    --set infos
    shareBoxMatchID = matchID;
    
    UIHelper.SetLabelTxt(shareBoxMyName,myPlayerName);
    UIHelper.SetLabelTxt(shareBoxOpponentName,opponentPlayerName);
    UIHelper.SetLabelTxt(shareBoxDescription,description);
end

function OnClickShareToWorld()
	if(shareBoxMatchID ~= nil and delegateOnClickShareToWorld ~= nil) then
		delegateOnClickShareToWorld(shareBoxMatchID);
	end
end

function OnClickShareToGuild()
	if(shareBoxMatchID ~= nil and delegateOnClickShareToGuild ~= nil) then
		delegateOnClickShareToGuild(shareBoxMatchID);
	end
end

function OnClickCloseButton()
	print("===== > OnClickCloseButton");
	-- disactive the box
	if(GameObjectActiveSelf(shareBox)) then
    	GameObjectSetActive(shareBox.transform,false);
    end
end

function OnDestroy()
    shareBox = nil;
    shareBoxMatchID = nil;
    
    shareBoxMyName = nil
    shareBoxOpponentName = nil;
    shareBoxDescription = nil;

    shareBoxButtonShareToWorld = nil;
    shareBoxButtonShareToGuild = nil;
    shareBoxButtonClose = nil;

    delegateOnClickShareToWorld = nil;
    delegateOnClickShareToGuild = nil;
end