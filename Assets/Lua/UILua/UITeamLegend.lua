--region NewFile_1.lua
--Author : hongpeng.zhao
--Date   : 2015/3/23
--此文件由[BabeLua]插件自动生成



--endregion

module("UITeamLegend", package.seeall)

require "Game/TeamLegendData"
require "UILua/UILevelMap"


local window = nil;
local windowComponent = nil;
local windowZoneFx = nil;
local windowFxLight = nil;
local windowFxGo = nil;
local windowFxMap = nil;
local windowFxPlayer = nil;
local windowFxIntro = nil;

local m_scrollView = nil;
local m_countryGrid = nil;
local m_centerObject = nil;
local m_countryItem = nil;
local m_zoneIndex = 1;
local lastUnLockIndex = nil;
local obj_right = nil;

local m_windowZoneFx = nil;
local m_bFinishFx = nil;
local m_bNewCountry = nil;
local m_fxGo = nil;
local m_fxLight = nil;

local m_circleItem = {};
local m_tfCountryTb = {};
local m_tfCountryTbCopy = {};
local m_objGameAlphaTb = {};
local m_fxIconMapTb = {};
local m_fxIconPlayerTb = {};

local CirclePosIndex = {[1]=2,[2]=1,[3]=0,[4]=-1,[5]=-2};
local m_zoneNameTb = {"UITeamLegend_1","UITeamLegend_2","UITeamLegend_3","UITeamLegend_4",
"UITeamLegend_5","UITeamLegend_2","UITeamLegend_3","UITeamLegend_4",};
function OnStart(gameObject, params)
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");
    BindUI();
    Util.TestinExceptionLog("UITeamLegend");
end


function BindUI()
    local transform = window.transform;

    m_countryItem = windowComponent:GetPrefab("UICountry");
    windowZoneFx = windowComponent:GetPrefab("UIZoneFx");
    windowFxLight = windowComponent:GetPrefab("FxLight");
    windowFxGo = windowComponent:GetPrefab("FxGo");
    windowFxMap = windowComponent:GetPrefab("FxMap");
    windowFxPlayer = windowComponent:GetPrefab("FxPlayer");
    windowFxIntro = windowComponent:GetPrefab("FxIntro");


    m_scrollView = TransformFindChild(transform,"LeftPanel/Left/LeftCountry/ScrollViewPanel");
    m_countryGrid = TransformFindChild(transform,"LeftPanel/Left/LeftCountry/ScrollViewPanel/Grid");
    obj_right = TransformFindChild(transform, "RightPanel/Right");
    
    local m_dragFinish = TransformFindChild(transform,"LeftPanel/Left/LeftCountry/ScrollViewPanel");
    UIHelper.AddDragOnStarted(m_dragFinish,OnDragFinishStart);
    UIHelper.AddDragOnFinish(m_dragFinish,OnDragFinish);

 
    Util.AddClick(TransformFindChild(transform,"LeftPanel/Left/Go").gameObject,BtnGo);
    Util.AddClick(TransformFindChild(transform, "RightPanel/RightTop/TogNormal").gameObject,BtnNormal);
    Util.AddClick(TransformFindChild(transform, "RightPanel/RightTop/TogElite").gameObject,BtnElite);

    --------------------------------
    m_circleItem = {};
    for i=1,8 do
        local tf = TransformFindChild(transform,"LeftPanel/Left/LeftCircle/Circle/"..i);
        table.insert(m_circleItem,tf);
    end


    InitCircle(TransformFindChild(transform,"LeftPanel/Left/LeftCircle/Circle"));

    InitZone();
    SetNormalEliteColor();
end

function InitZone()
    for i=1,#m_circleItem do
        GameObjectSetActive(TransformFindChild(m_circleItem[i],"Lock"),true);
        GameObjectSetActive(TransformFindChild(m_circleItem[i],"UnLock"),false);
        GameObjectSetActive(TransformFindChild(m_circleItem[i],"Finish"),false);
    end

    local size = tonumber(TeamLegendData.GetCurrZone());
