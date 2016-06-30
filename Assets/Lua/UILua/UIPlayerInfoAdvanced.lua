--region NewFile_1.lua
--Author : hongpeng.zhao
--Date   : 2015/3/5
--此文件由[BabeLua]插件自动生成



--endregion

module("UIPlayerInfoAdvanced", package.seeall)

local ThisTransform = nil;


local lblAttriTb = {};
local lblAttriNameTb = {};
local lblItemNumTb = {};
local lblItemNameTb = {};
local tfItemTb = {};
local tfItemIconTb = {};
local c_itmeMax = 6;    -- 3

local onAdvSuccess = nil;
function InitPlayerInfoAdvanced(subTransform_)
    ThisTransform = subTransform_;

    local lbl_unlockSkill = TransformFindChild(ThisTransform,"MiddlePart/lbl_unlockSkillTips");

    Util.AddClick(TransformFindChild(ThisTransform,"BottomPart/Advanced").gameObject, BtnAdvanced);
    

    tfItemTb = {};
    lblItemNumTb = {};
    lblItemNameTb = {};
    tfItemIconTb = {};
    lblAttriTb = {};
    lblAttriNameTb = {};
    for i=1,6 do
        table.insert(lblAttriTb,TransformFindChild(ThisTransform,"MiddlePart/Attribute/lbl_attribute"..i..i));
        table.insert(lblAttriNameTb,TransformFindChild(ThisTransform,"MiddlePart/Attribute/lbl_attribute"..i));
    end
    for j=1,6 do
        table.insert(tfItemTb,TransformFindChild(ThisTransform,"BottomPart/Item/"..j));
        table.insert(lblItemNameTb,TransformFindChild(ThisTransform,"BottomPart/Item/"..j.."/LabelName"));
        table.insert(lblItemNumTb,TransformFindChild(ThisTransform,"BottomPart/Item/"..j.."/Label"));
        table.insert(tfItemIconTb,TransformFindChild(ThisTransform,"BottomPart/Item/"..j.."/Icon"));
        local btnItem = TransformFindChild(ThisTransform,"BottomPart/Item/"..j).gameObject;
        Util.AddClick(btnItem, BtnItem);
    end

    RefreshAdvanced();
    RegisterMsgCallback();
end

function RegisterMsgCallback()
    SynSys.RegisterCallback("hero",RefreshAdvanced);
end

function UnRegisterMsgCallback()
    SynSys.UnRegisterCallback("hero",RefreshAdvanced);
end

function RefreshAdvanced()
    for i=1,#tfItemTb do
        GameObjectSetActive(tfItemTb[i],false);
    end

    for i=1,#lblItemNumTb do
        local id = 2*i-1  --(i-1)*2+1;
        local numId = 2*i --(i-1)*2+2;
        if tonumber(GetAdvItemSub(id)) > 0 then
            GameObjectSetActive(tfItemTb[i],true);
            Util.SetUITexture(tfItemIconTb[i],LuaConst.Const.ItemIcon,NeedItemIcon(id), false);
            UIHelper.SetLabelTxt(lblItemNameTb[i], NeedItemName(id));
            UIHelper.SetLabelTxt(lblItemNumTb[i], CurrItemAmount(id).."/"..GetAdvItemSub(numId));
        end
    end


    local playerId = PlayerInfoData.GetCurrPlayerId();
    for i=1,6 do
        UIHelper.SetLabelTxt(lblAttriNameTb[i],HeroData.GetAttr1Name(playerId,i));
        if NextQualityAttr1(playerId,i) ~= nil then
            UIHelper.SetLabelTxt(lblAttriTb[i],"+"..NextQualityAttr1(playerId,i));
        else
            UIHelper.SetLabelTxt(lblAttriTb[i],"--");
        end
    end

end

