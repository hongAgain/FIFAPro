module("UIDailyTaskController", package.seeall)

--child module
require "Game/TaskData"
require "UILua/UITaskItemManager"

local prefabDailyTask = "UITaskDaily"

local accomplishCountLbl = nil --活跃度奖励完成度
local activeLvCountLbl = nil   --当前活跃度
local rewardTable = {}
local progressBar = nil
local taskScrollView = nil
local taskContainer = nil
local taskTable = {}  --所有taskItem集合
--userdata
local gotPoint = 0
local awardList = {}
local gotCount = 0

local transform = nil
local windowComponent = nil

local RewardState = 
{
	Disable = 1,
	Enable = 2,
	HasGot = 3,
}
--data
local isFirst = false
local rewardActiveLvl = nil
local totalActivePoint = 0

function InitUI( trans ,winComponent)
	transform = trans
	windowComponent = winComponent
	UITaskItemManager.Init(windowComponent)
	taskTable = {}
	isFirst = true
	accomplishCountLbl = TransformFindChild(transform, "AccomplishToday/Count")
	activeLvCountLbl = TransformFindChild(transform, "Reward/Label")
	rewardTable[1] = TransformFindChild(transform, "Reward/List/1")
	rewardTable[2] = TransformFindChild(transform, "Reward/List/2")
	rewardTable[3] = TransformFindChild(transform, "Reward/List/3")
	rewardTable[4] = TransformFindChild(transform, "Reward/List/4")
	BindGetRewardBtn()
	progressBar = TransformFindChild(transform, "Reward/ProgressBar")
	taskScrollView = TransformFindChild(transform, "DailyList")
	taskContainer = TransformFindChild(transform, "DailyList/Container")
	UIHelper.SetPanelDepth(taskScrollView, UIHelper.GetPanelDepth(windowComponent.transform) + 1)
	BindTaskItem()
end

