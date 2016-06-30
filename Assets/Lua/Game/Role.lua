module("Role", package.seeall)

local sid = nil;
local oid = nil;
local gm = nil;
local lv = nil;
local lvBackup = nil;
local exp = nil;
local step = nil;
local power = nil;
local powerTime = nil;
local uid = nil;
local name = nil
local vip = nil;
local gold = nil;
local lock = nil;
local rmb = nil;
local icon = nil;
local iid = nil;
local cid = nil;
local dailyTask = nil;

onInitRoleSuccess = nil;
local onReqChangeNameCB = {};
local onReqChangeIconCB = {};
local onReqExchangeCode = nil;
local targetIcon = nil;
local targetName = nil;
local targetCodeData = nil;

-- local isGettingTargetRoleData = false;
local delegateOnGettingRoleData = nil;

function OnInit()
	DataSystemScript.RegisterMsgHandler(MsgID.tb.VerifyAuth, OnVerifyOK);
	DataSystemScript.RegisterMsgHandler(MsgID.tb.InitRole, OnInitRole);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.InitRole1, OnInitRole1);
	DataSystemScript.RegisterMsgHandler(MsgID.tb.RoleData, OnRecvRoleData);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.Exchange, OnReqExchangeCode);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.ChangeIcon, OnReqChangeIcon);
    DataSystemScript.RegisterMsgHandler(MsgID.tb.ChangeName, OnReqChangeName);
end

function OnRelease()
	DataSystemScript.UnRegisterMsgHandler(MsgID.tb.VerifyAuth, OnVerifyOK);
	DataSystemScript.UnRegisterMsgHandler(MsgID.tb.InitRole, OnInitRole);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.InitRole1, OnInitRole1);
	DataSystemScript.UnRegisterMsgHandler(MsgID.tb.RoleData, OnRecvRoleData);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.Exchange, OnReqExchangeCode);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.ChangeIcon, OnReqChangeIcon);
    DataSystemScript.UnRegisterMsgHandler(MsgID.tb.ChangeName, OnReqChangeName);
end

function OnVerifyOK(code, data)
	-- local dollar_id = data['id'];
	-- DataSystemScript.SetDollarId(dollar_id);

	step = data['step'];
	
	if (step >= 2) then
        GetRoleData();
    else
        Tutorial_UICreateRole.Start();
	end
end

function OnInitRole(code, data)
	if (code == nil or string.len(code) == 0) then
        if (onInitRoleSuccess ~= nil) then
            onInitRoleSuccess();
        end
	else
		local msg = "";

		if (code == 'init_name_empty') then
			msg = Util.LocalizeString("kongming");
		elseif (code == 'user_init_already') then
			msg = Util.LocalizeString("chongming");
        else
            msg = code;
		end

		WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { msg });
	end
end

function OnInitRole1(code, data)
    name = targetName;
    icon = targetIcon;
    
    for _, v in ipairs(onReqChangeNameCB) do
        v();
    end
    
    for _, v in ipairs(onReqChangeIconCB) do
        v();
    end
end

function GetRoleData()
    local param = {};
    param['key'] = 'info|hero|item|coach';
    
    RequestRoleData(param,OnRecMyRoleData);
end

function GetTargetRoleData(targetUID,delegatefunc)
    local param = {};
    param['key'] = 'info';
    param['tgtUid'] = targetUID;

    RequestRoleData(param,delegatefunc);
end

function OnRecMyRoleData(data)
    sid     = data['sid'];
    oid     = data['oid'];
    lv      = data['lv'];
    exp     = data['exp'];
    step    = data['step'];
    name    = data['name'];
    uid     = data['uid'];
    gm      = data['gm'];
    icon    = data['icon'];
    vip     = data['vip'];
    rmb     = data['rmb'];
    lock    = data['lock'];
    powerTime = data['powerTime']
    iid     = data['iid'] or 1;
    cid     = data['cid'];
    
    local team = data['team'];
    Hero.SetTeamData(team);

    ItemSys.OnItemData(data['item']);
    Hero.OnReqHeroData(data['hero']);
    PVPMsgManager.SetLadderSeasonData(data['ladder']);
    --CoachData.UpdateCoachUserInfo(data['coach'],data['coachTeam']);
    DailyDataSys.OnDailyData(data['daily'])
    CarnivalData.SetGegistTime(data['time'])

    SceneManager.RegisterWatcher(LobbySceneManager.OnEnterScene, LobbySceneManager.OnLeaveScene);
    LuaTutorial.Create();
    
    EnterGame();
end

function RequestRoleData(param,delegatefunc)
    delegateOnGettingRoleData = delegatefunc;
    DataSystemScript.RequestWithParams(LuaConst.Const.RoleData, param, MsgID.tb.RoleData);
end

function OnRecvRoleData(code, data)
    if (code == nil or string.len(code) == 0) then
        if(delegateOnGettingRoleData~=nil)then
            delegateOnGettingRoleData(data);
        end
    else
        print("OnRecvRoleData Error!\nCode = "..code);
    end
end

function CreateMyRole(coachid)
	if (step < 2) then
		--create role
		local param = {};
		param['id'] = "1";
        param['cid'] = coachid;
        
		DataSystemScript.RequestWithParams(LuaConst.Const.RoleInit, param, MsgID.tb.InitRole);
	else
		error("CreateMyRole enter else, something error! current step is "..step);
	end
end

function RoleDetail(name, area, clubIcon)
    local param = {};
    param['name'] = name;
    param['area'] = area;
    param['icon'] = clubIcon;

    DataSystemScript.RequestWithParams(LuaConst.Const.RoleInit1, param, MsgID.tb.InitRole1);
    
    targetName = name;
    targetIcon = clubIcon;
