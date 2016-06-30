module("UICoachSkillSlotItem", package.seeall)

local slotItemPrefab = nil

function Init(winComponent)
	if slotItemPrefab == nil then
		slotItemPrefab = winComponent:GetPrefab("SkillSlotItem")
	end
end

--configInfo 包含技能栏,slotInfo.lockType:0-已解锁，1-等级限制，2-vip限制，3-钻石限制
function CreateSkillSlotItem(slotInfo, idx, grid )
	local trans = InstantiatePrefab(slotItemPrefab,grid,idx).transform
	local skillIconTrans = TransformFindChild(trans, "Filled")
	local icon = TransformFindChild(trans, "Filled/Icon")--icon的复制待美术做好
	local unlockTrans = TransformFindChild(trans, "Unlock")
	local lockTrans = TransformFindChild(trans, "Lock")
	local lockType = slotInfo.lockType
	local isLock = lockType ~= 0
	lockTrans.gameObject:SetActive(isLock)
	if not isLock then --已解锁
		local hasSkill = slotInfo.skill_id ~= 0
		skillIconTrans.gameObject:SetActive(hasSkill)
		unlockTrans.gameObject:SetActive(not hasSkill)
		if hasSkill then --有技能
			--给icon复图片
		end
	else
		skillIconTrans.gameObject:SetActive(false)
		unlockTrans.gameObject:SetActive(false)
		--判断解锁条件
		local diamondTrans = TransformFindChild(lockTrans, "DiamondLock")
		local diamondValueLbl = TransformFindChild(diamondTrans, "Value")
		local lvlTrans = TransformFindChild(lockTrans, "LevelLockInfo")
		diamondTrans.gameObject:SetActive(lockType == 3)
		lvlTrans.gameObject:SetActive(lockType ~= 3)
		local config = Config.GetKeyTotalInfo(Config.CoachSkillSlot(), slotInfo.id)
		if lockType == 1 then
			UIHelper.SetLabelTxt(lvlTrans, string.format(Util.LocalizeString("UICSSlotLvLimit"),config.lv_require))
		elseif lockType == 2 then
			UIHelper.SetLabelTxt(lvlTrans, string.format(Util.LocalizeString("UICSSlotVipLimit"),config.vip_require))
		elseif lockType == 3 then
			local items = config.item:split(';')
			local diamond = 0
			for i,v in ipairs(items) do
				local pair = v:split(',')
				if pair[1] == LuaConst.Const.GB then
					diamond = tonumber(pair[2])
					break
				end
			end
			UIHelper.SetLabelTxt(diamondValueLbl, diamond)
		end
	end
	return trans
end

function Unlock( cloneTrans )
	local unlockTrans = TransformFindChild(cloneTrans, "Unlock")
	local lockTrans = TransformFindChild(cloneTrans, "Lock")
	unlockTrans.gameObject:SetActive(true)
	lockTrans.gameObject:SetActive(false)
end

function Equip( cloneTrans, skillID )
	local skillIconTrans = TransformFindChild(cloneTrans, "Filled")
	local icon = TransformFindChild(cloneTrans, "Filled/Icon")--icon的赋值待美术做好
	local unlockTrans = TransformFindChild(cloneTrans, "Unlock")
	skillIconTrans.gameObject:SetActive(true)
	unlockTrans.gameObject:SetActive(false)
end

function TakeOff( cloneTrans, skillID )
	local skillIconTrans = TransformFindChild(cloneTrans, "Filled")
	local unlockTrans = TransformFindChild(cloneTrans, "Unlock")
	unlockTrans.gameObject:SetActive(true)
	skillIconTrans.gameObject:SetActive(false)
end

function OnDestroy()
	slotItemPrefab = nil
end