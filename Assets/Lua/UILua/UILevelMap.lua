--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion

module("UILevelMap", package.seeall)

require "UILua/UIGetItemScript"
require "UILua/UIChallengeTL"

local window = nil;
local windowComponent = nil;
local windowEventIcon = nil;
local windowTeamIcon = nil;
local windowEventFx = nil;
local windowTeamFx = nil;

local m_teamFx = nil;
local m_eventFx = nil;
local pgr_tempo = nil;
local spr_mapNav = nil;
local tex_mapSmall = nil;
local m_spriteRoot = nil;
local lbl_tempoStars = nil;
local m_scrollView = nil;
local m_bossLvevlId = nil;

local m_heightCoeff = 1;
local m_widthCoeff = 1;
local m_rewardStar = {};
local m_rewardGetIcon = {};
local m_rewardGetedIcon = {};
local m_rewardFx = {};
local m_mapItemTb = {};

function OnStart(gameObject, params)
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");

    BindUI();
end

function BindUI()
    local transform = window.transform;

    windowEventIcon = windowComponent:GetPrefab("EventIcon");
    windowTeamIcon = windowComponent:GetPrefab("TeamIcon");
    windowEventFx = windowComponent:GetPrefab("EventFx");
    windowTeamFx = windowComponent:GetPrefab("TeamFx");

    m_scrollView = TransformFindChild(transform,"Center/ScrollViewPanel");
    spr_mapNav = TransformFindChild(transform,"LeftPanel/Left/Map/Nav");
    tex_mapSmall = TransformFindChild(transform,"LeftPanel/Left/Map/Map");
    m_spriteRoot = TransformFindChild(transform,"Center/ScrollViewPanel/SpriteRoot");
    lbl_tempoStars = TransformFindChild(transform,"BottomPanel/Bottom/AdvStars/Label");
    pgr_tempo = TransformFindChild(transform,"BottomPanel/Bottom/Sprite/ProgressAdv");
    
    Util.SetUITexture(tex_mapSmall,LuaConst.Const.PveMapS,Config.GetProperty(Config.RaidClassTable(),tostring(TeamLegendData.GetClickCountry()),'sMap'),true);
    Util.SetUITexture(m_spriteRoot,LuaConst.Const.PveMapB,Config.GetProperty(Config.RaidClassTable(),tostring(TeamLegendData.GetClickCountry()),'bMap'),true);
    Util.AddClick(TransformFindChild(transform, "LeftPanel/Left/Map/Left").gameObject, BtnLeftArrow);
    Util.AddClick(TransformFindChild(transform, "LeftPanel/Left/Map/Right").gameObject, BtnRightArrow);
    Util.AddClick(TransformFindChild(transform, "LeftPanel/Left/Country/Bg").gameObject, BtnBoss);

    Util.AddDragDrop(m_spriteRoot.gameObject,OnDragStart,OnDrag,OnDrop,OnDragEnd,nil);

    m_rewardStar = {};
    m_rewardGetIcon = {};
    m_rewardGetedIcon = {};
    m_rewardFx = {};
    for i=1,3 do
        table.insert(m_rewardStar,TransformFindChild(transform,"BottomPanel/Bottom/"..i.. "/Label"));
        table.insert(m_rewardGetIcon,TransformFindChild(transform,"BottomPanel/Bottom/"..i.. "/GetIcon"));
        table.insert(m_rewardGetedIcon,TransformFindChild(transform,"BottomPanel/Bottom/"..i.. "/GetedIcon"));
        table.insert(m_rewardFx,TransformFindChild(transform,"BottomPanel/Bottom/"..i.. "/Fx"));
        local btnStarReward = TransformFindChild(transform,"BottomPanel/Bottom/"..i).gameObject;
        Util.AddClick(btnStarReward, BtnStarReward);

    end

    InitMapIcon();
    RefreshReward();
end

function InitLevelMapData()
    if window ~= nil then
        InitMapIcon();
        RefreshReward();
    end

end

