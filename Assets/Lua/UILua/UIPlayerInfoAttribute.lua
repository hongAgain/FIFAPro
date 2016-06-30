module("UIPlayerInfoAttribute", package.seeall)

local scrollRoot = nil;

function InitPlayerInfoAttribute(subTransform_, windowComponent)
    local attr1st = windowComponent:GetPrefab("1stAttr");
    local attr2nd = windowComponent:GetPrefab("2ndAttr");
    
    scrollRoot = TransformFindChild(subTransform_, "Scroll View");
    local playerData = UIPlayerInfoBaseScript.GetCurrHeroData();
    for i = 1, #playerData.attr1 do
        local clone = AddChild(attr1st, scrollRoot);
        clone.name = "1stAttr"..i;
    end
    
    for i = 1, #playerData.attr2 do
        local clone = AddChild(attr2nd, scrollRoot);
        clone.name = "2ndAttr"..i;
    end
    
    RefreshAttribute();
end

function RefreshAttribute()
    local line = TransformFindChild(scrollRoot, "Sprite - Line");
    local playerData = UIPlayerInfoBaseScript.GetCurrHeroData();
    
    local attr1NotGK = { true, true, true, true, true, true, false, false };
    local attr1GK    = { true, true, false, true, true, false, true, true };
    local pro = Config.GetProperty(Config.HeroTable(), playerData.id, "ipos");
    local useTemplate1 = nil;
    if (pro == "29") then
        useTemplate1 = attr1GK;
    else
        useTemplate1 = attr1NotGK;
    end
    local activeNr1 = 0;
    
    for i = 1, #playerData.attr1 do
        local clone = TransformFindChild(scrollRoot, "1stAttr"..i);
        local name = TransformFindChild(clone, "Label1");
        local value = TransformFindChild(clone, "Label2");
        
        UIHelper.SetLabelTxt(name, Util.LocalizeString("UIAttr_1Attr"..i));
        UIHelper.SetLabelTxt(value, playerData.attr1[i]);
        
        clone.gameObject:SetActive(useTemplate1[i]);
        if (useTemplate1[i]) then
            clone.transform.localPosition = Vector3.New(0, 231 - 50 * activeNr1, 0);
            activeNr1 = activeNr1 + 1;
        end
        
        if (i == #playerData.attr1) then
            line.localPosition = Vector3.New(0, 231 - 50 * (activeNr1 - 1) - 25 - 2, 0);
        end
    end
    
    local attr2NotGK = { true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false, false, false, false };
    local attr2GK    = { true, true, true, false, false, false, true, true, true, true, true, true, true, true, true, true, true, true, true };
    local useTemplate2 = nil;
    if (pro == "29") then
        useTemplate2 = attr2GK;
    else
        useTemplate2 = attr2NotGK;
    end
    local activeNr2 = 0;
    
    for i = 1, #playerData.attr2 do
        local clone = TransformFindChild(scrollRoot, "2ndAttr"..i);
        local name = TransformFindChild(clone, "Label1");
        local value = TransformFindChild(clone, "Label2");
        
        UIHelper.SetLabelTxt(name, Util.LocalizeString("UIAttr_2Attr"..i));
        UIHelper.SetLabelTxt(value, playerData.attr2[i]);
        
        clone.gameObject:SetActive(useTemplate2[i]);
        if (useTemplate2[i]) then
            clone.transform.localPosition = line.localPosition + Vector3.New(0, -22 - 40 * activeNr2, 0);
            activeNr2 = activeNr2 + 1;
            local sprite = TransformFindChild(clone, "Sprite");
            sprite.gameObject:SetActive(activeNr2 % 2 ~= 0);
        end
    end
end

function OnDestroy()
end