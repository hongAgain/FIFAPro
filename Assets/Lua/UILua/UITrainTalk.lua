--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion


module("UITrainTalk", package.seeall)

require "UILua/UIMsgBoxConfirm"
require "UILua/UIGetItemScript"

local window = nil;
local windowComponent = nil;
local windowTrainTalk = nil;
local windowTrainTalkFx = nil;

local lbl_content = nil;
local lbl_contentFx = nil;
local lbl_trainTick = nil;
local spr_status = nil;
local spr_outCircle = nil;
local btn_sure = nil;
local btn_trainTalk = nil;

local m_playerId = nil;
local m_teData = nil;
local m_timerId = nil;
local m_value = nil;
local Callback = nil;
function OnStart(gameObject, params)
    window = gameObject;
    m_playerId = params.playerId;
    Callback = params.callback;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");

    BindUI();
end

function BindUI()
    local transform = window.transform;
    windowTrainTalk = AddChild(windowComponent:GetPrefab("TrainTalk"),transform).transform;
    windowTrainTalk.localPosition = Vector3.zero;

    lbl_content = TransformFindChild(windowTrainTalk, "Content");
    lbl_trainTick = TransformFindChild(windowTrainTalk, "TrainTick/Label");
    spr_status = TransformFindChild(windowTrainTalk, "Status")


    UIHelper.SetLabelTxt(TransformFindChild(windowTrainTalk, "Name"),Hero.ColorHeroName(Hero.GetCurrQuality(m_playerId),HeroData.GetHeroName(m_playerId)));
    Util.SetUITexture(TransformFindChild(windowTrainTalk, "Icon"),LuaConst.Const.PlayerHeadIcon,HeroData.GetHeroIcon(m_playerId), false);

    btn_sure = TransformFindChild(windowTrainTalk, "BtnSure");
    Util.AddClick(btn_sure.gameObject, BtnSure);
    btn_trainTalk = TransformFindChild(windowTrainTalk, "BtnTrainTalk");
    Util.AddClick(btn_trainTalk.gameObject, BtnTrainTalk);

    Util.AddClick(TransformFindChild(windowTrainTalk, "Close").gameObject, BtnClose);

    RefreshPlayerStatus();
end

function RefreshPlayerStatus()
    if Hero.GetHeroData2Id(m_playerId)['stat'] >= 5 then
        GameObjectSetActive(btn_sure,true);
        GameObjectSetActive(btn_trainTalk,false);
    else
        GameObjectSetActive(btn_sure,false);
        GameObjectSetActive(btn_trainTalk,true);
    end

    UIHelper.SetLabelTxt(lbl_trainTick,ItemSys.GetItemData(LuaConst.Const.TeachCard).num .."/999");
    local strBuff = Config.GetProperty(Config.HeroTeachTable(),tostring(Hero.GetHeroData2Id(m_playerId)['stat']),'buff').."%";
    UIHelper.SetLabelTxt(lbl_content,string.format(Util.LocalizeString("UIPlayerStatus_Status".. Hero.GetHeroData2Id(m_playerId)['stat']),HeroData.GetHeroName(m_playerId),strBuff));
    UIHelper.SetSpriteName(spr_status,Hero.GetHeroStatusName(m_playerId));
end

function BtnTrainTalk()
    local MsgBoxSure = function()
        print("MsgBoxSure");
    end;

    if ItemSys.GetItemData(LuaConst.Const.TeachCard).num < 1 then
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { Util.LocalizeString("UIPlayerStatus_NoTrainTick"),false, MsgBoxSure});
        return;
    end

    if windowTrainTalkFx == nil then
        windowTrainTalkFx = AddChild(windowComponent:GetPrefab("TrainTalkFx"),window.transform).transform;
        windowTrainTalkFx.localPosition = Vector3.zero;

        lbl_contentFx = TransformFindChild(windowTrainTalkFx, "Label");
        spr_outCircle = TransformFindChild(windowTrainTalkFx, "OutCircle");
    end

    local OnTe = function(data_)
        m_teData = data_;
        InitFx();
    end;

    Hero.ReqHeroTe(m_playerId,OnTe);
end

function InitFx()
    GameObjectSetActive(windowTrainTalkFx,true);
    GameObjectSetActive(windowTrainTalk,false);
    UIHelper.SetLabelTxt(lbl_contentFx, string.format( Util.LocalizeString("UIPlayerStatus_TalkContent"),HeroData.GetHeroName(m_playerId) ) );

    m_value = 0;
    m_timerId = LuaTimer.AddTimer(false,-0.1,PlayFx)

end

function PlayFx()
    m_value = m_value+0.1;
    UIHelper.SetSpriteFillAmount(spr_outCircle,m_value);
    if m_value >= 1 and m_timerId ~= nil then
        LuaTimer.RmvTimer(m_timerId);
        TrainResult();
    end

end

function TrainResult()
    RefreshPlayerStatus();
    GameObjectSetActive(windowTrainTalkFx,false);
    GameObjectSetActive(windowTrainTalk,true);

    local OnSure = function()
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("UIPlayerStatus_Success") });
    end;

    if m_teData['success'] == 1 then
        if m_teData['item'] == nil then
            OnSure();
        else
            WindowMgr.ShowWindow(LuaConst.Const.UIGetItem,{
                m_itemTb = m_teData,
                OnClose = OnSure
            });
        end
    else
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("UIPlayerStatus_Fail") });
    end

end

function BtnSure()
    BtnClose();
end

function BtnClose()
    ExitUITrainTalk();

end

function OnDestroy()
    window = nil;
    windowComponent = nil;
    windowTrainTalk = nil;
    windowTrainTalkFx = nil;
end

function ExitUITrainTalk()
   if Callback ~= nil then
      Callback();
      Callback = nil;
   end

   windowComponent:Close();
end

