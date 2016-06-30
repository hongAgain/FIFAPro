module("UITaskItemManager", package.seeall)

require "UILua/UIIconItem"
require "Game/TaskData"

local prefabTaskItem = "UITaskItem";
local prefabRewardItem = "IconItem"
local taskItemPrefab = nil;
local rewardItemPref = nil

local iconItemParam = {scale = 0.4, disableColid = false, offsetDepth = 1}

local TaskProgress = TaskData.TaskProgress

UITaskItemType = 
{
	DailyTask = "DailyTask_",
	Achieve = "Achieve_"
}

function Init( winComponent )
	windowComponet = winComponent
	if taskItemPrefab == nil then
		taskItemPrefab = winComponent:GetPrefab(prefabTaskItem)
	end
	if rewardItemPref == nil then
		rewardItemPref = winComponent:GetPrefab(prefabRewardItem)
	end
	UIIconItem.Init()
end

function CreateTaskItem(item,idx, itemType, stringKey, itemContainer, itemScrollView ,delegateOnGetReward,delegateOnGoto)

	local taskItem = {};
	--data
	taskItem.gameObject = GameObjectInstantiate(taskItemPrefab);
	taskItem.transform = taskItem.gameObject.transform;
	--taskItem.transform.name = itemType..stringKey;
	taskItem.type = itemType
	taskItem.itemInfo = item 
	taskItem.progress = TaskProgress.InProgress
	taskItem.num = 0
	--ui
	taskItem.BG = TransformFindChild(taskItem.transform,"BG");
	taskItem.Icon = TransformFindChild(taskItem.transform,"Icon");
	taskItem.Name = TransformFindChild(taskItem.transform,"Name");
	taskItem.AccomplishCondition = TransformFindChild(taskItem.transform,"AccomplishCondition");
	taskItem.AccomplishRewardList = TransformFindChild(taskItem.transform,"AccomplishReward/RewardList");
	taskItem.ButtonGet = TransformFindChild(taskItem.transform,"ButtonGet");
	taskItem.Achieve = TransformFindChild(taskItem.transform, "Completion/Achieve")
	taskItem.AchieveGoto = TransformFindChild(taskItem.transform,"Completion/Achieve/ButtonGoto")
	taskItem.AchievePercentLbl = TransformFindChild(taskItem.AchieveGoto, "PercentLabel")
	taskItem.AchievePercentSprite = TransformFindChild(taskItem.AchieveGoto, "Percent")
	taskItem.AchieveCompleteLbl = TransformFindChild(taskItem.transform,"Completion/Achieve/LabelComplete")
	taskItem.DailyTask = TransformFindChild(taskItem.transform, "Completion/DailyTask")
	taskItem.DailyTaskGoto = TransformFindChild(taskItem.transform, "Completion/DailyTask/ButtonGoto")
	taskItem.DailyTaskPercentLbl = TransformFindChild(taskItem.DailyTaskGoto, "PercentLabel")
	taskItem.DailyTaskGotLbl = TransformFindChild(taskItem.transform, "Completion/DailyTask/LabelGot")
	TransformFindChild(taskItem.transform,"ItemTypeId").name = itemType..stringKey

	--共有的属性
	taskItem.BG.gameObject:SetActive(idx%2 == 1)
	UIHelper.SetLabelTxt(taskItem.Name,item.name)
	UIHelper.SetLabelTxt(taskItem.AccomplishCondition,item.desc)
	local rewardItem = item.item
	for i=1,#rewardItem,3 do
		if rewardItem[i] ~= nil and rewardItem[i] ~= "" then
			UIIconItem.CreateRewardIconItem(taskItem.AccomplishRewardList,nil,{rewardItem[i],rewardItem[i+1]},iconItemParam)
			--CreateRewardItem(taskItem.AccomplishRewardList,{rewardItem[i],rewardItem[i+1],rewardItem[i+2]})
		end
	end
	UIHelper.RepositionGrid(taskItem.AccomplishRewardList)

	taskItem.liGet = Util.AddClick(taskItem.ButtonGet.gameObject,delegateOnGetReward)
	taskItem.liGet.parameter = {id = item.id}

	--特有的属性
	taskItem.liGoto = nil
	if itemType == UITaskItemType.DailyTask then
		UIHelper.SetSpriteNameNoPerfect(taskItem.Icon,"1")
		taskItem.DailyTask.gameObject:SetActive(true)
		taskItem.Achieve.gameObject:SetActive(false)
		taskItem.liGoto = Util.AddClick(taskItem.DailyTaskGoto.gameObject,delegateOnGoto)

	elseif itemType == UITaskItemType.Achieve then
		UIHelper.SetSpriteNameNoPerfect(taskItem.Icon,"Task_Type_"..item.type)
		taskItem.DailyTask.gameObject:SetActive(false)
		taskItem.Achieve.gameObject:SetActive(true)
		taskItem.liGoto = Util.AddClick(taskItem.AchieveGoto.gameObject,delegateOnGoto)
	end
	if taskItem.liGoto~=nil then
		taskItem.liGoto.parameter = {id = item.id}
	end

	taskItem.transform.parent = itemContainer
	taskItem.transform.localPosition= NewVector3(0,0,0)
	taskItem.transform.localScale= NewVector3(1,1,1)
	UIHelper.SetDragScrollViewTarget(taskItem.transform,itemScrollView)

	return taskItem;
