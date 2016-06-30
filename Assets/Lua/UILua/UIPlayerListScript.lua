--region NewFile_1.lua
--Author : hongpeng.zhao
--Date   : 2015/3/6
--此文件由[BabeLua]插件自动生成



--endregion


module("UIPlayerListScript", package.seeall)

require "Game/ItemSys"
require "Game/PlayerInfoData"

local window = nil;
local windowComponent = nil;
local windowLiving = nil;
local windowChips = nil;
local m_scrollView = nil;
local m_selectFire = nil;


local m_currClickPlayerId = nil;
local m_currPlayerIndex = nil;

local enumStatus = {Living="Living",Fire="Fire",Chips="Chips"};
local enumPlayerType = {All="All",Forward="Forward",Midfielder="Midfielder",Defender="Defender",Goalkeeper="Goalkeeper"};
local m_livingIdTb = {};
local m_fireIdTb = {};
local m_fireIdSelectTb = {};
local m_fireTFselectTb = {};
local m_sortLivingIdTb = {};
local m_sortFireIdTb = {};
local m_sortChipsIdTb = {};
local allChipsTb = {};

local m_currStatus = enumStatus.Living;
local m_currPlayerType = enumPlayerType.All;
function OnStart(gameObject, params)
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");
    SortPlayerIdTb();    
--    SortChipsIdTb();
    OnRefreshItemData();
    BindUI();

    RegisterMsgCallback();
end

function RegisterMsgCallback()
    SynSys.RegisterCallback("hero",OnRefreshHeroData);
    SynSys.RegisterCallback("item",OnRefreshItemData);
end

function UnRegisterMsgCallback()
    SynSys.UnRegisterCallback("hero",OnRefreshHeroData);
    SynSys.UnRegisterCallback("item",OnRefreshItemData);
end

function OnRefreshHeroData()
    SortPlayerIdTb();    
    SortChipsIdTb();
end

function OnRefreshItemData()
    allChipsTb = ItemSys.FilterItemByTab(3);
    for k,v in pairs(allChipsTb) do
        if Hero.IsContainPlayer(GetNeedPlayerId(k)) then
            allChipsTb[k] = nil;
        end
    end
end

function BindUI()
    local transform = window.transform;
    windowLiving = windowComponent:GetPrefab("PlayersItem");
    windowChips = windowComponent:GetPrefab("ChipsItem");

    m_scrollView = TransformFindChild(transform,"Center/ScrollViewPanel");
    m_selectFire = TransformFindChild(transform, "Center/BottomPart");
    GameObjectSetActive(m_selectFire,false);

    -- Toggle
    Util.AddClick(TransformFindChild(transform, "Center/Toggle/Living").gameObject,ToggleLiving);
    Util.AddClick(TransformFindChild(transform, "Center/Toggle/Fire").gameObject,ToggleFire);
    Util.AddClick(TransformFindChild(transform, "Center/Toggle/Chip").gameObject,ToggleChips);

    -- Button
    Util.AddClick(TransformFindChild(transform, "Center/Toggle/BtnAll").gameObject,BtnAll);
    Util.AddClick(TransformFindChild(transform, "Center/BottomPart/BtnCommitFire").gameObject,BtnCommitFire);
    Util.AddClick(TransformFindChild(transform, "Center/BottomPart/BtnCancelFire").gameObject,BtnCancelFire);
    Util.AddClick(TransformFindChild(transform, "Center/BottomPart/BtnShopFire").gameObject,BtnShopFire);

    InitLivingUI(true);
end

function InitLivingUI(first_)
    m_livingIdTb = {};
    GameObjectSetActive(m_selectFire,false);
    if not first_ then
        UIHelper.DestroyGrid(m_scrollView);
    end

    local m_playerLivingCount = GetPlayerLivingCount();
    local m_playerLivingCountSub = GetPlayerlivingCountSub();

    local index = 0;
    for i=1,m_playerLivingCount do
        local clone = InstantiatePrefab(windowLiving,m_scrollView).transform;
        clone.name = i;
        local btnItem1 = TransformFindChild(clone, "1").gameObject;
        Util.AddClick(btnItem1, BtnItemLiving);
        index = index + 1;
        SetLivingDataUI(i,clone,1,GetId2Index(m_sortLivingIdTb,index));
        local btnItem2 = TransformFindChild(clone, "2").gameObject;
        Util.AddClick(btnItem2, BtnItemLiving);
        index = index + 1;
        SetLivingDataUI(i,clone,2,GetId2Index(m_sortLivingIdTb,index));
        local btnItem3 = TransformFindChild(clone, "3").gameObject;
        Util.AddClick(btnItem3, BtnItemLiving);
        index = index + 1;
        SetLivingDataUI(i,clone,3,GetId2Index(m_sortLivingIdTb,index));
        if i == m_playerLivingCount then
            for i=(m_playerLivingCountSub+1),3 do 
                GameObjectSetActive(TransformFindChild(clone, i),false);
            end
        end
    end

    if not first_ then
        UIHelper.RepositionGrid(m_scrollView,m_scrollView);
        UIHelper.RefreshPanel(m_scrollView);
    end

