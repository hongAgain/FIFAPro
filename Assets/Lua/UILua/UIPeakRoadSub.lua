--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion
module("UIPeakRoadSub", package.seeall)


require "UILua/UIChallengeTL"
require "UILua/UIShop"

local windowFight = nil;

local lbl_name = nil;
local lbl_challenge = nil;
local lbl_finishChallenge = nil;
local lbl_leftName = nil;
local lbl_rightName = nil;
local lbl_enemyName = nil;
local lbl_enemyLv = nil;
local spr_enemyIcon = nil;
local lbl_enemyExp = nil;
local spr_leftClub = nil;
local spr_rightClub = nil;
local btn_challenge = nil;
local tex_centerCup = nil;

local obj_leftItem = nil;
local obj_rightItem = nil;
local obj_dropItem = nil;

local m_leftSelectTb = {};
local m_leftCurrTb = {};
local m_leftStarTb = {};
local m_leftIconTb = {};
local m_rightSelectTb = {};
local m_rightCurrTb = {};
local m_rightStarTb = {};
local m_rightIconTb = {};

local m_currAdv = 1;
function OnStart(gameObject, params)
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");

    BindUI();
end

function BindUI()
    local transform = window.transform;

    windowFight = TransformFindChild(transform, "Center/ChallengeTips");

    lbl_enemyName = TransformFindChild(transform, "Center/ChallengeTips/LeftPart/Name");
    lbl_enemyLv = TransformFindChild(transform, "Center/ChallengeTips/LeftPart/Lv");
    spr_enemyIcon = TransformFindChild(transform, "Center/ChallengeTips/LeftPart/Icon/Sprite");
    lbl_enemyExp = TransformFindChild(transform, "Center/ChallengeTips/RightPart/Exp/Label");
    lbl_name = TransformFindChild(transform, "Center/CenterPart/Label");
    tex_centerCup = TransformFindChild(transform, "Center/CenterPart/Icon/Cup");
    lbl_challenge = TransformFindChild(transform, "Center/CenterPart/Bottom/BtnChallenge/Label");
    lbl_finishChallenge = TransformFindChild(transform, "Center/CenterPart/Bottom/Label");
    lbl_leftName = TransformFindChild(transform, "Center/CenterPart/LeftClub/Label");
    lbl_rightName = TransformFindChild(transform, "Center/CenterPart/RightClub/Label");
    UIHelper.SetLabelTxt(TransformFindChild(transform, "Center/TopPart/Title/Label"),PeakRoadData.GetGroupName(tonumber(UIPeakRoadScript.GetCurrFloor())));

    spr_leftClub = TransformFindChild(transform, "Center/CenterPart/LeftClub/Icon");
    spr_rightClub = TransformFindChild(transform, "Center/CenterPart/RightClub/Icon");

    obj_leftItem = TransformFindChild(transform, "Center/CenterPart/LeftClub");
    obj_rightItem = TransformFindChild(transform, "Center/CenterPart/RightClub");
    obj_dropItem = TransformFindChild(transform, "Center/ChallengeTips/RightPart/DropItem");


    Util.AddClick(TransformFindChild(transform, "Center/TopPart/Reset").gameObject, BtnReset);
    Util.AddClick(TransformFindChild(transform, "Center/TopPart/Shop").gameObject, BtnShop);
    Util.AddClick(TransformFindChild(transform, "Center/TopPart/BtnSkip").gameObject, BtnPromotion);

    btn_challenge = TransformFindChild(transform, "Center/CenterPart/Bottom/BtnChallenge");
    Util.AddClick(btn_challenge.gameObject, BtnChallenge);

    Util.AddClick(TransformFindChild(transform, "Center/ChallengeTips/BtnChallenge").gameObject, BtnFight);
    Util.AddClick(TransformFindChild(transform, "Center/ChallengeTips/Close").gameObject, BtnCloseFight);

    m_leftSelectTb = {};
    m_leftCurrTb = {};
    m_leftStarTb = {};
    m_leftIconTb = {};
    m_leftLineTb = {};
    m_rightSelectTb = {};
    m_rightCurrTb = {};
    m_rightStarTb = {};
    m_rightIconTb = {};
    m_rightLineTb = {};
    for i=1,7 do 
       table.insert(m_leftSelectTb,TransformFindChild(transform, "Center/LeftPart/"..i.."/selected"));
       table.insert(m_leftCurrTb,TransformFindChild(transform, "Center/LeftPart/"..i.."/current"));
       table.insert(m_leftStarTb,TransformFindChild(transform, "Center/LeftPart/"..i.."/star"));
       table.insert(m_leftIconTb,TransformFindChild(transform, "Center/LeftPart/"..i.."/icon"));
       table.insert(m_leftLineTb,TransformFindChild(transform, "Center/LeftPart/"..i.."/line"));
       Util.AddClick(TransformFindChild(transform, "Center/LeftPart/"..i).gameObject, BtnLeftPlayer);

       table.insert(m_rightSelectTb,TransformFindChild(transform, "Center/RightPart/"..i.."/selected"));
       table.insert(m_rightCurrTb,TransformFindChild(transform, "Center/RightPart/"..i.."/current"));
       table.insert(m_rightStarTb,TransformFindChild(transform, "Center/RightPart/"..i.."/star"));
       table.insert(m_rightIconTb,TransformFindChild(transform, "Center/RightPart/"..i.."/icon"));
       table.insert(m_rightLineTb,TransformFindChild(transform, "Center/RightPart/"..i.."/line"));
       Util.AddClick(TransformFindChild(transform, "Center/RightPart/"..i).gameObject, BtnRightPlayer);
    end

    SetTarget(false);

    RefreshSub();
