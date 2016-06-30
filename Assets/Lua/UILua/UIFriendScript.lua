--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion

module("UIFriendScript", package.seeall)

require "Common/UnityCommonScript"
require "UILua/UIFriendList"
require "UILua/UIFriendPower"
require "UILua/UIFriendMatch"
require "UILua/UIFriendApply"
require "UILua/UIFriendRecommend"

local window = nil;
local windowComponent = nil;
local windowFriendList = nil;
local windowFriendPower = nil;
local windowFriendMatch = nil;
local windowFriendApply = nil;

local btn_friendList = nil;
local btn_friendPower = nil;
local btn_friendMatch = nil;
local btn_friendApply = nil;
local m_friendListGrid = nil;
local m_friendListScrollView = nil;
local m_friendPowerGrid = nil;
local m_friendPowerScrollView = nil;
local m_friendMatchGrid = nil;
local m_friendApplyGrid = nil;
local m_friendApplyScrollView = nil;
local m_friendListItem = nil;
local m_friendPowerItem = nil;
local m_friendMatchItem = nil;
local m_friendApplyItem = nil;

local m_bInstanceFriendList = nil;
local m_bInstanceFriendPower = nil;
local m_bInstanceFriendMatch = nil;
local m_bInstanceFriendApply = nil;

local m_friendListDataTb = {};
local m_friendRecoListDataTb = {};
local m_friendPowerListDataTb = nil;
local m_friendApplyListDataTb = nil;
local lbl_tfCheckTb = {};

local enmu_FriendStatus = {List=1,Power=2,PK=3,Apply=4};
local m_currFriendStatus = nil;
local b_updateList = nil;
function OnStart(gameObject, params)
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");
    m_bInstanceFriendList = true;
    m_bInstanceFriendPower = true;
    m_bInstanceFriendMatch = true;
    m_bInstanceFriendApply = true;
    b_updateList = true;
    m_friendListDataTb = params;

    BindUI();

end

function BindUI()
    local transform = window.transform;

    m_friendListItem = windowComponent:GetPrefab(LuaConst.Const.FriendListItem).transform;
    m_friendPowerItem = windowComponent:GetPrefab(LuaConst.Const.FriendPowerItem).transform;
    m_friendMatchItem = windowComponent:GetPrefab(LuaConst.Const.FriendMatchItem).transform;
    m_friendApplyItem = windowComponent:GetPrefab(LuaConst.Const.FriendApplyItem).transform;

    windowFriendList = TransformFindChild(transform, "CenterPanel/FriendList");
    windowFriendPower = TransformFindChild(transform, "CenterPanel/FriendPower");
    windowFriendMatch = TransformFindChild(transform, "CenterPanel/FriendMatch");
    windowFriendApply = TransformFindChild(transform, "CenterPanel/FriendApply");
    m_friendListScrollView = TransformFindChild(transform, "CenterPanel/FriendList/ScrollViewPanel");
    m_friendListGrid = TransformFindChild(transform, "CenterPanel/FriendList/ScrollViewPanel/Grid");
    m_friendPowerGrid = TransformFindChild(transform, "CenterPanel/FriendPower/ScrollViewPanel/Grid");
    m_friendPowerScrollView = TransformFindChild(transform, "CenterPanel/FriendPower/ScrollViewPanel");
    m_friendMatchGrid = TransformFindChild(transform, "CenterPanel/FriendMatch/ScrollViewPanel/Grid");
    m_friendApplyGrid = TransformFindChild(transform, "CenterPanel/FriendApply/ScrollViewPanel/Grid");
    m_friendApplyScrollView = TransformFindChild(transform, "CenterPanel/FriendApply/ScrollViewPanel");
    lbl_tfCheckTb = {};
    table.insert(lbl_tfCheckTb,TransformFindChild(transform, "CenterPanel/Toggle/FriendList/Label"));
    table.insert(lbl_tfCheckTb,TransformFindChild(transform, "CenterPanel/Toggle/FriendPower/Label"));
    table.insert(lbl_tfCheckTb,TransformFindChild(transform, "CenterPanel/Toggle/FriendMatch/Label"));
    table.insert(lbl_tfCheckTb,TransformFindChild(transform, "CenterPanel/Toggle/FriendApply/Label"));

    btn_friendList = TransformFindChild(transform, "CenterPanel/Toggle/FriendList").gameObject;
    Util.AddClick(btn_friendList, BtnFriendList);
    btn_friendPower = TransformFindChild(transform, "CenterPanel/Toggle/FriendPower").gameObject;
    Util.AddClick(btn_friendPower, BtnFriendPower);
    btn_friendMatch = TransformFindChild(transform, "CenterPanel/Toggle/FriendMatch").gameObject;
    Util.AddClick(btn_friendMatch, BtnFriendMatch);
    btn_friendApply = TransformFindChild(transform, "CenterPanel/Toggle/FriendApply").gameObject;
    Util.AddClick(btn_friendApply, BtnFriendApply);
    local btnLeftReturn = TransformFindChild(transform, "TopPanel/TopLeft/BtnLeftReturn").gameObject;
    Util.AddClick(btnLeftReturn, BtnLeftReturn);

    BtnFriendList();

end

