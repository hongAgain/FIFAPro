module("GameCheckInsData", package.seeall);

require "Game/Role"

local infoData = nil;
local checkInData = nil;
local localData = nil;

local infoDelegateFunc = nil;
local checkInDelegateFunc = nil;

function OnInit()
    DataSystemScript.RegisterMsgHandler(MsgID.tb.GameCheckInsInfo, OnReqCheckInInfo);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.GameCheckInsToday, OnReqCheckInToday);
end

function OnRelease()
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.GameCheckInsInfo, OnReqCheckInInfo);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.GameCheckInsToday, OnReqCheckInToday);
end

--no need to update every time you get into the shop
function RequestCheckInInfo( delegatefunc )
    infoDelegateFunc = delegatefunc;
    DataSystemScript.RequestWithParams(LuaConst.Const.GameCheckInsInfo, nil, MsgID.tb.GameCheckInsInfo);
end
--no need to update every time you get into the shop
function RequestCheckInToday( delegatefunc )
    checkInDelegateFunc = delegatefunc;
    DataSystemScript.RequestWithParams(LuaConst.Const.GameCheckInsCheck, nil, MsgID.tb.GameCheckInsToday);
end

function OnReqCheckInInfo(code_, data_)
    print(".. OnReqCheckInInfo!!!");
    infoData = data_;
    if (infoDelegateFunc ~= nil) then
        infoDelegateFunc();
        infoDelegateFunc = nil;
    end
end

function OnReqCheckInToday(code_, data_)
    print(".. OnReqCheckInToday!!!");
    checkInData = data_;
    if (checkInDelegateFunc ~= nil) then
        checkInDelegateFunc();
        checkInDelegateFunc = nil;
    end
end

function Get_InfoData()
    return infoData;
end

function Get_CheckInData()
    return checkInData;
end

function Get_LocalData()
	if (localData == nil) then
		localData = Config.GetTemplate(Config.DailySignTable());
	end
	local dataKey = tostring(Util.GetLocalYear());
	local month = Util.GetLocalMonth();
	if (month < 10)then
		dataKey = dataKey.."0";
	end
	dataKey = dataKey..month;
	if (localData ~= nil) then
		return localData[dataKey];
	else
		return nil;
	end
end

function Update_InfoData()

    local indexToday = nil;
    if(infoData.s==0)then
        infoData.n = infoData.n+1;
    end
    local indexToday = infoData.n;

	local currentMonthData = Get_LocalData();
    print("debug indexToday is "..indexToday);
	if(currentMonthData[indexToday]~=nil)then
        if(currentMonthData[indexToday].double == 1)then
    		if(Role.Get_vip() >= currentMonthData[indexToday].vip)then
    			infoData.s = 2;
    		else
    			infoData.s = 1;
    		end
        else
            infoData.s = 1;
        end
	else
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { "签到数据有误，本地无此信息："..indexToday }); 
		--infoData.s = 1;
	end
end
