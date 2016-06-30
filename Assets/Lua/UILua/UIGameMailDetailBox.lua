module("UIGameMailDetailBox", package.seeall);

require "Common/UnityCommonScript"
require "Game/GameMailData"
require "UILua/UIItemIcon"

local attrItemPrefabName = "UIGameMailAttrItem";
local typeSuffix = "Mail";

local attrTitle = "MailAttrsGet";

local uiMailTitle = nil;
local uiMailSender = nil;
local uiMailType = nil;
local uiMailTime = nil;
local uiMailMsg = nil;

-- local uiAttrScrollView = nil;
local uiAttrNode = nil;
local uiAttrContainer = nil;
local uiAttrItems = {};

local detailBoxButtonClose = nil;
local detailBoxButtonGet = nil;
local detailBoxButtonDel = nil;
local detailBoxButtonOK = nil;

local mailData = nil;
local mailIndex = nil;
local delegateOnGet = nil;
local delegateOnDel = nil;
local delegateOnClose = nil;

local mailGetLock = false;
local mailDelLock = false;

local MailType = {};

local window = nil;
local windowComponent = nil;

function OnStart(gameObject, params)
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");

    mailData = params.mailData;
    mailIndex = params.mailIndex;
    delegateOnGet = params.delegateOnGet;
    delegateOnDel = params.delegateOnDel;
    delegateOnClose = params.delegateOnClose;
    
    --fetch data then set UI
    PrepareLocalData();
    BindUI();
    SetInfo();
end

function PrepareLocalData()
	if(IsTableEmpty(MailType))then
		MailType = {};
		MailType[0] = GetLocalizedString("MailTypeSystem");
		MailType[1] = GetLocalizedString("MailTypeActivity");
		MailType[2] = GetLocalizedString("MailTypeSocial");
	end
end

function BindUI()
	local transform = window.transform;	

	--buttons
	detailBoxButtonClose = TransformFindChild(transform,"ButtonClose");
	detailBoxButtonGet = TransformFindChild(transform,"ButtonGet");
	detailBoxButtonDel = TransformFindChild(transform,"ButtonDel");
	detailBoxButtonOK = TransformFindChild(transform,"ButtonOK");
	AddOrChangeClickParameters(detailBoxButtonClose.gameObject,OnClickClose,nil);
	AddOrChangeClickParameters(detailBoxButtonGet.gameObject,OnClickGet,nil);
    AddOrChangeClickParameters(detailBoxButtonDel.gameObject,OnClickDel,nil);
    AddOrChangeClickParameters(detailBoxButtonOK.gameObject,OnClickClose,nil);
    --labels
	uiMailTitle = TransformFindChild(transform,"Title");
	uiMailSender = TransformFindChild(transform,"Sender");
	uiMailType = TransformFindChild(transform,"Type");
	uiMailTime = TransformFindChild(transform,"Time");
	uiMailMsg = TransformFindChild(transform,"ContentScrollView/Content");
	--attr list
	-- uiAttrScrollView = TransformFindChild(transform,"AttrList");
	uiAttrNode =  TransformFindChild(transform,"ContentScrollView/AttrNode");
	uiAttrContainer = TransformFindChild(uiAttrNode,"Container");
end

