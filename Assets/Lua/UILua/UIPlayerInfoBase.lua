module("UIPlayerInfoBase", package.seeall)

require "UILua/UITrainTalk"

local ThisTransform = nil;

local lbl_name = nil;
local lbl_name2 = nil;
local lbl_lv = nil;
local pgr_exp = nil;
local lbl_playerValue = nil;
local lbl_rating = nil;
local lbl_base = nil;
local lbl_evolve = nil;
local spr_zhuLi = nil;
local spr_club = nil;
local spr_country = nil;
local spr_qualityBg = nil;
local spr_qualityBannerBg = nil;
--local spr_status = nil;
local tex_playerHead = nil;
local tex_courtBg = nil;
local m_posR = nil;
local m_base = nil;
local m_evolve = nil;
local m_starsTb = {};
local m_evolveBtnTb = {};
local m_evolveLblTb = {};
local m_baseLblTb = {};


function InitPlayerInfoBase(subTransform_)
    ThisTransform = subTransform_;

    m_base = TransformFindChild(ThisTransform,"CenterPanel/BottomPart/Base");
    m_evolve = TransformFindChild(ThisTransform,"CenterPanel/BottomPart/Evolve");
    local m_topPart = TransformFindChild(ThisTransform,"CenterPanel/TopPart");
    local m_middlePart = TransformFindChild(ThisTransform,"CenterPanel/MiddlePart");

    
    tex_playerHead = TransformFindChild(m_middlePart,"spr_icom");
    lbl_name = TransformFindChild(m_middlePart,"lbl_name");
    lbl_name2 = TransformFindChild(m_middlePart,"lbl_name2");
    lbl_lv = TransformFindChild(m_middlePart,"lbl_lv");
    pgr_exp = TransformFindChild(m_middlePart,"pgr_exp/Foreground");
    lbl_playerValue = TransformFindChild(m_middlePart,"PosR/lbl_value");
    --spr_status = TransformFindChild(m_middlePart,"PosR/Whistle/Status");
    m_posR = TransformFindChild(m_middlePart,"PosR");
    spr_zhuLi = TransformFindChild(m_middlePart,"PosR/spr_mainMinor");

    lbl_rating = TransformFindChild(m_topPart,"Banner1/lbl_rating");
    spr_qualityBg = TransformFindChild(m_topPart,"FxImage");
    spr_qualityBannerBg = TransformFindChild(m_topPart,"Banner1/spr_quality");
    local spr_rating = TransformFindChild(m_topPart,"Banner1/spr_level");
    spr_club = TransformFindChild(m_topPart,"Flag/IconClub");
    spr_country = TransformFindChild(m_topPart,"Flag/IconCountry");
    tex_courtBg = TransformFindChild(ThisTransform,"CenterPanel/BgCourt");
    UIHelper.SetSpriteNameNoPerfect(spr_rating,HeroData.GetHeroRatingName(PlayerInfoData.GetCurrPlayerId()));
    Util.SetUITexture(tex_courtBg,LuaConst.Const.PlayerSystem,"Container_bg", true);
    Util.SetUITexture(TransformFindChild(m_topPart,"spr_subBg"),LuaConst.Const.PlayerSystem,"Top_bg", true);

    m_evolveLblTb = {};
    local toggleUpgrade = TransformFindChild(m_evolve,"BtnUpgrade").gameObject;
    local lblUpgrade = TransformFindChild(m_evolve,"BtnUpgrade/Label");
    Util.AddClick(toggleUpgrade,ToggleUpgrade);
    table.insert(m_evolveLblTb,lblUpgrade);
    
    local toggleBreak = TransformFindChild(m_evolve,"BtnBreak").gameObject;
    local lblBreak = TransformFindChild(m_evolve,"BtnBreak/Label")
    Util.AddClick(toggleBreak,ToggleBreak);
    table.insert(m_evolveLblTb,lblBreak);
    
    local toggleAdvanced = TransformFindChild(m_evolve,"BtnAdv").gameObject;
    local lblAdvanced = TransformFindChild(m_evolve,"BtnAdv/Label")
    Util.AddClick(toggleAdvanced,ToggleAdvanced);
    table.insert(m_evolveLblTb,lblAdvanced);
    
    local toggleCapability = TransformFindChild(m_evolve,"BtnCapability").gameObject;
    local lblCapability = TransformFindChild(m_evolve,"BtnCapability/Label");
    Util.AddClick(toggleCapability,TogglePotential);
    table.insert(m_evolveLblTb,lblCapability);
    
    local toggleSkill = TransformFindChild(m_evolve,"BtnSkill").gameObject;
    local lblSkill = TransformFindChild(m_evolve,"BtnSkill/Label");
    Util.AddClick(toggleSkill,ToggleSkill);
    table.insert(m_evolveLblTb,lblSkill);
    
    local toggleRelation = TransformFindChild(m_evolve,"BtnRelation").gameObject;
    local lblRelation = TransformFindChild(m_evolve,"BtnRelation/Label");
    Util.AddClick(toggleRelation,ToggleRelation);
    table.insert(m_evolveLblTb,lblRelation);

    m_starsTb = {};
    m_starsTb = InstantiateStars(TransformFindChild(m_topPart,"Banner1/Stars"),UIPlayerInfoBaseScript.GetWindowStar(), 5, 5);

    InitBtnUI();
    RefreshPlayerInfo();
    RegisterMsgCallback();

    SetUIExpLv(PlayerInfoData.GetCurrHeroLv(),PlayerInfoData.GetCurrExp()/PlayerInfoData.GetCurrMaxExp());
    
    ToggleUpgrade();