end


function RefreshSub()
    m_currAdv = GetCurrAdv();
    SetAdvTitle();
    if PeakRoadData.GetBRefreshPRSub() then
        if IsCurrFinish() then
            m_currAdv = 3;
        end

        PeakRoadData.SetBRefreshPRSub(false);
    end

    if m_currAdv == 3 then
        UIHelper.SetLabelTxt(lbl_name,Config.GetProperty(Config.RaidDFTable(),tostring(PeakRoadData.GetCurrFloorIndex()),'name'));
        Util.SetUITexture(tex_centerCup,LuaConst.Const.CupDF,Config.GetProperty(Config.RaidDFTable(),tostring(PeakRoadData.GetCurrFloorIndex()),'cup_icon'),true);
    else
        UIHelper.SetLabelTxt(lbl_name,Config.GetProperty(Config.RaidDFTable(),tostring(PeakRoadData.GetCurrFloorIndex()+1),'name'));
        Util.SetUITexture(tex_centerCup,LuaConst.Const.CupDF,Config.GetProperty(Config.RaidDFTable(),tostring(PeakRoadData.GetCurrFloorIndex()+1),'cup_icon'),true);
    end
    local m_clubTb = PeakRoadData.GetCurrClubTb();
    for i=1,7 do
        GameObjectSetActive(m_leftSelectTb[i],false);
        GameObjectSetActive(m_leftStarTb[i],false);
        GameObjectSetActive(m_leftCurrTb[i],false);
        GameObjectSetActive(m_leftLineTb[i],false);

        GameObjectSetActive(m_rightSelectTb[i],false);
        GameObjectSetActive(m_rightStarTb[i],false);
        GameObjectSetActive(m_rightCurrTb[i],false);
        GameObjectSetActive(m_rightLineTb[i],false);
    end
    
    if m_currAdv == 1 then  -- 第一次晋级比赛
        for i=1,4 do
            Util.SetUITexture(m_leftIconTb[i],LuaConst.Const.ClubIcon,m_clubTb[i].clubIcon,false);
            Util.SetUITexture(m_rightIconTb[i],LuaConst.Const.ClubIcon,m_clubTb[i+7].clubIcon,false);
            if m_clubTb[i].me then
                GameObjectSetActive(m_leftStarTb[i],true);
            end
            if m_clubTb[i+7].me then
                GameObjectSetActive(m_rightStarTb[i],true);
            end
        end
    elseif m_currAdv == 2 then  -- 第二次晋级比赛
        for i=1,6 do
            Util.SetUITexture(m_leftIconTb[i],LuaConst.Const.ClubIcon,m_clubTb[i].clubIcon,false);
            Util.SetUITexture(m_rightIconTb[i],LuaConst.Const.ClubIcon,m_clubTb[i+7].clubIcon,false);

            if m_clubTb[i].me then
                GameObjectSetActive(m_leftStarTb[i],true);
            end
            if m_clubTb[i+7].me then
                GameObjectSetActive(m_rightStarTb[i],true);
            end

            if i<=4 then 
               if m_clubTb[i].success then
                    GameObjectSetActive(m_leftLineTb[i],true);
                end
                if m_clubTb[i+7].success then
                    GameObjectSetActive(m_rightLineTb[i],true);
                end
            end
        end
     elseif m_currAdv == 3 then -- 第三次晋级比赛
        for i=1,7 do
            Util.SetUITexture(m_leftIconTb[i],LuaConst.Const.ClubIcon,m_clubTb[i].clubIcon,false);
            Util.SetUITexture(m_rightIconTb[i],LuaConst.Const.ClubIcon,m_clubTb[i+7].clubIcon,false);
        end
        for i=1,6 do
            if m_clubTb[i].me then
                GameObjectSetActive(m_leftStarTb[i],true);
            end
            if m_clubTb[i+7].me then
                GameObjectSetActive(m_rightStarTb[i],true);
            end

            if m_clubTb[i].success then
                GameObjectSetActive(m_leftLineTb[i],true);
            end
            if m_clubTb[i+7].success then
                GameObjectSetActive(m_rightLineTb[i],true);
            end
        end
    end