end
function SetLivingDataUI(i,transform,uiId,playerId)
     if tonumber(playerId) > 0 then
        local spr_mainPlayer = TransformFindChild(transform, uiId.."/lbl_lv/spr_mainPlayer");

        table.insert(m_livingIdTb,playerId);
        UIHelper.SetLabelTxt(TransformFindChild(transform, uiId.."/lbl_lv"), "Lv."..HeroData.GetHeroLv(playerId));
        UIHelper.SetLabelTxt(TransformFindChild(transform, uiId.."/lbl_name"),Hero.ColorHeroName(Hero.GetCurrQuality(playerId),HeroData.GetHeroName(playerId)));
        UIHelper.SetLabelTxt(TransformFindChild(transform, uiId.."/PosLabel/Root/ValueLabel"),string.format(Util.LocalizeString("UIPlayerList_BattleValue"), HeroData.GetBattleScore(playerId)));
        UIHelper.SetWidgetColor(TransformFindChild(transform, uiId.."/FxImage"),Hero.GetHeroQualityBgColor(Hero.GetCurrQuality(playerId)));
        UIHelper.SetWidgetColor(TransformFindChild(transform, uiId.."/Banner/Bg"),Hero.GetHeroQualityBannerBg(Hero.GetCurrQuality(playerId)));
        UIHelper.SetSpriteNameNoPerfect(TransformFindChild(transform, uiId.."/Banner/spr_Level"),HeroData.GetHeroRatingName(playerId));
        UIHelper.SetSpriteNameNoPerfect(TransformFindChild(transform, uiId.."/Banner/Status"),Hero.GetHeroStatusName(playerId));
        UIHelper.SetLabelTxt(TransformFindChild(transform, uiId.."/Banner/Label"),"("..HeroData.GetHeroRating(playerId)..")");
        Util.SetUITexture(TransformFindChild(transform, uiId.."/Icon"),LuaConst.Const.PlayerHeadIcon,HeroData.GetHeroIcon(playerId), false);
        if Hero.IsMainPlayer(playerId) then
            GameObjectSetActive(spr_mainPlayer,true);
        else
            GameObjectSetActive(spr_mainPlayer,false);
        end
        UIHelper.SetLabelTxt(TransformFindChild(transform, uiId.."/PosLabel"), HeroData.GetHeroPosName(playerId));
        InitStars(TransformFindChild(transform, uiId.."/Banner/Stars"),playerId);
    end

end

function InitFireUI()
    m_fireIdTb = {};
    SetSelectFireCount(true);

    GameObjectSetActive(m_selectFire,true);
    UIHelper.DestroyGrid(m_scrollView);

    local m_playerFireCount = GetPlayerFireCount();
    local m_playerFireCountSub = GetPlayerFireCountSub();

    local index = 0;
    for i=1,m_playerFireCount do
        local clone = InstantiatePrefab(windowLiving,m_scrollView).transform;
        clone.name = i;
        local btnItem1 = TransformFindChild(clone,"1").gameObject;
        Util.AddClick(btnItem1, BtnItemFire);
        index = index + 1;
        SetFireDataUI(i,clone,1,GetId2Index(m_sortFireIdTb,index));
        local btnItem2 = TransformFindChild(clone,"2").gameObject;
        Util.AddClick(btnItem2, BtnItemFire);
        index = index + 1;
        SetFireDataUI(i,clone,2,GetId2Index(m_sortFireIdTb,index));
        local btnItem3 = TransformFindChild(clone,"3").gameObject;
        Util.AddClick(btnItem3, BtnItemFire);
        index = index + 1;
        SetFireDataUI(i,clone,3,GetId2Index(m_sortFireIdTb,index));
        if i == m_playerFireCount then
            for i=(m_playerFireCountSub+1),3 do 
                GameObjectSetActive(TransformFindChild(clone, i),false);
            end
        end
    end

    UIHelper.RepositionGrid(m_scrollView,m_scrollView);
    UIHelper.RefreshPanel(m_scrollView);
end

