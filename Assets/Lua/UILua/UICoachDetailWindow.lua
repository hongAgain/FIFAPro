module("UICoachDetailWindow",package.seeall)

require "Game/CoachData"
require "UILua/UICoachDetailManager"
require "UILua/UICoachAdvManager"
require "UILua/UICoachDoUManager"

local detailWindowSettings = {
	SelectedLineColorSelected = Color.New(162/255,166/255,169/255,255/255),
	SelectedLineColorNormal = Color.New(162/255,166/255,169/255,0/255),

	ButtonLabelColorNormal = Color.New(171/255, 173/255, 185/255, 255/255),
	ButtonLabelColorSelected = Color.New(255/255, 255/255, 255/255, 255/255),
}

local window = nil;
local windowComponent = nil;

local buttonDetail = nil;
local buttonAdv = nil;
local buttonDoU = nil;
local buttonClose = nil;

local currentTab = nil;
local currentCoachID = nil;

--params {openTab :1 for detail, 2 for adv, 3 for DoU up, coachID}
function OnStart(gameObject, params)
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");

	Initialize();
    BindUI();

    currentTab = (params and params.openTab) or 1;
    currentCoachID = (params and params.coachID);
    print("currentCoachID:"..currentCoachID);
    SetInfo();
end

function Initialize()
	UICoachDetailManager.Initialize(window.transform,windowComponent,Close);
	UICoachAdvManager.Initialize(window.transform,windowComponent);
	UICoachDoUManager.Initialize(window.transform,windowComponent);
end

function BindUI()
	buttonDetail = {};
	buttonDetail.transform = TransformFindChild(window.transform,"ButtonDetail");
	buttonDetail.gameObject = buttonDetail.transform.gameObject;
	buttonDetail.label = TransformFindChild(buttonDetail.transform,"Label");
	buttonDetail.selectedLine = TransformFindChild(buttonDetail.transform,"SelectedLine");

	buttonAdv = {};
	buttonAdv.transform = TransformFindChild(window.transform,"ButtonAdv");
	buttonAdv.gameObject = buttonAdv.transform.gameObject;
	buttonAdv.label = TransformFindChild(buttonAdv.transform,"Label");
	buttonAdv.selectedLine = TransformFindChild(buttonAdv.transform,"SelectedLine");

	buttonDoU = {};
	buttonDoU.transform = TransformFindChild(window.transform,"ButtonDoU");
	buttonDoU.gameObject = buttonDoU.transform.gameObject;
	buttonDoU.label = TransformFindChild(buttonDoU.transform,"Label");
	buttonDoU.selectedLine = TransformFindChild(buttonDoU.transform,"SelectedLine");

	buttonClose = TransformFindChild(window.transform,"ButtonClose");

	AddOrChangeClickParameters(buttonDetail.gameObject,OnClickButtonDetail,nil);
	AddOrChangeClickParameters(buttonAdv.gameObject,OnClickButtonAdv,nil);
	AddOrChangeClickParameters(buttonDoU.gameObject,OnClickButtonDoU,nil);
	AddOrChangeClickParameters(buttonClose.gameObject,OnClickButtonClose,nil);
end

function SetInfo()
	if(currentTab == 1)then
		OnClickButtonDetail();
	elseif(currentTab == 2)then
		OnClickButtonAdv();
	elseif(currentTab == 3)then
		OnClickButtonDoU();
	end
end

function OnClickButtonDetail()
	UICoachDetailManager.ShowCoachDetail(true,currentCoachID);
	UICoachAdvManager.ShowCoachAdv(false,currentCoachID);
	UICoachDoUManager.ShowCoachDoU(false,currentCoachID);

	FadeButton(buttonDetail,false);
	FadeButton(buttonAdv,true);
	FadeButton(buttonDoU,true);
end

function OnClickButtonAdv()
	UICoachDetailManager.ShowCoachDetail(false,currentCoachID);
	UICoachAdvManager.ShowCoachAdv(true,currentCoachID);
	UICoachDoUManager.ShowCoachDoU(false,currentCoachID);

	FadeButton(buttonDetail,true);
	FadeButton(buttonAdv,false);
	FadeButton(buttonDoU,true);
end

function OnClickButtonDoU()
	UICoachDetailManager.ShowCoachDetail(false,currentCoachID);
	UICoachAdvManager.ShowCoachAdv(false,currentCoachID);
	UICoachDoUManager.ShowCoachDoU(true,currentCoachID);

	FadeButton(buttonDetail,true);
	FadeButton(buttonAdv,true);
	FadeButton(buttonDoU,false);
end

function FadeButton(button,isfadeout)
	if(isfadeout)then
		UIHelper.FadeUIWidgetColorTo(
			button.selectedLine,
			detailWindowSettings.SelectedLineColorNormal,0.5);
		UIHelper.FadeUIWidgetColorTo(
			button.label,
			detailWindowSettings.ButtonLabelColorNormal,0.5);
	else
		UIHelper.FadeUIWidgetColorTo(
			button.selectedLine,
			detailWindowSettings.SelectedLineColorSelected,0.5);
		UIHelper.FadeUIWidgetColorTo(
			button.label,
			detailWindowSettings.ButtonLabelColorSelected,0.5);
	end
end

function AfterFade()
	-- body
end

function OnClickButtonClose()
	Close();
end

function Close()
	windowComponent:Close();
end

function OnDestroy()
	UICoachDetailManager.OnDestroy();
	UICoachAdvManager.OnDestroy();
	UICoachDoUManager.OnDestroy();

	buttonDetail = nil;
	buttonAdv = nil;
	buttonDoU = nil;
	buttonClose = nil;

	currentTab = nil;
	currentCoachID = nil;
end