end

function RefreshFightUI()
    UIHelper.SetLabelTxt(lbl_enemyExp, Config.GetProperty(Config.RaidDFTable(),tostring(PeakRoadData.GetCurrFloorIndex()+1),'Hexp'));
    UIHelper.SetLabelTxt(lbl_enemyName, Config.GetProperty(Config.RaidDFTeamTable(),tostring(PeakRoadData.GetCurrFloorIndex()+1),'name'));
    UIHelper.SetLabelTxt(lbl_enemyLv, "Lv."..Config.GetProperty(Config.RaidDFTeamTable(),tostring(PeakRoadData.GetCurrFloorIndex()+1),'lv'));
    UIHelper.SetSpriteName(spr_enemyIcon,Config.GetProperty(Config.RaidDFTeamTable(),tostring(PeakRoadData.GetCurrFloorIndex()+1),'icon'));

    GameObjectSetActive(obj_dropItem,false);
end

function BtnReset()
    local OnReset = function()
        PeakRoadData.SetBRefreshPR(true);
        ExitUIPeakRoadSub();
    end

    PeakRoadData.OnBtnReset(OnReset);
end

function BtnPromotion()
    local PromotionResult = function()
        PeakRoadData.SetBRefreshPR(true);
        ExitUIPeakRoadSub();
    end

    PeakRoadData.OnPromotion(PromotionResult);

end

function BtnShop()
    WindowMgr.ShowWindow(LuaConst.Const.UIShop,{shopType = UIShopSettings.ShopType.EpicShop});
end

function BtnChallenge(args)
    PeakRoadData.SetCurrLevelId(PeakRoadData.GetCurrFloorIndex()+1);
    WindowMgr.ShowWindow(LuaConst.Const.UIChallengeTL,{levelId = PeakRoadData.GetCurrFloorIndex()+1,ChallengeType = UIChallengeTL.enum_ChallengeType.PeakRoad});  
end

function BtnFight(args)
    PeakRoadData.ClickChallengeTips();
    BtnCloseFight();
end

function BtnCloseFight()
    GameObjectSetActive(windowFight,false);
end

function ClickTargetLeft(index_)
    SetTarget(true);
    local i = index_;
    local m_clubTb = PeakRoadData.GetCurrClubTb();
    if i == 7 then
        GameObjectSetActive(m_leftSelectTb[i],true);
        GameObjectSetActive(m_rightSelectTb[i],true);
        UIHelper.SetLabelTxt(lbl_leftName,m_clubTb[i].clubName);
        UIHelper.SetLabelTxt(lbl_rightName,m_clubTb[i+7].clubName);
        Util.SetUITexture(spr_leftClub,LuaConst.Const.ClubIcon,m_clubTb[i].clubIcon,false);
        Util.SetUITexture(spr_rightClub,LuaConst.Const.ClubIcon,m_clubTb[i+7].clubIcon,false);
        return;
    end

    if math.mod(i,2) == 0 then
        GameObjectSetActive(m_leftSelectTb[i-1],true);
        GameObjectSetActive(m_leftSelectTb[i],true);
        UIHelper.SetLabelTxt(lbl_leftName,m_clubTb[i-1].clubName);
        UIHelper.SetLabelTxt(lbl_rightName,m_clubTb[i].clubName);
                Util.SetUITexture(spr_leftClub,LuaConst.Const.ClubIcon,m_clubTb[i-1].clubIcon,false);
        Util.SetUITexture(spr_rightClub,LuaConst.Const.ClubIcon,m_clubTb[i].clubIcon,false);
    else
        GameObjectSetActive(m_leftSelectTb[i],true);
        GameObjectSetActive(m_leftSelectTb[i+1],true);
        UIHelper.SetLabelTxt(lbl_leftName,m_clubTb[i].clubName);
        UIHelper.SetLabelTxt(lbl_rightName,m_clubTb[i+1].clubName);
                Util.SetUITexture(spr_leftClub,LuaConst.Const.ClubIcon,m_clubTb[i].clubIcon,false);
        Util.SetUITexture(spr_rightClub,LuaConst.Const.ClubIcon,m_clubTb[i+1].clubIcon,false);
    end

