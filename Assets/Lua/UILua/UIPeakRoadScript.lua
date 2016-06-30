--region NewFile_1.lua
--Author : hongpeng.zhao
--Date   : 2015/4/17
--此文件由[BabeLua]插件自动生成



--endregion


module("UIPeakRoadScript", package.seeall)

require "UILua/UIGetItemScript"
require "UILua/UIPrepareMatch"
require "UILua/UIBattleResultS"
require "UILua/UIPeakRoadTips"
require "UILua/UIShop"


local window = nil;
local windowComponent = nil;
local m_scrollView = nil;
local m_grid = nil;
local m_item = nil;
local m_selectFx = nil;
local m_shineCupFx = nil;
local m_centerObject = nil;

local lbl_maxRecord = nil;
local lbl_lastRecord = nil;
local lbl_titleName = nil;
local btn_leftArrow = nil;
local btn_rightArrow = nil;


local m_currFloor = 1;
local m_floorTb = {};
local m_floorSubTb = {};
local spr_challengeTimesTb = {};



local m_currStatus = 1;
local m_lastCurrFloorIndex = 0;
local m_lastCurrIndex = 0;
function OnStart(gameObject, params)
    InitData();
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");
    BindUI();
    WindowMgr.AdjustLayer(); 

    RefreshPeakRoad();
end

function InitData()
    m_currFloor = math.ceil((PeakRoadData.GetCurrFloorIndex()+1)/15);
    if PeakRoadData.GetCurrFloorIndex()+1 >= PeakRoadData.GetMaxFloor() then
        m_currFloor = math.ceil((PeakRoadData.GetMaxFloor())/5);
    end
end

function BindUI()
    local transform = window.transform;

    m_item = windowComponent:GetPrefab("ContinentItem");
    m_grid = TransformFindChild(transform, "CenterPart/ScrollViewPanel/UIGrid");
    m_scrollView = TransformFindChild(transform, "CenterPart/ScrollViewPanel");

    lbl_maxRecord = TransformFindChild(transform, "BottomPart/MaxRecord/Label");
    lbl_lastRecord = TransformFindChild(transform, "BottomPart/LastRecord/Label");
    lbl_titleName = TransformFindChild(transform, "TopPart/Title/Label");

    UIHelper.AddDragOnStarted(m_scrollView,OnDragFinishStart);
    UIHelper.AddDragOnFinish(m_scrollView,OnDragFinish);

    btn_leftArrow = TransformFindChild(transform, "CenterPart/Arrow/LeftArrow").gameObject;
    Util.AddClick(btn_leftArrow, BtnLeftArrow);
    btn_rightArrow = TransformFindChild(transform, "CenterPart/Arrow/RightArrow").gameObject;
    Util.AddClick(btn_rightArrow, BtnRightArrow);

    Util.AddClick(TransformFindChild(transform, "TopPart/Reset").gameObject, BtnReset);
    Util.AddClick(TransformFindChild(transform, "TopPart/Shop").gameObject, BtnShop);
    Util.AddClick(TransformFindChild(transform, "TopPart/BtnSkip").gameObject, BtnPromotion);
    Util.AddClick(TransformFindChild(transform, "TopPart/BtnRanking").gameObject, BtnRanking);
    Util.AddClick(TransformFindChild(transform, "BottomPart/BtnTips").gameObject, BtnTips);
    Util.AddClick(TransformFindChild(transform, "BottomPart/Detail").gameObject,BtnAdditionDetail);

    spr_challengeTimesTb = {};
    for i=1,3 do
        local spr = TransformFindChild(transform, "TopPart/ChallengeTims/Times"..i.."/Sprite");
        table.insert(spr_challengeTimesTb,spr);
    end

    InitScrollView(true);
    InitBuff();

    SetFloorTitle();
end

function InitBuff()
    UIHelper.SetLabelTxt(TransformFindChild(window.transform, "BottomPart/CurrAddition/Label"),
                         Config.GetProperty(Config.RaidDFBuffTable(),tostring(PeakRoadData.GetMaxYestoday()),'att')[1].."%");
    UIHelper.SetLabelTxt(TransformFindChild(window.transform, "BottomPart/TomorrowAddition/Label"),
                         Config.GetProperty(Config.RaidDFBuffTable(),tostring(PeakRoadData.GetMaxFloorIndex()),'att')[1].."%");

