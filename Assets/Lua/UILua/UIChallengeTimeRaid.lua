module("UIChallengeTimeRaid", package.seeall)

local window = nil;
local windowComponent = nil;

function OnStart(gameObject, params)
    window = gameObject;
    windowComponent = window:GetComponent("UIBaseWindowLua");
    
    local transform = TransformFindChild(window.transform, "UITimeRaid_Challenge");
    local label1 = TransformFindChild(transform, "Label1");
    local labelEnergy = TransformFindChild(transform, "Label2/Label - Energy");
    local label3 = TransformFindChild(transform, "Label3");
    local scrollView = TransformFindChild(transform, "Scroll View");

    local class = math.ceil(params[1] / LuaConst.Const.TimeRaidNrByType);
    local str1 = Util.LocalizeString("UITimeRaid_Challenge_Tips5");
    local str2 = Util.LocalizeString(string.format("UITimeRaid_Type%d", class));
    UIHelper.SetLabelTxt(label1, string.format(str1, str2));

    local power = Config.GetProperty(Config.RaidDSTable(), tostring(class), "Power");
    UIHelper.SetLabelTxt(labelEnergy, tostring(power));

    local str3 = Util.LocalizeString(string.format("UITimeRaid_Challenge_Tips%d", class));
    UIHelper.SetLabelTxt(label3, str3);
    
    local challengeBtn = TransformFindChild(window.transform, "Btn - Challenge");
    Util.AddClick(challengeBtn.gameObject, params[2].OnChallenge);
    
    local closeBtn = TransformFindChild(window.transform, "Btn - Close");
    Util.AddClick(closeBtn.gameObject, Close);
    
    local itemPrefab = Util.GetGameObject("UIItemIcon");
    local scrollRoot = TransformFindChild(window.transform, "UITimeRaid_Challenge/Scroll View");
    local displayDrop = Config.GetProperty(Config.RaidDSTable(), params[1], "display");
    for i = 1, #displayDrop do
        local clone = AddChild(itemPrefab, scrollRoot);
        clone.transform.localPosition = Vector3.New(-228 + (i - 1) * 118, 0, 0);
        local itemIcon = UIItemIcon.New(clone);
        itemIcon:SetSize("win_wb_20", Vector3.one * 86 / 180);
        itemIcon:InitIconOnly(displayDrop[i]);
    end
end

function OnDestroy()
    
end

function OnShow()
    
end

function OnHide()
    
end

function Close()
    windowComponent:Close();
end