module("UICoachTalent",package.seeall)

require "Common/CommonScript"
require "Game/Role"
require "Game/ItemSys"

local TalentSkillType = 
{
	["Attack"] = 1,
	["Defence"] = 2,
	["Comprehensive"] = 3
}

local pageSelectColor = nil
local pageUnSelectColor = nil

local lockColor = nil
local unlockColor = nil

local transform = nil
local windowComponent = nil

local talentPageTransList = nil --天赋页，3个toggle
local talentTypeTransList = nil --天赋分类，3个toggle
local remainPointLbl = nil
local usedPointLbl = nil
local resetPointBtn = nil
local talentTreeSV = nil
local talentTreeSB = nil
local curTypeSkillList = nil --天赋技能列表
local upgradeSkillWin = nil
local resetPointWin = nil
local unlockPageWin = nil

--data
local totalPoint = nil
local pageStatus = nil
local currentPage = nil --当前技能页
local talentInfo = nil --天赋树信息{pageIdx = {type = {}, status = 0}}
local isInit = nil

function Init( trans, winComponent )
	transform = trans
	windowComponent = winComponent
	pageSelectColor = Color.New(102/255,204/255,1,1)
	pageUnSelectColor = Color.New(125/255,128/255,137/255,1)
	unlockColor = Color.New(1,1,1,1)
	lockColor = Color.New(66/255,66/255,66/255,1)
	
	totalPoint = 0
	currentPage = 1
	isInit = true
	talentInfo = {}
	pageStatus = {}
	BindUI()
	transform.gameObject:SetActive(false)
end
function BindUI()
	talentPageTransList = {}
	talentPageTransList[1] = TransformFindChild(transform, "PageContainer/1")
	talentPageTransList[2] = TransformFindChild(transform, "PageContainer/2")
	talentPageTransList[3] = TransformFindChild(transform, "PageContainer/3")
	for i,v in ipairs(talentPageTransList) do
		local li = Util.AddClick(v.gameObject, PageChangeClick)
		li.parameter = {page = i}
	end
	talentTypeTransList = {}
	talentTypeTransList[1] = TransformFindChild(transform, "TypeContainer/Attack")
	talentTypeTransList[2] = TransformFindChild(transform, "TypeContainer/Defence")
	talentTypeTransList[3] = TransformFindChild(transform, "TypeContainer/Comprehensive")
	for i,v in ipairs(talentTypeTransList) do
		UIHelper.AddToggle(v, TypeToggleChange)
	end
	remainPointLbl = TransformFindChild(transform, "PointInfo/Remain/Value")
	usedPointLbl = TransformFindChild(transform, "PointInfo/Input/Value")
	resetPointBtn = TransformFindChild(transform, "PointInfo/GotButton")
	Util.AddClick(resetPointBtn.gameObject, TalentPointResetClick)
	talentTreeSV = TransformFindChild(transform, "TreeContainer")
	talentTreeSB = TransformFindChild(talentTreeSV, "Pb")
	curTypeSkillList = {}
	for i=1,19 do
		curTypeSkillList[i] = TransformFindChild(talentTreeSV, "Tree/Item"..i)
	end
	upgradeSkillWin = TransformFindChild(transform, "PopupWin/UpgradeSkillWin")
	resetPointWin = TransformFindChild(transform, "PopupWin/ResetPointWin")
	unlockPageWin = TransformFindChild(transform, "PopupWin/UnlockPageWin")
end
function OnShow()
	if isInit then
		CoachData.ReqCoachTalentInfo(InitTalentInfo)
		isInit = false
	else
		transform.gameObject:SetActive(true)
	end
end
function OnHide()
	transform.gameObject:SetActive(false)
end
function OnDestroy()
	pageSelectColor = nil
	pageUnSelectColor = nil

	lockColor = nil
	unlockColor = nil

	transform = nil
	windowComponent = nil

	talentPageTransList = nil --天赋页，3个toggle
	talentTypeTransList = nil --天赋分类，3个toggle
	remainPointLbl = nil
	usedPointLbl = nil
	resetPointBtn = nil
	talentTreeSV = nil
	talentTreeSB = nil
	curTypeSkillList = nil --天赋技能列表
	upgradeSkillWin = nil
	resetPointWin = nil
	unlockPageWin = nil

	totalPoint = nil
	pageStatus = nil
	currentPage = nil --当前技能页
	talentInfo = nil --天赋树信息{pageIdx = {type = {}, status = 0}}
	isInit = nil
