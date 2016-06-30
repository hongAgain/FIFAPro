module("UIRankList", package.seeall)

require "Common/UnityCommonScript"
require "Game/RankListData"
require "Game/Role"
require "UILua/UIFriendView"

--data
local lvlRankData = nil
local ftRankData = nil
local scntRankData = nil
local pageCount = 10

--ui 
local window = nil
local windowComponent = nil
local transform = nil
local container = nil
--local titleLbl = nil
local rankTypeLbl = nil
local myCurRankLbl = nil
local myClubTexture = nil
local myNameLbl = nil
local myCurRankValueLbl = nil
local tpTrans = nil
local rankListSV = nil
local rankListGrid = nil
local rankListPB = nil
local lvlRankTab = nil
local ftRankTab = nil
local scntRankTab = nil

local rankItemPrefabName = "RankItem"
local rankItemPrefab = nil 
local firstItem = nil
local secondItem = nil
local thirdItem = nil


local RankType = RankListData.RankType
local rankTabTable = {} -- {[tabName] = {data = nil, } }
local RankType2Name = {}
local curRankType = 0
local curRankPage = 1
local myCurRank = -1

function OnStart(go)
	rankTabTable = {}
	window = go
	transform = go.transform
	windowComponent = window:GetComponent("UIBaseWindowLua")
	if rankItemPrefab == nil then
		rankItemPrefab = windowComponent:GetPrefab(rankItemPrefabName)
	end
	BindUI()
	OnShow()
end
function BindUI()
	container = TransformFindChild(transform, "Container")
	--titleLbl = TransformFindChild(container, "Title")
	rankTypeLbl = TransformFindChild(container, "ItemInfo/RankTypeTitle")
	myCurRankLbl = TransformFindChild(container, "MyRankInfo/Rank")
	myClubTexture = TransformFindChild(container, "MyRankInfo/ClubIcon")
	myNameLbl = TransformFindChild(container, "MyRankInfo/Name")
	myCurRankValueLbl = TransformFindChild(container, "MyRankInfo/RankTypeValue")
	tpTrans = TransformFindChild(container, "TweenPosition")
	rankListSV = TransformFindChild(container, "TweenPosition/ScrollView")
	rankListGrid = TransformFindChild(container, "TweenPosition/ScrollView/Grid")
	rankListPB = TransformFindChild(container, "TweenPosition/ScrollView/ProgressBar")
	lvlRankTab = TransformFindChild(transform, "TabList/Grid/LevelRank")
	ftRankTab = TransformFindChild(transform, "TabList/Grid/FightingRank")
	scntRankTab = TransformFindChild(transform, "TabList/Grid/StarCountRank")

	UIHelper.AddToggle(lvlRankTab, OnToggleChange)
	UIHelper.AddToggle(ftRankTab, OnToggleChange)
	UIHelper.AddToggle(scntRankTab, OnToggleChange)
	rankTabTable[lvlRankTab.name] = {}
	rankTabTable[lvlRankTab.name].type = RankType.Level
	--rankTabTable[lvlRankTab.name].title = "UIRLLevelTitle"
	rankTabTable[lvlRankTab.name].rankTitle = "UIRLLevel"
	UIHelper.SetLabelTxt(TransformFindChild(lvlRankTab, "Title"), Util.LocalizeString("UIRLLevelTitle"))
	rankTabTable[ftRankTab.name] = {}
	rankTabTable[ftRankTab.name].type = RankType.Fighting
	--rankTabTable[ftRankTab.name].title = "UIRLFightingTitle"
	rankTabTable[ftRankTab.name].rankTitle = "UIRLFighting"
	UIHelper.SetLabelTxt(TransformFindChild(ftRankTab, "Title"), Util.LocalizeString("UIRLFightingTitle"))
	rankTabTable[scntRankTab.name] = {}
	rankTabTable[scntRankTab.name].type = RankType.StarCount
	--rankTabTable[scntRankTab.name].title = "UIRLStarCountTitle"
	rankTabTable[scntRankTab.name].rankTitle = "UIRLStarCount"
	UIHelper.SetLabelTxt(TransformFindChild(scntRankTab, "Title"), Util.LocalizeString("UIRLStarCountTitle"))
	container.gameObject:SetActive(false)
	UIHelper.AddDragOnFinish(rankListSV,OnDragSVEnd)

	firstItem = TransformFindChild(rankListSV, "FirstThreeGrid/FirstItem")
	secondItem = TransformFindChild(rankListSV, "FirstThreeGrid/SecondItem")
	thirdItem = TransformFindChild(rankListSV, "FirstThreeGrid/ThirdItem")
