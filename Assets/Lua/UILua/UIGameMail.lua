module("UIGameMail", package.seeall)

require "Common/UnityCommonScript"
require "Game/GameMailData"

local mailSettings = {
    BackgroundColorUnRead = Color.New(28/255,28/255,33/255,255/255),
    BackgroundColorRead = Color.New(28/255,28/255,33/255,204/255),
    IconColorUnRead = Color.New(102/255,204/255,255/255,255/255),
    IconColorRead = Color.New(171/255,173/255,185/255,255/255),
    TitleColorUnRead = "win_wb_24",
    TitleColorRead = "win_w_24",
    MailType = {},
    -- rewardMail = "Mail_reward",
    -- systemMail = "Mail_System",
    -- playerMail = "Mail_Socially",
    pageSize = 10
}

--BGM name
local strBGMusic = "BG_GameMail";

local mailScrollView = nil;
local mailListContainer = nil;
local uiPopupRoot = nil;

local unReadNum = nil;
local unreadnum = nil;
local currentTotalDataNum = nil;
local lastTotalDataNum = nil;
local nextPageStartIndex = nil;

local currentMailReqStart = nil;
local uiMailItems = {};
local mailItemPrefabName = "UIGameMailItem";

-- you should use a similar way like shop module to handle switching between multi-situations
-- following is a lazy choice
local mailMode = {Normal = 1,MultiEdit = 2};
local currentMode = mailMode.Normal;

local window = nil;
local windowComponent = nil;

function OnStart(gameObject, params)
    print("UIGameMail.lua .. OnStart");
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");
   
    --fetch data then set UI
    PrepareLocalData();
    BindUI();
    AudioMgr.Instance():PlayBGMusic(strBGMusic);

    local AfterRequesting = function ()
        SetInfo(false);
        currentMailReqStart = currentMailReqStart + GameMailData.Get_ListDataLength();
    end    
    currentMailReqStart = 1;
    GameMailData.RequestGameMailList({start=currentMailReqStart,num=mailSettings.pageSize},AfterRequesting);
end

function PrepareLocalData()
    mailSettings.MailType = {};
    mailSettings.MailType[0] = "Mail_System";
    mailSettings.MailType[1] = "Mail_reward";
    mailSettings.MailType[2] = "Mail_Socially";
end

function BindUI()
	local transform = window.transform;

    unReadNum = TransformFindChild(transform,"UnReadNum");
	
    mailScrollView = TransformFindChild(transform,"uiScrollView");
    mailListContainer = TransformFindChild(transform,"uiScrollView/uiContainer");
	uiPopupRoot = TransformFindChild(transform,"PopupRoot");
end

