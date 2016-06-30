--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion

module("UIFriendTips", package.seeall)

local window = nil;
local windowComponent = nil;
local windowFriendAdd = nil;

local ipt_id = nil;

local m_tfFriendAdd = nil;
local bg = nil

enum_FriendTips = {AddFriend=1};
local m_friendTipsType = nil;
function OnStart(gameObject, params)
    m_friendTipsType = params.TipsType;
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");

    BindUI();
    WindowMgr.AdjustLayer(); 
end

function BindUI()
    local transform = window.transform;

    windowFriendAdd = windowComponent:GetPrefab("FriendAdd");
    bg = TransformFindChild(transform, "Bg")
    Util.AddClick(bg.gameObject,BtnClose)
    if m_friendTipsType == enum_FriendTips.AddFriend then
        BindUIAddFriend();
    end

end

function BindUIAddFriend()
    m_tfFriendAdd = InstantiatePrefab(windowFriendAdd,window.transform).transform;

    ipt_id = TransformFindChild(m_tfFriendAdd, "Input");

    local btnSureAdd = TransformFindChild(m_tfFriendAdd, "BtnSure").gameObject;
    Util.AddClick(btnSureAdd, BtnSureAdd);
    local btnCancel = TransformFindChild(m_tfFriendAdd, "BtnCancel").gameObject;
    Util.AddClick(btnCancel, BtnCancel);
    local btnClose = TransformFindChild(m_tfFriendAdd, "BtnClose").gameObject;
    Util.AddClick(btnClose, BtnClose);

end

function BtnSureAdd()
    local id = UIHelper.InputTxt(ipt_id);
    print("FriendAdd id: ".. id.."/"..string.len(id));
    if string.len(id) == 0 then
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("UIFriend_AddError") });
        return;
    end

    if tostring(id) == Role.Get_name() or tostring(id) == Role.Get_uid() then
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("UIFriend_AddMeError") });
        return;
    end
    
    local OnFriendAdd = function (data_)
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("UIFriend_FriendAddTips") });
    end
    if string.len(id) == 24 then
        FriendData.ReqFriendAdd(OnFriendAdd,id,nil);
    else
        FriendData.ReqFriendAdd(OnFriendAdd,nil,id);
    end

    BtnClose();
end

function BtnCancel()
    BtnClose();
end

function BtnClose()
    ExitUIFriendTips();
end

function OnDestroy()
    window = nil;
    windowComponent = nil;

end



function ExitUIFriendTips()
   windowComponent:Close();

end


