module("HeroData", package.seeall)
require("Config")
require("Common/Color")

local cache = {};
cache.__index = function(table, key)
    if (key == "attr1" or key == "attr2" or key == "power") then
        CalcAttr(table);
    end
    
    return table[key];
end

function CalcAttr(hero)
    
    local id = hero["id"];
--    local _log = tostring(id) == "50519";
    local islv = Config.GetProperty(Config.HeroTable(), id, "islv");  --初始星级
    local lv = hero["lv"];      --等级
    local adv = hero["adv"];    --进阶Lvl
    local slv = hero["slv"];    --突破Lvl
    local train = hero["training"]; --洗练点
    local advMod = Config.GetProperty(Config.HeroTable(), id, "advMod");  --模板id
    
    local attr_tupo = Config.GetProperty(Config.GrowTable(), id, "star"..(islv + slv));
    local key = GetAdvID(advMod,adv);
    local attr_adv = Config.GetProperty(Config.HeroAdvTable(), key, "att");
    
    local attr1Length = 8;
    local attr1 = {};
    for i = 1, attr1Length do
        attr1[i] = math.floor(attr_tupo[i] * lv + attr_adv[i] + (train[i] or 0));
    end
    
    local attr2 = {};
    attr2[1] = math.floor(attr_tupo[9] * lv + attr_adv[9]);
    attr2[2] = math.floor(attr_tupo[10] * lv + attr_adv[10]);
    for i = 3, 19 do
        local value = 0;
        for j = 1, attr1Length do
            local iatt = Config.GetProperty(Config.attTransTB, tostring(j - 1), "iatt");
            value = value + iatt[i] * attr1[j];
        end
        attr2[i] = math.floor(value);
    end
    
    hero.attr1 = attr1;
    hero.attr2 = attr2;

    local ipos = Config.GetProperty(Config.HeroTable(), id, "ipos");
    local ratioArr = Config.GetProperty(Config.HeroAPTable(), ipos, "arg");
    
    local power = 0;
    for i = 1, #ratioArr do
        local ratio = ratioArr[i];
        power = power + hero.attr2[i] * ratio;
    end
    
    hero.power = math.floor(power);
    hero.star = islv + slv;

    hero.ClearAttr = function()
        hero.attr1 = nil;
        hero.attr2 = nil;
        hero.power = nil;
    end
    
    hero.PowerAtPos = function(pos)
        local rate = 1;
        if (ipos ~= pos) then
            rate = tonumber(Config.GetTemplate(Config.BaseTable())["wrongPositionPer"]) / 100;
        end
        
        return math.floor(power * rate);
    end
    
    hero.Stage = function()
        if (hero.adv == 0) then
            return 0;
        elseif (hero.adv <= 2) then
            return 1;
        elseif (hero.adv <= 5) then
            return 2;
        elseif (hero.adv <= 9) then
            return 3;
        elseif (hero.adv <= 14) then
            return 4;
        else
            return 5;
        end
    end
    
    setmetatable(hero, cache);


--    if (_log) then
--        print('------attr1------');
--        for i = 1, #attr1 do
--            print(attr1[i]);
--        end
--        print('------attr2------');
--        for i = 1, #attr2 do
--            print(attr2[i]);
--        end
--    end
end

function GetTeamBattleScoreUnderCoachEffect( coachTeamAbilityTable )
    local tempHeroTeam = {};

    local score = 0;
    --local formDataTb = Config.GetTemplate(Config.FormTable())[Hero.GetFormId()];
    local formDataTb = TableManager.FormationTbl:GetItem(Hero.GetFormId())
    --get info of every lined up hero by id-v
    for i,v in pairs(Hero.MainTeamHeroId()) do
        tempHeroTeam[i] = Hero.GetHeroData2Id(v);

        local finalPower = 0;

        local id = tempHeroTeam[i]["id"];
        local islv = Config.GetProperty(Config.HeroTable(), id, "islv");  --初始星级
        local lv = tempHeroTeam[i]["lv"];      --等级
        local adv = tempHeroTeam[i]["adv"];    --进阶Lvl
        local slv = tempHeroTeam[i]["slv"];    --突破Lvl
        local train = tempHeroTeam[i]["training"]; --洗练点
        local advMod = Config.GetProperty(Config.HeroTable(), id, "advMod");  --模板id
        
        local attr_tupo = Config.GetProperty(Config.GrowTable(), id, "star"..(islv + slv));
        local key = GetAdvID(advMod,adv);
        local attr_adv = Config.GetProperty(Config.HeroAdvTable(), key, "att");
        
        local attr1Length = 8;

        --calculate attr1 with coach effect
        local attr1 = {};
        for ii = 1, attr1Length do
            attr1[ii] = math.floor(attr_tupo[ii] * lv + attr_adv[ii] + (train[ii] or 0));
        end

        for ii,vv in ipairs(attr1) do
            attr1[ii] = vv+coachTeamAbilityTable[ii];
        end
        
        --calculate attr2
        local attr2 = {};
        attr2[1] = math.floor(attr_tupo[9] * lv + attr_adv[9]);
        attr2[2] = math.floor(attr_tupo[10] * lv + attr_adv[10]);
        for ii = 3, 19 do
            local value = 0;
            for iii = 1, attr1Length do
                local iatt = Config.GetProperty(Config.attTransTB, tostring(iii - 1), "iatt");
                value = value + iatt[ii] * attr1[iii];
            end
            attr2[ii] = math.floor(value);
        end
        

        local ipos = Config.GetProperty(Config.HeroTable(), id, "ipos");
        local ratioArr = Config.GetProperty(Config.HeroAPTable(), ipos, "arg");
        
        local power = 0;
        for ii = 1, #ratioArr do
            local ratio = ratioArr[ii];
            power = power + attr2[ii] * ratio;
        end
        
        --get basic power at proper position
        finalPower = math.floor(power);

        --judge the position
        local rate = 1;
        if (ipos ~= tostring(formDataTb.ProList[i])) then
            rate = tonumber(Config.GetTemplate(Config.BaseTable())["wrongPositionPer"]) / 100;
        end
        
        finalPower = math.floor(finalPower * rate);

        --accumulate score of each hero;
        score = score + finalPower;
    end
    return math.floor(score);
