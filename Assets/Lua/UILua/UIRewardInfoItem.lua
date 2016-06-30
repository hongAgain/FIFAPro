module("UIRewardInfoItem", package.seeall)

require "Common/UnityCommonScript"
require "Common/CommonScript"
require "UILua/UIIconItem"

local infoItemName = "RewardInfoItem"
local iconItemName = "IconItem"

local infoItemPref = nil
local iconItemPref = nil

local windowComponet = nil

ItemTitleType = 
{
	Label = 1,
	WithDiamondTex = 2,
	WithGoldTex = 3
}

RewardGetBtnType = 
{
	JustBtn = 1,
	BtnAndNum = 2,
}

RewardListType = 
{
	ConfigFile = 1,
	ServerData = 2
}

local RewardState = 
{
	CannotGet = 1,
	CanGet = 2,
	HasGot = 3,
}

function Init( winComponet )
	windowComponet = winComponet
	if infoItemPref == nil then
		infoItemPref = windowComponet:GetPrefab(infoItemName)
	end
	if iconItemPref == nil then
		iconItemPref = windowComponet:GetPrefab(iconItemName)
	end
	UIIconItem.Init()
end

--info={id,title,targetNum,getBtnType,rewardList, titleType, rewardType}
function CreateRewardInfoItem(info, idx, grid, scrollView, iconItemScale, delegateOnGet)
	local infoItem = {}
	infoItem.info = info
	--ui
	infoItem.gameObject = InstantiatePrefab(infoItemPref,grid,info.id)
	infoItem.transform = infoItem.gameObject.transform
	local trans = infoItem.transform
	local width = UIHelper.GetUIGridCellSize(grid).x
	infoItem.BG = trans
	local hight = UIHelper.HeightOfWidget(infoItem.BG)
	UIHelper.SetSizeOfWidget(infoItem.BG, NewVector2(width, hight))
	infoItem.getTrans = TransformFindChild(trans, "AnchorRight/GetState")
	InitGetState(infoItem.getTrans, info.id, delegateOnGet)
	infoItem.cmplLbl = TransformFindChild(trans, "AnchorRight/GetState/Completion")
	--infoItem.title = TransformFindChild(trans, "AnchorLeft/Title")
	local titleLbl = TransformFindChild(trans, "AnchorLeft/Title")
	local titleWithTex = TransformFindChild(trans, "AnchorLeft/TitleWithIcon")
	local curTitleType = info.titleType or ItemTitleType.Label
	if curTitleType == ItemTitleType.Label then
		UIHelper.SetLabelTxt(titleLbl, info.title)
	elseif curTitleType == ItemTitleType.WithDiamondTex or curTitleType == ItemTitleType.WithGoldTex then
		titleLbl.gameObject:SetActive(false)
		titleWithTex.gameObject:SetActive(true)
		SetTitleWithIconInfo(info.title, titleWithTex, curTitleType)
	end
	infoItem.iconScrollView = nil
	--UIHelper.SetPanelDepth(infoItem.iconScrollView,UIHelper.GetPanelDepth(scrollView) + 1)
	infoItem.iconGrid = TransformFindChild(trans, "AnchorLeft/RewardList")
	--create iconItem
	local rewardListType = info.rewardType or RewardListType.ConfigFile
	if rewardListType == RewardListType.ConfigFile then
		local list = info.rewardList:split(",")
		for i = 1,#list,2 do
			local iconId = list[i]
			local count = tonumber(list[i+1])
			local otherParam = {scale = iconItemScale, disableColid = false, offsetDepth = 1}
			UIIconItem.CreateRewardIconItem(infoItem.iconGrid,infoItem.iconScrollView,{iconId,count},otherParam)
		end
		UIHelper.RepositionGrid(infoItem.iconGrid)
	elseif rewardListType == RewardListType.ServerData then
		for i,v in ipairs(info.rewardList) do
			local iconId = v[2]
			local count = v[3]
			UIIconItem.CreateRewardIconItem(infoItem.iconGrid, infoItem.iconScrollView, {iconId, count}, {scale = iconItemScale})
		end
	end
	return infoItem
end

--num用于有infoItem.cmplLbl显示
function RefreshRewardItem( infoItem, hasGot, canGet, num)
	if infoItem == nil then
		return
	end
	if hasGot then
		SetGetState(infoItem.getTrans, RewardState.HasGot)
	elseif canGet then
		SetGetState(infoItem.getTrans, RewardState.CanGet)
	else
		SetGetState(infoItem.getTrans, RewardState.CannotGet)
	end
	--显示数字
	if infoItem.cmplLbl ~= nil then
		infoItem.cmplLbl.gameObject:SetActive(false)
		local btnType = infoItem.info.getBtnType
		if btnType == RewardGetBtnType.BtnAndNum then --含有数字显示的type
			infoItem.cmplLbl.gameObject:SetActive(not hasGot)
			if not hasGot then
				UIHelper.SetLabelTxt(infoItem.cmplLbl, num.."/"..infoItem.info.targetNum)
				local color = ""
				if canGet then
					color = "win_b_20"
				else
					color = "win_w_20"
				end
				UIHelper.SetWidgetColor(infoItem.cmplLbl, color)
			end
		end
	end
end

function InitGetState( trans, clickId, onGetClick )
	SetGetState(trans,RewardState.CannotGet)
	local btn = TransformFindChild(trans,"CanGet/Btn")
	local li = Util.AddClick(btn.gameObject, onGetClick)
	li.parameter = {id = clickId}
end
function SetGetState( trans, state )
	if trans == nil then
		return
	end
	local cantGetObj = TransformFindChild(trans, "CannotGet").gameObject
	local canGetObj = TransformFindChild(trans, "CanGet").gameObject
	local hasGotObj = TransformFindChild(trans, "HasGotLabel").gameObject
	cantGetObj:SetActive(state == RewardState.CannotGet)
	canGetObj:SetActive(state == RewardState.CanGet)
	hasGotObj:SetActive(state == RewardState.HasGot)
end

function SetTitleWithIconInfo( titleInfo, titleTrans, titleType )
	local iconName = "Icon_RMB"
	if titleType == ItemTitleType.WithGoldTex then
		iconName = "Icon_Money"
	end
	local title1Lbl = TransformFindChild(titleTrans, "Title1")
	local title2Lbl = TransformFindChild(titleTrans, "Title2")
	local iconSprt = TransformFindChild(titleTrans, "Icon")
	UIHelper.SetLabelTxt(title1Lbl, titleInfo.title1)
	UIHelper.SetLabelTxt(title2Lbl, titleInfo.title2)
	UIHelper.SetSpriteNameNoPerfect(iconSprt, iconName)
	--pos
	local title1Len = UIHelper.SizeOfWidget(title1Lbl).x
	local iconLen = UIHelper.SizeOfWidget(iconSprt).x
	local title1Pos = title1Lbl.localPosition
	local iconPosX = title1Pos.x + title1Len
	local title2LblPosX = iconPosX + iconLen
	iconSprt.localPosition = NewVector3(iconPosX, title1Pos.y, title1Pos.z)
	title2Lbl.localPosition = NewVector3(title2LblPosX, title1Pos.y, title1Pos.z)
end