function BindGetRewardBtn()
	if rewardActiveLvl == nil then
		local dayTaskAwardTable = Config.GetTemplate(Config.DayTaskAwardTable())
		rewardActiveLvl = {}
		for k,v in TaskData.pairsByKeys(dayTaskAwardTable) do
			rewardActiveLvl[#rewardActiveLvl +1]=v
		end
	end
	for i,v in pairs(rewardTable) do
		if rewardActiveLvl[i] ==nil then
			v.gameObject:SetActive(false)
		else
			local btn = TransformFindChild(v, "Button").gameObject
			local label = TransformFindChild(v, "Label")
			UIHelper.SetLabelTxt(label,rewardActiveLvl[i]["id"])
			Util.AddClick(btn,OnGetRewardClick)
		end
	end
end
--读表初始化任务列表
function BindTaskItem()
	local taskTableConfig = Config.GetTemplate(Config.DayTaskTable())
	local idx = 0
	totalActivePoint = 0
	for k,v in TaskData.pairsByKeys(taskTableConfig) do
		idx = idx + 1
		totalActivePoint = totalActivePoint + v.point
		--生成任务item
		local taskItem = UITaskItemManager.CreateTaskItem(v,idx,UITaskItemManager.UITaskItemType.DailyTask,k,taskContainer,taskScrollView,OnGetTaskRewardClick,OnGotoClick)
		taskTable[tostring(k)] = taskItem
	end
	--print()
	UIHelper.RepositionGrid(taskContainer,taskScrollView)
end

function RefreshTaskGrid()
	TaskData.RenameItemsByOrder(taskTable)
	UIHelper.GridSortByNumericName(taskContainer)
	UIHelper.RepositionGrid(taskContainer)
end

function OnGetRewardClick( go )
	local idx = tonumber(go.transform.parent.name)
	local key = rewardActiveLvl[idx]["id"]
	if gotPoint < tonumber(key) then
		local reardTb = {}
		reardTb['item'] = {}
		local rewardList = rewardActiveLvl[idx].item
		for i=1,#rewardList,3 do
			if rewardList[i] ~= nil and rewardList[i] ~= "" then
				table.insert(reardTb['item'],{id=rewardList[i],num=rewardList[i+1]})
			end
		end
        WindowMgr.ShowWindow(LuaConst.Const.UIGetItem,{
            m_itemTb = reardTb,
            titleName = Util.LocalizeString("UIRewardPreview"),
            getButtonName = Util.LocalizeString("UIPeakRoad_Confirm"),
            OnClose = nil
        })
	else
		local param = {}
		param.id = key
		TaskData.RequestDailyTaskAward(param,DailyRewardCallback)
	end
end

function OnGetTaskRewardClick( go )
	local listener = UIHelper.GetUIEventListener(go)
	local param = {}
	param.id = listener.parameter.id
	TaskData.RequestDailyTaskSubmit(param,TaskSubmitCallback)
end
function OnGotoClick(go)
	local curId = UIHelper.GetUIEventListener(go).parameter.id
	local curTypeId = taskTable[curId].itemInfo.type
	local curType = Config.GetProperty(Config.DayTaskTypeTable(),curTypeId,"type")
	if curType == "0" then --购买月卡
		if UIHeadScript == nil then
			require("UILua/UIHeadScript")
		end
		UIHeadScript.BtnDiamond()
	elseif curType == "1" then -- 主线副本
		if TeamLegendData == nil then
			require("Game/TeamLegendData")
		end
		TeamLegendData.OpenTeamLegend(TeamLegendData.e_raidDiff.Normal)
	elseif curType == "2" then --精英副本
		if TeamLegendData == nil then
			require("Game/TeamLegendData")
		end
		TeamLegendData.OpenTeamLegend(TeamLegendData.e_raidDiff.Elite)
	elseif curType == "3" then --定时副本
		if UIMatches == nil then
			require("UILua/UIMatches")
		end
		UIMatches.OnClickTimeRaid()
	elseif curType == "4" then --巅峰之路
		if UIMatches == nil then
			require("UILua/UIMatches")
		end
		UIMatches.OnClickEpicRoad()
	elseif curType == "5" then --天梯
		if UIMatches == nil then
			require("UILua/UIMatches")
		end
		UIMatches.OnClickLadderMatch()
	elseif curType == "6" then --日常杯赛
		if UIMatches == nil then
			require("UILua/UIMatches")
		end
		UIMatches.OnClickDailyCup()
	elseif curType == "7" then --球员升级
		WindowMgr.ShowWindow(LuaConst.Const.UIPlayerList)
	elseif curType == "8" then --球员培养
		WindowMgr.ShowWindow(LuaConst.Const.UIPlayerList)
	elseif curType == "9" then --进行抽卡
		if UILobbyScript == nil then
			require("UILua/UILobbyScript")
		end
		UILobbyScript.BtnScout()
	elseif curType == "10" then --赠送体力
		if UILobbyScript == nil then
			require("UILua/UILobbyScript")
		end
		UILobbyScript.OnClickBtnFriend()
	elseif curType == "11" then --装备强化
		WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { "装备系统紧张开发中，敬请期待！" })
	elseif curType == "12" then --购买体力
		if UIHeadScript == nil then
			require("UILua/UIHeadScript")
		end
		UIHeadScript.BtnCost()
	elseif curType == "13" then --购买金币
		if UIHeadScript == nil then
			require("UILua/UIHeadScript")
		end
		UIHeadScript.BtnGold()
	end

end

function OnShow() 
	--通信
	TaskData.RequestDailyTaskInfo(DailyTaskInfoCallBack)
	if not isFirst then
		transform.gameObject:SetActive(true)
	end	
end

function OnHide()
	transform.gameObject:SetActive(false)
end

