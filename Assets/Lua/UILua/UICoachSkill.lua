module("UICoachSkill",package.seeall)

require "Common/UnityCommonScript"
require "Common/CommonScript"
require "Game/CoachData"
require "UILua/UICoachSkillItem"
require "UILua/UICoachSkillSlotItem"
require "UILua/UIIconItem"--共有道具
require "Game/ItemSys"
require "Game/Role"
require "UILua/UIBubbleText"

local qualityUI = 
{
	"UICSQAll",
	"UICSQBeginner",
	"UICSQIntermediate",
	"UICSQAdvanced"
}
local qualityUIText = nil
 
local transform = nil
local windowComponent = nil 
--right
local qualityBtn = nil
local qualityBtnLbl  = nil
local skillItemSV = nil
local skillItemGrid = nil
local skillItemSVPb = nil --progressbar
local skillSlotItemGrid = nil
local additionBtn = nil
local addWin = nil
local unlockSlotWin = nil
local fullTipTrans = nil
--left
local descPanel = nil
local descTP = nil
local hideBtn = nil
local iconSprite = nil
local nameLbl = nil
local lvLbl = nil
local typeLbl = nil
local basicAttrLbl = nil
local basicAttrValueLbl = nil
local equipAttrLbl = nil
local equipAttrValueLbl = nil
local upgradeTrans = nil
local upgradeTitleLbl = nil
local upgradeCondGrid = nil
local costLbl = nil
local disableTipLbl = nil
local upgradeBtn = nil
local upgradeBtnLi = nil
local upgradeBtnEnableLbl = nil --未激活则修改btn按钮文字
local upgradeBtnDisableLbl = nil

local slotItemList = nil --装备技能框对象集合，包括没有解锁的
local slotInfo = nil --装备技能框数据
local skillItemInfo = nil --{info, trans}技能数据+对象集合
local skillCount = nil
local skillSvItemCount = nil
local toValue = nil
local fromValue = nil
local frequency = 60
local timeTick = nil
local skillDescOpen = nil
local skillDescSkillID = nil
local skillDescSkillLv = nil
local serverData = nil
local qualityDefaultId = nil
local isInit = nil

function Init( trans, winComponent )
	transform = trans
	windowComponent = winComponent
	if qualityUIText == nil then
		qualityUIText = {}
		for i=1,#qualityUI do
			table.insert(qualityUIText, Util.LocalizeString(qualityUI[i]))
		end
	end
	slotInfo = {}
	skillItemInfo = {}
	skillDescOpen = false
	skillDescSkillID = 0
	skillDescSkillLv = -1
	skillCount = 0
	qualityDefaultId =1
	UICoachSkillItem.Init(windowComponent)
	UICoachSkillSlotItem.Init(windowComponent)
	UIIconItem.Init()
	BindUI()
	isInit = true