function SetInfo(indexToNextPage,defaultContainerPos)

    --destroy old items

    DestroyUIListItemGameObjects(uiMailItems);
    -- for i,v in ipairs(uiMailItems) do 
    --     GameObjectSetActive(v.gameObject,false);
    --     GameObjectDestroy(v.gameObject);
    --     uiMailItems[i] = nil;
    -- end
    uiMailItems = {};

    --this is a list
    local totalListDataDict = GameMailData.Get_TotalListData();
    if(totalListDataDict == nil or IsTableEmpty(totalListDataDict)) then
        UIHelper.SetLabelTxt(unReadNum,0);
        return;
    end

    --export to a list and sort it
    local totalListDataList = {};
    local listCount = 1;
    for k,v in pairs(totalListDataDict) do
        totalListDataList[listCount] = v;
        listCount = listCount+1;
    end
    table.sort(totalListDataList,
        function (a,b)
            if(IsMailRead(a) and IsMailRead(b))then
                --all read
                return (a.time>b.time);
            elseif(IsMailRead(b) and not IsMailRead(a))then
                return true;
            elseif(IsMailRead(a) and not IsMailRead(b))then
                return false;
            else
                --not read all
                return (a.time>b.time);
            end
        end);

    unreadnum = 0;
    currentTotalDataNum = 0;
    --create new items
    local mailItemPrefab = windowComponent:GetPrefab(mailItemPrefabName);
    for i,v in ipairs(totalListDataList) do
        -- print("in loop"..i);
        uiMailItems[i] = {};
        --instantiate prefab and initialize it
        local clone = GameObjectInstantiate(mailItemPrefab);
        clone.transform.parent = mailListContainer;
        clone.transform.localPosition = Vector3.zero;
        clone.transform.localScale = Vector3.one;

        uiMailItems[i].gameObject = clone;
        uiMailItems[i].transform = clone.transform;
        uiMailItems[i].mailDetail = v;

        --bind ui
        uiMailItems[i].detailTitle = TransformFindChild(uiMailItems[i].transform,"Title");
        uiMailItems[i].detailSender = TransformFindChild(uiMailItems[i].transform,"Sender");
        uiMailItems[i].detailTime = TransformFindChild(uiMailItems[i].transform,"Time");
        uiMailItems[i].detailIcon = TransformFindChild(uiMailItems[i].transform,"Icon");
        uiMailItems[i].detailButton = TransformFindChild(uiMailItems[i].transform,"DetailButton");
        uiMailItems[i].detailChecked = TransformFindChild(uiMailItems[i].transform,"Checked");
        uiMailItems[i].detailBackground = TransformFindChild(uiMailItems[i].transform,"Background");

        --set info
        UIHelper.SetLabelOmitTail(uiMailItems[i].detailTitle,v.title);
        -- UIHelper.OmitLabelTail(uiMailItems[i].detailTitle);
        UIHelper.SetSpriteName(uiMailItems[i].detailIcon,mailSettings.MailType[v.type]);
        -- if(v.type==0)then
        --     --system
        --     UIHelper.SetSpriteName(uiMailItems[i].detailIcon,mailSettings.systemMail);
        -- elseif(v.type==1)then
        --     --activity
        --     UIHelper.SetSpriteName(uiMailItems[i].detailIcon,mailSettings.rewardMail);
        -- elseif(v.type==2)then
        --     --social
        --     UIHelper.SetSpriteName(uiMailItems[i].detailIcon,mailSettings.playerMail);
        -- end
        UIHelper.SetLabelTxt(uiMailItems[i].detailSender,v.fname);
        -- UIHelper.SetLabelTxt(uiMailItems[i].detailTime,Util.GetTimeToString(v.time));
        UIHelper.SetLabelTxt(uiMailItems[i].detailTime,Util.GetPassedTimeMeasurement(v.time));

        AddOrChangeClickParameters(uiMailItems[i].detailButton.gameObject,OnClickMailItem,{mailID = v._id,indexID = i});
        AddOrChangeClickParameters(uiMailItems[i].gameObject,OnClickMailItem,{mailID = v._id,indexID = i});
        if(v.stat==0 or (v.attr ~= nil and v.attr ~= ""))then
            SetMailRead(uiMailItems[i],false);
            unreadnum = unreadnum+1;
        else
            SetMailRead(uiMailItems[i],true);
        end
        currentTotalDataNum = currentTotalDataNum+1;
        UIHelper.SetDragScrollViewTarget(uiMailItems[i].transform,mailScrollView);
    end

    UIHelper.SetLabelTxt(unReadNum,unreadnum);

    UIHelper.AddDragOnFinish(mailScrollView,DragOnFinish);

    if(indexToNextPage)then
        UIHelper.SetGridPosition(
                mailListContainer,
                mailScrollView,
                NewVector3(0,-62,0),
                -- GetContainerPos(nextPageStartIndex),
                SetContainerPosOnDrag(),
                false);
    else
        UIHelper.SetGridPosition(
                mailListContainer,
                mailScrollView,
                NewVector3(0,-62,0),
                defaultContainerPos or GetContainerPos(nil),
                false);
    end
    
    UIHelper.RefreshPanel(mailScrollView);

    lastTotalDataNum = currentTotalDataNum;
end

function GetContainerPos(startIndex) 
    if(startIndex==nil)then
        return NewVector3(0, 193.53, 0);
    else
        return NewVector3(0, 193.53+(startIndex-1)*104, 0);
    end    
end

function SetContainerPosOnDrag()
     if(lastTotalDataNum<6)then
        return GetContainerPos(1);
    elseif(lastTotalDataNum>currentTotalDataNum-4)then
        if(currentTotalDataNum > 4)then
            return GetContainerPos(currentTotalDataNum-4);
        else
            return GetContainerPos(1);
        end
    else
        return GetContainerPos(lastTotalDataNum);
    end
end

function DragOnFinish()
    if(UIHelper.IsOverDragged(mailScrollView,false,true,false,false))then
        if(currentMode == mailMode.Normal)then
            OnNormalOverDragged();
        elseif(currentMode == mailMode.MultiEdit)then
            OnMultiEditOverDragged();
        end
    end
end

function OnNormalOverDragged()
    if(currentMailReqStart>=100)then
    -- if(GameMailData.ForbidToReqMore())then
        return;
    end
    local AfterRequesting = function ()
        nextPageStartIndex = unreadnum+1;
        SetInfo(true);
        currentMailReqStart = currentMailReqStart + GameMailData.Get_ListDataLength();
    end    
    GameMailData.RequestGameMailList({start=currentMailReqStart,num=mailSettings.pageSize},AfterRequesting);
end

function OnMultiEditOverDragged()

end

function OnClickMailItem(go)
    local listener = UIHelper.GetUIEventListener(go);
    if(listener~=nil and listener.parameter.mailID ~= nil) then
        if(currentMode == mailMode.Normal)then
            OnClickToLookUpDetail(listener.parameter);
        elseif(currentMode == mailMode.MultiEdit)then
            OnClickToMultiEdit(listener.parameter);
        end
    else
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { "listener or mailID is nil"});
        print("listener or mailID is nil");
    end
end

