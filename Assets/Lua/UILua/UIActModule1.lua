--module("UIActivityTypeBase", package.seeall)

require "Config"
require "Common/UnityCommonScript"
require "Common/CommonScript"
require "Common/Math"
require "Game/ActivityData"
require "UILua/UIRewardInfoItem"
--显示数据来源于静态配置表
--local tab = os.date("*t", 1131286710)返回值 tab 的数据 {year=2005, month=11, day=6, hour=22,min=18,sec=30}
UIActModule1 = {}
UIActModule1.__index = UIActModule1
--建立对象
function UIActModule1:new(trans, winComponent, info)
	local self = {}
	self.transform = trans
	self.transform.name = "UIActModule1_"..info["_id"].."(Clone)"
	self.windowComponent = winComponent
	setmetatable(self, UIActModule1)
	self.titleLbl = TransformFindChild(trans, "TitleText")
	self.timeValueLbl = TransformFindChild(trans, "TimeInfo/Value")
	self.ruleValueLbl = TransformFindChild(trans, "RuleInfo/Value")
	self.rewardSV = TransformFindChild(trans, "Reward/RewardItemList")
	UIHelper.SetPanelDepth(self.rewardSV, UIHelper.GetPanelDepth(self.windowComponent.transform) + 1)
	self.rewardGrid = TransformFindChild(trans, "Reward/RewardItemList/Grid")
	--label可以根据info赋值
	local lblConfig = info.uiConfig
	UIHelper.SetLabelTxt(self.titleLbl, info.name)
	local startTime = os.date("*t",info.STime/1000)
	local endTime = os.date("*t", info.ETime/1000)
	UIHelper.SetLabelTxt(self.timeValueLbl, GetLocalizedString("UIActivityTimeFormat", startTime.year, startTime.month, startTime.day,endTime.year, endTime.month, endTime.day))
	UIHelper.SetLabelTxt(self.ruleValueLbl, GetLocalizedString(lblConfig.rule))
	self.init = true
	self.info = info --数据
	return self
end

function UIActModule1:OnShow()
	--判断是否要请求最新数据
	if self.init == false and self.info.actType == 3 then
		local refreshInfo = function(data)
			for k,v in pairs(data[self.info._id]) do
			 	self.info[k] = v
			end 
			self:CreateItems()
		end
		ActivityData.ReqTimeActiveInfo(refreshInfo, self.info._id)
	end
	if self.init then
		self.init = false
		self:CreateItems()
	end
end

function UIActModule1:CreateItems()
	UIHelper.DestroyGrid(self.rewardGrid)
	self.rewardTable = {}
	local onGetRewardClick = function ( go )
		self:OnGetRewardClick(go)
	end
	--初始化奖励数据
	for k,v in CommonScript.PairsByOrderKey(self.info.award, "s", true) do
		local itemInfo = {}
		itemInfo.id = v._id
		itemInfo.targetNum = v.stepS
		local titleInfo = {}
		titleInfo.title1 = GetLocalizedString(self.info.uiConfig.itemTitle1)
		titleInfo.title2 = GetLocalizedString(self.info.uiConfig.itemTitle2,itemInfo.targetNum)
		itemInfo.title = titleInfo
		itemInfo.getBtnType = UIRewardInfoItem.RewardGetBtnType.BtnAndNum
		itemInfo.rewardList = v.item
		itemInfo.titleType = UIRewardInfoItem.ItemTitleType.WithDiamondTex
		itemInfo.rewardType = UIRewardInfoItem.RewardListType.ServerData
		local item = UIRewardInfoItem.CreateRewardInfoItem(itemInfo,0,self.rewardGrid,self.rewardSV,0.4, onGetRewardClick)
		local hasGot = false
		local canGet = false
		if self.info.log ~= nil and type(self.info.log) == "table" and self.info.log[k] ~= nil then
			hasGot = true
		end
		if self.info.score >= itemInfo.targetNum then
			canGet = true
		end
		UIRewardInfoItem.RefreshRewardItem(item, hasGot, canGet, tostring(self.info.score))
		self.rewardTable[#self.rewardTable+1] = item
	end
	UIHelper.RepositionGrid(self.rewardGrid,self.rewardSV)
end

function UIActModule1:OnGetRewardClick(go)
	local li = UIHelper.GetUIEventListener(go)
	local onGetReward = function( data )
		self:OnGetReward(data)
	end
	ActivityData.ReqFetchTimeActiveReward(onGetReward, li.parameter.id)
end

function UIActModule1:OnGetReward( data )
	local idx = 0
	for i,v in ipairs(self.rewardTable) do
		if v.info.id == tostring(data) then
			idx = i
			UIRewardInfoItem.RefreshRewardItem(v,true,true,"")
			break
		end
	end
	local itemList = {}
	itemList.item = {}
	local rewardItem = self.rewardTable[idx].info.rewardList
	for i,v in ipairs(rewardItem) do
		table.insert(itemList.item,{id=tostring(v[2]),num=v[3]})
	end
	local params = {}
	params.m_itemTb = itemList
	params.titleName = GetLocalizedString("GetActReward")
	WindowMgr.ShowWindow(LuaConst.Const.UIGetItem,params)
end

function UIActModule1:OnHide()
end

function UIActModule1:OnDestroy()
	print("destroy")
end
