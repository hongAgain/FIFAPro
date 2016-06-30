module("UINews",package.seeall);

require "Game/GameNewsData"

local newsSettings = {
	newsDotPrefabName = "DotItem",
	newsDotPrefab = nil,
	dotColorNormal = Color.New(63/255,67/255,78/255,255/255),
	dotColorSelected = Color.New(102/255,204/255,255/255,255/255)
};

local buttonClose = nil;
local buttonLeft = nil;
local buttonRight = nil;

local uiDotContainer = nil;
local uiScrollView = nil;
local contentRoot = nil;

local contentTitle = nil;
local contentMsg = nil;

local dotItems = {};
local newsData = {};
local currentNewsIndex = 1;

local window = nil;
local windowComponent = nil;

function OnStart(gameObject, params)
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");   
    BindUI();
    local AfterRequesting = function ()
	    SetDot();
        ProcessData();
        RefreshContent();
    end
    GameNewsData.RequestGameNewsList( AfterRequesting );
end

function BindUI()
	newsSettings.newsDotPrefab = windowComponent:GetPrefab(newsSettings.newsDotPrefabName);
	local transform = window.transform;
	uiDotContainer = TransformFindChild(transform,"uiDotContainer");
	uiScrollView = TransformFindChild(transform,"uiScrollView");
	contentRoot = TransformFindChild(uiScrollView,"Root");
	contentTitle = TransformFindChild(contentRoot,"Title");
	contentMsg = TransformFindChild(contentRoot,"Content");
	GameObjectSetActive(contentRoot,false);

	buttonClose = TransformFindChild(transform,"ButtonClose");
	buttonLeft =  TransformFindChild(transform,"ButtonLeft");
	buttonRight =  TransformFindChild(transform,"ButtonRight");
	AddOrChangeClickParameters(buttonClose.gameObject,OnClickClose,nil);
	AddOrChangeClickParameters(buttonLeft.gameObject,OnClickButtonLeft,nil);
	AddOrChangeClickParameters(buttonRight.gameObject,OnClickButtonRight,nil);
end

function SetDot()
	local dotCount = GameNewsData.Get_GameNewsCount();
	for i=1,dotCount do
		local clone = GameObjectInstantiate(newsSettings.newsDotPrefab);
        clone.transform.parent = uiDotContainer;
        clone.transform.localPosition = Vector3.zero;
        clone.transform.localScale = Vector3.one;
        dotItems[i] = {};
        dotItems[i].transform = clone.transform;
        dotItems[i].gameObject = clone;
		dotItems[i].transform.name = string.format("%03d",tonumber(i));
	end
	UIHelper.RepositionGrid(uiDotContainer);
end

function ProcessData()
   	newsData = GameNewsData.Get_OneGameNewsData(currentNewsIndex);
end

function RefreshContent()
	if(newsData == nil or IsTableEmpty(newsData)) then
       	return;
	end
	GameObjectSetActive(contentRoot,true);
	--refresh ui
	contentRoot.transform.localPosition = Vector3.zero;
	UIHelper.SetLabelTxt(contentTitle,newsData.name);
	UIHelper.SetLabelTxt(contentMsg,newsData.msg);

	UIHelper.ResetScroll(uiScrollView);
    UIHelper.RefreshPanel(uiScrollView);

    --switch dot
	if(dotItems~=nil and not IsTableEmpty(dotItems))then
		for i,v in ipairs(dotItems) do
			if(currentNewsIndex == i)then
				UIHelper.SetWidgetColor(v.transform,newsSettings.dotColorSelected);
			else
				UIHelper.SetWidgetColor(v.transform,newsSettings.dotColorNormal);
			end
		end
	end
end

function OnClickButtonLeft()
	newsData = GameNewsData.Get_OneGameNewsData(currentNewsIndex-1);
   	if(newsData == nil or IsTableEmpty(newsData)) then
       	return;
	end
	currentNewsIndex = currentNewsIndex-1;
	RefreshContent();
end

function OnClickButtonRight()
	newsData = GameNewsData.Get_OneGameNewsData(currentNewsIndex+1);
   	if(newsData == nil or IsTableEmpty(newsData)) then
       	return;
	end
	currentNewsIndex = currentNewsIndex+1;
	RefreshContent();
end

function OnClickClose()	
	Close();	
end

function Close()
	windowComponent:Close();
end

function OnDestroy()
	newsSettings.newsDotPrefab = nil;
	buttonClose = nil;
	buttonLeft = nil;
	buttonRight = nil;
	uiDotContainer = nil;
	uiScrollView = nil;
	contentRoot = nil;
	contentTitle = nil;
	contentMsg = nil;
	dotItems = {};
	newsData = {};
	currentNewsIndex = 1;
	window = nil;
	windowComponent = nil;
end