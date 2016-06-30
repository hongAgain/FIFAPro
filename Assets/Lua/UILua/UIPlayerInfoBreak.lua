--region NewFile_1.lua
--Author : hongpeng.zhao
--Date   : 2015/3/5
--此文件由[BabeLua]插件自动生成



--endregion


module("UIPlayerInfoBreak", package.seeall)

require "UILua/UIOriginTipsScript"

local ThisTransform = nil;

local lbl_prgChips = nil;
local prg_chips = nil;

local lbl_attr1Tb = {};
local lbl_attr11Tb = {};
function InitPlayerInfoBreak(subTransform_)
    ThisTransform = subTransform_;

    lbl_attr1Tb = {};
    lbl_attr11Tb = {};
    for i=1,6 do
        table.insert(lbl_attr1Tb,TransformFindChild(ThisTransform,"MiddlePart/Attribute/lbl_attribute"..i));
        table.insert(lbl_attr11Tb,TransformFindChild(ThisTransform,"MiddlePart/Attribute/lbl_attribute"..i..i));
    end


    pgr_exp = TransformFindChild(ThisTransform,"BottomPart/prg_chips");
    lbl_prgChips = TransformFindChild(ThisTransform,"BottomPart/prg_chips/Label");


    Util.AddClick(TransformFindChild(ThisTransform,"BottomPart/Break").gameObject, BtnBreak);
    Util.AddClick(TransformFindChild(ThisTransform,"BottomPart/BtnChips").gameObject, BtnChipsOrigin);

    RefreshBreak();
    RegisterMsgCallback();
end

function RegisterMsgCallback()
    SynSys.RegisterCallback("hero",RefreshBreak);
end

function UnRegisterMsgCallback()
    SynSys.UnRegisterCallback("hero",RefreshBreak);
end

function RefreshBreak()
    local playerId = PlayerInfoData.GetCurrPlayerId();
    local strChips,iChips = GetPlayerChips();
    UIHelper.SetLabelTxt(lbl_prgChips,strChips);
    UIHelper.SetProgressBar(pgr_exp,iChips);

    for i=1,6 do
        UIHelper.SetLabelTxt(lbl_attr1Tb[i],string.format(Util.LocalizeString("UIBreak_1Attr"),HeroData.GetAttr1Name(playerId,i)));
        UIHelper.SetLabelTxt(lbl_attr11Tb[i],NextStarAttr1(playerId,i));
    end

end

function NextStarAttr1(playerId_,index_)
    local cloneHeroData = {};
    cloneHeroData = CommonScript.DeepCopy(Hero.GetHeroData2Id(playerId_));
    if GetCurrStars() < Hero.GetHeroMaxStars() then
        cloneHeroData["slv"] = cloneHeroData["slv"] + 1;
    end
    HeroData.CalcAttr(cloneHeroData);
    
    return HeroData.GetAttr1ByData(cloneHeroData,playerId_,index_);
end

function OpenOriginTips()
    local tb = {};
    tb.originType = UIOriginTipsScript.enumOriginType.PlayerChips;
    tb.id = 1;
    UIOriginTipsScript.TryOpen(tb);
end


function BtnBreak()
    if (Time.realtimeSinceStartup - PlayerInfoData.GetLastClickTime()) < 2 then
        return;
    end
    PlayerInfoData.SetLastClickTime(Time.realtimeSinceStartup);
    if IsFullStars() then
        local msg = Util.LocalizeString("UIBreak_BreakFull");
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { msg });
        return;
    end

    if not IsBreakChips() then
        local msg = Util.LocalizeString("UIBreak_NoChips");
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { msg });
        return;
    end

    if not IsEnoughMoney(PlayerInfoData.GetCurrPlayerId()) then
        local msg = Util.LocalizeString("UIBreak_NoGold");
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { msg });
        return;
    end
    
    Hero.ReqHeroBreak(PlayerInfoData.GetCurrPlayerId(),OnBreakSuccess);
end

function BtnChipsOrigin()
    OpenOriginTips();

end


local onBreakSuccess = nil;
-- Logic
function RegisterOnBreakSuccess(args_)
    onBreakSuccess = args_;
end
function OnBreakSuccess(data_)
    if (onBreakSuccess ~= nil) then
        onBreakSuccess();
    end
end

function IsBreakChips()
    if not IsFullStars() then
        local currStar = GetCurrStars();
        local needChips = Config.GetProperty(Config.HeroSlvTable(), tostring(currStar+1),'subSoul');
        print("needChips-> "..needChips);
        local needChipsId = Config.GetProperty(Config.HeroTable(), PlayerInfoData.GetCurrPlayerId(), 'soul');
        print("needChipsId-> "..needChipsId);
        local chipsedNum = ItemSys.GetItemData(needChipsId).num;
    
        if chipsedNum >= needChips then
            return true;
         else
            return false;
        end
    else
        return false;
    end

end

function IsEnoughMoney(playerId_)
    local currStar = GetCurrStars();
    local  needMoney = Config.GetProperty(Config.HeroSlvTable(), tostring(currStar+1),'subMoney');
    if ItemSys.GetItemData(LuaConst.Const.SB).num >= needMoney then
        return true;
    else
        return false;
    end

end

function IsFullStars() -- true=Full
    if GetCurrStars() < Hero.GetHeroMaxStars() then
        return false;
    else
        return true;
    end
end

function GetPlayerChips()
    if IsFullStars() then
        print("IsFullStars");
        return Util.LocalizeString("UIBreak_BreakFull"),1;
    else
        local currStar = GetCurrStars();
        local needChips = Config.GetProperty(Config.HeroSlvTable(), tostring(currStar+1),'subSoul');
        print("needChips-> "..needChips);
        local needChipsId = Config.GetProperty(Config.HeroTable(), PlayerInfoData.GetCurrPlayerId(), 'soul');
        print("needChipsId-> "..needChipsId);
        local chipsedNum = ItemSys.GetItemData(needChipsId).num;
        return chipsedNum.."/"..needChips,chipsedNum/needChips;
    end

end

function GetCurrStars()
    local originStar =  Config.GetProperty(Config.HeroTable(), PlayerInfoData.GetCurrPlayerId(), 'islv');
--    print("originStar-> "..originStar);
    local star = UIPlayerInfoBaseScript.GetCurrHeroData()['slv'] + originStar;
--    print("star->"..star)
    return star;
end

function OnDestroy()
    onBreakSuccess = nil;
    
    UnRegisterMsgCallback();
end