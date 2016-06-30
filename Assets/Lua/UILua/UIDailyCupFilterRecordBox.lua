module("UIDailyCupFilterRecordBox",package.seeall);

--require "UILua/UIDailyCupScript";
require "Common/UnityCommonScript"

local filterSettings = {
    FilterTitle = "DailyCupFilterTitle",
    FilterRoundNum = "DailyCupFilterRoundNum",
    FilterNoRoundNum = "DailyCupFilterCalculating",
    FilterNotRegistered = "DailyCupFilterNotRegistered",
    RoundNum = "RoundNum"
}

local recordBoxPrefabName = "DailyCupFilterRecords";
local recordBoxItemPrefabName = "DailyCupFilterRecordItem";
local recordBoxItemPrefab = nil;
local recordBox = nil;
local recordItems = {};

local recordBoxButtonClose = nil;
local recordBoxButtonConfirm = nil;
local delegateOnClickClose = nil;
local uiTitle = nil;
local uiRoundNum = nil;

local uiScrollView = nil;
local uiContainer = nil;


function CreateRecordBox(containerTransform,windowComponent,sign,onclose)
	if(recordBox == nil) then
		recordBoxItemPrefab = windowComponent:GetPrefab(recordBoxItemPrefabName);
		--instantiate one
		local recordBoxPrefab = windowComponent:GetPrefab(recordBoxPrefabName);
		--instantiate prefab and initialize it
		recordBox = GameObjectInstantiate(recordBoxPrefab);
		recordBox.transform.parent = containerTransform;
    	recordBox.transform.localPosition = Vector3.zero;
    	recordBox.transform.localScale = Vector3.one;

    	recordBoxButtonClose = TransformFindChild(recordBox.transform,"ButtonClose");
    	recordBoxButtonConfirm = TransformFindChild(recordBox.transform,"ButtonConfirm");
		Util.AddClick(recordBoxButtonClose.gameObject,OnClickClose);
		Util.AddClick(recordBoxButtonConfirm.gameObject,OnClickConfirm);

		uiTitle = TransformFindChild(recordBox.transform,"Title");
		uiRoundNum = TransformFindChild(recordBox.transform,"RoundNum");
		uiScrollView = TransformFindChild(recordBox.transform,"RecordList");
		uiContainer = TransformFindChild(uiScrollView,"Container");
	end
	--active it
	if(not GameObjectActiveSelf(recordBox)) then
    	GameObjectSetActive(recordBox.transform,true);
    end
	--set info
	SetInfo(sign,onclose);
    AddDragFinishOnCondition();
end

