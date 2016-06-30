module("UISell", package.seeall)

local window = nil;
local windowComponent = nil;

local curItem = nil;

local maxNr = 0;
local curNr = 0;

local lable_nr = nil;
local label_total = nil;

local singlePrice = 0;

function OnStart(gameObject, params)
    curItem = params;
    maxNr = curItem.num;
    if (maxNr > 0) then
        curNr = 1;
    end

    window = gameObject;
    windowComponent = GetComponentInChildren(window, "UIBaseWindowLua");
    BindUI();
end

function OnDestroy()
    label_nr = nil;
    label_total = nil;
end

function OnShow()

end

function OnHide()

end

function BindUI()
    local transform = window.transform;

    local close = TransformFindChild(transform, "Close");
    Util.AddClick(close.gameObject, Close);

    local yes = TransformFindChild(transform, "Btn - Yes");
    Util.AddClick(yes.gameObject, Sell);

    local min = TransformFindChild(transform, "Btn - Min");
    local minus = TransformFindChild(transform, "Btn - Minus");
    local plus = TransformFindChild(transform, "Btn - Plus");
    local max = TransformFindChild(transform, "Btn - Max");

    UIHelper.AddPressRepeating(min.gameObject, Min);
    UIHelper.AddPressRepeating(minus.gameObject, Minus);
    UIHelper.AddPressRepeating(plus.gameObject, Plus);
    UIHelper.AddPressRepeating(max.gameObject, Max);

    lable_nr = TransformFindChild(transform, "Label - Nr");
    UIHelper.SetLabelTxt(lable_nr, tostring(curNr));

    local label_ownNr = TransformFindChild(transform, "Label - OwnNr");
    UIHelper.SetLabelTxt(label_ownNr, string.format(Util.LocalizeString("UIBag_Label2"), maxNr));

    singlePrice = Config.GetProperty(Config.ItemTable(), curItem.id, "price") or 0;
    
    label_total = TransformFindChild(transform, "Label - TotalPrice");
    UIHelper.SetLabelTxt(label_total, tostring(singlePrice * curNr));
end

function Close()
    windowComponent:Close();
end

function Sell()
    ItemSys.SellItem(curItem, curNr);
    Close();
end

function Minus()
    curNr = curNr - 1;
    if (curNr < 1) then
        curNr = 1;
    end

    UIHelper.SetLabelTxt(lable_nr, tostring(curNr));
    UIHelper.SetLabelTxt(label_total, tostring(singlePrice * curNr));
end

function Plus()
    curNr = curNr + 1;
    if (curNr > maxNr) then
        curNr = maxNr;
    end
    
    UIHelper.SetLabelTxt(lable_nr, tostring(curNr));
    UIHelper.SetLabelTxt(label_total, tostring(singlePrice * curNr));
end

function Min()
    curNr = curNr - 10;
    if (curNr < 1) then
        curNr = 1;
    end

    UIHelper.SetLabelTxt(lable_nr, tostring(curNr));
    UIHelper.SetLabelTxt(label_total, tostring(singlePrice * curNr));
end

function Max()
    curNr = curNr + 10;
    if (curNr > maxNr) then
        curNr = maxNr;
    end

    UIHelper.SetLabelTxt(lable_nr, tostring(curNr));
    UIHelper.SetLabelTxt(label_total, tostring(singlePrice * curNr));
end