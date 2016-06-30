module("UITeamSettingBox", package.seeall)

local TeamSettings = {
	changeNameHint = "PleaseChangeName",
	changeIconHint = "PleaseChangeIcon",
	selectIconHint = "PleaseSelectIcon",
	useValidName = "PleaseUseValidName"
}

-- local teamSettingBoxUI = nil;

local buttonClose = nil;

local buttonTreatFriend = nil;
local buttonHonorRecord = nil;
local buttonSystemSettings = nil;

local icon = nil;
local buttonIcon = nil;
local buttonChangeIcon = nil;

local nameInput = nil;
local buttonRandomName = nil;
local buttonChangeName = nil;

local code = nil;
local buttonExchangeCode = nil;

-- local popupRoot = nil;

local targetName = nil;
local targetIcon = nil;

local delegateOnChangeName = nil;
local delegateOnChangeIcon = nil;

local window = nil;
local windowcomponent = nil;

function OnStart(gameObject, params)
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");

	delegateOnChangeName = params.delegateOnChangeName;
	delegateOnChangeIcon = params.delegateOnChangeIcon;
	BindUI();
	SetInfo();
    
    Role.RegisterNameAndIconChanged(OnIconChanged, OnNameChanged);
end

function BindUI()
	buttonClose = TransformFindChild(window.transform,"ButtonClose");	

	buttonTreatFriend = TransformFindChild(window.transform,"ButtonTreatFriend");
	buttonHonorRecord = TransformFindChild(window.transform,"ButtonHonorRecord");
	buttonSystemSettings = TransformFindChild(window.transform,"ButtonSystemSettings");

	buttonIcon = TransformFindChild(window.transform,"ButtonIcon");
	icon = TransformFindChild(buttonIcon,"Icon");
	buttonChangeIcon = TransformFindChild(window.transform,"ButtonChangeIcon");

	nameInput = TransformFindChild(window.transform,"NameInput");
	buttonRandomName = TransformFindChild(window.transform,"ButtonRandom");
	buttonChangeName = TransformFindChild(window.transform,"ButtonChangeName");

	codeInput = TransformFindChild(window.transform,"CodeInput");
	buttonExchangeCode = TransformFindChild(window.transform,"ButtonCodeExchange");

	AddOrChangeClickParameters(buttonClose.gameObject,OnClickClose,nil);
	AddOrChangeClickParameters(buttonTreatFriend.gameObject,OnClickTreatFriend,nil);
	AddOrChangeClickParameters(buttonHonorRecord.gameObject,OnClickHonorRecord,nil);
	AddOrChangeClickParameters(buttonSystemSettings.gameObject,OnClickSystemSettings,nil);
	AddOrChangeClickParameters(buttonIcon.gameObject,OnClickIcon,nil);
	AddOrChangeClickParameters(buttonChangeIcon.gameObject,OnClickChangeIcon,nil);
	AddOrChangeClickParameters(buttonRandomName.gameObject,OnClickRandomName,nil);
	AddOrChangeClickParameters(buttonChangeName.gameObject,OnClickChangeName,nil);
	AddOrChangeClickParameters(buttonExchangeCode.gameObject,OnClickExchangeCode,nil);
end

function SetInfo()
	Util.SetUITexture(icon,LuaConst.Const.ClubIcon,Role.GetRoleIcon().."_1", true);
	UIHelper.SetInputText(nameInput,Role.Get_name());
end

function OnClickClose()
	Close();
end

function OnClickTreatFriend()
	-- body
end

function OnClickHonorRecord()
	-- body
end

function OnClickSystemSettings()
	-- body
end

function OnClickIcon()
	WindowMgr.ShowWindow(LuaConst.Const.UITeamIconList,{delegateOnConfirmToChangeIcon = OnIconChangedTemprary});
end

function OnClickChangeIcon()
	if(targetIcon == nil)then
		WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { GetLocalizedString(TeamSettings.selectIconHint) });
		return;
	end
	if(targetIcon == Role.GetRoleIcon())then
		WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { GetLocalizedString(TeamSettings.changeIconHint) });
		return;
	end
	Role.RequestChangeIcon(targetIcon);
end

function OnClickRandomName()	
    local size1 = 0;
    local size2 = 0;
    for k,v in pairs(Config.GetTemplate(Config.AutoNameTeam())) do
        size1 = size1 + 1;
    end
    for k,v in pairs(Config.GetTemplate(Config.AutoNamePlayer())) do
        size2 = size2 + 1;
    end
    local str1 = Config.GetProperty(Config.AutoNameTeam(), tostring(math.random(size1)), 'fNameCN');
    local str2 = Config.GetProperty(Config.AutoNamePlayer(), tostring(math.random(size2)), 'sNameCN');    
    UIHelper.SetInputText(nameInput, str1..str2);    
end

function OnClickChangeName()
	targetName = UIHelper.InputTxt(nameInput);
    if (targetName == nil or string.len(targetName) == 0) then
		WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { GetLocalizedString(TeamSettings.useValidName) });
		return;
	end
	if(targetName == Role.Get_name())then
		WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { GetLocalizedString(TeamSettings.changeNameHint) });
		return;
	end
    local bRet = TableWordFilter:FilterText(targetName)
    targetName = TableWordFilter.FilteredWords
    if true ==  bRet then
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("feifazhi") });
    else
	    Role.RequestChangeName(targetName);
    end
end

function OnClickExchangeCode()	
    local code = UIHelper.InputTxt(codeInput);
	Role.RequestExchangeCode(code,OnCodeExchanged);
end

function OnCodeExchanged()
	UIHelper.SetInputText(codeInput,"");
	WindowMgr.ShowWindow(LuaConst.Const.UIGetItem,{
        m_itemTb = Role.Get_ExchangeCodeData(),
        OnClose = nil
    });
end

function OnIconChanged()	
	Util.SetUITexture(icon,LuaConst.Const.ClubIcon,Role.GetRoleIcon().."_1", true);
	if(delegateOnChangeIcon~=nil)then
		delegateOnChangeIcon();
	end
end

function OnIconChangedTemprary(iconid)
	targetIcon = iconid;
	Util.SetUITexture(icon,LuaConst.Const.ClubIcon,targetIcon.."_1", true);
end

function OnNameChanged()
	UIHelper.SetInputText(nameInput,Role.Get_name());
	if(delegateOnChangeName~=nil)then
		delegateOnChangeName();
	end
end

function Close()
	windowComponent:Close();
end

function OnDestroy()
	buttonClose = nil;
	buttonTreatFriend = nil;
	buttonHonorRecord = nil;
	buttonSystemSettings = nil;
	icon = nil;
	buttonIcon = nil;
	buttonChangeIcon = nil;
	nameInput = nil;
	buttonRandomName = nil;
	buttonChangeName = nil;
	code = nil;
	buttonExchangeCode = nil;
	targetName = nil;
	targetIcon = nil;
	delegateOnChangeName = nil;
	delegateOnChangeIcon = nil;
	window = nil;
	windowcomponent = nil;
    Role.UnRegisterNameAndIconChanged(OnIconChanged, OnNameChanged);
end