--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion


module("UIFriendRecommend", package.seeall)

require "Common/LuaTimer"

local window = nil;
local windowComponent = nil;

local m_gridReco = nil;
local m_scrollViewReco = nil;
local m_gridItemReco = nil;
local lbl_refreshCD = nil;
local bg = nil
local m_lastSysTime = nil;
local m_lastTimeCD = nil;
local m_timerId = nil;

local m_dataFrieRecoTb = {};
local m_tfFriendRecommendTb = {};

local IntervalCD = 5*60;
function OnStart(gameObject, params)
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");

    BindUI();

    InitFriendRecommendUI(params.tb);
end

function BindUI()
    local transform = window.transform;

    m_gridItemReco = windowComponent:GetPrefab(LuaConst.Const.FriendRecommendItem).transform;
    m_gridReco = TransformFindChild(transform, "CenterPanel/ScrollViewPanel/Grid");
    m_scrollViewReco = TransformFindChild(transform, "CenterPanel/ScrollViewPanel");
    lbl_refreshCD = TransformFindChild(transform, "CenterPanel/Bottom/Label");
    bg = TransformFindChild(transform, "Bg")
    Util.AddClick(bg.gameObject, BtnClose)
    local btnRefreshCD = TransformFindChild(transform, "CenterPanel/Bottom/BtnRefresh").gameObject;
    Util.AddClick(btnRefreshCD, BtnRefreshCD);
    local btnClose = TransformFindChild(transform, "CenterPanel/BtnClose").gameObject;
    Util.AddClick(btnClose, BtnClose);

    m_lastSysTime = Util.GetString("RecFriendSysTime");
    m_lastTimeCD = Util.GetInt("RecFriendCD");

    if tonumber(m_lastSysTime) == nil then
        m_lastTimeCD = m_lastTimeCD - Util.GetTotalSeconds(0);
    else
        m_lastTimeCD = m_lastTimeCD - Util.GetTotalSeconds(tonumber(m_lastSysTime));
    end
    
    if m_lastTimeCD > IntervalCD then
        m_lastTimeCD = IntervalCD;
    end
    m_timerId = LuaTimer.AddTimer(false,-1,UpdateRefreshCD);
end

function InitFriendRecommendUI(data_)
    m_dataFrieRecoTb = data_;

    local size = 0;
    for k,v in pairs(data_) do
        size = size + 1;
    end
    UIHelper.DestroyGrid(m_gridReco);

    m_tfFriendRecommendTb = {};
    for i=1,size do
        local tf = InstantiatePrefab(m_gridItemReco,m_gridReco).transform;
        tf.name = i;
        table.insert(m_tfFriendRecommendTb,tf);
        if math.mod(i,2) == 0 then
            GameObjectSetActive(TransformFindChild(tf,"Bg"),false);
        end
        local icon = TransformFindChild(tf,"Head/Icon/Icon");
        if data_[i].icon ~= nil then
            Util.SetUITexture(icon,LuaConst.Const.ClubIcon,data_[i].icon, true);
        end
        local name = TransformFindChild(tf,"Head/lbl_name");
        UIHelper.SetLabelTxt(name,data_[i].name);
        local vip = TransformFindChild(tf,"Head/lbl_vip");
        UIHelper.SetLabelTxt(vip,"vip".. data_[i].vip);
        local lv = TransformFindChild(tf,"Head/Lv/lbl_lv");
        UIHelper.SetLabelTxt(lv,"Lv.".. data_[i].lv);
        local status = TransformFindChild(tf,"lbl_status");
        UIHelper.SetLabelTxt(status,string.format(Util.LocalizeString("UIFriend_LastLogin"),Util.GetTimeToString(data_[i].time)));
        local fightForce = TransformFindChild(tf,"FightForce/Label");
        UIHelper.SetLabelTxt(fightForce,string.format(Util.LocalizeString("UIPlayerList_BattleValue"),data_[i].fc));

        local btnSure = TransformFindChild(tf,"BtnSure").gameObject;
        Util.AddClick(btnSure, BtnSureReco);
        btnSure.name = i;
    end

    UIHelper.RepositionGrid(m_gridReco,m_scrollViewReco);
    UIHelper.RefreshPanel(m_scrollViewReco);
end

function BtnSureReco(args_)
    local OnAdd = function(args)
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("UIFriend_FriendAddTips") });
        GameObjectDestroyImmediate(args_.transform.parent.gameObject);
        UIHelper.RepositionGrid(m_gridReco);
    end;
    FriendData.ReqFriendAdd(OnAdd,m_dataFrieRecoTb[tonumber(args_.name)]._id);
end

function BtnRefreshCD()
    if m_lastTimeCD < 1 then
       local OnRecoList = function(data_)
            InitFriendRecommendUI(data_);
            m_lastTimeCD = IntervalCD;
        end;

        FriendData.ReqFriendRecommend(OnRecoList);
    else
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("UIFriend_RefreshCD") });
    end

end

function UpdateRefreshCD()
    if m_lastTimeCD > 1 then
        m_lastTimeCD = m_lastTimeCD - 1;
        UIHelper.SetLabelTxt(lbl_refreshCD,GetStrTime(m_lastTimeCD));
    else
        m_lastTimeCD = 0;
        UIHelper.SetLabelTxt(lbl_refreshCD," ");
    end

    
end

function GetStrTime(second_)
    local minute = math.floor(second_/60);
    local second = second_ - minute*60;

    return string.format("%02d",minute).." : "..string.format("%02d",second);
end

function BtnClose()
    ExitUIFriendRecommend();
end

function OnDestroy()
    window = nil;
    windowComponent = nil;

    LuaTimer.RmvTimer(m_timerId);
    Util.SetInt("RecFriendCD",m_lastTimeCD);
    Util.SetString("RecFriendSysTime",tostring(Util.GetTime()));
end


function ExitUIFriendRecommend()
   windowComponent:Close();

end


