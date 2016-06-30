module("UICarnivalItem", package.seeall)

require "Common/CommonScript"
require "Config"

local itemPref = nil

local CarnivalItem = 
{
	id = nil,
	aim = nil,
	transform = nil,
	getBtn = nil,
	completionLbl = nil,
	hasGotLbl = nil
}
CarnivalItem.__index = CarnivalItem

function Init( winComponent )
	if itemPref == nil then
		itemPref = winComponent:GetPrefab("CarnivalItem")
	end
end


--info={}
function Create(configInfo, grid, onGet)
	local item = {}
	setmetatable(item, CarnivalItem)
	item.id = configInfo.id
	item.aim = configInfo.val
	local transform = InstantiatePrefab(itemPref,grid,configInfo.id).transform
	item.transform = transform
	local iconTex = TransformFindChild(transform, "Icon") 
	local rewardDescLbl = TransformFindChild(transform, "Reward/Value")
	local conditionDescLbl = TransformFindChild(transform, "Condition/Describle")
	item.getBtn = TransformFindChild(transform, "GetState/Get/Btn")
	item.compLbl = TransformFindChild(transform, "GetState/Get/Completion")
	item.hasGotLbl = TransformFindChild(transform, "GetState/HasGotLabel")
	local li = Util.ChangeClick(item.getBtn.gameObject, onGet)
	li.parameter = {id = item.id}
	UIHelper.SetLabelTxt(conditionDescLbl, configInfo.desc)
	local point = configInfo.point
	local reward = configInfo.item
	local rewardId = reward[1]
	local rewardNum = reward[2]
	local rewardName = Config.GetProperty(Config.ItemTable(), rewardId, "name")
	local rewardIcon = Config.GetProperty(Config.ItemTable(), rewardId, "icon")
	UIHelper.SetLabelTxt(rewardDescLbl, GetLocalizedString("UICItemRewardFormat", rewardName,rewardNum,point))
	Util.SetUITexture(iconTex, LuaConst.Const.ItemIcon, rewardIcon, false)

	return item
end

function CarnivalItem:RefreshStatus( completeData, submitData )
	if submitData ~= nil then
		self.getBtn.gameObject:SetActive(false)
		self.compLbl.gameObject:SetActive(false)
		self.hasGotLbl.gameObject:SetActive(true)
	else
		self.getBtn.gameObject:SetActive(true)
		self.compLbl.gameObject:SetActive(true)
		self.hasGotLbl.gameObject:SetActive(false)
		local cur = completeData or 0
		UIHelper.SetLabelTxt(self.compLbl, ConvertNumber(cur).."/"..ConvertNumber(self.aim))
		if cur >= self.aim then
			CommonScript.SetButtonActive(self.getBtn, true)
			UIHelper.SetWidgetColor(self.compLbl, "win_b_20")
		else
			CommonScript.SetButtonActive(self.getBtn, false)
			UIHelper.SetWidgetColor(self.compLbl, "win_wb_20")
		end
	end
end

function OnDestroy()
	itemPref = nil
end