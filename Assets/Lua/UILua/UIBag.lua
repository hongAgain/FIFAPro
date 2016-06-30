module("UIBag", package.seeall)

require "Game/ItemSys"
require "Common/CommonScript"
require "Common/UnityCommonScript"
require "Config"
require "Game/SynSys"
require "UILua/UIItemIcon"

local window = nil;
local windowComponent = nil;
local scrollView = nil;
local label_name = nil;
local label_desc = nil;
local label_floating = nil;
local label_nr = nil;

local useBtn = nil;
local sellBtn = nil;

local select_ItemGrid = nil;
local select_ItemIcon = nil;
local curSelectItem = nil;
local leftBlock = nil;
local label_singlePrice = nil;

local itemScale = Vector3.one * 140 / 180;
local itemGridTB = { cloneChildren = {}, prefab = nil, parent = nil };
function itemGridTB:GetGrid(idx)
            
    local i = math.floor((idx - 1) / 4);
    local j = math.floor((idx - 1) % 4);

    local pos = Vector3.zero;
    pos.x = j * 144 - 216;
    pos.y = -i * 146 + 183;

    local itemIcon = self.cloneChildren[idx];
    if (itemIcon == nil) then
        local clone = AddChild(self.prefab, self.parent);
        clone.transform.localPosition = pos;
        
        itemIcon = UIItemIcon.New(clone);
        itemIcon:SetSize("win_wb_22", itemScale);
        self.cloneChildren[idx] = itemIcon;
    else
        itemIcon:SetActive(true);
    end
    
    return itemIcon;
end

function itemGridTB:Release()
    self.prefab = nil;
    self.parent = nil;
    self.cloneChildren = {};
end

function itemGridTB:HideAfter(idx)
    for i = idx, #self.cloneChildren do
        self.cloneChildren[i]:SetActive(false);
    end
end

function OnStart(gameObject, params)
	window = gameObject;
	windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");
    BindUI();

    SynSys.RegisterCallback("item", OnRefreshItemList);
end

function OnDestroy()
    SynSys.UnRegisterCallback("item", OnRefreshItemList);
    
    window = nil;
    windowComponent = nil;
    scrollView = nil;
    label_name = nil;
    label_desc = nil;
    label_floating = nil;
    label_nr = nil;

    useBtn = nil;
    sellBtn = nil;

    select_ItemGrid = nil;
    select_ItemIcon = nil;
    curSelectItem = nil;
    leftBlock = nil;
    label_singlePrice = nil;

    itemGridTB:Release();
end

function OnShow()
end

function OnHide()
end

function BindUI()

    local transform = window.transform;
    scrollView = TransformFindChild(transform, "UIScrollView");
    itemGridTB.parent = scrollView;

    useBtn = TransformFindChild(transform, "Operation/Use").gameObject;
    sellBtn = TransformFindChild(transform, "Operation/Sell").gameObject;

    Util.AddClick(useBtn, UseItem);
    Util.AddClick(sellBtn, SellItem);

    --UIHelper.EnableBtn(useBtn, false);
    --UIHelper.EnableBtn(sellBtn, false);
    
    label_name = TransformFindChild(transform, "Operation/Label - Name");
    label_desc = TransformFindChild(transform, "Operation/Label - Desc");
    label_floating = TransformFindChild(transform, "Operation/Label - Floating");
    label_nr = TransformFindChild(transform, "Operation/Label - Num");
    
    select_ItemGrid = TransformFindChild(transform, "UIScrollView/Sprite - Selected");
    select_ItemGrid.gameObject:SetActive(false);
    select_ItemIcon = TransformFindChild(transform, "Operation/Icon/SelectItemIcon");
    
    leftBlock = TransformFindChild(transform, "Operation").gameObject;
    leftBlock:SetActive(false);
    
    label_singlePrice = TransformFindChild(transform, "Operation/Label - SinglePrice");

    RefreshItemList();
end