end

function InitTalentInfo(data)
	--共有数据
	totalPoint = data.holdTalent
	pageStatus = data.pageStatus
	currentPage = data.page
	--每页单独持有数据
	local pageInfo = {}
	pageInfo.usedTotalPoint = data.usedTalent
	pageInfo.typeInfo = data.talentInfo
	talentInfo[currentPage] = pageInfo
	--标签页更新
	for i,v in ipairs(talentPageTransList) do
		SetPageButtonStatus(v, pageStatus[tostring(i)], i == currentPage)
	end
	--当前页刷新
	RefreshTalentPageInfo(false)
	transform.gameObject:SetActive(true)
end

function RefreshTalentPageInfo(isUpdate)
	local curTalentPage = talentInfo[currentPage]
	local usedTotalPoint = curTalentPage.usedTotalPoint
	UIHelper.SetLabelTxt(usedPointLbl, tostring(usedTotalPoint))
	UIHelper.SetLabelTxt(remainPointLbl, tostring(totalPoint - usedTotalPoint))
	CommonScript.SetButtonActive(resetPointBtn, usedTotalPoint > 0)
	for i,v in ipairs(talentTypeTransList) do
		local valueLbl = TransformFindChild(v, "Name/Value")
		UIHelper.SetLabelTxt(valueLbl, "( "..curTalentPage.typeInfo[tostring(i)].usedTalent.." )")
	end
	if not isUpdate then
		if UIHelper.GetToggleState(talentTypeTransList[1]) then
			TypeToggleChange(true, talentTypeTransList[1])
		else
			UIHelper.SetToggleState(talentTypeTransList[1], true)
		end
	end
end

--切换天赋页
function PageChangeClick(go)
	local page = UIHelper.GetUIEventListener(go).parameter.page
	if page == currentPage then
		return
	end
	--是否解锁判断
	if pageStatus[tostring(page)] == 0 then
		--弹出解锁天赋页窗口
		ShowUnlockPageWin(UIHelper.GetPanelDepth(talentTreeSV)+1, page)
	else
		CoachData.ReqCoachTalentInfo(InitTalentInfo, page)
	end
end
--设置天赋页按钮状态
function SetPageButtonStatus(trans, status, selected)
	local unLock = TransformFindChild(trans, "UnLock")
	local lock = TransformFindChild(trans, "Lock")
	local unLockLbl = TransformFindChild(unLock, "Label")
	unLock.gameObject:SetActive(status == 1)
	lock.gameObject:SetActive(status == 0)
	if status ~= 0 then
		if selected then
			UIHelper.SetWidgetColor(unLockLbl, pageSelectColor)
			UIHelper.SetWidgetColor(unLock, pageSelectColor)
		else
			UIHelper.SetWidgetColor(unLockLbl, pageUnSelectColor)
			UIHelper.SetWidgetColor(unLock, pageUnSelectColor)
		end
	end
end
--切换天赋点类别
function TypeToggleChange( selected, trans )
	local selectedTrans = TransformFindChild(trans,"Select")
	selectedTrans.gameObject:SetActive(selected)
	local nameKey = trans.name
	local ty = TalentSkillType[nameKey]
	if selected then
		SetSkillTreeByType(ty)
	end
end

function SetSkillTreeItem(itemTrans, ty, info)
	local icon = TransformFindChild(itemTrans, "Icon")
	local label = TransformFindChild(itemTrans, "Value")
	local canActive = TransformFindChild(itemTrans, "Active")
	local preCondArrow = TransformFindChild(itemTrans, "PreCondArrow") --前置条件满足的箭头
	local nextCondArrow = TransformFindChild(itemTrans, "NextCondArrow") --后置技能箭头
	local curId = info.id
	local curLv = info.lv --等级为0表示未激活状态
	--查前置条件-- 只判断lv1的前置条件
	local activeConfigID = curId.."1"
	local config = Config.GetKeyTotalInfo(Config.CoachTalent(), activeConfigID)
	local preCond = config.pre_skill
	local preActive = PreSkillConditionSatisfaction(preCond, ty) 
	SetConditionArrowActive(preCondArrow, preActive)
	--当前状态
	if curLv == 0 then
		 UIHelper.SetWidgetColor(icon, lockColor)
	else
		UIHelper.SetWidgetColor(icon, unlockColor)
		UIHelper.SetLabelTxt(label, curLv.."/"..config.max_lv)
	end
	label.gameObject:SetActive(curLv ~= 0)
	canActive.gameObject:SetActive(curLv == 0 and preActive)
	SetConditionArrowActive(nextCondArrow, curLv ~= 0) -- 后置箭头
