module("UIProfile", package.seeall)

require "Config"
require "Common/UnityCommonScript"

function Init(transform, data)
    --[[local spriteFrame = TransformFindChild(transform, "Sprite - Frame");
    local labelPro = TransformFindChild(transform, "Label - Pro");
    local spriteNation = TransformFindChild(transform, "Sprite - Nation");
    local spriteClub = TransformFindChild(transform, "Sprite - Club");
    local spriteEval = TransformFindChild(transform, "Sprite - Evaluation");
    local labelLvl = TransformFindChild(transform, "Label - Lvl");
    local labelName = TransformFindChild(transform, "Label - Name");
    local labelPower = TransformFindChild(transform, "Label - Power");

    local adv = data.adv;
    if (adv <= 1) then
        UIHelper.SetSpriteName(spriteFrame, "Card_Player_White");
    elseif (adv <= 3) then
        UIHelper.SetSpriteName(spriteFrame, "Card_Player_Green");
    elseif (adv <= 6) then
        UIHelper.SetSpriteName(spriteFrame, "Card_Player_Blue");
    elseif (adv <= 10) then
        UIHelper.SetSpriteName(spriteFrame, "Card_Player_Purple");
    elseif (adv <= 15) then
        UIHelper.SetSpriteName(spriteFrame, "Card_Player_Golden");
    else
        UIHelper.SetSpriteName(spriteFrame, "Card_Player_Red");
    end
    
    --pro
    --nation
    --club
    --evaluation
    UIHelper.SetLabelTxt(labelLvl, data.lv.."");
    
    local name = Config.GetProperty(Config.HeroTable(), data.id, "name");
    UIHelper.SetLabelTxt(labelName, name);

    UIHelper.SetLabelTxt(labelPower, data.power.."");

    UIHelper.SetLabelTxt(labelPro, HeroData.GetHeroPosName(data.id));
    UIHelper.SetSpriteName(spriteClub, HeroData.GetHeroClubIcon(data.id));
    UIHelper.SetSpriteName(spriteNation, HeroData.GetHeroCountryIcon(data.id));

    for i = 1, 5 do
        local starSprite = TransformFindChild(transform, "Sprite - Star"..i);
        local starLvl = data.slv + Config.GetProperty(Config.HeroTable(), data.id, 'islv');
        if (starLvl < i) then
            UIHelper.SetSpriteName(starSprite, "Icon_Star2");
        else
            UIHelper.SetSpriteName(starSprite, "Icon_Star1");
        end
    end
    ]]--
end