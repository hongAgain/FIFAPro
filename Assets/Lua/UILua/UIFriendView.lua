--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion


module("UIFriendView", package.seeall)
require "Game/FormationCMP"
require "Game/FriendData"

ParentType = 
{
    Friend = 1,
    RankList = 2
}

local window = nil;
local windowComponent = nil;
local windowFormHead = nil;
local curType = nil
local lbl_viewName = nil;
local lbl_viewLv = nil;
local lbl_viewVip = nil;
local lbl_battleValue = nil;
local lbl_formName = nil;
local tex_viewIcon = nil;
local tf_formRoot = nil;
local bg = nil
local m_friendViewTb = {};
local m_tfFormHeadTb = {};
function OnStart(gameObject, params)
    m_friendViewTb = params.data;
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");
    curType = params.type or ParentType.Friend
    BindUI();
--    WindowMgr.AdjustLayer();
end

function BindUI()
    local transform = window.transform;

    -- windowFormHead = windowComponent:GetPrefab("UIAvatar");
    windowFormHead = Util.GetGameObject("UIAvatar_Formation");

    tf_formRoot = TransformFindChild(transform, "CenterPanel/FriendView/Formation/FormRoot");

    lbl_formName = TransformFindChild(transform, "CenterPanel/FriendView/Formation/Label");
    lbl_battleValue = TransformFindChild(transform, "CenterPanel/FriendView/Head/lbl_battleValue");
    lbl_viewName = TransformFindChild(transform, "CenterPanel/FriendView/Head/lbl_name");
    lbl_viewVip = TransformFindChild(transform, "CenterPanel/FriendView/Head/lbl_vip");
    lbl_viewLv = TransformFindChild(transform, "CenterPanel/FriendView/Head/Lv/lbl_lv");
    tex_viewIcon = TransformFindChild(transform, "CenterPanel/FriendView/Head/Icon/Icon");
    bg = TransformFindChild(transform, "Bg")
    Util.AddClick(bg.gameObject, BtnClose)
    local btnClose = TransformFindChild(transform, "CenterPanel/BtnClose").gameObject;
    Util.AddClick(btnClose, BtnClose);
    local btnFriendPK = TransformFindChild(transform, "CenterPanel/FriendView/BtnPK").gameObject;
    Util.AddClick(btnFriendPK, BtnFriendPK);
    local btnFriendMail = TransformFindChild(transform, "CenterPanel/FriendView/BtnMail").gameObject;
    Util.AddClick(btnFriendMail, BtnFriendMail);
    local btnFriendGuild = TransformFindChild(transform, "CenterPanel/FriendView/BtnGuild").gameObject;
    Util.AddClick(btnFriendGuild, BtnFriendGuild);
    local btnFriendDel = TransformFindChild(transform, "CenterPanel/FriendView/BtnDel").gameObject;
    Util.AddClick(btnFriendDel, BtnFriendDel);
    local btnFriendAdd = TransformFindChild(transform, "CenterPanel/FriendView/BtnAdd").gameObject
    Util.AddClick(btnFriendAdd, BtnFriendAdd)

    if curType == ParentType.Friend then
        btnFriendPK:SetActive(true)
        btnFriendDel:SetActive(true)
        btnFriendAdd:SetActive(false)
    elseif curType == ParentType.RankList then
        btnFriendPK:SetActive(false)
        btnFriendDel:SetActive(false)
        btnFriendAdd:SetActive(true)
    end

    InitFriendViewData();
end

function InitFriendViewData()
    UIHelper.SetLabelTxt(lbl_viewName,m_friendViewTb.name);
    UIHelper.SetLabelTxt(lbl_viewLv,"Lv.".. m_friendViewTb.lv);
    UIHelper.SetLabelTxt(lbl_viewVip,"vip ".. m_friendViewTb.vip);
    UIHelper.SetLabelTxt(lbl_battleValue,string.format(Util.LocalizeString("UIFriend_BattleValue"),m_friendViewTb.fc));
    UIHelper.SetLabelTxt(lbl_formName,Config.GetTemplate(Config.positionTB)[tostring(m_friendViewTb.team.id)].name);
    Util.SetUITexture(tex_viewIcon,LuaConst.Const.ClubIcon,m_friendViewTb.icon, true);
    m_tfFormHeadTb = {};
    local size = #m_friendViewTb.team.hero; -- #teamIdTb
    for i = 1, size do
        local clone = AddChild(windowFormHead, tf_formRoot);
        UIHelper.AdjustDepth(clone, 10);
        clone.name = i;
        m_tfFormHeadTb[i] = clone.transform;
    end
    local iconTb = {};
    for i=1,size do
        table.insert(iconTb,HeroData.GetHeroIcon(m_friendViewTb.team.hero[i]));
    end

    local formationFriend = FormationCMP.New();
    formationFriend:InitFormation(tostring(m_friendViewTb.team.id),m_tfFormHeadTb,FormationCMP.enum_CMPType.Type1,iconTb);
end


function BtnFriendPK(args)
    local OnPK = function(args_)
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("UIFriend_FriendPKSuccess") });
    end;

    WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { "等待战斗系统完善后开放！" });
--    FriendData.ReqFriendPK(OnPK,m_friendViewTb.fid);
end

function BtnFriendMail(args)
    WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { "暂未开发，敬请期待！" });
--    local AfterRequesting = function ()
--        local tb = {};
--        tb.enterType = UIGameMail.enumMailEnterType.Friend;
--        UIFriendScript.TryClose();
--        WindowMgr.ShowWindow(LuaConst.Const.UIGameMail,tb);
--    end    
--    GameMailData.RequestGameMailList(nil,AfterRequesting);

end

function BtnFriendDel(args)
    local OnDel = function(data_)
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("UIFriend_FriendDelSuccess") });     

        local OnReqFriendList = function (dataList)
            UIFriendScript.SetFriendListDataTb(dataList);
            UIFriendList.InitFriendListData(dataList);
            ExitUIFriendView();
        end
    
        FriendData.ReqFriendList(OnReqFriendList,1,50);
    end;

    local MsgBoxSure = function()
        FriendData.ReqFriendDel(OnDel,m_friendViewTb.fid);
    end;
    

    local tipsMsg = string.format(Util.LocalizeString("UIFriend_DeleteTips"),m_friendViewTb.name);
    WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { tipsMsg,false, MsgBoxSure});
end

function BtnFriendAdd()
    local onFriendAdd = function( data )
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("UIFriend_FriendAddTips") })
    end
    FriendData.ReqFriendAdd(onFriendAdd,nil,m_friendViewTb.name)
end

function BtnFriendGuild(args)
    WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { "暂未开放，敬请期待" });
end

function BtnClose()
    ExitUIFriendView();
end

function OnDestroy()
    window = nil;
    windowComponent = nil;

end



function ExitUIFriendView()
   windowComponent:Close();

end



