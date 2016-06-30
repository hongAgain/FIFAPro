module("UIScoutAvatar", package.seeall)

require "Config"
require "Game/HeroData"
require "Common/UnityCommonScript"

local prototype =
{
    go = nil,
    bg = nil,
    sprite_Fragment = nil,
    label_Fragment = nil,
    label_Evaluate = nil,
    sprite_Evaluate = nil,
    label_Name = nil,
    label_Pro = nil,
};
prototype.__index = prototype;

function prototype:ToggleFragment(tf)
    self.sprite_Fragment.gameObject:SetActive(tf);
    self.label_Fragment.gameObject:SetActive(tf);
    
    if (tf) then
        UIHelper.SetSpriteNameNoPerfect(self.bg, "Card_tatter");
    end
end

local threshold1 = HeroData.GetHeroRatingMinMax(3);
local threshold2 = HeroData.GetHeroRatingMinMax(4);
local threshold3 = HeroData.GetHeroRatingMinMax(5);

function prototype:Init(id)
    local rating = HeroData.GetHeroRating(id);
    
    if (rating <= threshold1) then
        UIHelper.SetSpriteNameNoPerfect(self.bg, "Card_BCD");
    elseif (rating <= threshold2) then
        UIHelper.SetSpriteNameNoPerfect(self.bg, "Card_A");
    elseif (rating <= threshold3) then
        UIHelper.SetSpriteNameNoPerfect(self.bg, "Card_S");
    end
    UIHelper.SetLabelTxt(self.label_Evaluate, string.format("(%d)", rating));
    UIHelper.SetSpriteNameNoPerfect(self.sprite_Evaluate, HeroData.GetHeroRatingName(id));
    Util.SetUITexture(self.icon,LuaConst.Const.PlayerHeadIcon,HeroData.GetHeroIcon(id), true);
    
    local proId = Config.GetProperty(Config.HeroTable(), id, "ipos");
    local proName = Config.GetProperty(Config.ProTable(), proId, "shortName");
    UIHelper.SetLabelTxt(self.label_Pro, proName);
    
    local heroName = Config.GetProperty(Config.HeroTable(), id, "name");
    UIHelper.SetLabelTxt(self.label_Name, heroName);
end

function New(gameObject)
    local clone = {};
    setmetatable(clone, prototype);
    
    local root = gameObject.transform;
    clone.go =                  gameObject;
    clone.bg =                  TransformFindChild(root, "Sprite - BG");
    clone.sprite_Fragment =     TransformFindChild(root, "Sprite - Piece");
    clone.label_Fragment =      TransformFindChild(root, "Label - Piece");
    clone.label_Evaluate =      TransformFindChild(root, "Label - Evaluate");
    clone.sprite_Evaluate =     TransformFindChild(root, "Sprite - Evaluate");
    clone.label_Name =          TransformFindChild(root, "Label - Name");
    clone.label_Pro =           TransformFindChild(root, "Label - Pro");
    clone.icon =                TransformFindChild(root, "Icon/Icon");
    
    return clone;
end