end

function GetAdvID(advMode_,adv_)
    local index = "";
    if (adv_ > 9) then
        index = advMode_..''..adv_;
    else
        index = advMode_..'0'..adv_
    end

    return index;
end


function GetAttr(hero,index_)
    if hero.attr[index_] ~= nil then
        return hero.attr[index_];
     end

     return 0;
end

function GetAttr1(playerId_,index_)
    local hero = Hero.GetHeroData2Id(playerId_);
    local index = GetAttr1Index(playerId_,index_);

    if hero.attr1[index] ~= nil then
        return hero.attr1[index];
    else
        return -1;
    end
end

function GetAttr2(playerId_,index_)
    local hero = Hero.GetHeroData2Id(playerId_);
    index_ = GetAttr2Index(playerId_,index_);

    if hero.attr2[index_] ~= nil then
        return hero.attr2[index_];
    else
        return -1;
    end
end

function GetAttr1ByData(data_,playerId_,index_)
    local hero = data_;
    local index = GetAttr1Index(playerId_,index_);

    if hero.attr1[index] ~= nil then
        return hero.attr1[index];
    else
        return -1;
    end
end

function GetAttr1Name(playerId_,index_)
    return Util.LocalizeString("UIAttr_1Attr"..GetAttr1Index(playerId_,index_));
end

function GetAttr2Name(playerId_,index_)
    return Util.LocalizeString("UIAttr_2Attr"..GetAttr2Index(playerId_,index_));
end

function GetAttr1Pot(playerId_,index_)
    local hero = Hero.GetHeroData2Id(tostring(playerId_));
    local index = GetAttr1Index(playerId_,index_);

    if hero["training"][index] ~= nil then
        return hero["training"][index];
    else
        return -1;
    end
end
function GetAttr1PotLimit(playerId_,index_)
    local hero = Hero.GetHeroData2Id(tostring(playerId_));
    local index = GetAttr1Index(playerId_,index_);
    local potModel = Config.GetProperty(Config.HeroTable(),tostring(playerId_),'potential_model')..hero["adv"];
    local potBase = Config.GetProperty(Config.HeroTable(),tostring(playerId_),'potentiality');
    local potAdv = Config.GetProperty(Config.HeroPotAdv(),tostring(potModel),'att');
    local potStar = Config.GetProperty(Config.HeroPotStar(),tostring(Hero.GetCurrStars(playerId_)),'star_coef');

    return math.floor(potBase[index]*hero["lv"]*potStar+potAdv[index]);
end
function GetAttr1Index(playerId_,index_)
    local pos = Config.GetProperty(Config.HeroTable(),tostring(playerId_),'ipos');
    if tonumber(pos) == 29 then
        if index_ >= 5 then
            index_ = index_ + 2;
        elseif index_ >= 3 then
            index_ = index_ + 1;
        end
    end

    return index_;
end
function GetAttr2Index(playerId_,index_)
    local pos = Config.GetProperty(Config.HeroTable(),tostring(playerId_),'ipos');
    if tonumber(pos) == 29 then
--        if index_ == 4 or index_ == 5 or index_ == 6 then
--            index_ = index_ + 13;
--        end
        if index_ >= 4 then
            index_ = index_ + 3;
        end
    end

    return index_;
end
function IsGKPlayer(playerId_)
    local pos = Config.GetProperty(Config.HeroTable(),tostring(playerId_),'ipos');
    if tonumber(pos) == 29 then
        return true;
    end

    return false;
end
function GetBattleScore(playerId_)
    local heroTemp = Hero.GetHeroData2Id(playerId_);
    if heroTemp ~= nil then
        return heroTemp.power;
    else
        return -1;
    end;