end

function RefreshTaskItem( item, num, hasGot)
	item.num = num
	if hasGot then
		SetTaskProgress(item,TaskProgress.GotReward)
	else
		local tarNum = tonumber(item.itemInfo.val)
		if num == nil or num >= tarNum then
			SetTaskProgress(item,TaskProgress.Complete)
		else
			if item.type ==UITaskItemType.DailyTask then
				UIHelper.SetLabelTxt(item.DailyTaskPercentLbl,num.."/"..tarNum)
			elseif item.type == UITaskItemType.Achieve then
				UIHelper.SetLabelTxt(item.AchievePercentLbl,num.."/"..tarNum)
				UIHelper.SetSpriteFillAmount(item.AchievePercentSprite, num/tarNum)
			end
			SetTaskProgress(item,TaskProgress.InProgress)
		end
	end
end

function RefreshAchieveItem( item,itemInfo,num,hasGot )
	if itemInfo ~= nil then  --更新为该类型下一个成就  ，itemInfo == nil -->更新该类型当前成就
		UIHelper.SetLabelTxt(item.Name,itemInfo.name)
		UIHelper.SetLabelTxt(item.AccomplishCondition,itemInfo.desc)
		local rewardItem = itemInfo.item
		UIHelper.DestroyGrid(item.AccomplishRewardList)
		for i=1,#rewardItem,3 do
			if rewardItem[i] ~= nil and rewardItem[i] ~= "" then
				UIIconItem.CreateRewardIconItem(item.AccomplishRewardList,nil,{rewardItem[i],rewardItem[i+1]},iconItemParam)
				--CreateRewardItem(item.AccomplishRewardList,{rewardItem[i],rewardItem[i+1],rewardItem[i+2]})
			end
		end
		UIHelper.RepositionGrid(item.AccomplishRewardList)
		item.itemInfo = itemInfo
		item.liGet.parameter = {id = itemInfo.id}
		item.liGoto.parameter = {id = itemInfo.id}
	end
	RefreshTaskItem(item,num,hasGot)
end

function SetTaskProgress( item, progress )
	item.progress = progress
	item.ButtonGet.gameObject:SetActive(progress == TaskProgress.Complete)
	if item.type == UITaskItemType.DailyTask then
		item.DailyTaskGoto.gameObject:SetActive(progress == TaskProgress.InProgress)
		item.DailyTaskGotLbl.gameObject:SetActive(progress == TaskProgress.GotReward)
	elseif item.type == UITaskItemType.Achieve then
		item.AchieveGoto.gameObject:SetActive(progress == TaskProgress.InProgress)
		item.AchieveCompleteLbl.gameObject:SetActive(progress == TaskProgress.GotReward)
	end
end

function SetTaskItemBG(item,idx)
	item.BG.gameObject:SetActive(idx%2 == 1)
end

--具有特殊标记的item处理
--param{type, dataInfo},dataInfo为所需数据信息
function SetSpecialInfo( item, param )
	if param == nil or item == nil then
		return
	end
	local specialLbl = TransformFindChild(item.transform, "SpecialLabel")
	specialLbl.gameObject:SetActive(true)
	if item.type == UITaskItemType.DailyTask and param[1] == 0 and param[2] ~= nil then  --月卡
		UIHelper.SetLabelTxt(specialLbl,string.format(Util.LocalizeString("UITaskRechargeLeftDay"),param[2]))
	end
end

-- function CreateRewardItem( parent ,pair)
-- 	local id = pair[1]
-- 	local count	= pair[2]
-- 	local dropRate = pair[3]
-- 	if id~=nil and id~="" then
-- 		local itemTrans= GameObjectInstantiate(rewardItemPref).transform
-- 		itemTrans.parent = parent
-- 		itemTrans.localScale = NewVector3(1,1,1)
-- 		itemTrans.localPosition = NewVector3(0,0,0)
-- 		itemTrans.name = id
-- 		local icon = TransformFindChild(itemTrans , "BG")
-- 		local countLbl = TransformFindChild(itemTrans, "Count")
-- 		UIHelper.SetLabelTxt(countLbl,count)
-- 		local texName = Config.GetProperty(Config.ItemTable(),id,"icon")
-- 		Util.SetUITexture(icon,LuaConst.Const.ItemIcon,texName,false)
-- 	end
-- end

function OnDestroy()
	taskItemPrefab = nil;
	rewardItemPref = nil
end