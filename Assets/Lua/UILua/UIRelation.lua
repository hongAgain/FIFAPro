module("UIRelation", package.seeall)

require "Common/CommonScript"
require "Common/UnityCommonScript"

local TB = nil;
local relation_person = {};

local uiRelation =
{
    root = nil,
    label_name = nil,
    --label_status = nil,
    --label_condition = nil,
    --label_LMH = nil,
    descRoot = nil,
    point = nil,
    --prefab_relation_desc = nil,
};
uiRelation.__index = uiRelation;

function uiRelation:Bind(transform)
    self.root               = transform;
    self.label_name         = TransformFindChild(transform, "Label - Name");
    --self.label_status       = TransformFindChild(transform, "Label - State");
    --self.label_condition    = TransformFindChild(transform, "Label - Condition");
    --self.label_LMH          = TransformFindChild(transform, "Label - LMH");
    self.point              = TransformFindChild(transform, "Point");
    self.descRoot           = transform;
end

function uiRelation:Init(strName, strCond, strLMH)
    UIHelper.SetLabelTxt(self.label_name, strName);
    --self.label_condition.localPosition = self.label_name.localPosition + NewVector3(UIHelper.LabelSize(self.label_name).x, 0, 0);
    --UIHelper.SetLabelTxt(self.label_condition, strCond);
    --UIHelper.SetLabelTxt(self.label_LMH, strLMH);
end

function uiRelation:SetStatus(activeIdx, stat)
    if (stat == 0) then
        UIHelper.SetWidgetColor(self.label_name, "win_wa_24");
        self.point.gameObject:SetActive(false);
        --UIHelper.SetLabelTxt(self.label_status, Util.LocalizeString("UIPlayer_Relation_Deactive"));
        --UIHelper.SetWidgetColor(self.label_status, "win_w_20");
    else
        UIHelper.SetWidgetColor(self.label_name, "win_wb_24");
        self.point.gameObject:SetActive(true);
        local activeDesc = TransformFindChild(self.point.parent, "Label - Effect"..(activeIdx or 1));
        self.point.localPosition = activeDesc.localPosition - Vector3.New(21, 0, 0);
        --UIHelper.SetLabelTxt(self.label_status, Util.LocalizeString("UIPlayer_Relation_Active"));
        --UIHelper.SetWidgetColor(self.label_status, "win_b_20");
    end
end

function uiRelation:SetDesc(idx, strDesc, stat, enableSprite)
    local desc = TransformFindChild(self.descRoot, string.format("Label - Effect%d", idx));
    --[[if (desc == nil) then
        desc = AddChild(self.prefab_relation_desc, self.descRoot).transform;
        desc.name = string.format("Label - Effect%d", idx);
        desc.localPosition = Vector3.New(0, - (idx - 1) * 40, 0);
    end]]--
    
    if (stat == 1) then
        UIHelper.SetWidgetColor(desc, "win_b_20");
    else
        UIHelper.SetWidgetColor(desc, "win_wa_20");
    end
    UIHelper.SetLabelTxt(desc, strDesc);

    --[[local sprite = TransformFindChild(desc, "Sprite");
    UIHelper.EnableWidget(sprite, enableSprite);]]--
end

function New(transform)
    local instance = {};
    setmetatable(instance, uiRelation);
    instance:Bind(transform);
    return instance;
end