function SetFireDataUI(i,transform,uiId,playerId)
     if tonumber(playerId) > 0 then
        table.insert(m_fireIdTb,playerId);
        UIHelper.SetLabelTxt(TransformFindChild(transform, uiId.."/lbl_lv"), "Lv."..HeroData.GetHeroLv(playerId));
        UIHelper.SetLabelTxt(TransformFindChild(transform, uiId.."/lbl_name"),Hero.ColorHeroName(Hero.GetCurrQuality(playerId),HeroData.GetHeroName(playerId)));
        UIHelper.SetLabelTxt(TransformFindChild(transform, uiId.."/PosLabel/Root/ValueLabel"),string.format(Util.LocalizeString("UIPlayerList_BattleValue"), HeroData.GetBattleScore(playerId)));
        UIHelper.SetWidgetColor(TransformFindChild(transform, uiId.."/FxImage"),Hero.GetHeroQualityBgColor(Hero.GetCurrQuality(playerId)));
        UIHelper.SetWidgetColor(TransformFindChild(transform, uiId.."/Banner/Bg"),Hero.GetHeroQualityBannerBg(Hero.GetCurrQuality(playerId)));
        UIHelper.SetSpriteNameNoPerfect(TransformFindChild(transform, uiId.."/Banner/spr_Level"),HeroData.GetHeroRatingName(playerId));
        UIHelper.SetSpriteNameNoPerfect(TransformFindChild(transform, uiId.."/Banner/Status"),Hero.GetHeroStatusName(playerId));
        UIHelper.SetLabelTxt(TransformFindChild(transform, uiId.."/Banner/Label"),"("..HeroData.GetHeroRating(playerId)..")");        
        Util.SetUITexture(TransformFindChild(transform, uiId.."/Icon"),LuaConst.Const.PlayerHeadIcon,HeroData.GetHeroIcon(playerId), false);
        GameObjectSetActive(TransformFindChild(transform, uiId.."/lbl_lv/spr_mainPlayer"),false);

        UIHelper.SetLabelTxt(TransformFindChild(transform, uiId.."/PosLabel"), HeroData.GetHeroPosName(playerId));
        InitStars(TransformFindChild(transform, uiId.."/Banner/Stars"),playerId);
    end

end

function InitChipsUI()
    GameObjectSetActive(m_selectFire,false);
    UIHelper.DestroyGrid(m_scrollView);
    local m_playerChipsCount = GetChipsCount();
    local m_playerChipsCountSub = GetChipsCountSub();
--    GreenPrint(m_playerChipsCount.."/"..m_playerChipsCountSub)
    local index = 0;
    for i=1,m_playerChipsCount do
        local clone = InstantiatePrefab(windowChips,m_scrollView).transform;
        clone.name = i;
        Util.AddClick(TransformFindChild(clone, "1/Banner/Chips/BtnChips").gameObject, BtnChips);
        Util.AddClick(TransformFindChild(clone, "1").gameObject,BtnItemChips);
        index = index +1;
        SetChipsDataUI(i,index,clone,1);
        Util.AddClick(TransformFindChild(clone, "2/Banner/Chips/BtnChips").gameObject, BtnChips);
        Util.AddClick(TransformFindChild(clone, "2").gameObject,BtnItemChips);
        index = index +1;
        SetChipsDataUI(i,index,clone,2);
        Util.AddClick(TransformFindChild(clone, "3/Banner/Chips/BtnChips").gameObject, BtnChips);
        Util.AddClick(TransformFindChild(clone, "3").gameObject,BtnItemChips);
        index = index +1;
        SetChipsDataUI(i,index,clone,3);
        if i == m_playerChipsCount then
            for i=(m_playerChipsCountSub+1),3 do 
                GameObjectSetActive(TransformFindChild(clone, i),false);
            end
        end
    end

    UIHelper.RepositionGrid(m_scrollView,m_scrollView);
    UIHelper.RefreshPanel(m_scrollView);
end

function SetChipsDataUI(i,index,transform,uiId)
    if index <= #m_sortChipsIdTb then
        local floatChips,strChips = GetPrgChips(m_sortChipsIdTb[index].chipId);
        local active = TransformFindChild(transform, uiId.."/Active");
        if floatChips >= 1 then
            GameObjectSetActive(active,true);
        else
            GameObjectSetActive(active,false);
        end
        UIHelper.SetLabelTxt(TransformFindChild(transform, uiId.."/lbl_name"),HeroData.GetHeroName(m_sortChipsIdTb[index].playerId));
        UIHelper.SetLabelTxt(TransformFindChild(transform, uiId.."/Banner/Chips/prg_chips/Label"),strChips);
        UIHelper.SetProgressBar(TransformFindChild(transform, uiId.."/Banner/Chips/prg_chips"),floatChips);
        UIHelper.SetSpriteNameNoPerfect(TransformFindChild(transform, uiId.."/Banner/spr_Level"),HeroData.GetHeroRatingName(m_sortChipsIdTb[index].playerId))
        UIHelper.SetLabelTxt(TransformFindChild(transform, uiId.."/PosLabel"),HeroData.GetHeroPosName(m_sortChipsIdTb[index].playerId));
        Util.SetUITexture(TransformFindChild(transform, uiId.."/Icon"),LuaConst.Const.PlayerHeadIcon,HeroData.GetHeroIcon(m_sortChipsIdTb[index].playerId), true);
    end 
     