end

function IsCommonCourtBg(args_)
    GameObjectSetActive(tex_courtBg,args_);
end

function SetUIExpLv(lv_,amount_)
    UIHelper.SetLabelTxt(lbl_lv,"Lv." .. lv_);
    UIHelper.SetSpriteFillAmount(pgr_exp,amount_);
end
function InitBtnUI()
    m_evolveBtnTb = {};
    for i = 1, 6 do
        local strBtn = "";
        if i == 1 then
            strBtn = "BtnUpgrade";
        elseif i == 2 then
            strBtn = "BtnBreak";
        elseif i == 3 then
            strBtn = "BtnAdv";
        elseif i == 4 then
            strBtn = "BtnCapability";
        elseif i == 5 then
            strBtn = "BtnSkill";
        elseif i == 6 then
            strBtn = "BtnRelation";
        end

        local tb = {};
        --tb.bg = TransformFindChild(m_evolve,strBtn .."/Background");
        tb.tips = TransformFindChild(m_evolve,strBtn .."/Tips");
        tb.sprite2 = TransformFindChild(m_evolve,strBtn .."/Tips/Sprite2");
        tb.sprite22 = TransformFindChild(m_evolve,strBtn .."/Tips/Sprite22");
        table.insert(m_evolveBtnTb,tb);
    end

end

function UpdateBtnEvolve(args_)
    for i = 1, 6 do
        if i == args_ then
            --GameObjectSetActive(m_evolveBtnTb[i].bg,true);
            GameObjectSetActive(m_evolveBtnTb[i].tips,true);
            GameObjectSetActive(m_evolveBtnTb[i].sprite2,true);
            GameObjectSetActive(m_evolveBtnTb[i].sprite22,false);
            UIHelper.SetWidgetColor(m_evolveLblTb[i],UIPlayerInfoBaseScript.GetSelectFontColor(1));
        else
            --GameObjectSetActive(m_evolveBtnTb[i].bg,false);
            GameObjectSetActive(m_evolveBtnTb[i].tips,false);
            UIHelper.SetWidgetColor(m_evolveLblTb[i],UIPlayerInfoBaseScript.GetOffFontColor(1));
        end
    end