--    print("size: "..size);
    for i=1,size do
        GameObjectSetActive(TransformFindChild(m_circleItem[i],"Lock"),false);
        if i == 1 or i == 5 then
            if i == size then
                GameObjectSetActive(TransformFindChild(m_circleItem[i],"UnLock"),true);
                GameObjectSetActive(TransformFindChild(m_circleItem[i],"UnLock/UnSelect"),true);
                GameObjectSetActive(TransformFindChild(m_circleItem[i],"UnLock/Select"),false);
            else
                GameObjectSetActive(TransformFindChild(m_circleItem[i],"Finish"),true);
                GameObjectSetActive(TransformFindChild(m_circleItem[i],"Finish/UnSelect"),true);
                GameObjectSetActive(TransformFindChild(m_circleItem[i],"Finish/Select"),false);
            end
        else
            GameObjectSetActive(TransformFindChild(m_circleItem[i+4],"Lock"),false);
             if i == size then
                GameObjectSetActive(TransformFindChild(m_circleItem[i],"UnLock"),true);
                GameObjectSetActive(TransformFindChild(m_circleItem[i],"UnLock/UnSelect"),true);
                GameObjectSetActive(TransformFindChild(m_circleItem[i],"UnLock/Select"),false);
                GameObjectSetActive(TransformFindChild(m_circleItem[i+4],"UnLock"),true);
                GameObjectSetActive(TransformFindChild(m_circleItem[i+4],"UnLock/UnSelect"),true);
                GameObjectSetActive(TransformFindChild(m_circleItem[i+4],"UnLock/Select"),false);
            else
                GameObjectSetActive(TransformFindChild(m_circleItem[i],"Finish"),true);
                GameObjectSetActive(TransformFindChild(m_circleItem[i],"Finish/UnSelect"),true);
                GameObjectSetActive(TransformFindChild(m_circleItem[i],"Finish/Select"),false);
                GameObjectSetActive(TransformFindChild(m_circleItem[i+4],"Finish"),true);
                GameObjectSetActive(TransformFindChild(m_circleItem[i+4],"Finish/UnSelect"),true);
                GameObjectSetActive(TransformFindChild(m_circleItem[i+4],"Finish/Select"),false);
            end
        end
    end

    InitCountry();
end

function InitCircle(tfCircle_)
    UIHelper.SetCirclePos(tfCircle_,CirclePosIndex[TeamLegendData.GetCurrZone()]);

    UIHelper.OnDragEndCircle(tfCircle_,OnDragEndCircle);
--    UIHelper.OnDragCircle(tfCircle_,OnDragCircle);
    UIHelper.OnDragStartCircle(tfCircle_,OnDragStartCircle);

    m_bNewCountry = true;
    LuaTimer.AddTimer(true,0.5,UpdateCountryAlpha);
--    UpdateZoneFx();
end

function InitCountry()
    m_tfCountryTb = {};
    m_tfCountryTbCopy = {};
    m_fxIconMapTb = {};
    m_fxIconPlayerTb = {};
    UIHelper.DestroyGrid(m_countryGrid);
     
    lastUnLockIndex = 0;
    local countryId = 1;
    local countryTb = TeamLegendData.GetCountry2Zone(m_zoneIndex);
    local size = #countryTb;
    for i=1,size do
        countryId = tonumber(countryTb[i]);
        table.insert(m_fxIconMapTb,Config.GetProperty(Config.RaidClassTable(),tostring(countryId),'fxMap'));
        table.insert(m_fxIconPlayerTb,Config.GetProperty(Config.RaidClassTable(),tostring(countryId),'fxPlayer'));

        local icon = InstantiatePrefab(m_countryItem,m_countryGrid,i);
        table.insert(m_tfCountryTb,icon.transform);
        m_tfCountryTbCopy[i] = icon.transform;
        Util.AddDragDrop(icon,OnDragStart,OnDrag,OnDrop,OnDragEnd,nil);

        local bUnlock,bFinish = TeamLegendData.IsUnlockFinishCountry(countryId);

        if bUnlock then
            if bFinish then 
                SetCountryIconStatus(icon.transform,false,false,true)    
                TeamLegendData.SetCountryIcon(TransformFindChild(icon.transform,"Finish/Icon/Icon"),countryId);           
            else
                SetCountryIconStatus(icon.transform,false,true,false)
                 TeamLegendData.SetCountryIcon(TransformFindChild(icon.transform,"UnLock/Icon/Icon"),countryId);
            end

            lastUnLockIndex = lastUnLockIndex + 1;           
        else
            SetCountryIconStatus(icon.transform,true,false,false);
        end