end

function BtnItemLiving(GameObject_)
    local index = GameObject_.transform.parent.name - 1;
    m_currPlayerIndex = index*3+GameObject_.name;
    m_currClickPlayerId = m_livingIdTb[m_currPlayerIndex];

    PlayerInfoData.SetCurrPlayerId(m_currClickPlayerId);
    ShowPlayerAttribute();
end

function BtnItemFire(GameObject_)
    local index = GameObject_.transform.parent.name - 1;
    local id = m_fireIdTb[index*3 + GameObject_.name];

    local hight = TransformFindChild(GameObject_.transform,"Selected");
    if GameObjectActiveSelf(hight.gameObject) then
        GameObjectSetActive(hight,false);
    else
        GameObjectSetActive(hight,true); 
    end

    DoSelectFireId(id,GameObject_.transform);

   
    SetSelectFireCount(false);
end

function OnChipsProSuccess(data_)
    SortChipsIdTb();
    SortPlayerIdTb();

    InitChipsUI();
end

function BtnChips(GameObject_)
--    local index = GameObject_.transform.parent.parent.parent.parent.name;
--    local index_ = GameObject_.transform.parent.parent.parent.name;
--    local chipId = m_sortChipsIdTb[index*3 +index_].chipId;
--    local playerId = m_sortChipsIdTb[index*3 +index_].playerId;
--    print("Chips : "..chipId.." Player: "..playerId);
--    if IsEnoughMoney(playerId) then
--        Hero.ReqHeroPro(playerId,OnChipsProSuccess);
--    else
--        local msg = Util.LocalizeString("NoEnoughtGold");
--        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { msg });
--    end

end

function BtnItemChips(GameObject_)
    local index = GameObject_.transform.parent.name - 1;
    local index_ = GameObject_.name;
    local chipId = m_sortChipsIdTb[index*3 +index_].chipId;
    local playerId = m_sortChipsIdTb[index*3 +index_].playerId;
    if not IsEnoughChips(chipId) then
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("UIPlayerList_NoEnoughtChips") });
        return;
    end

    if not IsEnoughMoney(playerId) then
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("UIPlayerList_NoEnoughtGold") });
        return;
    end
    local MsgBoxSure = function()
        Hero.ReqHeroPro(playerId,OnChipsProSuccess);
    end

    local costMoney = Config.GetProperty(Config.HeroSlvTable(), tostring(Hero.GetOriginStars(playerId)),'subMoney');
    WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxConfirm, { string.format(Util.LocalizeString("UIPlayerList_CostChips"),costMoney),false, MsgBoxSure});

end


function BtnCommitFire()
    if #m_fireIdSelectTb == 0 then
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("UIPlayerList_EmptyFire") });
        return;
    end

    local strPlayerId = nil;
    for k,v in pairs(m_fireIdSelectTb) do
        if strPlayerId ~= nil then
            strPlayerId = strPlayerId..","..v;
        else
            strPlayerId = v;
        end
    end

    Hero.ReqHeroDis(strPlayerId,OnReqHeroDis);
    SetSelectFireCount(true) -- Reset
end
function BtnCancelFire()
    for i=1,#m_fireTFselectTb do
        local hight = TransformFindChild(m_fireTFselectTb[i],"Selected");
        if GameObjectActiveSelf(hight.gameObject) then
            GameObjectSetActive(hight,false);
        else
            GameObjectSetActive(hight,true); 
        end
    end

    SetSelectFireCount(true) -- Reset
end

function BtnShopFire(args)
    WindowMgr.ShowWindow(LuaConst.Const.UIShop,{shopType = UIShopSettings.ShopType.DismissShop});    
end

function OnReqHeroDis(data_)

    SortPlayerIdTb(true);
    InitFireUI();
end

function ToggleLiving(obj_)
    if m_currStatus ~= enumStatus.Living then
        local spr_togglePos = TransformFindChild(window.transform,"Center/Toggle/Pos");
        m_currStatus = enumStatus.Living;
        GameObjectLocalPostion(spr_togglePos.transform,NewVector3(obj_.transform.localPosition.x,spr_togglePos.transform.localPosition.y,0));

        SortPlayerIdTb();
        InitLivingUI();
    end