local  moveOffset = 1;
local counPerUnit = 1;
local tfFxTb = {};
local teamId = 0;
local eventId = 0;
function InitMapIcon()
    tfFxTb = {};
    moveOffset = (UIHelper.HeightOfWidget(m_spriteRoot) - 640)/2;
    counPerUnit = TeamLegendData.GetRaidLevelLength(TeamLegendData.GetClickCountry())/(moveOffset*2)
    m_heightCoeff = UIHelper.HeightOfWidget(TransformFindChild(window.transform,"LeftPanel/Left/Map/Bg"))/UIHelper.HeightOfWidget(m_spriteRoot);
    m_widthCoeff = UIHelper.WidthOfWidget(TransformFindChild(window.transform,"LeftPanel/Left/Map/Bg"))/UIHelper.WidthOfWidget(m_spriteRoot);

    if m_teamFx ~= nil then
        GameObjectSetActive(m_teamFx,false);
     end
    if m_eventFx ~= nil then
        GameObjectSetActive(m_eventFx,false);
     end


     local levelSize = TeamLegendData.GetRaidLevelLength(TeamLegendData.GetClickCountry());
     local isExist = false;
     if #m_mapItemTb ~= 0 then
        isExist = true;
     end

     local levelId = nil;
     local tempo = TeamLegendData.GetLevelTempo() + 1; -- +1
  
     for i=1,levelSize do
        levelId = TeamLegendData.GetLevelId(TeamLegendData.GetClickCountry(),i);
        local bFinish,starNum = TeamLegendData.IsFinishStars(levelId);
        local pos = TeamLegendData.GetIconPos(levelId);
        local isBoss = TeamLegendData.GetIsBoss(levelId);
        if isBoss == 1 then
            m_bossLvevlId = levelId;
            UIHelper.SetLabelTxt(TransformFindChild(window.transform,"LeftPanel/Left/Country/Label"),TeamLegendData.GetLevelName(levelId));
            TeamLegendData.SetClubIcon(TransformFindChild(window.transform,"LeftPanel/Left/Country/Icon"),levelId);
            local m_bossStars = TransformFindChild(window.transform,"LeftPanel/Left/Country/Stars");
            local m_bossCollider = TransformFindChild(window.transform, "LeftPanel/Left/Country/Bg");
            if bFinish then
                for i=1,starNum do
                    local light = TransformFindChild(m_bossStars,i.. "/Light");
                    GameObjectSetActive(light,true);
                end
                GameObjectSetActive(m_bossStars,true);
                UIHelper.SetBoxCollider(m_bossCollider,true);
            else
                GameObjectSetActive(m_bossStars,false);
                UIHelper.SetBoxCollider(m_bossCollider,false);
            end
        else
            if pos[1] ~= 0 and pos[2] ~= 0 then
                local clone = nil;
                local isEvent = TeamLegendData.GetIsEvent(levelId);        

                if isExist then 
                    clone = m_mapItemTb[i];
                else
                    if isEvent == 0 then
                        clone = GameObjectInstantiate(windowTeamIcon);
                    else
                        clone = GameObjectInstantiate(windowEventIcon);
                    end
                    clone.name = levelId;
                    clone.transform.parent = m_spriteRoot;
                    clone.transform.localScale = NewVector3(1,1,1);
                    clone.transform.localPosition = NewVector3(pos[1],pos[2],0);
                    Util.AddClick(clone,BtnIcon);
                    table.insert(m_mapItemTb,clone);
                end

                if isEvent == 0 then  -- No Event
                    local lock = TransformFindChild(clone.transform, "LockTeam");
                    local unlock = TransformFindChild(clone.transform, "UnLock");
                    local star = TransformFindChild(clone.transform, "UnLock/Stars");
                    if i == tempo then
                        if not TeamLegendData.GetBPlayLevelFx() then
                            if m_teamFx == nil then
                                m_teamFx = GameObjectInstantiate(windowTeamFx).transform;
                            end
                            GameObjectSetActive(m_teamFx,true);
                            m_teamFx.parent = clone.transform;
                            m_teamFx.localPosition = NewVector3(0,0,0);
                            m_teamFx.localScale = NewVector3(0.45, 0.45, 1);
                        end

                        tfFxTb.TfTeam = clone.transform;
                    end
                    if bFinish then
                        if TeamLegendData.GetBPlayLevelFx() and i >= (tempo-1) then
                            if i == tempo -1 then
                                teamId = levelId;
                                tfFxTb.TfTeamFinish = clone.transform;
                            end

                            GameObjectSetActive(lock,true);
                            GameObjectSetActive(unlock,false);
                            UIHelper.SetBoxCollider(clone.transform,false);
                        else
                            GameObjectSetActive(clone.transform,true)
                            GameObjectSetActive(lock,false);
                            GameObjectSetActive(unlock,true);
                            UIHelper.SetBoxCollider(clone.transform,true);
                            TeamLegendData.SetClubIcon(TransformFindChild(clone.transform, "UnLock/Icon/Icon"),levelId);
                            if starNum == -1 then
                                GameObjectSetActive(star,false);
                            else
                                GameObjectSetActive(star,true);
                                for i=1,3 do
                                    local light = TransformFindChild(clone.transform, "UnLock/Stars/"..i.."/Light");
                                    GameObjectSetActive(light,false);
                                end
                                for i=1,starNum do
                                    local light = TransformFindChild(clone.transform, "UnLock/Stars/"..i.."/Light");
                                    GameObjectSetActive(light,true);
                                end
                            end
                        end
                    else
                        GameObjectSetActive(lock,true);
                        GameObjectSetActive(unlock,false);
                        UIHelper.SetBoxCollider(clone.transform,false);
                    end
                else  -- Event
                    local finish = TransformFindChild(clone.transform, "Finish");
                    local unlock = TransformFindChild(clone.transform, "UnLock");
                    UIHelper.SetBoxCollider(clone.transform,false);
                    if i == tempo then
                        if TeamLegendData.GetBPlayLevelFx() then
                            tfFxTb.TfEvent = clone.transform;
                            eventId = levelId;
                        else
                            UIHelper.SetBoxCollider(clone.transform,true);
                            if m_eventFx == nil then
                                m_eventFx = GameObjectInstantiate(windowEventFx).transform;
                            end
                            GameObjectSetActive(m_eventFx,true);
                            m_eventFx.parent = clone.transform;
                            m_eventFx.localPosition = NewVector3(0,0,0);
                            m_eventFx.localScale = NewVector3(0.35, 0.35, 1);
                        end
                    end
                    if bFinish and i ~= tempo then
                        GameObjectSetActive(clone.transform,false);
                    else
                        GameObjectSetActive(clone.transform,true)
                        GameObjectSetActive(finish,false);
                        GameObjectSetActive(unlock,true);
                    end
                end
            end
        end
    end

    CheckLevelFx()
    UpdateSMapNav();