end
function GetBattleScore2Pos(playerId_,pos_)
    local heroTemp = Hero.GetHeroData2Id(playerId_);
    if heroTemp ~= nil then
        return heroTemp.PowerAtPos(tostring(pos_));
    else
        return -1;
    end;

end
function GetTeamBattleScore()
    local score = 0;
--    local formDataTb = Config.GetTemplate(Config.FormTable())[Hero.GetFormId()];
    local formDataTb = TableManager.FormationTbl:GetItem(Hero.GetFormId());
    local teamIdTb = Hero.MainTeamHeroId();

    for i=1,#teamIdTb do
        score = score + GetBattleScore2Pos(teamIdTb[i],formDataTb.ProList[i-1])
    end

    return math.floor(score);
end

function GetHeroLv(playerId_)
    local heroTemp = Hero.GetHeroData2Id(playerId_);
    if heroTemp ~= nil then
        return heroTemp.lv;
    else
        return -1;
    end;
end

function GetHeroName(playerId_)
    return Config.GetProperty(Config.HeroTable(), playerId_, 'name');
end

function GetHeroDesc(playerId_)
    return Config.GetProperty(Config.HeroTable(), playerId_, 'desc');
end

function GetHeroRating(playerId_)
    return Config.GetProperty(Config.HeroTable(), playerId_, 'rating');

end

function GetHeroPos(playerId_)
    return Config.GetProperty(Config.HeroTable(),playerId_, 'ipos');
end

function GetHeroRatingName(playerId_)
    local ratingValue = GetHeroRating(playerId_);
    for i=1,5 do
        local min,max,name = GetHeroRatingMinMax(i);
        if ratingValue >= min and ratingValue <= max then
            return name;
        end
    end

end

function GetHeroPosName(playerId_)
    local pos = GetHeroPos(playerId_);
    return Config.GetProperty(Config.HeroAPTable(),pos,'name');

end

function GetHeroRatingMinMax(id_)
    return Config.GetProperty(Config.HeroRatingTable(),tostring(id_),"min"),
           Config.GetProperty(Config.HeroRatingTable(),tostring(id_),"max"),
           Config.GetProperty(Config.HeroRatingTable(),tostring(id_),"name");

end
function GetHeroIcon(playerId_)
    local icon = 'Default';
    icon = Config.GetProperty(Config.HeroTable(),playerId_,'playericon');
    if icon == nil or string.len(icon) == 0 then
        icon = 'Default';
    end

    return icon;
end

function GetHeroClubIcon(playerId_)
    local icon = 'Default';
    local clubId = Config.GetProperty(Config.HeroTable(),playerId_,'club');
    if clubId ~= nil then
        icon = Config.GetProperty(Config.ClubTable(),clubId,'logo');
        if icon == nil or string.len(icon) == 0 then
            icon = 'Default';
        end
    end


    return icon;
end

function GetHeroCountryIcon(playerId_)
    local icon = 'Default';
    local countryId = Config.GetProperty(Config.HeroTable(),playerId_,'country');
    if countryId ~= nil then
        icon = Config.GetProperty(Config.CountryTable(),countryId,'logo');
        if icon == nil or string.len(icon) == 0 then
            icon = 'Default';
        end
    end

    return icon;
end

-- Hero Skill
function GetHeroSkillIdLv(playerId_,index_)
    local heroTemp = Hero.GetHeroData2Id(tostring(playerId_));
    if heroTemp ~= nil then
        if heroTemp["skill"][tostring(index_)] == nil then
            return nil;
        end
        return heroTemp["skill"][tostring(index_)]["id"],heroTemp["skill"][tostring(index_)]["lv"];
    else
        return nil;
    end;
end

function GetHeroSkillIcon(playerId_,index_)
    local heroTemp = Hero.GetHeroData2Id(tostring(playerId_));
    if heroTemp ~= nil then
        if heroTemp["skill"][tostring(index_)] == nil then
            return nil;
        end
        LogManager:RedLog(heroTemp["skill"][tostring(index_)]["id"]);
        return TableManager.SkillTbl:GetItem(heroTemp["skill"][tostring(index_)]["id"]).IconName;
    else
        return "Default";
    end;

end


local colors = {};
colors[1] = Color.New(108 / 255, 108 / 255, 108 / 255, 1);
colors[2] = Color.New( 65 / 255, 102 / 255,  63 / 255, 1);
colors[3] = Color.New( 58 / 255,  78 / 255,  97 / 255, 1);
colors[4] = Color.New( 91 / 255,  77 / 255, 113 / 255, 1);
colors[5] = Color.New(149 / 255, 110 / 255,  43 / 255, 1);
colors[6] = Color.New(128 / 255,  66 / 255,  66 / 255, 1);

function GetHeroRankColor(hero)
    return colors[(hero.Stage() + 1) or 1] or colors[1];
end