function SetInfo()	
	UIHelper.SetLabelOmitTail(uiMailTitle,mailData.title);
	-- UIHelper.OmitLabelTail(uiMailTitle);
	UIHelper.SetLabelTxt(uiMailSender,mailData.fname);
	UIHelper.SetLabelTxt(uiMailType,MailType[mailData.type or 0]);
	UIHelper.SetLabelTxt(uiMailTime, Util.GetTimeToString(mailData.time));
	UIHelper.SetLabelTxt(uiMailMsg,mailData.content);

	--destroy old items
    for i,v in ipairs(uiAttrItems) do 
        GameObjectSetActive(v.gameObject,false);
        GameObjectDestroy(v.gameObject);
        uiAttrItems[i] = nil;
    end
	uiAttrItems = {};

	local attrData = {};

	if(mailData.attr~=nil)then
		local attrArray = mailData.attr:split(",");
		for i,v in ipairs(attrArray) do
			if(i%2 == 1) then
				--this is an attr and i+1 is its num
				table.insert(attrData,{id=attrArray[i],num=attrArray[i+1]});
			end
		end
	end
		
    if(attrData == nil or #attrData == 0) then
    	-- GameObjectSetActive(detailBoxButtonGet.gameObject,false);
    	-- GameObjectSetActive(detailBoxButtonDel.gameObject,true);
    	--always show get button for new logic
    	GameObjectSetActive(detailBoxButtonGet.gameObject,false);
    	GameObjectSetActive(detailBoxButtonDel.gameObject,false);
    	GameObjectSetActive(detailBoxButtonOK.gameObject,true);
    	GameObjectSetActive(uiAttrNode.gameObject,false);
        return;
    else
    	-- GameObjectSetActive(detailBoxButtonGet.gameObject,true);
    	-- GameObjectSetActive(detailBoxButtonDel.gameObject,false);
    	--always show get button for new logic
    	GameObjectSetActive(detailBoxButtonGet.gameObject,true);
    	GameObjectSetActive(detailBoxButtonDel.gameObject,false);
    	GameObjectSetActive(detailBoxButtonOK.gameObject,false);
    	GameObjectSetActive(uiAttrNode.gameObject,true);
    end

    uiAttrNode.transform.localPosition = NewVector3(0,uiMailMsg.transform.localPosition.y-UIHelper.HeightOfWidget(uiMailMsg),0);

    --create new items
	--local attrItemPrefab = windowComponent:GetPrefab(attrItemPrefabName);
    local attrItemPrefab = Util.GetGameObject("UIItemIcon");
    for i,v in ipairs(attrData) do

        uiAttrItems[i] = {};
        --instantiate prefab and initialize it
        local clone = AddChild(attrItemPrefab, uiAttrContainer);
        local itemIcon = UIItemIcon.New(clone);
        itemIcon:SetSize("win_wb_20", Vector3.one * 86 / 180);
        itemIcon:Init(v, false);
        uiAttrItems[i].transform = clone.transform;
        uiAttrItems[i].gameObject = clone;

		uiAttrItems[i].transform.name = string.format("%03d",i);

        AddOrChangeClickParameters(uiAttrItems[i].gameObject,OnClickAttrItem,{id=v.id,num=v.num});

        -- UIHelper.SetDragScrollViewTarget(uiAttrItems[i].transform,uiAttrScrollView);
    end

    -- UIHelper.RepositionGrid(uiAttrContainer,uiAttrScrollView);
    -- UIHelper.RefreshPanel(uiAttrScrollView);
end

function OnClickClose()
	if(delegateOnClose~=nil) then
		delegateOnClose(mailData._id,mailIndex);
	end;
	Close();
end

function OnClickGet(go)

	if(mailGetLock)then
		return;
	end
	mailGetLock = true;

	local AfterGetAttr = function ()
		if(delegateOnGet~=nil) then
			delegateOnGet(mailData._id,mailIndex);
		end
		OnClickClose();
		mailGetLock = false;
	end

	local AfterRequestGetAttr = function()
		--access data you got, it's a string(id,num,id,num)
		local attrData = GameMailData.Get_AttrData();
		--show list you get
		local advAwardData = {};
		advAwardData.item = {};
		if(attrData~=nil)then
			for k,v in pairs(attrData) do
				table.insert(advAwardData.item,{id=v.id,num=v.val});
			end
		end

		WindowMgr.ShowWindow(LuaConst.Const.UIGetItem,{
            m_itemTb = advAwardData,
            OnClose = AfterGetAttr,
			titleName = GetLocalizedString(attrTitle)
        });		
	end
	GameMailData.RequestGameMailAttr({id = mailData._id},AfterRequestGetAttr);
end

function OnClickDel(go)
	if(mailDelLock)then
		return;
	end
	mailDelLock = true;

	local AfterRequestDelete = function ()
		if(delegateOnDel~=nil) then
			delegateOnDel(mailData._id,mailIndex);
		end
		OnClickClose();
		mailDelLock = false;
	end
	GameMailData.RequestGameMailDelete({id = mailData._id},AfterRequestDelete);
end

-- function OnClickOK(go)
-- 	if(mailDelLock)then
-- 		return;
-- 	end
-- 	mailDelLock = true;

-- 	local AfterRequestDelete = function ()
-- 		if(delegateOnDel~=nil) then
-- 			delegateOnDel(mailData._id,mailIndex);
-- 		end
-- 		OnClickClose();
-- 		mailDelLock = false;
-- 	end
-- 	GameMailData.RequestGameMailDelete({id = mailData._id},AfterRequestDelete);
-- end

function OnClickAttrItem(go)
	local listener = UIHelper.GetUIEventListener(go);
    if(listener~=nil and listener.parameter~=nil)then
        WindowMgr.ShowWindow(LuaConst.Const.UIItemTips,{id = listener.parameter.id});
    end
end

function Close()
	if(windowComponent~=nil)then
		windowComponent:Close();
	end
end

function OnDestroy()	
	uiMailTitle = nil;
	uiMailSender = nil;
	uiMailType = nil;
	uiMailTime = nil;
	uiMailMsg = nil;
	uiAttrNode = nil;
	uiAttrContainer = nil;
	uiAttrItems = {};
	detailBoxButtonClose = nil;
	detailBoxButtonGet = nil;
	detailBoxButtonDel = nil;
	mailData = nil;
	mailIndex = nil;
	delegateOnGet = nil;
	delegateOnDel = nil;
	delegateOnClose = nil;
	mailGetLock = false;
	mailDelLock = false;
	MailType = {};
	window = nil;
	windowComponent = nil;
end