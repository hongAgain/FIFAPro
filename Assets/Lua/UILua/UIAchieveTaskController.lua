module("UIAchieveTaskController", package.seeall)

--child module
require "Game/TaskData"
require "UILua/UITaskItemManager"

local prefabAchieveTask = "UITaskAchieve";

local achieveTaskItemPrefab = nil;

local achieveItemTable = {};
local achieveScrollView = nil;
local achieveContainer = nil;
local getCurId = ""

local transform = nil
local windowComponent = nil

function InitUI( trans ,winComponent)
	transform = trans
	windowComponent = winComponent
	UITaskItemManager.Init(windowComponent)

	achieveScrollView = TransformFindChild(transform, "AchieveList")
	achieveContainer = TransformFindChild(transform, "AchieveList/Container")
	UIHelper.SetPanelDepth(achieveScrollView, UIHelper.GetPanelDepth(windowComponent.transform) + 1)
	achieveItemTable = {}
end

function SetAchieveInfo(  )
	-- body
end

function OnShow()
	TaskData.RequestAchieveTaskInfo(AchieveTaskInfoCallback)
	transform.gameObject:SetActive(true)
end
function OnHide()
	transform.gameObject:SetActive(false)
end

function OnGetRewardClick( go )
	local  listener = UIHelper.GetUIEventListener(go)
	local param = {}
	param.id = listener.parameter.id
	getCurId = listener.parameter.id
	TaskData.RequestAchieveTaskSubmit(param,AchieveGetRewardCallback)
end

function OnGotoClick( go )
	-- body
end

function RefreshAchieveGrid()
	TaskData.RenameItemsByOrder(achieveItemTable)
	UIHelper.GridSortByNumericName(achieveContainer)
	UIHelper.RepositionGrid(achieveContainer)
end

function AchieveTaskInfoCallback( _data )
	local achieveData = Config.GetTemplate(Config.TaskTable())
	local typeConfig = Config.GetTemplate(Config.TaskClassTable())
	achieveItemTable = {}
	UIHelper.DestroyGrid(achieveContainer)
	local idx = 0
	for k,v in TaskData.pairsByKeys(_data) do
		local key = tostring(k)
		if achieveData[key] ~= nil then
			idx = idx + 1
			local count = v
			local item = achieveData[key]
			if item.type == "9" then
				item.desc = Util.FormatString(item.desc,GetRaidName("9",item.key))
			elseif item.type == "10" then
				item.desc = Util.FormatString(item.desc,GetRaidName("10",item.key))
			end
			if item.isEnd ==1 then
				print("last one not show")
			else
				if typeConfig[item.sign] ~= nil and typeConfig[item.sign].display == 0 then
					local achieveItem = UITaskItemManager.CreateTaskItem(item,idx,UITaskItemManager.UITaskItemType.Achieve,key,achieveContainer,achieveScrollView,OnGetRewardClick,OnGotoClick)
					achieveItemTable[key] = achieveItem
					local complete = (v == -1)  --该类型成就全部完成并领取
					UITaskItemManager.RefreshAchieveItem(achieveItemTable[key],nil,count,complete)
				end
			end
		end
	end
	RefreshAchieveGrid()
	UIHelper.RepositionGrid(achieveContainer,achieveScrollView)
end

function AchieveGetRewardCallback( _data )
	local achieveData = Config.GetTemplate(Config.TaskTable())
	local nextID = _data.task_id
	local count = _data.val
	if nextID == nil then
		UITaskItemManager.RefreshAchieveItem(achieveItemTable[getCurId],nil,count,true)
	else
		local item = achieveData[nextID]
		if item.type == "9" then
			item.desc = Util.FormatString(item.desc,GetRaidName("9",item.key))
		elseif item.type == "10" then
			item.desc = Util.FormatString(item.desc,GetRaidName("10",item.key))
		end
		UITaskItemManager.RefreshAchieveItem(achieveItemTable[getCurId],item,count,false)
		achieveItemTable[nextID] = achieveItemTable[getCurId]
		achieveItemTable[getCurId] = nil
	end
	RefreshAchieveGrid()
	local itemList = {}
	itemList.item = {}
	for i,v in ipairs(_data.item) do
		table.insert(itemList.item,{id=v.id,num=v.num})
	end
	local params = {}
	params.m_itemTb = itemList
	params.titleName = GetLocalizedString("GetActReward")
	WindowMgr.ShowWindow(LuaConst.Const.UIGetItem,params)
end

function OnDestroy()	
	chieveData = {}
	achieveItemTable = {};
	achieveScrollView = nil;
	achieveContainer = nil;

	transform = nil
	windowComponent = nil
end

function GetRaidName( tp,key )
	local ret = ""
	if tp == "9" then
		ret = Config.GetProperty(Config.RaidLevelTable(),key, "name")
	elseif tp =="10" then
		ret = Config.GetProperty(Config.RaidJYLevelTable(),key, "name")
	else
		ret = key
	end
	return ret
end