end
function InitScrollView(first_)
    m_floorTb = {};
    m_floorSubTb = {};
    m_selectFx = GameObjectInstantiate(windowComponent:GetPrefab("SelectedFx"));
    m_shineCupFx = GameObjectInstantiate(windowComponent:GetPrefab("ShineCupFx"));

    if not first_ then
        UIHelper.DestroyGrid(m_grid);
    end

    for i=1,PeakRoadData.GetMaxGroup() do
        local clone = InstantiatePrefab(m_item,m_grid).transform;
        clone.name = i;

        SetFinishState(clone,i);
        table.insert(m_floorTb, clone);
        for j=1,5 do
            local btnChallenge = TransformFindChild(clone, j).gameObject;
            Util.AddClick(btnChallenge, BtnChallenge);
            table.insert(m_floorSubTb,btnChallenge.transform);
        end
    end

    if not first_ then
        UIHelper.RepositionGrid(m_grid,m_scrollView);
        UIHelper.RefreshPanel(m_scrollView);
    end

    RefreshBtnChallenge();
    SetCurrPos();
end

function RefreshScrollView()
   for i=1,PeakRoadData.GetMaxGroup() do
        SetFinishState(m_floorTb[i],i);     
    end

    RefreshBtnChallenge();
end

function RefreshPeakRoad()
    UIHelper.SetLabelTxt(lbl_maxRecord, string.format(Util.LocalizeString("UIPeakRoad_MaxRecord"),PeakRoadData.GetMaxFloorIndex()));
    PeakRoadData.SetTodayMaxFloor(math.max(PeakRoadData.GetTodayMaxFloor(),PeakRoadData.GetCurrFloorIndex()));
    UIHelper.SetLabelTxt(lbl_lastRecord, string.format(Util.LocalizeString("UIPeakRoad_LastRecord"),PeakRoadData.GetTodayMaxFloor()));

    for i=1,#spr_challengeTimesTb do
        GameObjectSetActive(spr_challengeTimesTb[i],false);
    end
    for j=1,#spr_challengeTimesTb - PeakRoadData.GetCostTimes() do
         GameObjectSetActive(spr_challengeTimesTb[j],true);
    end

end


function OnDragFinishStart(gameObject_)
    Util.EnableScript(m_grid.gameObject,"UICenterOnChild",false);
end

function OnDragFinish()
    Util.EnableScript(m_grid.gameObject,"UICenterOnChild",true);

    m_centerObject = UIHelper.CenterOnRecenter(m_grid);
    m_currFloor = tonumber(m_centerObject.name);

    SetFloorTitle();
end

function SetCurrPos()
    if PeakRoadData.GetCurrFloorIndex() > 0 then
       GameObjectLocalPostion(m_grid,NewVector3(-860*(m_currFloor-1),0,0));
    end
end

function BtnLeftArrow()
    if m_currFloor-1 < 1 then
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("UIPeakRoad_FirstLevel")  });
        return;
    end

    UIHelper.OnClickScrollView(m_scrollView,m_floorTb[m_currFloor-1]);
    m_currFloor = m_currFloor-1;

    SetFloorTitle();
end

function BtnRightArrow()
    if m_currFloor+1 > #m_floorTb then
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("UIPeakRoad_LastLevel")  });
        return;
    end

    UIHelper.OnClickScrollView(m_scrollView,m_floorTb[m_currFloor+1]);
    m_currFloor = m_currFloor+1;

    SetFloorTitle();
end

function RefreshBtnArrowActive()
    btn_leftArrow.gameObject:SetActive(m_currFloor > 1)
    btn_rightArrow.gameObject:SetActive(m_currFloor < #m_floorTb)
end

function RefreshBtnChallenge()
        m_selectFx.transform.parent = m_floorSubTb[GetCurrFloorSub()];
        m_selectFx.transform.localPosition = NewVector3(0,-33,0);
        m_selectFx.transform.localScale = NewVector3(1,1,1);

        GameObjectSetActive(m_shineCupFx.transform,true);
        Util.SetUITexture(m_shineCupFx.transform, LuaConst.Const.CupDF, Config.GetProperty(Config.RaidDFTable(),tostring(GetCurrFloorSub()*3-2),'cup_icon'), true);
        m_shineCupFx.transform.parent = TransformFindChild(m_floorSubTb[GetCurrFloorSub()],"BtnChallenge");
        m_shineCupFx.transform.localPosition = NewVector3(0,0,0);
        m_shineCupFx.transform.localScale = NewVector3(1,1,1);
        
        local InactiveGame = function()
            if m_shineCupFx ~= nil then
                GameObjectSetActive(m_shineCupFx.transform,false);
            end
        end;
        LuaTimer.AddTimer(true,1,InactiveGame);

end

function GetCurrFloor()
    local advFloor = math.ceil((PeakRoadData.GetCurrFloorIndex()+1)/(3*5));
    return advFloor;
end
function SetWindow1Active(args_)
    if args_ then
        m_currStatus = 1;
    else
        m_currStatus = 2;
    end

    if not args_ then
        WindowMgr.ShowWindow(LuaConst.Const.UIPeakRoadSub);
    end
end

function BtnChallenge(args_)
    local index = tonumber(args_.name);
    local indexTemp = math.floor((PeakRoadData.GetCurrFloorIndex() - (GetCurrFloor()-1)*15)/3)+1;
    if IsFinish(m_currFloor,index) then
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("UIPeakRoad_Complete")  });
        return;
    end

    if index ~= indexTemp or m_currFloor > GetCurrFloor() then
        WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { Util.LocalizeString("UIPeakRoad_NoChallenge")  });
        return;
    end

    if(PeakRoadData.GetCurrFloorIndex() >= 9) then
         WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxHint, { "暂未开放，敬请期待！！！"  });
         return;
    end

    if m_lastCurrFloorIndex ~= (PeakRoadData.GetCurrFloorIndex()+1) then
        m_lastCurrFloorIndex = (PeakRoadData.GetCurrFloorIndex()+1);
        PeakRoadData.InitCurrFloorClub(GetCurrFloor());
    end

    if m_lastCurrIndex ~= index then       
        m_lastCurrIndex = index;
        PeakRoadData.InitCurrClub(m_lastCurrIndex);
    end
   
    SetWindow1Active(false);