function SetInfo(sign,onclose)

	delegateOnClickClose = onclose;
	UIHelper.SetLabelTxt(uiTitle,GetLocalizedString(filterSettings.FilterTitle,sign));
	
	--destroy old items
    for i,v in ipairs(recordItems) do 
        GameObjectSetActive(v.gameObject,false);
        GameObjectDestroy(v.gameObject);
        recordItems[i] = nil;
    end
    recordItems = {};
    
	local massRecordData = PVPMsgManager.GetPVPData(MsgID.tb.DailyCupMass);
    if(massRecordData == nil or massRecordData.log == nil or #massRecordData.log == 0) then        
        UIHelper.SetLabelTxt(uiRoundNum,GetLocalizedString(filterSettings.FilterNotRegistered));
        return;
    end
    UIHelper.SetLabelTxt(uiRoundNum,GetLocalizedString(filterSettings.FilterRoundNum,#massRecordData.log));
    --create new items
    for i,v in ipairs(massRecordData.log) do
        recordItems[i] = {};
        --instantiate prefab and initialize it
        local clone = GameObjectInstantiate(recordBoxItemPrefab);
        clone.transform.parent = uiContainer;
        clone.transform.localPosition = Vector3.zero;
        clone.transform.localScale = Vector3.one;

        --bind ui
        recordItems[i].gameObject = clone;
        recordItems[i].transform = clone.transform;
        recordItems[i].BG = TransformFindChild(recordItems[i].transform,"BG");
        recordItems[i].round = TransformFindChild(recordItems[i].transform,"RoundNum");
        recordItems[i].result = TransformFindChild(recordItems[i].transform,"Result");
        recordItems[i].noMatch = TransformFindChild(recordItems[i].transform,"NoMatch");
		recordItems[i].Players = {};        
        for j=1,2 do
        	recordItems[i].Players[j] = {};
	        recordItems[i].Players[j].transform = TransformFindChild(recordItems[i].transform,"Player"..j);
	        recordItems[i].Players[j].Icon = TransformFindChild(recordItems[i].Players[j].transform,"IconRoot/PlayerIcon");
	        recordItems[i].Players[j].Name = TransformFindChild(recordItems[i].Players[j].transform,"PlayerName");
        end

        if(i%2 == 1)then
        	UIHelper.SetWidgetColor(recordItems[i].BG,Color.New(28/255,28/255,33/255,179/255));
        else
        	UIHelper.SetWidgetColor(recordItems[i].BG,Color.New(204/255,204/255,241/255,26/255));
        end

        --set info
        UIHelper.SetLabelTxt(recordItems[i].round,GetLocalizedString(filterSettings.RoundNum,v.round+1));
        UIHelper.SetLabelTxt(recordItems[i].result,v.score[1]..":"..v.score[2]);

        local myData = massRecordData.users[v.user[1]];
        Util.SetUITexture(recordItems[i].Players[1].Icon, LuaConst.Const.ClubIcon, tostring(myData.icon).."_2", true);
        UIHelper.SetLabelTxt(recordItems[i].Players[1].Name,myData.name);

        local enemyData = massRecordData.users[v.user[2]];
        if(enemyData~=nil)then
            GameObjectSetActive(recordItems[i].Players[2].Icon,true);
            GameObjectSetActive(recordItems[i].Players[2].Name,true);
            GameObjectSetActive(recordItems[i].result,true);
            GameObjectSetActive(recordItems[i].noMatch,false);

            Util.SetUITexture(recordItems[i].Players[2].Icon, LuaConst.Const.ClubIcon, tostring(enemyData.icon).."_2", true);
            UIHelper.SetLabelTxt(recordItems[i].Players[2].Name,enemyData.name);
        else
            GameObjectSetActive(recordItems[i].Players[2].Icon,false);
            GameObjectSetActive(recordItems[i].Players[2].Name,false);
            GameObjectSetActive(recordItems[i].result,false);
            GameObjectSetActive(recordItems[i].noMatch,true);
        end
    end
    UIHelper.RepositionGrid(uiContainer,uiScrollView);
    UIHelper.RefreshPanel(uiScrollView);
end

function OnClickClose()
	if(delegateOnClickClose~=nil) then
		delegateOnClickClose();
	end
	Close();	
end

function OnClickConfirm()	
	if(delegateOnClickClose~=nil) then
		delegateOnClickClose();
	end
	Close();	
end

function AddDragFinishOnCondition()
    if(#recordItems<5)then
        UIHelper.AddDragOnFinish(uiScrollView,OnDragFinish);    
    end
end

function OnDragFinish()
    ScrollToTop();
end

--targetIndex start from 1
function ScrollToTop()
    BeforeScroll();
    UIHelper.ScrollClippedPanelTo(uiScrollView,GetScrollPos(),8,AfterScroll);
end

--targetColumn start from 0
function GetScrollPos()
    return NewVector3(0,-50,0);
end

function BeforeScroll()

end

function AfterScroll()
    Util.EnableScript(uiScrollView.gameObject,"SpringPanel",false);
end

function Close()
	if(GameObjectActiveSelf(recordBox)) then
    	GameObjectSetActive(recordBox.transform,false);
    end
end

function OnDestroy()
	recordBoxItemPrefab = nil;
	recordBox = nil;
	recordItems = {};
	recordBoxButtonClose = nil;
	recordBoxButtonConfirm = nil;
	delegateOnClickClose = nil;
	uiTitle = nil;
	uiRoundNum = nil;
	uiScrollView = nil;
	uiContainer = nil;
end
