module("UICarnivalPopWinItem", package.seeall)

require "Common/CommonScript"
require "Config"
require "UILua/UIIconItem"

local itemPref = nil
local PopWinType = nil

local CarnivalPopWinItem = 
{
	id = nil,
	aim = nil,
	transform = nil,
	getBtn = nil,
	hasGotLbl = nil
}
CarnivalPopWinItem.__index = CarnivalPopWinItem

function Init( winComponent ,typeInfo)
	if itemPref == nil then
		itemPref = winComponent:GetPrefab("CarnivalPopWinItem")
	end
	PopWinType = typeInfo
	UIIconItem.Init()
end

function Create( ty, configInfo, grid, onBtnClick )
	local item = {}
	setmetatable(item, CarnivalPopWinItem)
	item.id = configInfo.id
	item.aim = tonumber(item.id)
	local transform = InstantiatePrefab(itemPref, grid, item.id).transform
	item.transform = transform
	local titleLbl = TransformFindChild(transform, "Title")
	local grid = TransformFindChild(transform,"RewardList")
	local prTrans = TransformFindChild(transform, "PointReward")
	local sgTrans = TransformFindChild(transform, "SpGift")
	local param = {scale = 0.4, offsetDepth = 2}
	if ty == PopWinType.PointReward then
		prTrans.gameObject:SetActive(true)
		sgTrans.gameObject:SetActive(false)
		UIHelper.SetLabelTxt(titleLbl, GetLocalizedString("UICSRItemTitle",item.id))
		item.getBtn = TransformFindChild(prTrans, "GetState/Btn")
		item.hasGotLbl = TransformFindChild(prTrans, "GetState/HasGotLabel")
		local reward = configInfo.item
		for i=1,#reward,3 do
			local itemId = reward[i]
			local itemNum = reward[i+1]
			UIIconItem.CreateRewardIconItem(grid, nil, {itemId,itemNum}, param)
		end
	elseif ty == PopWinType.SpGift then
		prTrans.gameObject:SetActive(false)
		sgTrans.gameObject:SetActive(true)
		UIHelper.SetLabelTxt(titleLbl, GetLocalizedString("UICSGItemTitle",item.id))
		item.getBtn = TransformFindChild(sgTrans, "GetState/Btn")
		item.hasGotLbl = TransformFindChild(sgTrans, "GetState/HasGotLabel")
		local oPriceLbl = TransformFindChild(sgTrans, "GetState/Original/Value")
		local curPriceLbl = TransformFindChild(sgTrans, "GetState/Current/Value")
		UIHelper.SetLabelTxt(oPriceLbl, configInfo.origin_price)
		UIHelper.SetLabelTxt(curPriceLbl, configInfo.price)
		local reward = configInfo.item
		for i=1,#reward,2 do
			local itemId = reward[i]
			local itemNum = reward[i+1]
			UIIconItem.CreateRewardIconItem(grid, nil, {itemId,itemNum}, param)
		end
	end
	local li = Util.ChangeClick(item.getBtn.gameObject, onBtnClick)
	li.parameter = {id = item.id}

	return item
end

function CarnivalPopWinItem:RefreshStatus( getPoint, getData )
	if getData ~= nil then
		self.hasGotLbl.gameObject:SetActive(true)
		self.getBtn.gameObject:SetActive(false)
	else
		self.hasGotLbl.gameObject:SetActive(false)
		self.getBtn.gameObject:SetActive(true)
		CommonScript.SetButtonActive(self.getBtn, getPoint >= self.aim)
	end
end

function OnDestroy()
	itemPref = nil
	PopWinType = nil
end