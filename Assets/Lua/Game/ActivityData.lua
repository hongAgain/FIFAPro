module("ActivityData", package.seeall)

require "Common/CommonScript"

local timeActivityInfo = {}--限时性活动
local regularActivityInfo = {}--永久性活动
regularActivityInfo[1] = { _id = "1", name = "领取体力",     desc = "", timeType = 0, uiType = 1, sort = 1, openStatusId = "0" }
regularActivityInfo[2] = { _id = "2", name = "等级基金",     desc = "", timeType = 0, uiType = 2, sort = 2, openStatusId = "0"}
regularActivityInfo[3] = { _id = "3", name = "七日登录奖励", desc = "", timeType = 0, uiType = 3, sort = 3, openStatusId = "1302" }
regularActivityInfo[4] = { _id = "4", name = "累计登录奖励", desc = "", timeType = 0, uiType = 4, sort = 4, openStatusId = "0" }
regularActivityInfo[5] = { _id = "5", name = "等级礼包",     desc = "", timeType = 0, uiType = 5, sort = 5, openStatusId = "0" }
regularActivityInfo[6] = { _id = "6", name = "开服冲击奖励", desc = "", timeType = 0, uiType = 6, sort = 6, openStatusId = "1303" }

--callback 句柄
local getActivityInfoCB = nil
local getSevenLoginInfoCB = nil
local getSevenLoginRewardCB = nil
local getAccLoginInfoCB = nil
local getAccLoginRewardCB = nil
local getLvlGiftInfoCB = nil
local getLvlGiftRewardCB = nil
local getLvRankCB = nil
local getTimeActRewardCB = nil
local getActiveAdCB = nil

--初始化，注册网络通信的回调函数
function OnInit()
	ResetParameters()
	DataSystemScript.RegisterMsgHandler(MsgID.tb.ActivityInfo, OnGetActiveInfo)
	DataSystemScript.RegisterMsgHandler(MsgID.tb.SevenLoginInfo,OnGetSevenLoginInfo)
	DataSystemScript.RegisterMsgHandler(MsgID.tb.SevenLoginReward,OnSevenLoginReward)
	DataSystemScript.RegisterMsgHandler(MsgID.tb.AccLoginInfo,OnGetAccLoginInfo)
	DataSystemScript.RegisterMsgHandler(MsgID.tb.AccLoginReward,OnAccLoginReward)
	DataSystemScript.RegisterMsgHandler(MsgID.tb.LevelGiftInfo,OnGetLvlGiftInfo)
	DataSystemScript.RegisterMsgHandler(MsgID.tb.LevelGiftReward,OnLvlGiftReward)
	DataSystemScript.RegisterMsgHandler(MsgID.tb.LevelRankReward,OnLvRankInfo)
	DataSystemScript.RegisterMsgHandler(MsgID.tb.ActiveFetch,OnFetchTimeActiveReward)
	DataSystemScript.RegisterMsgHandler(MsgID.tb.ActiveAd,OnGetTimeActiveInfo)
end
--释放回调句柄
function OnRelease()
	ResetParameters()
	DataSystemScript.UnRegisterMsgHandler(MsgID.tb.ActivityInfo, OnGetActiveInfo)
	DataSystemScript.UnRegisterMsgHandler(MsgID.tb.SevenLoginInfo,OnGetSevenLoginInfo)
	DataSystemScript.UnRegisterMsgHandler(MsgID.tb.SevenLoginReward,OnSevenLoginReward)
	DataSystemScript.UnRegisterMsgHandler(MsgID.tb.AccLoginInfo,OnGetAccLoginInfo)
	DataSystemScript.UnRegisterMsgHandler(MsgID.tb.AccLoginReward,OnAccLoginReward)
	DataSystemScript.UnRegisterMsgHandler(MsgID.tb.LevelGiftInfo,OnGetLvlGiftInfo)
	DataSystemScript.UnRegisterMsgHandler(MsgID.tb.LevelGiftReward,OnLvlGiftReward)
	DataSystemScript.UnRegisterMsgHandler(MsgID.tb.LevelRankReward,OnLvRankInfo)
	DataSystemScript.UnRegisterMsgHandler(MsgID.tb.ActiveFetch,OnFetchTimeActiveReward)
	DataSystemScript.UnRegisterMsgHandler(MsgID.tb.ActiveAd,OnGetTimeActiveInfo)
	
end

function ResetParameters()
	activityInfo = nil
	getActivityInfoCB = nil
	getSevenLoginInfoCB = nil
	getSevenLoginRewardCB = nil
	getAccLoginInfoCB = nil
	getAccLoginRewardCB = nil
	getLvlGiftInfoCB = nil
	getLvlGiftRewardCB = nil
	getLvRankCB = nil
	getTimeActRewardCB = nil
	getActiveAdCB = nil
end

--相关通信接口
--相关通信接口回调函数
function ReqActiveInfo( cb ) 
	--每次打开界面都需向服务器获取最新活动数据
	getActivityInfoCB = cb
	DataSystemScript.RequestWithParams(LuaConst.Const.ActivityInfo, nil, MsgID.tb.ActivityInfo)
