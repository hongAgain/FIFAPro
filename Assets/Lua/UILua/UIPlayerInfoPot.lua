--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion

module("UIPlayerInfoPot", package.seeall)

require "Game/PlayerPotData"

local ThisTransform = nil;
local windowPotTarget = nil;

local m_potTarget1 = nil;
local m_potTarget11 = nil;
local m_potTarget2 = nil;

local attr1TxtTb = {};
local attr1NumTb = {};
function InitPlayerInfoPot(subTransform_)
    ThisTransform = subTransform_;

    attr1TxtTb = {};
    attr1NumTb = {};
    for i=1,6 do
        table.insert(attr1TxtTb,TransformFindChild(ThisTransform,i));
        table.insert(attr1NumTb,TransformFindChild(ThisTransform,i.."/Label"));
    end
    m_potTarget1 = TransformFindChild(ThisTransform, "PotTarget1");
    m_potTarget11 = TransformFindChild(ThisTransform, "PotTarget11");
    m_potTarget2 = TransformFindChild(ThisTransform, "PotTarget2");


    UIHelper.SetLabelTxt(TransformFindChild(ThisTransform, "Btn1/Num"),PlayerPotData.GetPotCost(PlayerPotData.enum_PotType.Normal,1));
    UIHelper.SetLabelTxt(TransformFindChild(ThisTransform, "Btn2/Num"),PlayerPotData.GetPotCost(PlayerPotData.enum_PotType.Rmb,1));
    UIHelper.SetLabelTxt(TransformFindChild(ThisTransform, "Btn3/Num"),PlayerPotData.GetPotCost(PlayerPotData.enum_PotType.King,1));

    Util.AddClick(TransformFindChild(ThisTransform, "Btn1").gameObject, BtnNormalPot);
    Util.AddClick(TransformFindChild(ThisTransform, "Btn2").gameObject, BtnRmbPot);
    Util.AddClick(TransformFindChild(ThisTransform, "Btn3").gameObject, BtnKingPot);
    Util.AddClick(TransformFindChild(ThisTransform, "Btn4").gameObject, BtnAutoPot);
    Util.AddClick(TransformFindChild(ThisTransform, "PotTarget1/LabelReward/Sprite").gameObject, BtnCurrReward);
    Util.AddClick(TransformFindChild(ThisTransform, "PotTarget11/LabelReward/Sprite").gameObject, BtnCurrReward);
    Util.AddClick(TransformFindChild(ThisTransform, "PotTarget2/LabelReward/Sprite").gameObject, BtnNextReward);

    RefreshPlayerInfo();
    RegisterMsgCallback();

    WindowMgr.AdjustLayer();
end

function RegisterMsgCallback()
    SynSys.RegisterCallback("hero",RefreshPlayerInfo);
end

function UnRegisterMsgCallback()
    SynSys.UnRegisterCallback("hero",RefreshPlayerInfo);
end

function RefreshPlayerInfo()
    local id = PlayerInfoData.GetCurrPlayerId();
    local potLv = PlayerPotData.GetTargetPotLv(id);
    for i=1,#attr1TxtTb do
        UIHelper.SetLabelTxt(attr1TxtTb[i],HeroData.GetAttr1Name(id,i));
        UIHelper.SetLabelTxt(attr1NumTb[i],HeroData.GetAttr1Pot(id,i).."/".. HeroData.GetAttr1PotLimit(id,i));
    end
    if potLv == 0 then
        potLv = 1;
        GameObjectSetActive(m_potTarget1,false);
        GameObjectSetActive(m_potTarget11,true);
        UIHelper.SetLabelTxt(TransformFindChild(m_potTarget11, "LabelName"),PlayerPotData.GetTargetDes(id,potLv));
        UIHelper.SetLabelTxt(TransformFindChild(m_potTarget11, "LabelCondition"),PlayerPotData.GetTargetNeedStr(id,potLv));
    else
        GameObjectSetActive(m_potTarget1,true);
        GameObjectSetActive(m_potTarget11,false);
        UIHelper.SetLabelTxt(TransformFindChild(m_potTarget1, "LabelName"),PlayerPotData.GetTargetDes(id,potLv));
        UIHelper.SetLabelTxt(TransformFindChild(m_potTarget1, "LabelCondition"),PlayerPotData.GetTargetNeedStr(id,potLv));
    end

    UIHelper.SetLabelTxt(TransformFindChild(m_potTarget2, "LabelName"),PlayerPotData.GetTargetDes(id,potLv+1));
    UIHelper.SetLabelTxt(TransformFindChild(m_potTarget2, "LabelCondition"),PlayerPotData.GetTargetNeedStr(id,potLv+1));

end

function BtnNormalPot()
    if PlayerPotData.IsEnoughMoney(PlayerPotData.enum_PotType.Normal,1) then
        NoAutoPot(PlayerPotData.enum_PotType.Normal);
    end
end

function BtnRmbPot()
    if PlayerPotData.IsEnoughMoney(PlayerPotData.enum_PotType.Rmb,1) then
        NoAutoPot(PlayerPotData.enum_PotType.Rmb);
    end
end

function BtnKingPot()
    if PlayerPotData.IsEnoughMoney(PlayerPotData.enum_PotType.King,1) then
        NoAutoPot(PlayerPotData.enum_PotType.King);
    end
end

function BtnAutoPot()
    WindowMgr.ShowWindow(LuaConst.Const.UIPlayerPot,
                        {PotData = nil,PotType = PlayerPotData.enum_PotType.AutoPot,PotId = PlayerInfoData.GetCurrPlayerId(),Callback = RefreshPlayerInfo}
                        );
end

function NoAutoPot(type_)
    local OnPot = function(data_)
        WindowMgr.ShowWindow(LuaConst.Const.UIPlayerPot,{PotData = data_,PotType = type_,PotId = PlayerInfoData.GetCurrPlayerId(),Callback = RefreshPlayerInfo});
    end;

    Hero.ReqHeroPot(PlayerInfoData.GetCurrPlayerId(),type_,OnPot);
end

function BtnCurrReward()
    local lv = PlayerPotData.GetTargetPotLv(PlayerInfoData.GetCurrPlayerId());
    if lv == 0 then
        lv = 1;
    end

    WindowMgr.ShowWindow(LuaConst.Const.UIPotTips,{PlayerId=PlayerInfoData.GetCurrPlayerId(),
                                                    PotType=PlayerPotData.enum_PotType.TargetReward,
                                                    PotLv=lv});
end

function BtnNextReward()
    local lv = PlayerPotData.GetTargetPotLv(PlayerInfoData.GetCurrPlayerId());
    if lv == 0 then
        lv = 1;
    end

    WindowMgr.ShowWindow(LuaConst.Const.UIPotTips,{PlayerId=PlayerInfoData.GetCurrPlayerId(),
                                            PotType=PlayerPotData.enum_PotType.TargetReward,
                                            PotLv=lv+1});
end

function OnShow()

end

function OnHide()


end

function OnDestroy()
    UnRegisterMsgCallback();
end