end
function BindUI()
	qualityBtn = TransformFindChild(transform, "QualityListButton")
	qualityBtnLbl = TransformFindChild(qualityBtn, "Label")
	Util.AddClick(qualityBtn.gameObject, QualityFilterClick)
	UIHelper.SetLabelTxt(qualityBtnLbl, qualityUIText[qualityDefaultId])
	skillItemSV = TransformFindChild(transform, "SkillScrollView")
	skillItemGrid = TransformFindChild(transform, "SkillScrollView/Grid")
	skillItemSVPb = TransformFindChild(transform, "ProgressBar")
	skillSlotItemGrid = TransformFindChild(transform, "EquipInfo/SlotItemList")
	additionBtn = TransformFindChild(transform, "EquipInfo/AddInfoButton")
	Util.AddClick(additionBtn.gameObject, ViewSkillAdditionClick)
	addWin = TransformFindChild(transform, "EquipInfo/PopupWin/AddInfoWin")
	unlockSlotWin = TransformFindChild(transform, "EquipInfo/PopupWin/UnlockSlotWin")
	fullTipTrans = TransformFindChild(transform, "EquipInfo/FullTipParent")
	
	descPanel = TransformFindChild(transform, "SkillDetailInfo")
	descTP = TransformFindChild(descPanel, "TweenPosition")
	hideBtn = TransformFindChild(descTP, "CloseButton")
	Util.AddClick(hideBtn.gameObject, SkillDescCloseClick)
	iconSprite = TransformFindChild(descTP, "Info/Icon")
	lvLbl = TransformFindChild(descTP, "Info/Lv")
	typeLbl = TransformFindChild(descTP, "Info/Type")
	nameLbl = TransformFindChild(descTP, "Info/Name")
	basicAttrLbl = TransformFindChild(descTP, "Info/BasicAttr")
	basicAttrValueLbl = TransformFindChild(basicAttrLbl, "Value")
	equipAttrLbl = TransformFindChild(descTP, "Info/EquipAttr")
	equipAttrValueLbl = TransformFindChild(equipAttrLbl, "Value")
	upgradeTrans = TransformFindChild(descTP, "Info/UpgradeCondition")
	upgradeTitleLbl = TransformFindChild(descTP, "Info/UpgradeCondition")
	upgradeCondGrid = TransformFindChild(upgradeTrans, "ItemGrid")
	costLbl = TransformFindChild(upgradeTrans, "GoldCost/Value")
	disableTipLbl = TransformFindChild(descTP, "Info/DisableTips")
	upgradeBtn = TransformFindChild(descTP, "Info/UpgradeButton")
	upgradeBtnEnableLbl = TransformFindChild(upgradeBtn, "EnableLbl")
	upgradeBtnDisableLbl = TransformFindChild(upgradeBtn, "DisableLbl")
	upgradeBtnLi = Util.AddClick(upgradeBtn.gameObject, UpgradeSkillClick)
	upgradeBtnLi.parameter = {id = 0}
	slotItemList = {}
	skillSvItemCount = UIHelper.GetPanelSize(skillItemSV).y / UIHelper.GetUIGridCellSize(skillItemGrid).y
	toValue = 0
	fromValue = 0
end
function OnShow()
	if isInit then
		CoachData.ReqCoachSkillInfo(InitSkillInfo)
		isInit = false
	else
		transform.gameObject:SetActive(true)
	end
end
function OnHide()
	transform.gameObject:SetActive(false)
end
function OnDestroy()
	qualityUIText = nil
 
	transform = nil
	windowComponent = nil 
	
	qualityBtn = nil
	qualityBtnLbl  = nil
	skillItemSV = nil
	skillItemGrid = nil
	skillItemSVPb = nil --progressbar
	skillSlotItemGrid = nil
	additionBtn = nil
	addWin = nil
	unlockSlotWin = nil
	fullTipTrans = nil
	
	descPanel = nil
	descTP = nil
	hideBtn = nil
	iconSprite = nil
	nameLbl = nil
	lvLbl = nil
	typeLbl = nil
	basicAttrLbl = nil
	basicAttrValueLbl = nil
	equipAttrLbl = nil
	equipAttrValueLbl = nil
	upgradeTrans = nil
	upgradeTitleLbl = nil
	upgradeCondGrid = nil
	costLbl = nil
	disableTipLbl = nil
	upgradeBtn = nil
	upgradeBtnLi = nil
	upgradeBtnEnableLbl = nil --未激活则修改btn按钮文字
	upgradeBtnDisableLbl = nil
	slotItemList = nil --装备技能框对象集合，包括没有解锁的
	slotInfo = nil --装备技能框数据
	skillItemInfo = nil --{info, trans}技能数据+对象集合
	skillDescOpen = nil
	skillDescSkillID = nil
	skillDescSkillLv = nil
	serverData = nil
	qualityDefaultId = nil
	isInit = nil
	skillCount = nil
	skillSvItemCount = nil
	toValue = nil
	fromValue = nil
	frequency = 60
	timeTick = nil

	UICoachSkillItem.OnDestroy()
	UICoachSkillSlotItem.OnDestroy()
	UIIconItem.OnDestroy()
end

