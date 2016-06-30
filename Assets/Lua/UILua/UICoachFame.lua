module("UICoachFame",package.seeall)

require "Config"
require "Game/CoachData"
require "UILua/UIIconItem"

local transform = nil
local infoItemPref = nil
local fameItemSV = nil
local fameItemGrid = nil

local curFamePoint = 0
local fameItemTable = {}
local itemTitleStr = ""
local isInit = true

function Init(trans, winComponent)
	transform = trans
	UIIconItem.Init()
	if infoItemPref == nil then
		infoItemPref = winComponent:GetPrefab("FameItem")
	end
	curFamePoint = 0
	fameItemTable = {}
	itemTitleStr = ""
	isInit = true
	BindUI()
end
function BindUI()
	fameItemSV = TransformFindChild(transform, "FameScrollView")
	fameItemGrid = TransformFindChild(transform, "FameScrollView/Grid")
	itemTitleStr = Util.LocalizeString("UICFItemTitle")
end
function OnShow()
	if isInit then
		CoachData.ReqCoachFameInfo(InitFameItems)
		isInit = false
	else
		transform.gameObject:SetActive(true)
	end
end
function OnHide()
	transform.gameObject:SetActive(false)
end
function OnDestroy()
	transform = nil
	infoItemPref = nil
	fameItemSV = nil
	fameItemGrid = nil
	fameItemTable = nil
	curFamePoint = nil
	itemTitleStr = nil
	isInit = nil
end

function InitFameItems(data)
	if data == nil then
		return
	end
	curFamePoint = data.fame or 0
	local rewardInfo = data.rewardInfo
	if rewardInfo == nil then
		return
	end
	local orderFun = function(t)
		local a = {}
		for k,v in pairs(t) do
			a[#a + 1] = tonumber(k)
		end
		table.sort(a)
		local i = 0
		return function()
			i = i + 1
			for k,v in pairs(t) do
				if k == tostring(a[i]) then
					return k, v
				end
			end
			return nil, nil
		end
	end
	for k,v in orderFun(rewardInfo) do
		local item = CreateFameItem(k, rewardInfo[k].status)
		fameItemTable[k] = item
	end
	UIHelper.RepositionGrid(fameItemGrid,fameItemSV)
	transform.gameObject:SetActive(true)
end

--keyFamePoint为id
function CreateFameItem( keyFamePoint, status )
 	local configInfo = Config.GetKeyTotalInfo(Config.CoachFameReward(), keyFamePoint)
 	local cloneTrans = InstantiatePrefab(infoItemPref,fameItemGrid,keyFamePoint).transform
 	--图标标记
 	local icon = TransformFindChild(cloneTrans, "Icon")
 	--UIHelper.SetSpriteNameNoPerfect(icon, configInfo.icon) --等配置表
 	local title = TransformFindChild(cloneTrans, "Name")
 	local rewardGrid = TransformFindChild(cloneTrans, "RewardGrid")
 	UIHelper.SetLabelTxt(title, string.format(itemTitleStr, keyFamePoint))
 	--rewardList
 	local rewardList = configInfo.reward:split(';')
 	for i,v in ipairs(rewardList) do
 		local rewardPairs = v:split(',')
 		if rewardPairs ~= nil then
 			UIIconItem.CreateRewardIconItem(rewardGrid, nil, rewardPairs, {scale = 0.4})
 		end
 	end
 	UIHelper.RepositionGrid(rewardGrid)
 	--status
 	SetFameItemStatus(cloneTrans, keyFamePoint, status)
 	return cloneTrans
end
function SetFameItemStatus( item, keyID, status )
	local hasGotTrans = TransformFindChild(item, "Status/HasGot")
 	local getBtn = TransformFindChild(item, "Status/GotButton")
	local pointLabel = TransformFindChild(getBtn, "CountLbl")
	local fameIcon = TransformFindChild(pointLabel, "Icon")
	local enableLbl = TransformFindChild(getBtn, "EnableLbl")
	local disableLbl = TransformFindChild(getBtn, "DisableLbl")
 	UIHelper.SetLabelTxt(pointLabel,  ConvertNumber(curFamePoint).."/"..ConvertNumber(keyID))
 	local countWidth = UIHelper.SizeOfWidget(pointLabel).x
 	fameIcon.localPosition = NewVector3(-countWidth/2,0,0)
 	local li = Util.AddClick(getBtn.gameObject, GetFameReward)
 	li.parameter = {id = keyID}
	hasGotTrans.gameObject:SetActive(status == 2)
	getBtn.gameObject:SetActive(status ~= 2)
	if status == 0 then --不可领取
		UIHelper.SetWidgetColor(pointLabel, "win_w_24")
		UIHelper.SetButtonActive(getBtn, false, false)
		enableLbl.gameObject:SetActive(false)
		disableLbl.gameObject:SetActive(true)
	elseif status == 1 then --可以领取
		UIHelper.SetWidgetColor(pointLabel, "win_b_24")
		UIHelper.SetButtonActive(getBtn, true, false)
		enableLbl.gameObject:SetActive(true)
		disableLbl.gameObject:SetActive(false)
	elseif status == 2 then --已领取
	end
end
function GetFameReward(go)
	local li = UIHelper.GetUIEventListener(go)
	CoachData.ReqCoachFameReward(RefreshFameItemStatus, li.parameter.id)
end
function RefreshFameItemStatus(data)
	local id = data.rewardSeq
	local reward = data.reward
	SetFameItemStatus(fameItemTable[tostring(id)], id, 2)

	--显示奖励
	local itemList = {}
	itemList.item = {}
	for i,v in ipairs(reward) do
		table.insert(itemList.item, {id=v.item_id,num=v.item_num})
	end
	local params = {}
	params.m_itemTb = itemList
	params.titleName = GetLocalizedString("GetActReward")
	WindowMgr.ShowWindow(LuaConst.Const.UIGetItem,params)
end