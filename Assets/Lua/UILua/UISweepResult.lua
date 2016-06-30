--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion

module("UISweepResult", package.seeall)

require "Common/UnityCommonScript"
require "UILua/UIItemIcon"

local window = nil;
local windowComponent = nil;
local itemType1 = nil;
local itemType2 = nil;

local m_scrollView = nil;
local lbl_btnSure = nil;

local m_itemList = {};
local m_sweepItemTb = {};
local m_titleName = nil;
local m_sweepTimes = nil;
local m_sweepType = nil;
local m_bPause = true;

local m_callBack = nil;
local m_type1H = 88;
local m_type2H = 180;
enumSweepType = {None = 0,TeamLegend = 1,PeakRoad = 2};
function OnStart(gameObject, params)
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");

    BindUI();
end

function BindUI()
    local transform = window.transform;

    itemType1 = windowComponent:GetPrefab("ItemType1").transform;
    itemType2 = windowComponent:GetPrefab("ItemType2").transform;

    m_scrollView = TransformFindChild(transform, "CenterPart/ScrollViewPanel");
    lbl_btnSure = TransformFindChild(transform, "BtnSure/Label");

    UIHelper.SetLabelTxt(TransformFindChild(transform, "Title/Label"),GetTitleName());
    UIHelper.SetLabelTxt(TransformFindChild(transform, "Title/LabelSweep"),string.format(Util.LocalizeString("UITeamLegend_Sweep"),GetSweepTimes()));
    Util.AddClick(TransformFindChild(transform, "BtnClose").gameObject, BtnLeftReturn);
    Util.AddClick(TransformFindChild(transform, "BtnSure").gameObject, BtnSure);

    CreateItem();
    SetBtnStatus();
end


function CreateItem()
    local size = #m_sweepItemTb['item'];

    function InstantiateItem(name_,type_)
        local clone = GameObjectInstantiate(type_);
        clone.transform.name = name_;
        clone.parent = m_scrollView;
        clone.localPosition = NewVector3(0,-160,0);
        clone.localScale = Vector3.one;
        return clone;
    end
    local bItem = false;
    local obj = nil;
    if size == 0 then
        obj = InstantiateItem("1",itemType1);
    else
        obj = InstantiateItem("2",itemType2);
        bItem = true;
    end

    table.insert(m_itemList,obj.transform);
    RefreshReward(obj.transform,bItem,m_sweepItemTb['item']);
    UIHelper.SetLabelTxt(TransformFindChild(obj.transform, "Title/Label"),string.format(Util.LocalizeString("UISweepResult_MatchTimes"),#m_itemList));

    UpdateItemPos();
end  

function UpdateItemPos()
    local maxIndex = #m_itemList;
    for i=1,maxIndex do
        if tonumber(m_itemList[i].name) == 1 then
            UIHelper.SpringPanelBegin(m_itemList[i], NewVector3(0, (maxIndex-i)*m_type1H, 0), 15);
        else
            UIHelper.SpringPanelBegin(m_itemList[i], NewVector3(0, (maxIndex-i)*m_type2H, 0), 15);
        end
    end
end 


function RefreshReward(tf_,bItem_,itemTb_)
    UIHelper.SetLabelTxt(TransformFindChild(tf_, "Exp/ExpPlayer/ExpPlayer"),m_sweepItemTb["HExp"]);
    UIHelper.SetLabelTxt(TransformFindChild(tf_, "Exp/ExpCoach/ExpCoach"),m_sweepItemTb["UExp"]);
    UIHelper.SetLabelTxt(TransformFindChild(tf_, "Exp/Money/Label"),m_sweepItemTb["money"]);
    
    local m_rewardItem = Util.GetGameObject("UIItemIcon");
    
    if bItem_ then
        for i=1,#itemTb_ do
            local clone = InstantiatePrefab(m_rewardItem,TransformFindChild(tf_,"Item/ScrollViewPanel"));
            clone.name = i;
            local itemIcon = UIItemIcon.New(clone);
            itemIcon:SetSize("win_wb_22", Vector3.one * 86 / 180);
            itemIcon:Init(itemTb_[i], false);
            --UIHelper.SetLabelTxt(TransformFindChild(clone.transform,"Label"),itemTb_[i].num);
            --Util.SetUITexture(TransformFindChild(clone.transform,"Icon"), LuaConst.Const.ItemIcon, Config.GetProperty(Config.ItemTable(),itemTb_[i].id,'icon'), false);
        end
    end
end

function TryOpen(args_,type_,cb_)
    m_sweepItemTb = args_;
    m_sweepType = type_;
    m_callBack = cb_;

    if window ~= nil then
        CreateItem();
        SetBtnStatus();
    end

    WindowMgr.ShowWindow(LuaConst.Const.UISweepResult);
end


function SetBtnStatus()
    if m_sweepType == enumSweepType.TeamLegend then
        m_bPause = true;
        UIHelper.SetLabelTxt(lbl_btnSure,Util.LocalizeString("UISweepResult_Pause"));
    else
        m_bPause = false;
        UIHelper.SetLabelTxt(lbl_btnSure,Util.LocalizeString("UISweepResult_Confirm"));
    end

end

function BtnSure(obj_)
    if m_bPause then
        m_bPause = false;
        if m_sweepType == enumSweepType.TeamLegend then
            TeamLegendData.SetFastTimes(0);
        end

        UIHelper.SetLabelTxt(lbl_btnSure,Util.LocalizeString("UISweepResult_Confirm"));
    else
        if obj_.name ~= "Sure" then
            local OnPlayer = function()
                obj_.name = "Sure";
            end;
            local OnCoach = function()
                CheckUpgradeMgr.CheckPlayerUpgrade(OnPlayer);
            end;

            CheckUpgradeMgr.CheckCoachUpgrade(OnCoach); 
        else
            BtnLeftReturn();
        end 
    end

end

function GetTitleName()
    if m_titleName == nil then
        return "Name";
    else
        return m_titleName;
    end

end

function GetSweepTimes()
    if m_sweepTimes == nil then
        return 10;
    else
        return m_sweepTimes;
    end
end

function SetTitleName(args_)
    m_titleName = args_;
end

function SetSweepTimes(args_)
    m_sweepTimes = args_;
end

function BtnLeftReturn()
    ExitUISweepResult();
end

function OnDestroy()
    window = nil;
    windowComponent = nil;

    m_itemList = {};
end


function ExitUISweepResult()
   if m_callBack ~= nil then
        m_callBack();
        m_callBack = nil;
    end

    windowComponent:Close();
end



