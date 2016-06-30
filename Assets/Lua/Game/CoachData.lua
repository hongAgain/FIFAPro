module("CoachData",package.seeall);

--需要保存一些教练基础数据在内存中
local getCoachInfoCB = nil
local getCoachFameInfoCB = nil
local getCoachFameRewardCB = nil
local getCoachSkillInfoCB = nil
local unlockSkillSlotCB = nil
local upgradeSkillCB = nil
local loadSkillCB = nil
local getCoachTalentInfoCB = nil
local upgradeTalentSkillCB = nil
local resetTalentPointCB = nil
local unlockTalentPageCB =nil
local enableTanlentPageCB = nil

function OnInit()
	DataSystemScript.RegisterMsgHandler(MsgID.tb.CoachInfo, OnReqCoachInfo)
	DataSystemScript.RegisterMsgHandler(MsgID.tb.CoachFameInfo, OnReqCoachFameInfo)
	DataSystemScript.RegisterMsgHandler(MsgID.tb.CoachFameReward, OnReqCoachFameReward)
	DataSystemScript.RegisterMsgHandler(MsgID.tb.CoachSkillInfo, OnReqCoachSkillInfo)
	DataSystemScript.RegisterMsgHandler(MsgID.tb.CSUnlockSlot, OnReqUnlockSkillSlot)
	DataSystemScript.RegisterMsgHandler(MsgID.tb.CSUpgrade, OnReqUpgradeSkill)
	DataSystemScript.RegisterMsgHandler(MsgID.tb.CSLoadSkill, OnReqLoadSkill)
	DataSystemScript.RegisterMsgHandler(MsgID.tb.CoachTalentInfo, OnReqCoachTalentInfo)
	DataSystemScript.RegisterMsgHandler(MsgID.tb.CTUpgradeSkill, OnReqUpgradeTalentSkill)
	DataSystemScript.RegisterMsgHandler(MsgID.tb.CTResetPoint, OnReqResetTalentPoint)
	DataSystemScript.RegisterMsgHandler(MsgID.tb.CTUnlockPage, OnReqUnlockTalentPage)
	DataSystemScript.RegisterMsgHandler(MsgID.tb.CTEnablePage, OnReqEnableTalentPage)
end
function OnRelease()
	DataSystemScript.UnRegisterMsgHandler(MsgID.tb.CoachInfo, OnReqCoachInfo)
	DataSystemScript.UnRegisterMsgHandler(MsgID.tb.CoachFameInfo, OnReqCoachFameInfo)
	DataSystemScript.UnRegisterMsgHandler(MsgID.tb.CoachFameReward, OnReqCoachFameReward)
	DataSystemScript.UnRegisterMsgHandler(MsgID.tb.CoachSkillInfo, OnReqCoachSkillInfo)
	DataSystemScript.UnRegisterMsgHandler(MsgID.tb.CSUnlockSlot, OnReqUnlockSkillSlot)
	DataSystemScript.UnRegisterMsgHandler(MsgID.tb.CSUpgrade, OnReqUpgradeSkill)
	DataSystemScript.UnRegisterMsgHandler(MsgID.tb.CSLoadSkill, OnReqLoadSkill)
	DataSystemScript.UnRegisterMsgHandler(MsgID.tb.CoachTalentInfo, OnReqCoachTalentInfo)
	DataSystemScript.UnRegisterMsgHandler(MsgID.tb.CTUpgradeSkill, OnReqUpgradeTalentSkill)
	DataSystemScript.UnRegisterMsgHandler(MsgID.tb.CTResetPoint, OnReqResetTalentPoint)
	DataSystemScript.UnRegisterMsgHandler(MsgID.tb.CTUnlockPage, OnReqUnlockTalentPage)
	DataSystemScript.UnRegisterMsgHandler(MsgID.tb.CTEnablePage, OnReqEnableTalentPage)
end

function ReqCoachInfo(cb)
	getCoachInfoCB = cb
	DataSystemScript.RequestWithParams(LuaConst.Const.CoachInfo, nil, MsgID.tb.CoachInfo)
end
function OnReqCoachInfo( code, data )
	if code == nil then
		getCoachInfoCB(data)
		getCoachInfoCB = nil
	end
end
------------------------------------------------------------------
--教练名声
function ReqCoachFameInfo(cb)
	getCoachFameInfoCB = cb
	DataSystemScript.RequestWithParams(LuaConst.Const.CoachFameInfo, nil, MsgID.tb.CoachFameInfo)
end
function OnReqCoachFameInfo( code, data )
	if code == nil then
		getCoachFameInfoCB(data)
		getCoachFameInfoCB = nil
	end
