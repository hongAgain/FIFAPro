--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion

module("PlayerPotData", package.seeall)

enum_PotType = {Normal=0,Rmb=1,King=2,AutoPot=3,TargetFinish=4,TargetReward=5};

function GetEnum2Num(num_)
    if num_ == 0 then
        return enum_PotType.Normal;
    elseif num_ == 1 then
        return enum_PotType.Rmb;
    elseif num_ == 2 then
        return enum_PotType;
    end
    return nil;
end

function IsEnoughMoney(type_,times_)
    if type_ == enum_PotType.Normal then
        if ItemSys.GetItemData(LuaConst.Const.SB).num < GetPotCost(type_,times_) then
            WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("UIPot_NoGold") });
            return false;
        end
    else
        if ItemSys.GetItemData(LuaConst.Const.GB).num < GetPotCost(type_,times_) then
            WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("UIPot_NoRMB") });
            return false;
        end
    end

    return true;
end


function GetPotCost(type_,times_)
    local playerId = PlayerInfoData.GetCurrPlayerId();
    if type_ == enum_PotType.Normal then
        return Config.GetProperty(Config.HeroPotCost(),tostring(Role.Get_lv()),'normal_train')*times_;
    elseif type_ == enum_PotType.Rmb then
        return Config.GetProperty(Config.HeroPotCost(),tostring(Role.Get_lv()),'gold_train')*times_;
    elseif type_ == enum_PotType.King then
        return Config.GetProperty(Config.HeroPotCost(),tostring(Role.Get_lv()),'ultimate_train')*times_;
    end

    return -1;
end

function GetTargetPotLv(playerId_)
    local potModel = Config.GetProperty(Config.HeroTable(),tostring(playerId_),'potential_model');
    local lv = 0;
    local needAttr = nil;
    local currAttr = nil;
    local totalNeed = nil;
    local totalCurr = nil;
    for i=1,10 do
        totalCurr = 0;
        totalNeed = Config.GetProperty(Config.HeroPotTarget(),potModel..i,'total_need');
        needAttr = Config.GetProperty(Config.HeroPotTarget(),potModel..i,'att_need');
        currAttr = Hero.GetHeroData2Id(playerId_)["training"];
        for i=1,#needAttr do
            totalCurr = totalCurr + currAttr[i];
            if tonumber(currAttr[i]) < needAttr[i] then
                return lv;
            end
        end
        if totalCurr < totalNeed then
            return lv;
        end
        lv = lv + 1;
    end

    return lv;
end

function GetTargetDes(playerId_,lv_)
    if lv_ < 1 then
        return Util.LocalizeString("UIPot_RewardDes0");
    else
        local potModel = Config.GetProperty(Config.HeroTable(),tostring(playerId_),'potential_model').. lv_;   
        return Config.GetProperty(Config.HeroPotTarget(),potModel,'des');
    end
end

function GetTargetNeedStr(playerId_,lv_)
    local potModel = Config.GetProperty(Config.HeroTable(),tostring(playerId_),'potential_model').. lv_;
    local totalNeed = Config.GetProperty(Config.HeroPotTarget(),potModel,'total_need');
    local needAttr = Config.GetProperty(Config.HeroPotTarget(),potModel,'att_need');
    local str = "";
    local iTest = 0;
    for i=1,#needAttr do
        if needAttr[i] ~= 0 then
            iTest = iTest + 1;
            if iTest == 4 then
                str = str .. Util.LocalizeString("UIAttr_1Attr"..i).." ".. needAttr[i] .."\n";
            else
                str = str .. Util.LocalizeString("UIAttr_1Attr"..i).." ".. needAttr[i] .."  ";
            end
        end
    end

    return str..Util.LocalizeString("UIPot_AllAttr").." ".. totalNeed;
end

function GetTargetRewardStr(playerId_,lv_)
    if lv_ < 1 then
        lv_ = 1;
    end

    local potModel = Config.GetProperty(Config.HeroTable(),tostring(playerId_),'potential_model').. lv_;
    local rewardAttr = Config.GetProperty(Config.HeroPotTarget(),potModel,'att_reward');
    local str = "";
    for i=1,#rewardAttr do
        if rewardAttr[i] ~= 0 then
            str = str .. Util.LocalizeString("UIAttr_1Attr"..i).." [66ccff]".. rewardAttr[i] .."[-]      ";
        end
        if i == 3 then
            str = str.. "\n";
        end
    end

    return str;
end