end

function SetSkillTreeByType(ty)
	local skillInfo = talentInfo[currentPage].typeInfo[tostring(ty)].talentSkill --19个技能等级信息
	local idx = 0
	for k,v in CommonScript.PairsByOrderKey(skillInfo, "id", true) do
		idx = idx + 1
		local trans = curTypeSkillList[idx]
		SetSkillTreeItem(trans, ty, v)
		local li = UIHelper.GetUIEventListener(trans)
		if li == nil then
			li = Util.AddClick(trans.gameObject, SkillIconClick)
		end
		li.parameter = {}
		li.parameter.idx = idx
		li.parameter.id = v.id
		li.parameter.type = ty
	end

	UIHelper.SetProgressBar(talentTreeSB, 0)
	
end
--当前天赋树在升级后更新
function UpdateSkillTree(id, idx, ty)
	--只更新 可激活状态，技能等级，后置箭头，后置技能的前置箭头，后置技能激活状态
	local skillTrans = curTypeSkillList[idx]
	local typeSkillInfo =talentInfo[currentPage].typeInfo[tostring(ty)].talentSkill
	local curInfo = typeSkillInfo[tostring(id)]
	SetSkillTreeItem(skillTrans, ty, curInfo)
	local index = 0
	for k,v in CommonScript.PairsByOrderKey(typeSkillInfo, "id", true) do --其他技能的前置条件判断
		index = index + 1
		if v.id ~= id then
			local nextConfigId = v.id..tostring(v.lv+1)
			local nextConfig = Config.GetKeyTotalInfo(Config.CoachTalent(), nextConfigId)
			if nextConfig ~= nil then
				local preSkillStr = nextConfig.pre_skill:split(';')
				for i,v1 in ipairs(preSkillStr) do
					local pair = v1:split(',')
					if pair[1] == id then
						SetSkillTreeItem(curTypeSkillList[index], ty, v)
						break
					end
				end
			end
		end
	end
end

function PreSkillConditionSatisfaction(preCond, ty)
	if preCond ~= nil then
		local skillInfo = talentInfo[currentPage].typeInfo[tostring(ty)].talentSkill
		preCondPairs = preCond:split(';')
		for k1,v1 in pairs(preCondPairs) do
			local pair = v1:split(',')
			if skillInfo[pair[1]].lv < tonumber(pair[2]) then
				return false
			end
		end
	end
	return true
end

function SetConditionArrowActive( arrowParent, active )
	if arrowParent ~= nil then
		local pbValue = 0
		if active then
			pbValue = 1
		end
		for i=1,10 do
			local child = TransformFindChild(arrowParent, "ProgressBar"..i)
			if child == nil then
				break
			end
			UIHelper.SetProgressBar(child, pbValue)
		end
	end
end
--点击小图标，弹出技能详细描述窗口
function SkillIconClick( go )
	local li = UIHelper.GetUIEventListener(go)
	local id = li.parameter.id
	local idx = li.parameter.idx
	local ty = li.parameter.type
	--激活点击
	ShowSkillDetailWin(UIHelper.GetPanelDepth(talentTreeSV)+1, id, idx, ty)