end

function RegisterMsgCallback()
    SynSys.RegisterCallback("hero",RefreshPlayerInfo);
end

function UnRegisterMsgCallback()
    SynSys.UnRegisterCallback("hero",RefreshPlayerInfo);
end


function RefreshPlayerInfo()
    local playerId = PlayerInfoData.GetCurrPlayerId();
    local currQuality = Hero.GetCurrQuality(playerId);

    UIHelper.SetLabelTxt(lbl_name,Hero.ColorHeroName(currQuality,HeroData.GetHeroName(playerId)));
    UIHelper.SetLabelTxt(lbl_name2,HeroData.GetHeroPosName(playerId));
--    UIHelper.SetLabelTxt(lbl_lv,"Lv." .. PlayerInfoData.GetCurrHeroLv());
    UIHelper.SetLabelTxt(lbl_rating,GetRating());
    UIHelper.SetLabelTxt(lbl_playerValue,Util.LocalizeString("UIPlayer_BattleValue").. PlayerInfoData.GetCurrHeroData().power);
    if Hero.IsMainPlayer(playerId) then
        GameObjectSetActive(spr_zhuLi,true);
    else
        GameObjectSetActive(spr_zhuLi,false);
    end

    --UIHelper.SetSpriteName(spr_status,Hero.GetHeroStatusName(playerId));
    UIHelper.SetWidgetColor(spr_qualityBg,Hero.GetHeroQualityBgColor(currQuality));
    UIHelper.SetWidgetColor(spr_qualityBannerBg,Hero.GetHeroQualityBannerBg(currQuality));
    Util.SetUITexture(spr_club,LuaConst.Const.ClubIcon,HeroData.GetHeroClubIcon(playerId), false);
    Util.SetUITexture(spr_country,LuaConst.Const.FlagIcon,HeroData.GetHeroCountryIcon(playerId), false);
    Util.SetUITexture(tex_playerHead,LuaConst.Const.PlayerHeadIcon,HeroData.GetHeroIcon(playerId), true);


    for i = 1, 5 do
        GameObjectSetActive(m_starsTb[i], false);
    end

    local currStar = Hero.GetCurrStars(playerId);
    for i = 1, tonumber(currStar) do
        GameObjectSetActive(m_starsTb[i], true);
    end

end

function BtnWhistle()
    WindowMgr.ShowWindow(LuaConst.Const.UITrainTalk,{ playerId = PlayerInfoData.GetCurrPlayerId()} );
end

function ToggleUpgrade()
    UpdateBtnEvolve(1);
    UIPlayerInfoBaseScript.ToggleEvolve("Upgrade");
end

function ToggleBreak()
    UpdateBtnEvolve(2);
    UIPlayerInfoBaseScript.ToggleEvolve("Break");
end

function ToggleAdvanced()
    UpdateBtnEvolve(3);
    UIPlayerInfoBaseScript.ToggleEvolve("Advance");
end

function TogglePotential()
    UpdateBtnEvolve(4);
    UIPlayerInfoBaseScript.ToggleEvolve("Pot");
end

function ToggleSkill()
    UpdateBtnEvolve(5);
--    WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { "此功能暂未开放，敬请期待" });
    UIPlayerInfoBaseScript.ToggleEvolve("Skill");
end

function ToggleRelation()
    UpdateBtnEvolve(6);
    UIPlayerInfoBaseScript.ToggleEvolve("Relation");
end

function GetRating()
    if HeroData.GetHeroRating(PlayerInfoData.GetCurrPlayerId()) ~= nil then
        return "("..HeroData.GetHeroRating(PlayerInfoData.GetCurrPlayerId())..")"
    else
        return "(-1)";
    end
end


function UpdateBaseBtn(args_)

end

function UpdateEvolveBtn(args_)

end

function OnDestroy()
    UnRegisterMsgCallback();
end
