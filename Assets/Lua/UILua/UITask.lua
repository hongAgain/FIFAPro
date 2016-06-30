module("UITask", package.seeall)

require "Common/UnityCommonScript"
require "Game/TaskData"
require "Config"

--child module
require "UILua/UIDailyTaskController"
require "UILua/UIAchieveTaskController"

--BGM name
local strBGMusic = "BG_Task";

local dailyTaskTab = nil
local achieveTab = nil
local dailyTaskTrans = nil
local achieveTrans = nil
local uiPopupRoot = nil;

local window = nil;
local windowComponent = nil;
local transform = nil

local taskSettings = 
{
	["DailyTask"] = { uiModule = UIDailyTaskController},
	["Achieve"] = { uiModule = UIAchieveTaskController}
}
local curShowKey = ""

function OnStart(gameObject, params)
    print("UITask.lua .. OnStart");
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");
    transform = window.transform

    BindUI();
    AudioMgr.Instance():PlayBGMusic(strBGMusic);
    InitSubWinUI()
end

function BindUI()
	dailyTaskTab = TransformFindChild(transform,"Top/TitleTab/DailyTask");
	achieveTab = TransformFindChild(transform,"Top/TitleTab/Achieve");
	dailyTaskTrans = TransformFindChild(transform,"PopupRoot/UITaskDaily")
	achieveTrans = TransformFindChild(transform,"PopupRoot/UITaskAchieve")

	uiPopupRoot = TransformFindChild(transform,"PopupRoot");

	UIHelper.AddToggle(dailyTaskTab, OnToggleChange)
	UIHelper.AddToggle(achieveTab , OnToggleChange)

	UIHelper.SetToggleState(dailyTaskTab,true)
end

function InitSubWinUI()
	UIDailyTaskController.InitUI(dailyTaskTrans,windowComponent)
	UIAchieveTaskController.InitUI(achieveTrans,windowComponent)
end

function OnToggleChange(selected , trans)
	selectedTrans = TransformFindChild(trans,"Selected")
	unselectedTrans = TransformFindChild(trans,"UnSelected")
	selectedTrans.gameObject:SetActive(selected)
	unselectedTrans.gameObject:SetActive(not selected)
	local nameKey = trans.name
	if selected then
		curShowKey = nameKey
		taskSettings[nameKey].uiModule.OnShow()
	else
		taskSettings[nameKey].uiModule.OnHide()
	end
end

function OnClickDailyTaskTab()
	print("OnClickDailyTaskTab")
	
	--UI


	local AfterRequesting = function ()
		UIHelper.SetButtonSpriteName(uiButtonDailyTab,taskSettings.IconButtonActive);
		UIHelper.SetButtonSpriteName(uiButtonAchieveTab,taskSettings.IconButtonDisactive);
		if(TaskUIDepth==nil) then
			TaskUIDepth = UIHelper.GetMaxDepthOfPanelInChildren(window.transform)+1;
		end
		UIDailyTaskController.CreateDailyTaskUI(window.transform,windowComponent,TaskUIDepth,OnClickTaskItemGetReward,OnClickTaskItemGoto,OnClickTaskItemGetPointReward);
		UIAchieveTaskController.CloseAchieveTaskUI();
	end

	--TaskData.RequestDailyTaskInfo(AfterRequesting);
end

function OnClickAchieveTaskTab()
	print("OnClickAchieveTaskTab")
	do 
		return
	end
	local AfterRequesting = function ()
		UIHelper.SetButtonSpriteName(uiButtonDailyTab,taskSettings.IconButtonDisactive);
		UIHelper.SetButtonSpriteName(uiButtonAchieveTab,taskSettings.IconButtonActive);	
		if(TaskUIDepth==nil) then
			TaskUIDepth = UIHelper.GetMaxDepthOfPanelInChildren(window.transform)+1;
		end
		UIDailyTaskController.CloseDailyTaskUI();
		UIAchieveTaskController.CreateAchieveTaskUI(window.transform,windowComponent,TaskUIDepth,OnClickTaskItemGetReward,OnClickTaskItemGoto);
	end
	TaskData.RequestAchieveTaskInfo(AfterRequesting);

end

function SelectTab( transform,selected )
	-- body
end

function OnClickTaskItemGoto(go)	
	local listener = UIHelper.GetUIEventListener(go);
	if(listener == nil) then
        print ("listener is nil");
    else
    	print("Daily Task Item "..listener.parameter.id.." is taking you to somewhere faraway");
    end
end

function OnClickTaskItemGetReward(go)
	local listener = UIHelper.GetUIEventListener(go);
	if(listener == nil) then
        print ("listener is nil");
    else
    	local AfterRequesting = function ()
    		--remove completed task:listener.parameter.id
    		TaskData.DeleteDailyTask(listener.parameter.id);
    		UIDailyTaskController.RefreshTaskUI();
    	end
    	TaskData.RequestDailyTaskSubmit({id=listener.parameter.id},AfterRequesting);
    end
end

function OnClickTaskItemGetPointReward(go)
	-- body
	local listener = UIHelper.GetUIEventListener(go);
	if(listener == nil) then
        print ("listener is nil");
    else
    	local AfterRequesting = function ()
    		--remove completed task:listener.parameter.id
    		TaskData.UpdateAchieveTask(listener.parameter.id);
    		UIAchieveTaskController.RefreshTaskUI();
    	end
    	TaskData.RequestDailyTaskAward({id=listener.parameter.id},AfterRequesting);
    end
end


function OnClickAchieveTaskItemGoto(go)
	local listener = UIHelper.GetUIEventListener(go);
	if(listener == nil) then
        print ("listener is nil");
    else
    	print("Achieve Task Item "..listener.parameter.id.." is taking you to somewhere faraway");
    end
end

function OnClickAchieveTaskItemGetReward(go)
	local listener = UIHelper.GetUIEventListener(go);
	if(listener == nil) then
        print ("listener is nil");
    else
    	local AfterRequestingInfo = function ()
    		UIAchieveTaskController.RefreshTaskUI();
    	end
    	local AfterRequesting = function ()
    		TaskData.RequestAchieveTaskInfo(AfterRequestingInfo);
    	end
    	TaskData.RequestAchieveTaskSubmit({id=listener.parameter.id},AfterRequesting);
    end
end

function OnHide()
    print("..OnHide UITask");
end

function OnShow()
	taskSettings[curShowKey].uiModule.OnShow()
end

function OnDestroy()
	UIDailyTaskController.OnDestroy();
	UIAchieveTaskController.OnDestroy();
	UITaskItemManager.OnDestroy();
	uiButtonDailyTab = nil;
	uiButtonAchieveTab = nil;
	uiPopupRoot = nil;
	TaskUIDepth = nil;
	window = nil;
	windowComponent = nil;
end