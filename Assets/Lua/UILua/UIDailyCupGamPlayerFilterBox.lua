module("UIDailyCupGamPlayerFilterBox",package.seeall);

require "UILua/UIDailyCupGamPlayerList"

local title = nil;

local buttonClose = nil;
local buttonConfirm = nil;
local buttonConfirmLabel = nil;
local buttonConfirmSprite = nil;

local uiScrollView = nil;
local uiContainer = nil;

local delegateOnClickFilter = nil;

local window = nil;
local windowComponent = nil;

function OnStart(gameObject, params)
    print("UIDailyCupGamPlayerFilterBox.lua .. OnStart");
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");

    delegateOnClickFilter = params.delegateOnClickFilter;

    BindUI();
    SetInfo();
end

function BindUI()
	local transform = window.transform;

	title = TransformFindChild(transform,"Title");

	buttonClose = TransformFindChild(transform,"ButtonClose");
	buttonConfirm = TransformFindChild(transform,"ButtonConfirm");
	buttonConfirmLabel = TransformFindChild(buttonConfirm,"Label");
	buttonConfirmSprite = TransformFindChild(buttonConfirm,"Sprite");

	uiScrollView = TransformFindChild(transform,"uiScrollView");
	uiContainer = TransformFindChild(uiScrollView,"uiContainer");

	AddOrChangeClickParameters(buttonConfirm.gameObject,OnClickFilterBoxConfirm,nil);
	AddOrChangeClickParameters(buttonClose.gameObject,OnClickCloseFilterBox,nil);
end

function SetInfo()
    --create filter box content
    if(circleListManager == nil)then
        circleListManager = UICircleListManager.New();
    end
    circleListManager:CreateUICircleList(uiScrollView,
		uiContainer,
		{UIDailyCupGamPlayerList.gamPListSettings.enumPlayerType.All,
		UIDailyCupGamPlayerList.gamPListSettings.enumPlayerType.Forward,
		UIDailyCupGamPlayerList.gamPListSettings.enumPlayerType.Midfielder,
		UIDailyCupGamPlayerList.gamPListSettings.enumPlayerType.Defender,
		UIDailyCupGamPlayerList.gamPListSettings.enumPlayerType.Goalkeeper},
		{
			OnCreateItem = OnInitFilterBoxItem,
			OnClickItem = OnClickFilterBoxItem,
			OnSelectByDrag = OnClickFilterBoxItem
		});	
end

function OnInitFilterBoxItem( randomIndex, key, value, itemNameTrans )
    UIHelper.SetLabelTxt(itemNameTrans,GetLocalizedString(UIDailyCupGamPlayerList.gamPListSettings.enumPlayerName[value]));
end

--( { item=items[randomIndex], data=value } )
function OnClickFilterBoxItem(params)
	if(delegateOnClickFilter~=nil)then
		delegateOnClickFilter(params.data);
	end
end

function OnClickFilterBoxConfirm()
	Close();
end

function OnClickCloseFilterBox()
	Close();
end

function Close()
	windowComponent:Close();
end

function OnDestroy()
	if(circleListManager~=nil)then
		circleListManager:OnDestroy();
	end
	title = nil;
	buttonClose = nil;
	buttonConfirm = nil;
	buttonConfirmLabel = nil;
	buttonConfirmSprite = nil;
	uiScrollView = nil;
	uiContainer = nil;
	delegateOnClickFilter = nil;
end