end

function BtnReset()
    local OnReset = function()
        InitScrollView();
        RefreshPeakRoad();
    end;

    PeakRoadData.OnBtnReset(OnReset);
end

function BtnPromotion()
    local OnFastResult = function()
        InitScrollView();
        SetFloorTitle();
        RefreshPeakRoad();
    end

    PeakRoadData.OnPromotion(OnFastResult)
end

function BtnShop()
    WindowMgr.ShowWindow(LuaConst.Const.UIShop,{shopType = UIShopSettings.ShopType.EpicShop});
end

function BtnRanking()
    local OnRank = function()
        WindowMgr.ShowWindow(LuaConst.Const.UIPeakRoadRank);
    end

    PeakRoadData.ReqRaidDFRank(1,30,OnRank);
end

function BtnAdditionDetail()
    local tb = {};
    tb.TipsType = UIPeakRoadTips.enum_PeakRoadTips.TipsAttr;
    local dataTb = {};
    dataTb.MaxYestoday = PeakRoadData.GetMaxYestoday();
    tb.DataTb = dataTb;
    WindowMgr.ShowWindow(LuaConst.Const.UIPeakRoadTips,tb);
end

function BtnTips()
    WindowMgr.ShowMsgBox(LuaConst.Const.UIMsgBoxScrollText, { Util.LocalizeString("UIPeakRoad_PeakRules") });
end

-- data
function SetCurrFloor(args_)
    m_currFloor = args_;
end

function SetFinishState(tf_,floor_)
    local transform = window.transform;

    for i=1,5 do
        local finish = TransformFindChild(tf_, i.."/Finish");
        local unFinish = TransformFindChild(tf_, i.."/BtnChallenge");
        local itemName = TransformFindChild(tf_, i.."/Label");
        UIHelper.SetLabelTxt(itemName,Config.GetProperty(Config.RaidDFTable(),tostring((floor_-1)*15+(i-1)*3+1),'name'));
        Util.SetUITexture(TransformFindChild(tf_, i.."/Finish/Cup"),LuaConst.Const.CupDF,
                                             Config.GetProperty(Config.RaidDFTable(),tostring((floor_-1)*15+(i-1)*3+1),'cup_icon'),true);
        Util.SetUITexture(TransformFindChild(tf_, i.."/BtnChallenge/Cup"),LuaConst.Const.CupDF,
                                             Config.GetProperty(Config.RaidDFTable(),tostring((floor_-1)*15+(i-1)*3+1),'cup_icon'),true);

        if IsFinish(floor_,i) then
            GameObjectSetActive(finish , true);
            GameObjectSetActive(unFinish,false);
        else
            GameObjectSetActive(finish , false);
            GameObjectSetActive(unFinish,true);
        end
    end

end

function SetFloorTitle()
    local index = PeakRoadData.GetCurrFloorIndex()+1;
     
    UIHelper.SetLabelTxt(lbl_titleName,PeakRoadData.GetGroupName(tonumber(m_currFloor)));

    RefreshBtnArrowActive()
end

function IsFinish(floor_,index_)
    local finishIndex = (floor_-1)*5 + index_*3;
    if PeakRoadData.GetCurrFloorIndex() >= finishIndex then
        return true;
    end

    return false;
end

function GetCurrFloorSub()
    return math.ceil((PeakRoadData.GetCurrFloorIndex()+1)/3);
end

function OnShow()
    if PeakRoadData.GetBRefreshPR() then
        RefreshScrollView();
        RefreshPeakRoad();
        PeakRoadData.SetBRefreshPR(false);
    end

end

function OnHide()

end

function OnDestroy()
    window = nil;
    windowComponent = nil;
    m_shineCupFx = nil;

    spr_challengeTimesTb = {};
end

function ExitUIPeakRoad()
    windowComponent:Close();
end