end
function ShowSkillDetailWin(depth, id, idx, ty)
	UIHelper.SetPanelDepth(upgradeSkillWin, depth)
	local closeBtn = TransformFindChild(upgradeSkillWin, "Close")
	local icon = TransformFindChild(upgradeSkillWin, "Info/Icon")
	local nameLbl = TransformFindChild(upgradeSkillWin, "Info/Name")
	local lvLbl = TransformFindChild(upgradeSkillWin, "Info/Level")
	local descLbl = TransformFindChild(upgradeSkillWin, "Info/Describle")
	local pointCostLbl = TransformFindChild(upgradeSkillWin, "Info/Cost/Point/Value")
	local goldCostLbl = TransformFindChild(upgradeSkillWin, "Info/Cost/Gold/Value")
	local confirmBtn = TransformFindChild(upgradeSkillWin, "Upgrade/ConfirmBtn")
	local upgradeTips = TransformFindChild(upgradeSkillWin, "Upgrade/Tips")
	local costStativLbl = TransformFindChild(upgradeSkillWin, "Info/Cost")
	local confirmBtnEnableLbl = TransformFindChild(confirmBtn, "EnableLbl")
	local confirmBtnDisableLbl = TransformFindChild(confirmBtn, "DisableLbl")
	local costTrans = TransformFindChild(upgradeSkillWin, "Info/Cost")
	local bg = TransformFindChild(upgradeSkillWin, "BGBlock")
	local closeFun = function( go )
		upgradeSkillWin.gameObject:SetActive(false)
	end
	Util.ChangeClick(bg.gameObject, closeFun)
	--更新窗口ui
	local refreshUI = function()
		--data
		local skillInfoList = talentInfo[currentPage].typeInfo[tostring(ty)].talentSkill
		local curSkillInfo = skillInfoList[id]
		local lv = curSkillInfo.lv
		local nextLv = lv + 1
		local configID = id..lv
		local nextConfigID = id..nextLv
		if lv == 0 then
			configID = id.."1"
		end
		local curSkillConfig = Config.GetKeyTotalInfo(Config.CoachTalent(), configID)
		local nextSkillConfig = Config.GetKeyTotalInfo(Config.CoachTalent(), nextConfigID)
		local remainPoint = totalPoint - talentInfo[currentPage].usedTotalPoint
		local isActive = lv == 0
		local isMaxLv = lv == curSkillConfig.max_lv
		--ui
		UIHelper.SetLabelTxt(nameLbl, curSkillConfig.name)
		UIHelper.SetLabelTxt(lvLbl, "Lv: "..lv.."/"..curSkillConfig.max_lv)
		UIHelper.SetLabelTxt(descLbl, curSkillConfig.desc)
		costTrans.gameObject:SetActive(not isMaxLv)
		if not isMaxLv then
			UIHelper.SetLabelTxt(pointCostLbl, nextSkillConfig.point_use)
			UIHelper.SetLabelTxt(goldCostLbl, nextSkillConfig.money_use)
		end
		if isActive then
			UIHelper.SetLabelTxt(costStativLbl, Util.LocalizeString("UICTActiveCost"))
			UIHelper.SetLabelTxt(confirmBtnEnableLbl, Util.LocalizeString("Active"))
			UIHelper.SetLabelTxt(confirmBtnDisableLbl, Util.LocalizeString("Active"))
		else
			UIHelper.SetLabelTxt(costStativLbl, Util.LocalizeString("UICTUpgradeCost"))
			UIHelper.SetLabelTxt(confirmBtnEnableLbl, Util.LocalizeString("Upgrade"))
			UIHelper.SetLabelTxt(confirmBtnDisableLbl, Util.LocalizeString("Upgrade"))
		end
		--查消耗是否足够
		local canUpgrade = true
		local canNotUpgradeTips = ""
		if isMaxLv then
			canUpgrade = false
			canNotUpgradeTips = Util.LocalizeString("UICTUpgradeLimit")
		elseif not PreSkillConditionSatisfaction(nextSkillConfig.pre_skill,ty) then
			canUpgrade = false
			local cond = nextSkillConfig.pre_skill:split(';')
			canNotUpgradeTips = Util.LocalizeString("UICTUpgradeLvlPre")
			for i,v1 in ipairs(cond) do
				local pair = v1:split(',')
				if skillInfoList[pair[1]].lv < tonumber(pair[2]) then
					if i ~= 1 then
						canNotUpgradeTips = canNotUpgradeTips.."，"
					end
					local nameStr = Config.GetProperty(Config.CoachTalent(), pair[1].."1", "name")
					canNotUpgradeTips = canNotUpgradeTips..string.format(Util.LocalizeString("UICTUpgradeLvlPreDetail"), nameStr, pair[2])
				end
			end
		elseif remainPoint < nextSkillConfig.point_use then
			canUpgrade = false
			canNotUpgradeTips = Util.LocalizeString("UICTUpgradeNeedPoint")
		elseif tonumber(ItemSys.GetItemData(LuaConst.Const.SB).num) < nextSkillConfig.money_use then
			canUpgrade = false
			canNotUpgradeTips = Util.LocalizeString("UICTUpgradeNeedGold")
		elseif talentInfo[currentPage].typeInfo[tostring(ty)].usedTalent < nextSkillConfig.point_need then
			canUpgrade = false
			canNotUpgradeTips = string.format(Util.LocalizeString("UICTUpgradePointPre"),nextSkillConfig.point_need)
		elseif tonumber(Role.Get_lv()) < nextSkillConfig.lv_need then
			canUpgrade = false
			canNotUpgradeTips = GetLocalizedString("UICTUpgradeNeedLevel",nextSkillConfig.lv_need)
		end
		upgradeTips.gameObject:SetActive(not canUpgrade)
		UIHelper.SetLabelTxt(upgradeTips, canNotUpgradeTips)
		CommonScript.SetButtonActive(confirmBtn, canUpgrade)
	end
	local upgradeCbFun = function(data)
		local page = data.page
		local skillId = data.skill_id
		local skillLv = data.skill_lv
		local skillType = tostring(data.skill_type)
		local pointUse = data.point_use
		--data
		local curInfo = talentInfo[page]
		curInfo.usedTotalPoint = curInfo.usedTotalPoint + pointUse
		curInfo.typeInfo[skillType].usedTalent = curInfo.typeInfo[skillType].usedTalent + pointUse
		curInfo.typeInfo[skillType].talentSkill[skillId].lv = skillLv
		--父窗口ui
		RefreshTalentPageInfo(true)
		UpdateSkillTree(id, idx, ty)
		--更新本窗口ui
		refreshUI()
	end
	local upgradeFun = function( go )
	 	local skillId = UIHelper.GetUIEventListener(go).parameter.id
		CoachData.ReqUpgradeTalentSkill(upgradeCbFun, currentPage, skillId)	
	end
	local confirmBtnLi = UIHelper.GetUIEventListener(confirmBtn)
	confirmBtnLi = Util.ChangeClick(confirmBtn.gameObject,  upgradeFun)
	confirmBtnLi.parameter = {["id"] = id}
	Util.ChangeClick(closeBtn.gameObject,  closeFun)
	refreshUI()
	upgradeSkillWin.gameObject:SetActive(true)