--skillInfo = {id = info}
--info = {id,configID, configInfo,isEquiped,lv,quality}
function InitSkillInfo(data)
	slotInfo = data.skillSlotInfo
	--skillItem
	local skillInfo = {}
	for k,v in pairs(data.skillInfo) do
		local info = {}
		info.id = v.id
		if v.lv == 0 then
			info.configID = v.id.."1"
		else
			info.configID = v.id..v.lv
		end
		--info.configInfo = Config.GetKeyTotalInfo(Config.CoachSkill(), info.configID)
		info.isEquiped = IsEquiped(v.id)
		info.lv = v.lv
		info.quality = v.quality
		skillInfo[v.id] = info
	end
	local idx = 0
	local defaultObj = nil
	UIHelper.DestroyGrid(skillItemGrid)
	for k,v in CommonScript.PairsByOrderKey(skillInfo, "id", true) do
		idx = idx + 1
		local itemTrans = UICoachSkillItem.CreateSkillItem(v,idx,skillItemGrid,skillItemSV,EquipSkillClick,TakeOffSkillClick)
 		local li = Util.AddClick(itemTrans.gameObject, SkillItemClick)
 		li.parameter = {id = k}
 		if skillItemInfo[k] == nil then
 			skillItemInfo[k] = {}
 		end
		skillItemInfo[k].info = v
		skillItemInfo[k].trans = itemTrans
		skillCount = skillCount + 1
		skillItemInfo[k].idx = skillCount
		if idx == 1 then
			defaultObj = itemTrans.gameObject
		end
	end
	UIHelper.RepositionGrid(skillItemGrid)
	UIHelper.SetProgressBar(skillItemSVPb, 0)

	--skillSlot
	local roleLv = tonumber(Role.Get_lv())
	local vip = tonumber(Role.Get_vip())
	idx = 0
	UIHelper.DestroyGrid(skillSlotItemGrid)
	for k,v in CommonScript.PairsByOrderKey(slotInfo, "id", true) do
		local slotConfig = Config.GetKeyTotalInfo(Config.CoachSkillSlot(), v.id)
		--判断slot被锁因素
		if v.status == 1 then
			v.lockType = 0 --已解锁
		elseif roleLv < tonumber(slotConfig.lv_require) then
			v.lockType = 1
		elseif vip < tonumber(slotConfig.vip_require) then
			v.lockType = 2
		else
			v.lockType = 3
		end
		idx = idx + 1
		local slotItem = UICoachSkillSlotItem.CreateSkillSlotItem(v, idx, skillSlotItemGrid)

		local li = Util.AddClick(slotItem.gameObject, SkillSlotItemClick)
		li.parameter = {id = k}
		slotItemList[k] = slotItem
	end
	UIHelper.RepositionGrid(skillSlotItemGrid)

	SkillItemClick(defaultObj) --默认显示第一个技能描述
	transform.gameObject:SetActive(true)
end

function IsEquiped(skillID)
	for k,v in pairs(slotInfo) do
		if v.status == 1 then
			if v.skill_id == skillID then
				return true
			end
		end
	end
	return false
end

function CanEquipSkill()
	for k,v in pairs(slotInfo) do
		if v.status == 1 and v.skill_id == 0 then
			return true
		end
	end
	return false
end
--筛选显示技能品质
function QualityFilterClick( go )
	local param = {}
	param.title = Util.LocalizeString("UICSQualityType")
	param.contentList = qualityUIText
	param.confirmCB = SkillFilterByCurValue
	param.defaulFilterId = qualityDefaultId
	WindowMgr.ShowWindow(LuaConst.Const.UIListFilter, param)
end
--具体筛选规则待定
function SkillFilterByCurValue(key)
	qualityDefaultId = key
	UIHelper.SetLabelTxt(qualityBtnLbl, qualityUIText[key])
	for k,v in pairs(skillItemInfo) do
		if key == 1 then
			v.trans.gameObject:SetActive(true)
		elseif v.info.quality <= (key -1) *2 and v.info.quality > (key - 2) * 2 then
			v.trans.gameObject:SetActive(true)
		else
			v.trans.gameObject:SetActive(false)
		end
	end
	UIHelper.RepositionGrid(skillItemGrid)
	UIHelper.SetProgressBar(skillItemSVPb, 0)