function TryOpenFriendList()
    GameObjectSetActive(windowFriendList,true);

    if b_updateList then
        b_updateList = false;
        if m_bInstanceFriendList then
            m_bInstanceFriendList = false;
            UIFriendList.InitFriendListUI(windowFriendList,m_friendListScrollView,m_friendListGrid,m_friendListItem,m_friendListDataTb)
        else
            UIFriendList.InitFriendListData(m_friendListDataTb);
        end
    end

end

function TryOpenFriendPower()
    GameObjectSetActive(windowFriendPower,true);

    if m_friendPowerListDataTb == nil then
        local OnPower = function(data_)
             m_friendPowerListDataTb = data_;

             if m_bInstanceFriendPower then
                m_bInstanceFriendPower = false;
                UIFriendPower.InitFriendPowerUI(windowFriendPower,m_friendPowerScrollView,m_friendPowerGrid,m_friendPowerItem,data_)
            else
                UIFriendPower.InitFriendPowerData(data_);
            end
        end;

        FriendData.ReqFriendPowerList(OnPower,1,50);
    end

end

function TryOpenFriendMatch()
--    GameObjectSetActive(windowFriendMatch,true);

--    if m_bInstanceFriendMatch then
--        m_bInstanceFriendMatch = false;
--        UIFriendMatch.InitFriendMatchUI(m_friendMatchGrid,m_friendMatchItem,4)
--    else
--        UIFriendMatch.InitFriendMatchData();
--    end

end

function TryOpenFriendApply()
    GameObjectSetActive(windowFriendApply,true);

    if m_friendApplyListDataTb == nil then
        local OnFriendApply = function (args_)
            m_friendApplyListDataTb = args_;

            if m_bInstanceFriendApply then
                UIFriendApply.InitFriendApplyUI(m_friendApplyScrollView,m_friendApplyGrid,m_friendApplyItem,args_)
            else
                UIFriendApply.InitFriendApplyData(args_);
            end
       end;

        FriendData.ReqFriendApply(OnFriendApply,1,50);
    end

end

function TryOpenFriendRecommend()
    local OnRecoList = function(data_)
        local tb = {};
        tb.tb = data_;
        WindowMgr.ShowWindow(LuaConst.Const.UIFriendRecommend,tb);
    end;

    FriendData.ReqFriendRecommend(OnRecoList);
end

function TryCloseFriendList()
    GameObjectSetActive(windowFriendList,false)

end

function TryCloseFriendPower()
    GameObjectSetActive(windowFriendPower,false)

end

function TryCloseFriendMatch()
    GameObjectSetActive(windowFriendMatch,false)

end

function TryCloseFriendApply()
    GameObjectSetActive(windowFriendApply,false)

end

function BtnFriendList()
    if m_currFriendStatus == enmu_FriendStatus.List then
        return;
     end 
    TryOpenFriendList();
    TryCloseFriendPower();
    TryCloseFriendApply();
    TryCloseFriendMatch();
    SetCheckColor(1);
    m_currFriendStatus = enmu_FriendStatus.List;
end

function BtnFriendPower()
    if m_currFriendStatus == enmu_FriendStatus.Power then
        return;
    end
    TryOpenFriendPower();
    TryCloseFriendList();
    TryCloseFriendApply();
    TryCloseFriendMatch();
    SetCheckColor(2);
    m_currFriendStatus = enmu_FriendStatus.Power;
end

function BtnFriendMatch()
    if m_currFriendStatus == enmu_FriendStatus.PK then
        return;
    end
    TryOpenFriendMatch()
    TryCloseFriendPower();
    TryCloseFriendApply();
    TryCloseFriendList();
    SetCheckColor(3);
    m_currFriendStatus = enmu_FriendStatus.PK
end

function BtnFriendApply()
    if m_currFriendStatus == enmu_FriendStatus.Apply then
        return;
    end
    TryOpenFriendApply();
    TryCloseFriendPower();
    TryCloseFriendList();
    TryCloseFriendMatch();
    SetCheckColor(4);
    m_currFriendStatus = enmu_FriendStatus.Apply;
end

function SetCheckColor(index_)
    for i=1,#lbl_tfCheckTb do
        if i == index_ then
            UIHelper.SetWidgetColor(lbl_tfCheckTb[i],Color.New(1,1,1,1));
        else
            UIHelper.SetWidgetColor(lbl_tfCheckTb[i],Color.New(171/255,173/255,185/255,1));
        end
    end

end

function GetWindowComponent(args)
    return windowComponent;
end

function GetFriendListDataTb()
    return m_friendListDataTb;
end

function SetFriendListDataTb(data_)
    b_updateList = true;
    m_friendListDataTb = data_;
end

function GetRecoListDataTb()
    return m_friendRecoListDataTb;
end

function GetPowerListDataTb()
    return m_friendPowerListDataTb;
end

function GetApplyListDataTb()
    return m_friendApplyListDataTb;
end

function TryOpen()
    if window ~= nil then
        GameObjectSetActive(window.transform,true);
    end

end

function TryClose()
    if window ~= nil then
        GameObjectSetActive(window.transform,false);
    end
end

function BtnLeftReturn()
    ExitFriend();
end

function OnDestroy()
    window = nil;
    windowComponent = nil;
    m_currFriendStatus = nil
    m_friendPowerListDataTb = nil;
    m_friendApplyListDataTb = nil;

end

function ExitFriend()
   windowComponent:Close();

end
















