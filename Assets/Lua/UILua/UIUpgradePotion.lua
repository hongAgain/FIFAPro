--region NewFile_1.lua
--Author : hongpeng.zhao
--Date   : 2015/3/28
--此文件由[BabeLua]插件自动生成



--endregion


module("UIUpgradePotion", package.seeall)

local ThisTransform = nil;

local m_lvLast = nil;
local m_expPrev = nil;
local m_expLast = nil;
local m_expFill = nil;
local m_lastFillLv = nil;
local m_lastFillAmount = nil;
local m_msgPotionCount = nil;
local m_fxTimerId = nil;

local m_expCard1 = nil;
local m_expCard2 = nil;
local m_expCard3 = nil;
local m_expCardCount1 = nil;
local m_expCardCount2 = nil;
local m_expCardCount3 = nil;
local m_timerId = nil;
local m_currItemType = nil;
local m_bSendUpgradeMsg = false;
local m_limitPlayerLv = nil;

local m_itemCountTb = {};
function InitUpgradPotion(subTransform_)
    ThisTransform = subTransform_;

    m_itemCountTb = {};
    for i=1,3 do
        local itemCount = TransformFindChild(ThisTransform, "BottomPart/Item"..i.."/Item/Label");
        local itemName = TransformFindChild(ThisTransform, "BottomPart/Item"..i.."/Item/LabelName");
        local itemIcon = TransformFindChild(ThisTransform, "BottomPart/Item"..i.."/Item/Icon");
        table.insert(m_itemCountTb,itemCount);
        SetExpCardNameIcon(itemName,itemIcon,i);
    end


    UIHelper.SetLabelTxt(TransformFindChild(ThisTransform,"TopPart/Label"),Util.LocalizeString("UIUpgrade_tipsContent"));

    Util.AddPress(TransformFindChild(ThisTransform,"BottomPart/Item1").gameObject,PressItem1);
    Util.AddPress(TransformFindChild(ThisTransform,"BottomPart/Item2").gameObject,PressItem2);
    Util.AddPress(TransformFindChild(ThisTransform,"BottomPart/Item3").gameObject, PressItem3);

    m_msgPotionCount = 0;
    m_limitPlayerLv = Config.GetProperty(Config.LevelTable(), tostring(Role.Get_lv()),"heroLv");
    m_lvLast =  UIPlayerInfoBaseScript.GetCurrHeroData()['lv'];
    m_lastFillLv = m_lvLast;
    m_expLast = UIPlayerInfoBaseScript.GetCurrHeroData()['exp'];
    m_expFill = m_expLast;
    m_lastFillAmount = PlayerInfoData.GetCurrFillAmount();


    m_expCard1 = Config.GetProperty(Config.ItemTable(),LuaConst.Const.ExpCard1,'useArgs');
    m_expCard2 = Config.GetProperty(Config.ItemTable(),LuaConst.Const.ExpCard2,'useArgs');
    m_expCard3 = Config.GetProperty(Config.ItemTable(),LuaConst.Const.ExpCard3,'useArgs');
    m_expCardCount1 = ItemSys.GetItemData(LuaConst.Const.ExpCard1).num;
    m_expCardCount2 = ItemSys.GetItemData(LuaConst.Const.ExpCard2).num;
    m_expCardCount3 = ItemSys.GetItemData(LuaConst.Const.ExpCard3).num;

--    RefreshUpgradeInfo();
    RefreshItemCount();
    RegisterMsgCallback();
end

function RegisterMsgCallback()
    SynSys.RegisterCallback("hero",RefreshUpgradeInfo);
end

function UnRegisterMsgCallback()
    SynSys.UnRegisterCallback("hero",RefreshUpgradeInfo);
end

function RefreshUpgradeInfo()
    RefreshItemCount();
