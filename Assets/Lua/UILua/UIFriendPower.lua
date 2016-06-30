--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion

module("UIFriendPower", package.seeall)


local lbl_powerNum = nil;
local btn_getAllPower = nil;

local m_gridPower = nil;
local m_scrollViewPower = nil;
local m_gridItemPower = nil;

local m_tfFriendTb = {};
local m_dataFriendPowerTb = {};
function InitFriendPowerUI(tf_,scrollView_,grid_,gridItem_,data_)
    m_dataFriendPowerTb = data_;
    m_gridPower = grid_;
    m_scrollViewPower = scrollView_;
    m_gridItemPower = gridItem_;

    btn_getAllPower = TransformFindChild(tf_, "TopPart/BtnGetAllPower").gameObject;
    Util.AddClick(btn_getAllPower, BtnGetAllPower);
    lbl_powerNum = TransformFindChild(tf_, "TopPart/PowerNum/Label");

    InitFriendPowerData(m_dataFriendPowerTb)
end

function InitFriendPowerData(data_)
    m_dataFriendPowerTb = data_;

    local  size = 0;
    for k,v in pairs(m_dataFriendPowerTb['list']) do
        size = size + 1;
    end


    UIHelper.DestroyGrid(m_gridPower);

    for i=1,size do
        local tf = InstantiatePrefab(m_gridItemPower,m_gridPower).transform;
        tf.name = i;
        table.insert(m_tfFriendTb,tf);
        if math.mod(i,2) == 0 then
            GameObjectSetActive(TransformFindChild(tf,"Bg"),false);
        end
        local icon = TransformFindChild(tf,"Head/Icon/Icon");
        Util.SetUITexture(icon,LuaConst.Const.ClubIcon,m_dataFriendPowerTb['list'][i].icon, true);
        local name = TransformFindChild(tf,"Head/lbl_name");
        UIHelper.SetLabelTxt(name,m_dataFriendPowerTb['list'][i].name);
        local vip = TransformFindChild(tf,"Head/lbl_vip");
        UIHelper.SetLabelTxt(vip,"vip ".. m_dataFriendPowerTb['list'][i].vip);
        local lv = TransformFindChild(tf,"Head/Lv/lbl_lv");
        UIHelper.SetLabelTxt(lv,"Lv.".. m_dataFriendPowerTb['list'][i].lv);
        local status = TransformFindChild(tf,"lbl_status");
        UIHelper.SetLabelTxt(status,string.format(Util.LocalizeString("UIFriend_LastLogin"),Util.GetTimeToString(m_dataFriendPowerTb['list'][i].time)));
        local fightForce = TransformFindChild(tf,"FightForce/Label");
        UIHelper.SetLabelTxt(fightForce,string.format(Util.LocalizeString("UIPlayerList_BattleValue"),m_dataFriendPowerTb['list'][i].fc));
        local btnGetPower = TransformFindChild(tf,"BtnGetPower").gameObject;
        Util.AddClick(btnGetPower, BtnGetPower);
        btnGetPower.name = i;
    end

    UIHelper.RepositionGrid(m_gridPower,m_scrollViewPower);
    UIHelper.RefreshPanel(m_scrollViewPower);
    UpdatePowerData();
end

function UpdatePowerData()
    local str = string.format(Util.LocalizeString("UIFriend_TodayGetPower"),m_dataFriendPowerTb['today']);
    UIHelper.SetLabelTxt(lbl_powerNum,str);

end

function BtnGetPower(args_)
    if not IsCanGetPower() then
        return;
    end

    local  id = m_dataFriendPowerTb['list'][tonumber(args_.name)].uid;
    local OnGetPower = function(data_)
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("UIFriend_GetSuccess") });

        m_dataFriendPowerTb['today'] = m_dataFriendPowerTb['today'] -1;
        m_dataFriendPowerTb['list'][tonumber(args_.name)] = nil;
        UpdatePowerData();

        GameObjectDestroyImmediate(args_.transform.parent.gameObject);
        UIHelper.RepositionGrid(m_gridPower);
    end;

    FriendData.ReqFriendGetPower(OnGetPower,id);
end

function BtnGetAllPower(args_)
    if not IsCanGetPower() then
        return;
    end

    local OnGetAllPower = function(data_)
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("UIFriend_GetSuccess") });
        UIHelper.EnableBtn(args_,false);

        UIHelper.DestroyGrid(m_gridPower);
        local size = 0;
        for k,v in pairs(m_dataFriendPowerTb['list']) do
            size = size + 1;
        end

        m_dataFriendPowerTb['today'] = m_dataFriendPowerTb['today'] -size;
        m_dataFriendPowerTb['list'] = nil;
        UpdatePowerData();
    end;

    FriendData.ReqFriendGetAllPower(OnGetAllPower);

end

function IsCanGetPower()
    if tonumber(m_dataFriendPowerTb['today']) <= 0 then
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("UIFriend_MaxGetPowerTimes") });
        return false;
    end

    if m_dataFriendPowerTb['list'] ~= nil then
        local  size = 0;
        for k,v in pairs(m_dataFriendPowerTb['list']) do
            size = size + 1;
        end

        if size == 0 then
            WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("UIFriend_NoGetPower") });
            return false;
        end
    else
       WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("UIFriend_NoGetPower") });
       return false;
    end

    return true;
end