end
function OnGetActiveInfo(code, data)
	local funcActivityInfo = {} --以显示的tab顺序排列
	local actList = data.activeList
	local openStatus = data.funcStatus
	timeActivityInfo = {}
	--regular active 需要根据服务器返回筛选掉不显示的
	for i,v in ipairs(regularActivityInfo) do
		if openStatus[v.openStatusId] ~= 1 then
			funcActivityInfo[#funcActivityInfo + 1] = v
		end
	end
	--需要筛选掉超过显示期的活动
	local curTime = Util.GetTime()  --当前时间ms，
	for k,v in CommonScript.PairsByOrderKey(actList,"sort",true) do
		if curTime >= v.STime and curTime <= v.ETime then --在开始和结束时间内
			v.uiType = tonumber(v.ui)
			timeActivityInfo[#timeActivityInfo + 1] = v
		end
	end
	getActivityInfoCB(funcActivityInfo, timeActivityInfo)
end

--运营活动奖励领取
function ReqFetchTimeActiveReward( cb, id )
	getTimeActRewardCB = cb
	local param = {}
	param.id = id
	DataSystemScript.RequestWithParams(LuaConst.Const.ActiveFetch,param,MsgID.tb.ActiveFetch)
end
function OnFetchTimeActiveReward( code, data )
	if code == nil then
		getTimeActRewardCB(data)
		getTimeActRewardCB = nil
	end
end

--七日登录
function ReqSevenLoginInfo( cb )
	getSevenLoginInfoCB = cb
	DataSystemScript.RequestWithParams(LuaConst.Const.SevenLoginInfo,nil,MsgID.tb.SevenLoginInfo)
end
function OnGetSevenLoginInfo( code,data )
	if code == nil then
		getSevenLoginInfoCB(data)
		getSevenLoginInfoCB = nil
	end
end
function ReqSevenLoginReward( cb, param )
	getSevenLoginRewardCB = cb
	DataSystemScript.RequestWithParams(LuaConst.Const.SevenLoginReward,param,MsgID.tb.SevenLoginReward)
end
function OnSevenLoginReward( code,data )
	if code == nil then
		getSevenLoginRewardCB(data)
		getSevenLoginRewardCB = nil
	end
end
--累积登录
function ReqAccLoginInfo( cb )
	getAccLoginInfoCB = cb
	DataSystemScript.RequestWithParams(LuaConst.Const.AccLoginInfo,nil,MsgID.tb.AccLoginInfo)
end
function OnGetAccLoginInfo( code,data)
	if code == nil then
		getAccLoginInfoCB(data)
		getAccLoginInfoCB = nil
	end
end
function ReqAccLoginReward( cb,param )
	getAccLoginRewardCB = cb
	DataSystemScript.RequestWithParams(LuaConst.Const.AccLoginReward,param,MsgID.tb.AccLoginReward)
end
function OnAccLoginReward( code,data )
	if code == nil then
		getAccLoginRewardCB(data)
		getAccLoginRewardCB = nil
	end
end
--等级礼包
function ReqLevelGiftInfo( cb )
	getLvlGiftInfoCB = cb
	DataSystemScript.RequestWithParams(LuaConst.Const.LevelGiftInfo,nil,MsgID.tb.LevelGiftInfo)
end
function OnGetLvlGiftInfo( code,data )
	if code == nil then
		getLvlGiftInfoCB(data)
		getLvlGiftInfoCB = nil
	end
end
function ReqLevelGiftReward( cb, param )
	getLvlGiftRewardCB = cb
	DataSystemScript.RequestWithParams(LuaConst.Const.LevelGiftReward,param,MsgID.tb.LevelGiftReward)
end
function OnLvlGiftReward( code,data )
	if code == nil then
		getLvlGiftRewardCB(data)
		getLvlGiftRewardCB = nil
	end
end
--开服冲级奖励
function ReqLvRankInfo( cb )
	getLvRankCB = cb
	DataSystemScript.RequestWithParams(LuaConst.Const.LevelRankReward, nil, MsgID.tb.LevelRankReward)
end
function OnLvRankInfo( code, data )
	if code == nil then
		getLvRankCB(data)
		getLvRankCB = nil
	end
end

--获取单个运营活动的数据
function ReqTimeActiveInfo( cb, id )
	getActiveAdCB = cb
	local param = {}
	param.id = id
	DataSystemScript.RequestWithParams(LuaConst.Const.ActiveAd,param,MsgID.tb.ActiveAd)
end
function OnGetTimeActiveInfo( code, data )
	if code == nil then
		getActiveAdCB(data)
		getActiveAdCB = nil
	end
end

--common function
function UpdatePBValueByDivision(barTrans,fromValue,toValue,div,endFunc)
	CommonScript.UpdatePBValueByDivision(barTrans, fromValue, toValue, div, endFunc)
end