end

function CheckLevelFx()
    if TeamLegendData.GetRaidDiff() == TeamLegendData.e_raidDiff.Normal then
        if teamId < eventId then -- Team
            CheckTeamEventFx();
        else
            CheckEventTeamFx();
        end
            teamId,eventId = 0,0;
    else
        CheckTeamTeamFx();
    end

end
-- cloneTeam cloneEvent cloneBg
function CheckTeamEventFx() -- Team to Event
    if TeamLegendData.GetBPlayLevelFx() and tfFxTb.TfTeamFinish ~= nil then     
        GameObjectSetActive(tfFxTb.TfTeamFinish,false);
        local bFinish,starNum = TeamLegendData.IsFinishStars(teamId);
        cloneTeam = GameObjectInstantiate(windowComponent:GetPrefab("TeamOnceFx"));       
        cloneTeam.name = tfFxTb.TfTeamFinish.name;
        cloneTeam.transform.parent = m_spriteRoot;
        cloneTeam.transform.localScale = NewVector3(1,1,1);
        cloneTeam.transform.localPosition = GetGameObjectLocalPostion(tfFxTb.TfTeamFinish);
        TeamLegendData.SetClubIcon(TransformFindChild(cloneTeam.transform,"UnLock/Icon/Icon"),cloneTeam.name);
        Util.AddClick(cloneTeam,BtnIcon);
        local PlayEventBg = function()
            if cloneEvent ~= nil then
                local cloneEventBg = GameObjectInstantiate(windowEventFx);       
                cloneEventBg.transform.parent = cloneEvent.transform;
                cloneEventBg.transform.localScale = NewVector3(0.35,0.35,1);
                cloneEventBg.transform.localPosition = NewVector3(0,0,0);
            end
        end;
        local PlayNextFx = function()
            GameObjectSetActive(tfFxTb.TfEvent,false)
            cloneEvent = GameObjectInstantiate(windowComponent:GetPrefab("EventOnceFx"));       
            cloneEvent.name = tfFxTb.TfEvent.name;
            cloneEvent.transform.parent = m_spriteRoot;
            cloneEvent.transform.localScale = NewVector3(1,1,1);
            cloneEvent.transform.localPosition = GetGameObjectLocalPostion(tfFxTb.TfEvent);
            Util.AddClick(cloneEvent,BtnIcon);
            LuaTimer.AddTimer(true,1,PlayEventBg);
        end;
        local PlayStars = function()
            if cloneBg ~= nil then
                GameObjectDestroy(cloneBg);
                local clone2 = GameObjectInstantiate(windowComponent:GetPrefab("Stars3"));       
                clone2.transform.parent = cloneTeam.transform;
                clone2.transform.localScale = NewVector3(0.4,0.4,1);
                clone2.transform.localPosition = NewVector3(0,0,0);
                Util.SetAutoActive(clone2.transform,starNum)
                LuaTimer.AddTimer(true,starNum*0.3,PlayNextFx);
            end
        end;
        local PlayTeamBg = function()
            if cloneTeam ~= nil then
                cloneBg = GameObjectInstantiate(windowTeamFx);       
                cloneBg.transform.parent = cloneTeam.transform;
                cloneBg.transform.localScale = NewVector3(0.45,0.45,1);
                cloneBg.transform.localPosition = NewVector3(0,0,0);
                LuaTimer.AddTimer(true,1,PlayStars)
            end
        end;

        LuaTimer.AddTimer(true,1,PlayTeamBg);
    end

