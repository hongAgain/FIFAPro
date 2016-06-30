module("RankListData", package.seeall)

RankType = 
{
	Level = 0,            --球队等级
	Fighting = 1,         --球队战力
	StarCount = 2,        --副本总星数
}

local getRankListInfoCB = nil

function OnInit()
	ResetParameters()
	DataSystemScript.RegisterMsgHandler(MsgID.tb.RankList, OnGetRankListInfo)
end
function OnRelease()
	ResetParameters()
	DataSystemScript.UnRegisterMsgHandler(MsgID.tb.RankList, OnGetRankListInfo)
end
function ResetParameters()
	getRankListInfoCB = nil
end

--newwork
function ReqGetRankListInfo(cb, tp)
	local param = {}
	param.type = tp
	getRankListInfoCB = cb
	DataSystemScript.RequestWithParams(LuaConst.Const.RankList, param, MsgID.tb.RankList)
end
function OnGetRankListInfo(code, data)
	if code == nil then
		getRankListInfoCB(data)
		getRankListInfoCB = nil
	end
end
