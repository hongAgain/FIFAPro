module("UICoachSkillItem", package.seeall)

require "Config"

local skillItemPrefab = nil

function Init(winComponent)
	if skillItemPrefab == nil then
		skillItemPrefab = winComponent:GetPrefab("SkillBasicInfoItem")
	end
end

function CreateSkillItem(itemInfo, idx, grid, scrollview, onEquipment, onTakeoff)
	local config = Config.GetKeyTotalInfo(Config.CoachSkill(), itemInfo.configID)
	local cloneTrans = InstantiatePrefab(skillItemPrefab,grid,idx).transform
	UIHelper.SetDragScrollViewTarget(cloneTrans,scrollview)
	local icon = TransformFindChild(cloneTrans, "Icon")
	local name = TransformFindChild(cloneTrans, "Name")
	local lvLbl = TransformFindChild(cloneTrans, "Lv")
	local ty = TransformFindChild(cloneTrans, "Type")
	--UIHelper.SetSpriteNameNoPerfect(icon, "")--icon等美术做好再加
	UIHelper.SetLabelTxt(name, config.skill_name)
	local nextConfig = Config.GetKeyTotalInfo(Config.CoachSkill(), itemInfo.id..(itemInfo.lv + 1))
	if nextConfig == nil then
		UIHelper.SetLabelTxt(lvLbl, "Lv."..itemInfo.lv.." MAX")
	else
		UIHelper.SetLabelTxt(lvLbl, "Lv."..itemInfo.lv)
	end
	UIHelper.SetLabelTxt(ty, GetSkillTypeStr((config.skill_type or 0)))
	local equipBtn = TransformFindChild(cloneTrans, "Status/EquipButton")
	local takeoffBtn = TransformFindChild(cloneTrans, "Status/TakeoffButton")
	local equipLi = Util.AddClick(equipBtn.gameObject, onEquipment)
	local takeoffLi = Util.AddClick(takeoffBtn.gameObject, onTakeoff)
	equipLi.parameter = {id = itemInfo.id}
	takeoffLi.parameter = {id = itemInfo.id}

	if itemInfo.lv == 0 then
		SetSkillItemStatus(cloneTrans, 0)
	elseif not itemInfo.isEquiped then
		SetSkillItemStatus(cloneTrans, 1)
	elseif itemInfo.isEquiped then
		SetSkillItemStatus(cloneTrans, 2)
	else
		SetSkillItemStatus(cloneTrans, 0)
	end
	return cloneTrans
end

--status:0-未激活，1-未装备，2-已装备
function SetSkillItemStatus( itemTrans, status )
	local notActive = TransformFindChild(itemTrans, "Status/NotActive").gameObject
	local equip = TransformFindChild(itemTrans, "Status/EquipButton").gameObject
	local takeoff = TransformFindChild(itemTrans, "Status/TakeoffButton").gameObject
	local lv = TransformFindChild(itemTrans, "Lv").gameObject
	notActive:SetActive(status == 0)
	lv:SetActive(status ~= 0)
	equip:SetActive(status == 1)
	takeoff:SetActive(status == 2)
end
function UpgradeSkill( itemTrans, lv, isMax )
	--根据等级来，lv==1则为新激活的技能
	if lv == 1 then
		SetSkillItemStatus(itemTrans, 1)
	end
	local lvLbl = TransformFindChild(itemTrans, "Lv")
	if isMax then
		UIHelper.SetLabelTxt(lvLbl, "Lv."..lv.." MAX")
	else
		UIHelper.SetLabelTxt(lvLbl, "Lv."..lv)
	end
end
function Selected( itemTrans, active )
	local selectObj = TransformFindChild(itemTrans, "Select").gameObject
	selectObj:SetActive(active)
end

function GetSkillTypeStr( ty )
	if ty == 0 then
		return ""
	end
	--添加其他类型对应的文字
	return ""
end

function OnDestroy()
	skillItemPrefab = nil
end