end
function CheckEventTeamFx() -- Event to Team
    if TeamLegendData.GetBPlayLevelFx() and tfFxTb.TfTeam ~= nil then  
        GameObjectSetActive(tfFxTb.TfTeam,false);
        cloneTeam = GameObjectInstantiate(windowComponent:GetPrefab("TeamOnceFx"));       
        cloneTeam.name = tfFxTb.TfTeam.name;
        cloneTeam.transform.parent = m_spriteRoot;
        cloneTeam.transform.localScale = NewVector3(1,1,1);
        cloneTeam.transform.localPosition = GetGameObjectLocalPostion(tfFxTb.TfTeam);
        TeamLegendData.SetClubIcon(TransformFindChild(cloneTeam.transform,"UnLock/Icon/Icon"),cloneTeam.name);
        Util.AddClick(cloneTeam,BtnIcon);
        local PlayTeamBg = function()
            if cloneTeam ~= nil then
                cloneBg = GameObjectInstantiate(windowTeamFx);       
                cloneBg.transform.parent = cloneTeam.transform;
                cloneBg.transform.localScale = NewVector3(0.45,0.45,1);
                cloneBg.transform.localPosition = NewVector3(0,0,0);
            end
        end;
        LuaTimer.AddTimer(true,1,PlayTeamBg);
    end

end
function CheckTeamTeamFx()
    if TeamLegendData.GetBPlayLevelFx() and tfFxTb.TfTeamFinish ~= nil and tfFxTb.TfTeam ~= nil then     
        GameObjectSetActive(tfFxTb.TfTeamFinish,false);
        local bFinish,starNum = TeamLegendData.IsFinishStars(teamId);

        cloneTeam = GameObjectInstantiate(windowComponent:GetPrefab("TeamOnceFx"));       
        cloneTeam.name = tfFxTb.TfTeamFinish.name;
        cloneTeam.transform.parent = m_spriteRoot;
        cloneTeam.transform.localScale = NewVector3(1,1,1);
        cloneTeam.transform.localPosition = GetGameObjectLocalPostion(tfFxTb.TfTeamFinish);
        TeamLegendData.SetClubIcon(TransformFindChild(cloneTeam.transform,"UnLock/Icon/Icon"),cloneTeam.name);
        Util.AddClick(cloneTeam,BtnIcon);
        local PlayEventBg = function()
            if cloneEvent ~= nil then
                local cloneEventBg = GameObjectInstantiate(windowTeamFx);       
                cloneEventBg.transform.parent = cloneEvent.transform;
                cloneEventBg.transform.localScale = NewVector3(0.45,0.45,1);
                cloneEventBg.transform.localPosition = NewVector3(0,0,0);
            end
        end;
        local PlayNextFx = function()
            GameObjectSetActive(tfFxTb.TfTeam,false)
            cloneEvent = GameObjectInstantiate(windowComponent:GetPrefab("TeamOnceFx"));       
            cloneEvent.name = tfFxTb.TfTeam.name;
            cloneEvent.transform.parent = m_spriteRoot;
            cloneEvent.transform.localScale = NewVector3(1,1,1);
            cloneEvent.transform.localPosition = GetGameObjectLocalPostion(tfFxTb.TfTeam);
            Util.AddClick(cloneEvent,BtnIcon);
            LuaTimer.AddTimer(true,1,PlayEventBg);
        end;
        local PlayStars = function()
            if cloneBg ~= nil then
                GameObjectDestroy(cloneBg);
                local clone2 = GameObjectInstantiate(windowComponent:GetPrefab("Stars3"));       
                clone2.transform.parent = cloneTeam.transform;
                clone2.transform.localScale = NewVector3(0.4,0.4,1);
                clone2.transform.localPosition = NewVector3(0,0,0);
                Util.SetAutoActive(clone2.transform,starNum)
                LuaTimer.AddTimer(true,starNum*0.3,PlayNextFx);
            end
        end;
        local PlayTeamBg = function()
            if cloneTeam ~= nil then
                cloneBg = GameObjectInstantiate(windowTeamFx);       
                cloneBg.transform.parent = cloneTeam.transform;
                cloneBg.transform.localScale = NewVector3(0.45,0.45,1);
                cloneBg.transform.localPosition = NewVector3(0,0,0);
                LuaTimer.AddTimer(true,1,PlayStars)
            end
        end;

        LuaTimer.AddTimer(true,1,PlayTeamBg);
    end

