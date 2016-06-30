module("UIActLvRankReward", package.seeall)

require "Config"
require "Common/UnityCommonScript"
require "Common/CommonScript"
require "Common/Math"
require "Game/ActivityData"
require "UILua/UIIconItem"
--显示数据来源于静态配置表

local rewardItemName = "LvRankRewardItem"
local rewardItemPrefab = nil

local transform = nil
local windowComponent = nil
--ui
local title = nil
local timeLabel = nil
local myRankLabel = nil

local rewardSV = nil
local rewardGrid = nil

local timeFormatStr = "UIActLRRTimeInfo"
local timeFormat = nil
local segTitleFmtStr = "UIActLRRSegTitle"
local segTitleFmt = nil

--data
local rewardItemTable = nil
local rankSegTable = {}
local preRankSegID = nil
local remainTime = nil
local timeTick = nil 
local day = 0
local hour = 0
local minute = 0
local second = 0

function Init( trans, winComponent )
	transform = trans
	windowComponent = winComponent
	if rewardItemPrefab == nil then
		rewardItemPrefab = windowComponent:GetPrefab(rewardItemName)
	end
	if timeFormat == nil then
		timeFormat = Util.LocalizeString(timeFormatStr)
	end
	if segTitleFmt == nil then
		segTitleFmt = Util.LocalizeString(segTitleFmtStr)
	end
	UIIconItem.Init(windowComponent)
	BindUI()
end
function BindUI()
	title = TransformFindChild(transform, "TitleText")
	timeLabel = TransformFindChild(transform, "Time/Info")
	myRankLabel = TransformFindChild(transform, "MyRank/Rank")
	rewardSV = TransformFindChild(transform, "Reward/RewardItemList")
	rewardGrid = TransformFindChild(transform, "Reward/RewardItemList/Grid")
	UIHelper.SetPanelDepth(rewardSV, UIHelper.GetPanelDepth(windowComponent.transform) + 1)
	InitRewardInfo()
end

function InitRewardInfo()
	rewardItemTable = {}
	rankSegTable = {}
	local rankReward = Config.GetTemplate(Config.LvRankReward())
	local preMaxRank = 0
	local idx = 1
	for k,v in CommonScript.PairsByOrderKey(rankReward, "id", true) do
		local rankSeg = ""
		if preMaxRank <= 0 then
			rankSeg = v.id
		else
			rankSeg = (preMaxRank+1).."~"..v.id
		end
		--create
		local item = CreateRewardItem(rewardSV, rewardGrid, idx, rankSeg, v.reward)
		rewardItemTable[v.id] = item
		preMaxRank = tonumber(v.id)
		rankSegTable[idx] = tonumber(v.id)
		idx = idx + 1
	end
	UIHelper.RepositionGrid(rewardGrid, rewardSV)
end

function OnShow()
	ActivityData.ReqLvRankInfo(OnGetInfo)
end
function OnGetInfo(data)
	--结束时间及排名获取
	local endTime = data.deadline --秒为单位
	local myRank = data.rank
	local remainTime = endTime - Util.GetTime()
	FormatSecond(remainTime)
	if myRank == nil then
		UIHelper.SetLabelTxt(myRankLabel, Util.LocalizeString("UIActLRRNullRank"))
	else
		UIHelper.SetLabelTxt(myRankLabel, tostring(myRank))
		for i,v in ipairs(rankSegTable) do
			if myRank <= v then
				if preRankSegID == nil then
					preRankSegID = tostring(v)
					RefreshRewardItem(rewardItemTable[preRankSegID], true)
				elseif preRankSegID ~= tostring(v) then
					RefreshRewardItem(rewardItemTable[preRankSegID], false)
					preRankSegID = tostring(v)
					RefreshRewardItem(rewardItemTable[preRankSegID], true)
				end
				break
			end
		end
	end
	if timeTick ~= nil then
		LuaTimer.RmvTimer(timeTick)
	end
	if remainTime > 0 then
		timeTick = LuaTimer.AddTimer(false,-1,RefreshTime)
	end
end

function RefreshRewardItem(item, isIn)
	item.inSegTrans.gameObject:SetActive(isIn)
end

--剩余时间转换为天时分秒
function FormatSecond( timeSecond )
	if timeSecond < 0 then
		day = 0
		hour = 0
		minute = 0
		second = 0
		return
	end
	timeSecond = math.modf(timeSecond / 1000)
	local daySecond = 3600 * 24
	day = math.floor(timeSecond / daySecond)
	hour = math.floor((timeSecond - day * daySecond) / 3600)
	minute = math.floor((timeSecond % 3600) / 60)
	second = timeSecond % 60
end

--一秒钟调用一次
function RefreshTime()
	if second > 0 then
		second = second -1
	elseif minute > 0 then
		minute = minute -1
		second = 59
	elseif hour > 0 then
		hour = hour -1
		minute = 59
		second = 59
	elseif day > 0 then
		day = day -1
		hour = 23
		minute = 59
		second = 59
	else
		second = 0
		minute = 0 
		hour = 0
		day = 0
		if timeTick ~= nil then
			LuaTimer.RmvTimer(timeTick)
		end
	end
	UIHelper.SetLabelTxt(timeLabel,string.format(timeFormat,day,hour,minute,second))
end

function CreateRewardItem(sv, grid, idx, rankSeg, rewardList)
	local itemInfo = {}
	local item = InstantiatePrefab(rewardItemPrefab, grid, tostring(idx))
	itemInfo.item = item
	local itemTrans = item.transform
	UIHelper.SetDragScrollViewTarget(itemTrans, sv)
	local itemTitle = TransformFindChild(itemTrans, "Title")
	local iconGrid = TransformFindChild(itemTrans, "RewardList")
	itemInfo.inSegTrans = TransformFindChild(itemTrans, "RankSegment")
	UIHelper.SetLabelTxt(itemTitle, string.format(segTitleFmt,rankSeg))
	local list = rewardList:split(",")
	for i = 1, #list, 2 do
		local param = {scale = 0.4, disableColid = false, offsetDepth = 1}
		UIIconItem.CreateRewardIconItem(iconGrid, nil, {list[i], tonumber(list[i+1])},param)
	end
	UIHelper.RepositionGrid(iconGrid)
	return itemInfo
end

function OnHide()
	if timeTick ~= nil then
		LuaTimer.RmvTimer(timeTick)
		timeTick = nil 
	end
end
function OnDestroy()
	OnHide()
	rankSegTable = {}
	preRankSegID = nil
end