end

function InitUIInfo()
	-- body
end

function OnShow()
	UIHelper.SetToggleState(lvlRankTab, true)
end

function OnToggleChange( selected, trans )
	if selected then
		local key = trans.name
		if rankTabTable[key].data == nil then
			RankListData.ReqGetRankListInfo(GetRankListData, rankTabTable[key].type)
		else
			InitRankInfo(rankTabTable[key].type)
		end
	end
	--update tab
	local selectedTrans = TransformFindChild(trans, "Select")
	selectedTrans.gameObject:SetActive(selected)
end

function GetRankListData(data)
	local tp = data.type
	local selfRank = data.rankSelf
	local rankList = ReorderRankListBySort(data.rankList)
	local key = GetRankTabTableKeyByType(tp)
	rankTabTable[key].data = {}
	rankTabTable[key].data.selfRank = selfRank
	rankTabTable[key].data.rankList = rankList
	InitRankInfo(tp)
	container.gameObject:SetActive(true)
end

function ReorderRankListBySort(list)
	local newList = {}
	for k,v in CommonScript.PairsByOrderKey(list, "sort", true) do
		v.uid = k
		newList[#newList + 1] = v
	end
	return newList
end

function InitRankInfo(tp)
	SetMyTypeRankInfo(tp)
	UIHelper.SetProgressBar(rankListPB, 0)
	SetRankListInfo(tp, 1)
	tpTrans.gameObject:SetActive(false) -- 强行刷新显示
	tpTrans.gameObject:SetActive(true)
	UIHelper.TweenPositionPlayForward(tpTrans, true)
end

function SetMyTypeRankInfo( rankType )
	local key = GetRankTabTableKeyByType(rankType)
	--UIHelper.SetLabelTxt(titleLbl, Util.LocalizeString(rankTabTable[key].title))
	UIHelper.SetLabelTxt(rankTypeLbl, Util.LocalizeString(rankTabTable[key].rankTitle))
	--mine
	local showData = rankTabTable[key].data.selfRank
	if showData.sort == nil then
		UIHelper.SetLabelTxt(myCurRankLbl, Util.LocalizeString("UIRLNullRank"))
		myCurRank = -1
	else
		UIHelper.SetLabelTxt(myCurRankLbl, string.format(Util.LocalizeString("UIRLCurRank"),showData.sort + 1))
		myCurRank = showData.sort
	end
	showData.icon = showData.icon or ""
	Util.SetUITexture(myClubTexture, LuaConst.Const.ClubIcon, showData.icon.."_2", false)
	UIHelper.SetLabelTxt(myNameLbl, showData.name)
	SetRankValueLbl(rankType, myCurRankValueLbl, showData.val, showData.lv)
end
function SetRankListInfo(rankType, page)
	curRankPage = page
	curRankType = rankType
	local key = GetRankTabTableKeyByType(rankType)
	local s = (page - 1) * pageCount + 1
	local e = page * pageCount
	local count = #rankTabTable[key].data.rankList
	if page == 1 then
		UIHelper.DestroyGrid(rankListGrid)
		UIHelper.RepositionGrid(rankListGrid, rankListSV)
		if count < 3 then
			thirdItem.gameObject:SetActive(false)
		end
		if count < 2 then
			secondItem.gameObject:SetActive(false)
		end
		if count < 1 then
			firstItem.gameObject:SetActive(false)
		end
	end

	for i = s, e do
		local itemData = rankTabTable[key].data.rankList[i]
		if itemData == nil then --到达最后一个数据
			break
		end
		local trans = nil
		if i <= 3 then
			if i == 1 then
				trans = firstItem
			elseif i == 2 then
				trans = secondItem
			elseif i == 3 then
				trans = thirdItem
			end
			trans.gameObject:SetActive(true)
		else
			trans = InstantiatePrefab(rankItemPrefab, rankListGrid, tostring(itemData.sort + 1)).transform
			local bgSprite = TransformFindChild(trans, "BG")
			local rankLbl = TransformFindChild(trans, "Rank")
			UIHelper.SetLabelTxt(rankLbl, itemData.sort + 1)
			bgSprite.gameObject:SetActive(i%2 ~= 0)
		end
		UIHelper.SetDragScrollViewTarget(trans, rankListSV)
		local clubIconTex = TransformFindChild(trans, "ClubIcon")
		local nameLbl = TransformFindChild(trans, "Name")
		local valueLbl = TransformFindChild(trans, "RankTypeValue")
		local myRankFlag = TransformFindChild(trans, "MyRank")

		if itemData.sort == myCurRank then
			--显示自己所在排名标识
			myRankFlag.gameObject:SetActive(true)
		else
			myRankFlag.gameObject:SetActive(false)
		end

		Util.SetUITexture(clubIconTex, LuaConst.Const.ClubIcon, itemData.icon.."_2", false)
		UIHelper.SetLabelTxt(nameLbl, itemData.name)
		SetRankValueLbl(rankType, valueLbl, itemData.val, itemData.lv)
		local li = Util.ChangeClick(trans.gameObject, RankItemClick)
		li.parameter = {idx = i}
	end

	UIHelper.RepositionGrid(rankListGrid)
	if rankListGrid.childCount == #rankTabTable[key].data.rankList - 3 then
		UIHelper.SetScrollViewRestrict(rankListSV, true)
	end
end

function RankItemClick( go )
	local li = UIHelper.GetUIEventListener(go)
	local idx = li.parameter.idx
	local curKey = GetRankTabTableKeyByType(curRankType)
	local curItem = rankTabTable[curKey].data.rankList[idx]
	if curItem.sort == myCurRank then
		return
	end
	local param = {}
	param.tgtUid = curItem.uid
	Role.RequestRoleData(param,ShowSelectedUserInfo)
end
function ShowSelectedUserInfo( data )
	local tb = {}
	tb.data = data
	tb.type = UIFriendView.ParentType.RankList
	WindowMgr.ShowWindow(LuaConst.Const.UIFriendView,tb)
end

local dragSvEndtimeTick = nil
local startTime = 0
local frequency = 60
local checkTime = 1
function OnDragSVEnd()
	local key = GetRankTabTableKeyByType(curRankType)
	if dragSvEndtimeTick ~= nil then
		LuaTimer.RmvTimer(dragSvEndtimeTick)
		dragSvEndtimeTick = nil
	end
	if rankListGrid.childCount == #rankTabTable[key].data.rankList then
		UIHelper.SetScrollViewRestrict(rankListSV, true)
	else
		UIHelper.SetScrollViewRestrict(rankListSV, UIHelper.IsOverDragged(rankListSV, true, false, false, false))
		if UIHelper.GetProgressBar(rankListPB) >= 1 then
			SetRankListInfo(curRankType, curRankPage + 1)
		else
			startTime = Util.GetTime() / 1000
			if dragSvEndtimeTick ~= nil then
				LuaTimer.RmvTimer(dragSvEndtimeTick)
			end
			dragSvEndtimeTick = LuaTimer.AddTimer(false, -1/60, CheckDragSVEnd)
		end
	end
end

function CheckDragSVEnd()
	if UIHelper.GetProgressBar(rankListPB) >= 1 then
		OnDragSVEnd()
	end
	local curTime = Util.GetTime() / 1000
	if curTime - startTime >= checkTime then
		if dragSvEndtimeTick ~= nil then
			LuaTimer.RmvTimer(dragSvEndtimeTick)
			dragSvEndtimeTick = nil
		end
	end
end

function SetRankValueLbl(tp, lbl, val, lv )
	if tp == RankType.Level then
		UIHelper.SetLabelTxt(lbl, "Lv."..lv)
	else
		UIHelper.SetLabelTxt(lbl, val)
	end
end

function GetRankTabTableKeyByType(tp)
	for k,v in pairs(rankTabTable) do
		if v.type == tp then
			return k
		end
	end
end

function OnHide()
end

function OnDestroy()
	OnHide()
	rankTabTable = nil
end