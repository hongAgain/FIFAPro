module("UICarnivalPopWin", package.seeall)

require "UILua/UICarnival"
require "UILua/UICarnivalPopWinItem"
require "Game/CarnivalData"
require "Common/CommonScript"
require "Config"

local PopWinType = nil
local showData = nil
local curType = nil
local getPoint = nil
local totalPoint = nil
local itemTable = nil
--ui
local transform = nil
local windowComponent = nil
local bg = nil
local titleLbl = nil
local closeBtn = nil
local scoreLbl = nil
local scorePb = nil
local itemSV = nil
local itemGrid = nil


--param = {type,data}
function OnStart( go, param)
	transform = go.transform
	windowComponent = GetComponentInChildren(go, "UIBaseWindowLua")
	PopWinType = UICarnival.PopWinType
	curType = param.type
	showData = param.data
	getPoint,totalPoint = UICarnival.GetPointInfo()
	UICarnivalPopWinItem.Init(windowComponent,PopWinType)
	BindUI()
	OnShow()
end

function BindUI()
	bg = TransformFindChild(transform, "MaskBG")
	titleLbl = TransformFindChild(transform, "Title")
	closeBtn = TransformFindChild(transform, "Close")
	scoreLbl = TransformFindChild(transform, "Score/Value")
	scorePb = TransformFindChild(transform, "ScoreProgressBar")
	itemSV = TransformFindChild(transform, "ScrollView")
	itemGrid = TransformFindChild(itemSV, "Grid")

	Util.ChangeClick(bg.gameObject, CloseClick)
	Util.ChangeClick(closeBtn.gameObject, CloseClick)
	--set
	if curType == PopWinType.PointReward then
		UIHelper.SetLabelTxt(titleLbl, GetLocalizedString("UICScoreReward"))
	elseif curType == PopWinType.SpGift then
		UIHelper.SetLabelTxt(titleLbl, GetLocalizedString("UICSpecialGift"))
	end
	UIHelper.SetLabelTxt(scoreLbl, getPoint.."/"..totalPoint)
	UIHelper.SetProgressBar(scorePb, getPoint/totalPoint)
end

function OnShow()
	UIHelper.DestroyGrid(itemGrid)
	itemTable = {}
	if curType == PopWinType.PointReward then
		local pointRewardConfig = Config.GetTemplate(Config.CarnivalPoint())
		for k,v in CommonScript.PairsByOrderKey(pointRewardConfig, "id", true) do
			local item = UICarnivalPopWinItem.Create(curType, v, itemGrid, PointRewardClick)
			UIHelper.SetDragScrollViewTarget(item.transform, itemSV)
			item:RefreshStatus(getPoint, showData[k])
			itemTable[v.id] = item
		end
	elseif curType == PopWinType.SpGift then
		local spGiftConfig = Config.GetTemplate(Config.CarnivalShop())
		for k,v in CommonScript.PairsByOrderKey(spGiftConfig, "id", true) do
			local item = UICarnivalPopWinItem.Create(curType, v, itemGrid, SpGiftBuyClick)
			UIHelper.SetDragScrollViewTarget(item.transform, itemSV)
			item:RefreshStatus(getPoint, showData[k])
			itemTable[v.id] = item
		end
	end
	UIHelper.RepositionGrid(itemGrid,itemSV)
end

function PointRewardClick( go )
	local li = UIHelper.GetUIEventListener(go)
	CarnivalData.ReqGetCPointAward(OnGetCallback, li.parameter.id)
end
function OnGetCallback( data )
	local id = tostring(data.id)
	showData[id] = Util.GetTime()
	UICarnival.RefreshPopWinData(curType, showData)
	itemTable[id]:RefreshStatus(getPoint, showData[id])
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

function SpGiftBuyClick( go )
	local li  = UIHelper.GetUIEventListener(go)
	CarnivalData.ReqBuyCSpGift(OnGetCallback,li.parameter.id)
end

function CloseClick(go)
	windowComponent:Close()
end

function OnDestroy()
	UICarnivalPopWinItem.OnDestroy()
	PopWinType = nil
	showData = nil
	curType = nil
	getPoint = nil
	totalPoint = nil
	itemTable = nil
	transform = nil
	windowComponent = nil
	titleLbl = nil
	closeBtn = nil
	scoreLbl = nil
	scorePb = nil
	itemSV = nil
	itemGrid = nil
end