end
function SetExpCardNameIcon(tfName_,tfIcon_,args_)
    if args_ == 1 then
        UIHelper.SetLabelTxt(tfName_,Config.GetProperty(Config.ItemTable(),LuaConst.Const.ExpCard1,'name'));
        Util.SetUITexture(tfIcon_,LuaConst.Const.ItemIcon,Config.GetProperty(Config.ItemTable(),LuaConst.Const.ExpCard1,'icon'), false);
    elseif args_ == 2 then
        UIHelper.SetLabelTxt(tfName_,Config.GetProperty(Config.ItemTable(),LuaConst.Const.ExpCard2,'name'));
        Util.SetUITexture(tfIcon_,LuaConst.Const.ItemIcon,Config.GetProperty(Config.ItemTable(),LuaConst.Const.ExpCard2,'icon'), false);
    elseif args_ == 3 then
        UIHelper.SetLabelTxt(tfName_,Config.GetProperty(Config.ItemTable(),LuaConst.Const.ExpCard3,'name'));
        Util.SetUITexture(tfIcon_,LuaConst.Const.ItemIcon,Config.GetProperty(Config.ItemTable(),LuaConst.Const.ExpCard3,'icon'), false);
    end
    
end

function RefreshItemCount()
    for i=1,3 do
        local count = GetExpPotionCount("Item"..i);
        if tonumber(count) > 999 then
            count = "999+";
        end
        UIHelper.SetLabelTxt(m_itemCountTb[i],"X".. count);
    end

end

function PressItem1(gameObject_, isPressed_)
    PressItem(gameObject_,isPressed_);
end
function PressItem2(gameObject_, isPressed_)
    PressItem(gameObject_,isPressed_);
end
function PressItem3(gameObject_, isPressed_)
    PressItem(gameObject_,isPressed_);
end

function PressItem(gameObject_, isPressed_)
    m_currItemType = gameObject_.name;
    if isPressed_ and not IsBeyondRoleLv() then
        if SustainUseItem() then
            m_timerId = LuaTimer.AddTimer(false,-0.2,SustainUseItem);
        end
    else
        if m_timerId ~= nil then
            LuaTimer.RmvTimer(m_timerId);
        end

        if m_bSendUpgradeMsg then 
            ReqUsePotion(gameObject_.name,m_msgPotionCount);
            m_msgPotionCount = 0;
            m_bSendUpgradeMsg = false;
        end
        CheckPlayerLv();
    end
end

function SustainUseItem()
    if not IsEnoughItem(m_currItemType) then 
        if IsValidClick(3) then
            WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("UIUpgrade_NoEnoughtExpCard") });
        end
        if m_msgPotionCount == 0 then
            m_bSendUpgradeMsg = false;
        end
        return false;
    end   

    if IsBeyondRoleLv() then
        if IsValidClick(3) then
            WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("UIUpgrade_NoBeyondRoleLv") });
        end
        if m_msgPotionCount == 0 then
            m_bSendUpgradeMsg = false;
        end
        return false;
    end
    m_bSendUpgradeMsg = true
    m_msgPotionCount = m_msgPotionCount + 1;
    MinusExpPotionCount(m_currItemType,1);
    m_expPrev = m_expLast;
    m_expLast = m_expLast + GetCardExp(m_currItemType);

    RefreshItemCount();
    UpdateHeroExpLv();
    return true
end

function UpdateHeroExpLv()
    if m_fxTimerId == nil then
        m_fxTimerId = LuaTimer.AddTimer(false,-0.02,UpgradeLvFx);
    end 
end

function ExpOffset()
    if m_lastFillLv < 2 then
        return UIPlayerInfoBaseScript.GetCurrHeroData()['exp'];
    else
        return Config.GetProperty(Config.LevelTable(), tostring(m_lastFillLv+1),"HExp") - Config.GetProperty(Config.LevelTable(), tostring(m_lastFillLv),"HExp");
    end
end

function UpgradeLvFx()
    if m_lastFillLv < PlayerInfoData.GetLv2Exp(m_expLast) and m_lastFillLv < m_limitPlayerLv then
        m_lastFillAmount = m_lastFillAmount + 0.04;
        if m_lastFillAmount >=1 then
            m_lastFillAmount = 0;
            m_lastFillLv = m_lastFillLv + 1;
        end
    elseif m_lastFillAmount < GetFxCurrFillAmount() then
        m_lastFillAmount = m_lastFillAmount + 0.01;
    else
        if m_fxTimerId ~= nil then
            LuaTimer.RmvTimer(m_fxTimerId);
            m_fxTimerId = nil;
            m_lastFillAmount = GetFxCurrFillAmount();
        end
    end

    UIPlayerInfoBase.SetUIExpLv(m_lastFillLv,m_lastFillAmount);