end
--预览技能加成
function ViewSkillAdditionClick(go)
	local addTable = {}
	--基本属性加成
	for k,v in pairs(skillItemInfo) do
		local info = v.info
		if info.lv > 0 then
			local basicAttrStrList = Config.GetProperty(Config.CoachSkill(),info.configID, "basic_attr_value"):split(';')
			addTable = AddAttrToTable(addTable,basicAttrStrList, true, 0)
			local basicAttrThsStrList = Config.GetProperty(Config.CoachSkill(),info.configID, "basic_attr_ths"):split(';')
			addTable = AddAttrToTable(addTable,basicAttrThsStrList, false, 0)
		end
	end
	--装备属性加成，一些针对特殊位置球员的加成todo
	for k,v in pairs(slotInfo) do
		if v.status == 1 and v.skill_id ~= 0 then
			local equipSkillInfo = skillItemInfo[v.skill_id].info
			local equipTar = Config.GetProperty(Config.CoachSkill(), equipSkillInfo.configID, "equip_target")
			local equipAttrStrList = Config.GetProperty(Config.CoachSkill(), equipSkillInfo.configID, "equip_attr_value"):split(';')
			--特殊装备目标待加新的处理
			addTable = AddAttrToTable(addTable,equipAttrStrList, true, equipTar)
			local equipAttrThsStrList = Config.GetProperty(Config.CoachSkill(), equipSkillInfo.configID, "equip_attr_ths"):split(';')
			addTable = AddAttrToTable(addTable,equipAttrThsStrList, false, equipTar)
		end
	end
	--弹出窗口
	OpenAdditionWin(UIHelper.GetPanelDepth(descPanel)+1, addTable)
end

function AddAttrToTable( t, attrList, isValue, tar )
	for k,v in pairs(attrList) do
		local pair = v:split(',')
		local key = pair[1]  -- 需要加上对tar的分类数据处理
		if t[key] == nil then
			t[key] = {}
			t[key].id = key
			t[key].value = 0
			t[key].ths = 0
		end
		if isValue then
			t[key].value = t[key].value + tonumber(pair[2])
		else
			t[key].ths = t[key].ths + tonumber(pair[2])
		end
	end
	return t
end

--弹出加成窗口
function OpenAdditionWin(depth, contentTable)
	UIHelper.SetPanelDepth(addWin, depth)
	local sv = TransformFindChild(addWin, "Scroll View")
	UIHelper.SetPanelDepth(sv, depth+1)
	local contentLbl = TransformFindChild(addWin, "Scroll View/Content")
	UIHelper.SetLabelTxt(contentLbl, ParseAttr2Str(contentTable))
	local closeFun = function(go)
		addWin.gameObject:SetActive(false)
	end
	local closeBtn = TransformFindChild(addWin, "Close")
	local confirmBtn = TransformFindChild(addWin, "ConfirmBtn")
	local bgBlock = TransformFindChild(addWin, "BGBlock")
	Util.ChangeClick(bgBlock.gameObject, closeFun)
	Util.ChangeClick(closeBtn.gameObject, closeFun)
	Util.ChangeClick(confirmBtn.gameObject, closeFun)
	addWin.gameObject:SetActive(true)
end
--点击升级技能
function UpgradeSkillClick(go)
	local skillId = upgradeBtnLi.parameter.id -- UIHelper.GetUIEventListener(go).parameter.id
	CoachData.ReqUpgradeSkill(UpgradeSkillCallback, skillId)
end
function UpgradeSkillCallback(data)
	local id = data.skill_id
	local lv = data.skill_lv
	skillItemInfo[id].info.configID = id..lv
	skillItemInfo[id].info.lv = lv
	--skillItemInfo[id].info.configInfo = Config.GetKeyTotalInfo(Config.CoachSkill(), skillItemInfo[id].info.configID)
	--刷新左侧技能描述
	RefreshSkillDesc(id)
	--刷新技能item
	local nextConfig = Config.GetKeyTotalInfo(Config.CoachSkill(), id..(lv + 1))
	local isMax =false
	if nextConfig == nil then
		isMax = true
	end
	UICoachSkillItem.UpgradeSkill(skillItemInfo[id].trans, lv, isMax)
end

function RefreshSkillDescByEquip( curSkillId, isEquip )
	if skillDescSkillID == curSkillId then
		if isEquip then
			UIHelper.SetWidgetColor(equipAttrLbl, "win_wb_20")
			UIHelper.SetWidgetColor(equipAttrValueLbl, "win_wb_20")
		else
			UIHelper.SetWidgetColor(equipAttrLbl, "win_wa_20")
			UIHelper.SetWidgetColor(equipAttrValueLbl, "win_wa_20")
		end
	end
end

--装备技能
function EquipSkillClick(go)
	--查看是否有空余的技能槽，没有则弹漂浮字

	if not CanEquipSkill() then
		UIBubbleText.NewBubble(Util.LocalizeString("UICSEquipSlotFull"))
		return
	end 
	local skillId = UIHelper.GetUIEventListener(go).parameter.id
	CoachData.ReqLoadSkill(EquipSkillCallback, skillId, 0)
