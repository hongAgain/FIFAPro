--处理daily相关数据
module("DailyDataSys", package.seeall)

require("Common/CommonScript")

local DailyDataTable = {}

local pb = nil

function OnInit(  )
	-- body
end

function  OnDailyData( data )
	if data~=nil and data ~= false then
		pb = data['pb']
	end
end

function GetPBCount()
	return pb
end
function SetPBCount(value)
	pb = value
end