--        if TeamLegendData.GetRaidDiff() == TeamLegendData.e_raidDiff.Elite and TeamLegendData.GetCurrCountryLimit() == 1 then
--                SetCountryIconStatus(icon.transform,true,false,false)
--        else
--            if i < TeamLegendData.GetCurrCountry() and TeamLegendData.GetClickZone() <= TeamLegendData.GetCurrZone() then
--                SetCountryIconStatus(icon.transform,false,false,true)
--                lastUnLockIndex = lastUnLockIndex + 1;
--                TeamLegendData.SetCountryIcon(TransformFindChild(icon.transform,"Finish/Icon/Icon"),countryId);
--            elseif i == TeamLegendData.GetCurrCountry() and TeamLegendData.GetClickZone() <= TeamLegendData.GetCurrZone() then
--                SetCountryIconStatus(icon.transform,false,true,false)
--                lastUnLockIndex = lastUnLockIndex + 1;
--                TeamLegendData.SetCountryIcon(TransformFindChild(icon.transform,"UnLock/Icon/Icon"),countryId);
--            else
--                SetCountryIconStatus(icon.transform,true,false,false)
--            end
--        end



    end

    UIHelper.RepositionGrid(m_countryGrid,m_scrollView);
    UpdateCountryScale();
    if lastUnLockIndex > 0 then
        for i=(lastUnLockIndex+1),size do
            CalcParentChild(m_tfCountryTb[lastUnLockIndex],m_tfCountryTb[i],lastUnLockIndex,i);
        end
    end
end
function SetCountryIconStatus(tf_,b1_,b2_,b3_)
    GameObjectSetActive(TransformFindChild(tf_,"Lock"),b1_);
    GameObjectSetActive(TransformFindChild(tf_,"UnLock"),b2_);
    GameObjectSetActive(TransformFindChild(tf_,"Finish"),b3_);
end

function CalcParentChild(tfParent_,tfChild_,iParent_,iChild_)
    local cLocalPosY = (iChild_-1)*(-74);
    local pLocalPosY = GetGameObjectLocalPostion(tfParent_).y;
    local cLocalScaleY = GetGameObjectLocalScale(tfChild_).y;
    local pLocalScaleY = GetGameObjectLocalScale(tfParent_).y;
    local icon = GameObjectInstantiate(m_countryItem);
    icon.name = tfChild_.name;
    local pos = (cLocalPosY-pLocalPosY)/pLocalScaleY;
    local scale = cLocalScaleY/pLocalScaleY;
    icon.transform.parent = tfParent_;
    icon.transform.localScale = NewVector3(scale,scale,1);
    icon.transform.localPosition = NewVector3(0,pos,0);
    Util.AddDragDrop(icon,OnDragStart,OnDrag,OnDrop,OnDragEnd,nil);

    GameObjectSetActive(tfChild_,false);
    m_tfCountryTbCopy[iChild_] = icon.transform;
end

function UpdateCountryScaleCopy()
    if lastUnLockIndex == 0 then
        return;
    end

    for i=(lastUnLockIndex+1),#m_tfCountryTb do
        local scale = GetGameObjectLocalScale(m_tfCountryTb[i]).y/GetGameObjectLocalScale(m_tfCountryTb[lastUnLockIndex]).y;
        local pos = ((i-1)*(-74)+(lastUnLockIndex-1)*74)/GetGameObjectLocalScale(m_tfCountryTb[lastUnLockIndex]).y;

        GameObjectLocalScale(m_tfCountryTbCopy[i],NewVector3(scale,scale,1));
        GameObjectLocalPostion(m_tfCountryTbCopy[i],NewVector3(0,pos,0));
    end

end

--function OnDragCircle(objectGame_,delta_)

--end

function OnDragStartCircle(objectGame_)

    SetZoneFx(false,nil,nil);
end

function OnDragEndCircle(args_)
    m_zoneIndex = tonumber(args_);
    if m_zoneIndex > 5 then
        m_zoneIndex = m_zoneIndex - 4;
    end
    TeamLegendData.SetClickZone(m_zoneIndex);

    InitCountry();
    UpdateZoneFx();
end

