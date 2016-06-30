module("UIHandOfMidas", package.seeall)

require "Game/HandOfMidasData"
require "Config"
require "Game/Role"
require "Common/UnityCommonScript"

local goldLblType = 
{
	"[abadb9]%d[-]",
	"[abadb9]%d[-] [66ccff]x%d[-]",
	"[abadb9]%d[-] [ffff00]x%d[-]"
}

local window = nil
local windowComponent = nil
local transform = nil
--ui
local tipsLbl = nil
local diamondCostLbl = nil
local goldGetLbl = nil
local buyBtn = nil
local closeBtn = nil
local businessMan1 = nil
local businessMan2 = nil
local businessMan3 = nil
local businessMan1Lbl = nil
local businessMan2Lbl = nil
local businessMan3Lbl = nil
local effTrans = nil
local effPrefab = nil
local bgBlock = nil

--data
local curTimes = 0
local nextTimes = 0
local maxTimes = 0
local multiClickBuy = false --用于屏蔽连续点击向服务器发送消息

function OnStart( go )
	window = go
	windowComponent = window:GetComponent("UIBaseWindowLua")
	transform = window.transform
	BindUI()
	HandOfMidasData.ReqGetHandOfMidasInfo(GetHOMidasInfo)
end

function BindUI()
	tipsLbl = TransformFindChild(transform, "TimesTips")
	diamondCostLbl = TransformFindChild(transform, "Cost/Label")
	goldGetLbl = TransformFindChild(transform,"Obtain/Label")
	buyBtn = TransformFindChild(transform, "ButtonBuy")
	closeBtn = TransformFindChild(transform, "ButtonClose")

	AddOrChangeClickParameters(buyBtn.gameObject, OnClickBuy, nil)
	AddOrChangeClickParameters(closeBtn.gameObject, OnClickClose, nil)
	bgBlock = TransformFindChild(transform, "BG/BGBlock")
	Util.ChangeClick(bgBlock.gameObject,OnClickClose)
	effTrans = TransformFindChild(transform, "Businessmen/3/Eff")
	businessMan1 = TransformFindChild(transform, "Businessmen/1")
	businessMan2 = TransformFindChild(transform, "Businessmen/2")
	businessMan3 = TransformFindChild(transform, "Businessmen/3")
	businessMan1Lbl = TransformFindChild(transform, "Businessmen/1/Words/Label")
	businessMan2Lbl = TransformFindChild(transform, "Businessmen/2/Words/Label")
	businessMan3Lbl = TransformFindChild(transform, "Businessmen/3/Words/Label")
	UIHelper.SetLabelTxt(businessMan1Lbl, Util.LocalizeString("UIHOMBusinessMan1"))
	UIHelper.SetLabelTxt(businessMan2Lbl, string.format(Util.LocalizeString("UIHOMBusinessMan2"), GetRateByType("2")))
	UIHelper.SetLabelTxt(businessMan3Lbl, string.format(Util.LocalizeString("UIHOMBusinessMan3"), GetRateByType("3")))
end

function GetHOMidasInfo(data)
	SetUIData(data.times,data.rate,true)
end

function SetUIData(times, rateType, isInit)
	curTimes = times
	nextTimes = times + 1
	local goldRate = GetRateByType(tostring(rateType))
	maxTimes = tonumber(Config.GetProperty(Config.VipBenefit(),tostring(Role.Get_vip()),"gb"))
	local diamondCost = Config.GetProperty(Config.MidasHand(),tostring(nextTimes),"gb")
	local baseGold = Config.GetProperty(Config.MidasBase(),tostring(Role.Get_lv()),"base")
	UIHelper.SetLabelTxt(tipsLbl, string.format(Util.LocalizeString("UIHOMInfo"),times.."/"..maxTimes))
	if diamondCost == 0 then
		UIHelper.SetLabelTxt(diamondCostLbl, Util.LocalizeString("free"))
	else
		UIHelper.SetLabelTxt(diamondCostLbl, diamondCost)
	end
	UIHelper.SetLabelTxt(goldGetLbl, string.format(goldLblType[rateType],baseGold,goldRate))
	SelectBusinessMan(rateType, isInit)
end

function OnClickBuy(go)
	--超过次数
	if multiClickBuy then
		return
	end
	if nextTimes > maxTimes then
		--show tips
		local nextVipMaxTimes = Config.GetProperty(Config.VipBenefit(),tostring(tonumber(Role.Get_vip()) + 1),"gb")
		if nextVipMaxTimes ~= nil then
			local params = {}
			params[1] = 
			{
				string.format(Util.LocalizeString("UIHOMBuyLimitTips"),curTimes,(tonumber(Role.Get_vip()) + 1),tonumber(nextVipMaxTimes)),
				Util.LocalizeString("UITimesLimitTitle"),
				Util.LocalizeString("Recharge") 
			}
			params[2] = true
			params[3] = OnClickRecharge
			WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm,params)
			return
		else
			WindowMgr.ShowMsgBox(Util.LocalizeString("UIHOMBuyMaxLimitTips"))
		end
	end
	multiClickBuy = true
	HandOfMidasData.ReqBuyHandOfMidas(BuyHandOfMidasCallback)
end
function OnClickRecharge()
	--跳转到充值界面
end
function OnClickClose()
	Close()
end

function BuyHandOfMidasCallback(data)
	multiClickBuy = false
	SetUIData(data.times,data.rate,false)
end

function SelectBusinessMan(tp, isInit)
	effTrans.gameObject:SetActive(false)
	if tp == 1 then
		businessMan1.gameObject:SetActive(true)
		businessMan2.gameObject:SetActive(false)
		businessMan3.gameObject:SetActive(false)
	elseif tp == 2 then
		businessMan2.gameObject:SetActive(true)
		businessMan1.gameObject:SetActive(false)
		businessMan3.gameObject:SetActive(false)
	elseif tp == 3 then
		if not isInit then
			if effPrefab == nil then
				effPrefab = windowComponent:GetPrefab("Buy_coin")
			end
			effTrans.gameObject:SetActive(true)
			InstantiatePrefab(effPrefab,effTrans)
		end
		businessMan3.gameObject:SetActive(true)
		businessMan2.gameObject:SetActive(false)
		businessMan1.gameObject:SetActive(false)
	end
end

function GetRateByType(tp)
	return Config.GetProperty(Config.MidasType(), tp, "rate")
end

function Close()
	windowComponent:Close()
end

function OnDestroy()
	window = nil
	windowComponent = nil
	transform = nil
	tipsLbl = nil
	diamondCostLbl = nil
	goldGetLbl = nil
	buyBtn = nil
	businessMan1 = nil
	businessMan2 = nil
	businessMan3 = nil
	businessMan1Lbl = nil
	businessMan2Lbl = nil
	businessMan3Lbl = nil
	effTrans = nil
	effPrefab = nil
end