end
function EquipSkillCallback( data )
	local skillId = data.skill_id
	local slotId = data.skill_slot_id

	slotInfo[slotId].skill_id = skillId
	UICoachSkillSlotItem.Equip(slotItemList[slotId], skillId)
	UICoachSkillItem.SetSkillItemStatus(skillItemInfo[skillId].trans, 2)
	--更新detail界面信息
	RefreshSkillDescByEquip(skillId, true)
end

--卸下技能
function TakeOffSkillClick(go)
	local skillId = UIHelper.GetUIEventListener(go).parameter.id
	CoachData.ReqLoadSkill(TakeOffSkillCallback, skillId, 1)
end
function TakeOffSkillCallback( data )
	local skillId = data.skill_id
	local slotId = data.skill_slot_id
	slotInfo[slotId].skill_id = 0
	UICoachSkillSlotItem.TakeOff(slotItemList[slotId], 0)
	UICoachSkillItem.SetSkillItemStatus(skillItemInfo[skillId].trans, 1)
	RefreshSkillDescByEquip(skillId,false)
end

--点击技能item
function SkillItemClick( go )
	local li = UIHelper.GetUIEventListener(go)
	local skillId = li.parameter.id
	--修改选中状态
	for k,v in pairs(skillItemInfo) do
		UICoachSkillItem.Selected(v.trans, v.info.id == skillId)
	end
	upgradeBtnLi.parameter.id = skillId
	RefreshSkillDesc(skillId)
end

--attrTable={[id] = {id, value, ths}}
function ParseAttr2Str( attrTable )
	local str = ""
	for k,v in CommonScript.PairsByOrderKey(attrTable, "id", true) do
		if str ~= "" then
			str = str.."\n"
		end
		str = str..Config.GetProperty(Config.heroAttTB, k, "name")
		local hasValue = false
		if v.value ~= nil and v.value ~= 0 then
			str = str.."+"..v.value
			hasValue = true
		end
		if v.ths ~= nil and v.ths ~= 0 then
			if hasValue then
				str = str..", +"..string.format("%.1f",v.ths/10).."%"
			else
				str = str .."+"..string.format("%.1f",v.ths/10).."%"
			end
		end
	end
	return str
end