end
function RefreshReward()
    local totalStars = TeamLegendData.GetRaidLevelLength(TeamLegendData.GetClickCountry())*3;
    UIHelper.SetProgressBar(pgr_tempo,TeamLegendData.GetLevelTempoStars()/totalStars);
    UIHelper.SetLabelTxt(lbl_tempoStars,TeamLegendData.GetLevelTempoStars().."/"..totalStars);
    UIHelper.SetLabelTxt(m_rewardStar[1],math.ceil(totalStars/3));
    UIHelper.SetLabelTxt(m_rewardStar[2],math.ceil(totalStars/2));
    UIHelper.SetLabelTxt(m_rewardStar[3],totalStars);
    for i=1,3 do
        GameObjectSetActive(m_rewardGetIcon[i],true);
        GameObjectSetActive(m_rewardGetedIcon[i],false);
        GameObjectSetActive(m_rewardFx[i],false);
    end

    if TeamLegendData.GetLevelTempoStars() >= totalStars/3 then
       if not TeamLegendData.IsGetReward(1) then
            GameObjectSetActive(m_rewardFx[1],true);
        else
            GameObjectSetActive(m_rewardGetedIcon[1],true);
            GameObjectSetActive(m_rewardGetIcon[1],false);
       end
    end
    if TeamLegendData.GetLevelTempoStars() >= totalStars/2 then
       if not TeamLegendData.IsGetReward(2) then
            GameObjectSetActive(m_rewardFx[2],true);
        else
            GameObjectSetActive(m_rewardGetedIcon[2],true);
            GameObjectSetActive(m_rewardGetIcon[2],false);
       end
    end
    if TeamLegendData.GetLevelTempoStars() >= totalStars then
       if not TeamLegendData.IsGetReward(3) then
            GameObjectSetActive(m_rewardFx[3],true);
       else
            GameObjectSetActive(m_rewardGetedIcon[3],true);
            GameObjectSetActive(m_rewardGetIcon[3],false);
       end
    end

end

function IsGetReward(index_)
    local totalStars = TeamLegendData.GetRaidLevelLength(TeamLegendData.GetClickCountry())*3;

    if TeamLegendData.GetLevelTempoStars() >= totalStars/(4-index_) and not TeamLegendData.IsGetReward(index_) then
        return true;
    end

    return false;
end

function BtnIcon(args_)
    TeamLegendData.SetChallengeLevelId(args_.name);
    ShowChallengeTL(args_.name);
end
function BtnBoss(args_)
    TeamLegendData.SetChallengeLevelId(m_bossLvevlId);

    ShowChallengeTL(m_bossLvevlId);
end

function ShowChallengeTL(level_)
    local tb = {};
    tb.levelId = tonumber(level_);
    tb.times = 1;
    tb.stars3 = TeamLegendData.GetStarNum(level_);
    tb.ChallengeType = UIChallengeTL.enum_ChallengeType.TeamLegend;

    local isEvent = TeamLegendData.GetIsEvent(tb.levelId);
    if isEvent == 0 then
        TeamLegendData.SetBEvent(false);
        TeamLegendData.SetBNoFight(false);
        WindowMgr.ShowWindow(LuaConst.Const.UIChallengeTL,tb);
    elseif isEvent == 1 then -- reward
        TeamLegendData.SetBEvent(true);
        TeamLegendData.SetBNoFight(true);
        WindowMgr.ShowWindow(LuaConst.Const.UIChallengeEvent,tb);
    elseif isEvent == 2 then -- challenge
        TeamLegendData.SetBEvent(true);
        TeamLegendData.SetBNoFight(false);
        WindowMgr.ShowWindow(LuaConst.Const.UIChallengeEvent,tb);
    end
