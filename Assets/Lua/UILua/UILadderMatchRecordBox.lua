module("UILadderMatchRecordBox",package.seeall);


require "Common/UnityCommonScript"
require "UILua/UILadderMatchShareBox"

local ladderRecordSettings = {
	LadderRecordBoxPrefabName = "LadderRecordBox",
	LadderRecordBoxPrefab = nil,

	LadderRecordItemPrefabName = "LadderRecordItem",
	LadderRecordItemPrefab = nil,

	ResultColorWin = Color.New(159/255,255/255,169/255,255/255),
	ResultColorTie = Color.New(255/255,255/255,255/255,255/255),
	ResultColorLoss = Color.New(255/255,102/255,102/255,255/255),

	ResultStrWin = "Win",
	ResultStrTie = "Tie",
	ResultStrLoss = "Loss"
}

local ladderRecordBoxUI = nil;

local uiScrollView = nil;
local uiContainer = nil;
local uiPopupRoot = nil;
local recordItems = {};
local buttonConfirm = nil;
local buttonClose = nil;
local winLabel = nil;
local lossLabel = nil;
local rateLabel = nil;

local winComponent = nil;

function CreateRecordBox(containerTransform, windowComponent, depth)

	if(ladderRecordBoxUI == nil)then
		winComponent = windowComponent;
		ladderRecordSettings.LadderRecordBoxPrefab = windowComponent:GetPrefab(ladderRecordSettings.LadderRecordBoxPrefabName);
		ladderRecordSettings.LadderRecordItemPrefab = windowComponent:GetPrefab(ladderRecordSettings.LadderRecordItemPrefabName);

		ladderRecordBoxUI = GameObjectInstantiate(ladderRecordSettings.LadderRecordBoxPrefab);
		ladderRecordBoxUI.transform.parent = containerTransform;
    	ladderRecordBoxUI.transform.localPosition = Vector3.zero;
    	ladderRecordBoxUI.transform.localScale = Vector3.one;

    	BindUI(depth)
	end
	--active it
	if(not GameObjectActiveSelf(ladderRecordBoxUI)) then
    	GameObjectSetActive(ladderRecordBoxUI.transform,true);
    end

	SetInfo(depth);

end

function BindUI(depth)

	uiScrollView = TransformFindChild(ladderRecordBoxUI.transform,"uiScrollView");
	uiContainer = TransformFindChild(uiScrollView,"uiContainer");
	uiPopupRoot = TransformFindChild(ladderRecordBoxUI.transform,"PopupRoot");
	buttonConfirm = TransformFindChild(ladderRecordBoxUI.transform,"ButtonConfirm");
	buttonClose = TransformFindChild(ladderRecordBoxUI.transform,"ButtonClose");
	winLabel = TransformFindChild(ladderRecordBoxUI.transform,"Statistics/Win");
	lossLabel = TransformFindChild(ladderRecordBoxUI.transform,"Statistics/Lose");
	rateLabel = TransformFindChild(ladderRecordBoxUI.transform,"Statistics/Rate");

	AddOrChangeClickParameters(buttonConfirm.gameObject,OnClickConfirm,nil);
	AddOrChangeClickParameters(buttonClose.gameObject,OnClickClose,nil);

	UIHelper.SetPanelDepth(uiScrollView,depth);
	UIHelper.SetPanelDepth(uiPopupRoot,depth+1);
end

function SetInfo(depth)
	local infoData = PVPMsgManager.GetPVPData(MsgID.tb.LadderInfo);
	DestroyUIListItemGameObjects(recordItems);
	--set statistics
	UIHelper.SetLabelTxt(winLabel,infoData.info.total[1]);
	UIHelper.SetLabelTxt(lossLabel,infoData.info.total[3]);
	local winRate=0;
	local totalNum = (infoData.info.total[1]+infoData.info.total[2]+infoData.info.total[3]);
	if(totalNum==0) then
		winRate = 0;
	else
		winRate = 100*infoData.info.total[1]/(infoData.info.total[1]+infoData.info.total[2]+infoData.info.total[3]);
	end	
	UIHelper.SetLabelTxt(rateLabel,string.format("%.1f",winRate).."%");

	if(infoData.info.logs~=nil)then
		CreateUIListItemGameObjects(uiContainer, infoData.info.logs, ladderRecordSettings.LadderRecordItemPrefab, OnInitItem);
	end
	UIHelper.RepositionGrid(uiContainer,uiScrollView);
	UIHelper.RefreshPanel(uiScrollView);
end

