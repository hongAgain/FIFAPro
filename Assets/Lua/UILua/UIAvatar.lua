module("UIAvatar", package.seeall)

require "Config"
require "Game/Hero"
require "Game/HeroData"
require "Common/UnityCommonScript"


local prototype =
{
    go = nil,
    spriteRank = nil,
    spriteShadow = nil,
    spriteWarn = nil,
    teamRelationRoot = nil,
    label_Pro = nil,
    --label_Rate = nil,
    tex_Icon = nil,
    tex_Club = nil,
    tex_Flag = nil,
    sprite_Rate = nil,
    label_Power = nil,
    label_Lvl = nil,
    mode1Root = nil,
    mode2Root = nil,
    mode3Root = nil,
    spriteStatus = nil,
    label_Name = nil,
    label_Plus = nil,
    id = nil,
    
    mode = 1;
};
prototype.__index = prototype;

function prototype:Mode1()
    self.mode1Root:SetActive(true);
    self.mode2Root:SetActive(false);
    self.mode3Root:SetActive(false);
end

function prototype:Mode2()
    self.mode1Root:SetActive(false);
    self.mode2Root:SetActive(true);
    self.mode3Root:SetActive(false);
end

function prototype:Mode3()
    self.mode1Root:SetActive(false);
    self.mode2Root:SetActive(false);
    self.mode3Root:SetActive(true);
end

function prototype:SwitchMode()
    if (self.mode == 1) then
        self:Mode1();
    elseif (self.mode == 2) then
        self:Mode2();
    else
        self:Mode3();
    end
end

function prototype:PrevMode()
    self.mode = self.mode - 1;
    if (self.mode < 1) then
        self.mode = self.mode + 3;
    end
    
    self:SwitchMode();
end

function prototype:NextMode()
    self.mode = self.mode + 1;
    if (self.mode > 3) then
        self.mode = self.mode - 3;
    end
        
    self:SwitchMode();
end

function prototype:Warn(tf)
    self.spriteWarn:SetActive(tf);
end

function prototype:TeamRelation(tf)
    self.teamRelationRoot:SetActive(tf);
    if (tf) then
        self.teamRelationRoot.animation:Play();
    end
end

function prototype:Shadow(tf)
    self.spriteShadow:SetActive(tf);
end

function prototype:Init(data)
    self.id = data.id;
    UIHelper.SetWidgetColor(self.spriteRank, HeroData.GetHeroRankColor(data));
    UIHelper.SetSpriteNameNoPerfect(self.sprite_Rate, HeroData.GetHeroRatingName(data.id));
    --UIHelper.SetLabelTxt(self.label_Rate, "("..HeroData.GetHeroRating(data.id)..")");
    UIHelper.SetLabelTxt(self.label_Power, tostring(data.power));
    local proId = Config.GetProperty(Config.HeroTable(), data.id, "ipos");
    local proName = Config.GetProperty(Config.ProTable(), proId, "shortName");
    UIHelper.SetLabelTxt(self.label_Pro, proName);
    UIHelper.SetLabelTxt(self.label_Lvl, "Lv."..data.lv);
    Util.SetUITexture(self.tex_Icon, LuaConst.Const.PlayerHeadIcon, HeroData.GetHeroIcon(data.id), true);
    Util.SetUITexture(self.tex_Flag, LuaConst.Const.FlagIcon, HeroData.GetHeroCountryIcon(data.id), true);
    Util.SetUITexture(self.tex_Club, LuaConst.Const.ClubIcon, HeroData.GetHeroClubIcon(data.id), true);
    UIHelper.SetSpriteName(self.spriteStatus, Hero.GetHeroStatusName(data.id));
    UIHelper.SetLabelTxt(self.label_Name, HeroData.GetHeroName(data.id));
    UIHelper.SetLabelTxt(self.label_Plus, string.format("+%d", data.adv - data.Stage()));
end

function New(gameObject)
    local clone = {};
    setmetatable(clone, prototype);
    
    local root = gameObject.transform;
    clone.go                = gameObject;
    clone.spriteRank        = TransformFindChild(root, "Sprite - Rank");
    clone.spriteShadow      = TransformFindChild(root, "Sprite - Shadow").gameObject;
    clone.spriteWarn        = TransformFindChild(root, "Sprite - Warn").gameObject;
    clone.teamRelationRoot  = TransformFindChild(root, "Team").gameObject;
    clone.label_Pro         = TransformFindChild(root, "Mode1/Label - Pro");
    --clone.label_Rate        = TransformFindChild(root, "Mode1/Label - Rate");
    clone.tex_Icon          = TransformFindChild(root, "Mode1/Icon/Icon");
    clone.sprite_Rate       = TransformFindChild(root, "Mode1/Sprite - Rate");
    clone.tex_Flag          = TransformFindChild(root, "Mode2/Flag/Flag");
    clone.tex_Club          = TransformFindChild(root, "Mode2/Club/Club");
    clone.label_Power       = TransformFindChild(root, "Mode2/Label - Power");
    clone.label_Lvl         = TransformFindChild(root, "Mode2/Label - Lvl");
    clone.mode1Root         = TransformFindChild(root, "Mode1").gameObject;
    clone.mode2Root         = TransformFindChild(root, "Mode2").gameObject;
    clone.mode3Root         = TransformFindChild(root, "Mode3").gameObject;
    clone.spriteStatus      = TransformFindChild(root, "Mode1/Status/Sprite - Status");
    clone.label_Name        = TransformFindChild(root, "Mode3/Label - Name");
    clone.label_Plus        = TransformFindChild(root, "Mode3/Label");
    
    return clone;
end