end

function ToggleFire(obj_)
    if m_currStatus ~= enumStatus.Fire then
        local spr_togglePos = TransformFindChild(window.transform,"Center/Toggle/Pos");
        m_currStatus = enumStatus.Fire;
        GameObjectLocalPostion(spr_togglePos.transform,NewVector3(obj_.transform.localPosition.x,spr_togglePos.transform.localPosition.y,0));

        SortPlayerIdTb(true);
        InitFireUI();
    end

end

function ToggleChips(obj_)
    if m_currStatus ~= enumStatus.Chips then
        local spr_togglePos = TransformFindChild(window.transform,"Center/Toggle/Pos");
        m_currStatus = enumStatus.Chips;
        GameObjectLocalPostion(spr_togglePos.transform,NewVector3(obj_.transform.localPosition.x,spr_togglePos.transform.localPosition.y,0));

        SortChipsIdTb();
        InitChipsUI();
    end

end

function BtnAll()
    local tb = {};
    tb.Callback = UpdatePlayerFilter;
    WindowMgr.ShowWindow(LuaConst.Const.UIPlayerListFilter,tb);
end

function UpdatePlayerFilter(filterId_)
    SetEnumPlayerType(filterId_);

    if m_currStatus == enumStatus.Living then
        SortPlayerIdTb();
        InitLivingUI();
    elseif m_currStatus == enumStatus.Fire then
        SortPlayerIdTb(true);
        InitFireUI();
    elseif m_currStatus == enumStatus.Chips then
        SortChipsIdTb();
        InitChipsUI();
    end

    UIHelper.SetLabelTxt(TransformFindChild(window.transform, "Center/Toggle/BtnAll/Label"),Util.LocalizeString("UIPlayerList_FilterType"..filterId_));

end

function SetEnumPlayerType(index_)
    if index_ == 1 then
        m_currPlayerType = enumPlayerType.All;
    elseif index_ == 2 then
        m_currPlayerType = enumPlayerType.Forward;
    elseif index_ == 3 then
        m_currPlayerType = enumPlayerType.Midfielder;
    elseif index_ == 4 then
        m_currPlayerType = enumPlayerType.Defender;
    elseif index_ == 5 then
        m_currPlayerType = enumPlayerType.Goalkeeper;
    end

end

function InitStars(transform_,playerId_)
    for i=1,5 do
        local light = TransformFindChild(transform_, i.."/Light");
        GameObjectSetActive(light,false);
    end
    for i=1,Hero.GetCurrStars(playerId_) do
        local light = TransformFindChild(transform_, i.."/Light");
        local dark = TransformFindChild(transform_, i.."/Dark");
        GameObjectSetActive(dark,false);
        GameObjectSetActive(light,true);
    end 

end

