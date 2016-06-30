module("UIItemIcon", package.seeall)

require "Config"

local prototype =
{
    gameObject = nil,
    icon = nil,
    iconRoot = nil,
    nr = nil,
    id = nil,
};
prototype.__index = prototype;

function prototype:Init(data, show999)
    self:InitIconOnly(data.id);
    
    if (show999 and tonumber(data.num) > 999) then
        UIHelper.SetLabelTxt(self.nr, "x999+");
    else
        UIHelper.SetLabelTxt(self.nr, string.format("x%d", data.num));
    end
end

function prototype:InitIconOnly(id)
    self.id = tostring(id);
    
    local tex = Config.GetProperty(Config.ItemTable(), self.id, 'icon');
    UIHelper.EnableWidget(self.icon, true);
    Util.SetUITexture(self.icon, LuaConst.Const.ItemIcon, tex, true);
end

function prototype:SetSize(labelEnum, iconScale)
    UIHelper.SetWidgetColor(self.nr, labelEnum);
    self.iconRoot.localScale = iconScale;
    local widget = self.gameObject:GetComponent("UIWidget");
    widget.width = widget.width * iconScale.x;
    widget.height = widget.height * iconScale.x;
end

function prototype:SetActive(tf)
    self.gameObject:SetActive(tf);
end

function New(gameObject)
    local clone = {};
    setmetatable(clone, prototype);
    
    local root       = gameObject.transform;
    clone.gameObject = gameObject;
    clone.icon       = TransformFindChild(root, "Icon/Texture");
    clone.iconRoot   = TransformFindChild(root, "Icon")
    clone.nr         = TransformFindChild(root, "Nr");
    
    UIHelper.EnableWidget(clone.icon, false);
    return clone;
end