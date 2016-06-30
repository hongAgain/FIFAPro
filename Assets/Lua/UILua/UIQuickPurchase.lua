module("UIQuickPurchase",package.seeall)

require "Common/UnityCommonScript"
require "Common/CommonScript"
require "Game/QuickPurchase"
require "Game/Role"
require "Game/ItemSys"

local titleLbl = nil
local costSprite = nil
local costLbl = nil
local obtainSprite = nil
local obtainLbl = nil
local buyTipsLbl = nil
local closeBtn = nil
local buyBtn = nil
local bgBlock = nil
local window = nil
local windowComponent = nil

local curType = nil
local BuyType = {["Gold"] = 1,["Energy"] = 2,}

local costNum = nil
local obtainNum = nil
local buyTimes = nil

local maxPower = 600

local TitleNameKeys = 
{
	"BuyGold",
	"BuyEnergy",
}
local ObtainNamekeys = 
{
	"Icon_Money",
	"Icon_Power",
}


function OnStart( gameObject, params )
	GetCurrentType(params)
	window = gameObject
	windowComponent = GetComponentInChildren(window, "UIBaseWindowLua")

	BindUI();
end

function GetCurrentType( params )
	if params~=nil then
		curType = BuyType[params[1]]
		if curType == nil then
			curType=1
			print("[error] Input type invalid")
		end
	else
		curType = 1
		print("[error] Input type is nil")
	end
end

function BindUI()
	local transform = window.transform
	titleLbl = TransformFindChild(transform, "Title")
	costSprite = TransformFindChild(transform,"Cost")
	costLbl = TransformFindChild(transform, "Cost/Label")
	obtainSprite = TransformFindChild(transform,"Obtain")
	obtainLbl = TransformFindChild(transform,"Obtain/Label")
	buyTipsLbl = TransformFindChild(transform,"Tips")
	closeBtn = TransformFindChild(transform,"ButtonClose")
	AddOrChangeClickParameters(closeBtn.gameObject,OnClickClose,nil)
	buyBtn = TransformFindChild(transform,"ButtonBuy")
	bgBlock = TransformFindChild(transform, "BG/BGBlock")
	Util.ChangeClick(bgBlock.gameObject, OnClickClose)
	AddOrChangeClickParameters(buyBtn.gameObject,OnClickBuy,nil)
	-- UIHelper.SetSpriteNameNoPerfect(obtainSprite,"Icon_RMB")
	UIHelper.SetLabelTxt(titleLbl,string.format(Util.LocalizeString("QuickBuyTitle"),Util.LocalizeString(TitleNameKeys[curType])))
	UIHelper.SetSpriteNameNoPerfect(obtainSprite,ObtainNamekeys[curType])
	local tipStr = Util.LocalizeString("BuyCountTips")
	costNum = GetCost()
	obtainNum = GetObtainNum()
	buyTimes= GetTimes()
	UIHelper.SetLabelTxt(costLbl, costNum)
	UIHelper.SetLabelTxt(obtainLbl,obtainNum)
	UIHelper.SetLabelTxt(buyTipsLbl,string.format(tipStr,buyTimes))
	CommonScript.SetButtonActive(buyBtn,buyTimes>0)
end

function OnClickClose()
	Close()
end

function Close()
	windowComponent:Close()
end

function BuyCallBack( _data )
	DailyDataSys.SetPBCount(_data['pb'])
	RefreshBuyTimes()
	Close()
end

function OnClickBuy()
	--check max power
	if ItemSys.GetItemData(LuaConst.Const.Power).num >=maxPower then
		WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { Util.LocalizeString("UIBuyEnergyLimit")});
		Close()
		return
	end
	--check buy times
	if buyTimes <= 0 then
		local msg = string.format(Util.LocalizeString("BuyUpperLimit"),Util.LocalizeString(TitleNameKeys[curType]))
		WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, {msg})
		return
	end
	--check diamond enough
	if ItemSys.GetItemData(LuaConst.Const.GB).num >= costNum then
		QuickPurchase.ReqBuyEnergy(BuyCallBack)
	else
		--todo..
		WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { "钻石不足！" });
	end
end

function GetCost()
	if curType == BuyType["Gold"] then
		return 50
	elseif curType == BuyType["Energy"] then
		return 50
	else
		return -1
	end
end
function GetObtainNum()
	if curType == BuyType["Gold"] then
		return 10000
	elseif curType == BuyType["Energy"] then
		return 120
	else
		return -1
	end
end
function GetTimes()
	if curType == BuyType["Gold"] then
		return tonumber(Config.GetProperty(Config.VipBenefit(),tostring(Role.Get_vip()),"gb"))
	elseif curType == BuyType["Energy"] then
		local times = DailyDataSys.GetPBCount() or 0
		return tonumber(Config.GetProperty(Config.VipBenefit(),tostring(Role.Get_vip()),"pb")) - times
	else
		return 0
	end
end
function  RefreshBuyTimes()
	buyTimes = GetTimes()
	UIHelper.SetLabelTxt(buyTipsLbl,string.format(Util.LocalizeString("BuyCountTips"),buyTimes))
	if UIDailyTaskController ~= nil then
		UIDailyTaskController.RefreshPopTaskItem("pb",1)--刷新每日任务中购买体力的标签
	end
end