--
function SetSelectFireCount(isReset_)
    local m_lblSelectFire = TransformFindChild(window.transform, "Center/BottomPart/lbl_selectNum");
    if isReset_ then
        m_fireIdSelectTb = {};
        m_fireTFselectTb = {};
        UIHelper.SetLabelTxt(m_lblSelectFire,0);
    else
        UIHelper.SetLabelTxt(m_lblSelectFire,#m_fireIdSelectTb);
    end


end

function DoSelectFireId(id_,tf_)
    local bHave = false;
    for i=1,#m_fireIdSelectTb do
        if m_fireIdSelectTb[i] == id_ then
            table.remove(m_fireIdSelectTb,i);
            table.remove(m_fireTFselectTb,i);
            bHave = true;
            break;
        end
    end

    if not bHave then
        table.insert(m_fireIdSelectTb,id_);
        table.insert(m_fireTFselectTb,tf_);
    end
end


--
function ShowPlayerAttribute()
    WindowMgr.ShowWindow(LuaConst.Const.UIPlayerInfoBase);
end

function ShowPlayerList()
    if window ~= nil then
        GameObjectSetActive(window.transform,true);
        InitPlayerLiving();
    end

end



-- 
function GetPlayerLivingCount()
    local heroListTb = Hero.GetHeroList();
    local intCout = 0;
    if m_currPlayerType == enumPlayerType.All then
        intCout = #heroListTb;
    elseif m_currPlayerType == enumPlayerType.Forward then -- 1-9
        for k,v in pairs(heroListTb) do
            local pos = tonumber(Config.GetProperty(Config.HeroTable(),v.id,"ipos"));
            if pos >= 1 and pos <= 9 then
                intCout = intCout+1;
            end
        end
    elseif m_currPlayerType == enumPlayerType.Midfielder then  -- 10-20
       for k,v in pairs(heroListTb) do
            local pos = tonumber(Config.GetProperty(Config.HeroTable(),v.id,"ipos"));
            if pos >= 10 and pos <= 20 then
                intCout = intCout+1;
            end
        end
    elseif m_currPlayerType == enumPlayerType.Defender then -- 21-28
        for k,v in pairs(heroListTb) do
            local pos = tonumber(Config.GetProperty(Config.HeroTable(),v.id,"ipos"));
            if pos >= 21 and pos <= 28 then
                intCout = intCout+1;
            end
        end
    elseif m_currPlayerType == enumPlayerType.Goalkeeper then -- 29
        for k,v in pairs(heroListTb) do
            local pos = tonumber(Config.GetProperty(Config.HeroTable(),v.id,"ipos"));
            if pos == 29 then
                intCout = intCout+1;
            end
        end
    end

    return math.ceil(intCout/3),intCout;
end

function GetPlayerlivingCountSub()
    local x,y = GetPlayerLivingCount();
    local  sub = 3 - (x*3 - y);
    if sub <= 0 then
        sub = 3;
    end

    return sub;
end

function GetPlayerFireCount()
    local heroListTb = Hero.GetHeroList();
    local mainHeroListTb = Hero.MainTeamHeroId();
    local intCout = 0;
    if m_currPlayerType == enumPlayerType.All then
        intCout = #heroListTb - #mainHeroListTb;
    elseif m_currPlayerType == enumPlayerType.Forward then -- 1-9
        for k,v in pairs(heroListTb) do
            local pos = tonumber(Config.GetProperty(Config.HeroTable(),v.id,"ipos"));
            if pos >= 1 and pos <= 9 then
                intCout = intCout+1;
            end
        end

        for k,v in pairs(mainHeroListTb) do
            local pos = tonumber(Config.GetProperty(Config.HeroTable(),v,"ipos"));
            if pos >= 1 and pos <= 9 then
                intCout = intCout-1;
            end
        end
    elseif m_currPlayerType == enumPlayerType.Midfielder then  -- 10-20
       for k,v in pairs(heroListTb) do
            local pos = tonumber(Config.GetProperty(Config.HeroTable(),v.id,"ipos"));
            if pos >= 10 and pos <= 20 then
                intCout = intCout+1;
            end
        end

        for k,v in pairs(mainHeroListTb) do
            local pos = tonumber(Config.GetProperty(Config.HeroTable(),v,"ipos"));
            if pos >= 10 and pos <= 20 then
                intCout = intCout-1;
            end
        end
    elseif m_currPlayerType == enumPlayerType.Defender then -- 21-28
        for k,v in pairs(heroListTb) do
            local pos = tonumber(Config.GetProperty(Config.HeroTable(),v.id,"ipos"));
            if pos >= 21 and pos <= 28 then
                intCout = intCout+1;
            end
        end

        for k,v in pairs(mainHeroListTb) do
            local pos = tonumber(Config.GetProperty(Config.HeroTable(),v,"ipos"));
            if pos >= 21 and pos <= 28 then
                intCout = intCout-1;
            end
        end
    elseif m_currPlayerType == enumPlayerType.Goalkeeper then -- 29
        for k,v in pairs(heroListTb) do
            local pos = tonumber(Config.GetProperty(Config.HeroTable(),v.id,"ipos"));
            if pos == 29 then
                intCout = intCout+1;
            end
        end

       for k,v in pairs(mainHeroListTb) do
            local pos = tonumber(Config.GetProperty(Config.HeroTable(),v,"ipos"));
            if pos == 29 then
                intCout = intCout-1;
            end
        end
    end

    return math.ceil(intCout/3),intCout;

end

function GetPlayerFireCountSub()
    local x,y = GetPlayerFireCount();
    local sub = math.fmod((3 - (x*3 - y)),3)
    if sub == 0 then
        sub = 3;
    end

    return  sub;
end

function GetChipsCount()
    local intCout = 0;
    if m_currPlayerType == enumPlayerType.All then
        intCout = #m_sortChipsIdTb;
    elseif m_currPlayerType == enumPlayerType.Forward then -- 1-9
        for k,v in pairs(m_sortChipsIdTb) do
            local pos = tonumber(Config.GetProperty(Config.HeroTable(),v.playerId,"ipos"));
            if pos >= 1 and pos <= 9 then
                intCout = intCout+1;
            end
        end
    elseif m_currPlayerType == enumPlayerType.Midfielder then  -- 10-20
       for k,v in pairs(m_sortChipsIdTb) do
            local pos = tonumber(Config.GetProperty(Config.HeroTable(),v.playerId,"ipos"));
            if pos >= 10 and pos <= 20 then
                intCout = intCout+1;
            end
        end
    elseif m_currPlayerType == enumPlayerType.Defender then -- 21-28
        for k,v in pairs(m_sortChipsIdTb) do
            local pos = tonumber(Config.GetProperty(Config.HeroTable(),v.playerId,"ipos"));
            if pos >= 21 and pos <= 28 then
                intCout = intCout+1;
            end
        end
    elseif m_currPlayerType == enumPlayerType.Goalkeeper then -- 29
        for k,v in pairs(m_sortChipsIdTb) do
            local pos = tonumber(Config.GetProperty(Config.HeroTable(),v.playerId,"ipos"));
            if pos == 29 then
                intCout = intCout+1;
            end
        end
    end

    return math.ceil(intCout/3),intCout;

end

function GetChipsCountSub()
    local x,y = GetChipsCount();
    return 3 - (x*3 - y);
end

function GetPrgChips(chipId_)
    local currCount = allChipsTb[chipId_];
    local maxCount = GetNeedChipsMax((GetNeedPlayerId(chipId_)));
    return currCount/maxCount,currCount.."/"..maxCount;
end

function SortChipsIdTb()
    local chipsTB = allChipsTb;
    m_sortChipsIdTb = {};
    for k,v in pairs(chipsTB) do
        if m_currPlayerType == enumPlayerType.All then
            local chip = {chipId = k, chipCount = v, playerId = GetNeedPlayerId(k),chipRating = tonumber(HeroData.GetHeroRating(GetNeedPlayerId(k)))};
            table.insert(m_sortChipsIdTb,chip);
        elseif m_currPlayerType == enumPlayerType.Forward then
            local pos = tonumber(Config.GetProperty(Config.HeroTable(),GetNeedPlayerId(k),"ipos"));
            if pos >= 1 and pos <= 9 then
                local chip = {chipId = k, chipCount = v, playerId = GetNeedPlayerId(k),chipRating = tonumber(HeroData.GetHeroRating(GetNeedPlayerId(k)))};
                table.insert(m_sortChipsIdTb,chip);
            end
        elseif m_currPlayerType == enumPlayerType.Midfielder then
            local pos = tonumber(Config.GetProperty(Config.HeroTable(),GetNeedPlayerId(k),"ipos"));
            if pos >= 10 and pos <= 20 then
                local chip = {chipId = k, chipCount = v, playerId = GetNeedPlayerId(k),chipRating = tonumber(HeroData.GetHeroRating(GetNeedPlayerId(k)))};
                table.insert(m_sortChipsIdTb,chip);
            end
        elseif m_currPlayerType == enumPlayerType.Defender then
            local pos = tonumber(Config.GetProperty(Config.HeroTable(),GetNeedPlayerId(k),"ipos"));
            if pos >= 21 and pos <= 28 then
                local chip = {chipId = k, chipCount = v, playerId = GetNeedPlayerId(k),chipRating = tonumber(HeroData.GetHeroRating(GetNeedPlayerId(k)))};
                table.insert(m_sortChipsIdTb,chip);
            end
        elseif m_currPlayerType == enumPlayerType.Goalkeeper then
            local pos = tonumber(Config.GetProperty(Config.HeroTable(),GetNeedPlayerId(k),"ipos"));
            if pos == 29 then
                local chip = {chipId = k, chipCount = v, playerId = GetNeedPlayerId(k),chipRating = tonumber(HeroData.GetHeroRating(GetNeedPlayerId(k)))};
                table.insert(m_sortChipsIdTb,chip);
            end
        end
    end

    table.sort(m_sortChipsIdTb,function(x,y) return x.chipCount > y.chipCount end);

end

function SortPlayerIdTb(isFire_)
    local size = Hero.GetHeroAmonut();
    m_sortLivingIdTb = {};
    m_sortFireIdTb = {};

    for i=1,size do
        local temp = {};
        temp.id = Hero.GetHeroId2Index(i);
        if temp.id ~= nil then
            if m_currPlayerType == enumPlayerType.All then
                temp.value = tonumber(Hero.GetHeroData2Id(temp.id).power);
                temp.rating = tonumber(HeroData.GetHeroRating(temp.id));
                table.insert(m_sortLivingIdTb,temp);
                table.insert(m_sortFireIdTb,temp);
            elseif m_currPlayerType == enumPlayerType.Forward then
                local pos = tonumber(Config.GetProperty(Config.HeroTable(),temp.id,"ipos"));
                if pos >= 1 and pos <= 9 then
                    temp.value = tonumber(Hero.GetHeroData2Id(temp.id).power);
                    temp.rating = tonumber(HeroData.GetHeroRating(temp.id));
                    table.insert(m_sortLivingIdTb,temp);
                    table.insert(m_sortFireIdTb,temp);
                end
            elseif m_currPlayerType == enumPlayerType.Midfielder then
                local pos = tonumber(Config.GetProperty(Config.HeroTable(),temp.id,"ipos"));
                if pos >= 10 and pos <= 20 then
                    temp.value = tonumber(Hero.GetHeroData2Id(temp.id).power);
                    temp.rating = tonumber(HeroData.GetHeroRating(temp.id));
                    table.insert(m_sortLivingIdTb,temp);
                    table.insert(m_sortFireIdTb,temp);
                end
            elseif m_currPlayerType == enumPlayerType.Defender then
                local pos = tonumber(Config.GetProperty(Config.HeroTable(),temp.id,"ipos"));
                if pos >= 21 and pos <= 28 then
                    temp.value = tonumber(Hero.GetHeroData2Id(temp.id).power);
                    temp.rating = tonumber(HeroData.GetHeroRating(temp.id));
                    table.insert(m_sortLivingIdTb,temp);
                    table.insert(m_sortFireIdTb,temp);
                end
            elseif m_currPlayerType == enumPlayerType.Goalkeeper then
                local pos = tonumber(Config.GetProperty(Config.HeroTable(),temp.id,"ipos"));
                if pos == 29 then
                    temp.value = tonumber(Hero.GetHeroData2Id(temp.id).power);
                    temp.rating = tonumber(HeroData.GetHeroRating(temp.id));
                    table.insert(m_sortLivingIdTb,temp);
                    table.insert(m_sortFireIdTb,temp);
                end
            end
        end
    end


    table.sort(m_sortLivingIdTb,function(x,y) return x.value > y.value end);

    if isFire_ then
        local  keyTb = {};
        for k,v in pairs(Hero.MainTeamHeroId()) do
            for k1,v1 in pairs(m_sortFireIdTb) do
                if v == v1.id then
                    table.remove(m_sortFireIdTb,k1);
                end
            end
        end

        table.sort(m_sortFireIdTb,function(x,y) return x.value < y.value end);
    end

end

function GetNeedPlayerId(chipId_)
    local playerId = Config.GetProperty(Config.ItemTable(),chipId_,'useArgs');
    return playerId;
end

function GetNeedChipsMax(playerId_)
--    print("GetNeedChipsMax(playerId_): "..playerId_);
    local currStar =    Hero.GetOriginStars(playerId_);
    local needChips = Config.GetProperty(Config.HeroSlvTable(), tostring(currStar),'subSoul');
    if needChips == nil then
        needChips = -1;
    end
    return needChips;
end

function IsEnoughMoney(playerId_)
    local currStar =    Hero.GetOriginStars(playerId_);
    local  needMoney = Config.GetProperty(Config.HeroSlvTable(), tostring(currStar),'subMoney');
    if tonumber(ItemSys.GetItemData(LuaConst.Const.GB).num) >= tonumber(needMoney) then
        return true;
    else
        return false;
    end

end

function IsEnoughChips(chipId_)
    local currCount = allChipsTb[chipId_];
    local maxCount = GetNeedChipsMax((GetNeedPlayerId(chipId_)));
    if currCount < maxCount then
        return false;
    end

    return true;
end

function SetCurrPlayerId(playerId_)
    m_currClickPlayerId = playerId_;
end

function GetCurrPlayerId()
    return m_currClickPlayerId;
end

function SetCurrPlayerIndex(index_)
    m_currPlayerIndex = index_;
end

function GetCurrPlayerIndex()
    return m_currPlayerIndex;
end

function GetPlayerId2Index(index_)
    if index_ <= #m_livingIdTb then
        return m_livingIdTb[index_];
    end

    return -1;
end

function GetId2Index(tb_,index_)
    if index_ <= #tb_ then
        return tb_[index_].id;
    end

    return 0;
end

function GetMaxLivingPlayer()
    return #m_livingIdTb;
end


function OnShow()
    if m_currStatus == enumStatus.Living then
        SortPlayerIdTb();
        InitLivingUI();
    elseif m_currStatus == enumStatus.Fire then
        SortPlayerIdTb(true);
        InitFireUI();
    elseif m_currStatus == enumStatus.Chips then
        SortChipsIdTb();
        InitChipsUI();
    end

    RegisterMsgCallback();
end

function OnHide()
    UnRegisterMsgCallback();
end

function OnDestroy()
    m_livingIdTb = {};
    m_fireIdTb = {};
    m_fireIdSelectTb = {};
    m_fireTFselectTb = {};

    UnRegisterMsgCallback();

    window = nil;
    windowComponent = nil;
    m_currStatus = enumStatus.Living;
    m_currPlayerType = enumPlayerType.All;

end

function ExitUIPlayerList()
    
    windowComponent:Close();
end
