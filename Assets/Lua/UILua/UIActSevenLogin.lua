module("UIActSevenLogin", package.seeall)

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
local spRewardLbl = nil
local spRewardTex = nil
local spRewardSV = nil
local spRewardGrid = nil
local spRewardGetTrans = nil
local comRewardSV = nil
local comRewardGrid = nil
local comRewardBar = nil

local rewardTable = nil

local comSvItemCount = 0
local comPBOValue = 0
local comPBTValue = 0
local frequency = 60
local timeTick = nil

local itemTitleUI = "UISLRewardInfoTitle"

function Init( trans, winComponent)
	transform = trans
	windowComponent = winComponent
	UIRewardInfoItem.Init(winComponent)
	BindUI()
end
function BindUI()
	title = TransformFindChild(transform, "TitleText")
	local spReward = TransformFindChild(transform, "Reward/SpRewardItem")
	spRewardLbl = TransformFindChild(spReward, "SPReward/RewardLabel")
	spRewardTex = TransformFindChild(spReward, "SPReward/RewardTexture")
	spRewardSV = TransformFindChild(spReward, "OtherReward")
	spRewardGrid = TransformFindChild(spReward, "OtherReward/Grid")
	spRewardGetTrans = TransformFindChild(spReward, "OtherReward/GetState")
	comRewardSV = TransformFindChild(transform, "Reward/RewardItemList")
	UIHelper.SetPanelDepth(comRewardSV, UIHelper.GetPanelDepth(windowComponent.transform) + 1)
	comRewardGrid = TransformFindChild(transform, "Reward/RewardItemList/Grid")
	comRewardBar = TransformFindChild(transform, "Reward/RewardItemList/ScrollViewBar")
	comSvItemCount = UIHelper.GetPanelSize(comRewardSV).y / UIHelper.GetUIGridCellSize(comRewardGrid).y
	InitRewardItem()
end

function InitRewardItem()
	rewardTabel = {}
	local reward = Config.GetTemplate(Config.Login7Reward())

	local gridIdx = 1
	for k,v in CommonScript.PairsByOrderKey(reward,"id",true) do
		if v.id == "7" then -- special
			local item = {}
			item.getTrans = spRewardGetTrans
			item.info = {id = v.id}
			UIRewardInfoItem.InitGetState(spRewardGetTrans, v.id, OnGetRewardClick)
			local list = v.reward:split(",")
			--第一个奖励是特殊物品,todo
			local spId = list[1]
			--UIHelper.SetLabelTxt(spRewardLbl,"梅西")
			--Util.SetUITexture(spRewardTex,LuaConst.Const.ActivityImg,Config.GetProperty(Config.ItemTable(), spId, 'icon'), false)
			for i = 3,#list,2 do
				local iconId = list[i]
				local times = tonumber(list[i+1])
				local otherParam = {scale = 0.4, disableColid = false, offsetDepth = 3}
				UIIconItem.CreateRewardIconItem(spRewardGrid,spRewardSV,{iconId,times},otherParam)
			end
			rewardTabel[#rewardTabel + 1] = item
		else
			local itemInfo = {}
			itemInfo.id = v.id
			itemInfo.targetNum = v.id
			itemInfo.title = GetLocalizedString(itemTitleUI, itemInfo.targetNum)
			itemInfo.getBtnType = UIRewardInfoItem.RewardGetBtnType.JustBtn
			itemInfo.rewardList = v.reward
			local item = UIRewardInfoItem.CreateRewardInfoItem(itemInfo,gridIdx,comRewardGrid,comRewardSV,0.4,OnGetRewardClick)
			gridIdx = gridIdx + 1
			rewardTabel[#rewardTabel+1] = item
		end
	end
end

function OnShow()
	ActivityData.ReqSevenLoginInfo(OnGetRewardInfo)
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

function OnGetRewardInfo(data)
	local fstIdx = 1
	local fstCanGet = #rewardTabel + 1
	local fstCantGet = #rewardTabel + 1
	for i,v in ipairs(rewardTabel) do
		local state = data[v.info.id].status
		local hasGot = state == 2
		local canGet = state == 1
		local cantGet = state == 0
		if canGet and i < fstCanGet then
			fstCanGet = i
		end
		if cantGet and i<fstCantGet then
			fstCantGet = i
		end
		UIRewardInfoItem.RefreshRewardItem(v, hasGot, canGet, "")
	end
	local total = #rewardTabel - 1 --有一个特殊天奖励
	if fstCanGet > total then
		if fstCantGet > total then
			fstIdx = fstCantGet
		end
	else
		fstIdx = fstCanGet
	end
	--滚动到fstIdx位置
	if comSvItemCount < total then
		comPBTValue = (fstIdx - 1) / (total - comSvItemCount)
	end
	if comPBTValue >=1 then
		comPBTValue = 1
	end
	comPBOValue = UIHelper.GetProgressBar(comRewardBar)
	--UIHelper.SetProgressBar(comRewardBar,value)
	if timeTick~=nil then
		LuaTimer.RmvTimer(timeTick)
	end
	timeTick = LuaTimer.AddTimer(false,-1/frequency,UpdateComRewardPB)
end
function UpdateComRewardPB()
	local duration = 0.3
	local func = function()
		LuaTimer.RmvTimer(timeTick)
		timeTick = nil
	end
	ActivityData.UpdatePBValueByDivision(comRewardBar,comPBOValue,comPBTValue,duration*frequency,func)
end

function OnGetRewardClick(go)
	local li = UIHelper.GetUIEventListener(go)
	local param = {}
	param.rewardSeq = li.parameter.id
	ActivityData.ReqSevenLoginReward(OnGetReward, param)
end

function OnGetReward(data)
	for i,v in ipairs(rewardTabel) do
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