function OnInitItem(randomIndex, key, value, cloneGameObject)	
	recordItems[randomIndex] = {};
	recordItems[randomIndex].gameObject = cloneGameObject;
	recordItems[randomIndex].transform = cloneGameObject.transform;
	--bind ui
	recordItems[randomIndex].Icon = TransformFindChild(recordItems[randomIndex].transform,"IconRoot/Icon");
	recordItems[randomIndex].Level = TransformFindChild(recordItems[randomIndex].transform,"Level");
	recordItems[randomIndex].Name = TransformFindChild(recordItems[randomIndex].transform,"Name");
	recordItems[randomIndex].Result = TransformFindChild(recordItems[randomIndex].transform,"Result");
	recordItems[randomIndex].Score = TransformFindChild(recordItems[randomIndex].transform,"Score");
	recordItems[randomIndex].Time = TransformFindChild(recordItems[randomIndex].transform,"Time");
	recordItems[randomIndex].ButtonShare = TransformFindChild(recordItems[randomIndex].transform,"ButtonShare");
	recordItems[randomIndex].ButtonReplay = TransformFindChild(recordItems[randomIndex].transform,"ButtonReplay");

	--set info
	UIHelper.SetSpriteName(recordItems[randomIndex].Icon,Role.CheckRoleIcon(tostring(value.icon)));
	UIHelper.SetLabelTxt(recordItems[randomIndex].Level,"Lv."..value.lv);
	UIHelper.SetLabelTxt(recordItems[randomIndex].Name,value.name);

	local goal1 = tonumber(value.goal[1]);
	local goal2 = tonumber(value.goal[2]);
	if(goal1 > goal2)then
		UIHelper.SetLabelTxt(recordItems[randomIndex].Result,GetLocalizedString(ladderRecordSettings.ResultStrWin));
		UIHelper.SetWidgetColor(recordItems[randomIndex].Result,ladderRecordSettings.ResultColorWin);
	elseif(goal1 < goal2)then
		UIHelper.SetLabelTxt(recordItems[randomIndex].Result,GetLocalizedString(ladderRecordSettings.ResultStrLoss));
		UIHelper.SetWidgetColor(recordItems[randomIndex].Result,ladderRecordSettings.ResultColorLoss);
	else
		UIHelper.SetLabelTxt(recordItems[randomIndex].Result,GetLocalizedString(ladderRecordSettings.ResultStrTie));
		UIHelper.SetWidgetColor(recordItems[randomIndex].Result,ladderRecordSettings.ResultColorTie);
	end
	UIHelper.SetLabelTxt(recordItems[randomIndex].Score,value.goal[1]..":"..value.goal[2]);
	UIHelper.SetLabelTxt(recordItems[randomIndex].Time,CalculateTimePassed(value.time));

	AddOrChangeClickParameters(recordItems[randomIndex].ButtonReplay.gameObject,OnClickReplayButton,{matchInfo=value});
	AddOrChangeClickParameters(recordItems[randomIndex].ButtonShare.gameObject,OnClickShareButton,{matchInfo=value});
end

function CalculateTimePassed( timeStamp )
	return Util.GetPassedTimeMeasurement(timeStamp);
end

function OnClickReplayButton(go)
	print("===== > OnClickReplayButton");
	WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { "PVP比赛回放功能紧张开发中。。。" });
end

function OnClickShareButton(go)
	local listener = UIHelper.GetUIEventListener(go);

	if(listener == nil or listener.parameter == nil )then
		return;
	end
	-- open sharing box
	UILadderMatchShareBox.CreateShareBox(uiPopupRoot,winComponent,
		listener.parameter.matchInfo.id,
		Role.Get_name(),
		listener.parameter.matchInfo.name,
		listener.parameter.matchInfo.goal[1]..":"..listener.parameter.matchInfo.goal[2],
		OnClickShareToWorld,OnClickShareToGuild);
end

function OnClickShareToWorld(matchID)
	print("===== > OnClickShareToWorld");
	WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { "PVP比赛世界分享功能紧张开发中。。。" });
end

function OnClickShareToGuild(matchID)
	print("===== > OnClickShareToGuild");
	WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { "PVP比赛公会分享功能紧张开发中。。。" });
end

function OnClickClose()
	CloseBox();
end

function OnClickConfirm()
	CloseBox();
end

function CloseBox()
	if(GameObjectActiveSelf(ladderRecordBoxUI)) then
    	GameObjectSetActive(ladderRecordBoxUI.transform,false);
    end
end

function OnDestroy()
	UILadderMatchShareBox.OnDestroy();
	ladderRecordSettings = {
		LadderRecordBoxPrefabName = "LadderRecordBox",
		LadderRecordBoxPrefab = nil,

		LadderRecordItemPrefabName = "LadderRecordItem",
		LadderRecordItemPrefab = nil,

		ResultColorWin = Color.New(159/255,255/255,169/255,255/255),
		ResultColorTie = Color.New(255/255,255/255,255/255,255/255),
		ResultColorLoss = Color.New(255/255,102/255,102/255,255/255),

		ResultStrWin = "Win",
		ResultStrWin = "Tie",
		ResultStrWin = "Loss"
	};

	ladderRecordBoxUI = nil;

	uiScrollView = nil;
	uiContainer = nil;
	uiPopupRoot = nil;
	recordItems = {};
	buttonConfirm = nil;
	buttonClose = nil;
	winLabel = nil;
	lossLabel = nil;
	rateLabel = nil;

	winComponent = nil;
end