end

function BtnStarReward(args_)
    local index = tonumber(args_.name);
    if IsGetReward(index) then
        ReqAward(index);
    else
        local reardTb = {};
        reardTb['item'] = TeamLegendData.GetPreviewRewardTb()[tonumber(TeamLegendData.GetClickCountry())][index];
        WindowMgr.ShowWindow(LuaConst.Const.UIGetItem,{
            m_itemTb = reardTb,
            titleName = Util.LocalizeString("UITeamLegend_Preview"),
            getButtonName = Util.LocalizeString("UIPeakRoad_Confirm"),
            OnClose = nil
        });
    end

end

function ReqAward(args_)
    if TeamLegendData.GetRaidDiff() == TeamLegendData.e_raidDiff.Normal then
        TeamLegendData.ReqRaidStarAward(TeamLegendData.GetClickCountry(),args_,OnStarAward);
    elseif TeamLegendData.GetRaidDiff() == TeamLegendData.e_raidDiff.Elite then
        TeamLegendData.ReqRaidStarAwardJY(TeamLegendData.GetClickCountry(),args_,OnStarAward);
    end

end

function OnStarAward(data_)
    local OnRefreshReward = function()
        RefreshReward();
    end
    local OnCloseAward = function ()
        if TeamLegendData.GetRaidDiff() == TeamLegendData.e_raidDiff.Normal then
            TeamLegendData.ReqRaidInfo(TeamLegendData.GetClickCountry(),OnRefreshReward);
        elseif TeamLegendData.GetRaidDiff() == TeamLegendData.e_raidDiff.Elite then
            TeamLegendData.ReqRaidInfoJY(TeamLegendData.GetClickCountry(),OnRefreshReward);
        end
    end

    if data_ ~= nil then
        WindowMgr.ShowWindow(LuaConst.Const.UIGetItem,{
            m_itemTb = data_,
            titleName = Util.LocalizeString("UITeamLegend_FinishLevel"),
            OnClose = OnCloseAward
        });
    end

end

function BtnLeftArrow(args)
    local clickCountry = TeamLegendData.GetClickCountry();
    if (clickCountry - 1) > 0 then
        TeamLegendData.SetClickCountry(clickCountry-1);
        UITeamLegend.BtnGo();
    else
        local str = Util.LocalizeString("UITeamLegend_FirstLevel");
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, {str});
    end

end

function BtnRightArrow(args)
    local clickCountry = TeamLegendData.GetClickCountry();
    local countryTb = TeamLegendData.GetCountry2Zone(TeamLegendData.GetCurrZone());
    if (clickCountry+1) > #countryTb then
        local str = Util.LocalizeString("UITeamLegend_LastLevel");
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, {str});
    else
        TeamLegendData.SetClickCountry(clickCountry+1);
        UITeamLegend.BtnGo();
    end

end

function OnDragStart(gameObject_)

end

function OnDrag(gameObject_,vec2Delta_)
    UpdateSMapNav();
end

function UpdateSMapNav()
    local indexMap = math.floor((GetGameObjectLocalPostion(m_scrollView).y + moveOffset)*counPerUnit);
    if indexMap < 1 then
        indexMap = 1
    elseif indexMap > #m_mapItemTb then
        indexMap = #m_mapItemTb
    end
    GameObjectLocalPostion(spr_mapNav,NewVector3((m_widthCoeff*GetGameObjectLocalPostion(m_mapItemTb[indexMap].transform).x/2 ),
                           -GetGameObjectLocalPostion(m_scrollView).y*m_heightCoeff, GetGameObjectLocalPostion(spr_mapNav).z));
end

function OnDrop(dragObj_,dropObj_)


end

function OnDragEnd(gameObject_)
    
    
end


-- logic
function ResetFxStatus()
    cloneTeam = nil;
    cloneEvent = nil;
    cloneBg = nil;
end

function OnShow()
    InitLevelMapData();
end

function OnHide()
    if cloneTeam ~= nil then
        GameObjectDestroy(cloneTeam);
        cloneTeam = nil;
    end
    if cloneEvent ~= nil then
        GameObjectDestroy(cloneEvent);
        cloneEvent = nil;
    end

    TeamLegendData.SetPlayLevelFx(false);
    ResetFxStatus();
end

function OnDestroy()
    window = nil;
    windowComponent = nil;
    m_teamFx = nil;
    m_eventFx = nil;

    m_mapItemTb = {};
    ResetFxStatus();
end