function RefreshSkillDesc( skillId )
	local info = skillItemInfo[skillId].info
	local config = Config.GetKeyTotalInfo(Config.CoachSkill(), info.configID)
	if not skillDescOpen then
		UIHelper.TweenPositionPlayForward(descTP, false)
		skillDescOpen = true
	end
	if skillDescSkillID == skillId and skillDescSkillLv == info.lv then
		return
	end
	skillDescSkillID = skillId
	skillDescSkillLv = info.lv
	--UIHelper.SetSpriteNameNoPerfect(iconSprite, "") --技能图标
	UIHelper.SetLabelTxt(nameLbl, config.skill_name)
	if info.lv == 0 then
		UIHelper.SetLabelTxt(lvLbl, "")
		UIHelper.SetWidgetColor(basicAttrLbl, "win_wa_20")
		UIHelper.SetWidgetColor(basicAttrValueLbl, "win_wa_20")
	else
		UIHelper.SetWidgetColor(basicAttrLbl, "win_wb_20")
		UIHelper.SetWidgetColor(basicAttrValueLbl, "win_b_20")
		UIHelper.SetLabelTxt(lvLbl, "Lv."..info.lv)
	end
	local ty = config.skill_type or 0
	UIHelper.SetLabelTxt(typeLbl,GetSkillTypeStr(ty))
	if IsEquiped(skillId) then
		UIHelper.SetWidgetColor(equipAttrLbl, "win_wb_20")
		UIHelper.SetWidgetColor(equipAttrValueLbl, "win_b_20")
	else
		UIHelper.SetWidgetColor(equipAttrLbl, "win_wa_20")
		UIHelper.SetWidgetColor(equipAttrValueLbl, "win_wa_20")
	end
	local basicAttr = config.basic_attr_value:split(';')
	local basicAttrThs = config.basic_attr_ths:split(';')
	local equipAttr = config.equip_attr_value:split(';')
	local equipAttrThs = config.equip_attr_ths:split(';')
	local tar = config.basic_target -- 标识属性加这那个位置如：前锋，门将等todo
	local basicAttrTable = {}
	basicAttrTable = AddAttrToTable(basicAttrTable,basicAttr, true, 0)
	basicAttrTable = AddAttrToTable(basicAttrTable, basicAttrThs, false, 0)
	UIHelper.SetLabelTxt(basicAttrValueLbl, ParseAttr2Str(basicAttrTable))
	--装备属性之后有其他描述 ，之后可能要分到其他函数中处理
	local equipAttrTable ={}
	equipAttrTable = AddAttrToTable(equipAttrTable,equipAttr, true, tar)
	equipAttrTable = AddAttrToTable(equipAttrTable, equipAttrThs, false, tar)
	UIHelper.SetLabelTxt(equipAttrValueLbl, ParseAttr2Str(equipAttrTable))
	UIHelper.SetLabelTxt(disableTipLbl, "")
	--升级条件
	local nextLvlConfig = Config.GetKeyTotalInfo(Config.CoachSkill(), info.id..(info.lv+1))
	if nextLvlConfig == nil then --最高等级
		upgradeTrans.gameObject:SetActive(false)
		upgradeBtn.gameObject:SetActive(false)
		UIHelper.SetLabelTxt(lvLbl, "Lv."..info.lv.." MAX")
	else
		upgradeTrans.gameObject:SetActive(true)
		upgradeBtn.gameObject:SetActive(true)
		local upgradeReq = nextLvlConfig.upgrade_require:split(';')
		UIHelper.DestroyGrid(upgradeCondGrid)
		local canUpgrade = true
		local notEnoughCol = Color.New(153/255, 51/255, 51/255, 1)
		for i,v in ipairs(upgradeReq) do
			local pair = v:split(',')
			local enough = true
			local ownNum = ItemSys.GetItemData(pair[1]).num
			if ownNum < tonumber(pair[2]) then
				canUpgrade = false
				enough = false
			end
			if pair[1] ~= LuaConst.Const.SB then
				local col = nil
				if not enough then
					col = notEnoughCol
				end
 				UIIconItem.CreateRewardIconItem(upgradeCondGrid, nil, pair, {scale = 0.5, color = col})
 			else
 				if not enough then
 					UIHelper.SetWidgetColor(costLbl, notEnoughCol)
 				else
 					UIHelper.SetWidgetColor(costLbl, "win_wa_20")
 				end
 				UIHelper.SetLabelTxt(costLbl, ConvertNumber(tonumber(pair[2])))
 			end
		end
		UIHelper.RepositionGrid(upgradeCondGrid)
		if info.lv == 0 then
			UIHelper.SetLabelTxt(upgradeBtnEnableLbl, Util.LocalizeString("UICSActiveBtn"))
			UIHelper.SetLabelTxt(upgradeBtnDisableLbl, Util.LocalizeString("UICSActiveBtn"))
			UIHelper.SetLabelTxt(upgradeTitleLbl, Util.LocalizeString("UICSActiveTitle"))
			if not canUpgrade then
				UIHelper.SetLabelTxt(disableTipLbl, Util.LocalizeString("UICSActiveTitle")..Util.LocalizeString("UICSUpgradeItemNotEnough"))
			end
		else
			UIHelper.SetLabelTxt(upgradeBtnEnableLbl, Util.LocalizeString("UICSUpgradeBtn"))
			UIHelper.SetLabelTxt(upgradeBtnDisableLbl, Util.LocalizeString("UICSUpgradeBtn"))
			UIHelper.SetLabelTxt(upgradeTitleLbl, Util.LocalizeString("UICSUpgradeTitle"))
			if not canUpgrade then
				UIHelper.SetLabelTxt(disableTipLbl, Util.LocalizeString("UICSUpgradeTitle")..Util.LocalizeString("UICSUpgradeItemNotEnough"))
			end
		end
		upgradeBtnLi.parameter.id = info.id
		CommonScript.SetButtonActive(upgradeBtn, canUpgrade)
	end
end

function GetSkillTypeStr( ty )
	return UICoachSkillItem.GetSkillTypeStr(ty)
end

--关闭技能详情
function SkillDescCloseClick( go )
	UIHelper.TweenPositionPlayReverse(descTP, false)
	for k,v in pairs(skillItemInfo) do
		UICoachSkillItem.Selected(v.trans, false)
	end
	skillDescOpen = false
