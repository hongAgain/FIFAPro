module("UITeamIconList",package.seeall);

local TeamIconListSettings = {
	TeamIconListGroupPrefabName = "IconListGroup",
	TeamIconListGroupPrefab = nil,
	TeamIconListItemPrefabName = "IconListItem",
	TeamIconListItemPrefab = nil,

	GroupTitleHeight = 37,
	GroupItemHeightPerLine = 120,
	GroupItemNumPerLine = 6,

	SortRegulation = function (a,b)
		if(a.lv < b.lv) then
			return true;
		elseif(a.lv == b.lv) then
			if(a.vip < b.vip) then
				return true;
			elseif(a.vip == b.vip) then
				if(a.id < b.id) then
					return true;
				elseif(a.id == b.id) then

				end
			end
		end
		return false;
	end,

	PleaseSelectIcon = "PleaseSelectIcon",
	NormalTeamIcon = "NormalTeamIcon",
	VIPTeamIcon = "VIPTeamIcon",

	UnlockAtLv = "UnlockAtLv",
	UnlockAtVIP = "UnlockAtVIP"
}

local uiScrollView = nil;
local uiContainer = nil;
local iconGroups = {};
local GroupCurrentOffset = 0;

local buttonClose = nil;
local buttonConfirm = nil;
local buttonConfirmSprite = nil;
local buttonConfirmContent = nil;

local normalIconData = {};
local vipIconData = {};

local targetIcon = nil;
local lastIconItem = nil;
local delegateOnConfirmToChangeIcon = nil;

local window = nil;
local windowcomponent = nil;

function OnStart(gameObject, params)
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");

	delegateOnConfirmToChangeIcon = params.delegateOnConfirmToChangeIcon;
	BindUI();
	SetInfo();
end

function BindUI()
	buttonConfirm = TransformFindChild(window.transform,"ButtonConfirm");
	buttonConfirmSprite = TransformFindChild(buttonConfirm,"Sprite");
	buttonConfirmContent = TransformFindChild(buttonConfirm,"Label");
	buttonClose = TransformFindChild(window.transform,"ButtonClose");

	AddOrChangeClickParameters(buttonConfirm.gameObject,OnClickConfirm,nil);
	AddOrChangeClickParameters(buttonClose.gameObject,OnClickClose,nil);

	uiScrollView = TransformFindChild(window.transform,"uiScrollView");
	uiContainer = TransformFindChild(uiScrollView,"uiContainer");
end

function SetInfo()
	
	local iconData = Config.GetTemplate(Config.IconTable());
	normalIconData = {};
	vipIconData = {};
	local normalCount = 1;
	local vipCount = 1;
	for k,v in pairs(iconData) do
		if(v.vip==0)then
			normalIconData[normalCount] = v;
			normalCount = normalCount+1;
		else
			vipIconData[vipCount] = v;
			vipCount = vipCount+1;
		end
	end
	table.sort(normalIconData,TeamIconListSettings.SortRegulation);
	table.sort(vipIconData,TeamIconListSettings.SortRegulation);
	
	--destroy old items
    for k,v in pairs(iconGroups) do 
        GameObjectSetActive(v.gameObject,false);
        GameObjectDestroy(v.gameObject);
        iconGroups[k] = nil;
    end
    iconGroups = {};

    CreateSingleGroup(1,GetLocalizedString(TeamIconListSettings.NormalTeamIcon),normalIconData);
    CreateSingleGroup(2,GetLocalizedString(TeamIconListSettings.VIPTeamIcon),vipIconData);

	lastIconItem = nil;

    UIHelper.ResetScroll(uiScrollView)
    UIHelper.RefreshPanel(uiScrollView);
end