end

function ReqCoachFameReward( cb, rewardID ) --param= {rewardSeq}
	getCoachFameRewardCB = cb
	local param = {}
	param.rewardSeq = rewardID
	DataSystemScript.RequestWithParams(LuaConst.Const.CoachFameReward, param, MsgID.tb.CoachFameReward)
end
function OnReqCoachFameReward( code, data )
	if code == nil then
		getCoachFameRewardCB(data)
		getCoachFameRewardCB = nil
	end
end
--------------------------------------------------------
--教练技能
function ReqCoachSkillInfo(cb)
	getCoachSkillInfoCB = cb
	DataSystemScript.RequestWithParams(LuaConst.Const.CoachSkillInfo, nil, MsgID.tb.CoachSkillInfo)
end
function OnReqCoachSkillInfo( code, data )
	if code == nil then
		getCoachSkillInfoCB(data)
		getCoachSkillInfoCB = nil
	end
end

function ReqUnlockSkillSlot(cb, slotID)
	unlockSkillSlotCB = cb
	local param = {}
	param.slot_id = slotID
	DataSystemScript.RequestWithParams(LuaConst.Const.CSUnlockSlot, param, MsgID.tb.CSUnlockSlot)
end
function OnReqUnlockSkillSlot( code, data )
	if code == nil then
		unlockSkillSlotCB(data)
		unlockSkillSlotCB = nil
	end
end

function ReqUpgradeSkill(cb, skillID)
	upgradeSkillCB = cb
	local param = {}
	param.skill_id = skillID
	DataSystemScript.RequestWithParams(LuaConst.Const.CSUpgrade, param, MsgID.tb.CSUpgrade)
end
function OnReqUpgradeSkill( code, data )
	if code == nil then
		upgradeSkillCB(data)
		upgradeSkillCB = nil
	end
end

function ReqLoadSkill(cb, skillID, type)
	loadSkillCB = cb
	local param = {}
	param.skill_id = skillID
	param.type = type
	DataSystemScript.RequestWithParams(LuaConst.Const.CSLoadSkill, param, MsgID.tb.CSLoadSkill)
end
function OnReqLoadSkill( code, data )
	if code == nil then
		loadSkillCB(data)
		loadSkillCB = nil
	end
end
----------------------------------------------------------------
--教练天赋
function ReqCoachTalentInfo(cb, page)
	getCoachTalentInfoCB = cb
	local param = nil
	if page ~= nil then
		param = {}
		param.page = page
	end
	DataSystemScript.RequestWithParams(LuaConst.Const.CoachTalentInfo, param, MsgID.tb.CoachTalentInfo)
end
function OnReqCoachTalentInfo( code, data )
	if code == nil then
		getCoachTalentInfoCB(data)
		getCoachTalentInfoCB = nil
	end
end
function ReqUpgradeTalentSkill(cb, page, skillId)
	upgradeTalentSkillCB = cb
	local param = {}
	param.page = page
	param.skill_id = skillId
	DataSystemScript.RequestWithParams(LuaConst.Const.CTUpgradeSkill, param, MsgID.tb.CTUpgradeSkill)
end
function OnReqUpgradeTalentSkill( code, data )
	if code == nil then
		upgradeTalentSkillCB(data)
		upgradeTalentSkillCB = nil
	end
end
function ReqResetTalentPoint(cb, page)
	resetTalentPointCB = cb
	local param = {}
	param.page = page
	DataSystemScript.RequestWithParams(LuaConst.Const.CTResetPoint, param, MsgID.tb.CTResetPoint)
end
function OnReqResetTalentPoint( code, data )
	if code == nil then
		resetTalentPointCB(data)
		resetTalentPointCB = nil
	end
end
function ReqUnlockTalentPage(cb, page)
	unlockTalentPageCB = cb
	local param = {}
	param.page = page
	DataSystemScript.RequestWithParams(LuaConst.Const.CTUnlockPage, param, MsgID.tb.CTUnlockPage)
end
function OnReqUnlockTalentPage( code, data )
	if code == nil then
		unlockTalentPageCB(data)
		unlockTalentPageCB = nil
	end
end
function ReqEnableTalentPage(cb, page)
	enableTanlentPageCB = cb
	local param = {}
	param.page = page
	DataSystemScript.RequestWithParams(LuaConst.Const.CTEnablePage, param, MsgID.tb.CTEnablePage)
end
function OnReqEnableTalentPage( code, data )
	if code == nil then
		enableTanlentPageCB(data)
		enableTanlentPageCB = nil
	end
end