end

function GetFxCurrFillAmount()
    local lv = PlayerInfoData.GetLv2Exp(m_expLast);
    local exp1 = m_expLast - Config.GetProperty(Config.LevelTable(), tostring(lv),"HExp");
    if lv >= tonumber( Config.GetTemplate(Config.BaseTable())["mlv"] ) then
        return 0;
    elseif lv >= m_limitPlayerLv then
        return 1;
    else
        local exp2 = Config.GetProperty(Config.LevelTable(), tostring(lv+1),"HExp") - Config.GetProperty(Config.LevelTable(), tostring(lv),"HExp");
        return exp1/exp2;
    end
end

function ReqUsePotion(type_,count_)
    local itemId = nil;
    if type_ == "Item1" then 
        if count_ ~= nil and count_ > 0 then
           itemId = LuaConst.Const.ExpCard1;
        end
    elseif type_ == "Item2" then
        if count_ ~= nil and count_ > 0 then
           itemId = LuaConst.Const.ExpCard2;
        end
    elseif type_ == "Item3" then
        if count_ ~= nil and count_ > 0 then
           itemId = LuaConst.Const.ExpCard3;
        end
    end

    ItemSys.UseItem(itemId,count_,PlayerInfoData.GetCurrPlayerId());
end

function IsEnoughItem(type_)
    if GetExpPotionCount(type_) > 0 then
        return true;
    end

    return false;
end

function IsBeyondRoleLv()
    local expNeedLast = Config.GetProperty(Config.LevelTable(), tostring(Role.Get_lv()),"HExp");

    if PlayerInfoData.GetLv2Exp(m_expLast) < m_limitPlayerLv then
        return false;
    end
    if PlayerInfoData.GetLv2Exp(m_expLast) == m_limitPlayerLv and tonumber(m_expLast) >= tonumber(expNeedLast) then
        return false;
    end
    
    if PlayerInfoData.GetLv2Exp(m_expPrev) < m_limitPlayerLv then
        return false;
    end

    return true;
end

function MinusExpPotionCount(type_,count_)
   if type_ == "Item1" then 
        if m_expCardCount1 ~= nil and m_expCardCount1 > 0 then
           m_expCardCount1 = m_expCardCount1 - count_;
        end
    elseif type_ == "Item2" then
        if m_expCardCount2 ~= nil and m_expCardCount2 > 0 then
           m_expCardCount2 = m_expCardCount2 - count_;
        end
    elseif type_ == "Item3" then
        if m_expCardCount3 ~= nil and m_expCardCount3 > 0 then
           m_expCardCount3 = m_expCardCount3 - count_;
        end
    end
end

function GetExpPotionCount(type_)
   if type_ == "Item1" then 
        if m_expCardCount1 ~= nil and m_expCardCount1 > 0 then
            return m_expCardCount1;
        end
    elseif type_ == "Item2" then
        if m_expCardCount2 ~= nil and m_expCardCount2 > 0 then
            return m_expCardCount2;
        end
    elseif type_ == "Item3" then
        if m_expCardCount3 ~= nil and m_expCardCount3 > 0 then
            return m_expCardCount3;
        end
    end


    return 0;
end

function GetCardExp(type_)
   if type_ == "Item1" then 
        return m_expCard1;
    elseif type_ == "Item2" then
        return m_expCard2;
    elseif type_ == "Item3" then
        return m_expCard3;
    end
end

function CheckPlayerLv()
    if IsBeyondRoleLv() then
        if IsValidClick(3) then
            WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("UIUpgrade_NoBeyondRoleLv") });
        end
        return true;
    end

    return false;
end

function OnHide()

end

function OnShow()

end


function OnDestroy()
    UnRegisterMsgCallback();
    
    if m_timerId ~= nil then
        LuaTimer.RmvTimer(m_timerId);
        m_timerId = nil;
    end
    if m_fxTimerId ~= nil then
        LuaTimer.RmvTimer(m_fxTimerId);
        m_fxTimerId = nil;
    end
    
    m_expPrev = 0;
end















