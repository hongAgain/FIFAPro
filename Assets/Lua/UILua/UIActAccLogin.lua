module("UIActAccLogin", package.seeall)

require "Config"
require "Common/UnityCommonScript"
require "Common/CommonScript"
require "Common/Math"
require "Game/ActivityData"
require "UILua/UIRewardInfoItem"
--显示数据来源于静态配置表

local transform = nil
local windowComponent = nil
--ui
local title = nil 
local rewardSV = nil
local rewardGrid = nil
local rewardBar = nil

local rewardTable = nil

local svItemCount = 0
local pbOValue = 0
local pbTValue = 0
local frequency = 60
local timeTick = nil

local itemTitleUI = "UIALRewardInfoTitle"

function Init( trans, winComponent )
	transform = trans
	windowComponent = winComponent
	UIRewardInfoItem.Init(winComponent)
	BindUI()
end
function BindUI()
	title = TransformFindChild(transform, "TitleText")
	rewardSV = TransformFindChild(transform, "Reward/RewardItemList")
	UIHelper.SetPanelDepth(rewardSV, UIHelper.GetPanelDepth(windowComponent.transform) + 1)
	rewardGrid = TransformFindChild(transform, "Reward/RewardItemList/Grid")
	rewardBar = TransformFindChild(transform, "Reward/RewardItemList/ScrollViewBar")
	svItemCount = UIHelper.GetPanelSize(rewardSV).y / UIHelper.GetUIGridCellSize(rewardGrid).y
	InitRewardItem()
end

function InitRewardItem()
	rewardTable = {}
	local rewardConfig = Config.GetTemplate(Config.LoginAccReward())
	for k,v in CommonScript.PairsByOrderKey(rewardConfig,"id",true) do
		local itemInfo = {}
		itemInfo.id = v.id
		itemInfo.targetNum = v.id
		itemInfo.title = GetLocalizedString(itemTitleUI, itemInfo.targetNum)
		itemInfo.getBtnType = UIRewardInfoItem.RewardGetBtnType.BtnAndNum
		itemInfo.rewardList = v.reward
		local item = UIRewardInfoItem.CreateRewardInfoItem(itemInfo, #rewardTable + 1,rewardGrid,rewardSV,0.4,OnGetRewardClick)
		rewardTable[#rewardTable+1] = item
	end
end

function OnShow()
	ActivityData.ReqAccLoginInfo(OnGetRewardInfo)
end
function OnHide()
	if timeTick ~= nil then
		LuaTimer.RmvTimer(timeTick)
		timeTick = nil
	end
end
function OnDestroy()
	OnHide()
end

function OnGetRewardInfo( data )
	local fstIdx = 1
	local fstCanGet = #rewardTable + 1
	local fstCantGet = #rewardTable + 1
	local loginAcc = data.loginAcc
	local rewardInfo = data.rewardInfo
	for i,v in ipairs(rewardTable) do
		local state = rewardInfo[v.info.id].status
		local hasGot = state==2
		local canGet = state==1
		local cantGet = state == 0
		if cantGet and i < fstCantGet then
			fstCantGet = i
		end
		if canGet and i < fstCanGet then
			fstCanGet = i
		end
		UIRewardInfoItem.RefreshRewardItem(v,hasGot,canGet,loginAcc)
	end
	if fstCanGet > #rewardTable then
		if fstCantGet < #rewardTable then
			fstIdx = fstCantGet
		end
	else
		fstIdx = fstCanGet
	end
	if svItemCount < #rewardTable then
		pbTValue = (fstIdx - 1) / (#rewardTable - svItemCount)
	end
	if pbTValue >=1 then
		pbTValue = 1
	end
	pbOValue = UIHelper.GetProgressBar(rewardBar)
	if timeTick ~= nil then
		LuaTimer.RmvTimer(timeTick)
	end
	timeTick = LuaTimer.AddTimer(false, -1/frequency, UpdateRewardPB)
end

function UpdateRewardPB()
	local duration = 0.3
	local func = function()
		LuaTimer.RmvTimer(timeTick)
		timeTick = nil
	end
	ActivityData.UpdatePBValueByDivision(rewardBar,pbOValue,pbTValue,duration*frequency,func)
end


function OnGetRewardClick(go)
	local li = UIHelper.GetUIEventListener(go)
	local param = {}
	param.rewardSeq = li.parameter.id
	ActivityData.ReqAccLoginReward(OnGetReward, param)
end
function OnGetReward( data )
	for i,v in ipairs(rewardTable) do
		if v.info.id == tostring(data.rewardSeq) then
			UIRewardInfoItem.RefreshRewardItem(v,true,true,"")
			break
		end
	end
	local itemList = {}
	itemList.item = {}
	for i,v in ipairs(data.reward) do
		table.insert(itemList.item,{id=v.item_id,num=v.item_num})
	end
	local params = {}
	params.m_itemTb = itemList
	params.titleName = GetLocalizedString("GetActReward")
	WindowMgr.ShowWindow(LuaConst.Const.UIGetItem,params)
end