function RefreshItemList()
    
    function SortByItemId(a, b)
        local aID = tonumber(a.id);
        local bID = tonumber(b.id);
        if (aID > bID) then
            return -1;
        elseif (aID == bID) then
            return 0;
        else
            return 1;
        end
    end

    local sortedItem = CommonScript.QuickSort(ItemSys.AllItem(), SortByItemId);

    local itemPrefab = Util.GetGameObject("UIItemIcon");
    itemGridTB.prefab = itemPrefab;

    local idx = 0;
    for k,v in ipairs(sortedItem) do
        if (v.num ~= 0) then
            local strID = tostring(v.id);            
            if (strID ~= LuaConst.Const.GB and
                strID ~= LuaConst.Const.SB and
                strID ~= LuaConst.Const.UExp and
                strID ~= LuaConst.Const.HExp and
                strID ~= LuaConst.Const.Power) then
                
                local clone = itemGridTB:GetGrid(idx + 1);
                local listener = Util.AddClick(clone.gameObject, SelectItem);
                listener.parameter = v;
                
                clone:Init(v, true);
                idx = idx + 1;
            end
        end
    end

    idx = idx + 1;
    while (idx <= 16) do
        local clone = itemGridTB:GetGrid(idx);
        Util.AddClick(clone.gameObject, SelectItem);
        idx = idx + 1;
    end

    itemGridTB:HideAfter(idx + 1);
end

function SelectItem(clickItem)
    if (clickItem ~= nil) then
        local listener = GetComponentInChildren(clickItem, "UIEventListener");
        curSelectItem = listener.parameter;
        if (curSelectItem ~= nil) then
            UIHelper.SetLabelTxt(label_desc, Config.GetProperty(Config.ItemTable(), curSelectItem.id, "desc"));
            UIHelper.SetLabelTxt(label_name, Config.GetProperty(Config.ItemTable(), curSelectItem.id, "name"));
            UIHelper.SetLabelTxt(label_singlePrice, tostring(Config.GetProperty(Config.ItemTable(), curSelectItem.id, "price")));

            useBtn:SetActive(ItemSys.CanUse(curSelectItem.id));
            UIHelper.EnableBtn(sellBtn, ItemSys.CanSell(curSelectItem.id));

            UIHelper.SetLabelTxt(label_nr, string.format(Util.LocalizeString("UIBag_Label2"), curSelectItem.num));
            if (leftBlock.activeInHierarchy == false) then
                leftBlock:SetActive(true);
            end
        else
            leftBlock:SetActive(false);
        end
        
        select_ItemGrid.gameObject:SetActive(true);
        select_ItemGrid.localPosition = clickItem.transform.localPosition;
        Util.SetUITexture(select_ItemIcon, LuaConst.Const.ItemIcon, Config.GetProperty(Config.ItemTable(), curSelectItem.id, 'icon'), true);
    else
        leftBlock:SetActive(false);
    end
end

function UseItem()
    local useType = Config.GetProperty(Config.ItemTable(), curSelectItem.id, "useType");
    if (useType == 2) then
        OpenPlayList();
    else
        ItemSys.UseItem(curSelectItem.id);
    
        local effect = ItemSys.ItemEffect(curSelectItem.id);
        UIHelper.SetLabelTxt(label_floating, effect);
        UIHelper.Floating(label_floating, NewVector3(-300, 153, 0), NewVector3(-300, 183, 0), 1);
    end
end

function SellItem()
    WindowMgr.ShowWindow(LuaConst.Const.UISell, curSelectItem);
end

function Close()
    windowComponent:Close();
end

function OnRefreshItemList()
    RefreshItemList();
    
    if (curSelectItem.num > 0) then
        UIHelper.SetLabelTxt(label_nr, string.format(Util.LocalizeString("UIBag_Label2"), curSelectItem.num));
    else
        curSelectItem = nil;
        SelectItem(curSelectItem);
    end
end

function OpenPlayList()

    WindowMgr.ShowWindow(LuaConst.Const.UIPlayerList);
end