end

function RegisterNameAndIconChanged(changeIcon, changeName)
    table.insert(onReqChangeNameCB, changeName);
    table.insert(onReqChangeIconCB, changeIcon);
end

function UnRegisterNameAndIconChanged(changeIcon, changeName)
    local rmvIdx = -1;
    for i = 1, #onReqChangeNameCB do
        if (onReqChangeNameCB[i] == changeName) then
            rmvIdx = i;
        end
    end
    if (rmvIdx ~= -1) then
        table.remove(onReqChangeNameCB, rmvIdx);
    end
    
    rmvIdx = -1;
    for i = 1, #onReqChangeIconCB do
        if (onReqChangeIconCB[i] == changeIcon) then
            rmvIdx = i;
        end
    end
    if (rmvIdx ~= -1) then
        table.remove(onReqChangeIconCB, rmvIdx);
    end
end

function RequestChangeIcon(paramIcon)
    targetIcon = paramIcon;
    DataSystemScript.RequestWithParams(LuaConst.Const.RoleChangeIcon, {key = paramIcon}, MsgID.tb.ChangeIcon);
end

function RequestChangeName(paramName)
    targetName = paramName;
    DataSystemScript.RequestWithParams(LuaConst.Const.RoleChangeName, {name = paramName}, MsgID.tb.ChangeName);    
end

function RequestExchangeCode(paramCode, delegatefunc)
    onReqExchangeCode = delegatefunc
    DataSystemScript.RequestWithParams(LuaConst.Const.Exchange, {id = paramCode}, MsgID.tb.Exchange);
end

function OnReqChangeIcon(code_, data_)
    if(targetIcon~=nil)then
        icon = targetIcon;
        targetIcon = nil;
    end
    for _, v in ipairs(onReqChangeIconCB) do
        v();
    end
end

function OnReqChangeName(code_, data_)
    if(targetName~=nil)then
        name = targetName;
        targetName = nil;
    end
    for _, v in ipairs(onReqChangeNameCB) do
        v();
    end
end

function OnReqExchangeCode(code_, data_)  
    targetCodeData = data_;
    if (onReqExchangeCode ~= nil) then
        onReqExchangeCode();
    end
end

function EnterGame()
    --SetSceneState;
    print("..EnterGame");
    -- Application.LoadLevel("train");
    -- WindowMgr.ShowWindow(LuaConst.Const.UILobby);
    SceneManager.LoadLobbyScene();
end

function SetRoleData(key_,value_)
    if key_ == "exp" then
        exp = value_;
    elseif key_ == "lv" then
        lv = value_;
    elseif key_ == "powerTime" then
        powerTime = value_
        Util.ShowTestTime(powerTime);
    end

end

function  CalcPower(args)

    local MaxPower = Config.GetProperty(Config.LevelTable(), tostring(lv),"Power");
    local sec = Util.GetTotalSeconds(tonumber(powerTime));
    local restorCost = math.floor(sec/(5*60));
    local currCost = ItemSys.GetItemData(LuaConst.Const.Power).num;
   
    if currCost < MaxPower then
        currCost = currCost + restorCost;
        if currCost > MaxPower then
            currCost = MaxPower;
        end
    end

    return currCost;
end

function  GetRoleIcon()
   -- local iconName = 'Default';
   -- local icon = Config.GetProperty(Config.InitTeam(),tostring(icon),'icon');
   -- if icon ~= nil and string.len(icon) ~= 0 then
   --      iconName = icon;
   -- end

   -- return iconName;

    if(icon==nil or icon == "")then
        return "Default";
    end
    return icon;
end

function GetClubIconType1(originalIconName)
    return originalIconName.."_1";
end

function GetClubIconType2(originalIconName)
    return originalIconName.."_2";
end

function CheckRoleIcon(iconName)
	if(iconName == nil)then
		return '11000';
	end
	iconName = tostring(iconName);
	for k,v in pairs(Config.GetTemplate(Config.InitTeam())) do
		if(iconName == v.icon) then
			return iconName;
		end
	end
	return '11000';
end

function SetRoleIcon(iconTrans,iconName)
    if(iconName == nil)then
        iconName = "11000";
    end
    if(UIHelper.IsSpriteInAtlas(iconTrans,iconName))then
        UIHelper.SetSpriteName(iconTrans,iconName);
    else
        UIHelper.SetSpriteName(iconTrans,"11000");
    end
end

function GetSB()
    return ConvertNumber(ItemSys.GetItemData(LuaConst.Const.SB).num)
end

function GetGB()
    return ConvertNumber(ItemSys.GetItemData(LuaConst.Const.GB).num);
end

function Get_sid()
	return sid;
end

function Get_oid()
	return oid;
end

function Get_gm()
	return gm;
end

function Set_lvBack()
    lvBackup = lv;
end

function Get_lvBack()
    return lvBackup or 1;
end

function Get_lv()
	return lv;
end

function Get_exp()
	return exp;
end

function Get_step()
    return step;
end

function Set_step(id)
    step = id;
end

function Get_power()
    power = CalcPower();
	return power;
end

function Get_powerTime()
	return powerTime;
end

function Get_uid()
	return uid;
end

function Get_name()
	return name;
end

function Get_vip()
	return vip;
end

function Get_gold()
	return gold;
end

function Get_lock()
	return lock;
end

function Get_rmb()
	return rmb;
end

function Get_icon()
	return icon;
end

function Get_iid()
   return iid;
end

function Get_cid()
    return cid;
end

function Get_ExchangeCodeData()
    return targetCodeData;
end