function DailyRewardCallback( _data )
	for k,v in pairs(_data) do
		if k ~= "item" then
			table.insert(awardList,k,v)
			gotCount = gotCount + 1
			for i,v in ipairs(rewardTable) do
				if k == rewardActiveLvl[i]["id"] then
					SetRewardItemInfo(v,RewardState.HasGot)
					UIHelper.SetLabelTxt(accomplishCountLbl,gotCount.."/"..#rewardTable)
					break
				end
			end
		else
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
	end
end

function DailyTaskInfoCallBack(_data)
	isFirst = false
	local rewardInfo = _data["data"]
	local taskInfo = _data["task"]
	gotPoint = rewardInfo["point"] or 0
	local record = rewardInfo["record"] or {}
	awardList = rewardInfo["award"] or {}
	--reward
	RefreshRewardItem()

	--taskItem
	for k,v in pairs(taskInfo) do
		local got = (record[k]~=nil and record[k] ==1) or false
		if taskTable[k].itemInfo.id == "0" then --月卡
			if v > 0 then
				UITaskItemManager.RefreshTaskItem(taskTable[k],1,got)
			else
				UITaskItemManager.RefreshTaskItem(taskTable[k],0,false)
			end
			UITaskItemManager.SetSpecialInfo(taskTable[k],{0,v})
		else
			UITaskItemManager.RefreshTaskItem(taskTable[k],v,got)
		end
	end
	transform.gameObject:SetActive(true)
	RefreshTaskGrid()
end

function TaskSubmitCallback( _data )
	local id  = ""
	local taskTableConfig = Config.GetTemplate(Config.DayTaskTable())
	for k,v in pairs(taskTableConfig) do
		if _data[k] ~= nil and _data[k] == 1 then
			id = k
			break
		end
	end
	local addPoint = taskTableConfig[id].point
	gotPoint = gotPoint + addPoint
	RefreshRewardItem()
	UITaskItemManager.RefreshTaskItem(taskTable[id],0,true)
	RefreshTaskGrid()
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

function RefreshRewardItem()
	local count = 0
	local percent = 0
	local idx = 0  --活跃度可领取最高奖励的索引
	for i,v in ipairs(rewardTable) do
		local num = tonumber(rewardActiveLvl[i]["id"])
		if awardList[tostring(num)]~=nil and awardList[tostring(num)]==1 then
			SetRewardItemInfo(v,RewardState.HasGot)
			count = count+1
		else 
			if gotPoint >= num then
				SetRewardItemInfo(v,RewardState.Enable)
			else
				SetRewardItemInfo(v,RewardState.Disable)
			end
		end
		if gotPoint >= num then
			idx =i
		end
	end
	percent = GetRewardPercent(idx,gotPoint)
	UIHelper.SetProgressBar(progressBar,percent)
	UIHelper.SetLabelTxt(activeLvCountLbl,gotPoint.."/"..totalActivePoint)
	UIHelper.SetLabelTxt(accomplishCountLbl,count.."/"..#rewardTable)
	gotCount = count
end

function GetRewardPercent( idx,point )
	local offset = idx*2 / 9
	local curIdxPoint = 0
	local nextIdxPoint = 0
	local rate = 0
	if idx ~= 0 then
		curIdxPoint = tonumber(rewardActiveLvl[idx]["id"])
	end
	if idx == #rewardTable then
		nextIdxPoint = totalActivePoint
		rate = 1/9
	else
		nextIdxPoint = tonumber(rewardActiveLvl[idx+1]["id"])
		rate = 2/9
	end
	local percent = offset + (point-curIdxPoint)/(nextIdxPoint - curIdxPoint)*rate
	return percent
end

function SetRewardItemInfo(trans, state)
	local btnSprite = TransformFindChild(trans, "Button")
	local eff = TransformFindChild(trans, "Effect").gameObject
	if state == RewardState.Disable then
		UIHelper.SetSpriteNameNoPerfect(btnSprite, "bag_disable")
	elseif state == RewardState.Enable then
		UIHelper.SetSpriteNameNoPerfect(btnSprite, "bag_enable")
	elseif state == RewardState.HasGot then
		UIHelper.SetSpriteNameNoPerfect(btnSprite,"bag_open")
	end
	local active = state == RewardState.Enable
	eff:SetActive(active)
	--UIHelper.SetBoxCollider(btnSprite,active)
end

function OnDestroy()	
	accomplishCountLbl = nil 
	activeLvCountLbl = nil 
	rewardTable = {}
	progressBar = nil
	taskScrollView = nil
	taskContainer = nil
	taskTable = {} 
	transform = nil
	windowComponent = nil
end

--前往位置为弹出窗口的任务刷新
function RefreshPopTaskItem(taskKey, addNum)
	if taskTable == nil or taskTable == {} then
		return
	end
	local refreskItemId = ""
	for k,v in pairs(taskTable) do
		local id = v.itemInfo.id
		local key = Config.GetProperty(Config.DayTaskTypeTable(), v.itemInfo.type, "key")
		if key == taskKey then
			refreskItemId = id
			break
		end
	end
	local item = taskTable[refreskItemId]
	if item ~= nil then
		if item.progress == TaskData.TaskProgress.InProgress then
			UITaskItemManager.RefreshTaskItem(item, item.num+addNum, false)
			RefreshTaskGrid()
		end
	end
end