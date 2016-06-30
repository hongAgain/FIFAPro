--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion

module("UIFriendList", package.seeall)

require "UILua/UIGameMail"
require "UILua/UIFriendTips"

local m_scrollViewList;
local m_gridList = nil;
local m_gridItemList;

local lbl_viewName = nil;
local lbl_viewLv = nil;
local lbl_viewVip = nil;
local spr_viewIcon = nil;
local lbl_friendNum = nil;
local ipt_id = nil;
local m_tfFriendListTb = {};
local m_tfFriendRecommendTb = {};
local m_dataFriendListTb = {};
local m_dataFrieRecoTb = {};

local m_viewIndex = nil;
function InitFriendListUI(tf_,scrollView_,grid_,gridItem_,data_)
    m_dataFriendListTb = data_;
    m_viewIndex = 1;

    m_scrollViewList = scrollView_;
    m_gridList = grid_;
    m_gridItemList = gridItem_;
    lbl_friendNum = TransformFindChild(tf_, "TopPart/FriendNum/Label");
    local btnFriendRecommend = TransformFindChild(tf_, "TopPart/BtnFriendRecommend").gameObject;
    Util.AddClick(btnFriendRecommend, BtnFriendRecommend);
    local btnFriendAdd = TransformFindChild(tf_, "TopPart/BtnFriendAdd").gameObject;
    Util.AddClick(btnFriendAdd, BtnFriendAdd);


    InitFriendListData(m_dataFriendListTb);
end

function InitFriendListData(data_)
    m_dataFriendListTb = data_;

    local  size = 0;
    if m_dataFriendListTb['list'] ~= nil then
        for k,v in pairs(m_dataFriendListTb['list']) do
            size = size + 1;
        end
    end
    UIHelper.DestroyGrid(m_gridList);

    local str = string.format(Util.LocalizeString("UIFriend_FriendLimit"),data_['total'].."/"..(data_['maxNum'] or 100));
    UIHelper.SetLabelTxt(lbl_friendNum,str);

    m_tfFriendListTb = {};
    for i=1,size do
        local tf = InstantiatePrefab(m_gridItemList,m_gridList).transform;
        tf.name = i;
        table.insert(m_tfFriendListTb,tf);
        if math.mod(i,2) == 0 then
            GameObjectSetActive(TransformFindChild(tf,"Bg"),false);
        end
        local icon = TransformFindChild(tf,"Head/Icon/Icon");
        Util.SetUITexture(icon,LuaConst.Const.ClubIcon,m_dataFriendListTb['list'][i].icon, true);
        local name = TransformFindChild(tf,"Head/lbl_name");
        UIHelper.SetLabelTxt(name,m_dataFriendListTb['list'][i].name);
        local vip = TransformFindChild(tf,"Head/lbl_vip");
        UIHelper.SetLabelTxt(vip,"vip "..m_dataFriendListTb['list'][i].vip);
        local lv = TransformFindChild(tf,"Head/Lv/lbl_lv");
        UIHelper.SetLabelTxt(lv,"Lv.".. m_dataFriendListTb['list'][i].lv);
        local status = TransformFindChild(tf,"lbl_status");
        UIHelper.SetLabelTxt(status,string.format(Util.LocalizeString("UIFriend_LastLogin"),Util.GetTimeToString(m_dataFriendListTb['list'][i].time)));
        local fightForce = TransformFindChild(tf,"FightForce/Label");
        UIHelper.SetLabelTxt(fightForce,string.format(Util.LocalizeString("UIPlayerList_BattleValue"),m_dataFriendListTb['list'][i].fc));
        local btnView = TransformFindChild(tf,"BtnView").gameObject;
        Util.AddClick(btnView, BtnView);
        btnView.name = i;
        local btnGivePower = TransformFindChild(tf,"BtnGivePower");
        Util.AddClick(btnGivePower.gameObject, BtnGivePower);
        local  btnGivePowered = TransformFindChild(tf,"BtnGivePowered");
        if IsGivePowerCD(m_dataFriendListTb['list'][i].lp) then
            GameObjectSetActive(btnGivePower,false);
            GameObjectSetActive(btnGivePowered,true);
        else
            GameObjectSetActive(btnGivePower,true);
            GameObjectSetActive(btnGivePowered,false);
        end
        btnGivePower.name = i;
    end

    UIHelper.RepositionGrid(m_gridList,m_scrollViewList);
    UIHelper.RefreshPanel(m_scrollViewList);
end

function BtnView(args_)
    local tb = {};
    tb.data = m_dataFriendListTb['list'][tonumber(args_.name)];
    m_viewIndex = tonumber(args_.name);
    WindowMgr.ShowWindow(LuaConst.Const.UIFriendView,tb);

end

function BtnGivePower(args_)
    local OnGivePower = function(data_)
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("UIFriend_GivePowerSuccess") });
        GameObjectSetActive(args_.transform,false);
        GameObjectSetActive(TransformFindChild(args_.transform.parent,"BtnGivePowered"),true);
        m_dataFriendListTb['list'][tonumber(args_.name)].lp = data_['lp'];
    end;

    FriendData.ReqFriendGivePower(OnGivePower,m_dataFriendListTb['list'][tonumber(args_.name)].fid);
end

function BtnFriendRecommend()
    local OnRecoList = function(data_)
        local tb = {};
        tb.tb = data_;
        WindowMgr.ShowWindow(LuaConst.Const.UIFriendRecommend,tb);
    end;

    FriendData.ReqFriendRecommend(OnRecoList);
end

function BtnFriendAdd()
    local tb = {};
    tb.TipsType = UIFriendTips.enum_FriendTips.AddFriend;
    WindowMgr.ShowWindow(LuaConst.Const.UIFriendTips,tb);

end

function DeleteFriendList()
    GameObjectDestroyImmediate(m_tfFriendListTb[m_viewIndex].gameObject);
    UIHelper.RepositionGrid(m_gridList);

end

function IsGivePowerCD(time_)
    local sec = Util.GetTotalSeconds(time_);
    local nextSec = 3600*Config.GetTemplate(Config.BaseTable())["gpCD"];
    if (nextSec - sec) > 0 then
        return true;
    end

    return false;
end
