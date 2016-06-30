--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion

module("UIFriendMatch", package.seeall)


local m_tfFriendTb = {};
function InitFriendMatchUI(grid_,gridItem_,size_)
    UIHelper.DestroyGrid(grid_);
    UIHelper.ReInstantiateGrid(grid_,gridItem_,size_,false)

    for i=0,size_-1 do
        local tf = TransformFindChild(grid_,i);
        table.insert(m_tfFriendTb,tf);
        local icon = TransformFindChild(grid_,i.."/Head/Icon/spr_icon");
--        UIHelper.SetSpriteName(icon,data_['list'][tonumber(i+1)].icon);
        local name = TransformFindChild(grid_,i.."/Head/lbl_name");
        UIHelper.SetLabelTxt(name,"Name");
        local vip = TransformFindChild(grid_,i.."/Head/lbl_vip");
        UIHelper.SetLabelTxt(vip,"");
        local lv = TransformFindChild(grid_,i.."/Head/Lv/lbl_lv");
        UIHelper.SetLabelTxt(lv,"Lv.".. "-1");
        local status = TransformFindChild(grid_,i.."/lbl_status");
        UIHelper.SetLabelTxt(status,"Login Time");
        local result = TransformFindChild(grid_,i.."/lbl_result");
        UIHelper.SetLabelTxt(result,"Success");
        local fightForce = TransformFindChild(grid_,i.."/FightForce/Label");
        UIHelper.SetLabelTxt(fightForce,string.format(Util.LocalizeString("UIPlayerList_BattleValue"),-1));
        local btnReplay = TransformFindChild(grid_,i.."/BtnReplay").gameObject;
        Util.AddClick(btnReplay, BtnReplay);
        btnReplay.name = i;
    end


end

function InitFriendMatchData()

end



function BtnReplay(args_)
    print(args_.name);

end