function UpdateZoneFx()
    local size = tonumber(TeamLegendData.GetCurrZone());
    local i = m_zoneIndex;
    if m_zoneIndex <= size then
        if i == 1 or i == 5 then
            if i == size then
                GameObjectSetActive(TransformFindChild(m_circleItem[i],"UnLock"),true);
                GameObjectSetActive(TransformFindChild(m_circleItem[i],"UnLock/UnSelect"),true);
                SetZoneFx(true,m_circleItem[i],false,i);
            else
                GameObjectSetActive(TransformFindChild(m_circleItem[i],"Finish"),true);
                GameObjectSetActive(TransformFindChild(m_circleItem[i],"Finish/UnSelect"),true);
                SetZoneFx(true,m_circleItem[i],true,i);
            end
        else
             if i == size then
                GameObjectSetActive(TransformFindChild(m_circleItem[i],"UnLock"),true);
                GameObjectSetActive(TransformFindChild(m_circleItem[i],"UnLock/UnSelect"),true);
                SetZoneFx(true,m_circleItem[i],false,i);
                GameObjectSetActive(TransformFindChild(m_circleItem[i+4],"UnLock"),true);
                GameObjectSetActive(TransformFindChild(m_circleItem[i+4],"UnLock/UnSelect"),true);
                SetZoneFx(true,m_circleItem[i],false,i+4);
            else
                GameObjectSetActive(TransformFindChild(m_circleItem[i],"Finish"),true);
                GameObjectSetActive(TransformFindChild(m_circleItem[i],"Finish/UnSelect"),true);
                SetZoneFx(true,m_circleItem[i],true,i);
                GameObjectSetActive(TransformFindChild(m_circleItem[i+4],"Finish"),true);
                GameObjectSetActive(TransformFindChild(m_circleItem[i+4],"Finish/UnSelect"),true);
                SetZoneFx(true,m_circleItem[i],true,i+4);
            end
        end 
    end

end

function SetZoneFx(bShow_,parent_,bFinish_,index_)
    if m_windowZoneFx == nil then
        m_windowZoneFx = GameObjectInstantiate(windowZoneFx).transform;
    end

    if bShow_ then
        m_bFinishFx = bFinish_;
        if m_windowZoneFx == nil then
            m_windowZoneFx = GameObjectInstantiate(windowZoneFx).transform;
        end
        m_windowZoneFx.parent = parent_;
        m_windowZoneFx.localPosition = NewVector3(0,0,0);
        m_windowZoneFx.localScale = NewVector3(1, 1, 1);
        m_windowZoneFx.localEulerAngles = NewVector3(0,0,0);

        if m_bFinishFx then
            GameObjectSetActive(TransformFindChild(m_windowZoneFx,"Finish/Select"),true);
            UIHelper.SetLabelTxt(TransformFindChild(m_windowZoneFx,"Finish/Select/Label"),Util.LocalizeString(m_zoneNameTb[index_]));
        else
            GameObjectSetActive(TransformFindChild(m_windowZoneFx,"UnLock/Select"),true);
            UIHelper.SetLabelTxt(TransformFindChild(m_windowZoneFx,"UnLock/Select/Label"),Util.LocalizeString(m_zoneNameTb[index_]));
        end
    else
        if m_windowZoneFx ~= nil then
            GameObjectDestroy(m_windowZoneFx.gameObject);
            m_windowZoneFx = nil;
        end
    end

end

function OnRaidInfo(data_)
     WindowMgr.ShowWindow(LuaConst.Const.UILevelMap);
     UILevelMap.InitLevelMapData();
end

function OnRaidInfoJY(data_)
     WindowMgr.ShowWindow(LuaConst.Const.UILevelMap);
     UILevelMap.InitLevelMapData();
end


function UpdateCountryScale()
    for i=1,#m_tfCountryTb do
        local offset = math.abs(GameObjectPostion(m_tfCountryTb[i]).y +0.125);

        GameObjectLocalScale(m_tfCountryTb[i],NewVector3(1-offset,1-offset,1));
    end

end