function OnClickToLookUpDetail(IdParams)
    local mailDetail = GameMailData.Get_OneDetailData(IdParams.mailID);
    local popupRootDepth = UIHelper.GetMaxDepthOfPanelInChildren(uiPopupRoot)+1;
    if(GameMailData.IsListDataContain(IdParams.mailID) and mailDetail ~= nil) then
        --you have this data locally
        OpenMailDetailBox(IdParams.indexID,mailDetail);
    else
        --you don't have it ,request
        local AfterRequesting = function()       
           OpenMailDetailBox(IdParams.indexID,GameMailData.Get_OneDetailData(IdParams.mailID));
        end
        GameMailData.RequestGameMailDetail({id = IdParams.mailID},AfterRequesting);
    end   
end

function OnClickToMultiEdit(IdParams)
    -- body
end

function OpenMailDetailBox( mailIndexID, MailDetail )
    if(MailDetail.attr == nil or MailDetail.attr == "") then
        local AfterCloseDetail = function ()
            UpdateReadStat(MailDetail._id,mailIndexID);
        end
        WindowMgr.ShowWindow(LuaConst.Const.UIGameMailDetailBox,{
            mailData = MailDetail,
            mailIndex = mailIndexID,
            delegateOnGet = OnClickDetailGet,
            delegateOnDel = OnClickDetailDel,
            delegateOnClose = OnClickDetailClose
            -- delegateOnClose = AfterCloseDetail
        });
    else
        local AfterGetAndDelete = function (mailID,indexID)
            OnClickDetailGet(mailID,indexID);
            UpdateReadStat(mailID,indexID);
        end
        WindowMgr.ShowWindow(LuaConst.Const.UIGameMailDetailBox,{
            mailData = MailDetail,
            mailIndex = mailIndexID,
            delegateOnGet = OnClickDetailGet,
            delegateOnDel = OnClickDetailDel,
            delegateOnClose = OnClickDetailClose
            -- delegateOnClose = OnClickDetailClose
        });
    end           
end

function UpdateReadStat(mailID,indexID)
    GameMailData.UpdateReadStat(mailID);
    for k,v in pairs(uiMailItems) do
        if(v.mailDetail._id == mailID) then
            SetMailRead(v,true)
            return
        end
    end
end

function IsMailRead(mailData)
    if(mailData.stat == 0 or (mailData.attr~=nil and mailData.attr~=""))then
        return false;
    end
    return true;
end

function SetMailRead(mailItem,isRead)
    if(isRead)then
        GameObjectSetActive(mailItem.detailButton,false);
        GameObjectSetActive(mailItem.detailChecked,true);
        UIHelper.SetWidgetColor(mailItem.detailTitle,mailSettings.TitleColorRead);
        UIHelper.SetWidgetColor(mailItem.detailIcon,mailSettings.IconColorRead);
        UIHelper.SetWidgetColor(mailItem.detailBackground,mailSettings.BackgroundColorRead);
    else        
        GameObjectSetActive(mailItem.detailButton,true);
        GameObjectSetActive(mailItem.detailChecked,false);
        UIHelper.SetWidgetColor(mailItem.detailTitle,mailSettings.TitleColorUnRead);
        UIHelper.SetWidgetColor(mailItem.detailIcon,mailSettings.IconColorUnRead);
        UIHelper.SetWidgetColor(mailItem.detailBackground,mailSettings.BackgroundColorUnRead);
    end
end

function OnClickDetailGet(mailID,indexID)
    GameMailData.GetMailAttr(mailID);
    if(indexID<6)then
        SetInfo(false,GetContainerPos(1));
    elseif(indexID>currentTotalDataNum-4)then
        SetInfo(false,GetContainerPos(currentTotalDataNum-4));
    else
        SetInfo(false,GetContainerPos(indexID));
    end
end

function OnClickDetailDel(mailID,indexID)
    OnClickDetailGet(mailID,indexID);
    -- GameMailData.DeleteMailData(mailID,indexID); 
    -- currentTotalDataNum = currentTotalDataNum-1;
    -- if(indexID<6)then
    --     SetInfo(false,GetContainerPos(1));
    -- elseif(indexID>currentTotalDataNum-4)then
    --     SetInfo(false,GetContainerPos(currentTotalDataNum-4));
    -- else
    --     SetInfo(false,GetContainerPos(indexID));
    -- end
    -- currentMailReqStart = currentMailReqStart-1;
end

function OnClickDetailClose(mailID,indexID)
    -- set this mail to read status
    UpdateReadStat(mailID,indexID);
end

function OnHide()

end

function OnShow()
    SetInfo(false);
end

function OnDestroy()
    mailScrollView = nil;
    mailListContainer = nil;
    uiPopupRoot = nil;
    unReadNum = nil;
    unreadnum = nil;
    nextPageStartIndex = nil;
    currentMailReqStart = nil;
    uiMailItems = {};
    currentMode = mailMode.Normal;
    window = nil;
    windowComponent = nil;
end