end
--点击skillslot
function SkillSlotItemClick( go )
	local li = UIHelper.GetUIEventListener(go)
	local slotID = li.parameter.id
	local curSlot = slotInfo[slotID]
	--根据slot状态区别对待
	if curSlot.status == 0 then --弹出解锁界面
		ShowUnlockSlotWin(UIHelper.GetPanelDepth(descPanel), curSlot)
	elseif curSlot.skill_id == 0 then
	else --点击了装有技能的技能槽
		local skillID = curSlot.skill_id 
		CenterOnSkillItem(skillID)
	end
end

function CenterOnSkillItem( skillID )
	SkillItemClick(skillItemInfo[skillID].trans.gameObject)
	if skillSvItemCount < skillCount then
		toValue = (skillItemInfo[skillID].idx -1) / (skillCount - skillSvItemCount)
	end
	if toValue > 1 then
		toValue = 1
	end
	fromValue = UIHelper.GetProgressBar(skillItemSVPb)
	if timeTick ~= nil then
		LuaTimer.RmvTimer(timeTick)
	end
	timeTick = LuaTimer.AddTimer(false, -1/frequency, UpdateSkillPb)
end
function UpdateSkillPb()
	local duration = 0.2
	local endFun = function()
		LuaTimer.RmvTimer(timeTick)
		timeTick = nil
	end
	CommonScript.UpdatePBValueByDivision(skillItemSVPb, fromValue, toValue, duration*frequency, endFun)
end

function ShowUnlockSlotWin(depth, slot)
	UIHelper.SetPanelDepth(unlockSlotWin, depth)
	local closeFun = function(go)
		unlockSlotWin.gameObject:SetActive(false)
	end
	local closeBtn = TransformFindChild(unlockSlotWin, "Close")
	local diamondTrans = TransformFindChild(unlockSlotWin, "LockDiamond")
	local lvlTrans = TransformFindChild(unlockSlotWin, "LockLevel")
	local bg = TransformFindChild(unlockSlotWin, "BGBlock")
	Util.ChangeClick(closeBtn.gameObject, closeFun)
	Util.ChangeClick(bg.gameObject, closeFun)
	local lockByCost = slot.lockType == 3
	diamondTrans.gameObject:SetActive(lockByCost)
	lvlTrans.gameObject:SetActive(not lockByCost)
	local config = Config.GetKeyTotalInfo(Config.CoachSkillSlot(), slot.id)
	if lockByCost then
		local diamondConfirmBtn = TransformFindChild(diamondTrans, "ConfirmBtn")
		local diamondCancelBtn = TransformFindChild(diamondTrans, "CancelBtn")
		local diamondLbl = TransformFindChild(diamondTrans, "Icon/Value")
		local itemCostStr = config.item:split(';')
		local cost = 0
		for k,v in pairs(itemCostStr) do
			local pair = v:split(',')
			if pair[1] == LuaConst.Const.GB then
				cost = tonumber(pair[2])
				break
			end
		end
		UIHelper.SetLabelTxt(diamondLbl, cost)
		local unlockFun = function(go)
			CoachData.ReqUnlockSkillSlot(UnlockSlotCallback, slot.id)
			closeFun()
		end
		Util.ChangeClick(diamondCancelBtn.gameObject, closeFun)
		Util.ChangeClick(diamondConfirmBtn.gameObject, unlockFun)
	else
		local lvlConfirmBtn = TransformFindChild(lvlTrans, "ConfirmBtn")
		local lvlDescLbl = TransformFindChild(lvlTrans, "Label")
		Util.ChangeClick(lvlConfirmBtn.gameObject, closeFun)
		if slot.lockType == 1 then
			UIHelper.SetLabelTxt(lvlDescLbl, string.format(Util.LocalizeString("UICSUnlockSlotLv"),config.lv_require))
		elseif slot.lockType == 2 then
			UIHelper.SetLabelTxt(lvlDescLbl, string.format(Util.LocalizeString("UICSUnlockSlotVip"),config.vip_require))
		end
	end
	unlockSlotWin.gameObject:SetActive(true)
end

function UnlockSlotCallback( data )
	local slotId = data
	slotInfo[slotId].status = 1
	slotInfo[slotId].skill_id = 0
	UICoachSkillSlotItem.Unlock(slotItemList[slotId])
end