function UpdateCountryAlpha()
    if m_bNewCountry then
        for i=1,#m_objGameAlphaTb do
            GameObjectDestroy(m_objGameAlphaTb[i]);
        end
         m_objGameAlphaTb = {};
         local map1 = InstantiatePrefab(windowFxMap,obj_right);
         Util.SetUITexture(TransformFindChild(map1.transform,"IconMap"),LuaConst.Const.PveMapFx,m_fxIconMapTb[TeamLegendData.GetClickCountry()],true);
         table.insert(m_objGameAlphaTb,map1);
         function Alpha3()
             if obj_right == nil then
                return;
             end
             m_fxGo = InstantiatePrefab(windowFxGo,TransformFindChild(window.transform,"LeftPanel/Left/Go"));          
             function DestroyFx()
                if m_fxGo ~= nil then
                    GameObjectDestroy(m_fxGo);
                    m_fxGo = nil;
                end
             end
             LuaTimer.AddTimer(true,2.0,DestroyFx);
         end
         function Alpha2()
            if obj_right == nil then
                return;
            end
            local intro1 = InstantiatePrefab(windowFxIntro,obj_right);
            table.insert(m_objGameAlphaTb,intro1);
            SetTeamIntro(intro1.transform);
            LuaTimer.AddTimer(true,0.3,Alpha3);
         end
         function Alpha1()
            if obj_right == nil then
                return;
            end
            local team1 = InstantiatePrefab(windowFxPlayer,obj_right);
            Util.SetUITexture(TransformFindChild(team1.transform,"IconPlayer"),LuaConst.Const.PvePlayerFx,m_fxIconPlayerTb[TeamLegendData.GetClickCountry()],true);
            table.insert(m_objGameAlphaTb,team1);
            m_fxLight = InstantiatePrefab(windowFxLight,obj_right);          
            function DestroyFx()
                if m_fxLight ~= nil then
                    GameObjectDestroy(m_fxLight);
                    m_fxLight = nil;
                end
            end
            LuaTimer.AddTimer(true,1.0,DestroyFx);
            LuaTimer.AddTimer(true,0.3,Alpha2);
         end   
         LuaTimer.AddTimer(true,0.3,Alpha1);
    end

end

function UpdataCountryUI(bNewCountry_)
    m_centerObject = UIHelper.CenterOnRecenter(m_countryGrid);

    local countryId = tonumber(TeamLegendData.GetCountry2Zone(TeamLegendData.GetCurrZone())[tonumber(m_centerObject.name)]);
    
    m_bNewCountry = false;
    if countryId ~= TeamLegendData.GetClickCountry() and countryId <= TeamLegendData.GetCurrCountry() 
       and TeamLegendData.GetClickCountry() <= TeamLegendData.GetCurrCountry() then
        m_bNewCountry = true;
    end
    if bNewCountry_ then
        m_bNewCountry = true;
    end

    TeamLegendData.SetClickCountry(countryId);
    if m_bNewCountry then
        UpdateCountryAlpha();
    end
end
-- Drag Country
function OnDrag(gameObject_,vec2Delta_)
    UpdateCountryScale();
    UpdateCountryScaleCopy();
end

function OnDragStart(gameObject_)
end

function OnDragEnd(gameObject_)
    function UpdatePos()
        UpdateCountryScale();
        UpdateCountryScaleCopy();
    end

    for i=1,10 do
        LuaTimer.AddTimer(true, i*0.1, UpdatePos);
    end
end

function OnDrop(dragObj_,dropObj_)
end

-- Drag Country
function OnDragFinishStart(gameObject_)
    Util.EnableScript(m_countryGrid.gameObject,"UICenterOnChild",false);
end
-- DragEnd Country
function OnDragFinish(gameObject_)
    Util.EnableScript(m_countryGrid.gameObject,"UICenterOnChild",true);
    UpdataCountryUI();
end

function SetTeamIntro(tf_)
    UIHelper.SetLabelTxt(TransformFindChild(tf_,"Intro/LblCountryName"),
                         Config.GetProperty(Config.RaidClassTable(),tostring(TeamLegendData.GetClickCountry()),'country'));
    UIHelper.SetLabelTxt(TransformFindChild(tf_,"Intro/LblTeamIntro"),
                         Config.GetProperty(Config.RaidClassTable(),tostring(TeamLegendData.GetClickCountry()),'name'));
end

-- Tog Normal Elite
function BtnNormal()
    if TeamLegendData.GetRaidDiff() == TeamLegendData.e_raidDiff.Normal then
        return;
    end
    TeamLegendData.SetRaidDiff(TeamLegendData.e_raidDiff.Normal);
    local function OnReqInfo(data_)
        InitCountry();
        UpdataCountryUI(true);
    end

    TeamLegendData.ReqRaidEnter(OnReqInfo);

    SetNormalEliteColor();
