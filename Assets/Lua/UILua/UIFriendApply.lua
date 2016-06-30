--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion

module("UIFriendApply", package.seeall)

local m_grid = nil;
local m_scrollView = nil;

local m_tfFriendTb = {};
local m_dataFriendApplyTb = {};
function InitFriendApplyUI(scrollView_,grid_,gridItem_,data_)
    m_dataFriendApplyTb = data_;
    m_grid = grid_;
    m_scrollView = scrollView_;
    local size = 0;
    for k,v in pairs(data_['list']) do
        size = size + 1;
    end
    
    UIHelper.DestroyGrid(grid_);

    for i=1,size do
        local tf = InstantiatePrefab(gridItem_,grid_).transform;
        tf.name = i;
        table.insert(m_tfFriendTb,tf);
        if math.mod(i,2) == 0 then
            GameObjectSetActive(TransformFindChild(tf,"Bg"),false);
        end
        local icon = TransformFindChild(tf,"Head/Icon/Icon");
        Util.SetUITexture(icon,LuaConst.Const.ClubIcon,data_['list'][i].icon, true)
        local name = TransformFindChild(tf,"Head/lbl_name");
        UIHelper.SetLabelTxt(name,data_['list'][i].name);
        local vip = TransformFindChild(tf,"Head/lbl_vip");
        UIHelper.SetLabelTxt(vip,"vip "..data_['list'][i].vip);
        local lv = TransformFindChild(tf,"Head/Lv/lbl_lv");
        UIHelper.SetLabelTxt(lv,"Lv.".. data_['list'][i].lv);
        local status = TransformFindChild(tf,"lbl_status");
        UIHelper.SetLabelTxt(status,string.format(Util.LocalizeString("UIFriend_LastLogin"),Util.GetTimeToString(data_['list'][i].time)));
        local fightForce = TransformFindChild(tf,"FightForce/Label");
        UIHelper.SetLabelTxt(fightForce,string.format(Util.LocalizeString("UIPlayerList_BattleValue"),data_['list'][i].fc));

        local btnRefuse = TransformFindChild(tf,"BtnRefuse").gameObject;
        Util.AddClick(btnRefuse, BtnRefuse);
        btnRefuse.name = i;
        local btnAccept = TransformFindChild(tf,"BtnAccept").gameObject;
        Util.AddClick(btnAccept, BtnAccept);
        btnAccept.name = i;
    end

    UIHelper.RepositionGrid(m_grid,m_scrollView);
    UIHelper.RefreshPanel(m_scrollView);
end

function InitFriendApplyData()

end



function BtnRefuse(args_)
    local OnRefuse = function(data_)
        GameObjectDestroyImmediate(args_.transform.parent.gameObject);
        UIHelper.RepositionGrid(m_grid);
    end;

    FriendData.ReqFriendRefuse(OnRefuse,m_dataFriendApplyTb['list'][tonumber(args_.name)].uid);
end

function BtnAccept(args_)
    local OnAccept = function (data_)
        GameObjectDestroyImmediate(args_.transform.parent.gameObject);
        UIHelper.RepositionGrid(m_grid);

        local OnReqFriendList = function (dataList)
            UIFriendScript.SetFriendListDataTb(dataList);
        end
    
        FriendData.ReqFriendList(OnReqFriendList,1,50);
    end;

    FriendData.ReqFriendAccept(OnAccept,m_dataFriendApplyTb['list'][tonumber(args_.name)].uid);
end