function BtnAdvanced()
    if (Time.realtimeSinceStartup - PlayerInfoData.GetLastClickTime() < 2) then
        return;
    end
    PlayerInfoData.SetLastClickTime(Time.realtimeSinceStartup);

    local advMod = Config.GetProperty(Config.HeroTable(), PlayerInfoData.GetCurrPlayerId(), "advMod");
    local currQuality = Hero.GetCurrQuality(PlayerInfoData.GetCurrPlayerId());
    local limitLv = Config.GetProperty(Config.HeroAdvTable(),HeroData.GetAdvID(advMod,currQuality),"lv_limit");
    if Hero.GetHeroData2Id(PlayerInfoData.GetCurrPlayerId()).lv < limitLv then
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { string.format(Util.LocalizeString("UIAdv_LilitLv"),limitLv) });
        return;
    end

    if not IsAdvanceItem() then
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("UIAdv_NoEnoughtItem") });
        return;
    end

    if IsMaxAdvance() then
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("UIAdv_MaxQuality") });
        return;
    end

    Hero.ReqHeroAdv(PlayerInfoData.GetCurrPlayerId(),OnAdvSuccess);
end
function RegisterOnAdvSuccess(args_)
    onAdvSuccess = args_;
end

function OnAdvSuccess(data_)
    if (onAdvSuccess ~= nil) then
        onAdvSuccess();
    end
end

function IsAdvanceItem()
    for i=1,c_itmeMax do
        if CurrItemAmount(2*i-1) < GetAdvItemSub(2*i) then
            return false;
        end
    end

    return true;
end

function IsMaxAdvance()
    local currQuality = Hero.GetCurrQuality(PlayerInfoData.GetCurrPlayerId());
    if currQuality >= Hero.GetHeroMaxQuality() then
        return true;
    else
        return false;
    end
end

function NextQualityAttr1(playerId_,index_)
--    local cloneHeroData = {};
--    cloneHeroData = CommonScript.DeepCopy(Hero.GetHeroData2Id(playerId_));
--    if cloneHeroData["adv"] < Hero.GetHeroMaxQuality() then
--        cloneHeroData["adv"] = cloneHeroData["adv"] + 1;
--    end
--    HeroData.CalcAttr(cloneHeroData);

--    return HeroData.GetAttr1ByData(cloneHeroData,playerId_,index_);
    local advMod = Config.GetProperty(Config.HeroTable(), playerId_, "advMod");
    local adv = Hero.GetHeroData2Id(playerId_)["adv"];
    if adv < Hero.GetHeroMaxQuality() then
        return math.floor(Config.GetProperty(Config.HeroAdvTable(),HeroData.GetAdvID(advMod,adv+1),"att")[index_] -
                          Config.GetProperty(Config.HeroAdvTable(),HeroData.GetAdvID(advMod,adv),"att")[index_]);
    else
        return nil;
    end
end

function GetAdvItemSub(index_)
    local advMod = Config.GetProperty(Config.HeroTable(), PlayerInfoData.GetCurrPlayerId(), "advMod");
    local currQuality = Hero.GetCurrQuality(PlayerInfoData.GetCurrPlayerId());
    local subItemTB = Config.GetProperty(Config.HeroAdvTable(),HeroData.GetAdvID(advMod,currQuality),"sub");

    if index_ <= #subItemTB then
        return subItemTB[index_];
    else
        return nil;
    end
end

function NeedItemName(index_)
    if not GetAdvItemSub(index_) then
        return Config.GetProperty(Config.ItemTable(),tostring(GetAdvItemSub(index_)),'name');
    else
        return " ";
    end
end
function NeedItemIcon(index_)
    if GetAdvItemSub(index_) ~= nil then
        return Config.GetProperty(Config.ItemTable(),tostring(GetAdvItemSub(index_)),'icon');
    else
        return "Default";
    end
end

function CurrItemAmount(index_)
    return ItemSys.GetItemData(tostring(GetAdvItemSub(index_))).num;
end

function BtnItem(args)
    local id = GetAdvItemSub(tonumber(args.name)*2-1);
    print("id: "..id)
    WindowMgr.ShowWindow(LuaConst.Const.UIItemTips,{id = id});
end

function BtnNeedItem(args)
    UIOriginTipsScript.TryOpen( {originType = UIOriginTipsScript.enumOriginType.AdvItem,
                                 id = GetAdvItemSub(tonumber(args.name)*2-1)} );
end

function OnDestroy()
    onAdvSuccess = nil;
    UnRegisterMsgCallback();
end


