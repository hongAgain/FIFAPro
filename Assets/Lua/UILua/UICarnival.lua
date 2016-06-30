module("UICarnival", package.seeall)

require "Config"
require "Game/CarnivalData"
require "UILua/UICarnivalItem"
require "Common/CommonScript"

PopWinType = 
{
	PointReward = 1,
	SpGift = 2
}

local transform = nil
local windowComponent = nil

--ui
local remainTimeLbl = nil
local scoreLbl = nil
local pointPB = nil
local nextRewardTipLbl = nil
local pointRewardBtn = nil
local spGiftBtn = nil
local carnivalItemSV = nil
local carnivalItemGrid = nil
local carnivalTypeSV = nil
local carnivalTypeGrid = nil
--data
local typeTabPrefab = nil
local cTypeTable = nil --保存所有标签页
local cItemTable = nil --所有类型的目标集合
local curTypeId = nil --当前标签页索引
local defaultTypeId = nil
local totalPoint = nil
local getPoint = nil
local timeTick = nil
local isInit = nil
local cTaskData = nil --嘉年华任务完成度信息
local cSubmitData = nil --任务提交信息
local rewardGetData = nil --积分奖励领取信息
local giftBuyData = nil --特价礼包购买信息

function OnStart(gameObject)
	transform = gameObject.transform
	windowComponent = GetComponentInChildren(gameObject, "UIBaseWindowLua")
	BindUI()
	isInit = true
	cItemTable = {}
	UICarnivalItem.Init(windowComponent)
	OnShow()
end
function OnShow()
	CarnivalData.ReqCarnivalInfo(InitCarnivalInfo)
end

function BindUI()
	remainTimeLbl = TransformFindChild(transform, "Head/RemainTime/Value")
	scoreLbl = TransformFindChild(transform, "Head/Score/Value")
	pointPB = TransformFindChild(transform, "Head/PointPB")
	nextRewardTipLbl = TransformFindChild(transform, "Head/NextRewardTips")
	pointRewardBtn = TransformFindChild(transform, "Head/ScoreRewardBtn")
	spGiftBtn = TransformFindChild(transform, "Head/SpecialGiftBtn")
	carnivalItemSV = TransformFindChild(transform, "LeftScrollView")
	carnivalItemGrid = TransformFindChild(carnivalItemSV, "Grid")
	carnivalTypeSV = TransformFindChild(transform, "RightScrollView")
	carnivalTypeGrid = TransformFindChild(carnivalTypeSV, "Grid")
	Util.ChangeClick(pointRewardBtn.gameObject, OpenCPointRewardWin)
	Util.ChangeClick(spGiftBtn.gameObject,  OpenSpGiftWin)
	--根据配置文件设置些数据
	--init ui
	--从配置表读总积分数
	totalPoint = 0
	cTypeTable = {}
	local cConfig = Config.GetTemplate(Config.Carnival())
	for k,v in CommonScript.PairsByOrderKey(cConfig, "id", true) do
		totalPoint = totalPoint + v.point
		local typeKey = v.type
		if cTypeTable[typeKey] == nil then
			if defaultTypeId == nil then
				defaultTypeId = typeKey
			end
			local typeConfig = Config.GetKeyTotalInfo(Config.CarnivalType(), typeKey)
			if typeTabPrefab == nil then
				typeTabPrefab = windowComponent:GetPrefab("CarnivalTypeItem")
			end
			local tabClone = InstantiatePrefab(typeTabPrefab, carnivalTypeGrid, typeKey).transform --标签的name字段为类型id
			UIHelper.SetDragScrollViewTarget(tabClone, carnivalTypeSV)
			local nameLbl = TransformFindChild(tabClone, "Title")
			UIHelper.SetLabelTxt(nameLbl, typeConfig.name)
			UIHelper.AddToggle(tabClone, TypeToggleChange)
			cTypeTable[typeKey] = tabClone
		end
	end
	UIHelper.RepositionGrid(carnivalTypeGrid, carnivalTypeSV)
end

function InitCarnivalInfo( data )
	cTaskData = data.task or {}
	if data.log ~= nil then
		cSubmitData = data.log.log or {}
		rewardGetData = data.log.pointLog or {}
		giftBuyData = data.log.shopLog or {}
		getPoint = data.log.point
	else
		cSubmitData = {}
		rewardGetData = {}
		giftBuyData = {}
		getPoint = 0
	end
	RefreshHead()
	UIHelper.SetToggleState(cTypeTable[defaultTypeId], true)
end

function RefreshHead()
	UIHelper.SetLabelTxt(scoreLbl, getPoint.."/"..totalPoint)
	local pointConfig = Config.GetTemplate(Config.CarnivalPoint())
	local toNextRewardPoint = 0
	for k,v in CommonScript.PairsByOrderKey(pointConfig, "id", true) do
		local aimPoint = tonumber(k)
		if getPoint < aimPoint then
			toNextRewardPoint = aimPoint - getPoint
			break
		end
	end
	UIHelper.SetProgressBar(pointPB, getPoint/totalPoint)
	if toNextRewardPoint > 0 then
		UIHelper.SetLabelTxt(nextRewardTipLbl, GetLocalizedString("UICNextRewardTips", toNextRewardPoint))
	else
		UIHelper.SetLabelTxt(nextRewardTipLbl, GetLocalizedString("UICNextRewardTips2"))
	end
	if timeTick ~= nil then
		LuaTimer.RmvTimer(timeTick)
	end
	timeTick = LuaTimer.AddTimer(false, -1, RefreshDeadLine)