end

function BtnElite()
    if TeamLegendData.GetRaidDiff() == TeamLegendData.e_raidDiff.Elite then
        return;
    end
    TeamLegendData.SetRaidDiff(TeamLegendData.e_raidDiff.Elite);
    local function OnReqInfo(data_)
        InitCountry();
        UpdataCountryUI(true);
    end

    TeamLegendData.ReqRaidEnterJY(OnReqInfo);

    SetNormalEliteColor();
end

function SetNormalEliteColor()
    local lbl_togNormal = TransformFindChild(window.transform, "RightPanel/RightTop/TogNormal/Label");
    local spr_togNormalBg = TransformFindChild(window.transform, "RightPanel/RightTop/TogNormal/Background");
    local spr_togNormalMark = TransformFindChild(window.transform, "RightPanel/RightTop/TogNormal/Checkmark");
    local lbl_togElite = TransformFindChild(window.transform, "RightPanel/RightTop/TogElite/Label");
    local spr_togEliteBg = TransformFindChild(window.transform, "RightPanel/RightTop/TogElite/Background");
    local spr_togEliteMark = TransformFindChild(window.transform, "RightPanel/RightTop/TogElite/Checkmark");
    local blueColor = Color.New(102/255,204/255,255/255,1);
    local grayColor = Color.New(171/255,173/255,185/255,1);
    if TeamLegendData.GetRaidDiff() == TeamLegendData.e_raidDiff.Normal then
        UIHelper.SetWidgetColor(lbl_togNormal,blueColor);
        UIHelper.SetWidgetColor(spr_togNormalBg,blueColor);
        UIHelper.SetWidgetColor(spr_togNormalMark,blueColor);
        UIHelper.SetWidgetColor(lbl_togElite,grayColor);
        UIHelper.SetWidgetColor(spr_togEliteBg,grayColor);
        UIHelper.SetWidgetColor(spr_togEliteMark,grayColor);
    elseif TeamLegendData.GetRaidDiff() == TeamLegendData.e_raidDiff.Elite then
        UIHelper.SetWidgetColor(lbl_togNormal,grayColor);
        UIHelper.SetWidgetColor(spr_togNormalBg,grayColor);
        UIHelper.SetWidgetColor(spr_togNormalMark,grayColor);
        UIHelper.SetWidgetColor(lbl_togElite,blueColor);
        UIHelper.SetWidgetColor(spr_togEliteBg,blueColor);
        UIHelper.SetWidgetColor(spr_togEliteMark,blueColor);
    end
end

function BtnGo()
    if TeamLegendData.GetClickZone() > TeamLegendData.GetCurrZone() then
       local str = string.format(Util.LocalizeString("UITeamLegend_NoZone"),
                                 Config.GetProperty(Config.RaidClassTable(),tostring(TeamLegendData.GetCurrCountry()),'zoneName'));
       WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, {str});
       return;
    end

    if TeamLegendData.IsFinishCountry() then
        if TeamLegendData.GetRaidDiff() == TeamLegendData.e_raidDiff.Normal then
            TeamLegendData.ReqRaidInfo(TeamLegendData.GetClickCountry(),OnRaidInfo);
        elseif TeamLegendData.GetRaidDiff() == TeamLegendData.e_raidDiff.Elite then
            TeamLegendData.ReqRaidInfoJY(TeamLegendData.GetClickCountry(),OnRaidInfoJY);
        end
    end
end

function OnShow()
    if m_centerObject ~= nil then
        local countryId = tonumber(TeamLegendData.GetCountry2Zone(TeamLegendData.GetCurrZone())[tonumber(m_centerObject.name)]);
        TeamLegendData.SetClickCountry(countryId);

        InitCountry();
    end

end

function OnHide()
    
end

function OnDestroy()
    m_zoneIndex = 1;

    m_centerObject = nil;
    obj_right = nil;
    m_countryItem = nil;
    windowZoneFx = nil;
    m_windowZoneFx = nil
    m_fxGo = nil;
    m_fxLight = nil;
    m_objGameAlphaTb = {};

    TeamLegendData.ResetData();
end

function ExitUITeamLegend()
    windowComponent:Close();
end
