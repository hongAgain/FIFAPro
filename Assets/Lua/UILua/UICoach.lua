module("UICoach",package.seeall)

--教练配置表
--加技能列表数据，技能数据读表，技能激活装备情况服务器发送
--成就列表数据，配置读表
--CommonScript.CreatePerson创建教练3d角色

require "Config"
require "Game/CoachData"
require "UILua/UICoachTalent"
require "UILua/UICoachFame"
require "UILua/UICoachSkill"
require "UILua/UICoachFormation"

local gameObject = nil
local windowComponent = nil
local transform = nil 
--ui
local lvlLbl = nil
local nameLbl =nil
--local fameLbl = nil
local fameValueLbl = nil
local skillTab = nil
local talentTab = nil
local fameTab = nil
local formationTab = nil
local selectSprite = nil
local selectPos = nil
local skillTrans = nil
local talentTrans = nil
local fameTrans = nil
local formationTrans = nil
--scene
local scene = nil
local coachModel = nil


local coachSubWinUIModule = 
{
	["SkillTab"] = UICoachSkill,
	["TalentTab"] = UICoachTalent,
	["FameTab"] = UICoachFame,
	["FormationTab"] = UICoachFormation
}
function OnStart(go)
	gameObject = go
	transform = go.transform
	windowComponent = GetComponentInChildren(go, "UIBaseWindowLua")
	BindUI()
	local needChange = 
	{
		["Table01"] = {pos = Vector3.New(4.68,0,-2.93)},
		["Book/Book01_Group_1/Book01B_1"] = {pos = Vector3.New(4.308,-1.06,1.107)},
		["Book/Book01_Group_1/Book01B_2"] = {pos = Vector3.New(4.228,-1,1.117)},
		["phone 1_lod1"] = {pos = Vector3.New(4.317,1.17,-3.089)},
		["Chair04B"] = {pos = Vector3.New(3.06,0.01, -1.76), rot = Vector3.New(270,101.039,0)}
	}
	scene = Util.ChangeLevelState("SelectCoach", Vector3.New(1.93, 0, 5.249))--创建教练场景
	local camera = windowComponent:GetPrefab("CameraSaver")
	AddChild(camera, scene.transform)
	for k,v in pairs(needChange) do
		local t = TransformFindChild(scene.transform, k)
		if t ~= nil then
			t.localPosition = v.pos
			if v.rot ~= nil then
				t.localEulerAngles = v.rot
			end
		end
	end
	CoachData.ReqCoachInfo(InitCoachBasic)
	InitSubWinUI()
	UIHelper.SetToggleState(talentTab, true)
end

function InitCoachBasic( data )
	UIHelper.SetLabelTxt(lvlLbl, "Lv."..data.lv)
	UIHelper.SetLabelTxt(nameLbl, data.name)
	local fameValue = data.item["14"] or 0
	UIHelper.SetLabelTxt(fameValueLbl, fameValue)

	--创建教练模型
	local coachInfo =
    {
        ["1004"] = {idx = 1, pos = Vector3.New(-0.2557784, 0, 2.508741), rot = Vector3.New(0, 102.6247, 0), default = "idle_01", special = "special_01" },
        ["1001"] = {idx = 2, pos = Vector3.New(0.325, 0, 0.366), rot = Vector3.New(0, 24.37092, 0), default = "idle_02", special = "special_02" },
        ["1010"] = {idx = 3, pos = Vector3.New(-1.347, 0, 1.233), rot = Vector3.New(0, 102.8578, 0), default = "idle_03", special = "special_03" }
    }
	local coachID = data.cid
	local createCb = function(go)
		coachModel = go
		go.transform.position = Vector3.New(0.69, 0, 0.57)
        go.transform.eulerAngles = Vector3.New(0, 51.217, 0)
        local coachHelper = go:AddComponent("CoachHelper")
        coachHelper:InitUniform(coachInfo[coachID].idx)
	end
	Util.CreateCoach(coachID, createCb, coachInfo[coachID].default, "CoachSelectAnim")
end

function InitSubWinUI()
	UICoachSkill.Init(skillTrans, windowComponent)
	UICoachFame.Init(fameTrans,windowComponent)
	UICoachTalent.Init(talentTrans, windowComponent)
end

function BindUI()
	local basicInfoTrans = TransformFindChild(transform, "AnchorLeft/CoachInfo")
	lvlLbl = TransformFindChild(basicInfoTrans, "Level/Lv")
	--fameLbl = TransformFindChild(basicInfoTrans, "Level/FameName")
	nameLbl = TransformFindChild(basicInfoTrans, "Name/Name")
	fameValueLbl = TransformFindChild(basicInfoTrans, "Fame/Value")
	local abilityInfoTrans = TransformFindChild(transform, "AnchorRight/AbilityInfo")
	skillTab = TransformFindChild(abilityInfoTrans, "Tab/SkillTab")
	talentTab = TransformFindChild(abilityInfoTrans, "Tab/TalentTab")
	fameTab = TransformFindChild(abilityInfoTrans, "Tab/FameTab")
	formationTab = TransformFindChild(abilityInfoTrans, "Tab/FormationTab")
	selectSprite = TransformFindChild(abilityInfoTrans, "Tab/SelectSprite")
	selectPos = selectSprite.localPosition
	skillTrans = TransformFindChild(abilityInfoTrans, "SubWin/Skill")
	fameTrans = TransformFindChild(abilityInfoTrans, "SubWin/Fame")
	talentTrans = TransformFindChild(abilityInfoTrans, "SubWin/Talent")

	UIHelper.AddToggle(skillTab, OnToggleChange)
	UIHelper.AddToggle(talentTab, OnToggleChange)
	UIHelper.AddToggle(fameTab, OnToggleChange)
	UIHelper.AddToggle(formationTab, OnToggleChange)
end

function OnToggleChange( selected, trans )
	--ui修改
	local key = trans.name
	if selected then
		selectSprite.localPosition = NewVector3(trans.localPosition.x, selectPos.y, selectPos.z)
		UIHelper.SetWidgetColor(trans, "win_wb_24")
		coachSubWinUIModule[key].OnShow()
	else
		UIHelper.SetWidgetColor(trans, "win_w_24")
		coachSubWinUIModule[key].OnHide()
	end
end


function SaveOnExit()
	--保存教练相关设置，是否调用有待后续考虑
end

function OnShow()
	if scene ~= nil then
		scene.gameObject:SetActive(true)
	end
	if coachModel ~= nil then
		coachModel.gameObject:SetActive(true)
	end
end

function OnHide()
	if scene ~= nil then
		scene.gameObject:SetActive(false)
	end
	if coachModel ~= nil then
		coachModel.gameObject:SetActive(false)
	end
end

function OnDestroy()
	SaveOnExit()
	UICoachTalent.OnDestroy()
	UICoachSkill.OnDestroy()
	UICoachFame.OnDestroy()
	lvlLbl = nil
	nameLbl =nil
	--fameLbl = nil
	fameValueLbl = nil
	skillTab = nil
	talentTab = nil
	fameTab = nil
	formationTab = nil
	selectSprite = nil
	skillTrans = nil
	talentTrans = nil
	fameTrans = nil
	formationTrans = nil
	if scene ~= nil then
		GameObjectDestroy(scene.gameObject)
		scene = nil
	end
	if coachModel ~= nil then
		GameObjectDestroy(coachModel)
		coachModel = nil
	end
	-- if windowComponent ~= nil then
	-- 	windowComponent:Close()
	-- end
end