end
function RefreshDeadLine()
	local remainTime = CarnivalData.GetRemainTime()
	if remainTime <= 0 then
		UIHelper.SetLabelTxt(remainTimeLbl, GetLocalizedString("UICRemainTimeFormat", 0, 0, 0, 0))
		LuaTimer.RmvTimer(timeTick)
		timeTick = nil
		return
	end
	local day = math.floor(remainTime / 86400)
	local hour = math.floor((remainTime % 86400)/ 3600)
	local min = math.floor((remainTime % 3600) / 60)
	local sec = remainTime % 60
	UIHelper.SetLabelTxt(remainTimeLbl, GetLocalizedString("UICRemainTimeFormat", day, hour, min, sec))
end
function ShowCarnivalItemByType(typeId)
	local preTypeId = curTypeId
	curTypeId = typeId
	if preTypeId == curTypeId then
		return
	end
	--隐藏原来的目标item
	if cItemTable[preTypeId] ~= nil then 
		for i,v in ipairs(cItemTable[preTypeId]) do
			v.transform.gameObject:SetActive(false)
		end
	end
	--可根据领取数据进行显示优化todo
	--显示当前的目标item
	if cItemTable[curTypeId] ~= nil and cItemTable[curTypeId] ~= {} then
		for i,v in ipairs(cItemTable[curTypeId]) do
			v.transform.gameObject:SetActive(true)
		end
	else
		--创建新数据
		local curTypeConfig = Config.GetSamePropertyInfo(Config.Carnival(), "type", curTypeId)
		local itemTable = {}
		for k,v in CommonScript.PairsByOrderKey(curTypeConfig,"id", true) do
			local item = UICarnivalItem.Create(v,carnivalItemGrid,SubmitClick)
			UIHelper.SetDragScrollViewTarget(item.transform, carnivalItemSV)
			item:RefreshStatus(cTaskData[item.id],cSubmitData[item.id])
			itemTable[#itemTable + 1] = item
		end
		cItemTable[curTypeId] = itemTable
	end
	UIHelper.RepositionGrid(carnivalItemGrid,carnivalItemSV) --可能要加progressbar
end

function SubmitClick(go)
	local li = UIHelper.GetUIEventListener(go)
	CarnivalData.ReqSubmitCarnival(OnSubmitCallback, li.parameter.id)
end
function OnSubmitCallback( data )
	local id = data.id
	cSubmitData[id] = Util.GetTime()
	local addPoint = Config.GetProperty(Config.Carnival(), id, "point" )
	getPoint = getPoint + addPoint
	RefreshHead()
	local itemType = Config.GetProperty(Config.Carnival(), id, "type")
	for i,v in ipairs(cItemTable[itemType]) do
		if v.id == id then
			v:RefreshStatus(cTaskData[id],cSubmitData[id])
			break
		end
	end
	local itemList = {}
	itemList.item = {}
	for i,v in ipairs(data.item) do
		table.insert(itemList.item,{id=v.id,num=v.num})
	end
	local params = {}
	params.m_itemTb = itemList
	params.titleName = GetLocalizedString("GetActReward")
	WindowMgr.ShowWindow(LuaConst.Const.UIGetItem,params)
end

function OpenCPointRewardWin(go)
	local param = {}
	param.type = PopWinType.PointReward
	param.data = rewardGetData
	WindowMgr.ShowWindow(LuaConst.Const.UICarnivalPopWin,param)
end
function OpenSpGiftWin(go)
	local param = {}
	param.type = PopWinType.SpGift
	param.data = giftBuyData
	WindowMgr.ShowWindow(LuaConst.Const.UICarnivalPopWin,param)
end

function TypeToggleChange(selected, trans)
	local selectTrans = TransformFindChild(trans, "Select")
	selectTrans.gameObject:SetActive(selected)
	if selected then
		ShowCarnivalItemByType(trans.name)
	end
end

function GetPointInfo()
	return getPoint,totalPoint
end
function RefreshPopWinData( winType,data )
	if winType == PopWinType.PointReward then
		rewardGetData = data
	elseif winType == PopWinType.SpGift then
		giftBuyData = data
	end
end

function OnHide()
	if timeTick ~= nil then
		LuaTimer.RmvTimer(timeTick)
		timeTick = nil
	end
end
function OnDestroy()
	OnHide()
	UICarnivalItem.OnDestroy()
	transform = nil
	windowComponent = nil
	remainTimeLbl = nil
	scoreLbl = nil
	pointPB = nil
	nextRewardTipLbl = nil
	pointRewardBtn = nil
	spGiftBtn = nil
	carnivalItemSV = nil
	carnivalItemGrid = nil
	carnivalTypeSV = nil
	carnivalTypeGrid = nil
	typeTabPrefab = nil
	cTypeTable = nil --保存所有标签页
	cItemTable = nil --所有类型的目标集合
	curTypeId = nil --当前标签页索引
	defaultTypeId = nil
	totalPoint = nil
	getPoint = nil
	timeTick = nil
	isInit = nil
	cTaskData = nil --嘉年华任务完成度信息
	cSubmitData = nil --任务提交信息
	rewardGetData = nil --积分奖励领取信息
	giftBuyData = nil --特价礼包购买信息
end