function CreateSingleGroup(index,titleName,dataList)
	--create normal group

	if(TeamIconListSettings.TeamIconListGroupPrefab==nil)then
		TeamIconListSettings.TeamIconListGroupPrefab = windowComponent:GetPrefab(TeamIconListSettings.TeamIconListGroupPrefabName);
	end

	local clone = GameObjectInstantiate(TeamIconListSettings.TeamIconListGroupPrefab);
	iconGroups[index] = {};
	iconGroups[index].gameObject = clone;
	iconGroups[index].transform = clone.transform;
	iconGroups[index].transform.parent = uiContainer;
	iconGroups[index].transform.localPosition = NewVector3(0, GroupCurrentOffset, 0);
	iconGroups[index].transform.localScale = Vector3.one;

	--bind ui
	iconGroups[index].Title = TransformFindChild(iconGroups[index].transform,"Title");
	iconGroups[index].ItemContainer = TransformFindChild(iconGroups[index].transform,"IconListGroup");
	UIHelper.SetLabelTxt(iconGroups[index].Title,titleName);
	iconGroups[index].items = {};

	if(dataList~=nil and dataList~={})then

		if(TeamIconListSettings.TeamIconListItemPrefab==nil)then
			TeamIconListSettings.TeamIconListItemPrefab = windowComponent:GetPrefab(TeamIconListSettings.TeamIconListItemPrefabName);
		end

	    --create new items for normal icons
	    for i,v in ipairs(dataList) do
	    	local itemclone = GameObjectInstantiate(TeamIconListSettings.TeamIconListItemPrefab);
			iconGroups[index].items[i] = {};
			iconGroups[index].items[i].gameObject = itemclone;
			iconGroups[index].items[i].transform = itemclone.transform;
			iconGroups[index].items[i].transform.name = string.format("%03d",i);
			iconGroups[index].items[i].transform.parent = iconGroups[index].ItemContainer;
			iconGroups[index].items[i].transform.localScale = Vector3.one;

			iconGroups[index].items[i].Icon = TransformFindChild(iconGroups[index].items[i].transform,"IconRoot/Icon");
			iconGroups[index].items[i].ActiveBG = TransformFindChild(iconGroups[index].items[i].transform,"ActiveBG");
			iconGroups[index].items[i].DisActiveBG = TransformFindChild(iconGroups[index].items[i].transform,"DisActiveBG");
			iconGroups[index].items[i].Selected = TransformFindChild(iconGroups[index].items[i].transform,"Selected");

			AddOrChangeClickParameters(iconGroups[index].items[i].gameObject,OnClickIconItem,{item = iconGroups[index].items[i],data = v});
			UIHelper.SetDragScrollViewTarget(iconGroups[index].items[i].transform,uiScrollView);
			Util.SetUITexture(iconGroups[index].items[i].Icon,LuaConst.Const.ClubIcon,v.id.."_1", true);
		
			GameObjectSetActive(iconGroups[index].items[i].Selected,false);
			if(v.vip>Role.Get_vip() or v.lv>Role.Get_lv())then
				--locked
				--set grayscale icon
				GameObjectSetActive(iconGroups[index].items[i].ActiveBG,false);
				GameObjectSetActive(iconGroups[index].items[i].DisActiveBG,true);
			else
				--available
				--set colored icon
				GameObjectSetActive(iconGroups[index].items[i].ActiveBG,true);
				GameObjectSetActive(iconGroups[index].items[i].DisActiveBG,false);

			end
	    end
	    SetCurrentOffset(#dataList);
    else
		SetCurrentOffset(0);
    end
end

function SetCurrentOffset( dataLength )
	GroupCurrentOffset = GroupCurrentOffset 
    	- TeamIconListSettings.GroupTitleHeight
    	- TeamIconListSettings.GroupItemHeightPerLine*math.floor((dataLength+5)/TeamIconListSettings.GroupItemNumPerLine);
end


function OnClickIconItem(go)
	local listener = UIHelper.GetUIEventListener(go);
	if(listener~=nil and listener.parameter~=nil)then

		if(listener.parameter.data.vip>Role.Get_vip() )then
			WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm,{GetLocalizedString(TeamIconListSettings.UnlockAtVIP,listener.parameter.data.vip)});
		elseif(listener.parameter.data.lv>Role.Get_lv())then
			WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm,{GetLocalizedString(TeamIconListSettings.UnlockAtLv,listener.parameter.data.lv)});
		else
			if(lastIconItem~=nil)then
				GameObjectSetActive(lastIconItem.Selected,false);
			end
			lastIconItem = listener.parameter.item;
			GameObjectSetActive(lastIconItem.Selected,true);
			targetIcon = listener.parameter.data.id;
			SetConfirmButtonActive(true);
		end
	end
end

function SetConfirmButtonActive(willActivate)
	if(willActivate)then
		UIHelper.SetWidgetColor(buttonConfirmSprite,Color.New(108/255,161/255,101/255,255/255));
		UIHelper.SetWidgetColor(buttonConfirmContent,Color.New(108/255,161/255,101/255,255/255));
	else
		UIHelper.SetWidgetColor(buttonConfirmSprite,Color.New(125/255,128/255,137/255,255/255));
		UIHelper.SetWidgetColor(buttonConfirmContent,Color.New(125/255,128/255,137/255,255/255));
	end
	UIHelper.SetBoxCollider(buttonConfirm,willActivate);
end

function OnClickConfirm()
	if(delegateOnConfirmToChangeIcon~=nil and targetIcon~=nil)then
		delegateOnConfirmToChangeIcon(targetIcon);
		Close();
	else
		WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { GetLocalizedString(TeamIconListSettings.PleaseSelectIcon) });
	end
end

function OnClickClose()
	Close();
end

function Close()
	windowComponent:Close();
end

function OnDestroy()
	uiScrollView = nil;
	uiContainer = nil;
	iconGroups = {};
	GroupCurrentOffset = 0;
	buttonClose = nil;
	buttonConfirm = nil;
	buttonConfirmSprite = nil;
	buttonConfirmContent = nil;
	normalIconData = {};
	vipIconData = {};
	targetIcon = nil;
	lastIconItem = nil;
	delegateOnConfirmToChangeIcon = nil;
	window = nil;
	windowcomponent = nil;
end