end

--重置天赋点
function TalentPointResetClick(go)
	ShowResetTalentPointWin(UIHelper.GetPanelDepth(talentTreeSV)+1)
end
function ShowResetTalentPointWin(depth)
	UIHelper.SetPanelDepth(resetPointWin, depth)
	local closeFun = function(go)
		resetPointWin.gameObject:SetActive(false)
	end
	local resetFun = function(go)
		CoachData.ReqResetTalentPoint(ResetTalentPointCB, currentPage)
		closeFun(go)
	end
	local closeBtn = TransformFindChild(resetPointWin, "Close")
	local confirmBtn = TransformFindChild(resetPointWin, "Info/ConfirmBtn")
	local cancelBtn = TransformFindChild(resetPointWin, "Info/CancelBtn")
	local costTrans = TransformFindChild(resetPointWin, "Info/Cost")
	local freeTrans = TransformFindChild(resetPointWin, "Info/Free")
	local costLbl = TransformFindChild(costTrans, "Icon/Value")
	local freeLbl = TransformFindChild(freeTrans, "Label")
	local bg = TransformFindChild(resetPointWin, "BGBlock")
	Util.ChangeClick(bg.gameObject, closeFun)
	Util.ChangeClick(closeBtn.gameObject, closeFun)
	Util.ChangeClick(cancelBtn.gameObject, closeFun)
	Util.ChangeClick(confirmBtn.gameObject, resetFun)
	local lv = tonumber(Role.Get_lv())
	local configTable = Config.GetTemplate(Config.CoachLv())
	local freeLimit = 0
	for k,v in CommonScript.PairsByOrderKey(configTable, "id", true) do
		if v.refresh_gold == 0 then
			freeLimit = tonumber(k)
		else
			break
		end
	end
	local isFree = lv <= freeLimit
	freeTrans.gameObject:SetActive(isFree)
	costTrans.gameObject:SetActive(not isFree)
	if isFree then
		UIHelper.SetLabelTxt(freeLbl, string.format(Util.LocalizeString("UICTResetPointFree"), freeLimit))
	else
		UIHelper.SetLabelTxt(costLbl, Config.GetProperty(Config.CoachLv(), tostring(lv), "refresh_gold"))
	end

	resetPointWin.gameObject:SetActive(true)