end

function ClickTargetRight(index_)
    SetTarget(true);
    local i = index_;
    local m_clubTb = PeakRoadData.GetCurrClubTb();
    if i == 7 then
        GameObjectSetActive(m_leftSelectTb[i],true);
        GameObjectSetActive(m_rightSelectTb[i],true);
        UIHelper.SetLabelTxt(lbl_leftName,m_clubTb[i].clubName);
        UIHelper.SetLabelTxt(lbl_rightName,m_clubTb[i+7].clubName);
                Util.SetUITexture(spr_leftClub,LuaConst.Const.ClubIcon,m_clubTb[i].clubIcon,false);
        Util.SetUITexture(spr_rightClub,LuaConst.Const.ClubIcon,m_clubTb[i+7].clubIcon,false);
        return;
    end

    if math.mod(i,2) == 0 then
        GameObjectSetActive(m_rightSelectTb[i-1],true);
        GameObjectSetActive(m_rightSelectTb[i],true);
        UIHelper.SetLabelTxt(lbl_leftName,m_clubTb[i-1+7].clubName);
        UIHelper.SetLabelTxt(lbl_rightName,m_clubTb[i+7].clubName);
                Util.SetUITexture(spr_leftClub,LuaConst.Const.ClubIcon,m_clubTb[i+6].clubIcon,false);
        Util.SetUITexture(spr_rightClub,LuaConst.Const.ClubIcon,m_clubTb[i+7].clubIcon,false);
    else
        GameObjectSetActive(m_rightSelectTb[i],true);
        GameObjectSetActive(m_rightSelectTb[i+1],true);
        UIHelper.SetLabelTxt(lbl_leftName,m_clubTb[i+7].clubName);
        UIHelper.SetLabelTxt(lbl_rightName,m_clubTb[i+1+7].clubName);
                Util.SetUITexture(spr_leftClub,LuaConst.Const.ClubIcon,m_clubTb[i+7].clubIcon,false);
        Util.SetUITexture(spr_rightClub,LuaConst.Const.ClubIcon,m_clubTb[i+8].clubIcon,false);
    end

end

function SetTarget(args_)
    GameObjectSetActive(obj_leftItem,args_);
    GameObjectSetActive(obj_rightItem,args_);
end

function ResetSelectUI()
    for i=1,7 do
        GameObjectSetActive(m_leftSelectTb[i],false);
        GameObjectSetActive(m_rightSelectTb[i],false);
    end

end

function BtnLeftPlayer(args_)
    local index = tonumber(args_.name);
    if GetCurrAdv() == 1 then
        if index > 4 then
            return;
        end
    elseif GetCurrAdv() == 2 then
        if index > 6 then
            return;
        end
    end

    ResetSelectUI();
    ClickTargetLeft(index);
end

function BtnRightPlayer(args_)
    local index = tonumber(args_.name);
    if GetCurrAdv() == 1 then
        if index > 4 then
            return;
        end
    elseif GetCurrAdv() == 2 then
        if index > 6 then
            return;
        end
    end

    ResetSelectUI();
    ClickTargetRight(index);
end

function IsCurrFinish()
    if PeakRoadData.GetCurrFloorIndex() ~= 0 and math.mod(PeakRoadData.GetCurrFloorIndex(),3) == 0 then
        GameObjectSetActive(btn_challenge,false);
        GameObjectSetActive(lbl_finishChallenge,true);
        return true
    else
        GameObjectSetActive(btn_challenge,true);
        GameObjectSetActive(lbl_finishChallenge,false);
    end

    return false;
end

function SetAdvTitle()
    GameObjectSetActive(btn_challenge,true);
    GameObjectSetActive(lbl_finishChallenge,false);
    if m_currAdv == 1 then
        UIHelper.SetLabelTxt(lbl_challenge,string.format(Util.LocalizeString("UIPeakRoad_PeakFinals"),"1/4"));
    elseif m_currAdv == 2 then
        UIHelper.SetLabelTxt(lbl_challenge,Util.LocalizeString("UIPeakRoad_2PeakFinals"));
    elseif m_currAdv == 3 then
        UIHelper.SetLabelTxt(lbl_challenge,Util.LocalizeString("UIPeakRoad_1PeakFinals"));
    end

end

function GetCurrAdv()
    local adv = math.mod((PeakRoadData.GetCurrFloorIndex()+1),3);
    if adv == 0 then
        adv = 3;
    end

    return adv;
end

function OnShow()


end

function OnHide()


end

function OnDestroy()
    window = nil;
    windowComponent = nil;

end

function ExitUIPeakRoadSub()

    windowComponent:Close();
end