function Init(gameObject, windowComponent)
    if (TB == nil) then
        TB = {};
    end
    
    local hero = Hero.GetHeroData2Id(PlayerInfoData.GetCurrPlayerId());

    local transform = gameObject.transform;

    local prefab_relation = windowComponent:GetPrefab("Relation - Club");
    local relation_person = Config.GetProperty(Config.HeroTable(), hero.id, "group");
    --local selfName = Config.GetProperty(Config.HeroTable(), hero.id, "name");
    local relation_root = TransformFindChild(transform, "Scroll View/Table");
    
    local teamEffects = Hero.MainTeamEffects();
    
    --Relation Country
    local country_id = Config.GetProperty(Config.HeroTable(), hero.id, "country");
    local relation_country = Config.GetProperty(Config.GroupCountryTable(), country_id, 'group');
    if (relation_country ~= nil) then
        local clone_relation_country = TransformFindChild(relation_root, "Relation -1- Country");
        if (TB[clone_relation_country] == nil) then
            local go = AddChild(prefab_relation, relation_root);
            go.name = "Relation -1- Country";
            clone_relation_country = go.transform;
            UIHelper.AdjustDepth(go, 1);

            TB[clone_relation_country] = New(clone_relation_country);
        end

        local countryStat = { stat = 0 };
        for k, v in ipairs(relation_country) do
            local strDesc = Config.GetProperty(Config.GroupCountryTable(), v, "desc");

            local stat = 0;
            if (teamEffects[v] ~= nil) then
                stat = teamEffects[v]['stat'];
            end

            if (countryStat.stat == nil) then
                countryStat.activeIdx = k;
                countryStat.stat = stat;
            end
            TB[clone_relation_club]:SetDesc(k, strDesc, stat, (k - 1) % 2 ~= 0);
        end

        local countryName = string.format(Util.LocalizeString("squareBrackets"), Config.GetProperty(Config.GroupCountryTable(), country_id, "name"));

        local strLMH = "";
        --[[if (clubStat.activeIdx ~= nil) then
            strLMH = Util.LocalizeString(string.format("UIPlayer_TeamRelation%d", countryStat.activeIdx));
        end]]--
        TB[clone_relation_club]:Init(countryName, "", strLMH);
        TB[clone_relation_club]:SetStatus(countryStat.activeIdx, countryStat.stat);
    end

    --Relation Club
    local club_id = Config.GetProperty(Config.HeroTable(), hero.id, "club");
    local relation_club = Config.GetProperty(Config.ClubTable(), club_id, 'group');
    if (relation_club ~= nil) then
        local clone_relation_club = TransformFindChild(relation_root, "Relation -2- Club");
        if (TB[clone_relation_club] == nil) then
            local go = AddChild(prefab_relation, relation_root);
            go.name = "Relation -2- Club";
            clone_relation_club = go.transform;
            UIHelper.AdjustDepth(go, 1);

            local relation = New(clone_relation_club);
            TB[clone_relation_club] = relation;
        end

        local clubStat = { stat = 0 };
        for k, v in ipairs(relation_club) do
            local strDesc = Config.GetProperty(Config.GroupClubTable(), v, "desc");

            local stat = 0;
            if (teamEffects[v] ~= nil) then
                stat = teamEffects[v]['stat'];
            end

            if (clubStat.stat == 0) then
                clubStat.activeIdx = k;
                clubStat.stat = stat;
            end
            TB[clone_relation_club]:SetDesc(k, strDesc, stat, (k - 1) % 2 ~= 0);
        end

        local clubName = Config.GetProperty(Config.ClubTable(), club_id, 'name');

        local strLMH = "";
        --[[if (clubStat.activeIdx ~= nil) then
            strLMH = Util.LocalizeString(string.format("UIPlayer_TeamRelation%d", clubStat.activeIdx));
        end]]--
        TB[clone_relation_club]:Init(string.format(Util.LocalizeString("squareBrackets"), clubName), "", strLMH);
        TB[clone_relation_club]:SetStatus(clubStat.activeIdx, clubStat.stat);
    end
    
    --Relation Player
    prefab_relation = windowComponent:GetPrefab("Relation - Person");
    for k, v in ipairs(relation_person) do
        local clone = TransformFindChild(relation_root, "Relation -3- Person"..k);
        if (clone == nil) then
            local go = AddChild(prefab_relation, relation_root);
            clone = go.transform;
            go.name = "Relation -3- Person"..k;
            UIHelper.AdjustDepth(go, 1);
        end
        
        if (TB[clone] == nil) then
            local relation = New(clone);
            TB[clone] = relation;
        end

        local relation = TB[clone];
        if (relation ~= nil) then
            local stat = 0;
            if (teamEffects[v] ~= nil) then
                stat = teamEffects[v]['stat'];
                clone.gameObject:SetActive(true);
            elseif (v == "") then
                clone.gameObject:SetActive(false);
            end
            relation:SetStatus(1, stat);
            
            local strName = "";
            local _name = Config.GetProperty(Config.GroupBaseTable(), v, "name");
            if (_name ~= nil) then
                strName = string.format(Util.LocalizeString("squareBrackets"), _name);
            end

            local strCond = "";
            --[[local unlockStarLvl = Config.GetProperty(Config.GroupBaseTable(), v, "star") or 1;
            if (hero.star < unlockStarLvl) then
                strCond = string.format(Util.LocalizeString("unlockCondition"), unlockStarLvl);
                UIHelper.SetWidgetColor(relation.label_condition, "win_w_20");
            end]]--
            
            relation:Init(strName, strCond, "");

            local heroArr = Config.GetProperty(Config.GroupBaseTable(), v, "hero") or {};
            local heroParam = {};
            for k1, v1 in ipairs(heroArr) do
                local heroName = Config.GetProperty(Config.HeroTable(), v1, "name");
                if (heroName == nil) then
                    heroName = "";
                end

                if CommonScript.TableContainValue(Hero.MainTeamHeroId(), v1) then
                    heroParam[k1] = heroName;
                else
                    heroParam[k1] = heroName;
                end
            end
            
            local strDesc = Config.GetProperty(Config.GroupBaseTable(), v, "desc") or "";
            if (#heroParam <= 3) then
                strDesc = string.format(strDesc, heroParam[1], heroParam[2], heroParam[3]);
            elseif (#heroParam <= 2) then
                strDesc = string.format(strDesc, heroParam[1], heroParam[2]);
            elseif (#heroParam <= 1) then
                strDesc = string.format(strDesc, heroParam[1]);
            end
            relation:SetDesc(1, strDesc, stat, false);
        end
    end
    
    relation_root:SendMessage("Reposition");
end

function OnDestroy()
    TB = nil;
end