end
--重置回调，更新本地数据和UI
function ResetTalentPointCB(data)
	currentPage = data.page
	local pageInfo = talentInfo[currentPage]
	pageInfo.usedTotalPoint = 0
	for k,v in pairs(pageInfo.typeInfo) do
		v.usedTalent = 0
		for k,v in pairs(v.talentSkill) do
			v.lv = 0
		end
	end
	talentInfo[currentPage] = pageInfo
	--UI
	RefreshTalentPageInfo(false)
end
--解锁天赋页界面
function ShowUnlockPageWin( depth, page )
	UIHelper.SetPanelDepth(unlockPageWin, depth)
	local closeFun = function(go)
		unlockPageWin.gameObject:SetActive(false)
	end
	local closeBtn = TransformFindChild(unlockPageWin, "Close")
	Util.ChangeClick(closeBtn.gameObject, closeFun)
	local diamondTrans = TransformFindChild(unlockPageWin, "Info/LockDiamond")
	local lvlTrans = TransformFindChild(unlockPageWin, "Info/LockLevel")
	local bg = TransformFindChild(unlockPageWin, "BGBlock")
	Util.ChangeClick(bg.gameObject, closeFun)
	local lv = tonumber(Role.Get_lv())
	local vip = tonumber(Role.Get_vip())

	local config = Config.GetKeyTotalInfo(Config.CoachTalentPage(), tostring(page))
	local lockType = 3 --1-按等级，2-按vip，3-钻石
	if lv < tonumber(config.lv_require) then 
		lockType = 1
	elseif vip < tonumber(config.vip_require)  then
		lockType = 2
	else
		lockType = 3
	end
	diamondTrans.gameObject:SetActive(lockType == 3)
	lvlTrans.gameObject:SetActive(lockType ~= 3)
	if lockType == 3 then
		local costLbl = TransformFindChild(diamondTrans, "Icon/Value")
		local unlockBtn = TransformFindChild(diamondTrans, "ConfirmBtn")
		local cancelBtn = TransformFindChild(diamondTrans, "CancelBtn")
		local cost = config.gold_use
		UIHelper.SetLabelTxt(costLbl, cost)
		local unlockFun = function(go)
			local containDiamond = ItemSys.GetItemData(LuaConst.Const.GB).num
			if containDiamond < cost then
				WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, Util.LocalizeString("DiamondNotEnough"))
			else
				CoachData.ReqUnlockTalentPage(UnlockPageCb, page)
				closeFun()
			end
		end
		Util.ChangeClick(unlockBtn.gameObject, unlockFun)
		Util.ChangeClick(cancelBtn.gameObject, closeFun)
	else
		local confirmBtn = TransformFindChild(lvlTrans, "ConfirmBtn")
		local label = TransformFindChild(lvlTrans, "Label")
		Util.ChangeClick(confirmBtn.gameObject, closeFun)
		if lockType == 1 then
			UIHelper.SetLabelTxt(label, string.format(Util.LocalizeString("UICTUnlockPageLv"), config.lv_require))
		elseif lockType == 2 then
			UIHelper.SetLabelTxt(label, string.format(Util.LocalizeString("UICTUnlockPageVip"), config.vip_require))
		end
	end
	unlockPageWin.gameObject:SetActive(true)
end
--解锁天赋页回调
function UnlockPageCb(data)
	local page = data.page
	SetPageButtonStatus(talentPageTransList[page], 1, false)
	pageStatus[tostring(page)] = 1
end
