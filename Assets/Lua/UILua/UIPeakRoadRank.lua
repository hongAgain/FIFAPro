--region *.lua
--Date
--此文件由[BabeLua]插件自动生成



--endregion

module("UIPeakRoadRank", package.seeall)

require "Game/PeakRoadData"

local window = nil;
local windowComponent = nil;
local itemType1 = nil;
local itemType2 = nil;

local m_scrollView = nil;
local m_grid = nil;

local color1 = Color.New(217/255,186/255,104/255,1);
function OnStart(gameObject, params)
    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");

    BindUI();

end

function BindUI()
    local transform = window.transform;

    itemType1 = windowComponent:GetPrefab("Item1");
    itemType2 = windowComponent:GetPrefab("Item2");

    m_scrollView = TransformFindChild(transform, "Center/ScrollViewPanel");
    m_grid = TransformFindChild(transform, "Center/ScrollViewPanel/Grid");


    local btnClose = TransformFindChild(transform, "BtnClose").gameObject;
    Util.AddClick(btnClose, BtnClose);
    local btnSure = TransformFindChild(transform, "BtnSure").gameObject;
    Util.AddClick(btnSure, BtnSure);

    InitRank();
end

function InitRank()
    local rankDataTb = PeakRoadData.GetRaidDFRankTb();
    local size = #rankDataTb;
    print("sizeeeeee: "..size);
    for i=1,size do
        local clone = nil;
        if math.mod(i,2) == 0 then
            clone = GameObjectInstantiate(itemType2);
        else
            clone = GameObjectInstantiate(itemType1);
        end
        clone.name = i;
        clone.transform.parent = m_grid;
        clone.transform.localScale = NewVector3(1,1,1);
        clone.transform.localPosition = NewVector3(0,0,0);
        
        local lbl_index = TransformFindChild(clone.transform, "Index");
        local lbl_name = TransformFindChild(clone.transform, "Name");
        local lbl_floor = TransformFindChild(clone.transform, "Floor");
        if i == 1 then
            UIHelper.SetWidgetColor(lbl_index,color1);
            UIHelper.SetWidgetColor(lbl_name,color1);
            UIHelper.SetWidgetColor(lbl_floor,color1);
        end
        UIHelper.SetLabelTxt(lbl_index,rankDataTb[i].sort+1);
        UIHelper.SetLabelTxt(lbl_name,rankDataTb[i].name);
        local strFloor = string.format(Util.LocalizeString("UIPeakRoad_Floor"),rankDataTb[i].val);
        UIHelper.SetLabelTxt(lbl_floor,strFloor);
    end

end

function BtnSure()
    Exit();
end

function BtnClose()
    Exit();

end

function OnDestroy()
    window = nil;
    windowComponent = nil;

end

function Exit()
   windowComponent:Close();

end








